#pragma warning disable CS1998

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Core.Roles.Models;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailIdentityManager.Core.Common.Admin.Services;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailIdentityManager.Infrastructure.Common.Admin.Factories;
using TasteTrailIdentityManager.Core.Users.Dtos;

namespace TasteTrailIdentityManager.Infrastructure.Common.Admin.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;

    private readonly RoleManager<Role> _roleManager;


    public AdminService(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} not found." });

        return await _userManager.AddToRoleAsync(user, roleName);
    }


    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentException($"cannot find user with username: {username}");

        return user;
    }

    public async Task<IEnumerable<string>> GetRolesByUsernameAsync(string username)
    {
        var user  = await GetUserByUsernameAsync(username: username);
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        return user;
    }

    public async Task<int> GetUsersCountAsync()
    {
        return _userManager.Users.Count();
    }

    public async Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} not found." });
        
        var rolesBeforeRemoval = await _userManager.GetRolesAsync(user);

        if(rolesBeforeRemoval.Count == 1 && rolesBeforeRemoval.First() == UserRoles.User.ToString())
        {
            return IdentityResult.Failed(new IdentityError { Description = $"You cannot delete default role." });
        }

        var removeResult = await _userManager.RemoveFromRoleAsync(user, roleName);

        var rolesAfterRemoval = await _userManager.GetRolesAsync(user);

        if(rolesAfterRemoval.Count == 0)
        {
            _ = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
        }

        return removeResult;
    }

    public async Task<IdentityResult> ToggleBanUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new ArgumentException("User not found!");

        user.IsBanned = !user.IsBanned;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ToggleMuteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        user.IsMuted = !user.IsMuted;
        await _userManager.UpdateAsync(user);

        var claims = await _userManager.GetClaimsAsync(user) ?? throw new ArgumentException($"cannot find calims of user with id: {user.Id}");

        var isMutedClaim = claims.FirstOrDefault(c => c.Type == "IsMuted") ?? throw new ArgumentException("Muted claim doesn't exist!");


        return await _userManager.ReplaceClaimAsync(user, isMutedClaim, new Claim("IsMuted", user.IsMuted.ToString()));
    }

    public async Task<FilterResponseDto<UserResponseDto>> GetUsersAsync(PaginationParametersDto paginationParameters)
    {
        var users = _userManager.Users.AsNoTracking();
        var userDtos = new List<UserResponseDto>();

        var totalUsers = await _userManager.Users.CountAsync();
        var totalPages = (int)Math.Ceiling(totalUsers / (double)paginationParameters.PageSize);

        var paginatedUsers = users.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize).Take(paginationParameters.PageSize).ToList();

        foreach (var user in paginatedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                User = user,
                Roles = roles
            };

            userDtos.Add(userDto);
        }

        var filterReponse = new FilterResponseDto<UserResponseDto>() {
            CurrentPage = paginationParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalUsers,
            Entities = userDtos
        };

        return filterReponse;
    }

    public async Task<FilterResponseDto<UserResponseDto>> GetUsersBySearchAsync(PaginationSearchParametersDto paginationParameters)
    {
        var users = _userManager.Users.AsQueryable();

        var totalUsers = await users.CountAsync();

        var searchedUsers = new List<User>(users);

        if (paginationParameters.SearchTerm is not null)
        {
            var searchTerm = $"%{paginationParameters.SearchTerm.ToLower()}%";

            searchedUsers = users.Where(f =>
                f.UserName != null && EF.Functions.Like(f.UserName.ToLower(), searchTerm)
            ).ToList();
            
            totalUsers = searchedUsers.Count();
        }

        var totalPages = (int)Math.Ceiling(totalUsers / (double)paginationParameters.PageSize);

        var userDtos = new List<UserResponseDto>();

        var paginatedUsers = searchedUsers.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize).Take(paginationParameters.PageSize);

        foreach (var user in paginatedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                User = user,
                Roles = roles
            };

            userDtos.Add(userDto);
        }

        var filterReponse = new FilterResponseDto<UserResponseDto>() {
            CurrentPage = paginationParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalUsers,
            Entities = userDtos
        };

        return filterReponse;
    }

    public async Task<FilterResponseDto<UserResponseDto>> GetUsersFilteredAsync(FilterParametersDto filterParameters)
    {
        var newFilterParameters = new FilterParameters<User>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = UserManipulationsFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = null
        };

        var users = _userManager.Users.AsQueryable();

        var totalUsers = await users.CountAsync();
        
        if(newFilterParameters.Specification != null)
        {
            users = newFilterParameters.Specification.Apply(users);
            totalUsers = await users.CountAsync();
        }

        var totalPages = (int)Math.Ceiling(totalUsers / (double)filterParameters.PageSize);

        var userDtos = new List<UserResponseDto>();

        var paginatedUsers = users.Skip((filterParameters.PageNumber - 1) * filterParameters.PageSize).Take(filterParameters.PageSize).ToList();

        foreach (var user in paginatedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                User = user,
                Roles = roles
            };

            userDtos.Add(userDto);
        }

        var filterReponse = new FilterResponseDto<UserResponseDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalUsers,
            Entities = userDtos
        };

        return filterReponse;
    }

    public async Task<FilterResponseDto<UserResponseDto>> GetUsersFiltereBySearchdAsync(FilterParametersSearchDto filterParameters)
    {
        var newFilterParameters = new FilterParameters<User>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = UserManipulationsFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = filterParameters.SearchTerm
        };

        var users = _userManager.Users.AsQueryable();

        var totalUsers = await users.CountAsync();

        if(newFilterParameters.Specification != null)
        {
            users = newFilterParameters.Specification.Apply(users);
            totalUsers = await users.CountAsync();
        }

        var searchedUsers = new List<User>(users);

        if (filterParameters.SearchTerm is not null)
        {
            var searchTerm = $"%{filterParameters.SearchTerm.ToLower()}%";

            searchedUsers = users.Where(f =>
                f.UserName != null && EF.Functions.Like(f.UserName.ToLower(), searchTerm)
            ).ToList();

            totalUsers = await users.CountAsync();
        }

        var totalPages = (int)Math.Ceiling(totalUsers / (double)filterParameters.PageSize);

        var userDtos = new List<UserResponseDto>();

        var paginatedUsers = searchedUsers.Skip((filterParameters.PageNumber - 1) * filterParameters.PageSize).Take(filterParameters.PageSize);

        foreach (var user in paginatedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserResponseDto
            {
                User = user,
                Roles = roles
            };

            userDtos.Add(userDto);
        }

        var filterReponse = new FilterResponseDto<UserResponseDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalUsers,
            Entities = userDtos
        };

        return filterReponse;
    }

    public async Task<int> GetCountFilteredAsync(FilterParametersDto filterParameters)
    {
        var newFilterParameters = new FilterParameters<User>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = UserManipulationsFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = null
        };

        var query = _userManager.Users.AsQueryable();

        if (filterParameters is null)
            return await query.CountAsync();

        if (newFilterParameters.Specification != null)
            query = newFilterParameters.Specification.Apply(query);

        return await query.CountAsync();
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TasteTrailIdentity.Core.Users.Models;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailIdentity.Core.Roles.Models;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailIdentity.Core.Common.Services;

namespace TasteTrailIdentity.Infrastructure.Users.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    private readonly RoleManager<Role> _roleManager;
    private readonly IRefreshTokenService _refreshService;
    private readonly IMessageBrokerService _messageBrokerService;

    public UserService(UserManager<User> userManager, IRefreshTokenService refreshService
            , IMessageBrokerService messageBrokerService, RoleManager<Role> roleManager)
    {
        _refreshService = refreshService;
        _userManager = userManager;
        _roleManager = roleManager;
        _messageBrokerService = messageBrokerService;
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
    public async Task<IList<string>> GetRolesByUsernameAsync(string username)
    {
        var user  = await GetUserByUsernameAsync(username: username); 
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IList<string>> GetRolesByEmailAsync(string email)
    {
        var user  = await GetUserByEmailAsync(email: email); 
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        return user;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username) ?? throw new ArgumentException($"cannot find user with username: {username}"); 

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new ArgumentException($"cannot find user with email: {email}"); 

        return user;
    }

    public async Task<IdentityResult> AddUserClaimAsync(User user, Claim claim)
    {
        var existingClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == claim.Type);

        return existingClaim is null ? await _userManager.AddClaimAsync(user, claim) : throw new ArgumentException($"user {user.Email} already has this claim!");
    }

       public async Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");
        var roleName = role.ToString();


        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> UpdateUserAsync(User userDto, Guid refresh)
    {
        var isUserNameEmpty = string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrWhiteSpace(userDto.UserName);
        var isEmailEmpty = string.IsNullOrEmpty(userDto.Email) || string.IsNullOrWhiteSpace(userDto.Email);

        if(isUserNameEmpty && isEmailEmpty)
        {
            throw new ArgumentException("dto is empty");
        }
        var userToChange = await _userManager.FindByIdAsync(userDto.Id) ?? throw new ArgumentException($"cannot find user with id: {userDto.Id}");

        if( (!isUserNameEmpty && userDto.UserName == userToChange.UserName) || (!isEmailEmpty && userDto.Email == userToChange.Email))
        {
            throw new ArgumentException("no actual change detected");
        }

        userToChange.Email = isEmailEmpty ? userToChange.Email : userDto.Email;
        userToChange.UserName = isUserNameEmpty ? userToChange.UserName : userDto.UserName ;

        var refreshToken = await _refreshService.GetByIdAsync(refresh) ?? throw new ArgumentException("Wrong refresh");

        if(refreshToken.UserId != userDto.Id)
        {
            throw new ArgumentException($"user with id {userDto.Id} doesn't possess refresh {refresh}");
        }

        var result = await _userManager.UpdateAsync(userToChange);

        if(result.Succeeded)
        {
            var updatedUser = await _userManager.FindByIdAsync(userDto.Id) ?? throw new Exception("no such user");

            await _messageBrokerService.PushAsync("user_update_admin", new {
                UserName = updatedUser.UserName,
                Id = updatedUser.Id,
                Email = updatedUser.Email,
            });

            await _messageBrokerService.PushAsync("user_update_userexperience", new {
                Username = updatedUser.UserName,
                Id = updatedUser.Id,
            });
        }
        return result;
    }

    public async Task PatchAvatarUrlPathAsync(string userId, string avatarPath)
    {
        var userToChange = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        if (string.IsNullOrWhiteSpace(avatarPath))
        {
            throw new ArgumentException("Logo URL path cannot be null or empty.", nameof(avatarPath));
        }
        userToChange.AvatarPath = avatarPath;

        var result = await _userManager.UpdateAsync(userToChange);

        if(!result.Succeeded)
        {
            throw new Exception("couldn't update avatar for user");
        }
    }

    public async Task UpdateUserRoleAsync(string userId, string roleId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"no role found with id: {userId}");
        var role = await _roleManager.FindByIdAsync(roleId) ?? throw new ArgumentException($"no role found with id: {roleId}");

        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);
        await _userManager.AddToRoleAsync(user, role.Name!);
    }

    public async Task UpdateBanAsync(string userId, bool IsBanned)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"no role found with id: {userId}");
        user.IsBanned = IsBanned;

        await _userManager.UpdateAsync(user);
    }

    public async Task UpdateMuteAsync(string userId, bool IsMuted)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"no role found with id: {userId}");
        user.IsMuted = IsMuted;

        await _userManager.UpdateAsync(user);
    }
}
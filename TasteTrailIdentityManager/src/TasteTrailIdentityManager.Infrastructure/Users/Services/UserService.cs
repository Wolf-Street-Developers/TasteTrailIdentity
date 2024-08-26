using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailIdentityManager.Core.Users.Services;

namespace TasteTrailIdentityManager.Infrastructure.Users.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    private readonly IRefreshTokenService _refreshService;

    public UserService(UserManager<User> userManager, IRefreshTokenService refreshService)
    {
        _userManager = userManager;
        _refreshService = refreshService;
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

    public async Task<IdentityResult> UpdateUserAsync(User user, Guid refresh)
    {
        var userToChange = await _userManager.FindByIdAsync(user.Id) ?? throw new ArgumentException($"cannot find user with id: {user.Id}");

        userToChange.Email = user.Email;
        userToChange.UserName = user.UserName;

        var refreshToken = await _refreshService.GetByIdAsync(refresh) ?? throw new ArgumentException("Wrong refresh");

        if(refreshToken.UserId != user.Id)
        {
            throw new ArgumentException($"user with id {user.Id} doesn't possess refresh {refresh}");
        }

        return await _userManager.UpdateAsync(userToChange);
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        return await _userManager.DeleteAsync(user);
    }

    public async Task<bool> HasRegisteredUsers()
    {
        return await _userManager.Users.AnyAsync();
    }

    public async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
    {
        return await _userManager.GetClaimsAsync(user) ?? throw new ArgumentException($"cannot find calims of user with id: {user.Id}");
    }

    public async Task<IdentityResult> AddUserClaimAsync(User user, Claim claim)
    {
        var existingClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == claim.Type);

        return existingClaim is null ? await _userManager.AddClaimAsync(user, claim) : throw new ArgumentException($"user {user.Email} already has this claim!");
    }

    public async Task PatchAvatarUrlPathAsync(string userId, string avatarPath)
    {
        var userToChange = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        if (string.IsNullOrWhiteSpace(avatarPath))
        {
            throw new ArgumentException("Logo URL path cannot be null or empty.", nameof(avatarPath));
        }
        userToChange.AvatarPath = avatarPath;

        await _userManager.UpdateAsync(userToChange);
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Users.Services;

namespace TasteTrailIdentityManager.Infrastructure.Users.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
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

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        _ = await _userManager.FindByIdAsync(user.Id) ?? throw new ArgumentException($"cannot find user with id: {user.Id}");

        return await _userManager.UpdateAsync(user);
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

        return existingClaim is null ? await _userManager.AddClaimAsync(user, claim) : throw new ArgumentException($"this claim is already exists!");
    }
}
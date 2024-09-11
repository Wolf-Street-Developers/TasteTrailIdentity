using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TasteTrailIdentity.Core.Users.Models;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailIdentity.Core.Roles.Models;

namespace TasteTrailIdentity.Infrastructure.Users.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

        if (!await _roleManager.RoleExistsAsync(roleName))
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} not found." });

        return await _userManager.AddToRoleAsync(user, roleName);
    }
}
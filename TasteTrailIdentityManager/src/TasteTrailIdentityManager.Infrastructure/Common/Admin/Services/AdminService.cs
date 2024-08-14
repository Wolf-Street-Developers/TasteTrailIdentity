#pragma warning disable CS1998

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Roles.Models;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Common.Data;
using TasteTrailIdentityManager.Core.Common.Admin.Services;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Infrastructure.Common.Admin.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<User> _userManager;

    private readonly RoleManager<Role> _roleManager;

        private readonly TasteTrailDbContext _context;

    public AdminService(UserManager<User> userManager, RoleManager<Role> roleManager, TasteTrailDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }
    public async Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} not found." });

        var roles = await _userManager.GetRolesAsync(user);

        if (_userManager.RemoveFromRolesAsync(user, roles).Result.Succeeded)
            return IdentityResult.Failed(new IdentityError { Description = $"Cannot rewrite roles." });

        return await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IEnumerable<User>> GetUsersByCountAsync(int count)
    {
        return await _context.Users.Take(count).ToListAsync();
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

    public async Task<IEnumerable<User>> GetUsersTotal()
    {
        return _context.Users.Include(u => u.Feedbacks).Include(u => u.Venues);
    }

    public async Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role)
    {
        var user = await _userManager.FindByNameAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");
        var roleName = role.ToString();

        if (!await _roleManager.RoleExistsAsync(roleName))
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} not found." });

        return await _userManager.RemoveFromRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ToggleBanUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new ArgumentException("User not found!");

        user.IsBanned = !user.IsBanned;
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> ToggleMuteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException($"cannot find user with id: {userId}");

        user.IsMuted = !user.IsMuted;
        await _userManager.UpdateAsync(user);

        var claims = await _userManager.GetClaimsAsync(user) ?? throw new ArgumentException($"cannot find calims of user with id: {user.Id}");

        var isMutedClaim = claims.FirstOrDefault(c => c.Type == "IsMuted") ?? throw new ArgumentException("Muted claim doesn't exist!");


        return await _userManager.ReplaceClaimAsync(user, isMutedClaim, new Claim("IsMuted", user.IsMuted.ToString()));
    }
}

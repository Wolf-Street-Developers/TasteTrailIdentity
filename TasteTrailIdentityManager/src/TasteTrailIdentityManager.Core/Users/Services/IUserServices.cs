using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Core.Users.Services;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(User user, string password);

    Task<IEnumerable<User>> GetAllAsync();
    Task<IList<string>> GetRolesByUsernameAsync(string username);
    Task<User> GetUserByIdAsync(string userId);

    Task<User> GetUserByUsernameAsync(string username);

    Task<IdentityResult> UpdateUserAsync(User user);

    Task<IdentityResult> DeleteUserAsync(string userId);

    Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role);

    Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role);

    Task AddUserClaimAsync(User user, Claim claim);

    Task ToggleBanUser(string userId);

    Task ToggleMuteUser(string userId);

    Task<bool> HasRegisteredUsers();
}
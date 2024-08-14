using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Core.Common.Admin.Services;

public interface IAdminService
{
    Task<IEnumerable<User>> GetUsersTotal();

    Task<User> GetUserByIdAsync(string userId);

    Task<IdentityResult> ToggleBanUser(string userId);

    Task<IdentityResult> ToggleMuteUser(string userId);

    Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role);

    Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role);

    Task<IList<string>> GetRolesByUsernameAsync(string username);

    Task<IList<User>> GetAllAsync();
}

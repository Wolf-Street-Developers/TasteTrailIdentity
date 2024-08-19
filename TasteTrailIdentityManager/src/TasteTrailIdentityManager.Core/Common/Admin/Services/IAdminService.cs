using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Core.Common.Admin.Services;

public interface IAdminService
{
    Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role);

    Task<IEnumerable<User>> GetFromToUsersAsync(int from, int to, Predicate<User>? filter = null);

    Task<User> GetUserByUsernameAsync(string username);

    Task<IEnumerable<string>> GetRolesByUsernameAsync(string username);

    Task<User> GetUserByIdAsync(string userId);

    Task<IEnumerable<User>> GetUsersTotal();

    Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role);

    Task<IdentityResult> ToggleBanUser(string userId);

    Task<IdentityResult> ToggleMuteUser(string userId);




}

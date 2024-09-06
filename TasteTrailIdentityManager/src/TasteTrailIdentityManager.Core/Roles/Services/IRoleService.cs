using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Core.Roles.Services;

public interface IRoleService
{
    Task<IdentityResult> CreateRoleAsync(UserRoles role);

    Task<IdentityResult> DeleteRoleAsync(UserRoles role);

    Task SetupRolesAsync();
}
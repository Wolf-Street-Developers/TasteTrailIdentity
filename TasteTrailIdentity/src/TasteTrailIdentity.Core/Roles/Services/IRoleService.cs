using TasteTrailData.Core.Roles.Enums;

namespace TasteTrailIdentity.Core.Roles.Services;

public interface IRoleService
{
    Task SetupRolesAsync();
    Task<string> GetRoleIdByName(UserRoles roleName);
}
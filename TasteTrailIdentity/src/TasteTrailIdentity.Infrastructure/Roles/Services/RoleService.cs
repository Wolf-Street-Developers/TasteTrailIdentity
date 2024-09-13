using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailIdentity.Core.Common.Services;
using TasteTrailIdentity.Core.Roles.Models;
using TasteTrailIdentity.Core.Roles.Services;

namespace TasteTrailIdentity.Infrastructure.Roles.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;

    private readonly IMessageBrokerService _messageBrokerService;

    public RoleService(RoleManager<Role> roleManager, IMessageBrokerService messageBrokerService)
    {
        _roleManager = roleManager;
        _messageBrokerService = messageBrokerService;
    }

    public async Task<string> GetRoleIdByName(UserRoles roleName)
    {
        return await _roleManager.GetRoleIdAsync(new Role(){
            Name = roleName.ToString(),
        }) ?? throw new Exception($"role {roleName} doen't exists");
    }
    public async Task SetupRolesAsync()
    {
        List<string> roleNames = [$"{UserRoles.Admin}", $"{UserRoles.User}", $"{UserRoles.Owner}"];
        List<string> ids = ["28fe9063-d351-4704-ab1d-2996660da786", "a4fdddcf-7526-4cef-820f-7c86533246fc", "087d87e1-3b25-4ef7-ab94-b2442370b3df"];
        
        

        for (int i = 0; i < roleNames.Count; i++)
        {
            var roleExists = await this._roleManager.RoleExistsAsync(roleNames[i]);

            if (!roleExists)
            {
                var role = new Role()
                {
                    Id = ids[i],
                    Name = roleNames[i]
                };
                var result = await this._roleManager.CreateAsync(role);
                
                if (!result.Succeeded)
                    throw new Exception("cannot create roles!!");
        
            }
        }
    }
}


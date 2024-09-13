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
        
        foreach (var roleName in roleNames)
        {
            var roleExists = await this._roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new Role()
                {
                    Name = roleName
                };
                var result = await this._roleManager.CreateAsync(role);
                
                if (!result.Succeeded)
                    throw new Exception("cannot create roles!!");

                await _messageBrokerService.PushAsync("role_create_admin", new {
                    Id = await _roleManager.GetRoleIdAsync(role),
                    Name = role.Name,
                });
                
                
            }
            else{
                var roleId = await _roleManager.GetRoleIdAsync(new Role(){Name = roleName});

                await _messageBrokerService.PushAsync("role_create_admin", new {
                      Id = roleId,
                      Name = roleName,
                    });
            }
  

        }
    }
}


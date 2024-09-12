using Microsoft.AspNetCore.Identity;
using TasteTrailIdentity.Core.Roles.Models;
using TasteTrailIdentity.Core.Roles.Services;

namespace TasteTrailIdentity.Infrastructure.Roles.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;

    public RoleService(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SetupRolesAsync()
    {
        List<string> roleNames = ["Admin", "User", "Owner"];
        
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
                    foreach (var error in result.Errors)
                        Console.WriteLine($"Error creating role {roleName}: {error.Description}");
            }
        }
    }
}


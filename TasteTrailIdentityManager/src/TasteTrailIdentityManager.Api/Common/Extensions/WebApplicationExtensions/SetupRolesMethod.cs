using TasteTrailIdentityManager.Core.Roles.Services;

namespace TasteTrailIdentityManager.Api.Common.Extensions.WebApplicationExtensions;

public static class SetupRolesMethod
{
    public async static Task SetupRoles(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
            await roleService.SetupRolesAsync();
        }
    }
    
}

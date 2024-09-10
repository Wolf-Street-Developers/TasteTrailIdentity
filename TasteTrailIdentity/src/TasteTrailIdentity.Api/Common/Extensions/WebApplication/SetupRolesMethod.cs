namespace TasteTrailIdentity.Api.Common.Extensions.WebApplication;

using TasteTrailIdentity.Core.Roles.Services;
using Microsoft.AspNetCore.Builder;

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




namespace TasteTrailIdentity.Api.Common.Extensions.WebApplication;

using Microsoft.EntityFrameworkCore;
using TasteTrailIdentity.Infrastructure.Common.Data;
using Microsoft.AspNetCore.Builder;

public static class UpdateDbMethod
{
    public static void UpdateDb(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<TasteTrailIdentityDbContext>();
        
            dbContext.Database.Migrate();
        }
    }
}

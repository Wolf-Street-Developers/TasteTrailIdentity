using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TasteTrailIdentity.Core.Roles.Models;
using TasteTrailIdentity.Core.Users.Models;
using TasteTrailIdentity.Infrastructure.Common.Data;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;

public static class InitAspNetIdentityMethod
{
    public static void InitAspnetIdentity(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<TasteTrailIdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("SqlConnection") ?? throw new SystemException("connectionString is not set");
            options.UseNpgsql(connectionString);
        });

        serviceCollection.AddIdentity<User, Role>( (options) => {
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<TasteTrailIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();
    }
}



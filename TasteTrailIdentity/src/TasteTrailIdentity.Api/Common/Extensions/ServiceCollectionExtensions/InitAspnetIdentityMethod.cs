namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;

using Microsoft.EntityFrameworkCore;
using TasteTrailData.Core.Roles.Models;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Common.Data;

public static class InitAspnetIdentityMethod
{
    public static void InitAspnetIdentity(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<TasteTrailDbContext>(options =>
        {
            var connectinoString = configuration.GetConnectionString("SqlConnection");
            options.UseNpgsql(connectinoString);
        });

        serviceCollection.AddIdentity<User, Role>( (options) => {
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<TasteTrailDbContext>();
    }
}

using Microsoft.AspNetCore.Cors.Infrastructure;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;

public static class InitCorseMethod
{
    public static void InitCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(delegate (CorsOptions options)
        {
            options.AddPolicy("AllowAllOrigins", delegate (CorsPolicyBuilder builder)
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }
}

namespace TasteTrailIdentityManager.Api.Common.Extensions.ServiceCollectionExtensions;

public static class InitCorsMethod
{
    public static void InitCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(name: "LocalHostPolicy", policyBuilder =>
            {
                policyBuilder
                    .WithOrigins("http://localhost")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}

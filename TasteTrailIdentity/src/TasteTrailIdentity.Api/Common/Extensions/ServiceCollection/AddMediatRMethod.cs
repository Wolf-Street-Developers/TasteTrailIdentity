using TasteTrailIdentity.Infrastructure.Common.Data;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;
public static class AddMediatRMethod
{
    public static void AddMediatR(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configuration => {
            Type typeInReferencedAssembly = typeof(TasteTrailIdentityDbContext);
            configuration.RegisterServicesFromAssembly( typeInReferencedAssembly.Assembly );
        });
    } 
}

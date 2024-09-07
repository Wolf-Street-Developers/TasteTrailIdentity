using TasteTrailData.Infrastructure.Common.Data;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;
public static class AddMediatRMethod
{
    public static void AddMediatR(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configuration => {
            Type typeInReferencedAssembly = typeof(TasteTrailDbContext);
            configuration.RegisterServicesFromAssembly( typeInReferencedAssembly.Assembly );
        });
    } 
}

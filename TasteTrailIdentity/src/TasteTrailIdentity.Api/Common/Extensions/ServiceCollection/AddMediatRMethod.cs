using TasteTrailIdentity.Infrastructure.Common.Assembly;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;
public static class AddMediatRMethod
{
    public static void AddMediatR(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configuration => {
            Type typeInReferencedAssembly = typeof(InfrastructureAssemblyMaker);
            configuration.RegisterServicesFromAssembly( typeInReferencedAssembly.Assembly );
        });
    } 
}

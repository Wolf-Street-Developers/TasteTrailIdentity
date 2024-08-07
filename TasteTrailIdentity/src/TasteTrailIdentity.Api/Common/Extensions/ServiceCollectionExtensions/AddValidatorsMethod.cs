namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;

using System.Reflection;
using FluentValidation;

public static class AddValidatorsMethod
{
    public static void AddValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

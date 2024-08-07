namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;

using TasteTrailIdentity.Core.Authentication.Services;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailIdentity.Core.Roles.Services;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailIdentity.Infrastructure.Authentication.Services;
using TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Repositories.Ef_Core;
using TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Services;
using TasteTrailIdentity.Infrastructure.Roles.Services;
using TasteTrailIdentity.Infrastructure.Users.Services;

public static class RegisterDpInjectionMethod
{
    public static void RegisterDpInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IUserService, UserService>();
        serviceCollection.AddTransient<IRoleService, RoleService>();
        serviceCollection.AddTransient<IIdentityAuthService, IdentityAuthService>();

        serviceCollection.AddTransient<IRefreshTokenRepository, RefreshTokenEfCoreRepository>();
        serviceCollection.AddTransient<IRefreshTokenService, RefreshTokenService>();
    } 
}

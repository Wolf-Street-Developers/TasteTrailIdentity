namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollectionExtensions;

using TasteTrail.Data.src.Core.Authentication.Services;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailData.Core.Roles.Services;
using TasteTrailData.Core.Users.Services;
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

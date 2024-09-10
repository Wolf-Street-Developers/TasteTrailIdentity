namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;

using TasteTrailIdentity.Core.Authentication.Services;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailIdentity.Core.Roles.Services;
using TasteTrailIdentity.Core.Users.Managers;
using TasteTrailIdentity.Core.Users.Services;
using TasteTrailIdentity.Infrastructure.Authentication.Services;
using TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Repositories.Ef_Core;
using TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Services;
using TasteTrailIdentity.Infrastructure.Roles.Services;
using TasteTrailIdentity.Infrastructure.Users.Managers;
using TasteTrailIdentity.Infrastructure.Users.Services;

public static class RegisterDependencyInjectionMethod 
{
    public static void RegisterDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IUserService, UserService>();
        serviceCollection.AddTransient<IRoleService, RoleService>();
        serviceCollection.AddTransient<IIdentityAuthService, IdentityAuthService>();

        serviceCollection.AddTransient<IRefreshTokenRepository, RefreshTokenEfCoreRepository>();
        serviceCollection.AddTransient<IRefreshTokenService, RefreshTokenService>();

        serviceCollection.AddTransient<IUserImageManager, UserImageManager>();
    } 
}

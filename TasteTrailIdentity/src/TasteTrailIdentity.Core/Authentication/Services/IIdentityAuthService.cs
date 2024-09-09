using Microsoft.AspNetCore.Identity;
using TasteTrailIdentity.Core.Users.Models;
using TasteTrailIdentity.Core.Common.Tokens.AccessTokens.Entities;

namespace TasteTrailIdentity.Core.Authentication.Services;
public interface IIdentityAuthService
{
    Task<IdentityResult> RegisterAsync(User user, string password);

    Task<AccessToken> SignInAsync(string username, string password, bool rememberMe);
    
    Task<Guid> SignOutAsync(Guid refresh, string jwt);

    Task<AccessToken> UpdateToken(Guid refresh, string jwt);
}
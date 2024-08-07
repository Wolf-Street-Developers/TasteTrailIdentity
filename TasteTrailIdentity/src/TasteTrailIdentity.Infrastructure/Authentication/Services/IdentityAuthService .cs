using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TasteTrail.Data.src.Core.Authentication.Services;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Entities;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Services;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Core.Users.Services;
using TasteTrailData.Infrastructure.Common.Dtos;
using TasteTrailIdentity.Infrastructure.Common.Options;

namespace TasteTrailIdentity.Infrastructure.Authentication.Services;

public class IdentityAuthService : IIdentityAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly IUserService _userService;
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenService _refreshTokenService;

    public IdentityAuthService(
        SignInManager<User> signInManager, 
        IUserService userService, 
        IOptionsSnapshot<JwtOptions> jwtOptionsSnapshot,
        IRefreshTokenService refreshTokenService
        )
    {
        _refreshTokenService = refreshTokenService;
        _signInManager = signInManager;
        _userService = userService;
        _jwtOptions = jwtOptionsSnapshot.Value;
    }

    public async Task<IdentityResult> RegisterAsync(User user, string password) {
        
        var creationResult = await _userService.CreateUserAsync(user, password);
        var roleAssignResult = await _userService.AssignRoleToUserAsync(user.Id, UserRoles.User);

        var result = creationResult.Succeeded && roleAssignResult.Succeeded;

        var errors = new List<IdentityError>();

        errors.AddRange(creationResult.Errors);
        errors.AddRange(roleAssignResult.Errors);

        return result ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
    }

    public async Task<AccessToken> SignInAsync(string username, string password, bool rememberMe)
    {
        var user = await _userService.GetUserByUsernameAsync(username);

        if(user == null)
            throw new InvalidCredentialException("User not found!");

        var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

        if (user.IsBanned)
            throw new AuthenticationFailureException("Account is banned!");

        if (!result.Succeeded)
            throw new AuthenticationFailureException("Invalid credentials!");

        await _userService.AddUserClaimAsync(user, new Claim("IsMuted", user.IsMuted.ToString()));

        var roles = await _userService.GetRoleByUsernameAsync(username);

        var claims = roles
            .Select(roleStr => new Claim(ClaimTypes.Role, roleStr))
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id))
            .Append(new Claim(ClaimTypes.Email, user.Email ?? "not set"))
            .Append(new Claim(ClaimTypes.Name, user.UserName ?? "not set"));

        var signingKey = new SymmetricSecurityKey(_jwtOptions.KeyInBytes);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.LifeTimeInMinutes),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var tokenStr = handler.WriteToken(token);

        var refreshToken = new RefreshToken {
            UserId = user.Id,
        };

        await _refreshTokenService.CreateAsync(refreshToken);

        return new AccessToken{
            Refresh = refreshToken.Token,
            Jwt = tokenStr,
        };
    }

    public async Task<AccessToken> UpdateToken(Guid refresh, string jwt)
    {
        if(jwt is null) {
            throw new InvalidCredentialException("jwt is null!");
        }

        if(jwt.StartsWith("Bearer ")) {
            jwt = jwt.Substring("Bearer ".Length);
        }

        var handler = new JwtSecurityTokenHandler();
        var tokenValidationResult = await handler.ValidateTokenAsync(
            jwt,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtOptions.KeyInBytes)
            }
        );

        if(tokenValidationResult.IsValid == false) {
            throw new InvalidCredentialException("invalid jwt token!");
        }

        var token = handler.ReadJwtToken(jwt);

        Claim? idClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if(idClaim is null) {
            throw new InvalidCredentialException($"Token has no claim with type '{ClaimTypes.NameIdentifier}'");
        }

        var userId = idClaim.Value;

        var foundUser = await _userService.GetUserByIdAsync(userId);

        if(foundUser is null) {
            throw new InvalidCredentialException($"User not found by id: '{userId}'");
        }

        var oldRefreshToken = await _refreshTokenService.GetByIdAsync(refresh);

        if(oldRefreshToken is null)
        {
            await _refreshTokenService.DeleteRangeRefreshTokensAsync(userId: userId);
            throw new InvalidCredentialException("Refresh token not found!");
        }

        await _refreshTokenService.DeleteByIdAsync(refresh);

        var newRefreshToken = await _refreshTokenService.CreateAsync(new RefreshToken{
            UserId = userId
        }) ;

        var roles = await _userService.GetRoleByUsernameAsync(foundUser.UserName!);

        var claims = roles
            .Select(roleStr => new Claim(ClaimTypes.Role, roleStr))
            .Append(new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()))
            .Append(new Claim(ClaimTypes.Email, foundUser.Email ?? "not set"))
            .Append(new Claim(ClaimTypes.Name, foundUser.UserName ?? "not set"));

        var signingKey = new SymmetricSecurityKey(_jwtOptions.KeyInBytes);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var newToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.LifeTimeInMinutes),
            signingCredentials: signingCredentials
        );

        var newTokenStr = handler.WriteToken(newToken);

        return new AccessToken {
            Refresh = newRefreshToken,
            Jwt = newTokenStr,
        };
    }

    public async Task<Guid> SignOutAsync(Guid refresh, string jwt)
    {
        if(jwt is null) {
            throw new InvalidCredentialException("jwt is null!");
        }

        if(jwt.StartsWith("Bearer ")) {
            jwt = jwt.Substring("Bearer ".Length);
        }

        var handler = new JwtSecurityTokenHandler();
        var tokenValidationResult = await handler.ValidateTokenAsync(
            jwt,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_jwtOptions.KeyInBytes)
            }
        );

        if(tokenValidationResult.IsValid == false) {
            throw new InvalidCredentialException("invalid jwt token!");
        }

        var token = handler.ReadJwtToken(jwt);

        Claim? idClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if(idClaim is null) {
            throw new InvalidCredentialException($"Token has no claim with type '{ClaimTypes.NameIdentifier}'");
        }

        var userId = idClaim.Value;

        var foundUser = await _userService.GetUserByIdAsync(userId);

        if(foundUser is null) {
            throw new InvalidCredentialException($"User not found by id: '{userId}'");
        }

        return await _refreshTokenService.DeleteByIdAsync(refresh);
    }

}
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Users.Models;

namespace TasteTrailIdentity.Core.Users.Services;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(User user, string password);

    Task<IList<string>> GetRolesByUsernameAsync(string username);

    Task<IList<string>> GetRolesByEmailAsync(string email);

    Task<User> GetUserByIdAsync(string userId);

    Task<User> GetUserByUsernameAsync(string username);

    Task<User> GetUserByEmailAsync(string email);

    Task<IdentityResult> UpdateUserAsync(User user, Guid refresh);

    Task<IdentityResult> DeleteUserAsync(string userId);

    Task<bool> HasRegisteredUsers();

    Task<IEnumerable<Claim>> GetUserClaimsAsync(User user);

    Task<IdentityResult> AddUserClaimAsync(User user, Claim claim);

    Task PatchAvatarUrlPathAsync(string userId, string avatarUrl); 

}
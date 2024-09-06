using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailData.Core.Roles.Enums;
using TasteTrailIdentityManager.Core.Users.Dtos;

namespace TasteTrailIdentityManager.Core.Common.Admin.Services;

public interface IAdminService
{
    Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role);

    Task<FilterResponseDto<UserResponseDto>> GetUsersFilteredAsync(FilterParametersDto filterParameters);

    Task<FilterResponseDto<UserResponseDto>> GetUsersFiltereBySearchdAsync(FilterParametersSearchDto filterParameters);

    Task<User> GetUserByUsernameAsync(string username);

    Task<IEnumerable<string>> GetRolesByUsernameAsync(string username);

    Task<User> GetUserByIdAsync(string userId);

    Task<int> GetUsersCountAsync();

    Task<int> GetCountFilteredAsync(FilterParametersDto filterParameters);

    Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role);

    Task<IdentityResult> ToggleBanUserAsync(string userId);

    Task<IdentityResult> ToggleMuteUserAsync(string userId);




}

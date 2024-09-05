using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Core.Users.Models;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Core.Common.Admin.Services;

public interface IAdminService
{
    Task<IdentityResult> AssignRoleToUserAsync(string userId, UserRoles role);

    Task<FilterResponseDto<User>> GetUsersFilteredAsync(PaginationParametersDto paginationParameters);

    Task<FilterResponseDto<User>> GetUsersFilteredAsync(PaginationSearchParametersDto paginationParameters);

    Task<FilterResponseDto<User>> GetUsersFilteredAsync(FilterParametersDto filterParameters);

    Task<FilterResponseDto<User>> GetUsersFilteredAsync(FilterParametersSearchDto filterParameters);

    Task<User> GetUserByUsernameAsync(string username);

    Task<IEnumerable<string>> GetRolesByUsernameAsync(string username);

    Task<User> GetUserByIdAsync(string userId);

    Task<int> GetUsersCountAsync();

    Task<int> GetCountBySpecificationAsync(IFilterSpecification<User>? specification);

    Task<IdentityResult> RemoveRoleFromUserAsync(string userId, UserRoles role);

    Task<IdentityResult> ToggleBanUserAsync(string userId);

    Task<IdentityResult> ToggleMuteUserAsync(string userId);




}

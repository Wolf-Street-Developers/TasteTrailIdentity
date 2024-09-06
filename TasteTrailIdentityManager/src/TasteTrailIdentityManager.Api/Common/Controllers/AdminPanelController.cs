using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailIdentityManager.Core.Common.Admin.Services;
using TasteTrailIdentityManager.Core.Roles.Enums;

namespace TasteTrailIdentityManager.Api.Common.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AdminPanelController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminPanelController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("User/Count")]
    public async Task<IActionResult> GetUsersCount()
    {
        try
        {
            var usersCount = await _adminService.GetUsersCountAsync();

            return Ok(new {
                usersCount = usersCount
            });
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("User/CountFiltered")]
    public async Task<IActionResult> GetUsersCountFiltered([FromQuery]FilterParametersDto filterParameters)
    {
        try
        {
            var usersCount = await _adminService.GetCountFilteredAsync(filterParameters);

            return Ok(new {
                usersCount = usersCount
            });
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpGet("User")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersAsync([FromQuery]PaginationParametersDto dto)
    {
        try
        {
            var userResonseDto = await _adminService.GetUsersAsync(dto);

            return Ok(userResonseDto);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpGet("User/Search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersBySearchAsync([FromQuery]PaginationSearchParametersDto dto)
    {
        try
        {
            var userResonseDto = await _adminService.GetUsersBySearchAsync(dto);

            return Ok(userResonseDto);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpGet("User/Filtered")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersFilteredAsync([FromQuery]FilterParametersDto dto)
    {
        try
        {
            var userResonseDto = await _adminService.GetUsersFilteredAsync(dto);

            return Ok(userResonseDto);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpGet("User/Filtered/Search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersFiltereBySearchdAsync([FromQuery]FilterParametersSearchDto dto)
    {
        try
        {
            var userResonseDto = await _adminService.GetUsersFiltereBySearchdAsync(dto);

            return Ok(userResonseDto);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpPost("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleAsync([FromQuery] string userId, [FromQuery] UserRoles role)
    {
        try
        {
            var result = await _adminService.AssignRoleToUserAsync(userId, role);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpPost("[action]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRoleAsync([FromQuery] string userId, [FromQuery] UserRoles role)
    {
        try
        {
            var result = await _adminService.RemoveRoleFromUserAsync(userId: userId, role);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpPost("[action]/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleMuteAsync(string userId)
    {
        try
        {
            var result = await _adminService.ToggleMuteUserAsync(userId);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpPost("[action]/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleBanAsync(string userId)
    {
        try
        {
            var result = await _adminService.ToggleBanUserAsync(userId);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

    [HttpGet("/api/[controller]/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserInfoAsync(string userId)
    {
        try
        {
            var user = await _adminService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }

  
}

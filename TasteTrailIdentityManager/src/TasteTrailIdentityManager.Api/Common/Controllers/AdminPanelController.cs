using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailIdentityManager.Core.Common.Admin.Services;
using TasteTrailData.Core.Roles.Enums;

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

    [HttpPost("User/Count")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersCountAsync([FromBody]FilterParametersDto filterParameters)
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

    [HttpPost("UserFilter")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsersAsync([FromBody]FilterParametersSearchDto dto)
    {
        try
        {
            var userResonseDto = await _adminService.GetUsersFiltereBySearchdAsync(dto);

            return Ok(userResonseDto);
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

    [HttpGet("UserInfo/{userId}")]
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

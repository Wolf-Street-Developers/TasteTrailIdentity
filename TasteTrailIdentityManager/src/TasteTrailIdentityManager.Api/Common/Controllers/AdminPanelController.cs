using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailIdentityManager.Core.Common.Admin.Services;
using TasteTrailIdentityManager.Core.Roles.Enums;
using TasteTrailIdentityManager.Infrastructure.Common.Dtos;
using TasteTrailIdentityManager.Infrastructure.Users.Dtos;

namespace TasteTrailIdentityManager.Api.Common.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AdminPanelController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminPanelController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> DashboardInfoAsync()
    {
        try
        {
            var users = await _adminService.GetUsersTotal();
            int feedbacks = 0;
            int venues = 0;

            foreach (var user in users)
            {
                if(user.Feedbacks.Count() == 0)
                {
                    continue;
                }
                feedbacks += user.Feedbacks.Count();

                if(user.Venues.Count() == 0)
                {
                    continue;
                }
                feedbacks += user.Venues.Count();
            }

            var dto = new AdminDashboardDto
            {
                TotalUsers = users.Count(),
                TotalFeedbacks = feedbacks,
                TotalVenues = venues
            };

            return Ok(dto);
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

    [HttpPost]
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRoleAsync([FromQuery] string username, [FromQuery] UserRoles role)
    {
        try
        {
            var result = await _adminService.RemoveRoleFromUserAsync(username, role);
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

    [HttpPost("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleMuteAsync(string userId)
    {
        try
        {
            var result = await _adminService.ToggleMuteUser(userId);
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

    [HttpPost("{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleBanAsync(string userId)
    {
        try
        {
            var result = await _adminService.ToggleBanUser(userId);
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

    [HttpGet("/api/[controller]/User/{userId}")]
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

      [HttpGet("/api/[controller]/User")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var users = await _adminService.GetAllAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _adminService.GetRolesByUsernameAsync(user.UserName!);

                var userDto = new UserDto()
                {
                    User = user,
                    Roles = roles.ToList()
                };

                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }
}

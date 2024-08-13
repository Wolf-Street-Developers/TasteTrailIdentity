using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Authentication.Services;
using TasteTrailIdentityManager.Core.Users.Services;
using TasteTrailIdentityManager.Infrastructure.Users.Dtos;

namespace TasteTrailIdentityManager.Api.Users.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IIdentityAuthService _identityAuthService;

    public UserController(IIdentityAuthService identityAuthService, IUserService userService)
    {
        _identityAuthService = identityAuthService;
        _userService = userService;
    }

    [HttpGet("/api/[controller]")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var users = await _userService.GetAllAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userService.GetRolesByUsernameAsync(user.UserName!);

                var userDto = new UserDto()
                {
                    User = user,
                    Roles = roles
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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserRolesAsync(string id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            var roles = await _userService.GetRolesByUsernameAsync(user.UserName!);

            var userDto = new UserDto()
            {
                User = user,
                Roles = roles
            };

            return Ok(userDto);
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

    [HttpPut("/api/[controller]/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserDto model, [FromBody] Guid refresh)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }

            var result = await _userService.UpdateUserAsync(new User()
            {
                Email = model.Email,
                UserName = model.Name
            });

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var jwt = HttpContext.Request.Headers.Authorization.FirstOrDefault();

            await _identityAuthService.SignOutAsync(jwt: jwt!, refresh: refresh);
            return Ok();
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

    // [HttpPost]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> AssignRole([FromQuery] string userId, [FromQuery] UserRoles role)
    // {
    //     try
    //     {
    //         await _userService.AssignRoleToUserAsync(userId, role);
    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message); // Возвращаем ошибку 400 при исключении
    //     }
    // }

    // [HttpPost]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> RemoveRole([FromQuery] string username, [FromQuery] UserRoles role)
    // {
    //     try
    //     {
    //         await _userService.RemoveRoleFromUserAsync(username, role);
    //     }
    //     catch (Exception ex)
    //     {
    //         BadRequest(ex.Message);
    //     }

    //     return Ok();
    // }

    // [HttpPost("{userId}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> ToggleMute(string userId)
    // {
    //     try
    //     {
    //         await _userService.ToggleMuteUser(userId);
    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }

    // }

    // [HttpPost("{userId}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> ToggleBan(string userId)
    // {
    //     try
    //     {
    //         await _userService.ToggleBanUser(userId);
    //     }
    //     catch (Exception ex)
    //     {
    //         BadRequest(ex.Message);
    //     }

    //     return RedirectToAction("Index");
    // }

    [HttpGet("/api/[controller]/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserInfoAsync(string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user is not null ? Ok(user) : BadRequest("")
    }
}
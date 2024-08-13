using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
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

                var userViewModel = new UserDto()
                {
                    User = user,
                    Roles = roles
                };

                userDtos.Add(userViewModel);
            }

            return Ok(userDtos);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }


    // [HttpGet("{id}")]
    // public async Task<IActionResult> Json(string id)
    // {
    //     var user = await this._userService.GetUserByIdAsync(id);
    //     var role = await this._userService.GetRolesByUsernameAsync(user.UserName!);

    //     if (user == null)
    //     {
    //         return NotFound();
    //     }

    //     return Json(new { id = user.Id, name = user.UserName, email = user.Email, role = role });
    // }

    // [HttpPut]
    // [Authorize]
    // public async Task<IActionResult> Update([FromBody] UpdateUserViewModel model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         var user = await _userService.GetUserByIdAsync(model.Id);
    //         if (user == null)
    //         {
    //             return NotFound();
    //         }

    //         user.UserName = model.Name;
    //         user.Email = model.Email;

    //         var result = await _userService.UpdateUserAsync(user);
    //         if (result.Succeeded)
    //         {
    //             await this._identityAuthService.SignOutAsync();
    //             return base.Ok();
        
    //         }

    //         return BadRequest(result.Errors);
    //     }

    //     return BadRequest(ModelState);
    // }

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

    // [HttpGet("{userId}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<IActionResult> UserInfo(string userId)
    // {
    //     var user = await this._userService.GetUserByIdAsync(userId);
    //     if (user == null)
    //     {
    //         return NotFound();
    //     }
    //     return View(user);
    // }
}
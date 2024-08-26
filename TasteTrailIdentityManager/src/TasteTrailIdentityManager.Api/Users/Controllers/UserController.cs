using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentityManager.Core.Authentication.Services;
using TasteTrailIdentityManager.Core.Users.Services;
using TasteTrailIdentityManager.Infrastructure.Users.Dtos;
using TasteTrailIdentityManager.Core.Users.Managers;
using System.Security.Claims;

namespace TasteTrailIdentityManager.Api.Users.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserImageManager _userImageManager;
    private readonly IIdentityAuthService _identityAuthService;

    public UserController(IIdentityAuthService identityAuthService, IUserService userService, IUserImageManager userImageManager)
    {
        _identityAuthService = identityAuthService;
        _userService = userService;
        _userImageManager = userImageManager;
    }


    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetUserRolesAsync()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByIdAsync(userId!);
            var roles = await _userService.GetRolesByUsernameAsync(user.UserName!);

            var userDto = new UserResponseDto()
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

    [HttpPut("/api/[controller]/{refresh}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync([FromBody]UpdateUserDto model, [FromQuery] Guid refresh)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _userService.UpdateUserAsync(new User()
            {
                Id = userId!,
                Email = model.Email,
                UserName = model.Name
            }, refresh);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var jwt = HttpContext.Request.Headers.Authorization.FirstOrDefault();

            _ = await _identityAuthService.SignOutAsync(jwt: jwt!, refresh: refresh);
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

    [HttpPatch("/api/[controller]/Avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateAvatarAsync(IFormFile? avatar)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var avatarUrlPath = await _userImageManager.SetImageAsync(userId!, avatar);

            return Ok(new {
                AvatarUrlPath = avatarUrlPath,
            });
        }       
        catch(ArgumentException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(InvalidOperationException exception)
        {   
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return this.InternalServerError(exception.Message);
        }
    }


}
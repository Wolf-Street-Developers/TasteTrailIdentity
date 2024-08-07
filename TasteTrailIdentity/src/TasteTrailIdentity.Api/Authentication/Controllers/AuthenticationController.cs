using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Core.Users.Models;
using TasteTrailIdentity.Core.Authentication.Services;
using TasteTrailIdentity.Infrastructure.Identities.Dtos;

namespace TasteTrailIdentity.Api.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AuthenticationController : ControllerBase
{

    private readonly IIdentityAuthService identityAuthService;

    public AuthenticationController(
        IIdentityAuthService identityAuthService
    )
    {
        this.identityAuthService = identityAuthService;
    }

    [HttpPost(Name = "LoginEndPoint")]
    public async Task<IActionResult> Login([Required, FromForm] LoginDto loginDto)
    {
        try
        {
            var accessToken = await identityAuthService.SignInAsync(loginDto.Username, loginDto.Password, true);
            return Ok(accessToken);
        }
        catch(InvalidCredentialException exeption)
        {
            return BadRequest(exeption.Message);
        }
        catch(ArgumentException exeption)
        {
            return BadRequest(exeption.Message);
        }
        catch(AuthenticationFailureException exeption)
        {
            return Forbid(exeption.Message);
        }
        catch (Exception exeption)
        {
            return StatusCode(500, exeption.Message);
        }
    }

    [HttpPost(Name = "RegistrationEndpoint")]
    public async Task<IActionResult> Registration([Required, FromForm] RegistrationDto registrationDto)
    {
        try
        {
            var user = new User
            {
                UserName = registrationDto.Name,
                Email = registrationDto.Email,
            };

            var result = await identityAuthService.RegisterAsync(user, registrationDto.Password);

            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        catch(ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(InvalidCredentialException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(Exception exeption)
        {
            return StatusCode(500, exeption.Message);
        }
    }

    [Authorize]
    [HttpPatch(Name = "LogOut")]
    public async Task<IActionResult> Logout([Required, FromForm] Guid refresh)
    {
        try
        {
            var jwt = base.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var deletedToken = await identityAuthService.SignOutAsync(refresh, jwt!);

            return Ok(new {
                Token = deletedToken
            });
        }
        catch(ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(InvalidCredentialException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
    
    [Authorize]
    [HttpPut(Name = "UpdateToken")]
    public async Task<IActionResult> UpdateToken([Required, FromBody]Guid refresh)
    {
        try
        {
            var jwt = base.HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var accessToken = await identityAuthService.UpdateToken(refresh, jwt!);

            return Ok(accessToken);
        }
        catch(ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(InvalidCredentialException exception)
        {
            return BadRequest(exception.Message);
        }
        catch(Exception exception)
        {
            return StatusCode(500, exception.Message);
        }
    }
}

using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
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


}

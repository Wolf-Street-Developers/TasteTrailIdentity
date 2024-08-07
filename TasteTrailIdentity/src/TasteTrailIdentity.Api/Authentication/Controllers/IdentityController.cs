using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using TasteTrail.Data.src.Core.Authentication.Services;
using TasteTrailData.Infrastructure.Identities.Dtos;

namespace TasteTrailIdentity.Api.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class IdentityController : ControllerBase
{

    private readonly IIdentityAuthService _identityAuthService;

    public IdentityController(
        IIdentityAuthService identityAuthService
    )
    {
        _identityAuthService = identityAuthService;
    }

    [HttpPost(Name = "LoginEndPoint")]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        try
        {
            
        }
        catch (Exception exeption)
        {
            
            
        }
    }


}

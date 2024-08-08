#pragma warning disable CS8618

namespace TasteTrailIdentity.Infrastructure.Identities.Dtos;

public class LoginDto
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}
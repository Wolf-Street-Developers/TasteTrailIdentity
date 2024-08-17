namespace TasteTrailIdentityManager.Infrastructure.Identities.Dtos;

public class LoginDto
{
    public required string NameIdentifier { get; set; }
    public required string Password { get; set; }
}
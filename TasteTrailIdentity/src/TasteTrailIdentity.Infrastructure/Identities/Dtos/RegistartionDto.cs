#pragma warning disable CS8618

namespace TasteTrailIdentity.Infrastructure.Identities.Dtos;

public class RegistrationDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
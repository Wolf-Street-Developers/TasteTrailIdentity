using TasteTrailData.Core.Users.Models;

namespace TasteTrailIdentityManager.Infrastructure.Users.Dtos;

public class UserResponseDto
{
    public required User User { get; set; }
    public required ICollection<string> Roles { get; set; }
}

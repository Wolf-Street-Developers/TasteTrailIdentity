using TasteTrailData.Core.Users.Models;

namespace TasteTrailIdentityManager.Infrastructure.Users.Dtos;

public class UserDto
{
    public required User User { get; set; }
    public required ICollection<string> Roles { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;

public class RefreshToken
{
    [Key] 
    public int Id { get; set; }
    public Guid Token { get; set; }
    public required string UserId { get; set; }
    public DateTime CreationDate { get; set; }
}

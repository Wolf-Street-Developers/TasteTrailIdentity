namespace TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;

public class RefreshToken
{
    public Guid Token { get; set; }
    public required string UserId { get; set; }
    public DateTime CreationDate { get; set; }
}

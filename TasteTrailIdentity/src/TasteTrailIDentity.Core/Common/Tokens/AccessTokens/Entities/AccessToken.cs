#pragma warning disable CS8618

namespace TasteTrailIdentity.Core.Common.Tokens.AccessTokens.Entities;

public class AccessToken
{
    public Guid Refresh { get; set; }
    public string Jwt { get; set; }
}

#pragma warning disable CS8618

namespace TasteTrailIdentityManager.Core.Common.Tokens.AccessTokens.Entities;

public class AccessToken
{
    public Guid Refresh { get; set; }
    public string Jwt { get; set; }
}

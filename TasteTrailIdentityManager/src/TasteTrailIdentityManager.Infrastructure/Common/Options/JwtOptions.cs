#pragma warning disable CS8618

using System.Text;

namespace TasteTrailIdentityManager.Infrastructure.Common.Options;

public class JwtOptions
{
    public string Key { get; set; }
    public byte[] KeyInBytes => Encoding.ASCII.GetBytes(Key);
    public int LifeTimeInMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
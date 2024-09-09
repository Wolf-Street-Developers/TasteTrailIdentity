#pragma warning disable CS8618

using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Common.Models.Base;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Entities;

namespace TasteTrailIdentity.Core.Users.Models;

public class User : IdentityUser, IBanable
{
    [DefaultValue(false)]
    public bool IsBanned { get; set; }

    public string? AvatarPath { get; set; }
    [JsonIgnore]
    public ICollection<RefreshToken> RefreshTokens { get; set; }

}
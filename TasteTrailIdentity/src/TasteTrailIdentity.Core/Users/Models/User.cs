#pragma warning disable CS8618

using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using TasteTrailData.Core.Common.Models.Base;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;

namespace TasteTrailIdentity.Core.Users.Models;

public class User : IdentityUser, IBanable, IMuteable
{
    [DefaultValue(false)]
    public bool IsBanned { get; set; }

    [DefaultValue(false)]
    public bool IsMuted { get; set; }

    public string? AvatarPath { get; set; }
    [JsonIgnore]
    public ICollection<RefreshToken> RefreshTokens { get; set; }

}
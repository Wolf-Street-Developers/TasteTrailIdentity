using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;

namespace TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .HasKey(rt => rt.Token);
        
        builder
            .Property(f => f.CreationDate)
            .IsRequired();
    }
}

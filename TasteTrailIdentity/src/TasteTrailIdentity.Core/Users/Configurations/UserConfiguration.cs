using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailIdentity.Core.Users.Models;

namespace TasteTrailData.Core.Users.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.Property(u => u.IsBanned)
            .IsRequired();


        builder.HasMany(u => u.RefreshTokens)
            .WithOne()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
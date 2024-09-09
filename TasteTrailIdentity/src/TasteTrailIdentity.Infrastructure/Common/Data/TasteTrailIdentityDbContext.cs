using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TasteTrailIdentity.Core.Users.Configurations;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Configurations;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;
using TasteTrailIdentity.Core.Roles.Models;
using TasteTrailIdentity.Core.Users.Models;

namespace TasteTrailIdentity.Infrastructure.Common.Data;
public class TasteTrailIdentityDbContext : IdentityDbContext<User, Role, string>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public TasteTrailIdentityDbContext(DbContextOptions options) : base(options)
    {}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }

}


using Microsoft.EntityFrameworkCore;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailIdentity.Infrastructure.Common.Data;

namespace TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Repositories.Ef_Core;

public class RefreshTokenEfCoreRepository : IRefreshTokenRepository
{
    private readonly TasteTrailIdentityDbContext _dbContext;
    public RefreshTokenEfCoreRepository(TasteTrailIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Guid> CreateAsync(RefreshToken entity)
    {   
        await _dbContext.RefreshTokens.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Token;
    }

    public async Task<Guid> DeleteByIdAsync(Guid id)
    {
        var tokenToDelete = _dbContext.RefreshTokens.Where(rt => rt.Token == id).FirstOrDefault();
        _dbContext.RefreshTokens.Remove(tokenToDelete!);

        await _dbContext.SaveChangesAsync();

        return id;
    }

    public async Task<int> DeleteRangeRefreshTokensAsync(string userId)
    {
        var refreshTokens = _dbContext.RefreshTokens.Where(rt => rt.UserId == userId).AsNoTracking();
        _dbContext.RemoveRange(refreshTokens);
        await _dbContext.SaveChangesAsync();

        return refreshTokens.Count();
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _dbContext.RefreshTokens.Where(rt => rt.Token == id).AsNoTracking().FirstOrDefaultAsync();
    }
}

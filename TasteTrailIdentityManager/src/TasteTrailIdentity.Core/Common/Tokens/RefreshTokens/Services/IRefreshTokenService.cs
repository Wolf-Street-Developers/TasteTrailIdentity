using TasteTrailData.Core.Common.Tokens.RefreshTokens.Entities;

namespace TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Services;

public interface IRefreshTokenService
{
    public Task<int> DeleteRangeRefreshTokensAsync(string userId);
    Task<Guid> DeleteByIdAsync(Guid id);
    Task<Guid> CreateAsync(RefreshToken entity);
    Task<RefreshToken?> GetByIdAsync(Guid id);
}

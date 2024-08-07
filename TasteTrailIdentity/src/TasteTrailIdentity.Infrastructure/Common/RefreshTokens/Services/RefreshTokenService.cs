

using TasteTrailData.Core.Common.Tokens.RefreshTokens.Entities;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailData.Core.Common.Tokens.RefreshTokens.Services;

namespace TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Services;

public class RefreshTokenService : IRefreshtokenService
{
    private readonly IRefreshTokenRepository _repository;
    public RefreshTokenService(IRefreshTokenRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> CreateAsync(RefreshToken entity)
    {
        if(string.IsNullOrEmpty(entity.UserId) || string.IsNullOrWhiteSpace(entity.UserId))
        {
            throw new ArgumentException("cannot create RefreshToken due to userId is empty");
        }

        entity.Token = Guid.NewGuid();
        return await _repository.CreateAsync(entity);
    }

    public async Task<Guid> DeleteByIdAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
        return id;
    }

    public async Task DeleteRangeRefreshTokensAsync(string userId)
    {
        await _repository.DeleteRangeRefreshTokensAsync(userId);
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Entities;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Repositories;
using TasteTrailIdentity.Core.Common.Tokens.RefreshTokens.Services;

namespace TasteTrailIdentity.Infrastructure.Common.RefreshTokens.Services;

public class RefreshTokenService : IRefreshTokenService
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

        entity.CreationDate = DateTime.Now.ToUniversalTime();
        entity.Token = Guid.NewGuid();

        return await _repository.CreateAsync(entity);
    }

    public async Task<Guid> DeleteByIdAsync(Guid id)
    {
        await _repository.DeleteByIdAsync(id);
        return id;
    }

    public async Task<int> DeleteRangeRefreshTokensAsync(string userId)
    {
        if(string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("cannot create RefreshToken due to userId is empty");
        }

        var count = await _repository.DeleteRangeRefreshTokensAsync(userId);
        return count;
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}

namespace TasteTrailIdentity.Core.Users.Managers;

public interface IUserImageManager
{
    public Task<string> GetDefaultImageUrlAsync();
}

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using TasteTrailIdentityManager.Core.Users.Services;
using TasteTrailIdentityManager.Core.Users.Managers;

namespace TasteTrailIdentityManager.Infrastructure.Users.Managers;

public class UserImageManager : IUserImageManager
{
    private readonly IUserService _userService;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _defaultAvatarUrl;
    private readonly string _containerName = "----";

    public UserImageManager(IUserService userService, BlobServiceClient blobServiceClient)
    {
        _userService = userService;
        _blobServiceClient = blobServiceClient;
        _defaultAvatarUrl = GetDefaultImageUrl();
    }

    public async Task<string> DeleteImageAsync(string id)
    {
        var user = await _userService.GetUserByIdAsync(id) ?? throw new ArgumentException($"User with Id {id} not found.");

        if (!string.IsNullOrEmpty(user.AvatarPath) && !user.AvatarPath.Equals(_defaultAvatarUrl, StringComparison.OrdinalIgnoreCase))
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobUri = new Uri(user.AvatarPath).AbsolutePath.TrimStart('/');
            var blobName = Path.GetFileName(blobUri);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        await _userService.PatchAvatarUrlPathAsync(id, _defaultAvatarUrl);

        return user.AvatarPath!;
    }

    public string GetDefaultImageUrl()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var defaultLogoBlobName = "default-logo.png";

        var blobClient = containerClient.GetBlobClient(defaultLogoBlobName);
        
        if (!blobClient.Exists())
            throw new InvalidOperationException("Default avatar does not exist in Blob Storage.");

        return blobClient.Uri.ToString();
    }

    public async Task<string> SetImageAsync(string id, IFormFile? avatar)
    {
        var user = await _userService.GetUserByIdAsync(id) ?? throw new ArgumentException($"User with Id {id} not found.");

        if (avatar == null || avatar.Length == 0)
        {
            await _userService.PatchAvatarUrlPathAsync(id, _defaultAvatarUrl);
            return _defaultAvatarUrl;
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobName = $"{user.Id}{Path.GetExtension(avatar.FileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        using (var stream = avatar.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = avatar.ContentType });
        }

        var avatarUrl = blobClient.Uri.ToString();
        await _userService.PatchAvatarUrlPathAsync(id, _defaultAvatarUrl);

        return avatarUrl;
    }

}

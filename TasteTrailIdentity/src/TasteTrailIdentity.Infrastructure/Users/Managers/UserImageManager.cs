#pragma warning disable CS1998

using Azure.Storage.Blobs;
using TasteTrailIdentity.Core.Users.Managers;

namespace TasteTrailIdentity.Infrastructure.Users.Managers;


public class UserImageManager : IUserImageManager
{
    private readonly string _containerName;

    protected readonly BlobServiceClient _blobServiceClient;

    public UserImageManager(BlobServiceClient blobServiceClient) 
    {
        _containerName = "user-avatars";
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> GetDefaultImageUrlAsync()
    {
        BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        string blobName = "default-image.png";
        BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
        if (!blobClient.Exists())
        {
            throw new InvalidOperationException("Default image does not exist in Blob Storage.");
        }

        return blobClient.Uri.ToString();
    }
}

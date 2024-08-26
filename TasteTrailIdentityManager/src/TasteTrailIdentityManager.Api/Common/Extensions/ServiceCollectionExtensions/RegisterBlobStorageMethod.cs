using Azure.Storage.Blobs;

namespace TasteTrailIdentityManager.Api.Common.Extensions.ServiceCollectionExtensions;

public static class RegisterBlobStorageMethod
{
    public static void RegisterBlobStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton(sp =>
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            return new BlobServiceClient(connectionString);
        });
    }
}

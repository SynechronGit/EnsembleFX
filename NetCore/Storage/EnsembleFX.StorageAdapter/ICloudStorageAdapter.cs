using Microsoft.WindowsAzure.Storage.Blob;

namespace EnsembleFX.StorageAdapter
{
    public interface ICloudStorageAdapter : IStorageAdapter
    {
        CloudBlobContainer BlobContainer { get; }
    }
}

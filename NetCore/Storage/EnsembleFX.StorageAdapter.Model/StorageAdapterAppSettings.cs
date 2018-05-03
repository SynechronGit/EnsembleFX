using System;

namespace EnsembleFX.StorageAdapter.Model
{
    public class StorageAdapterAppSettings
    {
        public string AzureStorageAccount { get; set; }
        public string CloudContainer { get; set; }
        public string SharedAccessTokenExpiryTimeInMinutes { get; set; }
        public string StoragePath { get; set; }
    }
}

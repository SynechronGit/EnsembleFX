using System;

namespace EnsembleFX.StorageAdapter.Model
{
    public class AppSettings
    {
        public string AzureStorageAccount { get; set; }
        public string CloudContainer { get; set; }

        public string SharedAccessTokenExpiryTimeInMinutes { get; set; }

        public string StoragePath { get; set; }
    }
}

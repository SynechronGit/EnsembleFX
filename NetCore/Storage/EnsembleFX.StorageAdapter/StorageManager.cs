
using EnsembleFX.StorageAdapter.Model;
using Microsoft.Extensions.Options;

namespace EnsembleFX.StorageAdapter
{
  
    public class StorageManager
    {
        private IStorageAdapter _storageAdapter;

        public StorageManager(string storageType, IOptions<StorageAdapterAppSetting> appSettings)
        {
            if (!string.IsNullOrEmpty(storageType))
            {
                if (storageType == StorageType.AzureStorage.ToString())
                {
                    _storageAdapter = new AzureStorageAdapter(appSettings);
                }
                else if (storageType == StorageType.LocalFileStorage.ToString())
                {
                    _storageAdapter = new LocalFileStorageAdapter(appSettings);
                }
            }
        }

        public IStorageAdapter Instance
        {
            get
            {
                return _storageAdapter;
            }
        }
    }

   
}

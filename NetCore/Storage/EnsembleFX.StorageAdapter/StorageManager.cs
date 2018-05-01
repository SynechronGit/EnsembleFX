
namespace EnsembleFX.StorageAdapter
{
    public class StorageManager
    {
        private IStorageAdapter _storageAdapter;

        public StorageManager(string storageType)
        {
            if (!string.IsNullOrEmpty(storageType))
            {
                if (storageType == StorageType.AzureStorage.ToString())
                {
                    _storageAdapter = new AzureStorageAdapter();
                }
                else if (storageType == StorageType.LocalFileStorage.ToString())
                {
                    _storageAdapter = new LocalFileStorageAdapter();
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

    public enum StorageType
    {
        AzureStorage,
        LocalFileStorage
    }
}

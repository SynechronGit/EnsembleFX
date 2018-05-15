using EnsembleFX.StorageAdapter.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter
{
    public class AzureStorageAdapter : ICloudStorageAdapter
    {

        #region Internal Members

        internal string CloudConnection;
        internal string CloudContainer;

        #endregion

        #region Private Members
        private readonly IConfiguration configuration;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudBlockBlob blockBlob;
        private string sasContainerToken;
        private double sharedAccessTokenExpiryTimeInMinutes;

        #endregion
        public CloudBlobContainer BlobContainer { get { return blobContainer; } }

        #region Constructors

        public AzureStorageAdapter(IOptions<StorageAdapterAppSettings> appSettings)
        {
            //TODO:: Need to configure the AppSettings in StartUp.cs file
            CloudConnection = appSettings.Value.AzureStorageAccount;
            CloudContainer = appSettings.Value.CloudContainer;
            sharedAccessTokenExpiryTimeInMinutes = string.IsNullOrEmpty(appSettings.Value.SharedAccessTokenExpiryTimeInMinutes) ? 2 : double.Parse(appSettings.Value.SharedAccessTokenExpiryTimeInMinutes);
            Initialize();
        }

        public AzureStorageAdapter(IConfiguration configuration)
        {
            this.configuration = configuration;
            CloudConnection = this.GetConfiguration("AzureStorageAccount");
            CloudContainer = this.GetConfiguration("CloudContainer");
            sharedAccessTokenExpiryTimeInMinutes = string.IsNullOrEmpty(this.GetConfiguration("SharedAccessTokenExpiryTimeInMinutes")) ? 2 : double.Parse(this.GetConfiguration("SharedAccessTokenExpiryTimeInMinutes"));
            Initialize();
        }

        public AzureStorageAdapter(string cloudConnectionString, string containerName)
        {
            CloudConnection = cloudConnectionString;
            CloudContainer = containerName;
            Initialize();
        }

        #endregion

        #region IStorageAdapter implementation
        public string GetConfiguration(string key)
        {
            return configuration[key];
        }

        public void Initialize()
        {
            if (!String.IsNullOrEmpty(CloudConnection))
            {
                storageAccount = CloudStorageAccount.Parse(CloudConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference(CloudContainer);
                blobContainer.CreateIfNotExistsAsync().Wait();
            }
            else
            {
                throw new InvalidOperationException("The Cloud Connection string is invalid. Cannot initialize the provider for Azure Cloud operations");
            }
        }

        public async Task<string> GetContentTextAsync(string key)
        {
            string returnText = string.Empty;
            try
            {
                MemoryStream blobStream = new MemoryStream();
                blockBlob = blobContainer.GetBlockBlobReference(key);
                await blockBlob.DownloadToStreamAsync(blobStream);
                returnText = Encoding.UTF8.GetString(blobStream.ToArray());
            }
            catch (Exception)
            {
                //TODO: Logs an error
                return "";
            }
            //return the encoded content of the blob
            return returnText;
        }

        public bool UploadContent(string key, string fileName, bool deleteAfter)
        {
            //check if the key and filename are not blanks else return false
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            blockBlob = blobContainer.GetBlockBlobReference(key);
            using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                try
                {
                    //upload the file as blob 
                    blockBlob.UploadFromStreamAsync(fs).Wait();
                }
                catch (Exception)
                {
                    //TODO : Logs an exception
                    return false;
                }

            }
            if (deleteAfter)
            {
                //delete the file from the source
                File.Delete(fileName);
            }

            return true;
        }

        public bool UploadContent(string key, Stream contentStream)
        {
            try
            {
                if (contentStream == null)
                {
                    return false;
                }
                contentStream.Seek(0, SeekOrigin.Begin);

                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.UploadFromStreamAsync(contentStream).Wait();
            }
            catch (Exception ex)
            {
                //TODO: logs exceptions
                return false;

            }
            return true;
        }


        public MemoryStream GetContent(string key)
        {
            MemoryStream blobStream = new MemoryStream();
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DownloadToStreamAsync(blobStream).Wait();
            }
            catch (Exception exp)
            {
                //Logg the error
            }
            return blobStream;
        }

        public bool DeleteContent(string key)
        {
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DeleteAsync().Wait();
                return true;
            }
            catch (Exception)
            {
                //TODO: Logs an error
                return false;
            }
        }

        public async Task<MemoryStream> GetContentAsync(string key)
        {
            MemoryStream blobStream = new MemoryStream();
            try
            {
                //get the blobcontainer reference and download the content of the blob
                blockBlob = blobContainer.GetBlockBlobReference(key);
                await blockBlob.DownloadToStreamAsync(blobStream);
                return blobStream;
            }
            catch (Exception ex)
            {
                //TODO: Logs an error
                return new MemoryStream();
            }
        }

        public string GetContentText(string key)
        {
            string returnText = string.Empty;
            try
            {
                MemoryStream blobStream = new MemoryStream();
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DownloadToStreamAsync(blobStream).Wait();
                returnText = System.Text.Encoding.UTF8.GetString(blobStream.ToArray());
            }
            catch (Exception exp)
            {
                //Logg the error
            }
            return returnText;
        }

        public string GetSASContainerToken()
        {
            //string sasContainerToken = null;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConnection);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(CloudContainer);

                //Set the expiry time and permissions for the container.
                //In this case no start time is specified, so the shared access signature becomes valid immediately.
                SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
                sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(sharedAccessTokenExpiryTimeInMinutes);
                sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

                //Generate the shared access signature on the container, setting the constraints directly on the signature.
                sasContainerToken = container.GetSharedAccessSignature(sasConstraints);


            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate SAS Token", ex);
            }
            //Return the URI string for the container, including the SAS token.
            return sasContainerToken;
        }
        #endregion
    }
}

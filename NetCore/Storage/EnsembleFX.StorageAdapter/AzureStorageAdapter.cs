using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsembleFX.StorageAdapter.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace EnsembleFX.StorageAdapter
{
    public class AzureStorageAdapter : ICloudStorageAdapter
    {

        #region Internal Members

        internal string CloudConnection;
        internal string CloudContainer;

        #endregion

        #region Private Members

        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudBlockBlob blockBlob;
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

        public AzureStorageAdapter(string cloudConnectionString, string containerName)
        {
            CloudConnection = cloudConnectionString;
            CloudContainer = containerName;
            Initialize();
        }

        #endregion

        #region IStorageAdapter implementation

        public void Initialize()
        {
            if (!String.IsNullOrEmpty(CloudConnection))
            {
                storageAccount = CloudStorageAccount.Parse(CloudConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference(CloudContainer);
                blobContainer.CreateIfNotExistsAsync();
            }
            else
            {
                throw new InvalidOperationException("The Cloud Connection string is invalid. Cannot initialize the provider for Azure Cloud operations");
            }
        }

        public void UploadContent(string key, string fileName, bool deleteAfter)
        {
            blockBlob = blobContainer.GetBlockBlobReference(key);
            using (var fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                blockBlob.UploadFromStreamAsync(fs);
            }
            if (deleteAfter)
            {
                File.Delete(fileName);
            }
        }

        public void UploadContent(string key, Stream contentStream)
        {
            if (contentStream == null)
                return;

            contentStream.Seek(0, SeekOrigin.Begin);

            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.UploadFromStreamAsync(contentStream);
        }

        public MemoryStream GetContent(string key)
        {
            MemoryStream blobStream = new MemoryStream();
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DownloadToStreamAsync(blobStream);
            }
            catch (Exception exp)
            {
                //Logg the error
            }
            return blobStream;
        }

        public void DeleteContent(string key)
        {
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DeleteIfExistsAsync();
            }
            catch (Exception exp)
            {
                //Logg the error
            }
        }

        public string GetContentText(string key)
        {
            string returnText = string.Empty;
            try
            {
                MemoryStream blobStream = new MemoryStream();
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DownloadToStreamAsync(blobStream);
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

            string sasContainerToken = null;
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

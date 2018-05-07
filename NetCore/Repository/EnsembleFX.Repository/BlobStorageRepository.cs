using EnsembleFX.Repository.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnsembleFX.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        internal string cloudConnection;
        internal string cloudContainer;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudBlockBlob blockBlob;

        public BlobStorageRepository(AzureConnectionString azureConnectionString)
        {
            cloudConnection = azureConnectionString.ConnectionString;
            cloudContainer = azureConnectionString.CloudContainer;
            InitializeCloudClient();
        }

        public BlobStorageRepository(string cloudConnectionString, string containerName)
        {
            cloudConnection = cloudConnectionString;
            cloudContainer = containerName;

            InitializeCloudClient();

        }

        private void InitializeCloudClient()
        {
            if (!String.IsNullOrEmpty(cloudConnection))
            {
                storageAccount = CloudStorageAccount.Parse(cloudConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference(cloudContainer);
                blobContainer.CreateIfNotExistsAsync().Wait();
                blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();
            }
            else
            {
                throw new InvalidOperationException("The cloud client was not initialized");
            }
        }

        public void UploadBlob(string key, string fileName, bool deleteAfter)
        {
            blockBlob = blobContainer.GetBlockBlobReference(key);
            using (var fs = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                blockBlob.Properties.ContentType = "text/plain";
                blockBlob.UploadFromStreamAsync(fs);
            }
            if (deleteAfter)
            {
                File.Delete(fileName);
            }
        }
        public void UploadBlob(string key, Stream contentStream)
        {
            if (contentStream == null)
                return;

            contentStream.Seek(0, SeekOrigin.Begin);

            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.UploadFromStreamAsync(contentStream);
        }

        public string GetTextBlob(string key)
        {
            string returnText = string.Empty;

            MemoryStream blobStream = new MemoryStream();
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.DownloadToStreamAsync(blobStream);
            returnText = System.Text.Encoding.UTF8.GetString(blobStream.ToArray());

            return returnText;
        }

        public string GetBlobURL(string key)
        {
            string blobUrl = string.Empty;
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blobUrl = blockBlob.Uri.ToString();
            return blobUrl;
        }
    }
}

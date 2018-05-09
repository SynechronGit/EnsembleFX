using EnsembleFX.Repository.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

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

        public BlobStorageRepository(IOptions<AzureConnectionString> azureConnectionString)
        {
            cloudConnection = azureConnectionString.Value.ConnectionString;
            cloudContainer = azureConnectionString.Value.CloudContainer;
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
                blockBlob.UploadFromStreamAsync(fs).Wait();
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
            blockBlob.UploadFromStreamAsync(contentStream).Wait();
        }

        public string GetTextBlob(string key)
        {
            string returnText = string.Empty;
            MemoryStream blobStream = new MemoryStream();
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.DownloadToStreamAsync(blobStream).Wait();
            returnText = System.Text.Encoding.UTF8.GetString(blobStream.ToArray());
            return returnText;
        }

        public string UploadFileToBlob(string key, Stream contentStream, string contentType)
        {
            string ImageUrl = string.Empty;
            if (contentStream == null)
                return string.Empty;

            contentStream.Seek(0, SeekOrigin.Begin);

            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.UploadFromStreamAsync(contentStream).Wait();

            blockBlob.FetchAttributesAsync().Wait();
            if (!string.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }
            else
            {
                blockBlob.Properties.ContentType = "image/tiff";
            }
            blockBlob.SetPropertiesAsync().Wait();
            var uriBuilder = new UriBuilder(blockBlob.Uri);
            uriBuilder.Scheme = "https";
            ImageUrl = uriBuilder.ToString();

            return ImageUrl;
        }

        public string GetBlobURL(string key)
        {
            string blobUrl = string.Empty;
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blobUrl = blockBlob.Uri.ToString();
            return blobUrl;
        }

        public MemoryStream GetBlob(string key)
        {
            MemoryStream blobStream = new MemoryStream();
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.DownloadToStreamAsync(blobStream).Wait();

            return blobStream;
        }

        public void DeleteBlog(string key)
        {
            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.DeleteAsync().Wait();
        }

        public CloudBlockBlob GetBlockBlobReference(string blobName)
        {
            return blobContainer.GetBlockBlobReference(blobName);
        }
    }
}

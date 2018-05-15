using EnsembleFX.StorageAdapter.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Blob
{
    public class BlobStorageAdapter : IBlobStorageAdapter
    {
        #region Internal Members
        internal string CloudConnection;
        internal string CloudContainer;
        #endregion

        #region Private Members

        private readonly IConfiguration _configuration;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudBlockBlob blockBlob;
        private string sasContainerToken;
        private double sharedAccessTokenExpiryTimeInMinutes = 0;

        #endregion

        #region Public Members
        public CloudBlobContainer BlobContainer { get { return blobContainer; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public BlobStorageAdapter(IConfiguration configuration)
        {
            _configuration = configuration;
            CloudConnection = this.GetConfiguration("BlobStorageAccount:CloudConnectionString");
            CloudContainer = this.GetConfiguration("BlobStorageAccount:CloudContainer");
            sharedAccessTokenExpiryTimeInMinutes = string.IsNullOrEmpty(this.GetConfiguration("BlobStorageAccount:SharedAccessTokenExpiryTimeInMinutes")) ? 2 : double.Parse(this.GetConfiguration("BlobStorageAccount:SharedAccessTokenExpiryTimeInMinutes"));
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cloudConnectionString"></param>
        /// <param name="cloudContainerName"></param>
        public BlobStorageAdapter(string cloudConnectionString, string cloudContainerName)
        {
            CloudConnection = cloudConnectionString;
            CloudContainer = cloudContainerName;

            Initialize();
        }

        /// <summary>
        /// Get configuration
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfiguration(string key)
        {
            return _configuration[key];
        }

        /// <summary>
        /// Get content and return stored text
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete content
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteContent(string key)
        {
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                //TODO: Logs an error

                return false;
            }
        }

        /// <summary>
        /// Get Content
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get access token
        /// </summary>
        /// <returns></returns>
        public string GetSASContainerToken()
        {
            try
            {
                //parse the cloudconnnection to create blob client
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
                //TODO : logs an exception
                throw new Exception("Failed to generate SAS Token", ex);
            }
            //Return the URI string for the container, including the SAS token.
            return sasContainerToken;
        }

        /// <summary>
        /// Initialize blob container, called implicitly in ctor
        /// </summary>
        public void Initialize()
        {
            if (!String.IsNullOrEmpty(CloudConnection))
            {
                //parse the cloudconnection to create the cloudblobclient
                storageAccount = CloudStorageAccount.Parse(CloudConnection);
                blobClient = storageAccount.CreateCloudBlobClient();
                //Create the container 
                blobContainer = blobClient.GetContainerReference(CloudContainer);
                blobContainer.CreateIfNotExistsAsync();
            }
            else
            {
                throw new InvalidOperationException("The Cloud Connection string is invalid. Cannot initialize the provider for Azure Cloud operations");
            }
        }

        /// <summary>
        /// Upload content from local file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <param name="deleteAfter">Delete file on local fs</param>
        /// <returns></returns>
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
                    blockBlob.UploadFromStreamAsync(fs);
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

        /// <summary>
        /// Upload content from stream
        /// </summary>
        /// <param name="key"></param>
        /// <param name="contentStream"></param>
        /// <returns></returns>
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
                blockBlob.UploadFromStreamAsync(contentStream);

            }
            catch (Exception ex)
            {
                //TODO: logs exceptions
                return false;

            }

            return true;
        }

        #endregion
    }
}

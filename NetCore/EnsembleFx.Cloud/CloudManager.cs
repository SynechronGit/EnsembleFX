
using EnsembleFX.Repository.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnsembleFX.Cloud
{
    [Obsolete]
    public class CloudManager
    {
        internal string CloudConnection;
        internal string CloudContainer;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        private CloudBlockBlob blockBlob;
        private string MediaAccountName;
        private string MediaAccountKey;

        public CloudManager(IOptions<AzureConnectionString> connectionString)
        {
            CloudConnection = connectionString.Value.ConnectionString; 
            CloudContainer = connectionString.Value.CloudContainer;
            //MediaAccountName = System.Configuration.ConfigurationManager.AppSettings["MediaAccountName"];
            //MediaAccountKey = System.Configuration.ConfigurationManager.AppSettings["MediaAccountKey"];

            InitializeCloudClient();
        }

        public CloudManager(string cloudConnectionString, string containerName)
        {
            CloudConnection = cloudConnectionString;
            CloudContainer = containerName;

            InitializeCloudClient();

        }

        private void InitializeCloudClient()
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
                throw new InvalidOperationException("The cloud client was not initialized");
            }
        }

        public void UploadBlob(string key, string fileName, bool deleteAfter)
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

        public void UploadBlob(string key, Stream contentStream)
        {
            if (contentStream == null)
                return;

            contentStream.Seek(0, SeekOrigin.Begin);

            blockBlob = blobContainer.GetBlockBlobReference(key);
            blockBlob.UploadFromStreamAsync(contentStream);
        }

        public async Task<string> UploadFileToBlob(string key, Stream contentStream, string contentType)
        {
            string ImageUrl = string.Empty;
            if (contentStream == null)
                return string.Empty;

            try
            {
                contentStream.Seek(0, SeekOrigin.Begin);

                blockBlob = blobContainer.GetBlockBlobReference(key);
                await blockBlob.UploadFromStreamAsync(contentStream);

                await blockBlob.FetchAttributesAsync();
                if (!string.IsNullOrEmpty(contentType))
                {
                    blockBlob.Properties.ContentType = contentType;
                }
                else
                {
                    blockBlob.Properties.ContentType = "image/tiff";
                }
                await blockBlob.SetPropertiesAsync();
                var uriBuilder = new UriBuilder(blockBlob.Uri);
                uriBuilder.Scheme = "https";
                ImageUrl = uriBuilder.ToString();
            }
            catch (Exception exp)
            {
                //Logg the error
            }

            return ImageUrl;
        }

        public async Task<MemoryStream> GetBlob(string key)
        {
            MemoryStream blobStream = new MemoryStream();
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                await blockBlob.DownloadToStreamAsync(blobStream);
            }
            catch (Exception exp)
            {
                //Logg the error
            }
            return blobStream;
        }


        public async Task<string> GetBlobURL(string key)
        {
            //Modified for .net core

            string blobUrl = string.Empty;
            BlobContinuationToken continuationToken = null;

            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blobUrl = blockBlob.Uri.ToString();

                blobContainer = blobClient.GetContainerReference(CloudContainer);
                var blobResultSegment = await blobContainer.ListBlobsSegmentedAsync(continuationToken);
                blobUrl = blobResultSegment.Results.Where(b => b.Uri.ToString().Contains(key)).Select(o => o.Uri.ToString()).LastOrDefault();
            }
            catch (Exception exp)
            {
                //Logg the error
            }
            return blobUrl;
        }

        public async Task<bool> GetBlobCount(string key)
        {
            //Modified for .net core
            BlobContinuationToken continuationToken = null;

            string blobUrl = string.Empty;
            string deleteblob = string.Empty;
            blobContainer = blobClient.GetContainerReference(CloudContainer);
            var blobSegment =await blobContainer.ListBlobsSegmentedAsync(continuationToken);
            var blobcount = blobSegment.Results.Where(b => b.Uri.ToString().Contains(key)).ToList();
            if (blobcount.Count >= 3)
            {
                blobUrl = blobcount.Where(b => b.Uri.ToString().Contains(key)).Select(o => o.Uri.ToString()).FirstOrDefault();

                deleteblob = blobUrl.Substring(blobUrl.LastIndexOf('/')).Replace("/", "").ToString();

                DeleteBlog(deleteblob);
            }

            return true;
        }


        public MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }

        public void DeleteBlog(string key)
        {
            try
            {
                blockBlob = blobContainer.GetBlockBlobReference(key);
                blockBlob.DeleteAsync();
            }
            catch (Exception exp)
            {
                //Logg the error
            }
        }

        public string GetTextBlob(string key)
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

        public string GetStreamingUrl(string assetId)
        {
            //CloudMediaContext mediaContext = new CloudMediaContext(MediaAccountName, MediaAccountKey);

            //TODO Need to find how to use mediaContext in .net core
            AzureAdTokenCredentials  tokenCredentials1 = null;//Need to see how to set this
            ITokenProvider provider = new AzureAdTokenProvider(tokenCredentials1);
            ;
            CloudMediaContext mediaContext = new CloudMediaContext(new Uri(MediaAccountName), provider);


            var streamingAsset = mediaContext.Assets.Where(a => a.Id == assetId).FirstOrDefault();

            IAccessPolicy accessPolicy = null;
            accessPolicy = mediaContext.AccessPolicies.Where(ce => ce.Name == streamingAsset.Name).FirstOrDefault();

            if (accessPolicy == null)
            {
                accessPolicy = mediaContext.AccessPolicies.Create(streamingAsset.Name, TimeSpan.FromDays(365), AccessPermissions.Read | AccessPermissions.List);
            }

            string streamingUrl = string.Empty;
            var assetFiles = streamingAsset.AssetFiles.ToList();
            var streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith("m3u8-aapl.ism")).FirstOrDefault();
            if (streamingAssetFile != null)
            {
                var locator = mediaContext.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy, DateTime.UtcNow.AddMinutes(-5));
                Uri hlsUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest(format=m3u8-aapl)");
                streamingUrl = hlsUri.ToString();
            }
            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".ism")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = mediaContext.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy, DateTime.UtcNow.AddMinutes(-5));
                Uri smoothUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest");
                streamingUrl = smoothUri.ToString();
            }
            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".mp4")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = mediaContext.Locators.CreateLocator(LocatorType.Sas, streamingAsset, accessPolicy, DateTime.UtcNow.AddMinutes(-5));
                var mp4Uri = new UriBuilder(locator.Path);
                mp4Uri.Path += "/" + streamingAssetFile.Name;
                streamingUrl = mp4Uri.ToString();
            }
            return streamingUrl;
        }
    }
}

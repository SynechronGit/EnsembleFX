using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnsembleFX.Repository
{
    public interface IBlobStorageRepository
    {
        void UploadBlob(string key, string fileName, bool deleteAfter);
        void UploadBlob(string key, Stream contentStream);
        string GetTextBlob(string key);
        string GetBlobURL(string key);
        CloudBlockBlob GetBlockBlobReference(string blobName);
    }
}

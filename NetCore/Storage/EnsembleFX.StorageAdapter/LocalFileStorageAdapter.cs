using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter
{
    public class LocalFileStorageAdapter : IStorageAdapter
    {

        internal string StoragePath = string.Empty;

        public LocalFileStorageAdapter(IOptions<AppSettings> appSettings)
        {
            StoragePath = appSettings.Value.StoragePath;
            Initialize();
        }

        public void Initialize()
        {
            if (String.IsNullOrEmpty(StoragePath))
            {
                throw new InvalidOperationException("The Storage Path was not set or initialized. Please check your web.config or app.config");
            }
        }

        public void UploadContent(string key, string fileName, bool deleteAfter)
        {
            throw new NotImplementedException();
        }

        public void UploadContent(string key, Stream contentStream)
        {
            if (contentStream == null)
                return;

            string fullFileName = Path.Combine(StoragePath, key);
            MemoryStream fileContentMemoryStream = new MemoryStream();
            contentStream.Seek(0, SeekOrigin.Begin);
            contentStream.CopyTo(fileContentMemoryStream);

            using (FileStream fileStream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                fileContentMemoryStream.WriteTo(fileStream);
            }
        }

        public MemoryStream GetContent(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("The key is not specified or is not a valid file on disk");
            }
            string fullFileName = Path.Combine(StoragePath, key);

            MemoryStream decodedContentStream = new MemoryStream();
            using (FileStream fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
            {
                byte[] contentByteArray = new byte[fileStream.Length];
                fileStream.Read(contentByteArray, 0, (int)fileStream.Length);
                decodedContentStream.Write(contentByteArray, 0, (int)fileStream.Length);
            }
            return decodedContentStream;
        }

        public void DeleteContent(string key)
        {
            // Delete file with key if it exists in the specified path
            string fullFileName = Path.Combine(StoragePath, key);
            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }
            else
            {
                throw new InvalidOperationException("The specified key is invalid or the file with that key does not exist on disk");
            }
        }

        public string GetContentText(string key)
        {
            String contentStreamText = String.Empty;
            MemoryStream contentStream = GetContent(key);
            if (contentStream != null)
            {
                contentStreamText = System.Text.Encoding.UTF8.GetString(contentStream.ToArray());
            }
            return contentStreamText;
        }

        public string GetSASContainerToken()
        {
            throw new NotImplementedException();
        }

    }
}

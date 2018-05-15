using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter
{
    public interface IStorageAdapter
    {
        void Initialize();       
        MemoryStream GetContent(string key);        
        string GetContentText(string key);        

        /// <summary>
        /// Gets a configuration for document storage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConfiguration(string key);


        /// <summary>
        /// Upload content to azure blob storage for a given blob and delete the file from the source if required
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="fileName">filename</param>
        /// <param name="deleteAfter">deleteAfter</param>
        bool UploadContent(string key, string fileName, bool deleteAfter);
        /// <summary>
        /// Upload the content to the azure blob storage for a given blob
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="contentStream">content stream</param>
        bool UploadContent(string key, Stream contentStream);
        /// <summary>
        /// Get the content from the azure blob storage for a given blob
        /// </summary>
        /// <param name="key">key to blob reference</param>
        /// <returns>memory stream</returns>
        Task<MemoryStream> GetContentAsync(string key);
        /// <summary>
        /// Delete the blob from the azure blob storage for a given blob
        /// </summary>
        /// <param name="key">key to blob reference</param>
        bool DeleteContent(string key);
        /// <summary>
        /// Get encoded content from the azure blob storage for a given blob
        /// </summary>
        /// <param name="key">key to blob reference</param>
        /// <returns>encoded text</returns>
        Task<string> GetContentTextAsync(string key);
        /// <summary>
        /// Gets the SAS container token for the azure storage account
        /// </summary>
        /// <returns>token to reference</returns>
        string GetSASContainerToken();
    }
}

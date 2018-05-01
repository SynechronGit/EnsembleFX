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
        void UploadContent(string key, string fileName, bool deleteAfter);
        void UploadContent(string key, Stream contentStream);
        MemoryStream GetContent(string key);
        void DeleteContent(string key);
        string GetContentText(string key);
        string GetSASContainerToken();
    }
}

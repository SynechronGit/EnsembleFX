using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.DataFrame.Formatters
{
    public class StreamPipeline
    {
        public bool IsStreamEncrypted { get; set; }
        public bool IsStreamCompressed { get; set; }

        public string EncryptionKey { get; set; }

        public Stream EncryptAndCompress(Stream stream)
        {
            return null;
        }
        public Stream CompressAndEncrypt(Stream stream)
        {
            return null;
        }
        public Stream DecryptAndDecompress(Stream stream)
        {
            return null;
        }
        public Stream DecompressAndDecrypt(Stream stream)
        {
            return null;
        }

    }
}

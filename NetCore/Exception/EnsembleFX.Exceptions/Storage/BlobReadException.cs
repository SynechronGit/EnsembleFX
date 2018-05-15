
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class BlobReadException : StorageException
    {
        const string BlobReadMessage = "Blob(s) could not be added";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobReadException"/> class.
        /// </summary>
        public BlobReadException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobReadException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BlobReadException(string message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobReadException"/> class.
        /// </summary>
        /// <param name="readMessage">The read message.</param>
        /// <param name="innerException">The exception that is caused by read operation</param>
        public BlobReadException(string readMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    BlobReadMessage, readMessage), innerException)
        {

        }
    }
}

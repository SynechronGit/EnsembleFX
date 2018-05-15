
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class BlobUpdateException : StorageException
    {
        const string BlobUpdateMessage = "Blob(s) could not be updated";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobUpdateException"/> class.
        /// </summary>
        public BlobUpdateException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobUpdateException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BlobUpdateException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobUpdateException"/> class.
        /// </summary>
        /// <param name="updateMessage">The update message.</param>
        /// <param name="innerException">The exception that is caused by update operation</param>
        public BlobUpdateException(string updateMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    BlobUpdateMessage, updateMessage), innerException)
        {

        }
    }
}

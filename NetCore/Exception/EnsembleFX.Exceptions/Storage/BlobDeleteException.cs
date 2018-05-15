
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class BlobDeleteException : StorageException
    {
        const string BlobUpdateMessage = "Blob(s) could not be deleted";

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobDeleteException"/> class.
        /// </summary>
        public BlobDeleteException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobDeleteException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BlobDeleteException(string message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobDeleteException"/> class.
        /// </summary>
        /// <param name="deleteMessage">The delete message.</param>
        /// <param name="innerException">The exception that is caused by delete operation</param>
        public BlobDeleteException(string deleteMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    BlobUpdateMessage, deleteMessage), innerException)
        {
        }
    }
}

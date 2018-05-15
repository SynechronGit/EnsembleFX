
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public abstract class BlobWriteException : StorageException
    {
        private readonly StorageException exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobWriteException"/> class.
        /// </summary>
        public BlobWriteException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobWriteException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BlobWriteException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobWriteException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <exception cref="System.ArgumentNullException">inner</exception>
        public BlobWriteException(string message, StorageException innerException) : base(message, innerException)
        {
            exception = innerException ?? throw new ArgumentNullException(message, nameof(innerException));
        }
    }
}

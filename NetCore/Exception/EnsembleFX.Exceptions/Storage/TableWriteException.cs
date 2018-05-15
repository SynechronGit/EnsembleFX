
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class TableWriteException : StorageException
    {
        private readonly StorageException localException;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableWriteException"/> class.
        /// </summary>
        public TableWriteException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableWriteException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TableWriteException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableWriteException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <exception cref="System.ArgumentNullException">inner</exception>
        public TableWriteException(string message, StorageException innerException) : base(message, innerException)
        {
            localException = innerException ?? throw new ArgumentNullException(message, nameof(innerException));
        }
    }
}


namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class TableReadException : StorageException
    {
        const string TableReadMessage = "Table(s) could not be added";

        /// <summary>
        /// Initializes a new instance of the <see cref="TableReadException"/> class.
        /// </summary>
        public TableReadException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableReadException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TableReadException(string message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableReadException"/> class.
        /// </summary>
        /// <param name="readMessage">The Read message.</param>
        /// <param name="innerException">The exception that is caused by Read operation</param>
        public TableReadException(string readMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    TableReadMessage, readMessage), innerException)
        {

        }
    }
}

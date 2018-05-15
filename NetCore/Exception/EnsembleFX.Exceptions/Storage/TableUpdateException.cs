
namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class TableUpdateException : StorageException
    {
        const string TableUpdateMessage = "Table(s) could not be updated";

        /// <summary>
        /// Initializes a new instance of the <see cref="TableUpdateException"/> class.
        /// </summary>
        public TableUpdateException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableUpdateException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TableUpdateException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableUpdateException"/> class.
        /// </summary>
        /// <param name="updateMessage">The update message.</param>
        /// <param name="innerException">The exception that is caused by update operation</param>
        public TableUpdateException(string updateMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    TableUpdateMessage, updateMessage), innerException)
        {

        }
    }
}


namespace EnsembleFX.Exceptions.Storage
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    public class TableDeleteException : StorageException
    {
        const string TableUpdateMessage = "Table(s) could not be deleted";

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDeleteException"/> class.
        /// </summary>
        public TableDeleteException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDeleteException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TableDeleteException(string message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDeleteException"/> class.
        /// </summary>
        /// <param name="deleteMessage">The delete message.</param>
        /// <param name="innerException">The exception that is caused by delete operation</param>
        public TableDeleteException(string deleteMessage, StorageException innerException) : base(String.Format("{0} - {1}",
                    TableUpdateMessage, deleteMessage), innerException)
        {
        }
    }
}

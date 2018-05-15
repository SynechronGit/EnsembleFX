
namespace EnsembleFX.Exceptions.Storage
{
    using System;
    public class DocumentQueryException : DocumentWriteException
    {
        private const string DOCUMENT_MESSAGE = "Unable to query documents";


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotInsertDocumentException"/> class.
        /// </summary>
        public DocumentQueryException() : base(DOCUMENT_MESSAGE)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotInsertDocumentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DocumentQueryException(string message) : base(String.Format("{0} - {1}",
                DOCUMENT_MESSAGE, message))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotInsertDocumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  
        /// if no inner exception is specified</param>
        public DocumentQueryException(string message, Exception innerException) : base(String.Format("{0} - {1}",
                    DOCUMENT_MESSAGE, message), innerException)
        {

        }
    }
}

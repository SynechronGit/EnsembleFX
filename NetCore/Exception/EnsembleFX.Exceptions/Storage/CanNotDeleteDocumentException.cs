
namespace EnsembleFX.Exceptions.Storage
{
    using System;
    public class CanNotDeleteDocumentException : DocumentWriteException
    {
        private const string DOCUMENT_MESSAGE = "Document(s) could not be deleted";


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotDeleteDocumentException"/> class.
        /// </summary>
        public CanNotDeleteDocumentException() : base(DOCUMENT_MESSAGE)
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotDeleteDocumentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CanNotDeleteDocumentException(string message) : base(String.Format("{0} - {1}",
                DOCUMENT_MESSAGE, message))
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotDeleteDocumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  
        /// if no inner exception is specified</param>
        public CanNotDeleteDocumentException(string message, Exception innerException) : base(String.Format("{0} - {1}",
                    DOCUMENT_MESSAGE, message), innerException)
        {
        }
    }
}

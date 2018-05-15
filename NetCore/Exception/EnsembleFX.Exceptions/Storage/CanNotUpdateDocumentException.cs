
namespace EnsembleFX.Exceptions.Storage
{
    using System;
    public class CanNotUpdateDocumentException : DocumentWriteException
    {
        private const string DOCUMENT_MESSAGE = "Document(s) could not be updated";


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotUpdateDocumentException"/> class.
        /// </summary>
        public CanNotUpdateDocumentException() : base(DOCUMENT_MESSAGE)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotUpdateDocumentException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CanNotUpdateDocumentException(string message) : base(String.Format("{0} - {1}",
                DOCUMENT_MESSAGE, message))
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CanNotUpdateDocumentException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  
        /// if no inner exception is specified</param>
        public CanNotUpdateDocumentException(string message, Exception innerException) : base(String.Format("{0} - {1}",
                    DOCUMENT_MESSAGE, message), innerException)
        {

        }
    }
}

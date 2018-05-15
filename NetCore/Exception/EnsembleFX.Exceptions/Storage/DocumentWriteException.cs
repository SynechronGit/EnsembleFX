
namespace EnsembleFX.Exceptions.Storage
{
    using System;
    public abstract class DocumentWriteException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWriteException"/> class.
        /// </summary>
        public DocumentWriteException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWriteException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DocumentWriteException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentWriteException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference  
        /// if no inner exception is specified</param>
        /// <exception cref="System.ArgumentNullException">inner</exception>
        public DocumentWriteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

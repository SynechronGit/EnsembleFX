using System;
using System.Runtime.Serialization;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// Represents class to handle exceptions that occur on a server side.
    /// </summary>
    public class ServerSideException : CoreException
    {
         #region Constructor
        /// <summary>
        ///  Initializes a new instance of the ServerSideException.
        /// </summary>
        public ServerSideException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the ServerSideException with the specified core fault.
        /// </summary>
        /// <param name="coreFault">The fault exception.</param>
        public ServerSideException(CoreFault coreFault)
        {
            Source = coreFault.Source;
            ErrorId = coreFault.ErrorId;
        }

        /// <summary>
        ///  Initializes a new instance of the <c>CoreVelocity.Core.Exception.CoreException</c> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ServerSideException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>CoreVelocity.Core.Exception.CoreException</c> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ServerSideException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CoreVelocity.Core.Exception.CoreException class with serialized data. Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info"> The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"> The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.ServiceFactory.HResult is zero (0).</exception>
        protected ServerSideException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}

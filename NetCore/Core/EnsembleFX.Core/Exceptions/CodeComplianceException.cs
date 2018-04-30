using System;
using System.Runtime.Serialization;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur when code compliance is not properly followed.
    /// </summary>
    [Serializable]
    public class CodeComplianceException : Exception
    {
        #region Constructor
        /// <summary>
        ///  Initializes a new instance of the <see cref="T:CoreVelocity.Core.Exceptions.CodeComplianceException"/>
        /// </summary>
        public CodeComplianceException()
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="T:CoreVelocity.Core.Exceptions.CodeComplianceException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CodeComplianceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CoreVelocity.Core.Exceptions.CodeComplianceException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CodeComplianceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CoreVelocity.Core.Exceptions.CodeComplianceException"/> class with serialized data. Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext"/>  that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.CodeComplianceException.HResult is zero (0).</exception>
        protected CodeComplianceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}
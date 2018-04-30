using System;
using System.Runtime.Serialization;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// Represents errors that occur when code compliance is not properly followed.
    /// </summary>
    [Serializable]
    public class CoreException : Exception
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets error Id
        /// </summary>
        /// <value>The error Id</value>
        [DataMember]
        public Guid ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the source
        /// </summary>
        /// <value>Method that raised the error</value>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the detailed description
        /// </summary>
        /// <value>Detailed error description</value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the exception was fatal or not
        /// </summary>
        [DataMember]
        public bool IsFatal { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        ///  Initializes a new instance of the <c>CoreVelocity.Core.Exception.CoreException</c>.
        /// </summary>
        public CoreException()
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <c>CoreVelocity.Core.Exception.CoreException</c> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CoreException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>CoreVelocity.Core.Exception.CoreException</c> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the CoreVelocity.Core.Exception.CoreException class with serialized data. Without this constructor, deserialization will fail
        /// </summary>
        /// <param name="info"> The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"> The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or System.ServiceFactory.HResult is zero (0).</exception>
        protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}
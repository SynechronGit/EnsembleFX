
namespace EnsembleFX.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    public class PreConditionFailedException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        public PreConditionFailedException()
            : base() { }


        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        public PreConditionFailedException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="params"The array of objects.</param>
        public PreConditionFailedException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="exception"The inner exception</param>
        public PreConditionFailedException(string message, Exception exception)
            : base(message, exception) { }

        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="params"The array of objects.</param>
        /// <param> name="exception"The inner exception</param>
        public PreConditionFailedException(string message, Exception exception, params object[] args)
            : base(string.Format(message, args), exception) { }

        /// <summary>
        /// Initializes a new instance of the PreConditionFailedException class.
        /// </summary>
        /// <param> name="serializationInfo"The exception message.</param>
        /// <param> name="context"The array of objects.</param>
        public PreConditionFailedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }


    }
}

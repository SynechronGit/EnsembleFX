
namespace EnsembleFX.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    public class ResourceNotModifiedException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        public ResourceNotModifiedException()
            : base() { }


        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        public ResourceNotModifiedException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="params"The array of objects.</param>
        public ResourceNotModifiedException(string message, params object[] args)
            : base(string.Format(message, args)) { }

        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="exception"The inner exception</param>
        public ResourceNotModifiedException(string message, Exception exception)
            : base(message, exception) { }

        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        /// <param> name="message"The exception message.</param>
        /// <param> name="params"The array of objects.</param>
        /// <param> name="exception"The inner exception</param>
        public ResourceNotModifiedException(string message, Exception exception, params object[] args)
            : base(string.Format(message, args), exception) { }

        /// <summary>
        /// Initializes a new instance of the ResourceNotModifiedException class.
        /// </summary>
        /// <param> name="serializationInfo"The exception message.</param>
        /// <param> name="context"The array of objects.</param>
        public ResourceNotModifiedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context) { }


    }
}

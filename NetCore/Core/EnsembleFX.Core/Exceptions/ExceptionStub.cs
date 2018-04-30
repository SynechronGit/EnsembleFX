using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EnsembleFX.Core.Serialization;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// ExceptionStub 
    /// Stand-in for non-serializable Exceptions to be passed back to client as part of Fault Contracts
    /// </summary>
    [Serializable]
    [DataContract]
    public class ExceptionStub
    {

        #region Public Properties
        /// <summary>
        /// Gets or sets exception message.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets exception stack trace.
        /// </summary>
        [DataMember]
        public string Stack { get; set; }

        /// <summary>
        /// Gets or sets source.
        /// </summary>
        /// <value>Method that raised the error</value>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets full type name.
        /// </summary>
        [DataMember]
        public string FullTypeName { get; set; }

        /// <summary>
        /// Gets or sets inner exception.
        /// </summary>
        [DataMember]
        public ExceptionStub InnerException { get; set; }

        /// <summary>
        /// Gets or sets the list of exception stubs.
        /// </summary>
        [DataMember]
        public static List<ExceptionStub> InnerExceptions { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the ExceptionStub object from the Exception passed
        /// </summary>
        /// <param name="exception">Exception object containing the runtime exceptions</param>
        /// <returns>Exceptionstub object</returns>
        public static ExceptionStub CreateExceptionStub(System.Exception exception)
        {
            ExceptionStub stub = new ExceptionStub();
            stub.Message = exception.Message;
            stub.Source = exception.Source;
            stub.FullTypeName = exception.GetType().FullName;
            stub.Stack = exception.StackTrace;
            if (exception.InnerException != null)
            {
                stub.InnerException = CreateExceptionStub(exception.InnerException);
            }

            if (InnerExceptions == null)
            {
                InnerExceptions = new List<ExceptionStub>();

            }
            if (exception is AggregateException)
            {
                AggregateException age = (exception as AggregateException);
                foreach (Exception innerEx in age.InnerExceptions)
                {
                    InnerExceptions.Add(CreateExceptionStub(innerEx));
                }
            }

            return stub;
        }

        /// <summary>
        /// Serializer the exception into XML
        /// </summary>
        /// <param name="exception">Exception object to serialize</param>
        /// <returns>XML serialized representation of the exception object</returns>
        public static string CreateExceptionStubXml(System.Exception exception)
        {
            ExceptionStub stub = CreateExceptionStub(exception);
            ISerializationManager serializer = new XmlSerializationManager();
            return serializer.Serialize<ExceptionStub>(stub);
        }

        #endregion

    }
}

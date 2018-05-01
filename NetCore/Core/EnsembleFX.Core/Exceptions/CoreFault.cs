using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace EnsembleFX.Core.Exceptions
{
    /// <summary>
    /// Represents class to support fault exception, used in a client application to catch contractually-specified SOAP faults.
    /// </summary>
    [DataContract]
    public abstract class CoreFault
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreFault" /> class.
        /// </summary>
        public CoreFault()
        {
            ErrorId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreFault" /> class.
        /// </summary>
        /// <param name="coreException">The core exception.</param>
        public CoreFault(CoreException coreException)
        {
            ErrorId = coreException.ErrorId;
            Source = coreException.Source;
            Description = coreException.Description;
            IsFatal = coreException.IsFatal;
            //q/Exception = coreException.InnerException;
        }

        #endregion
        /// <summary>
        /// Creates fault exception using supplied parameters.
        /// </summary>
        /// <typeparam name="T">The type of fault.</typeparam>
        /// <param name="ex">The exception.</param>
        /// <param name="source">The error source.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="isFatal">The value indicating whether the error is fatal.</param>
        /// <returns>The type of fault.</returns>
        public static T CreateFaultFromException<T>(Exception ex, string source, string message, bool isFatal) where T : CoreFault, new()
        {
            T fault = new T();
            fault.Source = source;

            if (string.IsNullOrEmpty(message))
                fault.Description = ex.Message;
            else
                fault.Description = message;

            fault.ErrorCode = ex.GetHashCode();

            fault.IsFatal = isFatal;

            fault.Reason = new FaultReason(fault.Description);

            fault.Exception = ExceptionStub.CreateExceptionStub(ex);
            return fault;
        }

        /// <summary>
        /// Creates fault exception using supplied parameters.
        /// </summary>
        /// <typeparam name="T">The type of fault.</typeparam>
        /// <param name="ex">The exception.</param>
        /// <param name="source">The error source.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="isFatal">The value indicating whether the error is fatal.</param>
        /// <returns>The fault exception of the type of fault. </returns>
        public static FaultException<T> CreateFaultException<T>(Exception ex, string source, string message, bool isFatal) where T : CoreFault, new()
        {
            T fault = CreateFaultFromException<T>(ex, source, message, isFatal);
            return new FaultException<T>(fault, fault.Reason, null, string.Empty );// Fault code null and action string.empty passed while converting it to .NET core.
        }


        #region Public Properties

        /// <summary>
        /// Gets or sets FaultReason that provides a text description of a SOAP fault.
        /// </summary>
        public FaultReason Reason { get; set; }

        /// <summary>
        /// Gets or sets error Id
        /// </summary>
        /// <value>The error Id</value>
        [DataMember]
        public Guid ErrorId { get; private set; }

        /// <summary>
        /// Gets or sets the error code
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the Error source
        /// </summary>
        /// <value>Method that raised the error</value>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the detailed error description
        /// </summary>
        /// <value>Detailed error description</value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether or not this CoreFaul is fatal.
        /// </summary>
        /// <value>The is fatal.</value>
        [DataMember]
        public bool IsFatal { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        [DataMember]
        public ExceptionStub Exception { get; set; }

        #endregion
              
    }
}

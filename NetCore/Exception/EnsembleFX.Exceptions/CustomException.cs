using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace EnsembleFX.Exceptions
{
    /// <summary>
    /// CustomException is the root of a wide but shallow tree of exceptions that the application will throw. 
    /// All Exceptions must inherit from CustomException
    /// </summary>
    [Serializable]
    public class CustomException : System.Exception
    {
        private bool isErrorLogged = true;

        private string userFriendlyMessage = string.Empty;

        /// <summary>
        /// Gets or sets the user friendly message.
        /// </summary>
        /// <value>The user friendly message.</value>
        public string UserFriendlyMessage
        {
            get { return userFriendlyMessage; }
            set { userFriendlyMessage = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        public CustomException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxE2Exception"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CustomException(string message) : base(message){}

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxE2Exception"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public CustomException(string message, System.Exception inner): base(message, inner){}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is error logged.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is error logged; otherwise, <c>false</c>.
        /// </value>
        public bool IsErrorLogged
        {
            get { return isErrorLogged; }
            set { isErrorLogged = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxE2Exception"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }


        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}

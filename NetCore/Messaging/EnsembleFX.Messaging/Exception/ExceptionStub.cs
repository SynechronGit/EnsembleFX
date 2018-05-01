using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Exception
{
    [Serializable]
    public class ExceptionStub
    {
        #region Public Properties
        public string Message { get; set; }
        public string Stack { get; set; }
        public string Source { get; set; }
        public ExceptionStub InnerException { get; set; }
        #endregion

        #region Public Methods
        public static ExceptionStub CreateExceptionStub(System.Exception exception)
        {
            ExceptionStub stub = new ExceptionStub();
            stub.Message = exception.Message;
            stub.Source = exception.Source;
            stub.Stack = exception.StackTrace;
            if (exception.InnerException != null)
            {
                stub.InnerException = CreateExceptionStub(exception.InnerException);
            }
            return stub;
        }

        public static string CreateExceptionStubXML(System.Exception exception)
        {
            ExceptionStub stub = CreateExceptionStub(exception);
            var objectSerializationManager = new ObjectSerializationManager();
            return objectSerializationManager.SerializeObject(stub);
        }

        #endregion
    }
}

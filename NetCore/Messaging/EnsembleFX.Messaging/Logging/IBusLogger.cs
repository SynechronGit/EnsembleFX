using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Logging
{
    public interface IBusLogger
    {
        void LogStopwatch(string className, string methodName, Dictionary<string, object> parameters, object returnValue, TimeSpan elapsedTimeSpan);

        void LogPublish(IMessageEnvelope envelope, string successMessage, Type messageType);
        void LogPublishFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type messageType);

        void LogBusInfo(string message, Type busType);
        void LogBusReceived(IMessageEnvelope envelope, string successMessage);
        void LogBusReceivedFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred);

        void LogAdapterInfo(string message, Type adapterType);
        void LogAdapterSuccess(IMessageEnvelope envelope, string successMessage, Type adapterType);
        void LogAdapterFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type adapterType);

        void LogInfo(string message);
        void LogError(string failureMessage, System.Exception exceptionOccurred);

        void LogSubscribeInfo(string message, Type busType);
        void LogSubscribeSuccess(IMessageEnvelope envelope, string successMessage, Type subscriberType);
        void LogSubscribeFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type subscriberType);
    }
}

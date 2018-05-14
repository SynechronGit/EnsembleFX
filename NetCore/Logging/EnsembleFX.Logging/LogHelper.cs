using EnsembleFX.Logging;
using Newtonsoft.Json;

namespace EnsembleFX.Logging
{
    public static class LogHelper
    {
        public static LogModel GetLogModel(string placeId, string eventName, string message, object request = null, object response = null)
        {
            LogModel logModel = new LogModel();
            logModel.ApplicationIdentifier = placeId;
            logModel.EventName = eventName;
            logModel.Message = message;

            if (request != null)
                logModel.RequestObject = JsonConvert.SerializeObject(request);

            if (response != null)
                logModel.ResponseObject = JsonConvert.SerializeObject(response);

            return logModel;
        }
    }
}

using EnsembleFX.Logging;
using EnsembleFX.Messaging;
using EnsembleFX.Messaging.Model;
using EnsembleFX.Messaging.Model.Enums;
using EnsembleFX.Repository;
using EnsembleFX.Repository.Model;
//using EnsembleFX.Shared;
//using Microsoft.Azure;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
//using STEPIN.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Core.Helpers
{
    public class AzureQueueHelper
    {

        #region Internal Members
        IQueueClient queueClient;
        #endregion

        #region Properties

        public string TopicName { get; set; }
        public string SubscriberName { get; set; }

        #endregion

        #region Private members
        public string fileTriggerCollectionName = ConfigurationManager.AppSettings.Get("FileTriggerCollection");
        public string emailTriggerCollectionName = ConfigurationManager.AppSettings.Get("EmailTriggerCollection");
        public string timeTriggerCollectionName = ConfigurationManager.AppSettings.Get("TimeTriggerCollection");
        private static string iFTTTCollectionName = ConfigurationManager.AppSettings.Get("IFTTTAppletCollection");
        private static string workflowInstanceCollectionName = ConfigurationManager.AppSettings.Get("WorkflowInstanceCollection");
        private string workflowCollectionName = ConfigurationManager.AppSettings.Get("WorkflowCollection");
        private string agentConfigurationcollectionName = ConfigurationManager.AppSettings.Get("AgentConfigurationCollection");

        IDBRepository<FileTrigger> _dbManageFileTrigger;
        IDBRepository<EmailTrigger> _dbManageEmailTrigger;
        IDBRepository<TimeTrigger> _dbManageTimeTrigger;
        IDBRepository<IFTTTApplet> _dbIFTTTRepository;
        IDBRepository<WorkflowInstance> _workflowInstanceDbRepository;
        IDBRepository<Workflow> _workflowDbRepository;
        IDBRepository<AgentConfiguration> _agentConfigurationDBRepository;
        ILogManager _LogManager { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        //public AzureQueueHelper(ILogController logController)
        //{
        //    //this.httpContextAccessor = httpContextAccessor;

        //    _dbManageFileTrigger = new DBManager<FileTrigger>(fileTriggerCollectionName, logController).Instance;
        //    _dbManageEmailTrigger = new DBManager<EmailTrigger>(emailTriggerCollectionName, logController).Instance;
        //    _dbManageTimeTrigger = new DBManager<TimeTrigger>(timeTriggerCollectionName, logController).Instance;
        //    _dbIFTTTRepository = new DBManager<IFTTTApplet>(iFTTTCollectionName, logController).Instance;
        //    _workflowInstanceDbRepository = new DBManager<WorkflowInstance>(workflowInstanceCollectionName, logController).Instance;
        //    _workflowDbRepository = new DBManager<Workflow>(workflowCollectionName, logController).Instance;
        //    _agentConfigurationDBRepository = new DBManager<AgentConfiguration>(agentConfigurationcollectionName, logController).Instance;
        //    _LogManager = new LogManager(logController);
        //}
        public AzureQueueHelper(ILogController logController, IOptions<ConnectionStrings> connectionStrings)
        {
            //this.httpContextAccessor = httpContextAccessor;

            _dbManageFileTrigger = new DBManager<FileTrigger>(fileTriggerCollectionName, logController, connectionStrings).Instance;
            _dbManageEmailTrigger = new DBManager<EmailTrigger>(emailTriggerCollectionName, logController, connectionStrings).Instance;
            _dbManageTimeTrigger = new DBManager<TimeTrigger>(timeTriggerCollectionName, logController, connectionStrings).Instance;
            _dbIFTTTRepository = new DBManager<IFTTTApplet>(iFTTTCollectionName, logController, connectionStrings).Instance;
            _workflowInstanceDbRepository = new DBManager<WorkflowInstance>(workflowInstanceCollectionName, logController, connectionStrings).Instance;
            _workflowDbRepository = new DBManager<Workflow>(workflowCollectionName, logController, connectionStrings).Instance;
            _agentConfigurationDBRepository = new DBManager<AgentConfiguration>(agentConfigurationcollectionName, logController, connectionStrings).Instance;
            _LogManager = new LogManager(logController);
        }

        #endregion

        #region Methods

        //public void CreateTopic(string topicName)
        //{
        //    if (String.IsNullOrEmpty(topicName))
        //    {
        //        topicName = "ensembleemailtopic_staging";
        //    }

        //    TopicName = topicName;
        //    namespaceManager = NamespaceManager.Create();
        //    if (!namespaceManager.TopicExists(topicName))
        //    {
        //        topicDescriptor = namespaceManager.CreateTopic(topicName);
        //    }
        //    else
        //    {
        //        topicDescriptor = namespaceManager.GetTopic(topicName);
        //    }
        //}

        public void SendMessage(IMessageEnvelope envelope)
        {
            //topicClient = TopicClient.Create(TopicName);
            //Message Message = CreateMessage(envelope);
            //try
            //{
            //    topicClient.Send(Message);
            //    topicClient.Close();
            //}
            //catch (MessagingException e)
            //{
            //    if (!e.IsTransient)
            //    {
            //        throw;
            //    }
            //    else
            //    {
            //        Thread.Sleep(2000);
            //        topicClient.Send(Message);
            //        topicClient.Close();
            //    }
            //}
        }

        /// <summary>
        /// Send Message to Azure Queue
        /// </summary>
        /// <param name="serviceBusConnectionString">Service Bus Connection string</param>
        /// <param name="queue">queue name</param>
        /// <param name="message">Brokered Message</param>
        public async void SendMessage(string serviceBusConnectionString, string queue, Message message)
        {
            try
            {
                QueueClient client = new QueueClient(serviceBusConnectionString, queue, ReceiveMode.PeekLock);
                client.SendAsync(message);
                await client.CloseAsync()
;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Send Message to Azure Queue
        /// </summary>
        /// <param name="serviceBusConnectionString">Service Bus Connection string</param>
        /// <param name="queue">queue name</param>
        /// <param name="message">Brokered Message</param>
        public async void SendMessage<T>(string serviceBusConnectionString, string queue, T messageObject)
        {
            try
            {
                string objectJson = this.SerializeObjectToJson<T>(messageObject);
                byte[] objectBytes = Encoding.UTF8.GetBytes(objectJson);
                Message queueMessage = new Message(objectBytes);
                QueueClient client = new QueueClient(serviceBusConnectionString, queue, ReceiveMode.PeekLock);
                client.SendAsync(queueMessage).Wait();//As per observation if we don't wait, No message added to queue. Try and check after removing Wait() call.
                await client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reveive Brokered Message
        /// </summary>
        /// <param name="serviceBusConnectionString">Service Bus Connection string</param>
        /// <param name="queue">queue name</param>
        /// <returns>Brokered Message</returns>
        public async Task ReceiveMessage(string serviceBusConnectionString, string queue)
        {
            queueClient = new QueueClient(serviceBusConnectionString, queue, ReceiveMode.PeekLock);

        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(LogErrors)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            //Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        private Task LogErrors(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            //Console.WriteLine("Exception context for troubleshooting:");
            //Console.WriteLine($"- Endpoint: {context.Endpoint}");
            //Console.WriteLine($"- Entity Path: {context.EntityPath}");
            //Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public IMessageEnvelope ReceiveMessage(bool isWaiting, int timeoutInSeconds, bool receiveAll, string queueName)
        {
            return new MessageEnvelope();
        }

        //public void RegisterSubscriber(string subscriberName)
        //{
        //    if (String.IsNullOrEmpty(subscriberName))
        //    {
        //        throw new ArgumentException("SubscriberName is not specified");
        //    }
        //    SubscriberName = subscriberName;
        //    if (!namespaceManager.SubscriptionExists(topicDescriptor.Path, subscriberName))
        //    {
        //        SubscriptionDescription subscription = namespaceManager.CreateSubscription(topicDescriptor.Path, subscriberName);
        //    }
        //}

        //public void UnregisterSubscriber(string subscriberName)
        //{
        //    if (String.IsNullOrEmpty(subscriberName))
        //    {
        //        throw new ArgumentException("SubscriberName is not specified");
        //    }
        //    if (namespaceManager.SubscriptionExists(topicDescriptor.Path, subscriberName))
        //    {
        //        namespaceManager.DeleteSubscription(topicDescriptor.Path, subscriberName);
        //    }
        //}

        //internal Message CreateMessage(IMessageEnvelope envelope)
        //{
        //    JsonMessageSerializer jsonMessageSerializer = new JsonMessageSerializer();
        //    string messageToPost = jsonMessageSerializer.SerializeEnvelope(envelope);
        //    Message Message = new Message(messageToPost) { MessageId = Guid.NewGuid().ToString() };
        //    return Message;
        //}

        /// <summary>
        /// Create New Queue 
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public void CreateQueue(string serviceBusConnectionString, string queueName)
        {
            //var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

            //// Create if it does not exists
            //if (!namespaceManager.QueueExists(queueName))
            //{
            //    namespaceManager.CreateQueue(queueName);
            //}
        }

        public bool QueueExits(string serviceBusConnectionString, string queueName)
        {
            //var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            //return namespaceManager.QueueExists(queueName);
            return false;
        }


        public int GetQueueCount(string connectionString)
        {
            //TODO:: Need to Alternative of Microsoft.Azure for.Net Core

            //NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            //return namespaceManager.GetQueues().Count();

            return 0;
        }

        public List<AzureQueueModel> GetQueueDetails(string connectionString)
        {
            //TODO:: Need to Alternative of Microsoft.Azure for.Net Core

            //NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            //List<QueueDescription> _queues = namespaceManager.GetQueues().ToList();

            //List<AzureQueueModel> _azureQueueModelCollection = new List<AzureQueueModel>();

            //_queues.ForEach(delegate (QueueDescription messageQueue)
            //{
            //    AzureQueueModel _azureQueueModel = new AzureQueueModel();
            //    _azureQueueModel.QueueName = messageQueue.Path;
            //    _azureQueueModel.MessageCount = messageQueue.MessageCount;
            //    _azureQueueModelCollection.Add(_azureQueueModel);
            //}

            //return _azureQueueModelCollection;

            return null;
        }

        public async Task<List<AzureMessageModel>> GetMessages(string connectionString, string QueueName)
        {
            //TODO:: Need to Alternative of Microsoft.Azure for.Net Core
            //    List<AzureQueueModel> _azureQueueModelCollection = new List<AzureQueueModel>();

            //    NamespaceManager namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            //    QueueClient _queueClient = QueueClient.CreateFromConnectionString(connectionString, QueueName);

            //    long _queueCount = namespaceManager.GetQueue(QueueName).MessageCount;

            //    List<AzureMessageModel> _azureMessageModelCollection = await GetAzureMessages(_queueClient, ConvertLongtoInt(_queueCount));

            //    return _azureMessageModelCollection;

            return null;
        }

        //private async Task<List<AzureMessageModel>> GetAzureMessages(QueueClient _queueClient, int _queueCount)
        //{
        //    List<AzureMessageModel> _azureMessageModelCollection = new List<AzureMessageModel>();

        //    var entityName = typeof(WorkflowOrchestratorMessageEnvelope).Name;

        //    for (int i = 0; i < _queueCount; i++)
        //    {
        //        Message _Message = await _queueClient.PeekAsync();
        //        WorkflowOrchestratorMessageEnvelope _workFlowOrchestratorMessageEnvelope = null;
        //        try
        //        {
        //            var contentType = _Message.ContentType;
        //            if (!string.IsNullOrEmpty(contentType))
        //            {
        //                var bodyType = Type.GetType(contentType);
        //                if (bodyType != null)
        //                {
        //                    if (bodyType.Name == entityName)
        //                    {
        //                        _workFlowOrchestratorMessageEnvelope = _Message.GetBody<WorkflowOrchestratorMessageEnvelope>();

        //                        if (_workFlowOrchestratorMessageEnvelope != null)
        //                        {
        //                            AzureMessageModel _azureMessageModel = new AzureMessageModel();

        //                            //Serialize Object To Jsondata.
        //                            _azureMessageModel.JSONData = SerializeObjectToJson<WorkflowOrchestratorMessageEnvelope>(_workFlowOrchestratorMessageEnvelope);

        //                            // Get TriggerEventMessage message details
        //                            if (_workFlowOrchestratorMessageEnvelope.TriggerEventMessage != null)
        //                            {
        //                                _azureMessageModel.TriggerName = GetTriggerName(_workFlowOrchestratorMessageEnvelope.TriggerEventMessage.TriggerId.ToString(),
        //                                                                _workFlowOrchestratorMessageEnvelope.TriggerEventMessage.TriggerType);

        //                                _azureMessageModel.TriggerType = GetTriggerType(_workFlowOrchestratorMessageEnvelope.TriggerEventMessage.TriggerType);

        //                                IFTTTApplet _iFTTTApplet = _dbIFTTTRepository.GetById(_workFlowOrchestratorMessageEnvelope.TriggerEventMessage.IFTTTAppletId.ToString());

        //                                if (_iFTTTApplet != null)
        //                                    _azureMessageModel.IFTTTAppletName = _iFTTTApplet.AppletTitle;
        //                                _azureMessageModel.MessageType = MessageTypes.Trigger_Event_Message.ToString().Replace("_", " ");

        //                            }

        //                            // Get WorkflowTaskStatusMessage message details
        //                            if (_workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage != null)
        //                            {
        //                                _azureMessageModel.WorkFlowName = GetWorkFlowName(_workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.WorkflowId.ToString());
        //                                _azureMessageModel.WorkFlowTaskType = GetWorkFlowTaskType(_workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.OriginalCommand.TaskType);
        //                                _azureMessageModel.WorkFlowTaskTypeStatus = _workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.Status.ToString();
        //                                _azureMessageModel.AgentName = GetAgentName(_workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.AssignedAgentConfigurationId.ToString());
        //                                _azureMessageModel.StartedOnUTC = _workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.StartedOnUTC;
        //                                _azureMessageModel.MessageType = MessageTypes.Workflow_Task_Status_Message.ToString().Replace("_", " ");
        //                                _azureMessageModel.Output = (_workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.Output == null) ? "" : _workFlowOrchestratorMessageEnvelope.WorkflowTaskStatusMessage.Output;
        //                            }
        //                            _azureMessageModelCollection.Add(_azureMessageModel);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //If entity name is not WorkflowOrchestratorMessageEnvelope then serialize the entity to JSON.
        //                        var _azureMessageModel = new AzureMessageModel();
        //                        var messageBody = GetMessageBodyType(_Message, bodyType);
        //                        _azureMessageModel.JSONData = SerializeObjectToJson<object>(messageBody);
        //                        _azureMessageModelCollection.Add(_azureMessageModel);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //TODO: Pass username and requestAbsoluteUrl from httpcontext here
        //            var logModel = CreateLogModel(ex.Message, ex.StackTrace, "", "");
        //            _LogManager.LogMessage(logModel, LogLevel.Error);

        //        }
        //    }
        //    return _azureMessageModelCollection;
        //}

        private string GetTriggerName(string triggerId, TriggerType triggerType)
        {
            string _triggerName = "";

            switch (triggerType)
            {
                case TriggerType.FileTrigger:
                    var fileTrigger = _dbManageFileTrigger.GetById(triggerId);
                    if (fileTrigger != null)
                        _triggerName = fileTrigger.TriggerName;
                    break;
                case TriggerType.EmailTrigger:
                    var emailTrigger = _dbManageEmailTrigger.GetById(triggerId);
                    if (emailTrigger != null)
                        _triggerName = emailTrigger.TriggerName;
                    break;

                case TriggerType.TimeTrigger:
                    //get file trigger
                    var timeTrigger = _dbManageTimeTrigger.GetById(triggerId);
                    if (timeTrigger != null)
                        _triggerName = timeTrigger.TriggerName;
                    break;
            }
            return _triggerName;
        }

        private string GetTriggerType(TriggerType triggerType)
        {
            string _triggerType = "";
            switch (triggerType)
            {
                case TriggerType.FileTrigger:
                    _triggerType = "File Tigger";
                    break;
                case TriggerType.EmailTrigger:
                    _triggerType = "Email Tigger";
                    break;

                case TriggerType.TimeTrigger:
                    _triggerType = "Time Tigger";
                    break;
            }
            return _triggerType;
        }

        private string GetWorkFlowName(string workFlowId)
        {
            string _workFlowName = "";
            Workflow _workFlow = _workflowDbRepository.GetById(workFlowId);
            if (_workFlow != null)
                _workFlowName = _workFlow.WorkflowName;
            return _workFlowName;
        }

        private string GetWorkFlowTaskType(WorkFlowTaskType workFlowTaskType)
        {
            string _workFlowTaskType = "";
            switch (workFlowTaskType)
            {
                case WorkFlowTaskType.Action_PythonScript:
                    _workFlowTaskType = WorkFlowTaskType.Action_PythonScript.ToString().Replace("_", " ");
                    break;
                case WorkFlowTaskType.Action_TestComplete:
                    _workFlowTaskType = WorkFlowTaskType.Action_TestComplete.ToString().Replace("_", " ");
                    break;
                case WorkFlowTaskType.Action_VBScript:
                    _workFlowTaskType = WorkFlowTaskType.Action_TestComplete.ToString().Replace("_", " ");
                    break;
                case WorkFlowTaskType.Condition:
                    _workFlowTaskType = WorkFlowTaskType.Condition.ToString();
                    break;
                case WorkFlowTaskType.Error:
                    _workFlowTaskType = WorkFlowTaskType.Error.ToString();
                    break;
                case WorkFlowTaskType.LoopEnd:
                    _workFlowTaskType = WorkFlowTaskType.LoopEnd.ToString();
                    break;
                case WorkFlowTaskType.LoopStart:
                    _workFlowTaskType = WorkFlowTaskType.LoopStart.ToString();
                    break;
                case WorkFlowTaskType.ManualTask:
                    _workFlowTaskType = WorkFlowTaskType.ManualTask.ToString();
                    break;
                case WorkFlowTaskType.Start:
                    _workFlowTaskType = WorkFlowTaskType.Start.ToString();
                    break;
                case WorkFlowTaskType.Unknown:
                    _workFlowTaskType = WorkFlowTaskType.Unknown.ToString();
                    break;
            }
            return _workFlowTaskType;
        }

        private string GetAgentName(string agentConfiguratioId)
        {
            string _agentName = "";
            AgentConfiguration _agentConfiguration = _agentConfigurationDBRepository.GetById(agentConfiguratioId);
            if (_agentConfiguration != null)
                _agentName = _agentConfiguration.AgentName;
            return _agentName;
        }

        private int ConvertLongtoInt(long number)
        {
            if (number < int.MaxValue)
            { return unchecked((int)number); }
            else
            { return 0; }
        }

        private string SerializeObjectToJson<T>(T ObjectToSerialize)
        {
            return JsonConvert.SerializeObject(ObjectToSerialize);
        }

        private object GetMessageBodyType(Message _Message, Type bodyType)
        {
            MethodInfo method = typeof(Message).GetMethod("GetBody", new Type[] { });
            MethodInfo generic = method.MakeGenericMethod(bodyType);
            return generic.Invoke(_Message, null);
        }

        private LogModel CreateLogModel(string message, string stacktrace, string username, string requestAbsoluteUrl)
        {
            LogModel logmodel = new LogModel();
            logmodel.StackTrace = stacktrace;
            logmodel.Message = message;
            //TODO: Need to pass with parameter
            //logmodel.UserName = this.httpContextAccessor.HttpContext.User.Identity.Name;
            logmodel.UserName = username;

            //TODO: Get Url from HttpContext
            //logmodel.Url = this.httpContextAccessor.HttpContext.Request.Url.AbsoluteUri;
            logmodel.Url = requestAbsoluteUrl;

            return logmodel;
        }
        #endregion
    }
}

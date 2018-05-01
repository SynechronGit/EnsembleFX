using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.Serialization;
using EnsembleFX.Messaging.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public class SqlQueueAdapter : IQueueAdapter
    {
        #region Public Members

        /// <summary>
        /// Occurs when [on message].
        /// </summary>
        public event OnMessageDelegate OnMessage;
        #endregion

        #region IQueueManager Members
        readonly SqlQueueConfiguration _configuration;
        readonly SqlConnection _connection;
        readonly IMessageSerializer _serializer;
        readonly SqlQueueHelper _sqlQueueHelper;
        bool _isStop = false;
        readonly IBusLogger _logger;
        IAsyncResult _asyncStart;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueueAdapter"/> class.
        /// </summary>
        /// <param name="configFactory">The config factory.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <param name="logger">The logger.</param>
        public SqlQueueAdapter(IConfigurationFactory configFactory, string configurationName, IBusLogger logger)
        {
            this._configuration = configFactory.GetConfiguration<SqlQueueConfiguration>("SqlQueueConfiguration/Queue");
            _serializer = new JSONMessageSerializer();
            _connection = new SqlConnection();
            _sqlQueueHelper = new SqlQueueHelper();
            this._logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueueAdapter"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public SqlQueueAdapter(SqlQueueConfiguration configuration, IBusLogger logger)
        {
            this._configuration = configuration;
            _serializer = new JSONMessageSerializer();
            _connection = new SqlConnection();
            _sqlQueueHelper = new SqlQueueHelper();
            this._logger = logger;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        public void Initialize(IServerContext serverContext)
        {
            try
            {

                _connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[_configuration.connectionString.Name].ToString();

                _connection.Open();
                _sqlQueueHelper.CreateMessageType(_configuration.messageType.Name, _connection);
                _sqlQueueHelper.CreateContract(_configuration.contract.Name, _configuration.messageType.Name, _connection);
                _sqlQueueHelper.CreateQueue(_configuration.queue.Name, _connection);
                _sqlQueueHelper.CreateService(_configuration.serviceTo.Name, _configuration.queue.Name, _configuration.contract.Name, _connection);
                _sqlQueueHelper.CreateService(_configuration.serviceFrom.Name, _configuration.queue.Name, _configuration.contract.Name, _connection);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }

        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _isStop = false;
            var start = new StartReceivingDelegate(ReceiveMessageAsync);
            _asyncStart = start.BeginInvoke(null, this);

        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        public void SendMessage(IMessageEnvelope messageEnvelope)
        {
            try
            {
                var swatch = new System.Diagnostics.Stopwatch();
                swatch.Start();
                _sqlQueueHelper.SendMessage(messageEnvelope.MessageUID, _configuration.messageType.Name, _serializer.SerializeEnvelope(messageEnvelope), _configuration.serviceFrom.Name, _configuration.serviceTo.Name, _configuration.contract.Name, _connection);
                swatch.Stop();

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Recieves the message async.
        /// </summary>
        protected void ReceiveMessageAsync()
        {
            SqlDataReader reader = null;

            while (!_isStop)
            {
                try
                {
                    reader = _sqlQueueHelper.ReceiveMessage(true, Convert.ToInt32(_configuration.timeoutInSeconds.Name), true, _configuration.queue.Name, _connection);
                    while (reader.Read())
                    {

                        byte[] messageBytes = (byte[])reader.GetValue(13);

                        String messageText = ASCIIEncoding.ASCII.GetString(messageBytes);
                        if (!string.IsNullOrEmpty(messageText))
                        {
                            IMessageEnvelope envelope = null;
                            try
                            {
                                envelope = _serializer.DeserializeEnvelope(messageText);

                                _logger.LogAdapterSuccess(envelope, "Message Received:" + envelope.MessageUID, this.GetType());

                                if (envelope != null)
                                    OnMessage(envelope);

                            }
                            catch (System.Exception ex)
                            {
                                _logger.LogAdapterFailure(envelope, ex.Message, ex, this.GetType());
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex.Message, ex);

                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    Thread.Sleep(2000);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!_isStop)
            {
                _isStop = true;
                if (_asyncStart != null && _asyncStart.AsyncWaitHandle != null)
                    _asyncStart.AsyncWaitHandle.WaitOne();
            }


        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        #endregion

        #region IQueueAdapter Members

        /// <summary>
        /// Gets a value indicating whether this instance is control queue.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is control queue; otherwise, <c>false</c>.
        /// </value>
        public bool IsControlQueue
        {
            get
            {
                return _configuration.IsControlQueue;
            }
        }

        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        /// <value>The name of the queue.</value>
        public string QueueName
        {
            get
            {
                return _configuration.Name;
            }
        }

        #endregion
    }
}

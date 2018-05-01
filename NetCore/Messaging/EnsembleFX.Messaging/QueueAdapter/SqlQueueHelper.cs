using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public class SqlQueueHelper
    {
        #region Public Members

        const string QUEUECHECKQUERY = "SELECT COUNT(name) FROM  sys.service_queues WHERE name = '{0}'";
        const string QUEUECREATEQUERY = "CREATE QUEUE {0}";

        const string SERVICECHECKQUERY = "SELECT COUNT(name) FROM sys.services WHERE name = '{0}'";
        const string SERVICECREATEQUERY = "CREATE SERVICE {0} ON QUEUE {1} ({2})";

        const string MESSAGETYPECHECKQUERY = "SELECT COUNT(name) FROM sys.service_message_types WHERE name = '{0}'";
        const string MESSAGETYPECREATEQUERY = "CREATE MESSAGE TYPE {0} VALIDATION = NONE";

        const string CONTRACTCHECKQUERY = "SELECT COUNT(name) FROM sys.service_contracts WHERE name = '{0}'";
        const string CONTRACTCREATEQUERY = "CREATE CONTRACT {0} ({1} SENT BY INITIATOR)";

        const string BEGINDIALOGQUERY = "DECLARE @conversionID UNIQUEIDENTIFIER;SET @conversionID = '{0}';BEGIN DIALOG CONVERSATION @conversionID FROM SERVICE {1} TO SERVICE '{2}' ON CONTRACT {3} WITH ENCRYPTION = OFF;";

        const string SENDQUERY = ";SEND ON CONVERSATION @conversionID MESSAGE TYPE {1} ('{2}');";

        const string WAITFORQUERY = "WAITFOR ( {0} ), TIMEOUT {1}; ";

        const string RECEIVEQUERY = "RECEIVE {0} FROM {1}";

        const string MESSAGECOUNTQUERY = "SELECT COUNT(*) FROM {0}";
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="connection">The connection.</param>
        public void CreateQueue(string queueName, SqlConnection connection)
        {
            if (!RowsExist(string.Format(CultureInfo.CurrentCulture, QUEUECHECKQUERY, queueName), connection))
            {
                ExecuteNonQuery(string.Format(CultureInfo.CurrentCulture, QUEUECREATEQUERY, queueName), connection);
            }
        }

        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="contractName">Name of the contract.</param>
        /// <param name="connection">The connection.</param>
        public void CreateService(string serviceName, string queueName, string contractName, SqlConnection connection)
        {
            if (!RowsExist(string.Format(CultureInfo.CurrentCulture, SERVICECHECKQUERY, serviceName, queueName), connection))
            {
                ExecuteNonQuery(string.Format(CultureInfo.CurrentCulture, SERVICECREATEQUERY, serviceName, queueName, contractName), connection);
            }
        }

        /// <summary>
        /// Creates the type of the message.
        /// </summary>
        /// <param name="messageTypeName">Name of the message type.</param>
        /// <param name="connection">The connection.</param>
        public void CreateMessageType(string messageTypeName, SqlConnection connection)
        {
            if (!RowsExist(string.Format(CultureInfo.CurrentCulture, MESSAGETYPECHECKQUERY, messageTypeName), connection))
            {
                ExecuteNonQuery(string.Format(CultureInfo.CurrentCulture, MESSAGETYPECREATEQUERY, messageTypeName), connection);
            }
        }

        /// <summary>
        /// Creates the contract.
        /// </summary>
        /// <param name="contractName">Name of the contract.</param>
        /// <param name="messageTypeName">Name of the message type.</param>
        /// <param name="connection">The connection.</param>
        public void CreateContract(string contractName, string messageTypeName, SqlConnection connection)
        {
            if (!RowsExist(string.Format(CultureInfo.CurrentCulture, CONTRACTCHECKQUERY, contractName, messageTypeName), connection))
            {
                ExecuteNonQuery(string.Format(CultureInfo.CurrentCulture, CONTRACTCREATEQUERY, contractName, messageTypeName), connection);
            }
        }

        /// <summary>
        /// Begins the dialog.
        /// </summary>
        /// <param name="conversationID">The conversation ID.</param>
        /// <param name="fromService">From service.</param>
        /// <param name="toService">To service.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="connection">The connection.</param>
        public void BeginDialog(Guid conversationID, string fromService, string toService, string contract, SqlConnection connection)
        {
            string query = string.Format(CultureInfo.CurrentCulture, BEGINDIALOGQUERY, conversationID.ToString("D"), fromService, toService, contract);
            ExecuteNonQuery(query, connection);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="conversationID">The conversation ID.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageBody">The message body.</param>
        /// <param name="fromService">From service.</param>
        /// <param name="toService">To service.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="connection">The connection.</param>
        public void SendMessage(Guid conversationID, string messageType, string messageBody, string fromService, string toService, string contract, SqlConnection connection)
        {
            string query = string.Format(CultureInfo.CurrentCulture, BEGINDIALOGQUERY, conversationID.ToString("D"), fromService, toService, contract);
            query += ";" + string.Format(CultureInfo.CurrentCulture, SENDQUERY, conversationID.ToString("D"), messageType, EscapeCharacters(messageBody));
            ExecuteNonQuery(query, connection);
        }

        /// <summary>
        /// Recieves the message.
        /// </summary>
        /// <param name="isWaiting">if set to <c>true</c> [is waiting].</param>
        /// <param name="timeoutInSeconds">The time out in seconds.</param>
        /// <param name="receiveAll">if set to <c>true</c> [recieve all].</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public SqlDataReader ReceiveMessage(bool isWaiting, int timeoutInSeconds, bool receiveAll, string queueName, SqlConnection connection)
        {
            string top = "TOP 1";
            if (receiveAll)
                top = "*";
            string query = string.Format(CultureInfo.CurrentCulture, RECEIVEQUERY, top, queueName);
            if (isWaiting)
                query = string.Format(CultureInfo.CurrentCulture, WAITFORQUERY, query, timeoutInSeconds);

            return ExecuteReader(query, connection);
        }

        /// <summary>
        /// Messageses the in queue count.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        public int MessagesInQueueCount(string queueName, SqlConnection connection)
        {
            return ExecuteRowCount(string.Format(CultureInfo.CurrentCulture, MESSAGECOUNTQUERY, queueName), connection);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Rowses the exist.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        private bool RowsExist(string commandText, SqlConnection connection)
        {
            if (ExecuteRowCount(commandText, connection) > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Executes the row count.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        private int ExecuteRowCount(string commandText, SqlConnection connection)
        {
            SqlDataReader reader = ExecuteReader(commandText, connection);
            int count = 0;
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();

            return count;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        /// <returns></returns>
        private SqlDataReader ExecuteReader(string commandText, SqlConnection connection)
        {
            SqlDataReader reader;
            SqlCommand receiveCommand = connection.CreateCommand();
            receiveCommand.CommandText = commandText;
            reader = receiveCommand.ExecuteReader(CommandBehavior.Default);
            return reader;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="connection">The connection.</param>
        private void ExecuteNonQuery(string commandText, SqlConnection connection)
        {
            SqlCommand receiveCommand = connection.CreateCommand();
            receiveCommand.CommandText = commandText;


            receiveCommand.ExecuteNonQuery();


            return;
        }

        /// <summary>
        /// Escapes the characters.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns></returns>
        private string EscapeCharacters(string inputString)
        {
            inputString = inputString.Replace("'", "''");
            return inputString;
        }
        #endregion

    }
}

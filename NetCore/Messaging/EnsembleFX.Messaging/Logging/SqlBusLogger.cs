using EnsembleFX.Logging;
using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Exception;
using EnsembleFX.Messaging.Serialization;
using EnsembleFX.Messaging.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Logging
{
    public class SqlBusLogger : IBusLogger
    {
        ILogController logController;
        public SqlBusLogger(ILogController logController)
        {
            this.logController = logController;
        }
        #region Public Methods

        public void MessagingPublisherLog(IMessageEnvelope envelope, string publisherType, bool isSuccess, string message, System.Exception occurredException)
        {

            string environment = ConfigurationHelper.GetEnvironment(envelope);
            JSONMessageSerializer serializer = new JSONMessageSerializer();
            string envelopeString = serializer.SerializeEnvelope(envelope);
            string occuredExceptionStack = occurredException != null ? ExceptionStub.CreateExceptionStubXML(occurredException) : string.Empty;

            const string insertQuery = @"INSERT INTO [Ensemble].[MessagingPublisherLog] ([MessageUID],[MessageSentOn],[MessageType],[Topic],[Originator],[UserName],[ReplyTo],[ReplyOf],[PublisherType]
                                 ,[IsSuccess],[Message],[MessageEnvelope],[ExceptionType],[ExceptionStack],[Environment]) VALUES 
                                 (@MessageUID,@MessageSentOn, @MessageType,@Topic,@Originator, @UserName, @ReplyTo, @ReplyOf, @PublisherType, @IsSuccess, @Message, @MessageEnvelope,
                                 @ExceptionType, @ExceptionStack, @Environment)";

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageUID",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.MessageUID.ToString().ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageSentOn",
                                                                SqlDbType = SqlDbType.DateTime,
                                                                Value = envelope.MessageSentOn.ToSqlDateTime()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.MessageType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Topic",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.Topic.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Originator",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.Originator.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@UserName",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.User.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ReplyTo",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.ReplyTo.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ReplyOf",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelope.ReplyOf.ToString().ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@PublisherType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = publisherType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsSuccess",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isSuccess
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Message",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = message.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageEnvelope",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = envelopeString
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = occurredException!=null? occurredException.GetType().FullName:SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionStack",
                                                                SqlDbType = SqlDbType.Xml,
                                                                Value = occuredExceptionStack
                                                            },
                                                             new SqlParameter
                                                            {
                                                                ParameterName = "@Environment",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = environment!=null?environment:SqlString.Null
                                                            }
                                                    };

            AddData(insertQuery, sqlParameters);
        }

        public void MessagingQueueLog(IMessageEnvelope envelope, string adapterType, bool isSuccess, string message, System.Exception occurredException)
        {
            if (envelope != null)
                MessagingQueueLog(envelope.MessageUID, envelope.MessageSentOn, adapterType, isSuccess, message, occurredException, ConfigurationHelper.GetEnvironment(envelope));
            else
                MessagingQueueLog(null, null, adapterType, isSuccess, message, occurredException, null);
        }

        public void MessagingQueueLog(Guid? messageUId, DateTime? loggedOn, string adapterType, bool isSuccess, string message, System.Exception occurredException, string environment)
        {

            var occuredExceptionStck = occurredException != null ? ExceptionStub.CreateExceptionStubXML(occurredException) : string.Empty;

            const string insertQuery = @"INSERT INTO [Ensemble].[MessagingQueueLog] ([MessageUID],[LoggedOn],[AdapterType],[IsSuccess],[Message],[ExceptionType],[ExceptionStack],[Environment]) VALUES 
                                       (@MessageUID,@LoggedOn, @AdapterType,@IsSuccess, @Message, @ExceptionType, @ExceptionStack,@Environment)";

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageUID",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = messageUId.HasValue? messageUId.Value.ToString():SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@LoggedOn",
                                                                SqlDbType = SqlDbType.DateTime,
                                                                Value = loggedOn.HasValue && (loggedOn.Value != DateTime.MinValue) ?loggedOn.Value:SqlDateTime.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@AdapterType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = adapterType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsSuccess",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isSuccess
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Message",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = message.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = occurredException!=null? occurredException.GetType().FullName:SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionStack",
                                                                SqlDbType = SqlDbType.Xml,
                                                                Value = occuredExceptionStck
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Environment",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = environment!=null?environment:SqlString.Null
                                                            }
                                                    };

            AddData(insertQuery, sqlParameters);
        }

        public void MessagingHostLog(IMessageEnvelope envelope, string busType, bool isSuccess, string message, System.Exception occurredException)
        {
            if (envelope != null)
                MessagingHostLog(envelope.MessageUID, envelope.MessageSentOn, envelope.Originator, busType, isSuccess, message, occurredException, ConfigurationHelper.GetEnvironment(envelope));
            else
                MessagingHostLog(null, null, string.Empty, busType, isSuccess, message, occurredException, null);
        }

        public void MessagingHostLog(Guid? messageUId, DateTime? loggedOn, string hostInstance, string busType, bool isSuccess, string message, System.Exception occurredException, string environment)
        {

            var occuredExceptionStck = occurredException != null ? ExceptionStub.CreateExceptionStubXML(occurredException) : string.Empty;

            const string insertQuery = @"INSERT INTO [Ensemble].[MessagingHostLog] ([MessageUID],[LoggedOn],[HostInstance],[BusType],[IsSuccess],[Message],[ExceptionType],[ExceptionStack],[Environment]) VALUES 
                                 (@MessageUID,@LoggedOn, @HostInstance,@BusType,@IsSuccess, @Message, 
                                 @ExceptionType, @ExceptionStack,@Environment)";

            SqlString sqlString = SqlString.Null;
            SqlDateTime sqlDateTime = SqlDateTime.Null;

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageUID",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = messageUId.HasValue ? messageUId.Value.ToString() : SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@LoggedOn",
                                                                SqlDbType = SqlDbType.DateTime,
                                                                Value = loggedOn.HasValue && (loggedOn.Value != DateTime.MinValue) ?loggedOn.Value:SqlDateTime.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@HostInstance",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = hostInstance.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@BusType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = busType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsSuccess",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isSuccess
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Message",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = message.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = occurredException!=null? occurredException.GetType().FullName:SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionStack",
                                                                SqlDbType = SqlDbType.Xml,
                                                                Value = occuredExceptionStck
                                                            },
                                                            new SqlParameter
                                                            {
                                                                ParameterName = "@Environment",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = environment!=null?environment:SqlString.Null
                                                            }
                                                    };

            AddData(insertQuery, sqlParameters);
        }

        public void MessagingSubscriberLog(IMessageEnvelope envelope, string subscriberType, bool isSuccess, string message, System.Exception occurredException)
        {
            if (envelope != null)
                MessagingSubscriberLog(envelope.MessageUID, envelope.MessageSentOn, subscriberType, isSuccess, message, 0, occurredException, false, false, 0, false, null, 0, ConfigurationHelper.GetEnvironment(envelope));
            else
                MessagingSubscriberLog(null, null, subscriberType, isSuccess, message, 0, occurredException, false, false, 0, false, null, 0, null);
        }
        public void DeleteMessagingPublisherLogById(string messageUID)
        {



            const string insertQuery = @"Delete from  [Ensemble].[MessagingPublisherLog] where MessageUID=@MessageUID";

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageUID",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value =messageUID
                                                            }
                                                        
                                                    };

            AddData(insertQuery, sqlParameters);
        }
        public void MessagingSubscriberLog(Guid? messageUId, DateTime? loggedOn, string subscriberType, bool isSuccess, string message, int retryCount, System.Exception occurredException,
                                           bool isFatal, bool isRetryAllowed, int retryIntervalInMs, bool isCommandMessage, string retryMessageSubscribers, int retryMessageId, string environment)
        {

            string occuredExceptionStck = occurredException != null ? ExceptionStub.CreateExceptionStubXML(occurredException) : string.Empty;

            const string insertQuery = @"INSERT INTO [Ensemble].[MessagingSubscriberLog] ([MessageUID],[LoggedOn],[SubscriberType],[IsSuccess],[Message],[RetryCount],[ExceptionType],[ExceptionStack],[IsFatal],[IsRetryAllowed],[RetryIntervalInMs],[IsCommandMessage],[RetryMessageSubscribers],[retryMessageId],[Environment]) VALUES 
                                 (@MessageUID,@LoggedOn, @SubscriberType, @IsSuccess, @Message, @RetryCount,@ExceptionType, @ExceptionStack, @IsFatal, @IsRetryAllowed, @RetryIntervalInMs, @IsCommandMessage, @RetryMessageSubscribers, @RetryMessageId,@Environment)";

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@MessageUID",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = messageUId.HasValue ? messageUId.Value.ToString() : SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@LoggedOn",
                                                                SqlDbType = SqlDbType.DateTime,
                                                                Value = loggedOn.HasValue && (loggedOn.Value != DateTime.MinValue) ?loggedOn.Value:SqlDateTime.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@SubscriberType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = subscriberType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsSuccess",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isSuccess
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Message",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = string.IsNullOrEmpty(message) ? SqlString.Null : message.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@RetryCount",
                                                                SqlDbType = SqlDbType.SmallInt,
                                                                Value = retryCount
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = occurredException != null ? occurredException.GetType().FullName : SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionStack",
                                                                SqlDbType = SqlDbType.Xml,
                                                                Value = occuredExceptionStck
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsFatal",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isFatal
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsRetryAllowed",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isRetryAllowed
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@RetryIntervalInMs",
                                                                SqlDbType = SqlDbType.Int,
                                                                Value = retryIntervalInMs
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@IsCommandMessage",
                                                                SqlDbType = SqlDbType.Bit,
                                                                Value = isCommandMessage
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@RetryMessageSubscribers",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = retryMessageSubscribers.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                        {
                                                            ParameterName = "@RetryMessageId",
                                                            SqlDbType = SqlDbType.Int,
                                                            Value = retryMessageId
                                                        }                                                        ,
                                                        new SqlParameter
                                                        {
                                                            ParameterName = "@Environment",
                                                            SqlDbType = SqlDbType.VarChar,
                                                            Value = environment!=null?environment:SqlString.Null
                                                        }
                                                    };

            AddData(insertQuery, sqlParameters);
        }

        public void MessagingInfoLog(bool isSuccess, string message, System.Exception occurredException)
        {
            MessagingInfoLog(DateTime.Now, null, null, null, null, null, message, occurredException);
        }

        public void MessagingInfoLog(DateTime? loggedOn, string logBy, string busType, string subscriberType, string publisherType, string adapterType, string message, System.Exception occurredException)
        {
            var occuredExceptionStck = occurredException != null ? ExceptionStub.CreateExceptionStubXML(occurredException) : string.Empty;

            const string insertQuery = @"INSERT INTO [MessagingInfoLog] ([LoggedOn],[LogBy],[BusType],[SubscriberType],[PublisherType],[AdapterType],[Message],[ExceptionType],[ExceptionStack]) VALUES 
                                       (@LoggedOn, @LogBy,@BusType,@SubscriberType, @PublisherType, @AdapterType,@Message,@ExceptionType, @ExceptionStack)";

            IList<SqlParameter> sqlParameters = new List<SqlParameter>
                                                    {
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@LoggedOn",
                                                                SqlDbType = SqlDbType.DateTime,
                                                                Value = loggedOn.HasValue && (loggedOn.Value != DateTime.MinValue) ?loggedOn.Value:SqlDateTime.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@LogBy",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = logBy.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@BusType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = busType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@SubscriberType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = subscriberType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@PublisherType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = publisherType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@AdapterType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = adapterType.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@Message",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = message.ToSqlString()
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionType",
                                                                SqlDbType = SqlDbType.VarChar,
                                                                Value = occurredException!=null? occurredException.GetType().FullName:SqlString.Null
                                                            },
                                                        new SqlParameter
                                                            {
                                                                ParameterName = "@ExceptionStack",
                                                                SqlDbType = SqlDbType.Xml,
                                                                Value = occuredExceptionStck
                                                            }
                                                    };

            AddData(insertQuery, sqlParameters);
        }



        /// <summary>
        /// Gets the publisher log.
        /// </summary>
        /// <param name="logFrom">The log from.</param>
        /// <param name="logTo">The log to.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public DataTable GetPublisherLog(DateTime logFrom, DateTime logTo, string messageType, string user, string context, string failonly)
        {
            var sqlCommand = new SqlCommand();

            string queryString = @"SELECT * FROM (SELECT p.MessagingPublisherLogId as [LogID], p.MessageType as [MessageType], " +
                                 @"p.MessageUID as [MessageUID], p.MessageSentOn, ISNULL(p.Topic,'') as [Context], ISNULL(p.Originator,'') as Originator , ISNULL(p.UserName,'') as UserName, p.MessageEnvelope as [Envelope], " +
                                 @"[Subscriber] = ISNULL((SELECT TOP 1 SubscriberType FROM [Ensemble].[MessagingSubscriberLog] WHERE MessageUID = p.MessageUID), 'Pending Processing'), " +
                                 @"[IsSuccess]  = ISNULL((SELECT IsSuccess  FROM [Ensemble].[MessagingSubscriberLog] WHERE MessagingSubscriberLogId = (SELECT MAX(MessagingSubscriberLogId) FROM [Ensemble].[MessagingSubscriberLog] WHERE MessageUID = p.MessageUID)), 0), " +
                                 @"[LastError]  = ISNULL((SELECT [Message] FROM [Ensemble].[MessagingSubscriberLog] WHERE MessagingSubscriberLogId = (select MAX (MessagingSubscriberLogId) from [Ensemble].[MessagingSubscriberLog] where MessageUID = p.MessageUID)), ''), " +
                                 @"[Attempts]   = (SELECT COUNT(MessageUID) FROM [Ensemble].[MessagingSubscriberLog] WHERE MessageUID = p.MessageUID), p.Environment FROM [Ensemble].[MessagingPublisherLog] as p ) as msg  " +
                                 @"WHERE CONVERT(date, msg.MessageSentOn) BETWEEN '" + String.Format("{0:yyyy/M/d HH:mm:ss}", logFrom) + "' AND '" + String.Format("{0:yyyy/M/d HH:mm:ss}", logTo) + "' AND msg.MessageType NOT LIKE '%RetryCommandMessage%' ";
            ;
            if (string.IsNullOrEmpty(messageType) == false)
            {
                queryString += @"AND msg.MessageType LIKE @MessageType ";
                sqlCommand.Parameters.Add("@MessageType", SqlDbType.VarChar).Value = "%" + messageType + "%";
            }

            if (string.IsNullOrEmpty(user) == false)
            {
                queryString += @"AND msg.UserName LIKE '%" + user + "%' ";
            }
            if (string.IsNullOrEmpty(context) == false)
            {
                queryString += @"AND msg.Context LIKE '%" + context + "%' ";
            }
            if (string.IsNullOrEmpty(failonly) == false)
            {
                if (failonly == "true")
                {
                    queryString += @"AND msg.IsSuccess = 0 ";
                }
            }
            queryString += "ORDER BY msg.MessageSentOn DESC";
            sqlCommand.CommandText = queryString;
            return GetData(sqlCommand).Tables[0];
        }



        /// <summary>
        /// Gets the publisher log details.
        /// </summary>
        /// <param name="messageUID">The message uid.</param>
        /// <returns></returns>
        public DataSet GetPublisherLogDetails(string messageUID)
        {
            SqlCommand sqlCommand = new SqlCommand();

            const string queryString = @"SELECT DISTINCT p.MessagingPublisherLogId, p.MessageUID AS [Message UID], s.SubscriberType [Subscriber], p.Environment
                                         FROM [Ensemble].[MessagingPublisherLog] AS p INNER JOIN [Ensemble].[MessagingSubscriberLog] AS s ON p.MessageUID = s.MessageUID 
                                         WHERE p.MessageUID = @MessageUID AND s.IsSuccess = 0;
                                         SELECT MessagingPublisherLogId, MessageUID AS [Message UID], MessageEnvelope AS [Envelope], Environment
                                         FROM [Ensemble].[MessagingPublisherLog]
                                         WHERE MessageUID = @MessageUID ";

            sqlCommand.Parameters.Add("@MessageUID", SqlDbType.VarChar).Value = messageUID;
            sqlCommand.CommandText = queryString;

            return GetData(sqlCommand);
        }


        /// <summary>
        /// Gets the publisher log details.
        /// </summary>
        /// <param name="messageUID">The message uid.</param>
        /// <returns></returns>
        public DataSet GetMessageLogDetails(string messageUID)
        {
            SqlCommand sqlCommand = new SqlCommand();

            const string queryString = @"SELECT MessageUID,MessageType,Originator,ReplyTo,MessageSentOn,Topic,UserName,ReplyOf,MessageEnvelope,Environment FROM [Ensemble].MessagingPublisherLog
                                        WHERE MessageUID = @MessageUID; 
                                        SELECT MessageUID,'Publisher' as TypeName,PublisherType as TypeDesc,IsSuccess,[Message],ExceptionType,ExceptionStack,Environment FROM [Ensemble].MessagingPublisherLog mpl
                                        WHERE MessageUID = @MessageUID UNION ALL 
                                        SELECT MessageUID,'Adapter' as TypeName,AdapterType as TypeDesc,IsSuccess,[Message],ExceptionType,ExceptionStack,Environment FROM [Ensemble].MessagingQueueLog mql 
                                        WHERE MessageUID = @MessageUID UNION ALL
                                        SELECT MessageUID,'Bus' as TypeName, BusType as TypeDesc,IsSuccess,[Message],ExceptionType,ExceptionStack,Environment FROM [Ensemble].MessagingHostLog mhl 
                                        WHERE MessageUID = @MessageUID;";

            sqlCommand.Parameters.Add("@MessageUID", SqlDbType.VarChar).Value = messageUID;
            sqlCommand.CommandText = queryString;

            return GetData(sqlCommand);
        }

        /// <summary>
        /// Getsubscribers the log details.
        /// </summary>
        /// <param name="messageUID">The message uid.</param>
        /// <returns></returns>
        public DataTable GetSubscriberLogDetails(string messageUID)
        {
            var sqlCommand = new SqlCommand();

            const string queryString = @"SELECT MessagingSubscriberLogId as [LogID], SubscriberType AS [Subscriber],LoggedOn,IsSuccess,CASE WHEN IsSuccess = 0 THEN [Message] ELSE '' END as [Message],
                                                RetryCount,ExceptionType,ExceptionStack,IsFatal,IsRetryAllowed,RetryIntervalInMS,Environment FROM [Ensemble].MessagingSubscriberLog
                                         WHERE MessageUID = @MessageUID";

            sqlCommand.Parameters.Add("@MessageUID", SqlDbType.VarChar).Value = messageUID;
            sqlCommand.CommandText = queryString;

            return GetData(sqlCommand).Tables[0];
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <returns></returns>
        private DataSet GetData(SqlCommand sqlCommand)
        {
            SqlConnection mySqlConnection = GetSqlConnection();
            sqlCommand.Connection = mySqlConnection;
            sqlCommand.CommandTimeout = 120;
            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter
            {
                SelectCommand = sqlCommand
            };

            DataSet myDataSet = new DataSet();
            mySqlConnection.Open();
            try
            {
                mySqlDataAdapter.Fill(myDataSet);
            }
            finally
            {
                mySqlConnection.Close();
            }
            return myDataSet;
        }

        private SqlConnection GetSqlConnection()
        {
            IMessageBusBuilder messageBuilder = new MessageBusBuilder(this.logController);
            IConfigurationFactory configFactory = messageBuilder.BuildConfigurationFactory();
            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PasdcContext"].ToString());
            return sqlConnection;
        }

        private void AddData(string updateQuery, IEnumerable<SqlParameter> sqlParameters)
        {
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand
                {
                    Connection = sqlConnection,
                    CommandType = CommandType.Text,
                    CommandText = updateQuery
                };
                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                sqlCommand.ExecuteNonQuery();
            }
        }


        void IBusLogger.LogStopwatch(string className, string methodName, Dictionary<string, object> parameters, object returnValue, TimeSpan elapsedTimeSpan)
        {
            throw new NotImplementedException();
        }

        void IBusLogger.LogPublish(IMessageEnvelope envelope, string successMessage, Type messageType)
        {
            MessagingPublisherLog(envelope, messageType.FullName, true, successMessage, null);
        }

        void IBusLogger.LogPublishFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type messageType)
        {
            try
            {
                MessagingPublisherLog(envelope, messageType.FullName, false, failureMessage, exceptionOccurred);
            }
            catch (System.Exception ex)
            {

            }
        }

        void IBusLogger.LogBusInfo(string message, Type busType)
        {
            MessagingHostLog(null, busType.FullName, true, message, null);
        }

        void IBusLogger.LogBusReceived(IMessageEnvelope envelope, string successMessage)
        {
            MessagingHostLog(envelope, null, true, successMessage, null);
        }

        void IBusLogger.LogBusReceivedFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred)
        {
            MessagingHostLog(envelope, null, false, failureMessage, exceptionOccurred);
        }

        void IBusLogger.LogAdapterInfo(string message, Type adapterType)
        {
            MessagingQueueLog(null, adapterType.FullName, true, message, null);
        }

        void IBusLogger.LogAdapterSuccess(IMessageEnvelope envelope, string successMessage, Type adapterType)
        {
            MessagingQueueLog(envelope, adapterType.FullName, true, successMessage, null);
        }

        void IBusLogger.LogAdapterFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type adapterType)
        {
            MessagingQueueLog(envelope, adapterType.FullName, true, failureMessage, exceptionOccurred);
        }

        void IBusLogger.LogInfo(string message)
        {
            MessagingInfoLog(true, message, null);
        }

        void IBusLogger.LogError(string failureMessage, System.Exception exceptionOccurred)
        {
            try
            {
                MessagingInfoLog(false, failureMessage, exceptionOccurred);
            }
            catch
            {
                Thread.Sleep(2000);
                try
                {
                    MessagingInfoLog(false, failureMessage, exceptionOccurred);
                }
                catch { }
            }
        }

        void IBusLogger.LogSubscribeInfo(string message, Type busType)
        {
            MessagingSubscriberLog(null, busType.FullName, true, message, null);
        }

        void IBusLogger.LogSubscribeSuccess(IMessageEnvelope envelope, string successMessage, Type subscriberType)
        {
            MessagingSubscriberLog(envelope, subscriberType.FullName, true, null, null);
        }

        void IBusLogger.LogSubscribeFailure(IMessageEnvelope envelope, string failureMessage, System.Exception exceptionOccurred, Type subscriberType)
        {
            MessagingSubscriberLog(envelope, subscriberType.FullName, false, failureMessage, exceptionOccurred);
        }
        #endregion

    }

    public static class SqlExtensions
    {
        #region Public Methods
        public static SqlString ToSqlString(this string source)
        {
            return string.IsNullOrEmpty(source) ? SqlString.Null : source;
        }

        public static SqlDateTime ToSqlDateTime(this DateTime source)
        {
            return source == DateTime.MinValue ? SqlDateTime.Null : source;
        }
        #endregion
    }
}

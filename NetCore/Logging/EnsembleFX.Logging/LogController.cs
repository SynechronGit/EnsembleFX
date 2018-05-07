using EnsembleFX.Logging.Configuration;
using EnsembleFX.Logging.Entities;
using EnsembleFX.Logging.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Logging
{
    /// <summary>
    /// Single Entry point to control CoreVelocity wide logging
    /// </summary>
    public class LogController : ILogController
    {
        #region Private Members

        private ILogger loggerInstance;
        private static ConcurrentQueue<ApplicationLogs> logItemsQueue;
        private static Task processQueueTask;
        private bool isInitialized = false;
        
        Dictionary<int, ILogger> targetLoggers;
        private int loggerSequence = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the LogController to target multiple configured loggers
        /// </summary>
        public LogController()
        {

        }
        #endregion

        #region Internal Members

        internal List<LoggerConfigurationSetting> LoggerSettings = new List<LoggerConfigurationSetting>();
        internal List<ILogger> TargetLoggers = new List<ILogger>();

        internal static ConcurrentQueue<ApplicationLogs> LogItems
        {
            get
            {
                return logItemsQueue;
            }
            set
            {
                if (value != logItemsQueue)
                {
                    logItemsQueue = value;
                }
            }
        }

        #endregion

        #region Internal Properties

        internal int MaxQueueSize
        {
            get;
            set;
        }

        internal int SleepInterval
        {
            get;
            set;
        }

        internal string LoggingMode
        {
            get;
            set;
        }

        internal LoggersConfigurationSection CurrentLoggersConfiguration
        {
            get
            {
                return LoggersConfigurationSection.GetCurrent();
            }
        }

        internal LoggingConfigurationSection CurrentLoggingConfiguration
        {
            get
            {
                return LoggingConfigurationSection.GetCurrent();
            }
        }


        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes target loggers in mode determined by configuration
        /// </summary>
        internal void Initialize()
        {
            logItemsQueue = new ConcurrentQueue<ApplicationLogs>();            
            targetLoggers = new Dictionary<int, ILogger>();

            // LogController Level settings
            LoggingConfigurationSetting loggingSetting = CurrentLoggingConfiguration.LoggingConfigurationSetting;
            if (loggingSetting != null)
            {
                this.LoggingMode = loggingSetting.Mode;
                this.SleepInterval = loggingSetting.SleepInterval;
                this.MaxQueueSize = loggingSetting.MaxQueueSize;
            }

            // Logger Level settings
            LoggerSettings = CurrentLoggersConfiguration.LoggerConfigurationSettings.Cast<LoggerConfigurationSetting>().ToList();

            try
            {
                // Setup Target loggers from configuration ordered by PriorityOrder
                foreach (LoggerConfigurationSetting configuredLogger in LoggerSettings.OrderBy(o => Convert.ToInt32(o.PriorityOrder)))
                {
                    loggerSequence++;
                    //loggerInstance = dependencyManager.Resolve<ILogger>(configuredLogger.TypeName);
                    loggerInstance.LoggerParameters = configuredLogger.Parameters;
                    loggerInstance.LoggerPriority = Convert.ToInt32(configuredLogger.PriorityOrder);
                    targetLoggers.Add(loggerSequence, loggerInstance);
                }
            }
            catch (Exception)
            {
                // Do Nothing if logger setup fails
            }

            // If using Asynchronous processing, use another thread for Async Queue processing
            if (LoggingMode.Trim().ToUpperInvariant() == LogMode.Asynchronous.ToString().Trim().ToUpperInvariant())
            {
                processQueueTask = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        ProcessFromQueue();
                        Thread.Sleep(SleepInterval);
                    }
                }, new CancellationToken(), TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            isInitialized = true;
        }

        /// <summary>
        /// If configuration is set to Async, adds log items to the processing queue
        /// </summary>
        /// <param name="item">LogItem to log</param>
        internal void AddToQueue(ApplicationLogs item)
        {
            try
            {
                if (LogItems.Count < MaxQueueSize)
                {
                    LogItems.Enqueue(item);
                }
            }
            catch (Exception)
            {
                // Do nothing if add to Queue failed
            }
        }

        /// <summary>
        /// Processes LogItems asyncronously from the queue
        /// </summary>
        internal void ProcessFromQueue()
        {
            ApplicationLogs logItem = null;
            while (LogItems.TryDequeue(out logItem))
            {
                if (logItem != null)
                {
                    // Target loggers are already ordered by PriorityOrder
                    foreach (KeyValuePair<int, ILogger> keyValuePair in targetLoggers.OrderBy(o => o.Key))
                    {
                        try
                        {
                            keyValuePair.Value.Log(logItem);
                            break; // If no exception stop targetting the remaining loggers
                        }
                        catch (Exception)
                        {
                            continue; // If exception occured in the higher priority logger, process next logger
                        }
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Passes the LogItem to all configured loggers for logging
        /// </summary>
        /// <param name="item">LogItem to log</param>
        public void Log(ApplicationLogs item)
        {
            try
            {
                if (!isInitialized)
                {
                    Initialize();
                }

                if (LoggingMode.Trim().ToUpperInvariant() == LogMode.Asynchronous.ToString().Trim().ToUpperInvariant())
                {
                    AddToQueue(item);
                }
                else
                {
                    foreach (KeyValuePair<int, ILogger> keyValuePair in targetLoggers)
                    {
                        try
                        {
                            keyValuePair.Value.Log(item);
                            break; // If no exception stop targetting the remaining loggers
                        }
                        catch (Exception)
                        {
                            continue; // If exception occured in the higher priority logger, process next logger
                        }
                    }
                }
            }
            catch (Exception)
            {
                // The exception throw is suppressed so that applications dont get runtime errors 
                // if logging fails for any reason
            }

        }

        #endregion

    }
}

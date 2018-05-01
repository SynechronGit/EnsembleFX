using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnsembleFX.Logging.Enums;
using log4net;
using log4net.Config;
using EnsembleFX.Logging.Entities;

namespace EnsembleFX.Logging.Loggers
{
    /// <summary>
    /// Wraps log4net logging functionality to CoreVelocity standard interface ILogger
    /// </summary>
    public class Log4NetLogger : ILogger
    {

        #region Private Members

        private ILog logger = null;
        private bool isInitalized = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Default parameterless constructor
        /// Initializes Log4NetLogger
        /// </summary>
        public Log4NetLogger()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the logger name
        /// </summary>
        public string LoggerName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Logger specific configuration parameters
        /// </summary>
        public string LoggerParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Logger execution priority, logger with higher priority is tried first
        /// </summary>
        public int LoggerPriority
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Checks if logging is enabled for the loglevel
        /// </summary>
        internal bool IsLogLevelEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return logger.IsDebugEnabled;
                case LogLevel.Error:
                    return logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return logger.IsFatalEnabled;
                case LogLevel.Info:
                    return logger.IsInfoEnabled;
                case LogLevel.Warn:
                    return logger.IsWarnEnabled;
                default:
                    return true;
            }
        }

        #endregion

        #region ILogger Implementation

        /// <summary>
        /// Initializes the Log4Net Logger
        /// </summary>
        public void Initialize()
        {
            if (!String.IsNullOrEmpty(LoggerParameters))
            {
                FileInfo configFileInfo = new FileInfo(LoggerParameters);
                XmlConfigurator.Configure(configFileInfo); // uses external config
            }
            else
            {
                XmlConfigurator.Configure(); // uses app.config
            }

            logger = log4net.LogManager.GetLogger(LoggerName);
            if (logger != null)
            {
                isInitalized = true;
            }
        }

        /// <summary>
        /// Logs the given message. Output depends on the associated
        /// log4net configuration.
        /// </summary>
        /// <param name="item">A <see cref="ApplicationLogs"/> which encapsulates
        /// information to be logged.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="item"/>
        /// is a null reference.</exception>
        public void Log(ApplicationLogs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!isInitalized)
            {
                Initialize();
            }

            item.LoggerName = this.LoggerName;

            switch (item.LogLevel)
            {
                case LogLevel.Fatal:
                    if (IsLogLevelEnabled(item.LogLevel))
                    {
                        logger.Fatal(item.Message, item.Exception);
                    }
                    break;

                case LogLevel.Error:
                    if (IsLogLevelEnabled(item.LogLevel))
                    {
                        logger.Error(item.Message, item.Exception);
                    }
                    break;

                case LogLevel.Warn:
                    if (IsLogLevelEnabled(item.LogLevel))
                    {
                        logger.Warn(item.Message, item.Exception);
                    }
                    break;

                case LogLevel.Info:
                    if (IsLogLevelEnabled(item.LogLevel))
                    {
                        logger.Info(item.Message, item.Exception);
                    }
                    break;

                case LogLevel.Debug:
                    if (IsLogLevelEnabled(item.LogLevel))
                    {
                        logger.Debug(item.Message, item.Exception);
                    }
                    break;

                default:
                    logger.Info(item.Message, item.Exception);
                    break;
            }
        }

        #endregion

    }
}

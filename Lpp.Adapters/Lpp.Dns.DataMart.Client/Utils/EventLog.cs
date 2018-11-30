using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Core;
using System.Xml;

namespace Lpp.Dns.DataMart.Client.Utils
{
    public class EventLogFilterAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            string message = loggingEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff") + " [" + loggingEvent.ThreadName + "] " + loggingEvent.Level + " " + loggingEvent.LoggerName + ": " + loggingEvent.RenderedMessage + " at " + loggingEvent.LocationInformation.FullInfo + "\r\n";
            // Send this to the portal via a web api call
            Program.logWatcher.LogEvent(loggingEvent);    
        }
    }

    public class LogWatcher
    {
        private System.Collections.Generic.Queue<string> events = new Queue<string>();
        public String DataMartClientId { get; set; }

        public LogWatcher()
        {
        }

        public LogWatcher(String LogLevel, String LogFilePath, String InstanceId)
        {
            DataMartClientId = InstanceId;
            SetLogLevel(LogLevel);
            SetLogFilePath(LogFilePath);
        }

        public void SetLogFilePath(string LogFilePath)
        {
            log4net.Repository.Hierarchy.Hierarchy hierachy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
            foreach (IAppender appender in hierachy.GetAppenders())
            {
                if (appender is RollingFileAppender)
                {
                    string filePath = Path.Combine(LogFilePath == null || LogFilePath == string.Empty ? Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"PopMedNet","PMN","logs") : LogFilePath, "DataMartClient.txt");
                    if (filePath.ToLower() != ((RollingFileAppender)appender).File.ToLower())
                    {
                        ((RollingFileAppender)appender).File = filePath;
                        ((RollingFileAppender)appender).ActivateOptions();
                        hierachy.Configured = true;
                        hierachy.RaiseConfigurationChanged(EventArgs.Empty);
                    }
                }
            }
        }

        public void SetLogLevel(string logLevel)
        {
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap[logLevel.ToUpper()];
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    ((log4net.Repository.Hierarchy.Logger)logger).Level = hier.LevelMap[logLevel.ToUpper()];
                }

                hier.Root.Level = GetLevel(logLevel);
            }

            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            rootLogger.Level = h.LevelMap[logLevel.ToUpper()];
        }

        private Level GetLevel(string logLevel)
        {
            string level = logLevel.ToUpper();
            if (level == Level.Error.Name)
                return Level.Error;
            else if (level == Level.Info.Name)
                return Level.Info;
            else 
                return Level.Debug;
        }

        public void LogEvent(LoggingEvent Event)
        {
            try
            {
                LogEvent logEvent = new Utils.LogEvent();
                logEvent.Event = Event;
                logEvent.DataMartClientId = DataMartClientId;
            }
            catch {}
        }

        public IList<string> GetAllEvents
        {
            get { return events.ToList(); }
        }

        public string GetNextEvent
        {
            get { return events.Dequeue(); }
        }

        public void ClearEvents()
        {
            events.Clear();
        }

    }

}
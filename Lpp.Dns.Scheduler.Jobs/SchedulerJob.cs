using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading;
using Common.Logging;
using Quartz;
using Lpp.Dns.Scheduler;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace Lpp.Dns.Scheduler.Jobs
{
    /// <summary>
    /// A sample job that just prints info on console for demostration purposes.
    /// </summary>
    public class SchedulerJob : IJob
    {
        public const string RequestIdKey    =   "RequestId";
        public const string UserIdKey       =   "UserId";
        public const string HostKey         =   "Host";
        public const int TicksPerSecond = 10000000;

        private static readonly ILog logger = LogManager.GetLogger(typeof(SchedulerJob));

        /// <summary>
        /// Called by the <see cref="IScheduler" /> when a <see cref="ITrigger" />
        /// fires that is associated with the <see cref="IJob" />.
        /// </summary>
        /// <remarks>
        /// The implementation may wish to set a  result object on the 
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to 
        /// <see cref="IJobListener" />s or 
        /// <see cref="ITriggerListener" />s that are watching the job's 
        /// execution.
        /// </remarks>
        /// <param name="context">The execution context.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
        public void Execute(IJobExecutionContext context)
        {
            if ( context == null ) return;

            string requestId = (string) context.JobDetail.JobDataMap.Get(RequestIdKey);
            string userId = (string) context.JobDetail.JobDataMap.Get(UserIdKey);
            string host = (string)context.JobDetail.JobDataMap.Get(HostKey);

            logger.Info("SchedulerJob " + requestId + " running for User " + userId + " on host " + host + "...");
            string url = Properties.Settings.Default.SchedulerServiceUrl;
            SettingsPropertyCollection props = Properties.Settings.Default.Properties;
            foreach( SettingsProperty prop in props )
            {
                string propertyName = prop.Name;
                if (propertyName.EndsWith("SchedulerServiceUrl", StringComparison.OrdinalIgnoreCase))
                {
                    logger.Debug("Host: " + propertyName + ", ServiceUrl: " + prop.DefaultValue);
                    if ( propertyName.StartsWith( host.Replace( '.', '_' ), StringComparison.OrdinalIgnoreCase ) )
                      url = (string)prop.DefaultValue;
                }
            }
            logger.Info("Making request to: " + url + "...");
            using (var factory = new ChannelFactory<ISchedulerService>(new BasicHttpBinding() { SendTimeout = new TimeSpan(0,5,0) }, url))
            {
                var channel = factory.CreateChannel();
                try
                {
                    channel.SubmitSchedulerRequest(userId, requestId);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }

            logger.Info("SchedulerJob " + requestId + " run finished.");
        }
    }
}
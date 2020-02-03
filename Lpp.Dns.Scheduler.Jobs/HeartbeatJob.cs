using System;
using System.Threading;
using Common.Logging;
using Quartz;

namespace Lpp.Dns.Scheduler.Jobs
{
    /// <summary>
    /// A sample job that just prints info on console for demostration purposes.
    /// </summary>
    public class HeartbeatJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HeartbeatJob));

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
        public void Execute(IJobExecutionContext context)
        {
            logger.Info("HeartbeatJob running...");
            Thread.Sleep(TimeSpan.FromSeconds(5));
            logger.Info("HeartbeatJob run finished.");
        }
    }
}
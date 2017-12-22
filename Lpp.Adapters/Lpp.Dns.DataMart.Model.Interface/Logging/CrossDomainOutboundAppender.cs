using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Logging
{
    public class CrossDomainOutboundAppender : AppenderSkeleton
    {
        readonly CrossDomainParentAppender ParentAppender;

        public CrossDomainOutboundAppender(CrossDomainParentAppender parentAppender)
        {
            if (parentAppender == null)
            {
                throw new ArgumentNullException("parentAppender", "Parent appender cannot be null.");
            }

            ParentAppender = parentAppender;
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            LoggingEvent copied = new LoggingEvent(loggingEvent.GetLoggingEventData());
            ParentAppender.Append(copied);
        }
    }
}

using log4net.Core;
using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Logging
{
    [Serializable]
    public class CrossDomainParentAppender : MarshalByRefObject
    {
        public void Append(LoggingEvent evt)
        {
            foreach (IAppender appender in LogManager.GetRepository().GetAppenders())
            {
                appender.DoAppend(evt);
            }
        }
    }
}

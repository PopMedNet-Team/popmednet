using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Logging
{
    [Serializable]
    public class CrossDomainChildLoggingSetup : MarshalByRefObject
    {
        public void ConfigureAppender(CrossDomainParentAppender parentAppender, log4net.Core.Level level)
        {
            CrossDomainOutboundAppender outboundAppender = new CrossDomainOutboundAppender(parentAppender);
            outboundAppender.Threshold = level;
            log4net.Config.BasicConfigurator.Configure(outboundAppender);
        }
    }
}

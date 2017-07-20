using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Logging
{
    [Serializable]
    public class CrossDomainChildLoggingSetup : MarshalByRefObject
    {
        public void ConfigureAppender(CrossDomainParentAppender parentAppender)
        {
            CrossDomainOutboundAppender outboundAppender = new CrossDomainOutboundAppender(parentAppender);
            log4net.Config.BasicConfigurator.Configure(outboundAppender);
        }
    }
}

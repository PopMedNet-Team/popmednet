
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class QueryJSONTests
    {
        const string ResourceFolder = "../Resources/QueryComposition";
        static readonly log4net.ILog Logger;

        static QueryJSONTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(QueryJSONTests));
        }
    }
}

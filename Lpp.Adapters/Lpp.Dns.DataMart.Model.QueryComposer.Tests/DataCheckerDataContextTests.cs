using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class DataCheckerDataContextTests
    {
        static readonly string ConnectionString;

        static DataCheckerDataContextTests()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataChecker"].ConnectionString;
        }

        [TestMethod]
        public void ConfirmTables_SqlServer()
        {
            using (var db = Helper.CreateDataCheckerDataContext(Model.Settings.SQLProvider.SQLServer, ConnectionString))
            {
                var diagnosis = db.Diagnoses.FirstOrDefault();
                var hispanic = db.Hispanics.FirstOrDefault();
                var metadata = db.Metadatas.FirstOrDefault();
                var ndcs = db.NDCs.FirstOrDefault();
                var pdx = db.PDXs.FirstOrDefault();
                var procedure = db.Procedures.FirstOrDefault();
                var race = db.Races.FirstOrDefault();
                var rxamt = db.RXAmts.FirstOrDefault();
                var rxsup = db.RXSups.FirstOrDefault();
                var age = db.Ages.FirstOrDefault();
                var height = db.Heights.FirstOrDefault();
                var sex = db.Sexes.FirstOrDefault();
                var weight = db.Weights.FirstOrDefault();
            }

        }
    }
}

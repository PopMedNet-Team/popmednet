using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using Lpp.Dns.Data;

namespace Lpp.Dns.Api.Tests.DataMartClient
{
    [TestClass]
    public class DistributedRegressionTrackingTableProcessorTests
    {
        [TestMethod]
        public void ParseDataPartnerTrackingTableToJson()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (var reader = new StreamReader(File.OpenRead("../DataMartClient/DataPartnerTrackingTable.csv")))
            using(var writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.QuoteName = true;

                writer.WriteStartArray();

                string line = reader.ReadLine();

                string[] columns = line.Split(',').Select(c => c.Trim()).ToArray();

                while (reader.EndOfStream == false)
                {
                    writer.WriteStartObject();

                    line = reader.ReadLine();

                    string[] split = line.Split(',');
                    for (int i = 0; i < columns.Length; i++)
                    {
                        writer.WritePropertyName(columns[i]);
                        writer.WriteValue(split[i].Trim());
                    }

                    writer.WriteEnd();                    
                }

                writer.WriteEndArray();
                writer.Flush();

                Console.WriteLine(sb.ToString());
            }

        }

        [TestMethod]
        public void ParseAnalysisCenterTrackingTableToJson()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (var reader = new StreamReader(File.OpenRead("../DataMartClient/AnalysisCenterTrackingTable.csv")))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                writer.QuoteName = true;

                writer.WriteStartArray();

                string line = reader.ReadLine();

                string[] columns = line.Split(',').Select(c => c.Trim()).ToArray();

                while (reader.EndOfStream == false)
                {
                    writer.WriteStartObject();

                    line = reader.ReadLine();

                    string[] split = line.Split(',');
                    for (int i = 0; i < columns.Length; i++)
                    {
                        writer.WritePropertyName(columns[i]);
                        writer.WriteValue(split[i].Trim());
                    }

                    writer.WriteEnd();
                }

                writer.WriteEndArray();
                writer.Flush();

                Console.WriteLine(sb.ToString());
            }
        }

    }
}

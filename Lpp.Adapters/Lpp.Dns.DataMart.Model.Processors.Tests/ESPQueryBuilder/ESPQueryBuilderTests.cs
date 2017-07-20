using Lpp.Dns.DataMart.Model.ESPQueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Lpp.Dns.DataMart.Model.Processors.Tests.ESPQueryBuilder
{
    [TestClass]
    public class ESPQueryBuilderTests
    {
        [TestMethod]
        public void QueryDemographics()
        {
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ESP"].ConnectionString;
            string connectionString = "Server=localhost;Port=5432;User Id=esp_mdphnet;Password=esp_mdphnet;Database=ESP;Timeout=15;CommandTimeout=120";
            using (var db = Lpp.Dns.DataMart.Model.ESPQueryBuilder.DataContext.Create(connectionString))
            {
                var results = db.Demographics.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.CenterID);
                }

                DataTable dt = new DataTable();

                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.Hispanic, r.PatID, r.Race }, LoadOption.OverwriteChanges));


                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void ConfirmMappings()
        {
            //            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ESP"].ConnectionString;
            string connectionString = "Server=localhost;Port=5432;User ID=esp_mdphnet;Password=esp_mdphnet;Database=esp;Timeout=30;CommandTimeout=600";
            using (DataContext db = Lpp.Dns.DataMart.Model.ESPQueryBuilder.DataContext.Create(connectionString))
            {
                var demographic = db.Demographics.First();
                var diagnosis = db.Diagnosis.First();
                var diagnosis3 = db.DiagnosisICD9_3digit.First();
                var diagnosis4 = db.DiagnosisICD9_4digit.First();
                var diagnosis5 = db.DiagnosisICD9_5digit.First();
                var diseases = db.Diseases.First();
                var encounter = db.Encounters.First();
                var IliSummary = db.IliSummaries.First();
                var uvt_ag10 = db.UVT_AgeGroup10yr.First();
                var uvt_ag5 = db.UVT_AgeGroup5yr.First();
                var uvt_agms = db.UVT_AgeGroupMS.First();
                var uvt_center = db.UVT_Center.First();
                var uvt_detectedcondition = db.UVT_DetectedCondition.First();
                var uvt_detectedcriteria = db.UVT_DetectedCriteria.First();
                var uvt_detectedstatus = db.UVT_DetectedStatus.First();
                var uvt_dx = db.UVT_Dx.First();
                var uvt_dx3 = db.UVT_Dx3Digit.First();
                var uvt_dx4 = db.UVT_Dx4Digit.First();
                var uvt_dx5 = db.UVT_Dx5Digit.First();
                var uvt_encounter = db.UVT_Encounter.First();
                var uvt_period = db.UVT_Period.First();
                var uvt_provider = db.UVT_Provider.First();
                var uvt_race = db.UVT_Race.First();
                var uvt_sex = db.UVT_Sex.First();
                var uvt_site = db.UVT_Site.First();
                var uvt_zip5 = db.UVT_Zip5.First();
            }
        }

        [TestMethod]
        public void ProjectableQueryTransformWithZip()
        {
            XPathDocument xml = new XPathDocument(File.OpenRead(@"..\ESPQueryBuilder\ESPRequest-includeZipProjection.xml"));
            XslCompiledTransform xslt = new XslCompiledTransform();

            using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.QueryComposer.xsl"))
            {
                using (XmlTextReader transform = new XmlTextReader(stream))
                {
                    using (var writer = new StringWriter())
                    {
                        xslt.Load(transform);
                        xslt.Transform(xml, null, writer);
                        var query = writer.ToString();
                        //log.Debug(query);
                        Console.WriteLine(query);
                    }
                }
            }
        }

        [TestMethod]
        public void ProjectableQueryTransformWithoutZip()
        {
            XPathDocument xml = new XPathDocument(File.OpenRead(@"..\ESPQueryBuilder\ESPRequest-noZipProjection.xml"));
            XslCompiledTransform xslt = new XslCompiledTransform();

            using (Stream stream = typeof(ESPQueryBuilderModelProcessor).Assembly.GetManifestResourceStream("Lpp.Dns.DataMart.Model.ESPQueryBuilder.QueryComposer.xsl"))
            {
                using (XmlTextReader transform = new XmlTextReader(stream))
                {
                    using (var writer = new StringWriter())
                    {
                        xslt.Load(transform);
                        xslt.Transform(xml, null, writer);
                        var query = writer.ToString();
                        //log.Debug(query);
                        Console.WriteLine(query);
                    }
                }
            }
        }


    }
}

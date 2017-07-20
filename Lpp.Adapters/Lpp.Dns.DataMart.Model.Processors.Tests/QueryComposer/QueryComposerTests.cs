using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DataMart.Model.QueryComposer;

namespace Lpp.Dns.DataMart.Model.Processors.Tests.QueryComposer
{
    [TestClass]
    public class QueryComposerTests
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ClassInitialize]
        public static void StartUp(TestContext context)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        }

        [TestMethod]
        public void QueryComposer1()
        {
            var processor = new Lpp.Dns.DataMart.Model.QueryComposerModelProcessor();
            processor.Settings = new Dictionary<string, object> {
                {"ModelID", new Guid("7C69584A-5602-4FC0-9F3F-A27F329B1113")},
                {"Server", "localhost"},
                {"UserID", "esp_mdphnet"},
                {"Password", "esp_mdphnet"},
                {"Database", "esp"},
                {"DataProvider", "PostgreSQL"},
                {"ConnectionTimeout", "240"},
                {"CommandTimeout", "0"}
            };

            byte[] requestDocumentContent = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(Create_ESP_1_WithExclusion()));

            System.IO.File.WriteAllBytes("request.json", requestDocumentContent);

            IDictionary<string, string> properties;
            Document[] documents;

            processor.Request("1",
                new NetworkConnectionMetadata
                {
                    OrganizationName = "Test Org",
                    UserEmail = "test@test.com",
                    UserFullName = "Administrator",
                    UserLogin = "administrator"
                },
                new RequestMetadata
                {
                    DataMartId = "dm1",
                    DataMartName = "Test DataMart",
                    DataMartOrganizationId = "1",
                    DataMartOrganizationName = "Test Org",
                    IsMetadataRequest = false,
                    RequestTypeId = ""
                },
                new Document[] {
                    new Document("1", "text/json", "request.json"){ IsViewable = false, Size = requestDocumentContent.Length }
                },
                out  properties,
                out documents);

            using (var ms = new System.IO.MemoryStream(requestDocumentContent))
            {
                processor.RequestDocument("1", "1", ms);
            }

            processor.Start("1");

            Document[] responseDocuments = processor.Response("1");

            System.IO.Stream responseStream = null;
            try{
                processor.ResponseDocument("1", "", out responseStream, int.MaxValue);

                string json;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    json = reader.ReadToEnd();                    
                }
                System.IO.File.WriteAllText("response.json", json, System.Text.Encoding.UTF8);

                Console.WriteLine(json);
            }
            finally
            {
                if(responseStream != null)
                    responseStream.Dispose();
            }
        }


        QueryComposerRequestDTO Create_ESP_1()
        {
            QueryComposerRequestDTO request = new QueryComposerRequestDTO
            {
                ID = Guid.NewGuid(),
                Timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks),
                Header = new QueryComposerHeaderDTO
                {
                    Name = "Example Request",
                    Description = "This is a unit test request."
                },
                Where = new QueryComposerWhereDTO
                {
                    Criteria = new QueryComposerCriteriaDTO[] 
                    {                        
                        new QueryComposerCriteriaDTO
                        {
                            Name = "Group 1",
                            Operator = DTO.Enums.QueryComposerOperators.And,
                            Exclusion = false,
                            Terms = new QueryComposerTermDTO[]
                            {
                                new QueryComposerTermDTO
                                { 
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                                    Values = new Dictionary<string,object>{ {"MinAge", 0}, {"MaxAge", 65 } }
                                },
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.VisitsID,
                                    Values = new Dictionary<string,object>{ { "Visits", 2 } }
                                },
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.GenderID,
                                //    Values = new Dictionary<string,object>{ { "Gender", "Male and Female" } }
                                //},
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                    //Values = new Dictionary<string,object>{ { "Codes", new List<string>{"172", "079", "250"} } }
                                    //Values = new Dictionary<string,object>{ { "Codes", "172, 079 ,250"} }
                                    Values = new Dictionary<string,object>{ { "Codes", "172"} }
                                },
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ObservationPeriodID,
                                    Values = new Dictionary<string,object>{ { "Start", "2008-01-01" }, {"End", "2008-01-01" } }
                                },
                                //Not sure how condictions is going to work since it is being implemented against an enum
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.ConditionsID,
                                //    Values = new Dictionary<string,object>{ { "Conditions", 1 } }
                                //},
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.ZipCodeID,
                                //    Values = new Dictionary<string,object>{ { "Codes", "02125,02127,02211"} }
                                //},

                            },
                            //ObservationPeriod = new QueryComposerPeriodDTO{ Start = new DateTimeOffset(new DateTime(2008,1,1)), End = DateTimeOffset.Now }
                        }
                    }
                }
            };

            return request;
        }

        QueryComposerRequestDTO Create_ESP_1_WithExclusion()
        {
            QueryComposerRequestDTO request = new QueryComposerRequestDTO
            {
                ID = Guid.NewGuid(),
                Timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks),
                Header = new QueryComposerHeaderDTO
                {
                    Name = "Example Request",
                    Description = "This is a unit test request."
                },
                Where = new QueryComposerWhereDTO
                {
                    Criteria = new QueryComposerCriteriaDTO[] 
                    {                        
                        new QueryComposerCriteriaDTO
                        {
                            Name = "Group 1",
                            Operator = DTO.Enums.QueryComposerOperators.And,
                            Exclusion = false,
                            Terms = new QueryComposerTermDTO[]
                            {
                                new QueryComposerTermDTO
                                { 
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                                    Values = new Dictionary<string,object>{ {"MinAge", 0}, {"MaxAge", 65 } }
                                },
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.VisitsID,
                                    Values = new Dictionary<string,object>{ { "Visits", 2 } }
                                },
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.GenderID,
                                //    Values = new Dictionary<string,object>{ { "Gender", "Male and Female" } }
                                //},
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                    //Values = new Dictionary<string,object>{ { "Codes", new List<string>{"172", "079", "250"} } }
                                    //Values = new Dictionary<string,object>{ { "Codes", "172, 079 ,250"} }
                                    Values = new Dictionary<string,object>{ { "Codes", "172"} }
                                },
                                //Not sure how condictions is going to work since it is being implemented against an enum
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.ConditionsID,
                                //    Values = new Dictionary<string,object>{ { "Condition", 1 } }
                                //},
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.ZipCodeID,
                                //    Values = new Dictionary<string,object>{ { "Codes", "02125,02127,02211"} }
                                //},
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ObservationPeriodID,
                                    Values = new Dictionary<string,object>{ { "Start", "2008-01-01" }, {"End", "2008-01-01" } }
                                },

                            },
                            //ObservationPeriod = new QueryComposerPeriodDTO{ Start = new DateTimeOffset(new DateTime(2008,1,1)), End = DateTimeOffset.Now }
                        },
                        new QueryComposerCriteriaDTO
                        {
                            Name = "Exclusion Group",
                            Operator = DTO.Enums.QueryComposerOperators.And,
                            Exclusion = true,
                            Terms = new QueryComposerTermDTO[]
                            {
                                //new QueryComposerTermDTO
                                //{ 
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                                //    Values = new Dictionary<string,object>{ {"MinAge", 5}, {"MaxAge", 15 } }
                                //},
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.VisitsID,
                                //    Values = new Dictionary<string,object>{ { "Visits", 5 } }
                                //},
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.GenderID,
                                //    Values = new Dictionary<string,object>{ { "Gender", "Male and Female" } }
                                //},
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                    //Values = new Dictionary<string,object>{ { "Codes", new List<string>{"172", "079", "250"} } }
                                    //Values = new Dictionary<string,object>{ { "Codes", "172, 079 ,250"} }
                                    Values = new Dictionary<string,object>{ { "Codes", "179"} }
                                },
                                //Not sure how condictions is going to work since it is being implemented against an enum
                                //new QueryComposerTermDTO
                                //{
                                //    Operator = DTO.Enums.QueryComposerOperators.And,
                                //    Type = Lpp.QueryComposer.ModelTermsFactory.ConditionsID,
                                //    Values = new Dictionary<string,object>{ { "Condition", 1 } }
                                //},
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ZipCodeID,
                                    Values = new Dictionary<string,object>{ { "Codes", "02125,02127,02211"} }
                                },
                                new QueryComposerTermDTO
                                {
                                    Operator = DTO.Enums.QueryComposerOperators.And,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.ObservationPeriodID,
                                    Values = new Dictionary<string,object>{ { "Start", "2009-01-01" }, {"End", "2011-01-01" } }
                                },

                            },
                            //ObservationPeriod = new QueryComposerPeriodDTO{ Start = new DateTimeOffset(new DateTime(2009,1,1)), End = new DateTimeOffset(new DateTime(2011,1,1)) }
                        }
                    }
                }
            };

            return request;
        }


        [TestMethod]
        public void QueryComposer_BuildRequest()
        {

            QueryComposerRequestDTO request = new QueryComposerRequestDTO
            {
                ID = Guid.NewGuid(),
                Timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks),
                Header = new QueryComposerHeaderDTO
                {
                    Name = "Example Request",
                    Description = "This is a unit test request."
                },
                Where = new QueryComposerWhereDTO
                {
                    Criteria = new QueryComposerCriteriaDTO[] 
                    {
                        new QueryComposerCriteriaDTO
                        {
                            Name = "Group 1",
                            Operator = DTO.Enums.QueryComposerOperators.And,
                            Exclusion = false,
                            Terms = new QueryComposerTermDTO[]
                            {
                                new QueryComposerTermDTO
                                { 
                                    Operator = DTO.Enums.QueryComposerOperators.Or,
                                    Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                                    Values = new Dictionary<string,object>{ {"Gender", 0} }
                                }
                            }
                        }
                    }
                }
            };


            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(request, Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine(serialized);

        }

        [TestMethod]
        public void QueryComposer_BuildResponse()
        {
            Console.WriteLine(DateTime.Now);

            var results = new Dictionary<string, object>{
                {"FieldName1", "Value"},
                { "FieldName2", "Value2" },
                {"Results", new List<Dictionary<string,object>>{ new Dictionary<string,object>{ {"InnerField1", "Value1"} } }}
            };

            QueryComposerResponseDTO response = new QueryComposerResponseDTO
            {
                RequestID = Guid.NewGuid(),
                Errors = new[] { new QueryComposerResponseErrorDTO { Code = "234233", Description = "Error description here" } },
                ResponseDateTime = DateTime.UtcNow,
                Results = new [] { new [] { results }}
            };

            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(response, Newtonsoft.Json.Formatting.Indented);

            Console.WriteLine(serialized);
        }

        [TestMethod]
        public void DeserializeRequestAndParseTermValues()
        {

            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;

            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(Samples.MFU_Dispensing_Query, jsonSettings);
            var criteria = request.Where.Criteria.First();

            var term = criteria.Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.QuarterYearID);
            string start = term.GetStringValue("StartYear");
            Console.WriteLine(start);

            var enumTerm = criteria.Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DispensingMetricID);
            DTO.Enums.DispensingMetric metricValue;
            if (enumTerm.GetEnumValue("Metric", out metricValue))
                Console.WriteLine(metricValue + " (" + metricValue.ToString("D") + ", " + enumTerm.GetStringValue("Metric") + ")");
            else
                Assert.Fail("Didn't parse metric value correctly.");

        }

        [TestMethod]
        public void DeserializeIncidenceICD9_Diagnosis_Query()
        {
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;

            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(Samples.Incidence_ICD9_Diagnosis_Query, jsonSettings);
            var criteria = request.Where.Criteria.First();

            var term = criteria.Terms.FirstOrDefault(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID);

            var c2 = term.GetCodeStringCollection();

            foreach (string code in c2)
            {
                Console.WriteLine(code);
            }

            var field = request.Select.Fields.First(f => f.Type == Lpp.QueryComposer.ModelTermsFactory.SexID);

            object groupBy = field.GroupBy;

            DTO.Enums.SexStratifications ss =(DTO.Enums.SexStratifications)Enum.Parse(typeof(DTO.Enums.SexStratifications), groupBy.ToString());
            Console.WriteLine(ss.ToString());

        }

    }
}

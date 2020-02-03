using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Lpp.Dns.Api.Tests.Requests
{
    [TestClass]
    public class ResponseAggregationTests
    {
        [TestMethod]
        public void ParseRequestDataMartStatus()
        {
            string description = @"Routing status of PCORI Datamart - Oracle for request [PCORnet] has been changed from Submitted to Awaiting Response Approval by OPOS\SystemAdministrator.";

            string pattern = @"from ([\w| ]+) to ([\w| ]+) by";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(description);

            string oldStatus = match.Groups[1].ToString();
            string newStatus = match.Groups[2].ToString();

            Console.WriteLine("Old Status: " + oldStatus);
            Console.WriteLine("Current Status: " + newStatus);

            Assert.IsTrue(!string.IsNullOrEmpty(oldStatus), "The old status is empty!");
            Assert.IsTrue(!string.IsNullOrEmpty(newStatus), "The new status is empty!");
        }

        [TestMethod]
        public void MergeTwoResponses()
        {

            var response1 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json");
            var response2 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM2-response.json");

            IEnumerable<Dictionary<string,object>> r1 = response1.Results.First();
            Console.WriteLine("Result 1 count:" + r1.Count());
            IEnumerable<Dictionary<string,object>> r2 = response2.Results.First();
            Console.WriteLine("Result 2 count:" + r2.Count());
            Console.WriteLine("Total count:" + (r1.Count() + r2.Count()));

            IEnumerable<Dictionary<string,object>> combined = r1.Select(d => { d.Add("DataMart", "DM1"); return d; }).Concat(r2.Select(d => {d.Add("DataMart", "DM2"); return d; })).ToArray();

            Dns.DTO.QueryComposer.QueryComposerResponseDTO combinedResponse = new DTO.QueryComposer.QueryComposerResponseDTO();
            combinedResponse.ResponseDateTime = DateTime.UtcNow;
            combinedResponse.RequestID = Guid.Empty;
            combinedResponse.Results = new []{ combined };

            string mergedSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(combinedResponse, Newtonsoft.Json.Formatting.None);

            System.IO.File.WriteAllText("merged-response.json", mergedSerialized);

            Newtonsoft.Json.Linq.JObject jobj = new Newtonsoft.Json.Linq.JObject();
            jobj.Add("prop1", new Newtonsoft.Json.Linq.JValue("23"));


            
            

            /**
             * Test Name:	MergeTwoResponses
Test Outcome:	Passed
Result StandardOutput:	
Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json
Lpp.Dns.Api.Tests.Requests.Samples.DM2-response.json
Lpp.Dns.Api.Tests.Requests.Samples.request.json


             * */

        }

        [TestMethod]
        public void ApplyGroupingToResult()
        {
            var response1 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json");

            string stringName = typeof(string).FullName;
            response1.Properties = new [] {
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO {
                    Name = "AgeGroup",
                    Type = "System.String" 
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Sex",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Year",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "MedCov",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "DrugCov",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Total Enrollment in Strata(Members)",
                    Type = "System.Decimal"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Days Covered",
                    Type = "System.Decimal"
                }
            };

            //create the response item type
            Type t1 = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ResponseItem", response1.Properties);

            //create the list type
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(t1);
            IList items = Activator.CreateInstance(constructedListType) as IList;

            //build the map of property info to the dictionary key values.
            IDictionary<string, PropertyInfo> propertyInfoMap = Lpp.Objects.Dynamic.TypeBuilderHelper.CreatePropertyInfoMap(t1, response1.Properties);

            foreach (var dic in response1.Results.First())
            {
                //create and add the populated object to the collection
                var obj = Lpp.Objects.Dynamic.TypeBuilderHelper.FlattenDictionaryToType(t1, dic, propertyInfoMap);
                items.Add(obj);
            }


            //build the aggregate definition
            var aggregate = new Lpp.Dns.DTO.QueryComposer.QueryComposerResponseAggregationDefinitionDTO
            {
                GroupBy = new[] { "Year", "Sex", "AgeGroup", "DrugCov", "MedCov" },
                Select = new[] { 
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "Year",
                        Type = "System.String"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "Sex",
                        Type = "System.String"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "AgeGroup",
                        Type = "System.String"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "DrugCov",
                        Type = "System.String",
                        As = "DrugCoverage"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "MedCov",
                        Type = "System.String",
                        As = "MedicalCoverage"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "DaysCovered",
                        Type = "System.Decimal",
                        Aggregate = "Sum"
                    },
                    new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                        Name = "TotalEnrollmentinStrataMembers",
                        As = "Members",
                        Type = "System.Decimal",
                        Aggregate = "Sum"
                    }
                }
            };

            string groupBy = string.Join(",", aggregate.GroupBy);

            List<string> selectBy = new List<string>(aggregate.Select.Count() + 10);
            foreach (Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO prop in aggregate.Select)
            {
                string s = aggregate.GroupBy.Contains(prop.Name, StringComparer.OrdinalIgnoreCase) ? "Key." + prop.Name : prop.Name;
                if (!string.IsNullOrWhiteSpace(prop.Aggregate))
                {
                    s = prop.Aggregate + "(" + s + ")";
                }

                if (!string.Equals(prop.Name, prop.As, StringComparison.OrdinalIgnoreCase) || !string.IsNullOrWhiteSpace(prop.Aggregate))
                {
                    s += " as " + prop.As;
                }
                selectBy.Add(s);
            }

            string groupingStatement = "new (" + groupBy + ")";
            string selectStatement = "new (" + string.Join(",", selectBy) + ")";
            var q = items.AsQueryable()
                         .GroupBy(groupingStatement)
                         .Select(selectStatement);

            var backToDictionary = Lpp.Objects.Dynamic.TypeBuilderHelper.ConvertToDictionary(q.AsEnumerable(), aggregate.Select);

            Console.WriteLine(backToDictionary.Count());

        }        

        [TestMethod]
        public void CreateTypeAndLoadFromResponseJson() 
        {
            var response1 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json");
            string stringName = typeof(string).FullName;
            response1.Properties = new[] {
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "AgeGroup",
                    Type = stringName 
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Sex",
                    Type = stringName
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Year",
                    Type = stringName
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "MedCov",
                    Type = stringName
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "DrugCov",
                    Type = stringName
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Total Enrollment in Strata(Members)",
                    Type = typeof(decimal).FullName
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Days Covered",
                    As = "DaysCovered",
                    Type = typeof(decimal).FullName
                }
            };

            Type t1 = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ResponseItem", response1.Properties);

            //create the list type
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(t1);
            IList items = Activator.CreateInstance(constructedListType) as IList;

            IDictionary<string, PropertyInfo> propertyInfoMap = Lpp.Objects.Dynamic.TypeBuilderHelper.CreatePropertyInfoMap(t1, response1.Properties);

            foreach (var dic in response1.Results.First())
            {
                //create and add the populated object to the collection
                var obj = Lpp.Objects.Dynamic.TypeBuilderHelper.FlattenDictionaryToType(t1, dic, propertyInfoMap);
                items.Add(obj);
            }

            var q = items.AsQueryable()
                         .GroupBy("new (Year,Sex,AgeGroup,DrugCov,MedCov)")
                         .Select("new (Key.Year, Key.Sex, Key.AgeGroup, Key.DrugCov as DrugCoverage, Key.MedCov as MedicalCoverage, Sum(Convert.ToDecimal(DaysCovered)) as DaysCovered, Sum(Convert.ToDecimal(TotalEnrollmentinStrataMembers)) as Members)")
                         .OrderBy("Year, DaysCovered DESC");


            foreach (dynamic i in q)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", i.Year, i.Sex, i.AgeGroup, i.DrugCoverage, i.MedicalCoverage, i.DaysCovered, i.Members);
            }
        }

        internal class ModularProgramTermValues {
            public IList<Document> Documents { get; set; }

            public class Document
            {
                public Guid RevisionSetID { get; set; }
            }
        }        


        [TestMethod]
        public void UpdateRequestJson()
        {
            string json = System.IO.File.ReadAllText("../Requests/Samples/ModularProgramRequest.json");
            DTO.QueryComposer.QueryComposerRequestDTO requestDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);

            var modularTerm = requestDTO.Where.Criteria.SelectMany(c => c.Terms.Where(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.ModularProgramID)).FirstOrDefault();
            var termValues = Newtonsoft.Json.JsonConvert.DeserializeObject<ModularProgramTermValues>(((JObject)modularTerm.Values["Values"]).ToString());

            //foreach(var x in termValues.Documents)
            //{
            //    Console.WriteLine(x.RevisionSetID);
            //}

            //add new revisionset values to update the request.json
            termValues.Documents.Add(new ModularProgramTermValues.Document { RevisionSetID = Guid.NewGuid() });
            termValues.Documents.Add(new ModularProgramTermValues.Document { RevisionSetID = Guid.NewGuid() });
            termValues.Documents.Add(new ModularProgramTermValues.Document { RevisionSetID = Guid.NewGuid() });


            modularTerm.Values["Values"] = Newtonsoft.Json.JsonConvert.SerializeObject(termValues);

            string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(requestDTO);
            Console.WriteLine(json2);
            System.Diagnostics.Debugger.Break();
        }


        T LoadJson<T>(string key)
        {
            string json;
            using(var stream = new System.IO.StreamReader(typeof(ResponseAggregationTests).Assembly.GetManifestResourceStream(key)))
            {
                json = stream.ReadToEnd();
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}

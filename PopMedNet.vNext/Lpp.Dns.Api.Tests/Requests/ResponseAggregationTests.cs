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

            IEnumerable<Dictionary<string,object>> r1 = response1.Queries.SelectMany(q => q.Results).First();
            Console.WriteLine("Result 1 count:" + r1.Count());
            IEnumerable<Dictionary<string,object>> r2 = response2.Queries.SelectMany(q => q.Results).First();
            Console.WriteLine("Result 2 count:" + r2.Count());
            Console.WriteLine("Total count:" + (r1.Count() + r2.Count()));

            IEnumerable<Dictionary<string,object>> combined = r1.Select(d => { d.Add("DataMart", "DM1"); return d; }).Concat(r2.Select(d => {d.Add("DataMart", "DM2"); return d; })).ToArray();

            Dns.DTO.QueryComposer.QueryComposerResponseDTO combinedResponse = new DTO.QueryComposer.QueryComposerResponseDTO {
                Header = new DTO.QueryComposer.QueryComposerResponseHeaderDTO {
                    ID = Guid.NewGuid(),
                    RequestID = Guid.Empty,
                    QueryingEnd = DateTimeOffset.UtcNow
                },
                Queries = new[] { new DTO.QueryComposer.QueryComposerResponseQueryResultDTO {
                    ID = Guid.NewGuid(),
                    QueryStart = DateTimeOffset.UtcNow,
                    QueryEnd = DateTimeOffset.UtcNow,
                    Results = new []{ combined }
                } }
            };

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
            var response1 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json");

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
            var response1 = LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO>("Lpp.Dns.Api.Tests.Requests.Samples.DM1-response.json");
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
            var requestDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerQueryDTO>(json);

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


        const string DefaultGroupingKey = "__DefaultGroupingKey";
        const string DefaultGroupingKeyValue = "__DefaultGroupingKeyValue";
        [TestMethod]
        public void TestReadingNullResult()
        {
            throw new NotImplementedException("Multi-query aggregation not implemented yet.");

            //var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            //serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            //var responsesToAggregate = new List<DTO.QueryComposer.QueryComposerResponseDTO> { LoadJson<Dns.DTO.QueryComposer.QueryComposerResponseDTO>("Lpp.Dns.Api.Tests.Requests.Samples.NullResponse.json", serializationSettings) };

            //DTO.QueryComposer.QueryComposerResponseDTO combinedResponse = new DTO.QueryComposer.QueryComposerResponseDTO();
            //combinedResponse.Header.RequestID = Guid.Empty;
            //combinedResponse.Header.QueryingStart = DateTime.UtcNow;
            //combinedResponse.Header.QueryingEnd = combinedResponse.Header.QueryingStart;

            ////get the aggregatable properties, assume that all the responses have the same aggregation definition
            //IEnumerable<Objects.Dynamic.IPropertyDefinition> propertyDefinitions = responsesToAggregate.Where(r => r.Aggregation.Select.Any()).Select(r => r.Aggregation.Select.Where(pd => !string.Equals("LowThreshold", pd.As, StringComparison.OrdinalIgnoreCase))).FirstOrDefault();
            ////add a default groupingkey, this is needed for when there is only a single property in the response and it is getting aggregated
            //propertyDefinitions = propertyDefinitions.Union(new[] { new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO { Name = DefaultGroupingKey, Type = typeof(string).FullName } });

            ////convert to typed objects so that we can work with the results using reflection, all responses must have the same property and aggregation definition.
            //Type resultType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("ResponseItem", propertyDefinitions);

            ////create a typed list to hold the converted response items
            //Type listType = typeof(List<>).MakeGenericType(resultType);
            ////System.Collections.IList items = Activator.CreateInstance(listType) as System.Collections.IList;
            //var items = Activator.CreateInstance(listType);

            ////build a map of the property info to the dictionary key values
            //IDictionary<string, System.Reflection.PropertyInfo> propertyInfoMap = Lpp.Objects.Dynamic.TypeBuilderHelper.CreatePropertyInfoMap(resultType, propertyDefinitions);

            ////merge results
            //if (responsesToAggregate.Count == 1)
            //{
            //    combinedResponse = responsesToAggregate[0];
            //}
            //else
            //{
            //    IEnumerable<Dictionary<string, object>> combined = Enumerable.Empty<Dictionary<string, object>>();
            //    foreach (var r in responsesToAggregate.SelectMany(rr => rr.Results))
            //    {
            //        //include only the properties that are defined in the aggregation select definition, and are not the low threshhold column's standard name
            //        var filtered = r.Select(d => d.Where(k => !string.Equals(k.Key, "LowThreshold", StringComparison.OrdinalIgnoreCase) && propertyDefinitions.Any(p => p.As == k.Key)).ToDictionary(dd => dd.Key, dd => dd.Value));
            //        combined = combined.Concat(filtered); ;
            //    }
            //    combinedResponse.Results = new[] { combined };
            //}

            //foreach (var dic in combinedResponse.Results.First())
            //{
            //    //add the default grouping value to the existing result item
            //    dic.Add(DefaultGroupingKey, DefaultGroupingKeyValue);

            //    //create and add the populated object to the collection
            //    var obj = Lpp.Objects.Dynamic.TypeBuilderHelper.FlattenDictionaryToType(resultType, dic, propertyInfoMap);
            //    ((System.Collections.IList)items).Add(obj);
            //}

            //if (((System.Collections.IList)items).Count == 0)
            //{
            //    combinedResponse.Results = new[] { new Dictionary<string, object>[0] };
            //}
            //else
            //{
            //    var aggregate = responsesToAggregate.Where(r => r.Aggregation != null).Select(r => r.Aggregation).FirstOrDefault();
            //    //in past some responses included the LowThreshold column in the aggregation select, remove explicitly from the combined aggregation defintion
            //    aggregate.Select = aggregate.Select.Where(pd => !string.Equals("LowThreshold", pd.As, StringComparison.OrdinalIgnoreCase)).ToArray();

            //    List<string> selectBy = new List<string>(aggregate.Select.Count() + 10);
            //    foreach (Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO prop in aggregate.Select)
            //    {
            //        string s = (aggregate.GroupBy.Contains(prop.Name, StringComparer.OrdinalIgnoreCase) ? "Key." : "") + prop.As;

            //        //don't add LowThreshold to the select
            //        if (s.Equals("LowThreshold", StringComparison.OrdinalIgnoreCase))
            //            continue;


            //        if (!string.IsNullOrWhiteSpace(prop.Aggregate))
            //        {
            //            s = prop.Aggregate + "(" + Lpp.Objects.Dynamic.TypeBuilderHelper.CleanString(s) + ")";
            //        }

            //        if (!string.IsNullOrWhiteSpace(prop.Aggregate))
            //        {
            //            s += " as " + Lpp.Objects.Dynamic.TypeBuilderHelper.CleanString(prop.As);
            //        }

            //        selectBy.Add(s);
            //    }

            //    var q = ((System.Collections.IList)items).AsQueryable();

            //    //group by the specified properties, else fall back to the default grouping key
            //    if (aggregate.GroupBy != null && aggregate.GroupBy.Where(x => !string.IsNullOrEmpty(x)).Any())
            //    {
            //        string groupingStatement = "new (" + string.Join(",", aggregate.GroupBy) + ")";
            //        q = q.GroupBy(groupingStatement);
            //    }
            //    else
            //    {
            //        q = q.GroupBy($"new ({DefaultGroupingKey})");
            //    }

            //    string selectStatement = "new (" + string.Join(",", selectBy) + ")";
            //    q = q.Select(selectStatement);

            //    //convert results back to IEnumerable<Dictionary<string,object>>, and add to the results being returned
            //    //dont include LowThreshold as an aggregation
            //    IEnumerable<Dictionary<string, object>> aggregatedResults = Lpp.Objects.Dynamic.TypeBuilderHelper.ConvertToDictionary(((IQueryable)q).AsEnumerable(), aggregate.Select);
            //    combinedResponse.Results = new[] { aggregatedResults.ToArray() };
            //    combinedResponse.Properties = responsesToAggregate.First().Properties.ToArray();
            //    combinedResponse.Aggregation = aggregate;

            //}

            //Assert.IsNotNull(combinedResponse);
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

        T LoadJson<T>(string key, Newtonsoft.Json.JsonSerializerSettings serializationSettings)
        {
            string json;
            using (var stream = new System.IO.StreamReader(typeof(ResponseAggregationTests).Assembly.GetManifestResourceStream(key)))
            {
                json = stream.ReadToEnd();
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, serializationSettings);
        }
    }
}

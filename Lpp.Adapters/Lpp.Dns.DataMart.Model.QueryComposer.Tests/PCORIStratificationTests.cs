using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.QueryComposer;
using System.Data.Entity;
using System.Reflection;
using System.Linq.Expressions;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORIStratificationTests
    {
        

        static PCORIStratificationTests()
        {   
            log4net.Config.XmlConfigurator.Configure();
        }

        [TestMethod]
        public void TwentyEightAndYounger_NoSelectSpecified()
        {
            //This is expected to be just a straight count of results based on specified criteria of all patients 28 and younger.
            var result = RunQueryAgainstAdapter("../Resources/MaxAgeOf28_NoSelect.json");
            Helper.DumpQueryComposerResults(result.Results);
        }

        [TestMethod]
        public void GroupBySex_NoCriteria()
        {
            var result = RunQueryAgainstAdapter("../Resources/GroupBySex_NoCriteria.json");
            Helper.DumpQueryComposerResults(result.Results);
        }

        [TestMethod]
        public void StrokeInDiabeticsQuery()
        {
            var result = RunQueryAgainstAdapter("../Resources/StrokeInDiabetics.json");
            Helper.DumpQueryComposerResults(result.Results);
        }
        [TestMethod]
        public void PcoriSqlDistribution()
        {
            var result = RunQueryAgainstAdapter("../Resources/QueryComposition/PcoriSqlDistribution.json");

            Helper.DumpQueryComposerResults(result.Results);
        }
        QueryComposerResponseQueryResultDTO RunQueryAgainstAdapter(string jsonSrc, string connectionString = null)
        {
            var json = System.IO.File.ReadAllText(jsonSrc);
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;

            var request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerQueryDTO>(json, jsonSettings);

            if(string.IsNullOrEmpty(connectionString))
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;

            using (var adapter = Helper.CreatePCORIModelAdapterAdapter(connectionString))
            {
                return adapter.Execute(request, false);
            }
        }


        [TestMethod]
        public void SexStratification()
        {
            //actual query
            using (var db = Helper.CreatePCORIDataContext("PCORNET"))
            {
                db.Database.Log = (s) =>
                {
                    Console.WriteLine(s);
                };

                var minDate = DateTime.Now.Date.AddYears(-28);

                var query = (from p in db.Patients
                             //where p.BornOn >= minDate
                             where DbFunctions.DiffYears(p.BornOn, DateTime.Now) <= 28
                             select new { Sex = p.Sex, p.Race })
                            .GroupBy(p => new {  p.Sex, p.Race })
                            .OrderBy(r => r.Key.Sex)
                            .ThenBy(r => r.Key.Race)
                            .Select(k => new { k.Key, Count = k.Count() });

                var result = query.ToArray();
                foreach (var r in result)
                {
                    Console.WriteLine("{0}/{1}  {2}", r.Key.Sex, r.Key.Race, r.Count);
                }

                

            }

           // var json = System.IO.File.ReadAllText("../Resources/MaxAgeOf28_StratifyBySex.json");
           // Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
           // jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
           // QueryComposerRequestDTO request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(json, jsonSettings);

           // //using model adapter
           // using (var adapter = CreateAdapter(connectionString))
           // {
           //     var result = adapter.Execute(request, false);
           //     DumpQueryComposerResults(result.Results);    
           //}

        }

        //[TestMethod]
        //public void T1()
        //{
        //    using (var db = CreateDataContext())
        //    {
        //        IQueryable<Model.QueryComposer.Adapters.PCORI.PCORIQueryResult> query = from p in db.Patients
        //                    select new Model.QueryComposer.Adapters.PCORI.PCORIQueryResult
        //                    {
        //                        PatientID = p.PatientID,
        //                        Patients = null,
        //                        Gender = p.Sex,
        //                        Ethnicity = p.Hispanic,
        //                        Age = DbFunctions.DiffYears(p.BornOn, DateTime.Now)
        //                    };

        //        query = query.GroupBy(k => new {  k.Patients }).Select(k => new Model.QueryComposer.Adapters.PCORI.PCORIQueryResult
        //                    {
        //                        PatientID = null,
        //                        Patients = k.Count(),
        //                        Gender = null,
        //                        Ethnicity = null,
        //                        Age = null
        //                    });

        //        foreach (var rst in query)
        //        {
        //            Console.WriteLine(rst.Patients);
        //        }
        //    }
        //}

        [TestMethod, Ignore]
        public void DynamicSelect()
        {
            var propDefinitions = new[] { 
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Sex",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "PatientID",
                    Type = "System.String"
                },
                new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO{
                    Name = "Race",
                    Type = "System.String"
                },
            };

            //create the dynamic return type
            Type selectType = Lpp.Objects.Dynamic.TypeBuilderHelper.CreateType("patSelect", propDefinitions);
            Type sourceType = typeof(Lpp.Dns.DataMart.Model.PCORIQueryBuilder.Model.Patient);
            Dictionary<string, PropertyInfo> sourceProperties = sourceType.GetProperties().ToDictionary(pi => pi.Name);
            
            ParameterExpression sourceItem = Expression.Parameter( sourceType, "p");
            
            IEnumerable<MemberBinding> bindings = selectType.GetProperties().Select(x => Expression.Bind(x, Expression.Property(sourceItem, sourceProperties[x.Name]))).OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(Expression.New(selectType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);
            
            using (var db = Helper.CreatePCORIDataContext("PCORNET"))
            {
                var minDate = DateTime.Now.Date.AddYears(-28);
                
                var query = db.Patients.AsNoTracking().Where(p => p.BornOn >= minDate);

                var result = query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select", new Type[] { sourceType, selectType }, Expression.Constant(query), selector));
                
            }


        }


    }
}

using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Portal.Areas.DataChecker.Models;
using Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class AgeDistributionController : BaseController
    {
        [HttpGet]
        public ActionResult AgeDistributionResponse()
        {
            return View();
        }

        public JsonResult GetTermValues(Guid? requestID)
        {
            if (requestID == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new DataContext())
            {
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_AgeDistribution;

                var req = db.Requests.Find(requestID);
                Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");


                AgeValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<AgeValues>(termValues.Value.ToString());

                return Json(val.AgeDistributionValue.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Age", typeof(string));
            table.Columns.Add("n", typeof(double));
            table.Columns.Add("LowThreshold", typeof(bool));

            foreach (var rawRow in records)
            {
                DataRow dr = table.NewRow();

                foreach (var col in rawRow)
                {
                    if ((col.Key == "Total" || col.Key == "n" || col.Key == "Count") && col.Value == null)
                    {
                        dr["n"] = 0;
                    }
                    else
                    {
                        string key = col.Key;
                        if (key == "DataPartner")
                        {
                            key = "DP";
                        }
                        else if (key == "Total" || key == "Count")
                        {
                            key = "n";
                        }
                        else if (key == "AgeRange")
                        {
                            key = "Age";
                        }

                        dr[key] = col.Value;
                    }
                }

                table.Rows.Add(dr);
            }

            return table;
        }

        [HttpGet]
        public JsonResult ProcessMetricsByResponse(Guid responseID)
        {
            DataTable dt = null;
            string json = string.Empty;
            using (var db = new DataContext())
            {
                var document = db.Documents.Where(r => r.ItemID == responseID).FirstOrDefault();
                if (document == null)
                {
                    return null;
                }

                var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
                serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
                var deserializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);

                Type queryComposerResponseDTOType = typeof(DTO.QueryComposer.QueryComposerResponseDTO);

                Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO rsp;

                using (var documentStream = new Data.Documents.DocumentStream(db, document.ID))
                using (var streamReader = new System.IO.StreamReader(documentStream))
                {
                    rsp = (Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO)deserializer.Deserialize(streamReader, queryComposerResponseDTOType);
                }

                dt = CreateTable(rsp.Results.First());
            }

            if (dt == null)
            {
                return null;
            }

            return GetMetrics(dt);
        }

        [HttpGet]
        public override JsonResult ProcessMetrics(Guid documentId)
        {
            var ds = LoadResults(documentId);

            return GetMetrics(ds.Tables[0]);
        }

        [HttpGet]
        public JsonResult GetMetrics(DataTable dataTable)
        {
            var dataPartners = dataTable.AsEnumerable().Select(row => row["DP"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();

            var AgeDataPartners = from dp in dataPartners
                                     from Age in new[] { "<0 yrs", "0-1 yrs", "2-4 yrs", "5-9 yrs", "10-14 yrs", "15-18 yrs", 
                                         "19-21 yrs", "22-44 yrs", "45-64 yrs", "65-74 yrs", "75-110 yrs", ">110 yrs", "Other", "NULL or Missing" }
                                     select new { Age = Age, DP = (string)dp };
            #region HIDE

            var Agees = from row in dataTable.AsEnumerable()
                           select new AgeDistributionItemData { DP = row.Field<string>("DP"), Age = row.Field<string>("Age"), n = row.Field<double?>("n") };
            IEnumerable<AgeDistributionItemData> rawResults = (from sdp in AgeDataPartners
                                                      join Age in Agees
                                                      on new { sdp.Age, sdp.DP } equals new { Age.Age, Age.DP } into sxs
                                                      from x in sxs.DefaultIfEmpty()
                                                   select new AgeDistributionItemData
                                                      {
                                                          DP = sdp.DP, //x.Field<string>("DP"),
                                                          Age = sdp.Age, //rx, //x.Field<string>("Race"),
                                                          n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                      }).ToArray();

            var codes = rawResults.GroupBy(g => g.Age).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.Age).Select((k) =>
                    new
                    {
                        Age = k.Key,
                        Age_Display = TranslateAge(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = totalCount <= 0 ? 0 : Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Age)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = TranslateAge(rx),
                                        Count = k.Where(x => x.Age == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.Age)
                                          .Select(k => new
                                          {
                                              Age = k.Key,
                                              Age_Display = TranslateAge(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Age)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
            #endregion
        }


        string TranslateAge(string value)
        {
            switch (value.ToUpper())
            {
                case "<0":
                    return "<0 years";
                case "0-1":
                    return "0-1 years";
                case "2-4":
                    return "2-4 years";
                case "5-9":
                    return "5-9 years";
                case "10-14":
                    return "10-14 years";
                case "15-18":
                    return "15-18 years";
                case "19-21":
                    return "19-21 years";
                case "22-44":
                    return "22-44 years";
                case "45-64":
                    return "45-64 years";
                case "65-74":
                    return "65-74 years";
                case "75-110":
                    return "75-110 years";
                case ">110":
                    return ">110 years";
                case "Other":
                    return "Other";
                case "NULL or Missing":
                    return "NULL or Missing";
            }
            return value;
        }
    }
public class AgeValues
    {

        public AgeValues()
        {
            AgeDistributionValue = new List<string>();
        }

        public IEnumerable<string> AgeDistributionValue { get; set; }
    }
    
}


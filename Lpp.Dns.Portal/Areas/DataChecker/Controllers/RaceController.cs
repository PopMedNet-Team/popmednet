using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class RaceController : BaseController
    {
        [HttpGet]
        public ActionResult RaceResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_Race;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");

                RaceValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<RaceValues>(termValues.Value.ToString());

                return Json(val.Races.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("RACE", typeof(string));
            table.Columns.Add("Total", typeof(double));
            table.Columns.Add("LowThreshold", typeof(bool));

            foreach (var rawRow in records)
            {
                DataRow dr = table.NewRow();

                foreach (var col in rawRow)
                {
                    if ((col.Key == "Total" || col.Key == "n" || col.Key == "Count") && col.Value == null)
                    {
                        dr["Total"] = 0;
                    }
                    else
                    {
                        string key = col.Key;
                        if (key == "DataPartner")
                        {
                            key = "DP";
                        }
                        else if (key == "n" || key == "Count")
                        {
                            key = "Total";
                        }
                        dr[key] = col.Value;
                    }
                }

                table.Rows.Add(dr);
            }

            return table;
        }

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

        public override JsonResult ProcessMetrics(Guid documentId)
        {
            var ds = LoadResults(documentId);

            return GetMetrics(ds.Tables[0]);
        }

        public JsonResult GetMetrics(DataTable dataTable)
        {
            var dataPartners = dataTable.AsEnumerable().Select(row => row["DP"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();
            //var raceDataPartners = from dp in dataPartners
            //                     from race in new[] { "Other", "Unknown", "American Indian or Alaska Native", "Asian", "Black or African American", "Native Hawaiian or Other Pacific Islander", "White", "Missing" }
            //                     select new { Race = race, DP = (string)dp };

            var raceDataPartners = from dp in dataPartners
                                   from race in new[] { "-1", "0", "1", "2", "3", "4", "5", "6" }
                                   select new { Race = race, DP = (string)dp };

            var races = from row in dataTable.AsEnumerable()
                         select new RaceItemData { DP = row.Field<string>("DP"), Race = row.Field<string>("Race"), n = row.Field<double?>("Total") };
            IEnumerable<RaceItemData> rawResults = (from rxdp in raceDataPartners
                                                     join rxAmt in races //ds.Tables[0].AsEnumerable()
                                                     on new { rxdp.Race, rxdp.DP } equals new { rxAmt.Race, rxAmt.DP } into rxamts
                                                     from x in rxamts.DefaultIfEmpty()
                                                     select new RaceItemData
                                                     {
                                                         DP = rxdp.DP, //x.Field<string>("DP"),
                                                         Race = rxdp.Race, //rx, //x.Field<string>("Race"),
                                                         n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                     }).ToArray();

            //remove from raw results where "Other" has no patients
            var rawResultsUpdated = rawResults.Where(r => ((r.Race == "-1" && (r.n > 0)) || (r.Race != "-1"))).ToArray();

            //var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResultsUpdated.GroupBy(g => g.Race).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResultsUpdated.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResultsUpdated.GroupBy(r => r.Race).Select((k) =>
                    new
                    {
                        Race = k.Key,
                        Race_Display = FormatValue(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Race)).ToArray();

            var codesByPartner = rawResultsUpdated.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResultsUpdated.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = FormatValue(rx),
                                        Count = k.Where(x => x.Race == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResultsUpdated.GroupBy(g => g.Race)
                                          .Select(k => new
                                          {
                                              Race = k.Key,
                                              Race_Display = FormatValue(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResultsUpdated.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Race)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        public static string FormatValue(string value)
        {
            switch (value)
            {
                case "-1":
                    return "Other";
                case "0":
                    return "Unknown";
                case "1":
                    return "American Indian or Alaska Native";
                case "2":
                    return "Asian";
                case "3":
                    return "Black or African American";
                case "4":
                    return "Native Hawaiian or Other Pacific Islander";
                case "5":
                    return "White";
                case "6":
                    return "Missing";
            }

            return "Missing";
        }
    }

    public class RaceValues
    {
        public RaceValues()
        {
            Races = new List<int>();
        }

        public IEnumerable<int> Races { get; set; }
    }
}
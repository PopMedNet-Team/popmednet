using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Portal.Areas.DataChecker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class WeightController : BaseController
    {
        [HttpGet]
        public ActionResult WeightResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_WeightDistribution;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");

                
                WeightValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<WeightValues>(termValues.Value.ToString());

                return Json(val.WeightDistributions.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Weight", typeof(string));
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

            var WeightDataPartners = from dp in dataPartners
                                     from Weight in new[] { "<0", "0-1", "2-6", "7-12", "13-20", "21-35", "36-50", "51-75", "76-100", "101-125", "126-150", "151-175", "176-200", "201-225", "226-250", "251-275", "276-300", "276-300", "301-350", "350+", "NULL or Missing", "OTHER" }
                                   select new { Weight = Weight, DP = (string)dp };
            #region HIDE

            var Weightes = from row in dataTable.AsEnumerable()
                        select new WeightItemData { DP = row.Field<string>("DP"), Weight = row.Field<string>("Weight"), n = row.Field<double?>("n") };
            IEnumerable<WeightItemData> rawResults = (from sdp in WeightDataPartners
                                                    join Weight in Weightes
                                                    on new { sdp.Weight, sdp.DP } equals new { Weight.Weight, Weight.DP } into sxs
                                                    from x in sxs.DefaultIfEmpty()
                                                    select new WeightItemData
                                                    {
                                                        DP = sdp.DP, //x.Field<string>("DP"),
                                                        Weight = sdp.Weight, //rx, //x.Field<string>("Race"),
                                                        n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                    }).ToArray();

            var codes = rawResults.GroupBy(g => g.Weight).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.Weight).Select((k) =>
                    new
                    {
                        Weight = k.Key,
                        Weight_Display = TranslateWeight(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Weight)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = TranslateWeight(rx),
                                        Count = k.Where(x => x.Weight == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.Weight)
                                          .Select(k => new
                                          {
                                              Weight = k.Key,
                                              Weight_Display = TranslateWeight(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Weight)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
            #endregion
        }


        string TranslateWeight(string value)
        {
            switch (value.ToUpper())
            {
                case "<0":
                    return "<0 lbs";
                case "0-1":
                    return "0-1 lbs";
                case "2-6":
                    return "2-6 lbs";
                case "7-12":
                    return "7-12 lbs";
                case "13-20":
                    return "13-20 lbs";
                case "21-35":
                    return "21-35 lbs";
                case "36-50":
                    return "36-50 lbs";
                case "51-75":
                    return "51-75 lbs";
                case "76-100":
                    return "75-100 lbs";
                case "101-125":
                    return "101-125 lbs";
                case "126-150":
                    return "126-150 lbs";
                case "151-175":
                    return "151-175 lbs";
                case "176-200":
                    return "176-200 lbs";
                case "201-225":
                    return "201-225 lbs";
                case "226-250":
                    return "226-250 lbs";
                case "251-275":
                    return "251-275 lbs";
                case "276-300":
                    return "276-300 lbs";
                case "301-350":
                    return "301-350 lbs";
                case "350+":
                    return "350+ lbs";
                case "NULL or Missing":
                    return "NULL or Missing";
                case "OTHER":
                    return "Other";
            }
            return value;
        }
    }

    public class WeightValues
    {

        public WeightValues()
        {
            WeightDistributions = new List<string>();
        }

        public IEnumerable<string> WeightDistributions { get; set; }
    }
}
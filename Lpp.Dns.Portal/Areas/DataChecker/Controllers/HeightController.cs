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
    public class HeightController : BaseController
    {
        [HttpGet]
        public ActionResult HeightResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_HeightDistribution;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Queries.First().Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");


                HeightValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<HeightValues>(termValues.Value.ToString());

                return Json(val.HeightDistributions.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Height", typeof(string));
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

                dt = CreateTable(rsp.Queries.First().Results.First());
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

            var HeightDataPartners = from dp in dataPartners
                                     from Height in new[] { "0-10", "11-20", "21-45", "46-52", "53-58", "59-64", "65-70", "71-76", "77-82", "83-88", "89-94", "95+", "<0", "NULL or Missing", "OTHER" }
                                     select new { Height = Height, DP = (string)dp };
            #region HIDE

            var Heightes = from row in dataTable.AsEnumerable()
                           select new HeightItemData { DP = row.Field<string>("DP"), Height = row.Field<string>("Height"), n = row.Field<double?>("n") };
            IEnumerable<HeightItemData> rawResults = (from sdp in HeightDataPartners
                                                      join Height in Heightes
                                                      on new { sdp.Height, sdp.DP } equals new { Height.Height, Height.DP } into sxs
                                                      from x in sxs.DefaultIfEmpty()
                                                      select new HeightItemData
                                                      {
                                                          DP = sdp.DP, //x.Field<string>("DP"),
                                                          Height = sdp.Height, //rx, //x.Field<string>("Race"),
                                                          n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                      }).ToArray();

            var codes = rawResults.GroupBy(g => g.Height).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.Height).Select((k) =>
                    new
                    {
                        Height = k.Key,
                        Height_Display = TranslateHeight(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Height)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = TranslateHeight(rx),
                                        Count = k.Where(x => x.Height == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.Height)
                                          .Select(k => new
                                          {
                                              Height = k.Key,
                                              Height_Display = TranslateHeight(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Height)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
            #endregion
        }


        string TranslateHeight(string value)
        {
            switch (value.ToUpper())
            {
                case "0-10":
                    return "0-10 in";
                case "11-20":
                    return "11-20 in";
                case "21-45":
                    return "21-45 in";
                case "46-52":
                    return "46-52 in";
                case "53-58":
                    return "53-58 in";
                case "59-64":
                    return "59-64 in";
                case "65-70":
                    return "65-70 in";
                case "71-76":
                    return "71-76 in";
                case "77-82":
                    return "77-82 in";
                case "83-88":
                    return "83-88 in";
                case "89-94":
                    return "89-94 in";
                case "95+":
                    return "95+ in";
                case "<0":
                    return "<0 in";
                case "NULL or Missing":
                    return "NULL or Missing";
                case "OTHER":
                    return "Other";
            }
            return value;
        }
    }

    public class HeightValues
    {

        public HeightValues()
        {
            HeightDistributions = new List<string>();
        }

        public IEnumerable<string> HeightDistributions { get; set; }
    }
}
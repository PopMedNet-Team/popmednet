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
    public class EthnicityController : BaseController
    {
        [HttpGet]
        public ActionResult EthnicityResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_Ethnicity;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Queries.First().Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");

                EthnicityValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<EthnicityValues>(termValues.Value.ToString());

                return Json(val.EthnicityValue.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Hispanic", typeof(string));
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

                dt = CreateTable(rsp.Queries.First().Results.First());
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

            var ethnicityDataPartners = from dp in dataPartners
                                   from hispanic in new[] { "U", "Y", "N", "MISSING", "Other" }
                                        select new { Hispanic = hispanic, DP = (string)dp };

            var Hispanics = from row in dataTable.AsEnumerable()
                        select new EthnicityItemData { DP = row.Field<string>("DP"), Hispanic = row.Field<string>("Hispanic"), n = row.Field<double?>("Total") };
            IEnumerable<EthnicityItemData> rawResults = (from rxdp in ethnicityDataPartners
                                                    join rxAmt in Hispanics //ds.Tables[0].AsEnumerable()
                                                    on new { rxdp.Hispanic, rxdp.DP } equals new { rxAmt.Hispanic, rxAmt.DP } into rxamts
                                                    from x in rxamts.DefaultIfEmpty()
                                                    select new EthnicityItemData
                                                    {
                                                        DP = rxdp.DP, //x.Field<string>("DP"),
                                                        Hispanic = rxdp.Hispanic, //rx, //x.Field<string>("Hispanic"),
                                                        n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                    }).ToArray();

            //var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResults.GroupBy(g => g.Hispanic).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.Hispanic).Select((k) =>
                    new
                    {
                        Hispanic = k.Key,
                        Hispanic_Display = FormatValue(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Hispanic)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = FormatValue(rx),
                                        Count = k.Where(x => x.Hispanic == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.Hispanic)
                                          .Select(k => new
                                          {
                                              Hispanic = k.Key,
                                              Hispanic_Display = FormatValue(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Hispanic)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        public static string FormatValue(string value)
        {
            switch (value)
            {                
                case "0": case "U":
                    return "Unknown";
                case "1": case "Y":
                    return "Hispanic";
                case "2": case "N":
                    return "Not Hispanic";
                case "3": case "MISSING":
                    return "Missing";
                case "Other": 
                    return "Other";
            }

            return "Other";
        }
    }

    public class EthnicityValues
    {
        public EthnicityValues()
        {
            EthnicityValue = new List<int>();
        }

        public IEnumerable<int> EthnicityValue { get; set; }
    }
}
using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Data;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class RxAmtController : BaseController
    {
        [HttpGet]
        public ActionResult RxAmtResponse()
        {
            return View();
        }

        public JsonResult GetRxAmounts(Guid? requestID)
        {
            if (requestID == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new DataContext())
            {
                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DispensingRXAmount)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_DispensingRXAmount);
                var termValues = term.Values.First(p => p.Key == "Values");
                DispensingRXAmountValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<DispensingRXAmountValues>(termValues.Value.ToString());

                return Json(val.RXAmounts.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("RxAmt", typeof(string));
            table.Columns.Add("Total", typeof(double));
            table.Columns.Add("LowThreshold", typeof(bool));

            foreach (var rawRow in records)
            {
                DataRow dr = table.NewRow();

                foreach (var col in rawRow)
                {
                    if (col.Key == "Total" && col.Value == null)
                    {
                        dr[col.Key] = 0;
                    }
                    else
                    {
                        dr[col.Key] = col.Value;
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
            var rxDataPartners = from dp in dataPartners
                                 from rx in new[] { "-1", "0", "30", "60", "90", "120", "180", "181", "OTHER", "MISSING" }
                                 select new { RxAmt = rx, DP = (string)dp };
            var rxAmts = from row in dataTable.AsEnumerable()
                         select new RxAmtItemData { DP = row.Field<string>("DP"), RxAmt = row.Field<string>("RxAmt"), n = row.Field<double?>("Total") };
            IEnumerable<RxAmtItemData> rawResults = (from rxdp in rxDataPartners //new[] { "-1", "0", "30", "60", "90", "120", "180", "181", "OTHER" }
                                                     join rxAmt in rxAmts //ds.Tables[0].AsEnumerable()
                                                     on new { rxdp.RxAmt, rxdp.DP } equals new { rxAmt.RxAmt, rxAmt.DP } into rxamts
                                                     from x in rxamts.DefaultIfEmpty()
                                                     select new RxAmtItemData
                                                     {
                                                         DP = rxdp.DP, //x.Field<string>("DP"),
                                                         RxAmt = rxdp.RxAmt, //rx, //x.Field<string>("RxAmt"),
                                                         n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                     }).ToArray();

            //var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResults.GroupBy(g => g.RxAmt).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.RxAmt).Select((k) =>
                    new
                    {
                        RxAmt = k.Key,
                        RxAmt_Display = FormatRxAmt(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.RxAmt)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = FormatRxAmt(rx),
                                        Count = k.Where(x => x.RxAmt == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.RxAmt)
                                          .Select(k => new
                                          {
                                              RxAmt = k.Key,
                                              RxAmt_Display = FormatRxAmt(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.RxAmt)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        public static string FormatRxAmt(string value) {
            switch (value) {
                case "-1":
                    return "< 0";
                case "0":
                    return "0-1";
                case "30":
                    return "2-30";
                case "60":
                    return "31-60";
                case "90":
                    return "61-90";
                case "120":
                    return "91-120";
                case "180":
                    return "121-180";
                case "181":
                    return ">180";
                case "OTHER":
                    return "Other RxAmt";
                case "MISSING":
                    return "Missing";
            }

            return "Missing";
        }
    }

    public class DispensingRXAmountValues
    {
        public DispensingRXAmountValues()
        {
            RXAmounts = new List<int>();
        }

        public IEnumerable<int> RXAmounts { get; set; }
    }
}
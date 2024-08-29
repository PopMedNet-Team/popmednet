using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class RxSupController : BaseController
    {
        [HttpGet]
        public ActionResult RxSupResponse()
        {
            return View();
        }

        public JsonResult GetRxSupplies(Guid? requestID)
        {
            Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_DispensingRXSupply;
            if (requestID == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new DataContext())
            {
                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Queries.First().Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");
                DispensingRXSupplyValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<DispensingRXSupplyValues>(termValues.Value.ToString());

                return Json(val.RXSupply.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("RxSup", typeof(string));
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
                        if (col.Key == "DataPartner")
                        {
                            dr["DP"] = col.Value;
                        }
                        else
                        {
                            dr[col.Key] = col.Value;
                        }
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
            var rxDataPartners = from dp in dataPartners
                                 from rx in new[] { "-1", "0", "2", "30", "60", "90", "OTHER", "MISSING" }
                                 select new { RxSup = rx, DP = (string)dp };
            var rxSups = from row in dataTable.AsEnumerable()
                         select new RxSupItemData { DP = row.Field<string>("DP"), RxSup = row.Field<string>("RxSup"), n = row.Field<double?>("Total") };
            IEnumerable<RxSupItemData> rawResults = (from rxdp in rxDataPartners //new[] { "-1", "0", "2", "30", "60", "90", "OTHER" }
                                                     join rxSup in rxSups //ds.Tables[0].AsEnumerable()
                                                     on new { rxdp.RxSup, rxdp.DP } equals new { rxSup.RxSup, rxSup.DP } into rxsups
                                                     from x in rxsups.DefaultIfEmpty()
                                                     select new RxSupItemData
                                                     {
                                                         DP = rxdp.DP, //x == null ? string.Empty : x.Field<string>("DP"),
                                                         RxSup = rxdp.RxSup, //x.Field<string>("RxSup"),
                                                         n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                     }).ToArray();

            //var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResults.GroupBy(g => g.RxSup).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.RxSup).Select((k) =>
                    new { 
                        RxSup = k.Key,
                        RxSup_Display = FormatRxSup(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.RxSup)).ToArray(); 

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new { 
                                        Code = rx,
                                        Code_Display = FormatRxSup(rx),
                                        Count = k.Where(x => x.RxSup == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(p => p.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.RxSup)
                                          .Select(k => new
                                          {
                                              RxSup = k.Key,
                                              RxSup_Display = FormatRxSup(k.Key), 
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new { 
                                                Partner = dp,
                                                Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(dp => dp.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.RxSup)).ToArray();            

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        static string FormatRxSup(string value) {
            switch (value) {
                case "-1":
                    return "< 0";
                case "0":
                    return "0-1";
                case "2":
                    return "2-30";
                case "30":
                    return "31-60";
                case "60":
                    return "61-90";
                case "90":
                    return ">90";
                case "OTHER":
                    return "Other RxSup";
                case "MISSING":
                    return "Missing";
            }

            return "Missing";
        }
    }

    public class DispensingRXSupplyValues
    {
        public DispensingRXSupplyValues()
        {
            RXSupply = new List<int>();
        }

        public IEnumerable<int> RXSupply { get; set; }
    }
}
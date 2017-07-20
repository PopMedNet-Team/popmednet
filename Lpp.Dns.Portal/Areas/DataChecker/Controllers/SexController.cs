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
    public class SexController : BaseController
    {
        [HttpGet]
        public ActionResult SexResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_SexDistribution;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");

                
                SexValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<SexValues>(termValues.Value.ToString());

                return Json(val.SexDistributions.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Sex", typeof(string));
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

            var sexDataPartners = from dp in dataPartners
                                   from sex in new[] { "A", "F", "M", "NI", "NULL or Missing", "OT", "UN", "Values outside of CDM specifications", "OTHER" }
                                   select new { Sex = sex, DP = (string)dp };

            var sexes = from row in dataTable.AsEnumerable()
                        select new SexItemData { DP = row.Field<string>("DP"), Sex = row.Field<string>("Sex"), n = row.Field<double?>("n") };
            IEnumerable<SexItemData> rawResults = (from sdp in sexDataPartners
                                                    join sex in sexes
                                                    on new { sdp.Sex, sdp.DP } equals new { sex.Sex, sex.DP } into sxs
                                                    from x in sxs.DefaultIfEmpty()
                                                    select new SexItemData
                                                    {
                                                        DP = sdp.DP, //x.Field<string>("DP"),
                                                        Sex = sdp.Sex, //rx, //x.Field<string>("Race"),
                                                        n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                    }).ToArray();

            var codes = rawResults.GroupBy(g => g.Sex).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.Sex).Select((k) =>
                    new
                    {
                        Sex = k.Key,
                        Sex_Display = TranslateSex(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Sex)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = TranslateSex(rx),
                                        Count = k.Where(x => x.Sex == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.Sex)
                                          .Select(k => new
                                          {
                                              Sex = k.Key,
                                              Sex_Display = TranslateSex(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Sex)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }


        string TranslateSex(string value)
        {
            switch (value.ToUpper())
            {
                case "A":
                    return "Ambiguous";
                case "F":
                    return "Female";
                case "M":
                    return "Male";
                case "NI":
                    return "No Information";
                case "NULL or Missing":
                    return "NULL or Missing";
                case "OT":
                    return "Other";
                case "UN":
                    return "Unknown";
                case "Values outside of CDM specifications":
                    return "Values outside of CDM specifications";
                case "OTHER":
                    return "Unselected Categories";
            }
            return value;
        }
    }

    public class SexValues
    {

        public SexValues()
        {
            SexDistributions = new List<string>();
        }

        public IEnumerable<string> SexDistributions { get; set; }
    }
}
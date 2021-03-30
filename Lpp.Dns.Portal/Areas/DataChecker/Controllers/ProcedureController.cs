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
    public class ProcedureController : BaseController
    {
        [HttpGet]
        public ActionResult ProcedureResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_ProcedureCodes;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Queries.First().Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");


                ProcedureValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<ProcedureValues>(termValues.Value.ToString());

                string[] arrSplit = { "," };
                string[] arrVal = val.CodeValues.Split(arrSplit, StringSplitOptions.RemoveEmptyEntries);

                return Json(arrVal, JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Px_CodeType", typeof(string));
            table.Columns.Add("PX", typeof(string));
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
            var diagCodes = dataTable.AsEnumerable().Select(row => row["PX"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();
            var codeTypes = dataTable.AsEnumerable().Select(row => row["Px_CodeType"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();

            var ProcedureDataPartners = from dp in dataPartners
                                        from dx in diagCodes
                                        from px_codetype in codeTypes
                                        select new { PX = (string)dx, DP = (string)dp, Px_CodeType = (string)px_codetype };
            #region HIDE

            var Procedurees = from row in dataTable.AsEnumerable()
                              select new ProcedureItemData { DP = row.Field<string>("DP"), PX = row.Field<string>("PX"), Px_CodeType = row.Field<string>("Px_CodeType"), n = row.Field<double?>("n") };
            IEnumerable<ProcedureItemData> rawResults = (from sdp in ProcedureDataPartners
                                                         join Procedure in Procedurees
                                                         on new { sdp.PX, sdp.DP, sdp.Px_CodeType } equals new { Procedure.PX, Procedure.DP, Procedure.Px_CodeType } into sxs
                                                         from x in sxs.DefaultIfEmpty()
                                                         select new ProcedureItemData
                                                         {
                                                             DP = sdp.DP, //x.Field<string>("DP"),
                                                             PX = sdp.PX, //rx, //x.Field<string>("Race"),
                                                             Px_CodeType = sdp.Px_CodeType,
                                                             n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                         }).ToArray();

            var codes = rawResults.GroupBy(g => new { g.PX, g.Px_CodeType,}).Select(r => new { r.Key.PX, r.Key.Px_CodeType}).OrderBy(c => ConverterForRangeSort(c.PX)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => new { r.PX, r.Px_CodeType }).Select((k) =>
                    new
                    {
                        Procedure = k.Key.PX,
                        Procedure_Display = k.Key.PX,
                        Code_Type = k.Key.Px_CodeType,
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Procedure)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx.PX,
                                        Code_Display = rx.PX,
                                        Code_Type = rx.Px_CodeType,
                                        Count = k.Where(x => (x.PX == rx.PX) && (x.Px_CodeType == rx.Px_CodeType)).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => new { g.PX, g.Px_CodeType })
                                          .Select(k => new
                                          {
                                              Procedure = k.Key.PX,
                                              Procedure_Display = k.Key.PX,
                                              Code_Type = k.Key.Px_CodeType,
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Procedure)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
            #endregion
        }
    }

    public class ProcedureValues
    {

        public ProcedureValues()
        {
            CodeType = string.Empty;
            CodeValues = string.Empty;
            SearchMethodType = DTO.Enums.TextSearchMethodType.ExactMatch;
        }


        public string CodeType { get; set; }
        public string CodeValues { get; set; }
        public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }
    }
}
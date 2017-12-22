using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Portal.Areas.DataChecker.Models;
using Lpp.Utilities;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{

    public class DiagnosisController : BaseController
    {
        [HttpGet]
        public ActionResult DiagnosisResponse()
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
                Guid termID = Lpp.QueryComposer.ModelTermsFactory.DC_DiagnosisCodes;

                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == termID)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == termID);
                var termValues = term.Values.First(p => p.Key == "Values");


                DiagnosisValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<DiagnosisValues>(termValues.Value.ToString());

                string[] arrSplit = { "," };
                string[] arrVal = val.CodeValues.Split(arrSplit, StringSplitOptions.RemoveEmptyEntries);

                return Json(arrVal, JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("Dx_CodeType", typeof(string));
            table.Columns.Add("DX", typeof(string));
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
            var diagCodes = dataTable.AsEnumerable().Select(row => row["DX"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();
            var codeTypes = dataTable.AsEnumerable().Select(row => row["Dx_CodeType"].ToStringEx()).Distinct().OrderBy(dp => dp).ToArray();

            var DiagnosisDataPartners = from dp in dataPartners
                                     from dx in diagCodes
                                     from dx_codetype in codeTypes
                                     select new { DX = (string)dx , DP = (string)dp , Dx_CodeType = (string)dx_codetype };
            #region HIDE

            var Diagnosises = from row in dataTable.AsEnumerable()
                              select new DiagnosisItemData { DP = row.Field<string>("DP"), DX = row.Field<string>("DX"), Dx_CodeType = row.Field<string>("Dx_CodeType"), n = row.Field<double?>("n") };
            IEnumerable<DiagnosisItemData> rawResults = (from sdp in DiagnosisDataPartners
                                                      join Diagnosis in Diagnosises
                                                      on new { sdp.DX, sdp.DP, sdp.Dx_CodeType } equals new { Diagnosis.DX, Diagnosis.DP, Diagnosis.Dx_CodeType } into sxs
                                                      from x in sxs.DefaultIfEmpty()
                                                      select new DiagnosisItemData
                                                      {
                                                          DP = sdp.DP, //x.Field<string>("DP"),
                                                          DX = sdp.DX, //rx, //x.Field<string>("Race"),
                                                          Dx_CodeType = sdp.Dx_CodeType,
                                                          n = x == null ? 0 : x.n //x.Field<double?>("Total")
                                                      }).ToArray();

            var codes = rawResults.GroupBy(g => new { g.DX, g.Dx_CodeType }).Select(r => new { r.Key.DX, r.Key.Dx_CodeType }).OrderBy(c => ConverterForRangeSort(c.DX)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => new { r.DX, r.Dx_CodeType }).Select((k) =>
                    new
                    {
                        Diagnosis = k.Key.DX,
                        Diagnosis_Display = k.Key.DX,
                        Code_Type = k.Key.Dx_CodeType,
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.Diagnosis)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx.DX,
                                        Code_Display = rx.DX,
                                        Code_Type = rx.Dx_CodeType,
                                        Count = k.Where(x => (x.DX == rx.DX) && (x.Dx_CodeType == rx.Dx_CodeType)).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => new { g.DX, g.Dx_CodeType })
                                          .Select(k => new
                                          {
                                              Diagnosis = k.Key.DX,
                                              Diagnosis_Display = k.Key.DX,
                                              Code_Type = k.Key.Dx_CodeType,
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.Diagnosis)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
            #endregion
        }
    }

    public class DiagnosisValues
    {

        public DiagnosisValues()
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
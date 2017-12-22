using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class DiagnosisPDXController : BaseController
    {
        [HttpGet]
        public ActionResult DiagnosisPDXResponse()
        {
            return View();
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("PDX", typeof(string));
            table.Columns.Add("EncType", typeof(string));
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
            IEnumerable<PDXItemData> rawResults = (from x in dataTable.AsEnumerable()
                                                   select new PDXItemData
                                                   {
                                                       DP = x.Field<string>("DP"),
                                                       PDX = x.Field<string>("PDX"),
                                                       EncType = x.Field<string>("EncType"),
                                                       n = x.Field<double?>("n")                                                       
                                                   }).ToArray();

            var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var encounterTypes = rawResults.GroupBy(g => g.EncType).Select(r => new { EncType = r.Key, EncType_Display = TranslateEncounter(r.Key) }).OrderBy(r => r.EncType).ToArray();
            var PDXTypes = rawResults.GroupBy(g => g.PDX).Select(r => new { PDX = r.Key, PDX_Display = TranslatePDX(r.Key) }).OrderBy(r => r.PDX).ToArray();
            
            //Overall Metrics
            var overallMetrics = rawResults.GroupBy(g => g.PDX).Select(r => new
            {
                PDX = r.Key,
                PDX_Display = TranslatePDX(r.Key),
                Encounters = encounterTypes.Select(e => new { 
                    EncType = e.EncType,
                    EncType_Display = e.EncType_Display,
                    Total = rawResults.Where(x => x.EncType == e.EncType).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                    Count = r.Where(x => x.EncType == e.EncType).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                })
            }).OrderBy(x => x.PDX).ToArray();

            //Percent within DataPartner
            var percentWithinDataPartner = rawResults.GroupBy(g => g.DP).Select(d => new
            {
                DP = d.Key,
                PDX = PDXTypes.Select(p => new { 
                    PDX = p.PDX,
                    PDX_Display = p.PDX_Display,
                    Encounters = encounterTypes.Select(e => new
                    {
                        EncType = e.EncType,
                        EncType_Display = e.EncType_Display,
                        Total = d.Where(x => x.EncType == e.EncType).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                        Count = d.Where(x => x.PDX == p.PDX && x.EncType == e.EncType).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                    })
                })
            });

            return Json(new { DataPartners = dataPartners, EncounterTypes = encounterTypes, PDXTypes = PDXTypes, OverallMetrics = overallMetrics, PercentWithinDataPartner = percentWithinDataPartner }, JsonRequestBehavior.AllowGet);
        }

        string TranslateEncounter(string value)
        {
            switch (value.ToUpper())
            {
                case "AV":
                    return "Ambulatory Visit (AV)";
                case "ED":
                    return "Emergency Department (ED)";
                case "IP":
                    return "Inpatient Hospital Stay (IP)";
                case "IS":
                    return "Non-Acute Institutional Stay (IS)";
                case "OA":
                    return "Other Ambulatory Visit (OA)";
                case "MISSING":
                    return "Missing";
                case "ALL":
                    return "All Encounters";
            }

            return value;
        }

        string TranslatePDX(string value)
        {
            switch (value.ToUpper())
            {
                case "P":
                    return "Principle";
                case "S":
                    return "Secondary";
                case "X":
                    return "Other PDX";
                case "MISSING":
                    return "Missing";
            }
            return value;
        }
    }

    public class DiagnosisPDXValues
    {

        public DiagnosisPDXValues()
        {
            PDXes = new List<string>();
        }

        public IEnumerable<string> PDXes { get; set; }
    }
}
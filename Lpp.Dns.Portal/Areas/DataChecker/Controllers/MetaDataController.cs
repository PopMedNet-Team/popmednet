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
    public class MetaDataController : BaseController
    {
        [HttpGet]
        public ActionResult MetaDataResponse()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetTermValues(Guid? requestID)
        {
            if (requestID == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            using (var db = new DataContext())
            {
                var req = db.Requests.Find(requestID);
                QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryComposerRequestDTO>(req.Query);
                var criteria = dto.Where.Criteria.Where(c => c.Terms.Any(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_MetadataCompleteness)).FirstOrDefault();
                var term = criteria.Terms.First(t => t.Type == Lpp.QueryComposer.ModelTermsFactory.DC_MetadataCompleteness);
                var termValues = term.Values.First(p => p.Key == "Values");

                MetadataCompletenessValues val = Newtonsoft.Json.JsonConvert.DeserializeObject<MetadataCompletenessValues>(termValues.Value.ToString());

                return Json(val.MetadataCompletenesses.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("ETL", typeof(short));
            table.Columns.Add("DIA_MIN", typeof(DateTime));
            table.Columns.Add("DIA_MAX", typeof(DateTime));
            table.Columns.Add("DIS_MIN", typeof(DateTime));
            table.Columns.Add("DIS_MAX", typeof(DateTime));
            table.Columns.Add("ENC_MIN", typeof(DateTime));
            table.Columns.Add("ENC_MAX", typeof(DateTime));
            table.Columns.Add("ENR_MIN", typeof(DateTime));
            table.Columns.Add("ENR_MAX", typeof(DateTime));
            table.Columns.Add("PRO_MIN", typeof(DateTime));
            table.Columns.Add("PRO_MAX", typeof(DateTime));
            table.Columns.Add("DP_MIN", typeof(DateTime));
            table.Columns.Add("DP_MAX", typeof(DateTime));
            table.Columns.Add("MSDD_MIN", typeof(DateTime));
            table.Columns.Add("MSDD_MAX", typeof(DateTime));

            foreach (var rawRow in records)
            {
                DataRow dr = table.NewRow();

                foreach (var col in rawRow)
                {
                    string key = col.Key;
                    if (key == "DataPartner")
                    {
                        key = "DP";
                    }
                    dr[key] = col.Value;
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

        public JsonResult GetMetrics(DataTable dataTable)
        {
            IEnumerable<MetaDataItemData> rawResults = (from x in dataTable.AsEnumerable()
                                                   select new MetaDataItemData
                                                   {
                                                       DP = x.Field<string>("DP"),
                                                       ETL = x.Field<short?>("ETL"),
                                                       DIA_MIN = x.Field<DateTime?>("DIA_MIN"),
                                                       DIA_MAX = x.Field<DateTime?>("DIA_MAX"),
                                                       DIS_MIN = x.Field<DateTime?>("DIS_MIN"),
                                                       DIS_MAX = x.Field<DateTime?>("DIS_MAX"),
                                                       ENC_MIN = x.Field<DateTime?>("ENC_MIN"),
                                                       ENC_MAX = x.Field<DateTime?>("ENC_MAX"),
                                                       ENR_MIN = x.Field<DateTime?>("ENR_MIN"),
                                                       ENR_MAX = x.Field<DateTime?>("ENR_MAX"),
                                                       PRO_MIN = x.Field<DateTime?>("PRO_MIN"),
                                                       PRO_MAX = x.Field<DateTime?>("PRO_MAX"),
                                                       DP_MIN = x.Field<DateTime?>("DP_MIN"),
                                                       DP_MAX = x.Field<DateTime?>("DP_MAX"),
                                                       MSDD_MIN = x.Field<DateTime?>("MSDD_MIN"),
                                                       MSDD_MAX = x.Field<DateTime?>("MSDD_MAX"),
                                                   }).OrderBy(x => x.DP).ToArray();
            
            return Json(new { Results = rawResults }, JsonRequestBehavior.AllowGet);
        }
    }

    public class MetadataCompletenessValues
    {
        public MetadataCompletenessValues()
        {
            MetadataCompletenesses = new List<int>();
        }
        public IEnumerable<int> MetadataCompletenesses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class SqlDistributionController : BaseController
    {
        [HttpGet]
        public ActionResult SqlResponse()
        {
            return View();
        }

        static DataTable CreateTable(IEnumerable<IDictionary<string, object>> records)
        {
            var table = new DataTable();

            table.Columns.Add("DP", typeof(string));
            table.Columns.Add("NDC", typeof(string));
            table.Columns.Add("LowThreshold", typeof(bool));

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
        public JsonResult GetResponseDataset(Guid responseID)
        {
            var dt = new List<IEnumerable<Dictionary<string, object>>>();
            var ht = new List<QueryComposerResponsePropertyDefinitionDTO>();
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

                dt.Add(rsp.Results.First());
                ht.AddRange(rsp.Properties);
            }



            return Json(new SqlDTO() { Results = dt, Header = ht }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public override JsonResult ProcessMetrics(Guid documentId)
        {
            throw new NotImplementedException();
        }
    }

    public class SqlDTO
    {
        public IEnumerable<IEnumerable<Dictionary<string, object>>> Results { get; set; }
        public IEnumerable<QueryComposerResponsePropertyDefinitionDTO> Header { get; set; }

    }
}
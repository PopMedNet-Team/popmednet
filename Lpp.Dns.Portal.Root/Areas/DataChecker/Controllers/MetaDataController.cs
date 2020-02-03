using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class MetaDataController : BaseController
    {
        public override JsonResult ProcessMetrics(Guid documentId)
        {
            var ds = LoadResults(documentId);
            IEnumerable<MetaDataItemData> rawResults = (from x in ds.Tables[0].AsEnumerable()
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
}

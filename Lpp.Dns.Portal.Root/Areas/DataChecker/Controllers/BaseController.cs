using Lpp.Dns.Model;
using Lpp.Utilities.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;

using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public abstract class BaseController : Controller
    {

        //[Import]
        //public IRepository<DnsDomain, Document> Documents
        //{
        //    get;
        //    set;
        //}

        public abstract JsonResult ProcessMetrics(Guid documentId);

        protected virtual DataSet LoadResults(Guid documentId)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var data = Documents.All.Where(d => d.ID == documentId).Select(d => d.Data).FirstOrDefault();
            //using (var ds = new DataSet())
            //{
            //    using (var ms = new MemoryStream(data))
            //    {
            //        ds.ReadXml(ms);
            //        return ds;
            //    }
            //}
        }

        protected static decimal ConverterForRangeSort(string value)
        {
            decimal d;
            if (decimal.TryParse(value, out d))
                return d;

            if (string.Equals(value, "OTHER", StringComparison.OrdinalIgnoreCase))
                return decimal.MaxValue - 1;
            if (string.Equals(value, "MISSING", StringComparison.OrdinalIgnoreCase))
                return decimal.MaxValue;

            return decimal.MinValue;
        }
        
    }
}
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
        public abstract JsonResult ProcessMetrics(Guid documentId);

        protected virtual DataSet LoadResults(Guid documentId)
        {
           using (var db = new DataContext())
            {
                var ds = new DataSet();            
                using (var stream = new Lpp.Dns.Data.Documents.DocumentStream(db, documentId))
                {
                    ds.ReadXml(stream);
                }
                return ds;                
            }
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
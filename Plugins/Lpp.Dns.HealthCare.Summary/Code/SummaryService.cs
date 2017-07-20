using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using Lpp.Mvc;
using Lpp.Mvc.Composition;
using System.ServiceModel;
using Lpp.Composition;

namespace Lpp.Dns.Models.Medical.Controllers
{
    public class SummaryService
    {
        public static string CreateQuerySQLText( Models.MedicalRequestModel query )
        {
            return "select from";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Dns.Portal.Controllers;
using Lpp.Audit;
using Lpp.Audit.UI;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal.Models
{
    public class LegacyReportModel
    {
        public string DataMart { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<LegacyReportRowModel> Rows { get; set; }
        public string CreatedByUsername { get; set; }
    }

    public class LegacyReportRowModel
    {
        public Request Request { get; set; }
        public PluginRequestType Type { get; set; }
        public string SubmittedByUsername { get; set; }
        public string Status { get; set; }
        public int DaysOpen { get; set; }
        public bool IsWorkflowRequest { get; set; }
        public string RequestTypeName { get; set; }
        public string WorkflowAdapter { get; set; }

    }
}
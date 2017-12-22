using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Portal.Models
{
    public class RequestHeader
    {
        public string Name { get; set; }
        public string PurposeOfUse { get; set; }
        public string PhiDisclosureLevel { get; set; }
        public string Description { get; set; }
        public string AdditionalInstructions { get; set; }
        public Priorities Priority { get; set; }
        public string DueDate { get; set; }
        public Guid? ActivityID { get; set; }
        public Guid? SourceActivityID { get; set; }
        public Guid? SourceActivityProjectID { get; set; }
        public Guid? SourceTaskOrderID { get; set; }
        public string MSRequestID { get; set; }
        public Guid? ReportAggregationLevelID { get; set; }

        [MaxLength(255)]
        public string ActivityDescription { get; set; }

        public Guid? RequesterCenterID { get; set; }
        public Guid? WorkplanTypeID { get; set; }
        public bool MirrorBudgetFields { get; set; }
    }
}
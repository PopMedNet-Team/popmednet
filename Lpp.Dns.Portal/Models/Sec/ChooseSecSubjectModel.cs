using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class ChooseSecSubjectModel
    {
        public IEnumerable<Organization> TopLevelOrganizations { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public string Handle { get; set; }
        public bool GroupsOnly { get; set; }
    }
}
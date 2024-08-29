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
    public class GroupEditModel : ICrudSecObjectEditModel
    {
        public Group Group { get; set; }
        public IEnumerable<OrganizationGroup> Organizations { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public EntitiesForSelectionModel AllOrganizations { get; set; }
        public bool ShowProjects { get; set; }
        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool ShowAcl { get; set; }
    }
}
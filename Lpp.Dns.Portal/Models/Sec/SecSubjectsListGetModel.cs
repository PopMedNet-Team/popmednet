using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Model;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public struct SecSubjectsListGetModel : IListGetModel
    {
        public string OrganizationID { get; set; }
        public string ProjectId { get; set; }

        public string SearchTerm { get; set; }
        public string GroupsOnly { get; set; }
        public bool IsGroupsOnly() { return !GroupsOnly.NullOrEmpty(); }

        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }
    }
}
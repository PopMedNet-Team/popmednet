using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Audit;
using Lpp.Mvc.Controls;
using Lpp.Audit.UI;
using Lpp.Dns.Model;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal.Models
{
    public struct NotificationsGetModel : IListGetModel
    {
        public string Page { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string PageSize { get; set; }

        public string PeriodDaysBack { get; set; }
        public string KindFilter { get; set; }

        public int GetPeriodDaysBack() { return Maybe.Parse<int>( int.TryParse, PeriodDaysBack ).ValueOrDefault( 7 ); }
        public MaybeNotNull<Guid> GetKindFilter() { return Maybe.ParseGuid( KindFilter ); }
    }
}
using System;
using System.Web;
using Lpp.Dns.DTO;
using Lpp.Mvc.Controls;

namespace Lpp.Dns.Portal.Models
{
    public class DataMartsListModel
    {
        public IListModel<DataMartListDTO> DataMarts { get; set; }
        public Func<object, IHtmlString> Suffix { get; set; }
        public string ReloadUrl { get; set; }
        public string HiddenFieldName { get; set; }
        public string AllDataMartIdsExpression { get; set; }
    }
}
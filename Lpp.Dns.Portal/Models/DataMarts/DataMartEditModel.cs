using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using Lpp.Mvc.Controls;
using Lpp.Security.UI;
using Lpp.Security;

namespace Lpp.Dns.Portal.Models
{
    public class DataMartEditModel : ICrudSecObjectEditModel
    {
        public DataMart DataMart { get; set; }
        public SettingsModel Settings { get; set; }
        public bool AllowInstall { get; set; }
        public bool AllowUninstall { get; set; }
        public bool AllowMetadataUpdate { get; set; }
        public IEnumerable<ModelWithMetadata> Models { get; set; }
        public EntitiesForSelectionModel Organizations { get; set; }

        public string OtherDataUpdateFrequency { get; set; }
        public bool EnableOtherClaims { get; set; }

        public AclEditModel Acl { get; set; }

        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowCopy { get; set; }
        public bool ShowAcl { get; set; }

        public string ReturnTo { get; set; }
        public static readonly SelectListItem[] DataUpdateFrequencySelectList =  { 
                new SelectListItem { Text = "None", Value = "None" } ,
                new SelectListItem { Text = "Daily", Value = "Daily" } ,
                new SelectListItem { Text = "Weekly", Value = "Weekly" } ,
                new SelectListItem { Text = "Monthly", Value = "Monthly" } ,
                new SelectListItem { Text = "Quarterly", Value = "Quarterly" } ,
                new SelectListItem { Text = "Semi-annually", Value = "Semi-annually" } ,
                new SelectListItem { Text = "Annually", Value = "Annually" }, 
                new SelectListItem { Text = "Other", Value = "Other" } ,
            };

        public static readonly SelectListItem[] DataModelSelectList =  { 
                new SelectListItem { Text = "None", Value = "None" } ,
                new SelectListItem { Text = "ESP", Value = "ESP" } ,
                new SelectListItem { Text = "HMORN VDW", Value = "HMORN VDW" } ,
                new SelectListItem { Text = "MSCDM", Value = "MSCDM" } ,
                new SelectListItem { Text = "I2b2", Value = "I2b2" } ,
                new SelectListItem { Text = "OMOP", Value = "OMOP" }, 
                new SelectListItem { Text = "Other", Value = "Other" } ,
            };
    }

    public class SettingsModel
    {
        public bool AllowUnattended { get; set; }
        public string UnattendedMode { get; set; }
    }

    public class ModelWithMetadata
    {
        public IDnsModel Model { get; set; }
        public DateTime? LastMetadataRequestTime { get; set; }
        public DateTime? LastMetadataResponseTime { get; set; }
        public Func<HtmlHelper, IHtmlString> ModelConfigView { get; set; }
        public bool AllowConfig { get { return ModelConfigView != null; } }
    }

    public class DataMartPermissionModel
    {
        public string Name { get; set; }
        public Guid ModelID { get; set; }
        public bool IsRequestType { get { return ModelID != default( Guid ); } }
    }
}
using System;
using System.Collections.Generic;
using Lpp.Audit;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class UserSignUpModel : ICrudSecObjectEditModel
    {
        public User User { get; set; }
        public EntitiesForSelectionModel Organizations { get; set; }

        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowCertificate { get; set; }
        public bool ShowAcl { get; set; }
        public bool AllowChangePassword { get; set; }
        public bool AllowChangeLogin { get; set; }
        public bool ShowNotifications { get; set; }
        public bool ShowPasswordExpirationWarning { get; set; }
        public bool HidePasswordFields { get; set; }

        public string ReturnTo { get; set; }
    }
}
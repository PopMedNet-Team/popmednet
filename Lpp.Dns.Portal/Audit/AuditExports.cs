using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Data;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Audit;
using Lpp.Audit.UI;
using System.Net.Mail;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    static class AuditExports
    {
        [Export]
        public static INotificationScheduler<Lpp.Dns.Model.DnsDomain, UserEventSubscription> Scheduler 
        {
            get
            {
                return new SimpleScheduler<Lpp.Dns.Model.DnsDomain, UserEventSubscription>(); 
            } 
        }

        [Export]
        public static IAuditPropertyValueSelector<int> IntValueSelector { get { return new PrimitiveValueSelector<int>(null); } }
        [Export]
        public static IAuditPropertyValueSelector<string> StringValueSelector { get { return new PrimitiveValueSelector<string>(null); } }
        [Export]
        public static IAuditPropertyValueSelector<DateTime> DateValueSelector { get { return new DateValueSelector(null); } }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class AuditPerTransactionExports
    {
        [Import]
        public ICompositionService Composition { get; set; }

        [Export, Export(typeof(PropertyFilterFactory<Lpp.Dns.Model.DnsDomain>))]
        public IAuditEventFilterFactory<Lpp.Dns.Model.DnsDomain> FilterFactory 
        { 
            get
            {
                return Composition.Compose(new PropertyFilterFactory<Lpp.Dns.Model.DnsDomain>(new Guid("{28CAA3E0-A582-46E5-9CCE-43011575F27A}"))); 
            }
        }

        [Export]
        public IAuditEventFilterUIFactory PropertyFilterUIFactory
        {
            get
            {
                return Composition.Compose(new PropertyFilterUIFactory<Lpp.Dns.Model.DnsDomain>(AuditEvents.NewRequest, AuditEvents.RequestStatusChange));
            }
        }

        [Export]
        public IAuditPropertyValueSelector<int> UserValueSelector
        {
            get
            {
                return null;
                //return Composition.Compose(new Controls.UserSelector(
                //    Aud.Property(Expr.Create((Events.RequestStatusChange e) => e.ActingUser)),
                //    Aud.Property(Expr.Create((Events.PasswordNag e) => e.TargetUser))));
            }
        }
    }
}
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
using Lpp.Security;

namespace Lpp.Dns.Portal.AuditPropertyVisualizers
{
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class UserPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, User>
    {
        public UserPropVisualizer()
            : base(u => (u as ISecuritySubject).DisplayName,
                Aud.Property((Events.PasswordNag e) => e.TargetUser),
                Aud.Property((Events.NewRequest e) => e.ActingUser))
        { }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class RequestPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, Request>
    {
        public RequestPropVisualizer() : base(r => r.Name + " (" + r.ID + ")", Aud.Property((Events.RequestStatusChange e) => e.Request)) { }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class ProjectPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, Project>
    {
        public ProjectPropVisualizer() : base(r => r.Name, Aud.Property((Events.RequestStatusChange e) => e.Project)) { }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class RequestTypePropVisualizer : IAuditPropertyValueVisualizer
    {
        [Import]
        public IPluginService Plugins { get; set; }

        static readonly IAuditProperty[] _props = new[] { Aud.Property((Events.RequestStatusChange e) => e.RequestType) };
        public IEnumerable<IAuditProperty> AppliesToProperties { get { return _props; } }

        public Func<HtmlHelper, IHtmlString> Visualize(Audit.Data.AuditPropertyValue v)
        {
            var rt = Plugins.GetPluginRequestType(v.GuidValue);
            if (rt == null) return _ => null;
            return _ => new MvcHtmlString(rt.RequestType.Name);
        }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class DataMartPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, DataMart>
    {
        public DataMartPropVisualizer()
            : base(dm => dm.Name + " (" + dm.ID + ")", Aud.Property((Events.RequestStatusChange e) => e.DataMart))
        { }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class OrganizationPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, Organization>
    {
        public OrganizationPropVisualizer()
            : base(o => o.Name, Aud.Property((Events.OrganizationCrud e) => e.Organization))
        { }
    }

    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class GroupPropVisualizer : EntityReferencePropertyVisualizer<Lpp.Dns.Model.DnsDomain, Group>
    {
        public GroupPropVisualizer()
            : base(g => g.Name, Aud.Property((Events.GroupCrud e) => e.Group))
        { }
    }

    [Export(typeof(IAuditPropertyValueVisualizer))]
    public class CrudEventKindVisualizer : IAuditPropertyValueVisualizer
    {
        static readonly IAuditProperty[] _props = new[] { Aud.Property((Events.UserCrud e) => e.CrudEventKind) };
        public IEnumerable<IAuditProperty> AppliesToProperties { get { return _props; } }

        public Func<HtmlHelper, IHtmlString> Visualize(Audit.Data.AuditPropertyValue v)
        {
            return _ => new MvcHtmlString(((Events.CrudEventKind)v.IntValue).ToString().ToLower());
        }
    }
}
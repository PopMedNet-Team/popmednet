using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Audit;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal.Controllers;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Security;
using Lpp.Security.UI;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export]
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class DnsAclService
    {
        [Import]
        internal ISecurityService<Lpp.Dns.Model.DnsDomain> Security { get; set; }
        [Import]
        internal ISecurityUIService<Lpp.Dns.Model.DnsDomain> SecurityUI { get; set; }
        [Import]
        internal IAuditService<Lpp.Dns.Model.DnsDomain> Audit { get; set; }
        [Import]
        internal IAuthenticationService Auth { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, Organization> Organizations { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, Project> Projects { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, OrganizationSecurityGroup> OrganizationsSecGroups { get; set; }
        //[Import]
        //internal IRepository<DnsDomain, ProjectSecurityGroup> ProjectsSecGroups { get; set; }
        [Import]
        internal IPluginService Plugins { get; set; }

        public Func<HtmlHelper, IHtmlString> AclUIFor(Lpp.Security.SecurityTarget target, string fieldName, Func<HtmlHelper, IJsControlledView> subjectSelector = null)
        {
            return AclUIFor(new[] { target }, fieldName, subjectSelector);
        }

        public Func<HtmlHelper, IHtmlString> AclUIFor(IEnumerable<Lpp.Security.SecurityTarget> targets, string fieldName, Func<HtmlHelper, IJsControlledView> subjectSelector = null)
        {
            return html => html.Render<IAccessControlList>().With(AclModelFor(targets, subjectSelector), fieldName);
        }

        public AclEditModel AclModelFor(Lpp.Security.SecurityTarget target, Func<HtmlHelper, IJsControlledView> subjectSelector = null)
        {
            return AclModelFor(new[] { target }, subjectSelector);
        }

        public AclEditModel AclModelFor(IEnumerable<Lpp.Security.SecurityTarget> targets, Func<HtmlHelper, IJsControlledView> subjectSelector = null)
        {

            foreach (var target in targets)
            {
                var tid = target.Id();
                var targetKind = Security.KindsFor(target).FirstOrDefault();

                foreach (var eventkind in Audit.AllEventKinds.Values)
                {
                    if (eventkind.AppliesTo.ObjectKindsInOrder == targetKind.ObjectKindsInOrder)
                    {
                        System.Diagnostics.Debug.WriteLine("test");
                    }
                }

                var events = (from e in Audit.AllEventKinds.Values
                              where e.AppliesTo == targetKind
                              let t = new Lpp.Security.SecurityTarget(target.Elements.Concat(new[] { e.AsSecurityObject() }))
                              let tKind = new SecurityTargetKind(targetKind.ObjectKindsInOrder.Concat(new[] { SecTargetKinds.AuditEventObjectKind }), new[] { SecPrivilegeSets.Event })
                              select new { e, t, tKind }
                                    ).ToList();
                var evtsAcl = (from e in events
                               let ee = Security.GetAcl(e.t).SkipMembershipImplied().ResolveAcl(Security, e.tKind)
                               from entry in ee
                               select new { tid = e.t.Id(), entry })
                                     .ToLookup(e => e.tid, e => e.entry);
                var evtPrivileges = events.Select(e => TargetPrivilege.Pair(e.t, SecPrivileges.Event.Observe, _ => new MvcHtmlString("Event: " + e.e.Name)));
            }

            var evts = from target in targets
                       let tid = target.Id()
                       let targetKind = Security.KindsFor(target).FirstOrDefault()

                       let events = (from e in Audit.AllEventKinds.Values
                                     where e.AppliesTo == targetKind
                                     let t = new Lpp.Security.SecurityTarget(target.Elements.Concat(new[] { e.AsSecurityObject() }))
                                     let tKind = new SecurityTargetKind(targetKind.ObjectKindsInOrder.Concat(new[] { SecTargetKinds.AuditEventObjectKind }), new[] { SecPrivilegeSets.Event })
                                     select new { e, t, tKind }
                                    ).ToList()
                       let evtsAcl = (from e in events
                                      let ee = Security.GetAcl(e.t).SkipMembershipImplied().ResolveAcl(Security, e.tKind)
                                      from entry in ee
                                      select new { tid = e.t.Id(), entry })
                                     .ToLookup(e => e.tid, e => e.entry)
                       let evtPrivileges = events.Select(e => TargetPrivilege.Pair(e.t, SecPrivileges.Event.Observe, _ => new MvcHtmlString("Event: " + e.e.Name)))
                       select new { evtsAcl, evtPrivileges };
            var evtsAcls = evts.Select(x => x.evtsAcl).Aggregate((a, b) => a.Merge(b));
            var evtsPrivs = evts.SelectMany(x => x.evtPrivileges);

            return null;
        }

        public IEnumerable<SecurityGroup> GetGroupsThatICannotManage(IEnumerable<Guid> groupsThatIWantToManage)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var allowedGroupIds = (from o in Security.AllGrantedObjects(Projects.All, Auth.CurrentUser, SecPrivileges.ManageSecurity)
            //                       from g in o.SecurityGroups
            //                       where groupsThatIWantToManage.Contains(g.SID)
            //                       select g.SID
            //                      )
            //                      .ToList()
            //                      .Concat
            //                      (from o in Security.AllGrantedObjects(Organizations.All, Auth.CurrentUser, SecPrivileges.ManageSecurity)
            //                       from g in o.SecurityGroups
            //                       where groupsThatIWantToManage.Contains(g.SID)
            //                       select g.SID
            //                      )
            //                      .ToList();
            //var disallowedGroupIds = groupsThatIWantToManage.Except(allowedGroupIds).ToList();
            //if (disallowedGroupIds.Any()) return
            //  ProjectsSecGroups.All.Where(g => disallowedGroupIds.Contains(g.SID)).Select(g => new { g.Name, g.SID, p = g.Parent.Name }).ToList().Concat(
            //  OrganizationsSecGroups.All.Where(g => disallowedGroupIds.Contains(g.SID)).Select(g => new { g.Name, g.SID, p = g.Parent.Name })).ToList()
            //  .Select(g => new MockSecurityGroup { Name = g.Name, Parent = new MockNamed { Name = g.p }, SID = g.SID });

            //return Enumerable.Empty<MockSecurityGroup>();
        }

        //class MockSecurityGroup : ISecurityGroup
        //{
        //    public string Name { get; set; }
        //    public MockNamed Parent { get; set; }
        //    INamed ISecurityGroup.Parent { get { return Parent; } }
        //    public Guid SID { get; set; }
        //    public string DisplayName { get { return Name; } }
        //    public SecurityGroupKinds Kind { get { return SecurityGroupKinds.Custom; } }
        //}
        //class MockNamed : INamed { public string Name { get; set; } }

        public void SetAclForTargetAndEvents(Lpp.Security.SecurityTarget target, string strAcl, Guid replaceNullObjectWith)
        {
            //Contract.Requires( target != null );
            SetAclForTargetAndEvents(target, SecurityUI.ParseAcls(strAcl).ReplaceObject(Sec.NullObject, replaceNullObjectWith));
        }

        public void SetAclForTargetAndEvents(IEnumerable<Lpp.Security.SecurityTarget> targets, string strAcl, Guid replaceNullObjectWith)
        {
            //Contract.Requires( targets != null );
            SetAclForTargetAndEvents(targets, SecurityUI.ParseAcls(strAcl).ReplaceObject(Sec.NullObject, replaceNullObjectWith));
        }

        public void SetAclForTargetAndEvents(IEnumerable<Lpp.Security.SecurityTarget> targets, ILookup<BigTuple<Guid>, AclEntry> acl)
        {
            //Contract.Requires( targets != null );
            //Contract.Requires( acl != null );

            foreach (var t in targets) Security.SetAcl(t, acl[t.Id()]);

            foreach (var x in from e in Audit.AllEventKinds.Values

                              join kt in
                                  from t in targets
                                  from k in Security.KindsFor(t)
                                  select new { k, t }
                              on e.AppliesTo equals kt.k

                              group e by kt.t into events
                              let t = events.Key

                              from e in events
                              let eventTarget = new Lpp.Security.SecurityTarget(t.Elements.Concat(new[] { e.AsSecurityObject() }))
                              let eventAcl = acl[eventTarget.Id()]
                              select new { eventTarget, eventAcl })
            {
                Security.SetAcl(x.eventTarget, x.eventAcl);
            }
        }

        public void SetAclForTargetAndEvents(Lpp.Security.SecurityTarget target, ILookup<BigTuple<Guid>, AclEntry> acl)
        {
            //Contract.Requires( acl != null );
            //Contract.Requires( target != null );
            SetAclForTargetAndEvents(new[] { target }, acl);
        }

        public AclEditModel RequestTypesAcl(Project project = null, Organization organization = null, DataMart dataMart = null)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var projObj = project ?? VirtualSecObjects.AllProjects;
            //var orgObj = organization ?? VirtualSecObjects.AllOrganizations;
            //var dmObj = dataMart ?? VirtualSecObjects.AllDataMarts;
            //return new AclEditModel
            //{
            //    Entries = RequestTypesEntriesForEdit(projObj, orgObj, dmObj),
            //    PrivilegeEditors = new[] { RequestTypesPrivilegesEditor(projObj, orgObj, dmObj) },
            //    SubjectSelector = html => project == null ? html.SubjectsSelector(true) : html.ProjectSubjectSelector(project, true)
            //};
        }

        public ILookup<BigTuple<Guid>, AnnotatedAclEntry> RequestTypesEntriesForEdit(ISecurityObject project, ISecurityObject organization, ISecurityObject dataMart, Func<PluginRequestType, bool> filter = null)
        {
            var allReqTypes = Plugins.GetPluginRequestTypes();

            var requestTypeIDs = allReqTypes.Select(a => a.Key).ToArray();

            var result = Security
                .GetAllAcls(SecTargetKinds.RequestTypePerDataMart.ObjectKindsInOrder.Count())
                .WhereFirstIs(project.ID)
                .WhereSecondIs(organization.ID)
                .WhereThirdIs(dataMart.ID)
                .Where(s => requestTypeIDs.Contains(s.TargetId.X3))
                .ToArray();

            var result2 = result
                .Where(e =>
                {
                    var rt = allReqTypes[e.TargetId.X3];
                    return rt != null && (filter == null || filter(rt));
                });

            var result3 = result2
                .ToLookup(
                    e => e.TargetId,
                    e => e.Entries.Where(ee => !ee.ViaMembership).Select(en => Security.ResolveAclEntry(en, SecTargetKinds.RequestTypePerDataMart))
                );

            return result3;
        }

        public RequestTypesAclModel RequestTypesPrivilegesForEdit(ISecurityObject project, ISecurityObject organization, ISecurityObject dataMart, Func<PluginRequestType, bool> filter = null)
        {
            return new RequestTypesAclModel
            {
                Targets = Plugins.GetPluginRequestTypes()
                    .Select(rt => rt.Value)
                    .Where(rt => filter == null || filter(rt))
                    .Select(rt => Pair.Create(Sec.Target(project, organization, dataMart, rt.RequestType.AsSecurityObject()), rt)),

                Privileges = new[] { 
                    Pair.Create( SecPrivileges.RequestType.SubmitManual, "Manual" ),
                    Pair.Create( SecPrivileges.RequestType.SubmitAuto, "Auto" )
                }
            };
        }

        public IJsControlledView RequestTypesPrivilegesEditor(ISecurityObject project, ISecurityObject organization, ISecurityObject dataMart, Func<PluginRequestType, bool> filter = null)
        {
            var mdl = RequestTypesPrivilegesForEdit(project, organization, dataMart, filter);
            return null;
        }

        public void SetRequestTypesAcl(ISecurityObject project, ISecurityObject organization, ISecurityObject dataMart, string acl, Guid replaceNullObjectWith)
        {
            SetRequestTypesAcl(project, organization, dataMart, SecurityUI.ParseAcls(acl).ReplaceObject(Sec.NullObject, replaceNullObjectWith));
        }

        public void SetRequestTypesAcl(ISecurityObject project, ISecurityObject organization, ISecurityObject dataMart, ILookup<BigTuple<Guid>, AclEntry> reqAcls)
        {
            foreach (var rt in Plugins.GetPluginRequestTypes().Values)
            {
                var reqTarget = Sec.Target(project, organization, dataMart, rt.RequestType.AsSecurityObject());
                Security.SetAcl(reqTarget, reqAcls[reqTarget.Id()]);
            }
        }
    }

    public static class AclServiceExtensions
    {
        public static IHtmlString DnsAclUIFor(this HtmlHelper html, Lpp.Security.SecurityTarget target, string fieldName, IJsControlledView subjectSelector = null)
        {
            return html.DnsAclUIFor(new[] { target }, fieldName, subjectSelector);
        }

        public static IHtmlString DnsAclUIFor(this HtmlHelper html, IEnumerable<Lpp.Security.SecurityTarget> targets, string fieldName, IJsControlledView subjectSelector = null)
        {
            var s = html.ViewContext.HttpContext.Composition().Get<DnsAclService>();
            Func<HtmlHelper, IJsControlledView> ss = _ => subjectSelector;
            return s.AclUIFor(targets, fieldName, subjectSelector == null ? null : ss)(html);
        }
    }
}
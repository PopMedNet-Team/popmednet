using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Lpp.Dns.Model;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Security;
using Lpp.Security.UI;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export(typeof(ICloneService<DataMart>)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class CloneDataMartService : CloneService<DataMart>
    {
        readonly string[] skipProperties = new[] {
            "Id",
            "Name",
            "IsDeleted",
            "EffectiveIsDeleted",
            "UnattendedMode",
            "SID",
            "Kind",
            "InstalledModels",
            "Routings",
            "Groups",
            "Projects"
        };

        //[Import]
        //public IRepository<DnsDomain, DataMart> DataMarts { get; set; }

        public override DataMart Clone(DataMart existing)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //DataMart datamart = new DataMart();
            //datamart.Name = CopyName(existing.Name, (index) => DataMarts.All.Count(dm => dm.Name.StartsWith(existing.Name.Substring(0, index))));

            //CopyProperties(existing, datamart, skipProperties);

            //foreach (var installedModel in existing.InstalledModels)
            //{
            //    datamart.InstalledModels.Add(new DataMartInstalledModel { DataMartId = datamart.Id, ModelId = installedModel.ModelId, PropertiesXml = installedModel.PropertiesXml });
            //}

            //SetAcl(existing, datamart);

            //SecurityHierarchy.SetObjectInheritanceParent(datamart, Lpp.Dns.Portal.VirtualSecObjects.AllDataMarts);

            //DataMarts.Add(datamart);

            //UnitOfWork.Commit();

            //return datamart;
        }

        void SetAcl(DataMart existing, DataMart datamart)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //var entries = DnsAclService.RequestTypesEntriesForEdit(Lpp.Dns.Portal.VirtualSecObjects.AllProjects, existing.Organization, existing)
            //                           .Merge(AllTargets(existing).Select(x => Lpp.Security.UI.AccessControlListExtensions.AllEntriesForEdit(SecurityService, x)));

            //var nonInheritedEntries = FilterForNonInheritedEntries(entries).ReplaceObject(existing.SID, datamart.SID);

            //DnsAclService.SetAclForTargetAndEvents(AllTargets(datamart), nonInheritedEntries);

            //DnsAclService.SetRequestTypesAcl(Lpp.Dns.Portal.VirtualSecObjects.AllProjects, datamart.Organization, datamart, nonInheritedEntries);
        }

        static IEnumerable<Lpp.Security.SecurityTarget> AllTargets(DataMart datamart)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //yield return Sec.Target(datamart.Organization, datamart);
            //yield return Sec.Target(Lpp.Dns.Portal.VirtualSecObjects.AllProjects, datamart.Organization, datamart);
            ////yield return Sec.Target(Lpp.Dns.Portal.VirtualSecObjects.AllProjects, datamart.Organization, datamart, Lpp.Dns.Portal.VirtualSecObjects.AllRequests);
        }
    }
}

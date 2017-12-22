using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Reflection.Context;
using Lpp.Composition;
using Lpp.Composition.Modules;
using Lpp.Dns.Data;
using Lpp.Security;
using v = Lpp.Dns.Portal.VirtualSecObjects;

namespace Lpp.Dns.Portal
{
    [Export(typeof(IRootModule))]
    class ObjectProviders : IRootModule
    {
        public IEnumerable<IModule> GetModules()
        {
            yield return new ModuleBuilder()
                .ExportMany<ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain>>(
                    new[] { v.AllUsers, v.AllDataMarts, v.AllGroups, v.AllOrganizations, v.AllProjects, v.Portal, v.AllRequestTypes }
                    .Select(o => new ExplicitSecObjectProvider<Lpp.Dns.Model.DnsDomain>(o))
                    .ToArray()
                )
                .CreateModule();
        }

        // TODO: This is a temporary dirty hack to get around this problem: http://stackoverflow.com/questions/13520227
        // Once the problem is resolved, we'll use ExportMany instead.
        [PartMetadata(ExportScope.Key, TransactionScope.Id)]
        class RepoProviders
        {
            [Import]
            internal ICompositionService Comp { get; set; }

            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Users { get { return e<User>(User.ObjectKind); } }
            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Dms { get { return e<DataMart>(DataMart.ObjectKind); } }
            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Orgs { get { return e<Organization>(Organization.ObjectKind); } }
            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Groups { get { return e<Group>(Group.ObjectKind); } }
            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Projs { get { return e<Project>(Project.ObjectKind); } }
            [Export]
            public ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> Registries { get { return e<Registry>(Registry.ObjectKind); } }

            ISecurityObjectProvider<Lpp.Dns.Model.DnsDomain> e<T>(SecurityObjectKind kind) where T : class, ISecurityObject
            {
                return Comp.Compose(new RepositorySecObjectProvider<Lpp.Dns.Model.DnsDomain, T>(kind));

                //return Comp.Compose(new RepositorySecObjectProvider<DnsDomain, T>(kind));
            }
        }
    }
}
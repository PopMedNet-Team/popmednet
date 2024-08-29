using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Lpp.Audit;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Security;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [Export(typeof(ITaskService)), PartMetadata(ExportScope.Key, TransactionScope.Id)]
    internal class TaskService : ITaskService
    {
        const int MaxRequestNameLength = 50;

        [Import]
        public IPluginService Plugins { get; set; }
        //[Import]
        //public IRepository<DnsDomain, User> Users { get; set; }
        //[Import]
        //public IRepository<DnsDomain, Organization> Orgs { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public ISecurityService<Lpp.Dns.Model.DnsDomain> Security { get; set; }

        protected static bool CanApproveRegistration(User e, IAuthenticationService Auth, ISecurityService<Lpp.Dns.Model.DnsDomain> Security) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Auth.CurrentUser != e && Security.HasPrivilege(Sec.Target(e.Organization), Auth.CurrentUser, SecPrivileges.Organization.ApproveRejectRegistrations); 
        }

        public IQueryable<User> GetTasks()
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //// Show only undeleted users who signed up themselves (not created by admin) and from the organizations for which the current user
            //// has authorization to approve/reject registrants.
            //return
            //       from u in Users.All
            //       join o in Security.AllGrantedObjects(Orgs.All, Auth.CurrentUser, SecPrivileges.Organization.ApproveRejectRegistrations)
            //       on u.Organization equals o
            //       where u.SignupDate != null && !u.EffectiveIsDeleted
            //       select u;
        }
    }
}
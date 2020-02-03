using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Lpp.Dns.Data;
using System.Configuration;
using Lpp.Security;
using Lpp.Composition;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    [Export]
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class UsersService
    {
        //[Import]
        //internal IRepository<DnsDomain, UserPasswordTrace> PasswordTraces { get; set; }
        [Import]
        internal IAuthenticationService Auth { get; set; }
        [Import]
        internal ISecurityService<Lpp.Dns.Model.DnsDomain> Security { get; set; }
        [Import]
        internal ISecurityObjectHierarchyService<Lpp.Dns.Model.DnsDomain> SecHierarchy { get; set; }
        [Import]
        internal ISecurityMembershipService<Lpp.Dns.Model.DnsDomain> Membership { get; set; }


        [ImportMany]
        internal IEnumerable<IPasswordRule> PasswordRules { get; set; }
        [ImportMany]
        internal IEnumerable<IPermissionsTemplate<User>> PermissionTemplates { get; set; }

        const int PasswordReuseHistoryLength = 5;
        public DnsResult ChangePassword(User u, string newPassword, bool verifyOnly = false, bool isNewUser = false)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //if (!isNewUser && Auth.CurrentUser != u && !Security.HasPrivilege(Sec.Target(u.Organization, u), Auth.CurrentUser, SecPrivileges.User.ChangePassword)) return DnsResult.Failed("Access Denied on changing password");
            //if (newPassword.NullOrEmpty()) return DnsResult.Failed("Password cannot be empty");

            //var errors = PasswordRules.Where(r => !r.Holds(newPassword)).Select(r => r.Name);
            //var hash = Password.ComputeHash(newPassword);
            //if (hash == u.Password ||
            //    u.UserPasswordTraces.OrderByDescending(t => t.DateAdded).Take(PasswordReuseHistoryLength - 1).Any(t => t.Password == hash))
            //{
            //    errors = errors.StartWith("Cannot reuse any of the past " + PasswordReuseHistoryLength + " passwords");
            //}

            //var result = new DnsResult { ErrorMessages = errors.ToList() };
            //if (verifyOnly || !result.IsSuccess) return result;

            //if (!u.Password.NullOrEmpty())
            //{
            //    var tr = PasswordTraces.Add(new UserPasswordTrace { User = u, DateAdded = DateTime.Now, AddedByUser = Auth.CurrentUser, Password = u.Password });
            //    u.UserPasswordTraces.Add(tr);
            //    u.UserPasswordTraces
            //        .OrderByDescending(t => t.DateAdded)
            //        .Skip(PasswordReuseHistoryLength).ToList()
            //        .ForEach(PasswordTraces.Remove);
            //}

            //u.Password = Password.ComputeHash(newPassword);
            //u.PasswordExpiration = DateTime.Now.AddMonths(int.Parse(ConfigurationManager.AppSettings["ConfiguredPasswordExpiryMonths"]));

            //return DnsResult.Success;
        }

        /// <summary>
        /// For a user to work properly, three conditions have to be enforced at all times.
        /// 
        /// 1. Membership in the Everyone group of the user’s own org.
        /// 2. Inheritance from “All Users”.
        /// 3. Permission to observe “Personal Events” on himself
        /// 
        /// This method puts these conditions in place for the given user and should be called
        /// on all newly created user.
        /// </summary>
        /// <param name="u">a new user</param>
        public void PrepareNewUser(User u)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //// Adding the user to the “Everyone” group will make it able to have all permissions specified at the Org level and at global level,
            //// such as the Login permission.
            //(from everyone in u.Organization.SecurityGroups.Where(g => g.Kind == SecurityGroupKinds.Everyone).MaybeFirst()
            // from userGroups in Membership.GetSubjectParents(u)
            // where !userGroups.Any(g => g.Id == everyone.SID)
            // select Maybe.Do(() => Membership.SetSubjectParents(u, userGroups.Select(g => g.Resolve()).StartWith(everyone)))
            //)
            //.ValueOrDefault();

            //// Let the security subsystem know that this user should “inherit” permissions from the virtual “All Users” object.
            //// This gives "Administrators" rights to read this user among other things.
            //SecHierarchy.SetObjectInheritanceParent(u, VirtualSecObjects.AllUsers);

            //PermissionTemplates.GetDefaultPermissions(u).ForEach(x => Security.SetAcl(x.Key, x));
        }

        public static int FailedLoginAttemptsBeforeLockingOut
        {
            get
            {
                return (from cfg in Maybe.Value(ConfigurationManager.AppSettings["FailedLoginAttemptsBeforeLockingOut"])
                        from i in int.Parse(cfg)
                        select i
                        )
                        .Catch()
                        .AsNullable()
                        ??
                        5;
            }
        }

        public static bool IsLocked(User u)
        {
            return u.FailedLoginCount >= FailedLoginAttemptsBeforeLockingOut;
        }

        public PasswordScore PasswordStrength(String password)
        {
            /*SCORING LOGIC.
             * Divide 1 by number of password rules. The result is RequirementScore.
             * Get the password-score using Password.Strength. This is StrengthScore
             * Take the smaller of RequirementScore,StrengthScore.
             * Project it to appropriate strength string.
             */

            double requirementScore = 0d; double strengthScore = 0d;

            int countRules = PasswordRules.Count(); int passedRules = countRules;
            if (countRules == 0)
            {
                requirementScore = 1;
            }
            else
            {
                var errors = PasswordRules.Where(r => !r.Holds(password)).Select(r => r.Name);
                if (errors != null) passedRules = countRules - errors.Count();
                requirementScore = (double)passedRules / countRules;
            }

            strengthScore = Password.Strength(password);
            var score = Math.Min(requirementScore, strengthScore);

            PasswordScore[] passwordStrengthVerdicts = new[] { PasswordScore.VeryWeak, PasswordScore.Weak, PasswordScore.Average, PasswordScore.Strong, PasswordScore.VeryStrong };
            score = score * passwordStrengthVerdicts.Length;

            int passwordVerdictIndex = Math.Min((int)score, passwordStrengthVerdicts.Length - 1);
            /* If resultScore is highest but requirementScore is less than 1. It means all password rules have not been met. Hence reduce strength by one level*/
            if (passwordVerdictIndex == (passwordStrengthVerdicts.Length - 1) && requirementScore < 1) passwordVerdictIndex -= 1;

            PasswordScore resultScore = passwordStrengthVerdicts[passwordVerdictIndex];

            return resultScore;
        }
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Average = 3,
            Strong = 4,
            VeryStrong = 5
        }
    }
}
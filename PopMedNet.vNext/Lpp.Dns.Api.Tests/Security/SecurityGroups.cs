using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Lpp.Objects;
using Lpp.Dns.Data;

namespace Lpp.Dns.Api.Tests.Security
{
    [TestClass]
    public class SecurityGroups
    {
        [TestMethod]
        public void SelectAllSecurityGroups()
        {
            using (var dataContext = new DataContext())
            {
                var securityGroups = (from s in dataContext.SecurityGroups orderby s.Name, s.Name select s).ToArray();

                Console.WriteLine("Security Groups count: " + securityGroups.Length);
                foreach(var sg in securityGroups)
                    Console.WriteLine( sg.ParentSecurityGroup.Name + "-" + sg.Name);
            }
        }

        [TestMethod]
        public void UpdateUser()
        {
            using (var db = new DataContext())
            {
                var user = (from u in db.Users where u.UserName == "SystemAdministrator" select u).SingleOrDefault();

                Console.WriteLine("User found, ID: " + user.ID);

                user.FailedLoginCount += 1;

                Console.WriteLine("increment failed login count to: " + user.FailedLoginCount);

                db.SaveChanges();
            }
        }
    }
}

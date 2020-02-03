using Lpp.Dns.Data;
using Lpp.Utilities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Tests.Projects
{
    [TestClass]
    public class CreateDataMartsUtility
    {
        static readonly Guid ProjectID = new Guid("17598FF8-7668-4C58-8225-A377011CD582");
        static readonly Guid OrganizationID = new Guid("1E25A5C8-BB7F-4C16-A849-A3850108AB0D");
        static readonly Guid DefaultSecurityGroupID = new Guid("5C3A0001-60E5-4306-AE1C-A2F9013FFC69");
        static readonly Guid RequestTypeID = new Guid("0F1EA011-B588-4775-9E16-CB6DBE12F8BE");//Data Checker: NDC
        const string DefaultName = "DM-";
        static readonly Guid IdentityID = new Guid("2CBF97E0-FF50-496A-8F77-A57DA62DAC05");//systemadministrator
        static readonly Guid DataMartTypeID = new Guid("346B0001-4380-4D4A-80B4-A3770152D4D8");
        const int BatchAmount = 20;

        [TestMethod]
        public void CreateDataMart()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;   
            DataContext db = new DataContext(connectionString);
            try
            {
                var user = db.Users.Single(u => u.ID == IdentityID);
                System.Threading.Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(new ApiIdentity(IdentityID, user.UserName, user.FullName, user.OrganizationID), null);
                
                Guid modelID = db.RequestTypeDataModels.Where(m => m.RequestTypeID == RequestTypeID).Select(m => m.DataModelID).FirstOrDefault();

                int index = db.DataMarts.Where(dm => dm.Name.StartsWith(DefaultName)).Count();

                for (int i = 1; i <= BatchAmount; i++)
                {

                    var datamart = db.DataMarts.Add(new DataMart
                    {
                        Name = DefaultName + (index + i) + " with the name really long to make it wrap in the add datamart list, like really really really long",
                        Acronym = DefaultName + (index + i),
                        OrganizationID = OrganizationID,
                        DataMartTypeID = DataMartTypeID
                    });

                    datamart.Models.Add(new DataMartInstalledModel
                    {
                        DataMartID = datamart.ID,
                        ModelID = modelID
                    });

                    db.ProjectDataMarts.Add(new ProjectDataMart
                    {
                        DataMartID = datamart.ID,
                        ProjectID = ProjectID
                    });

                    db.ProjectDataMartRequestTypeAcls.Add(new AclProjectDataMartRequestType
                    {
                        DataMartID = datamart.ID,
                        Permission = DTO.Enums.RequestTypePermissions.Manual,
                        ProjectID = ProjectID,
                        RequestTypeID = RequestTypeID,
                        SecurityGroupID = DefaultSecurityGroupID,
                        Overridden = true
                    });
                }

                db.SaveChanges();

            }
            finally
            {
                db.Dispose();
            }
        }
    }
}

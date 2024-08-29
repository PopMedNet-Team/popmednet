using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using PopMedNet.Dns.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Utilities.Security;
using System.Security.AccessControl;
using LinqKit;
using PopMedNet.Objects;

namespace PopMedNet.Dns.Api.Test
{
    [TestClass]
    public class Data_DataContext : IDisposable
    {
        bool _disposed = false;
        readonly ServiceProvider _serviceProvider;
        readonly string _connectionString;
        readonly ApiIdentity AdminIdentity = new ApiIdentity(new Guid("96dc0001-94f1-47cc-bfe6-a22201424ad0"), "admin", "System Administrator");

        public Data_DataContext()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            _connectionString = config.GetConnectionString("DataContext");
            services.AddDbContext<DataContext>(options =>
            {
                options.EnableDetailedErrors(true).EnableSensitiveDataLogging(true);
                options.UseSqlServer(_connectionString).LogTo(l =>
                {
                    if (!l.StartsWith("dbug"))
                        Console.WriteLine(l);
                });
                options.ConfigureWarnings(b => b.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.MultipleNavigationProperties));
            });

            //services.AddAutoMapper((config) => {
            //    config.CreateMap<DateTime, DateTimeOffset?>();
            //}
            //, typeof(PopMedNet.Dns.Data.DataContext).Assembly);

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _serviceProvider.Dispose();
            }
        }

        [TestMethod]
        public async Task LoadUsers()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var user = await db.Users.Include(u => u.Organization!)
                                         .Include(u => u.RejectedBy!)
                                         .Include(u => u.DeactivatedBy!).FirstAsync();
                Assert.IsNotNull(user);

                System.Console.WriteLine(user.FullName + " Organization:" + user.Organization!.Name + " Deactivated?:" + user.DeactivatedByID.HasValue + " Rejected?:" + user.RejectedByID.HasValue);

            }
        }

        [TestMethod]
        public async Task MapUserToDTO()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var user = await db.Users.Include(u => u.Organization!)
                                         .Include(u => u.RejectedBy!)
                                         .Include(u => u.DeactivatedBy!).FirstAsync();
                Assert.IsNotNull(user);

                var mapConfig = new AutoMapper.MapperConfiguration(config =>
                    config.AddProfile<UserMappingProfile>()
                );
                var mapper = mapConfig.CreateMapper();

                var dto = mapper.Map<DTO.UserDTO>(user);

                Assert.IsNotNull(dto);

                Console.WriteLine(JsonSerializer.Serialize(dto));
            }
        }

        [TestMethod]
        public async Task TableValueFunctionFromDbContext()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();


                var user = await db.Users.FirstAsync(u => u.UserName == "admin");
                Assert.IsNotNull(user);

                var securityGroups = await db.FilteredSecurityGroups(user.ID).ToArrayAsync();

                Console.WriteLine("Number of security groups: " + securityGroups.Length);
                foreach (var sg in securityGroups)
                {
                    Console.WriteLine(sg.Name);
                }
            }
        }

        [TestMethod]
        public async Task ConfirmSecurityGroupSecure()
        {
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<DataContext>();


            //    var user = await db.Users.FirstAsync(u => u.UserName == "admin");
            //    Assert.IsNotNull(user);

            //    var apiIdentity = new Utilities.Security.ApiIdentity(user.ID, user.UserName, user.FullName, user.OrganizationID);

            //    var result = from sg in db.Secure<SecurityGroup>(apiIdentity) select sg;

            //}

            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                var user = await db.Users.FirstAsync(u => u.UserName == AdminIdentity.UserName);
                Assert.IsNotNull(user);

                var apiIdentity = new Utilities.Security.ApiIdentity(user.ID, user.UserName, user.FullName, user.OrganizationID);

                // var result = (from sg in db.Secure<SecurityGroup>(apiIdentity) select sg).ToArrayAsync();

                //var groups = await (from sg in db.Secure<SecurityGroup>(apiIdentity)
                //                    where (sg.Type == DTO.Enums.SecurityGroupTypes.Organization && !db.Organizations.Where(o => o.ID == sg.OwnerID).FirstOrDefault().Deleted) || !db.Projects.Where(p => p.ID == sg.OwnerID).FirstOrDefault().Deleted
                //                    group sg by sg.Type into g
                //                    select new
                //                    {
                //                        Type = g.Key,
                //                        Parents = (from p in g
                //                                   group p by new { p.OwnerID, p.Owner } into parent
                //                                   select new
                //                                   {
                //                                       OwnerID = parent.Key.OwnerID,
                //                                       Owner = parent.Key.Owner,
                //                                       Groups = (from s in parent
                //                                                 orderby s.Name
                //                                                 select s)
                //                                   })
                //                    }
                //         ).ToArrayAsync();

                var sgroups = await (from sg in db.Secure<SecurityGroup>(apiIdentity)
                                     where (sg.Type == DTO.Enums.SecurityGroupTypes.Organization && db.Organizations.Where(o => o.ID == sg.OwnerID && o.Deleted).Count() == 0) || (db.Projects.Where(p => p.ID == sg.OwnerID && p.Deleted).Count() == 0)
                                     select sg).ToArrayAsync();

                var groups = sgroups.GroupBy(s => s.Type)
                        .Select(k => new
                        {
                            Type = k.Key,
                            Parents = from p in k
                                      group p by new { p.OwnerID, p.Owner } into parent
                                      select new
                                      {
                                          OwnerID = parent.Key.OwnerID,
                                          Owner = parent.Key.Owner,
                                          Groups = (from s in parent
                                                    orderby s.Name
                                                    select s)
                                      }
                        }).ToArray();

                Console.WriteLine(groups.Length);
            }

        }

        [TestMethod]
        public async Task QueryRegistryAndItems()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                //var registries = db.Registries.ToArray();
                var q = from d in db.RegistryItemDefinitions
                        where d.Registries.Any(r => r.ID == new Guid("F8786BD6-3284-4690-9EBE-A3B000E0C912"))
                        select new RegistryItemDefinitionDTO
                        {
                            ID = d.ID,
                            Category = d.Category,
                            Title = d.Title
                        };

                var result = await q.ToArrayAsync();
            }
        }

        [TestMethod]
        public async Task QueryNetworkMessages()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var mapConfig = new AutoMapper.MapperConfiguration(config =>
                    config.AddProfile<NetworkMessageMappingProfile>()
                );
                var mapper = mapConfig.CreateMapper();

                //var q = from n in db.NetworkMessages
                //        let networkUsers = db.NetworkMessageUsers.Where(nu => nu.NetworkMessageID == n.ID).AsEnumerable()
                //        where networkUsers.Count() == 0
                //        select n;



                var q = from l in db.Secure<NetworkMessage>(AdminIdentity)
                        let userID = AdminIdentity.ID
                        where
                        db.NetworkMessageUsers.Where(nu => nu.NetworkMessageID == l.ID).Any() == false
                        ||
                        db.NetworkMessageUsers.Where(nu => nu.NetworkMessageID == l.ID && nu.UserID == userID).Any()
                        select l;

                var result = q.ProjectTo<NetworkMessageDTO>(mapper.ConfigurationProvider).OrderByDescending(n => n.CreatedOn);



                //var dto = mapper.Map<DTO.UserDTO>(user);

                foreach (var r in result)
                {
                    Console.WriteLine(r.MessageText);
                }
            }
        }

        [TestMethod]
        public void GetUserEnvironments()
        {
            var userID = new Guid("92feceaf-52c6-41c1-819c-a3cb014e59d5");


            //var userID = new Guid("7FC31EA0-2537-4075-849B-A39C0126AA14");
            //var Identity = new ApiIdentity(new Guid("7FC31EA0-2537-4075-849B-A39C0126AA14"), "lakshmi", "Lakshmi");


            using (var scope = _serviceProvider.CreateScope())
            {
                var DataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                // var q = (from audit in DataContext.LogsUserAuthentication
                //          join u in DataContext.Users on audit.UserID equals u.ID
                //          let permissionID = PermissionIdentifiers.User.View.ID
                //          let identityID = Identity.ID
                //          let secGrps = DataContext.SecurityGroupUsers.Where(x => x.UserID == identityID).Select(x => x.SecurityGroupID).AsEnumerable()
                //          let gAcls = DataContext.GlobalAcls.Where(x => secGrps.Contains(x.SecurityGroupID) && x.PermissionID == permissionID).AsEnumerable()
                //          let oAcls = DataContext.OrganizationAcls.Where(x => secGrps.Contains(x.SecurityGroupID) && x.PermissionID == permissionID && u.OrganizationID == x.OrganizationID).AsEnumerable()
                //          let uAcls = DataContext.UserAcls.Where(x => secGrps.Contains(x.SecurityGroupID) && x.PermissionID == permissionID && u.ID == x.UserID).AsEnumerable()

                //          where audit.UserID == userID && (identityID == audit.UserID ||
                //          (
                //              (gAcls.Any() || oAcls.Any() || uAcls.Any())
                //              &&
                //              (gAcls.All(a => a.Allowed) && oAcls.All(a => a.Allowed) && uAcls.All(a => a.Allowed))

                //          ))
                //          select new UserAuthenticationDTO
                //          {
                //              Environment = string.IsNullOrEmpty(audit.Environment) ? "" : audit.Environment!
                //          }
                //).Distinct().OrderBy(a => a).ToArray();

                var q = from ua in DataContext.LogsUserAuthentication
                        let permissionID = PermissionIdentifiers.User.View.ID
                        let identityID = AdminIdentity.ID
                        let gAcls = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID).AsEnumerable()
                        let oAcls = DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID && DataContext.Users.Where(u => u.ID == ua.UserID && a.OrganizationID == u.OrganizationID).Any()).AsEnumerable()
                        let uAcls = DataContext.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID && a.UserID == ua.UserID).AsEnumerable()
                        where ua.UserID == userID &&
                        (identityID == ua.UserID ||
                          (
                            (gAcls.Any() || oAcls.Any() || uAcls.Any())
                            &&
                            (gAcls.All(a => a.Allowed) && oAcls.All(a => a.Allowed) && uAcls.All(a => a.Allowed))
                          )
                        )
                        select ua.Environment;

                var results = q.Distinct().OrderBy(a => a).Select(a => new UserAuthenticationDTO { Environment = a }).ToArray();

                Console.WriteLine("Result Count:" + results.Length);

                foreach (var r in results)
                {
                    Console.WriteLine("    " + r.Environment);
                }
            }
        }

        [TestMethod]
        public void SecureRequest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var DataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                var p = new QueryParameter
                {
                    ID = AdminIdentity.ID,
                    EditPermissionID = PermissionIdentifiers.Request.Edit.ID,
                    EditMetadataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                };

                var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(p.ID, Array.Empty<Guid>());
                var projectAcls = DataContext.ProjectAcls.FilterAcl(p.ID, Array.Empty<Guid>());

                bool canEditRequestMetadata = (
                                                from r in DataContext.Secure<Request>(AdminIdentity).AsNoTrackingWithIdentityResolution()
                                                    let identityID = p.ID
                                                    let editPermissionID = p.EditPermissionID
                                                    let editRequestMetaDataPermissionID = p.EditMetadataPermissionID
                                                let gAcl = DataContext.AclGlobalFiltered(identityID, editPermissionID).AsEnumerable()
                                                let pAcl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editPermissionID)
                                                let p2Acl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editRequestMetaDataPermissionID)
                                                let poAcl = projectOrganizationAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID && a.PermissionID == editPermissionID)
                                                where (gAcl.Any() || pAcl.Any() || poAcl.Any()) &&
                                                (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))
                                                && ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                                                select r).Any();
                Console.WriteLine("Result of original query:" + canEditRequestMetadata);

                //var permissions = DataContext.GlobalAcls.Select(a => new Tuple<Guid, Guid, bool>(a.PermissionID, a.SecurityGroupID, a.Allowed))
                //    .Union(DataContext.ProjectAcls.Select(a => new Tuple<Guid, Guid, bool>(a.PermissionID, a.SecurityGroupID, a.Allowed)))
                //    .Union(DataContext.ProjectOrganizationAcls.Select(a => new Tuple<Guid, Guid, bool>(a.PermissionID, a.SecurityGroupID, a.Allowed)))
                //    .Where(a => a.Item1 == PermissionIdentifiers.Request.Edit.ID && DataContext.SecurityGroupUsers.Any(sgu => sgu.UserID == AdminIdentity.ID && sgu.SecurityGroupID == a.Item2));

                bool query2 = (from r in DataContext.Secure<Request>(AdminIdentity).AsNoTrackingWithIdentityResolution()
                               let userID = AdminIdentity.ID
                               let editPermissionID = p.EditPermissionID
                               let editRequestMetaDataPermissionID = p.EditMetadataPermissionID
                               let perms = DataContext.GlobalAcls.Select(a => new AclProjectOrganization { OrganizationID = Guid.Empty, ProjectID = Guid.Empty, PermissionID = a.PermissionID, Allowed = a.Allowed, SecurityGroupID = a.SecurityGroupID })
                     .Union(DataContext.ProjectAcls.Select(a => new AclProjectOrganization { OrganizationID = Guid.Empty, ProjectID = a.ProjectID, PermissionID = a.PermissionID, Allowed = a.Allowed, SecurityGroupID = a.SecurityGroupID }))
                     .Union(DataContext.ProjectOrganizationAcls.Select(a => new AclProjectOrganization { OrganizationID = a.OrganizationID, ProjectID = a.ProjectID, PermissionID = a.PermissionID, Allowed = a.Allowed, SecurityGroupID = a.SecurityGroupID }))
                     .Where(a => (a.PermissionID == editPermissionID || a.PermissionID == editRequestMetaDataPermissionID ) && DataContext.SecurityGroupUsers.Any(sgu => sgu.UserID == userID && sgu.SecurityGroupID == a.SecurityGroupID) && (a.OrganizationID == Guid.Empty || a.OrganizationID == r.OrganizationID) && (a.ProjectID == Guid.Empty || a.ProjectID == r.ProjectID)).AsEnumerable()
                               where perms.Any(a => a.PermissionID == editPermissionID) && perms.All(a => a.Allowed && a.PermissionID == editPermissionID)
                                    //&& ((int)r.Status < 500 ? true : (perms.Any(a => a.PermissionID == editRequestMetaDataPermissionID) && perms.All(a => a.Allowed && a.PermissionID == editRequestMetaDataPermissionID)))

                               select r).TagWith("Union of permissions before predicate.").Any();

                Console.WriteLine("Result of unioned query:" + query2);

            }
        }

        [TestMethod]
        public void ProjectRequest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var DataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var mapConfig = new AutoMapper.MapperConfiguration(config => config.AddProfile<RequestMappingProfile>());
                var mapper = mapConfig.CreateMapper();

                var dto = DataContext.Secure<Request>(AdminIdentity).Where(r => r.Status == DTO.Enums.RequestStatuses.Submitted).OrderByDescending(r => r.CreatedOn).ProjectTo<DTO.RequestDTO>(mapper.ConfigurationProvider).First();
                                
                Console.WriteLine(JsonSerializer.Serialize(dto,new JsonSerializerOptions { WriteIndented = true }));
            }
        }
    }

    internal struct QueryParameter
    {
        public Guid ID;
        public Guid EditPermissionID;
        public Guid EditMetadataPermissionID;
    }
}
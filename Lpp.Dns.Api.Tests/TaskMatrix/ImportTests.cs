using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;

namespace Lpp.Dns.Api.Tests.TaskMatrix
{
    [TestClass]
    public class ImportActivitiesTests
    {
        static ImportActivitiesTests()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        [TestMethod]
        public void ReadFromFile()
        {
            IEnumerable<Lpp.Dns.DTO.TaskOrderImportDTO> tasks = null;

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            var serializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);
            using (var stream = typeof(ImportActivitiesTests).Assembly.GetManifestResourceStream("Lpp.Dns.Api.Tests.TaskMatrix.task_order.json"))
            using (var jStream = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
            {
                tasks = serializer.Deserialize<IEnumerable<Lpp.Dns.DTO.TaskOrderImportDTO>>(jStream);
            }

            Console.WriteLine("Number of tasks found:" + tasks.Count());

            foreach (var taskOrder in tasks)
            {
                Console.WriteLine(taskOrder.ID);
                foreach (var activity in taskOrder.Activities)
                {
                    Console.WriteLine("\t " + activity.Name + " (acronym: " + activity.Acronym + ")");
                    foreach (var subactivity in taskOrder.Activities)
                    {
                        Console.WriteLine("\t\t " + subactivity.Name + " (acronym: " + subactivity.Acronym + ")");
                    }
                }
            }
        }

        [TestMethod]
        public void PrepareCredentials()
        {
            var username = Utilities.Crypto.EncryptString("");
            var password = Utilities.Crypto.EncryptString("");

            Console.WriteLine("username: " + username);
            Console.WriteLine("password: " + password);
        }

        [TestMethod]
        public void DumpFromExisting()
        {
            //pull from QA4dns3_QA_Starter, values have been converted to new format
            string connectionString = "";
            Guid projectID = new Guid("");
            using (var db = new DataContext(connectionString))
            {
                var activities = db.Activities.Where(a => a.ProjectID == projectID).ToArray();

                var taskOrders = from to in activities
                                 where to.ParentActivityID == null
                                 orderby to.Name
                                 select new ActivityDTO
                                 {
                                     ProjectID = to.ProjectID,
                                     TaskLevel = to.TaskLevel,
                                     ID = to.ID,
                                     Name = to.Name,
                                     Acronym = to.Acronym,
                                     Description = to.Description,
                                     DisplayOrder = to.DisplayOrder,
                                     ParentActivityID = to.ParentActivityID,
                                     Deleted = to.Deleted,
                                     Activities = activities.Where(a => a.ParentActivityID == to.ID).OrderBy(a => a.Name)
                                                            .Select(activity => new ActivityDTO
                                                            {
                                                                ProjectID = activity.ProjectID,
                                                                TaskLevel = activity.TaskLevel,
                                                                ID = activity.ID,
                                                                Name = activity.Name,
                                                                Acronym = activity.Acronym,
                                                                Description = activity.Description,
                                                                DisplayOrder = activity.DisplayOrder,                                                                
                                                                ParentActivityID = activity.ParentActivityID,
                                                                Deleted = activity.Deleted,                                                                
                                                                Activities = activities.Where(aa => aa.ParentActivityID == activity.ID).OrderBy(aa => aa.Name)
                                                                                       .Select(activityProject => new ActivityDTO
                                                                                       {
                                                                                           ProjectID = activityProject.ProjectID,
                                                                                           TaskLevel = activityProject.TaskLevel,
                                                                                           ID = activityProject.ID,
                                                                                           Name = activityProject.Name,
                                                                                           Acronym = activityProject.Acronym,
                                                                                           Description = activityProject.Description,
                                                                                           DisplayOrder = activityProject.DisplayOrder,
                                                                                           ParentActivityID = activityProject.ParentActivityID,
                                                                                           Deleted = activityProject.Deleted                                                                                           
                                                                                       })
                                                            })
                                 };

                using (var writer = new System.IO.StreamWriter("activities_local_" + DateTime.Now.ToString("MMddyyyy_hh:mm:ss") + ".json"))
                {
                    var ss = new Newtonsoft.Json.JsonSerializerSettings();
                    ss.Formatting = Newtonsoft.Json.Formatting.Indented;

                    var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(ss);
                    serializer.Serialize(writer, taskOrders);
                }

            }

        }

        [TestMethod]
        public void DumpFromOldDb()
        {
            List<DTO.ActivityDTO> activities = new List<ActivityDTO>();
            string connectionstring = "";

            using (var conn = new System.Data.SqlClient.SqlConnection(connectionstring))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Activities ORDER BY TaskLevel ASC";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Guid? parentActivityID = null;
                            object parentID = reader.GetValue(reader.GetOrdinal("ParentActivityID"));
                            if (parentID != null && parentID != DBNull.Value)
                            {
                                parentActivityID = (Guid)parentID;
                            }
                            activities.Add(new ActivityDTO {
                                ProjectID = reader.GetGuid(reader.GetOrdinal("ProjectId")),
                                ID = reader.GetGuid(reader.GetOrdinal("ID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
                                TaskLevel = reader.GetInt32(reader.GetOrdinal("TaskLevel")),
                                ParentActivityID = parentActivityID
                            });
                        }
                    }
                }

            }

            var taskOrders = from to in activities
                             where to.ParentActivityID == null
                             orderby to.Name
                             select new ActivityDTO
                             {
                                 ProjectID = to.ProjectID,
                                 TaskLevel = to.TaskLevel,
                                 ID = to.ID,
                                 Name = to.Name,
                                 Acronym = to.Acronym,
                                 Description = to.Description,
                                 DisplayOrder = to.DisplayOrder,
                                 ParentActivityID = to.ParentActivityID,
                                 Deleted = to.Deleted,
                                 Activities = activities.Where(a => a.ParentActivityID == to.ID).OrderBy(a => a.Name)
                                                        .Select(a => new ActivityDTO
                                                        {
                                                            ProjectID = a.ProjectID,
                                                            TaskLevel = a.TaskLevel,
                                                            ID = a.ID,
                                                            Name = a.Name,
                                                            Acronym = a.Acronym,
                                                            Description = a.Description,
                                                            DisplayOrder = a.DisplayOrder,
                                                            ParentActivityID = a.ParentActivityID,
                                                            Deleted = a.Deleted,
                                                            Activities = activities.Where(aa => aa.ParentActivityID == a.ID).OrderBy(aa => aa.Name)
                                                                                   .Select(aa => new ActivityDTO
                                                                                   {
                                                                                       ProjectID = aa.ProjectID,
                                                                                       TaskLevel = aa.TaskLevel,
                                                                                       ID = aa.ID,
                                                                                       Name = aa.Name,
                                                                                       Acronym = aa.Acronym,
                                                                                       Description = aa.Description,
                                                                                       DisplayOrder = aa.DisplayOrder,
                                                                                       ParentActivityID = aa.ParentActivityID,
                                                                                       Deleted = aa.Deleted
                                                                                   })
                                                        })
                             };

            using (var writer = new System.IO.StreamWriter("activities_existing_imported.json"))
            {
                var ss = new Newtonsoft.Json.JsonSerializerSettings();
                ss.Formatting = Newtonsoft.Json.Formatting.Indented;

                var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault(ss);
                serializer.Serialize(writer, taskOrders);
            }

            Console.WriteLine(activities.Count);
        }

        

        [TestMethod]
        public async Task DumpFromService()
        {
            string serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Url"];
            string serviceUsername = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.User"] ?? string.Empty).DecryptString();
            string servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.Password"] ?? string.Empty).DecryptString();

            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));
                using (var writer = System.IO.File.Create("activities_from_service-"+ DateTime.Now.ToString("yyyyMMdd")+".json"))
                using(var stream = await web.GetStreamAsync(serviceUrl))
                {
                    await stream.CopyToAsync(writer);
                    await writer.FlushAsync();
                }
            }
        }

        [TestMethod]
        public async Task GetTotalNumberOfImportItemsFromService() {
            string serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Url"];
            string serviceUsername = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.User"] ?? string.Empty).DecryptString();
            string servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.Password"] ?? string.Empty).DecryptString();

            List<DTO.TaskOrderImportDTO> taskOrders = null;
            using (var web = new System.Net.Http.HttpClient())
            {
                web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));
                
                using (var stream = await web.GetStreamAsync(serviceUrl))
                using (var jReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
                {
                    var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
                    taskOrders = serializer.Deserialize<List<DTO.TaskOrderImportDTO>>(jReader);
                }
            }

            //var total = taskOrders.Count() + taskOrders.SelectMany(a => a.Activities.SelectMany(aa => aa.Activities).Select(x => 1)).Sum(x => x) + 
            Console.WriteLine("Total task orders: " + taskOrders.Count());
            Console.WriteLine("Total activities: " + taskOrders.SelectMany(a => a.Activities.Select(x => 1)).Sum());
            Console.WriteLine("Total activity projects: " + taskOrders.SelectMany(a => a.Activities.SelectMany(aa => aa.Activities.Select(x => 1))).Sum());
        }

        [TestMethod]
        public async Task TestProjectActivitiesImportInController()
        {
            Guid projectID = new Guid("");   

            string serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Url"];
            string serviceUsername = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.User"] ?? string.Empty).DecryptString();
            string servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["Activities.Import.Password"] ?? string.Empty).DecryptString();

            int initialImportCount = 0;

            using (var db = new DataContext())
            {
                int existingCount = db.Activities.Where(a => a.ProjectID == projectID).Count();

                var updater = new Lpp.Dns.Api.Projects.ProjectActivitiesUpdater(db, serviceUrl, serviceUsername, servicePassword);
                await updater.DoUpdate(projectID);

                Assert.IsTrue(updater.StatusCode == System.Net.HttpStatusCode.OK, updater.StatusMessage);

                initialImportCount = existingCount + db.Activities.Where(a => a.ProjectID == projectID).Count();
            }
        }

        [TestMethod]
        public void ImportFromExistingPMN()
        {
            List<ActivityDTO> existingActivities = new List<ActivityDTO>();
            string externalConnectionString = "";
            Guid externalProjectID = new Guid("");
            Guid projectID = new Guid("");

            using (var db = new DataContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Activities WHERE ProjectID = @projectID", new System.Data.SqlClient.SqlParameter("@projectID", projectID));

                using (var conn = new System.Data.SqlClient.SqlConnection(externalConnectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT * FROM Activities WHERE ProjectId = '" + externalProjectID + "' AND TaskLevel = 1";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Guid? parentActivityID = null;
                                object parentID = reader.GetValue(reader.GetOrdinal("ParentActivityID"));
                                if (parentID != null && parentID != DBNull.Value)
                                {
                                    parentActivityID = (Guid)parentID;
                                }
                                db.Activities.Add(
                                    new Activity
                                    {
                                        ProjectID = projectID,
                                        ParentActivityID = parentActivityID,
                                        ID = reader.GetGuid(reader.GetOrdinal("ID")),
                                        Name = reader.GetValue(reader.GetOrdinal("Name")).ToStringEx(),
                                        Acronym = reader.GetValue(reader.GetOrdinal("Acronym")).ToStringEx(),
                                        Description = reader.GetValue(reader.GetOrdinal("Description")).ToStringEx(),
                                        DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
                                        TaskLevel = reader.GetInt32(reader.GetOrdinal("TaskLevel"))                                        
                                    }
                                );
                            }
                        }

                        db.SaveChanges();

                        cmd.CommandText = "SELECT * FROM Activities WHERE ProjectId = '" + externalProjectID + "' AND TaskLevel >= 2 ORDER BY TaskLevel ASC";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Guid parentID = reader.GetGuid(reader.GetOrdinal("ParentActivityID"));

                                Activity parent = db.Activities.Find(parentID);

                                parent.DependantActivities.Add(
                                        new Activity
                                        {
                                            ProjectID = projectID,
                                            ParentActivityID = parentID,
                                            ID = reader.GetGuid(reader.GetOrdinal("ID")),
                                            Name = reader.GetValue(reader.GetOrdinal("Name")).ToStringEx(),
                                            Acronym = reader.GetValue(reader.GetOrdinal("Acronym")).ToStringEx(),
                                            Description = reader.GetValue(reader.GetOrdinal("Description")).ToStringEx(),
                                            DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
                                            TaskLevel = reader.GetInt32(reader.GetOrdinal("TaskLevel"))
                                        }
                                    );
                            }
                        }

                        db.SaveChanges();
                        
                    }
                }

                
            }          
            

        }

        [TestMethod]
        public void CompareExports()
        {
            IEnumerable<Lpp.Dns.DTO.TaskOrderImportDTO> importTasks = null;

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            var serializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);
            using (var stream = typeof(ImportActivitiesTests).Assembly.GetManifestResourceStream("Lpp.Dns.Api.Tests.TaskMatrix.PMNDEV_4121.activities_from_service_20150420.json"))
            using (var jStream = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
            {
                importTasks = serializer.Deserialize<IEnumerable<Lpp.Dns.DTO.TaskOrderImportDTO>>(jStream);
            }

            IEnumerable<Lpp.Dns.DTO.ActivityDTO> pmnTasks = null;

            using (var stream = typeof(ImportActivitiesTests).Assembly.GetManifestResourceStream("Lpp.Dns.Api.Tests.TaskMatrix.PMNDEV_4121.PMN_Activities_Export_20150420.json"))
            using (var jStream = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
            {
                var container = serializer.Deserialize<DeserializedActivityDTOs>(jStream);
                pmnTasks = container.results;
            }

            Assert.IsTrue(importTasks.Any() && pmnTasks.Any());

            //make sure that the task orders match

            var missingFromPMN = (from it in importTasks
                                 where !pmnTasks.Any(pt => string.Equals(it.ID, pt.Name, StringComparison.OrdinalIgnoreCase))
                                 select it).ToArray();

            if (missingFromPMN.Length > 0)
            {
                Console.WriteLine("The following were not imported:");

                foreach(var item in missingFromPMN)
                    Console.WriteLine("    " + item.ID);
            }

            foreach (var ito in importTasks)
            {
                var pmnTO = pmnTasks.First(pt => string.Equals(ito.ID, pt.Name, StringComparison.OrdinalIgnoreCase));

                if (ito.Activities.Count == 0)
                {
                    //if the import does not have any activities but the pmn to does that is not deleted make note

                    if (pmnTO.Activities.Any(a => a.Deleted == false))
                    {
                        Console.WriteLine(pmnTO.Name + " has activies that should be deleted!");
                        continue;
                    }

                }


                //make sure everything was import, and none are marked as deleted
                var pmnActivities = pmnTO.Activities.Where(a => ito.Activities.Any(i => string.Compare(i.Name, a.Name, true) == 0)).ToArray();

                Assert.IsTrue(ito.Activities.Count() == pmnActivities.Where(a => a.Deleted == false).Count(), "Imported activities count does not match for item non-deleted:" + pmnTO.Name);

                //select from import where exists in pmn but is marked as deleted
                var itActivities = ito.Activities.Where(i => pmnActivities.Any(a => a.Deleted && string.Compare(i.Name, a.Name, true) == 0)).ToArray();
                foreach (var ita in itActivities)
                {
                    Console.WriteLine(ito.ID + "/" + ita.Name + " is marked as deleted in pmn.");
                }

                foreach (var ia in ito.Activities)
                {
                    if (ia.Activities.Count == 0)
                        continue;

                    //make sure the sub activites match
                    var pmnActivity = pmnTO.Activities.First(a => string.Compare(ia.Name, a.Name, true) == 0);

                    foreach (var pa in pmnActivity.Activities.Where(a => (a.Deleted && ia.Activities.Any(x => string.Compare(x.Name, a.Name, true) == 0)) || !ia.Activities.Any(x => string.Compare(x.Name, a.Name, true) == 0)))
                    {
                        Console.WriteLine(pmnTO.Name + "/" + pmnActivity.Name + "/" + pa.Name + " is not marked as deleted and does not exist in the import.");
                    }
                }


            }
            
        }

        

        public class DeserializedActivityDTOs
        {
            public IEnumerable<Lpp.Dns.DTO.ActivityDTO> results { get; set; }
        }

    }
}

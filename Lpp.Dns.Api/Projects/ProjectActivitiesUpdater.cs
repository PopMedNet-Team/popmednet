using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api.Projects
{
    /// <summary>
    /// Helper class for updating a projects available activities.
    /// </summary>
    public class ProjectActivitiesUpdater
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(ProjectActivitiesUpdater));
        readonly DataContext db;
        readonly string serviceUrl;
        readonly string serviceUsername;
        readonly string servicePassword;

        /// <summary>
        /// Initiates a new ProjectActivitiesUpdater.
        /// </summary>
        /// <param name="dataContext">The database context to use.</param>
        /// <param name="serviceUrl">The url of the service providing the activities list to update from.</param>
        /// <param name="serviceUsername">The username for the update service.</param>
        /// <param name="servicePassword">The password for the update service.</param>
        public ProjectActivitiesUpdater(DataContext dataContext, string serviceUrl, string serviceUsername, string servicePassword)
        {
            this.db = dataContext;
            this.serviceUrl = serviceUrl;
            this.serviceUsername = serviceUsername;
            this.servicePassword = servicePassword;
            this.StatusCode = HttpStatusCode.OK;
            this.StatusMessage = string.Empty;
        }

        /// <summary>
        /// Gets the HttpStatusCode indicating the success or failure of the update operation.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current status message for the update operation.
        /// </summary>
        public string StatusMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks that the specified user has permission to update activities for the specified project.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="projectID">The ID of the project.</param>
        /// <returns></returns>
        public async Task<bool> CanUpdate(Guid userID, Guid projectID)
        {
            var globalAcls = db.GlobalAcls.FilterAcl(userID, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.UpdateActivities);
            var projectAcls = db.ProjectAcls.FilterAcl(userID, Lpp.Dns.DTO.Security.PermissionIdentifiers.Project.UpdateActivities);

            var project = from p in db.Projects
                          let gAcl = globalAcls
                          let pAcl = projectAcls.Where(a => a.ProjectID == p.ID)
                          where p.ID == projectID && p.Active && !p.Deleted
                          && (
                            (gAcl.Any(a => a.Allowed) || pAcl.Any(a => a.Allowed))
                            &&
                            (gAcl.All(a => a.Allowed) && gAcl.All(a => a.Allowed))
                          )
                          select p;

            bool granted = await project.AnyAsync();
            return granted;
        }

        /// <summary>
        /// Executes the update of the activities for the specified project.
        /// </summary>
        /// <param name="projectID">The ID of the project.</param>
        /// <returns></returns>
        public async Task DoUpdate(Guid projectID)
        {
            List<DTO.TaskOrderImportDTO> importList = await LoadImport();            

            if (importList.Count == 0)
            {
                return;
            }

            Logger.Debug("Loading existing activities and determining if each activity is associated with one or more requests.");

            IEnumerable<Activity> existingActivities = await db.Activities.Where(a => a.ProjectID == projectID).ToArrayAsync();
            IEnumerable<KeyValuePair<Guid,bool>> hasRequests = (await(
                                                                    from a in db.Activities
                                                                    let request = db.Requests.Where(r => (r.ActivityID.HasValue && r.ActivityID.Value == a.ID) || (r.SourceActivityID.HasValue && r.SourceActivityID.Value == a.ID) || (r.SourceActivityProjectID.HasValue && r.SourceActivityProjectID.Value == a.ID) || (r.SourceTaskOrderID.HasValue && r.SourceTaskOrderID.Value == a.ID)).FirstOrDefault()
                                                                    where a.ProjectID == projectID
                                                                    select new {a.ID, HasRequests = request != null }
                                                                ).ToArrayAsync()).Select(p => new KeyValuePair<Guid,bool>(p.ID, p.HasRequests)).ToArray();

            Logger.Debug("Deleting task orders that do not exist in update list.");

            //delete task order that do not exist in update list
            var taskOrdersToDelete = (from a in existingActivities
                                      where a.ParentActivityID == null && a.TaskLevel == 1
                                          && !importList.Any(i => string.Equals(a.Name, i.ID, StringComparison.OrdinalIgnoreCase))
                                      select a).ToArray();

            DeleteTaskOrders(taskOrdersToDelete, existingActivities, hasRequests);

            Logger.Debug("Beginning of update for " + importList.Count + " task orders.");

            //now import/update everything
            foreach (var to in importList)
            {
                var taskOrderToImport = to;

                Logger.Debug("Updating/importing task order: " + taskOrderToImport.ID);

                var taskOrder = existingActivities.Where(a => string.Equals(taskOrderToImport.ID, a.Name, StringComparison.OrdinalIgnoreCase) && a.TaskLevel == 1).FirstOrDefault();

                if (taskOrder == null)
                {
                    AddNewTaskOrder(projectID, taskOrderToImport);
                    continue;
                }

                //make sure the acronym is set for the taskorder, and marked as not deleted
                taskOrder.Acronym = taskOrderToImport.ID;
                taskOrder.Deleted = false;

                List<Activity> toDelete = new List<Activity>();
                //determine the ones that need to be deleted
                IEnumerable<Activity> activitiesToDelete = (from a in existingActivities
                                                            where a.ParentActivityID == taskOrder.ID && a.TaskLevel == 2
                                                            && !taskOrderToImport.Activities.Any(aa => string.Equals((string.IsNullOrWhiteSpace(aa.Name) ? aa.Acronym : aa.Name), a.Name, StringComparison.OrdinalIgnoreCase))
                                                            select a).ToArray();

                DeleteActivities(activitiesToDelete, existingActivities, hasRequests, toDelete.Add, null);
                

                //confirm the task orders child activities exist and their activity projects are up to date.
                foreach (var ati in taskOrderToImport.Activities)
                {
                    ActivityImportDTO activityToImport = ati;

                    
                    Activity childActivity = FindActivity(existingActivities, 2, taskOrder.ID, activityToImport.Key, activityToImport.Name, activityToImport.Acronym);
                    if (childActivity == null)
                    {
                        childActivity = new Activity
                                        {
                                            ParentActivityID = taskOrder.ID,
                                            ProjectID = projectID,
                                            Name = string.IsNullOrWhiteSpace(activityToImport.Name) ? activityToImport.Acronym : activityToImport.Name,
                                            Acronym = activityToImport.Acronym,
                                            Description = string.Empty,
                                            DisplayOrder = 0,
                                            TaskLevel = 2,
                                            ExternalKey = activityToImport.Key
                                        };

                        taskOrder.DependantActivities.Add(childActivity);
                    }
                    else
                    {
                        childActivity.Name = activityToImport.Name;
                        childActivity.Acronym = activityToImport.Acronym;
                        childActivity.ExternalKey = activityToImport.Key;
                        childActivity.Deleted = false;
                    }

                    //remove the sub-activities that do not exist in the update.
                    IEnumerable<Activity> activityProjectsToDelete = (from a in existingActivities
                                                                      where a.ParentActivityID == childActivity.ID && a.TaskLevel == 3
                                                                      && !activityToImport.Activities.Any(aa => string.Equals((string.IsNullOrWhiteSpace(aa.Name) ? aa.Acronym : aa.Name), a.Name, StringComparison.OrdinalIgnoreCase))
                                                                      select a).ToArray();

                    DeleteActivityProjects(activityProjectsToDelete, existingActivities, hasRequests, toDelete.Add, null);

                    //add/update the project activities
                    foreach (var apti in activityToImport.Activities)
                    {
                        var subActivityToImport = apti;
                        Activity activityProject = FindActivity(existingActivities, 3, childActivity.ID, subActivityToImport.Key, subActivityToImport.Name, subActivityToImport.Acronym);
                        if (activityProject == null)
                        {
                            childActivity.DependantActivities.Add(
                                    new Activity
                                    {
                                        ParentActivityID = childActivity.ID,
                                        ProjectID = projectID,
                                        Name = string.IsNullOrWhiteSpace(subActivityToImport.Name) ? subActivityToImport.Acronym : subActivityToImport.Name,
                                        Acronym = subActivityToImport.Acronym,
                                        Description = string.Empty,
                                        DisplayOrder = 0,
                                        TaskLevel = 3,
                                        ExternalKey = subActivityToImport.Key
                                    }
                                );
                        }
                        else
                        {
                            activityProject.Name = subActivityToImport.Name;
                            activityProject.Acronym = subActivityToImport.Acronym;
                            activityProject.ExternalKey = subActivityToImport.Key;
                            activityProject.Deleted = false;
                        }

                    }
                }

                if (toDelete.Any())
                {
                    db.Activities.RemoveRange(toDelete);
                }

            }//end of taskorder loop

            Logger.Debug("Finished processing import items, saving changes.");

            await db.SaveChangesAsync();

            Logger.Debug("Finished saving import of activities.");
        }

        

        async Task<List<DTO.TaskOrderImportDTO>> LoadImport()
        {
            try
            {
                using (var web = new HttpClient())
                {
                    web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));

                    Logger.Debug("Beginning request of import list from: " + serviceUrl);

                    using (var stream = await web.GetStreamAsync(serviceUrl))
                    using (var jReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

                        Logger.Debug("Deserializing import list from: " + serviceUrl);
                        return serializer.Deserialize<List<DTO.TaskOrderImportDTO>>(jReader);
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                Logger.Error("Error getting activities from external service: " + serviceUrl, httpex);

                StatusCode = HttpStatusCode.ServiceUnavailable;
                StatusMessage = "There was an error communicating with the external service.";

                return new List<DTO.TaskOrderImportDTO>();
            }
        }

        void DeleteTaskOrders(IEnumerable<Activity> taskOrdersToDelete, IEnumerable<Activity> existingActivities, IEnumerable<KeyValuePair<Guid, bool>> hasRequests)
        {
            List<Activity> toDelete = new List<Activity>();

            foreach (var to in taskOrdersToDelete)
            {
                var taskOrder = to;
                
                var activitiesToDelete = existingActivities.Where(a => a.ParentActivityID == taskOrder.ID);
                DeleteActivities(activitiesToDelete, existingActivities, hasRequests, toDelete.Add, taskOrder);

                //check if marked as soft delete, if not check if has requests, if not add to delete
                if (activitiesToDelete.Any(a => a.Deleted) || hasRequests.Any(r => r.Key == taskOrder.ID && r.Value))
                {
                    taskOrder.Deleted = true;
                }
                else
                {
                    toDelete.Add(taskOrder);
                }
            }

            if (toDelete.Any())
            {
                db.Activities.RemoveRange(toDelete);
            }
        }

        static void DeleteActivities(IEnumerable<Activity> activitiesToDelete, IEnumerable<Activity> existingActivities, IEnumerable<KeyValuePair<Guid, bool>> hasRequests, Action<Activity> addToDelete, Activity parentActivityToMarkDeleted)
        {
            foreach (var atd in activitiesToDelete)
            {
                var activity = atd;

                var activityProjectsToDelete = existingActivities.Where(a => a.ParentActivityID == activity.ID);
                DeleteActivityProjects(activityProjectsToDelete, existingActivities, hasRequests, addToDelete, activity);

                //check if marked as soft delete, if not check if has requests, if not add to delete
                if (activityProjectsToDelete.Any(a => a.Deleted) || hasRequests.Any(r => r.Key == activity.ID && r.Value))
                {
                    activity.Deleted = true;
                    if (parentActivityToMarkDeleted != null)
                        parentActivityToMarkDeleted.Deleted = true;
                }
                else
                {
                    addToDelete(activity);
                }
            }
        }

        static void DeleteActivityProjects(IEnumerable<Activity> activityProjectsToDelete, IEnumerable<Activity> existingActivities, IEnumerable<KeyValuePair<Guid, bool>> hasRequests, Action<Activity> addToDelete, Activity parentActivityToMarkDeleted)
        {
            foreach (var patd in activityProjectsToDelete)
            {
                var projectActivity = patd;
                if (hasRequests.Any(r => r.Key == projectActivity.ID && r.Value))
                {
                    //mark as soft delete, mark parent activity as soft delete, mark task order as soft delete
                    projectActivity.Deleted = true;
                    if(parentActivityToMarkDeleted != null)
                        parentActivityToMarkDeleted.Deleted = true;
                }
                else
                {
                    addToDelete(projectActivity);
                }
            }
        }
        
        void AddNewTaskOrder(Guid projectID, TaskOrderImportDTO taskOrderToImport)
        {
            //add the task order
            var taskOrder = db.Activities.Add(
                    new Activity
                    {
                        Name = taskOrderToImport.ID,
                        Acronym = taskOrderToImport.ID,
                        Description = string.Empty,
                        TaskLevel = 1,
                        ProjectID = projectID,
                        DisplayOrder = 0
                    }
                );

            //add the activities for the task order
            foreach (var ati in taskOrderToImport.Activities)
            {
                ActivityImportDTO activityToImport = ati;

                string activityName = string.IsNullOrWhiteSpace(activityToImport.Name) ? activityToImport.Acronym : activityToImport.Name;
                Activity childActivity = new Activity
                {
                    ProjectID = projectID,
                    ParentActivityID = taskOrder.ID,
                    Name = activityName,
                    Acronym = activityToImport.Acronym,
                    Description = string.Empty,
                    TaskLevel = 2,
                    DisplayOrder = 0,
                    ExternalKey = activityToImport.Key
                };

                taskOrder.DependantActivities.Add(childActivity);

                //add the activity projects for the activity
                foreach (var apti in activityToImport.Activities)
                {
                    var activityProjectToImport = apti;
                    string projectName = string.IsNullOrWhiteSpace(activityProjectToImport.Name) ? activityProjectToImport.Acronym : activityProjectToImport.Name;
                    childActivity.DependantActivities.Add(
                            new Activity
                            {
                                ProjectID = projectID,
                                ParentActivityID = childActivity.ID,
                                Name = projectName,
                                Acronym = activityProjectToImport.Acronym,
                                Description = string.Empty,
                                TaskLevel = 3,
                                DisplayOrder = 0,
                                ExternalKey = activityProjectToImport.Key
                            }
                        );
                }
            }
        }

        static Activity FindActivity(IEnumerable<Activity> activities, int taskLevel, Guid parentID, int? key, string name, string acronym)
        {
            Activity activity = null;

            //when the activity ID is available use that first to find the activity.
            if (key.HasValue)
            {
                activity = activities.Where(a => a.TaskLevel == taskLevel && a.ParentActivityID == parentID && a.ExternalKey == key).FirstOrDefault();
            }

            //next search based on acronym and then name; the values search must not already have a key specified or a key is not specified in the method parameters

            if (activity == null && !string.IsNullOrWhiteSpace(acronym))
            {
                activity = activities.Where(a => a.TaskLevel == taskLevel && a.ParentActivityID == parentID && string.Equals(acronym, a.Acronym, StringComparison.OrdinalIgnoreCase) && (!key.HasValue || !a.ExternalKey.HasValue)).FirstOrDefault();
            }

            if (activity == null)
            {
                name = string.IsNullOrWhiteSpace(name) ? acronym : name;
                activity = activities.Where(a => a.TaskLevel == taskLevel && a.ParentActivityID == parentID && string.Equals(name, a.Name, StringComparison.OrdinalIgnoreCase) && (!key.HasValue || !a.ExternalKey.HasValue)).FirstOrDefault();
            }

            return activity;
        }

    }    
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using PopMedNet.Dns.DTO.Events;
using PopMedNet.Utilities;
using PopMedNet.Dns.Data.Audit;
using System.Net.Mail;

namespace PopMedNet.Dns.Api.Users
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class UsersController : ApiDataControllerBase<User, UserDTO, DataContext, PermissionDefinition>
    {
        public UsersController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        [HttpPost("validatelogin"), AllowAnonymous]
        public async Task<LoginResultDTO> ValidateLogin(LoginDTO login)
        {
            Utilities.Security.IUser user;
            User? contact;
            if (!DataContext.ValidateUser(login.UserName, login.Password, out user))
            {
                if (user == null)
                {
                    throw new HttpResponseException((int)System.Net.HttpStatusCode.Unauthorized, "Invalid user name or password.");
                }
                string errorMessage = "The Login or Password is invalid.";

                contact = await (from c in DataContext.Users.Include(x => x.Organization).Include(x => x.RejectedBy).Include(x => x.DeactivatedBy)
                                 where c.ID == user.ID && !c.Deleted && c.Active
                                 select c).FirstOrDefaultAsync();

                if (contact == null)
                {
                    throw new HttpResponseException((int)System.Net.HttpStatusCode.Unauthorized, "Unable to find user details.");
                }

                contact.FailedLoginCount++;

                if (contact.FailedLoginCount >= _configuration.GetValue("appSettings:FailedLoginAttemptsBeforeLockingOut", 5))
                {
                    if (contact.Active == true)
                    {
                        contact.DeactivatedOn = DateTime.UtcNow;
                        contact.Active = false;
                    }
                    errorMessage = "Your account has been locked after too many unsuccessful login attempts. Please contact your administrator.";
                }

                Dns.Data.Audit.UserAuthenticationLogs failedAudit = new Data.Audit.UserAuthenticationLogs
                {
                    UserID = contact.ID,
                    Description = "User Authenticated Failed from " + login.Environment + " from IP Address: " + login.IPAddress,
                    Success = false,
                    IPAddress = login.IPAddress,
                    Environment = login.Environment,
                    Details = PopMedNet.Utilities.Crypto.EncryptStringAES("UserName: " + login.UserName + " was attempted with Password:" + login.Password, "AuthenticationLog", contact.ID.ToString("D"))
                };

                DataContext.LogsUserAuthentication.Add(failedAudit);
                await DataContext.SaveChangesAsync();

                if (!contact.Active)
                {
                    System.Threading.Thread.Sleep(contact.FailedLoginCount * 3000);
                }

                throw new HttpResponseException((int)System.Net.HttpStatusCode.Unauthorized, errorMessage);
            }

            contact = await (from c in DataContext.Users.Include(x => x.Organization).Include(x => x.RejectedBy).Include(x => x.DeactivatedBy)
                             where c.ID == user.ID && !c.Deleted && c.Active
                             select c).FirstOrDefaultAsync();

            if (contact == null)
            {
                throw new HttpResponseException((int)System.Net.HttpStatusCode.Unauthorized, "Unable to find user details.");
            }

            if (!contact.Active)
            {
                throw new HttpResponseException((int)System.Net.HttpStatusCode.Unauthorized, "User Account is locked");
            }

            if (contact.FailedLoginCount > 0)
            {
                contact.FailedLoginCount = 0;
            }

            if (contact.PasswordRestorationToken.HasValue || contact.PasswordRestorationTokenExpiration.HasValue)
            {
                contact.PasswordRestorationTokenExpiration = null;
                contact.PasswordRestorationToken = null;
            }

            if (login.Environment != null && login.IPAddress != null)
            {
                Dns.Data.Audit.UserAuthenticationLogs successAudit = new Data.Audit.UserAuthenticationLogs
                {
                    UserID = user.ID,
                    Description = "User Authenticated Successful from " + login.Environment + " from IP Address: " + login.IPAddress,
                    Success = true,
                    IPAddress = login.IPAddress,
                    Environment = login.Environment
                };
                DataContext.LogsUserAuthentication.Add(successAudit);
            }

            await DataContext.SaveChangesAsync();

            //return _mapper.Map<DTO.UserDTO>(contact);
            return new DTO.LoginResultDTO { ID = contact.ID, UserName = contact.UserName, FullName = contact.FullName, OrganizationID = contact.OrganizationID, PasswordExpiration = contact.PasswordExpiration };
        }

        /// <summary>
        /// Return main menu
        /// </summary>
        /// <returns></returns>
        [HttpGet("returnmainmenu"), ResponseCache(NoStore = true)]
        public async Task<IActionResult> ReturnMainMenu()
        {
            List<MenuItemDTO> menu = new List<MenuItemDTO>();


            var permissions = await DataContext.HasGrantedPermissions(Identity, PermissionIdentifiers.DataMart.RunAuditReport,
                PermissionIdentifiers.Portal.RunNetworkActivityReport,
                PermissionIdentifiers.Portal.RunEventsReport,
                PermissionIdentifiers.Portal.ListDataMarts,
                PermissionIdentifiers.Portal.ListGroups,
                PermissionIdentifiers.Project.View,
                PermissionIdentifiers.Project.Edit,
                PermissionIdentifiers.Request.ViewRequest,
                PermissionIdentifiers.Request.ViewHistory,
                PermissionIdentifiers.Request.ViewResults,
                PermissionIdentifiers.Request.ViewIndividualResults,
                PermissionIdentifiers.Request.ViewStatus,
                PermissionIdentifiers.Request.Edit,
                PermissionIdentifiers.Request.ApproveRejectSubmission,
                PermissionIdentifiers.Request.ChangeRoutings,
                PermissionIdentifiers.Request.Delete,
                PermissionIdentifiers.DataMartInProject.ApproveResponses,
                PermissionIdentifiers.DataMartInProject.GroupResponses,
                PermissionIdentifiers.DataMartInProject.HoldRequest,
                PermissionIdentifiers.DataMartInProject.RejectRequest,
                PermissionIdentifiers.DataMartInProject.SeeRequests,
                PermissionIdentifiers.DataMartInProject.SkipResponseApproval,
                PermissionIdentifiers.Group.ListProjects,
                PermissionIdentifiers.Portal.ListRegistries,
                PermissionIdentifiers.Portal.ListOrganizations,
                PermissionIdentifiers.Portal.ListUsers,
                PermissionIdentifiers.Portal.CreateNetworkMessages,
                PermissionIdentifiers.Portal.ManageSecurity,
                PermissionIdentifiers.Portal.ListTemplates,
                PermissionIdentifiers.Portal.ListRequestTypes);


            menu.Add(new MenuItemDTO
            {
                Url = "/",
                Text = "Home"
            });

            var projects = this.ReturnAvailableProjects().OrderBy(p => p.Name).Select(p => new { p.ID, p.Name }).ToArray();
            if (projects.Any())
            {
                menu.Add(new MenuItemDTO
                {
                    Url = "/requests",
                    Text = "Requests",
                    Items = projects.Select(p => new MenuItemDTO
                    {
                        Text = p.Name,
                        Url = "/requests?projectid=" + p.ID
                    })
                });
            }

            menu.Add(new MenuItemDTO
            {
                Text = "Profile",
                Url = "/users/details?ID=" + Identity.ID
            });

            menu.Add(new MenuItemDTO
            {
                Text = "Resources",
                Url = "/home/resources"
            });

            var reportItems = new List<MenuItemDTO>();

            if (permissions.Contains(PermissionIdentifiers.DataMart.RunAuditReport))
                reportItems.Add(new MenuItemDTO
                {
                    Text = "DataMart Audit",
                    Url = "/reports/datamartauditreport"
                });


            //if (permissions.Contains(PermissionIdentifiers.Portal.RunEventsReport))
            //    reportItems.Add(new MenuItemDTO
            //    {
            //        Text = "Event Log",
            //        Url = "/report/lists/create"
            //    });

            if (permissions.Contains(PermissionIdentifiers.Portal.RunNetworkActivityReport))
                reportItems.Add(new MenuItemDTO
                {
                    Text = "Network Activity",
                    Url = "/reports/networkactivityreport"
                });

            if (reportItems.Any())
                menu.Add(new MenuItemDTO
                {
                    Text = "Reports",
                    Url = null,
                    Items = reportItems
                });

            var setupItems = new List<MenuItemDTO>();
            if (permissions.Contains(PermissionIdentifiers.Portal.ListDataMarts))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "DataMarts",
                    Url = "/datamarts"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListGroups))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Groups",
                    Url = "/groups"
                });

            if (permissions.Contains(PermissionIdentifiers.Project.View) ||
                permissions.Contains(PermissionIdentifiers.Project.Edit) ||
                permissions.Contains(PermissionIdentifiers.Request.ViewHistory) ||
                permissions.Contains(PermissionIdentifiers.Request.ViewIndividualResults) ||
                permissions.Contains(PermissionIdentifiers.Request.ViewResults) ||
                permissions.Contains(PermissionIdentifiers.Request.ViewStatus) ||
                permissions.Contains(PermissionIdentifiers.Request.Edit) ||
                permissions.Contains(PermissionIdentifiers.Request.ApproveRejectSubmission) ||
                permissions.Contains(PermissionIdentifiers.Request.ChangeRoutings) ||
                permissions.Contains(PermissionIdentifiers.Request.Delete) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.ApproveResponses) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.GroupResponses) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.HoldRequest) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.RejectRequest) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.SeeRequests) ||
                permissions.Contains(PermissionIdentifiers.DataMartInProject.SkipResponseApproval) ||
                permissions.Contains(PermissionIdentifiers.Group.ListProjects))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Projects",
                    Url = "/projects"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListRegistries))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Registries",
                    Url = "/registries"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListOrganizations))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Organizations",
                    Url = "/organizations"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListUsers))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Users",
                    Url = "/users"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.CreateNetworkMessages))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Messages",
                    Url = "/networkmessages/create"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListTemplates))
            {
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Criteria Group Templates",
                    Url = "/templates"
                });
            }

            if (permissions.Contains(PermissionIdentifiers.Portal.ListRequestTypes))
            {
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Request Types",
                    Url = "/requesttype"
                });
            }

            if (permissions.Contains(PermissionIdentifiers.Portal.ManageSecurity))
                setupItems.Add(new MenuItemDTO
                {
                    Text = "Site-Wide Security Permissions",
                    Url = "/defaultsecuritypermissions"
                });

            if (setupItems.Any())
                menu.Add(new MenuItemDTO
                {
                    Text = "Network",
                    Url = null,
                    Items = setupItems
                });

            var result = new JsonResult(menu, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));
            return result;
        }

        [HttpGet("getsetting")]
        public async Task<IActionResult> GetSetting([FromQuery] IEnumerable<string> key)
        {
            var settings = await (from s in DataContext.UserSettings
                                  where s.UserID == Identity.ID && key.Contains(s.Key)
                                  select new UserSettingDTO
                                  {
                                      Key = s.Key,
                                      Setting = s.Setting,
                                      UserID = s.UserID
                                  }).ToArrayAsync();

            return Ok(settings);
        }

        [HttpPost("savesetting")]
        public async Task<IActionResult> SaveSetting(UserSettingDTO setting)
        {
            if (setting.UserID != Identity.ID)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to save settings on the specified user.");
            }

            var current = await (from s in DataContext.UserSettings where s.UserID == setting.UserID && s.Key == setting.Key select s).SingleOrDefaultAsync();

            if (current == null)
            {
                current = new UserSetting { UserID = Identity.ID, Key = setting.Key };
                DataContext.UserSettings.Add(current);
            }

            current.Setting = setting.Setting;

            try
            {
                await DataContext.SaveChangesAsync();

                return Accepted(current);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getglobalpermission")]
        public async Task<IActionResult> GetGlobalPermission(Guid permissionID)
        {
            var result = await DataContext.HasPermission(Identity, PermissionIdentifiers.Definitions.First(p => p.ID == permissionID));
            return Ok(result);
        }

        [HttpGet("get")]
        public override Task<ActionResult<UserDTO>> Get(Guid ID)
        {
            return base.Get(ID);
        }

        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<UserDTO> options)
        {
            IQueryable<UserDTO> q = (from u in DataContext.Secure<User>(Identity) where u.Deleted == false select u).ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<UserDTO>(q, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns notifications for the current user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("getnotifications")]
        public IActionResult GetNotifications(ODataQueryOptions<NotificationDTO> options, Guid userID)
        {
            if (userID != Identity.ID)
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to view notifications for the specified user.");

            var notifications = DataContext.GetNotifications(userID);


            var queryHelper = new Utilities.WebSites.ODataQueryHandler<NotificationDTO>(notifications, options);
            return Ok(queryHelper.Result());
        }

        [HttpGet("listavailableprojects")]
        public IActionResult ListAvailableProjects(ODataQueryOptions<ProjectDTO> options)
        {
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<ProjectDTO>(ReturnAvailableProjects(), options);

            return Ok(queryHelper.Result());
        }

        private IQueryable<ProjectDTO> ReturnAvailableProjects()
        {
            var projects = DataContext.Secure<Project>(Identity, PermissionIdentifiers.Request.ViewRequest, PermissionIdentifiers.Request.ApproveRejectSubmission, /*PermissionIdentifiers.Project.ListRequests,*/ PermissionIdentifiers.Project.Edit, PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.DataMartInProject.HoldRequest, PermissionIdentifiers.DataMartInProject.RejectRequest, PermissionIdentifiers.DataMartInProject.SeeRequests, /*PermissionIdentifiers.DataMartInProject.SkipResponseApproval,*/ PermissionIdentifiers.Request.ChangeRoutings, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.Request.ViewStatus);

            var projSubmit = from p in DataContext.Secure<Project>(Identity) where DataContext.ProjectRequestTypeAcls.Any(a => a.ProjectID == p.ID && a.SecurityGroup!.Users.Any(u => u.UserID == Identity.ID) && a.Permission != RequestTypePermissions.Deny) || DataContext.DataMartRequestTypeAcls.Any(a => p.DataMarts.Any(dm => a.DataMartID == dm.DataMartID) && a.Permission != RequestTypePermissions.Deny && a.SecurityGroup!.Users.Any(u => u.UserID == Identity.ID)) select p;

            return projects.Concat(projSubmit).Where(p => !p.Deleted).Distinct().ProjectTo<ProjectDTO>(_mapper.ConfigurationProvider);
            //return projects.Concat(projSubmit).Where(p => !p.Deleted).Distinct().Select(p => 
            //new ProjectDTO {
            // Acronym = p.Acronym,
            // Active = p.Active,
            // Deleted = p.Deleted,
            // Description = p.Description,
            // EndDate = p.EndDate,
            // GroupID = p.GroupID,
            // Group = p.Group != null ? p.Group.Name : null,
            // ID = p.ID,
            // Name = p.Name,
            // StartDate = p.StartDate,
            // Timestamp = p.Timestamp
            //});
        }

        /// <summary>
        /// Returns the events that a given user can subscribe to.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("getsubscribableevents"), EnableQuery]
        public IQueryable<EventDTO> GetSubscribableEvents(Guid userID)
        {
            var results = (from e in DataContext.Events
                           where
                               (
                               e.DataMartEvents.Any(dm => dm.SecurityGroup.Users.Any(u => u.UserID == userID) && dm.Allowed) && !e.DataMartEvents.Any(dm => dm.SecurityGroup.Users.Any(u => u.UserID == userID) && !dm.Allowed)
                               ) ||
                               (
                               e.GroupEvents.Any(g => g.SecurityGroup.Users.Any(u => u.UserID == userID) && g.Allowed) && !e.GroupEvents.Any(g => g.SecurityGroup.Users.Any(u => u.UserID == userID) && !g.Allowed)
                               ) ||

                               (
                               e.OrganizationEvents.Any(o => o.SecurityGroup.Users.Any(u => u.UserID == userID)) && !e.OrganizationEvents.Any(o => o.SecurityGroup.Users.Any(u => u.UserID == userID) && !o.Allowed)) ||
                               (
                               e.ProjectDataMartEvents.Any(pdm => pdm.SecurityGroup.Users.Any(u => u.UserID == userID) && pdm.Allowed) &&
                               !e.ProjectDataMartEvents.Any(pdm => pdm.SecurityGroup.Users.Any(u => u.UserID == userID) && !pdm.Allowed)
                               ) ||
                               (
                               e.ProjectEvents.Any(p => p.SecurityGroup.Users.Any(u => u.UserID == userID) && p.Allowed) &&
                               !e.ProjectEvents.Any(p => p.SecurityGroup.Users.Any(u => u.UserID == userID) && !p.Allowed)
                               )
                               ||
                               (
                               e.ProjectOrganizationEvents.Any(po => po.SecurityGroup.Users.Any(u => u.UserID == userID) && po.Allowed)
                               && !e.ProjectOrganizationEvents.Any(po => po.SecurityGroup.Users.Any(u => u.UserID == userID) && !po.Allowed)
                               ) ||
                               (
                               e.RegistryEvents.Any(r => r.SecurityGroup.Users.Any(u => u.UserID == userID) && r.Allowed) && !e.RegistryEvents.Any(r => r.SecurityGroup.Users.Any(u => u.UserID == userID) && !r.Allowed)
                               ) ||
                               (
                               e.UserEvents.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID) && ue.Allowed) &&
                               !e.UserEvents.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID) && !ue.Allowed)
                               ) ||
                               (
                               e.Events.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID) && ue.Allowed) &&
                               !e.Events.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID) && !ue.Allowed)
                               ) ||
                               (
                                   (e.ID == EventIdentifiers.User.ProfileUpdated.ID && userID == Identity.ID) ||
                                   (e.ID == EventIdentifiers.User.PasswordExpirationReminder.ID && userID == Identity.ID) ||
                                   (
                                   e.UserEvents.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID)) &&
                                   !e.UserEvents.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID) && !ue.Allowed)
                                   )
                               )
                               || //Special case for Request Status Changed
                               e.ID == EventIdentifiers.Request.RequestStatusChanged.ID

                           select e).ProjectTo<EventDTO>(_mapper.ConfigurationProvider);

            return results;
        }

        /// <summary>
        /// Returns all assigned notifications for user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("getassignednotifications"), EnableQuery]
        public async Task<IQueryable<AssignedUserNotificationDTO>> GetAssignedNotifications(Guid userID)
        {
            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                return await Task.FromResult(Enumerable.Empty<AssignedUserNotificationDTO>().AsQueryable());

            return DataContext.GetAssignedUserNotifications(userID);
        }

        /// <summary>
        /// Returns a list of security groups that the user is a member of based on the user passed
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("memberofsecuritygroups"), EnableQuery]
        public async Task<IQueryable<SecurityGroupDTO>> MemberOfSecurityGroups(Guid userID)
        {
            if (!(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageSecurity)).Any())
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to view the security groups of this user.");

            var results = (from sg in DataContext.Secure<SecurityGroup>(Identity) where sg.Users.Any(u => u.UserID == userID) select sg).ProjectTo<SecurityGroupDTO>(_mapper.ConfigurationProvider);

            return results;
        }

        /// <summary>
        /// Returns the currently subscribed events
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("getsubscribedevents"), EnableQuery]
        public async Task<IQueryable<UserEventSubscriptionDTO>> GetSubscribedEvents(Guid userID)
        {
            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to manage notifications for the specified user.");

            var results = (from se in DataContext.UserEventSubscriptions where se.UserID == userID select se).ProjectTo<UserEventSubscriptionDTO>(_mapper.ConfigurationProvider);

            return results;
        }

        /// <summary>
        /// Updates the user's subscribed events. Note: You MUST pass all subscribed events for the user to this endpoint.
        /// </summary>
        /// <param name="subscribedEvents"></param>
        /// <returns></returns>
        [HttpPost("updatesubscribedevents")]
        public async Task<IActionResult> UpdateSubscribedEvents(IEnumerable<UserEventSubscriptionDTO> subscribedEvents)
        {

            if (!subscribedEvents.Any())
                return Accepted();

            if (subscribedEvents.Select(s => s.UserID).Distinct().Count() > 1)
                throw new HttpResponseException(StatusCodes.Status400BadRequest, "You may only update a single user's event subscriptions at a time.");

            var userID = subscribedEvents.Select(s => (Guid?)s.UserID).First();

            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, (Guid)userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to manage notifications for the specified user.");

            var existing = await (from s in DataContext.UserEventSubscriptions where s.UserID == userID select s).ToArrayAsync();

            // NOTE See the impact of the changes in LastRunTime and NextDueTime(forMy) below on EntityLoggingConfiguration.CreateNotification and EntityLoggingConfiguration.FilterAuditLog

            //New Subscriptions:
            //Event subscription should have a value for either frequency or myFrequency and cannot already be in the list of existing subscriptions
            var newSubs = from se in subscribedEvents where (se.Frequency.HasValue || se.FrequencyForMy.HasValue) && !existing.Any(e => e.EventID == se.EventID && e.UserID == se.UserID) select se;

            foreach (var newSub in newSubs)
            {
                DataContext.UserEventSubscriptions.Add(new UserEventSubscription
                {
                    EventID = newSub.EventID,
                    Frequency = newSub.Frequency != null ? newSub.Frequency.Value : newSub.Frequency,
                    FrequencyForMy = newSub.FrequencyForMy != null ? newSub.FrequencyForMy.Value : newSub.FrequencyForMy,
                    LastRunTime = DateTime.UtcNow, // Regardless of frequency, any log older than before the switch should no longer be reported to avoid clogging the notification queue.
                    NextDueTime = DateTime.UtcNow, // Regardless of frequency, scheduled notification should run on the next scheduled task run. If immediate, this will be ignored.
                    NextDueTimeForMy = DateTime.UtcNow,  // Regardless of frequency, scheduled notification should run on the next scheduled task run. If immediate, this will be ignored.
                    UserID = newSub.UserID
                });
            }

            //Existing Subscription Changes
            //Get current matches and update frequencies as necessary
            var sameSubs = from ss in existing
                           join se in subscribedEvents on new { ss.EventID, ss.UserID } equals new { se.EventID, se.UserID }
                           where (((se.Frequency.HasValue && ss.Frequency != se.Frequency.Value) || (se.FrequencyForMy.HasValue && ss.FrequencyForMy != se.FrequencyForMy.Value))
                           || (!se.FrequencyForMy.HasValue && ss.FrequencyForMy.HasValue)
                           || (!se.Frequency.HasValue && ss.Frequency.HasValue)
                           )
                           select new { existing = ss, newSub = se };
            foreach (var sameSub in sameSubs)
            {
                sameSub.existing.Frequency = sameSub.newSub.Frequency != null ? sameSub.newSub.Frequency.Value : sameSub.newSub.Frequency;
                sameSub.existing.FrequencyForMy = sameSub.newSub.FrequencyForMy != null ? sameSub.newSub.FrequencyForMy.Value : sameSub.newSub.FrequencyForMy;
                sameSub.existing.LastRunTime = DateTime.UtcNow; // Regardless of changes of frequency, any log older than before the switch should no longer be reported to avoid clogging the notification queue.
                sameSub.existing.NextDueTime = DateTime.UtcNow; // Regardless of changes of frequency, scheduled notification should run on the next scheduled task run. If immediate, this will be ignored.
            }


            //Delete ones that are no longer there.
            var deleteSubs = from ee in existing
                             where !subscribedEvents.Any(se => se.EventID == ee.EventID && se.UserID == ee.UserID) ||
                             subscribedEvents.Any(se => se.EventID == ee.EventID && se.UserID == ee.UserID && !se.Frequency.HasValue && !se.FrequencyForMy.HasValue)
                             select ee;

            DataContext.UserEventSubscriptions.RemoveRange(deleteSubs);

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        [HttpGet("listdistinctenvironments")]
        public IEnumerable<UserAuthenticationDTO> ListDistinctEnvironments(Guid userID)
        {
            var q = from ua in DataContext.LogsUserAuthentication
                    let permissionID = PermissionIdentifiers.User.View.ID
                    let identityID = Identity.ID
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

            return results;
        }

        [HttpGet("ListAuthenticationAudits"), EnableQuery]
        public IQueryable<UserAuthenticationDTO> ListAuthenticationAudits(ODataQueryOptions<UserAuthenticationDTO> odata)
        {
            var q = from ua in DataContext.LogsUserAuthentication
                    let permissionID = PermissionIdentifiers.User.View.ID
                    let identityID = Identity.ID
                    let gAcls = DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID).AsEnumerable()
                    let oAcls = DataContext.OrganizationAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID && DataContext.Users.Where(u => u.ID == ua.UserID && a.OrganizationID == u.OrganizationID).Any()).AsEnumerable()
                    let uAcls = DataContext.UserAcls.Where(a => a.SecurityGroup.Users.Any(sgu => sgu.UserID == identityID) && a.PermissionID == permissionID && a.UserID == ua.UserID).AsEnumerable()
                    where identityID == ua.UserID ||
                      (
                        (gAcls.Any() || oAcls.Any() || uAcls.Any())
                        &&
                        (gAcls.All(a => a.Allowed) && oAcls.All(a => a.Allowed) && uAcls.All(a => a.Allowed))
                      )

                    select new UserAuthenticationDTO
                    {
                        ID = ua.ID,
                        DateTime = ua.TimeStamp,
                        UserID = ua.UserID,
                        Success = ua.Success,
                        Description = ua.Description,
                        Source = ua.Source,
                        IPAddress = ua.IPAddress,
                        Environment = ua.Environment,
                        Details = ua.Details,
                        DMCVersion = ua.DMCVersion
                    };

            return (IQueryable<UserAuthenticationDTO>)odata.ApplyTo(q);
        }

        /// <summary>
        /// Insert or update list of Users
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost("insertorupdate")]
        public override async Task<IActionResult> InsertOrUpdate(IEnumerable<UserDTO> values)
        {
            await ValidateActiveUserToOrganization(values);

            var origUsers = values.Where(x => x.ID != Guid.Empty && x.ID != null).ToList();
            var network = DataContext.Networks.Where(x => x.Name != "Aqueduct").FirstOrDefault();

            var users = await base.InsertOrUpdate(values);

            Dictionary<UserDTO, User> map = new Dictionary<UserDTO, User>();
            try
            {
                map = await DoInsertOrUpdate(values);
            }
            catch (DbUpdateException dbex)
            {
                return Unauthorized(dbex.UnwindException());
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException vex)
            {
                string validationErrors = $"{string.Join(", ", vex.ValidationResult.MemberNames)}: {vex.ValidationResult.ErrorMessage}";
                return BadRequest(validationErrors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.UnwindException());
            }


            await CompleteRegistrationTask(map.Select(k => k.Value.ID));

            return users;
        }

        async Task CompleteRegistrationTask(IEnumerable<Guid> userIDs)
        {
            var tasks = await (from a in DataContext.Actions
                               where a.References.Any(r => userIDs.Contains(r.ItemID))
                                   && a.Status != TaskStatuses.Complete
                                   && (a.Type & TaskTypes.NewUserRegistration) == TaskTypes.NewUserRegistration
                               select a).ToArrayAsync();

            foreach (var task in tasks)
            {
                task.Status = TaskStatuses.Complete;
            }

            await DataContext.SaveChangesAsync();
        }

        async Task ValidateActiveUserToOrganization(IEnumerable<UserDTO> users)
        {
            var userIDs = users.Where(u => u.ID.HasValue).Select(u => u.ID.Value).ToArray();

            var currentUsers = await (from u in DataContext.Users where userIDs.Contains(u.ID) select u).ToArrayAsync();

            foreach (var user in users)
            {
                if (user.ID == null)
                {
                    if (user.Active)
                        throw new HttpResponseException(StatusCodes.Status400BadRequest, user.FirstName + " " + user.LastName + " does not have a password and cannot be marked as active.");

                    var existingUserName = await (from u in DataContext.Users where u.UserName == user.UserName && !u.Deleted select u).CountAsync() > 0;
                    if (existingUserName)
                        throw new HttpResponseException(StatusCodes.Status400BadRequest, "The username " + user.UserName + " already exists.");
                }
                else
                {
                    var currentUser = currentUsers.FirstOrDefault(u => u.ID == user.ID.Value);
                    if (currentUser == null)
                        throw new HttpResponseException(StatusCodes.Status404NotFound, user.FirstName + " " + user.LastName + " could not be found.");

                    if (!currentUser.Active && user.Active && currentUser.PasswordHash!.IsNullOrEmpty())
                        throw new HttpResponseException(StatusCodes.Status403Forbidden, user.FirstName + " " + user.LastName + " must be assigned a password before the can be marked active.");

                    if (currentUser.OrganizationID == null & (user.OrganizationID != null && Identity.EmployerID.HasValue &&
                        user.OrganizationID.Value != Identity.EmployerID.Value) &&
                        !await DataContext.GlobalAcls.AnyAsync(a => a.PermissionID == PermissionIdentifiers.Organization.CreateUsers && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID)) &&
                        !await DataContext.OrganizationAcls.AnyAsync(a => a.PermissionID == PermissionIdentifiers.Organization.CreateUsers && currentUser.OrganizationID.HasValue && a.OrganizationID == currentUser.OrganizationID.Value && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID))
                        )
                    {
                        throw new HttpResponseException(StatusCodes.Status403Forbidden, "You may only set the organization for " + user.FirstName + " " + user.LastName + " to any organization for which you have the right to accept/reject registrations.");
                    }

                    if (!currentUser.Active && user.Active)
                    {
                        currentUser.FailedLoginCount = 0;
                        await DataContext.SaveChangesAsync();
                    }
                    //if (!currentUser.Active && user.Active && currentUser.OrganizationID != Identity.EmployerID)
                    //    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You may not activate a user that is not part of your organization."));
                }
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task Delete([FromQuery] IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<User>(Identity, ID.ToArray()))
                throw new HttpResponseException(StatusCodes.Status403Forbidden, "You do not have permission to delete this User.");

            var users = await (from u in DataContext.Users where ID.Contains(u.ID) select u).ToArrayAsync();
            foreach (var user in users)
            {
                user.Deleted = true;
            }

            await CompleteRegistrationTask(ID);

            await DataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the security groups that the user is assigned to. All security groups must be sent as ones not in the list will be removed.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpPost("updatesecuritygroups")]
        public async Task<IActionResult> UpdateSecurityGroups([FromBody] UpdateUserSecurityGroupsDTO groups)
        {
            var newIds = groups.Groups.Select(g => g.ID.Value);

            var current = await (from sg in DataContext.SecurityGroups where sg.Users.Any(u => u.UserID == groups.UserID) select sg.ID).ToArrayAsync();

            //Remove the ones that were removed
            var deleteIds = current.Except(newIds).ToArray();
            var sgus = (from u in DataContext.SecurityGroupUsers where u.UserID == groups.UserID && deleteIds.Contains(u.SecurityGroupID) select u);
            DataContext.SecurityGroupUsers.RemoveRange(sgus);

            //Add the ones that were added
            var addIds = newIds.Except(current).ToArray();
            foreach (var addId in addIds)
            {
                DataContext.SecurityGroupUsers.Add(new SecurityGroupUser
                {
                    Overridden = false,
                    SecurityGroupID = addId,
                    UserID = groups.UserID
                });
            }

            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdateUserPasswordDTO updateInfo)
        {

            if (Data.User.CheckPasswordStrength(updateInfo.Password) != PasswordScores.VeryStrong)
                return StatusCode(StatusCodes.Status403Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            //Check if the user has permissions
            if (updateInfo.UserID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, updateInfo.UserID, PermissionIdentifiers.User.ChangePassword)).Contains(PermissionIdentifiers.User.ChangePassword))
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to change the specified user's password");

            //Update the password
            var user = DataContext.Users.Find(updateInfo.UserID);

            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, "User not found.");

            string newHash = updateInfo.Password.ComputeHash();

            DateTimeOffset dateBack = DateTimeOffset.UtcNow.AddDays(_configuration.GetValue<int>("appSettings:PreviousDaysPasswordRestriction") * -1);
            int previousUses = _configuration.GetValue<int>("appSettings:PreviousPasswordUses");

            if (user.PasswordHash == newHash || await HasPasswordBeenUsed(user.ID, previousUses, dateBack, newHash))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The requested Password has been used too frequently.  Please use a different password.");
            }

            DataContext.LogsUserPasswordChange.Add(new UserPasswordChangeLog { UserID = Identity.ID, UserChangedID = user.ID, OriginalPassword = user.PasswordHash, Method = UserPasswordChange.Profile });

            user.PasswordHash = newHash;
            user.PasswordExpiration = DateTime.Now.AddMonths(_configuration.GetValue<int>("appSettings:ConfiguredPasswordExpiryMonths"));
            user.PasswordRestorationTokenExpiration = null;
            user.PasswordRestorationToken = null;
            user.FailedLoginCount = 0;

            //Save it
            await DataContext.SaveChangesAsync();

            return Accepted();
        }

        Task<bool> HasPasswordBeenUsed(Guid userID, int minimumNumberOfChangesAgo, DateTimeOffset lastUsedSince, string newHash)
        {
            return (from l in DataContext.LogsUserPasswordChange
                    where
                    DataContext.LogsUserPasswordChange.Where(l => l.UserChangedID == userID).OrderByDescending(l => l.TimeStamp).Take(minimumNumberOfChangesAgo).Any(l => l.OriginalPassword == newHash)
                    ||
                    DataContext.LogsUserPasswordChange.Where(l => l.UserChangedID == userID && l.TimeStamp > lastUsedSince && l.OriginalPassword == newHash).Any()
                    select l).AnyAsync();
        }

        /// <summary>
        /// Check if user password has been set.
        /// </summary>
        /// <param name="userID">The Identifier of the User</param>
        /// <returns></returns>
        [HttpGet("haspassword"), ResponseCache(NoStore = true, Duration = 0)]
        public async Task<bool> HasPassword(Guid userID)
        {
            var currentUserHash = await (from u in DataContext.Users where u.ID == userID select u.PasswordHash).FirstOrDefaultAsync();

            return !string.IsNullOrEmpty(currentUserHash);
        }

        /// <summary>
        /// Requests a password reset email
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("forgotpassword"), AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO data)
        {
            var contact = await (from c in DataContext.Users where ((!string.IsNullOrEmpty(data.UserName) && data.UserName == c.UserName) || (!string.IsNullOrEmpty(data.Email) && data.Email == c.Email)) && !c.Deleted select c).FirstOrDefaultAsync();

            if (contact == null)
                return StatusCode(StatusCodes.Status404NotFound, "We're sorry but we could not find you in our system. Please check your user name or password and try again.");

            if (contact.Email.IsNullOrWhiteSpace())
                return StatusCode(StatusCodes.Status400BadRequest, "We're sorry but we do not have a valid email address on file for you. Please use the contact us link to request that your account be reset.");

            // Send the email here.
            try
            {
                var emailSettings = PopMedNet.Utilities.WebSites.Models.EmailSettings.Read(_configuration);
                using (var smtp = emailSettings.CreateSmtpClient())
                {
                    contact.PasswordRestorationToken = Guid.NewGuid();
                    contact.PasswordRestorationTokenExpiration = DateTime.UtcNow.AddHours(1);

                    //TODO: Use an HTML template for this email that is specific per organization or project or site.
                    var message = new MailMessage
                    {
                        To = { new MailAddress(contact.Email, contact.UserName) },
                        Subject = "Restore Password",
                        From = new MailAddress(emailSettings.From)
                    };

                    string resetPasswordUrl = _configuration.GetValue("appSettings:ResetPasswordUrl", "");

                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(string.Format("<div style=\"font-face: arial;\"><p>{0},</p><p>To restore your password, please click this link: <a href=\"{1}\">{1}</a></p></div>",
                                    (contact.FirstName + " " + contact.LastName).Trim(),
                                    resetPasswordUrl + contact.PasswordRestorationToken.ToString()), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Html)));

                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(string.Format("{0}\r\nTo restore your password please click this link: \r\n{1}", (contact.FirstName + " " + contact.LastName).Trim(), resetPasswordUrl + contact.PasswordRestorationToken.ToString()), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain)));

                    message.AlternateViews[0].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
                    message.AlternateViews[1].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    await smtp.SendMailAsync(message);
                }

                await DataContext.SaveChangesAsync();

                return Accepted(new { success = true });

            }
            catch (SmtpException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Smtp Error");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Restores an account with a new password based on the forgot password token.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPut("restorepassword"), AllowAnonymous]
        public async Task<IActionResult> RestorePassword(RestorePasswordDTO updateInfo)
        {
            var user = await (from u in DataContext.Users where u.PasswordRestorationToken == updateInfo.PasswordRestoreToken && !u.Deleted select u).FirstOrDefaultAsync();

            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, "We're sorry but we could not find the password restore request.");

            if (user.PasswordRestorationTokenExpiration == null || user.PasswordRestorationTokenExpiration.Value < DateTime.UtcNow)
                return StatusCode(StatusCodes.Status403Forbidden, "We're sorry but the request to reset your password has expired. Please create a new request.");

            if (Data.User.CheckPasswordStrength(updateInfo.Password) != PasswordScores.VeryStrong)
                return StatusCode(StatusCodes.Status403Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            string newHash = updateInfo.Password.ComputeHash();
            DateTimeOffset dateBack = DateTimeOffset.UtcNow.AddDays(_configuration.GetValue<int>("appSettings:PreviousDaysPasswordRestriction") * -1);
            int previousUses = _configuration.GetValue<int>("appSettings:PreviousPasswordUses");

            if (user.PasswordHash == newHash || await HasPasswordBeenUsed(user.ID, previousUses, dateBack, newHash))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The requested Password has been used too frequently.  Please use a different password.");
            }

            DataContext.LogsUserPasswordChange.Add(new UserPasswordChangeLog { UserID = user.ID, UserChangedID = user.ID, OriginalPassword = user.PasswordHash, Method = UserPasswordChange.Reset });

            user.PasswordHash = newHash;
            user.PasswordExpiration = DateTime.Now.AddMonths(_configuration.GetValue<int>("appSettings:ConfiguredPasswordExpiryMonths"));
            user.PasswordRestorationToken = null;
            user.PasswordRestorationTokenExpiration = null;
            user.FailedLoginCount = 0;

            await DataContext.SaveChangesAsync();

            return Accepted(new { success = true });
        }

        /// <summary>
        ///User Registration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("userregistration"), AllowAnonymous]
        public async Task<IActionResult> UserRegistration(UserRegistrationDTO data)
        {
            if (Data.User.CheckPasswordStrength(data.Password) != PasswordScores.VeryStrong)
                return StatusCode(StatusCodes.Status403Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            var contact = await (from c in DataContext.Users where (data.UserName == c.UserName) select c).FirstOrDefaultAsync();

            if (contact != null)
                return StatusCode(StatusCodes.Status403Forbidden, "We're sorry but that UserName already Exists");

            var user = new User
            {
                UserName = data.UserName,
                PasswordHash = data.Password.ComputeHash(),
                Title = data.Title,
                FirstName = data.FirstName,
                LastName = data.LastName,
                MiddleName = data.MiddleName,
                Phone = data.Phone,
                Fax = data.Fax,
                Email = data.Email,
                Active = false,
                OrganizationRequested = data.OrganizationRequested,
                RoleRequested = data.RoleRequested,
                SignedUpOn = DateTime.Now,
                Deleted = false
            };

            DataContext.Users.Add(user);

            var action = new Data.PmnTask
            {
                Body = "<p>" + user.FirstName + " " + user.LastName + " has registered in the system. Please <a href=\"/users/details?ID=" + user.ID + "\">click here</a> to review their information.</p>",
                Subject = "A new user has registered: " + user.FirstName + " " + user.LastName,
                DueDate = DateTime.UtcNow,
                Status = TaskStatuses.NotStarted,
                Type = TaskTypes.NewUserRegistration | TaskTypes.Task
            };

            action.References.Add(new TaskReference
            {
                Task = action,
                Item = user.FirstName + " " + user.LastName,
                ItemID = user.ID,
                Type = TaskItemTypes.User
            });



            //Add the users here based on a query of people that can process it.
            var userIds = await (from u in DataContext.Users
                                 where
  (DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(usr => usr.UserID == u.ID) && a.PermissionID == PermissionIdentifiers.Organization.ApproveRejectRegistrations).Any() &&
  DataContext.GlobalAcls.Where(a => a.SecurityGroup.Users.Any(usr => usr.UserID == u.ID) && a.PermissionID == PermissionIdentifiers.Organization.ApproveRejectRegistrations).All(a => a.Allowed))
                                 select u.ID).ToArrayAsync();

            foreach (var userId in userIds)
                action.Users.Add(new PmnTaskUser
                {
                    Task = action,
                    Role = TaskRoles.Worker,
                    UserID = userId
                });

            DataContext.Actions.Add(action);

            await DataContext.SaveChangesAsync();

            return Accepted(new { success = true });
        }

        /// <summary>
        /// Returns a list of current tasks for the specified user or the currently logged in user, the content has been extended to provide workflow information if available.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("GetWorkflowTasks")]
        public IActionResult GetWorkflowTasks(ODataQueryOptions<HomepageTaskSummaryDTO> options, Guid? userID = null)
        {
            userID = userID ?? Identity.ID;

            var query = from t in DataContext.Actions
                        let request = DataContext.Requests.Where(r => t.References.Any(rf => rf.ItemID == r.ID && rf.Type == TaskItemTypes.Request)).FirstOrDefault()
                        let newUser = DataContext.Users.Where(u => t.References.Any(rf => rf.ItemID == u.ID && rf.Type == TaskItemTypes.User)).FirstOrDefault()
                        let isRegistration = (t.Type & TaskTypes.NewUserRegistration) == TaskTypes.NewUserRegistration
                        where t.Users.Any(u => u.UserID == userID.Value && u.User.Deleted == false)
                        && t.Status != TaskStatuses.Complete
                        select new HomepageTaskSummaryDTO
                        {
                            TaskID = t.ID,
                            TaskName = !string.IsNullOrEmpty(t.Subject) ? t.Subject : t.WorkflowActivityID.HasValue ? t.WorkflowActivity.Name : (isRegistration ? "Registration Review" : "Task"),
                            TaskStatus = t.Status,
                            TaskStatusText = t.Status == TaskStatuses.Cancelled ? "Cancelled" :
                                             t.Status == TaskStatuses.NotStarted ? "Not Started" :
                                             t.Status == TaskStatuses.InProgress ? "In Progress" :
                                             t.Status == TaskStatuses.Deferred ? "Deferred" :
                                             t.Status == TaskStatuses.Blocked ? "Blocked" :
                                             t.Status == TaskStatuses.Complete ? "Complete" : "Unknown",
                            CreatedOn = t.CreatedOn,
                            StartOn = t.StartOn,
                            EndOn = t.EndOn,
                            Type = t.WorkflowActivityID.HasValue ? "Workflow:" + request.Workflow.Name : isRegistration ? "User Registration" : "",
                            DirectToRequest = t.DirectToRequest,
                            MSRequestID = request.MSRequestID,
                            NewUserID = newUser.ID,
                            Name = isRegistration ? (newUser != null ? (newUser.FirstName + " " + newUser.LastName) : "") : request != null ? request.Name : t.Subject,
                            Identifier = newUser != null ? newUser.UserName : request != null ? request.Identifier.ToString() : string.Empty,
                            RequestID = request.ID,
                            RequestStatus = request.Status,
                            RequestStatusText = DataContext.GetRequestStatusDisplayText(request.ID),
                            AssignedResources = DataContext.GetRequestAssigneesForTask(t.ID, "<br/>")
                        };
            var queryHelper = new Utilities.WebSites.ODataQueryHandler<HomepageTaskSummaryDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns an object indicating if the current user has Request metadata edit permission, and a list of the datamarts that the user can edit at least 1 requests metadata for.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getmetadataeditpermissionssummary")]
        public async Task<MetadataEditPermissionsSummaryDTO> GetMetadataEditPermissionsSummary()
        {
            var result = new MetadataEditPermissionsSummaryDTO { CanEditRequestMetadata = false, EditableDataMarts = Enumerable.Empty<Guid>() };
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());
            var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity.ID, Array.Empty<Guid>());

            result.CanEditRequestMetadata = await (from r in DataContext.Secure<Request>(Identity).AsNoTrackingWithIdentityResolution()
                                                 let identityID = Identity.ID
                                                 let editPermissionID = PermissionIdentifiers.Request.Edit.ID
                                                 let editRequestMetaDataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                                                 let gAcl = DataContext.AclGlobalFiltered(identityID, editPermissionID).AsEnumerable()
                                                 let pAcl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editPermissionID)
                                                 let p2Acl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editRequestMetaDataPermissionID)
                                                 let poAcl = projectOrganizationAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID && a.PermissionID == editPermissionID)
                                                 where (gAcl.Any() || pAcl.Any() || poAcl.Any()) &&
                                                 (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))
                                                 && ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                                                 select r).AnyAsync();

            if (result.CanEditRequestMetadata)
            {

                var datamarts = DataContext.Secure<DataMart>(Identity, PermissionIdentifiers.DataMartInProject.SeeRequests);
                var requests = DataContext.Secure<Request>(Identity);

                result.EditableDataMarts = await (from rdm in DataContext.RequestDataMarts
                                                  join dm in datamarts on rdm.DataMartID equals dm.ID
                                                  join r in requests on rdm.RequestID equals r.ID
                                                  let identityID = Identity.ID
                                                  let editPermissionID = PermissionIdentifiers.Request.Edit.ID
                                                  let editRequestMetaDataPermissionID = PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID
                                                  let gAcl = DataContext.AclGlobalFiltered(identityID, editPermissionID).AsEnumerable()
                                                  let pAcl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editPermissionID)
                                                  let p2Acl = projectAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.PermissionID == editRequestMetaDataPermissionID)
                                                  let poAcl = projectOrganizationAcls.AsEnumerable().Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID && a.PermissionID == editPermissionID)
                                                  where ((gAcl.Any() || pAcl.Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                                          ((int)r.Status < 500 ? true : (p2Acl.Any() && p2Acl.All(a => a.Allowed)))
                                                  select rdm.DataMartID).GroupBy(k => k).Select(k => k.Key).ToArrayAsync();

            }

            return result;
        }


    }
}

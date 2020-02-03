using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Lpp.Utilities;
using System.Net.Mail;
using Lpp.Utilities.WebSites.Controllers;
using System.Web.Configuration;
using Lpp.Utilities.Security;
using Lpp.Dns.DTO.Security;
using Lpp.Dns.DTO.Enums;
using System.Configuration;
using Lpp.Dns.Data.Audit;
using Lpp.Dns.DTO.Events;

namespace Lpp.Dns.Api.Users
{
    /// <summary>
    /// Web Api endpoint for working with Users
    /// </summary>
    public partial class UsersController : LppApiDataController<User, UserDTO, DataContext, PermissionDefinition>
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Gets the specified User
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async Task<UserDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Returns a secure list of Users that can be filtered using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<UserDTO> List()
        {
            return base.List().Where(u => !u.Deleted);
        }

        /// <summary>
        /// Validates the login information provided
        /// </summary>
        /// <param name="login">The Login and password object</param>
        /// <returns>The User that logged in if valid login or password. Error codes if fail</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<UserDTO> ValidateLogin(LoginDTO login)
        {
            try
            {
                IUser user;
                if (!DataContext.ValidateUser2(login.UserName, login.Password, out user))
                {
                    if(user == null)
                    {
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid user name or password."));
                    }
                    string errorMessage = "The Login or Password is invalid.";
                    var contact = (User)user;
                    contact.FailedLoginCount++;
                    if (contact.FailedLoginCount >= Convert.ToInt32(ConfigurationManager.AppSettings["FailedLoginAttemptsBeforeLockingOut"]))
                    {
                        if (contact.Active == true)
                        {
                            contact.DeactivatedOn = DateTime.UtcNow;
                            contact.Active = false;
                        }
                        errorMessage = "Your account has been locked after too many unsuccessful login attempts. Please contact your administrator.";
                    }
                    Dns.Data.Audit.UserAuthenticationLogs failedAudit = new UserAuthenticationLogs
                    {
                        UserID = contact.ID,
                        Description = "User Authenticated Failed from " + login.Enviorment + " from IP Address: " + login.IPAddress,
                        Success = false,
                        IPAddress = login.IPAddress,
                        Enviorment = login.Enviorment,
                        Details = Lpp.Utilities.Crypto.EncryptStringAES("UserName: " + login.UserName + " was attempted with Password:" + login.Password,"AuthenticationLog", contact.ID.ToString("D"))
                    };
                    DataContext.LogsUserAuthentication.Add(failedAudit);
                    await DataContext.SaveChangesAsync();
                    if (!contact.Active)
                    {
                        System.Threading.Thread.Sleep(contact.FailedLoginCount * 3000);
                    }                    
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, errorMessage));
                }
                if(!((User)user).Active)
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User Account is locked"));
                }
                if(((User)user).FailedLoginCount > 0)
                {
                    var contact = (User)user;
                    contact.FailedLoginCount = 0;
                    await DataContext.SaveChangesAsync();
                }
                if (login.Enviorment != null && login.IPAddress != null)
                {
                    Dns.Data.Audit.UserAuthenticationLogs successAudit = new UserAuthenticationLogs
                    {
                        UserID = user.ID,
                        Description = "User Authenticated Successful from " + login.Enviorment + " from IP Address: " + login.IPAddress,
                        Success = true,
                        IPAddress = login.IPAddress,
                        Enviorment = login.Enviorment
                    };
                    DataContext.LogsUserAuthentication.Add(successAudit);
                    await DataContext.SaveChangesAsync(); 
                }
                return ((User)user).Map<User, UserDTO>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a specific user by User Name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserDTO> ByUserName(string userName)
        {
            var contacts = await (from c in DataContext.Secure<User>(Identity) where c.UserName == userName && c.Deleted == false select c).Map<User, UserDTO>().ToListAsync();
            if (!contacts.Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "User Not Found"));

            return contacts.First();
        }

        /// <summary>
        ///User Registration
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> UserRegistration(UserRegistrationDTO data)
        {
            if (Data.User.CheckPasswordStrength(data.Password) != PasswordScores.VeryStrong)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            var contact = await (from c in DataContext.Users where (data.UserName == c.UserName) select c).FirstOrDefaultAsync();

            if (contact != null)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "We're sorry but that UserName already Exists");

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

            var action = new Lpp.Dns.Data.PmnTask
            {
                Body = "<p>" + user.FirstName + " " + user.LastName + " has registered in the system. Please <a href=\"/users/details?ID=" + user.ID + "\">click here</a> to review their information.</p>",
                Subject = "A new user has registered: " + user.FirstName + " " + user.LastName,
                DueDate = DateTime.UtcNow,
                Status = TaskStatuses.InProgress,
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
            var userIds = await (from u in DataContext.Users where
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

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns the projects that a user can see, administer or otherwise submit or interacti with requests for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<ProjectDTO> ListAvailableProjects()
        {
            return ReturnAvailableProjects();
        }

        private IQueryable<ProjectDTO> ReturnAvailableProjects()
        {
            var projects = DataContext.Secure<Project>(Identity, PermissionIdentifiers.Request.ViewRequest, PermissionIdentifiers.Request.ApproveRejectSubmission, /*PermissionIdentifiers.Project.ListRequests,*/ PermissionIdentifiers.Project.Edit, PermissionIdentifiers.DataMartInProject.ApproveResponses, PermissionIdentifiers.DataMartInProject.GroupResponses, PermissionIdentifiers.DataMartInProject.HoldRequest, PermissionIdentifiers.DataMartInProject.RejectRequest, PermissionIdentifiers.DataMartInProject.SeeRequests, /*PermissionIdentifiers.DataMartInProject.SkipResponseApproval,*/ PermissionIdentifiers.Request.ChangeRoutings, PermissionIdentifiers.Request.ViewResults, PermissionIdentifiers.Request.ViewIndividualResults, PermissionIdentifiers.Request.ViewStatus);

            var projSubmit = from p in DataContext.Secure<Project>(Identity) where DataContext.ProjectRequestTypeAcls.Any(a => a.ProjectID == p.ID && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID) && a.Permission != RequestTypePermissions.Deny) || DataContext.DataMartRequestTypeAcls.Any(a => p.DataMarts.Any(dm => a.DataMartID == dm.DataMartID) && a.Permission != RequestTypePermissions.Deny && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID)) select p;

            return projects.Concat(projSubmit).Where(p => !p.Deleted).Distinct().Map<Project, ProjectDTO>();
        }

        /// <summary>
        /// Requests a password reset email
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> ForgotPassword(ForgotPasswordDTO data)
        {
            var contact = await (from c in DataContext.Users where ((!string.IsNullOrEmpty(data.UserName) && data.UserName == c.UserName) || (!string.IsNullOrEmpty(data.Email) && data.Email == c.Email)) && !c.Deleted select c).FirstOrDefaultAsync();

            if (contact == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "We're sorry but we could not find you in our system. Please check your user name or password and try again.");

            if (contact.Email.IsNullOrWhiteSpace())
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "We're sorry but we do not have a valid email address on file for you. Please use the contact us link to request that your account be reset.");

            // Send the email here.
            try
            {
                using (var smtp = new SmtpClient())
                {
                    contact.PasswordRestorationToken = Guid.NewGuid();
                    contact.PasswordRestorationTokenExpiration = DateTime.UtcNow.AddHours(1);

                    //TODO: Use an HTML template for this email that is specific per organization or project or site.
                    var message = new MailMessage
                    {
                        To = { new MailAddress(contact.Email, contact.UserName) },
                        Subject = "Restore Password"
                    };

                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(string.Format("<div style=\"font-face: arial;\"><p>{0},</p><p>To restore your password, please click this link: <a href=\"{1}\">{1}</a></p></div>",
                                    (contact.FirstName + " " + contact.LastName).Trim(),
                                    WebConfigurationManager.AppSettings["ResetPasswordUrl"] + contact.PasswordRestorationToken.ToString()), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Html)));

                    message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(string.Format("{0}\r\nTo restore your password please click this link: \r\n{1}", (contact.FirstName + " " + contact.LastName).Trim(), WebConfigurationManager.AppSettings["ResetPasswordUrl"] + contact.PasswordRestorationToken.ToString()), new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Text.Plain)));

                    message.AlternateViews[0].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;
                    message.AlternateViews[1].TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable;

                    //if(true)
                    //    throw new Exception("Thrown Exception");

                    await smtp.SendMailAsync(message);
                }

                await DataContext.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.Accepted);

            }
            catch (SmtpException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Smtp Error");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePassword(UpdateUserPasswordDTO updateInfo)
        {

            if (Data.User.CheckPasswordStrength(updateInfo.Password) != PasswordScores.VeryStrong)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            //Check if the user has permissions
            if (updateInfo.UserID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, updateInfo.UserID, PermissionIdentifiers.User.ChangePassword)).Contains(PermissionIdentifiers.User.ChangePassword))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to change the specified user's password");

            //Update the password
            var user = await DataContext.Users.FindAsync(updateInfo.UserID);
            user.PasswordHash = updateInfo.Password.ComputeHash();
            user.PasswordExpiration = DateTime.Now.AddMonths(ConfigurationManager.AppSettings["ConfiguredPasswordExpiryMonths"].ToInt32());
            //Save it
            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Restores an account with a new password based on the forgot password token.
        /// </summary>
        /// <param name="updateInfo"></param>
        /// <returns></returns>
        [HttpPut, AllowAnonymous]
        public async Task<HttpResponseMessage> RestorePassword(RestorePasswordDTO updateInfo)
        {
            var user = await (from u in DataContext.Users where u.PasswordRestorationToken == updateInfo.PasswordRestoreToken && !u.Deleted select u).FirstOrDefaultAsync();

            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "We're sorry but we could not find the password restore request.");

            if (user.PasswordRestorationTokenExpiration == null || user.PasswordRestorationTokenExpiration.Value < DateTime.UtcNow)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "We're sorry but the request to reset your password has expired. Please create a new request.");

            if (Data.User.CheckPasswordStrength(updateInfo.Password) != PasswordScores.VeryStrong)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "The password specified is not strong enough. Please ensure that the password has at least one upper-case letter, a number and at least one symbol and does not include: ':;<'.");

            user.PasswordHash = updateInfo.Password.ComputeHash();
            user.PasswordRestorationToken = null;
            user.PasswordRestorationTokenExpiration = null;

            await DataContext.SaveChangesAsync();
            
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns all assigned notifications for user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IQueryable<AssignedUserNotificationDTO>> GetAssignedNotifications(Guid userID)
        {
            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                return null;

            return DataContext.GetAssignedUserNotifications(userID);
        }

        /// <summary>
        /// Returns the events that a given user can subscribe to.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
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
                               e.UserEvents.Any(ue => ue.SecurityGroup.Users.Any(u => u.UserID == userID)  && ue.Allowed) &&
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
                               
                           select e).Map<Event, EventDTO>();

            return results;
        }

        /// <summary>
        /// Returns the currently subscribed events
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IQueryable<UserEventSubscriptionDTO>> GetSubscribedEvents(Guid userID)
        {
            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage notifications for the specified user."));

            var results = (from se in DataContext.UserEventSubscriptions where se.UserID == userID select se).Map<UserEventSubscription, UserEventSubscriptionDTO>();

            return results;
        }

        /// <summary>
        /// Returns notifications for the current user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<NotificationDTO> GetNotifications(Guid userID)
        {
            if (userID != Identity.ID)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to view notifications for the specified user."));

            // PMNDEV-3117 - Resolve performance issue as a stop gap measure by limiting it to 2 weeks of notifications.
            var twoWeeksAgo = DateTimeOffset.UtcNow.AddDays(-14);
            var notifications = DataContext.GetNotifications(userID); //.ToArray();
            //var nn = from n in notifications
            //         where n.Timestamp < twoWeeksAgo
            //         select n;

            return notifications; //.AsQueryable();
        }
        /// <summary>
        /// Returns an IQueryable List of Successful Audit Events for a specific User
        /// </summary>
        /// <returns>A Queryable list of UserAuthenticationDTO</returns>
        [HttpGet]
        public IQueryable<UserAuthenticationDTO> ListSuccessfulAudits()
        {
            return (from u in DataContext.LogsUserAuthentication
                    select new UserAuthenticationDTO {
                        DateTime = u.TimeStamp,
                        UserID = u.UserID,
                        Success = u.Success,
                        ID = u.ID,
                        Description = u.Description,
                        IPAddress = u.IPAddress,
                        Enviorment = u.Enviorment
                    }).AsQueryable();
        }

        /// <summary>
        /// Updates the user's subscribed events. Note: You MUST pass all subscribed events for the user to this endpoint.
        /// </summary>
        /// <param name="subscribedEvents"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateSubscribedEvents(IEnumerable<UserEventSubscriptionDTO> subscribedEvents) {

            if (!subscribedEvents.Any())
                return Request.CreateResponse(HttpStatusCode.Accepted);

            if (subscribedEvents.Select(s => s.UserID).Distinct().Count() > 1)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You may only update a single user's event subscriptions at a time."));

            var userID = subscribedEvents.Select(s => (Guid?)s.UserID).First();

            if (userID != Identity.ID && !(await DataContext.HasGrantedPermissions<User>(Identity, (Guid) userID, PermissionIdentifiers.User.ManageNotifications)).Contains(PermissionIdentifiers.User.ManageNotifications))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to manage notifications for the specified user."));

            var existing = await (from s in DataContext.UserEventSubscriptions where s.UserID == userID select s).ToArrayAsync();

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
                    LastRunTime = newSub.LastRunTime,
                    NextDueTime = newSub.NextDueTime,
                    UserID = newSub.UserID
                });
            }

            //Get current matches and update frequencies as necessary
            var sameSubs = from ss in existing join se in subscribedEvents on new { ss.EventID, ss.UserID } equals new { se.EventID, se.UserID }
                           where (((se.Frequency.HasValue && ss.Frequency != se.Frequency.Value) || (se.FrequencyForMy.HasValue && ss.FrequencyForMy != se.FrequencyForMy.Value))
                           || (!se.FrequencyForMy.HasValue && ss.FrequencyForMy.HasValue )
                           || (!se.Frequency.HasValue && ss.Frequency.HasValue)
                           )
                           select new { existing = ss, newSub = se };
            foreach (var sameSub in sameSubs)
            {
                sameSub.existing.Frequency = sameSub.newSub.Frequency != null ? sameSub.newSub.Frequency.Value : sameSub.newSub.Frequency;
                sameSub.existing.FrequencyForMy = sameSub.newSub.FrequencyForMy != null ? sameSub.newSub.FrequencyForMy.Value : sameSub.newSub.FrequencyForMy;
            }


            //Delete ones that are no longer there.
            var deleteSubs = from ee in existing
                             where !subscribedEvents.Any(se => se.EventID == ee.EventID && se.UserID == ee.UserID) || 
                             subscribedEvents.Any(se => se.EventID == ee.EventID && se.UserID == ee.UserID && !se.Frequency.HasValue && !se.FrequencyForMy.HasValue)
                             select ee;

            DataContext.UserEventSubscriptions.RemoveRange(deleteSubs);

            await DataContext.SaveChangesAsync();

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns a list of current tasks for the specified user or the currently logged in user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<TaskDTO> GetTasks(Guid? userID = null)
        {
            userID = userID ?? Identity.ID;

            var results = (from a in DataContext.Actions where a.Users.Any(u => u.UserID == userID.Value && a.Status != TaskStatuses.Complete && u.User.Deleted == false) select a).Map<Data.PmnTask, TaskDTO>();

            return results;
        }

        /// <summary>
        /// Returns a list of current tasks for the specified user or the currently logged in user, the content has been extended to provide workflow information if available.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<HomepageTaskSummaryDTO> GetWorkflowTasks(Guid? userID = null)
        {
            userID = userID ?? Identity.ID;

            var results = from t in DataContext.Actions
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
                              TaskStatusText = t.Status == TaskStatuses.Cancelled ? "Cancelled":
                                               t.Status == TaskStatuses.NotStarted ? "Not Started":
                                               t.Status == TaskStatuses.InProgress ? "In Progress":
                                               t.Status == TaskStatuses.Deferred ? "Deferred":
                                               t.Status == TaskStatuses.Blocked ? "Blocked":
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

            return results;
        }

        /// <summary>
        /// Gets a collection of user details based on the specified user's current workflow tasks. If no user is specified the current user is assumed.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<HomepageTaskRequestUserDTO> GetWorkflowTaskUsers(Guid? userID = null)
        {
            Guid ID = userID ?? Identity.ID;
            
            //get all the tasks the user is associated with
            var tasks = from t in DataContext.Actions
                        where t.Status != TaskStatuses.Complete
                        && t.References.Any(ar => ar.Type == TaskItemTypes.Request && DataContext.RequestUsers.Any(ru => ru.RequestID == ar.ItemID && ru.UserID == ID))
                        && t.Users.Any(tu => tu.UserID == ID)
                        select t;

            var results = (from u in DataContext.RequestUsers
                          join tr in DataContext.ActionReferences on u.RequestID equals tr.ItemID
                          join t in tasks on tr.TaskID equals t.ID
                          select new
                          {
                              RequestID = u.RequestID,
                              TaskID = tr.TaskID,
                              UserID = u.UserID,
                              UserName = u.User.UserName,
                              FirstName = u.User.FirstName,
                              LastName = u.User.LastName,
                              WorkflowRoleID = u.WorkflowRoleID,
                              WorkflowRole = u.WorkflowRole.Name,
                              WorkflowID = u.WorkflowRole.WorkflowID,
                              Workflow = u.WorkflowRole.Workflow.Name
                          })
                          .GroupBy(g => new { g.RequestID, g.TaskID, g.UserID, g.WorkflowRoleID })
                          .Select(s => new HomepageTaskRequestUserDTO {
                              RequestID = s.Key.RequestID,
                              TaskID = s.Key.TaskID,
                              UserID = s.Key.UserID,
                              UserName = s.Select(x => x.UserName).FirstOrDefault(),
                              FirstName = s.Select(x => x.FirstName).FirstOrDefault(),
                              LastName = s.Select(x => x.LastName).FirstOrDefault(),
                              WorkflowRoleID = s.Key.WorkflowRoleID,
                              WorkflowRole = s.Select(x => x.WorkflowRole).FirstOrDefault()
                          });

            return results;
        }


        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Logout()
        {
            //var user = await (from u in DataContext.Users where u.ID == Identity.ID select u).FirstOrDefaultAsync();

            //Notify all signlr clients of the logout based on security

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Returns a list of security groups that the user is a member of based on the user passed
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IQueryable<SecurityGroupDTO>> MemberOfSecurityGroups(Guid userID)
        {
            if (!(await DataContext.HasGrantedPermissions<User>(Identity, userID, PermissionIdentifiers.User.ManageSecurity)).Any())
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to view the security groups of this user."));

            var results = (from sg in DataContext.Secure<SecurityGroup>(Identity) where sg.Users.Any(u => u.UserID == userID) select sg).Map<SecurityGroup, SecurityGroupDTO>();

            return results;
        }

        /// <summary>
        /// Updates the security groups that the user is assigned to. All security groups must be sent as ones not in the list will be removed.
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateSecurityGroups(UpdateUserSecurityGroupsDTO groups)
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

            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// returns if the user has a global permission
        /// </summary>
        /// <param name="permissionID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> GetGlobalPermission(Guid permissionID)
        {
            return await DataContext.HasPermission(Identity, PermissionIdentifiers.Definitions.First(p => p.ID == permissionID));
        }
        /// <summary>
        /// Return main menu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<MenuItemDTO>> ReturnMainMenu()
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
                url = "/",
                text = "Home"
            });

            var projects = this.ReturnAvailableProjects().ToArray();
            if (projects.Any())
            {
                menu.Add(new MenuItemDTO
                {
                    url = "/requests",
                    text = "Requests",
                    items = projects.OrderBy(p => p.Name).Select(p => new MenuItemDTO
                    {
                        text = p.Name,
                        url = "/requests?projectid=" + p.ID
                    })
                });
            }

            menu.Add(new MenuItemDTO
            {
                text = "Profile",
                url = "/users/details?ID=" + Identity.ID
            });

            menu.Add(new MenuItemDTO
            {
                text = "Resources",
                url = "/Home/resources"
            });

            var reportItems = new List<MenuItemDTO>();

            if (permissions.Contains(PermissionIdentifiers.DataMart.RunAuditReport))
                reportItems.Add(new MenuItemDTO
                {
                    text = "DataMart Audit",
                    url = "/reports/DataMartAuditReport"
                });


            //if (permissions.Contains(PermissionIdentifiers.Portal.RunEventsReport))
            //    reportItems.Add(new MenuItemDTO
            //    {
            //        text = "Event Log",
            //        url = "/report/lists/create"
            //    });

            if (permissions.Contains(PermissionIdentifiers.Portal.RunNetworkActivityReport))
                reportItems.Add(new MenuItemDTO
                {
                    text = "Network Activity",
                    url = "/reports/NetworkActivityReport"
                });

            if (reportItems.Any())                        
                menu.Add(new MenuItemDTO
                {
                    text = "Reports",
                    url = null,
                    items = reportItems
                });

            var setupItems = new List<MenuItemDTO>();
            if (permissions.Contains(PermissionIdentifiers.Portal.ListDataMarts))
                setupItems.Add(new MenuItemDTO {
                        text = "DataMarts",
                        url = "/datamarts"
                    });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListGroups))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Groups",
                    url = "/groups"
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
                    text = "Projects",
                    url = "/projects"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListRegistries))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Registries",
                    url = "/registries"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListOrganizations))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Organizations",
                    url = "/organizations"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListUsers))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Users",
                    url = "/users"
                });

            //BUG: Should be secured, don't know what permission to use
            //setupItems.Add(new MenuItemDTO
            //{
            //    text = "Models",
            //    url = "/models/list"
            //});

            if (permissions.Contains(PermissionIdentifiers.Portal.CreateNetworkMessages))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Messages",
                    url = "/networkmessages/create"
                });

            if (permissions.Contains(PermissionIdentifiers.Portal.ListTemplates))
            {
                setupItems.Add(new MenuItemDTO { 
                    text = "Criteria Group Templates",
                    url = "/templates"
                });
            }

            if (permissions.Contains(PermissionIdentifiers.Portal.ListRequestTypes))
            {
                setupItems.Add(new MenuItemDTO
                {
                    text = "Request Types",
                    url = "/RequestType"
                });
            }

            if (permissions.Contains(PermissionIdentifiers.Portal.ManageSecurity))
                setupItems.Add(new MenuItemDTO
                {
                    text = "Site-Wide Security Permissions",
                    url = "/defaultsecuritypermissions"
                });

            if (setupItems.Any())
                menu.Add(new MenuItemDTO {
                    text = "Network",
                    url = null,
                    items = setupItems
                });

            return menu;
        }

        ///<summary>
        /// Tests Connection to lookup list update. To be removed once automatic updater is functioning.
        /// </summary>
        /// <param name="username">The username of the user executing the request.</param>
        /// <param name="password">The password of the user executing the request.</param>
        [HttpGet, AllowAnonymous]
        public async Task<HttpResponseMessage> UpdateLookupListsTest(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username and password are required.");

            string hashedPassword = Password.ComputeHash(password);
            Guid? userID = await DataContext.Users.Where(u => u.UserName == username && u.PasswordHash == hashedPassword && u.Active && !u.Deleted).Select(u => (Guid?)u.ID).FirstOrDefaultAsync();

            if (!userID.HasValue)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid credentials.");
            }

            string serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Url"];
            string serviceUser = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.User"] ?? string.Empty).DecryptString();
            string servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.Password"] ?? string.Empty).DecryptString();

            if (string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(serviceUser) || string.IsNullOrEmpty(servicePassword))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "External service configuration is incomplete, make sure the service url and credentials have been configured correctly.");
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, "Test was successful.");
        }

        /// <summary>
        /// Updates the medical code lists from an external service.
        /// </summary>
        /// <param name="username">The username of the user executing the request.</param>
        /// <param name="password">The password of the user executing the request.</param>
        [HttpGet, AllowAnonymous]
        public async Task<HttpResponseMessage> UpdateLookupLists(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username and password are required.");

            string hashedPassword = Password.ComputeHash(password);
            Guid? userID = await DataContext.Users.Where(u => u.UserName == username && u.PasswordHash == hashedPassword && u.Active && !u.Deleted).Select(u => (Guid?)u.ID).FirstOrDefaultAsync();

            if (!userID.HasValue)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid credentials.");
            }
            
            string serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Url"];
            string serviceUser = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.User"] ?? string.Empty).DecryptString();
            string servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.Password"] ?? string.Empty).DecryptString();

            if (string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(serviceUser) || string.IsNullOrEmpty(servicePassword))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "External service configuration is incomplete, make sure the service url and credentials have been configured correctly.");
            }

            //For Each Code Type:
            //Generic Name
            string genericNameCategory = "rx";
            string genericNameSource = "fdb";
            string genericNameCodeClass = "fdb-etc";

            var genericNameUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, genericNameCategory, genericNameSource, genericNameCodeClass, "", Lpp.Dns.DTO.Enums.Lists.GenericName);
            await genericNameUpdater.DoUpdate();

            //Drug Class
            string drugClassCategory = "rx";
            string drugClassSource = "fdb";
            string drugClassCodeClass = "fdb-etc";

            var drugClassUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, drugClassCategory, drugClassSource, drugClassCodeClass, "", Lpp.Dns.DTO.Enums.Lists.DrugClass);
            await drugClassUpdater.DoUpdate();

            //DX ICD-9 3 digit
            string diagnosisCategory = "dx";
            string diagnosisSource = "optum";
            string diagnosisCodeClass = "icd-9-cm";

            var threeDigitDiagnosisUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, diagnosisCategory, diagnosisSource, diagnosisCodeClass, "3", Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis);
            await threeDigitDiagnosisUpdater.DoUpdate();

            //DX ICD-9 4 digit
            var fourDigitDiagnosisUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, diagnosisCategory, diagnosisSource, diagnosisCodeClass, "4", Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis4Digits);
            await fourDigitDiagnosisUpdater.DoUpdate();

            //DX ICD-9 5 digit
            var fiveDigitDiagnosisUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, diagnosisCategory, diagnosisSource, diagnosisCodeClass, "5", Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis5Digits);
            await fiveDigitDiagnosisUpdater.DoUpdate();

            //PX ICD-9 3 digit
            string procedureCategory = "px";
            string procedureSource = "optum";
            string procedureCodeClass = "icd-9-cm";

            var threeDigitProcedureUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, procedureCategory, procedureSource, procedureCodeClass, "3", Lpp.Dns.DTO.Enums.Lists.ICD9Procedures);
            await threeDigitProcedureUpdater.DoUpdate();
            
            //PX ICD-9 4 digit
            var fourDigitProcedureUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, procedureCategory, procedureSource, procedureCodeClass, "4", Lpp.Dns.DTO.Enums.Lists.ICD9Procedures4Digits);
            await fourDigitProcedureUpdater.DoUpdate();
            
            //HCPCS Procedures
            string hcpcsCategory = "px";
            string hcpcsSource = "optum";
            string hcpcsCodeClass = "hcpcs-cpt";

            var hcpcsUpdater = new CodeLookupListsUpdater(DataContext, serviceUrl, serviceUser, servicePassword, hcpcsCategory, hcpcsSource, hcpcsCodeClass, "", Lpp.Dns.DTO.Enums.Lists.HCPCSProcedures);
            await hcpcsUpdater.DoUpdate();


            //return ok for status if update was successful.
            return Request.CreateResponse(HttpStatusCode.OK);

        }


        /// <summary>
        /// Allows saving of a user setting 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> SaveSetting(UserSettingDTO setting) {
            if (setting.UserID != Identity.ID)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to save settings on the specified user.");

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

                return Request.CreateResponse(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// Returns a list of settings based on the key
        /// </summary>
        /// <param name="key">The array of keys of the settings to retrieve. You may specify more than one.</param>
        /// <returns>A list of settings by key</returns>
        [HttpGet]
        public async Task<IEnumerable<UserSettingDTO>> GetSetting([FromUri]IEnumerable<string> key) {
            var setting = await (from s in DataContext.UserSettings where s.UserID == Identity.ID && key.Contains(s.Key) select new UserSettingDTO
            {
                Key = s.Key,
                Setting = s.Setting,
                UserID = s.UserID
            }).ToArrayAsync();

            return setting;
        }

        ///<summary>
        /// Check if user has permission to Approve or Reject a request
        /// </summary>
        [HttpGet]
        public async Task<bool> AllowApproveRejectRequest(Guid requestID)
        { 
            //Locations: Global, Organizations, Projects, Users, Project Organizations
            var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var organizationAcls = DataContext.OrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var userAcls = DataContext.UserAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);
            var projectOrgAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.ApproveRejectSubmission);


            var results = await ( from r in DataContext.Secure<Request>(Identity)
                          where r.ID == requestID
                          let gAcls = globalAcls
                          let oAcls = organizationAcls.Where(a => a.OrganizationID == r.OrganizationID )
                          let pAcls = projectAcls.Where(a => a.ProjectID == r.ProjectID )
                          let uAcls = userAcls.Where(a => a.UserID == r.SubmittedByID )
                          let poAcls = projectOrgAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID )
                          where (
                            (gAcls.Any() || oAcls.Any() || pAcls.Any() || uAcls.Any() || poAcls.Any())
                            &&
                            (gAcls.All(a => a.Allowed) && oAcls.All(a => a.Allowed) && pAcls.All(a => a.Allowed) && uAcls.All(a => a.Allowed) && poAcls.All(a => a.Allowed))
                          )
                          select r.ID).AnyAsync();

            return results;
            
        }

        /// <summary>
        /// Check if user password has been set.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> HasPassword(Guid userID)
        {
            var currentUser = await (from u in DataContext.Users where u.ID == userID select u).FirstOrDefaultAsync();

            return currentUser.PasswordHash != null && currentUser.PasswordHash != "";
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        public override async Task Delete([FromUri]IEnumerable<Guid> ID)
        {
            if (!await DataContext.CanDelete<User>(Identity, ID.ToArray()))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You do not have permission to delete this User."));

            var users = await (from u in DataContext.Users where ID.Contains(u.ID) select u).ToArrayAsync();
            foreach (var user in users)
                user.Deleted = true;

            await DataContext.SaveChangesAsync();
        }
        /// <summary>
        /// Insert or update list of Users
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IEnumerable<UserDTO>> InsertOrUpdate(IEnumerable<UserDTO> values)
        {
            await ValidateActiveUserToOrganization(values);
            var origUsers = values.Where(x => x.ID != Guid.Empty && x.ID != null).ToList();
            var network = DataContext.Networks.Where(x => x.Name != "Aqueduct").FirstOrDefault();
            var users = await base.InsertOrUpdate(values);

            await CompleteRegistrationTask(users);

            return users;
        }
        /// <summary>
        /// Update list of users
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPut]
        public override async Task<IEnumerable<UserDTO>> Update(IEnumerable<UserDTO> values)
        {
            await ValidateActiveUserToOrganization(values);
            var network = DataContext.Networks.Where(x => x.Name != "Aqueduct").FirstOrDefault();
            var users = await base.Update(values);

            await CompleteRegistrationTask(users);

            return users;
        }
        /// <summary>
        /// Insert list of users
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IEnumerable<UserDTO>> Insert(IEnumerable<UserDTO> values)
        {
            await ValidateActiveUserToOrganization(values);
            var network = DataContext.Networks.Where(x => x.Name != "Aqueduct").FirstOrDefault();
            var users = await base.Insert(values);
            return users;
        }

        private async Task CompleteRegistrationTask(IEnumerable<UserDTO> users)
        {
            var userIDs = users.Where(u => u.ID.HasValue && u.Active).Select(u => u.ID.Value).ToArray();

            var tasks = await (from a in DataContext.Actions where a.References.Any(r => userIDs.Contains(r.ItemID)) && a.Status != TaskStatuses.Complete && (a.Type & TaskTypes.NewUserRegistration) == TaskTypes.NewUserRegistration select a).ToArrayAsync();

            foreach (var task in tasks)
            {
                task.Status = TaskStatuses.Complete;
            }

            await DataContext.SaveChangesAsync();
        }

        private async Task ValidateActiveUserToOrganization(IEnumerable<UserDTO> users)
        {
            var userIDs = users.Where(u => u.ID.HasValue).Select(u => u.ID.Value).ToArray();

            var currentUsers = await (from u in DataContext.Users where userIDs.Contains(u.ID) select u).ToArrayAsync();

            foreach (var user in users)
            {
                if (user.ID == null)
                {
                    if (user.Active)
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, user.FirstName + " " + user.LastName + " does not have a password and cannot be marked as active."));

                    var existingUserName = await (from u in DataContext.Users where u.UserName == user.UserName && !u.Deleted select u).CountAsync() > 0;
                    if (existingUserName)
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The username " + user.UserName + " already exists."));
                }
                else
                {
                    var currentUser = currentUsers.FirstOrDefault(u => u.ID == user.ID.Value);
                    if (currentUser == null)
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, user.FirstName + " " + user.LastName + " could not be found."));

                    if (!currentUser.Active && user.Active && currentUser.PasswordHash.IsNullOrEmpty())
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, user.FirstName + " " + user.LastName + " must be assigned a password before the can be marked active."));

                    if (currentUser.OrganizationID == null & user.OrganizationID != null && 
                        user.OrganizationID.Value != Identity.EmployerID.Value && 
                        !await DataContext.GlobalAcls.AnyAsync(a => a.PermissionID == PermissionIdentifiers.Organization.CreateUsers && a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID)) && 
                        !await DataContext.OrganizationAcls.AnyAsync(a => a.PermissionID == PermissionIdentifiers.Organization.CreateUsers && a.OrganizationID == currentUser.OrganizationID.Value && 
                            a.SecurityGroup.Users.Any(u => u.UserID == Identity.ID)))
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You may only set the organization for " + user.FirstName + " " + user.LastName + " to any organization for which you have the right to accept/reject registrations."));

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
        /// Returns an object indicating if the current user has Request metadata edit permission, and a list of the datamarts that the user can edit at least 1 requests metadata for.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MetadataEditPermissionsSummaryDTO> GetMetadataEditPermissionsSummary()
        {
            var result = new MetadataEditPermissionsSummaryDTO { CanEditRequestMetadata = false, EditableDataMarts = Enumerable.Empty<Guid>() };
            var globalAcls = DataContext.GlobalAcls.FilterAcl(Identity, PermissionIdentifiers.Request.Edit);
            var projectAcls = DataContext.ProjectAcls.FilterAcl(Identity, PermissionIdentifiers.Request.Edit, PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata);
            var projectOrganizationAcls = DataContext.ProjectOrganizationAcls.FilterAcl(Identity, PermissionIdentifiers.Request.Edit);

            result.CanEditRequestMetadata = await (from r in DataContext.Secure<Request>(Identity).AsNoTracking()
                                             let gAcl = globalAcls
                                             let pAcl = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                                             let poAcl = projectOrganizationAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID)
                                             where ((gAcl.Any() || pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                                    ((int)r.Status < 500 ? true : (pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).Any() && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).All(a => a.Allowed)))
                                             select r).AnyAsync();

            if (result.CanEditRequestMetadata)
            {

                var datamarts = DataContext.Secure<DataMart>(Identity, PermissionIdentifiers.DataMartInProject.SeeRequests);
                var requests = DataContext.Secure<Request>(Identity);

                result.EditableDataMarts = await (from rdm in DataContext.RequestDataMarts
                                                  join dm in datamarts on rdm.DataMartID equals dm.ID
                                                  join r in requests on rdm.RequestID equals r.ID
                                                  let gAcl = globalAcls
                                                  let pAcl = projectAcls.Where(a => a.ProjectID == r.ProjectID)
                                                  let poAcl = projectOrganizationAcls.Where(a => a.ProjectID == r.ProjectID && a.OrganizationID == r.OrganizationID)
                                                  where ((gAcl.Any() || pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).Any() || poAcl.Any()) && (gAcl.All(a => a.Allowed) && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.Request.Edit.ID).All(a => a.Allowed) && poAcl.All(a => a.Allowed))) &&
                                                          ((int)r.Status < 500 ? true : (pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).Any() && pAcl.Where(a => a.PermissionID == PermissionIdentifiers.ProjectRequestTypeWorkflowActivities.EditRequestMetadata.ID).All(a => a.Allowed)))
                                                  select rdm.DataMartID).GroupBy(k => k).Select(k => k.Key).ToArrayAsync();

            }

            return result;
        }

    }
}
//disable the missing comment rule warning until auto comments are completed
#pragma warning disable 1591

using Lpp.Dns.DTO;
using Lpp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.ApiClient
{
	 public class DnsClient : HttpClientEx
	 {
	 	 public DnsClient(string host) : base(host) { }
	 	 public DnsClient(string host, string userName, string password) : base(host, userName, password) { }

	 	Workflow _Workflow = null;
        public Workflow Workflow
                            {
                                get {
                                    if (_Workflow == null)
                                        _Workflow = new Workflow(this);

                                    return _Workflow;
                                }
                            }
	 	Wbd _Wbd = null;
        public Wbd Wbd
                            {
                                get {
                                    if (_Wbd == null)
                                        _Wbd = new Wbd(this);

                                    return _Wbd;
                                }
                            }
	 	SsoEndpoints _SsoEndpoints = null;
        public SsoEndpoints SsoEndpoints
                            {
                                get {
                                    if (_SsoEndpoints == null)
                                        _SsoEndpoints = new SsoEndpoints(this);

                                    return _SsoEndpoints;
                                }
                            }
	 	Users _Users = null;
        public Users Users
                            {
                                get {
                                    if (_Users == null)
                                        _Users = new Users(this);

                                    return _Users;
                                }
                            }
	 	Theme _Theme = null;
        public Theme Theme
                            {
                                get {
                                    if (_Theme == null)
                                        _Theme = new Theme(this);

                                    return _Theme;
                                }
                            }
	 	Terms _Terms = null;
        public Terms Terms
                            {
                                get {
                                    if (_Terms == null)
                                        _Terms = new Terms(this);

                                    return _Terms;
                                }
                            }
	 	Security _Security = null;
        public Security Security
                            {
                                get {
                                    if (_Security == null)
                                        _Security = new Security(this);

                                    return _Security;
                                }
                            }
	 	SecurityGroups _SecurityGroups = null;
        public SecurityGroups SecurityGroups
                            {
                                get {
                                    if (_SecurityGroups == null)
                                        _SecurityGroups = new SecurityGroups(this);

                                    return _SecurityGroups;
                                }
                            }
	 	Organizations _Organizations = null;
        public Organizations Organizations
                            {
                                get {
                                    if (_Organizations == null)
                                        _Organizations = new Organizations(this);

                                    return _Organizations;
                                }
                            }
	 	OrganizationRegistries _OrganizationRegistries = null;
        public OrganizationRegistries OrganizationRegistries
                            {
                                get {
                                    if (_OrganizationRegistries == null)
                                        _OrganizationRegistries = new OrganizationRegistries(this);

                                    return _OrganizationRegistries;
                                }
                            }
	 	Registries _Registries = null;
        public Registries Registries
                            {
                                get {
                                    if (_Registries == null)
                                        _Registries = new Registries(this);

                                    return _Registries;
                                }
                            }
	 	RegistryItemDefinition _RegistryItemDefinition = null;
        public RegistryItemDefinition RegistryItemDefinition
                            {
                                get {
                                    if (_RegistryItemDefinition == null)
                                        _RegistryItemDefinition = new RegistryItemDefinition(this);

                                    return _RegistryItemDefinition;
                                }
                            }
	 	NetworkMessages _NetworkMessages = null;
        public NetworkMessages NetworkMessages
                            {
                                get {
                                    if (_NetworkMessages == null)
                                        _NetworkMessages = new NetworkMessages(this);

                                    return _NetworkMessages;
                                }
                            }
	 	LookupListCategory _LookupListCategory = null;
        public LookupListCategory LookupListCategory
                            {
                                get {
                                    if (_LookupListCategory == null)
                                        _LookupListCategory = new LookupListCategory(this);

                                    return _LookupListCategory;
                                }
                            }
	 	LookupList _LookupList = null;
        public LookupList LookupList
                            {
                                get {
                                    if (_LookupList == null)
                                        _LookupList = new LookupList(this);

                                    return _LookupList;
                                }
                            }
	 	LookupListValue _LookupListValue = null;
        public LookupListValue LookupListValue
                            {
                                get {
                                    if (_LookupListValue == null)
                                        _LookupListValue = new LookupListValue(this);

                                    return _LookupListValue;
                                }
                            }
	 	Groups _Groups = null;
        public Groups Groups
                            {
                                get {
                                    if (_Groups == null)
                                        _Groups = new Groups(this);

                                    return _Groups;
                                }
                            }
	 	OrganizationGroups _OrganizationGroups = null;
        public OrganizationGroups OrganizationGroups
                            {
                                get {
                                    if (_OrganizationGroups == null)
                                        _OrganizationGroups = new OrganizationGroups(this);

                                    return _OrganizationGroups;
                                }
                            }
	 	Events _Events = null;
        public Events Events
                            {
                                get {
                                    if (_Events == null)
                                        _Events = new Events(this);

                                    return _Events;
                                }
                            }
	 	DataModels _DataModels = null;
        public DataModels DataModels
                            {
                                get {
                                    if (_DataModels == null)
                                        _DataModels = new DataModels(this);

                                    return _DataModels;
                                }
                            }
	 	DataMarts _DataMarts = null;
        public DataMarts DataMarts
                            {
                                get {
                                    if (_DataMarts == null)
                                        _DataMarts = new DataMarts(this);

                                    return _DataMarts;
                                }
                            }
	 	Tasks _Tasks = null;
        public Tasks Tasks
                            {
                                get {
                                    if (_Tasks == null)
                                        _Tasks = new Tasks(this);

                                    return _Tasks;
                                }
                            }
	 	LegacyRequests _LegacyRequests = null;
        public LegacyRequests LegacyRequests
                            {
                                get {
                                    if (_LegacyRequests == null)
                                        _LegacyRequests = new LegacyRequests(this);

                                    return _LegacyRequests;
                                }
                            }
	 	ReportAggregationLevel _ReportAggregationLevel = null;
        public ReportAggregationLevel ReportAggregationLevel
                            {
                                get {
                                    if (_ReportAggregationLevel == null)
                                        _ReportAggregationLevel = new ReportAggregationLevel(this);

                                    return _ReportAggregationLevel;
                                }
                            }
	 	RequestObservers _RequestObservers = null;
        public RequestObservers RequestObservers
                            {
                                get {
                                    if (_RequestObservers == null)
                                        _RequestObservers = new RequestObservers(this);

                                    return _RequestObservers;
                                }
                            }
	 	RequestUsers _RequestUsers = null;
        public RequestUsers RequestUsers
                            {
                                get {
                                    if (_RequestUsers == null)
                                        _RequestUsers = new RequestUsers(this);

                                    return _RequestUsers;
                                }
                            }
	 	Response _Response = null;
        public Response Response
                            {
                                get {
                                    if (_Response == null)
                                        _Response = new Response(this);

                                    return _Response;
                                }
                            }
	 	Requests _Requests = null;
        public Requests Requests
                            {
                                get {
                                    if (_Requests == null)
                                        _Requests = new Requests(this);

                                    return _Requests;
                                }
                            }
	 	RequestTypes _RequestTypes = null;
        public RequestTypes RequestTypes
                            {
                                get {
                                    if (_RequestTypes == null)
                                        _RequestTypes = new RequestTypes(this);

                                    return _RequestTypes;
                                }
                            }
	 	Templates _Templates = null;
        public Templates Templates
                            {
                                get {
                                    if (_Templates == null)
                                        _Templates = new Templates(this);

                                    return _Templates;
                                }
                            }
	 	Notifications _Notifications = null;
        public Notifications Notifications
                            {
                                get {
                                    if (_Notifications == null)
                                        _Notifications = new Notifications(this);

                                    return _Notifications;
                                }
                            }
	 	DataMartInstalledModels _DataMartInstalledModels = null;
        public DataMartInstalledModels DataMartInstalledModels
                            {
                                get {
                                    if (_DataMartInstalledModels == null)
                                        _DataMartInstalledModels = new DataMartInstalledModels(this);

                                    return _DataMartInstalledModels;
                                }
                            }
	 	ProjectDataMarts _ProjectDataMarts = null;
        public ProjectDataMarts ProjectDataMarts
                            {
                                get {
                                    if (_ProjectDataMarts == null)
                                        _ProjectDataMarts = new ProjectDataMarts(this);

                                    return _ProjectDataMarts;
                                }
                            }
	 	ProjectOrganizations _ProjectOrganizations = null;
        public ProjectOrganizations ProjectOrganizations
                            {
                                get {
                                    if (_ProjectOrganizations == null)
                                        _ProjectOrganizations = new ProjectOrganizations(this);

                                    return _ProjectOrganizations;
                                }
                            }
	 	Projects _Projects = null;
        public Projects Projects
                            {
                                get {
                                    if (_Projects == null)
                                        _Projects = new Projects(this);

                                    return _Projects;
                                }
                            }
	 	Documents _Documents = null;
        public Documents Documents
                            {
                                get {
                                    if (_Documents == null)
                                        _Documents = new Documents(this);

                                    return _Documents;
                                }
                            }
	 	Comments _Comments = null;
        public Comments Comments
                            {
                                get {
                                    if (_Comments == null)
                                        _Comments = new Comments(this);

                                    return _Comments;
                                }
                            }
	 }
	 public class Workflow : HttpClientDataEndpoint<DnsClient, WorkflowDTO>
	 {
	 	 public Workflow(DnsClient client) : base(client, "/Workflow") {}
	 	 public async Task<Lpp.Dns.DTO.WorkflowActivityDTO> GetWorkflowEntryPointByRequestTypeID(System.Guid requestTypeID)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkflowActivityDTO>(Path + "/GetWorkflowEntryPointByRequestTypeID?requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.WorkflowActivityDTO> GetWorkflowActivity(System.Guid workflowActivityID)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkflowActivityDTO>(Path + "/GetWorkflowActivity?workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.WorkflowActivityDTO>> GetWorkflowActivitiesByWorkflowID(System.Guid workFlowID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkflowActivityDTO>(Path + "/GetWorkflowActivitiesByWorkflowID?workFlowID=" + System.Net.WebUtility.UrlEncode(workFlowID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.WorkflowRoleDTO>> GetWorkflowRolesByWorkflowID(System.Guid workflowID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkflowRoleDTO>(Path + "/GetWorkflowRolesByWorkflowID?workflowID=" + System.Net.WebUtility.UrlEncode(workflowID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class Wbd
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Wbd(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Wbd";
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ApproveRequest(System.Guid requestID)
	 	 {
	 	 	 var result = await Client.Put<System.Guid>(Path + "/ApproveRequest", requestID);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> RejectRequest(System.Guid requestID)
	 	 {
	 	 	 var result = await Client.Put<System.Guid>(Path + "/RejectRequest", requestID);
	 	 	 return result;
	 	 }
	 	 public async Task<Lpp.Dns.DTO.RequestDTO> GetRequestByID(System.Guid Id)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestDTO>(Path + "/GetRequestByID?Id=" + System.Net.WebUtility.UrlEncode(Id.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> SaveRequest(Lpp.Dns.DTO.RequestDTO request)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RequestDTO>(Path + "/SaveRequest", request);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ActivityDTO>> GetActivityTreeByProjectID(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ActivityDTO>(Path + "/GetActivityTreeByProjectID?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.DataMartRegistrationResultDTO> Register(Lpp.Dns.DTO.RegisterDataMartDTO registration)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RegisterDataMartDTO, Lpp.Dns.DTO.DataMartRegistrationResultDTO>(Path + "/Register", registration);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.WbdChangeSetDTO> GetChanges(Lpp.Dns.DTO.GetChangeRequestDTO criteria)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.GetChangeRequestDTO, Lpp.Dns.DTO.WbdChangeSetDTO>(Path + "/GetChanges", criteria);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> DownloadDocument(System.Guid documentId)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/DownloadDocument?documentId=" + System.Net.WebUtility.UrlEncode(documentId.ToString()) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> DownloadRequestViewableFile(System.Guid requestId)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/DownloadRequestViewableFile?requestId=" + System.Net.WebUtility.UrlEncode(requestId.ToString()) + "&");
	 	 }
	 	 public async Task<System.Guid> CopyRequest(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Guid>(Path + "/CopyRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task UpdateResponseStatus(Lpp.Dns.DTO.UpdateResponseStatusRequestDTO details)
	 	 {
	 	 	 var result = await Client.Post(Path + "/UpdateResponseStatus", details);
	 	 	 return;
	 	 }
	 }
	 public class SsoEndpoints : HttpClientDataEndpoint<DnsClient, SsoEndpointDTO>
	 {
	 	 public SsoEndpoints(DnsClient client) : base(client, "/SsoEndpoints") {}
	 }
	 public class Users : HttpClientDataEndpoint<DnsClient, UserDTO>
	 {
	 	 public Users(DnsClient client) : base(client, "/Users") {}
	 	 public async Task<Lpp.Dns.DTO.UserDTO> ValidateLogin(Lpp.Dns.DTO.LoginDTO login)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.LoginDTO, Lpp.Dns.DTO.UserDTO>(Path + "/ValidateLogin", login);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.UserDTO> ByUserName(string userName)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.UserDTO>(Path + "/ByUserName?userName=" + (userName == null ? "" : System.Net.WebUtility.UrlEncode(userName.ToString())) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UserRegistration(Lpp.Dns.DTO.UserRegistrationDTO data)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UserRegistrationDTO>(Path + "/UserRegistration", data);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDTO>> ListAvailableProjects(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDTO>(Path + "/ListAvailableProjects", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ForgotPassword(Lpp.Dns.DTO.ForgotPasswordDTO data)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.ForgotPasswordDTO>(Path + "/ForgotPassword", data);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ChangePassword(Lpp.Dns.DTO.UpdateUserPasswordDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateUserPasswordDTO>(Path + "/ChangePassword", updateInfo);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> RestorePassword(Lpp.Dns.DTO.RestorePasswordDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Put<Lpp.Dns.DTO.RestorePasswordDTO>(Path + "/RestorePassword", updateInfo);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AssignedUserNotificationDTO>> GetAssignedNotifications(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AssignedUserNotificationDTO>(Path + "/GetAssignedNotifications?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.EventDTO>> GetSubscribableEvents(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.EventDTO>(Path + "/GetSubscribableEvents?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.UserEventSubscriptionDTO>> GetSubscribedEvents(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.UserEventSubscriptionDTO>(Path + "/GetSubscribedEvents?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.NotificationDTO>> GetNotifications(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.NotificationDTO>(Path + "/GetNotifications?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.UserAuthenticationDTO>> ListSuccessfulAudits(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.UserAuthenticationDTO>(Path + "/ListSuccessfulAudits", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateSubscribedEvents(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UserEventSubscriptionDTO> subscribedEvents)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UserEventSubscriptionDTO>>(Path + "/UpdateSubscribedEvents", subscribedEvents);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.TaskDTO>> GetTasks(System.Guid? userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.TaskDTO>(Path + "/GetTasks?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.HomepageTaskSummaryDTO>> GetWorkflowTasks(System.Guid? userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.HomepageTaskSummaryDTO>(Path + "/GetWorkflowTasks?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.HomepageTaskRequestUserDTO>> GetWorkflowTaskUsers(System.Guid? userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.HomepageTaskRequestUserDTO>(Path + "/GetWorkflowTaskUsers?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task Logout()
	 	 {
	 	 	 var result = await Client.Post(Path + "/Logout");
	 	 	 return;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.SecurityGroupDTO>> MemberOfSecurityGroups(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.SecurityGroupDTO>(Path + "/MemberOfSecurityGroups?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateSecurityGroups(Lpp.Dns.DTO.UpdateUserSecurityGroupsDTO groups)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateUserSecurityGroupsDTO>(Path + "/UpdateSecurityGroups", groups);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Boolean> GetGlobalPermission(System.Guid permissionID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/GetGlobalPermission?permissionID=" + System.Net.WebUtility.UrlEncode(permissionID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.MenuItemDTO>> ReturnMainMenu(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.MenuItemDTO>(Path + "/ReturnMainMenu", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateLookupListsTest(string username, string password)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/UpdateLookupListsTest?username=" + (username == null ? "" : System.Net.WebUtility.UrlEncode(username.ToString())) + "&password=" + (password == null ? "" : System.Net.WebUtility.UrlEncode(password.ToString())) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateLookupLists(string username, string password)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/UpdateLookupLists?username=" + (username == null ? "" : System.Net.WebUtility.UrlEncode(username.ToString())) + "&password=" + (password == null ? "" : System.Net.WebUtility.UrlEncode(password.ToString())) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> SaveSetting(Lpp.Dns.DTO.UserSettingDTO setting)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UserSettingDTO>(Path + "/SaveSetting", setting);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UserSettingDTO>> GetSetting(System.Collections.Generic.IEnumerable<System.String> key, string oDataQuery = null)
	 	 {
	 	 	 var keyQueryString = string.Join("&", key.Select(i => string.Format("{0}={1}", "key", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.UserSettingDTO>(Path + "/GetSetting?" + keyQueryString + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Boolean> AllowApproveRejectRequest(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/AllowApproveRejectRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Boolean> HasPassword(System.Guid userID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/HasPassword?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.MetadataEditPermissionsSummaryDTO> GetMetadataEditPermissionsSummary()
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.MetadataEditPermissionsSummaryDTO>(Path + "/GetMetadataEditPermissionsSummary");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 }
	 public class Theme
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Theme(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Theme";
	 	 }
	 	 public async Task<Lpp.Dns.DTO.ThemeDTO> GetText(System.Collections.Generic.IEnumerable<System.String> keys)
	 	 {
	 	 	 var keysQueryString = string.Join("&", keys.Select(i => string.Format("{0}={1}", "keys", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ThemeDTO>(Path + "/GetText?" + keysQueryString + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.ThemeDTO> GetImagePath()
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ThemeDTO>(Path + "/GetImagePath");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 }
	 public class Terms : HttpClientDataEndpoint<DnsClient, TermDTO>
	 {
	 	 public Terms(DnsClient client) : base(client, "/Terms") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.QueryComposer.TemplateTermDTO>> ListTemplateTerms(System.Guid id, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.QueryComposer.TemplateTermDTO>(Path + "/ListTemplateTerms?id=" + System.Net.WebUtility.UrlEncode(id.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ParseCodeList()
	 	 {
	 	 	 var result = await Client.Post(Path + "/ParseCodeList");
	 	 	 return result;
	 	 }
	 }
	 public class Security
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Security(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Security";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.SecurityEntityDTO>> ListSecurityEntities(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.SecurityEntityDTO>(Path + "/ListSecurityEntities", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.PermissionDTO>> GetPermissionsByLocation(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Enums.PermissionAclTypes> locations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Enums.PermissionAclTypes>, Lpp.Dns.DTO.PermissionDTO>(Path + "/GetPermissionsByLocation", locations);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclDataMartDTO>> GetDataMartPermissions(System.Guid dataMartID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclDataMartDTO>(Path + "/GetDataMartPermissions?dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclOrganizationDTO>> GetOrganizationPermissions(System.Guid organizationID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclOrganizationDTO>(Path + "/GetOrganizationPermissions?organizationID=" + System.Net.WebUtility.UrlEncode(organizationID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclUserDTO>> GetUserPermissions(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclUserDTO>(Path + "/GetUserPermissions?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclGroupDTO>> GetGroupPermissions(System.Guid groupID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclGroupDTO>(Path + "/GetGroupPermissions?groupID=" + System.Net.WebUtility.UrlEncode(groupID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclRegistryDTO>> GetRegistryPermissions(System.Guid registryID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclRegistryDTO>(Path + "/GetRegistryPermissions?registryID=" + System.Net.WebUtility.UrlEncode(registryID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectDTO>> GetProjectPermissions(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectDTO>(Path + "/GetProjectPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclDTO>> GetGlobalPermissions(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclDTO>(Path + "/GetGlobalPermissions", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectOrganizationDTO>> GetProjectOrganizationPermissions(System.Guid projectID, System.Guid? organizationID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectOrganizationDTO>(Path + "/GetProjectOrganizationPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&organizationID=" + System.Net.WebUtility.UrlEncode(organizationID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.Security.AclProjectRequestTypeWorkflowActivityDTO>> GetProjectRequestTypeWorkflowActivityPermissions(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.Security.AclProjectRequestTypeWorkflowActivityDTO>(Path + "/GetProjectRequestTypeWorkflowActivityPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Guid[]> GetWorkflowActivityPermissionsForIdentity(System.Guid projectID, System.Guid workflowActivityID, System.Guid requestTypeID, System.Collections.Generic.IEnumerable<System.Guid> permissionID)
	 	 {
	 	 	 var permissionIDQueryString = string.Join("&", permissionID.Select(i => string.Format("{0}={1}", "permissionID", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<System.Guid[]>(Path + "/GetWorkflowActivityPermissionsForIdentity?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&" + permissionIDQueryString + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectDataMartDTO>> GetProjectDataMartPermissions(System.Guid projectID, System.Guid? dataMartID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectDataMartDTO>(Path + "/GetProjectDataMartPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectDataMartRequestTypeDTO>> GetProjectDataMartRequestTypePermissions(System.Guid projectID, System.Guid? dataMartID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectDataMartRequestTypeDTO>(Path + "/GetProjectDataMartRequestTypePermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectRequestTypeDTO>> GetProjectRequestTypePermissions(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectRequestTypeDTO>(Path + "/GetProjectRequestTypePermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclDataMartRequestTypeDTO>> GetDataMartRequestTypePermissions(System.Guid dataMartID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclDataMartRequestTypeDTO>(Path + "/GetDataMartRequestTypePermissions?dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclTemplateDTO>> GetTemplatePermissions(System.Guid templateID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclTemplateDTO>(Path + "/GetTemplatePermissions?templateID=" + System.Net.WebUtility.UrlEncode(templateID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclRequestTypeDTO>> GetRequestTypePermissions(System.Guid requestTypeID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclRequestTypeDTO>(Path + "/GetRequestTypePermissions?requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectRequestTypeWorkflowActivityPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Security.AclProjectRequestTypeWorkflowActivityDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Security.AclProjectRequestTypeWorkflowActivityDTO>>(Path + "/UpdateProjectRequestTypeWorkflowActivityPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateDataMartRequestTypePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDataMartRequestTypeDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDataMartRequestTypeDTO>>(Path + "/UpdateDataMartRequestTypePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectDataMartPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDataMartDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDataMartDTO>>(Path + "/UpdateProjectDataMartPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectDataMartRequestTypePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDataMartRequestTypeDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDataMartRequestTypeDTO>>(Path + "/UpdateProjectDataMartRequestTypePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectOrganizationPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectOrganizationDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectOrganizationDTO>>(Path + "/UpdateProjectOrganizationPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdatePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDTO>>(Path + "/UpdatePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateGroupPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclGroupDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclGroupDTO>>(Path + "/UpdateGroupPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRegistryPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclRegistryDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclRegistryDTO>>(Path + "/UpdateRegistryPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectDTO>>(Path + "/UpdateProjectPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectRequestTypePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectRequestTypeDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectRequestTypeDTO>>(Path + "/UpdateProjectRequestTypePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateDataMartPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDataMartDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclDataMartDTO>>(Path + "/UpdateDataMartPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateOrganizationPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclOrganizationDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclOrganizationDTO>>(Path + "/UpdateOrganizationPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateUserPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclUserDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclUserDTO>>(Path + "/UpdateUserPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.TreeItemDTO>> GetAvailableSecurityGroupTree(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.TreeItemDTO>(Path + "/GetAvailableSecurityGroupTree", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateTemplatePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclTemplateDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclTemplateDTO>>(Path + "/UpdateTemplatePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRequestTypePermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclRequestTypeDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclRequestTypeDTO>>(Path + "/UpdateRequestTypePermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclGlobalFieldOptionDTO>> GetGlobalFieldOptionPermissions(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclGlobalFieldOptionDTO>(Path + "/GetGlobalFieldOptionPermissions", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateFieldOptionPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclGlobalFieldOptionDTO> fieldOptionUpdates)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclGlobalFieldOptionDTO>>(Path + "/UpdateFieldOptionPermissions", fieldOptionUpdates);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.AclProjectFieldOptionDTO>> GetProjectFieldOptionPermissions(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.AclProjectFieldOptionDTO>(Path + "/GetProjectFieldOptionPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectFieldOptionPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectFieldOptionDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.AclProjectFieldOptionDTO>>(Path + "/UpdateProjectFieldOptionPermissions", permissions);
	 	 	 return result;
	 	 }
	 }
	 public class SecurityGroups : HttpClientDataEndpoint<DnsClient, SecurityGroupDTO>
	 {
	 	 public SecurityGroups(DnsClient client) : base(client, "/SecurityGroups") {}
	 }
	 public class Organizations : HttpClientDataEndpoint<DnsClient, OrganizationDTO>
	 {
	 	 public Organizations(DnsClient client) : base(client, "/Organizations") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationDTO>> ListByGroupMembership(System.Guid groupID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationDTO>(Path + "/ListByGroupMembership?groupID=" + System.Net.WebUtility.UrlEncode(groupID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Guid> Copy(System.Guid organizationID)
	 	 {

	 	 	 var result = await Client.Get<System.Guid>(Path + "/Copy?organizationID=" + System.Net.WebUtility.UrlEncode(organizationID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> EHRSInsertOrUpdate(Lpp.Dns.DTO.OrganizationUpdateEHRsesDTO updates)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.OrganizationUpdateEHRsesDTO>(Path + "/EHRSInsertOrUpdate", updates);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationEHRSDTO>> ListEHRS(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationEHRSDTO>(Path + "/ListEHRS", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task DeleteEHRS(System.Collections.Generic.IEnumerable<System.Guid> id)
	 	 {
	 	 	 await Client.Delete(Path + "/DeleteEHRS", id);
	 	 }
	 }
	 public class OrganizationRegistries
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public OrganizationRegistries(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/OrganizationRegistries";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationRegistryDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationRegistryDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> InsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationRegistryDTO> organizations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationRegistryDTO>>(Path + "/InsertOrUpdate", organizations);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Remove(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationRegistryDTO> organizations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationRegistryDTO>>(Path + "/Remove", organizations);
	 	 	 return result;
	 	 }
	 }
	 public class Registries : HttpClientDataEndpoint<DnsClient, RegistryDTO>
	 {
	 	 public Registries(DnsClient client) : base(client, "/Registries") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RegistryItemDefinitionDTO>> GetRegistryItemDefinitionList(System.Guid registryID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RegistryItemDefinitionDTO>(Path + "/GetRegistryItemDefinitionList?registryID=" + System.Net.WebUtility.UrlEncode(registryID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRegistryItemDefinitions(Lpp.Dns.DTO.UpdateRegistryItemsDTO updateParams)
	 	 {
	 	 	 var result = await Client.Put<Lpp.Dns.DTO.UpdateRegistryItemsDTO>(Path + "/UpdateRegistryItemDefinitions", updateParams);
	 	 	 return result;
	 	 }
	 }
	 public class RegistryItemDefinition
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public RegistryItemDefinition(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/RegistryItemDefinition";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RegistryItemDefinitionDTO>> GetList(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RegistryItemDefinitionDTO>(Path + "/GetList", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class NetworkMessages : HttpClientDataEndpoint<DnsClient, NetworkMessageDTO>
	 {
	 	 public NetworkMessages(DnsClient client) : base(client, "/NetworkMessages") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.NetworkMessageDTO>> ListLastDays(System.Int32 days, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.NetworkMessageDTO>(Path + "/ListLastDays?days=" + System.Net.WebUtility.UrlEncode(days.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class LookupListCategory
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public LookupListCategory(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/LookupListCategory";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.LookupListCategoryDTO>> GetList(Lpp.Dns.DTO.Enums.Lists listID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.LookupListCategoryDTO>(Path + "/GetList?listID=" + System.Net.WebUtility.UrlEncode(listID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class LookupList
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public LookupList(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/LookupList";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.LookupListDTO>> GetList(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.LookupListDTO>(Path + "/GetList", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class LookupListValue
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public LookupListValue(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/LookupListValue";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.LookupListValueDTO>> GetList(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.LookupListValueDTO>(Path + "/GetList", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.LookupListValueDTO>> GetCodeDetailsByCode(Lpp.Dns.DTO.LookupListDetailRequestDTO details)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.LookupListDetailRequestDTO, Lpp.Dns.DTO.LookupListValueDTO>(Path + "/GetCodeDetailsByCode", details);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.LookupListValueDTO>> LookupList(Lpp.Dns.DTO.Enums.Lists listID, string lookup, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.LookupListValueDTO>(Path + "/LookupList?listID=" + System.Net.WebUtility.UrlEncode(listID.ToString()) + "&lookup=" + (lookup == null ? "" : System.Net.WebUtility.UrlEncode(lookup.ToString())) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class Groups : HttpClientDataEndpoint<DnsClient, GroupDTO>
	 {
	 	 public Groups(DnsClient client) : base(client, "/Groups") {}
	 }
	 public class OrganizationGroups
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public OrganizationGroups(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/OrganizationGroups";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationGroupDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationGroupDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> InsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationGroupDTO> organizations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationGroupDTO>>(Path + "/InsertOrUpdate", organizations);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Remove(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationGroupDTO> organizations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationGroupDTO>>(Path + "/Remove", organizations);
	 	 	 return result;
	 	 }
	 }
	 public class Events : HttpClientDataEndpoint<DnsClient, EventDTO>
	 {
	 	 public Events(DnsClient client) : base(client, "/Events") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.EventDTO>> GetEventsByLocation(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Enums.PermissionAclTypes> locations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Enums.PermissionAclTypes>, Lpp.Dns.DTO.EventDTO>(Path + "/GetEventsByLocation", locations);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.GroupEventDTO>> GetGroupEventPermissions(System.Guid groupID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.GroupEventDTO>(Path + "/GetGroupEventPermissions?groupID=" + System.Net.WebUtility.UrlEncode(groupID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RegistryEventDTO>> GetRegistryEventPermissions(System.Guid registryID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RegistryEventDTO>(Path + "/GetRegistryEventPermissions?registryID=" + System.Net.WebUtility.UrlEncode(registryID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectEventDTO>> GetProjectEventPermissions(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectEventDTO>(Path + "/GetProjectEventPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationEventDTO>> GetOrganizationEventPermissions(System.Guid organizationID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationEventDTO>(Path + "/GetOrganizationEventPermissions?organizationID=" + System.Net.WebUtility.UrlEncode(organizationID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.UserEventDTO>> GetUserEventPermissions(System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.UserEventDTO>(Path + "/GetUserEventPermissions?userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.BaseEventPermissionDTO>> GetGlobalEventPermissions(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.BaseEventPermissionDTO>(Path + "/GetGlobalEventPermissions", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateGroupEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.GroupEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.GroupEventDTO>>(Path + "/UpdateGroupEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRegistryEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RegistryEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RegistryEventDTO>>(Path + "/UpdateRegistryEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectEventDTO>>(Path + "/UpdateProjectEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateOrganizationEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.OrganizationEventDTO>>(Path + "/UpdateOrganizationEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateUserEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UserEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UserEventDTO>>(Path + "/UpdateUserEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateGlobalEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.BaseEventPermissionDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.BaseEventPermissionDTO>>(Path + "/UpdateGlobalEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectOrganizationEventDTO>> GetProjectOrganizationEventPermissions(System.Guid projectID, System.Guid? organizationID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectOrganizationEventDTO>(Path + "/GetProjectOrganizationEventPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&organizationID=" + System.Net.WebUtility.UrlEncode(organizationID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectOrganizationEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectOrganizationEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectOrganizationEventDTO>>(Path + "/UpdateProjectOrganizationEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDataMartEventDTO>> GetProjectDataMartEventPermissions(System.Guid projectID, System.Guid? dataMartID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDataMartEventDTO>(Path + "/GetProjectDataMartEventPermissions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectDataMartEventPermissions(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectDataMartEventDTO> permissions)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectDataMartEventDTO>>(Path + "/UpdateProjectDataMartEventPermissions", permissions);
	 	 	 return result;
	 	 }
	 }
	 public class DataModels : HttpClientDataEndpoint<DnsClient, DataModelDTO>
	 {
	 	 public DataModels(DnsClient client) : base(client, "/DataModels") {}
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.DataModelProcessorDTO>> ListDataModelProcessors(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataModelProcessorDTO>(Path + "/ListDataModelProcessors", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class DataMarts : HttpClientDataEndpoint<DnsClient, DataMartDTO>
	 {
	 	 public DataMarts(DnsClient client) : base(client, "/DataMarts") {}
	 	 public async Task<Lpp.Dns.DTO.DataMartDTO> GetByRoute(System.Guid requestDataMartID)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataMartDTO>(Path + "/GetByRoute?requestDataMartID=" + System.Net.WebUtility.UrlEncode(requestDataMartID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartListDTO>> ListBasic(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataMartListDTO>(Path + "/ListBasic", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartTypeDTO>> DataMartTypeList(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataMartTypeDTO>(Path + "/DataMartTypeList", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeDTO>> GetRequestTypesByDataMarts(System.Collections.Generic.IEnumerable<System.Guid> DataMartId)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<System.Guid>, Lpp.Dns.DTO.RequestTypeDTO>(Path + "/GetRequestTypesByDataMarts", DataMartId);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartInstalledModelDTO>> GetInstalledModelsByDataMart(System.Guid DataMartId, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataMartInstalledModelDTO>(Path + "/GetInstalledModelsByDataMart?DataMartId=" + System.Net.WebUtility.UrlEncode(DataMartId.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UninstallModel(Lpp.Dns.DTO.DataMartInstalledModelDTO model)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.DataMartInstalledModelDTO>(Path + "/UninstallModel", model);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Guid> Copy(System.Guid datamartID)
	 	 {

	 	 	 var result = await Client.Get<System.Guid>(Path + "/Copy?datamartID=" + System.Net.WebUtility.UrlEncode(datamartID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 }
	 public class Tasks : HttpClientDataEndpoint<DnsClient, TaskDTO>
	 {
	 	 public Tasks(DnsClient client) : base(client, "/Tasks") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.TaskDTO>> ByRequestID(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.TaskDTO>(Path + "/ByRequestID?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> GetWorkflowActivityDataForRequest(System.Guid requestID, System.Guid workflowActivityID)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/GetWorkflowActivityDataForRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&");
	 	 }
	 }
	 public class LegacyRequests
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public LegacyRequests(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/LegacyRequests";
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ScheduleLegacyRequest(Lpp.Dns.DTO.Schedule.LegacySchedulerRequestDTO dto)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.Schedule.LegacySchedulerRequestDTO>(Path + "/ScheduleLegacyRequest", dto);
	 	 	 return result;
	 	 }
	 	 public async Task DeleteRequestSchedules(System.Guid requestID)
	 	 {
	 	 	 var result = await Client.Post(Path + "/DeleteRequestSchedules", requestID);
	 	 	 return;
	 	 }
	 }
	 public class ReportAggregationLevel : HttpClientDataEndpoint<DnsClient, ReportAggregationLevelDTO>
	 {
	 	 public ReportAggregationLevel(DnsClient client) : base(client, "/ReportAggregationLevel") {}
	 }
	 public class RequestObservers : HttpClientDataEndpoint<DnsClient, RequestObserverDTO>
	 {
	 	 public RequestObservers(DnsClient client) : base(client, "/RequestObservers") {}
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestObserverDTO>> ListRequestObservers(System.Guid? RequestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestObserverDTO>(Path + "/ListRequestObservers?RequestID=" + System.Net.WebUtility.UrlEncode(RequestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ObserverEventDTO>> LookupObserverEvents(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ObserverEventDTO>(Path + "/LookupObserverEvents", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ObserverDTO>> LookupObservers(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ObserverDTO>(Path + "/LookupObservers", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task ValidateInsertOrUpdate(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestObserverDTO> values)
	 	 {
	 	 	 var result = await Client.Post(Path + "/ValidateInsertOrUpdate", values);
	 	 	 return;
	 	 }
	 }
	 public class RequestUsers
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public RequestUsers(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/RequestUsers";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestUserDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestUserDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestUserDTO>> Insert(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestUserDTO> values)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestUserDTO>, Lpp.Dns.DTO.RequestUserDTO>(Path + "/Insert", values);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task Delete(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestUserDTO> requestUsers)
	 	 {
	 	 	 await Client.Delete(Path + "/Delete", requestUsers);
	 	 }
	 }
	 public class Response : HttpClientDataEndpoint<DnsClient, ResponseDTO>
	 {
	 	 public Response(DnsClient client) : base(client, "/Response") {}
	 	 public async Task<System.Net.Http.HttpResponseMessage> ApproveResponses(Lpp.Dns.DTO.ApproveResponseDTO responses)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.ApproveResponseDTO>(Path + "/ApproveResponses", responses);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> RejectResponses(Lpp.Dns.DTO.RejectResponseDTO responses)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RejectResponseDTO>(Path + "/RejectResponses", responses);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> RejectAndReSubmitResponses(Lpp.Dns.DTO.RejectResponseDTO responses)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RejectResponseDTO>(Path + "/RejectAndReSubmitResponses", responses);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ResponseDTO>> GetByWorkflowActivity(System.Guid requestID, System.Guid workflowActivityID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ResponseDTO>(Path + "/GetByWorkflowActivity?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Boolean> CanViewIndividualResponses(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/CanViewIndividualResponses?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Boolean> CanViewAggregateResponses(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/CanViewAggregateResponses?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Boolean> CanViewPendingApprovalResponses(Lpp.Dns.DTO.ApproveResponseDTO responses)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.ApproveResponseDTO, System.Boolean>(Path + "/CanViewPendingApprovalResponses", responses);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.CommonResponseDetailDTO> GetForWorkflowRequest(System.Guid requestID, System.Boolean? viewDocuments)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.CommonResponseDetailDTO>(Path + "/GetForWorkflowRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&viewDocuments=" + System.Net.WebUtility.UrlEncode(viewDocuments.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.CommonResponseDetailDTO> GetDetails(System.Collections.Generic.IEnumerable<System.Guid> id)
	 	 {
	 	 	 var idQueryString = string.Join("&", id.Select(i => string.Format("{0}={1}", "id", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.CommonResponseDetailDTO>(Path + "/GetDetails?" + idQueryString + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO>> GetWorkflowResponseContent(System.Collections.Generic.IEnumerable<System.Guid> id, Lpp.Dns.DTO.Enums.TaskItemTypes view, string oDataQuery = null)
	 	 {
	 	 	 var idQueryString = string.Join("&", id.Select(i => string.Format("{0}={1}", "id", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO>(Path + "/GetWorkflowResponseContent?" + idQueryString + "&view=" + System.Net.WebUtility.UrlEncode(view.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ResponseGroupDTO>> GetResponseGroups(System.Collections.Generic.IEnumerable<System.Guid> responseIDs, string oDataQuery = null)
	 	 {
	 	 	 var responseIDsQueryString = string.Join("&", responseIDs.Select(i => string.Format("{0}={1}", "responseIDs", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ResponseGroupDTO>(Path + "/GetResponseGroups?" + responseIDsQueryString + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ResponseGroupDTO>> GetResponseGroupsByRequestID(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ResponseGroupDTO>(Path + "/GetResponseGroupsByRequestID?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Export(System.Collections.Generic.IEnumerable<System.Guid> id, Lpp.Dns.DTO.Enums.TaskItemTypes view, string format)
	 	 {
	 	 	 var idQueryString = string.Join("&", id.Select(i => string.Format("{0}={1}", "id", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/Export?" + idQueryString + "&view=" + System.Net.WebUtility.UrlEncode(view.ToString()) + "&format=" + (format == null ? "" : System.Net.WebUtility.UrlEncode(format.ToString())) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ExportAllAsZip(System.Collections.Generic.IEnumerable<System.Guid> id)
	 	 {
	 	 	 var idQueryString = string.Join("&", id.Select(i => string.Format("{0}={1}", "id", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/ExportAllAsZip?" + idQueryString + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ExportResponse(System.Guid requestID, string format)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/ExportResponse?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&format=" + (format == null ? "" : System.Net.WebUtility.UrlEncode(format.ToString())) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> GetTrackingTableForAnalysisCenter(System.Guid requestID)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/GetTrackingTableForAnalysisCenter?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> GetTrackingTableForDataPartners(System.Guid requestID)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/GetTrackingTableForDataPartners?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> GetEnhancedEventLog(System.Guid requestID, string format, System.Boolean download)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/GetEnhancedEventLog?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&format=" + (format == null ? "" : System.Net.WebUtility.UrlEncode(format.ToString())) + "&download=" + System.Net.WebUtility.UrlEncode(download.ToString()) + "&");
	 	 }
	 }
	 public class Requests
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Requests(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Requests";
	 	 }
	 	 public async Task<Lpp.Dns.DTO.RequestCompletionResponseDTO> CompleteActivity(Lpp.Dns.DTO.RequestCompletionRequestDTO request)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RequestCompletionRequestDTO, Lpp.Dns.DTO.RequestCompletionResponseDTO>(Path + "/CompleteActivity", request);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> TerminateRequest(System.Guid requestID)
	 	 {
	 	 	 var result = await Client.Put<System.Guid>(Path + "/TerminateRequest", requestID);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.HomepageRequestDetailDTO>> ListForHomepage(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.HomepageRequestDetailDTO>(Path + "/ListForHomepage", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.HomepageRouteDetailDTO>> RequestsByRoute(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.HomepageRouteDetailDTO>(Path + "/RequestsByRoute", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartListDTO>> GetCompatibleDataMarts(Lpp.Dns.DTO.QueryComposer.MatchingCriteriaDTO requestDetails)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.QueryComposer.MatchingCriteriaDTO, Lpp.Dns.DTO.DataMartListDTO>(Path + "/GetCompatibleDataMarts", requestDetails);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataMartListDTO>> GetDataMartsForInstalledModels(Lpp.Dns.DTO.QueryComposer.MatchingCriteriaDTO requestDetails)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.QueryComposer.MatchingCriteriaDTO, Lpp.Dns.DTO.DataMartListDTO>(Path + "/GetDataMartsForInstalledModels", requestDetails);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestDataMartDTO>> RequestDataMarts(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestDataMartDTO>(Path + "/RequestDataMarts?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestDataMartDTO>> GetOverrideableRequestDataMarts(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestDataMartDTO>(Path + "/GetOverrideableRequestDataMarts?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRequestDataMarts(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UpdateRequestDataMartStatusDTO> dataMarts)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.UpdateRequestDataMartStatusDTO>>(Path + "/UpdateRequestDataMarts", dataMarts);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRequestDataMartsMetadata(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestDataMartDTO> dataMarts)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestDataMartDTO>>(Path + "/UpdateRequestDataMartsMetadata", dataMarts);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequesterCenterDTO>> ListRequesterCenters(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequesterCenterDTO>(Path + "/ListRequesterCenters", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.WorkplanTypeDTO>> ListWorkPlanTypes(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkplanTypeDTO>(Path + "/ListWorkPlanTypes", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ReportAggregationLevelDTO>> ListReportAggregationLevels(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ReportAggregationLevelDTO>(Path + "/ListReportAggregationLevels", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.WorkflowHistoryItemDTO>> GetWorkflowHistory(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WorkflowHistoryItemDTO>(Path + "/GetWorkflowHistory?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.ResponseHistoryDTO> GetResponseHistory(System.Guid requestDataMartID, System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ResponseHistoryDTO>(Path + "/GetResponseHistory?requestDataMartID=" + System.Net.WebUtility.UrlEncode(requestDataMartID.ToString()) + "&requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestSearchTermDTO>> GetRequestSearchTerms(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestSearchTermDTO>(Path + "/GetRequestSearchTerms?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Guid[]> GetRequestTypeModels(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Guid[]>(Path + "/GetRequestTypeModels?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRequestMetadata(Lpp.Dns.DTO.RequestMetadataDTO reqMetadata)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.RequestMetadataDTO>(Path + "/UpdateRequestMetadata", reqMetadata);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateMetadataForRequests(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestMetadataDTO> updates)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestMetadataDTO>>(Path + "/UpdateMetadataForRequests", updates);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.OrganizationDTO>> GetOrganizationsForRequest(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.OrganizationDTO>(Path + "/GetOrganizationsForRequest?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Boolean> AllowCopyRequest(System.Guid requestID)
	 	 {

	 	 	 var result = await Client.Get<System.Boolean>(Path + "/AllowCopyRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Guid> CopyRequest(System.Guid requestID)
	 	 {
	 	 	 var result = await Client.Post<System.Guid, System.Guid>(Path + "/CopyRequest", requestID);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 }
	 public class RequestTypes : HttpClientDataEndpoint<DnsClient, RequestTypeDTO>
	 {
	 	 public RequestTypes(DnsClient client) : base(client, "/RequestTypes") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeDTO>> ListAvailableRequestTypes(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeDTO>(Path + "/ListAvailableRequestTypes", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Save(Lpp.Dns.DTO.UpdateRequestTypeRequestDTO details)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateRequestTypeRequestDTO>(Path + "/Save", details);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateModels(Lpp.Dns.DTO.UpdateRequestTypeModelsDTO details)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateRequestTypeModelsDTO>(Path + "/UpdateModels", details);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeModelDTO>> GetRequestTypeModels(System.Guid requestTypeID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeModelDTO>(Path + "/GetRequestTypeModels?requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeTermDTO>> GetRequestTypeTerms(System.Guid requestTypeID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeTermDTO>(Path + "/GetRequestTypeTerms?requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeTermDTO>> GetFilteredTerms(System.Guid id, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeTermDTO>(Path + "/GetFilteredTerms?id=" + System.Net.WebUtility.UrlEncode(id.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.RequestTypeTermDTO>> GetTermsFilteredBy(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeTermDTO>(Path + "/GetTermsFilteredBy", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateRequestTypeTerms(Lpp.Dns.DTO.UpdateRequestTypeTermsDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateRequestTypeTermsDTO>(Path + "/UpdateRequestTypeTerms", updateInfo);
	 	 	 return result;
	 	 }
	 }
	 public class Templates : HttpClientDataEndpoint<DnsClient, TemplateDTO>
	 {
	 	 public Templates(DnsClient client) : base(client, "/Templates") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.TemplateDTO>> CriteriaGroups(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.TemplateDTO>(Path + "/CriteriaGroups", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.TemplateDTO>> GetByRequestType(System.Guid requestTypeID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.TemplateDTO>(Path + "/GetByRequestType?requestTypeID=" + System.Net.WebUtility.UrlEncode(requestTypeID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.HasGlobalSecurityForTemplateDTO>> GetGlobalTemplatePermissions(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.HasGlobalSecurityForTemplateDTO>(Path + "/GetGlobalTemplatePermissions", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.TemplateDTO>> SaveCriteriaGroup(Lpp.Dns.DTO.SaveCriteriaGroupRequestDTO details)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.SaveCriteriaGroupRequestDTO, Lpp.Dns.DTO.TemplateDTO>(Path + "/SaveCriteriaGroup", details);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class Notifications
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Notifications(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Notifications";
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> ExecuteScheduledNotifications(string userName, string password)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/ExecuteScheduledNotifications?userName=" + (userName == null ? "" : System.Net.WebUtility.UrlEncode(userName.ToString())) + "&password=" + (password == null ? "" : System.Net.WebUtility.UrlEncode(password.ToString())) + "&");
	 	 }
	 }
	 public class DataMartInstalledModels
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public DataMartInstalledModels(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/DataMartInstalledModels";
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> InsertOrUpdate(Lpp.Dns.DTO.UpdateDataMartInstalledModelsDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateDataMartInstalledModelsDTO>(Path + "/InsertOrUpdate", updateInfo);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Remove(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.DataMartInstalledModelDTO> models)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.DataMartInstalledModelDTO>>(Path + "/Remove", models);
	 	 	 return result;
	 	 }
	 }
	 public class ProjectDataMarts
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public ProjectDataMarts(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/ProjectDataMarts";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDataMartDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDataMartDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDataMartWithRequestTypesDTO>> ListWithRequestTypes(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDataMartWithRequestTypesDTO>(Path + "/ListWithRequestTypes", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.ProjectDataMartWithRequestTypesDTO> GetWithRequestTypes(System.Guid projectID, System.Guid dataMartID)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDataMartWithRequestTypesDTO>(Path + "/GetWithRequestTypes?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&dataMartID=" + System.Net.WebUtility.UrlEncode(dataMartID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> InsertOrUpdate(Lpp.Dns.DTO.ProjectDataMartUpdateDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.ProjectDataMartUpdateDTO>(Path + "/InsertOrUpdate", updateInfo);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Remove(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectDataMartDTO> dataMarts)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectDataMartDTO>>(Path + "/Remove", dataMarts);
	 	 	 return result;
	 	 }
	 }
	 public class ProjectOrganizations
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public ProjectOrganizations(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/ProjectOrganizations";
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectOrganizationDTO>> List(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectOrganizationDTO>(Path + "/List", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> InsertOrUpdate(Lpp.Dns.DTO.ProjectOrganizationUpdateDTO updateInfo)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.ProjectOrganizationUpdateDTO>(Path + "/InsertOrUpdate", updateInfo);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Remove(System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectOrganizationDTO> organizations)
	 	 {
	 	 	 var result = await Client.Post<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ProjectOrganizationDTO>>(Path + "/Remove", organizations);
	 	 	 return result;
	 	 }
	 }
	 public class Projects : HttpClientDataEndpoint<DnsClient, ProjectDTO>
	 {
	 	 public Projects(DnsClient client) : base(client, "/Projects") {}
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDTO>> ProjectsWithRequests(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDTO>(Path + "/ProjectsWithRequests", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ActivityDTO>> GetActivityTreeByProjectID(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ActivityDTO>(Path + "/GetActivityTreeByProjectID?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectDTO>> RequestableProjects(string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectDTO>(Path + "/RequestableProjects", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeDTO>> GetRequestTypes(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeDTO>(Path + "/GetRequestTypes?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeDTO>> GetRequestTypesByModel(System.Guid projectID, System.Guid dataModelID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeDTO>(Path + "/GetRequestTypesByModel?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&dataModelID=" + System.Net.WebUtility.UrlEncode(dataModelID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.RequestTypeDTO>> GetAvailableRequestTypeForNewRequest(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.RequestTypeDTO>(Path + "/GetAvailableRequestTypeForNewRequest?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.DataModelWithRequestTypesDTO>> GetDataModelsByProject(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.DataModelWithRequestTypesDTO>(Path + "/GetDataModelsByProject?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ProjectRequestTypeDTO>> GetProjectRequestTypes(System.Guid projectID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ProjectRequestTypeDTO>(Path + "/GetProjectRequestTypes?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateProjectRequestTypes(Lpp.Dns.DTO.UpdateProjectRequestTypesDTO requestTypes)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.UpdateProjectRequestTypesDTO>(Path + "/UpdateProjectRequestTypes", requestTypes);
	 	 	 return result;
	 	 }
	 	 public async Task<System.Guid> Copy(System.Guid projectID)
	 	 {

	 	 	 var result = await Client.Get<System.Guid>(Path + "/Copy?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&");
	 	 	 return result.ReturnSingleItem();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UpdateActivities(System.Guid ID, string username, string password)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/UpdateActivities?ID=" + System.Net.WebUtility.UrlEncode(ID.ToString()) + "&username=" + (username == null ? "" : System.Net.WebUtility.UrlEncode(username.ToString())) + "&password=" + (password == null ? "" : System.Net.WebUtility.UrlEncode(password.ToString())) + "&");
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.BaseFieldOptionAclDTO>> GetFieldOptions(System.Guid projectID, System.Guid userID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.BaseFieldOptionAclDTO>(Path + "/GetFieldOptions?projectID=" + System.Net.WebUtility.UrlEncode(projectID.ToString()) + "&userID=" + System.Net.WebUtility.UrlEncode(userID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 }
	 public class Documents
	 {
	 	 readonly DnsClient Client;
	 	 readonly string Path;
	 	 public Documents(DnsClient client)
	 	 {
	 	 	 this.Client = client;
	 	 	 this.Path = "/Documents";
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ExtendedDocumentDTO>> ByTask(System.Collections.Generic.IEnumerable<System.Guid> tasks, System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.Enums.TaskItemTypes> filterByTaskItemType, string oDataQuery = null)
	 	 {
	 	 	 var tasksQueryString = string.Join("&", tasks.Select(i => string.Format("{0}={1}", "tasks", System.Net.WebUtility.UrlEncode(i.ToString()))));
	 	 	 var filterByTaskItemTypeQueryString = string.Join("&", filterByTaskItemType.Select(i => string.Format("{0}={1}", "filterByTaskItemType", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ExtendedDocumentDTO>(Path + "/ByTask?" + tasksQueryString + "&" + filterByTaskItemTypeQueryString + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ExtendedDocumentDTO>> ByRevisionID(System.Collections.Generic.IEnumerable<System.Guid> revisionSets, string oDataQuery = null)
	 	 {
	 	 	 var revisionSetsQueryString = string.Join("&", revisionSets.Select(i => string.Format("{0}={1}", "revisionSets", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ExtendedDocumentDTO>(Path + "/ByRevisionID?" + revisionSetsQueryString + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.ExtendedDocumentDTO>> ByResponse(System.Collections.Generic.IEnumerable<System.Guid> ID, string oDataQuery = null)
	 	 {
	 	 	 var IDQueryString = string.Join("&", ID.Select(i => string.Format("{0}={1}", "ID", System.Net.WebUtility.UrlEncode(i.ToString()))));

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ExtendedDocumentDTO>(Path + "/ByResponse?" + IDQueryString + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Linq.IQueryable<Lpp.Dns.DTO.ExtendedDocumentDTO>> GeneralRequestDocuments(System.Guid requestID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.ExtendedDocumentDTO>(Path + "/GeneralRequestDocuments?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Read(System.Guid id)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/Read?id=" + System.Net.WebUtility.UrlEncode(id.ToString()) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Download(System.Guid id)
	 	 {

	 	 	 return await Client._Client.GetAsync(Client._Host + Path + "/Download?id=" + System.Net.WebUtility.UrlEncode(id.ToString()) + "&");
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> Upload()
	 	 {
	 	 	 var result = await Client.Post(Path + "/Upload");
	 	 	 return result;
	 	 }
	 	 public async Task<System.Net.Http.HttpResponseMessage> UploadChunked()
	 	 {
	 	 	 var result = await Client.Post(Path + "/UploadChunked");
	 	 	 return result;
	 	 }
	 	 public async Task Delete(System.Collections.Generic.IEnumerable<System.Guid> id)
	 	 {
	 	 	 await Client.Delete(Path + "/Delete", id);
	 	 }
	 }
	 public class Comments : HttpClientDataEndpoint<DnsClient, CommentDTO>
	 {
	 	 public Comments(DnsClient client) : base(client, "/Comments") {}
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.WFCommentDTO>> ByRequestID(System.Guid requestID, System.Guid? workflowActivityID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WFCommentDTO>(Path + "/ByRequestID?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.WFCommentDTO>> ByDocumentID(System.Guid documentID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.WFCommentDTO>(Path + "/ByDocumentID?documentID=" + System.Net.WebUtility.UrlEncode(documentID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<System.Collections.Generic.IEnumerable<Lpp.Dns.DTO.CommentDocumentReferenceDTO>> GetDocumentReferencesByRequest(System.Guid requestID, System.Guid? workflowActivityID, string oDataQuery = null)
	 	 {

	 	 	 var result = await Client.Get<Lpp.Dns.DTO.CommentDocumentReferenceDTO>(Path + "/GetDocumentReferencesByRequest?requestID=" + System.Net.WebUtility.UrlEncode(requestID.ToString()) + "&workflowActivityID=" + System.Net.WebUtility.UrlEncode(workflowActivityID.ToString()) + "&", oDataQuery);
	 	 	 return result.ReturnList();
	 	 }
	 	 public async Task<Lpp.Dns.DTO.WFCommentDTO> AddWorkflowComment(Lpp.Dns.DTO.AddWFCommentDTO value)
	 	 {
	 	 	 var result = await Client.Post<Lpp.Dns.DTO.AddWFCommentDTO, Lpp.Dns.DTO.WFCommentDTO>(Path + "/AddWorkflowComment", value);
	 	 	 return result.ReturnSingleItem();
	 	 }
	 }

}

#pragma warning restore 1591

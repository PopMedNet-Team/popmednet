/// <reference path='../../node_modules/@types/jquery/index.d.ts' />
/// <reference path='Lpp.Dns.ViewModels.ts' />
declare var ServiceUrl: string;
declare var User
module Dns.WebApi {
	 export class Workflow{
	 	 public static GetWorkflowEntryPointByRequestTypeID(requestTypeID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowEntryPointByRequestTypeID' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowActivity(workflowActivityID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowActivity' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowActivitiesByWorkflowID(workFlowID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (workFlowID != null) params += '&workFlowID=' + workFlowID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowActivitiesByWorkflowID' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowRolesByWorkflowID(workflowID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowRoleDTO[]>{
	 	 	 var params = '';
	 	 	 if (workflowID != null) params += '&workflowID=' + workflowID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowRoleDTO[]>('Workflow/GetWorkflowRolesByWorkflowID' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Workflow/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowDTO[]>('Workflow/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowDTO[]>('Workflow/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IWorkflowDTO[]>('Workflow/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IWorkflowDTO[]>('Workflow/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IWorkflowDTO[]>('Workflow/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Workflow/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Wbd{
	 	 public static ApproveRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PutAPIValue<any[]>('Wbd/ApproveRequest', requestID, doNotHandleFail);
	 	 }

	 	 public static RejectRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PutAPIValue<any[]>('Wbd/RejectRequest', requestID, doNotHandleFail);
	 	 }

	 	 public static GetRequestByID(Id: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 var params = '';
	 	 	 if (Id != null) params += '&Id=' + Id;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestDTO[]>('Wbd/GetRequestByID' + params, doNotHandleFail);
	 	 }

	 	 public static SaveRequest(request: Dns.Interfaces.IRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Wbd/SaveRequest', request, doNotHandleFail);
	 	 }

	 	 public static GetActivityTreeByProjectID(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IActivityDTO[]>('Wbd/GetActivityTreeByProjectID' + params, doNotHandleFail);
	 	 }

	 	 public static Register(registration: Dns.Interfaces.IRegisterDataMartDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartRegistrationResultDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataMartRegistrationResultDTO[]>('Wbd/Register', registration, doNotHandleFail);
	 	 }

	 	 public static GetChanges(criteria: Dns.Interfaces.IGetChangeRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWbdChangeSetDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IWbdChangeSetDTO[]>('Wbd/GetChanges', criteria, doNotHandleFail);
	 	 }

	 	 public static DownloadDocument(documentId: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (documentId != null) params += '&documentId=' + documentId;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Wbd/DownloadDocument' + params, doNotHandleFail);
	 	 }

	 	 public static DownloadRequestViewableFile(requestId: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestId != null) params += '&requestId=' + requestId;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Wbd/DownloadRequestViewableFile' + params, doNotHandleFail);
	 	 }

	 	 public static CopyRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Wbd/CopyRequest' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateResponseStatus(details: Dns.Interfaces.IUpdateResponseStatusRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Wbd/UpdateResponseStatus', details, doNotHandleFail);
	 	 }

	 }
	 export class SsoEndpoints{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISsoEndpointDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISsoEndpointDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/List' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('SsoEndpoints/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISsoEndpointDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISsoEndpointDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISsoEndpointDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('SsoEndpoints/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Users{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserDTO[]>('Users/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserDTO[]>('Users/List' + params, doNotHandleFail);
	 	 }

	 	 public static ValidateLogin(login: Dns.Interfaces.ILoginDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IUserDTO[]>('Users/ValidateLogin', login, doNotHandleFail);
	 	 }

	 	 public static ByUserName(userName: string, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 var params = '';
	 	 	 if (userName != null) params += '&userName=' + encodeURIComponent(userName);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserDTO[]>('Users/ByUserName' + params, doNotHandleFail);
	 	 }

	 	 public static UserRegistration(data: Dns.Interfaces.IUserRegistrationDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/UserRegistration', data, doNotHandleFail);
	 	 }

	 	 public static ListAvailableProjects($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDTO[]>('Users/ListAvailableProjects' + params, doNotHandleFail);
	 	 }

	 	 public static ForgotPassword(data: Dns.Interfaces.IForgotPasswordDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/ForgotPassword', data, doNotHandleFail);
	 	 }

	 	 public static ChangePassword(updateInfo: Dns.Interfaces.IUpdateUserPasswordDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/ChangePassword', updateInfo, doNotHandleFail);
	 	 }

	 	 public static RestorePassword(updateInfo: Dns.Interfaces.IRestorePasswordDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PutAPIValue<any[]>('Users/RestorePassword', updateInfo, doNotHandleFail);
	 	 }

	 	 public static GetAssignedNotifications(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAssignedUserNotificationDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAssignedUserNotificationDTO[]>('Users/GetAssignedNotifications' + params, doNotHandleFail);
	 	 }

	 	 public static GetSubscribableEvents(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IEventDTO[]>('Users/GetSubscribableEvents' + params, doNotHandleFail);
	 	 }

	 	 public static GetSubscribedEvents(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserEventSubscriptionDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserEventSubscriptionDTO[]>('Users/GetSubscribedEvents' + params, doNotHandleFail);
	 	 }

	 	 public static GetNotifications(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INotificationDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.INotificationDTO[]>('Users/GetNotifications' + params, doNotHandleFail);
	 	 }

	 	 public static ListAuthenticationAudits($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserAuthenticationDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserAuthenticationDTO[]>('Users/ListAuthenticationAudits' + params, doNotHandleFail);
	 	 }

	 	 public static ListDistinctEnvironments(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserAuthenticationDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserAuthenticationDTO[]>('Users/ListDistinctEnvironments' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateSubscribedEvents(subscribedEvents: Dns.Interfaces.IUserEventSubscriptionDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/UpdateSubscribedEvents', subscribedEvents, doNotHandleFail);
	 	 }

	 	 public static GetTasks(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITaskDTO[]>('Users/GetTasks' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowTasks(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IHomepageTaskSummaryDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IHomepageTaskSummaryDTO[]>('Users/GetWorkflowTasks' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowTaskUsers(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IHomepageTaskRequestUserDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IHomepageTaskRequestUserDTO[]>('Users/GetWorkflowTaskUsers' + params, doNotHandleFail);
	 	 }

	 	 public static Logout( doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/Logout', doNotHandleFail);
	 	 }

	 	 public static MemberOfSecurityGroups(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISecurityGroupDTO[]>('Users/MemberOfSecurityGroups' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateSecurityGroups(groups: Dns.Interfaces.IUpdateUserSecurityGroupsDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/UpdateSecurityGroups', groups, doNotHandleFail);
	 	 }

	 	 public static GetGlobalPermission(permissionID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (permissionID != null) params += '&permissionID=' + permissionID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Users/GetGlobalPermission' + params, doNotHandleFail);
	 	 }

	 	 public static ReturnMainMenu($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IMenuItemDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IMenuItemDTO[]>('Users/ReturnMainMenu' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateLookupListsTest(username: string, password: string, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (username != null) params += '&username=' + encodeURIComponent(username);
	 	 	 if (password != null) params += '&password=' + encodeURIComponent(password);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Users/UpdateLookupListsTest' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateLookupLists(username: string, password: string, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (username != null) params += '&username=' + encodeURIComponent(username);
	 	 	 if (password != null) params += '&password=' + encodeURIComponent(password);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Users/UpdateLookupLists' + params, doNotHandleFail);
	 	 }

	 	 public static SaveSetting(setting: Dns.Interfaces.IUserSettingDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Users/SaveSetting', setting, doNotHandleFail);
	 	 }

	 	 public static GetSetting(key: string[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserSettingDTO[]>{
	 	 	 var params = '';
	 	 	 if (key != null)
	 	 	 	 for(var j = 0; j < key.length; j++) { params += '&key=' + encodeURIComponent(key[j]); }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserSettingDTO[]>('Users/GetSetting' + params, doNotHandleFail);
	 	 }

	 	 public static AllowApproveRejectRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Users/AllowApproveRejectRequest' + params, doNotHandleFail);
	 	 }

	 	 public static HasPassword(userID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Users/HasPassword' + params, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Users/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IUserDTO[]>('Users/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IUserDTO[]>('Users/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IUserDTO[]>('Users/Insert', values, doNotHandleFail);
	 	 }

	 	 public static GetMetadataEditPermissionsSummary( doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IMetadataEditPermissionsSummaryDTO[]>{
	 	 	 var params = '';
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IMetadataEditPermissionsSummaryDTO[]>('Users/GetMetadataEditPermissionsSummary' + params, doNotHandleFail);
	 	 }

	 	 public static ExpireAllUserPasswords( doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Users/ExpireAllUserPasswords' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Users/GetPermissions' + params, doNotHandleFail);
	 	 }

	 }
	 export class Theme{
	 	 public static GetText(keys: string[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IThemeDTO[]>{
	 	 	 var params = '';
	 	 	 if (keys != null)
	 	 	 	 for(var j = 0; j < keys.length; j++) { params += '&keys=' + encodeURIComponent(keys[j]); }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IThemeDTO[]>('Theme/GetText' + params, doNotHandleFail);
	 	 }

	 	 public static GetImagePath( doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IThemeDTO[]>{
	 	 	 var params = '';
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IThemeDTO[]>('Theme/GetImagePath' + params, doNotHandleFail);
	 	 }

	 }
	 export class Terms{
	 	 public static ListTemplateTerms(id: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateTermDTO[]>{
	 	 	 var params = '';
	 	 	 if (id != null) params += '&id=' + id;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITemplateTermDTO[]>('Terms/ListTemplateTerms' + params, doNotHandleFail);
	 	 }

	 	 public static ParseCodeList( doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Terms/ParseCodeList', doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Terms/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITermDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITermDTO[]>('Terms/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITermDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITermDTO[]>('Terms/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ITermDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITermDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITermDTO[]>('Terms/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ITermDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITermDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ITermDTO[]>('Terms/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ITermDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITermDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITermDTO[]>('Terms/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Terms/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Security{
	 	 public static ListSecurityEntities($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityEntityDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISecurityEntityDTO[]>('Security/ListSecurityEntities' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissionsByLocation(locations: Dns.Enums.PermissionAclTypes[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IPermissionDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IPermissionDTO[]>('Security/GetPermissionsByLocation', locations, doNotHandleFail);
	 	 }

	 	 public static GetDataMartPermissions(dataMartID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclDataMartDTO[]>('Security/GetDataMartPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetOrganizationPermissions(organizationID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclOrganizationDTO[]>{
	 	 	 var params = '';
	 	 	 if (organizationID != null) params += '&organizationID=' + organizationID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclOrganizationDTO[]>('Security/GetOrganizationPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetUserPermissions(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclUserDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclUserDTO[]>('Security/GetUserPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetGroupPermissions(groupID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (groupID != null) params += '&groupID=' + groupID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclGroupDTO[]>('Security/GetGroupPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetRegistryPermissions(registryID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclRegistryDTO[]>{
	 	 	 var params = '';
	 	 	 if (registryID != null) params += '&registryID=' + registryID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclRegistryDTO[]>('Security/GetRegistryPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectPermissions(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectDTO[]>('Security/GetProjectPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetGlobalPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclDTO[]>('Security/GetGlobalPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectOrganizationPermissions(projectID: any, organizationID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectOrganizationDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (organizationID != null) params += '&organizationID=' + organizationID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectOrganizationDTO[]>('Security/GetProjectOrganizationPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectRequestTypeWorkflowActivityPermissions(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[]>('Security/GetProjectRequestTypeWorkflowActivityPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowActivityPermissionsForIdentity(projectID: any, workflowActivityID: any, requestTypeID: any, permissionID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
	 	 	 if (permissionID != null)
	 	 	 	 for(var j = 0; j < permissionID.length; j++) { params += '&permissionID=' + permissionID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Security/GetWorkflowActivityPermissionsForIdentity' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectDataMartPermissions(projectID: any, dataMartID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectDataMartDTO[]>('Security/GetProjectDataMartPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectDataMartRequestTypePermissions(projectID: any, dataMartID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectDataMartRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectDataMartRequestTypeDTO[]>('Security/GetProjectDataMartRequestTypePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectRequestTypePermissions(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectRequestTypeDTO[]>('Security/GetProjectRequestTypePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetDataMartRequestTypePermissions(dataMartID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclDataMartRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclDataMartRequestTypeDTO[]>('Security/GetDataMartRequestTypePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetTemplatePermissions(templateID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclTemplateDTO[]>{
	 	 	 var params = '';
	 	 	 if (templateID != null) params += '&templateID=' + templateID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclTemplateDTO[]>('Security/GetTemplatePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypePermissions(requestTypeID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclRequestTypeDTO[]>('Security/GetRequestTypePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectRequestTypeWorkflowActivityPermissions(permissions: Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectRequestTypeWorkflowActivityPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateDataMartRequestTypePermissions(permissions: Dns.Interfaces.IAclDataMartRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateDataMartRequestTypePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectDataMartPermissions(permissions: Dns.Interfaces.IAclProjectDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectDataMartPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectDataMartRequestTypePermissions(permissions: Dns.Interfaces.IAclProjectDataMartRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectDataMartRequestTypePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectOrganizationPermissions(permissions: Dns.Interfaces.IAclProjectOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectOrganizationPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdatePermissions(permissions: Dns.Interfaces.IAclDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdatePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateGroupPermissions(permissions: Dns.Interfaces.IAclGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateGroupPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateRegistryPermissions(permissions: Dns.Interfaces.IAclRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateRegistryPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectPermissions(permissions: Dns.Interfaces.IAclProjectDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectRequestTypePermissions(permissions: Dns.Interfaces.IAclProjectRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectRequestTypePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateDataMartPermissions(permissions: Dns.Interfaces.IAclDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateDataMartPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateOrganizationPermissions(permissions: Dns.Interfaces.IAclOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateOrganizationPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateUserPermissions(permissions: Dns.Interfaces.IAclUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateUserPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static GetAvailableSecurityGroupTree($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITreeItemDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITreeItemDTO[]>('Security/GetAvailableSecurityGroupTree' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateTemplatePermissions(permissions: Dns.Interfaces.IAclTemplateDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateTemplatePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateRequestTypePermissions(permissions: Dns.Interfaces.IAclRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateRequestTypePermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static GetGlobalFieldOptionPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclGlobalFieldOptionDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclGlobalFieldOptionDTO[]>('Security/GetGlobalFieldOptionPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateFieldOptionPermissions(fieldOptionUpdates: Dns.Interfaces.IAclGlobalFieldOptionDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateFieldOptionPermissions', fieldOptionUpdates, doNotHandleFail);
	 	 }

	 	 public static GetProjectFieldOptionPermissions(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IAclProjectFieldOptionDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IAclProjectFieldOptionDTO[]>('Security/GetProjectFieldOptionPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectFieldOptionPermissions(permissions: Dns.Interfaces.IAclProjectFieldOptionDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Security/UpdateProjectFieldOptionPermissions', permissions, doNotHandleFail);
	 	 }

	 }
	 export class SecurityGroups{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ISecurityGroupDTO[]>('SecurityGroups/List' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('SecurityGroups/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ISecurityGroupDTO[]>('SecurityGroups/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ISecurityGroupDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('SecurityGroups/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Organizations{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationDTO[]>('Organizations/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationDTO[]>('Organizations/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListByGroupMembership(groupID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 var params = '';
	 	 	 if (groupID != null) params += '&groupID=' + groupID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationDTO[]>('Organizations/ListByGroupMembership' + params, doNotHandleFail);
	 	 }

	 	 public static Copy(organizationID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (organizationID != null) params += '&organizationID=' + organizationID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Organizations/Copy' + params, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Organizations/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IOrganizationDTO[]>('Organizations/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IOrganizationDTO[]>('Organizations/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IOrganizationDTO[]>('Organizations/Update', values, doNotHandleFail);
	 	 }

	 	 public static EHRSInsertOrUpdate(updates: Dns.Interfaces.IOrganizationUpdateEHRsesDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Organizations/EHRSInsertOrUpdate', updates, doNotHandleFail);
	 	 }

	 	 public static ListEHRS($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationEHRSDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationEHRSDTO[]>('Organizations/ListEHRS' + params, doNotHandleFail);
	 	 }

	 	 public static DeleteEHRS(id: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Organizations/DeleteEHRS' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Organizations/GetPermissions' + params, doNotHandleFail);
	 	 }

	 }
	 export class OrganizationRegistries{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationRegistryDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationRegistryDTO[]>('OrganizationRegistries/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(organizations: Dns.Interfaces.IOrganizationRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('OrganizationRegistries/InsertOrUpdate', organizations, doNotHandleFail);
	 	 }

	 	 public static Remove(organizations: Dns.Interfaces.IOrganizationRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('OrganizationRegistries/Remove', organizations, doNotHandleFail);
	 	 }

	 }
	 export class Registries{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRegistryDTO[]>('Registries/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRegistryDTO[]>('Registries/List' + params, doNotHandleFail);
	 	 }

	 	 public static GetRegistryItemDefinitionList(registryID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryItemDefinitionDTO[]>{
	 	 	 var params = '';
	 	 	 if (registryID != null) params += '&registryID=' + registryID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRegistryItemDefinitionDTO[]>('Registries/GetRegistryItemDefinitionList' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateRegistryItemDefinitions(updateParams: Dns.Interfaces.IUpdateRegistryItemsDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PutAPIValue<any[]>('Registries/UpdateRegistryItemDefinitions', updateParams, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Registries/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRegistryDTO[]>('Registries/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IRegistryDTO[]>('Registries/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IRegistryDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRegistryDTO[]>('Registries/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Registries/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class RegistryItemDefinition{
	 	 public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryItemDefinitionDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRegistryItemDefinitionDTO[]>('RegistryItemDefinition/GetList' + params, doNotHandleFail);
	 	 }

	 }
	 export class NetworkMessages{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListLastDays(days: number,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 var params = '';
	 	 	 if (days != null) params += '&days=' + days;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/ListLastDays' + params, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/Insert', values, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('NetworkMessages/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.INetworkMessageDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.INetworkMessageDTO[]>('NetworkMessages/Update', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('NetworkMessages/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class LookupListCategory{
	 	 public static GetList(listID: Dns.Enums.Lists,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ILookupListCategoryDTO[]>{
	 	 	 var params = '';
	 	 	 if (listID != null) params += '&listID=' + listID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ILookupListCategoryDTO[]>('LookupListCategory/GetList' + params, doNotHandleFail);
	 	 }

	 }
	 export class LookupList{
	 	 public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ILookupListDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ILookupListDTO[]>('LookupList/GetList' + params, doNotHandleFail);
	 	 }

	 }
	 export class LookupListValue{
	 	 public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ILookupListValueDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ILookupListValueDTO[]>('LookupListValue/GetList' + params, doNotHandleFail);
	 	 }

	 	 public static GetCodeDetailsByCode(details: Dns.Interfaces.ILookupListDetailRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ILookupListValueDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ILookupListValueDTO[]>('LookupListValue/GetCodeDetailsByCode', details, doNotHandleFail);
	 	 }

	 	 public static LookupList(listID: Dns.Enums.Lists, lookup: string,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ILookupListValueDTO[]>{
	 	 	 var params = '';
	 	 	 if (listID != null) params += '&listID=' + listID;
	 	 	 if (lookup != null) params += '&lookup=' + encodeURIComponent(lookup);
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ILookupListValueDTO[]>('LookupListValue/LookupList' + params, doNotHandleFail);
	 	 }

	 }
	 export class Groups{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IGroupDTO[]>('Groups/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IGroupDTO[]>('Groups/List' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Groups/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IGroupDTO[]>('Groups/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IGroupDTO[]>('Groups/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IGroupDTO[]>('Groups/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Groups/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class OrganizationGroups{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationGroupDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationGroupDTO[]>('OrganizationGroups/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(organizations: Dns.Interfaces.IOrganizationGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('OrganizationGroups/InsertOrUpdate', organizations, doNotHandleFail);
	 	 }

	 	 public static Remove(organizations: Dns.Interfaces.IOrganizationGroupDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('OrganizationGroups/Remove', organizations, doNotHandleFail);
	 	 }

	 }
	 export class Events{
	 	 public static GetEventsByLocation(locations: Dns.Enums.PermissionAclTypes[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IEventDTO[]>('Events/GetEventsByLocation', locations, doNotHandleFail);
	 	 }

	 	 public static GetGroupEventPermissions(groupID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IGroupEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (groupID != null) params += '&groupID=' + groupID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IGroupEventDTO[]>('Events/GetGroupEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetRegistryEventPermissions(registryID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRegistryEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (registryID != null) params += '&registryID=' + registryID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRegistryEventDTO[]>('Events/GetRegistryEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectEventPermissions(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectEventDTO[]>('Events/GetProjectEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetOrganizationEventPermissions(organizationID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (organizationID != null) params += '&organizationID=' + organizationID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationEventDTO[]>('Events/GetOrganizationEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetUserEventPermissions(userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IUserEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IUserEventDTO[]>('Events/GetUserEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static GetGlobalEventPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IBaseEventPermissionDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IBaseEventPermissionDTO[]>('Events/GetGlobalEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateGroupEventPermissions(permissions: Dns.Interfaces.IGroupEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateGroupEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateRegistryEventPermissions(permissions: Dns.Interfaces.IRegistryEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateRegistryEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectEventPermissions(permissions: Dns.Interfaces.IProjectEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateProjectEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateOrganizationEventPermissions(permissions: Dns.Interfaces.IOrganizationEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateOrganizationEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateUserEventPermissions(permissions: Dns.Interfaces.IUserEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateUserEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static UpdateGlobalEventPermissions(permissions: Dns.Interfaces.IBaseEventPermissionDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateGlobalEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static GetProjectOrganizationEventPermissions(projectID: any, organizationID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectOrganizationEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (organizationID != null) params += '&organizationID=' + organizationID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectOrganizationEventDTO[]>('Events/GetProjectOrganizationEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectOrganizationEventPermissions(permissions: Dns.Interfaces.IProjectOrganizationEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateProjectOrganizationEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static GetProjectDataMartEventPermissions(projectID: any, dataMartID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDataMartEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDataMartEventDTO[]>('Events/GetProjectDataMartEventPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectDataMartEventPermissions(permissions: Dns.Interfaces.IProjectDataMartEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Events/UpdateProjectDataMartEventPermissions', permissions, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Events/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IEventDTO[]>('Events/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IEventDTO[]>('Events/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IEventDTO[]>('Events/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IEventDTO[]>('Events/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IEventDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IEventDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IEventDTO[]>('Events/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Events/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Tasks{
	 	 public static ByRequestID(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITaskDTO[]>('Tasks/ByRequestID' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowActivityDataForRequest(requestID: any, workflowActivityID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Tasks/GetWorkflowActivityDataForRequest' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Tasks/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITaskDTO[]>('Tasks/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITaskDTO[]>('Tasks/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ITaskDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITaskDTO[]>('Tasks/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ITaskDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ITaskDTO[]>('Tasks/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ITaskDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITaskDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITaskDTO[]>('Tasks/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Tasks/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class LegacyRequests{
	 	 public static ScheduleLegacyRequest(dto: Dns.Interfaces.ILegacySchedulerRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('LegacyRequests/ScheduleLegacyRequest', dto, doNotHandleFail);
	 	 }

	 	 public static DeleteRequestSchedules(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('LegacyRequests/DeleteRequestSchedules', requestID, doNotHandleFail);
	 	 }

	 }
	 export class ReportAggregationLevel{
	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('ReportAggregationLevel/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('ReportAggregationLevel/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Insert', values, doNotHandleFail);
	 	 }

	 }
	 export class RequestObservers{
	 	 public static Insert(values: Dns.Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/Insert', values, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/Update', values, doNotHandleFail);
	 	 }

	 	 public static ListRequestObservers(RequestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 var params = '';
	 	 	 if (RequestID != null) params += '&RequestID=' + RequestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/ListRequestObservers' + params, doNotHandleFail);
	 	 }

	 	 public static LookupObserverEvents($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IObserverEventDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IObserverEventDTO[]>('RequestObservers/LookupObserverEvents' + params, doNotHandleFail);
	 	 }

	 	 public static LookupObservers($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IObserverDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IObserverDTO[]>('RequestObservers/LookupObservers' + params, doNotHandleFail);
	 	 }

	 	 public static ValidateInsertOrUpdate(values: Dns.Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('RequestObservers/ValidateInsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('RequestObservers/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestObserverDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestObserverDTO[]>('RequestObservers/List' + params, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('RequestObservers/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class RequestUsers{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestUserDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestUserDTO[]>('RequestUsers/List' + params, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IRequestUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestUserDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestUserDTO[]>('RequestUsers/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(requestUsers: Dns.Interfaces.IRequestUserDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestUsers != null)
	 	 	 	 for(var j = 0; j < requestUsers.length; j++) { params += '&requestUsers=' + requestUsers[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('RequestUsers/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Response{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseDTO[]>('Response/Get' + params, doNotHandleFail);
	 	 }

	 	 public static ApproveResponses(responses: Dns.Interfaces.IApproveResponseDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Response/ApproveResponses', responses, doNotHandleFail);
	 	 }

	 	 public static RejectResponses(responses: Dns.Interfaces.IRejectResponseDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Response/RejectResponses', responses, doNotHandleFail);
	 	 }

	 	 public static RejectAndReSubmitResponses(responses: Dns.Interfaces.IRejectResponseDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Response/RejectAndReSubmitResponses', responses, doNotHandleFail);
	 	 }

	 	 public static GetByWorkflowActivity(requestID: any, workflowActivityID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseDTO[]>('Response/GetByWorkflowActivity' + params, doNotHandleFail);
	 	 }

	 	 public static CanViewIndividualResponses(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Response/CanViewIndividualResponses' + params, doNotHandleFail);
	 	 }

	 	 public static CanViewAggregateResponses(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Response/CanViewAggregateResponses' + params, doNotHandleFail);
	 	 }

	 	 public static CanViewPendingApprovalResponses(responses: Dns.Interfaces.IApproveResponseDTO, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 return Helpers.PostAPIValue<boolean[]>('Response/CanViewPendingApprovalResponses', responses, doNotHandleFail);
	 	 }

	 	 public static GetForWorkflowRequest(requestID: any, viewDocuments: boolean, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommonResponseDetailDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (viewDocuments != null) params += '&viewDocuments=' + viewDocuments;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ICommonResponseDetailDTO[]>('Response/GetForWorkflowRequest' + params, doNotHandleFail);
	 	 }

	 	 public static GetDetails(id: any[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommonResponseDetailDTO[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ICommonResponseDetailDTO[]>('Response/GetDetails' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowResponseContent(id: any[], view: Dns.Enums.TaskItemTypes,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IQueryComposerResponseDTO[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (view != null) params += '&view=' + view;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IQueryComposerResponseDTO[]>('Response/GetWorkflowResponseContent' + params, doNotHandleFail);
	 	 }

	 	 public static GetResponseGroups(responseIDs: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (responseIDs != null)
	 	 	 	 for(var j = 0; j < responseIDs.length; j++) { params += '&responseIDs=' + responseIDs[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseGroupDTO[]>('Response/GetResponseGroups' + params, doNotHandleFail);
	 	 }

	 	 public static GetResponseGroupsByRequestID(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseGroupDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseGroupDTO[]>('Response/GetResponseGroupsByRequestID' + params, doNotHandleFail);
	 	 }

	 	 public static Export(id: any[], view: Dns.Enums.TaskItemTypes, format: string, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (view != null) params += '&view=' + view;
	 	 	 if (format != null) params += '&format=' + encodeURIComponent(format);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/Export' + params, doNotHandleFail);
	 	 }

	 	 public static ExportAllAsZip(id: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/ExportAllAsZip' + params, doNotHandleFail);
	 	 }

	 	 public static GetTrackingTableForAnalysisCenter(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/GetTrackingTableForAnalysisCenter' + params, doNotHandleFail);
	 	 }

	 	 public static GetTrackingTableForDataPartners(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/GetTrackingTableForDataPartners' + params, doNotHandleFail);
	 	 }

	 	 public static GetEnhancedEventLog(requestID: any, format: string, download: boolean, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (format != null) params += '&format=' + encodeURIComponent(format);
	 	 	 if (download != null) params += '&download=' + download;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/GetEnhancedEventLog' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Response/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseDTO[]>('Response/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IResponseDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IResponseDTO[]>('Response/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IResponseDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IResponseDTO[]>('Response/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IResponseDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IResponseDTO[]>('Response/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Response/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Requests{
	 	 public static CompleteActivity(request: Dns.Interfaces.IRequestCompletionRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestCompletionResponseDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestCompletionResponseDTO[]>('Requests/CompleteActivity', request, doNotHandleFail);
	 	 }

	 	 public static TerminateRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PutAPIValue<any[]>('Requests/TerminateRequest', requestID, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestDTO[]>('Requests/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListForHomepage($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IHomepageRequestDetailDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IHomepageRequestDetailDTO[]>('Requests/ListForHomepage' + params, doNotHandleFail);
	 	 }

	 	 public static RequestsByRoute($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IHomepageRouteDetailDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IHomepageRouteDetailDTO[]>('Requests/RequestsByRoute' + params, doNotHandleFail);
	 	 }

	 	 public static GetCompatibleDataMarts(requestDetails: Dns.Interfaces.IMatchingCriteriaDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartListDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataMartListDTO[]>('Requests/GetCompatibleDataMarts', requestDetails, doNotHandleFail);
	 	 }

	 	 public static GetDataMartsForInstalledModels(requestDetails: Dns.Interfaces.IMatchingCriteriaDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartListDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataMartListDTO[]>('Requests/GetDataMartsForInstalledModels', requestDetails, doNotHandleFail);
	 	 }

	 	 public static RequestDataMarts(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestDataMartDTO[]>('Requests/RequestDataMarts' + params, doNotHandleFail);
	 	 }

	 	 public static GetOverrideableRequestDataMarts(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestDataMartDTO[]>('Requests/GetOverrideableRequestDataMarts' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateRequestDataMarts(dataMarts: Dns.Interfaces.IUpdateRequestDataMartStatusDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestDataMarts', dataMarts, doNotHandleFail);
	 	 }

	 	 public static UpdateRequestDataMartsMetadata(dataMarts: Dns.Interfaces.IRequestDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestDataMartsMetadata', dataMarts, doNotHandleFail);
	 	 }

	 	 public static ListRequesterCenters($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequesterCenterDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequesterCenterDTO[]>('Requests/ListRequesterCenters' + params, doNotHandleFail);
	 	 }

	 	 public static ListWorkPlanTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkplanTypeDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkplanTypeDTO[]>('Requests/ListWorkPlanTypes' + params, doNotHandleFail);
	 	 }

	 	 public static ListReportAggregationLevels($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IReportAggregationLevelDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IReportAggregationLevelDTO[]>('Requests/ListReportAggregationLevels' + params, doNotHandleFail);
	 	 }

	 	 public static GetWorkflowHistory(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWorkflowHistoryItemDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWorkflowHistoryItemDTO[]>('Requests/GetWorkflowHistory' + params, doNotHandleFail);
	 	 }

	 	 public static GetResponseHistory(requestDataMartID: any, requestID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IResponseHistoryDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestDataMartID != null) params += '&requestDataMartID=' + requestDataMartID;
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IResponseHistoryDTO[]>('Requests/GetResponseHistory' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestSearchTerms(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestSearchTermDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestSearchTermDTO[]>('Requests/GetRequestSearchTerms' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypeModels(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Requests/GetRequestTypeModels' + params, doNotHandleFail);
	 	 }

	 	 public static GetModelIDsforRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Requests/GetModelIDsforRequest' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateRequestMetadata(reqMetadata: Dns.Interfaces.IRequestMetadataDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestMetadata', reqMetadata, doNotHandleFail);
	 	 }

	 	 public static UpdateMetadataForRequests(updates: Dns.Interfaces.IRequestMetadataDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Requests/UpdateMetadataForRequests', updates, doNotHandleFail);
	 	 }

	 	 public static GetOrganizationsForRequest(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IOrganizationDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IOrganizationDTO[]>('Requests/GetOrganizationsForRequest' + params, doNotHandleFail);
	 	 }

	 	 public static AllowCopyRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<boolean[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<boolean[]>('Requests/AllowCopyRequest' + params, doNotHandleFail);
	 	 }

	 	 public static CopyRequest(requestID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Requests/CopyRequest', requestID, doNotHandleFail);
	 	 }

	 	 public static RetrieveBudgetInfoForRequests(ids: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestBudgetInfoDTO[]>{
	 	 	 var params = '';
	 	 	 if (ids != null)
	 	 	 	 for(var j = 0; j < ids.length; j++) { params += '&ids=' + ids[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestBudgetInfoDTO[]>('Requests/RetrieveBudgetInfoForRequests' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Requests/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestDTO[]>('Requests/Get' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IRequestDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestDTO[]>('Requests/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IRequestDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IRequestDTO[]>('Requests/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IRequestDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestDTO[]>('Requests/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Requests/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class RequestTypes{
	 	 public static Insert(values: Dns.Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/Insert', values, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListAvailableRequestTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/ListAvailableRequestTypes' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/Update', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('RequestTypes/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static Save(details: Dns.Interfaces.IUpdateRequestTypeRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('RequestTypes/Save', details, doNotHandleFail);
	 	 }

	 	 public static UpdateModels(details: Dns.Interfaces.IUpdateRequestTypeModelsDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('RequestTypes/UpdateModels', details, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypeModels(requestTypeID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeModelDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeModelDTO[]>('RequestTypes/GetRequestTypeModels' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypeTerms(requestTypeID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeTermDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetRequestTypeTerms' + params, doNotHandleFail);
	 	 }

	 	 public static GetFilteredTerms(id: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeTermDTO[]>{
	 	 	 var params = '';
	 	 	 if (id != null) params += '&id=' + id;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetFilteredTerms' + params, doNotHandleFail);
	 	 }

	 	 public static GetTermsFilteredBy($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeTermDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetTermsFilteredBy' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateRequestTypeTerms(updateInfo: Dns.Interfaces.IUpdateRequestTypeTermsDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('RequestTypes/UpdateRequestTypeTerms', updateInfo, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('RequestTypes/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('RequestTypes/Get' + params, doNotHandleFail);
	 	 }

	 }
	 export class Templates{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITemplateDTO[]>('Templates/List' + params, doNotHandleFail);
	 	 }

	 	 public static CriteriaGroups($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITemplateDTO[]>('Templates/CriteriaGroups' + params, doNotHandleFail);
	 	 }

	 	 public static GetByRequestType(requestTypeID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITemplateDTO[]>('Templates/GetByRequestType' + params, doNotHandleFail);
	 	 }

	 	 public static GetGlobalTemplatePermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IHasGlobalSecurityForTemplateDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IHasGlobalSecurityForTemplateDTO[]>('Templates/GetGlobalTemplatePermissions' + params, doNotHandleFail);
	 	 }

	 	 public static SaveCriteriaGroup(details: Dns.Interfaces.ISaveCriteriaGroupRequestDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITemplateDTO[]>('Templates/SaveCriteriaGroup', details, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Templates/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ITemplateDTO[]>('Templates/Get' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ITemplateDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITemplateDTO[]>('Templates/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ITemplateDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ITemplateDTO[]>('Templates/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ITemplateDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ITemplateDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ITemplateDTO[]>('Templates/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Templates/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class Notifications{
	 	 public static ExecuteScheduledNotifications(userName: string, password: string, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (userName != null) params += '&userName=' + encodeURIComponent(userName);
	 	 	 if (password != null) params += '&password=' + encodeURIComponent(password);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Notifications/ExecuteScheduledNotifications' + params, doNotHandleFail);
	 	 }

	 }
	 export class DataMartInstalledModels{
	 	 public static InsertOrUpdate(updateInfo: Dns.Interfaces.IUpdateDataMartInstalledModelsDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('DataMartInstalledModels/InsertOrUpdate', updateInfo, doNotHandleFail);
	 	 }

	 	 public static Remove(models: Dns.Interfaces.IDataMartInstalledModelDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('DataMartInstalledModels/Remove', models, doNotHandleFail);
	 	 }

	 }
	 export class ProjectDataMarts{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDataMartDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDataMartDTO[]>('ProjectDataMarts/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListWithRequestTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDataMartWithRequestTypesDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDataMartWithRequestTypesDTO[]>('ProjectDataMarts/ListWithRequestTypes' + params, doNotHandleFail);
	 	 }

	 	 public static GetWithRequestTypes(projectID: any, dataMartID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDataMartWithRequestTypesDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (dataMartID != null) params += '&dataMartID=' + dataMartID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDataMartWithRequestTypesDTO[]>('ProjectDataMarts/GetWithRequestTypes' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(updateInfo: Dns.Interfaces.IProjectDataMartUpdateDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('ProjectDataMarts/InsertOrUpdate', updateInfo, doNotHandleFail);
	 	 }

	 	 public static Remove(dataMarts: Dns.Interfaces.IProjectDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('ProjectDataMarts/Remove', dataMarts, doNotHandleFail);
	 	 }

	 }
	 export class ProjectOrganizations{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectOrganizationDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectOrganizationDTO[]>('ProjectOrganizations/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(updateInfo: Dns.Interfaces.IProjectOrganizationUpdateDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('ProjectOrganizations/InsertOrUpdate', updateInfo, doNotHandleFail);
	 	 }

	 	 public static Remove(organizations: Dns.Interfaces.IProjectOrganizationDTO[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('ProjectOrganizations/Remove', organizations, doNotHandleFail);
	 	 }

	 }
	 export class Projects{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDTO[]>('Projects/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDTO[]>('Projects/List' + params, doNotHandleFail);
	 	 }

	 	 public static ProjectsWithRequests($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDTO[]>('Projects/ProjectsWithRequests' + params, doNotHandleFail);
	 	 }

	 	 public static GetActivityTreeByProjectID(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IActivityDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IActivityDTO[]>('Projects/GetActivityTreeByProjectID' + params, doNotHandleFail);
	 	 }

	 	 public static RequestableProjects($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectDTO[]>('Projects/RequestableProjects' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypes(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('Projects/GetRequestTypes' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypesByModel(projectID: any, dataModelID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (dataModelID != null) params += '&dataModelID=' + dataModelID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('Projects/GetRequestTypesByModel' + params, doNotHandleFail);
	 	 }

	 	 public static GetAvailableRequestTypeForNewRequest(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IRequestTypeDTO[]>('Projects/GetAvailableRequestTypeForNewRequest' + params, doNotHandleFail);
	 	 }

	 	 public static GetDataModelsByProject(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelWithRequestTypesDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataModelWithRequestTypesDTO[]>('Projects/GetDataModelsByProject' + params, doNotHandleFail);
	 	 }

	 	 public static GetProjectRequestTypes(projectID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectRequestTypeDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IProjectRequestTypeDTO[]>('Projects/GetProjectRequestTypes' + params, doNotHandleFail);
	 	 }

	 	 public static UpdateProjectRequestTypes(requestTypes: Dns.Interfaces.IUpdateProjectRequestTypesDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Projects/UpdateProjectRequestTypes', requestTypes, doNotHandleFail);
	 	 }

	 	 public static Copy(projectID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Projects/Copy' + params, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Projects/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IProjectDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IProjectDTO[]>('Projects/Insert', values, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IProjectDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IProjectDTO[]>('Projects/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IProjectDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IProjectDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IProjectDTO[]>('Projects/Update', values, doNotHandleFail);
	 	 }

	 	 public static UpdateActivities(ID: any, username: string, password: string, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (username != null) params += '&username=' + encodeURIComponent(username);
	 	 	 if (password != null) params += '&password=' + encodeURIComponent(password);
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Projects/UpdateActivities' + params, doNotHandleFail);
	 	 }

	 	 public static GetFieldOptions(projectID: any, userID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IBaseFieldOptionAclDTO[]>{
	 	 	 var params = '';
	 	 	 if (projectID != null) params += '&projectID=' + projectID;
	 	 	 if (userID != null) params += '&userID=' + userID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IBaseFieldOptionAclDTO[]>('Projects/GetFieldOptions' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Projects/GetPermissions' + params, doNotHandleFail);
	 	 }

	 }
	 export class Documents{
	 	 public static ByTask(tasks: any[], filterByTaskItemType: Dns.Enums.TaskItemTypes[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IExtendedDocumentDTO[]>{
	 	 	 var params = '';
	 	 	 if (tasks != null)
	 	 	 	 for(var j = 0; j < tasks.length; j++) { params += '&tasks=' + tasks[j]; }
	 	 	 if (filterByTaskItemType != null)
	 	 	 	 for(var j = 0; j < filterByTaskItemType.length; j++) { params += '&filterByTaskItemType=' + filterByTaskItemType[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IExtendedDocumentDTO[]>('Documents/ByTask' + params, doNotHandleFail);
	 	 }

	 	 public static ByRevisionID(revisionSets: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IExtendedDocumentDTO[]>{
	 	 	 var params = '';
	 	 	 if (revisionSets != null)
	 	 	 	 for(var j = 0; j < revisionSets.length; j++) { params += '&revisionSets=' + revisionSets[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IExtendedDocumentDTO[]>('Documents/ByRevisionID' + params, doNotHandleFail);
	 	 }

	 	 public static ByResponse(ID: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IExtendedDocumentDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IExtendedDocumentDTO[]>('Documents/ByResponse' + params, doNotHandleFail);
	 	 }

	 	 public static GeneralRequestDocuments(requestID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IExtendedDocumentDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IExtendedDocumentDTO[]>('Documents/GeneralRequestDocuments' + params, doNotHandleFail);
	 	 }

	 	 public static Read(id: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null) params += '&id=' + id;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Documents/Read' + params, doNotHandleFail);
	 	 }

	 	 public static Download(id: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null) params += '&id=' + id;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Documents/Download' + params, doNotHandleFail);
	 	 }

	 	 public static Upload( doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Documents/Upload', doNotHandleFail);
	 	 }

	 	 public static UploadChunked( doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('Documents/UploadChunked', doNotHandleFail);
	 	 }

	 	 public static Delete(id: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (id != null)
	 	 	 	 for(var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Documents/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class DataMartAvailability{
	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartAvailabilityPeriodV2DTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartAvailabilityPeriodV2DTO[]>('DataMartAvailability/List' + params, doNotHandleFail);
	 	 }

	 }
	 export class DataModels{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataModelDTO[]>('DataModels/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataModelDTO[]>('DataModels/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListDataModelProcessors($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelProcessorDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataModelProcessorDTO[]>('DataModels/ListDataModelProcessors' + params, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('DataModels/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IDataModelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataModelDTO[]>('DataModels/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IDataModelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IDataModelDTO[]>('DataModels/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IDataModelDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataModelDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataModelDTO[]>('DataModels/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('DataModels/Delete' + params, doNotHandleFail);
	 	 }

	 }
	 export class DataMarts{
	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartDTO[]>('DataMarts/Get' + params, doNotHandleFail);
	 	 }

	 	 public static GetByRoute(requestDataMartID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestDataMartID != null) params += '&requestDataMartID=' + requestDataMartID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartDTO[]>('DataMarts/GetByRoute' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartDTO[]>('DataMarts/List' + params, doNotHandleFail);
	 	 }

	 	 public static ListBasic($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartListDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartListDTO[]>('DataMarts/ListBasic' + params, doNotHandleFail);
	 	 }

	 	 public static DataMartTypeList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartTypeDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartTypeDTO[]>('DataMarts/DataMartTypeList' + params, doNotHandleFail);
	 	 }

	 	 public static GetRequestTypesByDataMarts(DataMartId: any[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IRequestTypeDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IRequestTypeDTO[]>('DataMarts/GetRequestTypesByDataMarts', DataMartId, doNotHandleFail);
	 	 }

	 	 public static GetInstalledModelsByDataMart(DataMartId: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartInstalledModelDTO[]>{
	 	 	 var params = '';
	 	 	 if (DataMartId != null) params += '&DataMartId=' + DataMartId;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IDataMartInstalledModelDTO[]>('DataMarts/GetInstalledModelsByDataMart' + params, doNotHandleFail);
	 	 }

	 	 public static UninstallModel(model: Dns.Interfaces.IDataMartInstalledModelDTO, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 return Helpers.PostAPIValue<any[]>('DataMarts/UninstallModel', model, doNotHandleFail);
	 	 }

	 	 public static Copy(datamartID: any, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (datamartID != null) params += '&datamartID=' + datamartID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('DataMarts/Copy' + params, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('DataMarts/Delete' + params, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.IDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataMartDTO[]>('DataMarts/Insert', values, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.IDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IDataMartDTO[]>('DataMarts/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.IDataMartDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IDataMartDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.IDataMartDTO[]>('DataMarts/Update', values, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('DataMarts/GetPermissions' + params, doNotHandleFail);
	 	 }

	 }
	 export class Comments{
	 	 public static ByRequestID(requestID: any, workflowActivityID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWFCommentDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWFCommentDTO[]>('Comments/ByRequestID' + params, doNotHandleFail);
	 	 }

	 	 public static ByDocumentID(documentID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWFCommentDTO[]>{
	 	 	 var params = '';
	 	 	 if (documentID != null) params += '&documentID=' + documentID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.IWFCommentDTO[]>('Comments/ByDocumentID' + params, doNotHandleFail);
	 	 }

	 	 public static GetDocumentReferencesByRequest(requestID: any, workflowActivityID: any,$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDocumentReferenceDTO[]>{
	 	 	 var params = '';
	 	 	 if (requestID != null) params += '&requestID=' + requestID;
	 	 	 if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ICommentDocumentReferenceDTO[]>('Comments/GetDocumentReferencesByRequest' + params, doNotHandleFail);
	 	 }

	 	 public static AddWorkflowComment(value: Dns.Interfaces.IAddWFCommentDTO, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.IWFCommentDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.IWFCommentDTO[]>('Comments/AddWorkflowComment', value, doNotHandleFail);
	 	 }

	 	 public static GetPermissions(IDs: any[], permissions: any[],$filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (IDs != null)
	 	 	 	 for(var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
	 	 	 if (permissions != null)
	 	 	 	 for(var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<any[]>('Comments/GetPermissions' + params, doNotHandleFail);
	 	 }

	 	 public static Get(ID: any, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDTO[]>{
	 	 	 var params = '';
	 	 	 if (ID != null) params += '&ID=' + ID;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ICommentDTO[]>('Comments/Get' + params, doNotHandleFail);
	 	 }

	 	 public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDTO[]>{
	 	 	 var params = '';
             if($filter) params += '&$filter=' + $filter;
			if($select) params += '&$select=' + $select;
			if($orderby) params += '&$orderby=' + $orderby;
			if($skip) params += '&$skip=' + $skip;
			if($top) params += '&$top=' + $top;
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.GetAPIResult<Dns.Interfaces.ICommentDTO[]>('Comments/List' + params, doNotHandleFail);
	 	 }

	 	 public static InsertOrUpdate(values: Dns.Interfaces.ICommentDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ICommentDTO[]>('Comments/InsertOrUpdate', values, doNotHandleFail);
	 	 }

	 	 public static Update(values: Dns.Interfaces.ICommentDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDTO[]>{
	 	 	 return Helpers.PutAPIValue<Dns.Interfaces.ICommentDTO[]>('Comments/Update', values, doNotHandleFail);
	 	 }

	 	 public static Insert(values: Dns.Interfaces.ICommentDTO[], doNotHandleFail?: boolean):JQueryDeferred<Dns.Interfaces.ICommentDTO[]>{
	 	 	 return Helpers.PostAPIValue<Dns.Interfaces.ICommentDTO[]>('Comments/Insert', values, doNotHandleFail);
	 	 }

	 	 public static Delete(ID: any[], doNotHandleFail?: boolean):JQueryDeferred<any[]>{
	 	 	 var params = '';
	 	 	 if (ID != null)
	 	 	 	 for(var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
	 	 	 if (params.length > 0)
	 	 	 	 params = '?' + params.substr(1);

	 	 	 return Helpers.DeleteAPIValue<any[]>('Comments/Delete' + params, doNotHandleFail);
	 	 }

	 }
     export class Helpers {
            static failMethod: Function;
            public static RegisterFailMethod(method: Function) {
                this.failMethod = method;
            }

            static FixStringDatesInResults(results) {
                results.forEach((data) => {
                    for (var field in data) {
                        if (data[field]) {
                            if ($.isArray(data[field])) {
                                this.FixStringDatesInResults(data[field]);
                            } else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                                if (data[field].indexOf('Z') > -1) {
                                    data[field] = new Date(data[field]);
                                } else {
                                    data[field] = new Date(data[field] + 'Z');
                                }
                            }
                        }
                    }
                });
            }

            public static GetAPIResult<T>(url: string, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();

                if (!jQuery.support.cors) {
                    url = '/api/get?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'GET',
                    url: url,
                    dataType: 'json',
                }).done((result) => {
                    if (result == null || result.results == null) {
                        d.resolve();
                        return;
                    } 

                    if (!$.isArray(result.results))
                        result.results = [result.results];

                    //Fix dates from strings into real dates.
                    this.FixStringDatesInResults(result.results);

                    d.resolve(<any>result.results);
                }).fail((e, description, error) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);
                    d.reject(<any>e);
                });

                return d;
            }

            static PostAPIValue<T>(url: string, value: any, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/post?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'POST',
                    url: url,
                    dataType: 'json',
                    data: value == null ? null : JSON.stringify(value),
                    contentType: 'application/json; charset=utf-8',
                    timeout: 60000
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });


                return d;
            }

            static PutAPIValue<T>(url: string, value: any, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/put?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'PUT',
                    url: url,
                    dataType: 'json',
                    data: value == null ? null : JSON.stringify(value),
                    contentType: 'application/json; charset=utf-8',
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });

                return d;
            }

            static DeleteAPIValue<T>(url: string, doNotHandleFail?: boolean): JQueryDeferred<T> {
                var d = jQuery.Deferred<T>();
                if (!jQuery.support.cors) {
                    url = '/api/delete?Url=' + encodeURIComponent(url);
                } else {
                    url = ServiceUrl + url;
                }

                $.ajax({
                    type: 'DELETE',
                    url: url,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                }).done((result) => {
                    if (result == null) {
                        d.resolve();
                        return;
                    } else if (result.results != null) {
                        if (!$.isArray(result.results)) {
                            d.resolve(<any>[result.results]);
                            return;
                        } else {
                            d.resolve(<any>result.results);
                            return;
                        }
                    } else {
                        if (!$.isArray(result)) {
                            d.resolve(<any>[result]);
                            return;
                        }
                    }

                    d.resolve(<any>result);
                }).fail((e) => {
                    if (this.failMethod && !doNotHandleFail)
                        this.failMethod(e);

                    d.reject(<any>e);
                });

                return d;
            }
        }
     var _SignalRConnection: SignalR.Hub.Connection = null;
     export function SignalRConnection() : SignalR.Hub.Connection {
            if (_SignalRConnection == null) {
                _SignalRConnection = $.hubConnection(ServiceUrl + ' / signalr', null);

                _SignalRConnection.qs = { 'Auth': User.AuthToken };
                _SignalRConnection.start();
            }

            return _SignalRConnection;
     }
	 export class RequestsHub{
        static _proxy: SignalR.Hub.Proxy = null;
        public static Proxy(): SignalR.Hub.Proxy {
            if (this._proxy == null)
                this._proxy = SignalRConnection().createHubProxy('RequestsHub');

            return this._proxy;
            }


        public static NotifyCrud(NotifyFunction: (data: Dns.Interfaces.INotificationCrudDTO) => void) {
			this.Proxy().on('NotifyCrud', NotifyFunction);
        }
        public static ResultsReceived(OnResultsReceived: (string) => void) {
			this.Proxy().on('ResultsReceived', OnResultsReceived);
		}
                        
}
}

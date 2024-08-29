/// <reference path='Lpp.Dns.ViewModels.ts' />
import { UserInfo } from "./page/global.js";
import * as Enums from './Dns.Enums.js';
import * as Interfaces from "./Dns.Interfaces.js";

declare var ServiceUrl: string;
declare var User: UserInfo;
export class Workflow {
    public static GetWorkflowEntryPointByRequestTypeID(requestTypeID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowActivityDTO[]> {
        
        var params = '';
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowEntryPointByRequestTypeID' + params, doNotHandleFail);
    }

    public static GetWorkflowActivity(workflowActivityID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowActivityDTO[]> {
        var params = '';
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowActivity' + params, doNotHandleFail);
    }

    public static GetWorkflowActivitiesByWorkflowID(workFlowID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowActivityDTO[]> {
        var params = '';
        if (workFlowID != null) params += '&workFlowID=' + workFlowID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowActivityDTO[]>('Workflow/GetWorkflowActivitiesByWorkflowID' + params, doNotHandleFail);
    }

    public static GetWorkflowRolesByWorkflowID(workflowID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowRoleDTO[]> {
        var params = '';
        if (workflowID != null) params += '&workflowID=' + workflowID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowRoleDTO[]>('Workflow/GetWorkflowRolesByWorkflowID' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Workflow/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowDTO[]>('Workflow/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowDTO[]>('Workflow/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IWorkflowDTO[]>('Workflow/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IWorkflowDTO[]>('Workflow/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IWorkflowDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IWorkflowDTO[]>('Workflow/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Workflow/Delete' + params, doNotHandleFail);
    }

}
export class Wbd {
    public static ApproveRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PutAPIValue<any[]>('Wbd/ApproveRequest', requestID, doNotHandleFail);
    }

    public static RejectRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PutAPIValue<any[]>('Wbd/RejectRequest', requestID, doNotHandleFail);
    }

    public static GetRequestByID(Id: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        var params = '';
        if (Id != null) params += '&Id=' + Id;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestDTO[]>('Wbd/GetRequestByID' + params, doNotHandleFail);
    }

    public static SaveRequest(request: Interfaces.IRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Wbd/SaveRequest', request, doNotHandleFail);
    }

    public static GetActivityTreeByProjectID(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IActivityDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IActivityDTO[]>('Wbd/GetActivityTreeByProjectID' + params, doNotHandleFail);
    }

    public static Register(registration: Interfaces.IRegisterDataMartDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartRegistrationResultDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataMartRegistrationResultDTO[]>('Wbd/Register', registration, doNotHandleFail);
    }

    public static GetChanges(criteria: Interfaces.IGetChangeRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWbdChangeSetDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IWbdChangeSetDTO[]>('Wbd/GetChanges', criteria, doNotHandleFail);
    }

    public static DownloadDocument(documentId: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (documentId != null) params += '&documentId=' + documentId;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Wbd/DownloadDocument' + params, doNotHandleFail);
    }

    public static DownloadRequestViewableFile(requestId: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestId != null) params += '&requestId=' + requestId;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Wbd/DownloadRequestViewableFile' + params, doNotHandleFail);
    }

    public static CopyRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Wbd/CopyRequest' + params, doNotHandleFail);
    }

    public static UpdateResponseStatus(details: Interfaces.IUpdateResponseStatusRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Wbd/UpdateResponseStatus', details, doNotHandleFail);
    }

}
export class SsoEndpoints {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISsoEndpointDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISsoEndpointDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/List' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('SsoEndpoints/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISsoEndpointDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISsoEndpointDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ISsoEndpointDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISsoEndpointDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ISsoEndpointDTO[]>('SsoEndpoints/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('SsoEndpoints/Delete' + params, doNotHandleFail);
    }

}
export class Users {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserDTO>('Users/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserDTO[]>('Users/List' + params, doNotHandleFail);
    }

    public static ValidateLogin(login: Interfaces.ILoginDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IUserDTO[]>('Users/ValidateLogin', login, doNotHandleFail);
    }

    public static ByUserName(userName: string, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        var params = '';
        if (userName != null) params += '&userName=' + encodeURIComponent(userName);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserDTO[]>('Users/ByUserName' + params, doNotHandleFail);
    }

    public static UserRegistration(data: Interfaces.IUserRegistrationDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/UserRegistration', data, doNotHandleFail);
    }

    public static ListAvailableProjects($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDTO[]>('Users/ListAvailableProjects' + params, doNotHandleFail);
    }

    public static ForgotPassword(data: Interfaces.IForgotPasswordDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/ForgotPassword', data, doNotHandleFail);
    }

    public static ChangePassword(updateInfo: Interfaces.IUpdateUserPasswordDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/ChangePassword', updateInfo, doNotHandleFail);
    }

    public static RestorePassword(updateInfo: Interfaces.IRestorePasswordDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PutAPIValue<any[]>('Users/RestorePassword', updateInfo, doNotHandleFail);
    }

    public static GetAssignedNotifications(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAssignedUserNotificationDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAssignedUserNotificationDTO[]>('Users/GetAssignedNotifications' + params, doNotHandleFail);
    }

    public static GetSubscribableEvents(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IEventDTO[]>('Users/GetSubscribableEvents' + params, doNotHandleFail);
    }

    public static GetSubscribedEvents(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserEventSubscriptionDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserEventSubscriptionDTO[]>('Users/GetSubscribedEvents' + params, doNotHandleFail);
    }

    public static GetNotifications(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INotificationDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.INotificationDTO[]>('Users/GetNotifications' + params, doNotHandleFail);
    }

    public static ListAuthenticationAudits($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserAuthenticationDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserAuthenticationDTO[]>('Users/ListAuthenticationAudits' + params, doNotHandleFail);
    }

    public static ListDistinctEnvironments(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserAuthenticationDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserAuthenticationDTO[]>('Users/ListDistinctEnvironments' + params, doNotHandleFail);
    }

    public static UpdateSubscribedEvents(subscribedEvents: Interfaces.IUserEventSubscriptionDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/UpdateSubscribedEvents', subscribedEvents, doNotHandleFail);
    }

    public static GetTasks(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITaskDTO[]>('Users/GetTasks' + params, doNotHandleFail);
    }

    public static GetWorkflowTasks(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IHomepageTaskSummaryDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IHomepageTaskSummaryDTO[]>('Users/GetWorkflowTasks' + params, doNotHandleFail);
    }

    public static GetWorkflowTaskUsers(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IHomepageTaskRequestUserDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IHomepageTaskRequestUserDTO[]>('Users/GetWorkflowTaskUsers' + params, doNotHandleFail);
    }

    public static Logout(doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/Logout', doNotHandleFail);
    }

    public static MemberOfSecurityGroups(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISecurityGroupDTO[]>('Users/MemberOfSecurityGroups' + params, doNotHandleFail);
    }

    public static UpdateSecurityGroups(groups: Interfaces.IUpdateUserSecurityGroupsDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/UpdateSecurityGroups', groups, doNotHandleFail);
    }

    public static GetGlobalPermission(permissionID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (permissionID != null) params += '&permissionID=' + permissionID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Users/GetGlobalPermission' + params, doNotHandleFail);
    }

    public static ReturnMainMenu($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IMenuItemDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IMenuItemDTO[]>('Users/ReturnMainMenu' + params, doNotHandleFail);
    }

    public static UpdateLookupListsTest(username: string, password: string, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (username != null) params += '&username=' + encodeURIComponent(username);
        if (password != null) params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Users/UpdateLookupListsTest' + params, doNotHandleFail);
    }

    public static UpdateLookupLists(username: string, password: string, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (username != null) params += '&username=' + encodeURIComponent(username);
        if (password != null) params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Users/UpdateLookupLists' + params, doNotHandleFail);
    }

    public static SaveSetting(setting: Interfaces.IUserSettingDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Users/SaveSetting', setting, doNotHandleFail);
    }

    public static GetSetting(key: string[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserSettingDTO[]> {
        var params = '';
        if (key != null)
            for (var j = 0; j < key.length; j++) { params += '&key=' + encodeURIComponent(key[j]); }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserSettingDTO[]>('Users/GetSetting' + params, doNotHandleFail);
    }

    public static AllowApproveRejectRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Users/AllowApproveRejectRequest' + params, doNotHandleFail);
    }

    public static HasPassword(userID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Users/HasPassword' + params, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Users/Delete' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IUserDTO[]>('Users/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IUserDTO[]>('Users/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IUserDTO[]>('Users/Insert', values, doNotHandleFail);
    }

    public static GetMetadataEditPermissionsSummary(doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IMetadataEditPermissionsSummaryDTO> {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IMetadataEditPermissionsSummaryDTO>('Users/GetMetadataEditPermissionsSummary' + params, doNotHandleFail);
    }

    public static ExpireAllUserPasswords(doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Users/ExpireAllUserPasswords' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Users/GetPermissions' + params, doNotHandleFail);
    }

}
export class Theme {
    public static GetText(keys: string[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IThemeDTO> {
        var params = '';
        if (keys != null)
            for (var j = 0; j < keys.length; j++) { params += '&keys=' + encodeURIComponent(keys[j]); }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IThemeDTO>('Theme/GetText' + params, doNotHandleFail);
    }

    public static GetImagePath(doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IThemeDTO> {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IThemeDTO>('Theme/GetImagePath' + params, doNotHandleFail);
    }

}
export class Terms {
    public static ListTemplateTerms(id: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateTermDTO[]> {
        var params = '';
        if (id != null) params += '&id=' + id;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateTermDTO[]>('Terms/ListTemplateTerms' + params, doNotHandleFail);
    }

    public static ParseCodeList(doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Terms/ParseCodeList', doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Terms/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITermDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITermDTO[]>('Terms/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITermDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITermDTO[]>('Terms/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ITermDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITermDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITermDTO[]>('Terms/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ITermDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITermDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ITermDTO[]>('Terms/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ITermDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITermDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITermDTO[]>('Terms/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Terms/Delete' + params, doNotHandleFail);
    }

}
export class Security {
    public static ListSecurityEntities($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityEntityDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISecurityEntityDTO[]>('Security/ListSecurityEntities' + params, doNotHandleFail);
    }

    public static GetPermissionsByLocation(locations: Enums.PermissionAclTypes[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IPermissionDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IPermissionDTO[]>('Security/GetPermissionsByLocation', locations, doNotHandleFail);
    }

    public static GetDataMartPermissions(dataMartID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclDataMartDTO[]> {
        var params = '';
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclDataMartDTO[]>('Security/GetDataMartPermissions' + params, doNotHandleFail);
    }

    public static GetOrganizationPermissions(organizationID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclOrganizationDTO[]> {
        var params = '';
        if (organizationID != null) params += '&organizationID=' + organizationID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclOrganizationDTO[]>('Security/GetOrganizationPermissions' + params, doNotHandleFail);
    }

    public static GetUserPermissions(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclUserDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclUserDTO[]>('Security/GetUserPermissions' + params, doNotHandleFail);
    }

    public static GetGroupPermissions(groupID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclGroupDTO[]> {
        var params = '';
        if (groupID != null) params += '&groupID=' + groupID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclGroupDTO[]>('Security/GetGroupPermissions' + params, doNotHandleFail);
    }

    public static GetRegistryPermissions(registryID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclRegistryDTO[]> {
        var params = '';
        if (registryID != null) params += '&registryID=' + registryID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclRegistryDTO[]>('Security/GetRegistryPermissions' + params, doNotHandleFail);
    }

    public static GetProjectPermissions(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectDTO[]>('Security/GetProjectPermissions' + params, doNotHandleFail);
    }

    public static GetGlobalPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclDTO[]>('Security/GetGlobalPermissions' + params, doNotHandleFail);
    }

    public static GetProjectOrganizationPermissions(projectID: any, organizationID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectOrganizationDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (organizationID != null) params += '&organizationID=' + organizationID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectOrganizationDTO[]>('Security/GetProjectOrganizationPermissions' + params, doNotHandleFail);
    }

    public static GetProjectRequestTypeWorkflowActivityPermissions(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[]>('Security/GetProjectRequestTypeWorkflowActivityPermissions' + params, doNotHandleFail);
    }

    public static GetWorkflowActivityPermissionsForIdentity(projectID: any, workflowActivityID: any, requestTypeID: any, permissionID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if (permissionID != null)
            for (var j = 0; j < permissionID.length; j++) { params += '&permissionID=' + permissionID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Security/GetWorkflowActivityPermissionsForIdentity' + params, doNotHandleFail);
    }

    public static GetProjectDataMartPermissions(projectID: any, dataMartID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectDataMartDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectDataMartDTO[]>('Security/GetProjectDataMartPermissions' + params, doNotHandleFail);
    }

    public static GetProjectDataMartRequestTypePermissions(projectID: any, dataMartID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectDataMartRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectDataMartRequestTypeDTO[]>('Security/GetProjectDataMartRequestTypePermissions' + params, doNotHandleFail);
    }

    public static GetProjectRequestTypePermissions(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectRequestTypeDTO[]>('Security/GetProjectRequestTypePermissions' + params, doNotHandleFail);
    }

    public static GetDataMartRequestTypePermissions(dataMartID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclDataMartRequestTypeDTO[]> {
        var params = '';
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclDataMartRequestTypeDTO[]>('Security/GetDataMartRequestTypePermissions' + params, doNotHandleFail);
    }

    public static GetTemplatePermissions(templateID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclTemplateDTO[]> {
        var params = '';
        if (templateID != null) params += '&templateID=' + templateID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclTemplateDTO[]>('Security/GetTemplatePermissions' + params, doNotHandleFail);
    }

    public static GetRequestTypePermissions(requestTypeID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclRequestTypeDTO[]> {
        var params = '';
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclRequestTypeDTO[]>('Security/GetRequestTypePermissions' + params, doNotHandleFail);
    }

    public static UpdateProjectRequestTypeWorkflowActivityPermissions(permissions: Interfaces.IAclProjectRequestTypeWorkflowActivityDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectRequestTypeWorkflowActivityPermissions', permissions, doNotHandleFail);
    }

    public static UpdateDataMartRequestTypePermissions(permissions: Interfaces.IAclDataMartRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateDataMartRequestTypePermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectDataMartPermissions(permissions: Interfaces.IAclProjectDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectDataMartPermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectDataMartRequestTypePermissions(permissions: Interfaces.IAclProjectDataMartRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectDataMartRequestTypePermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectOrganizationPermissions(permissions: Interfaces.IAclProjectOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectOrganizationPermissions', permissions, doNotHandleFail);
    }

    public static UpdatePermissions(permissions: Interfaces.IAclDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdatePermissions', permissions, doNotHandleFail);
    }

    public static UpdateGroupPermissions(permissions: Interfaces.IAclGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateGroupPermissions', permissions, doNotHandleFail);
    }

    public static UpdateRegistryPermissions(permissions: Interfaces.IAclRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateRegistryPermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectPermissions(permissions: Interfaces.IAclProjectDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectPermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectRequestTypePermissions(permissions: Interfaces.IAclProjectRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectRequestTypePermissions', permissions, doNotHandleFail);
    }

    public static UpdateDataMartPermissions(permissions: Interfaces.IAclDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateDataMartPermissions', permissions, doNotHandleFail);
    }

    public static UpdateOrganizationPermissions(permissions: Interfaces.IAclOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateOrganizationPermissions', permissions, doNotHandleFail);
    }

    public static UpdateUserPermissions(permissions: Interfaces.IAclUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateUserPermissions', permissions, doNotHandleFail);
    }

    public static GetAvailableSecurityGroupTree($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITreeItemDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITreeItemDTO[]>('Security/GetAvailableSecurityGroupTree' + params, doNotHandleFail);
    }

    public static UpdateTemplatePermissions(permissions: Interfaces.IAclTemplateDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateTemplatePermissions', permissions, doNotHandleFail);
    }

    public static UpdateRequestTypePermissions(permissions: Interfaces.IAclRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateRequestTypePermissions', permissions, doNotHandleFail);
    }

    public static GetGlobalFieldOptionPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclGlobalFieldOptionDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclGlobalFieldOptionDTO[]>('Security/GetGlobalFieldOptionPermissions' + params, doNotHandleFail);
    }

    public static UpdateFieldOptionPermissions(fieldOptionUpdates: Interfaces.IAclGlobalFieldOptionDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateFieldOptionPermissions', fieldOptionUpdates, doNotHandleFail);
    }

    public static GetProjectFieldOptionPermissions(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IAclProjectFieldOptionDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IAclProjectFieldOptionDTO[]>('Security/GetProjectFieldOptionPermissions' + params, doNotHandleFail);
    }

    public static UpdateProjectFieldOptionPermissions(permissions: Interfaces.IAclProjectFieldOptionDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Security/UpdateProjectFieldOptionPermissions', permissions, doNotHandleFail);
    }

}
export class SecurityGroups {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ISecurityGroupDTO[]>('SecurityGroups/List' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('SecurityGroups/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ISecurityGroupDTO[]>('SecurityGroups/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ISecurityGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ISecurityGroupDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ISecurityGroupDTO[]>('SecurityGroups/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('SecurityGroups/Delete' + params, doNotHandleFail);
    }

}
export class Organizations {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationDTO[]>('Organizations/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationDTO[]>('Organizations/List' + params, doNotHandleFail);
    }

    public static ListByGroupMembership(groupID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        var params = '';
        if (groupID != null) params += '&groupID=' + groupID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationDTO[]>('Organizations/ListByGroupMembership' + params, doNotHandleFail);
    }

    public static Copy(organizationID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (organizationID != null) params += '&organizationID=' + organizationID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Organizations/Copy' + params, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Organizations/Delete' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IOrganizationDTO[]>('Organizations/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IOrganizationDTO[]>('Organizations/Insert', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IOrganizationDTO[]>('Organizations/Update', values, doNotHandleFail);
    }

    public static EHRSInsertOrUpdate(updates: Interfaces.IOrganizationUpdateEHRsesDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Organizations/EHRSInsertOrUpdate', updates, doNotHandleFail);
    }

    public static ListEHRS($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationEHRSDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationEHRSDTO[]>('Organizations/ListEHRS' + params, doNotHandleFail);
    }

    public static DeleteEHRS(id: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Organizations/DeleteEHRS' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Organizations/GetPermissions' + params, doNotHandleFail);
    }

}
export class OrganizationRegistries {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationRegistryDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationRegistryDTO[]>('OrganizationRegistries/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(organizations: Interfaces.IOrganizationRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('OrganizationRegistries/InsertOrUpdate', organizations, doNotHandleFail);
    }

    public static Remove(organizations: Interfaces.IOrganizationRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('OrganizationRegistries/Remove', organizations, doNotHandleFail);
    }

}
export class Registries {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRegistryDTO[]>('Registries/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRegistryDTO[]>('Registries/List' + params, doNotHandleFail);
    }

    public static GetRegistryItemDefinitionList(registryID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryItemDefinitionDTO[]> {
        var params = '';
        if (registryID != null) params += '&registryID=' + registryID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRegistryItemDefinitionDTO[]>('Registries/GetRegistryItemDefinitionList' + params, doNotHandleFail);
    }

    public static UpdateRegistryItemDefinitions(updateParams: Interfaces.IUpdateRegistryItemsDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PutAPIValue<any[]>('Registries/UpdateRegistryItemDefinitions', updateParams, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Registries/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRegistryDTO[]>('Registries/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IRegistryDTO[]>('Registries/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IRegistryDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRegistryDTO[]>('Registries/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Registries/Delete' + params, doNotHandleFail);
    }

}
export class RegistryItemDefinition {
    public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryItemDefinitionDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRegistryItemDefinitionDTO[]>('RegistryItemDefinition/GetList' + params, doNotHandleFail);
    }

}
export class NetworkMessages {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.INetworkMessageDTO[]>('NetworkMessages/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.INetworkMessageDTO[]>('NetworkMessages/List' + params, doNotHandleFail);
    }

    public static ListLastDays(days: number, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        var params = '';
        if (days != null) params += '&days=' + days;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.INetworkMessageDTO[]>('NetworkMessages/ListLastDays' + params, doNotHandleFail);
    }

    public static Insert(values: Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        return Helpers.PostAPIValue<Interfaces.INetworkMessageDTO[]>('NetworkMessages/Insert', values, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('NetworkMessages/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        return Helpers.PostAPIValue<Interfaces.INetworkMessageDTO[]>('NetworkMessages/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.INetworkMessageDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.INetworkMessageDTO[]> {
        return Helpers.PutAPIValue<Interfaces.INetworkMessageDTO[]>('NetworkMessages/Update', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('NetworkMessages/Delete' + params, doNotHandleFail);
    }

}
export class LookupListCategory {
    public static GetList(listID: Enums.Lists, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ILookupListCategoryDTO[]> {
        var params = '';
        if (listID != null) params += '&listID=' + listID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ILookupListCategoryDTO[]>('LookupListCategory/GetList' + params, doNotHandleFail);
    }

}
export class LookupList {
    public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ILookupListDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ILookupListDTO[]>('LookupList/GetList' + params, doNotHandleFail);
    }

}
export class LookupListValue {
    public static GetList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ILookupListValueDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ILookupListValueDTO[]>('LookupListValue/GetList' + params, doNotHandleFail);
    }

    public static GetCodeDetailsByCode(details: Interfaces.ILookupListDetailRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ILookupListValueDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ILookupListValueDTO[]>('LookupListValue/GetCodeDetailsByCode', details, doNotHandleFail);
    }

    public static LookupList(listID: Enums.Lists, lookup: string, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ILookupListValueDTO[]> {
        var params = '';
        if (listID != null) params += '&listID=' + listID;
        if (lookup != null) params += '&lookup=' + encodeURIComponent(lookup);
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ILookupListValueDTO[]>('LookupListValue/LookupList' + params, doNotHandleFail);
    }

}
export class Groups {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IGroupDTO[]>('Groups/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IGroupDTO[]>('Groups/List' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Groups/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IGroupDTO[]>('Groups/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IGroupDTO[]>('Groups/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IGroupDTO[]>('Groups/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Groups/Delete' + params, doNotHandleFail);
    }

}
export class OrganizationGroups {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationGroupDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationGroupDTO[]>('OrganizationGroups/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(organizations: Interfaces.IOrganizationGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('OrganizationGroups/InsertOrUpdate', organizations, doNotHandleFail);
    }

    public static Remove(organizations: Interfaces.IOrganizationGroupDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('OrganizationGroups/Remove', organizations, doNotHandleFail);
    }

}
export class Events {
    public static GetEventsByLocation(locations: Enums.PermissionAclTypes[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        var params = null;
        if (locations != null)
            for (var j = 0; j < locations.length; j++) { params += '&locations=' + locations[j]; }

        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IEventDTO[]>('Events/GetEventsByLocation' + params, doNotHandleFail);
    }

    public static GetGroupEventPermissions(groupID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IGroupEventDTO[]> {
        var params = '';
        if (groupID != null) params += '&groupID=' + groupID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IGroupEventDTO[]>('Events/GetGroupEventPermissions' + params, doNotHandleFail);
    }

    public static GetRegistryEventPermissions(registryID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRegistryEventDTO[]> {
        var params = '';
        if (registryID != null) params += '&registryID=' + registryID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRegistryEventDTO[]>('Events/GetRegistryEventPermissions' + params, doNotHandleFail);
    }

    public static GetProjectEventPermissions(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectEventDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectEventDTO[]>('Events/GetProjectEventPermissions' + params, doNotHandleFail);
    }

    public static GetOrganizationEventPermissions(organizationID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationEventDTO[]> {
        var params = '';
        if (organizationID != null) params += '&organizationID=' + organizationID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationEventDTO[]>('Events/GetOrganizationEventPermissions' + params, doNotHandleFail);
    }

    public static GetUserEventPermissions(userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IUserEventDTO[]> {
        var params = '';
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IUserEventDTO[]>('Events/GetUserEventPermissions' + params, doNotHandleFail);
    }

    public static GetGlobalEventPermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IBaseEventPermissionDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IBaseEventPermissionDTO[]>('Events/GetGlobalEventPermissions' + params, doNotHandleFail);
    }

    public static UpdateGroupEventPermissions(permissions: Interfaces.IGroupEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateGroupEventPermissions', permissions, doNotHandleFail);
    }

    public static UpdateRegistryEventPermissions(permissions: Interfaces.IRegistryEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateRegistryEventPermissions', permissions, doNotHandleFail);
    }

    public static UpdateProjectEventPermissions(permissions: Interfaces.IProjectEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateProjectEventPermissions', permissions, doNotHandleFail);
    }

    public static UpdateOrganizationEventPermissions(permissions: Interfaces.IOrganizationEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateOrganizationEventPermissions', permissions, doNotHandleFail);
    }

    public static UpdateUserEventPermissions(permissions: Interfaces.IUserEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateUserEventPermissions', permissions, doNotHandleFail);
    }

    public static UpdateGlobalEventPermissions(permissions: Interfaces.IBaseEventPermissionDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateGlobalEventPermissions', permissions, doNotHandleFail);
    }

    public static GetProjectOrganizationEventPermissions(projectID: any, organizationID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectOrganizationEventDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (organizationID != null) params += '&organizationID=' + organizationID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectOrganizationEventDTO[]>('Events/GetProjectOrganizationEventPermissions' + params, doNotHandleFail);
    }

    public static UpdateProjectOrganizationEventPermissions(permissions: Interfaces.IProjectOrganizationEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateProjectOrganizationEventPermissions', permissions, doNotHandleFail);
    }

    public static GetProjectDataMartEventPermissions(projectID: any, dataMartID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDataMartEventDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDataMartEventDTO[]>('Events/GetProjectDataMartEventPermissions' + params, doNotHandleFail);
    }

    public static UpdateProjectDataMartEventPermissions(permissions: Interfaces.IProjectDataMartEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Events/UpdateProjectDataMartEventPermissions', permissions, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Events/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IEventDTO[]>('Events/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IEventDTO[]>('Events/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IEventDTO[]>('Events/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IEventDTO[]>('Events/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IEventDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IEventDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IEventDTO[]>('Events/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Events/Delete' + params, doNotHandleFail);
    }

}
export class Tasks {
    public static ByRequestID(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITaskDTO[]>('Tasks/ByRequestID' + params, doNotHandleFail);
    }

    public static GetWorkflowActivityDataForRequest(requestID: any, workflowActivityID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Tasks/GetWorkflowActivityDataForRequest' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Tasks/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITaskDTO[]>('Tasks/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITaskDTO[]>('Tasks/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ITaskDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITaskDTO[]>('Tasks/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ITaskDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ITaskDTO[]>('Tasks/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ITaskDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITaskDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITaskDTO[]>('Tasks/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Tasks/Delete' + params, doNotHandleFail);
    }

}
export class LegacyRequests {
    public static ScheduleLegacyRequest(dto: Interfaces.ILegacySchedulerRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('LegacyRequests/ScheduleLegacyRequest', dto, doNotHandleFail);
    }

    public static DeleteRequestSchedules(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('LegacyRequests/DeleteRequestSchedules', requestID, doNotHandleFail);
    }

}
export class ReportAggregationLevel {
    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('ReportAggregationLevel/Delete' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('ReportAggregationLevel/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IReportAggregationLevelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IReportAggregationLevelDTO[]>('ReportAggregationLevel/Insert', values, doNotHandleFail);
    }

}
export class RequestObservers {
    public static Insert(values: Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestObserverDTO[]>('RequestObservers/Insert', values, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestObserverDTO[]>('RequestObservers/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestObserverDTO[]>('RequestObservers/Update', values, doNotHandleFail);
    }

    public static ListRequestObservers(RequestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        var params = '';
        if (RequestID != null) params += '&RequestID=' + RequestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestObserverDTO[]>('RequestObservers/ListRequestObservers' + params, doNotHandleFail);
    }

    public static LookupObserverEvents($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IObserverEventDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IObserverEventDTO[]>('RequestObservers/LookupObserverEvents' + params, doNotHandleFail);
    }

    public static LookupObservers($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IObserverDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IObserverDTO[]>('RequestObservers/LookupObservers' + params, doNotHandleFail);
    }

    public static ValidateInsertOrUpdate(values: Interfaces.IRequestObserverDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('RequestObservers/ValidateInsertOrUpdate', values, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('RequestObservers/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestObserverDTO[]>('RequestObservers/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestObserverDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestObserverDTO[]>('RequestObservers/List' + params, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('RequestObservers/Delete' + params, doNotHandleFail);
    }

}
export class RequestUsers {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestUserDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestUserDTO[]>('RequestUsers/List' + params, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IRequestUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestUserDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestUserDTO[]>('RequestUsers/Insert', values, doNotHandleFail);
    }

    public static Delete(requestUsers: Interfaces.IRequestUserDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestUsers != null)
            for (var j = 0; j < requestUsers.length; j++) { params += '&requestUsers=' + requestUsers[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('RequestUsers/Delete' + params, doNotHandleFail);
    }

}
export class Response {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseDTO[]>('Response/Get' + params, doNotHandleFail);
    }

    public static ApproveResponses(responses: Interfaces.IApproveResponseDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Response/ApproveResponses', responses, doNotHandleFail);
    }

    public static RejectResponses(responses: Interfaces.IRejectResponseDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Response/RejectResponses', responses, doNotHandleFail);
    }

    public static RejectAndReSubmitResponses(responses: Interfaces.IRejectResponseDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Response/RejectAndReSubmitResponses', responses, doNotHandleFail);
    }

    public static GetByWorkflowActivity(requestID: any, workflowActivityID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseDTO[]>('Response/GetByWorkflowActivity' + params, doNotHandleFail);
    }

    public static CanViewIndividualResponses(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Response/CanViewIndividualResponses' + params, doNotHandleFail);
    }

    public static CanViewAggregateResponses(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Response/CanViewAggregateResponses' + params, doNotHandleFail);
    }

    public static CanViewPendingApprovalResponses(responses: Interfaces.IApproveResponseDTO, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        return Helpers.PostAPIValue<boolean[]>('Response/CanViewPendingApprovalResponses', responses, doNotHandleFail);
    }

    public static GetForWorkflowRequest(requestID: any, viewDocuments: boolean, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommonResponseDetailDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (viewDocuments != null) params += '&viewDocuments=' + viewDocuments;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ICommonResponseDetailDTO[]>('Response/GetForWorkflowRequest' + params, doNotHandleFail);
    }

    public static GetDetails(id: any[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommonResponseDetailDTO[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ICommonResponseDetailDTO[]>('Response/GetDetails' + params, doNotHandleFail);
    }

    public static GetWorkflowResponseContent(id: any[], view: Enums.TaskItemTypes, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IQueryComposerResponseDTO[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (view != null) params += '&view=' + view;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IQueryComposerResponseDTO[]>('Response/GetWorkflowResponseContent' + params, doNotHandleFail);
    }

    public static GetResponseGroups(responseIDs: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseGroupDTO[]> {
        var params = '';
        if (responseIDs != null)
            for (var j = 0; j < responseIDs.length; j++) { params += '&responseIDs=' + responseIDs[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseGroupDTO[]>('Response/GetResponseGroups' + params, doNotHandleFail);
    }

    public static GetResponseGroupsByRequestID(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseGroupDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseGroupDTO[]>('Response/GetResponseGroupsByRequestID' + params, doNotHandleFail);
    }

    public static Export(id: any[], view: Enums.TaskItemTypes, format: string, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (view != null) params += '&view=' + view;
        if (format != null) params += '&format=' + encodeURIComponent(format);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/Export' + params, doNotHandleFail);
    }

    public static ExportAllAsZip(id: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/ExportAllAsZip' + params, doNotHandleFail);
    }

    public static GetTrackingTableForAnalysisCenter(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/GetTrackingTableForAnalysisCenter' + params, doNotHandleFail);
    }

    public static GetTrackingTableForDataPartners(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/GetTrackingTableForDataPartners' + params, doNotHandleFail);
    }

    public static GetEnhancedEventLog(requestID: any, format: string, download: boolean, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (format != null) params += '&format=' + encodeURIComponent(format);
        if (download != null) params += '&download=' + download;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/GetEnhancedEventLog' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Response/GetPermissions' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseDTO[]>('Response/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IResponseDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IResponseDTO[]>('Response/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IResponseDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IResponseDTO[]>('Response/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IResponseDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IResponseDTO[]>('Response/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Response/Delete' + params, doNotHandleFail);
    }

}
export class Requests {
    public static CompleteActivity(request: Interfaces.IRequestCompletionRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestCompletionResponseDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestCompletionResponseDTO[]>('Requests/CompleteActivity', request, doNotHandleFail);
    }

    public static TerminateRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PutAPIValue<any[]>('Requests/TerminateRequest', requestID, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestDTO[]>('Requests/List' + params, doNotHandleFail);
    }

    public static ListForHomepage($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IHomepageRequestDetailDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IHomepageRequestDetailDTO[]>('Requests/ListForHomepage' + params, doNotHandleFail);
    }

    public static RequestsByRoute($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IHomepageRouteDetailDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IHomepageRouteDetailDTO[]>('Requests/RequestsByRoute' + params, doNotHandleFail);
    }

    public static GetCompatibleDataMarts(requestDetails: Interfaces.IMatchingCriteriaDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartListDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataMartListDTO[]>('Requests/GetCompatibleDataMarts', requestDetails, doNotHandleFail);
    }

    public static GetDataMartsForInstalledModels(requestDetails: Interfaces.IMatchingCriteriaDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartListDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataMartListDTO[]>('Requests/GetDataMartsForInstalledModels', requestDetails, doNotHandleFail);
    }

    public static RequestDataMarts(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDataMartDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestDataMartDTO[]>('Requests/RequestDataMarts' + params, doNotHandleFail);
    }

    public static GetOverrideableRequestDataMarts(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDataMartDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestDataMartDTO[]>('Requests/GetOverrideableRequestDataMarts' + params, doNotHandleFail);
    }

    public static UpdateRequestDataMarts(dataMarts: Interfaces.IUpdateRequestDataMartStatusDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestDataMarts', dataMarts, doNotHandleFail);
    }

    public static UpdateRequestDataMartsMetadata(dataMarts: Interfaces.IRequestDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestDataMartsMetadata', dataMarts, doNotHandleFail);
    }

    public static ListRequesterCenters($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequesterCenterDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequesterCenterDTO[]>('Requests/ListRequesterCenters' + params, doNotHandleFail);
    }

    public static ListWorkPlanTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkplanTypeDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkplanTypeDTO[]>('Requests/ListWorkPlanTypes' + params, doNotHandleFail);
    }

    public static ListReportAggregationLevels($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IReportAggregationLevelDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IReportAggregationLevelDTO[]>('Requests/ListReportAggregationLevels' + params, doNotHandleFail);
    }

    public static GetWorkflowHistory(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWorkflowHistoryItemDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWorkflowHistoryItemDTO[]>('Requests/GetWorkflowHistory' + params, doNotHandleFail);
    }

    public static GetResponseHistory(requestDataMartID: any, requestID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IResponseHistoryDTO[]> {
        var params = '';
        if (requestDataMartID != null) params += '&requestDataMartID=' + requestDataMartID;
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IResponseHistoryDTO[]>('Requests/GetResponseHistory' + params, doNotHandleFail);
    }

    public static GetRequestSearchTerms(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestSearchTermDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestSearchTermDTO[]>('Requests/GetRequestSearchTerms' + params, doNotHandleFail);
    }

    public static GetRequestTypeModels(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Requests/GetRequestTypeModels' + params, doNotHandleFail);
    }

    public static GetModelIDsforRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Requests/GetModelIDsforRequest' + params, doNotHandleFail);
    }

    public static UpdateRequestMetadata(reqMetadata: Interfaces.IRequestMetadataDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Requests/UpdateRequestMetadata', reqMetadata, doNotHandleFail);
    }

    public static UpdateMetadataForRequests(updates: Interfaces.IRequestMetadataDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Requests/UpdateMetadataForRequests', updates, doNotHandleFail);
    }

    public static GetOrganizationsForRequest(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IOrganizationDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IOrganizationDTO[]>('Requests/GetOrganizationsForRequest' + params, doNotHandleFail);
    }

    public static AllowCopyRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<boolean[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<boolean[]>('Requests/AllowCopyRequest' + params, doNotHandleFail);
    }

    public static CopyRequest(requestID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Requests/CopyRequest', requestID, doNotHandleFail);
    }

    public static RetrieveBudgetInfoForRequests(ids: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestBudgetInfoDTO[]> {
        var params = '';
        if (ids != null)
            for (var j = 0; j < ids.length; j++) { params += '&ids=' + ids[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestBudgetInfoDTO[]>('Requests/RetrieveBudgetInfoForRequests' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Requests/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestDTO[]>('Requests/Get' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IRequestDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestDTO[]>('Requests/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IRequestDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IRequestDTO[]>('Requests/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IRequestDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestDTO[]>('Requests/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Requests/Delete' + params, doNotHandleFail);
    }

}
export class RequestTypes {
    public static Insert(values: Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestTypeDTO[]>('RequestTypes/Insert', values, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('RequestTypes/List' + params, doNotHandleFail);
    }

    public static ListAvailableRequestTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('RequestTypes/ListAvailableRequestTypes' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestTypeDTO[]>('RequestTypes/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IRequestTypeDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestTypeDTO[]>('RequestTypes/Update', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('RequestTypes/Delete' + params, doNotHandleFail);
    }

    public static Save(details: Interfaces.IUpdateRequestTypeRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('RequestTypes/Save', details, doNotHandleFail);
    }

    public static UpdateModels(details: Interfaces.IUpdateRequestTypeModelsDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('RequestTypes/UpdateModels', details, doNotHandleFail);
    }

    public static GetRequestTypeModels(requestTypeID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeModelDTO[]> {
        var params = '';
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeModelDTO[]>('RequestTypes/GetRequestTypeModels' + params, doNotHandleFail);
    }

    public static GetRequestTypeTerms(requestTypeID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeTermDTO[]> {
        var params = '';
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetRequestTypeTerms' + params, doNotHandleFail);
    }

    public static GetFilteredTerms(id: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeTermDTO[]> {
        var params = '';
        if (id != null) params += '&id=' + id;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetFilteredTerms' + params, doNotHandleFail);
    }

    public static GetTermsFilteredBy($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeTermDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeTermDTO[]>('RequestTypes/GetTermsFilteredBy' + params, doNotHandleFail);
    }

    public static TermsByAdapterAndDetail(details: Interfaces.IAvailableTermsRequestDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('RequestTypes/TermsByAdapterAndDetail', details, doNotHandleFail);
    }

    public static UpdateRequestTypeTerms(updateInfo: Interfaces.IUpdateRequestTypeTermsDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('RequestTypes/UpdateRequestTypeTerms', updateInfo, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('RequestTypes/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('RequestTypes/Get' + params, doNotHandleFail);
    }

}
export class Templates {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateDTO[]>('Templates/List' + params, doNotHandleFail);
    }

    public static CriteriaGroups($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateDTO[]>('Templates/CriteriaGroups' + params, doNotHandleFail);
    }

    public static GetByRequestType(requestTypeID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        var params = '';
        if (requestTypeID != null) params += '&requestTypeID=' + requestTypeID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateDTO[]>('Templates/GetByRequestType' + params, doNotHandleFail);
    }

    public static GetGlobalTemplatePermissions($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IHasGlobalSecurityForTemplateDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IHasGlobalSecurityForTemplateDTO[]>('Templates/GetGlobalTemplatePermissions' + params, doNotHandleFail);
    }

    public static SaveCriteriaGroup(details: Interfaces.ICreateCriteriaGroupTemplateDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITemplateDTO[]>('Templates/SaveCriteriaGroup', details, doNotHandleFail);
    }

    public static ListHiddenTerms(ID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateTermDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateTermDTO[]>('Templates/ListHiddenTerms' + params, doNotHandleFail);
    }

    public static ListHiddenTermsByRequestType(id: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateTermDTO[]> {
        var params = '';
        if (id != null) params += '&id=' + id;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateTermDTO[]>('Templates/ListHiddenTermsByRequestType' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Templates/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ITemplateDTO[]>('Templates/Get' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ITemplateDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITemplateDTO[]>('Templates/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ITemplateDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ITemplateDTO[]>('Templates/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ITemplateDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ITemplateDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ITemplateDTO[]>('Templates/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Templates/Delete' + params, doNotHandleFail);
    }

}
export class Notifications {
    public static ExecuteScheduledNotifications(userName: string, password: string, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (userName != null) params += '&userName=' + encodeURIComponent(userName);
        if (password != null) params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Notifications/ExecuteScheduledNotifications' + params, doNotHandleFail);
    }

}
export class DataMartInstalledModels {
    public static InsertOrUpdate(updateInfo: Interfaces.IUpdateDataMartInstalledModelsDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('DataMartInstalledModels/InsertOrUpdate', updateInfo, doNotHandleFail);
    }

    public static Remove(models: Interfaces.IDataMartInstalledModelDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('DataMartInstalledModels/Remove', models, doNotHandleFail);
    }

}
export class ProjectDataMarts {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDataMartDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDataMartDTO[]>('ProjectDataMarts/List' + params, doNotHandleFail);
    }

    public static ListWithRequestTypes($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDataMartWithRequestTypesDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDataMartWithRequestTypesDTO[]>('ProjectDataMarts/ListWithRequestTypes' + params, doNotHandleFail);
    }

    public static GetWithRequestTypes(projectID: any, dataMartID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDataMartWithRequestTypesDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (dataMartID != null) params += '&dataMartID=' + dataMartID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDataMartWithRequestTypesDTO[]>('ProjectDataMarts/GetWithRequestTypes' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(updateInfo: Interfaces.IProjectDataMartUpdateDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('ProjectDataMarts/InsertOrUpdate', updateInfo, doNotHandleFail);
    }

    public static Remove(dataMarts: Interfaces.IProjectDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('ProjectDataMarts/Remove', dataMarts, doNotHandleFail);
    }

}
export class ProjectOrganizations {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectOrganizationDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectOrganizationDTO[]>('ProjectOrganizations/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(updateInfo: Interfaces.IProjectOrganizationUpdateDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('ProjectOrganizations/InsertOrUpdate', updateInfo, doNotHandleFail);
    }

    public static Remove(organizations: Interfaces.IProjectOrganizationDTO[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('ProjectOrganizations/Remove', organizations, doNotHandleFail);
    }

}
export class Projects {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDTO[]>('Projects/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDTO[]>('Projects/List' + params, doNotHandleFail);
    }

    public static ProjectsWithRequests($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDTO[]>('Projects/ProjectsWithRequests' + params, doNotHandleFail);
    }

    public static GetActivityTreeByProjectID(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IActivityDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IActivityDTO[]>('Projects/GetActivityTreeByProjectID' + params, doNotHandleFail);
    }

    public static RequestableProjects($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectDTO[]>('Projects/RequestableProjects' + params, doNotHandleFail);
    }

    public static GetRequestTypes(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('Projects/GetRequestTypes' + params, doNotHandleFail);
    }

    public static GetRequestTypesByModel(projectID: any, dataModelID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (dataModelID != null) params += '&dataModelID=' + dataModelID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('Projects/GetRequestTypesByModel' + params, doNotHandleFail);
    }

    public static GetAvailableRequestTypeForNewRequest(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IRequestTypeDTO[]>('Projects/GetAvailableRequestTypeForNewRequest' + params, doNotHandleFail);
    }

    public static GetDataModelsByProject(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelWithRequestTypesDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataModelWithRequestTypesDTO[]>('Projects/GetDataModelsByProject' + params, doNotHandleFail);
    }

    public static GetProjectRequestTypes(projectID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectRequestTypeDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IProjectRequestTypeDTO[]>('Projects/GetProjectRequestTypes' + params, doNotHandleFail);
    }

    public static UpdateProjectRequestTypes(requestTypes: Interfaces.IUpdateProjectRequestTypesDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Projects/UpdateProjectRequestTypes', requestTypes, doNotHandleFail);
    }

    public static Copy(projectID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Projects/Copy' + params, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Projects/Delete' + params, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IProjectDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IProjectDTO[]>('Projects/Insert', values, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IProjectDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IProjectDTO[]>('Projects/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IProjectDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IProjectDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IProjectDTO[]>('Projects/Update', values, doNotHandleFail);
    }

    public static UpdateActivities(ID: any, username: string, password: string, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (username != null) params += '&username=' + encodeURIComponent(username);
        if (password != null) params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Projects/UpdateActivities' + params, doNotHandleFail);
    }

    public static GetFieldOptions(projectID: any, userID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IBaseFieldOptionAclDTO[]> {
        var params = '';
        if (projectID != null) params += '&projectID=' + projectID;
        if (userID != null) params += '&userID=' + userID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IBaseFieldOptionAclDTO[]>('Projects/GetFieldOptions' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Projects/GetPermissions' + params, doNotHandleFail);
    }

}
export class Documents {
    public static ByTask(tasks: any[], filterByTaskItemType: Enums.TaskItemTypes[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IExtendedDocumentDTO[]> {
        var params = '';
        if (tasks != null)
            for (var j = 0; j < tasks.length; j++) { params += '&tasks=' + tasks[j]; }
        if (filterByTaskItemType != null)
            for (var j = 0; j < filterByTaskItemType.length; j++) { params += '&filterByTaskItemType=' + filterByTaskItemType[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IExtendedDocumentDTO[]>('Documents/ByTask' + params, doNotHandleFail);
    }

    public static ByRevisionID(revisionSets: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IExtendedDocumentDTO[]> {
        var params = '';
        if (revisionSets != null)
            for (var j = 0; j < revisionSets.length; j++) { params += '&revisionSets=' + revisionSets[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IExtendedDocumentDTO[]>('Documents/ByRevisionID' + params, doNotHandleFail);
    }

    public static ByResponse(ID: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IExtendedDocumentDTO[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IExtendedDocumentDTO[]>('Documents/ByResponse' + params, doNotHandleFail);
    }

    public static GeneralRequestDocuments(requestID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IExtendedDocumentDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IExtendedDocumentDTO[]>('Documents/GeneralRequestDocuments' + params, doNotHandleFail);
    }

    public static Read(id: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null) params += '&id=' + id;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Documents/Read' + params, doNotHandleFail);
    }

    public static Download(id: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null) params += '&id=' + id;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Documents/Download' + params, doNotHandleFail);
    }

    public static Upload(doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Documents/Upload', doNotHandleFail);
    }

    public static UploadChunked(doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('Documents/UploadChunked', doNotHandleFail);
    }

    public static Delete(id: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) { params += '&id=' + id[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('Documents/Delete' + params, doNotHandleFail);
    }

}
export class DataMartAvailability {
    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartAvailabilityPeriodV2DTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartAvailabilityPeriodV2DTO[]>('DataMartAvailability/List' + params, doNotHandleFail);
    }

}
export class DataModels {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataModelDTO[]>('DataModels/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataModelDTO[]>('DataModels/List' + params, doNotHandleFail);
    }

    public static ListDataModelProcessors($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelProcessorDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataModelProcessorDTO[]>('DataModels/ListDataModelProcessors' + params, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('DataModels/GetPermissions' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IDataModelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataModelDTO[]>('DataModels/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IDataModelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IDataModelDTO[]>('DataModels/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IDataModelDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataModelDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataModelDTO[]>('DataModels/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('DataModels/Delete' + params, doNotHandleFail);
    }

}
export class DataMarts {
    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartDTO[]>('DataMarts/Get' + params, doNotHandleFail);
    }

    public static GetByRoute(requestDataMartID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        var params = '';
        if (requestDataMartID != null) params += '&requestDataMartID=' + requestDataMartID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartDTO[]>('DataMarts/GetByRoute' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartDTO[]>('DataMarts/List' + params, doNotHandleFail);
    }

    public static ListBasic($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartListDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartListDTO[]>('DataMarts/ListBasic' + params, doNotHandleFail);
    }

    public static DataMartTypeList($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartTypeDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartTypeDTO[]>('DataMarts/DataMartTypeList' + params, doNotHandleFail);
    }

    public static GetRequestTypesByDataMarts(DataMartId: any[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IRequestTypeDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IRequestTypeDTO[]>('DataMarts/GetRequestTypesByDataMarts', DataMartId, doNotHandleFail);
    }

    public static GetInstalledModelsByDataMart(DataMartId: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartInstalledModelDTO[]> {
        var params = '';
        if (DataMartId != null) params += '&DataMartId=' + DataMartId;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IDataMartInstalledModelDTO[]>('DataMarts/GetInstalledModelsByDataMart' + params, doNotHandleFail);
    }

    public static UninstallModel(model: Interfaces.IDataMartInstalledModelDTO, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        return Helpers.PostAPIValue<any[]>('DataMarts/UninstallModel', model, doNotHandleFail);
    }

    public static Copy(datamartID: any, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (datamartID != null) params += '&datamartID=' + datamartID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('DataMarts/Copy' + params, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.DeleteAPIValue<any[]>('DataMarts/Delete' + params, doNotHandleFail);
    }

    public static Insert(values: Interfaces.IDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataMartDTO[]>('DataMarts/Insert', values, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.IDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IDataMartDTO[]>('DataMarts/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.IDataMartDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IDataMartDTO[]> {
        return Helpers.PutAPIValue<Interfaces.IDataMartDTO[]>('DataMarts/Update', values, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('DataMarts/GetPermissions' + params, doNotHandleFail);
    }

}
export class Comments {
    public static ByRequestID(requestID: any, workflowActivityID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWFCommentDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWFCommentDTO[]>('Comments/ByRequestID' + params, doNotHandleFail);
    }

    public static ByDocumentID(documentID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWFCommentDTO[]> {
        var params = '';
        if (documentID != null) params += '&documentID=' + documentID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.IWFCommentDTO[]>('Comments/ByDocumentID' + params, doNotHandleFail);
    }

    public static GetDocumentReferencesByRequest(requestID: any, workflowActivityID: any, $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDocumentReferenceDTO[]> {
        var params = '';
        if (requestID != null) params += '&requestID=' + requestID;
        if (workflowActivityID != null) params += '&workflowActivityID=' + workflowActivityID;
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ICommentDocumentReferenceDTO[]>('Comments/GetDocumentReferencesByRequest' + params, doNotHandleFail);
    }

    public static AddWorkflowComment(value: Interfaces.IAddWFCommentDTO, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.IWFCommentDTO[]> {
        return Helpers.PostAPIValue<Interfaces.IWFCommentDTO[]>('Comments/AddWorkflowComment', value, doNotHandleFail);
    }

    public static GetPermissions(IDs: any[], permissions: any[], $filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) { params += '&IDs=' + IDs[j]; }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) { params += '&permissions=' + permissions[j]; }
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<any[]>('Comments/GetPermissions' + params, doNotHandleFail);
    }

    public static Get(ID: any, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDTO[]> {
        var params = '';
        if (ID != null) params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ICommentDTO[]>('Comments/Get' + params, doNotHandleFail);
    }

    public static List($filter?: string, $select?: string, $orderby?: string, $skip?: number, $top?: number, doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDTO[]> {
        var params = '';
        if ($filter) params += '&$filter=' + $filter;
        if ($select) params += '&$select=' + $select;
        if ($orderby) params += '&$orderby=' + $orderby;
        if ($skip) params += '&$skip=' + $skip;
        if ($top) params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);

        return Helpers.GetAPIResult<Interfaces.ICommentDTO[]>('Comments/List' + params, doNotHandleFail);
    }

    public static InsertOrUpdate(values: Interfaces.ICommentDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ICommentDTO[]>('Comments/InsertOrUpdate', values, doNotHandleFail);
    }

    public static Update(values: Interfaces.ICommentDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDTO[]> {
        return Helpers.PutAPIValue<Interfaces.ICommentDTO[]>('Comments/Update', values, doNotHandleFail);
    }

    public static Insert(values: Interfaces.ICommentDTO[], doNotHandleFail?: boolean): JQueryDeferred<Interfaces.ICommentDTO[]> {
        return Helpers.PostAPIValue<Interfaces.ICommentDTO[]>('Comments/Insert', values, doNotHandleFail);
    }

    public static Delete(ID: any[], doNotHandleFail?: boolean): JQueryDeferred<any[]> {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) { params += '&ID=' + ID[j]; }
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
        if (Array.isArray(results)) {
            results.forEach((data) => {
                this.FixStringDatesInResult(data);
            });
        } else {
            this.FixStringDatesInResult(results);
        }
    }

    static FixStringDatesInResult(data) {
        for (var field in data) {
            if (data[field]) {
                if ($.isArray(data[field])) {
                    this.FixStringDatesInResults(data[field]);
                } else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}\+{1}\d{2}:\d{2}$/g)) {
                    //this is to handle datetimeoffset serialization, it has the timezone offset included and does not need the 'Z' appended.
                    data[field] = new Date(data[field]);
                } else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                    //original datetime conversion that assumes the date is UTC.
                    if (data[field].indexOf('Z') > -1) {
                        data[field] = new Date(data[field]);
                    } else {
                        data[field] = new Date(data[field] + 'Z');
                    }
                }
            }
        }
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

            if (result == null || (result.results != undefined && result.results == null)) {
                d.resolve();
                return;
            }

            if (result.Results == null) {
                this.FixStringDatesInResults(result);
                d.resolve(result);
                return;
            }

            let results = result.Results;
            this.FixStringDatesInResults(results);

            d.resolve(results);
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

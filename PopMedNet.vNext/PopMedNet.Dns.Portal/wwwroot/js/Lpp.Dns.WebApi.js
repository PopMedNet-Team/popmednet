export class Workflow {
    static GetWorkflowEntryPointByRequestTypeID(requestTypeID, doNotHandleFail) {
        var params = '';
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/GetWorkflowEntryPointByRequestTypeID' + params, doNotHandleFail);
    }
    static GetWorkflowActivity(workflowActivityID, doNotHandleFail) {
        var params = '';
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/GetWorkflowActivity' + params, doNotHandleFail);
    }
    static GetWorkflowActivitiesByWorkflowID(workFlowID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (workFlowID != null)
            params += '&workFlowID=' + workFlowID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/GetWorkflowActivitiesByWorkflowID' + params, doNotHandleFail);
    }
    static GetWorkflowRolesByWorkflowID(workflowID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (workflowID != null)
            params += '&workflowID=' + workflowID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/GetWorkflowRolesByWorkflowID' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Workflow/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Workflow/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Workflow/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Workflow/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Workflow/Delete' + params, doNotHandleFail);
    }
}
export class Wbd {
    static ApproveRequest(requestID, doNotHandleFail) {
        return Helpers.PutAPIValue('Wbd/ApproveRequest', requestID, doNotHandleFail);
    }
    static RejectRequest(requestID, doNotHandleFail) {
        return Helpers.PutAPIValue('Wbd/RejectRequest', requestID, doNotHandleFail);
    }
    static GetRequestByID(Id, doNotHandleFail) {
        var params = '';
        if (Id != null)
            params += '&Id=' + Id;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Wbd/GetRequestByID' + params, doNotHandleFail);
    }
    static SaveRequest(request, doNotHandleFail) {
        return Helpers.PostAPIValue('Wbd/SaveRequest', request, doNotHandleFail);
    }
    static GetActivityTreeByProjectID(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Wbd/GetActivityTreeByProjectID' + params, doNotHandleFail);
    }
    static Register(registration, doNotHandleFail) {
        return Helpers.PostAPIValue('Wbd/Register', registration, doNotHandleFail);
    }
    static GetChanges(criteria, doNotHandleFail) {
        return Helpers.PostAPIValue('Wbd/GetChanges', criteria, doNotHandleFail);
    }
    static DownloadDocument(documentId, doNotHandleFail) {
        var params = '';
        if (documentId != null)
            params += '&documentId=' + documentId;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Wbd/DownloadDocument' + params, doNotHandleFail);
    }
    static DownloadRequestViewableFile(requestId, doNotHandleFail) {
        var params = '';
        if (requestId != null)
            params += '&requestId=' + requestId;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Wbd/DownloadRequestViewableFile' + params, doNotHandleFail);
    }
    static CopyRequest(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Wbd/CopyRequest' + params, doNotHandleFail);
    }
    static UpdateResponseStatus(details, doNotHandleFail) {
        return Helpers.PostAPIValue('Wbd/UpdateResponseStatus', details, doNotHandleFail);
    }
}
export class SsoEndpoints {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SsoEndpoints/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SsoEndpoints/List' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SsoEndpoints/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('SsoEndpoints/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('SsoEndpoints/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('SsoEndpoints/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('SsoEndpoints/Delete' + params, doNotHandleFail);
    }
}
export class Users {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/List' + params, doNotHandleFail);
    }
    static ValidateLogin(login, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/ValidateLogin', login, doNotHandleFail);
    }
    static ByUserName(userName, doNotHandleFail) {
        var params = '';
        if (userName != null)
            params += '&userName=' + encodeURIComponent(userName);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ByUserName' + params, doNotHandleFail);
    }
    static UserRegistration(data, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/UserRegistration', data, doNotHandleFail);
    }
    static ListAvailableProjects($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ListAvailableProjects' + params, doNotHandleFail);
    }
    static ForgotPassword(data, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/ForgotPassword', data, doNotHandleFail);
    }
    static ChangePassword(updateInfo, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/ChangePassword', updateInfo, doNotHandleFail);
    }
    static RestorePassword(updateInfo, doNotHandleFail) {
        return Helpers.PutAPIValue('Users/RestorePassword', updateInfo, doNotHandleFail);
    }
    static GetAssignedNotifications(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetAssignedNotifications' + params, doNotHandleFail);
    }
    static GetSubscribableEvents(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetSubscribableEvents' + params, doNotHandleFail);
    }
    static GetSubscribedEvents(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetSubscribedEvents' + params, doNotHandleFail);
    }
    static GetNotifications(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetNotifications' + params, doNotHandleFail);
    }
    static ListAuthenticationAudits($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ListAuthenticationAudits' + params, doNotHandleFail);
    }
    static ListDistinctEnvironments(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ListDistinctEnvironments' + params, doNotHandleFail);
    }
    static UpdateSubscribedEvents(subscribedEvents, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/UpdateSubscribedEvents', subscribedEvents, doNotHandleFail);
    }
    static GetTasks(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetTasks' + params, doNotHandleFail);
    }
    static GetWorkflowTasks(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetWorkflowTasks' + params, doNotHandleFail);
    }
    static GetWorkflowTaskUsers(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetWorkflowTaskUsers' + params, doNotHandleFail);
    }
    static Logout(doNotHandleFail) {
        return Helpers.PostAPIValue('Users/Logout', doNotHandleFail);
    }
    static MemberOfSecurityGroups(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/MemberOfSecurityGroups' + params, doNotHandleFail);
    }
    static UpdateSecurityGroups(groups, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/UpdateSecurityGroups', groups, doNotHandleFail);
    }
    static GetGlobalPermission(permissionID, doNotHandleFail) {
        var params = '';
        if (permissionID != null)
            params += '&permissionID=' + permissionID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetGlobalPermission' + params, doNotHandleFail);
    }
    static ReturnMainMenu($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ReturnMainMenu' + params, doNotHandleFail);
    }
    static UpdateLookupListsTest(username, password, doNotHandleFail) {
        var params = '';
        if (username != null)
            params += '&username=' + encodeURIComponent(username);
        if (password != null)
            params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/UpdateLookupListsTest' + params, doNotHandleFail);
    }
    static UpdateLookupLists(username, password, doNotHandleFail) {
        var params = '';
        if (username != null)
            params += '&username=' + encodeURIComponent(username);
        if (password != null)
            params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/UpdateLookupLists' + params, doNotHandleFail);
    }
    static SaveSetting(setting, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/SaveSetting', setting, doNotHandleFail);
    }
    static GetSetting(key, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (key != null)
            for (var j = 0; j < key.length; j++) {
                params += '&key=' + encodeURIComponent(key[j]);
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetSetting' + params, doNotHandleFail);
    }
    static AllowApproveRejectRequest(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/AllowApproveRejectRequest' + params, doNotHandleFail);
    }
    static HasPassword(userID, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/HasPassword' + params, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Users/Delete' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Users/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Users/Insert', values, doNotHandleFail);
    }
    static GetMetadataEditPermissionsSummary(doNotHandleFail) {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetMetadataEditPermissionsSummary' + params, doNotHandleFail);
    }
    static ExpireAllUserPasswords(doNotHandleFail) {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/ExpireAllUserPasswords' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Users/GetPermissions' + params, doNotHandleFail);
    }
}
export class Theme {
    static GetText(keys, doNotHandleFail) {
        var params = '';
        if (keys != null)
            for (var j = 0; j < keys.length; j++) {
                params += '&keys=' + encodeURIComponent(keys[j]);
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Theme/GetText' + params, doNotHandleFail);
    }
    static GetImagePath(doNotHandleFail) {
        var params = '';
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Theme/GetImagePath' + params, doNotHandleFail);
    }
}
export class Terms {
    static ListTemplateTerms(id, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (id != null)
            params += '&id=' + id;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Terms/ListTemplateTerms' + params, doNotHandleFail);
    }
    static ParseCodeList(doNotHandleFail) {
        return Helpers.PostAPIValue('Terms/ParseCodeList', doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Terms/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Terms/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Terms/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Terms/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Terms/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Terms/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Terms/Delete' + params, doNotHandleFail);
    }
}
export class Security {
    static ListSecurityEntities($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/ListSecurityEntities' + params, doNotHandleFail);
    }
    static GetPermissionsByLocation(locations, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/GetPermissionsByLocation', locations, doNotHandleFail);
    }
    static GetDataMartPermissions(dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetDataMartPermissions' + params, doNotHandleFail);
    }
    static GetOrganizationPermissions(organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (organizationID != null)
            params += '&organizationID=' + organizationID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetOrganizationPermissions' + params, doNotHandleFail);
    }
    static GetUserPermissions(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetUserPermissions' + params, doNotHandleFail);
    }
    static GetGroupPermissions(groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (groupID != null)
            params += '&groupID=' + groupID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetGroupPermissions' + params, doNotHandleFail);
    }
    static GetRegistryPermissions(registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (registryID != null)
            params += '&registryID=' + registryID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetRegistryPermissions' + params, doNotHandleFail);
    }
    static GetProjectPermissions(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectPermissions' + params, doNotHandleFail);
    }
    static GetGlobalPermissions($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetGlobalPermissions' + params, doNotHandleFail);
    }
    static GetProjectOrganizationPermissions(projectID, organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (organizationID != null)
            params += '&organizationID=' + organizationID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectOrganizationPermissions' + params, doNotHandleFail);
    }
    static GetProjectRequestTypeWorkflowActivityPermissions(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectRequestTypeWorkflowActivityPermissions' + params, doNotHandleFail);
    }
    static GetWorkflowActivityPermissionsForIdentity(projectID, workflowActivityID, requestTypeID, permissionID, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if (permissionID != null)
            for (var j = 0; j < permissionID.length; j++) {
                params += '&permissionID=' + permissionID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetWorkflowActivityPermissionsForIdentity' + params, doNotHandleFail);
    }
    static GetProjectDataMartPermissions(projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectDataMartPermissions' + params, doNotHandleFail);
    }
    static GetProjectDataMartRequestTypePermissions(projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectDataMartRequestTypePermissions' + params, doNotHandleFail);
    }
    static GetProjectRequestTypePermissions(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectRequestTypePermissions' + params, doNotHandleFail);
    }
    static GetDataMartRequestTypePermissions(dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetDataMartRequestTypePermissions' + params, doNotHandleFail);
    }
    static GetTemplatePermissions(templateID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (templateID != null)
            params += '&templateID=' + templateID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetTemplatePermissions' + params, doNotHandleFail);
    }
    static GetRequestTypePermissions(requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetRequestTypePermissions' + params, doNotHandleFail);
    }
    static UpdateProjectRequestTypeWorkflowActivityPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectRequestTypeWorkflowActivityPermissions', permissions, doNotHandleFail);
    }
    static UpdateDataMartRequestTypePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateDataMartRequestTypePermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectDataMartPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectDataMartPermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectDataMartRequestTypePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectDataMartRequestTypePermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectOrganizationPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectOrganizationPermissions', permissions, doNotHandleFail);
    }
    static UpdatePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdatePermissions', permissions, doNotHandleFail);
    }
    static UpdateGroupPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateGroupPermissions', permissions, doNotHandleFail);
    }
    static UpdateRegistryPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateRegistryPermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectPermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectRequestTypePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectRequestTypePermissions', permissions, doNotHandleFail);
    }
    static UpdateDataMartPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateDataMartPermissions', permissions, doNotHandleFail);
    }
    static UpdateOrganizationPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateOrganizationPermissions', permissions, doNotHandleFail);
    }
    static UpdateUserPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateUserPermissions', permissions, doNotHandleFail);
    }
    static GetAvailableSecurityGroupTree($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetAvailableSecurityGroupTree' + params, doNotHandleFail);
    }
    static UpdateTemplatePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateTemplatePermissions', permissions, doNotHandleFail);
    }
    static UpdateRequestTypePermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateRequestTypePermissions', permissions, doNotHandleFail);
    }
    static GetGlobalFieldOptionPermissions($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetGlobalFieldOptionPermissions' + params, doNotHandleFail);
    }
    static UpdateFieldOptionPermissions(fieldOptionUpdates, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateFieldOptionPermissions', fieldOptionUpdates, doNotHandleFail);
    }
    static GetProjectFieldOptionPermissions(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Security/GetProjectFieldOptionPermissions' + params, doNotHandleFail);
    }
    static UpdateProjectFieldOptionPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Security/UpdateProjectFieldOptionPermissions', permissions, doNotHandleFail);
    }
}
export class SecurityGroups {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SecurityGroups/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SecurityGroups/List' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('SecurityGroups/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('SecurityGroups/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('SecurityGroups/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('SecurityGroups/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('SecurityGroups/Delete' + params, doNotHandleFail);
    }
}
export class Organizations {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/List' + params, doNotHandleFail);
    }
    static ListByGroupMembership(groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (groupID != null)
            params += '&groupID=' + groupID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/ListByGroupMembership' + params, doNotHandleFail);
    }
    static Copy(organizationID, doNotHandleFail) {
        var params = '';
        if (organizationID != null)
            params += '&organizationID=' + organizationID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/Copy' + params, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Organizations/Delete' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Organizations/InsertOrUpdate', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Organizations/Insert', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Organizations/Update', values, doNotHandleFail);
    }
    static EHRSInsertOrUpdate(updates, doNotHandleFail) {
        return Helpers.PostAPIValue('Organizations/EHRSInsertOrUpdate', updates, doNotHandleFail);
    }
    static ListEHRS($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/ListEHRS' + params, doNotHandleFail);
    }
    static DeleteEHRS(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Organizations/DeleteEHRS' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Organizations/GetPermissions' + params, doNotHandleFail);
    }
}
export class OrganizationRegistries {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('OrganizationRegistries/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(organizations, doNotHandleFail) {
        return Helpers.PostAPIValue('OrganizationRegistries/InsertOrUpdate', organizations, doNotHandleFail);
    }
    static Remove(organizations, doNotHandleFail) {
        return Helpers.PostAPIValue('OrganizationRegistries/Remove', organizations, doNotHandleFail);
    }
}
export class Registries {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Registries/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Registries/List' + params, doNotHandleFail);
    }
    static GetRegistryItemDefinitionList(registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (registryID != null)
            params += '&registryID=' + registryID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Registries/GetRegistryItemDefinitionList' + params, doNotHandleFail);
    }
    static UpdateRegistryItemDefinitions(updateParams, doNotHandleFail) {
        return Helpers.PutAPIValue('Registries/UpdateRegistryItemDefinitions', updateParams, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Registries/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Registries/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Registries/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Registries/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Registries/Delete' + params, doNotHandleFail);
    }
}
export class RegistryItemDefinition {
    static GetList($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RegistryItemDefinition/GetList' + params, doNotHandleFail);
    }
}
export class NetworkMessages {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('NetworkMessages/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('NetworkMessages/List' + params, doNotHandleFail);
    }
    static ListLastDays(days, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (days != null)
            params += '&days=' + days;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('NetworkMessages/ListLastDays' + params, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('NetworkMessages/Insert', values, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('NetworkMessages/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('NetworkMessages/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('NetworkMessages/Update', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('NetworkMessages/Delete' + params, doNotHandleFail);
    }
}
export class LookupListCategory {
    static GetList(listID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (listID != null)
            params += '&listID=' + listID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('LookupListCategory/GetList' + params, doNotHandleFail);
    }
}
export class LookupList {
    static GetList($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('LookupList/GetList' + params, doNotHandleFail);
    }
}
export class LookupListValue {
    static GetList($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('LookupListValue/GetList' + params, doNotHandleFail);
    }
    static GetCodeDetailsByCode(details, doNotHandleFail) {
        return Helpers.PostAPIValue('LookupListValue/GetCodeDetailsByCode', details, doNotHandleFail);
    }
    static LookupList(listID, lookup, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (listID != null)
            params += '&listID=' + listID;
        if (lookup != null)
            params += '&lookup=' + encodeURIComponent(lookup);
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('LookupListValue/LookupList' + params, doNotHandleFail);
    }
}
export class Groups {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Groups/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Groups/List' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Groups/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Groups/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Groups/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Groups/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Groups/Delete' + params, doNotHandleFail);
    }
}
export class OrganizationGroups {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('OrganizationGroups/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(organizations, doNotHandleFail) {
        return Helpers.PostAPIValue('OrganizationGroups/InsertOrUpdate', organizations, doNotHandleFail);
    }
    static Remove(organizations, doNotHandleFail) {
        return Helpers.PostAPIValue('OrganizationGroups/Remove', organizations, doNotHandleFail);
    }
}
export class Events {
    static GetEventsByLocation(locations, doNotHandleFail) {
        var params = null;
        if (locations != null)
            for (var j = 0; j < locations.length; j++) {
                params += '&locations=' + locations[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetEventsByLocation' + params, doNotHandleFail);
    }
    static GetGroupEventPermissions(groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (groupID != null)
            params += '&groupID=' + groupID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetGroupEventPermissions' + params, doNotHandleFail);
    }
    static GetRegistryEventPermissions(registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (registryID != null)
            params += '&registryID=' + registryID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetRegistryEventPermissions' + params, doNotHandleFail);
    }
    static GetProjectEventPermissions(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetProjectEventPermissions' + params, doNotHandleFail);
    }
    static GetOrganizationEventPermissions(organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (organizationID != null)
            params += '&organizationID=' + organizationID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetOrganizationEventPermissions' + params, doNotHandleFail);
    }
    static GetUserEventPermissions(userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetUserEventPermissions' + params, doNotHandleFail);
    }
    static GetGlobalEventPermissions($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetGlobalEventPermissions' + params, doNotHandleFail);
    }
    static UpdateGroupEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateGroupEventPermissions', permissions, doNotHandleFail);
    }
    static UpdateRegistryEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateRegistryEventPermissions', permissions, doNotHandleFail);
    }
    static UpdateProjectEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateProjectEventPermissions', permissions, doNotHandleFail);
    }
    static UpdateOrganizationEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateOrganizationEventPermissions', permissions, doNotHandleFail);
    }
    static UpdateUserEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateUserEventPermissions', permissions, doNotHandleFail);
    }
    static UpdateGlobalEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateGlobalEventPermissions', permissions, doNotHandleFail);
    }
    static GetProjectOrganizationEventPermissions(projectID, organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (organizationID != null)
            params += '&organizationID=' + organizationID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetProjectOrganizationEventPermissions' + params, doNotHandleFail);
    }
    static UpdateProjectOrganizationEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateProjectOrganizationEventPermissions', permissions, doNotHandleFail);
    }
    static GetProjectDataMartEventPermissions(projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetProjectDataMartEventPermissions' + params, doNotHandleFail);
    }
    static UpdateProjectDataMartEventPermissions(permissions, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/UpdateProjectDataMartEventPermissions', permissions, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Events/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Events/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Events/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Events/Delete' + params, doNotHandleFail);
    }
}
export class Tasks {
    static ByRequestID(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Tasks/ByRequestID' + params, doNotHandleFail);
    }
    static GetWorkflowActivityDataForRequest(requestID, workflowActivityID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Tasks/GetWorkflowActivityDataForRequest' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Tasks/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Tasks/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Tasks/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Tasks/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Tasks/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Tasks/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Tasks/Delete' + params, doNotHandleFail);
    }
}
export class LegacyRequests {
    static ScheduleLegacyRequest(dto, doNotHandleFail) {
        return Helpers.PostAPIValue('LegacyRequests/ScheduleLegacyRequest', dto, doNotHandleFail);
    }
    static DeleteRequestSchedules(requestID, doNotHandleFail) {
        return Helpers.PostAPIValue('LegacyRequests/DeleteRequestSchedules', requestID, doNotHandleFail);
    }
}
export class ReportAggregationLevel {
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('ReportAggregationLevel/Delete' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ReportAggregationLevel/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ReportAggregationLevel/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ReportAggregationLevel/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('ReportAggregationLevel/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('ReportAggregationLevel/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('ReportAggregationLevel/Insert', values, doNotHandleFail);
    }
}
export class RequestObservers {
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestObservers/Insert', values, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestObservers/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestObservers/Update', values, doNotHandleFail);
    }
    static ListRequestObservers(RequestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (RequestID != null)
            params += '&RequestID=' + RequestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/ListRequestObservers' + params, doNotHandleFail);
    }
    static LookupObserverEvents($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/LookupObserverEvents' + params, doNotHandleFail);
    }
    static LookupObservers($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/LookupObservers' + params, doNotHandleFail);
    }
    static ValidateInsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestObservers/ValidateInsertOrUpdate', values, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestObservers/List' + params, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('RequestObservers/Delete' + params, doNotHandleFail);
    }
}
export class RequestUsers {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestUsers/List' + params, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestUsers/Insert', values, doNotHandleFail);
    }
    static Delete(requestUsers, doNotHandleFail) {
        var params = '';
        if (requestUsers != null)
            for (var j = 0; j < requestUsers.length; j++) {
                params += '&requestUsers=' + requestUsers[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('RequestUsers/Delete' + params, doNotHandleFail);
    }
}
export class Response {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/Get' + params, doNotHandleFail);
    }
    static ApproveResponses(responses, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/ApproveResponses', responses, doNotHandleFail);
    }
    static RejectResponses(responses, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/RejectResponses', responses, doNotHandleFail);
    }
    static RejectAndReSubmitResponses(responses, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/RejectAndReSubmitResponses', responses, doNotHandleFail);
    }
    static GetByWorkflowActivity(requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetByWorkflowActivity' + params, doNotHandleFail);
    }
    static CanViewIndividualResponses(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/CanViewIndividualResponses' + params, doNotHandleFail);
    }
    static CanViewAggregateResponses(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/CanViewAggregateResponses' + params, doNotHandleFail);
    }
    static CanViewPendingApprovalResponses(responses, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/CanViewPendingApprovalResponses', responses, doNotHandleFail);
    }
    static GetForWorkflowRequest(requestID, viewDocuments, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (viewDocuments != null)
            params += '&viewDocuments=' + viewDocuments;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetForWorkflowRequest' + params, doNotHandleFail);
    }
    static GetDetails(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetDetails' + params, doNotHandleFail);
    }
    static GetWorkflowResponseContent(id, view, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (view != null)
            params += '&view=' + view;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetWorkflowResponseContent' + params, doNotHandleFail);
    }
    static GetResponseGroups(responseIDs, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (responseIDs != null)
            for (var j = 0; j < responseIDs.length; j++) {
                params += '&responseIDs=' + responseIDs[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetResponseGroups' + params, doNotHandleFail);
    }
    static GetResponseGroupsByRequestID(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetResponseGroupsByRequestID' + params, doNotHandleFail);
    }
    static Export(id, view, format, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (view != null)
            params += '&view=' + view;
        if (format != null)
            params += '&format=' + encodeURIComponent(format);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/Export' + params, doNotHandleFail);
    }
    static ExportAllAsZip(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/ExportAllAsZip' + params, doNotHandleFail);
    }
    static GetTrackingTableForAnalysisCenter(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetTrackingTableForAnalysisCenter' + params, doNotHandleFail);
    }
    static GetTrackingTableForDataPartners(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetTrackingTableForDataPartners' + params, doNotHandleFail);
    }
    static GetEnhancedEventLog(requestID, format, download, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (format != null)
            params += '&format=' + encodeURIComponent(format);
        if (download != null)
            params += '&download=' + download;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetEnhancedEventLog' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/GetPermissions' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Response/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Response/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Response/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Response/Delete' + params, doNotHandleFail);
    }
}
export class Requests {
    static CompleteActivity(request, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/CompleteActivity', request, doNotHandleFail);
    }
    static TerminateRequest(requestID, doNotHandleFail) {
        return Helpers.PutAPIValue('Requests/TerminateRequest', requestID, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/List' + params, doNotHandleFail);
    }
    static ListForHomepage($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/ListForHomepage' + params, doNotHandleFail);
    }
    static RequestsByRoute($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/RequestsByRoute' + params, doNotHandleFail);
    }
    static GetCompatibleDataMarts(requestDetails, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/GetCompatibleDataMarts', requestDetails, doNotHandleFail);
    }
    static GetDataMartsForInstalledModels(requestDetails, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/GetDataMartsForInstalledModels', requestDetails, doNotHandleFail);
    }
    static RequestDataMarts(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/RequestDataMarts' + params, doNotHandleFail);
    }
    static GetOverrideableRequestDataMarts(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetOverrideableRequestDataMarts' + params, doNotHandleFail);
    }
    static UpdateRequestDataMarts(dataMarts, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/UpdateRequestDataMarts', dataMarts, doNotHandleFail);
    }
    static UpdateRequestDataMartsMetadata(dataMarts, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/UpdateRequestDataMartsMetadata', dataMarts, doNotHandleFail);
    }
    static ListRequesterCenters($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/ListRequesterCenters' + params, doNotHandleFail);
    }
    static ListWorkPlanTypes($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/ListWorkPlanTypes' + params, doNotHandleFail);
    }
    static ListReportAggregationLevels($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/ListReportAggregationLevels' + params, doNotHandleFail);
    }
    static GetWorkflowHistory(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetWorkflowHistory' + params, doNotHandleFail);
    }
    static GetResponseHistory(requestDataMartID, requestID, doNotHandleFail) {
        var params = '';
        if (requestDataMartID != null)
            params += '&requestDataMartID=' + requestDataMartID;
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetResponseHistory' + params, doNotHandleFail);
    }
    static GetRequestSearchTerms(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetRequestSearchTerms' + params, doNotHandleFail);
    }
    static GetRequestTypeModels(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetRequestTypeModels' + params, doNotHandleFail);
    }
    static GetModelIDsforRequest(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetModelIDsforRequest' + params, doNotHandleFail);
    }
    static UpdateRequestMetadata(reqMetadata, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/UpdateRequestMetadata', reqMetadata, doNotHandleFail);
    }
    static UpdateMetadataForRequests(updates, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/UpdateMetadataForRequests', updates, doNotHandleFail);
    }
    static GetOrganizationsForRequest(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetOrganizationsForRequest' + params, doNotHandleFail);
    }
    static AllowCopyRequest(requestID, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/AllowCopyRequest' + params, doNotHandleFail);
    }
    static CopyRequest(requestID, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/CopyRequest', requestID, doNotHandleFail);
    }
    static RetrieveBudgetInfoForRequests(ids, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (ids != null)
            for (var j = 0; j < ids.length; j++) {
                params += '&ids=' + ids[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/RetrieveBudgetInfoForRequests' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Requests/Get' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Requests/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Requests/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Requests/Delete' + params, doNotHandleFail);
    }
}
export class RequestTypes {
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/Insert', values, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/List' + params, doNotHandleFail);
    }
    static ListAvailableRequestTypes($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/ListAvailableRequestTypes' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/Update', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('RequestTypes/Delete' + params, doNotHandleFail);
    }
    static Save(details, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/Save', details, doNotHandleFail);
    }
    static UpdateModels(details, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/UpdateModels', details, doNotHandleFail);
    }
    static GetRequestTypeModels(requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/GetRequestTypeModels' + params, doNotHandleFail);
    }
    static GetRequestTypeTerms(requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/GetRequestTypeTerms' + params, doNotHandleFail);
    }
    static GetFilteredTerms(id, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (id != null)
            params += '&id=' + id;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/GetFilteredTerms' + params, doNotHandleFail);
    }
    static GetTermsFilteredBy($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/GetTermsFilteredBy' + params, doNotHandleFail);
    }
    static TermsByAdapterAndDetail(details, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/TermsByAdapterAndDetail', details, doNotHandleFail);
    }
    static UpdateRequestTypeTerms(updateInfo, doNotHandleFail) {
        return Helpers.PostAPIValue('RequestTypes/UpdateRequestTypeTerms', updateInfo, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('RequestTypes/Get' + params, doNotHandleFail);
    }
}
export class Templates {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/List' + params, doNotHandleFail);
    }
    static CriteriaGroups($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/CriteriaGroups' + params, doNotHandleFail);
    }
    static GetByRequestType(requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestTypeID != null)
            params += '&requestTypeID=' + requestTypeID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/GetByRequestType' + params, doNotHandleFail);
    }
    static GetGlobalTemplatePermissions($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/GetGlobalTemplatePermissions' + params, doNotHandleFail);
    }
    static SaveCriteriaGroup(details, doNotHandleFail) {
        return Helpers.PostAPIValue('Templates/SaveCriteriaGroup', details, doNotHandleFail);
    }
    static ListHiddenTerms(ID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/ListHiddenTerms' + params, doNotHandleFail);
    }
    static ListHiddenTermsByRequestType(id, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (id != null)
            params += '&id=' + id;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/ListHiddenTermsByRequestType' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Templates/Get' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Templates/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Templates/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Templates/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Templates/Delete' + params, doNotHandleFail);
    }
}
export class Notifications {
    static ExecuteScheduledNotifications(userName, password, doNotHandleFail) {
        var params = '';
        if (userName != null)
            params += '&userName=' + encodeURIComponent(userName);
        if (password != null)
            params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Notifications/ExecuteScheduledNotifications' + params, doNotHandleFail);
    }
}
export class DataMartInstalledModels {
    static InsertOrUpdate(updateInfo, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMartInstalledModels/InsertOrUpdate', updateInfo, doNotHandleFail);
    }
    static Remove(models, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMartInstalledModels/Remove', models, doNotHandleFail);
    }
}
export class ProjectDataMarts {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ProjectDataMarts/List' + params, doNotHandleFail);
    }
    static ListWithRequestTypes($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ProjectDataMarts/ListWithRequestTypes' + params, doNotHandleFail);
    }
    static GetWithRequestTypes(projectID, dataMartID, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (dataMartID != null)
            params += '&dataMartID=' + dataMartID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ProjectDataMarts/GetWithRequestTypes' + params, doNotHandleFail);
    }
    static InsertOrUpdate(updateInfo, doNotHandleFail) {
        return Helpers.PostAPIValue('ProjectDataMarts/InsertOrUpdate', updateInfo, doNotHandleFail);
    }
    static Remove(dataMarts, doNotHandleFail) {
        return Helpers.PostAPIValue('ProjectDataMarts/Remove', dataMarts, doNotHandleFail);
    }
}
export class ProjectOrganizations {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('ProjectOrganizations/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(updateInfo, doNotHandleFail) {
        return Helpers.PostAPIValue('ProjectOrganizations/InsertOrUpdate', updateInfo, doNotHandleFail);
    }
    static Remove(organizations, doNotHandleFail) {
        return Helpers.PostAPIValue('ProjectOrganizations/Remove', organizations, doNotHandleFail);
    }
}
export class Projects {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/List' + params, doNotHandleFail);
    }
    static ProjectsWithRequests($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/ProjectsWithRequests' + params, doNotHandleFail);
    }
    static GetActivityTreeByProjectID(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetActivityTreeByProjectID' + params, doNotHandleFail);
    }
    static RequestableProjects($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/RequestableProjects' + params, doNotHandleFail);
    }
    static GetRequestTypes(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetRequestTypes' + params, doNotHandleFail);
    }
    static GetRequestTypesByModel(projectID, dataModelID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (dataModelID != null)
            params += '&dataModelID=' + dataModelID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetRequestTypesByModel' + params, doNotHandleFail);
    }
    static GetAvailableRequestTypeForNewRequest(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetAvailableRequestTypeForNewRequest' + params, doNotHandleFail);
    }
    static GetDataModelsByProject(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetDataModelsByProject' + params, doNotHandleFail);
    }
    static GetProjectRequestTypes(projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetProjectRequestTypes' + params, doNotHandleFail);
    }
    static UpdateProjectRequestTypes(requestTypes, doNotHandleFail) {
        return Helpers.PostAPIValue('Projects/UpdateProjectRequestTypes', requestTypes, doNotHandleFail);
    }
    static Copy(projectID, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/Copy' + params, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Projects/Delete' + params, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Projects/Insert', values, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Projects/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Projects/Update', values, doNotHandleFail);
    }
    static UpdateActivities(ID, username, password, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (username != null)
            params += '&username=' + encodeURIComponent(username);
        if (password != null)
            params += '&password=' + encodeURIComponent(password);
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/UpdateActivities' + params, doNotHandleFail);
    }
    static GetFieldOptions(projectID, userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (projectID != null)
            params += '&projectID=' + projectID;
        if (userID != null)
            params += '&userID=' + userID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetFieldOptions' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Projects/GetPermissions' + params, doNotHandleFail);
    }
}
export class Documents {
    static ByTask(tasks, filterByTaskItemType, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (tasks != null)
            for (var j = 0; j < tasks.length; j++) {
                params += '&tasks=' + tasks[j];
            }
        if (filterByTaskItemType != null)
            for (var j = 0; j < filterByTaskItemType.length; j++) {
                params += '&filterByTaskItemType=' + filterByTaskItemType[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/ByTask' + params, doNotHandleFail);
    }
    static ByRevisionID(revisionSets, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (revisionSets != null)
            for (var j = 0; j < revisionSets.length; j++) {
                params += '&revisionSets=' + revisionSets[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/ByRevisionID' + params, doNotHandleFail);
    }
    static ByResponse(ID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/ByResponse' + params, doNotHandleFail);
    }
    static GeneralRequestDocuments(requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/GeneralRequestDocuments' + params, doNotHandleFail);
    }
    static Read(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            params += '&id=' + id;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/Read' + params, doNotHandleFail);
    }
    static Download(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            params += '&id=' + id;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Documents/Download' + params, doNotHandleFail);
    }
    static Upload(doNotHandleFail) {
        return Helpers.PostAPIValue('Documents/Upload', doNotHandleFail);
    }
    static UploadChunked(doNotHandleFail) {
        return Helpers.PostAPIValue('Documents/UploadChunked', doNotHandleFail);
    }
    static Delete(id, doNotHandleFail) {
        var params = '';
        if (id != null)
            for (var j = 0; j < id.length; j++) {
                params += '&id=' + id[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Documents/Delete' + params, doNotHandleFail);
    }
}
export class DataMartAvailability {
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMartAvailability/List' + params, doNotHandleFail);
    }
}
export class DataModels {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataModels/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataModels/List' + params, doNotHandleFail);
    }
    static ListDataModelProcessors($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataModels/ListDataModelProcessors' + params, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataModels/GetPermissions' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('DataModels/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('DataModels/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('DataModels/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('DataModels/Delete' + params, doNotHandleFail);
    }
}
export class DataMarts {
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/Get' + params, doNotHandleFail);
    }
    static GetByRoute(requestDataMartID, doNotHandleFail) {
        var params = '';
        if (requestDataMartID != null)
            params += '&requestDataMartID=' + requestDataMartID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/GetByRoute' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/List' + params, doNotHandleFail);
    }
    static ListBasic($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/ListBasic' + params, doNotHandleFail);
    }
    static DataMartTypeList($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/DataMartTypeList' + params, doNotHandleFail);
    }
    static GetRequestTypesByDataMarts(DataMartId, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMarts/GetRequestTypesByDataMarts', DataMartId, doNotHandleFail);
    }
    static GetInstalledModelsByDataMart(DataMartId, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (DataMartId != null)
            params += '&DataMartId=' + DataMartId;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/GetInstalledModelsByDataMart' + params, doNotHandleFail);
    }
    static UninstallModel(model, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMarts/UninstallModel', model, doNotHandleFail);
    }
    static Copy(datamartID, doNotHandleFail) {
        var params = '';
        if (datamartID != null)
            params += '&datamartID=' + datamartID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/Copy' + params, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('DataMarts/Delete' + params, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMarts/Insert', values, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('DataMarts/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('DataMarts/Update', values, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('DataMarts/GetPermissions' + params, doNotHandleFail);
    }
}
export class Comments {
    static ByRequestID(requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/ByRequestID' + params, doNotHandleFail);
    }
    static ByDocumentID(documentID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (documentID != null)
            params += '&documentID=' + documentID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/ByDocumentID' + params, doNotHandleFail);
    }
    static GetDocumentReferencesByRequest(requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (requestID != null)
            params += '&requestID=' + requestID;
        if (workflowActivityID != null)
            params += '&workflowActivityID=' + workflowActivityID;
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/GetDocumentReferencesByRequest' + params, doNotHandleFail);
    }
    static AddWorkflowComment(value, doNotHandleFail) {
        return Helpers.PostAPIValue('Comments/AddWorkflowComment', value, doNotHandleFail);
    }
    static GetPermissions(IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if (IDs != null)
            for (var j = 0; j < IDs.length; j++) {
                params += '&IDs=' + IDs[j];
            }
        if (permissions != null)
            for (var j = 0; j < permissions.length; j++) {
                params += '&permissions=' + permissions[j];
            }
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/GetPermissions' + params, doNotHandleFail);
    }
    static Get(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            params += '&ID=' + ID;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/Get' + params, doNotHandleFail);
    }
    static List($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
        var params = '';
        if ($filter)
            params += '&$filter=' + $filter;
        if ($select)
            params += '&$select=' + $select;
        if ($orderby)
            params += '&$orderby=' + $orderby;
        if ($skip)
            params += '&$skip=' + $skip;
        if ($top)
            params += '&$top=' + $top;
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.GetAPIResult('Comments/List' + params, doNotHandleFail);
    }
    static InsertOrUpdate(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Comments/InsertOrUpdate', values, doNotHandleFail);
    }
    static Update(values, doNotHandleFail) {
        return Helpers.PutAPIValue('Comments/Update', values, doNotHandleFail);
    }
    static Insert(values, doNotHandleFail) {
        return Helpers.PostAPIValue('Comments/Insert', values, doNotHandleFail);
    }
    static Delete(ID, doNotHandleFail) {
        var params = '';
        if (ID != null)
            for (var j = 0; j < ID.length; j++) {
                params += '&ID=' + ID[j];
            }
        if (params.length > 0)
            params = '?' + params.substr(1);
        return Helpers.DeleteAPIValue('Comments/Delete' + params, doNotHandleFail);
    }
}
export class Helpers {
    static failMethod;
    static RegisterFailMethod(method) {
        this.failMethod = method;
    }
    static FixStringDatesInResults(results) {
        if (Array.isArray(results)) {
            results.forEach((data) => {
                this.FixStringDatesInResult(data);
            });
        }
        else {
            this.FixStringDatesInResult(results);
        }
    }
    static FixStringDatesInResult(data) {
        for (var field in data) {
            if (data[field]) {
                if ($.isArray(data[field])) {
                    this.FixStringDatesInResults(data[field]);
                }
                else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}\+{1}\d{2}:\d{2}$/g)) {
                    //this is to handle datetimeoffset serialization, it has the timezone offset included and does not need the 'Z' appended.
                    data[field] = new Date(data[field]);
                }
                else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                    //original datetime conversion that assumes the date is UTC.
                    if (data[field].indexOf('Z') > -1) {
                        data[field] = new Date(data[field]);
                    }
                    else {
                        data[field] = new Date(data[field] + 'Z');
                    }
                }
            }
        }
    }
    static GetAPIResult(url, doNotHandleFail) {
        var d = jQuery.Deferred();
        if (!jQuery.support.cors) {
            url = '/api/get?Url=' + encodeURIComponent(url);
        }
        else {
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
            d.reject(e);
        });
        return d;
    }
    static PostAPIValue(url, value, doNotHandleFail) {
        var d = jQuery.Deferred();
        if (!jQuery.support.cors) {
            url = '/api/post?Url=' + encodeURIComponent(url);
        }
        else {
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
            }
            else if (result.results != null) {
                if (!$.isArray(result.results)) {
                    d.resolve([result.results]);
                    return;
                }
                else {
                    d.resolve(result.results);
                    return;
                }
            }
            else {
                if (!$.isArray(result)) {
                    d.resolve([result]);
                    return;
                }
            }
            d.resolve(result);
        }).fail((e) => {
            if (this.failMethod && !doNotHandleFail)
                this.failMethod(e);
            d.reject(e);
        });
        return d;
    }
    static PutAPIValue(url, value, doNotHandleFail) {
        var d = jQuery.Deferred();
        if (!jQuery.support.cors) {
            url = '/api/put?Url=' + encodeURIComponent(url);
        }
        else {
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
            }
            else if (result.results != null) {
                if (!$.isArray(result.results)) {
                    d.resolve([result.results]);
                    return;
                }
                else {
                    d.resolve(result.results);
                    return;
                }
            }
            else {
                if (!$.isArray(result)) {
                    d.resolve([result]);
                    return;
                }
            }
            d.resolve(result);
        }).fail((e) => {
            if (this.failMethod && !doNotHandleFail)
                this.failMethod(e);
            d.reject(e);
        });
        return d;
    }
    static DeleteAPIValue(url, doNotHandleFail) {
        var d = jQuery.Deferred();
        if (!jQuery.support.cors) {
            url = '/api/delete?Url=' + encodeURIComponent(url);
        }
        else {
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
            }
            else if (result.results != null) {
                if (!$.isArray(result.results)) {
                    d.resolve([result.results]);
                    return;
                }
                else {
                    d.resolve(result.results);
                    return;
                }
            }
            else {
                if (!$.isArray(result)) {
                    d.resolve([result]);
                    return;
                }
            }
            d.resolve(result);
        }).fail((e) => {
            if (this.failMethod && !doNotHandleFail)
                this.failMethod(e);
            d.reject(e);
        });
        return d;
    }
}
//# sourceMappingURL=Lpp.Dns.WebApi.js.map
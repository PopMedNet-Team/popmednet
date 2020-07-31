/// <reference path='../../node_modules/@types/jquery/index.d.ts' />
/// <reference path='Lpp.Dns.ViewModels.ts' />
var Dns;
(function (Dns) {
    var WebApi;
    (function (WebApi) {
        var Workflow = /** @class */ (function () {
            function Workflow() {
            }
            Workflow.GetWorkflowEntryPointByRequestTypeID = function (requestTypeID, doNotHandleFail) {
                var params = '';
                if (requestTypeID != null)
                    params += '&requestTypeID=' + requestTypeID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Workflow/GetWorkflowEntryPointByRequestTypeID' + params, doNotHandleFail);
            };
            Workflow.GetWorkflowActivity = function (workflowActivityID, doNotHandleFail) {
                var params = '';
                if (workflowActivityID != null)
                    params += '&workflowActivityID=' + workflowActivityID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Workflow/GetWorkflowActivity' + params, doNotHandleFail);
            };
            Workflow.GetWorkflowActivitiesByWorkflowID = function (workFlowID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Workflow.GetWorkflowRolesByWorkflowID = function (workflowID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Workflow.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Workflow.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Workflow/Get' + params, doNotHandleFail);
            };
            Workflow.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Workflow.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Workflow/InsertOrUpdate', values, doNotHandleFail);
            };
            Workflow.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Workflow/Update', values, doNotHandleFail);
            };
            Workflow.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Workflow/Insert', values, doNotHandleFail);
            };
            Workflow.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Workflow/Delete' + params, doNotHandleFail);
            };
            return Workflow;
        }());
        WebApi.Workflow = Workflow;
        var Wbd = /** @class */ (function () {
            function Wbd() {
            }
            Wbd.ApproveRequest = function (requestID, doNotHandleFail) {
                return Helpers.PutAPIValue('Wbd/ApproveRequest', requestID, doNotHandleFail);
            };
            Wbd.RejectRequest = function (requestID, doNotHandleFail) {
                return Helpers.PutAPIValue('Wbd/RejectRequest', requestID, doNotHandleFail);
            };
            Wbd.GetRequestByID = function (Id, doNotHandleFail) {
                var params = '';
                if (Id != null)
                    params += '&Id=' + Id;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Wbd/GetRequestByID' + params, doNotHandleFail);
            };
            Wbd.SaveRequest = function (request, doNotHandleFail) {
                return Helpers.PostAPIValue('Wbd/SaveRequest', request, doNotHandleFail);
            };
            Wbd.GetActivityTreeByProjectID = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Wbd.Register = function (registration, doNotHandleFail) {
                return Helpers.PostAPIValue('Wbd/Register', registration, doNotHandleFail);
            };
            Wbd.GetChanges = function (criteria, doNotHandleFail) {
                return Helpers.PostAPIValue('Wbd/GetChanges', criteria, doNotHandleFail);
            };
            Wbd.DownloadDocument = function (documentId, doNotHandleFail) {
                var params = '';
                if (documentId != null)
                    params += '&documentId=' + documentId;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Wbd/DownloadDocument' + params, doNotHandleFail);
            };
            Wbd.DownloadRequestViewableFile = function (requestId, doNotHandleFail) {
                var params = '';
                if (requestId != null)
                    params += '&requestId=' + requestId;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Wbd/DownloadRequestViewableFile' + params, doNotHandleFail);
            };
            Wbd.CopyRequest = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Wbd/CopyRequest' + params, doNotHandleFail);
            };
            Wbd.UpdateResponseStatus = function (details, doNotHandleFail) {
                return Helpers.PostAPIValue('Wbd/UpdateResponseStatus', details, doNotHandleFail);
            };
            return Wbd;
        }());
        WebApi.Wbd = Wbd;
        var SsoEndpoints = /** @class */ (function () {
            function SsoEndpoints() {
            }
            SsoEndpoints.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('SsoEndpoints/Get' + params, doNotHandleFail);
            };
            SsoEndpoints.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            SsoEndpoints.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            SsoEndpoints.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('SsoEndpoints/InsertOrUpdate', values, doNotHandleFail);
            };
            SsoEndpoints.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('SsoEndpoints/Update', values, doNotHandleFail);
            };
            SsoEndpoints.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('SsoEndpoints/Insert', values, doNotHandleFail);
            };
            SsoEndpoints.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('SsoEndpoints/Delete' + params, doNotHandleFail);
            };
            return SsoEndpoints;
        }());
        WebApi.SsoEndpoints = SsoEndpoints;
        var Users = /** @class */ (function () {
            function Users() {
            }
            Users.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/Get' + params, doNotHandleFail);
            };
            Users.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.ValidateLogin = function (login, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/ValidateLogin', login, doNotHandleFail);
            };
            Users.ByUserName = function (userName, doNotHandleFail) {
                var params = '';
                if (userName != null)
                    params += '&userName=' + encodeURIComponent(userName);
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/ByUserName' + params, doNotHandleFail);
            };
            Users.UserRegistration = function (data, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/UserRegistration', data, doNotHandleFail);
            };
            Users.ListAvailableProjects = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.ForgotPassword = function (data, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/ForgotPassword', data, doNotHandleFail);
            };
            Users.ChangePassword = function (updateInfo, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/ChangePassword', updateInfo, doNotHandleFail);
            };
            Users.RestorePassword = function (updateInfo, doNotHandleFail) {
                return Helpers.PutAPIValue('Users/RestorePassword', updateInfo, doNotHandleFail);
            };
            Users.GetAssignedNotifications = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.GetSubscribableEvents = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.GetSubscribedEvents = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.GetNotifications = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.ListAuthenticationAudits = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.ListDistinctEnvironments = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.UpdateSubscribedEvents = function (subscribedEvents, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/UpdateSubscribedEvents', subscribedEvents, doNotHandleFail);
            };
            Users.GetTasks = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.GetWorkflowTasks = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.GetWorkflowTaskUsers = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.Logout = function (doNotHandleFail) {
                return Helpers.PostAPIValue('Users/Logout', doNotHandleFail);
            };
            Users.MemberOfSecurityGroups = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.UpdateSecurityGroups = function (groups, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/UpdateSecurityGroups', groups, doNotHandleFail);
            };
            Users.GetGlobalPermission = function (permissionID, doNotHandleFail) {
                var params = '';
                if (permissionID != null)
                    params += '&permissionID=' + permissionID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/GetGlobalPermission' + params, doNotHandleFail);
            };
            Users.ReturnMainMenu = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.UpdateLookupListsTest = function (username, password, doNotHandleFail) {
                var params = '';
                if (username != null)
                    params += '&username=' + encodeURIComponent(username);
                if (password != null)
                    params += '&password=' + encodeURIComponent(password);
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/UpdateLookupListsTest' + params, doNotHandleFail);
            };
            Users.UpdateLookupLists = function (username, password, doNotHandleFail) {
                var params = '';
                if (username != null)
                    params += '&username=' + encodeURIComponent(username);
                if (password != null)
                    params += '&password=' + encodeURIComponent(password);
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/UpdateLookupLists' + params, doNotHandleFail);
            };
            Users.SaveSetting = function (setting, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/SaveSetting', setting, doNotHandleFail);
            };
            Users.GetSetting = function (key, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Users.AllowApproveRejectRequest = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/AllowApproveRejectRequest' + params, doNotHandleFail);
            };
            Users.HasPassword = function (userID, doNotHandleFail) {
                var params = '';
                if (userID != null)
                    params += '&userID=' + userID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/HasPassword' + params, doNotHandleFail);
            };
            Users.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Users/Delete' + params, doNotHandleFail);
            };
            Users.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/InsertOrUpdate', values, doNotHandleFail);
            };
            Users.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Users/Update', values, doNotHandleFail);
            };
            Users.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Users/Insert', values, doNotHandleFail);
            };
            Users.GetMetadataEditPermissionsSummary = function (doNotHandleFail) {
                var params = '';
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Users/GetMetadataEditPermissionsSummary' + params, doNotHandleFail);
            };
            Users.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return Users;
        }());
        WebApi.Users = Users;
        var Theme = /** @class */ (function () {
            function Theme() {
            }
            Theme.GetText = function (keys, doNotHandleFail) {
                var params = '';
                if (keys != null)
                    for (var j = 0; j < keys.length; j++) {
                        params += '&keys=' + encodeURIComponent(keys[j]);
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Theme/GetText' + params, doNotHandleFail);
            };
            Theme.GetImagePath = function (doNotHandleFail) {
                var params = '';
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Theme/GetImagePath' + params, doNotHandleFail);
            };
            return Theme;
        }());
        WebApi.Theme = Theme;
        var Terms = /** @class */ (function () {
            function Terms() {
            }
            Terms.ListTemplateTerms = function (id, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Terms.ParseCodeList = function (doNotHandleFail) {
                return Helpers.PostAPIValue('Terms/ParseCodeList', doNotHandleFail);
            };
            Terms.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Terms.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Terms/Get' + params, doNotHandleFail);
            };
            Terms.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Terms.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Terms/InsertOrUpdate', values, doNotHandleFail);
            };
            Terms.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Terms/Update', values, doNotHandleFail);
            };
            Terms.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Terms/Insert', values, doNotHandleFail);
            };
            Terms.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Terms/Delete' + params, doNotHandleFail);
            };
            return Terms;
        }());
        WebApi.Terms = Terms;
        var Security = /** @class */ (function () {
            function Security() {
            }
            Security.ListSecurityEntities = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetPermissionsByLocation = function (locations, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/GetPermissionsByLocation', locations, doNotHandleFail);
            };
            Security.GetDataMartPermissions = function (dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetOrganizationPermissions = function (organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetUserPermissions = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetGroupPermissions = function (groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetRegistryPermissions = function (registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetProjectPermissions = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetGlobalPermissions = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetProjectOrganizationPermissions = function (projectID, organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetProjectRequestTypeWorkflowActivityPermissions = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetWorkflowActivityPermissionsForIdentity = function (projectID, workflowActivityID, requestTypeID, permissionID, doNotHandleFail) {
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
            };
            Security.GetProjectDataMartPermissions = function (projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetProjectDataMartRequestTypePermissions = function (projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetProjectRequestTypePermissions = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetDataMartRequestTypePermissions = function (dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetTemplatePermissions = function (templateID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.GetRequestTypePermissions = function (requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.UpdateProjectRequestTypeWorkflowActivityPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectRequestTypeWorkflowActivityPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateDataMartRequestTypePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateDataMartRequestTypePermissions', permissions, doNotHandleFail);
            };
            Security.UpdateProjectDataMartPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectDataMartPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateProjectDataMartRequestTypePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectDataMartRequestTypePermissions', permissions, doNotHandleFail);
            };
            Security.UpdateProjectOrganizationPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectOrganizationPermissions', permissions, doNotHandleFail);
            };
            Security.UpdatePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdatePermissions', permissions, doNotHandleFail);
            };
            Security.UpdateGroupPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateGroupPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateRegistryPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateRegistryPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateProjectPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateProjectRequestTypePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectRequestTypePermissions', permissions, doNotHandleFail);
            };
            Security.UpdateDataMartPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateDataMartPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateOrganizationPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateOrganizationPermissions', permissions, doNotHandleFail);
            };
            Security.UpdateUserPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateUserPermissions', permissions, doNotHandleFail);
            };
            Security.GetAvailableSecurityGroupTree = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.UpdateTemplatePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateTemplatePermissions', permissions, doNotHandleFail);
            };
            Security.UpdateRequestTypePermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateRequestTypePermissions', permissions, doNotHandleFail);
            };
            Security.GetGlobalFieldOptionPermissions = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.UpdateFieldOptionPermissions = function (fieldOptionUpdates, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateFieldOptionPermissions', fieldOptionUpdates, doNotHandleFail);
            };
            Security.GetProjectFieldOptionPermissions = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Security.UpdateProjectFieldOptionPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Security/UpdateProjectFieldOptionPermissions', permissions, doNotHandleFail);
            };
            return Security;
        }());
        WebApi.Security = Security;
        var SecurityGroups = /** @class */ (function () {
            function SecurityGroups() {
            }
            SecurityGroups.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('SecurityGroups/Get' + params, doNotHandleFail);
            };
            SecurityGroups.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            SecurityGroups.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            SecurityGroups.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('SecurityGroups/InsertOrUpdate', values, doNotHandleFail);
            };
            SecurityGroups.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('SecurityGroups/Update', values, doNotHandleFail);
            };
            SecurityGroups.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('SecurityGroups/Insert', values, doNotHandleFail);
            };
            SecurityGroups.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('SecurityGroups/Delete' + params, doNotHandleFail);
            };
            return SecurityGroups;
        }());
        WebApi.SecurityGroups = SecurityGroups;
        var Organizations = /** @class */ (function () {
            function Organizations() {
            }
            Organizations.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Organizations/Get' + params, doNotHandleFail);
            };
            Organizations.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Organizations.ListByGroupMembership = function (groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Organizations.Copy = function (organizationID, doNotHandleFail) {
                var params = '';
                if (organizationID != null)
                    params += '&organizationID=' + organizationID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Organizations/Copy' + params, doNotHandleFail);
            };
            Organizations.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Organizations/Delete' + params, doNotHandleFail);
            };
            Organizations.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Organizations/InsertOrUpdate', values, doNotHandleFail);
            };
            Organizations.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Organizations/Insert', values, doNotHandleFail);
            };
            Organizations.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Organizations/Update', values, doNotHandleFail);
            };
            Organizations.EHRSInsertOrUpdate = function (updates, doNotHandleFail) {
                return Helpers.PostAPIValue('Organizations/EHRSInsertOrUpdate', updates, doNotHandleFail);
            };
            Organizations.ListEHRS = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Organizations.DeleteEHRS = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    for (var j = 0; j < id.length; j++) {
                        params += '&id=' + id[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Organizations/DeleteEHRS' + params, doNotHandleFail);
            };
            Organizations.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return Organizations;
        }());
        WebApi.Organizations = Organizations;
        var OrganizationRegistries = /** @class */ (function () {
            function OrganizationRegistries() {
            }
            OrganizationRegistries.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            OrganizationRegistries.InsertOrUpdate = function (organizations, doNotHandleFail) {
                return Helpers.PostAPIValue('OrganizationRegistries/InsertOrUpdate', organizations, doNotHandleFail);
            };
            OrganizationRegistries.Remove = function (organizations, doNotHandleFail) {
                return Helpers.PostAPIValue('OrganizationRegistries/Remove', organizations, doNotHandleFail);
            };
            return OrganizationRegistries;
        }());
        WebApi.OrganizationRegistries = OrganizationRegistries;
        var Registries = /** @class */ (function () {
            function Registries() {
            }
            Registries.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Registries/Get' + params, doNotHandleFail);
            };
            Registries.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Registries.GetRegistryItemDefinitionList = function (registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Registries.UpdateRegistryItemDefinitions = function (updateParams, doNotHandleFail) {
                return Helpers.PutAPIValue('Registries/UpdateRegistryItemDefinitions', updateParams, doNotHandleFail);
            };
            Registries.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Registries.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Registries/InsertOrUpdate', values, doNotHandleFail);
            };
            Registries.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Registries/Update', values, doNotHandleFail);
            };
            Registries.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Registries/Insert', values, doNotHandleFail);
            };
            Registries.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Registries/Delete' + params, doNotHandleFail);
            };
            return Registries;
        }());
        WebApi.Registries = Registries;
        var RegistryItemDefinition = /** @class */ (function () {
            function RegistryItemDefinition() {
            }
            RegistryItemDefinition.GetList = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return RegistryItemDefinition;
        }());
        WebApi.RegistryItemDefinition = RegistryItemDefinition;
        var NetworkMessages = /** @class */ (function () {
            function NetworkMessages() {
            }
            NetworkMessages.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('NetworkMessages/Get' + params, doNotHandleFail);
            };
            NetworkMessages.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            NetworkMessages.ListLastDays = function (days, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            NetworkMessages.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('NetworkMessages/Insert', values, doNotHandleFail);
            };
            NetworkMessages.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            NetworkMessages.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('NetworkMessages/InsertOrUpdate', values, doNotHandleFail);
            };
            NetworkMessages.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('NetworkMessages/Update', values, doNotHandleFail);
            };
            NetworkMessages.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('NetworkMessages/Delete' + params, doNotHandleFail);
            };
            return NetworkMessages;
        }());
        WebApi.NetworkMessages = NetworkMessages;
        var LookupListCategory = /** @class */ (function () {
            function LookupListCategory() {
            }
            LookupListCategory.GetList = function (listID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return LookupListCategory;
        }());
        WebApi.LookupListCategory = LookupListCategory;
        var LookupList = /** @class */ (function () {
            function LookupList() {
            }
            LookupList.GetList = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return LookupList;
        }());
        WebApi.LookupList = LookupList;
        var LookupListValue = /** @class */ (function () {
            function LookupListValue() {
            }
            LookupListValue.GetList = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            LookupListValue.GetCodeDetailsByCode = function (details, doNotHandleFail) {
                return Helpers.PostAPIValue('LookupListValue/GetCodeDetailsByCode', details, doNotHandleFail);
            };
            LookupListValue.LookupList = function (listID, lookup, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return LookupListValue;
        }());
        WebApi.LookupListValue = LookupListValue;
        var Groups = /** @class */ (function () {
            function Groups() {
            }
            Groups.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Groups/Get' + params, doNotHandleFail);
            };
            Groups.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Groups.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Groups.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Groups/InsertOrUpdate', values, doNotHandleFail);
            };
            Groups.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Groups/Update', values, doNotHandleFail);
            };
            Groups.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Groups/Insert', values, doNotHandleFail);
            };
            Groups.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Groups/Delete' + params, doNotHandleFail);
            };
            return Groups;
        }());
        WebApi.Groups = Groups;
        var OrganizationGroups = /** @class */ (function () {
            function OrganizationGroups() {
            }
            OrganizationGroups.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            OrganizationGroups.InsertOrUpdate = function (organizations, doNotHandleFail) {
                return Helpers.PostAPIValue('OrganizationGroups/InsertOrUpdate', organizations, doNotHandleFail);
            };
            OrganizationGroups.Remove = function (organizations, doNotHandleFail) {
                return Helpers.PostAPIValue('OrganizationGroups/Remove', organizations, doNotHandleFail);
            };
            return OrganizationGroups;
        }());
        WebApi.OrganizationGroups = OrganizationGroups;
        var Events = /** @class */ (function () {
            function Events() {
            }
            Events.GetEventsByLocation = function (locations, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/GetEventsByLocation', locations, doNotHandleFail);
            };
            Events.GetGroupEventPermissions = function (groupID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.GetRegistryEventPermissions = function (registryID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.GetProjectEventPermissions = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.GetOrganizationEventPermissions = function (organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.GetUserEventPermissions = function (userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.GetGlobalEventPermissions = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.UpdateGroupEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateGroupEventPermissions', permissions, doNotHandleFail);
            };
            Events.UpdateRegistryEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateRegistryEventPermissions', permissions, doNotHandleFail);
            };
            Events.UpdateProjectEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateProjectEventPermissions', permissions, doNotHandleFail);
            };
            Events.UpdateOrganizationEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateOrganizationEventPermissions', permissions, doNotHandleFail);
            };
            Events.UpdateUserEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateUserEventPermissions', permissions, doNotHandleFail);
            };
            Events.UpdateGlobalEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateGlobalEventPermissions', permissions, doNotHandleFail);
            };
            Events.GetProjectOrganizationEventPermissions = function (projectID, organizationID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.UpdateProjectOrganizationEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateProjectOrganizationEventPermissions', permissions, doNotHandleFail);
            };
            Events.GetProjectDataMartEventPermissions = function (projectID, dataMartID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.UpdateProjectDataMartEventPermissions = function (permissions, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/UpdateProjectDataMartEventPermissions', permissions, doNotHandleFail);
            };
            Events.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Events/Get' + params, doNotHandleFail);
            };
            Events.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Events.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/InsertOrUpdate', values, doNotHandleFail);
            };
            Events.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Events/Update', values, doNotHandleFail);
            };
            Events.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Events/Insert', values, doNotHandleFail);
            };
            Events.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Events/Delete' + params, doNotHandleFail);
            };
            return Events;
        }());
        WebApi.Events = Events;
        var Tasks = /** @class */ (function () {
            function Tasks() {
            }
            Tasks.ByRequestID = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Tasks.GetWorkflowActivityDataForRequest = function (requestID, workflowActivityID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (workflowActivityID != null)
                    params += '&workflowActivityID=' + workflowActivityID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Tasks/GetWorkflowActivityDataForRequest' + params, doNotHandleFail);
            };
            Tasks.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Tasks.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Tasks/Get' + params, doNotHandleFail);
            };
            Tasks.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Tasks.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Tasks/InsertOrUpdate', values, doNotHandleFail);
            };
            Tasks.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Tasks/Update', values, doNotHandleFail);
            };
            Tasks.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Tasks/Insert', values, doNotHandleFail);
            };
            Tasks.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Tasks/Delete' + params, doNotHandleFail);
            };
            return Tasks;
        }());
        WebApi.Tasks = Tasks;
        var LegacyRequests = /** @class */ (function () {
            function LegacyRequests() {
            }
            LegacyRequests.ScheduleLegacyRequest = function (dto, doNotHandleFail) {
                return Helpers.PostAPIValue('LegacyRequests/ScheduleLegacyRequest', dto, doNotHandleFail);
            };
            LegacyRequests.DeleteRequestSchedules = function (requestID, doNotHandleFail) {
                return Helpers.PostAPIValue('LegacyRequests/DeleteRequestSchedules', requestID, doNotHandleFail);
            };
            return LegacyRequests;
        }());
        WebApi.LegacyRequests = LegacyRequests;
        var ReportAggregationLevel = /** @class */ (function () {
            function ReportAggregationLevel() {
            }
            ReportAggregationLevel.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('ReportAggregationLevel/Delete' + params, doNotHandleFail);
            };
            ReportAggregationLevel.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            ReportAggregationLevel.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('ReportAggregationLevel/Get' + params, doNotHandleFail);
            };
            ReportAggregationLevel.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            ReportAggregationLevel.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('ReportAggregationLevel/InsertOrUpdate', values, doNotHandleFail);
            };
            ReportAggregationLevel.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('ReportAggregationLevel/Update', values, doNotHandleFail);
            };
            ReportAggregationLevel.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('ReportAggregationLevel/Insert', values, doNotHandleFail);
            };
            return ReportAggregationLevel;
        }());
        WebApi.ReportAggregationLevel = ReportAggregationLevel;
        var RequestObservers = /** @class */ (function () {
            function RequestObservers() {
            }
            RequestObservers.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestObservers/Insert', values, doNotHandleFail);
            };
            RequestObservers.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestObservers/InsertOrUpdate', values, doNotHandleFail);
            };
            RequestObservers.Update = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestObservers/Update', values, doNotHandleFail);
            };
            RequestObservers.ListRequestObservers = function (RequestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestObservers.LookupObserverEvents = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestObservers.LookupObservers = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestObservers.ValidateInsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestObservers/ValidateInsertOrUpdate', values, doNotHandleFail);
            };
            RequestObservers.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestObservers.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('RequestObservers/Get' + params, doNotHandleFail);
            };
            RequestObservers.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestObservers.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('RequestObservers/Delete' + params, doNotHandleFail);
            };
            return RequestObservers;
        }());
        WebApi.RequestObservers = RequestObservers;
        var RequestUsers = /** @class */ (function () {
            function RequestUsers() {
            }
            RequestUsers.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestUsers.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestUsers/Insert', values, doNotHandleFail);
            };
            RequestUsers.Delete = function (requestUsers, doNotHandleFail) {
                var params = '';
                if (requestUsers != null)
                    for (var j = 0; j < requestUsers.length; j++) {
                        params += '&requestUsers=' + requestUsers[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('RequestUsers/Delete' + params, doNotHandleFail);
            };
            return RequestUsers;
        }());
        WebApi.RequestUsers = RequestUsers;
        var Response = /** @class */ (function () {
            function Response() {
            }
            Response.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/Get' + params, doNotHandleFail);
            };
            Response.ApproveResponses = function (responses, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/ApproveResponses', responses, doNotHandleFail);
            };
            Response.RejectResponses = function (responses, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/RejectResponses', responses, doNotHandleFail);
            };
            Response.RejectAndReSubmitResponses = function (responses, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/RejectAndReSubmitResponses', responses, doNotHandleFail);
            };
            Response.GetByWorkflowActivity = function (requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.CanViewIndividualResponses = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/CanViewIndividualResponses' + params, doNotHandleFail);
            };
            Response.CanViewAggregateResponses = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/CanViewAggregateResponses' + params, doNotHandleFail);
            };
            Response.CanViewPendingApprovalResponses = function (responses, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/CanViewPendingApprovalResponses', responses, doNotHandleFail);
            };
            Response.GetForWorkflowRequest = function (requestID, viewDocuments, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (viewDocuments != null)
                    params += '&viewDocuments=' + viewDocuments;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/GetForWorkflowRequest' + params, doNotHandleFail);
            };
            Response.GetDetails = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    for (var j = 0; j < id.length; j++) {
                        params += '&id=' + id[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/GetDetails' + params, doNotHandleFail);
            };
            Response.GetWorkflowResponseContent = function (id, view, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.GetResponseGroups = function (responseIDs, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.GetResponseGroupsByRequestID = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.Export = function (id, view, format, doNotHandleFail) {
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
            };
            Response.ExportAllAsZip = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    for (var j = 0; j < id.length; j++) {
                        params += '&id=' + id[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/ExportAllAsZip' + params, doNotHandleFail);
            };
            Response.GetTrackingTableForAnalysisCenter = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/GetTrackingTableForAnalysisCenter' + params, doNotHandleFail);
            };
            Response.GetTrackingTableForDataPartners = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Response/GetTrackingTableForDataPartners' + params, doNotHandleFail);
            };
            Response.GetEnhancedEventLog = function (requestID, format, download, doNotHandleFail) {
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
            };
            Response.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Response.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/InsertOrUpdate', values, doNotHandleFail);
            };
            Response.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Response/Update', values, doNotHandleFail);
            };
            Response.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Response/Insert', values, doNotHandleFail);
            };
            Response.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Response/Delete' + params, doNotHandleFail);
            };
            return Response;
        }());
        WebApi.Response = Response;
        var Requests = /** @class */ (function () {
            function Requests() {
            }
            Requests.CompleteActivity = function (request, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/CompleteActivity', request, doNotHandleFail);
            };
            Requests.TerminateRequest = function (requestID, doNotHandleFail) {
                return Helpers.PutAPIValue('Requests/TerminateRequest', requestID, doNotHandleFail);
            };
            Requests.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.ListForHomepage = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.RequestsByRoute = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetCompatibleDataMarts = function (requestDetails, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/GetCompatibleDataMarts', requestDetails, doNotHandleFail);
            };
            Requests.GetDataMartsForInstalledModels = function (requestDetails, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/GetDataMartsForInstalledModels', requestDetails, doNotHandleFail);
            };
            Requests.RequestDataMarts = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetOverrideableRequestDataMarts = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.UpdateRequestDataMarts = function (dataMarts, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/UpdateRequestDataMarts', dataMarts, doNotHandleFail);
            };
            Requests.UpdateRequestDataMartsMetadata = function (dataMarts, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/UpdateRequestDataMartsMetadata', dataMarts, doNotHandleFail);
            };
            Requests.ListRequesterCenters = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.ListWorkPlanTypes = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.ListReportAggregationLevels = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetWorkflowHistory = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetResponseHistory = function (requestDataMartID, requestID, doNotHandleFail) {
                var params = '';
                if (requestDataMartID != null)
                    params += '&requestDataMartID=' + requestDataMartID;
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Requests/GetResponseHistory' + params, doNotHandleFail);
            };
            Requests.GetRequestSearchTerms = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetRequestTypeModels = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Requests/GetRequestTypeModels' + params, doNotHandleFail);
            };
            Requests.GetModelIDsforRequest = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Requests/GetModelIDsforRequest' + params, doNotHandleFail);
            };
            Requests.UpdateRequestMetadata = function (reqMetadata, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/UpdateRequestMetadata', reqMetadata, doNotHandleFail);
            };
            Requests.UpdateMetadataForRequests = function (updates, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/UpdateMetadataForRequests', updates, doNotHandleFail);
            };
            Requests.GetOrganizationsForRequest = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.AllowCopyRequest = function (requestID, doNotHandleFail) {
                var params = '';
                if (requestID != null)
                    params += '&requestID=' + requestID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Requests/AllowCopyRequest' + params, doNotHandleFail);
            };
            Requests.CopyRequest = function (requestID, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/CopyRequest', requestID, doNotHandleFail);
            };
            Requests.RetrieveBudgetInfoForRequests = function (ids, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Requests.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Requests/Get' + params, doNotHandleFail);
            };
            Requests.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/InsertOrUpdate', values, doNotHandleFail);
            };
            Requests.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Requests/Update', values, doNotHandleFail);
            };
            Requests.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Requests/Insert', values, doNotHandleFail);
            };
            Requests.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Requests/Delete' + params, doNotHandleFail);
            };
            return Requests;
        }());
        WebApi.Requests = Requests;
        var RequestTypes = /** @class */ (function () {
            function RequestTypes() {
            }
            RequestTypes.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/Insert', values, doNotHandleFail);
            };
            RequestTypes.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.ListAvailableRequestTypes = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/InsertOrUpdate', values, doNotHandleFail);
            };
            RequestTypes.Update = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/Update', values, doNotHandleFail);
            };
            RequestTypes.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('RequestTypes/Delete' + params, doNotHandleFail);
            };
            RequestTypes.Save = function (details, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/Save', details, doNotHandleFail);
            };
            RequestTypes.UpdateModels = function (details, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/UpdateModels', details, doNotHandleFail);
            };
            RequestTypes.GetRequestTypeModels = function (requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.GetRequestTypeTerms = function (requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.GetFilteredTerms = function (id, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.GetTermsFilteredBy = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.UpdateRequestTypeTerms = function (updateInfo, doNotHandleFail) {
                return Helpers.PostAPIValue('RequestTypes/UpdateRequestTypeTerms', updateInfo, doNotHandleFail);
            };
            RequestTypes.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            RequestTypes.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('RequestTypes/Get' + params, doNotHandleFail);
            };
            return RequestTypes;
        }());
        WebApi.RequestTypes = RequestTypes;
        var Templates = /** @class */ (function () {
            function Templates() {
            }
            Templates.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Templates.CriteriaGroups = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Templates.GetByRequestType = function (requestTypeID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Templates.GetGlobalTemplatePermissions = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Templates.SaveCriteriaGroup = function (details, doNotHandleFail) {
                return Helpers.PostAPIValue('Templates/SaveCriteriaGroup', details, doNotHandleFail);
            };
            Templates.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Templates.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Templates/Get' + params, doNotHandleFail);
            };
            Templates.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Templates/InsertOrUpdate', values, doNotHandleFail);
            };
            Templates.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Templates/Update', values, doNotHandleFail);
            };
            Templates.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Templates/Insert', values, doNotHandleFail);
            };
            Templates.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Templates/Delete' + params, doNotHandleFail);
            };
            return Templates;
        }());
        WebApi.Templates = Templates;
        var Notifications = /** @class */ (function () {
            function Notifications() {
            }
            Notifications.ExecuteScheduledNotifications = function (userName, password, doNotHandleFail) {
                var params = '';
                if (userName != null)
                    params += '&userName=' + encodeURIComponent(userName);
                if (password != null)
                    params += '&password=' + encodeURIComponent(password);
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Notifications/ExecuteScheduledNotifications' + params, doNotHandleFail);
            };
            return Notifications;
        }());
        WebApi.Notifications = Notifications;
        var DataMartInstalledModels = /** @class */ (function () {
            function DataMartInstalledModels() {
            }
            DataMartInstalledModels.InsertOrUpdate = function (updateInfo, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMartInstalledModels/InsertOrUpdate', updateInfo, doNotHandleFail);
            };
            DataMartInstalledModels.Remove = function (models, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMartInstalledModels/Remove', models, doNotHandleFail);
            };
            return DataMartInstalledModels;
        }());
        WebApi.DataMartInstalledModels = DataMartInstalledModels;
        var ProjectDataMarts = /** @class */ (function () {
            function ProjectDataMarts() {
            }
            ProjectDataMarts.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            ProjectDataMarts.ListWithRequestTypes = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            ProjectDataMarts.GetWithRequestTypes = function (projectID, dataMartID, doNotHandleFail) {
                var params = '';
                if (projectID != null)
                    params += '&projectID=' + projectID;
                if (dataMartID != null)
                    params += '&dataMartID=' + dataMartID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('ProjectDataMarts/GetWithRequestTypes' + params, doNotHandleFail);
            };
            ProjectDataMarts.InsertOrUpdate = function (updateInfo, doNotHandleFail) {
                return Helpers.PostAPIValue('ProjectDataMarts/InsertOrUpdate', updateInfo, doNotHandleFail);
            };
            ProjectDataMarts.Remove = function (dataMarts, doNotHandleFail) {
                return Helpers.PostAPIValue('ProjectDataMarts/Remove', dataMarts, doNotHandleFail);
            };
            return ProjectDataMarts;
        }());
        WebApi.ProjectDataMarts = ProjectDataMarts;
        var ProjectOrganizations = /** @class */ (function () {
            function ProjectOrganizations() {
            }
            ProjectOrganizations.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            ProjectOrganizations.InsertOrUpdate = function (updateInfo, doNotHandleFail) {
                return Helpers.PostAPIValue('ProjectOrganizations/InsertOrUpdate', updateInfo, doNotHandleFail);
            };
            ProjectOrganizations.Remove = function (organizations, doNotHandleFail) {
                return Helpers.PostAPIValue('ProjectOrganizations/Remove', organizations, doNotHandleFail);
            };
            return ProjectOrganizations;
        }());
        WebApi.ProjectOrganizations = ProjectOrganizations;
        var Projects = /** @class */ (function () {
            function Projects() {
            }
            Projects.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Projects/Get' + params, doNotHandleFail);
            };
            Projects.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.ProjectsWithRequests = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetActivityTreeByProjectID = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.RequestableProjects = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetRequestTypes = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetRequestTypesByModel = function (projectID, dataModelID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetAvailableRequestTypeForNewRequest = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetDataModelsByProject = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetProjectRequestTypes = function (projectID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.UpdateProjectRequestTypes = function (requestTypes, doNotHandleFail) {
                return Helpers.PostAPIValue('Projects/UpdateProjectRequestTypes', requestTypes, doNotHandleFail);
            };
            Projects.Copy = function (projectID, doNotHandleFail) {
                var params = '';
                if (projectID != null)
                    params += '&projectID=' + projectID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Projects/Copy' + params, doNotHandleFail);
            };
            Projects.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Projects/Delete' + params, doNotHandleFail);
            };
            Projects.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Projects/Insert', values, doNotHandleFail);
            };
            Projects.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Projects/InsertOrUpdate', values, doNotHandleFail);
            };
            Projects.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Projects/Update', values, doNotHandleFail);
            };
            Projects.UpdateActivities = function (ID, username, password, doNotHandleFail) {
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
            };
            Projects.GetFieldOptions = function (projectID, userID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Projects.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return Projects;
        }());
        WebApi.Projects = Projects;
        var Documents = /** @class */ (function () {
            function Documents() {
            }
            Documents.ByTask = function (tasks, filterByTaskItemType, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Documents.ByRevisionID = function (revisionSets, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Documents.ByResponse = function (ID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Documents.GeneralRequestDocuments = function (requestID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Documents.Read = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    params += '&id=' + id;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Documents/Read' + params, doNotHandleFail);
            };
            Documents.Download = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    params += '&id=' + id;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Documents/Download' + params, doNotHandleFail);
            };
            Documents.Upload = function (doNotHandleFail) {
                return Helpers.PostAPIValue('Documents/Upload', doNotHandleFail);
            };
            Documents.UploadChunked = function (doNotHandleFail) {
                return Helpers.PostAPIValue('Documents/UploadChunked', doNotHandleFail);
            };
            Documents.Delete = function (id, doNotHandleFail) {
                var params = '';
                if (id != null)
                    for (var j = 0; j < id.length; j++) {
                        params += '&id=' + id[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Documents/Delete' + params, doNotHandleFail);
            };
            return Documents;
        }());
        WebApi.Documents = Documents;
        var DataMartAvailability = /** @class */ (function () {
            function DataMartAvailability() {
            }
            DataMartAvailability.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return DataMartAvailability;
        }());
        WebApi.DataMartAvailability = DataMartAvailability;
        var DataModels = /** @class */ (function () {
            function DataModels() {
            }
            DataModels.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('DataModels/Get' + params, doNotHandleFail);
            };
            DataModels.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataModels.ListDataModelProcessors = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataModels.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataModels.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('DataModels/InsertOrUpdate', values, doNotHandleFail);
            };
            DataModels.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('DataModels/Update', values, doNotHandleFail);
            };
            DataModels.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('DataModels/Insert', values, doNotHandleFail);
            };
            DataModels.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('DataModels/Delete' + params, doNotHandleFail);
            };
            return DataModels;
        }());
        WebApi.DataModels = DataModels;
        var DataMarts = /** @class */ (function () {
            function DataMarts() {
            }
            DataMarts.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('DataMarts/Get' + params, doNotHandleFail);
            };
            DataMarts.GetByRoute = function (requestDataMartID, doNotHandleFail) {
                var params = '';
                if (requestDataMartID != null)
                    params += '&requestDataMartID=' + requestDataMartID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('DataMarts/GetByRoute' + params, doNotHandleFail);
            };
            DataMarts.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataMarts.ListBasic = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataMarts.DataMartTypeList = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataMarts.GetRequestTypesByDataMarts = function (DataMartId, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMarts/GetRequestTypesByDataMarts', DataMartId, doNotHandleFail);
            };
            DataMarts.GetInstalledModelsByDataMart = function (DataMartId, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            DataMarts.UninstallModel = function (model, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMarts/UninstallModel', model, doNotHandleFail);
            };
            DataMarts.Copy = function (datamartID, doNotHandleFail) {
                var params = '';
                if (datamartID != null)
                    params += '&datamartID=' + datamartID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('DataMarts/Copy' + params, doNotHandleFail);
            };
            DataMarts.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('DataMarts/Delete' + params, doNotHandleFail);
            };
            DataMarts.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMarts/Insert', values, doNotHandleFail);
            };
            DataMarts.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('DataMarts/InsertOrUpdate', values, doNotHandleFail);
            };
            DataMarts.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('DataMarts/Update', values, doNotHandleFail);
            };
            DataMarts.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            return DataMarts;
        }());
        WebApi.DataMarts = DataMarts;
        var Comments = /** @class */ (function () {
            function Comments() {
            }
            Comments.ByRequestID = function (requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Comments.ByDocumentID = function (documentID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Comments.GetDocumentReferencesByRequest = function (requestID, workflowActivityID, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Comments.AddWorkflowComment = function (value, doNotHandleFail) {
                return Helpers.PostAPIValue('Comments/AddWorkflowComment', value, doNotHandleFail);
            };
            Comments.GetPermissions = function (IDs, permissions, $filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Comments.Get = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    params += '&ID=' + ID;
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.GetAPIResult('Comments/Get' + params, doNotHandleFail);
            };
            Comments.List = function ($filter, $select, $orderby, $skip, $top, doNotHandleFail) {
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
            };
            Comments.InsertOrUpdate = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Comments/InsertOrUpdate', values, doNotHandleFail);
            };
            Comments.Update = function (values, doNotHandleFail) {
                return Helpers.PutAPIValue('Comments/Update', values, doNotHandleFail);
            };
            Comments.Insert = function (values, doNotHandleFail) {
                return Helpers.PostAPIValue('Comments/Insert', values, doNotHandleFail);
            };
            Comments.Delete = function (ID, doNotHandleFail) {
                var params = '';
                if (ID != null)
                    for (var j = 0; j < ID.length; j++) {
                        params += '&ID=' + ID[j];
                    }
                if (params.length > 0)
                    params = '?' + params.substr(1);
                return Helpers.DeleteAPIValue('Comments/Delete' + params, doNotHandleFail);
            };
            return Comments;
        }());
        WebApi.Comments = Comments;
        var Helpers = /** @class */ (function () {
            function Helpers() {
            }
            Helpers.RegisterFailMethod = function (method) {
                this.failMethod = method;
            };
            Helpers.FixStringDatesInResults = function (results) {
                var _this = this;
                results.forEach(function (data) {
                    for (var field in data) {
                        if (data[field]) {
                            if ($.isArray(data[field])) {
                                _this.FixStringDatesInResults(data[field]);
                            }
                            else if (data[field].substring && data[field].match(/^\d{4}-\d{2}-\d{2}T{1}\d{2}:\d{2}:\d{2}(\.\d*)?Z?$/g)) {
                                if (data[field].indexOf('Z') > -1) {
                                    data[field] = new Date(data[field]);
                                }
                                else {
                                    data[field] = new Date(data[field] + 'Z');
                                }
                            }
                        }
                    }
                });
            };
            Helpers.GetAPIResult = function (url, doNotHandleFail) {
                var _this = this;
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
                }).done(function (result) {
                    if (result == null || result.results == null) {
                        d.resolve();
                        return;
                    }
                    if (!$.isArray(result.results))
                        result.results = [result.results];
                    //Fix dates from strings into real dates.
                    _this.FixStringDatesInResults(result.results);
                    d.resolve(result.results);
                }).fail(function (e, description, error) {
                    if (_this.failMethod && !doNotHandleFail)
                        _this.failMethod(e);
                    d.reject(e);
                });
                return d;
            };
            Helpers.PostAPIValue = function (url, value, doNotHandleFail) {
                var _this = this;
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
                }).done(function (result) {
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
                }).fail(function (e) {
                    if (_this.failMethod && !doNotHandleFail)
                        _this.failMethod(e);
                    d.reject(e);
                });
                return d;
            };
            Helpers.PutAPIValue = function (url, value, doNotHandleFail) {
                var _this = this;
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
                }).done(function (result) {
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
                }).fail(function (e) {
                    if (_this.failMethod && !doNotHandleFail)
                        _this.failMethod(e);
                    d.reject(e);
                });
                return d;
            };
            Helpers.DeleteAPIValue = function (url, doNotHandleFail) {
                var _this = this;
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
                }).done(function (result) {
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
                }).fail(function (e) {
                    if (_this.failMethod && !doNotHandleFail)
                        _this.failMethod(e);
                    d.reject(e);
                });
                return d;
            };
            return Helpers;
        }());
        WebApi.Helpers = Helpers;
        var _SignalRConnection = null;
        function SignalRConnection() {
            if (_SignalRConnection == null) {
                _SignalRConnection = $.hubConnection(ServiceUrl + ' / signalr', null);
                _SignalRConnection.qs = { 'Auth': User.AuthToken };
                _SignalRConnection.start();
            }
            return _SignalRConnection;
        }
        WebApi.SignalRConnection = SignalRConnection;
        var RequestsHub = /** @class */ (function () {
            function RequestsHub() {
            }
            RequestsHub.Proxy = function () {
                if (this._proxy == null)
                    this._proxy = SignalRConnection().createHubProxy('RequestsHub');
                return this._proxy;
            };
            RequestsHub.NotifyCrud = function (NotifyFunction) {
                this.Proxy().on('NotifyCrud', NotifyFunction);
            };
            RequestsHub.ResultsReceived = function (OnResultsReceived) {
                this.Proxy().on('ResultsReceived', OnResultsReceived);
            };
            RequestsHub._proxy = null;
            return RequestsHub;
        }());
        WebApi.RequestsHub = RequestsHub;
    })(WebApi = Dns.WebApi || (Dns.WebApi = {}));
})(Dns || (Dns = {}));

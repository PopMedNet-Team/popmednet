/// <reference path="../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
var RootLayout;
(function (RootLayout) {
    $(function () {
        //Set all of the items with title tags to have qtip enabled
        var title = $('img[title!=""], span[title!=""]');
        title.tooltip({
            html: true,
            trigger: 'click',
            placement: 'auto bottom'
        });
        title.click(function (event) {
            event.preventDefault();
            event.stopPropagation();
        });
        Dns.WebApi.Helpers.RegisterFailMethod(function (e) {
            Global.Helpers.ShowErrorAlert("Application Error", e, 800);
        });
    });
    function LoadSpinner() {
        //var form = <any> $(".Content form");
        //if (!form.checkValidity || form.checkValidity()) {
        //    form.showLoadingSign({ foregroundClass: "BodyLoadingSign" });
        //}
    }
})(RootLayout || (RootLayout = {}));
var Users;
(function (Users) {
    function GetSetting(key) {
        var deferred = $.Deferred();
        GetSettings([key]).done(function (results) {
            if (results.length === 1) {
                deferred.resolve(results[0].Setting);
            }
            else {
                deferred.resolve(null);
            }
        });
        return deferred;
    }
    Users.GetSetting = GetSetting;
    function GetSettings(key) {
        var deferred = $.Deferred();
        Dns.WebApi.Users.GetSetting(key).done(function (results) {
            deferred.resolve(results);
        });
        return deferred;
    }
    Users.GetSettings = GetSettings;
    function SetSetting(Key, setting) {
        if (setting === Global.Session(Key)) {
            var deferred = $.Deferred();
            deferred.resolve();
            return deferred;
        }
        Global.Session(Key, setting);
        return Dns.WebApi.Users.SaveSetting({
            Key: Key,
            UserID: User.ID,
            Setting: setting
        });
    }
    Users.SetSetting = SetSetting;
    function ApplySettingsToGrid(grid, key) {
        var deferred = $.Deferred();
        Users.GetSetting(key).done(function (setting) {
            if (setting)
                Global.Helpers.SetGridFromSettings(grid, setting);
            Global.Helpers.WatchGridForChanges(grid, function () {
                Users.SetSetting(key, Global.Helpers.GetGridSettings(grid));
            });
            grid.dataSource.read();
            deferred.resolve();
        }).fail(function (e) {
            deferred.reject(e);
        });
        return deferred;
    }
    Users.ApplySettingsToGrid = ApplySettingsToGrid;
})(Users || (Users = {}));
var Permissions;
(function (Permissions) {
    var Group = (function () {
        function Group() {
        }
        return Group;
    }());
    Group.CreateProject = '93623C60-6425-40A0-91A0-01FA34920913';
    Group.Edit = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
    Group.Delete = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
    Group.View = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791';
    Group.ManageSecurity = '368E7007-E95F-435C-8FAF-0B9FBC9CA997';
    Group.ListProjects = '8C5E44DC-284E-45D8-A014-A0CD815883AE';
    Permissions.Group = Group;
    var Organization = (function () {
        function Organization() {
        }
        return Organization;
    }());
    Organization.Edit = '0B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
    Organization.Delete = '0C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
    Organization.View = '0CCB0EC2-006D-4345-895E-5DD2C6C8C791';
    Organization.ManageSecurity = '068E7007-E95F-435C-8FAF-0B9FBC9CA997';
    Organization.CreateUsers = 'AF37A115-9D40-4F38-8BAF-4B050AC6F185';
    Organization.ApproveRejectRegistrations = 'ECF3B864-7DB3-497B-A2E4-F2B435EF2803';
    Organization.CreateDataMarts = '135F153D-D0BE-4D51-B55C-4B8807E74584';
    Organization.CreateRegistries = '92F1A228-44E4-4A5A-9C78-0FC37F4B18C6';
    Organization.Copy = '64A00001-A1D6-41DD-AB20-A2B200EEB9A3';
    Permissions.Organization = Organization;
    var Project = (function () {
        function Project() {
        }
        return Project;
    }());
    Project.Copy = '25BD0001-4739-41D8-BC74-A2AF01733B64';
    Project.Edit = '4B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
    Project.Delete = '4C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
    Project.ManageSecurity = '468E7007-E95F-435C-8FAF-0B9FBC9CA997';
    Project.ManageRequestTypes = '25BD1111-4739-41D8-BC74-A2AF01733B64';
    Project.ListRequests = '8DCA22F0-EA18-4353-BA45-CC2692C7A844';
    Project.ResubmitRequests = 'B3D4266D-5DC6-497E-848F-567442F946F4';
    Project.EditRequestID = '43BF0001-4735-4598-BBAD-A4D801478AAA';
    Permissions.Project = Project;
    var User = (function () {
        function User() {
        }
        return User;
    }());
    User.Edit = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
    User.Delete = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
    User.View = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791';
    User.ManageSecurity = '268E7007-E95F-435C-8FAF-0B9FBC9CA997';
    User.ChangePassword = '4A7C9495-BB01-4EA7-9419-65ACE6B24865';
    User.ChangeLogin = '92687123-6F38-400E-97EC-C837AA92305F';
    User.ManageNotifications = '22FB4F13-0492-417F-ACA1-A1338F705748';
    User.ChangeCertificate = 'FDE2D32E-A045-4062-9969-00962E182367';
    Permissions.User = User;
    var DataMart = (function () {
        function DataMart() {
        }
        return DataMart;
    }());
    DataMart.Edit = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
    DataMart.Delete = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
    DataMart.View = '6CCB0EC2-006D-4345-895E-5DD2C6C8C791';
    DataMart.Copy = 'BB640001-5BA7-4658-93AF-A2B201579BFA';
    DataMart.InstallModels = '7710B3EA-B91E-4C85-978F-6BFCDE8C817C';
    DataMart.UninstallModels = 'D4770F67-7DB5-4D47-9413-CA1C777179C9';
    DataMart.ManageSecurity = '668E7007-E95F-435C-8FAF-0B9FBC9CA997';
    DataMart.ManageProjects = '6B42D2D8-F7A7-4119-9CC5-22991DC12AD3';
    Permissions.DataMart = DataMart;
    var Registry = (function () {
        function Registry() {
        }
        return Registry;
    }());
    Registry.Edit = '2B42D2E7-F7A7-4119-9CC5-22991DC12AD3';
    Registry.Delete = '2C019782-1B9D-48F8-9FCD-AC44BC6FD97B';
    Registry.View = '2CCB0FC2-006D-4345-895E-5DD2C6C8C791';
    Registry.ManageSecurity = '268F7007-E95F-435C-8FAF-0B9FBC9CA997';
    Permissions.Registry = Registry;
    var Portal = (function () {
        function Portal() {
        }
        return Portal;
    }());
    Portal.CreateGroup = '064FBC63-B8F1-4C31-B5AB-AB42DE5779C7';
    Portal.CreateOrganization = '5652252C-0265-4E47-8480-6FEF4690B7A5';
    Portal.CreateSharedFolders = 'E7EFB727-AE14-49D9-8D73-F691B00B8251';
    Portal.CreateRegistry = '39A642B4-E782-4051-9329-3A7246052E16';
    Portal.CreateTemplate = 'AE340001-020E-4E32-9E9F-A3B00134A862';
    Portal.CreateRequestType = 'AE341111-020E-4E32-9E9F-A3B00134A862';
    Permissions.Portal = Portal;
    var Templates = (function () {
        function Templates() {
        }
        return Templates;
    }());
    Templates.Edit = '47F80001-6C45-4144-A02B-A3B00131E7D6';
    Templates.Delete = '119B0001-59C8-4234-94BC-A3B00131EF63';
    Templates.View = 'E5A30001-3916-4223-9CB9-A3B00131F6DC';
    Templates.ManageSecurity = 'D3B50001-528C-4E85-BC1B-A3B00131FD69';
    Permissions.Templates = Templates;
    var RequestTypes = (function () {
        function RequestTypes() {
        }
        return RequestTypes;
    }());
    RequestTypes.Edit = '47F80021-6C45-4144-A02B-A3B00131E7D6';
    RequestTypes.Delete = '119B0021-59C8-4234-94BC-A3B00131EF63';
    RequestTypes.View = 'E5A30021-3916-4223-9CB9-A3B00131F6DC';
    RequestTypes.ManageSecurity = 'D3B50021-528C-4E85-BC1B-A3B00131FD69';
    Permissions.RequestTypes = RequestTypes;
    var ProjectRequestTypeWorkflowActivities = (function () {
        function ProjectRequestTypeWorkflowActivities() {
        }
        return ProjectRequestTypeWorkflowActivities;
    }());
    ProjectRequestTypeWorkflowActivities.ViewTask = 'DD20EE1B-C433-49F8-8A91-76AD10DB1BEC';
    ProjectRequestTypeWorkflowActivities.EditTask = '75FC4DEA-220C-486D-9E8C-AC2B6F6F8415';
    ProjectRequestTypeWorkflowActivities.ViewComments = '7025F490-9635-4540-B682-3A4F152E73EF';
    ProjectRequestTypeWorkflowActivities.AddComments = 'B03BDDE0-CD76-47C3-BB7D-C39A28B232B4';
    ProjectRequestTypeWorkflowActivities.ViewDocuments = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9';
    ProjectRequestTypeWorkflowActivities.AddDocuments = 'A593C7EC-61F3-42F8-8D26-8A4BACC8BC17';
    ProjectRequestTypeWorkflowActivities.ReviseDocuments = '0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045';
    ProjectRequestTypeWorkflowActivities.CloseTask = '32DC49AE-E845-4EA9-80CD-CC0281559443';
    ProjectRequestTypeWorkflowActivities.ViewRequestOverview = 'FFADFDE8-2ADA-488E-90AA-0AD29874A61B';
    ProjectRequestTypeWorkflowActivities.TerminateWorkflow = '712B3B5D-5115-40C0-AB5C-73132965902A';
    ProjectRequestTypeWorkflowActivities.EditRequestMetadata = '51A43BE0-290A-49D4-8278-ADE36706A80D';
    ProjectRequestTypeWorkflowActivities.ViewTrackingTable = '97850001-E880-40FB-AC98-A6C601592C15';
    Permissions.ProjectRequestTypeWorkflowActivities = ProjectRequestTypeWorkflowActivities;
    var Request = (function () {
        function Request() {
        }
        return Request;
    }());
    Request.ViewHistory = '0475D452-4B7A-4D3A-8295-4FC122F6A546';
    Request.AssignRequestLevelNotifications = '3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA';
    Request.OverrideDataMartRoutingStatus = '7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F';
    Request.ApproveRejectResponse = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6';
    Request.ChangeRoutingsAfterSubmission = 'FDEE0BA5-AC09-4580-BAA4-496362985BF7';
    Permissions.Request = Request;
})(Permissions || (Permissions = {}));
//# sourceMappingURL=_rootlayout.js.map
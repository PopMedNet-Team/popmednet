/// <reference path="../Scripts/page/Page.ts" />


module RootLayout {
    $(() => {
        //Set all of the items with title tags to have qtip enabled
        var title = $('img[title!=""], span[title!=""]');

        title.tooltip({
            html: true,
            trigger: 'click',
            placement: 'auto bottom'
        });

        title.click((event) => {
            event.preventDefault();
            event.stopPropagation();
        });


        Dns.WebApi.Helpers.RegisterFailMethod((e : JQueryXHR) => {
            Global.Helpers.ShowErrorAlert("Application Error", e, 800);
        });
    });

    function LoadSpinner() {
        //var form = <any> $(".Content form");
        //if (!form.checkValidity || form.checkValidity()) {
        //    form.showLoadingSign({ foregroundClass: "BodyLoadingSign" });
        //}
    }
}

module Users {
    export function GetSetting(key: string) : JQueryDeferred<string> {
        var deferred = $.Deferred<string>();

				GetSettings([key]).done(results => {
					if (results.length === 1) {
						deferred.resolve(results[0].Setting);
					} else {
						deferred.resolve(null);
					}
				});


	    return deferred;
	}

    export function GetSettings(key: Array<string>): JQueryDeferred<Dns.Interfaces.IUserSettingDTO[]> {
			var deferred = $.Deferred<Dns.Interfaces.IUserSettingDTO[]>();
      Dns.WebApi.Users.GetSetting(key).done((results) => {
        deferred.resolve(results);
			});
			return deferred;
		}

	export function SetSetting(Key: string, setting: string): JQueryDeferred<void> {
        if (setting === Global.Session(Key)) {
            var deferred: JQueryDeferred<void> = $.Deferred<void>();
            deferred.resolve();
            return deferred;
        }
            
        Global.Session(Key, setting);
        

        return <any> Dns.WebApi.Users.SaveSetting({
            Key: Key,
            UserID: User.ID,
            Setting: setting
        });
    }

    export function ApplySettingsToGrid(grid: kendo.ui.Grid, key: string) {

        var deferred = $.Deferred<void>();

        Users.GetSetting(key).done((setting) => {
            if (setting)
                Global.Helpers.SetGridFromSettings(grid, setting);

            Global.Helpers.WatchGridForChanges(grid, () => {
                Users.SetSetting(key, Global.Helpers.GetGridSettings(grid));
            });
            grid.dataSource.read();
            deferred.resolve();
        }).fail((e) => {
            deferred.reject(e);
            });

        return deferred;
    }
}

module Permissions {
    export class Group {
        public static CreateProject: any = '93623C60-6425-40A0-91A0-01FA34920913';
        public static Edit: any = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static View: any = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791';
        public static ManageSecurity: any  = '368E7007-E95F-435C-8FAF-0B9FBC9CA997';
        public static ListProjects: any = '8C5E44DC-284E-45D8-A014-A0CD815883AE';

    }

    export class Organization {
        public static Edit: any = '0B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '0C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static View: any = '0CCB0EC2-006D-4345-895E-5DD2C6C8C791';
        public static ManageSecurity: any = '068E7007-E95F-435C-8FAF-0B9FBC9CA997';
        public static CreateUsers: any = 'AF37A115-9D40-4F38-8BAF-4B050AC6F185';
        public static ApproveRejectRegistrations: any = 'ECF3B864-7DB3-497B-A2E4-F2B435EF2803';
        public static CreateDataMarts: any = '135F153D-D0BE-4D51-B55C-4B8807E74584';
        public static CreateRegistries: any = '92F1A228-44E4-4A5A-9C78-0FC37F4B18C6';
        public static Copy: any = '64A00001-A1D6-41DD-AB20-A2B200EEB9A3';
    }

    export class Project {
        public static Copy : any = '25BD0001-4739-41D8-BC74-A2AF01733B64';
        public static Edit: any = '4B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '4C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static ManageSecurity: any = '468E7007-E95F-435C-8FAF-0B9FBC9CA997';
        public static ManageRequestTypes: any = '25BD1111-4739-41D8-BC74-A2AF01733B64';
        public static ListRequests: any = '8DCA22F0-EA18-4353-BA45-CC2692C7A844';
        public static ResubmitRequests: any = 'B3D4266D-5DC6-497E-848F-567442F946F4';
        public static EditRequestID: any = '43BF0001-4735-4598-BBAD-A4D801478AAA';       
    }

    export class User {
        public static Edit: any = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static View: any = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791';
        public static ManageSecurity: any = '268E7007-E95F-435C-8FAF-0B9FBC9CA997';
        public static ChangePassword: any = '4A7C9495-BB01-4EA7-9419-65ACE6B24865';
        public static ChangeLogin: any = '92687123-6F38-400E-97EC-C837AA92305F';
        public static ManageNotifications: any = '22FB4F13-0492-417F-ACA1-A1338F705748';
        public static ChangeCertificate: any = 'FDE2D32E-A045-4062-9969-00962E182367';
    }

    export class DataMart {
        public static Edit: any = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static View: any = '6CCB0EC2-006D-4345-895E-5DD2C6C8C791';
        public static Copy: any = 'BB640001-5BA7-4658-93AF-A2B201579BFA';
        public static InstallModels: any = '7710B3EA-B91E-4C85-978F-6BFCDE8C817C';
        public static UninstallModels: any = 'D4770F67-7DB5-4D47-9413-CA1C777179C9';
        public static ManageSecurity: any = '668E7007-E95F-435C-8FAF-0B9FBC9CA997';
        public static ManageProjects: any = '6B42D2D8-F7A7-4119-9CC5-22991DC12AD3';
    }

    export class Registry {
        public static Edit: any = '2B42D2E7-F7A7-4119-9CC5-22991DC12AD3';
        public static Delete: any = '2C019782-1B9D-48F8-9FCD-AC44BC6FD97B';
        public static View: any = '2CCB0FC2-006D-4345-895E-5DD2C6C8C791';
        public static ManageSecurity: any = '268F7007-E95F-435C-8FAF-0B9FBC9CA997';
    }

    export class Portal {
        public static CreateGroup: any = '064FBC63-B8F1-4C31-B5AB-AB42DE5779C7';
        public static CreateOrganization: any = '5652252C-0265-4E47-8480-6FEF4690B7A5';
        public static CreateSharedFolders: any = 'E7EFB727-AE14-49D9-8D73-F691B00B8251';
        public static CreateRegistry: any = '39A642B4-E782-4051-9329-3A7246052E16';
        public static CreateTemplate: any = 'AE340001-020E-4E32-9E9F-A3B00134A862';
        public static CreateRequestType: any = 'AE341111-020E-4E32-9E9F-A3B00134A862';
    }

    export class Templates {
        public static Edit: any = '47F80001-6C45-4144-A02B-A3B00131E7D6';
        public static Delete: any = '119B0001-59C8-4234-94BC-A3B00131EF63';
        public static View: any = 'E5A30001-3916-4223-9CB9-A3B00131F6DC';
        public static ManageSecurity: any = 'D3B50001-528C-4E85-BC1B-A3B00131FD69';
    }

    export class RequestTypes {
        public static Edit: any = '47F80021-6C45-4144-A02B-A3B00131E7D6';
        public static Delete: any = '119B0021-59C8-4234-94BC-A3B00131EF63';
        public static View: any = 'E5A30021-3916-4223-9CB9-A3B00131F6DC';
        public static ManageSecurity: any = 'D3B50021-528C-4E85-BC1B-A3B00131FD69';
    }

    export class ProjectRequestTypeWorkflowActivities {
        public static ViewTask: any = 'DD20EE1B-C433-49F8-8A91-76AD10DB1BEC';
        public static EditTask: any = '75FC4DEA-220C-486D-9E8C-AC2B6F6F8415';
        public static ViewComments: any = '7025F490-9635-4540-B682-3A4F152E73EF';
        public static AddComments: any = 'B03BDDE0-CD76-47C3-BB7D-C39A28B232B4';
        public static ModifyAttachments: any = 'D59FA0D4-15FA-4088-9A98-35CDD7902EC1';
        public static ViewAttachments: any = '50157D72-8EED-45E4-B6F4-2A935191F57F';
        public static ViewDocuments: any = 'FAE8FC24-362D-4382-AF31-0933AF95FDE9';
        public static AddDocuments: any = 'A593C7EC-61F3-42F8-8D26-8A4BACC8BC17';
        public static ReviseDocuments: any = '0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045';
        public static CloseTask: any = '32DC49AE-E845-4EA9-80CD-CC0281559443';
        public static ViewRequestOverview: any = 'FFADFDE8-2ADA-488E-90AA-0AD29874A61B';
        public static TerminateWorkflow: any = '712B3B5D-5115-40C0-AB5C-73132965902A';
        public static EditRequestMetadata: any = '51A43BE0-290A-49D4-8278-ADE36706A80D';
        public static ViewTrackingTable: any = '97850001-E880-40FB-AC98-A6C601592C15';
    }

    export class Request {
        public static ViewHistory: any = '0475D452-4B7A-4D3A-8295-4FC122F6A546';
        public static AssignRequestLevelNotifications: any = '3EB92A0A-5A9B-4860-898D-E32ACC2D5EEA';
        public static OverrideDataMartRoutingStatus: any = '7A401F1F-46C2-4F6F-9FAE-AE94A6DDB21F';
        public static ApproveRejectResponse: any = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6';
        public static ChangeRoutingsAfterSubmission: any = 'FDEE0BA5-AC09-4580-BAA4-496362985BF7';
        public static SkipSubmissionApproval: any = '39683790-A857-4247-85DF-A9B425AC79CC';
    }
}
/// <reference path="../_rootlayout.ts" />

// Unused for the most part. Still relies on MVC because of use of Plugin.DisplayResponse
// and Html.BodyView.
module Home.MetadataList {
    export var RawModel: any = null;
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public dsMetadata: kendo.data.DataSource;
        public metadata: KnockoutObservableArray<any>;
        public dmMetadataModel: KnockoutObservableArray<IViewModelData>;

        constructor(/*rawModel: IViewModelData[],*/ bindingControl: JQuery) {
            super(bindingControl);
            var self = this;

            //this.dmMetadataModel = ko.observableArray(rawModel);
            this.metadata = ko.observableArray([{ DataMart: 'Sample DM 1', Name: 'Sample Metadata Name', Type: 'Sample Type' }]);
            //this.dsMetadata = new kendo.data.DataSource({
            //    type: "webapi",
            //    serverPaging: true,
            //    serverSorting: true,
            //    serverFiltering: true,
            //    pageSize: 100,
            //    transport: {
            //        read: {
            //            url: Global.Helpers.GetServiceUrl("/users/getnotifications?UserID=" + User.ID /*+ "&$top=5"*/),
            //        }
            //    },
            //    schema: {
            //        model: kendo.data.Model.define(Dns.Interfaces.KendoModelNotificationDTO)
            //    },
            //    sort: {
            //        field: "DataMart", dir: "desc"
            //    }
            //});     
        }

    }

    //export function NameAnchor(dataItem: Dns.Interfaces.IRequestDTO): string {
    //    return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
    //}

    export interface IViewModelData {
        DataMartName: string;
        ModelMetadataList: IModelMetadata[]
    }

    export interface IModelMetadata {
        ModelName: string;
        Responses: IRequestTypeResponse[];
    }

    export interface IRequestTypeResponse {
        RequestTypeName: string;
        BodyView: string;
    }

    export interface IRequestType {
        Name: string;
    }

    function init() {
        $(() => {

            var bindingControl = $("#Content");
            vm = new ViewModel(/*RawModel,*/ bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}
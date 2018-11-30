 /// <reference path="../../Scripts/page/Page.ts" />

module Security.SecurityGroupWindow {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public dsOrgResults: kendo.data.DataSource;
        public dsOrgSecResults: kendo.data.DataSource;
        public dsProjResults: kendo.data.DataSource;
        public dsProjSecResults: kendo.data.DataSource;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            this.dsOrgResults = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/organizations/list"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "Name", dir: "asc" },
            });

            this.dsOrgSecResults = new kendo.data.DataSource({
                data: []
            });

            this.dsProjResults = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl("/projects/list?$select=ID,Name,Group, GroupID,Description"),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                },
                sort: { field: "Name", dir: "asc" },
            });

            this.dsProjSecResults = new kendo.data.DataSource({
                data: []
            });
                 
        }

        public AddOrganization(arg: kendo.ui.GridChangeEvent) {
            
            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);
                Dns.WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done((results) => {
                    $("#gOrgSecResults").data("kendoGrid").dataSource.data(results);
                    $("#gOrgSecResults").data("kendoGrid").refresh();
                })
            });
        }
        public AddProject(arg: kendo.ui.GridChangeEvent) {

            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);
                Dns.WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done((results) => {
                    $("#gProjSecResults").data("kendoGrid").dataSource.data(results);
                    $("#gProjSecResults").data("kendoGrid").refresh();
                })
            });
        }

        public AddOrgGrp(arg: kendo.ui.GridChangeEvent) {
            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);
                vm.Close(dataItem);   
            });     
        }

        public AddProjectGrp(arg: kendo.ui.GridChangeEvent) {
            $.each(arg.sender.select(), (count: number, item: JQuery) => {
                var dataItem: any = arg.sender.dataItem(item);
                vm.Close(dataItem);
            });
        }

    }
    
    function init() {
            $(() => {
                var bindingControl = $("body");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
    }

    init();
}
import * as Global from "../../scripts/page/global.js";
import * as Interfaces from "../Dns.Interfaces.js";
import * as WebApi from '../Lpp.Dns.WebApi.js';
import PMNGridDataSource from "../../scripts/PmnGrid/PMNGridDataSource.js";


export class ViewModel extends Global.DialogViewModel {
    public dsOrgResults: PMNGridDataSource<typeof Interfaces.KendoModelOrganizationDTO>;
    public dsOrgSecResults: kendo.data.DataSource;
    public dsProjResults: PMNGridDataSource<typeof Interfaces.KendoModelProjectDTO>;
    public dsProjSecResults: kendo.data.DataSource;


    constructor(bindingControl: JQuery) {
        super(bindingControl);

        this.dsOrgResults = new PMNGridDataSource<typeof Interfaces.KendoModelOrganizationDTO>(Global.Helpers.GetServiceUrl("/organizations/list?$select=ID,Timestamp,Acronym,Name"), { field: "Name", dir: "asc" }, null, Interfaces.KendoModelOrganizationDTO);

        this.dsOrgSecResults = new kendo.data.DataSource({
            data: []
        });

        this.dsProjResults = new PMNGridDataSource<typeof Interfaces.KendoModelProjectDTO>(Global.Helpers.GetServiceUrl("/projects/list?$select=ID,Name,Group, GroupID,Description"), { field: "Name", dir: "asc" }, null, Interfaces.KendoModelProjectDTO);

        this.dsProjSecResults = new kendo.data.DataSource({
            data: []
        });       

    }

    public AddOrganization(arg: kendo.ui.GridChangeEvent) {
        $.each(arg.sender.select(), (count: number, item: JQuery) => {
            let dataItem: any = arg.sender.dataItem(item);
            WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done((results) => {
                $("#gOrgSecResults").data("kendoGrid").dataSource.data(results);
                $("#gOrgSecResults").data("kendoGrid").refresh();
            })
        });
    }
    public AddProject(arg: kendo.ui.GridChangeEvent) {
        $.each(arg.sender.select(), (count: number, item: JQuery) => {
            let dataItem: any = arg.sender.dataItem(item);
            WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done((results) => {
                $("#gProjSecResults").data("kendoGrid").dataSource.data(results);
                $("#gProjSecResults").data("kendoGrid").refresh();
            })
        });
    }

    public AddOrgGrp(arg: kendo.ui.GridChangeEvent) {
        let self = this;
        $.each(arg.sender.select(), (count: number, item: JQuery) => {
            let dataItem: any = arg.sender.dataItem(item);
            self.Close(dataItem);
        });
    }

    public AddProjectGrp(arg: kendo.ui.GridChangeEvent) {
        let self = this;
        $.each(arg.sender.select(), (count: number, item: JQuery) => {
            let dataItem: any = arg.sender.dataItem(item);
            self.Close(dataItem);
        });
    }

}

function init() {
    $(() => {
        let bindingControl = $("body");
        let vm = new ViewModel(bindingControl);
        ko.applyBindings(vm, bindingControl[0]);

        $("#gOrgSecResults").data("kendoGrid").bind("change", vm.AddOrgGrp.bind(vm));
        $("#gProjSecResults").data("kendoGrid").bind("change", vm.AddProjectGrp.bind(vm));
        
    });
}

init();

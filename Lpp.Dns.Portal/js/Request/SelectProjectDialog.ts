/// <reference path="../../Scripts/page/Page.ts" />

module Requests.Utility {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {

        public Projects: Dns.Interfaces.IProjectDTO[];

        constructor(projects: Dns.Interfaces.IProjectDTO[], bindingControl: JQuery) {
            super(bindingControl);

            this.Projects = projects;
        }

        public onSelectProject(project: Dns.Interfaces.IProjectDTO) {
            vm.Close({ ID: project.ID, Name: project.Name });
        }
    }

    export function init() {
        $(() => {
            var window: kendo.ui.Window = Global.Helpers.GetDialogWindow();
            var projects = (<any>(window.options)).parameters.Projects;
            var bindingControl = $("#Content");
            vm = new ViewModel(projects, bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
} 
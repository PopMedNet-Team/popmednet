/// <reference path="../../../js/_layout.ts" />
module Tests.KendoGridFilteringTest {

    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {

        public dsRequest: kendo.data.DataSource;
        public gRequestsHeight: KnockoutObservable<string>;

        public onColumnMenuInit: (e: any) => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);


            this.gRequestsHeight = ko.observable<string>(null);
            this.dsRequest = new kendo.data.DataSource({
                type: "webapi",
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 500,
                transport: {
                    read: {
                        url: Global.Helpers.GetServiceUrl('/requests/listforhomepage'),
                    }
                },
                schema: {
                    model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRequestDetailDTO)
                },
                sort: { field: "SubmittedOn", dir: "desc" },
                change: (e) => {
                    vm.gRequestsHeight(e.items != null && e.items.length > 0 ? "600px" : "34px");
                }
            });


            this.onColumnMenuInit = (e) => {
                var menu = e.container.find(".k-menu").data("kendoMenu");
                menu.bind("close", (e) => {
                });
            };
        }
    }

    export function init() {
        $(() => {
            var bindingControl = $('#Content');
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    export function NameAnchor(dataItem: Dns.Interfaces.IHomepageRequestDetailDTO): string {
        if (dataItem.IsWorkflowRequest) {
            return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        } else {
            return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
    }

    export function SubjectAnchor(dataItem: Dns.Interfaces.ITaskDTO): string {
        return "<a href=\"/tasks/details?ID=" + dataItem.ID + "\">" + dataItem.Subject + "</a>";
    }

    init();

} 
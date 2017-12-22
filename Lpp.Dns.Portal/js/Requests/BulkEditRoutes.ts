 
module Requests.BulkEditRoutes {

    export class ViewModel extends Global.PageViewModel {
        public DataMart: Dns.Interfaces.IDataMartDTO;
        public dsRequest: kendo.data.DataSource;
        public BulkEditEnabled: KnockoutObservable<boolean>;

        public formatDueDateCell: (item: any) => string;
        public formatPriorityCell: (item: any) => string;
        public onApplyChanges: () => void;
        public onBulkEdit: () => void;
        public onRowSelectionChange: (e) => void;
        public onColumnMenuInit: (e: any) => void;
        public SaveGridSettings: () => void;

        private DueDateTemplate: (data: any) => string = kendo.template('<input value=\'#= DueDate #\' data-bind=\'value:DueDate\' data-role=\'datepicker\' />');
        private PriorityTemplate: (data: any) => string = kendo.template('<input value=\'#= Priority #\' data-bind=\'value:Priority\' data-role=\'dropdownlist\' data-source=\'Dns.Enums.PrioritiesTranslation\' data-text-field=\'text\' data-value-field=\'value\' />');

        constructor(routes: any[], datamart: Dns.Interfaces.IDataMartDTO, bindingControl: JQuery, screenPermissions: any[], gridSetting: string) {
            super(bindingControl, screenPermissions);
            var self = this;

            this.DataMart = datamart;
            this.dsRequest = kendo.data.DataSource.create(
                {
                    data: [],
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRouteDetailDTO)
                    }
                }
            );

            this.BulkEditEnabled = ko.observable(false);

            this.onRowSelectionChange = (e: any) => {
                var grid = $(e.sender.wrapper).data('kendoGrid');
                var rows = grid.select();

                self.BulkEditEnabled(rows.length > 0);
            };

            var filter = ko.utils.arrayMap(routes, (id: string) => 'RequestDataMartID eq ' + id).join(' or ');
            Dns.WebApi.Requests.RequestsByRoute(filter, null, "SubmittedOn desc").done((requests: any) => {
                if (requests == null)
                    return;

                var models = ko.utils.arrayMap(requests, (r: any) => new RouteDetail(r));
                self.dsRequest.data(models);

            });

            self.formatDueDateCell = (item: any) => self.DueDateTemplate(item);
            self.formatPriorityCell = (item: any) => self.PriorityTemplate(item);

            self.onBulkEdit = () => {
                var grid = $('#gRequests').data('kendoGrid');
                var rows = grid.select();

                if (rows.length == 0)
                    return;

                Global.Helpers.ShowDialog("Edit Metadata Values", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 650, 415, { defaultPriority: Dns.Enums.Priorities.Medium, defaultDueDate: null, isRequestLevel: false })
                    .done((result: any) => {

                        if (result == null)
                            return;


                        self.dsRequest.data().forEach((rq: RouteDetail) => {

                            if (result.UpdatePriority) {
                                rq.Priority(result.PriorityValue);
                            }
                            if (result.UpdateDueDate) {
                                rq.DueDate(result.DueDateValue);
                            }

                        });

                        grid.refresh();
                        self.BulkEditEnabled(false);
                    });

            };

            self.onApplyChanges = () => {

                Global.Helpers.ShowExecuting();

                var changed = self.dsRequest.data().filter((req: RouteDetail) => req.Changed());
                if (changed.length == 0) {
                    Global.Helpers.HideExecuting();
                    Global.Helpers.ShowAlert('No Changes to Update', '<p class="alert alert-info" style="text-align:center">There were no changes to save for the requests being editted.</p>');
                    return;
                }

                var updateArgs = ko.utils.arrayMap(changed, (i: RouteDetail) => {
                    var rx = new Dns.ViewModels.RequestDataMartViewModel();
                    rx.ID(i.RequestDataMartID());
                    rx.Priority(i.Priority());
                    rx.DueDate(i.DueDate());
                    return rx.toData();
                });

                Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(updateArgs).done(() => {
                    location.href = '/';
                }).fail((err) => {
                        Global.Helpers.HideExecuting();
                        var description: string = (err.responseJSON.errors[0].Description).toString();
                        Global.Helpers.ShowErrorAlert('Error Updating Request DataMart Metadata', '<p>There was an error updating the metadata for the changed requests:</p><p class="alert alert-danger">' + description.replace('\r\n', '<br/>') + '</p>');

                    });

            };

            this.SaveGridSettings = () => {
                Users.SetSetting("Requests.BulkEditRoutes.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(self.RequestsGrid()));
            };

            this.onColumnMenuInit = (e) => {
                var menu = e.container.find(".k-menu").data("kendoMenu");
                menu.bind("close", (e) => {
                    self.SaveGridSettings();
                });
            };

        }

        public onGridDataBound(e) {
            var grid = e.sender as kendo.ui.Grid;
            grid.tbody.find('tr').each((i: number, elem: Element) => {
                var item = grid.dataItem(elem);
                kendo.bind(elem, item);
            });
        }

        public RequestsGrid(): kendo.ui.Grid {
            return $("#gRequests").data("kendoGrid");
        }
    }

    export class RouteDetail extends Dns.ViewModels.HomepageRouteDetailViewModel {
        public Changed: KnockoutComputed<boolean>;
        private _originalValues: any;

        constructor(data: Dns.Interfaces.IHomepageRouteDetailDTO) {
            super(data);
            var self = this;

            this._originalValues = {
                Priority: data.Priority,
                DueDate: data.DueDate
            };

            this.Changed = ko.computed(() => {
                return self.Priority() != self._originalValues.Priority || self.DueDate() != self._originalValues.DueDate;
            }, this, { pure: true });
        }
    }

    export function init() {
        var routeID = $.url().param('r').split(',');

        $.when<any>(
            Users.GetSetting('Requests.BulkEditRoutes.gRequests.User:' + User.ID),
            Dns.WebApi.DataMarts.GetByRoute(routeID[0])
        ).done((gridSetting: string, datamarts: Dns.Interfaces.IDataMartDTO[]) => {

            var bindingControl = $("#Content");
            var vm = new ViewModel(routeID, datamarts[0], bindingControl, [], gridSetting);

            $(() => {

                ko.applyBindings(vm, bindingControl[0]);

                $(window).unload(() => {
                    vm.SaveGridSettings();
                });

                Global.Helpers.SetGridFromSettings(vm.RequestsGrid(), gridSetting);

            });
        });

        
    }

    init();
}
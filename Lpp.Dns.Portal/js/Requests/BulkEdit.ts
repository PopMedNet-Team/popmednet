
module Requests.BulkEdit {
    export class ViewModel extends Global.PageViewModel {
        public dsRequest: kendo.data.DataSource;
        public BulkEditEnabled: KnockoutObservable<boolean>;
        public SaveEnabled: KnockoutObservable<boolean>;

        public formatDueDateCell: (item: any) => string;
        public formatPriorityCell: (item: any) => string;    
        public onApplyChanges: () => void;
        public onBulkEdit: () => void;
        public onRowSelectionChange: (e) => void;
        public onColumnMenuInit: (e: any) => void;
        public SaveGridSettings: () => void;

        public selectionChanged: (e: any) => void;

        private DueDateTemplate: (data: any) => string = kendo.template('<input value=\'#= DueDate #\' data-bind=\'value:DueDate\' data-role=\'datepicker\' />');
        private PriorityTemplate: (data: any) => string = kendo.template('<input value=\'#= Priority #\' data-bind=\'value:Priority\' data-role=\'dropdownlist\' data-source=\'Dns.Enums.PrioritiesTranslation\' data-text-field=\'text\' data-value-field=\'value\' />');

        

        constructor(requestID: string[], bindingControl: JQuery, screenPermissions: any[], gridSetting:string) {
            super(bindingControl, screenPermissions);
            var self = this;
            
            this.dsRequest = kendo.data.DataSource.create(
                {
                    data: [],
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestDTO)
                    }
                }
            );

            this.BulkEditEnabled = ko.observable(false);
            this.SaveEnabled = ko.observable(false);

            this.onRowSelectionChange = (e: any) => {
                var grid = $(e.sender.wrapper).data('kendoGrid');
                var rows = grid.select();

                self.BulkEditEnabled(rows.length > 0);
            };

            var filter = ko.utils.arrayMap(requestID, (id: string) => 'ID eq ' + id).join(' or ');
            Dns.WebApi.Requests.List(filter, "ID,Name,Identifier,MajorEventDate,MajorEventBy,SubmittedOn,Status,StatusText,RequestType,Project,Priority,DueDate,MSRequestID,SubmittedByName", "SubmittedOn desc").done((requests: any) => {
                if (requests == null)
                    return;

                var models = ko.utils.arrayMap(requests, (r: any) => new Request(r));
                self.dsRequest.data(models);

            });

            $(document).on("RequestChanged", function () {
              self.SaveEnabled(true);
            });

            self.formatDueDateCell = (item: any) => self.DueDateTemplate(item);
            self.formatPriorityCell = (item: any) => self.PriorityTemplate(item);

            self.onBulkEdit = () => {
                var grid = $('#gRequests').data('kendoGrid');
                var rows = grid.select();
                
                if (rows.length == 0)
                    return;

                Global.Helpers.ShowDialog("Edit Metadata Values", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 650, 415, { defaultPriority: Dns.Enums.Priorities.Medium, defaultDueDate: null, isRequestLevel: true })
                    .done((result: any) => {

                        if (result == null)
                            return;


                        self.dsRequest.data().forEach((rq: Request) => {
                            
                            if (result.UpdatePriority) {
                                rq.Priority(result.PriorityValue);
                            }
                            if (result.UpdateDueDate) {
                                rq.DueDate(result.DueDateValue);
                            }
                            rq.ApplyChangesToRoutings(result.ApplyToRoutings);
                        });

                        grid.refresh();
                        self.BulkEditEnabled(false);
                    });

            };

            self.onApplyChanges = () => {

                Global.Helpers.ShowExecuting();

                var changed = self.dsRequest.data().filter((req: Request) => req.Changed());
                if (changed.length == 0) {
                    Global.Helpers.HideExecuting();
                    Global.Helpers.ShowAlert('No Changes to Update', '<p class="alert alert-info" style="text-align:center">There were no changes to save for the requests being editted.</p>');
                    return;
                }                    
                
                var updateArgs = ko.utils.arrayMap(changed, (i: Request) => {
                    var rx = new Dns.ViewModels.RequestMetadataViewModel();
                    rx.ID(i.ID);
                    rx.Priority(i.Priority());
                    rx.DueDate(i.DueDate());
                    rx.ApplyChangesToRoutings(i.ApplyChangesToRoutings());
                    return rx.toData();
                });

                Dns.WebApi.Requests.UpdateMetadataForRequests(updateArgs, true).done(() => {
                    location.href = '/';
                }).fail((err) => {
                        Global.Helpers.HideExecuting();
                        var description: string = (err.responseJSON.errors[0].Description).toString();                    
                        Global.Helpers.ShowErrorAlert('Error Updating Request Metadata', '<p>There was an error updating the metadata for the changed requests:</p><p class="alert alert-danger">' + description.replace('\r\n','<br/>') + '</p>');
                    });
                
            };

            this.SaveGridSettings = () => {
                Users.SetSetting("Requests.BulkEdit.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(self.RequestsGrid()));
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

    export class Request {
        public ID: any;
        public Name: string;
        public Identifier: string;
        public SubmittedOn: Date;
        public DueDate: KnockoutObservable<any>;
        public Priority: KnockoutObservable<any>;
        public Status: number;
        public StatusText: any;
        public RequestType: string;
        public Project: string;
        public MSRequestID: string;
        public SubmittedByName: string;
        private _request: any;
        public Changed: KnockoutComputed<boolean>;
        public ApplyChangesToRoutings: KnockoutObservable<boolean>;

        constructor(request: any) {
            var self = this;
            this._request = request;
            this.ID = request.ID;
            this.Name = request.Name;
            this.Identifier = request.Identifier;
            this.SubmittedOn = request.SubmittedOn;
            this.DueDate = ko.observable(request.DueDate);
            this.Priority = ko.observable(request.Priority);
            this.Status = request.Status;
            this.StatusText = request.StatusText;
            this.RequestType = request.RequestType;
            this.Project = request.Project;
            this.MSRequestID = request.MSRequestID;
            this.SubmittedByName = request.SubmittedByName;

            this.Changed = ko.computed(() => {
                return self.Priority() != self._request.Priority || self.DueDate() != self._request.DueDate;
            }, this, { pure: true });

            this.Changed.subscribe((val) => {
              if (val)
                $(document).trigger("RequestChanged");
            });

            this.ApplyChangesToRoutings = ko.observable(false);
        }
    }

    export function init() {

        $.when(Users.GetSetting("Requests.BulkEdit.gRequests.User:" + User.ID)).done((gridSetting:string) => {

            var requestID = $.url().param('r').split(',');

            var bindingControl = $("#Content");
            var vm = new ViewModel(requestID, bindingControl, [], gridSetting);

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
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Requests;
(function (Requests) {
    var BulkEdit;
    (function (BulkEdit) {
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(requestID, bindingControl, screenPermissions, gridSetting) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.DueDateTemplate = kendo.template('<input value=\'#= DueDate #\' data-bind=\'value:DueDate\' data-role=\'datepicker\' />');
                _this.PriorityTemplate = kendo.template('<input value=\'#= Priority #\' data-bind=\'value:Priority\' data-role=\'dropdownlist\' data-source=\'Dns.Enums.PrioritiesTranslation\' data-text-field=\'text\' data-value-field=\'value\' />');
                var self = _this;
                _this.dsRequest = kendo.data.DataSource.create({
                    data: [],
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestDTO)
                    }
                });
                _this.BulkEditEnabled = ko.observable(false);
                _this.SaveEnabled = ko.observable(false);
                _this.onRowSelectionChange = function (e) {
                    var grid = $(e.sender.wrapper).data('kendoGrid');
                    var rows = grid.select();
                    self.BulkEditEnabled(rows.length > 0);
                };
                var filter = ko.utils.arrayMap(requestID, function (id) { return 'ID eq ' + id; }).join(' or ');
                Dns.WebApi.Requests.List(filter, "ID,Name,Identifier,MajorEventDate,MajorEventBy,SubmittedOn,Status,StatusText,RequestType,Project,Priority,DueDate,MSRequestID,SubmittedByName", "SubmittedOn desc").done(function (requests) {
                    if (requests == null)
                        return;
                    var models = ko.utils.arrayMap(requests, function (r) { return new Request(r); });
                    self.dsRequest.data(models);
                });
                $(document).on("RequestChanged", function () {
                    self.SaveEnabled(true);
                });
                self.formatDueDateCell = function (item) { return self.DueDateTemplate(item); };
                self.formatPriorityCell = function (item) { return self.PriorityTemplate(item); };
                self.onBulkEdit = function () {
                    var grid = $('#gRequests').data('kendoGrid');
                    var rows = grid.select();
                    if (rows.length == 0)
                        return;
                    Global.Helpers.ShowDialog("Edit Metadata Values", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 650, 415, { defaultPriority: Dns.Enums.Priorities.Medium, defaultDueDate: null, isRequestLevel: true })
                        .done(function (result) {
                        if (result == null)
                            return;
                        self.dsRequest.data().forEach(function (rq) {
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
                self.onApplyChanges = function () {
                    Global.Helpers.ShowExecuting();
                    var changed = self.dsRequest.data().filter(function (req) { return req.Changed(); });
                    if (changed.length == 0) {
                        Global.Helpers.HideExecuting();
                        Global.Helpers.ShowAlert('No Changes to Update', '<p class="alert alert-info" style="text-align:center">There were no changes to save for the requests being editted.</p>');
                        return;
                    }
                    var updateArgs = ko.utils.arrayMap(changed, function (i) {
                        var rx = new Dns.ViewModels.RequestMetadataViewModel();
                        rx.ID(i.ID);
                        rx.Priority(i.Priority());
                        rx.DueDate(i.DueDate());
                        rx.ApplyChangesToRoutings(i.ApplyChangesToRoutings());
                        return rx.toData();
                    });
                    Dns.WebApi.Requests.UpdateMetadataForRequests(updateArgs, true).done(function () {
                        location.href = '/';
                    }).fail(function (err) {
                        Global.Helpers.HideExecuting();
                        var description = (err.responseJSON.errors[0].Description).toString();
                        Global.Helpers.ShowErrorAlert('Error Updating Request Metadata', '<p>There was an error updating the metadata for the changed requests:</p><p class="alert alert-danger">' + description.replace('\r\n', '<br/>') + '</p>');
                    });
                };
                _this.SaveGridSettings = function () {
                    Users.SetSetting("Requests.BulkEdit.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(self.RequestsGrid()));
                };
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.SaveGridSettings();
                    });
                };
                return _this;
            }
            ViewModel.prototype.onGridDataBound = function (e) {
                var grid = e.sender;
                grid.tbody.find('tr').each(function (i, elem) {
                    var item = grid.dataItem(elem);
                    kendo.bind(elem, item);
                });
            };
            ViewModel.prototype.RequestsGrid = function () {
                return $("#gRequests").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        BulkEdit.ViewModel = ViewModel;
        var Request = /** @class */ (function () {
            function Request(request) {
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
                this.Changed = ko.computed(function () {
                    return self.Priority() != self._request.Priority || self.DueDate() != self._request.DueDate;
                }, this, { pure: true });
                this.Changed.subscribe(function (val) {
                    if (val)
                        $(document).trigger("RequestChanged");
                });
                this.ApplyChangesToRoutings = ko.observable(false);
            }
            return Request;
        }());
        BulkEdit.Request = Request;
        function init() {
            $.when(Users.GetSetting("Requests.BulkEdit.gRequests.User:" + User.ID)).done(function (gridSetting) {
                var requestID = $.url().param('r').split(',');
                var bindingControl = $("#Content");
                var vm = new ViewModel(requestID, bindingControl, [], gridSetting);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () {
                        vm.SaveGridSettings();
                    });
                    Global.Helpers.SetGridFromSettings(vm.RequestsGrid(), gridSetting);
                });
            });
        }
        BulkEdit.init = init;
        init();
    })(BulkEdit = Requests.BulkEdit || (Requests.BulkEdit = {}));
})(Requests || (Requests = {}));

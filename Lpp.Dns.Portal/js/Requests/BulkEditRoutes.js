var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Requests;
(function (Requests) {
    var BulkEditRoutes;
    (function (BulkEditRoutes) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(routes, datamart, bindingControl, screenPermissions, gridSetting) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                _this.DueDateTemplate = kendo.template('<input value=\'#= DueDate #\' data-bind=\'value:DueDate\' data-role=\'datepicker\' />');
                _this.PriorityTemplate = kendo.template('<input value=\'#= Priority #\' data-bind=\'value:Priority\' data-role=\'dropdownlist\' data-source=\'Dns.Enums.PrioritiesTranslation\' data-text-field=\'text\' data-value-field=\'value\' />');
                var self = _this;
                _this.DataMart = datamart;
                _this.dsRequest = kendo.data.DataSource.create({
                    data: [],
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRouteDetailDTO)
                    }
                });
                _this.BulkEditEnabled = ko.observable(false);
                _this.onRowSelectionChange = function (e) {
                    var grid = $(e.sender.wrapper).data('kendoGrid');
                    var rows = grid.select();
                    self.BulkEditEnabled(rows.length > 0);
                };
                var filter = ko.utils.arrayMap(routes, function (id) { return 'RequestDataMartID eq ' + id; }).join(' or ');
                Dns.WebApi.Requests.RequestsByRoute(filter, null, "SubmittedOn desc").done(function (requests) {
                    if (requests == null)
                        return;
                    var models = ko.utils.arrayMap(requests, function (r) { return new RouteDetail(r); });
                    self.dsRequest.data(models);
                });
                self.formatDueDateCell = function (item) { return self.DueDateTemplate(item); };
                self.formatPriorityCell = function (item) { return self.PriorityTemplate(item); };
                self.onBulkEdit = function () {
                    var grid = $('#gRequests').data('kendoGrid');
                    var rows = grid.select();
                    if (rows.length == 0)
                        return;
                    Global.Helpers.ShowDialog("Edit Metadata Values", "/dialogs/metadatabulkeditpropertieseditor", ["Close"], 650, 415, { defaultPriority: Dns.Enums.Priorities.Medium, defaultDueDate: null, isRequestLevel: false })
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
                        var rx = new Dns.ViewModels.RequestDataMartViewModel();
                        rx.ID(i.RequestDataMartID());
                        rx.Priority(i.Priority());
                        rx.DueDate(i.DueDate());
                        return rx.toData();
                    });
                    Dns.WebApi.Requests.UpdateRequestDataMartsMetadata(updateArgs).done(function () {
                        location.href = '/';
                    }).fail(function (err) {
                        Global.Helpers.HideExecuting();
                        var description = (err.responseJSON.errors[0].Description).toString();
                        Global.Helpers.ShowErrorAlert('Error Updating Request DataMart Metadata', '<p>There was an error updating the metadata for the changed requests:</p><p class="alert alert-danger">' + description.replace('\r\n', '<br/>') + '</p>');
                    });
                };
                _this.SaveGridSettings = function () {
                    Users.SetSetting("Requests.BulkEditRoutes.gRequests.User:" + User.ID, Global.Helpers.GetGridSettings(self.RequestsGrid()));
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
        BulkEditRoutes.ViewModel = ViewModel;
        var RouteDetail = (function (_super) {
            __extends(RouteDetail, _super);
            function RouteDetail(data) {
                var _this = _super.call(this, data) || this;
                var self = _this;
                _this._originalValues = {
                    Priority: data.Priority,
                    DueDate: data.DueDate
                };
                _this.Changed = ko.computed(function () {
                    return self.Priority() != self._originalValues.Priority || self.DueDate() != self._originalValues.DueDate;
                }, _this, { pure: true });
                return _this;
            }
            return RouteDetail;
        }(Dns.ViewModels.HomepageRouteDetailViewModel));
        BulkEditRoutes.RouteDetail = RouteDetail;
        function init() {
            var routeID = $.url().param('r').split(',');
            $.when(Users.GetSetting('Requests.BulkEditRoutes.gRequests.User:' + User.ID), Dns.WebApi.DataMarts.GetByRoute(routeID[0])).done(function (gridSetting, datamarts) {
                var bindingControl = $("#Content");
                var vm = new ViewModel(routeID, datamarts[0], bindingControl, [], gridSetting);
                $(function () {
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () {
                        vm.SaveGridSettings();
                    });
                    Global.Helpers.SetGridFromSettings(vm.RequestsGrid(), gridSetting);
                });
            });
        }
        BulkEditRoutes.init = init;
        init();
    })(BulkEditRoutes = Requests.BulkEditRoutes || (Requests.BulkEditRoutes = {}));
})(Requests || (Requests = {}));

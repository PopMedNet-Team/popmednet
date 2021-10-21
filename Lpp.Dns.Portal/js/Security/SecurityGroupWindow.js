/// <reference path="../../Scripts/page/Page.ts" />
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
var Security;
(function (Security) {
    var SecurityGroupWindow;
    (function (SecurityGroupWindow) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.dsOrgResults = new kendo.data.DataSource({
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
                _this.dsOrgSecResults = new kendo.data.DataSource({
                    data: []
                });
                _this.dsProjResults = new kendo.data.DataSource({
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
                _this.dsProjSecResults = new kendo.data.DataSource({
                    data: []
                });
                return _this;
            }
            ViewModel.prototype.AddOrganization = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    Dns.WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done(function (results) {
                        $("#gOrgSecResults").data("kendoGrid").dataSource.data(results);
                        $("#gOrgSecResults").data("kendoGrid").refresh();
                    });
                });
            };
            ViewModel.prototype.AddProject = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    Dns.WebApi.SecurityGroups.List("OwnerID eq " + dataItem.ID).done(function (results) {
                        $("#gProjSecResults").data("kendoGrid").dataSource.data(results);
                        $("#gProjSecResults").data("kendoGrid").refresh();
                    });
                });
            };
            ViewModel.prototype.AddOrgGrp = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    vm.Close(dataItem);
                });
            };
            ViewModel.prototype.AddProjectGrp = function (arg) {
                $.each(arg.sender.select(), function (count, item) {
                    var dataItem = arg.sender.dataItem(item);
                    vm.Close(dataItem);
                });
            };
            return ViewModel;
        }(Global.DialogViewModel));
        SecurityGroupWindow.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("body");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(SecurityGroupWindow = Security.SecurityGroupWindow || (Security.SecurityGroupWindow = {}));
})(Security || (Security = {}));

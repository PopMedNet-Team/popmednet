/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DataMarts;
(function (DataMarts) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gDataMartsSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/datamarts/list"),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                    },
                    sort: { field: "Name", dir: "asc" },
                });
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gDataMartsSetting);
                return _this;
            }
            ViewModel.prototype.btnNewDataMart_Click = function () {
                window.location.href = "/datamarts/details";
            };
            ViewModel.prototype.DataMartsGrid = function () {
                return $("#gDataMarts").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/datamarts/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function init() {
            $.when(Users.GetSetting("DataMarts.Index.gDataMarts.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateDataMarts)).done(function (gDataMartsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gDataMartsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Organization.CreateDataMarts] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    Global.Helpers.SetGridFromSettings(vm.DataMartsGrid(), gDataMartsSetting);
                    vm.DataMartsGrid().bind("dataBound", function (e) {
                        Users.SetSetting("DataMarts.Index.gDataMarts.User:" + User.ID, Global.Helpers.GetGridSettings(vm.DataMartsGrid()));
                    });
                    vm.DataMartsGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
                });
            });
        }
        init();
    })(Index = DataMarts.Index || (DataMarts.Index = {}));
})(DataMarts || (DataMarts = {}));

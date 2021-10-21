/// <reference path="../_rootlayout.ts" />
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
                var dsDataMartSettings = gDataMartsSetting.filter(function (item) { return item.Key === "DataMarts.Index.gResults.User:" + User.ID; });
                _this.dsSetting = (dsDataMartSettings.length > 0 && dsDataMartSettings[0] !== null) ? dsDataMartSettings[0] : null;
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
            $.when(Users.GetSettings(["DataMarts.Index.gDataMarts.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateDataMarts)).done(function (gDataMartsSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gDataMartsSetting, bindingControl, canAdd[0] ? [PMNPermissions.Organization.CreateDataMarts] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = DataMarts.Index || (DataMarts.Index = {}));
})(DataMarts || (DataMarts = {}));

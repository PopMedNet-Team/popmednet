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
/// <reference path="../_rootlayout.ts" />
var Registries;
(function (Registries) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gRegistriesSettings, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gRegistriesSettings.filter(function (item) { return item.Key === "Registries.Index.gRegistries.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/registries/list"),
                        },
                        parameterMap: function (options, transportType) {
                            //override the parameterMap implementation to update the odata filter value for the enum type RegistryTypes. 
                            //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                            var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Type', format: "Lpp.Dns.DTO.Enums.RegistryTypes'{0}'" }]);
                            var map = new kendo.data.transports.webapi.parameterMap(opt);
                            return map;
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelRegistryDTO)
                    },
                    sort: { field: "Name", dir: "asc" },
                });
                return _this;
            }
            ViewModel.prototype.btnNewRegistry_Click = function () {
                window.location.href = "/registries/details";
            };
            ViewModel.prototype.RegistriesGrid = function () {
                return $("#gRegistries").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/registries/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function typeFilterUI(element) {
            element.kendoDropDownList({
                dataSource: Dns.Enums.RegistryTypesTranslation,
                optionLabel: '--Select Value--',
                dataTextField: 'text',
                dataValueField: 'value'
            });
        }
        Index.typeFilterUI = typeFilterUI;
        function init() {
            $.when(Users.GetSettings(["Registries.Index.gRegistries.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateRegistry)).done(function (gRegistriesSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gRegistriesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateRegistry] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Registries.Index || (Registries.Index = {}));
})(Registries || (Registries = {}));

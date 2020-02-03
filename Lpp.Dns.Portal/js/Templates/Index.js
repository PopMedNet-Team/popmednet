var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Templates;
(function (Templates) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gTemplatesSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/templates/criteriagroups"),
                        },
                        parameterMap: function (options, transportType) {
                            //override the parameterMap implementation to update the odata filter value for the enum type TemplateTypes. 
                            //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                            var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Type', format: "Lpp.Dns.DTO.Enums.TemplateTypes'{0}'" }]);
                            var map = new kendo.data.transports.webapi.parameterMap(opt);
                            return map;
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelTemplateDTO)
                    },
                    sort: { field: "Name", dir: "asc" },
                });
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gTemplatesSetting);
                return _this;
            }
            ViewModel.prototype.TemplatesGrid = function () {
                return $("#gTemplates").data("kendoGrid");
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/templates/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function typeFilterUI(element) {
            element.kendoDropDownList({
                dataSource: Dns.Enums.TemplateTypesTranslation,
                optionLabel: '--Select Value--',
                dataTextField: 'text',
                dataValueField: 'value'
            });
        }
        Index.typeFilterUI = typeFilterUI;
        function init() {
            //TODO: get the screen permissions and then bind the screen
            $.when(Users.GetSetting("Templates.Index.gTemplates.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateTemplate)).done(function (gTemplatesSetting, canAdd) {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateTemplate] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    Global.Helpers.SetGridFromSettings(vm.TemplatesGrid(), gTemplatesSetting);
                    vm.TemplatesGrid().bind("dataBound", function (e) {
                        Users.SetSetting("Templates.Index.gTemplates.User:" + User.ID, Global.Helpers.GetGridSettings(vm.TemplatesGrid()));
                    });
                    vm.TemplatesGrid().bind("columnMenuInit", Global.Helpers.AddClearAllFiltersMenuItem);
                });
            });
        }
        init();
    })(Index = Templates.Index || (Templates.Index = {}));
})(Templates || (Templates = {}));

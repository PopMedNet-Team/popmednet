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
                var dsgroupSettings = gTemplatesSetting.filter(function (item) { return item.Key === "Templates.Index.gTemplates.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
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
            $.when(Users.GetSettings(["Templates.Index.gTemplates.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateTemplate)).done(function (gTemplatesSetting, canAdd) {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateTemplate] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Templates.Index || (Templates.Index = {}));
})(Templates || (Templates = {}));

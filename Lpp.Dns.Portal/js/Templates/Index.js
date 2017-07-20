var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Templates;
(function (Templates) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
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
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.TemplatesGrid = function () {
                return $("#gTemplates").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Templates.Index.gTemplates.User:" + User.ID, Global.Helpers.GetGridSettings(this.TemplatesGrid()));
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
            $.when(Users.GetSetting("Templates.Index.gTemplates.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateTemplate)).done(function (gTemplatesSetting, canAdd) {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(gTemplatesSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateTemplate] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    Global.Helpers.SetGridFromSettings(vm.TemplatesGrid(), gTemplatesSetting);
                });
            });
        }
        init();
    })(Index = Templates.Index || (Templates.Index = {}));
})(Templates || (Templates = {}));

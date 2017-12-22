/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Users;
(function (Users) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gUsersSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 50,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/users/list"),
                        },
                        parameterMap: function (options, transportType) {
                            //override the parameterMap implementation to update the odata filter value for the enum type RegistryTypes. 
                            //By default the webapi paramter mapper does not know to include the enum typename in the filter value.
                            var opt = Global.Helpers.UpdateKendoGridFilterOptions(options, [{ field: 'Active', format: "{0}" }]);
                            var map = new kendo.data.transports.webapi.parameterMap(opt);
                            return map;
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelDataMartDTO)
                    },
                    sort: { field: "UserName", dir: "asc" },
                });
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gUsersSetting);
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.btnNewUser_Click = function () {
                window.location.href = "/users/details?OrganizationID=" + User.EmployerID;
            };
            ViewModel.prototype.UsersGrid = function () {
                return $("#gUsers").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("Users.Index.gUsers.User:" + User.ID, Global.Helpers.GetGridSettings(this.UsersGrid()));
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            return "<a href=\"/users/details?ID=" + dataItem.ID + "\">" + dataItem.UserName + "</a>";
        }
        Index.NameAchor = NameAchor;
        Index.UserTypesTranslation = [
            { value: true, text: 'Active' },
            { value: false, text: 'Inactive' },
        ];
        function typeFilterUI(element) {
            element.kendoDropDownList({
                dataSource: Index.UserTypesTranslation,
                optionLabel: '--Select Value--',
                dataTextField: 'text',
                dataValueField: 'value'
            });
        }
        Index.typeFilterUI = typeFilterUI;
        function ActiveTemplate(dataItem) {
            return dataItem.Active ? "Active" : "Inactive";
        }
        Index.ActiveTemplate = ActiveTemplate;
        function init() {
            $.when(Users.GetSetting("Users.Index.gUsers.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Organization.CreateUsers)).done(function (gUsersSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gUsersSetting, bindingControl, canAdd[0] ? [Permissions.Organization.CreateUsers] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                    $(window).unload(function () { return vm.Save(); });
                    Global.Helpers.SetGridFromSettings(vm.UsersGrid(), gUsersSetting);
                });
            });
        }
        init();
    })(Index = Users.Index || (Users.Index = {}));
})(Users || (Users = {}));
//# sourceMappingURL=Index.js.map
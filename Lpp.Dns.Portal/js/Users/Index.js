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
var Users;
(function (Users) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gUsersSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gUsersSetting.filter(function (item) { return item.Key === "Users.Index.gUsers.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 50,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/users/list"),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelUserDTO)
                    },
                    sort: { field: "UserName", dir: "asc" },
                });
                return _this;
            }
            ViewModel.prototype.btnNewUser_Click = function () {
                window.location.href = "/users/details?OrganizationID=" + User.EmployerID;
            };
            ViewModel.prototype.UsersGrid = function () {
                return $("#gUsers").data("kendoGrid");
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
            $.when(Users.GetSettings(["Users.Index.gUsers.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Organization.CreateUsers)).done(function (gUsersSetting, canAdd) {
                $(function () {
                    var bindingControl = $("#Content");
                    vm = new ViewModel(gUsersSetting, bindingControl, canAdd[0] ? [PMNPermissions.Organization.CreateUsers] : []);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = Users.Index || (Users.Index = {}));
})(Users || (Users = {}));

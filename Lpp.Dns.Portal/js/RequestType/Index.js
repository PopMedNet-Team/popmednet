var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../_rootlayout.ts" />
var RequestType;
(function (RequestType) {
    var Index;
    (function (Index) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gRequestTypesSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                _this.ds = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/requesttypes/list"),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelRequestTypeDTO)
                    },
                    sort: { field: "Name", dir: "asc" },
                });
                Global.Helpers.SetDataSourceFromSettings(_this.ds, gRequestTypesSetting);
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                        self.Save();
                    });
                };
                return _this;
            }
            ViewModel.prototype.onNewRequestType = function () {
                window.location.href = '/requesttype/details';
            };
            ViewModel.prototype.RequestTypesGrid = function () {
                return $("#gRequestTypes").data("kendoGrid");
            };
            ViewModel.prototype.Save = function () {
                Users.SetSetting("RequestType.Index.gRequestTypes.User:" + User.ID, Global.Helpers.GetGridSettings(this.RequestTypesGrid()));
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function NameAchor(dataItem) {
            if (!dataItem.TemplateID || dataItem.TemplateID == '00000000-0000-0000-0000-000000000000') {
                return dataItem.Name;
            }
            else {
                return "<a href=\"/requesttype/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
        }
        Index.NameAchor = NameAchor;
        function init() {
            $.when(Users.GetSetting("RequestType.Index.gRequestTypes.User:" + User.ID), Dns.WebApi.Users.GetGlobalPermission(Permissions.Portal.CreateRequestType)).done(function (gRequestTypesSetting, canAdd) {
                $(function () {
                    var bindingControl = $('#Content');
                    Index.vm = new ViewModel(gRequestTypesSetting, bindingControl, canAdd[0] ? [Permissions.Portal.CreateRequestType] : []);
                    ko.applyBindings(Index.vm, bindingControl[0]);
                    $(window).unload(function () { return Index.vm.Save(); });
                    Global.Helpers.SetGridFromSettings(Index.vm.RequestTypesGrid(), gRequestTypesSetting);
                });
            });
        }
        init();
    })(Index = RequestType.Index || (RequestType.Index = {}));
})(RequestType || (RequestType = {}));
//# sourceMappingURL=Index.js.map
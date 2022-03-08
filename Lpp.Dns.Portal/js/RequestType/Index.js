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
var RequestType;
(function (RequestType) {
    var Index;
    (function (Index) {
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(gRequestTypesSetting, bindingControl, screenPermissions) {
                var _this = _super.call(this, bindingControl, screenPermissions) || this;
                var self = _this;
                var dsgroupSettings = gRequestTypesSetting.filter(function (item) { return item.Key === "RequestType.Index.gRequestTypes.User:" + User.ID; });
                _this.dsSetting = (dsgroupSettings.length > 0 && dsgroupSettings[0] !== null) ? dsgroupSettings[0] : null;
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
            return "<a href=\"/requesttype/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
        }
        Index.NameAchor = NameAchor;
        function init() {
            $.when(Users.GetSettings(["RequestType.Index.gRequestTypes.User:" + User.ID]), Dns.WebApi.Users.GetGlobalPermission(PMNPermissions.Portal.CreateRequestType)).done(function (gRequestTypesSetting, canAdd) {
                $(function () {
                    var bindingControl = $('#Content');
                    Index.vm = new ViewModel(gRequestTypesSetting, bindingControl, canAdd[0] ? [PMNPermissions.Portal.CreateRequestType] : []);
                    ko.applyBindings(Index.vm, bindingControl[0]);
                });
            });
        }
        init();
    })(Index = RequestType.Index || (RequestType.Index = {}));
})(RequestType || (RequestType = {}));

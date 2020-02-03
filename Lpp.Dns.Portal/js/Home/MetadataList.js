/// <reference path="../_rootlayout.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
// Unused for the most part. Still relies on MVC because of use of Plugin.DisplayResponse
// and Html.BodyView.
var Home;
(function (Home) {
    var MetadataList;
    (function (MetadataList) {
        MetadataList.RawModel = null;
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(/*rawModel: IViewModelData[],*/ bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                //this.dmMetadataModel = ko.observableArray(rawModel);
                _this.metadata = ko.observableArray([{ DataMart: 'Sample DM 1', Name: 'Sample Metadata Name', Type: 'Sample Type' }]);
                return _this;
                //this.dsMetadata = new kendo.data.DataSource({
                //    type: "webapi",
                //    serverPaging: true,
                //    serverSorting: true,
                //    serverFiltering: true,
                //    pageSize: 100,
                //    transport: {
                //        read: {
                //            url: Global.Helpers.GetServiceUrl("/users/getnotifications?UserID=" + User.ID /*+ "&$top=5"*/),
                //        }
                //    },
                //    schema: {
                //        model: kendo.data.Model.define(Dns.Interfaces.KendoModelNotificationDTO)
                //    },
                //    sort: {
                //        field: "DataMart", dir: "desc"
                //    }
                //});     
            }
            return ViewModel;
        }(Global.PageViewModel));
        MetadataList.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(/*RawModel,*/ bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(MetadataList = Home.MetadataList || (Home.MetadataList = {}));
})(Home || (Home = {}));
//# sourceMappingURL=MetadataList.js.map
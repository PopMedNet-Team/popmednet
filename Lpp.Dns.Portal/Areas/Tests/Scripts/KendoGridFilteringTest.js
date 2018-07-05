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
/// <reference path="../../../js/_layout.ts" />
var Tests;
(function (Tests) {
    var KendoGridFilteringTest;
    (function (KendoGridFilteringTest) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.gRequestsHeight = ko.observable(null);
                _this.dsRequest = new kendo.data.DataSource({
                    type: "webapi",
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    pageSize: 500,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl('/requests/listforhomepage'),
                        }
                    },
                    schema: {
                        model: kendo.data.Model.define(Dns.Interfaces.KendoModelHomepageRequestDetailDTO)
                    },
                    sort: { field: "SubmittedOn", dir: "desc" },
                    change: function (e) {
                        vm.gRequestsHeight(e.items != null && e.items.length > 0 ? "600px" : "34px");
                    }
                });
                _this.onColumnMenuInit = function (e) {
                    var menu = e.container.find(".k-menu").data("kendoMenu");
                    menu.bind("close", function (e) {
                    });
                };
                return _this;
            }
            return ViewModel;
        }(Global.PageViewModel));
        KendoGridFilteringTest.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $('#Content');
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        KendoGridFilteringTest.init = init;
        function NameAnchor(dataItem) {
            if (dataItem.IsWorkflowRequest) {
                return "<a href=\"/requests/details?ID=" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
            else {
                return "<a href=\"/request/" + dataItem.ID + "\">" + dataItem.Name + "</a>";
            }
        }
        KendoGridFilteringTest.NameAnchor = NameAnchor;
        function SubjectAnchor(dataItem) {
            return "<a href=\"/tasks/details?ID=" + dataItem.ID + "\">" + dataItem.Subject + "</a>";
        }
        KendoGridFilteringTest.SubjectAnchor = SubjectAnchor;
        init();
    })(KendoGridFilteringTest = Tests.KendoGridFilteringTest || (Tests.KendoGridFilteringTest = {}));
})(Tests || (Tests = {}));
//# sourceMappingURL=KendoGridFilteringTest.js.map
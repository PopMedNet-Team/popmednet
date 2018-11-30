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
/// <reference path="../_rootlayout.ts" />
var NetworkMessages;
(function (NetworkMessages) {
    var Create;
    (function (Create) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Message = new Dns.ViewModels.NetworkMessageViewModel();
                _this.Target = ko.observable("0");
                _this.Targets = ko.observable([]);
                _this.dsTargets = new kendo.data.DataSource({
                    type: "webapi",
                    serverFiltering: true,
                    transport: {
                        read: {
                            url: Global.Helpers.GetServiceUrl("/security/ListSecurityEntities")
                        }
                    }
                });
                return _this;
            }
            ViewModel.prototype.btnSendMessage_Click = function () {
                if (!this.Validate())
                    return;
                var data = this.Message.toData();
                data.Targets = this.Targets();
                Dns.WebApi.NetworkMessages.Insert([data]).done(function () {
                    window.history.back();
                });
            };
            return ViewModel;
        }(Global.PageViewModel));
        Create.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Create = NetworkMessages.Create || (NetworkMessages.Create = {}));
})(NetworkMessages || (NetworkMessages = {}));

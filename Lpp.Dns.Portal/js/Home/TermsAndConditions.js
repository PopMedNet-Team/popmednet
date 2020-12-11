/// <reference path="../../Scripts/page/Page.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Dialog;
(function (Dialog) {
    var Terms;
    (function (Terms) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                _this.Terms = ko.observable(_this.Parameters);
                return _this;
            }
            return ViewModel;
        }(Global.DialogViewModel));
        Terms.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("body");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Terms = Dialog.Terms || (Dialog.Terms = {}));
})(Dialog || (Dialog = {}));

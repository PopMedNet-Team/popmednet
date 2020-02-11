var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../js/_layout.ts" />
var Tests;
(function (Tests) {
    var AddRequestObserver;
    (function (AddRequestObserver) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                return _super.call(this, bindingControl) || this;
            }
            return ViewModel;
        }(Global.DialogViewModel));
        AddRequestObserver.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $('#Content');
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        AddRequestObserver.init = init;
        init();
    })(AddRequestObserver = Tests.AddRequestObserver || (Tests.AddRequestObserver = {}));
})(Tests || (Tests = {}));

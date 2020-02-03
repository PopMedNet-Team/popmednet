var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Tests;
(function (Tests) {
    var Index;
    (function (Index) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                return _super.call(this, bindingControl) || this;
            }
            ViewModel.prototype.onAddRequestObserver = function () {
                Global.Helpers.ShowDialog("Add Users", "/tests/test/addrequestobservers", ['Close'], 640, 630, {});
            };
            return ViewModel;
        }(Global.PageViewModel));
        Index.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $('#Content');
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        Index.init = init;
        init();
    })(Index = Tests.Index || (Tests.Index = {}));
})(Tests || (Tests = {}));

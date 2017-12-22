/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Dialog;
(function (Dialog) {
    var Terms;
    (function (Terms) {
        var vm;
        var ViewModel = (function (_super) {
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
//# sourceMappingURL=TermsAndConditions.js.map
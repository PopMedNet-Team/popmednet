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
var Home;
(function (Home) {
    var Resources;
    (function (Resources) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(theme, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                _this.ResourceHeader = new Dns.ViewModels.ThemeViewModel(theme);
                return _this;
            }
            return ViewModel;
        }(Global.PageViewModel));
        Resources.ViewModel = ViewModel;
        function init() {
            var theming;
            $.when(Dns.WebApi.Theme.GetText(["Resources"]).done(function (results) { theming = results[0]; })).then(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(theming, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Resources = Home.Resources || (Home.Resources = {}));
})(Home || (Home = {}));

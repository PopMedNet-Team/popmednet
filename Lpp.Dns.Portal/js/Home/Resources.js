var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Home;
(function (Home) {
    var Resources;
    (function (Resources) {
        var vm;
        var ViewModel = (function (_super) {
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
            $.when(Dns.WebApi.Theme.GetText(theme, ["Resources"]).done(function (results) { theming = results[0]; })).then(function () {
                var bindingControl = $("#Content");
                vm = new ViewModel(theming, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Resources = Home.Resources || (Home.Resources = {}));
})(Home || (Home = {}));

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
    var Login;
    (function (Login) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(bindingControl) {
                return _super.call(this, bindingControl) || this;
            }
            ViewModel.prototype.ShowTerms = function () {
                Global.ShowTerms(Layout.vmFooter.Theme.Terms());
            };
            ViewModel.prototype.ShowInfo = function () {
                Global.ShowInfo(Layout.vmFooter.Theme.Info());
            };
            ViewModel.prototype.ForgotPassword = function () {
                Global.Helpers.ShowDialog("Forgot Password", "/home/forgotpassword", ["close"], 800, 350, { Username: $('#txtUserName').val() });
            };
            ViewModel.prototype.Registration = function () {
                Global.Helpers.ShowDialog("User Registration", "/home/userregistration", ["close"], 900, 550);
            };
            ViewModel.prototype.Submit = function () {
                var formElement = $("#fLogin");
                if (!this.Validate())
                    return;
                formElement.submit();
            };
            return ViewModel;
        }(Global.PageViewModel));
        Login.ViewModel = ViewModel;
        function init() {
            $(function () {
                var bindingControl = $("#fLogin");
                vm = new ViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(Login = Home.Login || (Home.Login = {}));
})(Home || (Home = {}));

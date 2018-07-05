/// <reference path="../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
var Layout;
(function (Layout) {
    var FooterViewModel = /** @class */ (function () {
        function FooterViewModel(themeing) {
            this.Theme = new Dns.ViewModels.ThemeViewModel(themeing);
        }
        FooterViewModel.prototype.ShowTerms = function () {
            Global.ShowTerms(Layout.vmFooter.Theme.Terms());
        };
        FooterViewModel.prototype.ShowInfo = function () {
            Global.ShowInfo(Layout.vmFooter.Theme.Info());
        };
        return FooterViewModel;
    }());
    Layout.FooterViewModel = FooterViewModel;
    var HeaderViewModel = /** @class */ (function () {
        function HeaderViewModel(menu, themeing, themeimg) {
            this.MainMenu = ko.observable(menu);
            this.Theme = new Dns.ViewModels.ThemeViewModel(themeing);
            this.ThemeIMG = new Dns.ViewModels.ThemeViewModel(themeimg);
        }
        HeaderViewModel.prototype.Logout = function (e) {
            if (User != null) {
                if (User.AuthInfo != null) {
                    User.AuthInfo.ID = null;
                    User.AuthInfo.Authorization = null;
                    User.AuthInfo.UserName = null;
                    User.AuthToken = null;
                    User.AuthInfo = null;
                }
                User.ID = null;
                User.EmployerID = null;
                User.AuthToken = null;
            }
            Global.Session("MainMenu", null);
            $.removeCookie("Authorization");
            sessionStorage.clear();
            return true;
        };
        HeaderViewModel.prototype.ReloadMenu = function () {
            var _this = this;
            Dns.WebApi.Users.ReturnMainMenu().done(function (results) {
                _this.MainMenu(results);
                Global.Session("MainMenu", results);
                var menu = $("#menu").data("kendoMenu");
                menu.destroy();
                $("#menu").kendoMenu({
                    dataSource: results
                });
            });
        };
        return HeaderViewModel;
    }());
    Layout.HeaderViewModel = HeaderViewModel;
    function init() {
        $.when(Dns.WebApi.Theme.GetText(["Title", "Terms", "Info", "Footer", "ContactUsHref"]), Dns.WebApi.Theme.GetImagePath()).then(function (textThemeResult, imageThemeResult) {
            var menu = Global.Session("MainMenu");
            if (menu == null && User.AuthInfo) {
                Dns.WebApi.Users.ReturnMainMenu().done(function (results) {
                    menu = results;
                    Global.Session("MainMenu", menu);
                    bind(menu, textThemeResult[0], imageThemeResult[0]);
                });
            }
            else {
                bind(menu, textThemeResult[0], imageThemeResult[0]);
            }
        });
    }
    function bind(menu, themeing, themeimg) {
        $(function () {
            var header = $("header");
            var footer = $("footer");
            Layout.vmFooter = new FooterViewModel(themeing);
            Layout.vmHeader = new HeaderViewModel(menu, themeing, themeimg);
            ko.applyBindings(Layout.vmFooter, footer[0]);
            ko.applyBindings(Layout.vmHeader, header[0]);
        });
    }
    init();
})(Layout || (Layout = {}));
//# sourceMappingURL=_Layout.js.map
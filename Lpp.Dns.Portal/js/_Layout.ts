/// <reference path="../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />

module Layout {
    export var vmFooter: FooterViewModel;
    export var vmHeader: HeaderViewModel;

    export class FooterViewModel {
        public Theme: Dns.ViewModels.ThemeViewModel;
        constructor(themeing: Dns.Interfaces.IThemeDTO) {
            this.Theme = new Dns.ViewModels.ThemeViewModel(themeing);
        }
        public ShowTerms() {
            Global.ShowTerms(vmFooter.Theme.Terms());
        }

        public ShowInfo() {
            Global.ShowInfo(vmFooter.Theme.Info());
        }

    }

    export class HeaderViewModel {
        public MainMenu: KnockoutObservable<any[]>;
        public Theme: Dns.ViewModels.ThemeViewModel;
        public ThemeIMG: Dns.ViewModels.ThemeViewModel;
        constructor(menu: Dns.Interfaces.IMenuItemDTO[], themeing: Dns.Interfaces.IThemeDTO, themeimg: Dns.Interfaces.IThemeDTO) {
            this.MainMenu = ko.observable(menu);
            this.Theme = new Dns.ViewModels.ThemeViewModel(themeing);
            this.ThemeIMG = new Dns.ViewModels.ThemeViewModel(themeimg);
        }

        public Logout(e: JQueryEventObject): boolean {
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
        }

        public ReloadMenu() {
            Dns.WebApi.Users.ReturnMainMenu().done((results) => {
                this.MainMenu(results);
                Global.Session("MainMenu", results);
                var menu : kendo.ui.Menu = $("#menu").data("kendoMenu");
                menu.destroy();
                $("#menu").kendoMenu({
                    dataSource: results
                });
            });
        }
    }


    function init() {

        $.when<any>(
            Dns.WebApi.Theme.GetText(["Title", "Terms", "Info", "Footer", "ContactUsHref"]),
            Dns.WebApi.Theme.GetImagePath()
            ).then((textThemeResult, imageThemeResult) => {  
                var menu: Dns.Interfaces.IMenuItemDTO[] = Global.Session("MainMenu");              
                if (menu == null && User.AuthInfo) {
                    Dns.WebApi.Users.ReturnMainMenu().done((results) => {
                        menu = results;
                        Global.Session("MainMenu", menu);
                        bind(menu, textThemeResult[0], imageThemeResult[0]);
                    });
                } else {
                    bind(menu, textThemeResult[0], imageThemeResult[0]);
                }
            });        
    }

    function bind(menu: Dns.Interfaces.IMenuItemDTO[], themeing: Dns.Interfaces.IThemeDTO, themeimg: Dns.Interfaces.IThemeDTO) {
        $(() => {
            var header = $("header");
            var footer = $("footer");

            vmFooter = new FooterViewModel(themeing);
            vmHeader = new HeaderViewModel(menu, themeing, themeimg);

            ko.applyBindings(vmFooter, footer[0]);
            ko.applyBindings(vmHeader, header[0]);
        });

    }

    init();
} 
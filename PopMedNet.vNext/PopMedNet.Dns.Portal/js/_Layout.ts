import * as Global from "../scripts/page/global.js";
import { IThemeDTO, IMenuItemDTO } from "./Dns.Interfaces.js";
import { Theme, Users } from "./Lpp.Dns.WebApi.js";
export var vmFooter: FooterViewModel;
export var vmHeader: HeaderViewModel;

export class FooterViewModel {
    public Theme: IThemeDTO;
    constructor(theme: IThemeDTO) {
        this.Theme = theme;
    }
    public ShowTerms() {
        Global.ShowTerms(vmFooter.Theme.Terms);
    }

    public ShowInfo() {
        Global.ShowInfo(vmFooter.Theme.Info);
    }
}

export class HeaderViewModel {
    public MainMenu: KnockoutObservable<any[]>;
    public Theme: IThemeDTO;
    constructor(menu: IMenuItemDTO[], theme: IThemeDTO) {
        this.MainMenu = ko.observable(menu);
        this.Theme = theme;
    }

    public Logout(e: JQueryEventObject): boolean {
        Global.Session("MainMenu", null);
        //$.removeCookie("Authorization");

        sessionStorage.clear();
        return true;
    }

    public ReloadMenu() {
        Users.ReturnMainMenu().done((results) => {
            this.MainMenu(results);
            Global.Session("MainMenu", results);
            let menu: kendo.ui.Menu = $("#menu").data("kendoMenu");
            menu.destroy();
            $("#menu").kendoMenu({
                dataSource: results
            });
        });
    }
}

function bind(menu: IMenuItemDTO[], themeText: IThemeDTO) {
    $(() => {
        let header = document.getElementById("header");
        let footer = document.getElementById("footer");

        vmFooter = new FooterViewModel(themeText);
        vmHeader = new HeaderViewModel(menu, themeText);

        ko.applyBindings(vmFooter, footer);
        ko.applyBindings(vmHeader, header);
    });
}

$.when<any>(
    Theme.GetText(["Title", "Terms", "Info", "Footer", "ContactUsHref", "LogoImage"])
).then((themeResult: IThemeDTO) => {
    $(() => { 
        let menu: IMenuItemDTO[] = Global.Session("MainMenu");
        
    if (menu == null && typeof Global.User !== 'undefined' && Global.User.AuthInfo) {
        Users.ReturnMainMenu().done((results) => {
            menu = results;
            Global.Session("MainMenu", menu);
            bind(menu, themeResult);
        });
    } else {
        bind(menu, themeResult);
        }
    });
});
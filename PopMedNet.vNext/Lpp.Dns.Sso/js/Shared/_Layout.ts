/// <reference path="../../../Lpp.Dns.Portal/Scripts/Page/Page.ts" />
module Header {
    export var vm: ViewModel;
    export class ViewModel {
        public Theme: Dns.ViewModels.ThemeViewModel;
        public ThemeIMG: Dns.ViewModels.ThemeViewModel;
        constructor(themeing: Dns.Interfaces.IThemeDTO, themeimg: Dns.Interfaces.IThemeDTO) {
            this.Theme = new Dns.ViewModels.ThemeViewModel(themeing);
            this.ThemeIMG = new Dns.ViewModels.ThemeViewModel(themeimg);
        }

        public lLogout_Click(data, event) {
            $.cookie("Authorization", null, { path: "/" }); //Path is required or the cookie is not cleared.
            return true;
        }
    }

    function init() {
        var theming: Dns.Interfaces.IThemeDTO;
        var themimg: Dns.Interfaces.IThemeDTO;
        $.when(
            Dns.WebApi.Theme.GetText(["Title", "Terms", "Info", "Footer", "ContactUsHref"]).done((results: Dns.Interfaces.IThemeDTO[]) => { theming = results[0] }),
            Dns.WebApi.Theme.GetImagePath().done((results: Dns.Interfaces.IThemeDTO[]) => { themimg = results[0] })
            ).then(() => {
                var bindingControl = $("#header");
                vm = new ViewModel(theming, themimg);
                ko.applyBindings(vm, bindingControl[0]);
                $(() => {
                    $("#TermsAndConditionsFooterLink").click((e) => {
                        e.preventDefault();
                        Global.ShowTerms(theming.Terms);
                    });

                    $("#InfoFooterLink").click((e) => {
                        e.preventDefault();
                        Global.ShowInfo(theming.Info);
                    });
                })
            });
    }

    init();
}
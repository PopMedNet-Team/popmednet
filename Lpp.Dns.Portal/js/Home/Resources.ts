/// <reference path="../_rootlayout.ts" />

module Home.Resources {
    var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public ResourceHeader: Dns.ViewModels.ThemeViewModel;


        constructor(theme: Dns.Interfaces.IThemeDTO, bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            this.ResourceHeader = new Dns.ViewModels.ThemeViewModel(theme);

        }


    }

    function init() {
        var theming: Dns.Interfaces.IThemeDTO;
        $.when<any>(
            Dns.WebApi.Theme.GetText(theme, ["Resources"]).done((results: Dns.Interfaces.IThemeDTO[]) => { theming = results[0] })
            ).then(() => {
                var bindingControl = $("#Content");
                vm = new ViewModel(theming, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
    }

    init();
} 
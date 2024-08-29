import * as Global from "../../scripts/page/global.js";
import { Theme } from '../Lpp.Dns.WebApi.js';
import { IThemeDTO } from "../Dns.Interfaces.js";

export class ViewModel extends Global.PageViewModel {
    ResourceHeader: IThemeDTO;

    constructor(theme: IThemeDTO, bindingControl: JQuery) {
        super(bindingControl);
        this.ResourceHeader = theme;
    }
}

Theme.GetText(["Resources"], true).done((theme: IThemeDTO) => {
    let bindingControl = $("#contactInfoPanel");
    let vm = new ViewModel(theme, bindingControl);

    $(function () {        
        ko.applyBindings(vm, bindingControl[0]);
    });
});
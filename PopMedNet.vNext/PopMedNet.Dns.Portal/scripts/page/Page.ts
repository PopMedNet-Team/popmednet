import { Helpers } from "./global.js";
import { Helpers as HelpersService } from '../../js/Lpp.Dns.WebApi.js';

declare var theme: string;

if ((typeof HelpersService != 'undefined') && HelpersService != null)
    HelpersService.RegisterFailMethod((error) => {        
        Helpers.ShowErrorAlert("Error", error, 800);
    });

$(() => {
    if (!("autofocus" in document.createElement("input"))) {
        $("[autofocus]").focus();
    }
});
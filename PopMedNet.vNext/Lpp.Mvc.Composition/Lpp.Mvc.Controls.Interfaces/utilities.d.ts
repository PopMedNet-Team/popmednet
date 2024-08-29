/// <reference path="../lpp.mvc.boilerplate/jsbootstrap.d.ts" />


//declare function gate(condition: () => boolean, func: Function): any;
//declare function releaseGate(gate: any);

interface OverlayOptions
{
    backgroundClass?: string;
    foregroundClass?: string;
    autoRemoveTimeout?: number;
    fadeInSpeed?: number;
    fadeOutSpeed?: number;
}

interface JQuery
{
    alternateClasses(arrayClasses: string[]): JQuery;
    alternateClasses(...classes: string[]): JQuery;
    dataDisplay(strDisplay: string): JQuery;
    dataDisplay(): string;

    showLoadingSign(options?: OverlayOptions, context?: any): JQuery;
    hideLoadingSign(context?: any): JQuery;
    floatErrorMessage(header: string, message: string, options?: OverlayOptions): JQuery;
}


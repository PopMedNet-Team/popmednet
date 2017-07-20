/// <reference path="../../Lpp.Mvc.Composition/Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />

var submitHandlers: Array<() => boolean> = [];
  
module Dns { 
    export function EnableValidation(parseInputAttributes?: boolean, messagesOnModified?: boolean, insertMessages?: boolean, messageTemplate?: string) {
        ko.validation.init({
            registerExtenders: true,
            messagesOnModified: messagesOnModified == null ? true : messagesOnModified,
            insertMessages: insertMessages == null ? true : insertMessages,
            parseInputAttributes: parseInputAttributes == null ? true : parseInputAttributes,
            messageTemplate: messageTemplate,
            decorateElement: true,
            errorMessageClass: 'validationMessage',
            errorElementClass: 'validationElement',
            errorsAsTitle: true,
            errorClass: 'error'
        });
    }

    export class PageViewModel {
        public isValid: KnockoutComputed<boolean>;
        private HiddenDataControl: JQuery;
        public validationGroup: KnockoutValidationErrors;
        

        constructor(hiddenDataControl: JQuery) {
            this.validationGroup = ko.validation.group(this);

            //Store the hidden data control information here so that we can persist to it on the save
            this.HiddenDataControl = hiddenDataControl;

            //Register the save method that is here with the handler. Beware this might not work because of javascript not knowing about inheritence.
            //Will have to test this implementation once we have a stub page working.
            var form = $(".Content form");
            var self = this;
            form.submit(function () {
                return self.save();
            });

            this.validationGroup.showAllMessages();
        }

        public raiseChange() {
            var form = $(".Content form");

            if ((<any>form).formChanged)
                (<any>form).formChanged(true); 

            $("input[name='save']").prop("disabled", false);
            $("input[name='Save']").prop("disabled", false);
            $("input[name='Copy']").prop("disabled", true);
        }

        public SubscribeObservables() {
            this.SubscribeObservableHelper(this);
        }

        private SubscribeObservableHelper(parent: any) {
            for (var prop in parent) {
                if (parent[prop] && parent[prop].subscribe) {
                    parent[prop].subscribe((value) => {
                        this.raiseChange();
                    });
                } else if (!!parent[prop] && parent[prop].constructor === Object) {
                    this.SubscribeObservableHelper(parent[prop]);
                }

                if ($.isArray(parent[prop])) {
                    parent[prop].forEach((item) => {
                        this.SubscribeObservableHelper(item);
                    });
                }
            }

        }

        public save() : boolean {
            throw new Error("Save not implemented. Override this method and call store to persist your data");
        }

        public store(json: any): boolean {
            var sJson = JSON.stringify(json);
            this.HiddenDataControl.val(sJson);
            
            var form = $(".Content form");

            if (form && (<any>form).formChanged) //This is done for the places where this isn't available
                (<any>form).formChanged(false); //This forces it to a generic variable because this is hacked into other code that cannot be pulled out right now.
            $("input[name=save]").prop("disabled", true);
            $("input[name=Copy]").prop("disabled", false);
            return true;
        }
    }

    export class ChildViewModel {
        public isValid: KnockoutComputed<boolean>;
        public validationGroup: KnockoutValidationErrors;

        constructor() {
            this.validationGroup = ko.validation.group(this);
            this.validationGroup.showAllMessages();
        }

        public raiseChange() {
            var form = $(".Content form");
            if (form && (<any>form).formChanged) {
                (<any>form).formChanged(true);
            }

            $("input[name=save]").prop("disabled", false);
            $("input[name=Copy]").prop("disabled", true);
        }

        public subscribeObservables() {
            for (var prop in this) {
                if (this[prop] && this[prop].subscribe) {
                    this[prop].subscribe((value) => {
                        //console.log("raising change for value: " + value);
                        this.raiseChange();
                    });
                }
            }
        }
    }

    export class KeyValuePairData<K, V> {
        public Key: K;
        public Value: V;

        constructor( key: K, value: V ) {
            this.Key = key;
            this.Value = value;
        }
    }

    export class KeyValuePair<K, V> {
        public Key: KnockoutObservable<K>;
        public Value: KnockoutObservable<V>;

        constructor( key: K, value: V ) {
            this.Key = ko.observable<K>( key );
            this.Value = ko.observable<V>( value );
        }
    }

    // this class is used to create drop down lists
    export class SelectItem extends KeyValuePairData<string, string> {
        constructor( display: string, value?: string ) {
            super( display, ( typeof value === "undefined" ) ? display : value );
        }
    }
}

//Fix Fyodor

$.fn.alternateClasses = function jQuery$alternateClasses(arrayClasses) {
    if (typeof arrayClasses != "Array") arrayClasses = arguments;
    arrayClasses = $.makeArray(arrayClasses);
    this.removeClass(arrayClasses.join(" "));

    var arrayCopy = arrayClasses.slice(0);
    this.each(function () {
        $(this).addClass(arrayCopy.shift());
        if (!arrayCopy.length) arrayCopy = arrayClasses.slice(0);
    });

    return this;
};

var _loadingSignContextKey = "{4C3C9D9D-DAEC-4A7C-89CA-E76F92D60BE8}";
function loadingSignContext(target: JQuery, context) {
    if (!context) context = target.data(_loadingSignContextKey)
        if (!context) target.data(_loadingSignContextKey, context = {});
    return context;
}

$.fn.showLoadingSign = function (options: OverlayOptions, context?: any) {
    var target = <JQuery>this;
    options = <OverlayOptions>$.extend({
        backgroundClass: "LoadingOverlay", foregroundClass: "LoadingSign",
        fadeInSpeed: 200, fadeOutSpeed: 200
    }, options);
    context = loadingSignContext(target, context);

    target.hideLoadingSign(context);
    var icon;
    var loadingSign =
        $('<div>').css({ width: 10, height: 10, position: "absolute", top: 0, left: 0 })
            .append($("<div>").addClass(options.backgroundClass)
                .css({ width: target.width(), height: target.height() })
                .position({ my: "left top", at: "left top", of: target })
            )
            .append(icon = $("<div>&nbsp;</div>").addClass(options.foregroundClass))
            .fadeIn(options.fadeInSpeed)
            .appendTo("body");

    setTimeout(() => icon.position(targetCenterTopPosition(target)), 20);

    context.sign = loadingSign;
    context.options = options;
    return target;
};

$.fn.hideLoadingSign = function (context?: any) {
    var s = loadingSignContext(this, context);
    if (s && s.sign) s.sign.fadeOut(s.options.fadeOutSpeed, function () { s.sign.remove(); });
    return this;
};

$.fn.floatErrorMessage = function (header: string, message: string, options: OverlayOptions) {
    var target = <JQuery>this;
    options = <OverlayOptions>$.extend({
        foregroundClass: "ErrorMessage",
        autoRemoveTimeout: 3000, fadeOutSpeed: 1500, fadeInSpeed: 200
    }, options);

    var error = $("<div>").addClass(options.foregroundClass)
        .append($("<span class='Header'>").text(header))
        .append($("<span class='Message'>").html(message))
        .appendTo("body")
        .fadeIn(options.fadeInSpeed)
        .position(targetCenterTopPosition(target));
    setTimeout(function () { error.fadeOut(options.fadeOutSpeed, function () { error.remove(); }); }, options.autoRemoveTimeout);
};

function targetCenterTopPosition(target: JQuery) {
    var bodyTop = $("body").scrollTop();
    var bodyHeight = $(window).height();
    var targetTop = target.position().top;
    var targetHeight = target.height();
    var ofs =
        (bodyTop > targetTop ? bodyTop - targetTop : 0) +
        Math.min(bodyHeight, target.height()) / 4;

    return { my: "center top", at: "center top", of: target, offset: "0 " + ofs };
}

$.fn.dataDisplay = function jQuery$dataDisplay(strDisplay) {
    return strDisplay === undefined ?
        this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}") :
        this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}", strDisplay);
};

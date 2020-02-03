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
            decorateInputElement: true,
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
            var form = $("form.trackChanges");
            var self = this;
            form.submit(function () {
                return self.save();
            });

            this.validationGroup.showAllMessages();
        }

        public raiseChange() {
            var form = $("form.trackChanges");

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
            var par: any = this;
            for (var prop in par) {
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

/// <reference path="../../Lpp.Mvc.Composition/Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var submitHandlers = [];
var Dns;
(function (Dns) {
    function EnableValidation(parseInputAttributes, messagesOnModified, insertMessages, messageTemplate) {
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
    Dns.EnableValidation = EnableValidation;
    var PageViewModel = /** @class */ (function () {
        function PageViewModel(hiddenDataControl) {
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
        PageViewModel.prototype.raiseChange = function () {
            var form = $("form.trackChanges");
            if (form.formChanged)
                form.formChanged(true);
            $("input[name='save']").prop("disabled", false);
            $("input[name='Save']").prop("disabled", false);
            $("input[name='Copy']").prop("disabled", true);
        };
        PageViewModel.prototype.SubscribeObservables = function () {
            this.SubscribeObservableHelper(this);
        };
        PageViewModel.prototype.SubscribeObservableHelper = function (parent) {
            var _this = this;
            for (var prop in parent) {
                if (parent[prop] && parent[prop].subscribe) {
                    parent[prop].subscribe(function (value) {
                        _this.raiseChange();
                    });
                }
                else if (!!parent[prop] && parent[prop].constructor === Object) {
                    this.SubscribeObservableHelper(parent[prop]);
                }
                if ($.isArray(parent[prop])) {
                    parent[prop].forEach(function (item) {
                        _this.SubscribeObservableHelper(item);
                    });
                }
            }
        };
        PageViewModel.prototype.save = function () {
            throw new Error("Save not implemented. Override this method and call store to persist your data");
        };
        PageViewModel.prototype.store = function (json) {
            var sJson = JSON.stringify(json);
            this.HiddenDataControl.val(sJson);
            var form = $(".Content form");
            if (form && form.formChanged) //This is done for the places where this isn't available
                form.formChanged(false); //This forces it to a generic variable because this is hacked into other code that cannot be pulled out right now.
            $("input[name=save]").prop("disabled", true);
            $("input[name=Copy]").prop("disabled", false);
            return true;
        };
        return PageViewModel;
    }());
    Dns.PageViewModel = PageViewModel;
    var ChildViewModel = /** @class */ (function () {
        function ChildViewModel() {
            this.validationGroup = ko.validation.group(this);
            this.validationGroup.showAllMessages();
        }
        ChildViewModel.prototype.raiseChange = function () {
            var form = $(".Content form");
            if (form && form.formChanged) {
                form.formChanged(true);
            }
            $("input[name=save]").prop("disabled", false);
            $("input[name=Copy]").prop("disabled", true);
        };
        ChildViewModel.prototype.subscribeObservables = function () {
            var _this = this;
            var par = this;
            for (var prop in par) {
                if (this[prop] && this[prop].subscribe) {
                    this[prop].subscribe(function (value) {
                        //console.log("raising change for value: " + value);
                        _this.raiseChange();
                    });
                }
            }
        };
        return ChildViewModel;
    }());
    Dns.ChildViewModel = ChildViewModel;
    var KeyValuePairData = /** @class */ (function () {
        function KeyValuePairData(key, value) {
            this.Key = key;
            this.Value = value;
        }
        return KeyValuePairData;
    }());
    Dns.KeyValuePairData = KeyValuePairData;
    var KeyValuePair = /** @class */ (function () {
        function KeyValuePair(key, value) {
            this.Key = ko.observable(key);
            this.Value = ko.observable(value);
        }
        return KeyValuePair;
    }());
    Dns.KeyValuePair = KeyValuePair;
    // this class is used to create drop down lists
    var SelectItem = /** @class */ (function (_super) {
        __extends(SelectItem, _super);
        function SelectItem(display, value) {
            return _super.call(this, display, (typeof value === "undefined") ? display : value) || this;
        }
        return SelectItem;
    }(KeyValuePairData));
    Dns.SelectItem = SelectItem;
})(Dns || (Dns = {}));
//Fix Fyodor
$.fn.alternateClasses = function jQuery$alternateClasses(arrayClasses) {
    //if (typeof arrayClasses != "Array") arrayClasses = arguments;
    if ((arrayClasses instanceof Array) == false)
        arrayClasses = arguments;
    arrayClasses = $.makeArray(arrayClasses);
    this.removeClass(arrayClasses.join(" "));
    var arrayCopy = arrayClasses.slice(0);
    this.each(function () {
        $(this).addClass(arrayCopy.shift());
        if (!arrayCopy.length)
            arrayCopy = arrayClasses.slice(0);
    });
    return this;
};
function targetCenterTopPosition(target) {
    var bodyTop = $("body").scrollTop();
    var bodyHeight = $(window).height();
    var targetTop = target.position().top;
    var targetHeight = target.height();
    var ofs = (bodyTop > targetTop ? bodyTop - targetTop : 0) +
        Math.min(bodyHeight, target.height()) / 4;
    return { my: "center top", at: "center top", of: target, offset: "0 " + ofs };
}
$.fn.dataDisplay = function jQuery$dataDisplay(strDisplay) {
    return strDisplay === undefined ?
        this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}") :
        this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}", strDisplay);
};

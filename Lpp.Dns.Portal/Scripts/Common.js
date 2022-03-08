var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
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
    var PageViewModel = (function () {
        function PageViewModel(hiddenDataControl) {
            this.validationGroup = ko.validation.group(this);
            this.HiddenDataControl = hiddenDataControl;
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
            if (form && form.formChanged)
                form.formChanged(false);
            $("input[name=save]").prop("disabled", true);
            $("input[name=Copy]").prop("disabled", false);
            return true;
        };
        return PageViewModel;
    }());
    Dns.PageViewModel = PageViewModel;
    var ChildViewModel = (function () {
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
                        _this.raiseChange();
                    });
                }
            }
        };
        return ChildViewModel;
    }());
    Dns.ChildViewModel = ChildViewModel;
    var KeyValuePairData = (function () {
        function KeyValuePairData(key, value) {
            this.Key = key;
            this.Value = value;
        }
        return KeyValuePairData;
    }());
    Dns.KeyValuePairData = KeyValuePairData;
    var KeyValuePair = (function () {
        function KeyValuePair(key, value) {
            this.Key = ko.observable(key);
            this.Value = ko.observable(value);
        }
        return KeyValuePair;
    }());
    Dns.KeyValuePair = KeyValuePair;
    var SelectItem = (function (_super) {
        __extends(SelectItem, _super);
        function SelectItem(display, value) {
            return _super.call(this, display, (typeof value === "undefined") ? display : value) || this;
        }
        return SelectItem;
    }(KeyValuePairData));
    Dns.SelectItem = SelectItem;
})(Dns || (Dns = {}));
$.fn.alternateClasses = function jQuery$alternateClasses(arrayClasses) {
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

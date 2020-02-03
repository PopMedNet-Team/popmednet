/// <reference path="../../Lpp.Mvc.Composition/Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
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
            decorateElement: true,
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
        PageViewModel.prototype.raiseChange = function () {
            var form = $(".Content form");

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
                } else if (!!parent[prop] && parent[prop].constructor === Object) {
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
                form.formChanged(false); //This forces it to a generic variable because this is hacked into other code that cannot be pulled out right now.
            $("input[name=save]").prop("disabled", true);
            $("input[name=Copy]").prop("disabled", false);
            return true;
        };
        return PageViewModel;
    })();
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
            for (var prop in this) {
                if (this[prop] && this[prop].subscribe) {
                    this[prop].subscribe(function (value) {
                        //console.log("raising change for value: " + value);
                        _this.raiseChange();
                    });
                }
            }
        };
        return ChildViewModel;
    })();
    Dns.ChildViewModel = ChildViewModel;

    var KeyValuePairData = (function () {
        function KeyValuePairData(key, value) {
            this.Key = key;
            this.Value = value;
        }
        return KeyValuePairData;
    })();
    Dns.KeyValuePairData = KeyValuePairData;

    var KeyValuePair = (function () {
        function KeyValuePair(key, value) {
            this.Key = ko.observable(key);
            this.Value = ko.observable(value);
        }
        return KeyValuePair;
    })();
    Dns.KeyValuePair = KeyValuePair;

    // this class is used to create drop down lists
    var SelectItem = (function (_super) {
        __extends(SelectItem, _super);
        function SelectItem(display, value) {
            _super.call(this, display, (typeof value === "undefined") ? display : value);
        }
        return SelectItem;
    })(KeyValuePairData);
    Dns.SelectItem = SelectItem;
})(Dns || (Dns = {}));

//Fix Fyodor
$.fn.alternateClasses = function jQuery$alternateClasses(arrayClasses) {
    if (typeof arrayClasses != "Array")
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

var _loadingSignContextKey = "{4C3C9D9D-DAEC-4A7C-89CA-E76F92D60BE8}";
function loadingSignContext(target, context) {
    if (!context)
        context = target.data(_loadingSignContextKey);
    if (!context)
        target.data(_loadingSignContextKey, context = {});
    return context;
}

$.fn.showLoadingSign = function (options, context) {
    var target = this;
    options = $.extend({
        backgroundClass: "LoadingOverlay", foregroundClass: "LoadingSign",
        fadeInSpeed: 200, fadeOutSpeed: 200
    }, options);
    context = loadingSignContext(target, context);

    target.hideLoadingSign(context);
    var icon;
    var loadingSign = $('<div>').css({ width: 10, height: 10, position: "absolute", top: 0, left: 0 }).append($("<div>").addClass(options.backgroundClass).css({ width: target.width(), height: target.height() }).position({ my: "left top", at: "left top", of: target })).append(icon = $("<div>&nbsp;</div>").addClass(options.foregroundClass)).fadeIn(options.fadeInSpeed).appendTo("body");

    setTimeout(function () {
        return icon.position(targetCenterTopPosition(target));
    }, 20);

    context.sign = loadingSign;
    context.options = options;
    return target;
};

$.fn.hideLoadingSign = function (context) {
    var s = loadingSignContext(this, context);
    if (s && s.sign)
        s.sign.fadeOut(s.options.fadeOutSpeed, function () {
            s.sign.remove();
        });
    return this;
};

$.fn.floatErrorMessage = function (header, message, options) {
    var target = this;
    options = $.extend({
        foregroundClass: "ErrorMessage",
        autoRemoveTimeout: 3000, fadeOutSpeed: 1500, fadeInSpeed: 200
    }, options);

    var error = $("<div>").addClass(options.foregroundClass).append($("<span class='Header'>").text(header)).append($("<span class='Message'>").html(message)).appendTo("body").fadeIn(options.fadeInSpeed).position(targetCenterTopPosition(target));
    setTimeout(function () {
        error.fadeOut(options.fadeOutSpeed, function () {
            error.remove();
        });
    }, options.autoRemoveTimeout);
};

function targetCenterTopPosition(target) {
    var bodyTop = $("body").scrollTop();
    var bodyHeight = $(window).height();
    var targetTop = target.position().top;
    var targetHeight = target.height();
    var ofs = (bodyTop > targetTop ? bodyTop - targetTop : 0) + Math.min(bodyHeight, target.height()) / 4;

    return { my: "center top", at: "center top", of: target, offset: "0 " + ofs };
}

$.fn.dataDisplay = function jQuery$dataDisplay(strDisplay) {
    return strDisplay === undefined ? this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}") : this.data("{1863E2EF-AEF9-446E-B70D-DABACC8D9CAB}", strDisplay);
};
//# sourceMappingURL=Common.js.map

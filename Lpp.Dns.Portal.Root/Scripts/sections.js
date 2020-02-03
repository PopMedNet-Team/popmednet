var clientSettings;
//require(['clientSettings'], function (s) { clientSettings = s; });
$(function clientSettings(s) { clientSettings = s; });
$.fn.sections = function () {
    this.each(function () {
        var $this = $(this);
        if ($this.data(wrappedKey)) return this;
        $this.data(wrappedKey, true);

        var header = $("<div class='ui-section-header'>");
        var help = $(".ui-context-help", $this);
        help.hide();

        // If element is of class ui-minimizable and/or ui-maximizable, add corresponding buttons.
        var min = $this.hasClass("ui-minimizable");
        var max = $this.hasClass("ui-maximizable");
        if (min) header.append("<div class='ui-button-minimize' />");
        if (max) header.append("<div class='ui-button-maximize' />");
           
        header
            .append("<div class='ui-section-title'>" + $this.attr("title-text") + "</div>")
            .append($this.children(".ui-in-header"));
        // If help dialog exists, add a "help" button to the header section.
        if (help.size()) header.append($("<img class='helptip' src='../../../__r/Lpp.Dns.Portal/-/img/icons/help16.gif' style='float:left; margin-left:5px;' title='" + help.attr("tooltip-title") + "' />").qtip());

        // Add other buttons and bindings.
        var buttonsBindingsAttr = $this.attr("buttons-bindings");
        if (buttonsBindingsAttr) {
            buttonsBindings = buttonsBindingsAttr.split(",");
            $.each(buttonsBindings, function (index, buttonBinding) {
                var s = buttonBinding.split(":");
                header.append("<div class='" + s[0] + "' onclick='" + s[1] + "' />");
            });
        }

        $this.find("script").remove(); // Otherwise, all scripts will be executed twice

        $("<div class='ui-section-outside' />")
            .insertBefore($this)
            .append(header)
            .append($this);

        handleFlagEvent(header, "minimize", maximized, minimized);
        handleFlagEvent(header, "maximize", minimized, maximized);

        setTimeout(function () {
            $.each(["max", "min"], function () {
                if ($this.hasClass("ui-" + this + "imized")) {
                    header.find(".ui-button-" + this + "imize").click();
                }
            });
        }, 50);
    });

    return this;
};

var wrappedKey = "{E5D4A4D6-4C6C-4C81-AB8D-2730A7C2E0DE}";

function createTrigger(dataKey, fnOn, fnOff) {
    return function (section, flag) {
        var current = !!section.data(dataKey);
        if (flag === undefined) return current;
        if (!!flag == current) return;

        section.data(dataKey, flag);
        var sectionAndBorder = section.add(section.closest(".ui-section-outside"));
        if (flag) sectionAndBorder.addClass("ui-" + dataKey); else sectionAndBorder.removeClass("ui-" + dataKey);
        if (flag) fnOn(section); else fnOff(section);

        var clientSettingKey = section.data("settingskey");
        if (clientSettingKey && clientSettings && clientSettings.set) {
            clientSettings.set(clientSettingKey + "." + dataKey, flag ? 'true' : 'false');
        }
    };
}

function handleFlagEvent(context, name, clearFlag, toggleFlag) {
    $(".ui-button-" + name, context).click(function () {
        var section = $(this).closest(".ui-section-outside").children(".ui-section");
        if (clearFlag(section)) clearFlag(section, false);
        toggleFlag(section, !toggleFlag(section));
    });
}

function pxToInt(px) { return parseInt(px.replace("px", "")); }

var maximized = createTrigger("maximized",
    function (section) {
        var frame = section.closest(".ui-section-outside");
        var maxArea = frame.closest(".ui-maximize-area");
        if (!frame.size() || !maxArea.size()) return;

        var appendTarget = $( maxArea.children("form")[0] || maxArea );

        var otherChildren = appendTarget.children(":visible").filter(function () { return this != frame[0]; });
        var frameNextSibling = frame.next();
        var framePrevSibling = frame.prev();
        var frameParent = frame.parent();
        var oldMinHeight = frame.css("min-height");

        otherChildren.hide();
        frame.css("min-height", appendTarget.height());
        if (frameParent[0] != maxArea[0]) appendTarget.append(frame);

        section.data("demaximize", function () {
            otherChildren.filter(":not(.ui-section)").show();
            otherChildren.filter(function () { return !$(this).data("minimized"); }).show();
            frame.css("min-height", oldMinHeight);

            if (frameNextSibling.size()) frame.insertBefore(frameNextSibling);
            else if (framePrevSibling.size()) frame.insertAfter(framePrevSibling);
            else frame.appendTo(frameParent);
        });
    },
    function (section) {
        var demax = section.data("demaximize");
        if (demax) { demax(); section.removeData("demaximize"); }
    });

var minimized = createTrigger("minimized",
    function (section) { section.slideUp(100); },
    function (section) { section.slideDown(100); });

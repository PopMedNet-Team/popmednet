//define(['require', 'jQuery', 'css!./groupboxes.min.css'], function (require, $) {
$(function groupboxes(s) {
    var clientSettings;
    //require(['clientSettings'], function (s) { clientSettings = s; });
    
    $.fn.sections = function () {
        this.each(function () {
            var $this = $(this);
            if ($this.data(wrappedKey)) return this;
            $this.data(wrappedKey, true);

            var header = $("<div class='ui-section-header'>");
            header
                .append("<div class='ui-section-title'>" + $this.attr("title-text") + "</div>")
                .append($this.children(".ui-in-header"));

            // Add other buttons and bindings.
            var buttonsBindingsAttr = $this.attr("buttons-bindings");
            if (buttonsBindingsAttr) {
                buttonsBindings = buttonsBindingsAttr.split(",");
                $.each(buttonsBindings, function (index, buttonBinding) {
                    var s = buttonBinding.split(":");
                    header.append("<div class='" + s[0] + "' onclick='" + s[1] + "();' />");
                });
            }

            $this.find("script").remove(); // Otherwise, all scripts will be executed twice

            $("<div class='ui-section-outside' />")
                .insertBefore($this)
                .append(header)
                .append($this);

            handleFlagEvent(header, "minimize", maximized, minimized);
            handleFlagEvent(header, "maximize", minimized, maximized);

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

});
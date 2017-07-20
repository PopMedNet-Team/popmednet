//define(['jQuery', 'css!./ellipsisEditor.css'], function ($) {
$(function ellipsisEditor() {
    /* this     -   an element that gets hidden and replaced with an "ellipsis" button.
                    When the user clicks that button, a dialog is popped up with "this" as content.
      
       The logic that is inside "this" should use the $.dataDisplay() function (see utilities.js)
       to set the user-friendly representation that would be displayed for the user. */
    $.fn.ellipsisEditor = function jQuery$ellipsisEditor(options) {
        options = $.extend({
            dialog: {},
            valueContainer: "<span class='ui-ellipsis-value'></span>",
            button: "<button class='ui-ellipsis-button'>...</button>",
            getValue: function (target) { return ""; }
        }, options);

        var target = this;
        var displayContainer = $(options.valueContainer).insertBefore(target);
        //var button = $(options.button).insertBefore(target);

        target.hide();
        displayContainer.html(options.getValue(target));

        var button = $(options.button);
        //var dialogOptions = $.extend({
        //    modal: true,
        //    buttons: {
        //        Close: function () {
        //            target.dialog("close");
        //            displayContainer.html(options.getValue(target));
        //        }
        //    }
        //}, options.dialog); 

        var dialogOptions = $.extend({
            actions: ['Close'],
            modal: true,
            draggable: false,
            title: false,
            visible: false,
            minHeight: 400,
            close: function (e) {
                displayContainer.html(options.getValue(target));
            }
        }, options.dialog);

        button.click(function () {
            //target.dialog(dialogOptions);
            kendoWindow = $(target).kendoWindow(dialogOptions);
            kendoWindow.data("kendoWindow").center().open();
            return false;
        });

        button.insertBefore(target);

        return this;
    };
});
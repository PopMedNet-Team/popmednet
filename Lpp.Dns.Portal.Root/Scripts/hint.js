$.fn.hints = function () {
    var hint = $("body > div.ui-hint") || $("<div class='ui-hint'>").hide().appendTo("body");
    var hideTimer = 0;

    this.on("mouseover", ".ui-has-hint", function () {
        var txt = $(this).attr("hint-text");
        if (!txt) return;

        if (hideTimer) clearTimeout(hideTimer);
        hint
            .data("target", this)
            .html(txt)
            .css({ left: "", top: "" })
            .position({ my: "left bottom", at: "left top", of: this })
            .fadeIn(100);
    });

    this.on("mouseout", ".ui-has-hint", function () {
        if (hint.data("target") != this) return;
        hideTimer = setTimeout(function () { hint.fadeOut(100); }, 400);
    });

    return this;
};
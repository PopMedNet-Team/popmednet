/// <reference path="../../Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="../../Lpp.Mvc.Controls.Interfaces/utilities.d.ts" />
//define(['jQuery', 'lpp.mvc.controls/utilities'], () =>
//{
$(function dynamicPopup() {
    return function (_link) {
        var link = $(_link);
        var url = link.data("get-url");
        var popup;
        link.click(function () {
            if (!popup) {
                popup = $("<div>").appendTo(link.data("popup-parent") || "body")
                    .addClass((link.data("popup-class") || "").toString())
                    .css({ position: "absolute" });
                setTimeout(function () { return popup
                    .showLoadingSign()
                    .load(url, null, function () { return popup.hideLoadingSign(); }, position()); });
            }
            position();
            popup.fadeIn(100);
            setTimeout(function () { return $(document).one("click", function () { return popup.fadeOut(100); }); }, 50);
            return false;
        });
        function position() {
            popup.css({ left: "", top: "" })
                .position({ my: "left top", at: "left bottom", of: link });
        }
    };
});
//# sourceMappingURL=dynamicPopup.js.map
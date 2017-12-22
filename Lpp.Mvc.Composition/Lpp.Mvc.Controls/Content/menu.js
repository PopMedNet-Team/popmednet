//define(['jQuery'], function ($) {
$(function menuItems() {
    $.fn.menuItems = function jQuery$menuItems() {
        var allMenuItems = this;
        var slideUpTimer = 0;

        allMenuItems.hover(
            function () {
                var subMenu = $(this).children(".SubMenu");
                if (subMenu.size() && !subMenu.is(":visible")) {
                    if (slideUpTimer) clearTimeout(slideUpTimer);
                    slideUpTimer = 0;

                    $(".SubMenu", allMenuItems).slideUp(100);
                    subMenu
                        .css({ left: "", top: "" })
                        .position({ my: "left top", at: "left bottom", of: this })
                        .slideDown(100);
                }
            },
            function () {
                var subMenu = $(this).children(".SubMenu");
                if (subMenu.size()) {
                    slideUpTimer = setTimeout(function () {
                        slideUpTimer = 0;
                        subMenu.slideUp(100);
                    }, 1000);
                }
            }
        );

        $("a", allMenuItems).click(function () { if (!$(this).attr('href')) return false; });

        return this;
    };
});
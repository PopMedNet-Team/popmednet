//require(['jQuery', 'css!./contextMenu.css'], function ($) {
$(function contextMenu($) {
    $.fn.contextMenu = function jQuery$contextMenu( menu ) {
        menu = $(menu);

        function hide() { 
            menu.fadeOut( 200 ); 
            $(document).unbind("click", hide);
        }

        this.mousedown(function (ev) {
            if (ev.which == 3) { // Right click
                menu
                    .css({ position: "absolute", left: ev.pageX, top: ev.pageY })
                    .fadeIn(200);
                $(document).bind("click", hide);
            }
        });
    };

});
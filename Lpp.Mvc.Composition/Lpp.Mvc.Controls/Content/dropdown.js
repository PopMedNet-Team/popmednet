
$(function dropdown(){
    $.fn.dropdown = function () {
        this.each(function () {
            var $this = $(this);
            var dropdown = $this.next('.ui-dropdown');

            $this.click(function () {
                dropdown.css({ left: "", top: "" }).position({ my: 'left top', at: 'left bottom', of: this }).fadeIn(100);
                $(document).one("click", function() { dropdown.fadeOut(100); });
                return false;
            });
        });

        return this;
    };
});


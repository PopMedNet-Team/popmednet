
$(function objectSelector() {
    $.fn.objectSelector = function (dialogTitle) {
        var $this = this;
        var dlg = $(".Options", $this);
        var field = $this.children("input[type=hidden]");
        var link = $this.children("a.Link");
        link.click(function () {
            dlg.dialog({
                modal: true, title: dialogTitle,
                width: 600, buttons: { Cancel: function () { dlg.dialog("close"); } }
            });
            return false;
        });

        $this.children("a.Clear").click(function () {
            if ( field.val() ) field.val('').trigger("change");
            link.text('<none>'); 
            return false;
        });

        dlg.delegate("a.ChooseLink", "click", function () {
            var id = $(this).closest("tr").attr("id");
            if (id) {
                dlg.dialog("close");
                link.text($(this).text());
                var props = $('a', $(this).closest("tr")).data("props")
                if (field.val() != id) field.val(id).trigger("change", props);
            }
            return false;
        });

        return this;
    }
});
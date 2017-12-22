$(document).ready(function () {
    $.fn.IsKeyPrintable = function jQuery$IsKeyPrintable(e) {
        if (e.which == 27) e.preventDefault(); if (e.ctrlKey || e.altKey) return;
        if ((e.which == 8 || e.which == 32 || e.which == 42 || e.which == 43 || e.which == 59 || e.which == 61 || e.which == 78 || e.which == 109) || (e.which > 44 && e.which < 58) || (e.which > 64 && e.which < 91) || (e.which > 95 && e.which < 112) || (e.which > 185 && e.which < 193) || (e.which > 218 && e.which < 223))
            return true;
    };

    $.fn.formChanged = function jQuery$isFormChanged(changed) {
        var form = this;
        var dataKey = "formChanged";
        if (changed == undefined) return !!(form.data(dataKey));
        else {
            changed = !!changed;
            if (changed != form.data(dataKey)) {
                form.data(dataKey, changed);
                if (changed)
                    form.trigger(dataKey);
            }
        }
    };

    $.fn.affectFormChange = function jQuery$affectFormChange(affect) {
        if (affect == undefined) return !this.data("dontAffectFormChange");
        else this.data("dontAffectFormChange", !affect);
    };

    function bindToForm() {
        var form = $("form.trackChanges");
        if (form.size() == 0) {
            window.setTimeout(bindToForm, 300);
            return;
        }

        $("input, textarea, select", form).change(function () {
            if (($(this).affectFormChange == null || $(this).affectFormChange()) && form.formChanged != null)
                form.formChanged(true);
        });

        $(window).bind("beforeunload", function () {
            if (form.formChanged())
                return "You have made changes to this form. Are you sure you want to discard them?";
        });
    }

    $(bindToForm);
});
define(['jQuery'], function ($) {

    // "this" - the input of type "checkbox" that is the header of the column
    $.fn.gridCheckboxColumn = function jQuery$gridCheckboxColumn(hiddenFieldName, allPossibleIdsCommaSeparated) {
        var $globalCheckbox = this;
        var $grid = this.parents("table").eq(0);
        $grid.ready(function () {

            var $column = $globalCheckbox.parents("th").eq(0);
            var $index = $column.parent().children().index($column);
            var $hiddenField = $("input[name=" + hiddenFieldName + "]");
            if ($grid.size() == 0) return;

            function parseSelectedList() {
                var res = ($hiddenField.val() || "").split(',');
                for (var i in res) if (!res[i]) res.splice(i, 1);
                return res;
            }
            function allChecked() { for (var i = 0; i < $allCheckboxes.size(); i++) if (!$allCheckboxes.eq(i).prop("checked")) return false; return true; }
            function idOf(cb) { return $(cb).parents("tr").eq(0).attr("id"); }

            var $allCheckboxes =
                    $("tr", $grid)
                    .filter(function () { return $(this).attr("id"); })
                    .map(function () { return $(this).children()[$index]; })
                    .find("input");

            var list = parseSelectedList();
            $allCheckboxes.each(function () {
                var $this = $(this);
                var id = idOf(this);
                if (!id) return;
                var index = $.inArray(id, list);
                $this.prop("checked", index >= 0);
            });
            $globalCheckbox.prop("checked", list.length > 0);

            $allCheckboxes.bind("click change", function () {
                var $this = $(this);
                var id = idOf(this);
                if (!id) return;
                var list = parseSelectedList();
                var index = $.inArray(id, list);
                var checked = $this.prop("checked");

                if (checked && index < 0) list.push(id);
                if (!checked && index >= 0) list.splice(index, 1);
                $hiddenField.val(list.join(','));
                $hiddenField.trigger("change");
                $globalCheckbox.prop("checked", list.length > 0);
            });

            $globalCheckbox.bind("change click", function () {
                var checked = $(this).prop("checked");

                var newHiddenFieldValue =
                    checked ?
                        allPossibleIdsCommaSeparated || $.makeArray($.map($allCheckboxes, idOf)).join(",")
                        : "";

                $hiddenField.val(newHiddenFieldValue);
                $hiddenField.trigger("change");
                $allCheckboxes.prop("checked", checked).trigger("change");
            });

        }); // $grid.ready

        return this;
    };

});
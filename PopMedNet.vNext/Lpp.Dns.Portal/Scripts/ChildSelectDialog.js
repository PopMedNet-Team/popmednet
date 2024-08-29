define(['jQuery', 'rx'], function ($, Rx) {
    var fnOnChosenKey = "{08F45E78-21C6-46EE-BFCA-EDA2D0D5C258}";

    $.fn.childSelectDialog = function (dialogOptions, fnLoadChildrenUrl /* function( projId, orgId, searchTerm ) */) {
        var dialog = $(this);
        var searchBox = $(".ChildrenBox .Search", dialog);
        if ( searchBox.affectFormChange ) searchBox.affectFormChange(false);

        dialog.delegate(".Children .ChooseLink", "click", function () {
            var fn = dialog.data(fnOnChosenKey);
            if ( !fn ) return;
            dialog.removeData(fnOnChosenKey).dialog("close");
            fn([{ Id: $(this).closest("tr").attr("id"), Name: dialog.find(".ParentLink.Selected").text() + "\\" + $(this).text() }]);
        });

        $(".Header", dialog).click(function() {
            var t = $(this).next(".Nodes");
            if (t.is(":visible")) t.slideUp(100);
            else t.slideDown(100);
        })

        function selectedParentId(kind) { return dialog.find(".Parents > ." + kind + " .ParentLink.Selected").closest(".Node").attr("id"); }

        function reloadChildren() {
            $(".ChildrenBox .Children", dialog)
                .empty()
                .append('<div class="Loading">&nbsp;</div>')
                .load(fnLoadChildrenUrl(selectedParentId("Projects"), selectedParentId("Orgs"), searchBox.val()));
        }

        dialog.delegate(".Parents .ParentLink", "click", function () {
            var $this = $(this);
            if ( $this.is(".Selected") ) return;

            $(".Parents .ParentLink.Selected", dialog).removeClass("Selected");
            $this.addClass("Selected");
            searchBox.val("");

            reloadChildren();
            return false;
        } );

        $(".Parents .ParentLink:first", dialog).click();

        Rx.Observable.fromArray( [ searchBox.toObservable("change blur keypress"), Rx.Observable.timer( 500, 500 ) ] )
            .mergeObservable()
            .delay(1) 
            .select(function () { return searchBox.val(); })
            .where( function(v) { return !!v; } )
            .distinctUntilChanged()
            .throttle(500)
            .subscribe(reloadChildren);

        return function (fnOnChosen) {
            dialog
                .data(fnOnChosenKey, fnOnChosen)
                .dialog($.extend({ width: 600, height: 400, modal: true }, dialogOptions));
        };
    };
});
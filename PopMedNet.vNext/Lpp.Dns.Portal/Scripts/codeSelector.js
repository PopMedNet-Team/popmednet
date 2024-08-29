//define(['jQuery', 'rx', 'lpp.mvc.controls/utilities'], function ($) {
$(function (){
    /* selectedCodes = [ { code:'1', name:'A' }, { code:'2', name:'B' } ... ]  */
    $.fn.codeSelector = function jQuery$codeSelector(hiddenField, searchUrl, categoryUrl, selectedCodes, codeColumnClass, nameColumnClass) {
        var me;

        /* we must filter the hiddenfield, as KO currently is not specified correctly using
           real templates with type=text/html, so there will be two (the template and the bound version) */

        var isKO = typeof (KOViewModel) !== 'undefined';

        if (isKO)
            hiddenField = $(hiddenField).filter('[data-bind]');

        if (this.length > 1) {
            me = $(this[1]);            
        } else {
            me = this;
        }

        if (hiddenField.length > 1)
            hiddenField = $(hiddenField[1]);
 
        var searchBox = me.find(".Search");
        if (searchBox.length > 1)
            searchBox = $(searchBox[1]);

        var categoryList = me.find(".Category");
        if (categoryList.length > 1)
            categoryList = $(categoryList[1]);

        var searchResultBox = me.find(".SearchResultBox");
        if (searchResultBox.length > 1)
            searchResultBox = $(searchResultBox[1]);

        var searchResult = me.find(".SearchResult");
        if (searchResult.length > 1)
            searchResult = $(searchResult[1]);

        var selectedCodesArea = me.find(".SelectedCodes");
        if (selectedCodesArea.length > 1)
            selectedCodesArea = $(selectedCodesArea[1]);

        var emptySelectionMessage = me.find(".EmptySelectionMessage");
        if (emptySelectionMessage.length > 1)
            emptySelectionMessage = $(emptySelectionMessage[1]);

        var allSelectedCodes = function () {
            return $("tr", selectedCodesArea);
        };

        var allResultAddLinks = function () {
            return $("a.Add", searchResult);
        };

        var textSearch =
                    //searchBox
                    //.toObservable("keypress paste textInput input")
                    Rx.Observable.fromEvent(searchBox, 'keypress paste textInput input')
                    .where(function () { return !!searchBox.val(); })
                    .throttle(500)
                    .select(function () {
                        return searchUrl.replace('_query_', searchBox.val());
                    });

        var categorySearch =
                    //categoryList
                    //.toObservable("change")
                    Rx.Observable.fromEvent(categoryList, 'change')
                    .throttle(100)
                    .select(function () {
                        searchBox.val("");
                        return categoryUrl.replace('_cat_', categoryList.val());
                    });

        Rx.Observable.merge(null, textSearch, categorySearch)
                    .selectMany(function (url) {
                        return searchResult.loadAsObservable(url);
                    })
                    .doAction(function () {
                        toggleExistingResultLinks();
                    })
                    .retry()
                    .subscribe(function () { });

        function selectedCode(id, name, codeColumnClass, nameColumnClass) {
            return $("<tr>").attr("id", id)
                .append($("<td>").append(
                    $("<a href='#' class='Delete'>[remove]</a>")
                    .click(function () {
                        $(this).parents("tr").eq(0).remove();
                        updateHiddenField();
                        toggleExistingResultLinks();
                        updateSelectedCodesArea();
                        return false;
                    }))
                )
                .append($("<td class='" + codeColumnClass + "'>").text(id))
                .append($("<td class='" + nameColumnClass + "'>").text(name));
            return res;
        }

        searchResult.delegate("a.Add", "click", function () {
            var id = $(this).attr("id");
            $(this).hide();
            if ($.inArray(id, allSelectedCodeIds()) >= 0) return false;
            selectedCodesArea.append(selectedCode(id, $(this).attr("href"), codeColumnClass, nameColumnClass));
            updateSelectedCodesArea();
            updateHiddenField();

            return false;
        });

        function distinctVals(arr) {
            var newArray = [];

            for (var i = 0, j = arr.length; i < j; i++) {
                if ($.inArray(arr[i], newArray) == -1)
                    newArray.push(arr[i]);
            }

            return newArray;
        }

        function allSelectedCodeIds() {
            return $.map(allSelectedCodes(), function (e) {
                return $(e).attr("id");
            })
        }

        function distinctSelectedCodeIds() {
            return distinctVals(allSelectedCodeIds());
        }

        function updateHiddenField() {
            var codes = distinctSelectedCodeIds().join("\t").replace(/,/g, "%comma;").replace(/\t/g, ",");
            hiddenField.val(codes);
            /* raise the change event on the hidden input field... if it is two-way bound to KO,
               this will make sure the underlying observable in the viewmodel gets updated */
            hiddenField.change();            
            updateDataDisplay();
        }

        function toggleExistingResultLinks() {
            var ids = allSelectedCodeIds();
            allResultAddLinks().each(function () {
                $(this).toggle($.inArray($(this).attr("id"), ids) < 0);
            });
        }

        function updateSelectedCodesArea() {
            var codes = allSelectedCodes();
            codes.alternateClasses("", "Alt");
            emptySelectionMessage.toggle(!codes.size());
        }

        searchResult.bind("reloaded", toggleExistingResultLinks);
        selectedCodes.forEach(function (item) {
            if (item.code && item.name)
                selectedCodesArea.append(selectedCode(item.code, item.name, codeColumnClass, nameColumnClass));
        });

        function updateDataDisplay() {
            me.dataDisplay(hiddenField.val() || "No codes selected");
        }

        /* this will scrub out dup values, fix the hidden field, then display the codes in a list, but need to
           accomodate the case where multiple copies of this control or the hidden control are on the page, so
           don't kill off a real value for a fake value */
        if ((isKO && (hiddenField.val() == '')) || !isKO)
            updateHiddenField();

        /* this will show the grid on the right */
        updateSelectedCodesArea();

        return me;
    };
});
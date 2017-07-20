define(['jQuery'], function ($) {
    var flgcheckgroup = false;
    // "this" - the td or th tag that is the header of the column
    // fnCompareRows = function( cell1, cell2 ) { return cell1 > cell2 ? 1 : cell1 < cell2 ? -1 : 0; }
    $.fn.gridClientSideSortColumn = function jQuery$gridClientSideSortColumn(fnCompareCells, bAscendingByDefault) {
        if (bAscendingByDefault == undefined) bAscendingByDefault = true;

        var $grid = this.parents("table").eq(0);
        if ($grid.size() == 0) return;

        var $index = this.parent().children().index(this);
        var $body = $("tbody", $grid);
        this.addClass("ClientSortable").prepend('<div class="SortIcon">&nbsp;</div>');
        this.click(function () {
            var $header = $(this);
            var $headertext = $(this).text();
            if ($headertext.indexOf("Year Age Group") !== -1) {
                flgcheckgroup = true;
            }
            else {
                flgcheckgroup = false;
            }
            var sortedAsc = $header.hasClass("SortedAsc");
            var sortedDesc = $header.hasClass("SortedDesc");
            var sortAsc = (!sortedAsc && !sortedDesc) ? bAscendingByDefault : !sortedAsc;
            var compCells = sortAsc ? fnCompareCells : function (a, b) { return fnCompareCells(b, a); };
            var compRows = function (r1, r2) { return compCells(r1[$index], r2[$index]); }
            var allRows = $body.children();
            var allCells = $.map(allRows, function (row) { return $(row).children(); });
            var sortedCells = allCells.sort(compRows);
            $grid.find(".ClientSortable").removeClass("SortedAsc").removeClass("SortedDesc");
            $header.addClass(sortAsc ? "SortedAsc" : "SortedDesc");
            for (var i = 0; i < sortedCells.length; i++) {
                sortedCells[i].appendTo(allRows[i]);
            }

        });

        return this;
    };

    return {
        defaultComparer: function (expressionAttributeName) {
            return function (a, b) {
                try {
                    var x = $(a).attr(expressionAttributeName);
                    var y = $(b).attr(expressionAttributeName);
                    if (flgcheckgroup == true) {
                        var tempX = x.substring(1).split("-");
                        var tempY = y.substring(1).split("-");
                        if (tempX[0].length == 1) {

                            x = "00" + tempX[0];
                        }
                        if (tempX[0].length == 2) {
                            x = "0" + tempX[0];
                        }
                        if (tempY[0].length == 1) {
                            y = "00" + tempY[0];
                        }
                        if (tempY[0].length == 2) {
                            y = "0" + tempY[0];
                        }
                    }
                    x = x == "" ? null : eval(x);
                    y = y == "" ? null : eval(y);
                    return x == y ? 0 :
                           x > y ? 1 :
                           -1;
                }
                catch (e)
                { return 0; }
            };
        }
    };
});
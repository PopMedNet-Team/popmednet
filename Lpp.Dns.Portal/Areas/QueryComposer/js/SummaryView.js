var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var SummaryView;
            (function (SummaryView) {
                function GetVisualTerms() {
                    var d = $.Deferred();
                    $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
                        .done(function (result) {
                        d.resolve(result);
                    }).fail(function (e, description, error) {
                        d.reject(e);
                    });
                    return d;
                }
                function init() {
                    var ID = Global.GetQueryParam("ID");
                    $.when(GetVisualTerms(), Dns.WebApi.Requests.Get(ID)).done(function (visualTerms, requests) {
                        Plugins.Requests.QueryBuilder.View.init(JSON.parse(requests[0].Query), visualTerms, $('#queryview'));
                    });
                }
                SummaryView.init = init;
                init();
                $(function () {
                    $('#summaryviewloadingcontainer').toggle();
                });
            })(SummaryView = QueryBuilder.SummaryView || (QueryBuilder.SummaryView = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));
//# sourceMappingURL=SummaryView.js.map
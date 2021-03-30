var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var SummaryView;
            (function (SummaryView) {
                function init() {
                    var ID = Global.GetQueryParam("ID");
                    Dns.WebApi.Requests.Get(ID)
                        .done(function (requests) {
                        Plugins.Requests.QueryBuilder.View.initialize(JSON.parse(requests[0].Query), new Dns.ViewModels.RequestViewModel(requests[0]), $("#overview-queryview"));
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

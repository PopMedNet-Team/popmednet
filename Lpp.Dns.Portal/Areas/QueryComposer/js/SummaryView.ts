module Plugins.Requests.QueryBuilder.SummaryView {

    export function init() {
        var ID: any = Global.GetQueryParam("ID");

        Dns.WebApi.Requests.Get(ID)
            .done((requests: Dns.Interfaces.IRequestDTO[]) => {
                Plugins.Requests.QueryBuilder.View.initialize(JSON.parse(requests[0].Query), new Dns.ViewModels.RequestViewModel(requests[0]), $("#overview-queryview"));
        });
    }

    init();

    $(() => {
        $('#summaryviewloadingcontainer').toggle();
    });
} 
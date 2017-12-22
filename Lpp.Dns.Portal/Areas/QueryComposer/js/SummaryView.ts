module Plugins.Requests.QueryBuilder.SummaryView {

    function GetVisualTerms(): JQueryDeferred<IVisualTerm[]> {
        var d = $.Deferred<IVisualTerm[]>();

        $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
            .done((result: IVisualTerm[]) => {
            d.resolve(result);
        }).fail((e, description, error) => {
            d.reject(<any>e);
        });

        return d;
    }

    export function init() {
        var ID: any = Global.GetQueryParam("ID");
        $.when<any>(
            GetVisualTerms(),
            Dns.WebApi.Requests.Get(ID)
        ).done((visualTerms: IVisualTerm[], requests: Dns.Interfaces.IRequestDTO[]) => {
            Plugins.Requests.QueryBuilder.View.init(JSON.parse(requests[0].Query), visualTerms,  $('#queryview'));
        });
    }

    init();

    $(() => {
        $('#summaryviewloadingcontainer').toggle();
    });
} 
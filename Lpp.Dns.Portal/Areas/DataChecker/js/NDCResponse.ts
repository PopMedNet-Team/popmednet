/// <reference path="../../../../Lpp.Mvc.Composition/Lpp.Mvc.Boilerplate/jsBootstrap.d.ts" />
/// <reference path="common.ts" />
module DataChecker.NDC {
    var vm: ViewModel;
    var _bindingControl: JQuery;

    export class ViewModel {
        public requestID: KnockoutObservable<any> = ko.observable(null);
        public responseID: KnockoutObservable<any> = ko.observable(null);
        public isLoaded: KnockoutObservable<boolean> = ko.observable<boolean>(false);

        public DataPartners: Array<any> = [];
        public CountByDataPartner: Array<any> = [];
        public HasResults: boolean = false;

        constructor(parameters: any) {
            var self = this;

            if (parameters == null) {
                return;
            }
            else if (parameters.ResponseID == null || parameters.ResponseID() == null) {
                return;
            }
            else if (parameters.RequestID == null || parameters.RequestID() == null) {
                return;
            }

            self.responseID(parameters.ResponseID());
            self.requestID(parameters.RequestID());

            $.when<any>(
                $.get('/DataChecker/NationalDrugCodes/GetResponseDataset?responseID=' + self.responseID().toString())
                ).then((data: any) => {

                var table = data.Table;

                self.DataPartners = $.Enumerable.From(table).Distinct((item: INDCItemData) => item.DP).Select((item: INDCItemData) => item.DP).OrderBy(x => x).ToArray();
                self.CountByDataPartner = $.Enumerable.From(table)
                    .GroupBy((x: INDCItemData) => x.NDC,
                    (x: INDCItemData) => x,
                    (key, group) => <any>{
                        NDC: key,
                        TotalCount: $.Enumerable.From(group.source).Count(x => x.NDC == key),
                        Partners: $.Enumerable.From(self.DataPartners).Select(dp => <any>{ Partner: dp, Count: $.Enumerable.From(group.source).Count(x => x.NDC == key && dp == x.DP) }).ToArray()
                    }).ToArray();

                self.HasResults = self.CountByDataPartner.length > 0;

                self.isLoaded(true);

                //resize the iframe to the contents plus padding for the export dropdown menu
                $(window.frameElement).height($('html').height() + 70);

            }).fail((error) => {
                alert(error);
                return;
            });
        }

    }
} 
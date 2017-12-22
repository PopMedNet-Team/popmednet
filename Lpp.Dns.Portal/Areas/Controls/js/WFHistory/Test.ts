module Controls.WFHistory.Test {
    var requestID = '4AD8BF04-19F3-4F25-82B1-A3AF00A9E96A';
    var vm: ViewModel;

    export class ViewModel {
        public Requests: any[];
        public Request: KnockoutObservable<any>;

        constructor(requests: any[]) {
            this.Requests = requests;
            this.Request = ko.observable(requests[0]);

            this.Request.subscribe((v) => {
                Controls.WFHistory.List.setRequestID(v.ID);
            });
        }
    }

    export function init() {
        $.when<any>(
            Dns.WebApi.Requests.List(null, 'ID,Identifier,Name', 'Identifier desc')
            ).done((requests: any) => {

                $(() => {

                    vm = new ViewModel(requests);
                    ko.applyBindings(vm, $('#Content')[0]);

                    Controls.WFHistory.List.init(requests[0].ID);

                });

            });

    }

    init();
}  
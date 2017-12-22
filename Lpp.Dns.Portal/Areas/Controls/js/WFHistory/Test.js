var Controls;
(function (Controls) {
    var WFHistory;
    (function (WFHistory) {
        var Test;
        (function (Test) {
            var requestID = '4AD8BF04-19F3-4F25-82B1-A3AF00A9E96A';
            var vm;
            var ViewModel = (function () {
                function ViewModel(requests) {
                    this.Requests = requests;
                    this.Request = ko.observable(requests[0]);
                    this.Request.subscribe(function (v) {
                        Controls.WFHistory.List.setRequestID(v.ID);
                    });
                }
                return ViewModel;
            }());
            Test.ViewModel = ViewModel;
            function init() {
                $.when(Dns.WebApi.Requests.List(null, 'ID,Identifier,Name', 'Identifier desc')).done(function (requests) {
                    $(function () {
                        vm = new ViewModel(requests);
                        ko.applyBindings(vm, $('#Content')[0]);
                        Controls.WFHistory.List.init(requests[0].ID);
                    });
                });
            }
            Test.init = init;
            init();
        })(Test = WFHistory.Test || (WFHistory.Test = {}));
    })(WFHistory = Controls.WFHistory || (Controls.WFHistory = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=Test.js.map
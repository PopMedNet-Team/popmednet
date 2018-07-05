/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
module Dialog.RoutingHistory {
    var vm: SessionExpiringViewModel;

    export class SessionExpiringViewModel extends Global.DialogViewModel {
        public CountDown: KnockoutObservable<string> = ko.observable("");

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            var self = this;
            setInterval(() => {
                self.CountDown(self.Timer());
            }, 1000)

        }

        public Timer(): string {
            let self = this;
            ///Due to regression in IE if we want to get an updated local storage item that may have been updated cross tab, we need to set a random Item first which will force a refresh of the localStorage.
            if (Global.Helpers.DetectInternetExplorer()) {
                localStorage.setItem("ieFix", null);
            }
            var exireDateTime = localStorage.getItem("SessionExpire");
            if (moment(exireDateTime).isAfter(moment(new Date()))) {
                var time = moment(exireDateTime).toDate().getTime() - moment(new Date()).toDate().getTime();
                var seconds = Math.floor((time / 1000) % 60);
                var minutes = Math.floor((time / 1000 / 60));
                if ((seconds <= 1 && minutes <= 0 )|| minutes > 5) {
                    self.Close(false);
                }
                return minutes + ":" + (seconds < 10 ? "0" + seconds.toString() : seconds.toString());
            }
            else {
                self.Close(false);
            }
            
        }

        public RefreshSession() {
            this.Close(true);
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("SessionExpiringSection");
            vm = new SessionExpiringViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();
}
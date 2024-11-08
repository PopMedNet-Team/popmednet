var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Dialog;
(function (Dialog) {
    var RoutingHistory;
    (function (RoutingHistory) {
        var vm;
        var SessionExpiringViewModel = (function (_super) {
            __extends(SessionExpiringViewModel, _super);
            function SessionExpiringViewModel(bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.CountDown = ko.observable("");
                var self = _this;
                setInterval(function () {
                    self.CountDown(self.Timer());
                }, 1000);
                return _this;
            }
            SessionExpiringViewModel.prototype.Timer = function () {
                var self = this;
                if (Global.Helpers.DetectInternetExplorer()) {
                    localStorage.setItem("ieFix", null);
                }
                var exireDateTime = localStorage.getItem("SessionExpire");
                if (moment(exireDateTime).isAfter(moment(new Date()))) {
                    var time = moment(exireDateTime).toDate().getTime() - moment(new Date()).toDate().getTime();
                    var seconds = Math.floor((time / 1000) % 60);
                    var minutes = Math.floor((time / 1000 / 60));
                    if ((seconds <= 1 && minutes <= 0) || minutes > 5) {
                        self.Close(false);
                    }
                    return minutes + ":" + (seconds < 10 ? "0" + seconds.toString() : seconds.toString());
                }
                else {
                    self.Close(false);
                }
            };
            SessionExpiringViewModel.prototype.RefreshSession = function () {
                this.Close(true);
            };
            return SessionExpiringViewModel;
        }(Global.DialogViewModel));
        RoutingHistory.SessionExpiringViewModel = SessionExpiringViewModel;
        function init() {
            $(function () {
                var bindingControl = $("SessionExpiringSection");
                vm = new SessionExpiringViewModel(bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        init();
    })(RoutingHistory = Dialog.RoutingHistory || (Dialog.RoutingHistory = {}));
})(Dialog || (Dialog = {}));

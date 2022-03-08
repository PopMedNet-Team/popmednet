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
var Requests;
(function (Requests) {
    var Create;
    (function (Create) {
        var vm;
        var ViewModel = (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(projectID, requestTypes, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                var self = _this;
                self.RequestTypes = requestTypes;
                _this.onSelectRequestType = function (requestType) {
                    self.Close(requestType);
                };
                return _this;
            }
            return ViewModel;
        }(Global.DialogViewModel));
        Create.ViewModel = ViewModel;
        function init() {
            var window = Global.Helpers.GetDialogWindow();
            var projectID = (window.options).parameters.ProjectID || null;
            if (projectID === undefined || projectID == '')
                projectID = Constants.GuidEmpty;
            Dns.WebApi.Projects.GetAvailableRequestTypeForNewRequest(projectID, null, null, "Name").done(function (results) {
                var bindingControl = $('#Content');
                vm = new ViewModel(projectID, results, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        Create.init = init;
        init();
    })(Create = Requests.Create || (Requests.Create = {}));
})(Requests || (Requests = {}));

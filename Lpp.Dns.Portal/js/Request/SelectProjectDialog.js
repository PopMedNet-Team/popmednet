/// <reference path="../../Scripts/page/Page.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Requests;
(function (Requests) {
    var Utility;
    (function (Utility) {
        var vm;
        var ViewModel = /** @class */ (function (_super) {
            __extends(ViewModel, _super);
            function ViewModel(projects, bindingControl) {
                var _this = _super.call(this, bindingControl) || this;
                _this.Projects = projects;
                return _this;
            }
            ViewModel.prototype.onSelectProject = function (project) {
                vm.Close({ ID: project.ID, Name: project.Name });
            };
            return ViewModel;
        }(Global.DialogViewModel));
        Utility.ViewModel = ViewModel;
        function init() {
            $(function () {
                var window = Global.Helpers.GetDialogWindow();
                var projects = (window.options).parameters.Projects;
                var bindingControl = $("#Content");
                vm = new ViewModel(projects, bindingControl);
                ko.applyBindings(vm, bindingControl[0]);
            });
        }
        Utility.init = init;
        init();
    })(Utility = Requests.Utility || (Requests.Utility = {}));
})(Requests || (Requests = {}));

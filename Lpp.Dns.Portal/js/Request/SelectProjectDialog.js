/// <reference path="../../../Lpp.Pmn.Resources/Scripts/page/5.1.0/Page.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Request;
(function (Request) {
    var Utility;
    (function (Utility) {
        var vm;
        var ViewModel = (function (_super) {
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
    })(Utility = Request.Utility || (Request.Utility = {}));
})(Request || (Request = {}));
//# sourceMappingURL=SelectProjectDialog.js.map
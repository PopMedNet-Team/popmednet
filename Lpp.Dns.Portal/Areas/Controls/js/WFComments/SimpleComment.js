var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../../../js/_layout.ts" />
var Controls;
(function (Controls) {
    var WFComments;
    (function (WFComments) {
        var SimpleComment;
        (function (SimpleComment) {
            var vm;
            var ViewModel = (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.Comment = ko.observable('');
                    var self = _this;
                    self.onCancel = function () {
                        self.Close();
                    };
                    self.onSave = function () {
                        if (!self.Validate())
                            return;
                        self.Close(self.Comment());
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            SimpleComment.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            SimpleComment.init = init;
            init();
        })(SimpleComment = WFComments.SimpleComment || (WFComments.SimpleComment = {}));
    })(WFComments = Controls.WFComments || (Controls.WFComments = {}));
})(Controls || (Controls = {}));
//# sourceMappingURL=SimpleComment.js.map
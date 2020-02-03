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
/// <reference path="../../../../js/_layout.ts" />
var Controls;
(function (Controls) {
    var WFComments;
    (function (WFComments) {
        var AddComment;
        (function (AddComment) {
            var vm;
            var ViewModel = /** @class */ (function (_super) {
                __extends(ViewModel, _super);
                function ViewModel(bindingControl) {
                    var _this = _super.call(this, bindingControl) || this;
                    _this.RequestID = _this.Parameters.RequestID;
                    _this.WorkflowActivityID = _this.Parameters.WorkflowActivityID;
                    _this.Comment = ko.observable('');
                    var self = _this;
                    self.onCancel = function () {
                        self.Close();
                    };
                    self.onSave = function () {
                        if (!self.Validate())
                            return;
                        Global.Helpers.ShowExecuting();
                        Dns.WebApi.Comments.AddWorkflowComment({
                            RequestID: self.RequestID,
                            WorkflowActivityID: self.WorkflowActivityID,
                            Comment: self.Comment()
                        })
                            .done(function (result) {
                            result.forEach(function (r) {
                                r.CreatedOn = moment.utc(r.CreatedOn).toDate();
                            });
                            self.Close(result[0]);
                        })
                            .always(function () { Global.Helpers.HideExecuting(); });
                    };
                    return _this;
                }
                return ViewModel;
            }(Global.DialogViewModel));
            AddComment.ViewModel = ViewModel;
            function init() {
                $(function () {
                    var bindingControl = $('#Content');
                    vm = new ViewModel(bindingControl);
                    ko.applyBindings(vm, bindingControl[0]);
                });
            }
            AddComment.init = init;
            init();
        })(AddComment = WFComments.AddComment || (WFComments.AddComment = {}));
    })(WFComments = Controls.WFComments || (Controls.WFComments = {}));
})(Controls || (Controls = {}));

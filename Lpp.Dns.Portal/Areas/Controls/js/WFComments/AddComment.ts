/// <reference path="../../../../js/_layout.ts" />
module Controls.WFComments.AddComment {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public RequestID: any;
        public WorkflowActivityID: any;
        public Comment: KnockoutObservable<string>;

        public onCancel: () => void;
        public onSave: () => void;

        constructor(bindingControl: JQuery) {
            super(bindingControl);
            
            this.RequestID = this.Parameters.RequestID;
            this.WorkflowActivityID = this.Parameters.WorkflowActivityID;
            this.Comment = ko.observable('');
            
            var self = this;
            self.onCancel = () => {
                self.Close();
            };

            self.onSave = () => {

                if (!self.Validate())
                    return;

                Global.Helpers.ShowExecuting();

                Dns.WebApi.Comments.AddWorkflowComment({
                    RequestID: self.RequestID,
                    WorkflowActivityID: self.WorkflowActivityID,
                    Comment: self.Comment()
                })
                    .done((result: Dns.Interfaces.IWFCommentDTO[]) => {

                        result.forEach(r => {
                            r.CreatedOn = moment.utc(r.CreatedOn).toDate();
                        });

                        self.Close(result[0]);
                    })
                    .always(() => { Global.Helpers.HideExecuting(); });
            };

        }
    }

    export function init() {
        $(() => {
            var bindingControl = $('#Content');
            vm = new ViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);
        });
    }

    init();

}
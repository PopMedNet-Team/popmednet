/// <reference path="../_rootlayout.ts" />

namespace Users.EmailTemplates {
    interface EmailTemplateItem {
        TemplateID: any;
        Name: string;
        Subject: string;
    }

    export class SendEmailViewModel extends Global.DialogViewModel {
        public TemplateID: KnockoutObservable<any>;
        public TemplateSubject: KnockoutObservable<string>;

        constructor(bindingControl: JQuery) {
            super(bindingControl);            

            let self = this;
            this.TemplateID = ko.observable("");
            this.TemplateSubject = ko.observable("");
            
            this.TemplateID.subscribe((val) => {
                if (val == "") {
                    this.TemplateSubject("");
                    $('#template-container').load('/users/RenderEmailPreview?userID=' + self.Parameters.userID);
                } else {
                    this.TemplateSubject($("#cboTemplates").data("kendoDropDownList").dataItem().Subject);
                    $('#template-container').load('/users/RenderEmailPreview?templateType='+ val +'&userID=' + self.Parameters.userID);
                }                
            });
        }

        public Save() {
            let self = this;
            Global.Helpers.ShowExecuting();

            $.ajax({
                url: "/users/SendEmail",
                method: "POST",
                data: { userID: this.Parameters.userID, templateType: parseInt(self.TemplateID()) }
            }).done(function () {
                self.Close();
            }).fail(function (xhr, b, c) {
                Global.Helpers.ShowErrorAlert(xhr.statusText, xhr.responseText, 480);
            })
            .always(function () {
                Global.Helpers.HideExecuting();
            });
            
        }

        public Cancel() {
            this.Close();
        }
    }

    function init() {
        $(() => {
            var bindingControl = $("#Content");
            let vm = new SendEmailViewModel(bindingControl);
            ko.applyBindings(vm, bindingControl[0]);            
        });
    }

    init();

}

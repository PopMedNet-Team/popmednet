/// <reference path="../../Scripts/typings/bootstrap.dns.d.ts" />

module Responses.Detail{
    export var InitialGroupName: string = '';
    var vm: ViewModel;
    var vmHeader: HeaderViewModel = null;

    export class HeaderViewModel {

        public FieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[];

        public IsFieldVisible: (id: string) => boolean;
        public IsFieldRequired: (id: string) => boolean;
        public PurposeOfUse_Display: (modelPurposeOfUse: string) => string;
        public PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });


        constructor(fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[]) {
            var self = this;

            this.FieldOptions = fieldOptions;

            self.IsFieldRequired = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
            };

            self.IsFieldVisible = (id: string) => {
                var options = ko.utils.arrayFirst(self.FieldOptions,(item) => { return item.FieldIdentifier == id; });
                return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
            };

            self.PurposeOfUse_Display = (modelPurposeOfUse: string) => {
                if (modelPurposeOfUse == null) {
                    return '';
                }
                var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions,(a) => a.Value == modelPurposeOfUse);
                if (pou) {
                    return pou.Name;
                } else { return ''; }
            };   
        }
    }


    export class ViewModel {
        public GroupName: KnockoutObservable<string>;
        public RejectMessage: KnockoutObservable<string>;
        public ShowGroupNamePrompt: KnockoutObservable<boolean>;
        public ShowMessagePrompt: KnockoutObservable<boolean>;
        public SubmitAction: KnockoutObservable<string>;

        constructor() {
            var self = this;

            this.GroupName = ko.observable(Responses.Detail.InitialGroupName || '');
            this.RejectMessage = ko.observable('');
            this.ShowGroupNamePrompt = ko.observable(false);
            this.ShowMessagePrompt = ko.observable(false);
            this.SubmitAction = ko.observable('');
     
        }

        public onPromptForGroup() {
            this.SubmitAction('Group');
            this.promptForGroupName();
        }

        public onPromptForGroupAndApprove() {
            this.SubmitAction('GroupAndApprove');
            this.promptForGroupName();
        }

        private promptForGroupName() {
            Global.Helpers.ShowPrompt('', 'Please enter a group name.').done((value: any) => {
                this.GroupName(value == null || value == undefined ? '' : value);

                if (this.GroupName() == '') {
                    Global.Helpers.ShowAlert('Validation Error', '<p class="bg-danger">A group name is required.</p>', 400, ['Close']).done(() => {
                        return;
                    });
                } else {
                    this.SubmitForm();
                }

            });
        }

        public onApprove() {
            this.SubmitAction('Approve');
            this.SubmitForm();
        }

        public onPromptForRejectMessage() {
            this.SubmitAction('Reject');

            Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done((value: any) => {
                this.RejectMessage(value == null || value == undefined ? '' : value);
                this.SubmitForm();
            });
            
        }

        public onUngroup() {
            this.SubmitAction('Ungroup');
            this.SubmitForm();
        }

        public SubmitForm() {
            if (this.SubmitAction() == '')
                return;

            Global.Helpers.ShowExecuting();
            $('#frmActions').submit();
        }       

    }
    export function init(projectID: any) {
        $.when<any>(
            Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID)
            ).done((fieldOptions) => {
            $(() => {  

                var bindingControl = $('#ResponseActions');
                vm = new ViewModel();
                ko.applyBindings(vm, bindingControl[0]);

                var headerBindingControl = $("#frmDetails");
                vmHeader = new HeaderViewModel(fieldOptions);
                ko.applyBindings(vmHeader, headerBindingControl[0]);
            });
        });
    }

} 
/// <reference path="../../../Lpp.Pmn.Resources/Scripts/typings/bootstrap.dns.d.ts" />
var Responses;
(function (Responses) {
    var Detail;
    (function (Detail) {
        Detail.InitialGroupName = '';
        var vm;
        var vmHeader = null;
        var HeaderViewModel = /** @class */ (function () {
            function HeaderViewModel(fieldOptions) {
                this.PurposeOfUseOptions = new Array({ Name: 'Clinical Trial Research', Value: 'CLINTRCH' }, { Name: 'Healthcare Payment', Value: 'HPAYMT' }, { Name: 'Healthcare Operations', Value: 'HOPERAT' }, { Name: 'Healthcare Research', Value: 'HRESCH' }, { Name: 'Healthcare Marketing', Value: 'HMARKT' }, { Name: 'Observational Research', Value: 'OBSRCH' }, { Name: 'Patient Requested', Value: 'PATRQT' }, { Name: 'Public Health', Value: 'PUBHLTH' }, { Name: 'Prep-to-Research', Value: 'PTR' }, { Name: 'Quality Assurance', Value: 'QA' }, { Name: 'Treatment', Value: 'TREAT' });
                var self = this;
                this.FieldOptions = fieldOptions;
                self.IsFieldRequired = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission == Dns.Enums.FieldOptionPermissions.Required;
                };
                self.IsFieldVisible = function (id) {
                    var options = ko.utils.arrayFirst(self.FieldOptions, function (item) { return item.FieldIdentifier == id; });
                    return options.Permission != Dns.Enums.FieldOptionPermissions.Hidden;
                };
                self.PurposeOfUse_Display = function (modelPurposeOfUse) {
                    if (modelPurposeOfUse == null) {
                        return '';
                    }
                    var pou = ko.utils.arrayFirst(self.PurposeOfUseOptions, function (a) { return a.Value == modelPurposeOfUse; });
                    if (pou) {
                        return pou.Name;
                    }
                    else {
                        return '';
                    }
                };
            }
            return HeaderViewModel;
        }());
        Detail.HeaderViewModel = HeaderViewModel;
        var ViewModel = /** @class */ (function () {
            function ViewModel() {
                var self = this;
                this.GroupName = ko.observable(Responses.Detail.InitialGroupName || '');
                this.RejectMessage = ko.observable('');
                this.ShowGroupNamePrompt = ko.observable(false);
                this.ShowMessagePrompt = ko.observable(false);
                this.SubmitAction = ko.observable('');
            }
            ViewModel.prototype.onPromptForGroup = function () {
                this.SubmitAction('Group');
                this.promptForGroupName();
            };
            ViewModel.prototype.onPromptForGroupAndApprove = function () {
                this.SubmitAction('GroupAndApprove');
                this.promptForGroupName();
            };
            ViewModel.prototype.promptForGroupName = function () {
                var _this = this;
                Global.Helpers.ShowPrompt('', 'Please enter a group name.').done(function (value) {
                    _this.GroupName(value == null || value == undefined ? '' : value);
                    if (_this.GroupName() == '') {
                        Global.Helpers.ShowAlert('Validation Error', '<p class="bg-danger">A group name is required.</p>', 400, ['Close']).done(function () {
                            return;
                        });
                    }
                    else {
                        _this.SubmitForm();
                    }
                });
            };
            ViewModel.prototype.onApprove = function () {
                this.SubmitAction('Approve');
                this.SubmitForm();
            };
            ViewModel.prototype.onPromptForRejectMessage = function () {
                var _this = this;
                this.SubmitAction('Reject');
                Global.Helpers.ShowPrompt('', 'Please enter a rejection message.').done(function (value) {
                    _this.RejectMessage(value == null || value == undefined ? '' : value);
                    _this.SubmitForm();
                });
            };
            ViewModel.prototype.onUngroup = function () {
                this.SubmitAction('Ungroup');
                this.SubmitForm();
            };
            ViewModel.prototype.SubmitForm = function () {
                if (this.SubmitAction() == '')
                    return;
                Global.Helpers.ShowExecuting();
                $('#frmActions').submit();
            };
            return ViewModel;
        }());
        Detail.ViewModel = ViewModel;
        function init(projectID) {
            $.when(Dns.WebApi.Projects.GetFieldOptions(projectID, User.ID)).done(function (fieldOptions) {
                $(function () {
                    var bindingControl = $('#ResponseActions');
                    vm = new ViewModel();
                    ko.applyBindings(vm, bindingControl[0]);
                    var headerBindingControl = $("#frmDetails");
                    vmHeader = new HeaderViewModel(fieldOptions);
                    ko.applyBindings(vmHeader, headerBindingControl[0]);
                });
            });
        }
        Detail.init = init;
    })(Detail = Responses.Detail || (Responses.Detail = {}));
})(Responses || (Responses = {}));
//# sourceMappingURL=Detail.js.map
/// <reference path="../../../js/_rootlayout.ts" />
module Plugins.Requests.QueryBuilder.SaveCriteriaGroup {
    var vm: ViewModel;

    export class ViewModel extends Global.DialogViewModel {
        public Template: Dns.ViewModels.TemplateViewModel;
        private options: Options;

        constructor(bindingControl: JQuery) {
            super(bindingControl);

            this.options = <Options>this.Parameters;

            this.Template = new Dns.ViewModels.TemplateViewModel();
            this.Template.Data(JSON.stringify(this.options.CriteriaGroup));
            this.Template.CreatedByID(User.ID);
            this.Template.CreatedOn(moment.utc().toDate());
            this.Template.Type(Dns.Enums.TemplateTypes.CriteriaGroup);
            this.Template.QueryType(this.options.AdapterDetail);         
        }

        public Save() {
            if (!this.Validate())
                return;               

            var updateDetails: Dns.Interfaces.ISaveCriteriaGroupRequestDTO = {
                Name: this.Template.Name(),
                Description: this.Template.Description(),
                Json: JSON.stringify(this.options.CriteriaGroup),
                AdapterDetail: this.options.AdapterDetail,
                TemplateID: this.options.TemplateID,
                RequestTypeID: this.options.RequestTypeID,
                RequestID: this.options.RequestID
            };

            Dns.WebApi.Templates.SaveCriteriaGroup(updateDetails).done((results) => {
                this.Close();
            });
        }
    }

    function init() {
        var bindingControl = $("#Content");
        vm = new ViewModel(bindingControl);

        ko.applyBindings(vm, bindingControl[0]);
    }

    init();

    interface Options {
        CriteriaGroup: Dns.Interfaces.IQueryComposerCriteriaDTO,
        AdapterDetail: Dns.Enums.QueryComposerQueryTypes,
        TemplateID: any,
        RequestTypeID: any,
        RequestID: any
    }
}
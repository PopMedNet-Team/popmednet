/// <reference path="../../../js/_rootlayout.ts" />
/// <reference path="termvaluefilter.ts" />
/// <reference path="../../../js/requests/details.ts" />

//ko.components.register('MDQ', {
//    viewModel: { require: Plugins.Requests.QueryBuilder.MDQ.vm },
//        template: { require: '<span>...</span>' }
//});

module Plugins.Requests.QueryBuilder.Edit {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        fileUpload: KnockoutObservable<boolean>;
        MDQ: KnockoutObservable<boolean> = ko.observable<boolean>();
        IsTemplateEdit: KnockoutObservable<boolean>;
        UploadViewModel: Controls.WFFileUpload.Index.ViewModel;
        rawRequestData: Dns.Interfaces.IQueryComposerRequestDTO;
        projectID: any;

        
        public UpdateRoutings(updates) {
            Plugins.Requests.QueryBuilder.DataMartRouting.vm.UpdateRoutings(updates);
        }

        constructor(
            bindingControl: JQuery,
            rawRequestData: Dns.Interfaces.IQueryComposerRequestDTO,
            projectID: any,
            requestID?: any
        ) {
            super(bindingControl);
            var self = this;
            this.IsTemplateEdit = ko.observable<boolean>(false);
            this.fileUpload = ko.observable<boolean>(false);
            this.MDQ = ko.observable<boolean>(false);
            this.rawRequestData = rawRequestData;
            this.projectID = projectID;
        }

        public fileUploadDMLoad() {
            Plugins.Requests.QueryBuilder.DataMartRouting.vm.LoadDataMarts(this.projectID, JSON.stringify((new Dns.ViewModels.QueryComposerRequestViewModel(this.rawRequestData)).toData()));
        }


    }


    export function init(
        rawRequestData: Dns.Interfaces.IQueryComposerRequestDTO,
        fieldOptions: Dns.Interfaces.IBaseFieldOptionAclDTO[],
        defaultPriority: Dns.Enums.Priorities,
        defaultDueDate: Date,
        additionalInstructions: string,
        existingRequestDataMarts: Dns.Interfaces.IRequestDataMartDTO[],
        requestTypeID: any,
        visualTerms: IVisualTerm[],
        isTemplateEdit: boolean = false,
        projectID: any = Global.GetQueryParam("projectID"),
        templateID?: any,
        requestID?: any
    ): JQueryPromise<Plugins.Requests.QueryBuilder.MDQ.ViewModel> {
        var promise = $.Deferred<Plugins.Requests.QueryBuilder.MDQ.ViewModel>();
        $.when<any>(
            requestTypeID == null ? null : Dns.WebApi.Templates.GetByRequestType(requestTypeID)
        ).done((
            requestTypeTemplates: Dns.Interfaces.ITemplateDTO[]
        ) => {

            var bindingControl = $('#ComposerControl');
            vm = new ViewModel(bindingControl, rawRequestData, projectID, requestID);
            ko.applyBindings(vm, bindingControl[0])
            Plugins.Requests.QueryBuilder.DataMartRouting.init($('#DataMartsControl'), fieldOptions, existingRequestDataMarts, defaultDueDate, defaultPriority, additionalInstructions);


            if (isTemplateEdit) {
                vm.MDQ(true);
                vm.fileUpload(false);
                Plugins.Requests.QueryBuilder.MDQ.init(
                    rawRequestData,
                    fieldOptions,
                    defaultPriority,
                    defaultDueDate,
                    additionalInstructions,
                    existingRequestDataMarts,
                    requestTypeID,
                    visualTerms,
                    isTemplateEdit,
                    projectID,
                    templateID
                ).done((viewModel) => {
                    promise.resolve(viewModel);
                });
            } else {
                if (requestTypeTemplates != null && requestTypeTemplates.length > 0) {
                    if (requestTypeTemplates[0].ComposerInterface == Dns.Enums.QueryComposerInterface.FileDistribution) {
                        vm.MDQ(false);   
                        vm.fileUpload(true);
                        var fileUploadID = "2F60504D-9B2F-4DB1-A961-6390117D3CAC";
                        vm.UploadViewModel = Controls.WFFileUpload.Index.init($('#FileUploadControl'), rawRequestData, fileUploadID);
                        //vm.UploadViewModel = Controls.WFFileUpload.ForTask.init($("#FileUploadControl"), tasks);
                        vm.fileUploadDMLoad();
                        promise.resolve();

                        
                    } else {
                        vm.MDQ(true);
                        Plugins.Requests.QueryBuilder.MDQ.init(
                            rawRequestData,
                            fieldOptions,
                            defaultPriority,
                            defaultDueDate,
                            additionalInstructions,
                            existingRequestDataMarts,
                            requestTypeID,
                            visualTerms,
                            isTemplateEdit,
                            projectID,
                            templateID
                        ).done((viewModel) => {
                            promise.resolve(viewModel);
                        });
                    }
                }
                
            }

            vm.IsTemplateEdit(isTemplateEdit);              
            
        });

        return promise;
    }


}



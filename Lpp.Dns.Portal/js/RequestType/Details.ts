/// <reference path="../_rootlayout.ts" />
module RequestType.Details {
    export var vm: ViewModel;

    export class ViewModel extends Global.PageViewModel {
        public RequestType: Dns.ViewModels.RequestTypeViewModel;
        public Template: Dns.ViewModels.TemplateViewModel;
        public SelectedModels: KnockoutObservableArray<any>;

        public RequestTypeAcls: KnockoutObservableArray<Dns.ViewModels.AclRequestTypeViewModel>;
        public RequestTypeSecurity: Security.Acl.AclEditViewModel<Dns.ViewModels.AclRequestTypeViewModel>;

        public Workflows: Dns.Interfaces.IWorkflowDTO[];
        public Templates: Dns.Interfaces.ITemplateDTO[];

        public RequestTypeTerms: KnockoutObservableArray<Dns.ViewModels.RequestTypeTermViewModel>;
        public TermList: Dns.Interfaces.ITermDTO[];
        public AddableTerms: KnockoutComputed<Dns.Interfaces.ITermDTO[]>;

        public Save: () => void;
        public Delete: () => void;
        public DeleteTerm: (requestTypeTerm: Dns.ViewModels.RequestTypeTermViewModel) => void;
        public AddRequestTypeTerm: (term: Dns.Interfaces.ITermDTO) => void;
        
        constructor(requestType: Dns.Interfaces.IRequestTypeDTO,
                    requestTypeModels: Dns.Interfaces.IRequestTypeModelDTO[],
                    requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
                    bindingControl: JQuery, screenPermissions: any[],
                    permissionList: Dns.Interfaces.IPermissionDTO[],
                    requestTypePermissions: Dns.Interfaces.IAclRequestTypeDTO[],
                    securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
                    workflows: Dns.Interfaces.IWorkflowDTO[],
                    templates: Dns.Interfaces.ITemplateDTO[],
                    termList: Dns.Interfaces.ITermDTO[],
                    template: Dns.Interfaces.ITemplateDTO
            )
        {
            super(bindingControl, screenPermissions);

            var self = this;

            self.Workflows = workflows;
            self.Templates = templates;

            self.RequestType = new Dns.ViewModels.RequestTypeViewModel(requestType);
            self.Template = new Dns.ViewModels.TemplateViewModel(template);

            //set default template
            if (self.Template.ComposerInterface() == null)
                self.Template.ComposerInterface(Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery);          

            self.SelectedModels = ko.observableArray(ko.utils.arrayFilter(requestTypeModels, (rtm) => {
                var modelID = rtm.DataModelID.toLowerCase();
                return modelID == '321adaa1-a350-4dd0-93de-5de658a507df' || //Data Characterization
                    modelID == '7c69584a-5602-4fc0-9f3f-a27f329b1113' || //ESP
                    modelID == '85ee982e-f017-4bc4-9acd-ee6ee55d2446' || //PCORnet
                    modelID == 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb' || //Summary Tables
                    modelID == '4c8a25dc-6816-4202-88f4-6d17e72a43bc' ||//Distributed Regression
                    modelID == '1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154'; //Modular Program
                
                }).map((item) => {
                    return item.DataModelID.toLowerCase();
            }));
            
            self.RequestTypeTerms = ko.observableArray(requestTypeTerms.map((item) => {
                return new Dns.ViewModels.RequestTypeTermViewModel(item);
            }));
            self.TermList = termList;

            self.AddableTerms = ko.computed<Dns.Interfaces.ITermDTO[]>(() => {
                var results = self.TermList.filter((t) => {
                    var exists = false;
                    self.RequestTypeTerms().forEach((rtt) => {
                        if (rtt.TermID() == t.ID) {
                            exists = true;
                            return;
                        }
                    });

                    return !exists;
                });

                return results.sort(function (left, right) { return left.Name == right.Name ? 0 : (left.Name < right.Name ? -1 : 1) });
            });

            self.RequestTypeAcls = ko.observableArray(requestTypePermissions.map((item) => {
                return new Dns.ViewModels.AclRequestTypeViewModel(item);
            }));

            self.RequestTypeSecurity = new Security.Acl.AclEditViewModel(permissionList, securityGroupTree, self.RequestTypeAcls, [
                {
                    Field: "RequestTypeID",
                    Value: self.RequestType.ID()
                }
            ], Dns.ViewModels.AclRequestTypeViewModel);

            self.Template.QueryType.subscribe((value) => {
                if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                    Plugins.Requests.QueryBuilder.MDQ.vm.Request.Header.QueryType(value);

                    Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(self.SelectedModels(), value, self.RequestTypeTerms().map((t) => t.TermID()));
                }
            });

            self.SelectedModels.subscribe((values) => {                
                if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                    Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(values, self.Template.QueryType(), self.RequestTypeTerms().map((t) => t.TermID()));
                }
            });

            self.RequestTypeTerms.subscribe((values) => {
                if (self.Template.Type() == Dns.Enums.TemplateTypes.Request) {
                    Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(self.SelectedModels(), self.Template.QueryType(), values.map((t) => t.TermID()));
                }
            });


            self.WatchTitle(this.RequestType.Name, "Request Type: ");   
            
            self.Save = () => {
                if (self.RequestType.WorkflowID() == null || self.RequestType.WorkflowID() == "") {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have a Workflow Selected.</p>");
                    return;
                }

                if (!super.Validate())
                    return;

                if (!Plugins.Requests.QueryBuilder.MDQ.vm.AreTermsValid())
                    return;

                if (self.RequestTypeAcls().length == 0) {
                    Global.Helpers.ShowAlert("Validation Error", "<p>Please ensure that you have added at least one security group to the Permissions tab to be able to administer this request type.</p>");
                    return;
                }

                self.Template.Data(JSON.stringify(Plugins.Requests.QueryBuilder.MDQ.vm.Request.toData()));
                var update = new Dns.ViewModels.UpdateRequestTypeRequestViewModel().toData();
                update.RequestType = self.RequestType.toData();
                update.Template = self.Template.toData();
                update.Models = self.SelectedModels();
                update.Terms = self.RequestTypeTerms().map((t) => t.TermID());
                update.NotAllowedTerms = Plugins.Requests.QueryBuilder.MDQ.vm.NotAllowedTerms();
                Dns.WebApi.RequestTypes.Save(update).done((results: Dns.Interfaces.IUpdateRequestTypeResponseDTO[]) => {
                    var result = results[0];
                    self.RequestType.ID(result.RequestType.ID);
                    self.RequestType.Timestamp(result.RequestType.Timestamp);
                    self.RequestType.TemplateID(result.Template.ID);

                    window.history.replaceState(null, window.document.title, "/requesttype/details?ID=" + self.RequestType.ID());

                    self.Template.ID(result.Template.ID);
                    self.Template.Timestamp(result.Template.Timestamp);
                    self.Template.CreatedByID(result.Template.CreatedByID);

                    var requestTypeAcls = self.RequestTypeAcls().map((a) => {
                        a.RequestTypeID(self.RequestType.ID());
                        return a.toData();
                    });

                    Dns.WebApi.Security.UpdateRequestTypePermissions(requestTypeAcls).done(() => {
                        Global.Helpers.ShowAlert("Save", "<p>Save completed successfully!</p>");
                    });
                });
            };  
            
            self.Delete = () => {
                Global.Helpers.ShowConfirm("Delete Confirmation", "<p>Are you sure you wish to delete this Request Type?</p>").done(() => {
                    Dns.WebApi.RequestTypes.Delete([self.RequestType.ID()]).done(() => {
                        window.location.href = "/requesttype";
                    });
                });
            };
            
            self.DeleteTerm = (requestTypeTerm: Dns.ViewModels.RequestTypeTermViewModel) => {
                self.RequestTypeTerms.remove(requestTypeTerm);
            };      

            self.AddRequestTypeTerm = (term: Dns.Interfaces.ITermDTO) => {
                self.RequestTypeTerms.push(new Dns.ViewModels.RequestTypeTermViewModel({
                    Description: term.Description,
                    OID: term.OID,
                    ReferenceUrl: term.ReferenceUrl,
                    RequestTypeID: self.RequestType.ID(),
                    Term: term.Name,
                    TermID: term.ID
                }));
            };
        }        

        public Cancel() {
            window.location.href = "/requesttype";
        }
    }

    function GetVisualTerms(): JQueryDeferred<IVisualTerm[]> {
        var d = $.Deferred<IVisualTerm[]>();

        $.ajax({ type: "GET", url: '/QueryComposer/VisualTerms', dataType: "json" })
            .done((result: IVisualTerm[]) => {
                d.resolve(result);
            }).fail((e, description, error) => {
                d.reject(<any>e);
            });

        return d;
    } 

    function init() {
        var id: any = $.url().param("ID");
        $.when<any>(
            id == null ? null : Dns.WebApi.RequestTypes.Get(id),
            id == null ? [] : Dns.WebApi.RequestTypes.GetRequestTypeModels(id),
            id == null ? null : Dns.WebApi.RequestTypes.GetRequestTypeTerms(id),
            id == null ? null : Dns.WebApi.RequestTypes.GetPermissions([id], [PMNPermissions.RequestTypes.Delete, PMNPermissions.RequestTypes.Edit, PMNPermissions.RequestTypes.ManageSecurity]),
            Dns.WebApi.Security.GetPermissionsByLocation([Dns.Enums.PermissionAclTypes.RequestTypes]),
            Dns.WebApi.Security.GetRequestTypePermissions(id ? id : Constants.GuidEmpty),
            Dns.WebApi.Security.GetAvailableSecurityGroupTree(),
            Dns.WebApi.Workflow.List(null, "ID,Name", "Name"),
            Dns.WebApi.Templates.List("Type eq Lpp.Dns.DTO.Enums.TemplateTypes'" + Dns.Enums.TemplateTypes.Request + "'", "ID,Name", "Name"),
            Dns.WebApi.Terms.List(),
            GetVisualTerms()
        ).done((
            requestTypes: Dns.Interfaces.IRequestTypeDTO[],
            requestTypeModels: Dns.Interfaces.IRequestTypeModelDTO[],
            requestTypeTerms: Dns.Interfaces.IRequestTypeTermDTO[],
            screenPermissions,
            permissionList,
            requestTypePermissions: Dns.Interfaces.IAclRequestTypeDTO[],
            securityGroupTree: Dns.Interfaces.ITreeItemDTO[],
            workflows: Dns.Interfaces.IWorkflowDTO[],
            templates: Dns.Interfaces.ITemplateDTO[],
            termList: Dns.Interfaces.ITermDTO[],
            visualTerms: IVisualTerm[]          
        ) => {
                var requestType = (requestTypes == null || requestTypes.length == 0) ? new Dns.ViewModels.RequestTypeViewModel().toData() : requestTypes[0];

                $.when<Dns.Interfaces.ITemplateDTO[]>(
                    requestType.TemplateID == null ? null : Dns.WebApi.Templates.Get(requestType.TemplateID)
                ).done((templates) => {

                    var template: Dns.Interfaces.ITemplateDTO;
                    if (templates == null || templates.length == 0) {
                        template = new Dns.ViewModels.TemplateViewModel().toData();
                        template.Type = Dns.Enums.TemplateTypes.Request;
                        template.ComposerInterface = Dns.Enums.QueryComposerInterface.FlexibleMenuDrivenQuery;
                        template.QueryType = null;
                    } else {
                        template = templates[0];
                    }

                    var json: any = JSON.parse(((template.Data || '').trim() != '') ? template.Data : '{"Header":{},"Where":{"Criteria":[{"Name":"Group 1","Criteria":[],"Terms":[],"ObservationPeriod":{}}]}}');

                     
                    $(() => {
                        var bindingControl = $('#Content');
                        vm = new ViewModel(
                            requestType,
                            requestTypeModels || [],
                            requestTypeTerms || [],
                            bindingControl,
                            screenPermissions || [PMNPermissions.RequestTypes.Delete,
                            PMNPermissions.RequestTypes.Edit,
                            PMNPermissions.RequestTypes.ManageSecurity],
                            permissionList || [],
                            requestTypePermissions || [],
                            securityGroupTree || [],
                            workflows,
                            templates,
                            termList,
                            template
                        );


                        Plugins.Requests.QueryBuilder.Edit.init(json, [], null, null, '', [], null, visualTerms, true, null, requestType.TemplateID, null).done(() => {
                            Plugins.Requests.QueryBuilder.MDQ.vm.UpdateTermList(vm.SelectedModels(), vm.Template.QueryType(), vm.RequestTypeTerms().map((t) => t.TermID()));
                        });  
                        
                        $('#tabs').kendoTabStrip().data('kendoTabStrip').bind('show', (e) => {
                            if ($(e.contentElement).has('#txtNotes')) {
                                //to make the kendo editor initialize correctly it needs to be refreshed when the tab is show
                                var editor = $('#txtNotes').data('kendoEditor');
                                editor.refresh();
                            }
                        });
                        ko.applyBindings(vm, bindingControl[0]);

                        
                    });

                });
                
            });
    }

    init();
}
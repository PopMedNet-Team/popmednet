var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Plugins;
(function (Plugins) {
    var Requests;
    (function (Requests) {
        var QueryBuilder;
        (function (QueryBuilder) {
            var Edit;
            (function (Edit) {
                var ViewModel = (function (_super) {
                    __extends(ViewModel, _super);
                    function ViewModel(bindingControl, requestDTO, projectID, requestID, requestTypeDTO, requestTypeModelIDs, requestTypeTerms, visualTerms, criteriaGroupTemplates, hiddenTerms, routingsEditor, attachmentsEditor) {
                        var _this = _super.call(this, bindingControl) || this;
                        _this.IsFileUpload = false;
                        _this.MDQ_Host = null;
                        _this.FileUpload_Host = null;
                        var self = _this;
                        _this.ProjectID = projectID;
                        _this.RequestID = requestID;
                        _this.RequestDTO = requestDTO;
                        _this.RequestTypeDTO = requestTypeDTO;
                        _this.RoutingsEditor = routingsEditor;
                        _this.AttachmentsEditor = attachmentsEditor;
                        if (_this.RequestDTO.SchemaVersion !== "2.0") {
                            _this.RequestDTO.SchemaVersion = "2.0";
                        }
                        _this.TermsObserver = new Plugins.Requests.QueryBuilder.TermsObserver();
                        if (_this.RequestDTO.Queries != null && _this.RequestDTO.Queries.length > 0 &&
                            (_this.RequestDTO.Queries[0].Header.ComposerInterface == Dns.Enums.QueryComposerInterface.FileDistribution ||
                                Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.QueryContainsTerm(_this.RequestDTO.Queries[0], Plugins.Requests.QueryBuilder.MDQ.Terms.FileUploadID) ||
                                Plugins.Requests.QueryBuilder.MDQ.TermValueFilter.QueryContainsTerm(_this.RequestDTO.Queries[0], Plugins.Requests.QueryBuilder.MDQ.Terms.ModularProgramID))) {
                            if (_this.RequestDTO.Queries[0].Header.ComposerInterface != Dns.Enums.QueryComposerInterface.FileDistribution) {
                                _this.RequestDTO.Queries[0].Header.ComposerInterface = Dns.Enums.QueryComposerInterface.FileDistribution;
                            }
                            _this.IsFileUpload = true;
                            _this.FileUpload_Host = new Controls.WFFileUpload.Index.ViewModel($('#FileUploadControl'), _this.ScreenPermissions, _this.RequestDTO.Queries[0], QueryBuilder.MDQ.Terms.FileUploadID);
                            var currentTerms = Plugins.Requests.QueryBuilder.MDQ.TermProvider.FlattenToAllTermIDs(_this.RequestDTO.Queries[0]);
                            _this.TermsObserver.RegisterTermCollection(_this.RequestDTO.Queries[0].Header.ID, ko.observableArray(currentTerms));
                        }
                        else {
                            var index_1 = 0;
                            var templateViewModels = ko.utils.arrayMap(_this.RequestDTO.Queries, function (query) {
                                var template = new Dns.ViewModels.TemplateViewModel();
                                template.ID(query.Header.ID);
                                template.ComposerInterface(query.Header.ComposerInterface);
                                template.Data(JSON.stringify(query));
                                template.Description(query.Header.Description);
                                template.Name(query.Header.Name);
                                template.Order(index_1++);
                                template.QueryType(query.Header.QueryType);
                                template.RequestTypeID(requestTypeDTO.ID);
                                template.Type(Dns.Enums.TemplateTypes.Request);
                                return template;
                            });
                            var initParams = {
                                IsTemplateEdit: false,
                                RequestTypeModelIDs: ko.observableArray(requestTypeModelIDs),
                                RequestTypeTerms: ko.observableArray(requestTypeTerms.map(function (item) {
                                    return new Dns.ViewModels.RequestTypeTermViewModel(item);
                                })),
                                Templates: templateViewModels,
                                TemplateType: Dns.Enums.TemplateTypes.Request,
                                CriteriaGroupTemplates: criteriaGroupTemplates,
                                VisualTerms: visualTerms,
                                HiddenTerms: hiddenTerms,
                                SupportsMultiQuery: ko.observable(_this.RequestTypeDTO.SupportMultiQuery),
                                TermsObserver: _this.TermsObserver
                            };
                            _this.MDQ_Host = new Plugins.Requests.QueryBuilder.QueryEditorHost(initParams);
                        }
                        _this.TermsObserver.DistinctTerms.subscribe(function (newValue) {
                            _this.RoutingsEditor.LoadDataMarts(_this.ProjectID, newValue);
                        }, _this);
                        _this.RoutingsEditor.LoadDataMarts(_this.ProjectID, _this.TermsObserver.DistinctTerms());
                        return _this;
                    }
                    Object.defineProperty(ViewModel.prototype, "SelectedRoutings", {
                        get: function () {
                            return this.RoutingsEditor.SelectedRoutings();
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ViewModel.prototype, "AdditionalInstructions", {
                        get: function () {
                            return this.RoutingsEditor.DataMartAdditionalInstructions();
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ViewModel.prototype.Validate = function () {
                        if (this.MDQ_Host != null) {
                            var emptyCriteriaGroupQuery = ko.utils.arrayFirst(this.MDQ_Host.ExportQueries(), function (query) {
                                if (ko.utils.arrayFirst(query.Where.Criteria, function (criteria) { return (criteria.Terms == null || criteria.Terms.length == 0) && (criteria.Criteria == null || criteria.Criteria.length == 0); }) != null) {
                                    return true;
                                }
                                return false;
                            });
                            if (emptyCriteriaGroupQuery != null) {
                                Global.Helpers.ShowAlert('Validation Error', '<div class="alert alert-warning" style="text-align:center;line-height:2em;"><p>The Criteria Group cannot be empty.</p></div>');
                                return false;
                            }
                            if (!this.MDQ_Host.onValidateEditors()) {
                                return false;
                            }
                            _super.prototype.Validate.call(this);
                        }
                        return true;
                    };
                    ViewModel.prototype.VerifyNoDuplicates = function () {
                        if (this.MDQ_Host != null) {
                            return this.MDQ_Host.VerifyNoDuplicates();
                        }
                        return true;
                    };
                    ViewModel.prototype.fileUploadDMLoad = function () {
                        this.RoutingsEditor.LoadDataMarts(this.ProjectID, this.TermsObserver.DistinctTerms());
                    };
                    ViewModel.prototype.UpdateRoutings = function (updates) {
                        this.RoutingsEditor.UpdateRoutings(updates);
                    };
                    ViewModel.prototype.bindComponents = function () {
                        if (this.MDQ_Host != null) {
                            this.MDQ_Host.onKnockoutBind();
                        }
                        else {
                            this.FileUpload_Host.onKnockoutBind();
                        }
                    };
                    ViewModel.prototype.exportRequestDTO = function () {
                        var requestDTO = JSON.parse(JSON.stringify(this.RequestDTO));
                        requestDTO.Queries = this.MDQ_Host == null ? this.FileUpload_Host.ExportQueries() : this.MDQ_Host.ExportQueries();
                        return requestDTO;
                    };
                    ViewModel.prototype.updateRequestHeader = function (name, viewUrl, submittedOn) {
                        this.RequestDTO.Header.Name = name;
                        this.RequestDTO.Header.ViewUrl = viewUrl;
                        this.RequestDTO.Header.SubmittedOn = submittedOn;
                    };
                    return ViewModel;
                }(Global.PageViewModel));
                Edit.ViewModel = ViewModel;
                function init(requestDTO, fieldOptions, defaultPriority, defaultDueDate, additionalInstructions, existingRequestDataMarts, requestTypeDTO, projectID, requestID) {
                    if (projectID === void 0) { projectID = Global.GetQueryParam("projectID"); }
                    var promise = $.Deferred();
                    $.when(Dns.WebApi.RequestTypes.GetRequestTypeModels(requestTypeDTO.ID, null, 'DataModelID'), Dns.WebApi.RequestTypes.GetRequestTypeTerms(requestTypeDTO.ID), Plugins.Requests.QueryBuilder.MDQ.TermProvider.GetVisualTerms(), Dns.WebApi.Templates.CriteriaGroups(), Dns.WebApi.Templates.ListHiddenTermsByRequestType(requestTypeDTO.ID), Controls.WFFileUpload.ForAttachments.init($('#attachments_upload'), true)).done(function (requestTypeModelIDs, requestTypeTerms, visualTerms, criteriaGroupTemplates, hiddenTerms, attachmentsVM) {
                        $(function () {
                            var routingsVM = Plugins.Requests.QueryBuilder.DataMartRouting.init(fieldOptions, existingRequestDataMarts, defaultDueDate, defaultPriority, additionalInstructions);
                            var bindingControl = $('#ComposerControl');
                            Edit.vm = new ViewModel(bindingControl, requestDTO, projectID, requestID, requestTypeDTO, (requestTypeModelIDs || []).map(function (m) { return m.DataModelID; }), requestTypeTerms, visualTerms, criteriaGroupTemplates, hiddenTerms, routingsVM, attachmentsVM);
                            if (Edit.vm.IsFileUpload == false) {
                                $('#FileUploadContainer').remove();
                            }
                            else {
                                $('#QueryEditorHostContainer').remove();
                            }
                            ko.applyBindings(Edit.vm, bindingControl[0]);
                            Edit.vm.bindComponents();
                            routingsVM.onKnockoutBind();
                            promise.resolve(Edit.vm);
                        });
                    });
                    return promise;
                }
                Edit.init = init;
            })(Edit = QueryBuilder.Edit || (QueryBuilder.Edit = {}));
        })(QueryBuilder = Requests.QueryBuilder || (Requests.QueryBuilder = {}));
    })(Requests = Plugins.Requests || (Plugins.Requests = {}));
})(Plugins || (Plugins = {}));

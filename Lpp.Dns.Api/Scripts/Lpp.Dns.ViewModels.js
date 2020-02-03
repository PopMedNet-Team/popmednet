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
/// <reference path='../../Lpp.Pmn.Resources/node_modules/@types/knockout.mapping/index.d.ts' />
/// <reference path='Lpp.Dns.Interfaces.ts' />
var Dns;
(function (Dns) {
    var ViewModels;
    (function (ViewModels) {
        var ViewModel = (function () {
            function ViewModel() {
            }
            ViewModel.prototype.update = function (obj) {
                for (var prop in obj) {
                    this[prop](obj[prop]);
                }
            };
            return ViewModel;
        }());
        ViewModels.ViewModel = ViewModel;
        var EntityDtoViewModel = (function (_super) {
            __extends(EntityDtoViewModel, _super);
            function EntityDtoViewModel(BaseDTO) {
                return _super.call(this) || this;
            }
            EntityDtoViewModel.prototype.toData = function () {
                return {};
            };
            return EntityDtoViewModel;
        }(ViewModel));
        ViewModels.EntityDtoViewModel = EntityDtoViewModel;
        var EntityDtoWithIDViewModel = (function (_super) {
            __extends(EntityDtoWithIDViewModel, _super);
            function EntityDtoWithIDViewModel(BaseDTO) {
                var _this = _super.call(this, BaseDTO) || this;
                if (BaseDTO == null) {
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                return _this;
            }
            EntityDtoWithIDViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return EntityDtoWithIDViewModel;
        }(EntityDtoViewModel));
        ViewModels.EntityDtoWithIDViewModel = EntityDtoWithIDViewModel;
        var DataModelProcessorViewModel = (function (_super) {
            __extends(DataModelProcessorViewModel, _super);
            function DataModelProcessorViewModel(DataModelProcessorDTO) {
                var _this = _super.call(this) || this;
                if (DataModelProcessorDTO == null) {
                    _this.ModelID = ko.observable();
                    _this.Processor = ko.observable();
                    _this.ProcessorID = ko.observable();
                }
                else {
                    _this.ModelID = ko.observable(DataModelProcessorDTO.ModelID);
                    _this.Processor = ko.observable(DataModelProcessorDTO.Processor);
                    _this.ProcessorID = ko.observable(DataModelProcessorDTO.ProcessorID);
                }
                return _this;
            }
            DataModelProcessorViewModel.prototype.toData = function () {
                return {
                    ModelID: this.ModelID(),
                    Processor: this.Processor(),
                    ProcessorID: this.ProcessorID(),
                };
            };
            return DataModelProcessorViewModel;
        }(ViewModel));
        ViewModels.DataModelProcessorViewModel = DataModelProcessorViewModel;
        var PropertyChangeDetailViewModel = (function (_super) {
            __extends(PropertyChangeDetailViewModel, _super);
            function PropertyChangeDetailViewModel(PropertyChangeDetailDTO) {
                var _this = _super.call(this) || this;
                if (PropertyChangeDetailDTO == null) {
                    _this.Property = ko.observable();
                    _this.PropertyDisplayName = ko.observable();
                    _this.OriginalValue = ko.observable();
                    _this.OriginalValueDisplay = ko.observable();
                    _this.NewValue = ko.observable();
                    _this.NewValueDisplay = ko.observable();
                }
                else {
                    _this.Property = ko.observable(PropertyChangeDetailDTO.Property);
                    _this.PropertyDisplayName = ko.observable(PropertyChangeDetailDTO.PropertyDisplayName);
                    _this.OriginalValue = ko.observable(PropertyChangeDetailDTO.OriginalValue);
                    _this.OriginalValueDisplay = ko.observable(PropertyChangeDetailDTO.OriginalValueDisplay);
                    _this.NewValue = ko.observable(PropertyChangeDetailDTO.NewValue);
                    _this.NewValueDisplay = ko.observable(PropertyChangeDetailDTO.NewValueDisplay);
                }
                return _this;
            }
            PropertyChangeDetailViewModel.prototype.toData = function () {
                return {
                    Property: this.Property(),
                    PropertyDisplayName: this.PropertyDisplayName(),
                    OriginalValue: this.OriginalValue(),
                    OriginalValueDisplay: this.OriginalValueDisplay(),
                    NewValue: this.NewValue(),
                    NewValueDisplay: this.NewValueDisplay(),
                };
            };
            return PropertyChangeDetailViewModel;
        }(ViewModel));
        ViewModels.PropertyChangeDetailViewModel = PropertyChangeDetailViewModel;
        var HttpResponseErrors = (function (_super) {
            __extends(HttpResponseErrors, _super);
            function HttpResponseErrors(HttpResponseErrors) {
                var _this = _super.call(this) || this;
                if (HttpResponseErrors == null) {
                    _this.Errors = ko.observableArray();
                }
                else {
                    _this.Errors = ko.observableArray(HttpResponseErrors.Errors == null ? null : HttpResponseErrors.Errors.map(function (item) { return item; }));
                }
                return _this;
            }
            HttpResponseErrors.prototype.toData = function () {
                return {
                    Errors: this.Errors == null ? null : this.Errors().map(function (item) { return item; }),
                };
            };
            return HttpResponseErrors;
        }(ViewModel));
        ViewModels.HttpResponseErrors = HttpResponseErrors;
        var AddWFCommentViewModel = (function (_super) {
            __extends(AddWFCommentViewModel, _super);
            function AddWFCommentViewModel(AddWFCommentDTO) {
                var _this = _super.call(this) || this;
                if (AddWFCommentDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.WorkflowActivityID = ko.observable();
                    _this.Comment = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(AddWFCommentDTO.RequestID);
                    _this.WorkflowActivityID = ko.observable(AddWFCommentDTO.WorkflowActivityID);
                    _this.Comment = ko.observable(AddWFCommentDTO.Comment);
                }
                return _this;
            }
            AddWFCommentViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    WorkflowActivityID: this.WorkflowActivityID(),
                    Comment: this.Comment(),
                };
            };
            return AddWFCommentViewModel;
        }(ViewModel));
        ViewModels.AddWFCommentViewModel = AddWFCommentViewModel;
        var CommentDocumentReferenceViewModel = (function (_super) {
            __extends(CommentDocumentReferenceViewModel, _super);
            function CommentDocumentReferenceViewModel(CommentDocumentReferenceDTO) {
                var _this = _super.call(this) || this;
                if (CommentDocumentReferenceDTO == null) {
                    _this.CommentID = ko.observable();
                    _this.DocumentID = ko.observable();
                    _this.RevisionSetID = ko.observable();
                    _this.DocumentName = ko.observable();
                    _this.FileName = ko.observable();
                }
                else {
                    _this.CommentID = ko.observable(CommentDocumentReferenceDTO.CommentID);
                    _this.DocumentID = ko.observable(CommentDocumentReferenceDTO.DocumentID);
                    _this.RevisionSetID = ko.observable(CommentDocumentReferenceDTO.RevisionSetID);
                    _this.DocumentName = ko.observable(CommentDocumentReferenceDTO.DocumentName);
                    _this.FileName = ko.observable(CommentDocumentReferenceDTO.FileName);
                }
                return _this;
            }
            CommentDocumentReferenceViewModel.prototype.toData = function () {
                return {
                    CommentID: this.CommentID(),
                    DocumentID: this.DocumentID(),
                    RevisionSetID: this.RevisionSetID(),
                    DocumentName: this.DocumentName(),
                    FileName: this.FileName(),
                };
            };
            return CommentDocumentReferenceViewModel;
        }(ViewModel));
        ViewModels.CommentDocumentReferenceViewModel = CommentDocumentReferenceViewModel;
        var UpdateDataMartInstalledModelsViewModel = (function (_super) {
            __extends(UpdateDataMartInstalledModelsViewModel, _super);
            function UpdateDataMartInstalledModelsViewModel(UpdateDataMartInstalledModelsDTO) {
                var _this = _super.call(this) || this;
                if (UpdateDataMartInstalledModelsDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.Models = ko.observableArray();
                }
                else {
                    _this.DataMartID = ko.observable(UpdateDataMartInstalledModelsDTO.DataMartID);
                    _this.Models = ko.observableArray(UpdateDataMartInstalledModelsDTO.Models == null ? null : UpdateDataMartInstalledModelsDTO.Models.map(function (item) { return new DataMartInstalledModelViewModel(item); }));
                }
                return _this;
            }
            UpdateDataMartInstalledModelsViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    Models: this.Models == null ? null : this.Models().map(function (item) { return item.toData(); }),
                };
            };
            return UpdateDataMartInstalledModelsViewModel;
        }(ViewModel));
        ViewModels.UpdateDataMartInstalledModelsViewModel = UpdateDataMartInstalledModelsViewModel;
        var DataAvailabilityPeriodCategoryViewModel = (function (_super) {
            __extends(DataAvailabilityPeriodCategoryViewModel, _super);
            function DataAvailabilityPeriodCategoryViewModel(DataAvailabilityPeriodCategoryDTO) {
                var _this = _super.call(this) || this;
                if (DataAvailabilityPeriodCategoryDTO == null) {
                    _this.CategoryType = ko.observable();
                    _this.CategoryDescription = ko.observable();
                    _this.Published = ko.observable();
                    _this.DataMartDescription = ko.observable();
                }
                else {
                    _this.CategoryType = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryType);
                    _this.CategoryDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryDescription);
                    _this.Published = ko.observable(DataAvailabilityPeriodCategoryDTO.Published);
                    _this.DataMartDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.DataMartDescription);
                }
                return _this;
            }
            DataAvailabilityPeriodCategoryViewModel.prototype.toData = function () {
                return {
                    CategoryType: this.CategoryType(),
                    CategoryDescription: this.CategoryDescription(),
                    Published: this.Published(),
                    DataMartDescription: this.DataMartDescription(),
                };
            };
            return DataAvailabilityPeriodCategoryViewModel;
        }(ViewModel));
        ViewModels.DataAvailabilityPeriodCategoryViewModel = DataAvailabilityPeriodCategoryViewModel;
        var DataMartAvailabilityPeriodViewModel = (function (_super) {
            __extends(DataMartAvailabilityPeriodViewModel, _super);
            function DataMartAvailabilityPeriodViewModel(DataMartAvailabilityPeriodDTO) {
                var _this = _super.call(this) || this;
                if (DataMartAvailabilityPeriodDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.Period = ko.observable();
                    _this.Active = ko.observable();
                }
                else {
                    _this.DataMartID = ko.observable(DataMartAvailabilityPeriodDTO.DataMartID);
                    _this.RequestID = ko.observable(DataMartAvailabilityPeriodDTO.RequestID);
                    _this.RequestTypeID = ko.observable(DataMartAvailabilityPeriodDTO.RequestTypeID);
                    _this.Period = ko.observable(DataMartAvailabilityPeriodDTO.Period);
                    _this.Active = ko.observable(DataMartAvailabilityPeriodDTO.Active);
                }
                return _this;
            }
            DataMartAvailabilityPeriodViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    RequestID: this.RequestID(),
                    RequestTypeID: this.RequestTypeID(),
                    Period: this.Period(),
                    Active: this.Active(),
                };
            };
            return DataMartAvailabilityPeriodViewModel;
        }(ViewModel));
        ViewModels.DataMartAvailabilityPeriodViewModel = DataMartAvailabilityPeriodViewModel;
        var NotificationCrudViewModel = (function (_super) {
            __extends(NotificationCrudViewModel, _super);
            function NotificationCrudViewModel(NotificationCrudDTO) {
                var _this = _super.call(this) || this;
                if (NotificationCrudDTO == null) {
                    _this.ObjectID = ko.observable();
                    _this.State = ko.observable();
                }
                else {
                    _this.ObjectID = ko.observable(NotificationCrudDTO.ObjectID);
                    _this.State = ko.observable(NotificationCrudDTO.State);
                }
                return _this;
            }
            NotificationCrudViewModel.prototype.toData = function () {
                return {
                    ObjectID: this.ObjectID(),
                    State: this.State(),
                };
            };
            return NotificationCrudViewModel;
        }(ViewModel));
        ViewModels.NotificationCrudViewModel = NotificationCrudViewModel;
        var OrganizationUpdateEHRsesViewModel = (function (_super) {
            __extends(OrganizationUpdateEHRsesViewModel, _super);
            function OrganizationUpdateEHRsesViewModel(OrganizationUpdateEHRsesDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationUpdateEHRsesDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.EHRS = ko.observableArray();
                }
                else {
                    _this.OrganizationID = ko.observable(OrganizationUpdateEHRsesDTO.OrganizationID);
                    _this.EHRS = ko.observableArray(OrganizationUpdateEHRsesDTO.EHRS == null ? null : OrganizationUpdateEHRsesDTO.EHRS.map(function (item) { return new OrganizationEHRSViewModel(item); }));
                }
                return _this;
            }
            OrganizationUpdateEHRsesViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    EHRS: this.EHRS == null ? null : this.EHRS().map(function (item) { return item.toData(); }),
                };
            };
            return OrganizationUpdateEHRsesViewModel;
        }(ViewModel));
        ViewModels.OrganizationUpdateEHRsesViewModel = OrganizationUpdateEHRsesViewModel;
        var ProjectDataMartUpdateViewModel = (function (_super) {
            __extends(ProjectDataMartUpdateViewModel, _super);
            function ProjectDataMartUpdateViewModel(ProjectDataMartUpdateDTO) {
                var _this = _super.call(this) || this;
                if (ProjectDataMartUpdateDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.DataMarts = ko.observableArray();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectDataMartUpdateDTO.ProjectID);
                    _this.DataMarts = ko.observableArray(ProjectDataMartUpdateDTO.DataMarts == null ? null : ProjectDataMartUpdateDTO.DataMarts.map(function (item) { return new ProjectDataMartViewModel(item); }));
                }
                return _this;
            }
            ProjectDataMartUpdateViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    DataMarts: this.DataMarts == null ? null : this.DataMarts().map(function (item) { return item.toData(); }),
                };
            };
            return ProjectDataMartUpdateViewModel;
        }(ViewModel));
        ViewModels.ProjectDataMartUpdateViewModel = ProjectDataMartUpdateViewModel;
        var ProjectOrganizationUpdateViewModel = (function (_super) {
            __extends(ProjectOrganizationUpdateViewModel, _super);
            function ProjectOrganizationUpdateViewModel(ProjectOrganizationUpdateDTO) {
                var _this = _super.call(this) || this;
                if (ProjectOrganizationUpdateDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.Organizations = ko.observableArray();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectOrganizationUpdateDTO.ProjectID);
                    _this.Organizations = ko.observableArray(ProjectOrganizationUpdateDTO.Organizations == null ? null : ProjectOrganizationUpdateDTO.Organizations.map(function (item) { return new ProjectOrganizationViewModel(item); }));
                }
                return _this;
            }
            ProjectOrganizationUpdateViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    Organizations: this.Organizations == null ? null : this.Organizations().map(function (item) { return item.toData(); }),
                };
            };
            return ProjectOrganizationUpdateViewModel;
        }(ViewModel));
        ViewModels.ProjectOrganizationUpdateViewModel = ProjectOrganizationUpdateViewModel;
        var UpdateProjectRequestTypesViewModel = (function (_super) {
            __extends(UpdateProjectRequestTypesViewModel, _super);
            function UpdateProjectRequestTypesViewModel(UpdateProjectRequestTypesDTO) {
                var _this = _super.call(this) || this;
                if (UpdateProjectRequestTypesDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.RequestTypes = ko.observableArray();
                }
                else {
                    _this.ProjectID = ko.observable(UpdateProjectRequestTypesDTO.ProjectID);
                    _this.RequestTypes = ko.observableArray(UpdateProjectRequestTypesDTO.RequestTypes == null ? null : UpdateProjectRequestTypesDTO.RequestTypes.map(function (item) { return new ProjectRequestTypeViewModel(item); }));
                }
                return _this;
            }
            UpdateProjectRequestTypesViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map(function (item) { return item.toData(); }),
                };
            };
            return UpdateProjectRequestTypesViewModel;
        }(ViewModel));
        ViewModels.UpdateProjectRequestTypesViewModel = UpdateProjectRequestTypesViewModel;
        var HasGlobalSecurityForTemplateViewModel = (function (_super) {
            __extends(HasGlobalSecurityForTemplateViewModel, _super);
            function HasGlobalSecurityForTemplateViewModel(HasGlobalSecurityForTemplateDTO) {
                var _this = _super.call(this) || this;
                if (HasGlobalSecurityForTemplateDTO == null) {
                    _this.SecurityGroupExistsForGlobalPermission = ko.observable();
                    _this.CurrentUserHasGlobalPermission = ko.observable();
                }
                else {
                    _this.SecurityGroupExistsForGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.SecurityGroupExistsForGlobalPermission);
                    _this.CurrentUserHasGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.CurrentUserHasGlobalPermission);
                }
                return _this;
            }
            HasGlobalSecurityForTemplateViewModel.prototype.toData = function () {
                return {
                    SecurityGroupExistsForGlobalPermission: this.SecurityGroupExistsForGlobalPermission(),
                    CurrentUserHasGlobalPermission: this.CurrentUserHasGlobalPermission(),
                };
            };
            return HasGlobalSecurityForTemplateViewModel;
        }(ViewModel));
        ViewModels.HasGlobalSecurityForTemplateViewModel = HasGlobalSecurityForTemplateViewModel;
        var ApproveRejectResponseViewModel = (function (_super) {
            __extends(ApproveRejectResponseViewModel, _super);
            function ApproveRejectResponseViewModel(ApproveRejectResponseDTO) {
                var _this = _super.call(this) || this;
                if (ApproveRejectResponseDTO == null) {
                    _this.ResponseID = ko.observable();
                }
                else {
                    _this.ResponseID = ko.observable(ApproveRejectResponseDTO.ResponseID);
                }
                return _this;
            }
            ApproveRejectResponseViewModel.prototype.toData = function () {
                return {
                    ResponseID: this.ResponseID(),
                };
            };
            return ApproveRejectResponseViewModel;
        }(ViewModel));
        ViewModels.ApproveRejectResponseViewModel = ApproveRejectResponseViewModel;
        var EnhancedEventLogItemViewModel = (function (_super) {
            __extends(EnhancedEventLogItemViewModel, _super);
            function EnhancedEventLogItemViewModel(EnhancedEventLogItemDTO) {
                var _this = _super.call(this) || this;
                if (EnhancedEventLogItemDTO == null) {
                    _this.Step = ko.observable();
                    _this.Timestamp = ko.observable();
                    _this.Description = ko.observable();
                    _this.Source = ko.observable();
                    _this.EventType = ko.observable();
                }
                else {
                    _this.Step = ko.observable(EnhancedEventLogItemDTO.Step);
                    _this.Timestamp = ko.observable(EnhancedEventLogItemDTO.Timestamp);
                    _this.Description = ko.observable(EnhancedEventLogItemDTO.Description);
                    _this.Source = ko.observable(EnhancedEventLogItemDTO.Source);
                    _this.EventType = ko.observable(EnhancedEventLogItemDTO.EventType);
                }
                return _this;
            }
            EnhancedEventLogItemViewModel.prototype.toData = function () {
                return {
                    Step: this.Step(),
                    Timestamp: this.Timestamp(),
                    Description: this.Description(),
                    Source: this.Source(),
                    EventType: this.EventType(),
                };
            };
            return EnhancedEventLogItemViewModel;
        }(ViewModel));
        ViewModels.EnhancedEventLogItemViewModel = EnhancedEventLogItemViewModel;
        var HomepageRouteDetailViewModel = (function (_super) {
            __extends(HomepageRouteDetailViewModel, _super);
            function HomepageRouteDetailViewModel(HomepageRouteDetailDTO) {
                var _this = _super.call(this) || this;
                if (HomepageRouteDetailDTO == null) {
                    _this.RequestDataMartID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Identifier = ko.observable();
                    _this.SubmittedOn = ko.observable();
                    _this.SubmittedByName = ko.observable();
                    _this.StatusText = ko.observable();
                    _this.RequestStatus = ko.observable();
                    _this.RoutingStatus = ko.observable();
                    _this.RoutingStatusText = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.Project = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.MSRequestID = ko.observable();
                    _this.IsWorkflowRequest = ko.observable();
                    _this.CanEditMetadata = ko.observable();
                }
                else {
                    _this.RequestDataMartID = ko.observable(HomepageRouteDetailDTO.RequestDataMartID);
                    _this.DataMartID = ko.observable(HomepageRouteDetailDTO.DataMartID);
                    _this.RequestID = ko.observable(HomepageRouteDetailDTO.RequestID);
                    _this.Name = ko.observable(HomepageRouteDetailDTO.Name);
                    _this.Identifier = ko.observable(HomepageRouteDetailDTO.Identifier);
                    _this.SubmittedOn = ko.observable(HomepageRouteDetailDTO.SubmittedOn);
                    _this.SubmittedByName = ko.observable(HomepageRouteDetailDTO.SubmittedByName);
                    _this.StatusText = ko.observable(HomepageRouteDetailDTO.StatusText);
                    _this.RequestStatus = ko.observable(HomepageRouteDetailDTO.RequestStatus);
                    _this.RoutingStatus = ko.observable(HomepageRouteDetailDTO.RoutingStatus);
                    _this.RoutingStatusText = ko.observable(HomepageRouteDetailDTO.RoutingStatusText);
                    _this.RequestType = ko.observable(HomepageRouteDetailDTO.RequestType);
                    _this.Project = ko.observable(HomepageRouteDetailDTO.Project);
                    _this.Priority = ko.observable(HomepageRouteDetailDTO.Priority);
                    _this.DueDate = ko.observable(HomepageRouteDetailDTO.DueDate);
                    _this.MSRequestID = ko.observable(HomepageRouteDetailDTO.MSRequestID);
                    _this.IsWorkflowRequest = ko.observable(HomepageRouteDetailDTO.IsWorkflowRequest);
                    _this.CanEditMetadata = ko.observable(HomepageRouteDetailDTO.CanEditMetadata);
                }
                return _this;
            }
            HomepageRouteDetailViewModel.prototype.toData = function () {
                return {
                    RequestDataMartID: this.RequestDataMartID(),
                    DataMartID: this.DataMartID(),
                    RequestID: this.RequestID(),
                    Name: this.Name(),
                    Identifier: this.Identifier(),
                    SubmittedOn: this.SubmittedOn(),
                    SubmittedByName: this.SubmittedByName(),
                    StatusText: this.StatusText(),
                    RequestStatus: this.RequestStatus(),
                    RoutingStatus: this.RoutingStatus(),
                    RoutingStatusText: this.RoutingStatusText(),
                    RequestType: this.RequestType(),
                    Project: this.Project(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    MSRequestID: this.MSRequestID(),
                    IsWorkflowRequest: this.IsWorkflowRequest(),
                    CanEditMetadata: this.CanEditMetadata(),
                };
            };
            return HomepageRouteDetailViewModel;
        }(ViewModel));
        ViewModels.HomepageRouteDetailViewModel = HomepageRouteDetailViewModel;
        var RejectResponseViewModel = (function (_super) {
            __extends(RejectResponseViewModel, _super);
            function RejectResponseViewModel(RejectResponseDTO) {
                var _this = _super.call(this) || this;
                if (RejectResponseDTO == null) {
                    _this.Message = ko.observable();
                    _this.ResponseIDs = ko.observableArray();
                }
                else {
                    _this.Message = ko.observable(RejectResponseDTO.Message);
                    _this.ResponseIDs = ko.observableArray(RejectResponseDTO.ResponseIDs == null ? null : RejectResponseDTO.ResponseIDs.map(function (item) { return item; }));
                }
                return _this;
            }
            RejectResponseViewModel.prototype.toData = function () {
                return {
                    Message: this.Message(),
                    ResponseIDs: this.ResponseIDs(),
                };
            };
            return RejectResponseViewModel;
        }(ViewModel));
        ViewModels.RejectResponseViewModel = RejectResponseViewModel;
        var ApproveResponseViewModel = (function (_super) {
            __extends(ApproveResponseViewModel, _super);
            function ApproveResponseViewModel(ApproveResponseDTO) {
                var _this = _super.call(this) || this;
                if (ApproveResponseDTO == null) {
                    _this.Message = ko.observable();
                    _this.ResponseIDs = ko.observableArray();
                }
                else {
                    _this.Message = ko.observable(ApproveResponseDTO.Message);
                    _this.ResponseIDs = ko.observableArray(ApproveResponseDTO.ResponseIDs == null ? null : ApproveResponseDTO.ResponseIDs.map(function (item) { return item; }));
                }
                return _this;
            }
            ApproveResponseViewModel.prototype.toData = function () {
                return {
                    Message: this.Message(),
                    ResponseIDs: this.ResponseIDs(),
                };
            };
            return ApproveResponseViewModel;
        }(ViewModel));
        ViewModels.ApproveResponseViewModel = ApproveResponseViewModel;
        var RequestCompletionRequestViewModel = (function (_super) {
            __extends(RequestCompletionRequestViewModel, _super);
            function RequestCompletionRequestViewModel(RequestCompletionRequestDTO) {
                var _this = _super.call(this) || this;
                if (RequestCompletionRequestDTO == null) {
                    _this.DemandActivityResultID = ko.observable();
                    _this.Dto = new RequestViewModel();
                    _this.DataMarts = ko.observableArray();
                    _this.Data = ko.observable();
                    _this.Comment = ko.observable();
                }
                else {
                    _this.DemandActivityResultID = ko.observable(RequestCompletionRequestDTO.DemandActivityResultID);
                    _this.Dto = new RequestViewModel(RequestCompletionRequestDTO.Dto);
                    _this.DataMarts = ko.observableArray(RequestCompletionRequestDTO.DataMarts == null ? null : RequestCompletionRequestDTO.DataMarts.map(function (item) { return new RequestDataMartViewModel(item); }));
                    _this.Data = ko.observable(RequestCompletionRequestDTO.Data);
                    _this.Comment = ko.observable(RequestCompletionRequestDTO.Comment);
                }
                return _this;
            }
            RequestCompletionRequestViewModel.prototype.toData = function () {
                return {
                    DemandActivityResultID: this.DemandActivityResultID(),
                    Dto: this.Dto.toData(),
                    DataMarts: this.DataMarts == null ? null : this.DataMarts().map(function (item) { return item.toData(); }),
                    Data: this.Data(),
                    Comment: this.Comment(),
                };
            };
            return RequestCompletionRequestViewModel;
        }(ViewModel));
        ViewModels.RequestCompletionRequestViewModel = RequestCompletionRequestViewModel;
        var RequestCompletionResponseViewModel = (function (_super) {
            __extends(RequestCompletionResponseViewModel, _super);
            function RequestCompletionResponseViewModel(RequestCompletionResponseDTO) {
                var _this = _super.call(this) || this;
                if (RequestCompletionResponseDTO == null) {
                    _this.Uri = ko.observable();
                    _this.Entity = new RequestViewModel();
                    _this.DataMarts = ko.observableArray();
                }
                else {
                    _this.Uri = ko.observable(RequestCompletionResponseDTO.Uri);
                    _this.Entity = new RequestViewModel(RequestCompletionResponseDTO.Entity);
                    _this.DataMarts = ko.observableArray(RequestCompletionResponseDTO.DataMarts == null ? null : RequestCompletionResponseDTO.DataMarts.map(function (item) { return new RequestDataMartViewModel(item); }));
                }
                return _this;
            }
            RequestCompletionResponseViewModel.prototype.toData = function () {
                return {
                    Uri: this.Uri(),
                    Entity: this.Entity.toData(),
                    DataMarts: this.DataMarts == null ? null : this.DataMarts().map(function (item) { return item.toData(); }),
                };
            };
            return RequestCompletionResponseViewModel;
        }(ViewModel));
        ViewModels.RequestCompletionResponseViewModel = RequestCompletionResponseViewModel;
        var RequestSearchTermViewModel = (function (_super) {
            __extends(RequestSearchTermViewModel, _super);
            function RequestSearchTermViewModel(RequestSearchTermDTO) {
                var _this = _super.call(this) || this;
                if (RequestSearchTermDTO == null) {
                    _this.Type = ko.observable();
                    _this.StringValue = ko.observable();
                    _this.NumberValue = ko.observable();
                    _this.DateFrom = ko.observable();
                    _this.DateTo = ko.observable();
                    _this.NumberFrom = ko.observable();
                    _this.NumberTo = ko.observable();
                    _this.RequestID = ko.observable();
                }
                else {
                    _this.Type = ko.observable(RequestSearchTermDTO.Type);
                    _this.StringValue = ko.observable(RequestSearchTermDTO.StringValue);
                    _this.NumberValue = ko.observable(RequestSearchTermDTO.NumberValue);
                    _this.DateFrom = ko.observable(RequestSearchTermDTO.DateFrom);
                    _this.DateTo = ko.observable(RequestSearchTermDTO.DateTo);
                    _this.NumberFrom = ko.observable(RequestSearchTermDTO.NumberFrom);
                    _this.NumberTo = ko.observable(RequestSearchTermDTO.NumberTo);
                    _this.RequestID = ko.observable(RequestSearchTermDTO.RequestID);
                }
                return _this;
            }
            RequestSearchTermViewModel.prototype.toData = function () {
                return {
                    Type: this.Type(),
                    StringValue: this.StringValue(),
                    NumberValue: this.NumberValue(),
                    DateFrom: this.DateFrom(),
                    DateTo: this.DateTo(),
                    NumberFrom: this.NumberFrom(),
                    NumberTo: this.NumberTo(),
                    RequestID: this.RequestID(),
                };
            };
            return RequestSearchTermViewModel;
        }(ViewModel));
        ViewModels.RequestSearchTermViewModel = RequestSearchTermViewModel;
        var RequestTypeModelViewModel = (function (_super) {
            __extends(RequestTypeModelViewModel, _super);
            function RequestTypeModelViewModel(RequestTypeModelDTO) {
                var _this = _super.call(this) || this;
                if (RequestTypeModelDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.DataModelID = ko.observable();
                }
                else {
                    _this.RequestTypeID = ko.observable(RequestTypeModelDTO.RequestTypeID);
                    _this.DataModelID = ko.observable(RequestTypeModelDTO.DataModelID);
                }
                return _this;
            }
            RequestTypeModelViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    DataModelID: this.DataModelID(),
                };
            };
            return RequestTypeModelViewModel;
        }(ViewModel));
        ViewModels.RequestTypeModelViewModel = RequestTypeModelViewModel;
        var RequestUserViewModel = (function (_super) {
            __extends(RequestUserViewModel, _super);
            function RequestUserViewModel(RequestUserDTO) {
                var _this = _super.call(this) || this;
                if (RequestUserDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.UserID = ko.observable();
                    _this.Username = ko.observable();
                    _this.FullName = ko.observable();
                    _this.Email = ko.observable();
                    _this.WorkflowRoleID = ko.observable();
                    _this.WorkflowRole = ko.observable();
                    _this.IsRequestCreatorRole = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(RequestUserDTO.RequestID);
                    _this.UserID = ko.observable(RequestUserDTO.UserID);
                    _this.Username = ko.observable(RequestUserDTO.Username);
                    _this.FullName = ko.observable(RequestUserDTO.FullName);
                    _this.Email = ko.observable(RequestUserDTO.Email);
                    _this.WorkflowRoleID = ko.observable(RequestUserDTO.WorkflowRoleID);
                    _this.WorkflowRole = ko.observable(RequestUserDTO.WorkflowRole);
                    _this.IsRequestCreatorRole = ko.observable(RequestUserDTO.IsRequestCreatorRole);
                }
                return _this;
            }
            RequestUserViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    UserID: this.UserID(),
                    Username: this.Username(),
                    FullName: this.FullName(),
                    Email: this.Email(),
                    WorkflowRoleID: this.WorkflowRoleID(),
                    WorkflowRole: this.WorkflowRole(),
                    IsRequestCreatorRole: this.IsRequestCreatorRole(),
                };
            };
            return RequestUserViewModel;
        }(ViewModel));
        ViewModels.RequestUserViewModel = RequestUserViewModel;
        var ResponseHistoryViewModel = (function (_super) {
            __extends(ResponseHistoryViewModel, _super);
            function ResponseHistoryViewModel(ResponseHistoryDTO) {
                var _this = _super.call(this) || this;
                if (ResponseHistoryDTO == null) {
                    _this.DataMartName = ko.observable();
                    _this.HistoryItems = ko.observableArray();
                    _this.ErrorMessage = ko.observable();
                }
                else {
                    _this.DataMartName = ko.observable(ResponseHistoryDTO.DataMartName);
                    _this.HistoryItems = ko.observableArray(ResponseHistoryDTO.HistoryItems == null ? null : ResponseHistoryDTO.HistoryItems.map(function (item) { return new ResponseHistoryItemViewModel(item); }));
                    _this.ErrorMessage = ko.observable(ResponseHistoryDTO.ErrorMessage);
                }
                return _this;
            }
            ResponseHistoryViewModel.prototype.toData = function () {
                return {
                    DataMartName: this.DataMartName(),
                    HistoryItems: this.HistoryItems == null ? null : this.HistoryItems().map(function (item) { return item.toData(); }),
                    ErrorMessage: this.ErrorMessage(),
                };
            };
            return ResponseHistoryViewModel;
        }(ViewModel));
        ViewModels.ResponseHistoryViewModel = ResponseHistoryViewModel;
        var ResponseHistoryItemViewModel = (function (_super) {
            __extends(ResponseHistoryItemViewModel, _super);
            function ResponseHistoryItemViewModel(ResponseHistoryItemDTO) {
                var _this = _super.call(this) || this;
                if (ResponseHistoryItemDTO == null) {
                    _this.ResponseID = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.DateTime = ko.observable();
                    _this.Action = ko.observable();
                    _this.UserName = ko.observable();
                    _this.Message = ko.observable();
                    _this.IsResponseItem = ko.observable();
                    _this.IsCurrent = ko.observable();
                }
                else {
                    _this.ResponseID = ko.observable(ResponseHistoryItemDTO.ResponseID);
                    _this.RequestID = ko.observable(ResponseHistoryItemDTO.RequestID);
                    _this.DateTime = ko.observable(ResponseHistoryItemDTO.DateTime);
                    _this.Action = ko.observable(ResponseHistoryItemDTO.Action);
                    _this.UserName = ko.observable(ResponseHistoryItemDTO.UserName);
                    _this.Message = ko.observable(ResponseHistoryItemDTO.Message);
                    _this.IsResponseItem = ko.observable(ResponseHistoryItemDTO.IsResponseItem);
                    _this.IsCurrent = ko.observable(ResponseHistoryItemDTO.IsCurrent);
                }
                return _this;
            }
            ResponseHistoryItemViewModel.prototype.toData = function () {
                return {
                    ResponseID: this.ResponseID(),
                    RequestID: this.RequestID(),
                    DateTime: this.DateTime(),
                    Action: this.Action(),
                    UserName: this.UserName(),
                    Message: this.Message(),
                    IsResponseItem: this.IsResponseItem(),
                    IsCurrent: this.IsCurrent(),
                };
            };
            return ResponseHistoryItemViewModel;
        }(ViewModel));
        ViewModels.ResponseHistoryItemViewModel = ResponseHistoryItemViewModel;
        var SaveCriteriaGroupRequestViewModel = (function (_super) {
            __extends(SaveCriteriaGroupRequestViewModel, _super);
            function SaveCriteriaGroupRequestViewModel(SaveCriteriaGroupRequestDTO) {
                var _this = _super.call(this) || this;
                if (SaveCriteriaGroupRequestDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Json = ko.observable();
                    _this.AdapterDetail = ko.observable();
                    _this.TemplateID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.RequestID = ko.observable();
                }
                else {
                    _this.Name = ko.observable(SaveCriteriaGroupRequestDTO.Name);
                    _this.Description = ko.observable(SaveCriteriaGroupRequestDTO.Description);
                    _this.Json = ko.observable(SaveCriteriaGroupRequestDTO.Json);
                    _this.AdapterDetail = ko.observable(SaveCriteriaGroupRequestDTO.AdapterDetail);
                    _this.TemplateID = ko.observable(SaveCriteriaGroupRequestDTO.TemplateID);
                    _this.RequestTypeID = ko.observable(SaveCriteriaGroupRequestDTO.RequestTypeID);
                    _this.RequestID = ko.observable(SaveCriteriaGroupRequestDTO.RequestID);
                }
                return _this;
            }
            SaveCriteriaGroupRequestViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Json: this.Json(),
                    AdapterDetail: this.AdapterDetail(),
                    TemplateID: this.TemplateID(),
                    RequestTypeID: this.RequestTypeID(),
                    RequestID: this.RequestID(),
                };
            };
            return SaveCriteriaGroupRequestViewModel;
        }(ViewModel));
        ViewModels.SaveCriteriaGroupRequestViewModel = SaveCriteriaGroupRequestViewModel;
        var UpdateRequestDataMartStatusViewModel = (function (_super) {
            __extends(UpdateRequestDataMartStatusViewModel, _super);
            function UpdateRequestDataMartStatusViewModel(UpdateRequestDataMartStatusDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRequestDataMartStatusDTO == null) {
                    _this.RequestDataMartID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.NewStatus = ko.observable();
                    _this.Message = ko.observable();
                }
                else {
                    _this.RequestDataMartID = ko.observable(UpdateRequestDataMartStatusDTO.RequestDataMartID);
                    _this.DataMartID = ko.observable(UpdateRequestDataMartStatusDTO.DataMartID);
                    _this.NewStatus = ko.observable(UpdateRequestDataMartStatusDTO.NewStatus);
                    _this.Message = ko.observable(UpdateRequestDataMartStatusDTO.Message);
                }
                return _this;
            }
            UpdateRequestDataMartStatusViewModel.prototype.toData = function () {
                return {
                    RequestDataMartID: this.RequestDataMartID(),
                    DataMartID: this.DataMartID(),
                    NewStatus: this.NewStatus(),
                    Message: this.Message(),
                };
            };
            return UpdateRequestDataMartStatusViewModel;
        }(ViewModel));
        ViewModels.UpdateRequestDataMartStatusViewModel = UpdateRequestDataMartStatusViewModel;
        var UpdateRequestTypeModelsViewModel = (function (_super) {
            __extends(UpdateRequestTypeModelsViewModel, _super);
            function UpdateRequestTypeModelsViewModel(UpdateRequestTypeModelsDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRequestTypeModelsDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.DataModels = ko.observableArray();
                }
                else {
                    _this.RequestTypeID = ko.observable(UpdateRequestTypeModelsDTO.RequestTypeID);
                    _this.DataModels = ko.observableArray(UpdateRequestTypeModelsDTO.DataModels == null ? null : UpdateRequestTypeModelsDTO.DataModels.map(function (item) { return item; }));
                }
                return _this;
            }
            UpdateRequestTypeModelsViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    DataModels: this.DataModels(),
                };
            };
            return UpdateRequestTypeModelsViewModel;
        }(ViewModel));
        ViewModels.UpdateRequestTypeModelsViewModel = UpdateRequestTypeModelsViewModel;
        var UpdateRequestTypeRequestViewModel = (function (_super) {
            __extends(UpdateRequestTypeRequestViewModel, _super);
            function UpdateRequestTypeRequestViewModel(UpdateRequestTypeRequestDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRequestTypeRequestDTO == null) {
                    _this.RequestType = new RequestTypeViewModel();
                    _this.Template = new TemplateViewModel();
                    _this.Terms = ko.observableArray();
                    _this.NotAllowedTerms = ko.observableArray();
                    _this.Models = ko.observableArray();
                }
                else {
                    _this.RequestType = new RequestTypeViewModel(UpdateRequestTypeRequestDTO.RequestType);
                    _this.Template = new TemplateViewModel(UpdateRequestTypeRequestDTO.Template);
                    _this.Terms = ko.observableArray(UpdateRequestTypeRequestDTO.Terms == null ? null : UpdateRequestTypeRequestDTO.Terms.map(function (item) { return item; }));
                    _this.NotAllowedTerms = ko.observableArray(UpdateRequestTypeRequestDTO.NotAllowedTerms == null ? null : UpdateRequestTypeRequestDTO.NotAllowedTerms.map(function (item) { return new SectionSpecificTermViewModel(item); }));
                    _this.Models = ko.observableArray(UpdateRequestTypeRequestDTO.Models == null ? null : UpdateRequestTypeRequestDTO.Models.map(function (item) { return item; }));
                }
                return _this;
            }
            UpdateRequestTypeRequestViewModel.prototype.toData = function () {
                return {
                    RequestType: this.RequestType.toData(),
                    Template: this.Template.toData(),
                    Terms: this.Terms(),
                    NotAllowedTerms: this.NotAllowedTerms == null ? null : this.NotAllowedTerms().map(function (item) { return item.toData(); }),
                    Models: this.Models(),
                };
            };
            return UpdateRequestTypeRequestViewModel;
        }(ViewModel));
        ViewModels.UpdateRequestTypeRequestViewModel = UpdateRequestTypeRequestViewModel;
        var UpdateRequestTypeResponseViewModel = (function (_super) {
            __extends(UpdateRequestTypeResponseViewModel, _super);
            function UpdateRequestTypeResponseViewModel(UpdateRequestTypeResponseDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRequestTypeResponseDTO == null) {
                    _this.RequestType = new RequestTypeViewModel();
                    _this.Template = new TemplateViewModel();
                }
                else {
                    _this.RequestType = new RequestTypeViewModel(UpdateRequestTypeResponseDTO.RequestType);
                    _this.Template = new TemplateViewModel(UpdateRequestTypeResponseDTO.Template);
                }
                return _this;
            }
            UpdateRequestTypeResponseViewModel.prototype.toData = function () {
                return {
                    RequestType: this.RequestType.toData(),
                    Template: this.Template.toData(),
                };
            };
            return UpdateRequestTypeResponseViewModel;
        }(ViewModel));
        ViewModels.UpdateRequestTypeResponseViewModel = UpdateRequestTypeResponseViewModel;
        var UpdateRequestTypeTermsViewModel = (function (_super) {
            __extends(UpdateRequestTypeTermsViewModel, _super);
            function UpdateRequestTypeTermsViewModel(UpdateRequestTypeTermsDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRequestTypeTermsDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.Terms = ko.observableArray();
                }
                else {
                    _this.RequestTypeID = ko.observable(UpdateRequestTypeTermsDTO.RequestTypeID);
                    _this.Terms = ko.observableArray(UpdateRequestTypeTermsDTO.Terms == null ? null : UpdateRequestTypeTermsDTO.Terms.map(function (item) { return item; }));
                }
                return _this;
            }
            UpdateRequestTypeTermsViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    Terms: this.Terms(),
                };
            };
            return UpdateRequestTypeTermsViewModel;
        }(ViewModel));
        ViewModels.UpdateRequestTypeTermsViewModel = UpdateRequestTypeTermsViewModel;
        var HomepageTaskRequestUserViewModel = (function (_super) {
            __extends(HomepageTaskRequestUserViewModel, _super);
            function HomepageTaskRequestUserViewModel(HomepageTaskRequestUserDTO) {
                var _this = _super.call(this) || this;
                if (HomepageTaskRequestUserDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.TaskID = ko.observable();
                    _this.UserID = ko.observable();
                    _this.UserName = ko.observable();
                    _this.FirstName = ko.observable();
                    _this.LastName = ko.observable();
                    _this.WorkflowRoleID = ko.observable();
                    _this.WorkflowRole = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(HomepageTaskRequestUserDTO.RequestID);
                    _this.TaskID = ko.observable(HomepageTaskRequestUserDTO.TaskID);
                    _this.UserID = ko.observable(HomepageTaskRequestUserDTO.UserID);
                    _this.UserName = ko.observable(HomepageTaskRequestUserDTO.UserName);
                    _this.FirstName = ko.observable(HomepageTaskRequestUserDTO.FirstName);
                    _this.LastName = ko.observable(HomepageTaskRequestUserDTO.LastName);
                    _this.WorkflowRoleID = ko.observable(HomepageTaskRequestUserDTO.WorkflowRoleID);
                    _this.WorkflowRole = ko.observable(HomepageTaskRequestUserDTO.WorkflowRole);
                }
                return _this;
            }
            HomepageTaskRequestUserViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    TaskID: this.TaskID(),
                    UserID: this.UserID(),
                    UserName: this.UserName(),
                    FirstName: this.FirstName(),
                    LastName: this.LastName(),
                    WorkflowRoleID: this.WorkflowRoleID(),
                    WorkflowRole: this.WorkflowRole(),
                };
            };
            return HomepageTaskRequestUserViewModel;
        }(ViewModel));
        ViewModels.HomepageTaskRequestUserViewModel = HomepageTaskRequestUserViewModel;
        var HomepageTaskSummaryViewModel = (function (_super) {
            __extends(HomepageTaskSummaryViewModel, _super);
            function HomepageTaskSummaryViewModel(HomepageTaskSummaryDTO) {
                var _this = _super.call(this) || this;
                if (HomepageTaskSummaryDTO == null) {
                    _this.TaskID = ko.observable();
                    _this.TaskName = ko.observable();
                    _this.TaskStatus = ko.observable();
                    _this.TaskStatusText = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.StartOn = ko.observable();
                    _this.EndOn = ko.observable();
                    _this.Type = ko.observable();
                    _this.DirectToRequest = ko.observable();
                    _this.Name = ko.observable();
                    _this.Identifier = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.MSRequestID = ko.observable();
                    _this.RequestStatus = ko.observable();
                    _this.RequestStatusText = ko.observable();
                    _this.NewUserID = ko.observable();
                    _this.AssignedResources = ko.observable();
                }
                else {
                    _this.TaskID = ko.observable(HomepageTaskSummaryDTO.TaskID);
                    _this.TaskName = ko.observable(HomepageTaskSummaryDTO.TaskName);
                    _this.TaskStatus = ko.observable(HomepageTaskSummaryDTO.TaskStatus);
                    _this.TaskStatusText = ko.observable(HomepageTaskSummaryDTO.TaskStatusText);
                    _this.CreatedOn = ko.observable(HomepageTaskSummaryDTO.CreatedOn);
                    _this.StartOn = ko.observable(HomepageTaskSummaryDTO.StartOn);
                    _this.EndOn = ko.observable(HomepageTaskSummaryDTO.EndOn);
                    _this.Type = ko.observable(HomepageTaskSummaryDTO.Type);
                    _this.DirectToRequest = ko.observable(HomepageTaskSummaryDTO.DirectToRequest);
                    _this.Name = ko.observable(HomepageTaskSummaryDTO.Name);
                    _this.Identifier = ko.observable(HomepageTaskSummaryDTO.Identifier);
                    _this.RequestID = ko.observable(HomepageTaskSummaryDTO.RequestID);
                    _this.MSRequestID = ko.observable(HomepageTaskSummaryDTO.MSRequestID);
                    _this.RequestStatus = ko.observable(HomepageTaskSummaryDTO.RequestStatus);
                    _this.RequestStatusText = ko.observable(HomepageTaskSummaryDTO.RequestStatusText);
                    _this.NewUserID = ko.observable(HomepageTaskSummaryDTO.NewUserID);
                    _this.AssignedResources = ko.observable(HomepageTaskSummaryDTO.AssignedResources);
                }
                return _this;
            }
            HomepageTaskSummaryViewModel.prototype.toData = function () {
                return {
                    TaskID: this.TaskID(),
                    TaskName: this.TaskName(),
                    TaskStatus: this.TaskStatus(),
                    TaskStatusText: this.TaskStatusText(),
                    CreatedOn: this.CreatedOn(),
                    StartOn: this.StartOn(),
                    EndOn: this.EndOn(),
                    Type: this.Type(),
                    DirectToRequest: this.DirectToRequest(),
                    Name: this.Name(),
                    Identifier: this.Identifier(),
                    RequestID: this.RequestID(),
                    MSRequestID: this.MSRequestID(),
                    RequestStatus: this.RequestStatus(),
                    RequestStatusText: this.RequestStatusText(),
                    NewUserID: this.NewUserID(),
                    AssignedResources: this.AssignedResources(),
                };
            };
            return HomepageTaskSummaryViewModel;
        }(ViewModel));
        ViewModels.HomepageTaskSummaryViewModel = HomepageTaskSummaryViewModel;
        var ActivityViewModel = (function (_super) {
            __extends(ActivityViewModel, _super);
            function ActivityViewModel(ActivityDTO) {
                var _this = _super.call(this) || this;
                if (ActivityDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Activities = ko.observableArray();
                    _this.Description = ko.observable();
                    _this.ProjectID = ko.observable();
                    _this.DisplayOrder = ko.observable();
                    _this.TaskLevel = ko.observable();
                    _this.ParentActivityID = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.Deleted = ko.observable();
                }
                else {
                    _this.ID = ko.observable(ActivityDTO.ID);
                    _this.Name = ko.observable(ActivityDTO.Name);
                    _this.Activities = ko.observableArray(ActivityDTO.Activities == null ? null : ActivityDTO.Activities.map(function (item) { return new ActivityViewModel(item); }));
                    _this.Description = ko.observable(ActivityDTO.Description);
                    _this.ProjectID = ko.observable(ActivityDTO.ProjectID);
                    _this.DisplayOrder = ko.observable(ActivityDTO.DisplayOrder);
                    _this.TaskLevel = ko.observable(ActivityDTO.TaskLevel);
                    _this.ParentActivityID = ko.observable(ActivityDTO.ParentActivityID);
                    _this.Acronym = ko.observable(ActivityDTO.Acronym);
                    _this.Deleted = ko.observable(ActivityDTO.Deleted);
                }
                return _this;
            }
            ActivityViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                    Activities: this.Activities == null ? null : this.Activities().map(function (item) { return item.toData(); }),
                    Description: this.Description(),
                    ProjectID: this.ProjectID(),
                    DisplayOrder: this.DisplayOrder(),
                    TaskLevel: this.TaskLevel(),
                    ParentActivityID: this.ParentActivityID(),
                    Acronym: this.Acronym(),
                    Deleted: this.Deleted(),
                };
            };
            return ActivityViewModel;
        }(ViewModel));
        ViewModels.ActivityViewModel = ActivityViewModel;
        var DataMartTypeViewModel = (function (_super) {
            __extends(DataMartTypeViewModel, _super);
            function DataMartTypeViewModel(DataMartTypeDTO) {
                var _this = _super.call(this) || this;
                if (DataMartTypeDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                }
                else {
                    _this.ID = ko.observable(DataMartTypeDTO.ID);
                    _this.Name = ko.observable(DataMartTypeDTO.Name);
                }
                return _this;
            }
            DataMartTypeViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                };
            };
            return DataMartTypeViewModel;
        }(ViewModel));
        ViewModels.DataMartTypeViewModel = DataMartTypeViewModel;
        var DataMartInstalledModelViewModel = (function (_super) {
            __extends(DataMartInstalledModelViewModel, _super);
            function DataMartInstalledModelViewModel(DataMartInstalledModelDTO) {
                var _this = _super.call(this) || this;
                if (DataMartInstalledModelDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.ModelID = ko.observable();
                    _this.Model = ko.observable();
                    _this.Properties = ko.observable();
                }
                else {
                    _this.DataMartID = ko.observable(DataMartInstalledModelDTO.DataMartID);
                    _this.ModelID = ko.observable(DataMartInstalledModelDTO.ModelID);
                    _this.Model = ko.observable(DataMartInstalledModelDTO.Model);
                    _this.Properties = ko.observable(DataMartInstalledModelDTO.Properties);
                }
                return _this;
            }
            DataMartInstalledModelViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    ModelID: this.ModelID(),
                    Model: this.Model(),
                    Properties: this.Properties(),
                };
            };
            return DataMartInstalledModelViewModel;
        }(ViewModel));
        ViewModels.DataMartInstalledModelViewModel = DataMartInstalledModelViewModel;
        var DemographicViewModel = (function (_super) {
            __extends(DemographicViewModel, _super);
            function DemographicViewModel(DemographicDTO) {
                var _this = _super.call(this) || this;
                if (DemographicDTO == null) {
                    _this.Country = ko.observable();
                    _this.State = ko.observable();
                    _this.Town = ko.observable();
                    _this.Region = ko.observable();
                    _this.Gender = ko.observable();
                    _this.AgeGroup = ko.observable();
                    _this.Ethnicity = ko.observable();
                    _this.Count = ko.observable();
                }
                else {
                    _this.Country = ko.observable(DemographicDTO.Country);
                    _this.State = ko.observable(DemographicDTO.State);
                    _this.Town = ko.observable(DemographicDTO.Town);
                    _this.Region = ko.observable(DemographicDTO.Region);
                    _this.Gender = ko.observable(DemographicDTO.Gender);
                    _this.AgeGroup = ko.observable(DemographicDTO.AgeGroup);
                    _this.Ethnicity = ko.observable(DemographicDTO.Ethnicity);
                    _this.Count = ko.observable(DemographicDTO.Count);
                }
                return _this;
            }
            DemographicViewModel.prototype.toData = function () {
                return {
                    Country: this.Country(),
                    State: this.State(),
                    Town: this.Town(),
                    Region: this.Region(),
                    Gender: this.Gender(),
                    AgeGroup: this.AgeGroup(),
                    Ethnicity: this.Ethnicity(),
                    Count: this.Count(),
                };
            };
            return DemographicViewModel;
        }(ViewModel));
        ViewModels.DemographicViewModel = DemographicViewModel;
        var LookupListCategoryViewModel = (function (_super) {
            __extends(LookupListCategoryViewModel, _super);
            function LookupListCategoryViewModel(LookupListCategoryDTO) {
                var _this = _super.call(this) || this;
                if (LookupListCategoryDTO == null) {
                    _this.ListId = ko.observable();
                    _this.CategoryId = ko.observable();
                    _this.CategoryName = ko.observable();
                }
                else {
                    _this.ListId = ko.observable(LookupListCategoryDTO.ListId);
                    _this.CategoryId = ko.observable(LookupListCategoryDTO.CategoryId);
                    _this.CategoryName = ko.observable(LookupListCategoryDTO.CategoryName);
                }
                return _this;
            }
            LookupListCategoryViewModel.prototype.toData = function () {
                return {
                    ListId: this.ListId(),
                    CategoryId: this.CategoryId(),
                    CategoryName: this.CategoryName(),
                };
            };
            return LookupListCategoryViewModel;
        }(ViewModel));
        ViewModels.LookupListCategoryViewModel = LookupListCategoryViewModel;
        var LookupListDetailRequestViewModel = (function (_super) {
            __extends(LookupListDetailRequestViewModel, _super);
            function LookupListDetailRequestViewModel(LookupListDetailRequestDTO) {
                var _this = _super.call(this) || this;
                if (LookupListDetailRequestDTO == null) {
                    _this.Codes = ko.observableArray();
                    _this.ListID = ko.observable();
                }
                else {
                    _this.Codes = ko.observableArray(LookupListDetailRequestDTO.Codes == null ? null : LookupListDetailRequestDTO.Codes.map(function (item) { return item; }));
                    _this.ListID = ko.observable(LookupListDetailRequestDTO.ListID);
                }
                return _this;
            }
            LookupListDetailRequestViewModel.prototype.toData = function () {
                return {
                    Codes: this.Codes == null ? null : this.Codes().map(function (item) { return item; }),
                    ListID: this.ListID(),
                };
            };
            return LookupListDetailRequestViewModel;
        }(ViewModel));
        ViewModels.LookupListDetailRequestViewModel = LookupListDetailRequestViewModel;
        var LookupListViewModel = (function (_super) {
            __extends(LookupListViewModel, _super);
            function LookupListViewModel(LookupListDTO) {
                var _this = _super.call(this) || this;
                if (LookupListDTO == null) {
                    _this.ListId = ko.observable();
                    _this.ListName = ko.observable();
                    _this.Version = ko.observable();
                }
                else {
                    _this.ListId = ko.observable(LookupListDTO.ListId);
                    _this.ListName = ko.observable(LookupListDTO.ListName);
                    _this.Version = ko.observable(LookupListDTO.Version);
                }
                return _this;
            }
            LookupListViewModel.prototype.toData = function () {
                return {
                    ListId: this.ListId(),
                    ListName: this.ListName(),
                    Version: this.Version(),
                };
            };
            return LookupListViewModel;
        }(ViewModel));
        ViewModels.LookupListViewModel = LookupListViewModel;
        var LookupListValueViewModel = (function (_super) {
            __extends(LookupListValueViewModel, _super);
            function LookupListValueViewModel(LookupListValueDTO) {
                var _this = _super.call(this) || this;
                if (LookupListValueDTO == null) {
                    _this.ListId = ko.observable();
                    _this.CategoryId = ko.observable();
                    _this.ItemName = ko.observable();
                    _this.ItemCode = ko.observable();
                    _this.ItemCodeWithNoPeriod = ko.observable();
                    _this.ExpireDate = ko.observable();
                    _this.ID = ko.observable();
                }
                else {
                    _this.ListId = ko.observable(LookupListValueDTO.ListId);
                    _this.CategoryId = ko.observable(LookupListValueDTO.CategoryId);
                    _this.ItemName = ko.observable(LookupListValueDTO.ItemName);
                    _this.ItemCode = ko.observable(LookupListValueDTO.ItemCode);
                    _this.ItemCodeWithNoPeriod = ko.observable(LookupListValueDTO.ItemCodeWithNoPeriod);
                    _this.ExpireDate = ko.observable(LookupListValueDTO.ExpireDate);
                    _this.ID = ko.observable(LookupListValueDTO.ID);
                }
                return _this;
            }
            LookupListValueViewModel.prototype.toData = function () {
                return {
                    ListId: this.ListId(),
                    CategoryId: this.CategoryId(),
                    ItemName: this.ItemName(),
                    ItemCode: this.ItemCode(),
                    ItemCodeWithNoPeriod: this.ItemCodeWithNoPeriod(),
                    ExpireDate: this.ExpireDate(),
                    ID: this.ID(),
                };
            };
            return LookupListValueViewModel;
        }(ViewModel));
        ViewModels.LookupListValueViewModel = LookupListValueViewModel;
        var ProjectDataMartViewModel = (function (_super) {
            __extends(ProjectDataMartViewModel, _super);
            function ProjectDataMartViewModel(ProjectDataMartDTO) {
                var _this = _super.call(this) || this;
                if (ProjectDataMartDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.Project = ko.observable();
                    _this.ProjectAcronym = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.Organization = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectDataMartDTO.ProjectID);
                    _this.Project = ko.observable(ProjectDataMartDTO.Project);
                    _this.ProjectAcronym = ko.observable(ProjectDataMartDTO.ProjectAcronym);
                    _this.DataMartID = ko.observable(ProjectDataMartDTO.DataMartID);
                    _this.DataMart = ko.observable(ProjectDataMartDTO.DataMart);
                    _this.Organization = ko.observable(ProjectDataMartDTO.Organization);
                }
                return _this;
            }
            ProjectDataMartViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    Project: this.Project(),
                    ProjectAcronym: this.ProjectAcronym(),
                    DataMartID: this.DataMartID(),
                    DataMart: this.DataMart(),
                    Organization: this.Organization(),
                };
            };
            return ProjectDataMartViewModel;
        }(ViewModel));
        ViewModels.ProjectDataMartViewModel = ProjectDataMartViewModel;
        var RegistryItemDefinitionViewModel = (function (_super) {
            __extends(RegistryItemDefinitionViewModel, _super);
            function RegistryItemDefinitionViewModel(RegistryItemDefinitionDTO) {
                var _this = _super.call(this) || this;
                if (RegistryItemDefinitionDTO == null) {
                    _this.ID = ko.observable();
                    _this.Category = ko.observable();
                    _this.Title = ko.observable();
                }
                else {
                    _this.ID = ko.observable(RegistryItemDefinitionDTO.ID);
                    _this.Category = ko.observable(RegistryItemDefinitionDTO.Category);
                    _this.Title = ko.observable(RegistryItemDefinitionDTO.Title);
                }
                return _this;
            }
            RegistryItemDefinitionViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Category: this.Category(),
                    Title: this.Title(),
                };
            };
            return RegistryItemDefinitionViewModel;
        }(ViewModel));
        ViewModels.RegistryItemDefinitionViewModel = RegistryItemDefinitionViewModel;
        var UpdateRegistryItemsViewModel = (function (_super) {
            __extends(UpdateRegistryItemsViewModel, _super);
            function UpdateRegistryItemsViewModel(UpdateRegistryItemsDTO) {
                var _this = _super.call(this) || this;
                if (UpdateRegistryItemsDTO == null) {
                }
                else {
                }
                return _this;
            }
            UpdateRegistryItemsViewModel.prototype.toData = function () {
                return {};
            };
            return UpdateRegistryItemsViewModel;
        }(ViewModel));
        ViewModels.UpdateRegistryItemsViewModel = UpdateRegistryItemsViewModel;
        var WorkplanTypeViewModel = (function (_super) {
            __extends(WorkplanTypeViewModel, _super);
            function WorkplanTypeViewModel(WorkplanTypeDTO) {
                var _this = _super.call(this) || this;
                if (WorkplanTypeDTO == null) {
                    _this.ID = ko.observable();
                    _this.WorkplanTypeID = ko.observable();
                    _this.Name = ko.observable();
                    _this.NetworkID = ko.observable();
                    _this.Acronym = ko.observable();
                }
                else {
                    _this.ID = ko.observable(WorkplanTypeDTO.ID);
                    _this.WorkplanTypeID = ko.observable(WorkplanTypeDTO.WorkplanTypeID);
                    _this.Name = ko.observable(WorkplanTypeDTO.Name);
                    _this.NetworkID = ko.observable(WorkplanTypeDTO.NetworkID);
                    _this.Acronym = ko.observable(WorkplanTypeDTO.Acronym);
                }
                return _this;
            }
            WorkplanTypeViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    WorkplanTypeID: this.WorkplanTypeID(),
                    Name: this.Name(),
                    NetworkID: this.NetworkID(),
                    Acronym: this.Acronym(),
                };
            };
            return WorkplanTypeViewModel;
        }(ViewModel));
        ViewModels.WorkplanTypeViewModel = WorkplanTypeViewModel;
        var RequesterCenterViewModel = (function (_super) {
            __extends(RequesterCenterViewModel, _super);
            function RequesterCenterViewModel(RequesterCenterDTO) {
                var _this = _super.call(this) || this;
                if (RequesterCenterDTO == null) {
                    _this.ID = ko.observable();
                    _this.RequesterCenterID = ko.observable();
                    _this.Name = ko.observable();
                    _this.NetworkID = ko.observable();
                }
                else {
                    _this.ID = ko.observable(RequesterCenterDTO.ID);
                    _this.RequesterCenterID = ko.observable(RequesterCenterDTO.RequesterCenterID);
                    _this.Name = ko.observable(RequesterCenterDTO.Name);
                    _this.NetworkID = ko.observable(RequesterCenterDTO.NetworkID);
                }
                return _this;
            }
            RequesterCenterViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    RequesterCenterID: this.RequesterCenterID(),
                    Name: this.Name(),
                    NetworkID: this.NetworkID(),
                };
            };
            return RequesterCenterViewModel;
        }(ViewModel));
        ViewModels.RequesterCenterViewModel = RequesterCenterViewModel;
        var QueryTypeViewModel = (function (_super) {
            __extends(QueryTypeViewModel, _super);
            function QueryTypeViewModel(QueryTypeDTO) {
                var _this = _super.call(this) || this;
                if (QueryTypeDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                }
                else {
                    _this.ID = ko.observable(QueryTypeDTO.ID);
                    _this.Name = ko.observable(QueryTypeDTO.Name);
                    _this.Description = ko.observable(QueryTypeDTO.Description);
                }
                return _this;
            }
            QueryTypeViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                    Description: this.Description(),
                };
            };
            return QueryTypeViewModel;
        }(ViewModel));
        ViewModels.QueryTypeViewModel = QueryTypeViewModel;
        var SecurityTupleViewModel = (function (_super) {
            __extends(SecurityTupleViewModel, _super);
            function SecurityTupleViewModel(SecurityTupleDTO) {
                var _this = _super.call(this) || this;
                if (SecurityTupleDTO == null) {
                    _this.ID1 = ko.observable();
                    _this.ID2 = ko.observable();
                    _this.ID3 = ko.observable();
                    _this.ID4 = ko.observable();
                    _this.SubjectID = ko.observable();
                    _this.PrivilegeID = ko.observable();
                    _this.ViaMembership = ko.observable();
                    _this.DeniedEntries = ko.observable();
                    _this.ExplicitDeniedEntries = ko.observable();
                    _this.ExplicitAllowedEntries = ko.observable();
                    _this.ChangedOn = ko.observable();
                }
                else {
                    _this.ID1 = ko.observable(SecurityTupleDTO.ID1);
                    _this.ID2 = ko.observable(SecurityTupleDTO.ID2);
                    _this.ID3 = ko.observable(SecurityTupleDTO.ID3);
                    _this.ID4 = ko.observable(SecurityTupleDTO.ID4);
                    _this.SubjectID = ko.observable(SecurityTupleDTO.SubjectID);
                    _this.PrivilegeID = ko.observable(SecurityTupleDTO.PrivilegeID);
                    _this.ViaMembership = ko.observable(SecurityTupleDTO.ViaMembership);
                    _this.DeniedEntries = ko.observable(SecurityTupleDTO.DeniedEntries);
                    _this.ExplicitDeniedEntries = ko.observable(SecurityTupleDTO.ExplicitDeniedEntries);
                    _this.ExplicitAllowedEntries = ko.observable(SecurityTupleDTO.ExplicitAllowedEntries);
                    _this.ChangedOn = ko.observable(SecurityTupleDTO.ChangedOn);
                }
                return _this;
            }
            SecurityTupleViewModel.prototype.toData = function () {
                return {
                    ID1: this.ID1(),
                    ID2: this.ID2(),
                    ID3: this.ID3(),
                    ID4: this.ID4(),
                    SubjectID: this.SubjectID(),
                    PrivilegeID: this.PrivilegeID(),
                    ViaMembership: this.ViaMembership(),
                    DeniedEntries: this.DeniedEntries(),
                    ExplicitDeniedEntries: this.ExplicitDeniedEntries(),
                    ExplicitAllowedEntries: this.ExplicitAllowedEntries(),
                    ChangedOn: this.ChangedOn(),
                };
            };
            return SecurityTupleViewModel;
        }(ViewModel));
        ViewModels.SecurityTupleViewModel = SecurityTupleViewModel;
        var UpdateUserSecurityGroupsViewModel = (function (_super) {
            __extends(UpdateUserSecurityGroupsViewModel, _super);
            function UpdateUserSecurityGroupsViewModel(UpdateUserSecurityGroupsDTO) {
                var _this = _super.call(this) || this;
                if (UpdateUserSecurityGroupsDTO == null) {
                    _this.UserID = ko.observable();
                    _this.Groups = ko.observableArray();
                }
                else {
                    _this.UserID = ko.observable(UpdateUserSecurityGroupsDTO.UserID);
                    _this.Groups = ko.observableArray(UpdateUserSecurityGroupsDTO.Groups == null ? null : UpdateUserSecurityGroupsDTO.Groups.map(function (item) { return new SecurityGroupViewModel(item); }));
                }
                return _this;
            }
            UpdateUserSecurityGroupsViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    Groups: this.Groups == null ? null : this.Groups().map(function (item) { return item.toData(); }),
                };
            };
            return UpdateUserSecurityGroupsViewModel;
        }(ViewModel));
        ViewModels.UpdateUserSecurityGroupsViewModel = UpdateUserSecurityGroupsViewModel;
        var DesignViewModel = (function (_super) {
            __extends(DesignViewModel, _super);
            function DesignViewModel(DesignDTO) {
                var _this = _super.call(this) || this;
                if (DesignDTO == null) {
                    _this.Locked = ko.observable();
                }
                else {
                    _this.Locked = ko.observable(DesignDTO.Locked);
                }
                return _this;
            }
            DesignViewModel.prototype.toData = function () {
                return {
                    Locked: this.Locked(),
                };
            };
            return DesignViewModel;
        }(ViewModel));
        ViewModels.DesignViewModel = DesignViewModel;
        var CodeSelectorValueViewModel = (function (_super) {
            __extends(CodeSelectorValueViewModel, _super);
            function CodeSelectorValueViewModel(CodeSelectorValueDTO) {
                var _this = _super.call(this) || this;
                if (CodeSelectorValueDTO == null) {
                    _this.Code = ko.observable();
                    _this.Name = ko.observable();
                    _this.ExpireDate = ko.observable();
                }
                else {
                    _this.Code = ko.observable(CodeSelectorValueDTO.Code);
                    _this.Name = ko.observable(CodeSelectorValueDTO.Name);
                    _this.ExpireDate = ko.observable(CodeSelectorValueDTO.ExpireDate);
                }
                return _this;
            }
            CodeSelectorValueViewModel.prototype.toData = function () {
                return {
                    Code: this.Code(),
                    Name: this.Name(),
                    ExpireDate: this.ExpireDate(),
                };
            };
            return CodeSelectorValueViewModel;
        }(ViewModel));
        ViewModels.CodeSelectorValueViewModel = CodeSelectorValueViewModel;
        var ThemeViewModel = (function (_super) {
            __extends(ThemeViewModel, _super);
            function ThemeViewModel(ThemeDTO) {
                var _this = _super.call(this) || this;
                if (ThemeDTO == null) {
                    _this.Title = ko.observable();
                    _this.Terms = ko.observable();
                    _this.Info = ko.observable();
                    _this.Resources = ko.observable();
                    _this.Footer = ko.observable();
                    _this.LogoImage = ko.observable();
                    _this.SystemUserConfirmationTitle = ko.observable();
                    _this.SystemUserConfirmationContent = ko.observable();
                }
                else {
                    _this.Title = ko.observable(ThemeDTO.Title);
                    _this.Terms = ko.observable(ThemeDTO.Terms);
                    _this.Info = ko.observable(ThemeDTO.Info);
                    _this.Resources = ko.observable(ThemeDTO.Resources);
                    _this.Footer = ko.observable(ThemeDTO.Footer);
                    _this.LogoImage = ko.observable(ThemeDTO.LogoImage);
                    _this.SystemUserConfirmationTitle = ko.observable(ThemeDTO.SystemUserConfirmationTitle);
                    _this.SystemUserConfirmationContent = ko.observable(ThemeDTO.SystemUserConfirmationContent);
                }
                return _this;
            }
            ThemeViewModel.prototype.toData = function () {
                return {
                    Title: this.Title(),
                    Terms: this.Terms(),
                    Info: this.Info(),
                    Resources: this.Resources(),
                    Footer: this.Footer(),
                    LogoImage: this.LogoImage(),
                    SystemUserConfirmationTitle: this.SystemUserConfirmationTitle(),
                    SystemUserConfirmationContent: this.SystemUserConfirmationContent(),
                };
            };
            return ThemeViewModel;
        }(ViewModel));
        ViewModels.ThemeViewModel = ThemeViewModel;
        var AssignedUserNotificationViewModel = (function (_super) {
            __extends(AssignedUserNotificationViewModel, _super);
            function AssignedUserNotificationViewModel(AssignedUserNotificationDTO) {
                var _this = _super.call(this) || this;
                if (AssignedUserNotificationDTO == null) {
                    _this.Event = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Level = ko.observable();
                    _this.Description = ko.observable();
                }
                else {
                    _this.Event = ko.observable(AssignedUserNotificationDTO.Event);
                    _this.EventID = ko.observable(AssignedUserNotificationDTO.EventID);
                    _this.Level = ko.observable(AssignedUserNotificationDTO.Level);
                    _this.Description = ko.observable(AssignedUserNotificationDTO.Description);
                }
                return _this;
            }
            AssignedUserNotificationViewModel.prototype.toData = function () {
                return {
                    Event: this.Event(),
                    EventID: this.EventID(),
                    Level: this.Level(),
                    Description: this.Description(),
                };
            };
            return AssignedUserNotificationViewModel;
        }(ViewModel));
        ViewModels.AssignedUserNotificationViewModel = AssignedUserNotificationViewModel;
        var MetadataEditPermissionsSummaryViewModel = (function (_super) {
            __extends(MetadataEditPermissionsSummaryViewModel, _super);
            function MetadataEditPermissionsSummaryViewModel(MetadataEditPermissionsSummaryDTO) {
                var _this = _super.call(this) || this;
                if (MetadataEditPermissionsSummaryDTO == null) {
                    _this.CanEditRequestMetadata = ko.observable();
                    _this.EditableDataMarts = ko.observableArray();
                }
                else {
                    _this.CanEditRequestMetadata = ko.observable(MetadataEditPermissionsSummaryDTO.CanEditRequestMetadata);
                    _this.EditableDataMarts = ko.observableArray(MetadataEditPermissionsSummaryDTO.EditableDataMarts == null ? null : MetadataEditPermissionsSummaryDTO.EditableDataMarts.map(function (item) { return item; }));
                }
                return _this;
            }
            MetadataEditPermissionsSummaryViewModel.prototype.toData = function () {
                return {
                    CanEditRequestMetadata: this.CanEditRequestMetadata(),
                    EditableDataMarts: this.EditableDataMarts(),
                };
            };
            return MetadataEditPermissionsSummaryViewModel;
        }(ViewModel));
        ViewModels.MetadataEditPermissionsSummaryViewModel = MetadataEditPermissionsSummaryViewModel;
        var NotificationViewModel = (function (_super) {
            __extends(NotificationViewModel, _super);
            function NotificationViewModel(NotificationDTO) {
                var _this = _super.call(this) || this;
                if (NotificationDTO == null) {
                    _this.Timestamp = ko.observable();
                    _this.Event = ko.observable();
                    _this.Message = ko.observable();
                }
                else {
                    _this.Timestamp = ko.observable(NotificationDTO.Timestamp);
                    _this.Event = ko.observable(NotificationDTO.Event);
                    _this.Message = ko.observable(NotificationDTO.Message);
                }
                return _this;
            }
            NotificationViewModel.prototype.toData = function () {
                return {
                    Timestamp: this.Timestamp(),
                    Event: this.Event(),
                    Message: this.Message(),
                };
            };
            return NotificationViewModel;
        }(ViewModel));
        ViewModels.NotificationViewModel = NotificationViewModel;
        var ForgotPasswordViewModel = (function (_super) {
            __extends(ForgotPasswordViewModel, _super);
            function ForgotPasswordViewModel(ForgotPasswordDTO) {
                var _this = _super.call(this) || this;
                if (ForgotPasswordDTO == null) {
                    _this.UserName = ko.observable();
                    _this.Email = ko.observable();
                }
                else {
                    _this.UserName = ko.observable(ForgotPasswordDTO.UserName);
                    _this.Email = ko.observable(ForgotPasswordDTO.Email);
                }
                return _this;
            }
            ForgotPasswordViewModel.prototype.toData = function () {
                return {
                    UserName: this.UserName(),
                    Email: this.Email(),
                };
            };
            return ForgotPasswordViewModel;
        }(ViewModel));
        ViewModels.ForgotPasswordViewModel = ForgotPasswordViewModel;
        var LoginViewModel = (function (_super) {
            __extends(LoginViewModel, _super);
            function LoginViewModel(LoginDTO) {
                var _this = _super.call(this) || this;
                if (LoginDTO == null) {
                    _this.UserName = ko.observable();
                    _this.Password = ko.observable();
                    _this.RememberMe = ko.observable();
                    _this.IPAddress = ko.observable();
                    _this.Enviorment = ko.observable();
                }
                else {
                    _this.UserName = ko.observable(LoginDTO.UserName);
                    _this.Password = ko.observable(LoginDTO.Password);
                    _this.RememberMe = ko.observable(LoginDTO.RememberMe);
                    _this.IPAddress = ko.observable(LoginDTO.IPAddress);
                    _this.Enviorment = ko.observable(LoginDTO.Enviorment);
                }
                return _this;
            }
            LoginViewModel.prototype.toData = function () {
                return {
                    UserName: this.UserName(),
                    Password: this.Password(),
                    RememberMe: this.RememberMe(),
                    IPAddress: this.IPAddress(),
                    Enviorment: this.Enviorment(),
                };
            };
            return LoginViewModel;
        }(ViewModel));
        ViewModels.LoginViewModel = LoginViewModel;
        var MenuItemViewModel = (function (_super) {
            __extends(MenuItemViewModel, _super);
            function MenuItemViewModel(MenuItemDTO) {
                var _this = _super.call(this) || this;
                if (MenuItemDTO == null) {
                    _this.text = ko.observable();
                    _this.url = ko.observable();
                    _this.encoded = ko.observable();
                    _this.content = ko.observable();
                    _this.items = ko.observableArray();
                }
                else {
                    _this.text = ko.observable(MenuItemDTO.text);
                    _this.url = ko.observable(MenuItemDTO.url);
                    _this.encoded = ko.observable(MenuItemDTO.encoded);
                    _this.content = ko.observable(MenuItemDTO.content);
                    _this.items = ko.observableArray(MenuItemDTO.items == null ? null : MenuItemDTO.items.map(function (item) { return new MenuItemViewModel(item); }));
                }
                return _this;
            }
            MenuItemViewModel.prototype.toData = function () {
                return {
                    text: this.text(),
                    url: this.url(),
                    encoded: this.encoded(),
                    content: this.content(),
                    items: this.items == null ? null : this.items().map(function (item) { return item.toData(); }),
                };
            };
            return MenuItemViewModel;
        }(ViewModel));
        ViewModels.MenuItemViewModel = MenuItemViewModel;
        var ObserverViewModel = (function (_super) {
            __extends(ObserverViewModel, _super);
            function ObserverViewModel(ObserverDTO) {
                var _this = _super.call(this) || this;
                if (ObserverDTO == null) {
                    _this.ID = ko.observable();
                    _this.DisplayName = ko.observable();
                    _this.DisplayNameWithType = ko.observable();
                    _this.ObserverType = ko.observable();
                }
                else {
                    _this.ID = ko.observable(ObserverDTO.ID);
                    _this.DisplayName = ko.observable(ObserverDTO.DisplayName);
                    _this.DisplayNameWithType = ko.observable(ObserverDTO.DisplayNameWithType);
                    _this.ObserverType = ko.observable(ObserverDTO.ObserverType);
                }
                return _this;
            }
            ObserverViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    DisplayName: this.DisplayName(),
                    DisplayNameWithType: this.DisplayNameWithType(),
                    ObserverType: this.ObserverType(),
                };
            };
            return ObserverViewModel;
        }(ViewModel));
        ViewModels.ObserverViewModel = ObserverViewModel;
        var ObserverEventViewModel = (function (_super) {
            __extends(ObserverEventViewModel, _super);
            function ObserverEventViewModel(ObserverEventDTO) {
                var _this = _super.call(this) || this;
                if (ObserverEventDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                }
                else {
                    _this.ID = ko.observable(ObserverEventDTO.ID);
                    _this.Name = ko.observable(ObserverEventDTO.Name);
                }
                return _this;
            }
            ObserverEventViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                };
            };
            return ObserverEventViewModel;
        }(ViewModel));
        ViewModels.ObserverEventViewModel = ObserverEventViewModel;
        var RestorePasswordViewModel = (function (_super) {
            __extends(RestorePasswordViewModel, _super);
            function RestorePasswordViewModel(RestorePasswordDTO) {
                var _this = _super.call(this) || this;
                if (RestorePasswordDTO == null) {
                    _this.PasswordRestoreToken = ko.observable();
                    _this.Password = ko.observable();
                }
                else {
                    _this.PasswordRestoreToken = ko.observable(RestorePasswordDTO.PasswordRestoreToken);
                    _this.Password = ko.observable(RestorePasswordDTO.Password);
                }
                return _this;
            }
            RestorePasswordViewModel.prototype.toData = function () {
                return {
                    PasswordRestoreToken: this.PasswordRestoreToken(),
                    Password: this.Password(),
                };
            };
            return RestorePasswordViewModel;
        }(ViewModel));
        ViewModels.RestorePasswordViewModel = RestorePasswordViewModel;
        var TreeItemViewModel = (function (_super) {
            __extends(TreeItemViewModel, _super);
            function TreeItemViewModel(TreeItemDTO) {
                var _this = _super.call(this) || this;
                if (TreeItemDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Path = ko.observable();
                    _this.Type = ko.observable();
                    _this.SubItems = ko.observableArray();
                    _this.HasChildren = ko.observable();
                }
                else {
                    _this.ID = ko.observable(TreeItemDTO.ID);
                    _this.Name = ko.observable(TreeItemDTO.Name);
                    _this.Path = ko.observable(TreeItemDTO.Path);
                    _this.Type = ko.observable(TreeItemDTO.Type);
                    _this.SubItems = ko.observableArray(TreeItemDTO.SubItems == null ? null : TreeItemDTO.SubItems.map(function (item) { return new TreeItemViewModel(item); }));
                    _this.HasChildren = ko.observable(TreeItemDTO.HasChildren);
                }
                return _this;
            }
            TreeItemViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                    Path: this.Path(),
                    Type: this.Type(),
                    SubItems: this.SubItems == null ? null : this.SubItems().map(function (item) { return item.toData(); }),
                    HasChildren: this.HasChildren(),
                };
            };
            return TreeItemViewModel;
        }(ViewModel));
        ViewModels.TreeItemViewModel = TreeItemViewModel;
        var UpdateUserPasswordViewModel = (function (_super) {
            __extends(UpdateUserPasswordViewModel, _super);
            function UpdateUserPasswordViewModel(UpdateUserPasswordDTO) {
                var _this = _super.call(this) || this;
                if (UpdateUserPasswordDTO == null) {
                    _this.UserID = ko.observable();
                    _this.Password = ko.observable();
                }
                else {
                    _this.UserID = ko.observable(UpdateUserPasswordDTO.UserID);
                    _this.Password = ko.observable(UpdateUserPasswordDTO.Password);
                }
                return _this;
            }
            UpdateUserPasswordViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    Password: this.Password(),
                };
            };
            return UpdateUserPasswordViewModel;
        }(ViewModel));
        ViewModels.UpdateUserPasswordViewModel = UpdateUserPasswordViewModel;
        var UserAuthenticationViewModel = (function (_super) {
            __extends(UserAuthenticationViewModel, _super);
            function UserAuthenticationViewModel(UserAuthenticationDTO) {
                var _this = _super.call(this) || this;
                if (UserAuthenticationDTO == null) {
                    _this.ID = ko.observable();
                    _this.UserID = ko.observable();
                    _this.Success = ko.observable();
                    _this.Description = ko.observable();
                    _this.IPAddress = ko.observable();
                    _this.Enviorment = ko.observable();
                    _this.DateTime = ko.observable();
                }
                else {
                    _this.ID = ko.observable(UserAuthenticationDTO.ID);
                    _this.UserID = ko.observable(UserAuthenticationDTO.UserID);
                    _this.Success = ko.observable(UserAuthenticationDTO.Success);
                    _this.Description = ko.observable(UserAuthenticationDTO.Description);
                    _this.IPAddress = ko.observable(UserAuthenticationDTO.IPAddress);
                    _this.Enviorment = ko.observable(UserAuthenticationDTO.Enviorment);
                    _this.DateTime = ko.observable(UserAuthenticationDTO.DateTime);
                }
                return _this;
            }
            UserAuthenticationViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    UserID: this.UserID(),
                    Success: this.Success(),
                    Description: this.Description(),
                    IPAddress: this.IPAddress(),
                    Enviorment: this.Enviorment(),
                    DateTime: this.DateTime(),
                };
            };
            return UserAuthenticationViewModel;
        }(ViewModel));
        ViewModels.UserAuthenticationViewModel = UserAuthenticationViewModel;
        var UserRegistrationViewModel = (function (_super) {
            __extends(UserRegistrationViewModel, _super);
            function UserRegistrationViewModel(UserRegistrationDTO) {
                var _this = _super.call(this) || this;
                if (UserRegistrationDTO == null) {
                    _this.UserName = ko.observable();
                    _this.Password = ko.observable();
                    _this.Title = ko.observable();
                    _this.FirstName = ko.observable();
                    _this.LastName = ko.observable();
                    _this.MiddleName = ko.observable();
                    _this.Phone = ko.observable();
                    _this.Fax = ko.observable();
                    _this.Email = ko.observable();
                    _this.Active = ko.observable();
                    _this.SignedUpOn = ko.observable();
                    _this.OrganizationRequested = ko.observable();
                    _this.RoleRequested = ko.observable();
                }
                else {
                    _this.UserName = ko.observable(UserRegistrationDTO.UserName);
                    _this.Password = ko.observable(UserRegistrationDTO.Password);
                    _this.Title = ko.observable(UserRegistrationDTO.Title);
                    _this.FirstName = ko.observable(UserRegistrationDTO.FirstName);
                    _this.LastName = ko.observable(UserRegistrationDTO.LastName);
                    _this.MiddleName = ko.observable(UserRegistrationDTO.MiddleName);
                    _this.Phone = ko.observable(UserRegistrationDTO.Phone);
                    _this.Fax = ko.observable(UserRegistrationDTO.Fax);
                    _this.Email = ko.observable(UserRegistrationDTO.Email);
                    _this.Active = ko.observable(UserRegistrationDTO.Active);
                    _this.SignedUpOn = ko.observable(UserRegistrationDTO.SignedUpOn);
                    _this.OrganizationRequested = ko.observable(UserRegistrationDTO.OrganizationRequested);
                    _this.RoleRequested = ko.observable(UserRegistrationDTO.RoleRequested);
                }
                return _this;
            }
            UserRegistrationViewModel.prototype.toData = function () {
                return {
                    UserName: this.UserName(),
                    Password: this.Password(),
                    Title: this.Title(),
                    FirstName: this.FirstName(),
                    LastName: this.LastName(),
                    MiddleName: this.MiddleName(),
                    Phone: this.Phone(),
                    Fax: this.Fax(),
                    Email: this.Email(),
                    Active: this.Active(),
                    SignedUpOn: this.SignedUpOn(),
                    OrganizationRequested: this.OrganizationRequested(),
                    RoleRequested: this.RoleRequested(),
                };
            };
            return UserRegistrationViewModel;
        }(ViewModel));
        ViewModels.UserRegistrationViewModel = UserRegistrationViewModel;
        var DataMartRegistrationResultViewModel = (function (_super) {
            __extends(DataMartRegistrationResultViewModel, _super);
            function DataMartRegistrationResultViewModel(DataMartRegistrationResultDTO) {
                var _this = _super.call(this) || this;
                if (DataMartRegistrationResultDTO == null) {
                    _this.DataMarts = ko.observableArray();
                    _this.DataMartModels = ko.observableArray();
                    _this.Users = ko.observableArray();
                    _this.ResearchOrganization = new OrganizationViewModel();
                    _this.ProviderOrganization = new OrganizationViewModel();
                }
                else {
                    _this.DataMarts = ko.observableArray(DataMartRegistrationResultDTO.DataMarts == null ? null : DataMartRegistrationResultDTO.DataMarts.map(function (item) { return new DataMartViewModel(item); }));
                    _this.DataMartModels = ko.observableArray(DataMartRegistrationResultDTO.DataMartModels == null ? null : DataMartRegistrationResultDTO.DataMartModels.map(function (item) { return new DataMartInstalledModelViewModel(item); }));
                    _this.Users = ko.observableArray(DataMartRegistrationResultDTO.Users == null ? null : DataMartRegistrationResultDTO.Users.map(function (item) { return new UserWithSecurityDetailsViewModel(item); }));
                    _this.ResearchOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ResearchOrganization);
                    _this.ProviderOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ProviderOrganization);
                }
                return _this;
            }
            DataMartRegistrationResultViewModel.prototype.toData = function () {
                return {
                    DataMarts: this.DataMarts == null ? null : this.DataMarts().map(function (item) { return item.toData(); }),
                    DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map(function (item) { return item.toData(); }),
                    Users: this.Users == null ? null : this.Users().map(function (item) { return item.toData(); }),
                    ResearchOrganization: this.ResearchOrganization.toData(),
                    ProviderOrganization: this.ProviderOrganization.toData(),
                };
            };
            return DataMartRegistrationResultViewModel;
        }(ViewModel));
        ViewModels.DataMartRegistrationResultViewModel = DataMartRegistrationResultViewModel;
        var GetChangeRequestViewModel = (function (_super) {
            __extends(GetChangeRequestViewModel, _super);
            function GetChangeRequestViewModel(GetChangeRequestDTO) {
                var _this = _super.call(this) || this;
                if (GetChangeRequestDTO == null) {
                    _this.LastChecked = ko.observable();
                    _this.ProviderIDs = ko.observableArray();
                }
                else {
                    _this.LastChecked = ko.observable(GetChangeRequestDTO.LastChecked);
                    _this.ProviderIDs = ko.observableArray(GetChangeRequestDTO.ProviderIDs == null ? null : GetChangeRequestDTO.ProviderIDs.map(function (item) { return item; }));
                }
                return _this;
            }
            GetChangeRequestViewModel.prototype.toData = function () {
                return {
                    LastChecked: this.LastChecked(),
                    ProviderIDs: this.ProviderIDs(),
                };
            };
            return GetChangeRequestViewModel;
        }(ViewModel));
        ViewModels.GetChangeRequestViewModel = GetChangeRequestViewModel;
        var RegisterDataMartViewModel = (function (_super) {
            __extends(RegisterDataMartViewModel, _super);
            function RegisterDataMartViewModel(RegisterDataMartDTO) {
                var _this = _super.call(this) || this;
                if (RegisterDataMartDTO == null) {
                    _this.Password = ko.observable();
                    _this.Token = ko.observable();
                }
                else {
                    _this.Password = ko.observable(RegisterDataMartDTO.Password);
                    _this.Token = ko.observable(RegisterDataMartDTO.Token);
                }
                return _this;
            }
            RegisterDataMartViewModel.prototype.toData = function () {
                return {
                    Password: this.Password(),
                    Token: this.Token(),
                };
            };
            return RegisterDataMartViewModel;
        }(ViewModel));
        ViewModels.RegisterDataMartViewModel = RegisterDataMartViewModel;
        var RequestDocumentViewModel = (function (_super) {
            __extends(RequestDocumentViewModel, _super);
            function RequestDocumentViewModel(RequestDocumentDTO) {
                var _this = _super.call(this) || this;
                if (RequestDocumentDTO == null) {
                    _this.ID = ko.observable();
                    _this.Name = ko.observable();
                    _this.FileName = ko.observable();
                    _this.MimeType = ko.observable();
                    _this.Viewable = ko.observable();
                    _this.ItemID = ko.observable();
                }
                else {
                    _this.ID = ko.observable(RequestDocumentDTO.ID);
                    _this.Name = ko.observable(RequestDocumentDTO.Name);
                    _this.FileName = ko.observable(RequestDocumentDTO.FileName);
                    _this.MimeType = ko.observable(RequestDocumentDTO.MimeType);
                    _this.Viewable = ko.observable(RequestDocumentDTO.Viewable);
                    _this.ItemID = ko.observable(RequestDocumentDTO.ItemID);
                }
                return _this;
            }
            RequestDocumentViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    Name: this.Name(),
                    FileName: this.FileName(),
                    MimeType: this.MimeType(),
                    Viewable: this.Viewable(),
                    ItemID: this.ItemID(),
                };
            };
            return RequestDocumentViewModel;
        }(ViewModel));
        ViewModels.RequestDocumentViewModel = RequestDocumentViewModel;
        var UpdateResponseStatusRequestViewModel = (function (_super) {
            __extends(UpdateResponseStatusRequestViewModel, _super);
            function UpdateResponseStatusRequestViewModel(UpdateResponseStatusRequestDTO) {
                var _this = _super.call(this) || this;
                if (UpdateResponseStatusRequestDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.ResponseID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.ProjectID = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.UserID = ko.observable();
                    _this.StatusID = ko.observable();
                    _this.Message = ko.observable();
                    _this.RejectReason = ko.observable();
                    _this.HoldReason = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.RequestTypeName = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(UpdateResponseStatusRequestDTO.RequestID);
                    _this.ResponseID = ko.observable(UpdateResponseStatusRequestDTO.ResponseID);
                    _this.DataMartID = ko.observable(UpdateResponseStatusRequestDTO.DataMartID);
                    _this.ProjectID = ko.observable(UpdateResponseStatusRequestDTO.ProjectID);
                    _this.OrganizationID = ko.observable(UpdateResponseStatusRequestDTO.OrganizationID);
                    _this.UserID = ko.observable(UpdateResponseStatusRequestDTO.UserID);
                    _this.StatusID = ko.observable(UpdateResponseStatusRequestDTO.StatusID);
                    _this.Message = ko.observable(UpdateResponseStatusRequestDTO.Message);
                    _this.RejectReason = ko.observable(UpdateResponseStatusRequestDTO.RejectReason);
                    _this.HoldReason = ko.observable(UpdateResponseStatusRequestDTO.HoldReason);
                    _this.RequestTypeID = ko.observable(UpdateResponseStatusRequestDTO.RequestTypeID);
                    _this.RequestTypeName = ko.observable(UpdateResponseStatusRequestDTO.RequestTypeName);
                }
                return _this;
            }
            UpdateResponseStatusRequestViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    ResponseID: this.ResponseID(),
                    DataMartID: this.DataMartID(),
                    ProjectID: this.ProjectID(),
                    OrganizationID: this.OrganizationID(),
                    UserID: this.UserID(),
                    StatusID: this.StatusID(),
                    Message: this.Message(),
                    RejectReason: this.RejectReason(),
                    HoldReason: this.HoldReason(),
                    RequestTypeID: this.RequestTypeID(),
                    RequestTypeName: this.RequestTypeName(),
                };
            };
            return UpdateResponseStatusRequestViewModel;
        }(ViewModel));
        ViewModels.UpdateResponseStatusRequestViewModel = UpdateResponseStatusRequestViewModel;
        var WbdChangeSetViewModel = (function (_super) {
            __extends(WbdChangeSetViewModel, _super);
            function WbdChangeSetViewModel(WbdChangeSetDTO) {
                var _this = _super.call(this) || this;
                if (WbdChangeSetDTO == null) {
                    _this.Requests = ko.observableArray();
                    _this.Projects = ko.observableArray();
                    _this.DataMarts = ko.observableArray();
                    _this.DataMartModels = ko.observableArray();
                    _this.RequestDataMarts = ko.observableArray();
                    _this.ProjectDataMarts = ko.observableArray();
                    _this.Organizations = ko.observableArray();
                    _this.Documents = ko.observableArray();
                    _this.Users = ko.observableArray();
                    _this.Responses = ko.observableArray();
                    _this.SecurityGroups = ko.observableArray();
                    _this.RequestResponseSecurityACLs = ko.observableArray();
                    _this.DataMartSecurityACLs = ko.observableArray();
                    _this.ManageWbdACLs = ko.observableArray();
                }
                else {
                    _this.Requests = ko.observableArray(WbdChangeSetDTO.Requests == null ? null : WbdChangeSetDTO.Requests.map(function (item) { return new RequestViewModel(item); }));
                    _this.Projects = ko.observableArray(WbdChangeSetDTO.Projects == null ? null : WbdChangeSetDTO.Projects.map(function (item) { return new ProjectViewModel(item); }));
                    _this.DataMarts = ko.observableArray(WbdChangeSetDTO.DataMarts == null ? null : WbdChangeSetDTO.DataMarts.map(function (item) { return new DataMartViewModel(item); }));
                    _this.DataMartModels = ko.observableArray(WbdChangeSetDTO.DataMartModels == null ? null : WbdChangeSetDTO.DataMartModels.map(function (item) { return new DataMartInstalledModelViewModel(item); }));
                    _this.RequestDataMarts = ko.observableArray(WbdChangeSetDTO.RequestDataMarts == null ? null : WbdChangeSetDTO.RequestDataMarts.map(function (item) { return new RequestDataMartViewModel(item); }));
                    _this.ProjectDataMarts = ko.observableArray(WbdChangeSetDTO.ProjectDataMarts == null ? null : WbdChangeSetDTO.ProjectDataMarts.map(function (item) { return new ProjectDataMartViewModel(item); }));
                    _this.Organizations = ko.observableArray(WbdChangeSetDTO.Organizations == null ? null : WbdChangeSetDTO.Organizations.map(function (item) { return new OrganizationViewModel(item); }));
                    _this.Documents = ko.observableArray(WbdChangeSetDTO.Documents == null ? null : WbdChangeSetDTO.Documents.map(function (item) { return new RequestDocumentViewModel(item); }));
                    _this.Users = ko.observableArray(WbdChangeSetDTO.Users == null ? null : WbdChangeSetDTO.Users.map(function (item) { return new UserWithSecurityDetailsViewModel(item); }));
                    _this.Responses = ko.observableArray(WbdChangeSetDTO.Responses == null ? null : WbdChangeSetDTO.Responses.map(function (item) { return new ResponseDetailViewModel(item); }));
                    _this.SecurityGroups = ko.observableArray(WbdChangeSetDTO.SecurityGroups == null ? null : WbdChangeSetDTO.SecurityGroups.map(function (item) { return new SecurityGroupWithUsersViewModel(item); }));
                    _this.RequestResponseSecurityACLs = ko.observableArray(WbdChangeSetDTO.RequestResponseSecurityACLs == null ? null : WbdChangeSetDTO.RequestResponseSecurityACLs.map(function (item) { return new SecurityTupleViewModel(item); }));
                    _this.DataMartSecurityACLs = ko.observableArray(WbdChangeSetDTO.DataMartSecurityACLs == null ? null : WbdChangeSetDTO.DataMartSecurityACLs.map(function (item) { return new SecurityTupleViewModel(item); }));
                    _this.ManageWbdACLs = ko.observableArray(WbdChangeSetDTO.ManageWbdACLs == null ? null : WbdChangeSetDTO.ManageWbdACLs.map(function (item) { return new SecurityTupleViewModel(item); }));
                }
                return _this;
            }
            WbdChangeSetViewModel.prototype.toData = function () {
                return {
                    Requests: this.Requests == null ? null : this.Requests().map(function (item) { return item.toData(); }),
                    Projects: this.Projects == null ? null : this.Projects().map(function (item) { return item.toData(); }),
                    DataMarts: this.DataMarts == null ? null : this.DataMarts().map(function (item) { return item.toData(); }),
                    DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map(function (item) { return item.toData(); }),
                    RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map(function (item) { return item.toData(); }),
                    ProjectDataMarts: this.ProjectDataMarts == null ? null : this.ProjectDataMarts().map(function (item) { return item.toData(); }),
                    Organizations: this.Organizations == null ? null : this.Organizations().map(function (item) { return item.toData(); }),
                    Documents: this.Documents == null ? null : this.Documents().map(function (item) { return item.toData(); }),
                    Users: this.Users == null ? null : this.Users().map(function (item) { return item.toData(); }),
                    Responses: this.Responses == null ? null : this.Responses().map(function (item) { return item.toData(); }),
                    SecurityGroups: this.SecurityGroups == null ? null : this.SecurityGroups().map(function (item) { return item.toData(); }),
                    RequestResponseSecurityACLs: this.RequestResponseSecurityACLs == null ? null : this.RequestResponseSecurityACLs().map(function (item) { return item.toData(); }),
                    DataMartSecurityACLs: this.DataMartSecurityACLs == null ? null : this.DataMartSecurityACLs().map(function (item) { return item.toData(); }),
                    ManageWbdACLs: this.ManageWbdACLs == null ? null : this.ManageWbdACLs().map(function (item) { return item.toData(); }),
                };
            };
            return WbdChangeSetViewModel;
        }(ViewModel));
        ViewModels.WbdChangeSetViewModel = WbdChangeSetViewModel;
        var CommonResponseDetailViewModel = (function (_super) {
            __extends(CommonResponseDetailViewModel, _super);
            function CommonResponseDetailViewModel(CommonResponseDetailDTO) {
                var _this = _super.call(this) || this;
                if (CommonResponseDetailDTO == null) {
                    _this.RequestDataMarts = ko.observableArray();
                    _this.Responses = ko.observableArray();
                    _this.Documents = ko.observableArray();
                    _this.CanViewPendingApprovalResponses = ko.observable();
                    _this.ExportForFileDistribution = ko.observable();
                }
                else {
                    _this.RequestDataMarts = ko.observableArray(CommonResponseDetailDTO.RequestDataMarts == null ? null : CommonResponseDetailDTO.RequestDataMarts.map(function (item) { return new RequestDataMartViewModel(item); }));
                    _this.Responses = ko.observableArray(CommonResponseDetailDTO.Responses == null ? null : CommonResponseDetailDTO.Responses.map(function (item) { return new ResponseViewModel(item); }));
                    _this.Documents = ko.observableArray(CommonResponseDetailDTO.Documents == null ? null : CommonResponseDetailDTO.Documents.map(function (item) { return new ExtendedDocumentViewModel(item); }));
                    _this.CanViewPendingApprovalResponses = ko.observable(CommonResponseDetailDTO.CanViewPendingApprovalResponses);
                    _this.ExportForFileDistribution = ko.observable(CommonResponseDetailDTO.ExportForFileDistribution);
                }
                return _this;
            }
            CommonResponseDetailViewModel.prototype.toData = function () {
                return {
                    RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map(function (item) { return item.toData(); }),
                    Responses: this.Responses == null ? null : this.Responses().map(function (item) { return item.toData(); }),
                    Documents: this.Documents == null ? null : this.Documents().map(function (item) { return item.toData(); }),
                    CanViewPendingApprovalResponses: this.CanViewPendingApprovalResponses(),
                    ExportForFileDistribution: this.ExportForFileDistribution(),
                };
            };
            return CommonResponseDetailViewModel;
        }(ViewModel));
        ViewModels.CommonResponseDetailViewModel = CommonResponseDetailViewModel;
        var PrepareSpecificationViewModel = (function (_super) {
            __extends(PrepareSpecificationViewModel, _super);
            function PrepareSpecificationViewModel(PrepareSpecificationDTO) {
                var _this = _super.call(this) || this;
                if (PrepareSpecificationDTO == null) {
                }
                else {
                }
                return _this;
            }
            PrepareSpecificationViewModel.prototype.toData = function () {
                return {};
            };
            return PrepareSpecificationViewModel;
        }(ViewModel));
        ViewModels.PrepareSpecificationViewModel = PrepareSpecificationViewModel;
        var RequestFormViewModel = (function (_super) {
            __extends(RequestFormViewModel, _super);
            function RequestFormViewModel(RequestFormDTO) {
                var _this = _super.call(this) || this;
                if (RequestFormDTO == null) {
                    _this.RequestDueDate = ko.observable();
                    _this.ContactInfo = ko.observable();
                    _this.RequestingTeam = ko.observable();
                    _this.FDAReview = ko.observable();
                    _this.FDADivisionNA = ko.observable();
                    _this.FDADivisionDAAAP = ko.observable();
                    _this.FDADivisionDBRUP = ko.observable();
                    _this.FDADivisionDCARP = ko.observable();
                    _this.FDADivisionDDDP = ko.observable();
                    _this.FDADivisionDGIEP = ko.observable();
                    _this.FDADivisionDMIP = ko.observable();
                    _this.FDADivisionDMEP = ko.observable();
                    _this.FDADivisionDNP = ko.observable();
                    _this.FDADivisionDDP = ko.observable();
                    _this.FDADivisionDPARP = ko.observable();
                    _this.FDADivisionOther = ko.observable();
                    _this.QueryLevel = ko.observable();
                    _this.AdjustmentMethod = ko.observable();
                    _this.CohortID = ko.observable();
                    _this.StudyObjectives = ko.observable();
                    _this.RequestStartDate = ko.observable();
                    _this.RequestEndDate = ko.observable();
                    _this.AgeGroups = ko.observable();
                    _this.CoverageTypes = ko.observable();
                    _this.EnrollmentGap = ko.observable();
                    _this.EnrollmentExposure = ko.observable();
                    _this.DefineExposures = ko.observable();
                    _this.WashoutPeirod = ko.observable();
                    _this.OtherExposures = ko.observable();
                    _this.OneOrManyExposures = ko.observable();
                    _this.AdditionalInclusion = ko.observable();
                    _this.AdditionalInclusionEvaluation = ko.observable();
                    _this.AdditionalExclusion = ko.observable();
                    _this.AdditionalExclusionEvaluation = ko.observable();
                    _this.VaryWashoutPeirod = ko.observable();
                    _this.VaryExposures = ko.observable();
                    _this.DefineExposures1 = ko.observable();
                    _this.DefineExposures2 = ko.observable();
                    _this.DefineExposures3 = ko.observable();
                    _this.DefineExposures4 = ko.observable();
                    _this.DefineExposures5 = ko.observable();
                    _this.DefineExposures6 = ko.observable();
                    _this.DefineExposures7 = ko.observable();
                    _this.DefineExposures8 = ko.observable();
                    _this.DefineExposures9 = ko.observable();
                    _this.DefineExposures10 = ko.observable();
                    _this.DefineExposures11 = ko.observable();
                    _this.DefineExposures12 = ko.observable();
                    _this.WashoutPeriod1 = ko.observable();
                    _this.WashoutPeriod2 = ko.observable();
                    _this.WashoutPeriod3 = ko.observable();
                    _this.WashoutPeriod4 = ko.observable();
                    _this.WashoutPeriod5 = ko.observable();
                    _this.WashoutPeriod6 = ko.observable();
                    _this.WashoutPeriod7 = ko.observable();
                    _this.WashoutPeriod8 = ko.observable();
                    _this.WashoutPeriod9 = ko.observable();
                    _this.WashoutPeriod10 = ko.observable();
                    _this.WashoutPeriod11 = ko.observable();
                    _this.WashoutPeriod12 = ko.observable();
                    _this.IncidenceRefinement1 = ko.observable();
                    _this.IncidenceRefinement2 = ko.observable();
                    _this.IncidenceRefinement3 = ko.observable();
                    _this.IncidenceRefinement4 = ko.observable();
                    _this.IncidenceRefinement5 = ko.observable();
                    _this.IncidenceRefinement6 = ko.observable();
                    _this.IncidenceRefinement7 = ko.observable();
                    _this.IncidenceRefinement8 = ko.observable();
                    _this.IncidenceRefinement9 = ko.observable();
                    _this.IncidenceRefinement10 = ko.observable();
                    _this.IncidenceRefinement11 = ko.observable();
                    _this.IncidenceRefinement12 = ko.observable();
                    _this.SpecifyExposedTimeAssessment1 = ko.observable();
                    _this.SpecifyExposedTimeAssessment2 = ko.observable();
                    _this.SpecifyExposedTimeAssessment3 = ko.observable();
                    _this.SpecifyExposedTimeAssessment4 = ko.observable();
                    _this.SpecifyExposedTimeAssessment5 = ko.observable();
                    _this.SpecifyExposedTimeAssessment6 = ko.observable();
                    _this.SpecifyExposedTimeAssessment7 = ko.observable();
                    _this.SpecifyExposedTimeAssessment8 = ko.observable();
                    _this.SpecifyExposedTimeAssessment9 = ko.observable();
                    _this.SpecifyExposedTimeAssessment10 = ko.observable();
                    _this.SpecifyExposedTimeAssessment11 = ko.observable();
                    _this.SpecifyExposedTimeAssessment12 = ko.observable();
                    _this.EpisodeAllowableGap1 = ko.observable();
                    _this.EpisodeAllowableGap2 = ko.observable();
                    _this.EpisodeAllowableGap3 = ko.observable();
                    _this.EpisodeAllowableGap4 = ko.observable();
                    _this.EpisodeAllowableGap5 = ko.observable();
                    _this.EpisodeAllowableGap6 = ko.observable();
                    _this.EpisodeAllowableGap7 = ko.observable();
                    _this.EpisodeAllowableGap8 = ko.observable();
                    _this.EpisodeAllowableGap9 = ko.observable();
                    _this.EpisodeAllowableGap10 = ko.observable();
                    _this.EpisodeAllowableGap11 = ko.observable();
                    _this.EpisodeAllowableGap12 = ko.observable();
                    _this.EpisodeExtensionPeriod1 = ko.observable();
                    _this.EpisodeExtensionPeriod2 = ko.observable();
                    _this.EpisodeExtensionPeriod3 = ko.observable();
                    _this.EpisodeExtensionPeriod4 = ko.observable();
                    _this.EpisodeExtensionPeriod5 = ko.observable();
                    _this.EpisodeExtensionPeriod6 = ko.observable();
                    _this.EpisodeExtensionPeriod7 = ko.observable();
                    _this.EpisodeExtensionPeriod8 = ko.observable();
                    _this.EpisodeExtensionPeriod9 = ko.observable();
                    _this.EpisodeExtensionPeriod10 = ko.observable();
                    _this.EpisodeExtensionPeriod11 = ko.observable();
                    _this.EpisodeExtensionPeriod12 = ko.observable();
                    _this.MinimumEpisodeDuration1 = ko.observable();
                    _this.MinimumEpisodeDuration2 = ko.observable();
                    _this.MinimumEpisodeDuration3 = ko.observable();
                    _this.MinimumEpisodeDuration4 = ko.observable();
                    _this.MinimumEpisodeDuration5 = ko.observable();
                    _this.MinimumEpisodeDuration6 = ko.observable();
                    _this.MinimumEpisodeDuration7 = ko.observable();
                    _this.MinimumEpisodeDuration8 = ko.observable();
                    _this.MinimumEpisodeDuration9 = ko.observable();
                    _this.MinimumEpisodeDuration10 = ko.observable();
                    _this.MinimumEpisodeDuration11 = ko.observable();
                    _this.MinimumEpisodeDuration12 = ko.observable();
                    _this.MinimumDaysSupply1 = ko.observable();
                    _this.MinimumDaysSupply2 = ko.observable();
                    _this.MinimumDaysSupply3 = ko.observable();
                    _this.MinimumDaysSupply4 = ko.observable();
                    _this.MinimumDaysSupply5 = ko.observable();
                    _this.MinimumDaysSupply6 = ko.observable();
                    _this.MinimumDaysSupply7 = ko.observable();
                    _this.MinimumDaysSupply8 = ko.observable();
                    _this.MinimumDaysSupply9 = ko.observable();
                    _this.MinimumDaysSupply10 = ko.observable();
                    _this.MinimumDaysSupply11 = ko.observable();
                    _this.MinimumDaysSupply12 = ko.observable();
                    _this.SpecifyFollowUpDuration1 = ko.observable();
                    _this.SpecifyFollowUpDuration2 = ko.observable();
                    _this.SpecifyFollowUpDuration3 = ko.observable();
                    _this.SpecifyFollowUpDuration4 = ko.observable();
                    _this.SpecifyFollowUpDuration5 = ko.observable();
                    _this.SpecifyFollowUpDuration6 = ko.observable();
                    _this.SpecifyFollowUpDuration7 = ko.observable();
                    _this.SpecifyFollowUpDuration8 = ko.observable();
                    _this.SpecifyFollowUpDuration9 = ko.observable();
                    _this.SpecifyFollowUpDuration10 = ko.observable();
                    _this.SpecifyFollowUpDuration11 = ko.observable();
                    _this.SpecifyFollowUpDuration12 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes1 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes2 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes3 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes4 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes5 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes6 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes7 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes8 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes9 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes10 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes11 = ko.observable();
                    _this.AllowOnOrMultipleExposureEpisodes12 = ko.observable();
                    _this.TruncateExposedtime1 = ko.observable();
                    _this.TruncateExposedtime2 = ko.observable();
                    _this.TruncateExposedtime3 = ko.observable();
                    _this.TruncateExposedtime4 = ko.observable();
                    _this.TruncateExposedtime5 = ko.observable();
                    _this.TruncateExposedtime6 = ko.observable();
                    _this.TruncateExposedtime7 = ko.observable();
                    _this.TruncateExposedtime8 = ko.observable();
                    _this.TruncateExposedtime9 = ko.observable();
                    _this.TruncateExposedtime10 = ko.observable();
                    _this.TruncateExposedtime11 = ko.observable();
                    _this.TruncateExposedtime12 = ko.observable();
                    _this.TruncateExposedTimeSpecified1 = ko.observable();
                    _this.TruncateExposedTimeSpecified2 = ko.observable();
                    _this.TruncateExposedTimeSpecified3 = ko.observable();
                    _this.TruncateExposedTimeSpecified4 = ko.observable();
                    _this.TruncateExposedTimeSpecified5 = ko.observable();
                    _this.TruncateExposedTimeSpecified6 = ko.observable();
                    _this.TruncateExposedTimeSpecified7 = ko.observable();
                    _this.TruncateExposedTimeSpecified8 = ko.observable();
                    _this.TruncateExposedTimeSpecified9 = ko.observable();
                    _this.TruncateExposedTimeSpecified10 = ko.observable();
                    _this.TruncateExposedTimeSpecified11 = ko.observable();
                    _this.TruncateExposedTimeSpecified12 = ko.observable();
                    _this.SpecifyBlackoutPeriod1 = ko.observable();
                    _this.SpecifyBlackoutPeriod2 = ko.observable();
                    _this.SpecifyBlackoutPeriod3 = ko.observable();
                    _this.SpecifyBlackoutPeriod4 = ko.observable();
                    _this.SpecifyBlackoutPeriod5 = ko.observable();
                    _this.SpecifyBlackoutPeriod6 = ko.observable();
                    _this.SpecifyBlackoutPeriod7 = ko.observable();
                    _this.SpecifyBlackoutPeriod8 = ko.observable();
                    _this.SpecifyBlackoutPeriod9 = ko.observable();
                    _this.SpecifyBlackoutPeriod10 = ko.observable();
                    _this.SpecifyBlackoutPeriod11 = ko.observable();
                    _this.SpecifyBlackoutPeriod12 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup14 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup15 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup16 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup14 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup15 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup16 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup24 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup25 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup26 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup24 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup25 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup26 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup34 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup35 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup36 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup34 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup35 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup36 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup44 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup45 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup46 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup44 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup45 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup46 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup54 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup55 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup56 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup54 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup55 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup56 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup64 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup65 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup66 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup64 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup65 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup66 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup71 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup72 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup73 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup74 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup75 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup76 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup71 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup72 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup73 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup74 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup75 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup76 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup81 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup82 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup83 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup84 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup85 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup86 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup81 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup82 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup83 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup84 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup85 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup86 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup91 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup92 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup93 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup94 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup95 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup96 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup91 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup92 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup93 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup94 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup95 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup96 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup101 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup102 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup103 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup104 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup105 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup106 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup101 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup102 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup103 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup104 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup105 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup106 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup111 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup112 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup113 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup114 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup115 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup116 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup111 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup112 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup113 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup114 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup115 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup116 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup121 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup122 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup123 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup124 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup125 = ko.observable();
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup126 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup121 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup122 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup123 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup124 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup125 = ko.observable();
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup126 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup14 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup15 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup16 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup14 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup15 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup16 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup24 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup25 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup26 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup24 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup25 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup26 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup34 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup35 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup36 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup34 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup35 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup36 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup44 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup45 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup46 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup44 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup45 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup46 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup54 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup55 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup56 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup54 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup55 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup56 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup64 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup65 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup66 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup64 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup65 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup66 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup71 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup72 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup73 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup74 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup75 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup76 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup71 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup72 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup73 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup74 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup75 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup76 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup81 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup82 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup83 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup84 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup85 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup86 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup81 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup82 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup83 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup84 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup85 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup86 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup91 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup92 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup93 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup94 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup95 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup96 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup91 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup92 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup93 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup94 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup95 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup96 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup101 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup102 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup103 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup104 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup105 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup106 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup101 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup102 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup103 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup104 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup105 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup106 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup111 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup112 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup113 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup114 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup115 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup116 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup111 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup112 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup113 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup114 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup115 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup116 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup121 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup122 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup123 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup124 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup125 = ko.observable();
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup126 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup121 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup122 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup123 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup124 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup125 = ko.observable();
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup126 = ko.observable();
                    _this.LookBackPeriodGroup1 = ko.observable();
                    _this.LookBackPeriodGroup2 = ko.observable();
                    _this.LookBackPeriodGroup3 = ko.observable();
                    _this.LookBackPeriodGroup4 = ko.observable();
                    _this.LookBackPeriodGroup5 = ko.observable();
                    _this.LookBackPeriodGroup6 = ko.observable();
                    _this.LookBackPeriodGroup7 = ko.observable();
                    _this.LookBackPeriodGroup8 = ko.observable();
                    _this.LookBackPeriodGroup9 = ko.observable();
                    _this.LookBackPeriodGroup10 = ko.observable();
                    _this.LookBackPeriodGroup11 = ko.observable();
                    _this.LookBackPeriodGroup12 = ko.observable();
                    _this.IncludeIndexDate1 = ko.observable();
                    _this.IncludeIndexDate2 = ko.observable();
                    _this.IncludeIndexDate3 = ko.observable();
                    _this.IncludeIndexDate4 = ko.observable();
                    _this.IncludeIndexDate5 = ko.observable();
                    _this.IncludeIndexDate6 = ko.observable();
                    _this.IncludeIndexDate7 = ko.observable();
                    _this.IncludeIndexDate8 = ko.observable();
                    _this.IncludeIndexDate9 = ko.observable();
                    _this.IncludeIndexDate10 = ko.observable();
                    _this.IncludeIndexDate11 = ko.observable();
                    _this.IncludeIndexDate12 = ko.observable();
                    _this.StratificationCategories1 = ko.observable();
                    _this.StratificationCategories2 = ko.observable();
                    _this.StratificationCategories3 = ko.observable();
                    _this.StratificationCategories4 = ko.observable();
                    _this.StratificationCategories5 = ko.observable();
                    _this.StratificationCategories6 = ko.observable();
                    _this.StratificationCategories7 = ko.observable();
                    _this.StratificationCategories8 = ko.observable();
                    _this.StratificationCategories9 = ko.observable();
                    _this.StratificationCategories10 = ko.observable();
                    _this.StratificationCategories11 = ko.observable();
                    _this.StratificationCategories12 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod1 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod2 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod3 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod4 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod5 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod6 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod7 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod8 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod9 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod10 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod11 = ko.observable();
                    _this.TwelveSpecifyLoopBackPeriod12 = ko.observable();
                    _this.TwelveIncludeIndexDate1 = ko.observable();
                    _this.TwelveIncludeIndexDate2 = ko.observable();
                    _this.TwelveIncludeIndexDate3 = ko.observable();
                    _this.TwelveIncludeIndexDate4 = ko.observable();
                    _this.TwelveIncludeIndexDate5 = ko.observable();
                    _this.TwelveIncludeIndexDate6 = ko.observable();
                    _this.TwelveIncludeIndexDate7 = ko.observable();
                    _this.TwelveIncludeIndexDate8 = ko.observable();
                    _this.TwelveIncludeIndexDate9 = ko.observable();
                    _this.TwelveIncludeIndexDate10 = ko.observable();
                    _this.TwelveIncludeIndexDate11 = ko.observable();
                    _this.TwelveIncludeIndexDate12 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits1 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits2 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits3 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits4 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits5 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits6 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits7 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits8 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits9 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits10 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits11 = ko.observable();
                    _this.CareSettingsToDefineMedicalVisits12 = ko.observable();
                    _this.TwelveStratificationCategories1 = ko.observable();
                    _this.TwelveStratificationCategories2 = ko.observable();
                    _this.TwelveStratificationCategories3 = ko.observable();
                    _this.TwelveStratificationCategories4 = ko.observable();
                    _this.TwelveStratificationCategories5 = ko.observable();
                    _this.TwelveStratificationCategories6 = ko.observable();
                    _this.TwelveStratificationCategories7 = ko.observable();
                    _this.TwelveStratificationCategories8 = ko.observable();
                    _this.TwelveStratificationCategories9 = ko.observable();
                    _this.TwelveStratificationCategories10 = ko.observable();
                    _this.TwelveStratificationCategories11 = ko.observable();
                    _this.TwelveStratificationCategories12 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod1 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod2 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod3 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod4 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod5 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod6 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod7 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod8 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod9 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod10 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod11 = ko.observable();
                    _this.VaryLengthOfWashoutPeriod12 = ko.observable();
                    _this.VaryUserExposedTime1 = ko.observable();
                    _this.VaryUserExposedTime2 = ko.observable();
                    _this.VaryUserExposedTime3 = ko.observable();
                    _this.VaryUserExposedTime4 = ko.observable();
                    _this.VaryUserExposedTime5 = ko.observable();
                    _this.VaryUserExposedTime6 = ko.observable();
                    _this.VaryUserExposedTime7 = ko.observable();
                    _this.VaryUserExposedTime8 = ko.observable();
                    _this.VaryUserExposedTime9 = ko.observable();
                    _this.VaryUserExposedTime10 = ko.observable();
                    _this.VaryUserExposedTime11 = ko.observable();
                    _this.VaryUserExposedTime12 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration1 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration2 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration3 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration4 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration5 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration6 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration7 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration8 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration9 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration10 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration11 = ko.observable();
                    _this.VaryUserFollowupPeriodDuration12 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod1 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod2 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod3 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod4 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod5 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod6 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod7 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod8 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod9 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod10 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod11 = ko.observable();
                    _this.VaryBlackoutPeriodPeriod12 = ko.observable();
                    _this.Level2or3DefineExposures1Exposure = ko.observable();
                    _this.Level2or3DefineExposures1Compare = ko.observable();
                    _this.Level2or3DefineExposures2Exposure = ko.observable();
                    _this.Level2or3DefineExposures2Compare = ko.observable();
                    _this.Level2or3DefineExposures3Exposure = ko.observable();
                    _this.Level2or3DefineExposures3Compare = ko.observable();
                    _this.Level2or3WashoutPeriod1Exposure = ko.observable();
                    _this.Level2or3WashoutPeriod1Compare = ko.observable();
                    _this.Level2or3WashoutPeriod2Exposure = ko.observable();
                    _this.Level2or3WashoutPeriod2Compare = ko.observable();
                    _this.Level2or3WashoutPeriod3Exposure = ko.observable();
                    _this.Level2or3WashoutPeriod3Compare = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment1Exposure = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment1Compare = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment2Exposure = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment2Compare = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment3Exposure = ko.observable();
                    _this.Level2or3SpecifyExposedTimeAssessment3Compare = ko.observable();
                    _this.Level2or3EpisodeAllowableGap1Exposure = ko.observable();
                    _this.Level2or3EpisodeAllowableGap1Compare = ko.observable();
                    _this.Level2or3EpisodeAllowableGap2Exposure = ko.observable();
                    _this.Level2or3EpisodeAllowableGap2Compare = ko.observable();
                    _this.Level2or3EpisodeAllowableGap3Exposure = ko.observable();
                    _this.Level2or3EpisodeAllowableGap3Compare = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod1Exposure = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod1Compare = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod2Exposure = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod2Compare = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod3Exposure = ko.observable();
                    _this.Level2or3EpisodeExtensionPeriod3Compare = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration1Exposure = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration1Compare = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration2Exposure = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration2Compare = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration3Exposure = ko.observable();
                    _this.Level2or3MinimumEpisodeDuration3Compare = ko.observable();
                    _this.Level2or3MinimumDaysSupply1Exposure = ko.observable();
                    _this.Level2or3MinimumDaysSupply1Compare = ko.observable();
                    _this.Level2or3MinimumDaysSupply2Exposure = ko.observable();
                    _this.Level2or3MinimumDaysSupply2Compare = ko.observable();
                    _this.Level2or3MinimumDaysSupply3Exposure = ko.observable();
                    _this.Level2or3MinimumDaysSupply3Compare = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration1Exposure = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration1Compare = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration2Exposure = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration2Compare = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration3Exposure = ko.observable();
                    _this.Level2or3SpecifyFollowUpDuration3Compare = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure = ko.observable();
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare = ko.observable();
                    _this.Level2or3TruncateExposedtime1Exposure = ko.observable();
                    _this.Level2or3TruncateExposedtime1Compare = ko.observable();
                    _this.Level2or3TruncateExposedtime2Exposure = ko.observable();
                    _this.Level2or3TruncateExposedtime2Compare = ko.observable();
                    _this.Level2or3TruncateExposedtime3Exposure = ko.observable();
                    _this.Level2or3TruncateExposedtime3Compare = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified1Exposure = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified1Compare = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified2Exposure = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified2Compare = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified3Exposure = ko.observable();
                    _this.Level2or3TruncateExposedTimeSpecified3Compare = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod1Exposure = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod1Compare = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod2Exposure = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod2Compare = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod3Exposure = ko.observable();
                    _this.Level2or3SpecifyBlackoutPeriod3Compare = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable();
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable();
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod1Exposure = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod1Compare = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod2Exposure = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod2Compare = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod3Exposure = ko.observable();
                    _this.Level2or3VaryLengthOfWashoutPeriod3Compare = ko.observable();
                    _this.Level2or3VaryUserExposedTime1Exposure = ko.observable();
                    _this.Level2or3VaryUserExposedTime1Compare = ko.observable();
                    _this.Level2or3VaryUserExposedTime2Exposure = ko.observable();
                    _this.Level2or3VaryUserExposedTime2Compare = ko.observable();
                    _this.Level2or3VaryUserExposedTime3Exposure = ko.observable();
                    _this.Level2or3VaryUserExposedTime3Compare = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod1Exposure = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod1Compare = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod2Exposure = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod2Compare = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod3Exposure = ko.observable();
                    _this.Level2or3VaryBlackoutPeriodPeriod3Compare = ko.observable();
                    _this.OutcomeList = ko.observableArray();
                    _this.AgeCovariate = ko.observable();
                    _this.SexCovariate = ko.observable();
                    _this.TimeCovariate = ko.observable();
                    _this.YearCovariate = ko.observable();
                    _this.ComorbidityCovariate = ko.observable();
                    _this.HealthCovariate = ko.observable();
                    _this.DrugCovariate = ko.observable();
                    _this.CovariateList = ko.observableArray();
                    _this.hdPSAnalysis = ko.observable();
                    _this.InclusionCovariates = ko.observable();
                    _this.PoolCovariates = ko.observable();
                    _this.SelectionCovariates = ko.observable();
                    _this.ZeroCellCorrection = ko.observable();
                    _this.MatchingRatio = ko.observable();
                    _this.MatchingCalipers = ko.observable();
                    _this.VaryMatchingRatio = ko.observable();
                    _this.VaryMatchingCalipers = ko.observable();
                }
                else {
                    _this.RequestDueDate = ko.observable(RequestFormDTO.RequestDueDate);
                    _this.ContactInfo = ko.observable(RequestFormDTO.ContactInfo);
                    _this.RequestingTeam = ko.observable(RequestFormDTO.RequestingTeam);
                    _this.FDAReview = ko.observable(RequestFormDTO.FDAReview);
                    _this.FDADivisionNA = ko.observable(RequestFormDTO.FDADivisionNA);
                    _this.FDADivisionDAAAP = ko.observable(RequestFormDTO.FDADivisionDAAAP);
                    _this.FDADivisionDBRUP = ko.observable(RequestFormDTO.FDADivisionDBRUP);
                    _this.FDADivisionDCARP = ko.observable(RequestFormDTO.FDADivisionDCARP);
                    _this.FDADivisionDDDP = ko.observable(RequestFormDTO.FDADivisionDDDP);
                    _this.FDADivisionDGIEP = ko.observable(RequestFormDTO.FDADivisionDGIEP);
                    _this.FDADivisionDMIP = ko.observable(RequestFormDTO.FDADivisionDMIP);
                    _this.FDADivisionDMEP = ko.observable(RequestFormDTO.FDADivisionDMEP);
                    _this.FDADivisionDNP = ko.observable(RequestFormDTO.FDADivisionDNP);
                    _this.FDADivisionDDP = ko.observable(RequestFormDTO.FDADivisionDDP);
                    _this.FDADivisionDPARP = ko.observable(RequestFormDTO.FDADivisionDPARP);
                    _this.FDADivisionOther = ko.observable(RequestFormDTO.FDADivisionOther);
                    _this.QueryLevel = ko.observable(RequestFormDTO.QueryLevel);
                    _this.AdjustmentMethod = ko.observable(RequestFormDTO.AdjustmentMethod);
                    _this.CohortID = ko.observable(RequestFormDTO.CohortID);
                    _this.StudyObjectives = ko.observable(RequestFormDTO.StudyObjectives);
                    _this.RequestStartDate = ko.observable(RequestFormDTO.RequestStartDate);
                    _this.RequestEndDate = ko.observable(RequestFormDTO.RequestEndDate);
                    _this.AgeGroups = ko.observable(RequestFormDTO.AgeGroups);
                    _this.CoverageTypes = ko.observable(RequestFormDTO.CoverageTypes);
                    _this.EnrollmentGap = ko.observable(RequestFormDTO.EnrollmentGap);
                    _this.EnrollmentExposure = ko.observable(RequestFormDTO.EnrollmentExposure);
                    _this.DefineExposures = ko.observable(RequestFormDTO.DefineExposures);
                    _this.WashoutPeirod = ko.observable(RequestFormDTO.WashoutPeirod);
                    _this.OtherExposures = ko.observable(RequestFormDTO.OtherExposures);
                    _this.OneOrManyExposures = ko.observable(RequestFormDTO.OneOrManyExposures);
                    _this.AdditionalInclusion = ko.observable(RequestFormDTO.AdditionalInclusion);
                    _this.AdditionalInclusionEvaluation = ko.observable(RequestFormDTO.AdditionalInclusionEvaluation);
                    _this.AdditionalExclusion = ko.observable(RequestFormDTO.AdditionalExclusion);
                    _this.AdditionalExclusionEvaluation = ko.observable(RequestFormDTO.AdditionalExclusionEvaluation);
                    _this.VaryWashoutPeirod = ko.observable(RequestFormDTO.VaryWashoutPeirod);
                    _this.VaryExposures = ko.observable(RequestFormDTO.VaryExposures);
                    _this.DefineExposures1 = ko.observable(RequestFormDTO.DefineExposures1);
                    _this.DefineExposures2 = ko.observable(RequestFormDTO.DefineExposures2);
                    _this.DefineExposures3 = ko.observable(RequestFormDTO.DefineExposures3);
                    _this.DefineExposures4 = ko.observable(RequestFormDTO.DefineExposures4);
                    _this.DefineExposures5 = ko.observable(RequestFormDTO.DefineExposures5);
                    _this.DefineExposures6 = ko.observable(RequestFormDTO.DefineExposures6);
                    _this.DefineExposures7 = ko.observable(RequestFormDTO.DefineExposures7);
                    _this.DefineExposures8 = ko.observable(RequestFormDTO.DefineExposures8);
                    _this.DefineExposures9 = ko.observable(RequestFormDTO.DefineExposures9);
                    _this.DefineExposures10 = ko.observable(RequestFormDTO.DefineExposures10);
                    _this.DefineExposures11 = ko.observable(RequestFormDTO.DefineExposures11);
                    _this.DefineExposures12 = ko.observable(RequestFormDTO.DefineExposures12);
                    _this.WashoutPeriod1 = ko.observable(RequestFormDTO.WashoutPeriod1);
                    _this.WashoutPeriod2 = ko.observable(RequestFormDTO.WashoutPeriod2);
                    _this.WashoutPeriod3 = ko.observable(RequestFormDTO.WashoutPeriod3);
                    _this.WashoutPeriod4 = ko.observable(RequestFormDTO.WashoutPeriod4);
                    _this.WashoutPeriod5 = ko.observable(RequestFormDTO.WashoutPeriod5);
                    _this.WashoutPeriod6 = ko.observable(RequestFormDTO.WashoutPeriod6);
                    _this.WashoutPeriod7 = ko.observable(RequestFormDTO.WashoutPeriod7);
                    _this.WashoutPeriod8 = ko.observable(RequestFormDTO.WashoutPeriod8);
                    _this.WashoutPeriod9 = ko.observable(RequestFormDTO.WashoutPeriod9);
                    _this.WashoutPeriod10 = ko.observable(RequestFormDTO.WashoutPeriod10);
                    _this.WashoutPeriod11 = ko.observable(RequestFormDTO.WashoutPeriod11);
                    _this.WashoutPeriod12 = ko.observable(RequestFormDTO.WashoutPeriod12);
                    _this.IncidenceRefinement1 = ko.observable(RequestFormDTO.IncidenceRefinement1);
                    _this.IncidenceRefinement2 = ko.observable(RequestFormDTO.IncidenceRefinement2);
                    _this.IncidenceRefinement3 = ko.observable(RequestFormDTO.IncidenceRefinement3);
                    _this.IncidenceRefinement4 = ko.observable(RequestFormDTO.IncidenceRefinement4);
                    _this.IncidenceRefinement5 = ko.observable(RequestFormDTO.IncidenceRefinement5);
                    _this.IncidenceRefinement6 = ko.observable(RequestFormDTO.IncidenceRefinement6);
                    _this.IncidenceRefinement7 = ko.observable(RequestFormDTO.IncidenceRefinement7);
                    _this.IncidenceRefinement8 = ko.observable(RequestFormDTO.IncidenceRefinement8);
                    _this.IncidenceRefinement9 = ko.observable(RequestFormDTO.IncidenceRefinement9);
                    _this.IncidenceRefinement10 = ko.observable(RequestFormDTO.IncidenceRefinement10);
                    _this.IncidenceRefinement11 = ko.observable(RequestFormDTO.IncidenceRefinement11);
                    _this.IncidenceRefinement12 = ko.observable(RequestFormDTO.IncidenceRefinement12);
                    _this.SpecifyExposedTimeAssessment1 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment1);
                    _this.SpecifyExposedTimeAssessment2 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment2);
                    _this.SpecifyExposedTimeAssessment3 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment3);
                    _this.SpecifyExposedTimeAssessment4 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment4);
                    _this.SpecifyExposedTimeAssessment5 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment5);
                    _this.SpecifyExposedTimeAssessment6 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment6);
                    _this.SpecifyExposedTimeAssessment7 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment7);
                    _this.SpecifyExposedTimeAssessment8 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment8);
                    _this.SpecifyExposedTimeAssessment9 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment9);
                    _this.SpecifyExposedTimeAssessment10 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment10);
                    _this.SpecifyExposedTimeAssessment11 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment11);
                    _this.SpecifyExposedTimeAssessment12 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment12);
                    _this.EpisodeAllowableGap1 = ko.observable(RequestFormDTO.EpisodeAllowableGap1);
                    _this.EpisodeAllowableGap2 = ko.observable(RequestFormDTO.EpisodeAllowableGap2);
                    _this.EpisodeAllowableGap3 = ko.observable(RequestFormDTO.EpisodeAllowableGap3);
                    _this.EpisodeAllowableGap4 = ko.observable(RequestFormDTO.EpisodeAllowableGap4);
                    _this.EpisodeAllowableGap5 = ko.observable(RequestFormDTO.EpisodeAllowableGap5);
                    _this.EpisodeAllowableGap6 = ko.observable(RequestFormDTO.EpisodeAllowableGap6);
                    _this.EpisodeAllowableGap7 = ko.observable(RequestFormDTO.EpisodeAllowableGap7);
                    _this.EpisodeAllowableGap8 = ko.observable(RequestFormDTO.EpisodeAllowableGap8);
                    _this.EpisodeAllowableGap9 = ko.observable(RequestFormDTO.EpisodeAllowableGap9);
                    _this.EpisodeAllowableGap10 = ko.observable(RequestFormDTO.EpisodeAllowableGap10);
                    _this.EpisodeAllowableGap11 = ko.observable(RequestFormDTO.EpisodeAllowableGap11);
                    _this.EpisodeAllowableGap12 = ko.observable(RequestFormDTO.EpisodeAllowableGap12);
                    _this.EpisodeExtensionPeriod1 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod1);
                    _this.EpisodeExtensionPeriod2 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod2);
                    _this.EpisodeExtensionPeriod3 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod3);
                    _this.EpisodeExtensionPeriod4 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod4);
                    _this.EpisodeExtensionPeriod5 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod5);
                    _this.EpisodeExtensionPeriod6 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod6);
                    _this.EpisodeExtensionPeriod7 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod7);
                    _this.EpisodeExtensionPeriod8 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod8);
                    _this.EpisodeExtensionPeriod9 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod9);
                    _this.EpisodeExtensionPeriod10 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod10);
                    _this.EpisodeExtensionPeriod11 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod11);
                    _this.EpisodeExtensionPeriod12 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod12);
                    _this.MinimumEpisodeDuration1 = ko.observable(RequestFormDTO.MinimumEpisodeDuration1);
                    _this.MinimumEpisodeDuration2 = ko.observable(RequestFormDTO.MinimumEpisodeDuration2);
                    _this.MinimumEpisodeDuration3 = ko.observable(RequestFormDTO.MinimumEpisodeDuration3);
                    _this.MinimumEpisodeDuration4 = ko.observable(RequestFormDTO.MinimumEpisodeDuration4);
                    _this.MinimumEpisodeDuration5 = ko.observable(RequestFormDTO.MinimumEpisodeDuration5);
                    _this.MinimumEpisodeDuration6 = ko.observable(RequestFormDTO.MinimumEpisodeDuration6);
                    _this.MinimumEpisodeDuration7 = ko.observable(RequestFormDTO.MinimumEpisodeDuration7);
                    _this.MinimumEpisodeDuration8 = ko.observable(RequestFormDTO.MinimumEpisodeDuration8);
                    _this.MinimumEpisodeDuration9 = ko.observable(RequestFormDTO.MinimumEpisodeDuration9);
                    _this.MinimumEpisodeDuration10 = ko.observable(RequestFormDTO.MinimumEpisodeDuration10);
                    _this.MinimumEpisodeDuration11 = ko.observable(RequestFormDTO.MinimumEpisodeDuration11);
                    _this.MinimumEpisodeDuration12 = ko.observable(RequestFormDTO.MinimumEpisodeDuration12);
                    _this.MinimumDaysSupply1 = ko.observable(RequestFormDTO.MinimumDaysSupply1);
                    _this.MinimumDaysSupply2 = ko.observable(RequestFormDTO.MinimumDaysSupply2);
                    _this.MinimumDaysSupply3 = ko.observable(RequestFormDTO.MinimumDaysSupply3);
                    _this.MinimumDaysSupply4 = ko.observable(RequestFormDTO.MinimumDaysSupply4);
                    _this.MinimumDaysSupply5 = ko.observable(RequestFormDTO.MinimumDaysSupply5);
                    _this.MinimumDaysSupply6 = ko.observable(RequestFormDTO.MinimumDaysSupply6);
                    _this.MinimumDaysSupply7 = ko.observable(RequestFormDTO.MinimumDaysSupply7);
                    _this.MinimumDaysSupply8 = ko.observable(RequestFormDTO.MinimumDaysSupply8);
                    _this.MinimumDaysSupply9 = ko.observable(RequestFormDTO.MinimumDaysSupply9);
                    _this.MinimumDaysSupply10 = ko.observable(RequestFormDTO.MinimumDaysSupply10);
                    _this.MinimumDaysSupply11 = ko.observable(RequestFormDTO.MinimumDaysSupply11);
                    _this.MinimumDaysSupply12 = ko.observable(RequestFormDTO.MinimumDaysSupply12);
                    _this.SpecifyFollowUpDuration1 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration1);
                    _this.SpecifyFollowUpDuration2 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration2);
                    _this.SpecifyFollowUpDuration3 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration3);
                    _this.SpecifyFollowUpDuration4 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration4);
                    _this.SpecifyFollowUpDuration5 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration5);
                    _this.SpecifyFollowUpDuration6 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration6);
                    _this.SpecifyFollowUpDuration7 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration7);
                    _this.SpecifyFollowUpDuration8 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration8);
                    _this.SpecifyFollowUpDuration9 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration9);
                    _this.SpecifyFollowUpDuration10 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration10);
                    _this.SpecifyFollowUpDuration11 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration11);
                    _this.SpecifyFollowUpDuration12 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration12);
                    _this.AllowOnOrMultipleExposureEpisodes1 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes1);
                    _this.AllowOnOrMultipleExposureEpisodes2 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes2);
                    _this.AllowOnOrMultipleExposureEpisodes3 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes3);
                    _this.AllowOnOrMultipleExposureEpisodes4 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes4);
                    _this.AllowOnOrMultipleExposureEpisodes5 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes5);
                    _this.AllowOnOrMultipleExposureEpisodes6 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes6);
                    _this.AllowOnOrMultipleExposureEpisodes7 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes7);
                    _this.AllowOnOrMultipleExposureEpisodes8 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes8);
                    _this.AllowOnOrMultipleExposureEpisodes9 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes9);
                    _this.AllowOnOrMultipleExposureEpisodes10 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes10);
                    _this.AllowOnOrMultipleExposureEpisodes11 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes11);
                    _this.AllowOnOrMultipleExposureEpisodes12 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes12);
                    _this.TruncateExposedtime1 = ko.observable(RequestFormDTO.TruncateExposedtime1);
                    _this.TruncateExposedtime2 = ko.observable(RequestFormDTO.TruncateExposedtime2);
                    _this.TruncateExposedtime3 = ko.observable(RequestFormDTO.TruncateExposedtime3);
                    _this.TruncateExposedtime4 = ko.observable(RequestFormDTO.TruncateExposedtime4);
                    _this.TruncateExposedtime5 = ko.observable(RequestFormDTO.TruncateExposedtime5);
                    _this.TruncateExposedtime6 = ko.observable(RequestFormDTO.TruncateExposedtime6);
                    _this.TruncateExposedtime7 = ko.observable(RequestFormDTO.TruncateExposedtime7);
                    _this.TruncateExposedtime8 = ko.observable(RequestFormDTO.TruncateExposedtime8);
                    _this.TruncateExposedtime9 = ko.observable(RequestFormDTO.TruncateExposedtime9);
                    _this.TruncateExposedtime10 = ko.observable(RequestFormDTO.TruncateExposedtime10);
                    _this.TruncateExposedtime11 = ko.observable(RequestFormDTO.TruncateExposedtime11);
                    _this.TruncateExposedtime12 = ko.observable(RequestFormDTO.TruncateExposedtime12);
                    _this.TruncateExposedTimeSpecified1 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified1);
                    _this.TruncateExposedTimeSpecified2 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified2);
                    _this.TruncateExposedTimeSpecified3 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified3);
                    _this.TruncateExposedTimeSpecified4 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified4);
                    _this.TruncateExposedTimeSpecified5 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified5);
                    _this.TruncateExposedTimeSpecified6 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified6);
                    _this.TruncateExposedTimeSpecified7 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified7);
                    _this.TruncateExposedTimeSpecified8 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified8);
                    _this.TruncateExposedTimeSpecified9 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified9);
                    _this.TruncateExposedTimeSpecified10 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified10);
                    _this.TruncateExposedTimeSpecified11 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified11);
                    _this.TruncateExposedTimeSpecified12 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified12);
                    _this.SpecifyBlackoutPeriod1 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod1);
                    _this.SpecifyBlackoutPeriod2 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod2);
                    _this.SpecifyBlackoutPeriod3 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod3);
                    _this.SpecifyBlackoutPeriod4 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod4);
                    _this.SpecifyBlackoutPeriod5 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod5);
                    _this.SpecifyBlackoutPeriod6 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod6);
                    _this.SpecifyBlackoutPeriod7 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod7);
                    _this.SpecifyBlackoutPeriod8 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod8);
                    _this.SpecifyBlackoutPeriod9 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod9);
                    _this.SpecifyBlackoutPeriod10 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod10);
                    _this.SpecifyBlackoutPeriod11 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod11);
                    _this.SpecifyBlackoutPeriod12 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod12);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup11);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup12);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup13);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup14);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup15);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup16);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup11);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup12);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup13);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup14);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup15);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup16);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup21);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup22);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup23);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup24);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup25);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup26);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup21);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup22);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup23);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup24);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup25);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup26);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup31);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup32);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup33);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup34);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup35);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup36);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup31);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup32);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup33);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup34);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup35);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup36);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup41);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup42);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup43);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup44);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup45);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup46);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup41);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup42);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup43);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup44);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup45);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup46);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup51);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup52);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup53);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup54);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup55);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup56);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup51);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup52);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup53);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup54);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup55);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup56);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup61);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup62);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup63);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup64);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup65);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup66);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup61);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup62);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup63);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup64);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup65);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup66);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup71);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup72);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup73);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup74);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup75);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup76);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup71);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup72);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup73);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup74);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup75);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup76);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup81);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup82);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup83);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup84);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup85);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup86);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup81);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup82);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup83);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup84);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup85);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup86);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup91);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup92);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup93);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup94);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup95);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup96);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup91);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup92);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup93);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup94);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup95);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup96);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup101);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup102);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup103);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup104);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup105);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup106);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup101);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup102);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup103);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup104);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup105);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup106);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup111);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup112);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup113);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup114);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup115);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup116);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup111);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup112);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup113);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup114);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup115);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup116);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup121);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup122);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup123);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup124);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup125);
                    _this.SpecifyAdditionalInclusionInclusionCriteriaGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup126);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup121);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup122);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup123);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup124);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup125);
                    _this.SpecifyAdditionalInclusionEvaluationWindowGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup126);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup11);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup12);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup13);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup14);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup15);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup16);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup11);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup12);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup13);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup14);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup15);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup16);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup21);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup22);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup23);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup24);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup25);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup26);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup21);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup22);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup23);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup24);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup25);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup26);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup31);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup32);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup33);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup34);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup35);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup36);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup31);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup32);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup33);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup34);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup35);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup36);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup41);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup42);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup43);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup44);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup45);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup46);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup41);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup42);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup43);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup44);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup45);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup46);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup51);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup52);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup53);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup54);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup55);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup56);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup51);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup52);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup53);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup54);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup55);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup56);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup61);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup62);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup63);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup64);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup65);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup66);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup61);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup62);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup63);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup64);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup65);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup66);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup71);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup72);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup73);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup74);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup75);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup76);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup71);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup72);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup73);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup74);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup75);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup76);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup81);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup82);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup83);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup84);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup85);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup86);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup81);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup82);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup83);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup84);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup85);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup86);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup91);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup92);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup93);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup94);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup95);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup96);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup91);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup92);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup93);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup94);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup95);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup96);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup101);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup102);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup103);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup104);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup105);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup106);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup101);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup102);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup103);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup104);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup105);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup106);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup111);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup112);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup113);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup114);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup115);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup116);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup111);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup112);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup113);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup114);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup115);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup116);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup121);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup122);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup123);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup124);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup125);
                    _this.SpecifyAdditionalExclusionInclusionCriteriaGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup126);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup121);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup122);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup123);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup124);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup125);
                    _this.SpecifyAdditionalExclusionEvaluationWindowGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup126);
                    _this.LookBackPeriodGroup1 = ko.observable(RequestFormDTO.LookBackPeriodGroup1);
                    _this.LookBackPeriodGroup2 = ko.observable(RequestFormDTO.LookBackPeriodGroup2);
                    _this.LookBackPeriodGroup3 = ko.observable(RequestFormDTO.LookBackPeriodGroup3);
                    _this.LookBackPeriodGroup4 = ko.observable(RequestFormDTO.LookBackPeriodGroup4);
                    _this.LookBackPeriodGroup5 = ko.observable(RequestFormDTO.LookBackPeriodGroup5);
                    _this.LookBackPeriodGroup6 = ko.observable(RequestFormDTO.LookBackPeriodGroup6);
                    _this.LookBackPeriodGroup7 = ko.observable(RequestFormDTO.LookBackPeriodGroup7);
                    _this.LookBackPeriodGroup8 = ko.observable(RequestFormDTO.LookBackPeriodGroup8);
                    _this.LookBackPeriodGroup9 = ko.observable(RequestFormDTO.LookBackPeriodGroup9);
                    _this.LookBackPeriodGroup10 = ko.observable(RequestFormDTO.LookBackPeriodGroup10);
                    _this.LookBackPeriodGroup11 = ko.observable(RequestFormDTO.LookBackPeriodGroup11);
                    _this.LookBackPeriodGroup12 = ko.observable(RequestFormDTO.LookBackPeriodGroup12);
                    _this.IncludeIndexDate1 = ko.observable(RequestFormDTO.IncludeIndexDate1);
                    _this.IncludeIndexDate2 = ko.observable(RequestFormDTO.IncludeIndexDate2);
                    _this.IncludeIndexDate3 = ko.observable(RequestFormDTO.IncludeIndexDate3);
                    _this.IncludeIndexDate4 = ko.observable(RequestFormDTO.IncludeIndexDate4);
                    _this.IncludeIndexDate5 = ko.observable(RequestFormDTO.IncludeIndexDate5);
                    _this.IncludeIndexDate6 = ko.observable(RequestFormDTO.IncludeIndexDate6);
                    _this.IncludeIndexDate7 = ko.observable(RequestFormDTO.IncludeIndexDate7);
                    _this.IncludeIndexDate8 = ko.observable(RequestFormDTO.IncludeIndexDate8);
                    _this.IncludeIndexDate9 = ko.observable(RequestFormDTO.IncludeIndexDate9);
                    _this.IncludeIndexDate10 = ko.observable(RequestFormDTO.IncludeIndexDate10);
                    _this.IncludeIndexDate11 = ko.observable(RequestFormDTO.IncludeIndexDate11);
                    _this.IncludeIndexDate12 = ko.observable(RequestFormDTO.IncludeIndexDate12);
                    _this.StratificationCategories1 = ko.observable(RequestFormDTO.StratificationCategories1);
                    _this.StratificationCategories2 = ko.observable(RequestFormDTO.StratificationCategories2);
                    _this.StratificationCategories3 = ko.observable(RequestFormDTO.StratificationCategories3);
                    _this.StratificationCategories4 = ko.observable(RequestFormDTO.StratificationCategories4);
                    _this.StratificationCategories5 = ko.observable(RequestFormDTO.StratificationCategories5);
                    _this.StratificationCategories6 = ko.observable(RequestFormDTO.StratificationCategories6);
                    _this.StratificationCategories7 = ko.observable(RequestFormDTO.StratificationCategories7);
                    _this.StratificationCategories8 = ko.observable(RequestFormDTO.StratificationCategories8);
                    _this.StratificationCategories9 = ko.observable(RequestFormDTO.StratificationCategories9);
                    _this.StratificationCategories10 = ko.observable(RequestFormDTO.StratificationCategories10);
                    _this.StratificationCategories11 = ko.observable(RequestFormDTO.StratificationCategories11);
                    _this.StratificationCategories12 = ko.observable(RequestFormDTO.StratificationCategories12);
                    _this.TwelveSpecifyLoopBackPeriod1 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod1);
                    _this.TwelveSpecifyLoopBackPeriod2 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod2);
                    _this.TwelveSpecifyLoopBackPeriod3 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod3);
                    _this.TwelveSpecifyLoopBackPeriod4 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod4);
                    _this.TwelveSpecifyLoopBackPeriod5 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod5);
                    _this.TwelveSpecifyLoopBackPeriod6 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod6);
                    _this.TwelveSpecifyLoopBackPeriod7 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod7);
                    _this.TwelveSpecifyLoopBackPeriod8 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod8);
                    _this.TwelveSpecifyLoopBackPeriod9 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod9);
                    _this.TwelveSpecifyLoopBackPeriod10 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod10);
                    _this.TwelveSpecifyLoopBackPeriod11 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod11);
                    _this.TwelveSpecifyLoopBackPeriod12 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod12);
                    _this.TwelveIncludeIndexDate1 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate1);
                    _this.TwelveIncludeIndexDate2 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate2);
                    _this.TwelveIncludeIndexDate3 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate3);
                    _this.TwelveIncludeIndexDate4 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate4);
                    _this.TwelveIncludeIndexDate5 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate5);
                    _this.TwelveIncludeIndexDate6 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate6);
                    _this.TwelveIncludeIndexDate7 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate7);
                    _this.TwelveIncludeIndexDate8 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate8);
                    _this.TwelveIncludeIndexDate9 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate9);
                    _this.TwelveIncludeIndexDate10 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate10);
                    _this.TwelveIncludeIndexDate11 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate11);
                    _this.TwelveIncludeIndexDate12 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate12);
                    _this.CareSettingsToDefineMedicalVisits1 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits1);
                    _this.CareSettingsToDefineMedicalVisits2 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits2);
                    _this.CareSettingsToDefineMedicalVisits3 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits3);
                    _this.CareSettingsToDefineMedicalVisits4 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits4);
                    _this.CareSettingsToDefineMedicalVisits5 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits5);
                    _this.CareSettingsToDefineMedicalVisits6 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits6);
                    _this.CareSettingsToDefineMedicalVisits7 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits7);
                    _this.CareSettingsToDefineMedicalVisits8 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits8);
                    _this.CareSettingsToDefineMedicalVisits9 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits9);
                    _this.CareSettingsToDefineMedicalVisits10 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits10);
                    _this.CareSettingsToDefineMedicalVisits11 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits11);
                    _this.CareSettingsToDefineMedicalVisits12 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits12);
                    _this.TwelveStratificationCategories1 = ko.observable(RequestFormDTO.TwelveStratificationCategories1);
                    _this.TwelveStratificationCategories2 = ko.observable(RequestFormDTO.TwelveStratificationCategories2);
                    _this.TwelveStratificationCategories3 = ko.observable(RequestFormDTO.TwelveStratificationCategories3);
                    _this.TwelveStratificationCategories4 = ko.observable(RequestFormDTO.TwelveStratificationCategories4);
                    _this.TwelveStratificationCategories5 = ko.observable(RequestFormDTO.TwelveStratificationCategories5);
                    _this.TwelveStratificationCategories6 = ko.observable(RequestFormDTO.TwelveStratificationCategories6);
                    _this.TwelveStratificationCategories7 = ko.observable(RequestFormDTO.TwelveStratificationCategories7);
                    _this.TwelveStratificationCategories8 = ko.observable(RequestFormDTO.TwelveStratificationCategories8);
                    _this.TwelveStratificationCategories9 = ko.observable(RequestFormDTO.TwelveStratificationCategories9);
                    _this.TwelveStratificationCategories10 = ko.observable(RequestFormDTO.TwelveStratificationCategories10);
                    _this.TwelveStratificationCategories11 = ko.observable(RequestFormDTO.TwelveStratificationCategories11);
                    _this.TwelveStratificationCategories12 = ko.observable(RequestFormDTO.TwelveStratificationCategories12);
                    _this.VaryLengthOfWashoutPeriod1 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod1);
                    _this.VaryLengthOfWashoutPeriod2 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod2);
                    _this.VaryLengthOfWashoutPeriod3 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod3);
                    _this.VaryLengthOfWashoutPeriod4 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod4);
                    _this.VaryLengthOfWashoutPeriod5 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod5);
                    _this.VaryLengthOfWashoutPeriod6 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod6);
                    _this.VaryLengthOfWashoutPeriod7 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod7);
                    _this.VaryLengthOfWashoutPeriod8 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod8);
                    _this.VaryLengthOfWashoutPeriod9 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod9);
                    _this.VaryLengthOfWashoutPeriod10 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod10);
                    _this.VaryLengthOfWashoutPeriod11 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod11);
                    _this.VaryLengthOfWashoutPeriod12 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod12);
                    _this.VaryUserExposedTime1 = ko.observable(RequestFormDTO.VaryUserExposedTime1);
                    _this.VaryUserExposedTime2 = ko.observable(RequestFormDTO.VaryUserExposedTime2);
                    _this.VaryUserExposedTime3 = ko.observable(RequestFormDTO.VaryUserExposedTime3);
                    _this.VaryUserExposedTime4 = ko.observable(RequestFormDTO.VaryUserExposedTime4);
                    _this.VaryUserExposedTime5 = ko.observable(RequestFormDTO.VaryUserExposedTime5);
                    _this.VaryUserExposedTime6 = ko.observable(RequestFormDTO.VaryUserExposedTime6);
                    _this.VaryUserExposedTime7 = ko.observable(RequestFormDTO.VaryUserExposedTime7);
                    _this.VaryUserExposedTime8 = ko.observable(RequestFormDTO.VaryUserExposedTime8);
                    _this.VaryUserExposedTime9 = ko.observable(RequestFormDTO.VaryUserExposedTime9);
                    _this.VaryUserExposedTime10 = ko.observable(RequestFormDTO.VaryUserExposedTime10);
                    _this.VaryUserExposedTime11 = ko.observable(RequestFormDTO.VaryUserExposedTime11);
                    _this.VaryUserExposedTime12 = ko.observable(RequestFormDTO.VaryUserExposedTime12);
                    _this.VaryUserFollowupPeriodDuration1 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration1);
                    _this.VaryUserFollowupPeriodDuration2 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration2);
                    _this.VaryUserFollowupPeriodDuration3 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration3);
                    _this.VaryUserFollowupPeriodDuration4 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration4);
                    _this.VaryUserFollowupPeriodDuration5 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration5);
                    _this.VaryUserFollowupPeriodDuration6 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration6);
                    _this.VaryUserFollowupPeriodDuration7 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration7);
                    _this.VaryUserFollowupPeriodDuration8 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration8);
                    _this.VaryUserFollowupPeriodDuration9 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration9);
                    _this.VaryUserFollowupPeriodDuration10 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration10);
                    _this.VaryUserFollowupPeriodDuration11 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration11);
                    _this.VaryUserFollowupPeriodDuration12 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration12);
                    _this.VaryBlackoutPeriodPeriod1 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod1);
                    _this.VaryBlackoutPeriodPeriod2 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod2);
                    _this.VaryBlackoutPeriodPeriod3 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod3);
                    _this.VaryBlackoutPeriodPeriod4 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod4);
                    _this.VaryBlackoutPeriodPeriod5 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod5);
                    _this.VaryBlackoutPeriodPeriod6 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod6);
                    _this.VaryBlackoutPeriodPeriod7 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod7);
                    _this.VaryBlackoutPeriodPeriod8 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod8);
                    _this.VaryBlackoutPeriodPeriod9 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod9);
                    _this.VaryBlackoutPeriodPeriod10 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod10);
                    _this.VaryBlackoutPeriodPeriod11 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod11);
                    _this.VaryBlackoutPeriodPeriod12 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod12);
                    _this.Level2or3DefineExposures1Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures1Exposure);
                    _this.Level2or3DefineExposures1Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures1Compare);
                    _this.Level2or3DefineExposures2Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures2Exposure);
                    _this.Level2or3DefineExposures2Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures2Compare);
                    _this.Level2or3DefineExposures3Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures3Exposure);
                    _this.Level2or3DefineExposures3Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures3Compare);
                    _this.Level2or3WashoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod1Exposure);
                    _this.Level2or3WashoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod1Compare);
                    _this.Level2or3WashoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod2Exposure);
                    _this.Level2or3WashoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod2Compare);
                    _this.Level2or3WashoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod3Exposure);
                    _this.Level2or3WashoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod3Compare);
                    _this.Level2or3SpecifyExposedTimeAssessment1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment1Exposure);
                    _this.Level2or3SpecifyExposedTimeAssessment1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment1Compare);
                    _this.Level2or3SpecifyExposedTimeAssessment2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment2Exposure);
                    _this.Level2or3SpecifyExposedTimeAssessment2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment2Compare);
                    _this.Level2or3SpecifyExposedTimeAssessment3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment3Exposure);
                    _this.Level2or3SpecifyExposedTimeAssessment3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment3Compare);
                    _this.Level2or3EpisodeAllowableGap1Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap1Exposure);
                    _this.Level2or3EpisodeAllowableGap1Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap1Compare);
                    _this.Level2or3EpisodeAllowableGap2Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap2Exposure);
                    _this.Level2or3EpisodeAllowableGap2Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap2Compare);
                    _this.Level2or3EpisodeAllowableGap3Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap3Exposure);
                    _this.Level2or3EpisodeAllowableGap3Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap3Compare);
                    _this.Level2or3EpisodeExtensionPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod1Exposure);
                    _this.Level2or3EpisodeExtensionPeriod1Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod1Compare);
                    _this.Level2or3EpisodeExtensionPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod2Exposure);
                    _this.Level2or3EpisodeExtensionPeriod2Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod2Compare);
                    _this.Level2or3EpisodeExtensionPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod3Exposure);
                    _this.Level2or3EpisodeExtensionPeriod3Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod3Compare);
                    _this.Level2or3MinimumEpisodeDuration1Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration1Exposure);
                    _this.Level2or3MinimumEpisodeDuration1Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration1Compare);
                    _this.Level2or3MinimumEpisodeDuration2Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration2Exposure);
                    _this.Level2or3MinimumEpisodeDuration2Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration2Compare);
                    _this.Level2or3MinimumEpisodeDuration3Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration3Exposure);
                    _this.Level2or3MinimumEpisodeDuration3Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration3Compare);
                    _this.Level2or3MinimumDaysSupply1Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply1Exposure);
                    _this.Level2or3MinimumDaysSupply1Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply1Compare);
                    _this.Level2or3MinimumDaysSupply2Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply2Exposure);
                    _this.Level2or3MinimumDaysSupply2Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply2Compare);
                    _this.Level2or3MinimumDaysSupply3Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply3Exposure);
                    _this.Level2or3MinimumDaysSupply3Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply3Compare);
                    _this.Level2or3SpecifyFollowUpDuration1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration1Exposure);
                    _this.Level2or3SpecifyFollowUpDuration1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration1Compare);
                    _this.Level2or3SpecifyFollowUpDuration2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration2Exposure);
                    _this.Level2or3SpecifyFollowUpDuration2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration2Compare);
                    _this.Level2or3SpecifyFollowUpDuration3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration3Exposure);
                    _this.Level2or3SpecifyFollowUpDuration3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration3Compare);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes1Compare);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes2Compare);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure);
                    _this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes3Compare);
                    _this.Level2or3TruncateExposedtime1Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime1Exposure);
                    _this.Level2or3TruncateExposedtime1Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime1Compare);
                    _this.Level2or3TruncateExposedtime2Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime2Exposure);
                    _this.Level2or3TruncateExposedtime2Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime2Compare);
                    _this.Level2or3TruncateExposedtime3Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime3Exposure);
                    _this.Level2or3TruncateExposedtime3Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime3Compare);
                    _this.Level2or3TruncateExposedTimeSpecified1Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified1Exposure);
                    _this.Level2or3TruncateExposedTimeSpecified1Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified1Compare);
                    _this.Level2or3TruncateExposedTimeSpecified2Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified2Exposure);
                    _this.Level2or3TruncateExposedTimeSpecified2Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified2Compare);
                    _this.Level2or3TruncateExposedTimeSpecified3Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified3Exposure);
                    _this.Level2or3TruncateExposedTimeSpecified3Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified3Compare);
                    _this.Level2or3SpecifyBlackoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod1Exposure);
                    _this.Level2or3SpecifyBlackoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod1Compare);
                    _this.Level2or3SpecifyBlackoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod2Exposure);
                    _this.Level2or3SpecifyBlackoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod2Compare);
                    _this.Level2or3SpecifyBlackoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod3Exposure);
                    _this.Level2or3SpecifyBlackoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod3Compare);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62);
                    _this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62);
                    _this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62);
                    _this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62);
                    _this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63);
                    _this.Level2or3VaryLengthOfWashoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod1Exposure);
                    _this.Level2or3VaryLengthOfWashoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod1Compare);
                    _this.Level2or3VaryLengthOfWashoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod2Exposure);
                    _this.Level2or3VaryLengthOfWashoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod2Compare);
                    _this.Level2or3VaryLengthOfWashoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod3Exposure);
                    _this.Level2or3VaryLengthOfWashoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod3Compare);
                    _this.Level2or3VaryUserExposedTime1Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime1Exposure);
                    _this.Level2or3VaryUserExposedTime1Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime1Compare);
                    _this.Level2or3VaryUserExposedTime2Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime2Exposure);
                    _this.Level2or3VaryUserExposedTime2Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime2Compare);
                    _this.Level2or3VaryUserExposedTime3Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime3Exposure);
                    _this.Level2or3VaryUserExposedTime3Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime3Compare);
                    _this.Level2or3VaryBlackoutPeriodPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod1Exposure);
                    _this.Level2or3VaryBlackoutPeriodPeriod1Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod1Compare);
                    _this.Level2or3VaryBlackoutPeriodPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod2Exposure);
                    _this.Level2or3VaryBlackoutPeriodPeriod2Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod2Compare);
                    _this.Level2or3VaryBlackoutPeriodPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod3Exposure);
                    _this.Level2or3VaryBlackoutPeriodPeriod3Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod3Compare);
                    _this.OutcomeList = ko.observableArray(RequestFormDTO.OutcomeList == null ? null : RequestFormDTO.OutcomeList.map(function (item) { return new OutcomeItemViewModel(item); }));
                    _this.AgeCovariate = ko.observable(RequestFormDTO.AgeCovariate);
                    _this.SexCovariate = ko.observable(RequestFormDTO.SexCovariate);
                    _this.TimeCovariate = ko.observable(RequestFormDTO.TimeCovariate);
                    _this.YearCovariate = ko.observable(RequestFormDTO.YearCovariate);
                    _this.ComorbidityCovariate = ko.observable(RequestFormDTO.ComorbidityCovariate);
                    _this.HealthCovariate = ko.observable(RequestFormDTO.HealthCovariate);
                    _this.DrugCovariate = ko.observable(RequestFormDTO.DrugCovariate);
                    _this.CovariateList = ko.observableArray(RequestFormDTO.CovariateList == null ? null : RequestFormDTO.CovariateList.map(function (item) { return new CovariateItemViewModel(item); }));
                    _this.hdPSAnalysis = ko.observable(RequestFormDTO.hdPSAnalysis);
                    _this.InclusionCovariates = ko.observable(RequestFormDTO.InclusionCovariates);
                    _this.PoolCovariates = ko.observable(RequestFormDTO.PoolCovariates);
                    _this.SelectionCovariates = ko.observable(RequestFormDTO.SelectionCovariates);
                    _this.ZeroCellCorrection = ko.observable(RequestFormDTO.ZeroCellCorrection);
                    _this.MatchingRatio = ko.observable(RequestFormDTO.MatchingRatio);
                    _this.MatchingCalipers = ko.observable(RequestFormDTO.MatchingCalipers);
                    _this.VaryMatchingRatio = ko.observable(RequestFormDTO.VaryMatchingRatio);
                    _this.VaryMatchingCalipers = ko.observable(RequestFormDTO.VaryMatchingCalipers);
                }
                return _this;
            }
            RequestFormViewModel.prototype.toData = function () {
                return {
                    RequestDueDate: this.RequestDueDate(),
                    ContactInfo: this.ContactInfo(),
                    RequestingTeam: this.RequestingTeam(),
                    FDAReview: this.FDAReview(),
                    FDADivisionNA: this.FDADivisionNA(),
                    FDADivisionDAAAP: this.FDADivisionDAAAP(),
                    FDADivisionDBRUP: this.FDADivisionDBRUP(),
                    FDADivisionDCARP: this.FDADivisionDCARP(),
                    FDADivisionDDDP: this.FDADivisionDDDP(),
                    FDADivisionDGIEP: this.FDADivisionDGIEP(),
                    FDADivisionDMIP: this.FDADivisionDMIP(),
                    FDADivisionDMEP: this.FDADivisionDMEP(),
                    FDADivisionDNP: this.FDADivisionDNP(),
                    FDADivisionDDP: this.FDADivisionDDP(),
                    FDADivisionDPARP: this.FDADivisionDPARP(),
                    FDADivisionOther: this.FDADivisionOther(),
                    QueryLevel: this.QueryLevel(),
                    AdjustmentMethod: this.AdjustmentMethod(),
                    CohortID: this.CohortID(),
                    StudyObjectives: this.StudyObjectives(),
                    RequestStartDate: this.RequestStartDate(),
                    RequestEndDate: this.RequestEndDate(),
                    AgeGroups: this.AgeGroups(),
                    CoverageTypes: this.CoverageTypes(),
                    EnrollmentGap: this.EnrollmentGap(),
                    EnrollmentExposure: this.EnrollmentExposure(),
                    DefineExposures: this.DefineExposures(),
                    WashoutPeirod: this.WashoutPeirod(),
                    OtherExposures: this.OtherExposures(),
                    OneOrManyExposures: this.OneOrManyExposures(),
                    AdditionalInclusion: this.AdditionalInclusion(),
                    AdditionalInclusionEvaluation: this.AdditionalInclusionEvaluation(),
                    AdditionalExclusion: this.AdditionalExclusion(),
                    AdditionalExclusionEvaluation: this.AdditionalExclusionEvaluation(),
                    VaryWashoutPeirod: this.VaryWashoutPeirod(),
                    VaryExposures: this.VaryExposures(),
                    DefineExposures1: this.DefineExposures1(),
                    DefineExposures2: this.DefineExposures2(),
                    DefineExposures3: this.DefineExposures3(),
                    DefineExposures4: this.DefineExposures4(),
                    DefineExposures5: this.DefineExposures5(),
                    DefineExposures6: this.DefineExposures6(),
                    DefineExposures7: this.DefineExposures7(),
                    DefineExposures8: this.DefineExposures8(),
                    DefineExposures9: this.DefineExposures9(),
                    DefineExposures10: this.DefineExposures10(),
                    DefineExposures11: this.DefineExposures11(),
                    DefineExposures12: this.DefineExposures12(),
                    WashoutPeriod1: this.WashoutPeriod1(),
                    WashoutPeriod2: this.WashoutPeriod2(),
                    WashoutPeriod3: this.WashoutPeriod3(),
                    WashoutPeriod4: this.WashoutPeriod4(),
                    WashoutPeriod5: this.WashoutPeriod5(),
                    WashoutPeriod6: this.WashoutPeriod6(),
                    WashoutPeriod7: this.WashoutPeriod7(),
                    WashoutPeriod8: this.WashoutPeriod8(),
                    WashoutPeriod9: this.WashoutPeriod9(),
                    WashoutPeriod10: this.WashoutPeriod10(),
                    WashoutPeriod11: this.WashoutPeriod11(),
                    WashoutPeriod12: this.WashoutPeriod12(),
                    IncidenceRefinement1: this.IncidenceRefinement1(),
                    IncidenceRefinement2: this.IncidenceRefinement2(),
                    IncidenceRefinement3: this.IncidenceRefinement3(),
                    IncidenceRefinement4: this.IncidenceRefinement4(),
                    IncidenceRefinement5: this.IncidenceRefinement5(),
                    IncidenceRefinement6: this.IncidenceRefinement6(),
                    IncidenceRefinement7: this.IncidenceRefinement7(),
                    IncidenceRefinement8: this.IncidenceRefinement8(),
                    IncidenceRefinement9: this.IncidenceRefinement9(),
                    IncidenceRefinement10: this.IncidenceRefinement10(),
                    IncidenceRefinement11: this.IncidenceRefinement11(),
                    IncidenceRefinement12: this.IncidenceRefinement12(),
                    SpecifyExposedTimeAssessment1: this.SpecifyExposedTimeAssessment1(),
                    SpecifyExposedTimeAssessment2: this.SpecifyExposedTimeAssessment2(),
                    SpecifyExposedTimeAssessment3: this.SpecifyExposedTimeAssessment3(),
                    SpecifyExposedTimeAssessment4: this.SpecifyExposedTimeAssessment4(),
                    SpecifyExposedTimeAssessment5: this.SpecifyExposedTimeAssessment5(),
                    SpecifyExposedTimeAssessment6: this.SpecifyExposedTimeAssessment6(),
                    SpecifyExposedTimeAssessment7: this.SpecifyExposedTimeAssessment7(),
                    SpecifyExposedTimeAssessment8: this.SpecifyExposedTimeAssessment8(),
                    SpecifyExposedTimeAssessment9: this.SpecifyExposedTimeAssessment9(),
                    SpecifyExposedTimeAssessment10: this.SpecifyExposedTimeAssessment10(),
                    SpecifyExposedTimeAssessment11: this.SpecifyExposedTimeAssessment11(),
                    SpecifyExposedTimeAssessment12: this.SpecifyExposedTimeAssessment12(),
                    EpisodeAllowableGap1: this.EpisodeAllowableGap1(),
                    EpisodeAllowableGap2: this.EpisodeAllowableGap2(),
                    EpisodeAllowableGap3: this.EpisodeAllowableGap3(),
                    EpisodeAllowableGap4: this.EpisodeAllowableGap4(),
                    EpisodeAllowableGap5: this.EpisodeAllowableGap5(),
                    EpisodeAllowableGap6: this.EpisodeAllowableGap6(),
                    EpisodeAllowableGap7: this.EpisodeAllowableGap7(),
                    EpisodeAllowableGap8: this.EpisodeAllowableGap8(),
                    EpisodeAllowableGap9: this.EpisodeAllowableGap9(),
                    EpisodeAllowableGap10: this.EpisodeAllowableGap10(),
                    EpisodeAllowableGap11: this.EpisodeAllowableGap11(),
                    EpisodeAllowableGap12: this.EpisodeAllowableGap12(),
                    EpisodeExtensionPeriod1: this.EpisodeExtensionPeriod1(),
                    EpisodeExtensionPeriod2: this.EpisodeExtensionPeriod2(),
                    EpisodeExtensionPeriod3: this.EpisodeExtensionPeriod3(),
                    EpisodeExtensionPeriod4: this.EpisodeExtensionPeriod4(),
                    EpisodeExtensionPeriod5: this.EpisodeExtensionPeriod5(),
                    EpisodeExtensionPeriod6: this.EpisodeExtensionPeriod6(),
                    EpisodeExtensionPeriod7: this.EpisodeExtensionPeriod7(),
                    EpisodeExtensionPeriod8: this.EpisodeExtensionPeriod8(),
                    EpisodeExtensionPeriod9: this.EpisodeExtensionPeriod9(),
                    EpisodeExtensionPeriod10: this.EpisodeExtensionPeriod10(),
                    EpisodeExtensionPeriod11: this.EpisodeExtensionPeriod11(),
                    EpisodeExtensionPeriod12: this.EpisodeExtensionPeriod12(),
                    MinimumEpisodeDuration1: this.MinimumEpisodeDuration1(),
                    MinimumEpisodeDuration2: this.MinimumEpisodeDuration2(),
                    MinimumEpisodeDuration3: this.MinimumEpisodeDuration3(),
                    MinimumEpisodeDuration4: this.MinimumEpisodeDuration4(),
                    MinimumEpisodeDuration5: this.MinimumEpisodeDuration5(),
                    MinimumEpisodeDuration6: this.MinimumEpisodeDuration6(),
                    MinimumEpisodeDuration7: this.MinimumEpisodeDuration7(),
                    MinimumEpisodeDuration8: this.MinimumEpisodeDuration8(),
                    MinimumEpisodeDuration9: this.MinimumEpisodeDuration9(),
                    MinimumEpisodeDuration10: this.MinimumEpisodeDuration10(),
                    MinimumEpisodeDuration11: this.MinimumEpisodeDuration11(),
                    MinimumEpisodeDuration12: this.MinimumEpisodeDuration12(),
                    MinimumDaysSupply1: this.MinimumDaysSupply1(),
                    MinimumDaysSupply2: this.MinimumDaysSupply2(),
                    MinimumDaysSupply3: this.MinimumDaysSupply3(),
                    MinimumDaysSupply4: this.MinimumDaysSupply4(),
                    MinimumDaysSupply5: this.MinimumDaysSupply5(),
                    MinimumDaysSupply6: this.MinimumDaysSupply6(),
                    MinimumDaysSupply7: this.MinimumDaysSupply7(),
                    MinimumDaysSupply8: this.MinimumDaysSupply8(),
                    MinimumDaysSupply9: this.MinimumDaysSupply9(),
                    MinimumDaysSupply10: this.MinimumDaysSupply10(),
                    MinimumDaysSupply11: this.MinimumDaysSupply11(),
                    MinimumDaysSupply12: this.MinimumDaysSupply12(),
                    SpecifyFollowUpDuration1: this.SpecifyFollowUpDuration1(),
                    SpecifyFollowUpDuration2: this.SpecifyFollowUpDuration2(),
                    SpecifyFollowUpDuration3: this.SpecifyFollowUpDuration3(),
                    SpecifyFollowUpDuration4: this.SpecifyFollowUpDuration4(),
                    SpecifyFollowUpDuration5: this.SpecifyFollowUpDuration5(),
                    SpecifyFollowUpDuration6: this.SpecifyFollowUpDuration6(),
                    SpecifyFollowUpDuration7: this.SpecifyFollowUpDuration7(),
                    SpecifyFollowUpDuration8: this.SpecifyFollowUpDuration8(),
                    SpecifyFollowUpDuration9: this.SpecifyFollowUpDuration9(),
                    SpecifyFollowUpDuration10: this.SpecifyFollowUpDuration10(),
                    SpecifyFollowUpDuration11: this.SpecifyFollowUpDuration11(),
                    SpecifyFollowUpDuration12: this.SpecifyFollowUpDuration12(),
                    AllowOnOrMultipleExposureEpisodes1: this.AllowOnOrMultipleExposureEpisodes1(),
                    AllowOnOrMultipleExposureEpisodes2: this.AllowOnOrMultipleExposureEpisodes2(),
                    AllowOnOrMultipleExposureEpisodes3: this.AllowOnOrMultipleExposureEpisodes3(),
                    AllowOnOrMultipleExposureEpisodes4: this.AllowOnOrMultipleExposureEpisodes4(),
                    AllowOnOrMultipleExposureEpisodes5: this.AllowOnOrMultipleExposureEpisodes5(),
                    AllowOnOrMultipleExposureEpisodes6: this.AllowOnOrMultipleExposureEpisodes6(),
                    AllowOnOrMultipleExposureEpisodes7: this.AllowOnOrMultipleExposureEpisodes7(),
                    AllowOnOrMultipleExposureEpisodes8: this.AllowOnOrMultipleExposureEpisodes8(),
                    AllowOnOrMultipleExposureEpisodes9: this.AllowOnOrMultipleExposureEpisodes9(),
                    AllowOnOrMultipleExposureEpisodes10: this.AllowOnOrMultipleExposureEpisodes10(),
                    AllowOnOrMultipleExposureEpisodes11: this.AllowOnOrMultipleExposureEpisodes11(),
                    AllowOnOrMultipleExposureEpisodes12: this.AllowOnOrMultipleExposureEpisodes12(),
                    TruncateExposedtime1: this.TruncateExposedtime1(),
                    TruncateExposedtime2: this.TruncateExposedtime2(),
                    TruncateExposedtime3: this.TruncateExposedtime3(),
                    TruncateExposedtime4: this.TruncateExposedtime4(),
                    TruncateExposedtime5: this.TruncateExposedtime5(),
                    TruncateExposedtime6: this.TruncateExposedtime6(),
                    TruncateExposedtime7: this.TruncateExposedtime7(),
                    TruncateExposedtime8: this.TruncateExposedtime8(),
                    TruncateExposedtime9: this.TruncateExposedtime9(),
                    TruncateExposedtime10: this.TruncateExposedtime10(),
                    TruncateExposedtime11: this.TruncateExposedtime11(),
                    TruncateExposedtime12: this.TruncateExposedtime12(),
                    TruncateExposedTimeSpecified1: this.TruncateExposedTimeSpecified1(),
                    TruncateExposedTimeSpecified2: this.TruncateExposedTimeSpecified2(),
                    TruncateExposedTimeSpecified3: this.TruncateExposedTimeSpecified3(),
                    TruncateExposedTimeSpecified4: this.TruncateExposedTimeSpecified4(),
                    TruncateExposedTimeSpecified5: this.TruncateExposedTimeSpecified5(),
                    TruncateExposedTimeSpecified6: this.TruncateExposedTimeSpecified6(),
                    TruncateExposedTimeSpecified7: this.TruncateExposedTimeSpecified7(),
                    TruncateExposedTimeSpecified8: this.TruncateExposedTimeSpecified8(),
                    TruncateExposedTimeSpecified9: this.TruncateExposedTimeSpecified9(),
                    TruncateExposedTimeSpecified10: this.TruncateExposedTimeSpecified10(),
                    TruncateExposedTimeSpecified11: this.TruncateExposedTimeSpecified11(),
                    TruncateExposedTimeSpecified12: this.TruncateExposedTimeSpecified12(),
                    SpecifyBlackoutPeriod1: this.SpecifyBlackoutPeriod1(),
                    SpecifyBlackoutPeriod2: this.SpecifyBlackoutPeriod2(),
                    SpecifyBlackoutPeriod3: this.SpecifyBlackoutPeriod3(),
                    SpecifyBlackoutPeriod4: this.SpecifyBlackoutPeriod4(),
                    SpecifyBlackoutPeriod5: this.SpecifyBlackoutPeriod5(),
                    SpecifyBlackoutPeriod6: this.SpecifyBlackoutPeriod6(),
                    SpecifyBlackoutPeriod7: this.SpecifyBlackoutPeriod7(),
                    SpecifyBlackoutPeriod8: this.SpecifyBlackoutPeriod8(),
                    SpecifyBlackoutPeriod9: this.SpecifyBlackoutPeriod9(),
                    SpecifyBlackoutPeriod10: this.SpecifyBlackoutPeriod10(),
                    SpecifyBlackoutPeriod11: this.SpecifyBlackoutPeriod11(),
                    SpecifyBlackoutPeriod12: this.SpecifyBlackoutPeriod12(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup11: this.SpecifyAdditionalInclusionInclusionCriteriaGroup11(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup12: this.SpecifyAdditionalInclusionInclusionCriteriaGroup12(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup13: this.SpecifyAdditionalInclusionInclusionCriteriaGroup13(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup14: this.SpecifyAdditionalInclusionInclusionCriteriaGroup14(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup15: this.SpecifyAdditionalInclusionInclusionCriteriaGroup15(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup16: this.SpecifyAdditionalInclusionInclusionCriteriaGroup16(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup11: this.SpecifyAdditionalInclusionEvaluationWindowGroup11(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup12: this.SpecifyAdditionalInclusionEvaluationWindowGroup12(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup13: this.SpecifyAdditionalInclusionEvaluationWindowGroup13(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup14: this.SpecifyAdditionalInclusionEvaluationWindowGroup14(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup15: this.SpecifyAdditionalInclusionEvaluationWindowGroup15(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup16: this.SpecifyAdditionalInclusionEvaluationWindowGroup16(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup21: this.SpecifyAdditionalInclusionInclusionCriteriaGroup21(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup22: this.SpecifyAdditionalInclusionInclusionCriteriaGroup22(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup23: this.SpecifyAdditionalInclusionInclusionCriteriaGroup23(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup24: this.SpecifyAdditionalInclusionInclusionCriteriaGroup24(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup25: this.SpecifyAdditionalInclusionInclusionCriteriaGroup25(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup26: this.SpecifyAdditionalInclusionInclusionCriteriaGroup26(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup21: this.SpecifyAdditionalInclusionEvaluationWindowGroup21(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup22: this.SpecifyAdditionalInclusionEvaluationWindowGroup22(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup23: this.SpecifyAdditionalInclusionEvaluationWindowGroup23(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup24: this.SpecifyAdditionalInclusionEvaluationWindowGroup24(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup25: this.SpecifyAdditionalInclusionEvaluationWindowGroup25(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup26: this.SpecifyAdditionalInclusionEvaluationWindowGroup26(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup31: this.SpecifyAdditionalInclusionInclusionCriteriaGroup31(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup32: this.SpecifyAdditionalInclusionInclusionCriteriaGroup32(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup33: this.SpecifyAdditionalInclusionInclusionCriteriaGroup33(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup34: this.SpecifyAdditionalInclusionInclusionCriteriaGroup34(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup35: this.SpecifyAdditionalInclusionInclusionCriteriaGroup35(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup36: this.SpecifyAdditionalInclusionInclusionCriteriaGroup36(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup31: this.SpecifyAdditionalInclusionEvaluationWindowGroup31(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup32: this.SpecifyAdditionalInclusionEvaluationWindowGroup32(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup33: this.SpecifyAdditionalInclusionEvaluationWindowGroup33(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup34: this.SpecifyAdditionalInclusionEvaluationWindowGroup34(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup35: this.SpecifyAdditionalInclusionEvaluationWindowGroup35(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup36: this.SpecifyAdditionalInclusionEvaluationWindowGroup36(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup41: this.SpecifyAdditionalInclusionInclusionCriteriaGroup41(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup42: this.SpecifyAdditionalInclusionInclusionCriteriaGroup42(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup43: this.SpecifyAdditionalInclusionInclusionCriteriaGroup43(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup44: this.SpecifyAdditionalInclusionInclusionCriteriaGroup44(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup45: this.SpecifyAdditionalInclusionInclusionCriteriaGroup45(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup46: this.SpecifyAdditionalInclusionInclusionCriteriaGroup46(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup41: this.SpecifyAdditionalInclusionEvaluationWindowGroup41(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup42: this.SpecifyAdditionalInclusionEvaluationWindowGroup42(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup43: this.SpecifyAdditionalInclusionEvaluationWindowGroup43(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup44: this.SpecifyAdditionalInclusionEvaluationWindowGroup44(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup45: this.SpecifyAdditionalInclusionEvaluationWindowGroup45(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup46: this.SpecifyAdditionalInclusionEvaluationWindowGroup46(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup51: this.SpecifyAdditionalInclusionInclusionCriteriaGroup51(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup52: this.SpecifyAdditionalInclusionInclusionCriteriaGroup52(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup53: this.SpecifyAdditionalInclusionInclusionCriteriaGroup53(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup54: this.SpecifyAdditionalInclusionInclusionCriteriaGroup54(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup55: this.SpecifyAdditionalInclusionInclusionCriteriaGroup55(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup56: this.SpecifyAdditionalInclusionInclusionCriteriaGroup56(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup51: this.SpecifyAdditionalInclusionEvaluationWindowGroup51(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup52: this.SpecifyAdditionalInclusionEvaluationWindowGroup52(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup53: this.SpecifyAdditionalInclusionEvaluationWindowGroup53(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup54: this.SpecifyAdditionalInclusionEvaluationWindowGroup54(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup55: this.SpecifyAdditionalInclusionEvaluationWindowGroup55(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup56: this.SpecifyAdditionalInclusionEvaluationWindowGroup56(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup61: this.SpecifyAdditionalInclusionInclusionCriteriaGroup61(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup62: this.SpecifyAdditionalInclusionInclusionCriteriaGroup62(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup63: this.SpecifyAdditionalInclusionInclusionCriteriaGroup63(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup64: this.SpecifyAdditionalInclusionInclusionCriteriaGroup64(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup65: this.SpecifyAdditionalInclusionInclusionCriteriaGroup65(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup66: this.SpecifyAdditionalInclusionInclusionCriteriaGroup66(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup61: this.SpecifyAdditionalInclusionEvaluationWindowGroup61(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup62: this.SpecifyAdditionalInclusionEvaluationWindowGroup62(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup63: this.SpecifyAdditionalInclusionEvaluationWindowGroup63(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup64: this.SpecifyAdditionalInclusionEvaluationWindowGroup64(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup65: this.SpecifyAdditionalInclusionEvaluationWindowGroup65(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup66: this.SpecifyAdditionalInclusionEvaluationWindowGroup66(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup71: this.SpecifyAdditionalInclusionInclusionCriteriaGroup71(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup72: this.SpecifyAdditionalInclusionInclusionCriteriaGroup72(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup73: this.SpecifyAdditionalInclusionInclusionCriteriaGroup73(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup74: this.SpecifyAdditionalInclusionInclusionCriteriaGroup74(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup75: this.SpecifyAdditionalInclusionInclusionCriteriaGroup75(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup76: this.SpecifyAdditionalInclusionInclusionCriteriaGroup76(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup71: this.SpecifyAdditionalInclusionEvaluationWindowGroup71(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup72: this.SpecifyAdditionalInclusionEvaluationWindowGroup72(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup73: this.SpecifyAdditionalInclusionEvaluationWindowGroup73(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup74: this.SpecifyAdditionalInclusionEvaluationWindowGroup74(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup75: this.SpecifyAdditionalInclusionEvaluationWindowGroup75(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup76: this.SpecifyAdditionalInclusionEvaluationWindowGroup76(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup81: this.SpecifyAdditionalInclusionInclusionCriteriaGroup81(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup82: this.SpecifyAdditionalInclusionInclusionCriteriaGroup82(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup83: this.SpecifyAdditionalInclusionInclusionCriteriaGroup83(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup84: this.SpecifyAdditionalInclusionInclusionCriteriaGroup84(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup85: this.SpecifyAdditionalInclusionInclusionCriteriaGroup85(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup86: this.SpecifyAdditionalInclusionInclusionCriteriaGroup86(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup81: this.SpecifyAdditionalInclusionEvaluationWindowGroup81(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup82: this.SpecifyAdditionalInclusionEvaluationWindowGroup82(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup83: this.SpecifyAdditionalInclusionEvaluationWindowGroup83(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup84: this.SpecifyAdditionalInclusionEvaluationWindowGroup84(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup85: this.SpecifyAdditionalInclusionEvaluationWindowGroup85(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup86: this.SpecifyAdditionalInclusionEvaluationWindowGroup86(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup91: this.SpecifyAdditionalInclusionInclusionCriteriaGroup91(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup92: this.SpecifyAdditionalInclusionInclusionCriteriaGroup92(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup93: this.SpecifyAdditionalInclusionInclusionCriteriaGroup93(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup94: this.SpecifyAdditionalInclusionInclusionCriteriaGroup94(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup95: this.SpecifyAdditionalInclusionInclusionCriteriaGroup95(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup96: this.SpecifyAdditionalInclusionInclusionCriteriaGroup96(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup91: this.SpecifyAdditionalInclusionEvaluationWindowGroup91(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup92: this.SpecifyAdditionalInclusionEvaluationWindowGroup92(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup93: this.SpecifyAdditionalInclusionEvaluationWindowGroup93(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup94: this.SpecifyAdditionalInclusionEvaluationWindowGroup94(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup95: this.SpecifyAdditionalInclusionEvaluationWindowGroup95(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup96: this.SpecifyAdditionalInclusionEvaluationWindowGroup96(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup101: this.SpecifyAdditionalInclusionInclusionCriteriaGroup101(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup102: this.SpecifyAdditionalInclusionInclusionCriteriaGroup102(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup103: this.SpecifyAdditionalInclusionInclusionCriteriaGroup103(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup104: this.SpecifyAdditionalInclusionInclusionCriteriaGroup104(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup105: this.SpecifyAdditionalInclusionInclusionCriteriaGroup105(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup106: this.SpecifyAdditionalInclusionInclusionCriteriaGroup106(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup101: this.SpecifyAdditionalInclusionEvaluationWindowGroup101(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup102: this.SpecifyAdditionalInclusionEvaluationWindowGroup102(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup103: this.SpecifyAdditionalInclusionEvaluationWindowGroup103(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup104: this.SpecifyAdditionalInclusionEvaluationWindowGroup104(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup105: this.SpecifyAdditionalInclusionEvaluationWindowGroup105(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup106: this.SpecifyAdditionalInclusionEvaluationWindowGroup106(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup111: this.SpecifyAdditionalInclusionInclusionCriteriaGroup111(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup112: this.SpecifyAdditionalInclusionInclusionCriteriaGroup112(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup113: this.SpecifyAdditionalInclusionInclusionCriteriaGroup113(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup114: this.SpecifyAdditionalInclusionInclusionCriteriaGroup114(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup115: this.SpecifyAdditionalInclusionInclusionCriteriaGroup115(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup116: this.SpecifyAdditionalInclusionInclusionCriteriaGroup116(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup111: this.SpecifyAdditionalInclusionEvaluationWindowGroup111(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup112: this.SpecifyAdditionalInclusionEvaluationWindowGroup112(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup113: this.SpecifyAdditionalInclusionEvaluationWindowGroup113(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup114: this.SpecifyAdditionalInclusionEvaluationWindowGroup114(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup115: this.SpecifyAdditionalInclusionEvaluationWindowGroup115(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup116: this.SpecifyAdditionalInclusionEvaluationWindowGroup116(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup121: this.SpecifyAdditionalInclusionInclusionCriteriaGroup121(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup122: this.SpecifyAdditionalInclusionInclusionCriteriaGroup122(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup123: this.SpecifyAdditionalInclusionInclusionCriteriaGroup123(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup124: this.SpecifyAdditionalInclusionInclusionCriteriaGroup124(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup125: this.SpecifyAdditionalInclusionInclusionCriteriaGroup125(),
                    SpecifyAdditionalInclusionInclusionCriteriaGroup126: this.SpecifyAdditionalInclusionInclusionCriteriaGroup126(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup121: this.SpecifyAdditionalInclusionEvaluationWindowGroup121(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup122: this.SpecifyAdditionalInclusionEvaluationWindowGroup122(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup123: this.SpecifyAdditionalInclusionEvaluationWindowGroup123(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup124: this.SpecifyAdditionalInclusionEvaluationWindowGroup124(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup125: this.SpecifyAdditionalInclusionEvaluationWindowGroup125(),
                    SpecifyAdditionalInclusionEvaluationWindowGroup126: this.SpecifyAdditionalInclusionEvaluationWindowGroup126(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup11: this.SpecifyAdditionalExclusionInclusionCriteriaGroup11(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup12: this.SpecifyAdditionalExclusionInclusionCriteriaGroup12(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup13: this.SpecifyAdditionalExclusionInclusionCriteriaGroup13(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup14: this.SpecifyAdditionalExclusionInclusionCriteriaGroup14(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup15: this.SpecifyAdditionalExclusionInclusionCriteriaGroup15(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup16: this.SpecifyAdditionalExclusionInclusionCriteriaGroup16(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup11: this.SpecifyAdditionalExclusionEvaluationWindowGroup11(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup12: this.SpecifyAdditionalExclusionEvaluationWindowGroup12(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup13: this.SpecifyAdditionalExclusionEvaluationWindowGroup13(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup14: this.SpecifyAdditionalExclusionEvaluationWindowGroup14(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup15: this.SpecifyAdditionalExclusionEvaluationWindowGroup15(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup16: this.SpecifyAdditionalExclusionEvaluationWindowGroup16(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup21: this.SpecifyAdditionalExclusionInclusionCriteriaGroup21(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup22: this.SpecifyAdditionalExclusionInclusionCriteriaGroup22(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup23: this.SpecifyAdditionalExclusionInclusionCriteriaGroup23(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup24: this.SpecifyAdditionalExclusionInclusionCriteriaGroup24(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup25: this.SpecifyAdditionalExclusionInclusionCriteriaGroup25(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup26: this.SpecifyAdditionalExclusionInclusionCriteriaGroup26(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup21: this.SpecifyAdditionalExclusionEvaluationWindowGroup21(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup22: this.SpecifyAdditionalExclusionEvaluationWindowGroup22(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup23: this.SpecifyAdditionalExclusionEvaluationWindowGroup23(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup24: this.SpecifyAdditionalExclusionEvaluationWindowGroup24(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup25: this.SpecifyAdditionalExclusionEvaluationWindowGroup25(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup26: this.SpecifyAdditionalExclusionEvaluationWindowGroup26(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup31: this.SpecifyAdditionalExclusionInclusionCriteriaGroup31(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup32: this.SpecifyAdditionalExclusionInclusionCriteriaGroup32(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup33: this.SpecifyAdditionalExclusionInclusionCriteriaGroup33(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup34: this.SpecifyAdditionalExclusionInclusionCriteriaGroup34(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup35: this.SpecifyAdditionalExclusionInclusionCriteriaGroup35(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup36: this.SpecifyAdditionalExclusionInclusionCriteriaGroup36(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup31: this.SpecifyAdditionalExclusionEvaluationWindowGroup31(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup32: this.SpecifyAdditionalExclusionEvaluationWindowGroup32(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup33: this.SpecifyAdditionalExclusionEvaluationWindowGroup33(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup34: this.SpecifyAdditionalExclusionEvaluationWindowGroup34(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup35: this.SpecifyAdditionalExclusionEvaluationWindowGroup35(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup36: this.SpecifyAdditionalExclusionEvaluationWindowGroup36(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup41: this.SpecifyAdditionalExclusionInclusionCriteriaGroup41(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup42: this.SpecifyAdditionalExclusionInclusionCriteriaGroup42(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup43: this.SpecifyAdditionalExclusionInclusionCriteriaGroup43(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup44: this.SpecifyAdditionalExclusionInclusionCriteriaGroup44(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup45: this.SpecifyAdditionalExclusionInclusionCriteriaGroup45(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup46: this.SpecifyAdditionalExclusionInclusionCriteriaGroup46(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup41: this.SpecifyAdditionalExclusionEvaluationWindowGroup41(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup42: this.SpecifyAdditionalExclusionEvaluationWindowGroup42(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup43: this.SpecifyAdditionalExclusionEvaluationWindowGroup43(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup44: this.SpecifyAdditionalExclusionEvaluationWindowGroup44(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup45: this.SpecifyAdditionalExclusionEvaluationWindowGroup45(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup46: this.SpecifyAdditionalExclusionEvaluationWindowGroup46(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup51: this.SpecifyAdditionalExclusionInclusionCriteriaGroup51(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup52: this.SpecifyAdditionalExclusionInclusionCriteriaGroup52(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup53: this.SpecifyAdditionalExclusionInclusionCriteriaGroup53(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup54: this.SpecifyAdditionalExclusionInclusionCriteriaGroup54(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup55: this.SpecifyAdditionalExclusionInclusionCriteriaGroup55(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup56: this.SpecifyAdditionalExclusionInclusionCriteriaGroup56(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup51: this.SpecifyAdditionalExclusionEvaluationWindowGroup51(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup52: this.SpecifyAdditionalExclusionEvaluationWindowGroup52(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup53: this.SpecifyAdditionalExclusionEvaluationWindowGroup53(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup54: this.SpecifyAdditionalExclusionEvaluationWindowGroup54(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup55: this.SpecifyAdditionalExclusionEvaluationWindowGroup55(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup56: this.SpecifyAdditionalExclusionEvaluationWindowGroup56(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup61: this.SpecifyAdditionalExclusionInclusionCriteriaGroup61(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup62: this.SpecifyAdditionalExclusionInclusionCriteriaGroup62(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup63: this.SpecifyAdditionalExclusionInclusionCriteriaGroup63(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup64: this.SpecifyAdditionalExclusionInclusionCriteriaGroup64(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup65: this.SpecifyAdditionalExclusionInclusionCriteriaGroup65(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup66: this.SpecifyAdditionalExclusionInclusionCriteriaGroup66(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup61: this.SpecifyAdditionalExclusionEvaluationWindowGroup61(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup62: this.SpecifyAdditionalExclusionEvaluationWindowGroup62(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup63: this.SpecifyAdditionalExclusionEvaluationWindowGroup63(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup64: this.SpecifyAdditionalExclusionEvaluationWindowGroup64(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup65: this.SpecifyAdditionalExclusionEvaluationWindowGroup65(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup66: this.SpecifyAdditionalExclusionEvaluationWindowGroup66(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup71: this.SpecifyAdditionalExclusionInclusionCriteriaGroup71(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup72: this.SpecifyAdditionalExclusionInclusionCriteriaGroup72(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup73: this.SpecifyAdditionalExclusionInclusionCriteriaGroup73(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup74: this.SpecifyAdditionalExclusionInclusionCriteriaGroup74(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup75: this.SpecifyAdditionalExclusionInclusionCriteriaGroup75(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup76: this.SpecifyAdditionalExclusionInclusionCriteriaGroup76(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup71: this.SpecifyAdditionalExclusionEvaluationWindowGroup71(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup72: this.SpecifyAdditionalExclusionEvaluationWindowGroup72(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup73: this.SpecifyAdditionalExclusionEvaluationWindowGroup73(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup74: this.SpecifyAdditionalExclusionEvaluationWindowGroup74(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup75: this.SpecifyAdditionalExclusionEvaluationWindowGroup75(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup76: this.SpecifyAdditionalExclusionEvaluationWindowGroup76(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup81: this.SpecifyAdditionalExclusionInclusionCriteriaGroup81(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup82: this.SpecifyAdditionalExclusionInclusionCriteriaGroup82(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup83: this.SpecifyAdditionalExclusionInclusionCriteriaGroup83(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup84: this.SpecifyAdditionalExclusionInclusionCriteriaGroup84(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup85: this.SpecifyAdditionalExclusionInclusionCriteriaGroup85(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup86: this.SpecifyAdditionalExclusionInclusionCriteriaGroup86(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup81: this.SpecifyAdditionalExclusionEvaluationWindowGroup81(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup82: this.SpecifyAdditionalExclusionEvaluationWindowGroup82(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup83: this.SpecifyAdditionalExclusionEvaluationWindowGroup83(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup84: this.SpecifyAdditionalExclusionEvaluationWindowGroup84(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup85: this.SpecifyAdditionalExclusionEvaluationWindowGroup85(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup86: this.SpecifyAdditionalExclusionEvaluationWindowGroup86(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup91: this.SpecifyAdditionalExclusionInclusionCriteriaGroup91(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup92: this.SpecifyAdditionalExclusionInclusionCriteriaGroup92(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup93: this.SpecifyAdditionalExclusionInclusionCriteriaGroup93(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup94: this.SpecifyAdditionalExclusionInclusionCriteriaGroup94(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup95: this.SpecifyAdditionalExclusionInclusionCriteriaGroup95(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup96: this.SpecifyAdditionalExclusionInclusionCriteriaGroup96(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup91: this.SpecifyAdditionalExclusionEvaluationWindowGroup91(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup92: this.SpecifyAdditionalExclusionEvaluationWindowGroup92(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup93: this.SpecifyAdditionalExclusionEvaluationWindowGroup93(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup94: this.SpecifyAdditionalExclusionEvaluationWindowGroup94(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup95: this.SpecifyAdditionalExclusionEvaluationWindowGroup95(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup96: this.SpecifyAdditionalExclusionEvaluationWindowGroup96(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup101: this.SpecifyAdditionalExclusionInclusionCriteriaGroup101(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup102: this.SpecifyAdditionalExclusionInclusionCriteriaGroup102(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup103: this.SpecifyAdditionalExclusionInclusionCriteriaGroup103(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup104: this.SpecifyAdditionalExclusionInclusionCriteriaGroup104(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup105: this.SpecifyAdditionalExclusionInclusionCriteriaGroup105(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup106: this.SpecifyAdditionalExclusionInclusionCriteriaGroup106(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup101: this.SpecifyAdditionalExclusionEvaluationWindowGroup101(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup102: this.SpecifyAdditionalExclusionEvaluationWindowGroup102(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup103: this.SpecifyAdditionalExclusionEvaluationWindowGroup103(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup104: this.SpecifyAdditionalExclusionEvaluationWindowGroup104(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup105: this.SpecifyAdditionalExclusionEvaluationWindowGroup105(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup106: this.SpecifyAdditionalExclusionEvaluationWindowGroup106(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup111: this.SpecifyAdditionalExclusionInclusionCriteriaGroup111(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup112: this.SpecifyAdditionalExclusionInclusionCriteriaGroup112(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup113: this.SpecifyAdditionalExclusionInclusionCriteriaGroup113(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup114: this.SpecifyAdditionalExclusionInclusionCriteriaGroup114(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup115: this.SpecifyAdditionalExclusionInclusionCriteriaGroup115(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup116: this.SpecifyAdditionalExclusionInclusionCriteriaGroup116(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup111: this.SpecifyAdditionalExclusionEvaluationWindowGroup111(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup112: this.SpecifyAdditionalExclusionEvaluationWindowGroup112(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup113: this.SpecifyAdditionalExclusionEvaluationWindowGroup113(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup114: this.SpecifyAdditionalExclusionEvaluationWindowGroup114(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup115: this.SpecifyAdditionalExclusionEvaluationWindowGroup115(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup116: this.SpecifyAdditionalExclusionEvaluationWindowGroup116(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup121: this.SpecifyAdditionalExclusionInclusionCriteriaGroup121(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup122: this.SpecifyAdditionalExclusionInclusionCriteriaGroup122(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup123: this.SpecifyAdditionalExclusionInclusionCriteriaGroup123(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup124: this.SpecifyAdditionalExclusionInclusionCriteriaGroup124(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup125: this.SpecifyAdditionalExclusionInclusionCriteriaGroup125(),
                    SpecifyAdditionalExclusionInclusionCriteriaGroup126: this.SpecifyAdditionalExclusionInclusionCriteriaGroup126(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup121: this.SpecifyAdditionalExclusionEvaluationWindowGroup121(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup122: this.SpecifyAdditionalExclusionEvaluationWindowGroup122(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup123: this.SpecifyAdditionalExclusionEvaluationWindowGroup123(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup124: this.SpecifyAdditionalExclusionEvaluationWindowGroup124(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup125: this.SpecifyAdditionalExclusionEvaluationWindowGroup125(),
                    SpecifyAdditionalExclusionEvaluationWindowGroup126: this.SpecifyAdditionalExclusionEvaluationWindowGroup126(),
                    LookBackPeriodGroup1: this.LookBackPeriodGroup1(),
                    LookBackPeriodGroup2: this.LookBackPeriodGroup2(),
                    LookBackPeriodGroup3: this.LookBackPeriodGroup3(),
                    LookBackPeriodGroup4: this.LookBackPeriodGroup4(),
                    LookBackPeriodGroup5: this.LookBackPeriodGroup5(),
                    LookBackPeriodGroup6: this.LookBackPeriodGroup6(),
                    LookBackPeriodGroup7: this.LookBackPeriodGroup7(),
                    LookBackPeriodGroup8: this.LookBackPeriodGroup8(),
                    LookBackPeriodGroup9: this.LookBackPeriodGroup9(),
                    LookBackPeriodGroup10: this.LookBackPeriodGroup10(),
                    LookBackPeriodGroup11: this.LookBackPeriodGroup11(),
                    LookBackPeriodGroup12: this.LookBackPeriodGroup12(),
                    IncludeIndexDate1: this.IncludeIndexDate1(),
                    IncludeIndexDate2: this.IncludeIndexDate2(),
                    IncludeIndexDate3: this.IncludeIndexDate3(),
                    IncludeIndexDate4: this.IncludeIndexDate4(),
                    IncludeIndexDate5: this.IncludeIndexDate5(),
                    IncludeIndexDate6: this.IncludeIndexDate6(),
                    IncludeIndexDate7: this.IncludeIndexDate7(),
                    IncludeIndexDate8: this.IncludeIndexDate8(),
                    IncludeIndexDate9: this.IncludeIndexDate9(),
                    IncludeIndexDate10: this.IncludeIndexDate10(),
                    IncludeIndexDate11: this.IncludeIndexDate11(),
                    IncludeIndexDate12: this.IncludeIndexDate12(),
                    StratificationCategories1: this.StratificationCategories1(),
                    StratificationCategories2: this.StratificationCategories2(),
                    StratificationCategories3: this.StratificationCategories3(),
                    StratificationCategories4: this.StratificationCategories4(),
                    StratificationCategories5: this.StratificationCategories5(),
                    StratificationCategories6: this.StratificationCategories6(),
                    StratificationCategories7: this.StratificationCategories7(),
                    StratificationCategories8: this.StratificationCategories8(),
                    StratificationCategories9: this.StratificationCategories9(),
                    StratificationCategories10: this.StratificationCategories10(),
                    StratificationCategories11: this.StratificationCategories11(),
                    StratificationCategories12: this.StratificationCategories12(),
                    TwelveSpecifyLoopBackPeriod1: this.TwelveSpecifyLoopBackPeriod1(),
                    TwelveSpecifyLoopBackPeriod2: this.TwelveSpecifyLoopBackPeriod2(),
                    TwelveSpecifyLoopBackPeriod3: this.TwelveSpecifyLoopBackPeriod3(),
                    TwelveSpecifyLoopBackPeriod4: this.TwelveSpecifyLoopBackPeriod4(),
                    TwelveSpecifyLoopBackPeriod5: this.TwelveSpecifyLoopBackPeriod5(),
                    TwelveSpecifyLoopBackPeriod6: this.TwelveSpecifyLoopBackPeriod6(),
                    TwelveSpecifyLoopBackPeriod7: this.TwelveSpecifyLoopBackPeriod7(),
                    TwelveSpecifyLoopBackPeriod8: this.TwelveSpecifyLoopBackPeriod8(),
                    TwelveSpecifyLoopBackPeriod9: this.TwelveSpecifyLoopBackPeriod9(),
                    TwelveSpecifyLoopBackPeriod10: this.TwelveSpecifyLoopBackPeriod10(),
                    TwelveSpecifyLoopBackPeriod11: this.TwelveSpecifyLoopBackPeriod11(),
                    TwelveSpecifyLoopBackPeriod12: this.TwelveSpecifyLoopBackPeriod12(),
                    TwelveIncludeIndexDate1: this.TwelveIncludeIndexDate1(),
                    TwelveIncludeIndexDate2: this.TwelveIncludeIndexDate2(),
                    TwelveIncludeIndexDate3: this.TwelveIncludeIndexDate3(),
                    TwelveIncludeIndexDate4: this.TwelveIncludeIndexDate4(),
                    TwelveIncludeIndexDate5: this.TwelveIncludeIndexDate5(),
                    TwelveIncludeIndexDate6: this.TwelveIncludeIndexDate6(),
                    TwelveIncludeIndexDate7: this.TwelveIncludeIndexDate7(),
                    TwelveIncludeIndexDate8: this.TwelveIncludeIndexDate8(),
                    TwelveIncludeIndexDate9: this.TwelveIncludeIndexDate9(),
                    TwelveIncludeIndexDate10: this.TwelveIncludeIndexDate10(),
                    TwelveIncludeIndexDate11: this.TwelveIncludeIndexDate11(),
                    TwelveIncludeIndexDate12: this.TwelveIncludeIndexDate12(),
                    CareSettingsToDefineMedicalVisits1: this.CareSettingsToDefineMedicalVisits1(),
                    CareSettingsToDefineMedicalVisits2: this.CareSettingsToDefineMedicalVisits2(),
                    CareSettingsToDefineMedicalVisits3: this.CareSettingsToDefineMedicalVisits3(),
                    CareSettingsToDefineMedicalVisits4: this.CareSettingsToDefineMedicalVisits4(),
                    CareSettingsToDefineMedicalVisits5: this.CareSettingsToDefineMedicalVisits5(),
                    CareSettingsToDefineMedicalVisits6: this.CareSettingsToDefineMedicalVisits6(),
                    CareSettingsToDefineMedicalVisits7: this.CareSettingsToDefineMedicalVisits7(),
                    CareSettingsToDefineMedicalVisits8: this.CareSettingsToDefineMedicalVisits8(),
                    CareSettingsToDefineMedicalVisits9: this.CareSettingsToDefineMedicalVisits9(),
                    CareSettingsToDefineMedicalVisits10: this.CareSettingsToDefineMedicalVisits10(),
                    CareSettingsToDefineMedicalVisits11: this.CareSettingsToDefineMedicalVisits11(),
                    CareSettingsToDefineMedicalVisits12: this.CareSettingsToDefineMedicalVisits12(),
                    TwelveStratificationCategories1: this.TwelveStratificationCategories1(),
                    TwelveStratificationCategories2: this.TwelveStratificationCategories2(),
                    TwelveStratificationCategories3: this.TwelveStratificationCategories3(),
                    TwelveStratificationCategories4: this.TwelveStratificationCategories4(),
                    TwelveStratificationCategories5: this.TwelveStratificationCategories5(),
                    TwelveStratificationCategories6: this.TwelveStratificationCategories6(),
                    TwelveStratificationCategories7: this.TwelveStratificationCategories7(),
                    TwelveStratificationCategories8: this.TwelveStratificationCategories8(),
                    TwelveStratificationCategories9: this.TwelveStratificationCategories9(),
                    TwelveStratificationCategories10: this.TwelveStratificationCategories10(),
                    TwelveStratificationCategories11: this.TwelveStratificationCategories11(),
                    TwelveStratificationCategories12: this.TwelveStratificationCategories12(),
                    VaryLengthOfWashoutPeriod1: this.VaryLengthOfWashoutPeriod1(),
                    VaryLengthOfWashoutPeriod2: this.VaryLengthOfWashoutPeriod2(),
                    VaryLengthOfWashoutPeriod3: this.VaryLengthOfWashoutPeriod3(),
                    VaryLengthOfWashoutPeriod4: this.VaryLengthOfWashoutPeriod4(),
                    VaryLengthOfWashoutPeriod5: this.VaryLengthOfWashoutPeriod5(),
                    VaryLengthOfWashoutPeriod6: this.VaryLengthOfWashoutPeriod6(),
                    VaryLengthOfWashoutPeriod7: this.VaryLengthOfWashoutPeriod7(),
                    VaryLengthOfWashoutPeriod8: this.VaryLengthOfWashoutPeriod8(),
                    VaryLengthOfWashoutPeriod9: this.VaryLengthOfWashoutPeriod9(),
                    VaryLengthOfWashoutPeriod10: this.VaryLengthOfWashoutPeriod10(),
                    VaryLengthOfWashoutPeriod11: this.VaryLengthOfWashoutPeriod11(),
                    VaryLengthOfWashoutPeriod12: this.VaryLengthOfWashoutPeriod12(),
                    VaryUserExposedTime1: this.VaryUserExposedTime1(),
                    VaryUserExposedTime2: this.VaryUserExposedTime2(),
                    VaryUserExposedTime3: this.VaryUserExposedTime3(),
                    VaryUserExposedTime4: this.VaryUserExposedTime4(),
                    VaryUserExposedTime5: this.VaryUserExposedTime5(),
                    VaryUserExposedTime6: this.VaryUserExposedTime6(),
                    VaryUserExposedTime7: this.VaryUserExposedTime7(),
                    VaryUserExposedTime8: this.VaryUserExposedTime8(),
                    VaryUserExposedTime9: this.VaryUserExposedTime9(),
                    VaryUserExposedTime10: this.VaryUserExposedTime10(),
                    VaryUserExposedTime11: this.VaryUserExposedTime11(),
                    VaryUserExposedTime12: this.VaryUserExposedTime12(),
                    VaryUserFollowupPeriodDuration1: this.VaryUserFollowupPeriodDuration1(),
                    VaryUserFollowupPeriodDuration2: this.VaryUserFollowupPeriodDuration2(),
                    VaryUserFollowupPeriodDuration3: this.VaryUserFollowupPeriodDuration3(),
                    VaryUserFollowupPeriodDuration4: this.VaryUserFollowupPeriodDuration4(),
                    VaryUserFollowupPeriodDuration5: this.VaryUserFollowupPeriodDuration5(),
                    VaryUserFollowupPeriodDuration6: this.VaryUserFollowupPeriodDuration6(),
                    VaryUserFollowupPeriodDuration7: this.VaryUserFollowupPeriodDuration7(),
                    VaryUserFollowupPeriodDuration8: this.VaryUserFollowupPeriodDuration8(),
                    VaryUserFollowupPeriodDuration9: this.VaryUserFollowupPeriodDuration9(),
                    VaryUserFollowupPeriodDuration10: this.VaryUserFollowupPeriodDuration10(),
                    VaryUserFollowupPeriodDuration11: this.VaryUserFollowupPeriodDuration11(),
                    VaryUserFollowupPeriodDuration12: this.VaryUserFollowupPeriodDuration12(),
                    VaryBlackoutPeriodPeriod1: this.VaryBlackoutPeriodPeriod1(),
                    VaryBlackoutPeriodPeriod2: this.VaryBlackoutPeriodPeriod2(),
                    VaryBlackoutPeriodPeriod3: this.VaryBlackoutPeriodPeriod3(),
                    VaryBlackoutPeriodPeriod4: this.VaryBlackoutPeriodPeriod4(),
                    VaryBlackoutPeriodPeriod5: this.VaryBlackoutPeriodPeriod5(),
                    VaryBlackoutPeriodPeriod6: this.VaryBlackoutPeriodPeriod6(),
                    VaryBlackoutPeriodPeriod7: this.VaryBlackoutPeriodPeriod7(),
                    VaryBlackoutPeriodPeriod8: this.VaryBlackoutPeriodPeriod8(),
                    VaryBlackoutPeriodPeriod9: this.VaryBlackoutPeriodPeriod9(),
                    VaryBlackoutPeriodPeriod10: this.VaryBlackoutPeriodPeriod10(),
                    VaryBlackoutPeriodPeriod11: this.VaryBlackoutPeriodPeriod11(),
                    VaryBlackoutPeriodPeriod12: this.VaryBlackoutPeriodPeriod12(),
                    Level2or3DefineExposures1Exposure: this.Level2or3DefineExposures1Exposure(),
                    Level2or3DefineExposures1Compare: this.Level2or3DefineExposures1Compare(),
                    Level2or3DefineExposures2Exposure: this.Level2or3DefineExposures2Exposure(),
                    Level2or3DefineExposures2Compare: this.Level2or3DefineExposures2Compare(),
                    Level2or3DefineExposures3Exposure: this.Level2or3DefineExposures3Exposure(),
                    Level2or3DefineExposures3Compare: this.Level2or3DefineExposures3Compare(),
                    Level2or3WashoutPeriod1Exposure: this.Level2or3WashoutPeriod1Exposure(),
                    Level2or3WashoutPeriod1Compare: this.Level2or3WashoutPeriod1Compare(),
                    Level2or3WashoutPeriod2Exposure: this.Level2or3WashoutPeriod2Exposure(),
                    Level2or3WashoutPeriod2Compare: this.Level2or3WashoutPeriod2Compare(),
                    Level2or3WashoutPeriod3Exposure: this.Level2or3WashoutPeriod3Exposure(),
                    Level2or3WashoutPeriod3Compare: this.Level2or3WashoutPeriod3Compare(),
                    Level2or3SpecifyExposedTimeAssessment1Exposure: this.Level2or3SpecifyExposedTimeAssessment1Exposure(),
                    Level2or3SpecifyExposedTimeAssessment1Compare: this.Level2or3SpecifyExposedTimeAssessment1Compare(),
                    Level2or3SpecifyExposedTimeAssessment2Exposure: this.Level2or3SpecifyExposedTimeAssessment2Exposure(),
                    Level2or3SpecifyExposedTimeAssessment2Compare: this.Level2or3SpecifyExposedTimeAssessment2Compare(),
                    Level2or3SpecifyExposedTimeAssessment3Exposure: this.Level2or3SpecifyExposedTimeAssessment3Exposure(),
                    Level2or3SpecifyExposedTimeAssessment3Compare: this.Level2or3SpecifyExposedTimeAssessment3Compare(),
                    Level2or3EpisodeAllowableGap1Exposure: this.Level2or3EpisodeAllowableGap1Exposure(),
                    Level2or3EpisodeAllowableGap1Compare: this.Level2or3EpisodeAllowableGap1Compare(),
                    Level2or3EpisodeAllowableGap2Exposure: this.Level2or3EpisodeAllowableGap2Exposure(),
                    Level2or3EpisodeAllowableGap2Compare: this.Level2or3EpisodeAllowableGap2Compare(),
                    Level2or3EpisodeAllowableGap3Exposure: this.Level2or3EpisodeAllowableGap3Exposure(),
                    Level2or3EpisodeAllowableGap3Compare: this.Level2or3EpisodeAllowableGap3Compare(),
                    Level2or3EpisodeExtensionPeriod1Exposure: this.Level2or3EpisodeExtensionPeriod1Exposure(),
                    Level2or3EpisodeExtensionPeriod1Compare: this.Level2or3EpisodeExtensionPeriod1Compare(),
                    Level2or3EpisodeExtensionPeriod2Exposure: this.Level2or3EpisodeExtensionPeriod2Exposure(),
                    Level2or3EpisodeExtensionPeriod2Compare: this.Level2or3EpisodeExtensionPeriod2Compare(),
                    Level2or3EpisodeExtensionPeriod3Exposure: this.Level2or3EpisodeExtensionPeriod3Exposure(),
                    Level2or3EpisodeExtensionPeriod3Compare: this.Level2or3EpisodeExtensionPeriod3Compare(),
                    Level2or3MinimumEpisodeDuration1Exposure: this.Level2or3MinimumEpisodeDuration1Exposure(),
                    Level2or3MinimumEpisodeDuration1Compare: this.Level2or3MinimumEpisodeDuration1Compare(),
                    Level2or3MinimumEpisodeDuration2Exposure: this.Level2or3MinimumEpisodeDuration2Exposure(),
                    Level2or3MinimumEpisodeDuration2Compare: this.Level2or3MinimumEpisodeDuration2Compare(),
                    Level2or3MinimumEpisodeDuration3Exposure: this.Level2or3MinimumEpisodeDuration3Exposure(),
                    Level2or3MinimumEpisodeDuration3Compare: this.Level2or3MinimumEpisodeDuration3Compare(),
                    Level2or3MinimumDaysSupply1Exposure: this.Level2or3MinimumDaysSupply1Exposure(),
                    Level2or3MinimumDaysSupply1Compare: this.Level2or3MinimumDaysSupply1Compare(),
                    Level2or3MinimumDaysSupply2Exposure: this.Level2or3MinimumDaysSupply2Exposure(),
                    Level2or3MinimumDaysSupply2Compare: this.Level2or3MinimumDaysSupply2Compare(),
                    Level2or3MinimumDaysSupply3Exposure: this.Level2or3MinimumDaysSupply3Exposure(),
                    Level2or3MinimumDaysSupply3Compare: this.Level2or3MinimumDaysSupply3Compare(),
                    Level2or3SpecifyFollowUpDuration1Exposure: this.Level2or3SpecifyFollowUpDuration1Exposure(),
                    Level2or3SpecifyFollowUpDuration1Compare: this.Level2or3SpecifyFollowUpDuration1Compare(),
                    Level2or3SpecifyFollowUpDuration2Exposure: this.Level2or3SpecifyFollowUpDuration2Exposure(),
                    Level2or3SpecifyFollowUpDuration2Compare: this.Level2or3SpecifyFollowUpDuration2Compare(),
                    Level2or3SpecifyFollowUpDuration3Exposure: this.Level2or3SpecifyFollowUpDuration3Exposure(),
                    Level2or3SpecifyFollowUpDuration3Compare: this.Level2or3SpecifyFollowUpDuration3Compare(),
                    Level2or3AllowOnOrMultipleExposureEpisodes1Exposure: this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure(),
                    Level2or3AllowOnOrMultipleExposureEpisodes1Compare: this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare(),
                    Level2or3AllowOnOrMultipleExposureEpisodes2Exposure: this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure(),
                    Level2or3AllowOnOrMultipleExposureEpisodes2Compare: this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare(),
                    Level2or3AllowOnOrMultipleExposureEpisodes3Exposure: this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure(),
                    Level2or3AllowOnOrMultipleExposureEpisodes3Compare: this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare(),
                    Level2or3TruncateExposedtime1Exposure: this.Level2or3TruncateExposedtime1Exposure(),
                    Level2or3TruncateExposedtime1Compare: this.Level2or3TruncateExposedtime1Compare(),
                    Level2or3TruncateExposedtime2Exposure: this.Level2or3TruncateExposedtime2Exposure(),
                    Level2or3TruncateExposedtime2Compare: this.Level2or3TruncateExposedtime2Compare(),
                    Level2or3TruncateExposedtime3Exposure: this.Level2or3TruncateExposedtime3Exposure(),
                    Level2or3TruncateExposedtime3Compare: this.Level2or3TruncateExposedtime3Compare(),
                    Level2or3TruncateExposedTimeSpecified1Exposure: this.Level2or3TruncateExposedTimeSpecified1Exposure(),
                    Level2or3TruncateExposedTimeSpecified1Compare: this.Level2or3TruncateExposedTimeSpecified1Compare(),
                    Level2or3TruncateExposedTimeSpecified2Exposure: this.Level2or3TruncateExposedTimeSpecified2Exposure(),
                    Level2or3TruncateExposedTimeSpecified2Compare: this.Level2or3TruncateExposedTimeSpecified2Compare(),
                    Level2or3TruncateExposedTimeSpecified3Exposure: this.Level2or3TruncateExposedTimeSpecified3Exposure(),
                    Level2or3TruncateExposedTimeSpecified3Compare: this.Level2or3TruncateExposedTimeSpecified3Compare(),
                    Level2or3SpecifyBlackoutPeriod1Exposure: this.Level2or3SpecifyBlackoutPeriod1Exposure(),
                    Level2or3SpecifyBlackoutPeriod1Compare: this.Level2or3SpecifyBlackoutPeriod1Compare(),
                    Level2or3SpecifyBlackoutPeriod2Exposure: this.Level2or3SpecifyBlackoutPeriod2Exposure(),
                    Level2or3SpecifyBlackoutPeriod2Compare: this.Level2or3SpecifyBlackoutPeriod2Compare(),
                    Level2or3SpecifyBlackoutPeriod3Exposure: this.Level2or3SpecifyBlackoutPeriod3Exposure(),
                    Level2or3SpecifyBlackoutPeriod3Compare: this.Level2or3SpecifyBlackoutPeriod3Compare(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62(),
                    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63: this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62(),
                    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63: this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62(),
                    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63: this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62(),
                    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63: this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63(),
                    Level2or3VaryLengthOfWashoutPeriod1Exposure: this.Level2or3VaryLengthOfWashoutPeriod1Exposure(),
                    Level2or3VaryLengthOfWashoutPeriod1Compare: this.Level2or3VaryLengthOfWashoutPeriod1Compare(),
                    Level2or3VaryLengthOfWashoutPeriod2Exposure: this.Level2or3VaryLengthOfWashoutPeriod2Exposure(),
                    Level2or3VaryLengthOfWashoutPeriod2Compare: this.Level2or3VaryLengthOfWashoutPeriod2Compare(),
                    Level2or3VaryLengthOfWashoutPeriod3Exposure: this.Level2or3VaryLengthOfWashoutPeriod3Exposure(),
                    Level2or3VaryLengthOfWashoutPeriod3Compare: this.Level2or3VaryLengthOfWashoutPeriod3Compare(),
                    Level2or3VaryUserExposedTime1Exposure: this.Level2or3VaryUserExposedTime1Exposure(),
                    Level2or3VaryUserExposedTime1Compare: this.Level2or3VaryUserExposedTime1Compare(),
                    Level2or3VaryUserExposedTime2Exposure: this.Level2or3VaryUserExposedTime2Exposure(),
                    Level2or3VaryUserExposedTime2Compare: this.Level2or3VaryUserExposedTime2Compare(),
                    Level2or3VaryUserExposedTime3Exposure: this.Level2or3VaryUserExposedTime3Exposure(),
                    Level2or3VaryUserExposedTime3Compare: this.Level2or3VaryUserExposedTime3Compare(),
                    Level2or3VaryBlackoutPeriodPeriod1Exposure: this.Level2or3VaryBlackoutPeriodPeriod1Exposure(),
                    Level2or3VaryBlackoutPeriodPeriod1Compare: this.Level2or3VaryBlackoutPeriodPeriod1Compare(),
                    Level2or3VaryBlackoutPeriodPeriod2Exposure: this.Level2or3VaryBlackoutPeriodPeriod2Exposure(),
                    Level2or3VaryBlackoutPeriodPeriod2Compare: this.Level2or3VaryBlackoutPeriodPeriod2Compare(),
                    Level2or3VaryBlackoutPeriodPeriod3Exposure: this.Level2or3VaryBlackoutPeriodPeriod3Exposure(),
                    Level2or3VaryBlackoutPeriodPeriod3Compare: this.Level2or3VaryBlackoutPeriodPeriod3Compare(),
                    OutcomeList: this.OutcomeList == null ? null : this.OutcomeList().map(function (item) { return item.toData(); }),
                    AgeCovariate: this.AgeCovariate(),
                    SexCovariate: this.SexCovariate(),
                    TimeCovariate: this.TimeCovariate(),
                    YearCovariate: this.YearCovariate(),
                    ComorbidityCovariate: this.ComorbidityCovariate(),
                    HealthCovariate: this.HealthCovariate(),
                    DrugCovariate: this.DrugCovariate(),
                    CovariateList: this.CovariateList == null ? null : this.CovariateList().map(function (item) { return item.toData(); }),
                    hdPSAnalysis: this.hdPSAnalysis(),
                    InclusionCovariates: this.InclusionCovariates(),
                    PoolCovariates: this.PoolCovariates(),
                    SelectionCovariates: this.SelectionCovariates(),
                    ZeroCellCorrection: this.ZeroCellCorrection(),
                    MatchingRatio: this.MatchingRatio(),
                    MatchingCalipers: this.MatchingCalipers(),
                    VaryMatchingRatio: this.VaryMatchingRatio(),
                    VaryMatchingCalipers: this.VaryMatchingCalipers(),
                };
            };
            return RequestFormViewModel;
        }(ViewModel));
        ViewModels.RequestFormViewModel = RequestFormViewModel;
        var OutcomeItemViewModel = (function (_super) {
            __extends(OutcomeItemViewModel, _super);
            function OutcomeItemViewModel(OutcomeItemDTO) {
                var _this = _super.call(this) || this;
                if (OutcomeItemDTO == null) {
                    _this.CommonName = ko.observable();
                    _this.Outcome = ko.observable();
                    _this.WashoutPeriod = ko.observable();
                    _this.VaryWashoutPeriod = ko.observable();
                }
                else {
                    _this.CommonName = ko.observable(OutcomeItemDTO.CommonName);
                    _this.Outcome = ko.observable(OutcomeItemDTO.Outcome);
                    _this.WashoutPeriod = ko.observable(OutcomeItemDTO.WashoutPeriod);
                    _this.VaryWashoutPeriod = ko.observable(OutcomeItemDTO.VaryWashoutPeriod);
                }
                return _this;
            }
            OutcomeItemViewModel.prototype.toData = function () {
                return {
                    CommonName: this.CommonName(),
                    Outcome: this.Outcome(),
                    WashoutPeriod: this.WashoutPeriod(),
                    VaryWashoutPeriod: this.VaryWashoutPeriod(),
                };
            };
            return OutcomeItemViewModel;
        }(ViewModel));
        ViewModels.OutcomeItemViewModel = OutcomeItemViewModel;
        var CovariateItemViewModel = (function (_super) {
            __extends(CovariateItemViewModel, _super);
            function CovariateItemViewModel(CovariateItemDTO) {
                var _this = _super.call(this) || this;
                if (CovariateItemDTO == null) {
                    _this.GroupingIndicator = ko.observable();
                    _this.Description = ko.observable();
                    _this.CodeType = ko.observable();
                    _this.Ingredients = ko.observable();
                    _this.SubGroupAnalysis = ko.observable();
                }
                else {
                    _this.GroupingIndicator = ko.observable(CovariateItemDTO.GroupingIndicator);
                    _this.Description = ko.observable(CovariateItemDTO.Description);
                    _this.CodeType = ko.observable(CovariateItemDTO.CodeType);
                    _this.Ingredients = ko.observable(CovariateItemDTO.Ingredients);
                    _this.SubGroupAnalysis = ko.observable(CovariateItemDTO.SubGroupAnalysis);
                }
                return _this;
            }
            CovariateItemViewModel.prototype.toData = function () {
                return {
                    GroupingIndicator: this.GroupingIndicator(),
                    Description: this.Description(),
                    CodeType: this.CodeType(),
                    Ingredients: this.Ingredients(),
                    SubGroupAnalysis: this.SubGroupAnalysis(),
                };
            };
            return CovariateItemViewModel;
        }(ViewModel));
        ViewModels.CovariateItemViewModel = CovariateItemViewModel;
        var WorkflowHistoryItemViewModel = (function (_super) {
            __extends(WorkflowHistoryItemViewModel, _super);
            function WorkflowHistoryItemViewModel(WorkflowHistoryItemDTO) {
                var _this = _super.call(this) || this;
                if (WorkflowHistoryItemDTO == null) {
                    _this.TaskID = ko.observable();
                    _this.TaskName = ko.observable();
                    _this.UserID = ko.observable();
                    _this.UserName = ko.observable();
                    _this.UserFullName = ko.observable();
                    _this.Message = ko.observable();
                    _this.Date = ko.observable();
                    _this.RoutingID = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.WorkflowActivityID = ko.observable();
                }
                else {
                    _this.TaskID = ko.observable(WorkflowHistoryItemDTO.TaskID);
                    _this.TaskName = ko.observable(WorkflowHistoryItemDTO.TaskName);
                    _this.UserID = ko.observable(WorkflowHistoryItemDTO.UserID);
                    _this.UserName = ko.observable(WorkflowHistoryItemDTO.UserName);
                    _this.UserFullName = ko.observable(WorkflowHistoryItemDTO.UserFullName);
                    _this.Message = ko.observable(WorkflowHistoryItemDTO.Message);
                    _this.Date = ko.observable(WorkflowHistoryItemDTO.Date);
                    _this.RoutingID = ko.observable(WorkflowHistoryItemDTO.RoutingID);
                    _this.DataMart = ko.observable(WorkflowHistoryItemDTO.DataMart);
                    _this.WorkflowActivityID = ko.observable(WorkflowHistoryItemDTO.WorkflowActivityID);
                }
                return _this;
            }
            WorkflowHistoryItemViewModel.prototype.toData = function () {
                return {
                    TaskID: this.TaskID(),
                    TaskName: this.TaskName(),
                    UserID: this.UserID(),
                    UserName: this.UserName(),
                    UserFullName: this.UserFullName(),
                    Message: this.Message(),
                    Date: this.Date(),
                    RoutingID: this.RoutingID(),
                    DataMart: this.DataMart(),
                    WorkflowActivityID: this.WorkflowActivityID(),
                };
            };
            return WorkflowHistoryItemViewModel;
        }(ViewModel));
        ViewModels.WorkflowHistoryItemViewModel = WorkflowHistoryItemViewModel;
        var LegacySchedulerRequestViewModel = (function (_super) {
            __extends(LegacySchedulerRequestViewModel, _super);
            function LegacySchedulerRequestViewModel(LegacySchedulerRequestDTO) {
                var _this = _super.call(this) || this;
                if (LegacySchedulerRequestDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.AdapterPackageVersion = ko.observable();
                    _this.ScheduleJSON = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(LegacySchedulerRequestDTO.RequestID);
                    _this.AdapterPackageVersion = ko.observable(LegacySchedulerRequestDTO.AdapterPackageVersion);
                    _this.ScheduleJSON = ko.observable(LegacySchedulerRequestDTO.ScheduleJSON);
                }
                return _this;
            }
            LegacySchedulerRequestViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    AdapterPackageVersion: this.AdapterPackageVersion(),
                    ScheduleJSON: this.ScheduleJSON(),
                };
            };
            return LegacySchedulerRequestViewModel;
        }(ViewModel));
        ViewModels.LegacySchedulerRequestViewModel = LegacySchedulerRequestViewModel;
        var DistributedRegressionAnalysisCenterManifestItem = (function (_super) {
            __extends(DistributedRegressionAnalysisCenterManifestItem, _super);
            function DistributedRegressionAnalysisCenterManifestItem(DistributedRegressionAnalysisCenterManifestItem) {
                var _this = _super.call(this) || this;
                if (DistributedRegressionAnalysisCenterManifestItem == null) {
                    _this.DocumentID = ko.observable();
                    _this.RevisionSetID = ko.observable();
                    _this.ResponseID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.DataPartnerIdentifier = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.RequestDataMartID = ko.observable();
                }
                else {
                    _this.DocumentID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DocumentID);
                    _this.RevisionSetID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RevisionSetID);
                    _this.ResponseID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.ResponseID);
                    _this.DataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMartID);
                    _this.DataPartnerIdentifier = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataPartnerIdentifier);
                    _this.DataMart = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMart);
                    _this.RequestDataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RequestDataMartID);
                }
                return _this;
            }
            DistributedRegressionAnalysisCenterManifestItem.prototype.toData = function () {
                return {
                    DocumentID: this.DocumentID(),
                    RevisionSetID: this.RevisionSetID(),
                    ResponseID: this.ResponseID(),
                    DataMartID: this.DataMartID(),
                    DataPartnerIdentifier: this.DataPartnerIdentifier(),
                    DataMart: this.DataMart(),
                    RequestDataMartID: this.RequestDataMartID(),
                };
            };
            return DistributedRegressionAnalysisCenterManifestItem;
        }(ViewModel));
        ViewModels.DistributedRegressionAnalysisCenterManifestItem = DistributedRegressionAnalysisCenterManifestItem;
        var SectionSpecificTermViewModel = (function (_super) {
            __extends(SectionSpecificTermViewModel, _super);
            function SectionSpecificTermViewModel(SectionSpecificTermDTO) {
                var _this = _super.call(this) || this;
                if (SectionSpecificTermDTO == null) {
                    _this.TermID = ko.observable();
                    _this.Section = ko.observable();
                }
                else {
                    _this.TermID = ko.observable(SectionSpecificTermDTO.TermID);
                    _this.Section = ko.observable(SectionSpecificTermDTO.Section);
                }
                return _this;
            }
            SectionSpecificTermViewModel.prototype.toData = function () {
                return {
                    TermID: this.TermID(),
                    Section: this.Section(),
                };
            };
            return SectionSpecificTermViewModel;
        }(ViewModel));
        ViewModels.SectionSpecificTermViewModel = SectionSpecificTermViewModel;
        var TemplateTermViewModel = (function (_super) {
            __extends(TemplateTermViewModel, _super);
            function TemplateTermViewModel(TemplateTermDTO) {
                var _this = _super.call(this) || this;
                if (TemplateTermDTO == null) {
                    _this.TemplateID = ko.observable();
                    _this.Template = new TemplateViewModel();
                    _this.TermID = ko.observable();
                    _this.Term = new TermViewModel();
                    _this.Allowed = ko.observable();
                    _this.Section = ko.observable();
                }
                else {
                    _this.TemplateID = ko.observable(TemplateTermDTO.TemplateID);
                    _this.Template = new TemplateViewModel(TemplateTermDTO.Template);
                    _this.TermID = ko.observable(TemplateTermDTO.TermID);
                    _this.Term = new TermViewModel(TemplateTermDTO.Term);
                    _this.Allowed = ko.observable(TemplateTermDTO.Allowed);
                    _this.Section = ko.observable(TemplateTermDTO.Section);
                }
                return _this;
            }
            TemplateTermViewModel.prototype.toData = function () {
                return {
                    TemplateID: this.TemplateID(),
                    Template: this.Template.toData(),
                    TermID: this.TermID(),
                    Term: this.Term.toData(),
                    Allowed: this.Allowed(),
                    Section: this.Section(),
                };
            };
            return TemplateTermViewModel;
        }(ViewModel));
        ViewModels.TemplateTermViewModel = TemplateTermViewModel;
        var MatchingCriteriaViewModel = (function (_super) {
            __extends(MatchingCriteriaViewModel, _super);
            function MatchingCriteriaViewModel(MatchingCriteriaDTO) {
                var _this = _super.call(this) || this;
                if (MatchingCriteriaDTO == null) {
                    _this.TermIDs = ko.observableArray();
                    _this.ProjectID = ko.observable();
                    _this.Request = ko.observable();
                    _this.RequestID = ko.observable();
                }
                else {
                    _this.TermIDs = ko.observableArray(MatchingCriteriaDTO.TermIDs == null ? null : MatchingCriteriaDTO.TermIDs.map(function (item) { return item; }));
                    _this.ProjectID = ko.observable(MatchingCriteriaDTO.ProjectID);
                    _this.Request = ko.observable(MatchingCriteriaDTO.Request);
                    _this.RequestID = ko.observable(MatchingCriteriaDTO.RequestID);
                }
                return _this;
            }
            MatchingCriteriaViewModel.prototype.toData = function () {
                return {
                    TermIDs: this.TermIDs(),
                    ProjectID: this.ProjectID(),
                    Request: this.Request(),
                    RequestID: this.RequestID(),
                };
            };
            return MatchingCriteriaViewModel;
        }(ViewModel));
        ViewModels.MatchingCriteriaViewModel = MatchingCriteriaViewModel;
        var QueryComposerCriteriaViewModel = (function (_super) {
            __extends(QueryComposerCriteriaViewModel, _super);
            function QueryComposerCriteriaViewModel(QueryComposerCriteriaDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerCriteriaDTO == null) {
                    _this.ID = ko.observable();
                    _this.RelatedToID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Operator = ko.observable();
                    _this.IndexEvent = ko.observable();
                    _this.Exclusion = ko.observable();
                    _this.Criteria = ko.observableArray();
                    _this.Terms = ko.observableArray();
                    _this.Type = ko.observable();
                }
                else {
                    _this.ID = ko.observable(QueryComposerCriteriaDTO.ID);
                    _this.RelatedToID = ko.observable(QueryComposerCriteriaDTO.RelatedToID);
                    _this.Name = ko.observable(QueryComposerCriteriaDTO.Name);
                    _this.Operator = ko.observable(QueryComposerCriteriaDTO.Operator);
                    _this.IndexEvent = ko.observable(QueryComposerCriteriaDTO.IndexEvent);
                    _this.Exclusion = ko.observable(QueryComposerCriteriaDTO.Exclusion);
                    _this.Criteria = ko.observableArray(QueryComposerCriteriaDTO.Criteria == null ? null : QueryComposerCriteriaDTO.Criteria.map(function (item) { return new QueryComposerCriteriaViewModel(item); }));
                    _this.Terms = ko.observableArray(QueryComposerCriteriaDTO.Terms == null ? null : QueryComposerCriteriaDTO.Terms.map(function (item) { return new QueryComposerTermViewModel(item); }));
                    _this.Type = ko.observable(QueryComposerCriteriaDTO.Type);
                }
                return _this;
            }
            QueryComposerCriteriaViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    RelatedToID: this.RelatedToID(),
                    Name: this.Name(),
                    Operator: this.Operator(),
                    IndexEvent: this.IndexEvent(),
                    Exclusion: this.Exclusion(),
                    Criteria: this.Criteria == null ? null : this.Criteria().map(function (item) { return item.toData(); }),
                    Terms: this.Terms == null ? null : this.Terms().map(function (item) { return item.toData(); }),
                    Type: this.Type(),
                };
            };
            return QueryComposerCriteriaViewModel;
        }(ViewModel));
        ViewModels.QueryComposerCriteriaViewModel = QueryComposerCriteriaViewModel;
        var QueryComposerFieldViewModel = (function (_super) {
            __extends(QueryComposerFieldViewModel, _super);
            function QueryComposerFieldViewModel(QueryComposerFieldDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerFieldDTO == null) {
                    _this.FieldName = ko.observable();
                    _this.Type = ko.observable();
                    _this.GroupBy = ko.observable();
                    _this.StratifyBy = ko.observable();
                    _this.Aggregate = ko.observable();
                    _this.Select = ko.observableArray();
                    _this.OrderBy = ko.observable();
                }
                else {
                    _this.FieldName = ko.observable(QueryComposerFieldDTO.FieldName);
                    _this.Type = ko.observable(QueryComposerFieldDTO.Type);
                    _this.GroupBy = ko.observable(QueryComposerFieldDTO.GroupBy);
                    _this.StratifyBy = ko.observable(QueryComposerFieldDTO.StratifyBy);
                    _this.Aggregate = ko.observable(QueryComposerFieldDTO.Aggregate);
                    _this.Select = ko.observableArray(QueryComposerFieldDTO.Select == null ? null : QueryComposerFieldDTO.Select.map(function (item) { return new QueryComposerSelectViewModel(item); }));
                    _this.OrderBy = ko.observable(QueryComposerFieldDTO.OrderBy);
                }
                return _this;
            }
            QueryComposerFieldViewModel.prototype.toData = function () {
                return {
                    FieldName: this.FieldName(),
                    Type: this.Type(),
                    GroupBy: this.GroupBy(),
                    StratifyBy: this.StratifyBy(),
                    Aggregate: this.Aggregate(),
                    Select: this.Select == null ? null : this.Select().map(function (item) { return item.toData(); }),
                    OrderBy: this.OrderBy(),
                };
            };
            return QueryComposerFieldViewModel;
        }(ViewModel));
        ViewModels.QueryComposerFieldViewModel = QueryComposerFieldViewModel;
        var QueryComposerGroupByViewModel = (function (_super) {
            __extends(QueryComposerGroupByViewModel, _super);
            function QueryComposerGroupByViewModel(QueryComposerGroupByDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerGroupByDTO == null) {
                    _this.Field = ko.observable();
                    _this.Aggregate = ko.observable();
                }
                else {
                    _this.Field = ko.observable(QueryComposerGroupByDTO.Field);
                    _this.Aggregate = ko.observable(QueryComposerGroupByDTO.Aggregate);
                }
                return _this;
            }
            QueryComposerGroupByViewModel.prototype.toData = function () {
                return {
                    Field: this.Field(),
                    Aggregate: this.Aggregate(),
                };
            };
            return QueryComposerGroupByViewModel;
        }(ViewModel));
        ViewModels.QueryComposerGroupByViewModel = QueryComposerGroupByViewModel;
        var QueryComposerHeaderViewModel = (function (_super) {
            __extends(QueryComposerHeaderViewModel, _super);
            function QueryComposerHeaderViewModel(QueryComposerHeaderDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerHeaderDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.ViewUrl = ko.observable();
                    _this.Grammar = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.QueryType = ko.observable();
                    _this.SubmittedOn = ko.observable();
                }
                else {
                    _this.Name = ko.observable(QueryComposerHeaderDTO.Name);
                    _this.Description = ko.observable(QueryComposerHeaderDTO.Description);
                    _this.ViewUrl = ko.observable(QueryComposerHeaderDTO.ViewUrl);
                    _this.Grammar = ko.observable(QueryComposerHeaderDTO.Grammar);
                    _this.Priority = ko.observable(QueryComposerHeaderDTO.Priority);
                    _this.DueDate = ko.observable(QueryComposerHeaderDTO.DueDate);
                    _this.QueryType = ko.observable(QueryComposerHeaderDTO.QueryType);
                    _this.SubmittedOn = ko.observable(QueryComposerHeaderDTO.SubmittedOn);
                }
                return _this;
            }
            QueryComposerHeaderViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    ViewUrl: this.ViewUrl(),
                    Grammar: this.Grammar(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    QueryType: this.QueryType(),
                    SubmittedOn: this.SubmittedOn(),
                };
            };
            return QueryComposerHeaderViewModel;
        }(ViewModel));
        ViewModels.QueryComposerHeaderViewModel = QueryComposerHeaderViewModel;
        var QueryComposerOrderByViewModel = (function (_super) {
            __extends(QueryComposerOrderByViewModel, _super);
            function QueryComposerOrderByViewModel(QueryComposerOrderByDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerOrderByDTO == null) {
                    _this.Direction = ko.observable();
                }
                else {
                    _this.Direction = ko.observable(QueryComposerOrderByDTO.Direction);
                }
                return _this;
            }
            QueryComposerOrderByViewModel.prototype.toData = function () {
                return {
                    Direction: this.Direction(),
                };
            };
            return QueryComposerOrderByViewModel;
        }(ViewModel));
        ViewModels.QueryComposerOrderByViewModel = QueryComposerOrderByViewModel;
        var QueryComposerResponseErrorViewModel = (function (_super) {
            __extends(QueryComposerResponseErrorViewModel, _super);
            function QueryComposerResponseErrorViewModel(QueryComposerResponseErrorDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerResponseErrorDTO == null) {
                    _this.Code = ko.observable();
                    _this.Description = ko.observable();
                }
                else {
                    _this.Code = ko.observable(QueryComposerResponseErrorDTO.Code);
                    _this.Description = ko.observable(QueryComposerResponseErrorDTO.Description);
                }
                return _this;
            }
            QueryComposerResponseErrorViewModel.prototype.toData = function () {
                return {
                    Code: this.Code(),
                    Description: this.Description(),
                };
            };
            return QueryComposerResponseErrorViewModel;
        }(ViewModel));
        ViewModels.QueryComposerResponseErrorViewModel = QueryComposerResponseErrorViewModel;
        var QueryComposerSelectViewModel = (function (_super) {
            __extends(QueryComposerSelectViewModel, _super);
            function QueryComposerSelectViewModel(QueryComposerSelectDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerSelectDTO == null) {
                    _this.Fields = ko.observableArray();
                }
                else {
                    _this.Fields = ko.observableArray(QueryComposerSelectDTO.Fields == null ? null : QueryComposerSelectDTO.Fields.map(function (item) { return new QueryComposerFieldViewModel(item); }));
                }
                return _this;
            }
            QueryComposerSelectViewModel.prototype.toData = function () {
                return {
                    Fields: this.Fields == null ? null : this.Fields().map(function (item) { return item.toData(); }),
                };
            };
            return QueryComposerSelectViewModel;
        }(ViewModel));
        ViewModels.QueryComposerSelectViewModel = QueryComposerSelectViewModel;
        var QueryComposerResponseViewModel = (function (_super) {
            __extends(QueryComposerResponseViewModel, _super);
            function QueryComposerResponseViewModel(QueryComposerResponseDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerResponseDTO == null) {
                    _this.ID = ko.observable();
                    _this.DocumentID = ko.observable();
                    _this.ResponseDateTime = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.Errors = ko.observableArray();
                    _this.Results = ko.observableArray();
                    _this.LowCellThrehold = ko.observable();
                    _this.Properties = ko.observableArray();
                    _this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel();
                }
                else {
                    _this.ID = ko.observable(QueryComposerResponseDTO.ID);
                    _this.DocumentID = ko.observable(QueryComposerResponseDTO.DocumentID);
                    _this.ResponseDateTime = ko.observable(QueryComposerResponseDTO.ResponseDateTime);
                    _this.RequestID = ko.observable(QueryComposerResponseDTO.RequestID);
                    _this.Errors = ko.observableArray(QueryComposerResponseDTO.Errors == null ? null : QueryComposerResponseDTO.Errors.map(function (item) { return new QueryComposerResponseErrorViewModel(item); }));
                    _this.Results = ko.observableArray(QueryComposerResponseDTO.Results == null ? null : QueryComposerResponseDTO.Results.map(function (item) { return item; }));
                    _this.LowCellThrehold = ko.observable(QueryComposerResponseDTO.LowCellThrehold);
                    _this.Properties = ko.observableArray(QueryComposerResponseDTO.Properties == null ? null : QueryComposerResponseDTO.Properties.map(function (item) { return new QueryComposerResponsePropertyDefinitionViewModel(item); }));
                    _this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel(QueryComposerResponseDTO.Aggregation);
                }
                return _this;
            }
            QueryComposerResponseViewModel.prototype.toData = function () {
                return {
                    ID: this.ID(),
                    DocumentID: this.DocumentID(),
                    ResponseDateTime: this.ResponseDateTime(),
                    RequestID: this.RequestID(),
                    Errors: this.Errors == null ? null : this.Errors().map(function (item) { return item.toData(); }),
                    Results: this.Results(),
                    LowCellThrehold: this.LowCellThrehold(),
                    Properties: this.Properties == null ? null : this.Properties().map(function (item) { return item.toData(); }),
                    Aggregation: this.Aggregation.toData(),
                };
            };
            return QueryComposerResponseViewModel;
        }(ViewModel));
        ViewModels.QueryComposerResponseViewModel = QueryComposerResponseViewModel;
        var QueryComposerResponseAggregationDefinitionViewModel = (function (_super) {
            __extends(QueryComposerResponseAggregationDefinitionViewModel, _super);
            function QueryComposerResponseAggregationDefinitionViewModel(QueryComposerResponseAggregationDefinitionDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerResponseAggregationDefinitionDTO == null) {
                    _this.GroupBy = ko.observableArray();
                    _this.Select = ko.observableArray();
                    _this.Name = ko.observable();
                }
                else {
                    _this.GroupBy = ko.observableArray(QueryComposerResponseAggregationDefinitionDTO.GroupBy == null ? null : QueryComposerResponseAggregationDefinitionDTO.GroupBy.map(function (item) { return item; }));
                    _this.Select = ko.observableArray(QueryComposerResponseAggregationDefinitionDTO.Select == null ? null : QueryComposerResponseAggregationDefinitionDTO.Select.map(function (item) { return item; }));
                    _this.Name = ko.observable(QueryComposerResponseAggregationDefinitionDTO.Name);
                }
                return _this;
            }
            QueryComposerResponseAggregationDefinitionViewModel.prototype.toData = function () {
                return {
                    GroupBy: this.GroupBy == null ? null : this.GroupBy().map(function (item) { return item; }),
                    Select: this.Select(),
                    Name: this.Name(),
                };
            };
            return QueryComposerResponseAggregationDefinitionViewModel;
        }(ViewModel));
        ViewModels.QueryComposerResponseAggregationDefinitionViewModel = QueryComposerResponseAggregationDefinitionViewModel;
        var QueryComposerResponsePropertyDefinitionViewModel = (function (_super) {
            __extends(QueryComposerResponsePropertyDefinitionViewModel, _super);
            function QueryComposerResponsePropertyDefinitionViewModel(QueryComposerResponsePropertyDefinitionDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerResponsePropertyDefinitionDTO == null) {
                    _this.Name = ko.observable();
                    _this.Type = ko.observable();
                    _this.As = ko.observable();
                    _this.Aggregate = ko.observable();
                }
                else {
                    _this.Name = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Name);
                    _this.Type = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Type);
                    _this.As = ko.observable(QueryComposerResponsePropertyDefinitionDTO.As);
                    _this.Aggregate = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Aggregate);
                }
                return _this;
            }
            QueryComposerResponsePropertyDefinitionViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Type: this.Type(),
                    As: this.As(),
                    Aggregate: this.Aggregate(),
                };
            };
            return QueryComposerResponsePropertyDefinitionViewModel;
        }(ViewModel));
        ViewModels.QueryComposerResponsePropertyDefinitionViewModel = QueryComposerResponsePropertyDefinitionViewModel;
        var QueryComposerTermViewModel = (function (_super) {
            __extends(QueryComposerTermViewModel, _super);
            function QueryComposerTermViewModel(QueryComposerTermDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerTermDTO == null) {
                    _this.Operator = ko.observable();
                    _this.Type = ko.observable();
                    _this.Values = ko.observable({});
                    _this.Criteria = ko.observableArray();
                    _this.Design = new DesignViewModel();
                }
                else {
                    _this.Operator = ko.observable(QueryComposerTermDTO.Operator);
                    _this.Type = ko.observable(QueryComposerTermDTO.Type);
                    _this.Values = ko.observable(QueryComposerTermDTO.Values);
                    _this.Criteria = ko.observableArray(QueryComposerTermDTO.Criteria == null ? null : QueryComposerTermDTO.Criteria.map(function (item) { return new QueryComposerCriteriaViewModel(item); }));
                    _this.Design = new DesignViewModel(QueryComposerTermDTO.Design);
                }
                return _this;
            }
            QueryComposerTermViewModel.prototype.toData = function () {
                return {
                    Operator: this.Operator(),
                    Type: this.Type(),
                    Values: ko.mapping.toJS(this.Values()),
                    Criteria: this.Criteria == null ? null : this.Criteria().map(function (item) { return item.toData(); }),
                    Design: this.Design.toData(),
                };
            };
            return QueryComposerTermViewModel;
        }(ViewModel));
        ViewModels.QueryComposerTermViewModel = QueryComposerTermViewModel;
        var QueryComposerWhereViewModel = (function (_super) {
            __extends(QueryComposerWhereViewModel, _super);
            function QueryComposerWhereViewModel(QueryComposerWhereDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerWhereDTO == null) {
                    _this.Criteria = ko.observableArray();
                }
                else {
                    _this.Criteria = ko.observableArray(QueryComposerWhereDTO.Criteria == null ? null : QueryComposerWhereDTO.Criteria.map(function (item) { return new QueryComposerCriteriaViewModel(item); }));
                }
                return _this;
            }
            QueryComposerWhereViewModel.prototype.toData = function () {
                return {
                    Criteria: this.Criteria == null ? null : this.Criteria().map(function (item) { return item.toData(); }),
                };
            };
            return QueryComposerWhereViewModel;
        }(ViewModel));
        ViewModels.QueryComposerWhereViewModel = QueryComposerWhereViewModel;
        var ProjectRequestTypeViewModel = (function (_super) {
            __extends(ProjectRequestTypeViewModel, _super);
            function ProjectRequestTypeViewModel(ProjectRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (ProjectRequestTypeDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.WorkflowID = ko.observable();
                    _this.Workflow = ko.observable();
                    _this.Template = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectRequestTypeDTO.ProjectID);
                    _this.RequestTypeID = ko.observable(ProjectRequestTypeDTO.RequestTypeID);
                    _this.RequestType = ko.observable(ProjectRequestTypeDTO.RequestType);
                    _this.WorkflowID = ko.observable(ProjectRequestTypeDTO.WorkflowID);
                    _this.Workflow = ko.observable(ProjectRequestTypeDTO.Workflow);
                    _this.Template = ko.observable(ProjectRequestTypeDTO.Template);
                }
                return _this;
            }
            ProjectRequestTypeViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    RequestTypeID: this.RequestTypeID(),
                    RequestType: this.RequestType(),
                    WorkflowID: this.WorkflowID(),
                    Workflow: this.Workflow(),
                    Template: this.Template(),
                };
            };
            return ProjectRequestTypeViewModel;
        }(EntityDtoViewModel));
        ViewModels.ProjectRequestTypeViewModel = ProjectRequestTypeViewModel;
        var RequestObserverEventSubscriptionViewModel = (function (_super) {
            __extends(RequestObserverEventSubscriptionViewModel, _super);
            function RequestObserverEventSubscriptionViewModel(RequestObserverEventSubscriptionDTO) {
                var _this = _super.call(this) || this;
                if (RequestObserverEventSubscriptionDTO == null) {
                    _this.RequestObserverID = ko.observable();
                    _this.EventID = ko.observable();
                    _this.LastRunTime = ko.observable();
                    _this.NextDueTime = ko.observable();
                    _this.Frequency = ko.observable();
                }
                else {
                    _this.RequestObserverID = ko.observable(RequestObserverEventSubscriptionDTO.RequestObserverID);
                    _this.EventID = ko.observable(RequestObserverEventSubscriptionDTO.EventID);
                    _this.LastRunTime = ko.observable(RequestObserverEventSubscriptionDTO.LastRunTime);
                    _this.NextDueTime = ko.observable(RequestObserverEventSubscriptionDTO.NextDueTime);
                    _this.Frequency = ko.observable(RequestObserverEventSubscriptionDTO.Frequency);
                }
                return _this;
            }
            RequestObserverEventSubscriptionViewModel.prototype.toData = function () {
                return {
                    RequestObserverID: this.RequestObserverID(),
                    EventID: this.EventID(),
                    LastRunTime: this.LastRunTime(),
                    NextDueTime: this.NextDueTime(),
                    Frequency: this.Frequency(),
                };
            };
            return RequestObserverEventSubscriptionViewModel;
        }(EntityDtoViewModel));
        ViewModels.RequestObserverEventSubscriptionViewModel = RequestObserverEventSubscriptionViewModel;
        var RequestTypeTermViewModel = (function (_super) {
            __extends(RequestTypeTermViewModel, _super);
            function RequestTypeTermViewModel(RequestTypeTermDTO) {
                var _this = _super.call(this) || this;
                if (RequestTypeTermDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.TermID = ko.observable();
                    _this.Term = ko.observable();
                    _this.Description = ko.observable();
                    _this.OID = ko.observable();
                    _this.ReferenceUrl = ko.observable();
                }
                else {
                    _this.RequestTypeID = ko.observable(RequestTypeTermDTO.RequestTypeID);
                    _this.TermID = ko.observable(RequestTypeTermDTO.TermID);
                    _this.Term = ko.observable(RequestTypeTermDTO.Term);
                    _this.Description = ko.observable(RequestTypeTermDTO.Description);
                    _this.OID = ko.observable(RequestTypeTermDTO.OID);
                    _this.ReferenceUrl = ko.observable(RequestTypeTermDTO.ReferenceUrl);
                }
                return _this;
            }
            RequestTypeTermViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    TermID: this.TermID(),
                    Term: this.Term(),
                    Description: this.Description(),
                    OID: this.OID(),
                    ReferenceUrl: this.ReferenceUrl(),
                };
            };
            return RequestTypeTermViewModel;
        }(EntityDtoViewModel));
        ViewModels.RequestTypeTermViewModel = RequestTypeTermViewModel;
        var BaseFieldOptionAclViewModel = (function (_super) {
            __extends(BaseFieldOptionAclViewModel, _super);
            function BaseFieldOptionAclViewModel(BaseFieldOptionAclDTO) {
                var _this = _super.call(this) || this;
                if (BaseFieldOptionAclDTO == null) {
                    _this.FieldIdentifier = ko.observable();
                    _this.Permission = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                }
                else {
                    _this.FieldIdentifier = ko.observable(BaseFieldOptionAclDTO.FieldIdentifier);
                    _this.Permission = ko.observable(BaseFieldOptionAclDTO.Permission);
                    _this.Overridden = ko.observable(BaseFieldOptionAclDTO.Overridden);
                    _this.SecurityGroupID = ko.observable(BaseFieldOptionAclDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(BaseFieldOptionAclDTO.SecurityGroup);
                }
                return _this;
            }
            BaseFieldOptionAclViewModel.prototype.toData = function () {
                return {
                    FieldIdentifier: this.FieldIdentifier(),
                    Permission: this.Permission(),
                    Overridden: this.Overridden(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                };
            };
            return BaseFieldOptionAclViewModel;
        }(EntityDtoViewModel));
        ViewModels.BaseFieldOptionAclViewModel = BaseFieldOptionAclViewModel;
        var BaseEventPermissionViewModel = (function (_super) {
            __extends(BaseEventPermissionViewModel, _super);
            function BaseEventPermissionViewModel(BaseEventPermissionDTO) {
                var _this = _super.call(this) || this;
                if (BaseEventPermissionDTO == null) {
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.SecurityGroupID = ko.observable(BaseEventPermissionDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(BaseEventPermissionDTO.SecurityGroup);
                    _this.Allowed = ko.observable(BaseEventPermissionDTO.Allowed);
                    _this.Overridden = ko.observable(BaseEventPermissionDTO.Overridden);
                    _this.EventID = ko.observable(BaseEventPermissionDTO.EventID);
                    _this.Event = ko.observable(BaseEventPermissionDTO.Event);
                }
                return _this;
            }
            BaseEventPermissionViewModel.prototype.toData = function () {
                return {
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return BaseEventPermissionViewModel;
        }(EntityDtoViewModel));
        ViewModels.BaseEventPermissionViewModel = BaseEventPermissionViewModel;
        var OrganizationGroupViewModel = (function (_super) {
            __extends(OrganizationGroupViewModel, _super);
            function OrganizationGroupViewModel(OrganizationGroupDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationGroupDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.GroupID = ko.observable();
                    _this.Group = ko.observable();
                }
                else {
                    _this.OrganizationID = ko.observable(OrganizationGroupDTO.OrganizationID);
                    _this.Organization = ko.observable(OrganizationGroupDTO.Organization);
                    _this.GroupID = ko.observable(OrganizationGroupDTO.GroupID);
                    _this.Group = ko.observable(OrganizationGroupDTO.Group);
                }
                return _this;
            }
            OrganizationGroupViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    GroupID: this.GroupID(),
                    Group: this.Group(),
                };
            };
            return OrganizationGroupViewModel;
        }(EntityDtoViewModel));
        ViewModels.OrganizationGroupViewModel = OrganizationGroupViewModel;
        var OrganizationRegistryViewModel = (function (_super) {
            __extends(OrganizationRegistryViewModel, _super);
            function OrganizationRegistryViewModel(OrganizationRegistryDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationRegistryDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.OrganizationParent = ko.observable();
                    _this.RegistryID = ko.observable();
                    _this.Registry = ko.observable();
                    _this.Description = ko.observable();
                    _this.Type = ko.observable();
                }
                else {
                    _this.OrganizationID = ko.observable(OrganizationRegistryDTO.OrganizationID);
                    _this.Organization = ko.observable(OrganizationRegistryDTO.Organization);
                    _this.Acronym = ko.observable(OrganizationRegistryDTO.Acronym);
                    _this.OrganizationParent = ko.observable(OrganizationRegistryDTO.OrganizationParent);
                    _this.RegistryID = ko.observable(OrganizationRegistryDTO.RegistryID);
                    _this.Registry = ko.observable(OrganizationRegistryDTO.Registry);
                    _this.Description = ko.observable(OrganizationRegistryDTO.Description);
                    _this.Type = ko.observable(OrganizationRegistryDTO.Type);
                }
                return _this;
            }
            OrganizationRegistryViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    Acronym: this.Acronym(),
                    OrganizationParent: this.OrganizationParent(),
                    RegistryID: this.RegistryID(),
                    Registry: this.Registry(),
                    Description: this.Description(),
                    Type: this.Type(),
                };
            };
            return OrganizationRegistryViewModel;
        }(EntityDtoViewModel));
        ViewModels.OrganizationRegistryViewModel = OrganizationRegistryViewModel;
        var ProjectDataMartWithRequestTypesViewModel = (function (_super) {
            __extends(ProjectDataMartWithRequestTypesViewModel, _super);
            function ProjectDataMartWithRequestTypesViewModel(ProjectDataMartWithRequestTypesDTO) {
                var _this = _super.call(this) || this;
                if (ProjectDataMartWithRequestTypesDTO == null) {
                    _this.RequestTypes = ko.observableArray();
                    _this.ProjectID = ko.observable();
                    _this.Project = ko.observable();
                    _this.ProjectAcronym = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.Organization = ko.observable();
                }
                else {
                    _this.RequestTypes = ko.observableArray(ProjectDataMartWithRequestTypesDTO.RequestTypes == null ? null : ProjectDataMartWithRequestTypesDTO.RequestTypes.map(function (item) { return new RequestTypeViewModel(item); }));
                    _this.ProjectID = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectID);
                    _this.Project = ko.observable(ProjectDataMartWithRequestTypesDTO.Project);
                    _this.ProjectAcronym = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectAcronym);
                    _this.DataMartID = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMartID);
                    _this.DataMart = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMart);
                    _this.Organization = ko.observable(ProjectDataMartWithRequestTypesDTO.Organization);
                }
                return _this;
            }
            ProjectDataMartWithRequestTypesViewModel.prototype.toData = function () {
                return {
                    RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map(function (item) { return item.toData(); }),
                    ProjectID: this.ProjectID(),
                    Project: this.Project(),
                    ProjectAcronym: this.ProjectAcronym(),
                    DataMartID: this.DataMartID(),
                    DataMart: this.DataMart(),
                    Organization: this.Organization(),
                };
            };
            return ProjectDataMartWithRequestTypesViewModel;
        }(ProjectDataMartViewModel));
        ViewModels.ProjectDataMartWithRequestTypesViewModel = ProjectDataMartWithRequestTypesViewModel;
        var ProjectOrganizationViewModel = (function (_super) {
            __extends(ProjectOrganizationViewModel, _super);
            function ProjectOrganizationViewModel(ProjectOrganizationDTO) {
                var _this = _super.call(this) || this;
                if (ProjectOrganizationDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.Project = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectOrganizationDTO.ProjectID);
                    _this.Project = ko.observable(ProjectOrganizationDTO.Project);
                    _this.OrganizationID = ko.observable(ProjectOrganizationDTO.OrganizationID);
                    _this.Organization = ko.observable(ProjectOrganizationDTO.Organization);
                }
                return _this;
            }
            ProjectOrganizationViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    Project: this.Project(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                };
            };
            return ProjectOrganizationViewModel;
        }(EntityDtoViewModel));
        ViewModels.ProjectOrganizationViewModel = ProjectOrganizationViewModel;
        var BaseAclViewModel = (function (_super) {
            __extends(BaseAclViewModel, _super);
            function BaseAclViewModel(BaseAclDTO) {
                var _this = _super.call(this) || this;
                if (BaseAclDTO == null) {
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.SecurityGroupID = ko.observable(BaseAclDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(BaseAclDTO.SecurityGroup);
                    _this.Overridden = ko.observable(BaseAclDTO.Overridden);
                }
                return _this;
            }
            BaseAclViewModel.prototype.toData = function () {
                return {
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return BaseAclViewModel;
        }(EntityDtoViewModel));
        ViewModels.BaseAclViewModel = BaseAclViewModel;
        var UserEventSubscriptionViewModel = (function (_super) {
            __extends(UserEventSubscriptionViewModel, _super);
            function UserEventSubscriptionViewModel(UserEventSubscriptionDTO) {
                var _this = _super.call(this) || this;
                if (UserEventSubscriptionDTO == null) {
                    _this.UserID = ko.observable();
                    _this.EventID = ko.observable();
                    _this.LastRunTime = ko.observable();
                    _this.NextDueTime = ko.observable();
                    _this.Frequency = ko.observable();
                    _this.FrequencyForMy = ko.observable();
                }
                else {
                    _this.UserID = ko.observable(UserEventSubscriptionDTO.UserID);
                    _this.EventID = ko.observable(UserEventSubscriptionDTO.EventID);
                    _this.LastRunTime = ko.observable(UserEventSubscriptionDTO.LastRunTime);
                    _this.NextDueTime = ko.observable(UserEventSubscriptionDTO.NextDueTime);
                    _this.Frequency = ko.observable(UserEventSubscriptionDTO.Frequency);
                    _this.FrequencyForMy = ko.observable(UserEventSubscriptionDTO.FrequencyForMy);
                }
                return _this;
            }
            UserEventSubscriptionViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    EventID: this.EventID(),
                    LastRunTime: this.LastRunTime(),
                    NextDueTime: this.NextDueTime(),
                    Frequency: this.Frequency(),
                    FrequencyForMy: this.FrequencyForMy(),
                };
            };
            return UserEventSubscriptionViewModel;
        }(EntityDtoViewModel));
        ViewModels.UserEventSubscriptionViewModel = UserEventSubscriptionViewModel;
        var UserSettingViewModel = (function (_super) {
            __extends(UserSettingViewModel, _super);
            function UserSettingViewModel(UserSettingDTO) {
                var _this = _super.call(this) || this;
                if (UserSettingDTO == null) {
                    _this.UserID = ko.observable();
                    _this.Key = ko.observable();
                    _this.Setting = ko.observable();
                }
                else {
                    _this.UserID = ko.observable(UserSettingDTO.UserID);
                    _this.Key = ko.observable(UserSettingDTO.Key);
                    _this.Setting = ko.observable(UserSettingDTO.Setting);
                }
                return _this;
            }
            UserSettingViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    Key: this.Key(),
                    Setting: this.Setting(),
                };
            };
            return UserSettingViewModel;
        }(EntityDtoViewModel));
        ViewModels.UserSettingViewModel = UserSettingViewModel;
        var WFCommentViewModel = (function (_super) {
            __extends(WFCommentViewModel, _super);
            function WFCommentViewModel(WFCommentDTO) {
                var _this = _super.call(this) || this;
                if (WFCommentDTO == null) {
                    _this.Comment = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.CreatedByID = ko.observable();
                    _this.CreatedBy = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.TaskID = ko.observable();
                    _this.WorkflowActivityID = ko.observable();
                    _this.WorkflowActivity = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Comment = ko.observable(WFCommentDTO.Comment);
                    _this.CreatedOn = ko.observable(WFCommentDTO.CreatedOn);
                    _this.CreatedByID = ko.observable(WFCommentDTO.CreatedByID);
                    _this.CreatedBy = ko.observable(WFCommentDTO.CreatedBy);
                    _this.RequestID = ko.observable(WFCommentDTO.RequestID);
                    _this.TaskID = ko.observable(WFCommentDTO.TaskID);
                    _this.WorkflowActivityID = ko.observable(WFCommentDTO.WorkflowActivityID);
                    _this.WorkflowActivity = ko.observable(WFCommentDTO.WorkflowActivity);
                    _this.ID = ko.observable(WFCommentDTO.ID);
                    _this.Timestamp = ko.observable(WFCommentDTO.Timestamp);
                }
                return _this;
            }
            WFCommentViewModel.prototype.toData = function () {
                return {
                    Comment: this.Comment(),
                    CreatedOn: this.CreatedOn(),
                    CreatedByID: this.CreatedByID(),
                    CreatedBy: this.CreatedBy(),
                    RequestID: this.RequestID(),
                    TaskID: this.TaskID(),
                    WorkflowActivityID: this.WorkflowActivityID(),
                    WorkflowActivity: this.WorkflowActivity(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return WFCommentViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.WFCommentViewModel = WFCommentViewModel;
        var CommentViewModel = (function (_super) {
            __extends(CommentViewModel, _super);
            function CommentViewModel(CommentDTO) {
                var _this = _super.call(this) || this;
                if (CommentDTO == null) {
                    _this.Comment = ko.observable();
                    _this.ItemID = ko.observable();
                    _this.ItemTitle = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.CreatedByID = ko.observable();
                    _this.CreatedBy = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Comment = ko.observable(CommentDTO.Comment);
                    _this.ItemID = ko.observable(CommentDTO.ItemID);
                    _this.ItemTitle = ko.observable(CommentDTO.ItemTitle);
                    _this.CreatedOn = ko.observable(CommentDTO.CreatedOn);
                    _this.CreatedByID = ko.observable(CommentDTO.CreatedByID);
                    _this.CreatedBy = ko.observable(CommentDTO.CreatedBy);
                    _this.ID = ko.observable(CommentDTO.ID);
                    _this.Timestamp = ko.observable(CommentDTO.Timestamp);
                }
                return _this;
            }
            CommentViewModel.prototype.toData = function () {
                return {
                    Comment: this.Comment(),
                    ItemID: this.ItemID(),
                    ItemTitle: this.ItemTitle(),
                    CreatedOn: this.CreatedOn(),
                    CreatedByID: this.CreatedByID(),
                    CreatedBy: this.CreatedBy(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return CommentViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.CommentViewModel = CommentViewModel;
        var DocumentViewModel = (function (_super) {
            __extends(DocumentViewModel, _super);
            function DocumentViewModel(DocumentDTO) {
                var _this = _super.call(this) || this;
                if (DocumentDTO == null) {
                    _this.Name = ko.observable();
                    _this.FileName = ko.observable();
                    _this.Viewable = ko.observable();
                    _this.MimeType = ko.observable();
                    _this.Kind = ko.observable();
                    _this.Data = ko.observable();
                    _this.Length = ko.observable();
                    _this.ItemID = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(DocumentDTO.Name);
                    _this.FileName = ko.observable(DocumentDTO.FileName);
                    _this.Viewable = ko.observable(DocumentDTO.Viewable);
                    _this.MimeType = ko.observable(DocumentDTO.MimeType);
                    _this.Kind = ko.observable(DocumentDTO.Kind);
                    _this.Data = ko.observable(DocumentDTO.Data);
                    _this.Length = ko.observable(DocumentDTO.Length);
                    _this.ItemID = ko.observable(DocumentDTO.ItemID);
                    _this.ID = ko.observable(DocumentDTO.ID);
                    _this.Timestamp = ko.observable(DocumentDTO.Timestamp);
                }
                return _this;
            }
            DocumentViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    FileName: this.FileName(),
                    Viewable: this.Viewable(),
                    MimeType: this.MimeType(),
                    Kind: this.Kind(),
                    Data: this.Data(),
                    Length: this.Length(),
                    ItemID: this.ItemID(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return DocumentViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.DocumentViewModel = DocumentViewModel;
        var ExtendedDocumentViewModel = (function (_super) {
            __extends(ExtendedDocumentViewModel, _super);
            function ExtendedDocumentViewModel(ExtendedDocumentDTO) {
                var _this = _super.call(this) || this;
                if (ExtendedDocumentDTO == null) {
                    _this.Name = ko.observable();
                    _this.FileName = ko.observable();
                    _this.Viewable = ko.observable();
                    _this.MimeType = ko.observable();
                    _this.Kind = ko.observable();
                    _this.Length = ko.observable();
                    _this.ItemID = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.ItemTitle = ko.observable();
                    _this.Description = ko.observable();
                    _this.ParentDocumentID = ko.observable();
                    _this.UploadedByID = ko.observable();
                    _this.UploadedBy = ko.observable();
                    _this.RevisionSetID = ko.observable();
                    _this.RevisionDescription = ko.observable();
                    _this.MajorVersion = ko.observable();
                    _this.MinorVersion = ko.observable();
                    _this.BuildVersion = ko.observable();
                    _this.RevisionVersion = ko.observable();
                    _this.TaskItemType = ko.observable();
                    _this.DocumentType = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(ExtendedDocumentDTO.Name);
                    _this.FileName = ko.observable(ExtendedDocumentDTO.FileName);
                    _this.Viewable = ko.observable(ExtendedDocumentDTO.Viewable);
                    _this.MimeType = ko.observable(ExtendedDocumentDTO.MimeType);
                    _this.Kind = ko.observable(ExtendedDocumentDTO.Kind);
                    _this.Length = ko.observable(ExtendedDocumentDTO.Length);
                    _this.ItemID = ko.observable(ExtendedDocumentDTO.ItemID);
                    _this.CreatedOn = ko.observable(ExtendedDocumentDTO.CreatedOn);
                    _this.ItemTitle = ko.observable(ExtendedDocumentDTO.ItemTitle);
                    _this.Description = ko.observable(ExtendedDocumentDTO.Description);
                    _this.ParentDocumentID = ko.observable(ExtendedDocumentDTO.ParentDocumentID);
                    _this.UploadedByID = ko.observable(ExtendedDocumentDTO.UploadedByID);
                    _this.UploadedBy = ko.observable(ExtendedDocumentDTO.UploadedBy);
                    _this.RevisionSetID = ko.observable(ExtendedDocumentDTO.RevisionSetID);
                    _this.RevisionDescription = ko.observable(ExtendedDocumentDTO.RevisionDescription);
                    _this.MajorVersion = ko.observable(ExtendedDocumentDTO.MajorVersion);
                    _this.MinorVersion = ko.observable(ExtendedDocumentDTO.MinorVersion);
                    _this.BuildVersion = ko.observable(ExtendedDocumentDTO.BuildVersion);
                    _this.RevisionVersion = ko.observable(ExtendedDocumentDTO.RevisionVersion);
                    _this.TaskItemType = ko.observable(ExtendedDocumentDTO.TaskItemType);
                    _this.DocumentType = ko.observable(ExtendedDocumentDTO.DocumentType);
                    _this.ID = ko.observable(ExtendedDocumentDTO.ID);
                    _this.Timestamp = ko.observable(ExtendedDocumentDTO.Timestamp);
                }
                return _this;
            }
            ExtendedDocumentViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    FileName: this.FileName(),
                    Viewable: this.Viewable(),
                    MimeType: this.MimeType(),
                    Kind: this.Kind(),
                    Length: this.Length(),
                    ItemID: this.ItemID(),
                    CreatedOn: this.CreatedOn(),
                    ItemTitle: this.ItemTitle(),
                    Description: this.Description(),
                    ParentDocumentID: this.ParentDocumentID(),
                    UploadedByID: this.UploadedByID(),
                    UploadedBy: this.UploadedBy(),
                    RevisionSetID: this.RevisionSetID(),
                    RevisionDescription: this.RevisionDescription(),
                    MajorVersion: this.MajorVersion(),
                    MinorVersion: this.MinorVersion(),
                    BuildVersion: this.BuildVersion(),
                    RevisionVersion: this.RevisionVersion(),
                    TaskItemType: this.TaskItemType(),
                    DocumentType: this.DocumentType(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ExtendedDocumentViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.ExtendedDocumentViewModel = ExtendedDocumentViewModel;
        var OrganizationEHRSViewModel = (function (_super) {
            __extends(OrganizationEHRSViewModel, _super);
            function OrganizationEHRSViewModel(OrganizationEHRSDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationEHRSDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.Type = ko.observable();
                    _this.System = ko.observable();
                    _this.Other = ko.observable();
                    _this.StartYear = ko.observable();
                    _this.EndYear = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.OrganizationID = ko.observable(OrganizationEHRSDTO.OrganizationID);
                    _this.Type = ko.observable(OrganizationEHRSDTO.Type);
                    _this.System = ko.observable(OrganizationEHRSDTO.System);
                    _this.Other = ko.observable(OrganizationEHRSDTO.Other);
                    _this.StartYear = ko.observable(OrganizationEHRSDTO.StartYear);
                    _this.EndYear = ko.observable(OrganizationEHRSDTO.EndYear);
                    _this.ID = ko.observable(OrganizationEHRSDTO.ID);
                    _this.Timestamp = ko.observable(OrganizationEHRSDTO.Timestamp);
                }
                return _this;
            }
            OrganizationEHRSViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    Type: this.Type(),
                    System: this.System(),
                    Other: this.Other(),
                    StartYear: this.StartYear(),
                    EndYear: this.EndYear(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return OrganizationEHRSViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.OrganizationEHRSViewModel = OrganizationEHRSViewModel;
        var TemplateViewModel = (function (_super) {
            __extends(TemplateViewModel, _super);
            function TemplateViewModel(TemplateDTO) {
                var _this = _super.call(this) || this;
                if (TemplateDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.CreatedByID = ko.observable();
                    _this.CreatedBy = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.Data = ko.observable();
                    _this.Type = ko.observable();
                    _this.Notes = ko.observable();
                    _this.QueryType = ko.observable();
                    _this.ComposerInterface = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(TemplateDTO.Name);
                    _this.Description = ko.observable(TemplateDTO.Description);
                    _this.CreatedByID = ko.observable(TemplateDTO.CreatedByID);
                    _this.CreatedBy = ko.observable(TemplateDTO.CreatedBy);
                    _this.CreatedOn = ko.observable(TemplateDTO.CreatedOn);
                    _this.Data = ko.observable(TemplateDTO.Data);
                    _this.Type = ko.observable(TemplateDTO.Type);
                    _this.Notes = ko.observable(TemplateDTO.Notes);
                    _this.QueryType = ko.observable(TemplateDTO.QueryType);
                    _this.ComposerInterface = ko.observable(TemplateDTO.ComposerInterface);
                    _this.ID = ko.observable(TemplateDTO.ID);
                    _this.Timestamp = ko.observable(TemplateDTO.Timestamp);
                }
                return _this;
            }
            TemplateViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    CreatedByID: this.CreatedByID(),
                    CreatedBy: this.CreatedBy(),
                    CreatedOn: this.CreatedOn(),
                    Data: this.Data(),
                    Type: this.Type(),
                    Notes: this.Notes(),
                    QueryType: this.QueryType(),
                    ComposerInterface: this.ComposerInterface(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return TemplateViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.TemplateViewModel = TemplateViewModel;
        var TermViewModel = (function (_super) {
            __extends(TermViewModel, _super);
            function TermViewModel(TermDTO) {
                var _this = _super.call(this) || this;
                if (TermDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.OID = ko.observable();
                    _this.ReferenceUrl = ko.observable();
                    _this.Type = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(TermDTO.Name);
                    _this.Description = ko.observable(TermDTO.Description);
                    _this.OID = ko.observable(TermDTO.OID);
                    _this.ReferenceUrl = ko.observable(TermDTO.ReferenceUrl);
                    _this.Type = ko.observable(TermDTO.Type);
                    _this.ID = ko.observable(TermDTO.ID);
                    _this.Timestamp = ko.observable(TermDTO.Timestamp);
                }
                return _this;
            }
            TermViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    OID: this.OID(),
                    ReferenceUrl: this.ReferenceUrl(),
                    Type: this.Type(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return TermViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.TermViewModel = TermViewModel;
        var HomepageRequestDetailViewModel = (function (_super) {
            __extends(HomepageRequestDetailViewModel, _super);
            function HomepageRequestDetailViewModel(HomepageRequestDetailDTO) {
                var _this = _super.call(this) || this;
                if (HomepageRequestDetailDTO == null) {
                    _this.Name = ko.observable();
                    _this.Identifier = ko.observable();
                    _this.SubmittedOn = ko.observable();
                    _this.SubmittedByName = ko.observable();
                    _this.SubmittedBy = ko.observable();
                    _this.SubmittedByID = ko.observable();
                    _this.StatusText = ko.observable();
                    _this.Status = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.Project = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.MSRequestID = ko.observable();
                    _this.IsWorkflowRequest = ko.observable();
                    _this.CanEditMetadata = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(HomepageRequestDetailDTO.Name);
                    _this.Identifier = ko.observable(HomepageRequestDetailDTO.Identifier);
                    _this.SubmittedOn = ko.observable(HomepageRequestDetailDTO.SubmittedOn);
                    _this.SubmittedByName = ko.observable(HomepageRequestDetailDTO.SubmittedByName);
                    _this.SubmittedBy = ko.observable(HomepageRequestDetailDTO.SubmittedBy);
                    _this.SubmittedByID = ko.observable(HomepageRequestDetailDTO.SubmittedByID);
                    _this.StatusText = ko.observable(HomepageRequestDetailDTO.StatusText);
                    _this.Status = ko.observable(HomepageRequestDetailDTO.Status);
                    _this.RequestType = ko.observable(HomepageRequestDetailDTO.RequestType);
                    _this.Project = ko.observable(HomepageRequestDetailDTO.Project);
                    _this.Priority = ko.observable(HomepageRequestDetailDTO.Priority);
                    _this.DueDate = ko.observable(HomepageRequestDetailDTO.DueDate);
                    _this.MSRequestID = ko.observable(HomepageRequestDetailDTO.MSRequestID);
                    _this.IsWorkflowRequest = ko.observable(HomepageRequestDetailDTO.IsWorkflowRequest);
                    _this.CanEditMetadata = ko.observable(HomepageRequestDetailDTO.CanEditMetadata);
                    _this.ID = ko.observable(HomepageRequestDetailDTO.ID);
                    _this.Timestamp = ko.observable(HomepageRequestDetailDTO.Timestamp);
                }
                return _this;
            }
            HomepageRequestDetailViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Identifier: this.Identifier(),
                    SubmittedOn: this.SubmittedOn(),
                    SubmittedByName: this.SubmittedByName(),
                    SubmittedBy: this.SubmittedBy(),
                    SubmittedByID: this.SubmittedByID(),
                    StatusText: this.StatusText(),
                    Status: this.Status(),
                    RequestType: this.RequestType(),
                    Project: this.Project(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    MSRequestID: this.MSRequestID(),
                    IsWorkflowRequest: this.IsWorkflowRequest(),
                    CanEditMetadata: this.CanEditMetadata(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return HomepageRequestDetailViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.HomepageRequestDetailViewModel = HomepageRequestDetailViewModel;
        var ReportAggregationLevelViewModel = (function (_super) {
            __extends(ReportAggregationLevelViewModel, _super);
            function ReportAggregationLevelViewModel(ReportAggregationLevelDTO) {
                var _this = _super.call(this) || this;
                if (ReportAggregationLevelDTO == null) {
                    _this.NetworkID = ko.observable();
                    _this.Name = ko.observable();
                    _this.DeletedOn = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.NetworkID = ko.observable(ReportAggregationLevelDTO.NetworkID);
                    _this.Name = ko.observable(ReportAggregationLevelDTO.Name);
                    _this.DeletedOn = ko.observable(ReportAggregationLevelDTO.DeletedOn);
                    _this.ID = ko.observable(ReportAggregationLevelDTO.ID);
                    _this.Timestamp = ko.observable(ReportAggregationLevelDTO.Timestamp);
                }
                return _this;
            }
            ReportAggregationLevelViewModel.prototype.toData = function () {
                return {
                    NetworkID: this.NetworkID(),
                    Name: this.Name(),
                    DeletedOn: this.DeletedOn(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ReportAggregationLevelViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.ReportAggregationLevelViewModel = ReportAggregationLevelViewModel;
        var RequestMetadataViewModel = (function (_super) {
            __extends(RequestMetadataViewModel, _super);
            function RequestMetadataViewModel(RequestMetadataDTO) {
                var _this = _super.call(this) || this;
                if (RequestMetadataDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.Priority = ko.observable();
                    _this.PurposeOfUse = ko.observable();
                    _this.PhiDisclosureLevel = ko.observable();
                    _this.RequesterCenterID = ko.observable();
                    _this.ActivityID = ko.observable();
                    _this.ActivityProjectID = ko.observable();
                    _this.TaskOrderID = ko.observable();
                    _this.SourceActivityID = ko.observable();
                    _this.SourceActivityProjectID = ko.observable();
                    _this.SourceTaskOrderID = ko.observable();
                    _this.WorkplanTypeID = ko.observable();
                    _this.MSRequestID = ko.observable();
                    _this.ReportAggregationLevelID = ko.observable();
                    _this.ApplyChangesToRoutings = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(RequestMetadataDTO.Name);
                    _this.Description = ko.observable(RequestMetadataDTO.Description);
                    _this.DueDate = ko.observable(RequestMetadataDTO.DueDate);
                    _this.Priority = ko.observable(RequestMetadataDTO.Priority);
                    _this.PurposeOfUse = ko.observable(RequestMetadataDTO.PurposeOfUse);
                    _this.PhiDisclosureLevel = ko.observable(RequestMetadataDTO.PhiDisclosureLevel);
                    _this.RequesterCenterID = ko.observable(RequestMetadataDTO.RequesterCenterID);
                    _this.ActivityID = ko.observable(RequestMetadataDTO.ActivityID);
                    _this.ActivityProjectID = ko.observable(RequestMetadataDTO.ActivityProjectID);
                    _this.TaskOrderID = ko.observable(RequestMetadataDTO.TaskOrderID);
                    _this.SourceActivityID = ko.observable(RequestMetadataDTO.SourceActivityID);
                    _this.SourceActivityProjectID = ko.observable(RequestMetadataDTO.SourceActivityProjectID);
                    _this.SourceTaskOrderID = ko.observable(RequestMetadataDTO.SourceTaskOrderID);
                    _this.WorkplanTypeID = ko.observable(RequestMetadataDTO.WorkplanTypeID);
                    _this.MSRequestID = ko.observable(RequestMetadataDTO.MSRequestID);
                    _this.ReportAggregationLevelID = ko.observable(RequestMetadataDTO.ReportAggregationLevelID);
                    _this.ApplyChangesToRoutings = ko.observable(RequestMetadataDTO.ApplyChangesToRoutings);
                    _this.ID = ko.observable(RequestMetadataDTO.ID);
                    _this.Timestamp = ko.observable(RequestMetadataDTO.Timestamp);
                }
                return _this;
            }
            RequestMetadataViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    DueDate: this.DueDate(),
                    Priority: this.Priority(),
                    PurposeOfUse: this.PurposeOfUse(),
                    PhiDisclosureLevel: this.PhiDisclosureLevel(),
                    RequesterCenterID: this.RequesterCenterID(),
                    ActivityID: this.ActivityID(),
                    ActivityProjectID: this.ActivityProjectID(),
                    TaskOrderID: this.TaskOrderID(),
                    SourceActivityID: this.SourceActivityID(),
                    SourceActivityProjectID: this.SourceActivityProjectID(),
                    SourceTaskOrderID: this.SourceTaskOrderID(),
                    WorkplanTypeID: this.WorkplanTypeID(),
                    MSRequestID: this.MSRequestID(),
                    ReportAggregationLevelID: this.ReportAggregationLevelID(),
                    ApplyChangesToRoutings: this.ApplyChangesToRoutings(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RequestMetadataViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RequestMetadataViewModel = RequestMetadataViewModel;
        var RequestObserverViewModel = (function (_super) {
            __extends(RequestObserverViewModel, _super);
            function RequestObserverViewModel(RequestObserverDTO) {
                var _this = _super.call(this) || this;
                if (RequestObserverDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.UserID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.DisplayName = ko.observable();
                    _this.Email = ko.observable();
                    _this.EventSubscriptions = ko.observableArray();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(RequestObserverDTO.RequestID);
                    _this.UserID = ko.observable(RequestObserverDTO.UserID);
                    _this.SecurityGroupID = ko.observable(RequestObserverDTO.SecurityGroupID);
                    _this.DisplayName = ko.observable(RequestObserverDTO.DisplayName);
                    _this.Email = ko.observable(RequestObserverDTO.Email);
                    _this.EventSubscriptions = ko.observableArray(RequestObserverDTO.EventSubscriptions == null ? null : RequestObserverDTO.EventSubscriptions.map(function (item) { return new RequestObserverEventSubscriptionViewModel(item); }));
                    _this.ID = ko.observable(RequestObserverDTO.ID);
                    _this.Timestamp = ko.observable(RequestObserverDTO.Timestamp);
                }
                return _this;
            }
            RequestObserverViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    UserID: this.UserID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    DisplayName: this.DisplayName(),
                    Email: this.Email(),
                    EventSubscriptions: this.EventSubscriptions == null ? null : this.EventSubscriptions().map(function (item) { return item.toData(); }),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RequestObserverViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RequestObserverViewModel = RequestObserverViewModel;
        var ResponseGroupViewModel = (function (_super) {
            __extends(ResponseGroupViewModel, _super);
            function ResponseGroupViewModel(ResponseGroupDTO) {
                var _this = _super.call(this) || this;
                if (ResponseGroupDTO == null) {
                    _this.Name = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(ResponseGroupDTO.Name);
                    _this.ID = ko.observable(ResponseGroupDTO.ID);
                    _this.Timestamp = ko.observable(ResponseGroupDTO.Timestamp);
                }
                return _this;
            }
            ResponseGroupViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ResponseGroupViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.ResponseGroupViewModel = ResponseGroupViewModel;
        var AclGlobalFieldOptionViewModel = (function (_super) {
            __extends(AclGlobalFieldOptionViewModel, _super);
            function AclGlobalFieldOptionViewModel(AclGlobalFieldOptionDTO) {
                var _this = _super.call(this) || this;
                if (AclGlobalFieldOptionDTO == null) {
                    _this.FieldIdentifier = ko.observable();
                    _this.Permission = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                }
                else {
                    _this.FieldIdentifier = ko.observable(AclGlobalFieldOptionDTO.FieldIdentifier);
                    _this.Permission = ko.observable(AclGlobalFieldOptionDTO.Permission);
                    _this.Overridden = ko.observable(AclGlobalFieldOptionDTO.Overridden);
                    _this.SecurityGroupID = ko.observable(AclGlobalFieldOptionDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclGlobalFieldOptionDTO.SecurityGroup);
                }
                return _this;
            }
            AclGlobalFieldOptionViewModel.prototype.toData = function () {
                return {
                    FieldIdentifier: this.FieldIdentifier(),
                    Permission: this.Permission(),
                    Overridden: this.Overridden(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                };
            };
            return AclGlobalFieldOptionViewModel;
        }(BaseFieldOptionAclViewModel));
        ViewModels.AclGlobalFieldOptionViewModel = AclGlobalFieldOptionViewModel;
        var AclProjectFieldOptionViewModel = (function (_super) {
            __extends(AclProjectFieldOptionViewModel, _super);
            function AclProjectFieldOptionViewModel(AclProjectFieldOptionDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectFieldOptionDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.FieldIdentifier = ko.observable();
                    _this.Permission = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectFieldOptionDTO.ProjectID);
                    _this.FieldIdentifier = ko.observable(AclProjectFieldOptionDTO.FieldIdentifier);
                    _this.Permission = ko.observable(AclProjectFieldOptionDTO.Permission);
                    _this.Overridden = ko.observable(AclProjectFieldOptionDTO.Overridden);
                    _this.SecurityGroupID = ko.observable(AclProjectFieldOptionDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectFieldOptionDTO.SecurityGroup);
                }
                return _this;
            }
            AclProjectFieldOptionViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    FieldIdentifier: this.FieldIdentifier(),
                    Permission: this.Permission(),
                    Overridden: this.Overridden(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                };
            };
            return AclProjectFieldOptionViewModel;
        }(BaseFieldOptionAclViewModel));
        ViewModels.AclProjectFieldOptionViewModel = AclProjectFieldOptionViewModel;
        var BaseAclRequestTypeViewModel = (function (_super) {
            __extends(BaseAclRequestTypeViewModel, _super);
            function BaseAclRequestTypeViewModel(BaseAclRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (BaseAclRequestTypeDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.RequestTypeID = ko.observable(BaseAclRequestTypeDTO.RequestTypeID);
                    _this.Permission = ko.observable(BaseAclRequestTypeDTO.Permission);
                    _this.SecurityGroupID = ko.observable(BaseAclRequestTypeDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(BaseAclRequestTypeDTO.SecurityGroup);
                    _this.Overridden = ko.observable(BaseAclRequestTypeDTO.Overridden);
                }
                return _this;
            }
            BaseAclRequestTypeViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return BaseAclRequestTypeViewModel;
        }(BaseAclViewModel));
        ViewModels.BaseAclRequestTypeViewModel = BaseAclRequestTypeViewModel;
        var SecurityEntityViewModel = (function (_super) {
            __extends(SecurityEntityViewModel, _super);
            function SecurityEntityViewModel(SecurityEntityDTO) {
                var _this = _super.call(this) || this;
                if (SecurityEntityDTO == null) {
                    _this.Name = ko.observable();
                    _this.Type = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(SecurityEntityDTO.Name);
                    _this.Type = ko.observable(SecurityEntityDTO.Type);
                    _this.ID = ko.observable(SecurityEntityDTO.ID);
                    _this.Timestamp = ko.observable(SecurityEntityDTO.Timestamp);
                }
                return _this;
            }
            SecurityEntityViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Type: this.Type(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return SecurityEntityViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.SecurityEntityViewModel = SecurityEntityViewModel;
        var TaskViewModel = (function (_super) {
            __extends(TaskViewModel, _super);
            function TaskViewModel(TaskDTO) {
                var _this = _super.call(this) || this;
                if (TaskDTO == null) {
                    _this.Subject = ko.observable();
                    _this.Location = ko.observable();
                    _this.Body = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.StartOn = ko.observable();
                    _this.EndOn = ko.observable();
                    _this.EstimatedCompletedOn = ko.observable();
                    _this.Priority = ko.observable();
                    _this.Status = ko.observable();
                    _this.Type = ko.observable();
                    _this.PercentComplete = ko.observable();
                    _this.WorkflowActivityID = ko.observable();
                    _this.DirectToRequest = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Subject = ko.observable(TaskDTO.Subject);
                    _this.Location = ko.observable(TaskDTO.Location);
                    _this.Body = ko.observable(TaskDTO.Body);
                    _this.DueDate = ko.observable(TaskDTO.DueDate);
                    _this.CreatedOn = ko.observable(TaskDTO.CreatedOn);
                    _this.StartOn = ko.observable(TaskDTO.StartOn);
                    _this.EndOn = ko.observable(TaskDTO.EndOn);
                    _this.EstimatedCompletedOn = ko.observable(TaskDTO.EstimatedCompletedOn);
                    _this.Priority = ko.observable(TaskDTO.Priority);
                    _this.Status = ko.observable(TaskDTO.Status);
                    _this.Type = ko.observable(TaskDTO.Type);
                    _this.PercentComplete = ko.observable(TaskDTO.PercentComplete);
                    _this.WorkflowActivityID = ko.observable(TaskDTO.WorkflowActivityID);
                    _this.DirectToRequest = ko.observable(TaskDTO.DirectToRequest);
                    _this.ID = ko.observable(TaskDTO.ID);
                    _this.Timestamp = ko.observable(TaskDTO.Timestamp);
                }
                return _this;
            }
            TaskViewModel.prototype.toData = function () {
                return {
                    Subject: this.Subject(),
                    Location: this.Location(),
                    Body: this.Body(),
                    DueDate: this.DueDate(),
                    CreatedOn: this.CreatedOn(),
                    StartOn: this.StartOn(),
                    EndOn: this.EndOn(),
                    EstimatedCompletedOn: this.EstimatedCompletedOn(),
                    Priority: this.Priority(),
                    Status: this.Status(),
                    Type: this.Type(),
                    PercentComplete: this.PercentComplete(),
                    WorkflowActivityID: this.WorkflowActivityID(),
                    DirectToRequest: this.DirectToRequest(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return TaskViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.TaskViewModel = TaskViewModel;
        var DataModelViewModel = (function (_super) {
            __extends(DataModelViewModel, _super);
            function DataModelViewModel(DataModelDTO) {
                var _this = _super.call(this) || this;
                if (DataModelDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.RequiresConfiguration = ko.observable();
                    _this.QueryComposer = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(DataModelDTO.Name);
                    _this.Description = ko.observable(DataModelDTO.Description);
                    _this.RequiresConfiguration = ko.observable(DataModelDTO.RequiresConfiguration);
                    _this.QueryComposer = ko.observable(DataModelDTO.QueryComposer);
                    _this.ID = ko.observable(DataModelDTO.ID);
                    _this.Timestamp = ko.observable(DataModelDTO.Timestamp);
                }
                return _this;
            }
            DataModelViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    RequiresConfiguration: this.RequiresConfiguration(),
                    QueryComposer: this.QueryComposer(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return DataModelViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.DataModelViewModel = DataModelViewModel;
        var DataMartListViewModel = (function (_super) {
            __extends(DataMartListViewModel, _super);
            function DataMartListViewModel(DataMartListDTO) {
                var _this = _super.call(this) || this;
                if (DataMartListDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.StartDate = ko.observable();
                    _this.EndDate = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.ParentOrganziationID = ko.observable();
                    _this.ParentOrganization = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(DataMartListDTO.Name);
                    _this.Description = ko.observable(DataMartListDTO.Description);
                    _this.Acronym = ko.observable(DataMartListDTO.Acronym);
                    _this.StartDate = ko.observable(DataMartListDTO.StartDate);
                    _this.EndDate = ko.observable(DataMartListDTO.EndDate);
                    _this.OrganizationID = ko.observable(DataMartListDTO.OrganizationID);
                    _this.Organization = ko.observable(DataMartListDTO.Organization);
                    _this.ParentOrganziationID = ko.observable(DataMartListDTO.ParentOrganziationID);
                    _this.ParentOrganization = ko.observable(DataMartListDTO.ParentOrganization);
                    _this.Priority = ko.observable(DataMartListDTO.Priority);
                    _this.DueDate = ko.observable(DataMartListDTO.DueDate);
                    _this.ID = ko.observable(DataMartListDTO.ID);
                    _this.Timestamp = ko.observable(DataMartListDTO.Timestamp);
                }
                return _this;
            }
            DataMartListViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Acronym: this.Acronym(),
                    StartDate: this.StartDate(),
                    EndDate: this.EndDate(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    ParentOrganziationID: this.ParentOrganziationID(),
                    ParentOrganization: this.ParentOrganization(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return DataMartListViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.DataMartListViewModel = DataMartListViewModel;
        var EventViewModel = (function (_super) {
            __extends(EventViewModel, _super);
            function EventViewModel(EventDTO) {
                var _this = _super.call(this) || this;
                if (EventDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Locations = ko.observableArray();
                    _this.SupportsMyNotifications = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(EventDTO.Name);
                    _this.Description = ko.observable(EventDTO.Description);
                    _this.Locations = ko.observableArray(EventDTO.Locations == null ? null : EventDTO.Locations.map(function (item) { return item; }));
                    _this.SupportsMyNotifications = ko.observable(EventDTO.SupportsMyNotifications);
                    _this.ID = ko.observable(EventDTO.ID);
                    _this.Timestamp = ko.observable(EventDTO.Timestamp);
                }
                return _this;
            }
            EventViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Locations: this.Locations(),
                    SupportsMyNotifications: this.SupportsMyNotifications(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return EventViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.EventViewModel = EventViewModel;
        var GroupEventViewModel = (function (_super) {
            __extends(GroupEventViewModel, _super);
            function GroupEventViewModel(GroupEventDTO) {
                var _this = _super.call(this) || this;
                if (GroupEventDTO == null) {
                    _this.GroupID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.GroupID = ko.observable(GroupEventDTO.GroupID);
                    _this.SecurityGroupID = ko.observable(GroupEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(GroupEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(GroupEventDTO.Allowed);
                    _this.Overridden = ko.observable(GroupEventDTO.Overridden);
                    _this.EventID = ko.observable(GroupEventDTO.EventID);
                    _this.Event = ko.observable(GroupEventDTO.Event);
                }
                return _this;
            }
            GroupEventViewModel.prototype.toData = function () {
                return {
                    GroupID: this.GroupID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return GroupEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.GroupEventViewModel = GroupEventViewModel;
        var OrganizationEventViewModel = (function (_super) {
            __extends(OrganizationEventViewModel, _super);
            function OrganizationEventViewModel(OrganizationEventDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationEventDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.OrganizationID = ko.observable(OrganizationEventDTO.OrganizationID);
                    _this.SecurityGroupID = ko.observable(OrganizationEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(OrganizationEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(OrganizationEventDTO.Allowed);
                    _this.Overridden = ko.observable(OrganizationEventDTO.Overridden);
                    _this.EventID = ko.observable(OrganizationEventDTO.EventID);
                    _this.Event = ko.observable(OrganizationEventDTO.Event);
                }
                return _this;
            }
            OrganizationEventViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return OrganizationEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.OrganizationEventViewModel = OrganizationEventViewModel;
        var RegistryEventViewModel = (function (_super) {
            __extends(RegistryEventViewModel, _super);
            function RegistryEventViewModel(RegistryEventDTO) {
                var _this = _super.call(this) || this;
                if (RegistryEventDTO == null) {
                    _this.RegistryID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.RegistryID = ko.observable(RegistryEventDTO.RegistryID);
                    _this.SecurityGroupID = ko.observable(RegistryEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(RegistryEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(RegistryEventDTO.Allowed);
                    _this.Overridden = ko.observable(RegistryEventDTO.Overridden);
                    _this.EventID = ko.observable(RegistryEventDTO.EventID);
                    _this.Event = ko.observable(RegistryEventDTO.Event);
                }
                return _this;
            }
            RegistryEventViewModel.prototype.toData = function () {
                return {
                    RegistryID: this.RegistryID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return RegistryEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.RegistryEventViewModel = RegistryEventViewModel;
        var UserEventViewModel = (function (_super) {
            __extends(UserEventViewModel, _super);
            function UserEventViewModel(UserEventDTO) {
                var _this = _super.call(this) || this;
                if (UserEventDTO == null) {
                    _this.UserID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.UserID = ko.observable(UserEventDTO.UserID);
                    _this.SecurityGroupID = ko.observable(UserEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(UserEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(UserEventDTO.Allowed);
                    _this.Overridden = ko.observable(UserEventDTO.Overridden);
                    _this.EventID = ko.observable(UserEventDTO.EventID);
                    _this.Event = ko.observable(UserEventDTO.Event);
                }
                return _this;
            }
            UserEventViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return UserEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.UserEventViewModel = UserEventViewModel;
        var GroupViewModel = (function (_super) {
            __extends(GroupViewModel, _super);
            function GroupViewModel(GroupDTO) {
                var _this = _super.call(this) || this;
                if (GroupDTO == null) {
                    _this.Name = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.ApprovalRequired = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(GroupDTO.Name);
                    _this.Deleted = ko.observable(GroupDTO.Deleted);
                    _this.ApprovalRequired = ko.observable(GroupDTO.ApprovalRequired);
                    _this.ID = ko.observable(GroupDTO.ID);
                    _this.Timestamp = ko.observable(GroupDTO.Timestamp);
                }
                return _this;
            }
            GroupViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Deleted: this.Deleted(),
                    ApprovalRequired: this.ApprovalRequired(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return GroupViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.GroupViewModel = GroupViewModel;
        var NetworkMessageViewModel = (function (_super) {
            __extends(NetworkMessageViewModel, _super);
            function NetworkMessageViewModel(NetworkMessageDTO) {
                var _this = _super.call(this) || this;
                if (NetworkMessageDTO == null) {
                    _this.Subject = ko.observable();
                    _this.MessageText = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.Targets = ko.observableArray();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Subject = ko.observable(NetworkMessageDTO.Subject);
                    _this.MessageText = ko.observable(NetworkMessageDTO.MessageText);
                    _this.CreatedOn = ko.observable(NetworkMessageDTO.CreatedOn);
                    _this.Targets = ko.observableArray(NetworkMessageDTO.Targets == null ? null : NetworkMessageDTO.Targets.map(function (item) { return item; }));
                    _this.ID = ko.observable(NetworkMessageDTO.ID);
                    _this.Timestamp = ko.observable(NetworkMessageDTO.Timestamp);
                }
                return _this;
            }
            NetworkMessageViewModel.prototype.toData = function () {
                return {
                    Subject: this.Subject(),
                    MessageText: this.MessageText(),
                    CreatedOn: this.CreatedOn(),
                    Targets: this.Targets(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return NetworkMessageViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.NetworkMessageViewModel = NetworkMessageViewModel;
        var OrganizationViewModel = (function (_super) {
            __extends(OrganizationViewModel, _super);
            function OrganizationViewModel(OrganizationDTO) {
                var _this = _super.call(this) || this;
                if (OrganizationDTO == null) {
                    _this.Name = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.Primary = ko.observable();
                    _this.ParentOrganizationID = ko.observable();
                    _this.ParentOrganization = ko.observable();
                    _this.ContactEmail = ko.observable();
                    _this.ContactFirstName = ko.observable();
                    _this.ContactLastName = ko.observable();
                    _this.ContactPhone = ko.observable();
                    _this.SpecialRequirements = ko.observable();
                    _this.UsageRestrictions = ko.observable();
                    _this.OrganizationDescription = ko.observable();
                    _this.PragmaticClinicalTrials = ko.observable();
                    _this.ObservationalParticipation = ko.observable();
                    _this.ProspectiveTrials = ko.observable();
                    _this.EnableClaimsAndBilling = ko.observable();
                    _this.EnableEHRA = ko.observable();
                    _this.EnableRegistries = ko.observable();
                    _this.DataModelMSCDM = ko.observable();
                    _this.DataModelHMORNVDW = ko.observable();
                    _this.DataModelESP = ko.observable();
                    _this.DataModelI2B2 = ko.observable();
                    _this.DataModelOMOP = ko.observable();
                    _this.DataModelPCORI = ko.observable();
                    _this.DataModelOther = ko.observable();
                    _this.DataModelOtherText = ko.observable();
                    _this.InpatientClaims = ko.observable();
                    _this.OutpatientClaims = ko.observable();
                    _this.OutpatientPharmacyClaims = ko.observable();
                    _this.EnrollmentClaims = ko.observable();
                    _this.DemographicsClaims = ko.observable();
                    _this.LaboratoryResultsClaims = ko.observable();
                    _this.VitalSignsClaims = ko.observable();
                    _this.OtherClaims = ko.observable();
                    _this.OtherClaimsText = ko.observable();
                    _this.Biorepositories = ko.observable();
                    _this.PatientReportedOutcomes = ko.observable();
                    _this.PatientReportedBehaviors = ko.observable();
                    _this.PrescriptionOrders = ko.observable();
                    _this.X509PublicKey = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(OrganizationDTO.Name);
                    _this.Acronym = ko.observable(OrganizationDTO.Acronym);
                    _this.Deleted = ko.observable(OrganizationDTO.Deleted);
                    _this.Primary = ko.observable(OrganizationDTO.Primary);
                    _this.ParentOrganizationID = ko.observable(OrganizationDTO.ParentOrganizationID);
                    _this.ParentOrganization = ko.observable(OrganizationDTO.ParentOrganization);
                    _this.ContactEmail = ko.observable(OrganizationDTO.ContactEmail);
                    _this.ContactFirstName = ko.observable(OrganizationDTO.ContactFirstName);
                    _this.ContactLastName = ko.observable(OrganizationDTO.ContactLastName);
                    _this.ContactPhone = ko.observable(OrganizationDTO.ContactPhone);
                    _this.SpecialRequirements = ko.observable(OrganizationDTO.SpecialRequirements);
                    _this.UsageRestrictions = ko.observable(OrganizationDTO.UsageRestrictions);
                    _this.OrganizationDescription = ko.observable(OrganizationDTO.OrganizationDescription);
                    _this.PragmaticClinicalTrials = ko.observable(OrganizationDTO.PragmaticClinicalTrials);
                    _this.ObservationalParticipation = ko.observable(OrganizationDTO.ObservationalParticipation);
                    _this.ProspectiveTrials = ko.observable(OrganizationDTO.ProspectiveTrials);
                    _this.EnableClaimsAndBilling = ko.observable(OrganizationDTO.EnableClaimsAndBilling);
                    _this.EnableEHRA = ko.observable(OrganizationDTO.EnableEHRA);
                    _this.EnableRegistries = ko.observable(OrganizationDTO.EnableRegistries);
                    _this.DataModelMSCDM = ko.observable(OrganizationDTO.DataModelMSCDM);
                    _this.DataModelHMORNVDW = ko.observable(OrganizationDTO.DataModelHMORNVDW);
                    _this.DataModelESP = ko.observable(OrganizationDTO.DataModelESP);
                    _this.DataModelI2B2 = ko.observable(OrganizationDTO.DataModelI2B2);
                    _this.DataModelOMOP = ko.observable(OrganizationDTO.DataModelOMOP);
                    _this.DataModelPCORI = ko.observable(OrganizationDTO.DataModelPCORI);
                    _this.DataModelOther = ko.observable(OrganizationDTO.DataModelOther);
                    _this.DataModelOtherText = ko.observable(OrganizationDTO.DataModelOtherText);
                    _this.InpatientClaims = ko.observable(OrganizationDTO.InpatientClaims);
                    _this.OutpatientClaims = ko.observable(OrganizationDTO.OutpatientClaims);
                    _this.OutpatientPharmacyClaims = ko.observable(OrganizationDTO.OutpatientPharmacyClaims);
                    _this.EnrollmentClaims = ko.observable(OrganizationDTO.EnrollmentClaims);
                    _this.DemographicsClaims = ko.observable(OrganizationDTO.DemographicsClaims);
                    _this.LaboratoryResultsClaims = ko.observable(OrganizationDTO.LaboratoryResultsClaims);
                    _this.VitalSignsClaims = ko.observable(OrganizationDTO.VitalSignsClaims);
                    _this.OtherClaims = ko.observable(OrganizationDTO.OtherClaims);
                    _this.OtherClaimsText = ko.observable(OrganizationDTO.OtherClaimsText);
                    _this.Biorepositories = ko.observable(OrganizationDTO.Biorepositories);
                    _this.PatientReportedOutcomes = ko.observable(OrganizationDTO.PatientReportedOutcomes);
                    _this.PatientReportedBehaviors = ko.observable(OrganizationDTO.PatientReportedBehaviors);
                    _this.PrescriptionOrders = ko.observable(OrganizationDTO.PrescriptionOrders);
                    _this.X509PublicKey = ko.observable(OrganizationDTO.X509PublicKey);
                    _this.ID = ko.observable(OrganizationDTO.ID);
                    _this.Timestamp = ko.observable(OrganizationDTO.Timestamp);
                }
                return _this;
            }
            OrganizationViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Acronym: this.Acronym(),
                    Deleted: this.Deleted(),
                    Primary: this.Primary(),
                    ParentOrganizationID: this.ParentOrganizationID(),
                    ParentOrganization: this.ParentOrganization(),
                    ContactEmail: this.ContactEmail(),
                    ContactFirstName: this.ContactFirstName(),
                    ContactLastName: this.ContactLastName(),
                    ContactPhone: this.ContactPhone(),
                    SpecialRequirements: this.SpecialRequirements(),
                    UsageRestrictions: this.UsageRestrictions(),
                    OrganizationDescription: this.OrganizationDescription(),
                    PragmaticClinicalTrials: this.PragmaticClinicalTrials(),
                    ObservationalParticipation: this.ObservationalParticipation(),
                    ProspectiveTrials: this.ProspectiveTrials(),
                    EnableClaimsAndBilling: this.EnableClaimsAndBilling(),
                    EnableEHRA: this.EnableEHRA(),
                    EnableRegistries: this.EnableRegistries(),
                    DataModelMSCDM: this.DataModelMSCDM(),
                    DataModelHMORNVDW: this.DataModelHMORNVDW(),
                    DataModelESP: this.DataModelESP(),
                    DataModelI2B2: this.DataModelI2B2(),
                    DataModelOMOP: this.DataModelOMOP(),
                    DataModelPCORI: this.DataModelPCORI(),
                    DataModelOther: this.DataModelOther(),
                    DataModelOtherText: this.DataModelOtherText(),
                    InpatientClaims: this.InpatientClaims(),
                    OutpatientClaims: this.OutpatientClaims(),
                    OutpatientPharmacyClaims: this.OutpatientPharmacyClaims(),
                    EnrollmentClaims: this.EnrollmentClaims(),
                    DemographicsClaims: this.DemographicsClaims(),
                    LaboratoryResultsClaims: this.LaboratoryResultsClaims(),
                    VitalSignsClaims: this.VitalSignsClaims(),
                    OtherClaims: this.OtherClaims(),
                    OtherClaimsText: this.OtherClaimsText(),
                    Biorepositories: this.Biorepositories(),
                    PatientReportedOutcomes: this.PatientReportedOutcomes(),
                    PatientReportedBehaviors: this.PatientReportedBehaviors(),
                    PrescriptionOrders: this.PrescriptionOrders(),
                    X509PublicKey: this.X509PublicKey(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return OrganizationViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.OrganizationViewModel = OrganizationViewModel;
        var ProjectViewModel = (function (_super) {
            __extends(ProjectViewModel, _super);
            function ProjectViewModel(ProjectDTO) {
                var _this = _super.call(this) || this;
                if (ProjectDTO == null) {
                    _this.Name = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.StartDate = ko.observable();
                    _this.EndDate = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.Active = ko.observable();
                    _this.Description = ko.observable();
                    _this.GroupID = ko.observable();
                    _this.Group = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(ProjectDTO.Name);
                    _this.Acronym = ko.observable(ProjectDTO.Acronym);
                    _this.StartDate = ko.observable(ProjectDTO.StartDate);
                    _this.EndDate = ko.observable(ProjectDTO.EndDate);
                    _this.Deleted = ko.observable(ProjectDTO.Deleted);
                    _this.Active = ko.observable(ProjectDTO.Active);
                    _this.Description = ko.observable(ProjectDTO.Description);
                    _this.GroupID = ko.observable(ProjectDTO.GroupID);
                    _this.Group = ko.observable(ProjectDTO.Group);
                    _this.ID = ko.observable(ProjectDTO.ID);
                    _this.Timestamp = ko.observable(ProjectDTO.Timestamp);
                }
                return _this;
            }
            ProjectViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Acronym: this.Acronym(),
                    StartDate: this.StartDate(),
                    EndDate: this.EndDate(),
                    Deleted: this.Deleted(),
                    Active: this.Active(),
                    Description: this.Description(),
                    GroupID: this.GroupID(),
                    Group: this.Group(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ProjectViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.ProjectViewModel = ProjectViewModel;
        var RegistryViewModel = (function (_super) {
            __extends(RegistryViewModel, _super);
            function RegistryViewModel(RegistryDTO) {
                var _this = _super.call(this) || this;
                if (RegistryDTO == null) {
                    _this.Deleted = ko.observable();
                    _this.Type = ko.observable();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.RoPRUrl = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Deleted = ko.observable(RegistryDTO.Deleted);
                    _this.Type = ko.observable(RegistryDTO.Type);
                    _this.Name = ko.observable(RegistryDTO.Name);
                    _this.Description = ko.observable(RegistryDTO.Description);
                    _this.RoPRUrl = ko.observable(RegistryDTO.RoPRUrl);
                    _this.ID = ko.observable(RegistryDTO.ID);
                    _this.Timestamp = ko.observable(RegistryDTO.Timestamp);
                }
                return _this;
            }
            RegistryViewModel.prototype.toData = function () {
                return {
                    Deleted: this.Deleted(),
                    Type: this.Type(),
                    Name: this.Name(),
                    Description: this.Description(),
                    RoPRUrl: this.RoPRUrl(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RegistryViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RegistryViewModel = RegistryViewModel;
        var RequestDataMartViewModel = (function (_super) {
            __extends(RequestDataMartViewModel, _super);
            function RequestDataMartViewModel(RequestDataMartDTO) {
                var _this = _super.call(this) || this;
                if (RequestDataMartDTO == null) {
                    _this.RequestID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.Status = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.RequestTime = ko.observable();
                    _this.ResponseTime = ko.observable();
                    _this.ErrorMessage = ko.observable();
                    _this.ErrorDetail = ko.observable();
                    _this.RejectReason = ko.observable();
                    _this.ResultsGrouped = ko.observable();
                    _this.Properties = ko.observable();
                    _this.RoutingType = ko.observable();
                    _this.ResponseID = ko.observable();
                    _this.ResponseGroupID = ko.observable();
                    _this.ResponseGroup = ko.observable();
                    _this.ResponseMessage = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.RequestID = ko.observable(RequestDataMartDTO.RequestID);
                    _this.DataMartID = ko.observable(RequestDataMartDTO.DataMartID);
                    _this.DataMart = ko.observable(RequestDataMartDTO.DataMart);
                    _this.Status = ko.observable(RequestDataMartDTO.Status);
                    _this.Priority = ko.observable(RequestDataMartDTO.Priority);
                    _this.DueDate = ko.observable(RequestDataMartDTO.DueDate);
                    _this.RequestTime = ko.observable(RequestDataMartDTO.RequestTime);
                    _this.ResponseTime = ko.observable(RequestDataMartDTO.ResponseTime);
                    _this.ErrorMessage = ko.observable(RequestDataMartDTO.ErrorMessage);
                    _this.ErrorDetail = ko.observable(RequestDataMartDTO.ErrorDetail);
                    _this.RejectReason = ko.observable(RequestDataMartDTO.RejectReason);
                    _this.ResultsGrouped = ko.observable(RequestDataMartDTO.ResultsGrouped);
                    _this.Properties = ko.observable(RequestDataMartDTO.Properties);
                    _this.RoutingType = ko.observable(RequestDataMartDTO.RoutingType);
                    _this.ResponseID = ko.observable(RequestDataMartDTO.ResponseID);
                    _this.ResponseGroupID = ko.observable(RequestDataMartDTO.ResponseGroupID);
                    _this.ResponseGroup = ko.observable(RequestDataMartDTO.ResponseGroup);
                    _this.ResponseMessage = ko.observable(RequestDataMartDTO.ResponseMessage);
                    _this.ID = ko.observable(RequestDataMartDTO.ID);
                    _this.Timestamp = ko.observable(RequestDataMartDTO.Timestamp);
                }
                return _this;
            }
            RequestDataMartViewModel.prototype.toData = function () {
                return {
                    RequestID: this.RequestID(),
                    DataMartID: this.DataMartID(),
                    DataMart: this.DataMart(),
                    Status: this.Status(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    RequestTime: this.RequestTime(),
                    ResponseTime: this.ResponseTime(),
                    ErrorMessage: this.ErrorMessage(),
                    ErrorDetail: this.ErrorDetail(),
                    RejectReason: this.RejectReason(),
                    ResultsGrouped: this.ResultsGrouped(),
                    Properties: this.Properties(),
                    RoutingType: this.RoutingType(),
                    ResponseID: this.ResponseID(),
                    ResponseGroupID: this.ResponseGroupID(),
                    ResponseGroup: this.ResponseGroup(),
                    ResponseMessage: this.ResponseMessage(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RequestDataMartViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RequestDataMartViewModel = RequestDataMartViewModel;
        var RequestViewModel = (function (_super) {
            __extends(RequestViewModel, _super);
            function RequestViewModel(RequestDTO) {
                var _this = _super.call(this) || this;
                if (RequestDTO == null) {
                    _this.Identifier = ko.observable();
                    _this.MSRequestID = ko.observable();
                    _this.ProjectID = ko.observable();
                    _this.Project = ko.observable();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.AdditionalInstructions = ko.observable();
                    _this.UpdatedOn = ko.observable();
                    _this.UpdatedByID = ko.observable();
                    _this.UpdatedBy = ko.observable();
                    _this.MirrorBudgetFields = ko.observable();
                    _this.Scheduled = ko.observable();
                    _this.Template = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.Priority = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.PurposeOfUse = ko.observable();
                    _this.PhiDisclosureLevel = ko.observable();
                    _this.ReportAggregationLevelID = ko.observable();
                    _this.ReportAggregationLevel = ko.observable();
                    _this.Schedule = ko.observable();
                    _this.ScheduleCount = ko.observable();
                    _this.SubmittedOn = ko.observable();
                    _this.SubmittedByID = ko.observable();
                    _this.SubmittedByName = ko.observable();
                    _this.SubmittedBy = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.AdapterPackageVersion = ko.observable();
                    _this.IRBApprovalNo = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.ActivityDescription = ko.observable();
                    _this.ActivityID = ko.observable();
                    _this.SourceActivityID = ko.observable();
                    _this.SourceActivityProjectID = ko.observable();
                    _this.SourceTaskOrderID = ko.observable();
                    _this.RequesterCenterID = ko.observable();
                    _this.RequesterCenter = ko.observable();
                    _this.WorkplanTypeID = ko.observable();
                    _this.WorkplanType = ko.observable();
                    _this.WorkflowID = ko.observable();
                    _this.Workflow = ko.observable();
                    _this.CurrentWorkFlowActivityID = ko.observable();
                    _this.CurrentWorkFlowActivity = ko.observable();
                    _this.Status = ko.observable();
                    _this.StatusText = ko.observable();
                    _this.MajorEventDate = ko.observable();
                    _this.MajorEventByID = ko.observable();
                    _this.MajorEventBy = ko.observable();
                    _this.CreatedOn = ko.observable();
                    _this.CreatedByID = ko.observable();
                    _this.CreatedBy = ko.observable();
                    _this.CompletedOn = ko.observable();
                    _this.CancelledOn = ko.observable();
                    _this.UserIdentifier = ko.observable();
                    _this.Query = ko.observable();
                    _this.ParentRequestID = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Identifier = ko.observable(RequestDTO.Identifier);
                    _this.MSRequestID = ko.observable(RequestDTO.MSRequestID);
                    _this.ProjectID = ko.observable(RequestDTO.ProjectID);
                    _this.Project = ko.observable(RequestDTO.Project);
                    _this.Name = ko.observable(RequestDTO.Name);
                    _this.Description = ko.observable(RequestDTO.Description);
                    _this.AdditionalInstructions = ko.observable(RequestDTO.AdditionalInstructions);
                    _this.UpdatedOn = ko.observable(RequestDTO.UpdatedOn);
                    _this.UpdatedByID = ko.observable(RequestDTO.UpdatedByID);
                    _this.UpdatedBy = ko.observable(RequestDTO.UpdatedBy);
                    _this.MirrorBudgetFields = ko.observable(RequestDTO.MirrorBudgetFields);
                    _this.Scheduled = ko.observable(RequestDTO.Scheduled);
                    _this.Template = ko.observable(RequestDTO.Template);
                    _this.Deleted = ko.observable(RequestDTO.Deleted);
                    _this.Priority = ko.observable(RequestDTO.Priority);
                    _this.OrganizationID = ko.observable(RequestDTO.OrganizationID);
                    _this.Organization = ko.observable(RequestDTO.Organization);
                    _this.PurposeOfUse = ko.observable(RequestDTO.PurposeOfUse);
                    _this.PhiDisclosureLevel = ko.observable(RequestDTO.PhiDisclosureLevel);
                    _this.ReportAggregationLevelID = ko.observable(RequestDTO.ReportAggregationLevelID);
                    _this.ReportAggregationLevel = ko.observable(RequestDTO.ReportAggregationLevel);
                    _this.Schedule = ko.observable(RequestDTO.Schedule);
                    _this.ScheduleCount = ko.observable(RequestDTO.ScheduleCount);
                    _this.SubmittedOn = ko.observable(RequestDTO.SubmittedOn);
                    _this.SubmittedByID = ko.observable(RequestDTO.SubmittedByID);
                    _this.SubmittedByName = ko.observable(RequestDTO.SubmittedByName);
                    _this.SubmittedBy = ko.observable(RequestDTO.SubmittedBy);
                    _this.RequestTypeID = ko.observable(RequestDTO.RequestTypeID);
                    _this.RequestType = ko.observable(RequestDTO.RequestType);
                    _this.AdapterPackageVersion = ko.observable(RequestDTO.AdapterPackageVersion);
                    _this.IRBApprovalNo = ko.observable(RequestDTO.IRBApprovalNo);
                    _this.DueDate = ko.observable(RequestDTO.DueDate);
                    _this.ActivityDescription = ko.observable(RequestDTO.ActivityDescription);
                    _this.ActivityID = ko.observable(RequestDTO.ActivityID);
                    _this.SourceActivityID = ko.observable(RequestDTO.SourceActivityID);
                    _this.SourceActivityProjectID = ko.observable(RequestDTO.SourceActivityProjectID);
                    _this.SourceTaskOrderID = ko.observable(RequestDTO.SourceTaskOrderID);
                    _this.RequesterCenterID = ko.observable(RequestDTO.RequesterCenterID);
                    _this.RequesterCenter = ko.observable(RequestDTO.RequesterCenter);
                    _this.WorkplanTypeID = ko.observable(RequestDTO.WorkplanTypeID);
                    _this.WorkplanType = ko.observable(RequestDTO.WorkplanType);
                    _this.WorkflowID = ko.observable(RequestDTO.WorkflowID);
                    _this.Workflow = ko.observable(RequestDTO.Workflow);
                    _this.CurrentWorkFlowActivityID = ko.observable(RequestDTO.CurrentWorkFlowActivityID);
                    _this.CurrentWorkFlowActivity = ko.observable(RequestDTO.CurrentWorkFlowActivity);
                    _this.Status = ko.observable(RequestDTO.Status);
                    _this.StatusText = ko.observable(RequestDTO.StatusText);
                    _this.MajorEventDate = ko.observable(RequestDTO.MajorEventDate);
                    _this.MajorEventByID = ko.observable(RequestDTO.MajorEventByID);
                    _this.MajorEventBy = ko.observable(RequestDTO.MajorEventBy);
                    _this.CreatedOn = ko.observable(RequestDTO.CreatedOn);
                    _this.CreatedByID = ko.observable(RequestDTO.CreatedByID);
                    _this.CreatedBy = ko.observable(RequestDTO.CreatedBy);
                    _this.CompletedOn = ko.observable(RequestDTO.CompletedOn);
                    _this.CancelledOn = ko.observable(RequestDTO.CancelledOn);
                    _this.UserIdentifier = ko.observable(RequestDTO.UserIdentifier);
                    _this.Query = ko.observable(RequestDTO.Query);
                    _this.ParentRequestID = ko.observable(RequestDTO.ParentRequestID);
                    _this.ID = ko.observable(RequestDTO.ID);
                    _this.Timestamp = ko.observable(RequestDTO.Timestamp);
                }
                return _this;
            }
            RequestViewModel.prototype.toData = function () {
                return {
                    Identifier: this.Identifier(),
                    MSRequestID: this.MSRequestID(),
                    ProjectID: this.ProjectID(),
                    Project: this.Project(),
                    Name: this.Name(),
                    Description: this.Description(),
                    AdditionalInstructions: this.AdditionalInstructions(),
                    UpdatedOn: this.UpdatedOn(),
                    UpdatedByID: this.UpdatedByID(),
                    UpdatedBy: this.UpdatedBy(),
                    MirrorBudgetFields: this.MirrorBudgetFields(),
                    Scheduled: this.Scheduled(),
                    Template: this.Template(),
                    Deleted: this.Deleted(),
                    Priority: this.Priority(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    PurposeOfUse: this.PurposeOfUse(),
                    PhiDisclosureLevel: this.PhiDisclosureLevel(),
                    ReportAggregationLevelID: this.ReportAggregationLevelID(),
                    ReportAggregationLevel: this.ReportAggregationLevel(),
                    Schedule: this.Schedule(),
                    ScheduleCount: this.ScheduleCount(),
                    SubmittedOn: this.SubmittedOn(),
                    SubmittedByID: this.SubmittedByID(),
                    SubmittedByName: this.SubmittedByName(),
                    SubmittedBy: this.SubmittedBy(),
                    RequestTypeID: this.RequestTypeID(),
                    RequestType: this.RequestType(),
                    AdapterPackageVersion: this.AdapterPackageVersion(),
                    IRBApprovalNo: this.IRBApprovalNo(),
                    DueDate: this.DueDate(),
                    ActivityDescription: this.ActivityDescription(),
                    ActivityID: this.ActivityID(),
                    SourceActivityID: this.SourceActivityID(),
                    SourceActivityProjectID: this.SourceActivityProjectID(),
                    SourceTaskOrderID: this.SourceTaskOrderID(),
                    RequesterCenterID: this.RequesterCenterID(),
                    RequesterCenter: this.RequesterCenter(),
                    WorkplanTypeID: this.WorkplanTypeID(),
                    WorkplanType: this.WorkplanType(),
                    WorkflowID: this.WorkflowID(),
                    Workflow: this.Workflow(),
                    CurrentWorkFlowActivityID: this.CurrentWorkFlowActivityID(),
                    CurrentWorkFlowActivity: this.CurrentWorkFlowActivity(),
                    Status: this.Status(),
                    StatusText: this.StatusText(),
                    MajorEventDate: this.MajorEventDate(),
                    MajorEventByID: this.MajorEventByID(),
                    MajorEventBy: this.MajorEventBy(),
                    CreatedOn: this.CreatedOn(),
                    CreatedByID: this.CreatedByID(),
                    CreatedBy: this.CreatedBy(),
                    CompletedOn: this.CompletedOn(),
                    CancelledOn: this.CancelledOn(),
                    UserIdentifier: this.UserIdentifier(),
                    Query: this.Query(),
                    ParentRequestID: this.ParentRequestID(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RequestViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RequestViewModel = RequestViewModel;
        var RequestTypeViewModel = (function (_super) {
            __extends(RequestTypeViewModel, _super);
            function RequestTypeViewModel(RequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (RequestTypeDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Metadata = ko.observable();
                    _this.PostProcess = ko.observable();
                    _this.AddFiles = ko.observable();
                    _this.RequiresProcessing = ko.observable();
                    _this.TemplateID = ko.observable();
                    _this.Template = ko.observable();
                    _this.WorkflowID = ko.observable();
                    _this.Workflow = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(RequestTypeDTO.Name);
                    _this.Description = ko.observable(RequestTypeDTO.Description);
                    _this.Metadata = ko.observable(RequestTypeDTO.Metadata);
                    _this.PostProcess = ko.observable(RequestTypeDTO.PostProcess);
                    _this.AddFiles = ko.observable(RequestTypeDTO.AddFiles);
                    _this.RequiresProcessing = ko.observable(RequestTypeDTO.RequiresProcessing);
                    _this.TemplateID = ko.observable(RequestTypeDTO.TemplateID);
                    _this.Template = ko.observable(RequestTypeDTO.Template);
                    _this.WorkflowID = ko.observable(RequestTypeDTO.WorkflowID);
                    _this.Workflow = ko.observable(RequestTypeDTO.Workflow);
                    _this.ID = ko.observable(RequestTypeDTO.ID);
                    _this.Timestamp = ko.observable(RequestTypeDTO.Timestamp);
                }
                return _this;
            }
            RequestTypeViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Metadata: this.Metadata(),
                    PostProcess: this.PostProcess(),
                    AddFiles: this.AddFiles(),
                    RequiresProcessing: this.RequiresProcessing(),
                    TemplateID: this.TemplateID(),
                    Template: this.Template(),
                    WorkflowID: this.WorkflowID(),
                    Workflow: this.Workflow(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return RequestTypeViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.RequestTypeViewModel = RequestTypeViewModel;
        var ResponseViewModel = (function (_super) {
            __extends(ResponseViewModel, _super);
            function ResponseViewModel(ResponseDTO) {
                var _this = _super.call(this) || this;
                if (ResponseDTO == null) {
                    _this.RequestDataMartID = ko.observable();
                    _this.ResponseGroupID = ko.observable();
                    _this.RespondedByID = ko.observable();
                    _this.ResponseTime = ko.observable();
                    _this.Count = ko.observable();
                    _this.SubmittedOn = ko.observable();
                    _this.SubmittedByID = ko.observable();
                    _this.SubmitMessage = ko.observable();
                    _this.ResponseMessage = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.RequestDataMartID = ko.observable(ResponseDTO.RequestDataMartID);
                    _this.ResponseGroupID = ko.observable(ResponseDTO.ResponseGroupID);
                    _this.RespondedByID = ko.observable(ResponseDTO.RespondedByID);
                    _this.ResponseTime = ko.observable(ResponseDTO.ResponseTime);
                    _this.Count = ko.observable(ResponseDTO.Count);
                    _this.SubmittedOn = ko.observable(ResponseDTO.SubmittedOn);
                    _this.SubmittedByID = ko.observable(ResponseDTO.SubmittedByID);
                    _this.SubmitMessage = ko.observable(ResponseDTO.SubmitMessage);
                    _this.ResponseMessage = ko.observable(ResponseDTO.ResponseMessage);
                    _this.ID = ko.observable(ResponseDTO.ID);
                    _this.Timestamp = ko.observable(ResponseDTO.Timestamp);
                }
                return _this;
            }
            ResponseViewModel.prototype.toData = function () {
                return {
                    RequestDataMartID: this.RequestDataMartID(),
                    ResponseGroupID: this.ResponseGroupID(),
                    RespondedByID: this.RespondedByID(),
                    ResponseTime: this.ResponseTime(),
                    Count: this.Count(),
                    SubmittedOn: this.SubmittedOn(),
                    SubmittedByID: this.SubmittedByID(),
                    SubmitMessage: this.SubmitMessage(),
                    ResponseMessage: this.ResponseMessage(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ResponseViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.ResponseViewModel = ResponseViewModel;
        var DataMartEventViewModel = (function (_super) {
            __extends(DataMartEventViewModel, _super);
            function DataMartEventViewModel(DataMartEventDTO) {
                var _this = _super.call(this) || this;
                if (DataMartEventDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.DataMartID = ko.observable(DataMartEventDTO.DataMartID);
                    _this.SecurityGroupID = ko.observable(DataMartEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(DataMartEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(DataMartEventDTO.Allowed);
                    _this.Overridden = ko.observable(DataMartEventDTO.Overridden);
                    _this.EventID = ko.observable(DataMartEventDTO.EventID);
                    _this.Event = ko.observable(DataMartEventDTO.Event);
                }
                return _this;
            }
            DataMartEventViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return DataMartEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.DataMartEventViewModel = DataMartEventViewModel;
        var AclViewModel = (function (_super) {
            __extends(AclViewModel, _super);
            function AclViewModel(AclDTO) {
                var _this = _super.call(this) || this;
                if (AclDTO == null) {
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.Allowed = ko.observable(AclDTO.Allowed);
                    _this.PermissionID = ko.observable(AclDTO.PermissionID);
                    _this.Permission = ko.observable(AclDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclDTO.Overridden);
                }
                return _this;
            }
            AclViewModel.prototype.toData = function () {
                return {
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclViewModel;
        }(BaseAclViewModel));
        ViewModels.AclViewModel = AclViewModel;
        var ProjectDataMartEventViewModel = (function (_super) {
            __extends(ProjectDataMartEventViewModel, _super);
            function ProjectDataMartEventViewModel(ProjectDataMartEventDTO) {
                var _this = _super.call(this) || this;
                if (ProjectDataMartEventDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectDataMartEventDTO.ProjectID);
                    _this.DataMartID = ko.observable(ProjectDataMartEventDTO.DataMartID);
                    _this.SecurityGroupID = ko.observable(ProjectDataMartEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(ProjectDataMartEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(ProjectDataMartEventDTO.Allowed);
                    _this.Overridden = ko.observable(ProjectDataMartEventDTO.Overridden);
                    _this.EventID = ko.observable(ProjectDataMartEventDTO.EventID);
                    _this.Event = ko.observable(ProjectDataMartEventDTO.Event);
                }
                return _this;
            }
            ProjectDataMartEventViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    DataMartID: this.DataMartID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return ProjectDataMartEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.ProjectDataMartEventViewModel = ProjectDataMartEventViewModel;
        var ProjectEventViewModel = (function (_super) {
            __extends(ProjectEventViewModel, _super);
            function ProjectEventViewModel(ProjectEventDTO) {
                var _this = _super.call(this) || this;
                if (ProjectEventDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectEventDTO.ProjectID);
                    _this.SecurityGroupID = ko.observable(ProjectEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(ProjectEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(ProjectEventDTO.Allowed);
                    _this.Overridden = ko.observable(ProjectEventDTO.Overridden);
                    _this.EventID = ko.observable(ProjectEventDTO.EventID);
                    _this.Event = ko.observable(ProjectEventDTO.Event);
                }
                return _this;
            }
            ProjectEventViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return ProjectEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.ProjectEventViewModel = ProjectEventViewModel;
        var ProjectOrganizationEventViewModel = (function (_super) {
            __extends(ProjectOrganizationEventViewModel, _super);
            function ProjectOrganizationEventViewModel(ProjectOrganizationEventDTO) {
                var _this = _super.call(this) || this;
                if (ProjectOrganizationEventDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.Overridden = ko.observable();
                    _this.EventID = ko.observable();
                    _this.Event = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(ProjectOrganizationEventDTO.ProjectID);
                    _this.OrganizationID = ko.observable(ProjectOrganizationEventDTO.OrganizationID);
                    _this.SecurityGroupID = ko.observable(ProjectOrganizationEventDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(ProjectOrganizationEventDTO.SecurityGroup);
                    _this.Allowed = ko.observable(ProjectOrganizationEventDTO.Allowed);
                    _this.Overridden = ko.observable(ProjectOrganizationEventDTO.Overridden);
                    _this.EventID = ko.observable(ProjectOrganizationEventDTO.EventID);
                    _this.Event = ko.observable(ProjectOrganizationEventDTO.Event);
                }
                return _this;
            }
            ProjectOrganizationEventViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    OrganizationID: this.OrganizationID(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Allowed: this.Allowed(),
                    Overridden: this.Overridden(),
                    EventID: this.EventID(),
                    Event: this.Event(),
                };
            };
            return ProjectOrganizationEventViewModel;
        }(BaseEventPermissionViewModel));
        ViewModels.ProjectOrganizationEventViewModel = ProjectOrganizationEventViewModel;
        var PermissionViewModel = (function (_super) {
            __extends(PermissionViewModel, _super);
            function PermissionViewModel(PermissionDTO) {
                var _this = _super.call(this) || this;
                if (PermissionDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Locations = ko.observableArray();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(PermissionDTO.Name);
                    _this.Description = ko.observable(PermissionDTO.Description);
                    _this.Locations = ko.observableArray(PermissionDTO.Locations == null ? null : PermissionDTO.Locations.map(function (item) { return item; }));
                    _this.ID = ko.observable(PermissionDTO.ID);
                    _this.Timestamp = ko.observable(PermissionDTO.Timestamp);
                }
                return _this;
            }
            PermissionViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Locations: this.Locations(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return PermissionViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.PermissionViewModel = PermissionViewModel;
        var SecurityGroupViewModel = (function (_super) {
            __extends(SecurityGroupViewModel, _super);
            function SecurityGroupViewModel(SecurityGroupDTO) {
                var _this = _super.call(this) || this;
                if (SecurityGroupDTO == null) {
                    _this.Name = ko.observable();
                    _this.Path = ko.observable();
                    _this.OwnerID = ko.observable();
                    _this.Owner = ko.observable();
                    _this.ParentSecurityGroupID = ko.observable();
                    _this.ParentSecurityGroup = ko.observable();
                    _this.Kind = ko.observable();
                    _this.Type = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(SecurityGroupDTO.Name);
                    _this.Path = ko.observable(SecurityGroupDTO.Path);
                    _this.OwnerID = ko.observable(SecurityGroupDTO.OwnerID);
                    _this.Owner = ko.observable(SecurityGroupDTO.Owner);
                    _this.ParentSecurityGroupID = ko.observable(SecurityGroupDTO.ParentSecurityGroupID);
                    _this.ParentSecurityGroup = ko.observable(SecurityGroupDTO.ParentSecurityGroup);
                    _this.Kind = ko.observable(SecurityGroupDTO.Kind);
                    _this.Type = ko.observable(SecurityGroupDTO.Type);
                    _this.ID = ko.observable(SecurityGroupDTO.ID);
                    _this.Timestamp = ko.observable(SecurityGroupDTO.Timestamp);
                }
                return _this;
            }
            SecurityGroupViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Path: this.Path(),
                    OwnerID: this.OwnerID(),
                    Owner: this.Owner(),
                    ParentSecurityGroupID: this.ParentSecurityGroupID(),
                    ParentSecurityGroup: this.ParentSecurityGroup(),
                    Kind: this.Kind(),
                    Type: this.Type(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return SecurityGroupViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.SecurityGroupViewModel = SecurityGroupViewModel;
        var SsoEndpointViewModel = (function (_super) {
            __extends(SsoEndpointViewModel, _super);
            function SsoEndpointViewModel(SsoEndpointDTO) {
                var _this = _super.call(this) || this;
                if (SsoEndpointDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.PostUrl = ko.observable();
                    _this.oAuthKey = ko.observable();
                    _this.oAuthHash = ko.observable();
                    _this.RequirePassword = ko.observable();
                    _this.Group = ko.observable();
                    _this.DisplayIndex = ko.observable();
                    _this.Enabled = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(SsoEndpointDTO.Name);
                    _this.Description = ko.observable(SsoEndpointDTO.Description);
                    _this.PostUrl = ko.observable(SsoEndpointDTO.PostUrl);
                    _this.oAuthKey = ko.observable(SsoEndpointDTO.oAuthKey);
                    _this.oAuthHash = ko.observable(SsoEndpointDTO.oAuthHash);
                    _this.RequirePassword = ko.observable(SsoEndpointDTO.RequirePassword);
                    _this.Group = ko.observable(SsoEndpointDTO.Group);
                    _this.DisplayIndex = ko.observable(SsoEndpointDTO.DisplayIndex);
                    _this.Enabled = ko.observable(SsoEndpointDTO.Enabled);
                    _this.ID = ko.observable(SsoEndpointDTO.ID);
                    _this.Timestamp = ko.observable(SsoEndpointDTO.Timestamp);
                }
                return _this;
            }
            SsoEndpointViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    PostUrl: this.PostUrl(),
                    oAuthKey: this.oAuthKey(),
                    oAuthHash: this.oAuthHash(),
                    RequirePassword: this.RequirePassword(),
                    Group: this.Group(),
                    DisplayIndex: this.DisplayIndex(),
                    Enabled: this.Enabled(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return SsoEndpointViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.SsoEndpointViewModel = SsoEndpointViewModel;
        var UserViewModel = (function (_super) {
            __extends(UserViewModel, _super);
            function UserViewModel(UserDTO) {
                var _this = _super.call(this) || this;
                if (UserDTO == null) {
                    _this.UserName = ko.observable();
                    _this.Title = ko.observable();
                    _this.FirstName = ko.observable();
                    _this.LastName = ko.observable();
                    _this.MiddleName = ko.observable();
                    _this.Phone = ko.observable();
                    _this.Fax = ko.observable();
                    _this.Email = ko.observable();
                    _this.Active = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.OrganizationRequested = ko.observable();
                    _this.RoleID = ko.observable();
                    _this.RoleRequested = ko.observable();
                    _this.SignedUpOn = ko.observable();
                    _this.ActivatedOn = ko.observable();
                    _this.DeactivatedOn = ko.observable();
                    _this.DeactivatedByID = ko.observable();
                    _this.DeactivatedBy = ko.observable();
                    _this.DeactivationReason = ko.observable();
                    _this.RejectReason = ko.observable();
                    _this.RejectedOn = ko.observable();
                    _this.RejectedByID = ko.observable();
                    _this.RejectedBy = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.UserName = ko.observable(UserDTO.UserName);
                    _this.Title = ko.observable(UserDTO.Title);
                    _this.FirstName = ko.observable(UserDTO.FirstName);
                    _this.LastName = ko.observable(UserDTO.LastName);
                    _this.MiddleName = ko.observable(UserDTO.MiddleName);
                    _this.Phone = ko.observable(UserDTO.Phone);
                    _this.Fax = ko.observable(UserDTO.Fax);
                    _this.Email = ko.observable(UserDTO.Email);
                    _this.Active = ko.observable(UserDTO.Active);
                    _this.Deleted = ko.observable(UserDTO.Deleted);
                    _this.OrganizationID = ko.observable(UserDTO.OrganizationID);
                    _this.Organization = ko.observable(UserDTO.Organization);
                    _this.OrganizationRequested = ko.observable(UserDTO.OrganizationRequested);
                    _this.RoleID = ko.observable(UserDTO.RoleID);
                    _this.RoleRequested = ko.observable(UserDTO.RoleRequested);
                    _this.SignedUpOn = ko.observable(UserDTO.SignedUpOn);
                    _this.ActivatedOn = ko.observable(UserDTO.ActivatedOn);
                    _this.DeactivatedOn = ko.observable(UserDTO.DeactivatedOn);
                    _this.DeactivatedByID = ko.observable(UserDTO.DeactivatedByID);
                    _this.DeactivatedBy = ko.observable(UserDTO.DeactivatedBy);
                    _this.DeactivationReason = ko.observable(UserDTO.DeactivationReason);
                    _this.RejectReason = ko.observable(UserDTO.RejectReason);
                    _this.RejectedOn = ko.observable(UserDTO.RejectedOn);
                    _this.RejectedByID = ko.observable(UserDTO.RejectedByID);
                    _this.RejectedBy = ko.observable(UserDTO.RejectedBy);
                    _this.ID = ko.observable(UserDTO.ID);
                    _this.Timestamp = ko.observable(UserDTO.Timestamp);
                }
                return _this;
            }
            UserViewModel.prototype.toData = function () {
                return {
                    UserName: this.UserName(),
                    Title: this.Title(),
                    FirstName: this.FirstName(),
                    LastName: this.LastName(),
                    MiddleName: this.MiddleName(),
                    Phone: this.Phone(),
                    Fax: this.Fax(),
                    Email: this.Email(),
                    Active: this.Active(),
                    Deleted: this.Deleted(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    OrganizationRequested: this.OrganizationRequested(),
                    RoleID: this.RoleID(),
                    RoleRequested: this.RoleRequested(),
                    SignedUpOn: this.SignedUpOn(),
                    ActivatedOn: this.ActivatedOn(),
                    DeactivatedOn: this.DeactivatedOn(),
                    DeactivatedByID: this.DeactivatedByID(),
                    DeactivatedBy: this.DeactivatedBy(),
                    DeactivationReason: this.DeactivationReason(),
                    RejectReason: this.RejectReason(),
                    RejectedOn: this.RejectedOn(),
                    RejectedByID: this.RejectedByID(),
                    RejectedBy: this.RejectedBy(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return UserViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.UserViewModel = UserViewModel;
        var WorkflowActivityViewModel = (function (_super) {
            __extends(WorkflowActivityViewModel, _super);
            function WorkflowActivityViewModel(WorkflowActivityDTO) {
                var _this = _super.call(this) || this;
                if (WorkflowActivityDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Start = ko.observable();
                    _this.End = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(WorkflowActivityDTO.Name);
                    _this.Description = ko.observable(WorkflowActivityDTO.Description);
                    _this.Start = ko.observable(WorkflowActivityDTO.Start);
                    _this.End = ko.observable(WorkflowActivityDTO.End);
                    _this.ID = ko.observable(WorkflowActivityDTO.ID);
                    _this.Timestamp = ko.observable(WorkflowActivityDTO.Timestamp);
                }
                return _this;
            }
            WorkflowActivityViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    Start: this.Start(),
                    End: this.End(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return WorkflowActivityViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.WorkflowActivityViewModel = WorkflowActivityViewModel;
        var WorkflowViewModel = (function (_super) {
            __extends(WorkflowViewModel, _super);
            function WorkflowViewModel(WorkflowDTO) {
                var _this = _super.call(this) || this;
                if (WorkflowDTO == null) {
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Name = ko.observable(WorkflowDTO.Name);
                    _this.Description = ko.observable(WorkflowDTO.Description);
                    _this.ID = ko.observable(WorkflowDTO.ID);
                    _this.Timestamp = ko.observable(WorkflowDTO.Timestamp);
                }
                return _this;
            }
            WorkflowViewModel.prototype.toData = function () {
                return {
                    Name: this.Name(),
                    Description: this.Description(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return WorkflowViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.WorkflowViewModel = WorkflowViewModel;
        var WorkflowRoleViewModel = (function (_super) {
            __extends(WorkflowRoleViewModel, _super);
            function WorkflowRoleViewModel(WorkflowRoleDTO) {
                var _this = _super.call(this) || this;
                if (WorkflowRoleDTO == null) {
                    _this.WorkflowID = ko.observable();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.IsRequestCreator = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.WorkflowID = ko.observable(WorkflowRoleDTO.WorkflowID);
                    _this.Name = ko.observable(WorkflowRoleDTO.Name);
                    _this.Description = ko.observable(WorkflowRoleDTO.Description);
                    _this.IsRequestCreator = ko.observable(WorkflowRoleDTO.IsRequestCreator);
                    _this.ID = ko.observable(WorkflowRoleDTO.ID);
                    _this.Timestamp = ko.observable(WorkflowRoleDTO.Timestamp);
                }
                return _this;
            }
            WorkflowRoleViewModel.prototype.toData = function () {
                return {
                    WorkflowID: this.WorkflowID(),
                    Name: this.Name(),
                    Description: this.Description(),
                    IsRequestCreator: this.IsRequestCreator(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return WorkflowRoleViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.WorkflowRoleViewModel = WorkflowRoleViewModel;
        var QueryComposerRequestViewModel = (function (_super) {
            __extends(QueryComposerRequestViewModel, _super);
            function QueryComposerRequestViewModel(QueryComposerRequestDTO) {
                var _this = _super.call(this) || this;
                if (QueryComposerRequestDTO == null) {
                    _this.Header = new QueryComposerHeaderViewModel();
                    _this.Where = new QueryComposerWhereViewModel();
                    _this.Select = new QueryComposerSelectViewModel();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Header = new QueryComposerHeaderViewModel(QueryComposerRequestDTO.Header);
                    _this.Where = new QueryComposerWhereViewModel(QueryComposerRequestDTO.Where);
                    _this.Select = new QueryComposerSelectViewModel(QueryComposerRequestDTO.Select);
                    _this.ID = ko.observable(QueryComposerRequestDTO.ID);
                    _this.Timestamp = ko.observable(QueryComposerRequestDTO.Timestamp);
                }
                return _this;
            }
            QueryComposerRequestViewModel.prototype.toData = function () {
                return {
                    Header: this.Header.toData(),
                    Where: this.Where.toData(),
                    Select: this.Select.toData(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return QueryComposerRequestViewModel;
        }(EntityDtoWithIDViewModel));
        ViewModels.QueryComposerRequestViewModel = QueryComposerRequestViewModel;
        var DataModelWithRequestTypesViewModel = (function (_super) {
            __extends(DataModelWithRequestTypesViewModel, _super);
            function DataModelWithRequestTypesViewModel(DataModelWithRequestTypesDTO) {
                var _this = _super.call(this) || this;
                if (DataModelWithRequestTypesDTO == null) {
                    _this.RequestTypes = ko.observableArray();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.RequiresConfiguration = ko.observable();
                    _this.QueryComposer = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.RequestTypes = ko.observableArray(DataModelWithRequestTypesDTO.RequestTypes == null ? null : DataModelWithRequestTypesDTO.RequestTypes.map(function (item) { return new RequestTypeViewModel(item); }));
                    _this.Name = ko.observable(DataModelWithRequestTypesDTO.Name);
                    _this.Description = ko.observable(DataModelWithRequestTypesDTO.Description);
                    _this.RequiresConfiguration = ko.observable(DataModelWithRequestTypesDTO.RequiresConfiguration);
                    _this.QueryComposer = ko.observable(DataModelWithRequestTypesDTO.QueryComposer);
                    _this.ID = ko.observable(DataModelWithRequestTypesDTO.ID);
                    _this.Timestamp = ko.observable(DataModelWithRequestTypesDTO.Timestamp);
                }
                return _this;
            }
            DataModelWithRequestTypesViewModel.prototype.toData = function () {
                return {
                    RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map(function (item) { return item.toData(); }),
                    Name: this.Name(),
                    Description: this.Description(),
                    RequiresConfiguration: this.RequiresConfiguration(),
                    QueryComposer: this.QueryComposer(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return DataModelWithRequestTypesViewModel;
        }(DataModelViewModel));
        ViewModels.DataModelWithRequestTypesViewModel = DataModelWithRequestTypesViewModel;
        var AclTemplateViewModel = (function (_super) {
            __extends(AclTemplateViewModel, _super);
            function AclTemplateViewModel(AclTemplateDTO) {
                var _this = _super.call(this) || this;
                if (AclTemplateDTO == null) {
                    _this.TemplateID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.TemplateID = ko.observable(AclTemplateDTO.TemplateID);
                    _this.Allowed = ko.observable(AclTemplateDTO.Allowed);
                    _this.PermissionID = ko.observable(AclTemplateDTO.PermissionID);
                    _this.Permission = ko.observable(AclTemplateDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclTemplateDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclTemplateDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclTemplateDTO.Overridden);
                }
                return _this;
            }
            AclTemplateViewModel.prototype.toData = function () {
                return {
                    TemplateID: this.TemplateID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclTemplateViewModel;
        }(AclViewModel));
        ViewModels.AclTemplateViewModel = AclTemplateViewModel;
        var DataMartViewModel = (function (_super) {
            __extends(DataMartViewModel, _super);
            function DataMartViewModel(DataMartDTO) {
                var _this = _super.call(this) || this;
                if (DataMartDTO == null) {
                    _this.RequiresApproval = ko.observable();
                    _this.DataMartTypeID = ko.observable();
                    _this.DataMartType = ko.observable();
                    _this.AvailablePeriod = ko.observable();
                    _this.ContactEmail = ko.observable();
                    _this.ContactFirstName = ko.observable();
                    _this.ContactLastName = ko.observable();
                    _this.ContactPhone = ko.observable();
                    _this.SpecialRequirements = ko.observable();
                    _this.UsageRestrictions = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.HealthPlanDescription = ko.observable();
                    _this.IsGroupDataMart = ko.observable();
                    _this.UnattendedMode = ko.observable();
                    _this.DataUpdateFrequency = ko.observable();
                    _this.InpatientEHRApplication = ko.observable();
                    _this.OutpatientEHRApplication = ko.observable();
                    _this.OtherClaims = ko.observable();
                    _this.OtherInpatientEHRApplication = ko.observable();
                    _this.OtherOutpatientEHRApplication = ko.observable();
                    _this.LaboratoryResultsAny = ko.observable();
                    _this.LaboratoryResultsClaims = ko.observable();
                    _this.LaboratoryResultsTestName = ko.observable();
                    _this.LaboratoryResultsDates = ko.observable();
                    _this.LaboratoryResultsTestLOINC = ko.observable();
                    _this.LaboratoryResultsTestSNOMED = ko.observable();
                    _this.LaboratoryResultsSpecimenSource = ko.observable();
                    _this.LaboratoryResultsTestDescriptions = ko.observable();
                    _this.LaboratoryResultsOrderDates = ko.observable();
                    _this.LaboratoryResultsTestResultsInterpretation = ko.observable();
                    _this.LaboratoryResultsTestOther = ko.observable();
                    _this.LaboratoryResultsTestOtherText = ko.observable();
                    _this.InpatientEncountersAny = ko.observable();
                    _this.InpatientEncountersEncounterID = ko.observable();
                    _this.InpatientEncountersProviderIdentifier = ko.observable();
                    _this.InpatientDatesOfService = ko.observable();
                    _this.InpatientICD9Procedures = ko.observable();
                    _this.InpatientICD10Procedures = ko.observable();
                    _this.InpatientICD9Diagnosis = ko.observable();
                    _this.InpatientICD10Diagnosis = ko.observable();
                    _this.InpatientSNOMED = ko.observable();
                    _this.InpatientHPHCS = ko.observable();
                    _this.InpatientDisposition = ko.observable();
                    _this.InpatientDischargeStatus = ko.observable();
                    _this.InpatientOther = ko.observable();
                    _this.InpatientOtherText = ko.observable();
                    _this.OutpatientEncountersAny = ko.observable();
                    _this.OutpatientEncountersEncounterID = ko.observable();
                    _this.OutpatientEncountersProviderIdentifier = ko.observable();
                    _this.OutpatientClinicalSetting = ko.observable();
                    _this.OutpatientDatesOfService = ko.observable();
                    _this.OutpatientICD9Procedures = ko.observable();
                    _this.OutpatientICD10Procedures = ko.observable();
                    _this.OutpatientICD9Diagnosis = ko.observable();
                    _this.OutpatientICD10Diagnosis = ko.observable();
                    _this.OutpatientSNOMED = ko.observable();
                    _this.OutpatientHPHCS = ko.observable();
                    _this.OutpatientOther = ko.observable();
                    _this.OutpatientOtherText = ko.observable();
                    _this.ERPatientID = ko.observable();
                    _this.EREncounterID = ko.observable();
                    _this.EREnrollmentDates = ko.observable();
                    _this.EREncounterDates = ko.observable();
                    _this.ERClinicalSetting = ko.observable();
                    _this.ERICD9Diagnosis = ko.observable();
                    _this.ERICD10Diagnosis = ko.observable();
                    _this.ERHPHCS = ko.observable();
                    _this.ERNDC = ko.observable();
                    _this.ERSNOMED = ko.observable();
                    _this.ERProviderIdentifier = ko.observable();
                    _this.ERProviderFacility = ko.observable();
                    _this.EREncounterType = ko.observable();
                    _this.ERDRG = ko.observable();
                    _this.ERDRGType = ko.observable();
                    _this.EROther = ko.observable();
                    _this.EROtherText = ko.observable();
                    _this.DemographicsAny = ko.observable();
                    _this.DemographicsPatientID = ko.observable();
                    _this.DemographicsSex = ko.observable();
                    _this.DemographicsDateOfBirth = ko.observable();
                    _this.DemographicsDateOfDeath = ko.observable();
                    _this.DemographicsAddressInfo = ko.observable();
                    _this.DemographicsRace = ko.observable();
                    _this.DemographicsEthnicity = ko.observable();
                    _this.DemographicsOther = ko.observable();
                    _this.DemographicsOtherText = ko.observable();
                    _this.PatientOutcomesAny = ko.observable();
                    _this.PatientOutcomesInstruments = ko.observable();
                    _this.PatientOutcomesInstrumentText = ko.observable();
                    _this.PatientOutcomesHealthBehavior = ko.observable();
                    _this.PatientOutcomesHRQoL = ko.observable();
                    _this.PatientOutcomesReportedOutcome = ko.observable();
                    _this.PatientOutcomesOther = ko.observable();
                    _this.PatientOutcomesOtherText = ko.observable();
                    _this.PatientBehaviorHealthBehavior = ko.observable();
                    _this.PatientBehaviorInstruments = ko.observable();
                    _this.PatientBehaviorInstrumentText = ko.observable();
                    _this.PatientBehaviorOther = ko.observable();
                    _this.PatientBehaviorOtherText = ko.observable();
                    _this.VitalSignsAny = ko.observable();
                    _this.VitalSignsTemperature = ko.observable();
                    _this.VitalSignsHeight = ko.observable();
                    _this.VitalSignsWeight = ko.observable();
                    _this.VitalSignsBMI = ko.observable();
                    _this.VitalSignsBloodPressure = ko.observable();
                    _this.VitalSignsOther = ko.observable();
                    _this.VitalSignsOtherText = ko.observable();
                    _this.VitalSignsLength = ko.observable();
                    _this.PrescriptionOrdersAny = ko.observable();
                    _this.PrescriptionOrderDates = ko.observable();
                    _this.PrescriptionOrderRxNorm = ko.observable();
                    _this.PrescriptionOrderNDC = ko.observable();
                    _this.PrescriptionOrderOther = ko.observable();
                    _this.PrescriptionOrderOtherText = ko.observable();
                    _this.PharmacyDispensingAny = ko.observable();
                    _this.PharmacyDispensingDates = ko.observable();
                    _this.PharmacyDispensingRxNorm = ko.observable();
                    _this.PharmacyDispensingDaysSupply = ko.observable();
                    _this.PharmacyDispensingAmountDispensed = ko.observable();
                    _this.PharmacyDispensingNDC = ko.observable();
                    _this.PharmacyDispensingOther = ko.observable();
                    _this.PharmacyDispensingOtherText = ko.observable();
                    _this.BiorepositoriesAny = ko.observable();
                    _this.BiorepositoriesName = ko.observable();
                    _this.BiorepositoriesDescription = ko.observable();
                    _this.BiorepositoriesDiseaseName = ko.observable();
                    _this.BiorepositoriesSpecimenSource = ko.observable();
                    _this.BiorepositoriesSpecimenType = ko.observable();
                    _this.BiorepositoriesProcessingMethod = ko.observable();
                    _this.BiorepositoriesSNOMED = ko.observable();
                    _this.BiorepositoriesStorageMethod = ko.observable();
                    _this.BiorepositoriesOther = ko.observable();
                    _this.BiorepositoriesOtherText = ko.observable();
                    _this.LongitudinalCaptureAny = ko.observable();
                    _this.LongitudinalCapturePatientID = ko.observable();
                    _this.LongitudinalCaptureStart = ko.observable();
                    _this.LongitudinalCaptureStop = ko.observable();
                    _this.LongitudinalCaptureOther = ko.observable();
                    _this.LongitudinalCaptureOtherValue = ko.observable();
                    _this.DataModel = ko.observable();
                    _this.OtherDataModel = ko.observable();
                    _this.IsLocal = ko.observable();
                    _this.Url = ko.observable();
                    _this.AdapterID = ko.observable();
                    _this.Adapter = ko.observable();
                    _this.ProcessorID = ko.observable();
                    _this.DataPartnerIdentifier = ko.observable();
                    _this.DataPartnerCode = ko.observable();
                    _this.Name = ko.observable();
                    _this.Description = ko.observable();
                    _this.Acronym = ko.observable();
                    _this.StartDate = ko.observable();
                    _this.EndDate = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.ParentOrganziationID = ko.observable();
                    _this.ParentOrganization = ko.observable();
                    _this.Priority = ko.observable();
                    _this.DueDate = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.RequiresApproval = ko.observable(DataMartDTO.RequiresApproval);
                    _this.DataMartTypeID = ko.observable(DataMartDTO.DataMartTypeID);
                    _this.DataMartType = ko.observable(DataMartDTO.DataMartType);
                    _this.AvailablePeriod = ko.observable(DataMartDTO.AvailablePeriod);
                    _this.ContactEmail = ko.observable(DataMartDTO.ContactEmail);
                    _this.ContactFirstName = ko.observable(DataMartDTO.ContactFirstName);
                    _this.ContactLastName = ko.observable(DataMartDTO.ContactLastName);
                    _this.ContactPhone = ko.observable(DataMartDTO.ContactPhone);
                    _this.SpecialRequirements = ko.observable(DataMartDTO.SpecialRequirements);
                    _this.UsageRestrictions = ko.observable(DataMartDTO.UsageRestrictions);
                    _this.Deleted = ko.observable(DataMartDTO.Deleted);
                    _this.HealthPlanDescription = ko.observable(DataMartDTO.HealthPlanDescription);
                    _this.IsGroupDataMart = ko.observable(DataMartDTO.IsGroupDataMart);
                    _this.UnattendedMode = ko.observable(DataMartDTO.UnattendedMode);
                    _this.DataUpdateFrequency = ko.observable(DataMartDTO.DataUpdateFrequency);
                    _this.InpatientEHRApplication = ko.observable(DataMartDTO.InpatientEHRApplication);
                    _this.OutpatientEHRApplication = ko.observable(DataMartDTO.OutpatientEHRApplication);
                    _this.OtherClaims = ko.observable(DataMartDTO.OtherClaims);
                    _this.OtherInpatientEHRApplication = ko.observable(DataMartDTO.OtherInpatientEHRApplication);
                    _this.OtherOutpatientEHRApplication = ko.observable(DataMartDTO.OtherOutpatientEHRApplication);
                    _this.LaboratoryResultsAny = ko.observable(DataMartDTO.LaboratoryResultsAny);
                    _this.LaboratoryResultsClaims = ko.observable(DataMartDTO.LaboratoryResultsClaims);
                    _this.LaboratoryResultsTestName = ko.observable(DataMartDTO.LaboratoryResultsTestName);
                    _this.LaboratoryResultsDates = ko.observable(DataMartDTO.LaboratoryResultsDates);
                    _this.LaboratoryResultsTestLOINC = ko.observable(DataMartDTO.LaboratoryResultsTestLOINC);
                    _this.LaboratoryResultsTestSNOMED = ko.observable(DataMartDTO.LaboratoryResultsTestSNOMED);
                    _this.LaboratoryResultsSpecimenSource = ko.observable(DataMartDTO.LaboratoryResultsSpecimenSource);
                    _this.LaboratoryResultsTestDescriptions = ko.observable(DataMartDTO.LaboratoryResultsTestDescriptions);
                    _this.LaboratoryResultsOrderDates = ko.observable(DataMartDTO.LaboratoryResultsOrderDates);
                    _this.LaboratoryResultsTestResultsInterpretation = ko.observable(DataMartDTO.LaboratoryResultsTestResultsInterpretation);
                    _this.LaboratoryResultsTestOther = ko.observable(DataMartDTO.LaboratoryResultsTestOther);
                    _this.LaboratoryResultsTestOtherText = ko.observable(DataMartDTO.LaboratoryResultsTestOtherText);
                    _this.InpatientEncountersAny = ko.observable(DataMartDTO.InpatientEncountersAny);
                    _this.InpatientEncountersEncounterID = ko.observable(DataMartDTO.InpatientEncountersEncounterID);
                    _this.InpatientEncountersProviderIdentifier = ko.observable(DataMartDTO.InpatientEncountersProviderIdentifier);
                    _this.InpatientDatesOfService = ko.observable(DataMartDTO.InpatientDatesOfService);
                    _this.InpatientICD9Procedures = ko.observable(DataMartDTO.InpatientICD9Procedures);
                    _this.InpatientICD10Procedures = ko.observable(DataMartDTO.InpatientICD10Procedures);
                    _this.InpatientICD9Diagnosis = ko.observable(DataMartDTO.InpatientICD9Diagnosis);
                    _this.InpatientICD10Diagnosis = ko.observable(DataMartDTO.InpatientICD10Diagnosis);
                    _this.InpatientSNOMED = ko.observable(DataMartDTO.InpatientSNOMED);
                    _this.InpatientHPHCS = ko.observable(DataMartDTO.InpatientHPHCS);
                    _this.InpatientDisposition = ko.observable(DataMartDTO.InpatientDisposition);
                    _this.InpatientDischargeStatus = ko.observable(DataMartDTO.InpatientDischargeStatus);
                    _this.InpatientOther = ko.observable(DataMartDTO.InpatientOther);
                    _this.InpatientOtherText = ko.observable(DataMartDTO.InpatientOtherText);
                    _this.OutpatientEncountersAny = ko.observable(DataMartDTO.OutpatientEncountersAny);
                    _this.OutpatientEncountersEncounterID = ko.observable(DataMartDTO.OutpatientEncountersEncounterID);
                    _this.OutpatientEncountersProviderIdentifier = ko.observable(DataMartDTO.OutpatientEncountersProviderIdentifier);
                    _this.OutpatientClinicalSetting = ko.observable(DataMartDTO.OutpatientClinicalSetting);
                    _this.OutpatientDatesOfService = ko.observable(DataMartDTO.OutpatientDatesOfService);
                    _this.OutpatientICD9Procedures = ko.observable(DataMartDTO.OutpatientICD9Procedures);
                    _this.OutpatientICD10Procedures = ko.observable(DataMartDTO.OutpatientICD10Procedures);
                    _this.OutpatientICD9Diagnosis = ko.observable(DataMartDTO.OutpatientICD9Diagnosis);
                    _this.OutpatientICD10Diagnosis = ko.observable(DataMartDTO.OutpatientICD10Diagnosis);
                    _this.OutpatientSNOMED = ko.observable(DataMartDTO.OutpatientSNOMED);
                    _this.OutpatientHPHCS = ko.observable(DataMartDTO.OutpatientHPHCS);
                    _this.OutpatientOther = ko.observable(DataMartDTO.OutpatientOther);
                    _this.OutpatientOtherText = ko.observable(DataMartDTO.OutpatientOtherText);
                    _this.ERPatientID = ko.observable(DataMartDTO.ERPatientID);
                    _this.EREncounterID = ko.observable(DataMartDTO.EREncounterID);
                    _this.EREnrollmentDates = ko.observable(DataMartDTO.EREnrollmentDates);
                    _this.EREncounterDates = ko.observable(DataMartDTO.EREncounterDates);
                    _this.ERClinicalSetting = ko.observable(DataMartDTO.ERClinicalSetting);
                    _this.ERICD9Diagnosis = ko.observable(DataMartDTO.ERICD9Diagnosis);
                    _this.ERICD10Diagnosis = ko.observable(DataMartDTO.ERICD10Diagnosis);
                    _this.ERHPHCS = ko.observable(DataMartDTO.ERHPHCS);
                    _this.ERNDC = ko.observable(DataMartDTO.ERNDC);
                    _this.ERSNOMED = ko.observable(DataMartDTO.ERSNOMED);
                    _this.ERProviderIdentifier = ko.observable(DataMartDTO.ERProviderIdentifier);
                    _this.ERProviderFacility = ko.observable(DataMartDTO.ERProviderFacility);
                    _this.EREncounterType = ko.observable(DataMartDTO.EREncounterType);
                    _this.ERDRG = ko.observable(DataMartDTO.ERDRG);
                    _this.ERDRGType = ko.observable(DataMartDTO.ERDRGType);
                    _this.EROther = ko.observable(DataMartDTO.EROther);
                    _this.EROtherText = ko.observable(DataMartDTO.EROtherText);
                    _this.DemographicsAny = ko.observable(DataMartDTO.DemographicsAny);
                    _this.DemographicsPatientID = ko.observable(DataMartDTO.DemographicsPatientID);
                    _this.DemographicsSex = ko.observable(DataMartDTO.DemographicsSex);
                    _this.DemographicsDateOfBirth = ko.observable(DataMartDTO.DemographicsDateOfBirth);
                    _this.DemographicsDateOfDeath = ko.observable(DataMartDTO.DemographicsDateOfDeath);
                    _this.DemographicsAddressInfo = ko.observable(DataMartDTO.DemographicsAddressInfo);
                    _this.DemographicsRace = ko.observable(DataMartDTO.DemographicsRace);
                    _this.DemographicsEthnicity = ko.observable(DataMartDTO.DemographicsEthnicity);
                    _this.DemographicsOther = ko.observable(DataMartDTO.DemographicsOther);
                    _this.DemographicsOtherText = ko.observable(DataMartDTO.DemographicsOtherText);
                    _this.PatientOutcomesAny = ko.observable(DataMartDTO.PatientOutcomesAny);
                    _this.PatientOutcomesInstruments = ko.observable(DataMartDTO.PatientOutcomesInstruments);
                    _this.PatientOutcomesInstrumentText = ko.observable(DataMartDTO.PatientOutcomesInstrumentText);
                    _this.PatientOutcomesHealthBehavior = ko.observable(DataMartDTO.PatientOutcomesHealthBehavior);
                    _this.PatientOutcomesHRQoL = ko.observable(DataMartDTO.PatientOutcomesHRQoL);
                    _this.PatientOutcomesReportedOutcome = ko.observable(DataMartDTO.PatientOutcomesReportedOutcome);
                    _this.PatientOutcomesOther = ko.observable(DataMartDTO.PatientOutcomesOther);
                    _this.PatientOutcomesOtherText = ko.observable(DataMartDTO.PatientOutcomesOtherText);
                    _this.PatientBehaviorHealthBehavior = ko.observable(DataMartDTO.PatientBehaviorHealthBehavior);
                    _this.PatientBehaviorInstruments = ko.observable(DataMartDTO.PatientBehaviorInstruments);
                    _this.PatientBehaviorInstrumentText = ko.observable(DataMartDTO.PatientBehaviorInstrumentText);
                    _this.PatientBehaviorOther = ko.observable(DataMartDTO.PatientBehaviorOther);
                    _this.PatientBehaviorOtherText = ko.observable(DataMartDTO.PatientBehaviorOtherText);
                    _this.VitalSignsAny = ko.observable(DataMartDTO.VitalSignsAny);
                    _this.VitalSignsTemperature = ko.observable(DataMartDTO.VitalSignsTemperature);
                    _this.VitalSignsHeight = ko.observable(DataMartDTO.VitalSignsHeight);
                    _this.VitalSignsWeight = ko.observable(DataMartDTO.VitalSignsWeight);
                    _this.VitalSignsBMI = ko.observable(DataMartDTO.VitalSignsBMI);
                    _this.VitalSignsBloodPressure = ko.observable(DataMartDTO.VitalSignsBloodPressure);
                    _this.VitalSignsOther = ko.observable(DataMartDTO.VitalSignsOther);
                    _this.VitalSignsOtherText = ko.observable(DataMartDTO.VitalSignsOtherText);
                    _this.VitalSignsLength = ko.observable(DataMartDTO.VitalSignsLength);
                    _this.PrescriptionOrdersAny = ko.observable(DataMartDTO.PrescriptionOrdersAny);
                    _this.PrescriptionOrderDates = ko.observable(DataMartDTO.PrescriptionOrderDates);
                    _this.PrescriptionOrderRxNorm = ko.observable(DataMartDTO.PrescriptionOrderRxNorm);
                    _this.PrescriptionOrderNDC = ko.observable(DataMartDTO.PrescriptionOrderNDC);
                    _this.PrescriptionOrderOther = ko.observable(DataMartDTO.PrescriptionOrderOther);
                    _this.PrescriptionOrderOtherText = ko.observable(DataMartDTO.PrescriptionOrderOtherText);
                    _this.PharmacyDispensingAny = ko.observable(DataMartDTO.PharmacyDispensingAny);
                    _this.PharmacyDispensingDates = ko.observable(DataMartDTO.PharmacyDispensingDates);
                    _this.PharmacyDispensingRxNorm = ko.observable(DataMartDTO.PharmacyDispensingRxNorm);
                    _this.PharmacyDispensingDaysSupply = ko.observable(DataMartDTO.PharmacyDispensingDaysSupply);
                    _this.PharmacyDispensingAmountDispensed = ko.observable(DataMartDTO.PharmacyDispensingAmountDispensed);
                    _this.PharmacyDispensingNDC = ko.observable(DataMartDTO.PharmacyDispensingNDC);
                    _this.PharmacyDispensingOther = ko.observable(DataMartDTO.PharmacyDispensingOther);
                    _this.PharmacyDispensingOtherText = ko.observable(DataMartDTO.PharmacyDispensingOtherText);
                    _this.BiorepositoriesAny = ko.observable(DataMartDTO.BiorepositoriesAny);
                    _this.BiorepositoriesName = ko.observable(DataMartDTO.BiorepositoriesName);
                    _this.BiorepositoriesDescription = ko.observable(DataMartDTO.BiorepositoriesDescription);
                    _this.BiorepositoriesDiseaseName = ko.observable(DataMartDTO.BiorepositoriesDiseaseName);
                    _this.BiorepositoriesSpecimenSource = ko.observable(DataMartDTO.BiorepositoriesSpecimenSource);
                    _this.BiorepositoriesSpecimenType = ko.observable(DataMartDTO.BiorepositoriesSpecimenType);
                    _this.BiorepositoriesProcessingMethod = ko.observable(DataMartDTO.BiorepositoriesProcessingMethod);
                    _this.BiorepositoriesSNOMED = ko.observable(DataMartDTO.BiorepositoriesSNOMED);
                    _this.BiorepositoriesStorageMethod = ko.observable(DataMartDTO.BiorepositoriesStorageMethod);
                    _this.BiorepositoriesOther = ko.observable(DataMartDTO.BiorepositoriesOther);
                    _this.BiorepositoriesOtherText = ko.observable(DataMartDTO.BiorepositoriesOtherText);
                    _this.LongitudinalCaptureAny = ko.observable(DataMartDTO.LongitudinalCaptureAny);
                    _this.LongitudinalCapturePatientID = ko.observable(DataMartDTO.LongitudinalCapturePatientID);
                    _this.LongitudinalCaptureStart = ko.observable(DataMartDTO.LongitudinalCaptureStart);
                    _this.LongitudinalCaptureStop = ko.observable(DataMartDTO.LongitudinalCaptureStop);
                    _this.LongitudinalCaptureOther = ko.observable(DataMartDTO.LongitudinalCaptureOther);
                    _this.LongitudinalCaptureOtherValue = ko.observable(DataMartDTO.LongitudinalCaptureOtherValue);
                    _this.DataModel = ko.observable(DataMartDTO.DataModel);
                    _this.OtherDataModel = ko.observable(DataMartDTO.OtherDataModel);
                    _this.IsLocal = ko.observable(DataMartDTO.IsLocal);
                    _this.Url = ko.observable(DataMartDTO.Url);
                    _this.AdapterID = ko.observable(DataMartDTO.AdapterID);
                    _this.Adapter = ko.observable(DataMartDTO.Adapter);
                    _this.ProcessorID = ko.observable(DataMartDTO.ProcessorID);
                    _this.DataPartnerIdentifier = ko.observable(DataMartDTO.DataPartnerIdentifier);
                    _this.DataPartnerCode = ko.observable(DataMartDTO.DataPartnerCode);
                    _this.Name = ko.observable(DataMartDTO.Name);
                    _this.Description = ko.observable(DataMartDTO.Description);
                    _this.Acronym = ko.observable(DataMartDTO.Acronym);
                    _this.StartDate = ko.observable(DataMartDTO.StartDate);
                    _this.EndDate = ko.observable(DataMartDTO.EndDate);
                    _this.OrganizationID = ko.observable(DataMartDTO.OrganizationID);
                    _this.Organization = ko.observable(DataMartDTO.Organization);
                    _this.ParentOrganziationID = ko.observable(DataMartDTO.ParentOrganziationID);
                    _this.ParentOrganization = ko.observable(DataMartDTO.ParentOrganization);
                    _this.Priority = ko.observable(DataMartDTO.Priority);
                    _this.DueDate = ko.observable(DataMartDTO.DueDate);
                    _this.ID = ko.observable(DataMartDTO.ID);
                    _this.Timestamp = ko.observable(DataMartDTO.Timestamp);
                }
                return _this;
            }
            DataMartViewModel.prototype.toData = function () {
                return {
                    RequiresApproval: this.RequiresApproval(),
                    DataMartTypeID: this.DataMartTypeID(),
                    DataMartType: this.DataMartType(),
                    AvailablePeriod: this.AvailablePeriod(),
                    ContactEmail: this.ContactEmail(),
                    ContactFirstName: this.ContactFirstName(),
                    ContactLastName: this.ContactLastName(),
                    ContactPhone: this.ContactPhone(),
                    SpecialRequirements: this.SpecialRequirements(),
                    UsageRestrictions: this.UsageRestrictions(),
                    Deleted: this.Deleted(),
                    HealthPlanDescription: this.HealthPlanDescription(),
                    IsGroupDataMart: this.IsGroupDataMart(),
                    UnattendedMode: this.UnattendedMode(),
                    DataUpdateFrequency: this.DataUpdateFrequency(),
                    InpatientEHRApplication: this.InpatientEHRApplication(),
                    OutpatientEHRApplication: this.OutpatientEHRApplication(),
                    OtherClaims: this.OtherClaims(),
                    OtherInpatientEHRApplication: this.OtherInpatientEHRApplication(),
                    OtherOutpatientEHRApplication: this.OtherOutpatientEHRApplication(),
                    LaboratoryResultsAny: this.LaboratoryResultsAny(),
                    LaboratoryResultsClaims: this.LaboratoryResultsClaims(),
                    LaboratoryResultsTestName: this.LaboratoryResultsTestName(),
                    LaboratoryResultsDates: this.LaboratoryResultsDates(),
                    LaboratoryResultsTestLOINC: this.LaboratoryResultsTestLOINC(),
                    LaboratoryResultsTestSNOMED: this.LaboratoryResultsTestSNOMED(),
                    LaboratoryResultsSpecimenSource: this.LaboratoryResultsSpecimenSource(),
                    LaboratoryResultsTestDescriptions: this.LaboratoryResultsTestDescriptions(),
                    LaboratoryResultsOrderDates: this.LaboratoryResultsOrderDates(),
                    LaboratoryResultsTestResultsInterpretation: this.LaboratoryResultsTestResultsInterpretation(),
                    LaboratoryResultsTestOther: this.LaboratoryResultsTestOther(),
                    LaboratoryResultsTestOtherText: this.LaboratoryResultsTestOtherText(),
                    InpatientEncountersAny: this.InpatientEncountersAny(),
                    InpatientEncountersEncounterID: this.InpatientEncountersEncounterID(),
                    InpatientEncountersProviderIdentifier: this.InpatientEncountersProviderIdentifier(),
                    InpatientDatesOfService: this.InpatientDatesOfService(),
                    InpatientICD9Procedures: this.InpatientICD9Procedures(),
                    InpatientICD10Procedures: this.InpatientICD10Procedures(),
                    InpatientICD9Diagnosis: this.InpatientICD9Diagnosis(),
                    InpatientICD10Diagnosis: this.InpatientICD10Diagnosis(),
                    InpatientSNOMED: this.InpatientSNOMED(),
                    InpatientHPHCS: this.InpatientHPHCS(),
                    InpatientDisposition: this.InpatientDisposition(),
                    InpatientDischargeStatus: this.InpatientDischargeStatus(),
                    InpatientOther: this.InpatientOther(),
                    InpatientOtherText: this.InpatientOtherText(),
                    OutpatientEncountersAny: this.OutpatientEncountersAny(),
                    OutpatientEncountersEncounterID: this.OutpatientEncountersEncounterID(),
                    OutpatientEncountersProviderIdentifier: this.OutpatientEncountersProviderIdentifier(),
                    OutpatientClinicalSetting: this.OutpatientClinicalSetting(),
                    OutpatientDatesOfService: this.OutpatientDatesOfService(),
                    OutpatientICD9Procedures: this.OutpatientICD9Procedures(),
                    OutpatientICD10Procedures: this.OutpatientICD10Procedures(),
                    OutpatientICD9Diagnosis: this.OutpatientICD9Diagnosis(),
                    OutpatientICD10Diagnosis: this.OutpatientICD10Diagnosis(),
                    OutpatientSNOMED: this.OutpatientSNOMED(),
                    OutpatientHPHCS: this.OutpatientHPHCS(),
                    OutpatientOther: this.OutpatientOther(),
                    OutpatientOtherText: this.OutpatientOtherText(),
                    ERPatientID: this.ERPatientID(),
                    EREncounterID: this.EREncounterID(),
                    EREnrollmentDates: this.EREnrollmentDates(),
                    EREncounterDates: this.EREncounterDates(),
                    ERClinicalSetting: this.ERClinicalSetting(),
                    ERICD9Diagnosis: this.ERICD9Diagnosis(),
                    ERICD10Diagnosis: this.ERICD10Diagnosis(),
                    ERHPHCS: this.ERHPHCS(),
                    ERNDC: this.ERNDC(),
                    ERSNOMED: this.ERSNOMED(),
                    ERProviderIdentifier: this.ERProviderIdentifier(),
                    ERProviderFacility: this.ERProviderFacility(),
                    EREncounterType: this.EREncounterType(),
                    ERDRG: this.ERDRG(),
                    ERDRGType: this.ERDRGType(),
                    EROther: this.EROther(),
                    EROtherText: this.EROtherText(),
                    DemographicsAny: this.DemographicsAny(),
                    DemographicsPatientID: this.DemographicsPatientID(),
                    DemographicsSex: this.DemographicsSex(),
                    DemographicsDateOfBirth: this.DemographicsDateOfBirth(),
                    DemographicsDateOfDeath: this.DemographicsDateOfDeath(),
                    DemographicsAddressInfo: this.DemographicsAddressInfo(),
                    DemographicsRace: this.DemographicsRace(),
                    DemographicsEthnicity: this.DemographicsEthnicity(),
                    DemographicsOther: this.DemographicsOther(),
                    DemographicsOtherText: this.DemographicsOtherText(),
                    PatientOutcomesAny: this.PatientOutcomesAny(),
                    PatientOutcomesInstruments: this.PatientOutcomesInstruments(),
                    PatientOutcomesInstrumentText: this.PatientOutcomesInstrumentText(),
                    PatientOutcomesHealthBehavior: this.PatientOutcomesHealthBehavior(),
                    PatientOutcomesHRQoL: this.PatientOutcomesHRQoL(),
                    PatientOutcomesReportedOutcome: this.PatientOutcomesReportedOutcome(),
                    PatientOutcomesOther: this.PatientOutcomesOther(),
                    PatientOutcomesOtherText: this.PatientOutcomesOtherText(),
                    PatientBehaviorHealthBehavior: this.PatientBehaviorHealthBehavior(),
                    PatientBehaviorInstruments: this.PatientBehaviorInstruments(),
                    PatientBehaviorInstrumentText: this.PatientBehaviorInstrumentText(),
                    PatientBehaviorOther: this.PatientBehaviorOther(),
                    PatientBehaviorOtherText: this.PatientBehaviorOtherText(),
                    VitalSignsAny: this.VitalSignsAny(),
                    VitalSignsTemperature: this.VitalSignsTemperature(),
                    VitalSignsHeight: this.VitalSignsHeight(),
                    VitalSignsWeight: this.VitalSignsWeight(),
                    VitalSignsBMI: this.VitalSignsBMI(),
                    VitalSignsBloodPressure: this.VitalSignsBloodPressure(),
                    VitalSignsOther: this.VitalSignsOther(),
                    VitalSignsOtherText: this.VitalSignsOtherText(),
                    VitalSignsLength: this.VitalSignsLength(),
                    PrescriptionOrdersAny: this.PrescriptionOrdersAny(),
                    PrescriptionOrderDates: this.PrescriptionOrderDates(),
                    PrescriptionOrderRxNorm: this.PrescriptionOrderRxNorm(),
                    PrescriptionOrderNDC: this.PrescriptionOrderNDC(),
                    PrescriptionOrderOther: this.PrescriptionOrderOther(),
                    PrescriptionOrderOtherText: this.PrescriptionOrderOtherText(),
                    PharmacyDispensingAny: this.PharmacyDispensingAny(),
                    PharmacyDispensingDates: this.PharmacyDispensingDates(),
                    PharmacyDispensingRxNorm: this.PharmacyDispensingRxNorm(),
                    PharmacyDispensingDaysSupply: this.PharmacyDispensingDaysSupply(),
                    PharmacyDispensingAmountDispensed: this.PharmacyDispensingAmountDispensed(),
                    PharmacyDispensingNDC: this.PharmacyDispensingNDC(),
                    PharmacyDispensingOther: this.PharmacyDispensingOther(),
                    PharmacyDispensingOtherText: this.PharmacyDispensingOtherText(),
                    BiorepositoriesAny: this.BiorepositoriesAny(),
                    BiorepositoriesName: this.BiorepositoriesName(),
                    BiorepositoriesDescription: this.BiorepositoriesDescription(),
                    BiorepositoriesDiseaseName: this.BiorepositoriesDiseaseName(),
                    BiorepositoriesSpecimenSource: this.BiorepositoriesSpecimenSource(),
                    BiorepositoriesSpecimenType: this.BiorepositoriesSpecimenType(),
                    BiorepositoriesProcessingMethod: this.BiorepositoriesProcessingMethod(),
                    BiorepositoriesSNOMED: this.BiorepositoriesSNOMED(),
                    BiorepositoriesStorageMethod: this.BiorepositoriesStorageMethod(),
                    BiorepositoriesOther: this.BiorepositoriesOther(),
                    BiorepositoriesOtherText: this.BiorepositoriesOtherText(),
                    LongitudinalCaptureAny: this.LongitudinalCaptureAny(),
                    LongitudinalCapturePatientID: this.LongitudinalCapturePatientID(),
                    LongitudinalCaptureStart: this.LongitudinalCaptureStart(),
                    LongitudinalCaptureStop: this.LongitudinalCaptureStop(),
                    LongitudinalCaptureOther: this.LongitudinalCaptureOther(),
                    LongitudinalCaptureOtherValue: this.LongitudinalCaptureOtherValue(),
                    DataModel: this.DataModel(),
                    OtherDataModel: this.OtherDataModel(),
                    IsLocal: this.IsLocal(),
                    Url: this.Url(),
                    AdapterID: this.AdapterID(),
                    Adapter: this.Adapter(),
                    ProcessorID: this.ProcessorID(),
                    DataPartnerIdentifier: this.DataPartnerIdentifier(),
                    DataPartnerCode: this.DataPartnerCode(),
                    Name: this.Name(),
                    Description: this.Description(),
                    Acronym: this.Acronym(),
                    StartDate: this.StartDate(),
                    EndDate: this.EndDate(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    ParentOrganziationID: this.ParentOrganziationID(),
                    ParentOrganization: this.ParentOrganization(),
                    Priority: this.Priority(),
                    DueDate: this.DueDate(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return DataMartViewModel;
        }(DataMartListViewModel));
        ViewModels.DataMartViewModel = DataMartViewModel;
        var ResponseDetailViewModel = (function (_super) {
            __extends(ResponseDetailViewModel, _super);
            function ResponseDetailViewModel(ResponseDetailDTO) {
                var _this = _super.call(this) || this;
                if (ResponseDetailDTO == null) {
                    _this.Request = ko.observable();
                    _this.RequestID = ko.observable();
                    _this.DataMart = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.SubmittedBy = ko.observable();
                    _this.RespondedBy = ko.observable();
                    _this.Status = ko.observable();
                    _this.RequestDataMartID = ko.observable();
                    _this.ResponseGroupID = ko.observable();
                    _this.RespondedByID = ko.observable();
                    _this.ResponseTime = ko.observable();
                    _this.Count = ko.observable();
                    _this.SubmittedOn = ko.observable();
                    _this.SubmittedByID = ko.observable();
                    _this.SubmitMessage = ko.observable();
                    _this.ResponseMessage = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Request = ko.observable(ResponseDetailDTO.Request);
                    _this.RequestID = ko.observable(ResponseDetailDTO.RequestID);
                    _this.DataMart = ko.observable(ResponseDetailDTO.DataMart);
                    _this.DataMartID = ko.observable(ResponseDetailDTO.DataMartID);
                    _this.SubmittedBy = ko.observable(ResponseDetailDTO.SubmittedBy);
                    _this.RespondedBy = ko.observable(ResponseDetailDTO.RespondedBy);
                    _this.Status = ko.observable(ResponseDetailDTO.Status);
                    _this.RequestDataMartID = ko.observable(ResponseDetailDTO.RequestDataMartID);
                    _this.ResponseGroupID = ko.observable(ResponseDetailDTO.ResponseGroupID);
                    _this.RespondedByID = ko.observable(ResponseDetailDTO.RespondedByID);
                    _this.ResponseTime = ko.observable(ResponseDetailDTO.ResponseTime);
                    _this.Count = ko.observable(ResponseDetailDTO.Count);
                    _this.SubmittedOn = ko.observable(ResponseDetailDTO.SubmittedOn);
                    _this.SubmittedByID = ko.observable(ResponseDetailDTO.SubmittedByID);
                    _this.SubmitMessage = ko.observable(ResponseDetailDTO.SubmitMessage);
                    _this.ResponseMessage = ko.observable(ResponseDetailDTO.ResponseMessage);
                    _this.ID = ko.observable(ResponseDetailDTO.ID);
                    _this.Timestamp = ko.observable(ResponseDetailDTO.Timestamp);
                }
                return _this;
            }
            ResponseDetailViewModel.prototype.toData = function () {
                return {
                    Request: this.Request(),
                    RequestID: this.RequestID(),
                    DataMart: this.DataMart(),
                    DataMartID: this.DataMartID(),
                    SubmittedBy: this.SubmittedBy(),
                    RespondedBy: this.RespondedBy(),
                    Status: this.Status(),
                    RequestDataMartID: this.RequestDataMartID(),
                    ResponseGroupID: this.ResponseGroupID(),
                    RespondedByID: this.RespondedByID(),
                    ResponseTime: this.ResponseTime(),
                    Count: this.Count(),
                    SubmittedOn: this.SubmittedOn(),
                    SubmittedByID: this.SubmittedByID(),
                    SubmitMessage: this.SubmitMessage(),
                    ResponseMessage: this.ResponseMessage(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return ResponseDetailViewModel;
        }(ResponseViewModel));
        ViewModels.ResponseDetailViewModel = ResponseDetailViewModel;
        var AclDataMartRequestTypeViewModel = (function (_super) {
            __extends(AclDataMartRequestTypeViewModel, _super);
            function AclDataMartRequestTypeViewModel(AclDataMartRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (AclDataMartRequestTypeDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.DataMartID = ko.observable(AclDataMartRequestTypeDTO.DataMartID);
                    _this.RequestTypeID = ko.observable(AclDataMartRequestTypeDTO.RequestTypeID);
                    _this.Permission = ko.observable(AclDataMartRequestTypeDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclDataMartRequestTypeDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclDataMartRequestTypeDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclDataMartRequestTypeDTO.Overridden);
                }
                return _this;
            }
            AclDataMartRequestTypeViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    RequestTypeID: this.RequestTypeID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclDataMartRequestTypeViewModel;
        }(BaseAclRequestTypeViewModel));
        ViewModels.AclDataMartRequestTypeViewModel = AclDataMartRequestTypeViewModel;
        var AclDataMartViewModel = (function (_super) {
            __extends(AclDataMartViewModel, _super);
            function AclDataMartViewModel(AclDataMartDTO) {
                var _this = _super.call(this) || this;
                if (AclDataMartDTO == null) {
                    _this.DataMartID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.DataMartID = ko.observable(AclDataMartDTO.DataMartID);
                    _this.Allowed = ko.observable(AclDataMartDTO.Allowed);
                    _this.PermissionID = ko.observable(AclDataMartDTO.PermissionID);
                    _this.Permission = ko.observable(AclDataMartDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclDataMartDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclDataMartDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclDataMartDTO.Overridden);
                }
                return _this;
            }
            AclDataMartViewModel.prototype.toData = function () {
                return {
                    DataMartID: this.DataMartID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclDataMartViewModel;
        }(AclViewModel));
        ViewModels.AclDataMartViewModel = AclDataMartViewModel;
        var AclGroupViewModel = (function (_super) {
            __extends(AclGroupViewModel, _super);
            function AclGroupViewModel(AclGroupDTO) {
                var _this = _super.call(this) || this;
                if (AclGroupDTO == null) {
                    _this.GroupID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.GroupID = ko.observable(AclGroupDTO.GroupID);
                    _this.Allowed = ko.observable(AclGroupDTO.Allowed);
                    _this.PermissionID = ko.observable(AclGroupDTO.PermissionID);
                    _this.Permission = ko.observable(AclGroupDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclGroupDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclGroupDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclGroupDTO.Overridden);
                }
                return _this;
            }
            AclGroupViewModel.prototype.toData = function () {
                return {
                    GroupID: this.GroupID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclGroupViewModel;
        }(AclViewModel));
        ViewModels.AclGroupViewModel = AclGroupViewModel;
        var AclOrganizationViewModel = (function (_super) {
            __extends(AclOrganizationViewModel, _super);
            function AclOrganizationViewModel(AclOrganizationDTO) {
                var _this = _super.call(this) || this;
                if (AclOrganizationDTO == null) {
                    _this.OrganizationID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.OrganizationID = ko.observable(AclOrganizationDTO.OrganizationID);
                    _this.Allowed = ko.observable(AclOrganizationDTO.Allowed);
                    _this.PermissionID = ko.observable(AclOrganizationDTO.PermissionID);
                    _this.Permission = ko.observable(AclOrganizationDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclOrganizationDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclOrganizationDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclOrganizationDTO.Overridden);
                }
                return _this;
            }
            AclOrganizationViewModel.prototype.toData = function () {
                return {
                    OrganizationID: this.OrganizationID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclOrganizationViewModel;
        }(AclViewModel));
        ViewModels.AclOrganizationViewModel = AclOrganizationViewModel;
        var AclProjectOrganizationViewModel = (function (_super) {
            __extends(AclProjectOrganizationViewModel, _super);
            function AclProjectOrganizationViewModel(AclProjectOrganizationDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectOrganizationDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectOrganizationDTO.ProjectID);
                    _this.OrganizationID = ko.observable(AclProjectOrganizationDTO.OrganizationID);
                    _this.Allowed = ko.observable(AclProjectOrganizationDTO.Allowed);
                    _this.PermissionID = ko.observable(AclProjectOrganizationDTO.PermissionID);
                    _this.Permission = ko.observable(AclProjectOrganizationDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectOrganizationDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectOrganizationDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectOrganizationDTO.Overridden);
                }
                return _this;
            }
            AclProjectOrganizationViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    OrganizationID: this.OrganizationID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectOrganizationViewModel;
        }(AclViewModel));
        ViewModels.AclProjectOrganizationViewModel = AclProjectOrganizationViewModel;
        var AclProjectDataMartViewModel = (function (_super) {
            __extends(AclProjectDataMartViewModel, _super);
            function AclProjectDataMartViewModel(AclProjectDataMartDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectDataMartDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectDataMartDTO.ProjectID);
                    _this.DataMartID = ko.observable(AclProjectDataMartDTO.DataMartID);
                    _this.Allowed = ko.observable(AclProjectDataMartDTO.Allowed);
                    _this.PermissionID = ko.observable(AclProjectDataMartDTO.PermissionID);
                    _this.Permission = ko.observable(AclProjectDataMartDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectDataMartDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectDataMartDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectDataMartDTO.Overridden);
                }
                return _this;
            }
            AclProjectDataMartViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    DataMartID: this.DataMartID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectDataMartViewModel;
        }(AclViewModel));
        ViewModels.AclProjectDataMartViewModel = AclProjectDataMartViewModel;
        var AclProjectViewModel = (function (_super) {
            __extends(AclProjectViewModel, _super);
            function AclProjectViewModel(AclProjectDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectDTO.ProjectID);
                    _this.Allowed = ko.observable(AclProjectDTO.Allowed);
                    _this.PermissionID = ko.observable(AclProjectDTO.PermissionID);
                    _this.Permission = ko.observable(AclProjectDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectDTO.Overridden);
                }
                return _this;
            }
            AclProjectViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectViewModel;
        }(AclViewModel));
        ViewModels.AclProjectViewModel = AclProjectViewModel;
        var AclProjectRequestTypeViewModel = (function (_super) {
            __extends(AclProjectRequestTypeViewModel, _super);
            function AclProjectRequestTypeViewModel(AclProjectRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectRequestTypeDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectRequestTypeDTO.ProjectID);
                    _this.RequestTypeID = ko.observable(AclProjectRequestTypeDTO.RequestTypeID);
                    _this.Permission = ko.observable(AclProjectRequestTypeDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectRequestTypeDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectRequestTypeDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectRequestTypeDTO.Overridden);
                }
                return _this;
            }
            AclProjectRequestTypeViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    RequestTypeID: this.RequestTypeID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectRequestTypeViewModel;
        }(BaseAclRequestTypeViewModel));
        ViewModels.AclProjectRequestTypeViewModel = AclProjectRequestTypeViewModel;
        var AclRegistryViewModel = (function (_super) {
            __extends(AclRegistryViewModel, _super);
            function AclRegistryViewModel(AclRegistryDTO) {
                var _this = _super.call(this) || this;
                if (AclRegistryDTO == null) {
                    _this.RegistryID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.RegistryID = ko.observable(AclRegistryDTO.RegistryID);
                    _this.Allowed = ko.observable(AclRegistryDTO.Allowed);
                    _this.PermissionID = ko.observable(AclRegistryDTO.PermissionID);
                    _this.Permission = ko.observable(AclRegistryDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclRegistryDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclRegistryDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclRegistryDTO.Overridden);
                }
                return _this;
            }
            AclRegistryViewModel.prototype.toData = function () {
                return {
                    RegistryID: this.RegistryID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclRegistryViewModel;
        }(AclViewModel));
        ViewModels.AclRegistryViewModel = AclRegistryViewModel;
        var AclRequestTypeViewModel = (function (_super) {
            __extends(AclRequestTypeViewModel, _super);
            function AclRequestTypeViewModel(AclRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (AclRequestTypeDTO == null) {
                    _this.RequestTypeID = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.RequestTypeID = ko.observable(AclRequestTypeDTO.RequestTypeID);
                    _this.RequestType = ko.observable(AclRequestTypeDTO.RequestType);
                    _this.Allowed = ko.observable(AclRequestTypeDTO.Allowed);
                    _this.PermissionID = ko.observable(AclRequestTypeDTO.PermissionID);
                    _this.Permission = ko.observable(AclRequestTypeDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclRequestTypeDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclRequestTypeDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclRequestTypeDTO.Overridden);
                }
                return _this;
            }
            AclRequestTypeViewModel.prototype.toData = function () {
                return {
                    RequestTypeID: this.RequestTypeID(),
                    RequestType: this.RequestType(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclRequestTypeViewModel;
        }(AclViewModel));
        ViewModels.AclRequestTypeViewModel = AclRequestTypeViewModel;
        var AclUserViewModel = (function (_super) {
            __extends(AclUserViewModel, _super);
            function AclUserViewModel(AclUserDTO) {
                var _this = _super.call(this) || this;
                if (AclUserDTO == null) {
                    _this.UserID = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.UserID = ko.observable(AclUserDTO.UserID);
                    _this.Allowed = ko.observable(AclUserDTO.Allowed);
                    _this.PermissionID = ko.observable(AclUserDTO.PermissionID);
                    _this.Permission = ko.observable(AclUserDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclUserDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclUserDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclUserDTO.Overridden);
                }
                return _this;
            }
            AclUserViewModel.prototype.toData = function () {
                return {
                    UserID: this.UserID(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclUserViewModel;
        }(AclViewModel));
        ViewModels.AclUserViewModel = AclUserViewModel;
        var SecurityGroupWithUsersViewModel = (function (_super) {
            __extends(SecurityGroupWithUsersViewModel, _super);
            function SecurityGroupWithUsersViewModel(SecurityGroupWithUsersDTO) {
                var _this = _super.call(this) || this;
                if (SecurityGroupWithUsersDTO == null) {
                    _this.Users = ko.observableArray();
                    _this.Name = ko.observable();
                    _this.Path = ko.observable();
                    _this.OwnerID = ko.observable();
                    _this.Owner = ko.observable();
                    _this.ParentSecurityGroupID = ko.observable();
                    _this.ParentSecurityGroup = ko.observable();
                    _this.Kind = ko.observable();
                    _this.Type = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.Users = ko.observableArray(SecurityGroupWithUsersDTO.Users == null ? null : SecurityGroupWithUsersDTO.Users.map(function (item) { return item; }));
                    _this.Name = ko.observable(SecurityGroupWithUsersDTO.Name);
                    _this.Path = ko.observable(SecurityGroupWithUsersDTO.Path);
                    _this.OwnerID = ko.observable(SecurityGroupWithUsersDTO.OwnerID);
                    _this.Owner = ko.observable(SecurityGroupWithUsersDTO.Owner);
                    _this.ParentSecurityGroupID = ko.observable(SecurityGroupWithUsersDTO.ParentSecurityGroupID);
                    _this.ParentSecurityGroup = ko.observable(SecurityGroupWithUsersDTO.ParentSecurityGroup);
                    _this.Kind = ko.observable(SecurityGroupWithUsersDTO.Kind);
                    _this.Type = ko.observable(SecurityGroupWithUsersDTO.Type);
                    _this.ID = ko.observable(SecurityGroupWithUsersDTO.ID);
                    _this.Timestamp = ko.observable(SecurityGroupWithUsersDTO.Timestamp);
                }
                return _this;
            }
            SecurityGroupWithUsersViewModel.prototype.toData = function () {
                return {
                    Users: this.Users(),
                    Name: this.Name(),
                    Path: this.Path(),
                    OwnerID: this.OwnerID(),
                    Owner: this.Owner(),
                    ParentSecurityGroupID: this.ParentSecurityGroupID(),
                    ParentSecurityGroup: this.ParentSecurityGroup(),
                    Kind: this.Kind(),
                    Type: this.Type(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return SecurityGroupWithUsersViewModel;
        }(SecurityGroupViewModel));
        ViewModels.SecurityGroupWithUsersViewModel = SecurityGroupWithUsersViewModel;
        var UserWithSecurityDetailsViewModel = (function (_super) {
            __extends(UserWithSecurityDetailsViewModel, _super);
            function UserWithSecurityDetailsViewModel(UserWithSecurityDetailsDTO) {
                var _this = _super.call(this) || this;
                if (UserWithSecurityDetailsDTO == null) {
                    _this.PasswordHash = ko.observable();
                    _this.UserName = ko.observable();
                    _this.Title = ko.observable();
                    _this.FirstName = ko.observable();
                    _this.LastName = ko.observable();
                    _this.MiddleName = ko.observable();
                    _this.Phone = ko.observable();
                    _this.Fax = ko.observable();
                    _this.Email = ko.observable();
                    _this.Active = ko.observable();
                    _this.Deleted = ko.observable();
                    _this.OrganizationID = ko.observable();
                    _this.Organization = ko.observable();
                    _this.OrganizationRequested = ko.observable();
                    _this.RoleID = ko.observable();
                    _this.RoleRequested = ko.observable();
                    _this.SignedUpOn = ko.observable();
                    _this.ActivatedOn = ko.observable();
                    _this.DeactivatedOn = ko.observable();
                    _this.DeactivatedByID = ko.observable();
                    _this.DeactivatedBy = ko.observable();
                    _this.DeactivationReason = ko.observable();
                    _this.RejectReason = ko.observable();
                    _this.RejectedOn = ko.observable();
                    _this.RejectedByID = ko.observable();
                    _this.RejectedBy = ko.observable();
                    _this.ID = ko.observable();
                    _this.Timestamp = ko.observable();
                }
                else {
                    _this.PasswordHash = ko.observable(UserWithSecurityDetailsDTO.PasswordHash);
                    _this.UserName = ko.observable(UserWithSecurityDetailsDTO.UserName);
                    _this.Title = ko.observable(UserWithSecurityDetailsDTO.Title);
                    _this.FirstName = ko.observable(UserWithSecurityDetailsDTO.FirstName);
                    _this.LastName = ko.observable(UserWithSecurityDetailsDTO.LastName);
                    _this.MiddleName = ko.observable(UserWithSecurityDetailsDTO.MiddleName);
                    _this.Phone = ko.observable(UserWithSecurityDetailsDTO.Phone);
                    _this.Fax = ko.observable(UserWithSecurityDetailsDTO.Fax);
                    _this.Email = ko.observable(UserWithSecurityDetailsDTO.Email);
                    _this.Active = ko.observable(UserWithSecurityDetailsDTO.Active);
                    _this.Deleted = ko.observable(UserWithSecurityDetailsDTO.Deleted);
                    _this.OrganizationID = ko.observable(UserWithSecurityDetailsDTO.OrganizationID);
                    _this.Organization = ko.observable(UserWithSecurityDetailsDTO.Organization);
                    _this.OrganizationRequested = ko.observable(UserWithSecurityDetailsDTO.OrganizationRequested);
                    _this.RoleID = ko.observable(UserWithSecurityDetailsDTO.RoleID);
                    _this.RoleRequested = ko.observable(UserWithSecurityDetailsDTO.RoleRequested);
                    _this.SignedUpOn = ko.observable(UserWithSecurityDetailsDTO.SignedUpOn);
                    _this.ActivatedOn = ko.observable(UserWithSecurityDetailsDTO.ActivatedOn);
                    _this.DeactivatedOn = ko.observable(UserWithSecurityDetailsDTO.DeactivatedOn);
                    _this.DeactivatedByID = ko.observable(UserWithSecurityDetailsDTO.DeactivatedByID);
                    _this.DeactivatedBy = ko.observable(UserWithSecurityDetailsDTO.DeactivatedBy);
                    _this.DeactivationReason = ko.observable(UserWithSecurityDetailsDTO.DeactivationReason);
                    _this.RejectReason = ko.observable(UserWithSecurityDetailsDTO.RejectReason);
                    _this.RejectedOn = ko.observable(UserWithSecurityDetailsDTO.RejectedOn);
                    _this.RejectedByID = ko.observable(UserWithSecurityDetailsDTO.RejectedByID);
                    _this.RejectedBy = ko.observable(UserWithSecurityDetailsDTO.RejectedBy);
                    _this.ID = ko.observable(UserWithSecurityDetailsDTO.ID);
                    _this.Timestamp = ko.observable(UserWithSecurityDetailsDTO.Timestamp);
                }
                return _this;
            }
            UserWithSecurityDetailsViewModel.prototype.toData = function () {
                return {
                    PasswordHash: this.PasswordHash(),
                    UserName: this.UserName(),
                    Title: this.Title(),
                    FirstName: this.FirstName(),
                    LastName: this.LastName(),
                    MiddleName: this.MiddleName(),
                    Phone: this.Phone(),
                    Fax: this.Fax(),
                    Email: this.Email(),
                    Active: this.Active(),
                    Deleted: this.Deleted(),
                    OrganizationID: this.OrganizationID(),
                    Organization: this.Organization(),
                    OrganizationRequested: this.OrganizationRequested(),
                    RoleID: this.RoleID(),
                    RoleRequested: this.RoleRequested(),
                    SignedUpOn: this.SignedUpOn(),
                    ActivatedOn: this.ActivatedOn(),
                    DeactivatedOn: this.DeactivatedOn(),
                    DeactivatedByID: this.DeactivatedByID(),
                    DeactivatedBy: this.DeactivatedBy(),
                    DeactivationReason: this.DeactivationReason(),
                    RejectReason: this.RejectReason(),
                    RejectedOn: this.RejectedOn(),
                    RejectedByID: this.RejectedByID(),
                    RejectedBy: this.RejectedBy(),
                    ID: this.ID(),
                    Timestamp: this.Timestamp(),
                };
            };
            return UserWithSecurityDetailsViewModel;
        }(UserViewModel));
        ViewModels.UserWithSecurityDetailsViewModel = UserWithSecurityDetailsViewModel;
        var AclProjectRequestTypeWorkflowActivityViewModel = (function (_super) {
            __extends(AclProjectRequestTypeWorkflowActivityViewModel, _super);
            function AclProjectRequestTypeWorkflowActivityViewModel(AclProjectRequestTypeWorkflowActivityDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectRequestTypeWorkflowActivityDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.Project = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.RequestType = ko.observable();
                    _this.WorkflowActivityID = ko.observable();
                    _this.WorkflowActivity = ko.observable();
                    _this.Allowed = ko.observable();
                    _this.PermissionID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.ProjectID);
                    _this.Project = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Project);
                    _this.RequestTypeID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.RequestTypeID);
                    _this.RequestType = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.RequestType);
                    _this.WorkflowActivityID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.WorkflowActivityID);
                    _this.WorkflowActivity = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.WorkflowActivity);
                    _this.Allowed = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Allowed);
                    _this.PermissionID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.PermissionID);
                    _this.Permission = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Overridden);
                }
                return _this;
            }
            AclProjectRequestTypeWorkflowActivityViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    Project: this.Project(),
                    RequestTypeID: this.RequestTypeID(),
                    RequestType: this.RequestType(),
                    WorkflowActivityID: this.WorkflowActivityID(),
                    WorkflowActivity: this.WorkflowActivity(),
                    Allowed: this.Allowed(),
                    PermissionID: this.PermissionID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectRequestTypeWorkflowActivityViewModel;
        }(AclViewModel));
        ViewModels.AclProjectRequestTypeWorkflowActivityViewModel = AclProjectRequestTypeWorkflowActivityViewModel;
        var AclProjectDataMartRequestTypeViewModel = (function (_super) {
            __extends(AclProjectDataMartRequestTypeViewModel, _super);
            function AclProjectDataMartRequestTypeViewModel(AclProjectDataMartRequestTypeDTO) {
                var _this = _super.call(this) || this;
                if (AclProjectDataMartRequestTypeDTO == null) {
                    _this.ProjectID = ko.observable();
                    _this.DataMartID = ko.observable();
                    _this.RequestTypeID = ko.observable();
                    _this.Permission = ko.observable();
                    _this.SecurityGroupID = ko.observable();
                    _this.SecurityGroup = ko.observable();
                    _this.Overridden = ko.observable();
                }
                else {
                    _this.ProjectID = ko.observable(AclProjectDataMartRequestTypeDTO.ProjectID);
                    _this.DataMartID = ko.observable(AclProjectDataMartRequestTypeDTO.DataMartID);
                    _this.RequestTypeID = ko.observable(AclProjectDataMartRequestTypeDTO.RequestTypeID);
                    _this.Permission = ko.observable(AclProjectDataMartRequestTypeDTO.Permission);
                    _this.SecurityGroupID = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroupID);
                    _this.SecurityGroup = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroup);
                    _this.Overridden = ko.observable(AclProjectDataMartRequestTypeDTO.Overridden);
                }
                return _this;
            }
            AclProjectDataMartRequestTypeViewModel.prototype.toData = function () {
                return {
                    ProjectID: this.ProjectID(),
                    DataMartID: this.DataMartID(),
                    RequestTypeID: this.RequestTypeID(),
                    Permission: this.Permission(),
                    SecurityGroupID: this.SecurityGroupID(),
                    SecurityGroup: this.SecurityGroup(),
                    Overridden: this.Overridden(),
                };
            };
            return AclProjectDataMartRequestTypeViewModel;
        }(AclDataMartRequestTypeViewModel));
        ViewModels.AclProjectDataMartRequestTypeViewModel = AclProjectDataMartRequestTypeViewModel;
    })(ViewModels = Dns.ViewModels || (Dns.ViewModels = {}));
})(Dns || (Dns = {}));
//# sourceMappingURL=Lpp.Dns.ViewModels.js.map
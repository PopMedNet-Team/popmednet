export class ViewModel {
    constructor() {
    }
    update(obj) {
        for (var prop in obj) {
            this[prop](obj[prop]);
        }
    }
}
export class EntityDtoViewModel extends ViewModel {
    constructor(BaseDTO) {
        super();
    }
    toData() {
        return {};
    }
}
export class EntityDtoWithIDViewModel extends EntityDtoViewModel {
    ID;
    Timestamp;
    constructor(BaseDTO) {
        super(BaseDTO);
        if (BaseDTO == null) {
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class DataMartAvailabilityPeriodV2ViewModel extends ViewModel {
    DataMartID;
    DataMart;
    DataTable;
    PeriodCategory;
    Period;
    Year;
    Quarter;
    constructor(DataMartAvailabilityPeriodV2DTO) {
        super();
        if (DataMartAvailabilityPeriodV2DTO == null) {
            this.DataMartID = ko.observable();
            this.DataMart = ko.observable();
            this.DataTable = ko.observable();
            this.PeriodCategory = ko.observable();
            this.Period = ko.observable();
            this.Year = ko.observable();
            this.Quarter = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(DataMartAvailabilityPeriodV2DTO.DataMartID);
            this.DataMart = ko.observable(DataMartAvailabilityPeriodV2DTO.DataMart);
            this.DataTable = ko.observable(DataMartAvailabilityPeriodV2DTO.DataTable);
            this.PeriodCategory = ko.observable(DataMartAvailabilityPeriodV2DTO.PeriodCategory);
            this.Period = ko.observable(DataMartAvailabilityPeriodV2DTO.Period);
            this.Year = ko.observable(DataMartAvailabilityPeriodV2DTO.Year);
            this.Quarter = ko.observable(DataMartAvailabilityPeriodV2DTO.Quarter);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            DataMart: this.DataMart(),
            DataTable: this.DataTable(),
            PeriodCategory: this.PeriodCategory(),
            Period: this.Period(),
            Year: this.Year(),
            Quarter: this.Quarter(),
        };
    }
}
export class DataModelProcessorViewModel extends ViewModel {
    ModelID;
    Processor;
    ProcessorID;
    constructor(DataModelProcessorDTO) {
        super();
        if (DataModelProcessorDTO == null) {
            this.ModelID = ko.observable();
            this.Processor = ko.observable();
            this.ProcessorID = ko.observable();
        }
        else {
            this.ModelID = ko.observable(DataModelProcessorDTO.ModelID);
            this.Processor = ko.observable(DataModelProcessorDTO.Processor);
            this.ProcessorID = ko.observable(DataModelProcessorDTO.ProcessorID);
        }
    }
    toData() {
        return {
            ModelID: this.ModelID(),
            Processor: this.Processor(),
            ProcessorID: this.ProcessorID(),
        };
    }
}
export class PropertyChangeDetailViewModel extends ViewModel {
    Property;
    PropertyDisplayName;
    OriginalValue;
    OriginalValueDisplay;
    NewValue;
    NewValueDisplay;
    constructor(PropertyChangeDetailDTO) {
        super();
        if (PropertyChangeDetailDTO == null) {
            this.Property = ko.observable();
            this.PropertyDisplayName = ko.observable();
            this.OriginalValue = ko.observable();
            this.OriginalValueDisplay = ko.observable();
            this.NewValue = ko.observable();
            this.NewValueDisplay = ko.observable();
        }
        else {
            this.Property = ko.observable(PropertyChangeDetailDTO.Property);
            this.PropertyDisplayName = ko.observable(PropertyChangeDetailDTO.PropertyDisplayName);
            this.OriginalValue = ko.observable(PropertyChangeDetailDTO.OriginalValue);
            this.OriginalValueDisplay = ko.observable(PropertyChangeDetailDTO.OriginalValueDisplay);
            this.NewValue = ko.observable(PropertyChangeDetailDTO.NewValue);
            this.NewValueDisplay = ko.observable(PropertyChangeDetailDTO.NewValueDisplay);
        }
    }
    toData() {
        return {
            Property: this.Property(),
            PropertyDisplayName: this.PropertyDisplayName(),
            OriginalValue: this.OriginalValue(),
            OriginalValueDisplay: this.OriginalValueDisplay(),
            NewValue: this.NewValue(),
            NewValueDisplay: this.NewValueDisplay(),
        };
    }
}
export class HttpResponseErrors extends ViewModel {
    Errors;
    constructor(HttpResponseErrors) {
        super();
        if (HttpResponseErrors == null) {
            this.Errors = ko.observableArray();
        }
        else {
            this.Errors = ko.observableArray(HttpResponseErrors.Errors == null ? null : HttpResponseErrors.Errors.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            Errors: this.Errors == null ? null : this.Errors().map((item) => { return item; }),
        };
    }
}
export class AddWFCommentViewModel extends ViewModel {
    RequestID;
    WorkflowActivityID;
    Comment;
    constructor(AddWFCommentDTO) {
        super();
        if (AddWFCommentDTO == null) {
            this.RequestID = ko.observable();
            this.WorkflowActivityID = ko.observable();
            this.Comment = ko.observable();
        }
        else {
            this.RequestID = ko.observable(AddWFCommentDTO.RequestID);
            this.WorkflowActivityID = ko.observable(AddWFCommentDTO.WorkflowActivityID);
            this.Comment = ko.observable(AddWFCommentDTO.Comment);
        }
    }
    toData() {
        return {
            RequestID: this.RequestID(),
            WorkflowActivityID: this.WorkflowActivityID(),
            Comment: this.Comment(),
        };
    }
}
export class CommentDocumentReferenceViewModel extends ViewModel {
    CommentID;
    DocumentID;
    RevisionSetID;
    DocumentName;
    FileName;
    constructor(CommentDocumentReferenceDTO) {
        super();
        if (CommentDocumentReferenceDTO == null) {
            this.CommentID = ko.observable();
            this.DocumentID = ko.observable();
            this.RevisionSetID = ko.observable();
            this.DocumentName = ko.observable();
            this.FileName = ko.observable();
        }
        else {
            this.CommentID = ko.observable(CommentDocumentReferenceDTO.CommentID);
            this.DocumentID = ko.observable(CommentDocumentReferenceDTO.DocumentID);
            this.RevisionSetID = ko.observable(CommentDocumentReferenceDTO.RevisionSetID);
            this.DocumentName = ko.observable(CommentDocumentReferenceDTO.DocumentName);
            this.FileName = ko.observable(CommentDocumentReferenceDTO.FileName);
        }
    }
    toData() {
        return {
            CommentID: this.CommentID(),
            DocumentID: this.DocumentID(),
            RevisionSetID: this.RevisionSetID(),
            DocumentName: this.DocumentName(),
            FileName: this.FileName(),
        };
    }
}
export class UpdateDataMartInstalledModelsViewModel extends ViewModel {
    DataMartID;
    Models;
    constructor(UpdateDataMartInstalledModelsDTO) {
        super();
        if (UpdateDataMartInstalledModelsDTO == null) {
            this.DataMartID = ko.observable();
            this.Models = ko.observableArray();
        }
        else {
            this.DataMartID = ko.observable(UpdateDataMartInstalledModelsDTO.DataMartID);
            this.Models = ko.observableArray(UpdateDataMartInstalledModelsDTO.Models == null ? null : UpdateDataMartInstalledModelsDTO.Models.map((item) => { return new DataMartInstalledModelViewModel(item); }));
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            Models: this.Models == null ? null : this.Models().map((item) => { return item.toData(); }),
        };
    }
}
export class DataAvailabilityPeriodCategoryViewModel extends ViewModel {
    CategoryType;
    CategoryDescription;
    Published;
    DataMartDescription;
    constructor(DataAvailabilityPeriodCategoryDTO) {
        super();
        if (DataAvailabilityPeriodCategoryDTO == null) {
            this.CategoryType = ko.observable();
            this.CategoryDescription = ko.observable();
            this.Published = ko.observable();
            this.DataMartDescription = ko.observable();
        }
        else {
            this.CategoryType = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryType);
            this.CategoryDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryDescription);
            this.Published = ko.observable(DataAvailabilityPeriodCategoryDTO.Published);
            this.DataMartDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.DataMartDescription);
        }
    }
    toData() {
        return {
            CategoryType: this.CategoryType(),
            CategoryDescription: this.CategoryDescription(),
            Published: this.Published(),
            DataMartDescription: this.DataMartDescription(),
        };
    }
}
export class DataMartAvailabilityPeriodViewModel extends ViewModel {
    DataMartID;
    RequestID;
    RequestTypeID;
    Period;
    Active;
    constructor(DataMartAvailabilityPeriodDTO) {
        super();
        if (DataMartAvailabilityPeriodDTO == null) {
            this.DataMartID = ko.observable();
            this.RequestID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.Period = ko.observable();
            this.Active = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(DataMartAvailabilityPeriodDTO.DataMartID);
            this.RequestID = ko.observable(DataMartAvailabilityPeriodDTO.RequestID);
            this.RequestTypeID = ko.observable(DataMartAvailabilityPeriodDTO.RequestTypeID);
            this.Period = ko.observable(DataMartAvailabilityPeriodDTO.Period);
            this.Active = ko.observable(DataMartAvailabilityPeriodDTO.Active);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            RequestID: this.RequestID(),
            RequestTypeID: this.RequestTypeID(),
            Period: this.Period(),
            Active: this.Active(),
        };
    }
}
export class NotificationCrudViewModel extends ViewModel {
    ObjectID;
    State;
    constructor(NotificationCrudDTO) {
        super();
        if (NotificationCrudDTO == null) {
            this.ObjectID = ko.observable();
            this.State = ko.observable();
        }
        else {
            this.ObjectID = ko.observable(NotificationCrudDTO.ObjectID);
            this.State = ko.observable(NotificationCrudDTO.State);
        }
    }
    toData() {
        return {
            ObjectID: this.ObjectID(),
            State: this.State(),
        };
    }
}
export class OrganizationUpdateEHRsesViewModel extends ViewModel {
    OrganizationID;
    EHRS;
    constructor(OrganizationUpdateEHRsesDTO) {
        super();
        if (OrganizationUpdateEHRsesDTO == null) {
            this.OrganizationID = ko.observable();
            this.EHRS = ko.observableArray();
        }
        else {
            this.OrganizationID = ko.observable(OrganizationUpdateEHRsesDTO.OrganizationID);
            this.EHRS = ko.observableArray(OrganizationUpdateEHRsesDTO.EHRS == null ? null : OrganizationUpdateEHRsesDTO.EHRS.map((item) => { return new OrganizationEHRSViewModel(item); }));
        }
    }
    toData() {
        return {
            OrganizationID: this.OrganizationID(),
            EHRS: this.EHRS == null ? null : this.EHRS().map((item) => { return item.toData(); }),
        };
    }
}
export class ProjectDataMartUpdateViewModel extends ViewModel {
    ProjectID;
    DataMarts;
    constructor(ProjectDataMartUpdateDTO) {
        super();
        if (ProjectDataMartUpdateDTO == null) {
            this.ProjectID = ko.observable();
            this.DataMarts = ko.observableArray();
        }
        else {
            this.ProjectID = ko.observable(ProjectDataMartUpdateDTO.ProjectID);
            this.DataMarts = ko.observableArray(ProjectDataMartUpdateDTO.DataMarts == null ? null : ProjectDataMartUpdateDTO.DataMarts.map((item) => { return new ProjectDataMartViewModel(item); }));
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => { return item.toData(); }),
        };
    }
}
export class ProjectOrganizationUpdateViewModel extends ViewModel {
    ProjectID;
    Organizations;
    constructor(ProjectOrganizationUpdateDTO) {
        super();
        if (ProjectOrganizationUpdateDTO == null) {
            this.ProjectID = ko.observable();
            this.Organizations = ko.observableArray();
        }
        else {
            this.ProjectID = ko.observable(ProjectOrganizationUpdateDTO.ProjectID);
            this.Organizations = ko.observableArray(ProjectOrganizationUpdateDTO.Organizations == null ? null : ProjectOrganizationUpdateDTO.Organizations.map((item) => { return new ProjectOrganizationViewModel(item); }));
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            Organizations: this.Organizations == null ? null : this.Organizations().map((item) => { return item.toData(); }),
        };
    }
}
export class UpdateProjectRequestTypesViewModel extends ViewModel {
    ProjectID;
    RequestTypes;
    constructor(UpdateProjectRequestTypesDTO) {
        super();
        if (UpdateProjectRequestTypesDTO == null) {
            this.ProjectID = ko.observable();
            this.RequestTypes = ko.observableArray();
        }
        else {
            this.ProjectID = ko.observable(UpdateProjectRequestTypesDTO.ProjectID);
            this.RequestTypes = ko.observableArray(UpdateProjectRequestTypesDTO.RequestTypes == null ? null : UpdateProjectRequestTypesDTO.RequestTypes.map((item) => { return new ProjectRequestTypeViewModel(item); }));
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => { return item.toData(); }),
        };
    }
}
export class HasGlobalSecurityForTemplateViewModel extends ViewModel {
    SecurityGroupExistsForGlobalPermission;
    CurrentUserHasGlobalPermission;
    constructor(HasGlobalSecurityForTemplateDTO) {
        super();
        if (HasGlobalSecurityForTemplateDTO == null) {
            this.SecurityGroupExistsForGlobalPermission = ko.observable();
            this.CurrentUserHasGlobalPermission = ko.observable();
        }
        else {
            this.SecurityGroupExistsForGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.SecurityGroupExistsForGlobalPermission);
            this.CurrentUserHasGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.CurrentUserHasGlobalPermission);
        }
    }
    toData() {
        return {
            SecurityGroupExistsForGlobalPermission: this.SecurityGroupExistsForGlobalPermission(),
            CurrentUserHasGlobalPermission: this.CurrentUserHasGlobalPermission(),
        };
    }
}
export class ApproveRejectResponseViewModel extends ViewModel {
    ResponseID;
    constructor(ApproveRejectResponseDTO) {
        super();
        if (ApproveRejectResponseDTO == null) {
            this.ResponseID = ko.observable();
        }
        else {
            this.ResponseID = ko.observable(ApproveRejectResponseDTO.ResponseID);
        }
    }
    toData() {
        return {
            ResponseID: this.ResponseID(),
        };
    }
}
export class CreateCriteriaGroupTemplateViewModel extends ViewModel {
    Name;
    Description;
    Json;
    AdapterDetail;
    constructor(CreateCriteriaGroupTemplateDTO) {
        super();
        if (CreateCriteriaGroupTemplateDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Json = ko.observable();
            this.AdapterDetail = ko.observable();
        }
        else {
            this.Name = ko.observable(CreateCriteriaGroupTemplateDTO.Name);
            this.Description = ko.observable(CreateCriteriaGroupTemplateDTO.Description);
            this.Json = ko.observable(CreateCriteriaGroupTemplateDTO.Json);
            this.AdapterDetail = ko.observable(CreateCriteriaGroupTemplateDTO.AdapterDetail);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Json: this.Json(),
            AdapterDetail: this.AdapterDetail(),
        };
    }
}
export class EnhancedEventLogItemViewModel extends ViewModel {
    Step;
    Timestamp;
    Description;
    Source;
    EventType;
    constructor(EnhancedEventLogItemDTO) {
        super();
        if (EnhancedEventLogItemDTO == null) {
            this.Step = ko.observable();
            this.Timestamp = ko.observable();
            this.Description = ko.observable();
            this.Source = ko.observable();
            this.EventType = ko.observable();
        }
        else {
            this.Step = ko.observable(EnhancedEventLogItemDTO.Step);
            this.Timestamp = ko.observable(EnhancedEventLogItemDTO.Timestamp);
            this.Description = ko.observable(EnhancedEventLogItemDTO.Description);
            this.Source = ko.observable(EnhancedEventLogItemDTO.Source);
            this.EventType = ko.observable(EnhancedEventLogItemDTO.EventType);
        }
    }
    toData() {
        return {
            Step: this.Step(),
            Timestamp: this.Timestamp(),
            Description: this.Description(),
            Source: this.Source(),
            EventType: this.EventType(),
        };
    }
}
export class HomepageRouteDetailViewModel extends ViewModel {
    RequestDataMartID;
    DataMart;
    DataMartID;
    RoutingType;
    RequestID;
    Name;
    Identifier;
    SubmittedOn;
    SubmittedByName;
    ResponseID;
    ResponseSubmittedOn;
    ResponseSubmittedByID;
    ResponseSubmittedBy;
    ResponseTime;
    RespondedByID;
    RespondedBy;
    ResponseGroupID;
    ResponseGroup;
    ResponseMessage;
    StatusText;
    RequestStatus;
    RoutingStatus;
    RoutingStatusText;
    RequestType;
    Project;
    Priority;
    DueDate;
    MSRequestID;
    IsWorkflowRequest;
    CanEditMetadata;
    constructor(HomepageRouteDetailDTO) {
        super();
        if (HomepageRouteDetailDTO == null) {
            this.RequestDataMartID = ko.observable();
            this.DataMart = ko.observable();
            this.DataMartID = ko.observable();
            this.RoutingType = ko.observable();
            this.RequestID = ko.observable();
            this.Name = ko.observable();
            this.Identifier = ko.observable();
            this.SubmittedOn = ko.observable();
            this.SubmittedByName = ko.observable();
            this.ResponseID = ko.observable();
            this.ResponseSubmittedOn = ko.observable();
            this.ResponseSubmittedByID = ko.observable();
            this.ResponseSubmittedBy = ko.observable();
            this.ResponseTime = ko.observable();
            this.RespondedByID = ko.observable();
            this.RespondedBy = ko.observable();
            this.ResponseGroupID = ko.observable();
            this.ResponseGroup = ko.observable();
            this.ResponseMessage = ko.observable();
            this.StatusText = ko.observable();
            this.RequestStatus = ko.observable();
            this.RoutingStatus = ko.observable();
            this.RoutingStatusText = ko.observable();
            this.RequestType = ko.observable();
            this.Project = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.MSRequestID = ko.observable();
            this.IsWorkflowRequest = ko.observable();
            this.CanEditMetadata = ko.observable();
        }
        else {
            this.RequestDataMartID = ko.observable(HomepageRouteDetailDTO.RequestDataMartID);
            this.DataMart = ko.observable(HomepageRouteDetailDTO.DataMart);
            this.DataMartID = ko.observable(HomepageRouteDetailDTO.DataMartID);
            this.RoutingType = ko.observable(HomepageRouteDetailDTO.RoutingType);
            this.RequestID = ko.observable(HomepageRouteDetailDTO.RequestID);
            this.Name = ko.observable(HomepageRouteDetailDTO.Name);
            this.Identifier = ko.observable(HomepageRouteDetailDTO.Identifier);
            this.SubmittedOn = ko.observable(HomepageRouteDetailDTO.SubmittedOn);
            this.SubmittedByName = ko.observable(HomepageRouteDetailDTO.SubmittedByName);
            this.ResponseID = ko.observable(HomepageRouteDetailDTO.ResponseID);
            this.ResponseSubmittedOn = ko.observable(HomepageRouteDetailDTO.ResponseSubmittedOn);
            this.ResponseSubmittedByID = ko.observable(HomepageRouteDetailDTO.ResponseSubmittedByID);
            this.ResponseSubmittedBy = ko.observable(HomepageRouteDetailDTO.ResponseSubmittedBy);
            this.ResponseTime = ko.observable(HomepageRouteDetailDTO.ResponseTime);
            this.RespondedByID = ko.observable(HomepageRouteDetailDTO.RespondedByID);
            this.RespondedBy = ko.observable(HomepageRouteDetailDTO.RespondedBy);
            this.ResponseGroupID = ko.observable(HomepageRouteDetailDTO.ResponseGroupID);
            this.ResponseGroup = ko.observable(HomepageRouteDetailDTO.ResponseGroup);
            this.ResponseMessage = ko.observable(HomepageRouteDetailDTO.ResponseMessage);
            this.StatusText = ko.observable(HomepageRouteDetailDTO.StatusText);
            this.RequestStatus = ko.observable(HomepageRouteDetailDTO.RequestStatus);
            this.RoutingStatus = ko.observable(HomepageRouteDetailDTO.RoutingStatus);
            this.RoutingStatusText = ko.observable(HomepageRouteDetailDTO.RoutingStatusText);
            this.RequestType = ko.observable(HomepageRouteDetailDTO.RequestType);
            this.Project = ko.observable(HomepageRouteDetailDTO.Project);
            this.Priority = ko.observable(HomepageRouteDetailDTO.Priority);
            this.DueDate = ko.observable(HomepageRouteDetailDTO.DueDate);
            this.MSRequestID = ko.observable(HomepageRouteDetailDTO.MSRequestID);
            this.IsWorkflowRequest = ko.observable(HomepageRouteDetailDTO.IsWorkflowRequest);
            this.CanEditMetadata = ko.observable(HomepageRouteDetailDTO.CanEditMetadata);
        }
    }
    toData() {
        return {
            RequestDataMartID: this.RequestDataMartID(),
            DataMart: this.DataMart(),
            DataMartID: this.DataMartID(),
            RoutingType: this.RoutingType(),
            RequestID: this.RequestID(),
            Name: this.Name(),
            Identifier: this.Identifier(),
            SubmittedOn: this.SubmittedOn(),
            SubmittedByName: this.SubmittedByName(),
            ResponseID: this.ResponseID(),
            ResponseSubmittedOn: this.ResponseSubmittedOn(),
            ResponseSubmittedByID: this.ResponseSubmittedByID(),
            ResponseSubmittedBy: this.ResponseSubmittedBy(),
            ResponseTime: this.ResponseTime(),
            RespondedByID: this.RespondedByID(),
            RespondedBy: this.RespondedBy(),
            ResponseGroupID: this.ResponseGroupID(),
            ResponseGroup: this.ResponseGroup(),
            ResponseMessage: this.ResponseMessage(),
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
    }
}
export class RejectResponseViewModel extends ViewModel {
    Message;
    ResponseIDs;
    constructor(RejectResponseDTO) {
        super();
        if (RejectResponseDTO == null) {
            this.Message = ko.observable();
            this.ResponseIDs = ko.observableArray();
        }
        else {
            this.Message = ko.observable(RejectResponseDTO.Message);
            this.ResponseIDs = ko.observableArray(RejectResponseDTO.ResponseIDs == null ? null : RejectResponseDTO.ResponseIDs.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            Message: this.Message(),
            ResponseIDs: this.ResponseIDs(),
        };
    }
}
export class ApproveResponseViewModel extends ViewModel {
    Message;
    ResponseIDs;
    constructor(ApproveResponseDTO) {
        super();
        if (ApproveResponseDTO == null) {
            this.Message = ko.observable();
            this.ResponseIDs = ko.observableArray();
        }
        else {
            this.Message = ko.observable(ApproveResponseDTO.Message);
            this.ResponseIDs = ko.observableArray(ApproveResponseDTO.ResponseIDs == null ? null : ApproveResponseDTO.ResponseIDs.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            Message: this.Message(),
            ResponseIDs: this.ResponseIDs(),
        };
    }
}
export class RequestCompletionRequestViewModel extends ViewModel {
    DemandActivityResultID;
    Dto;
    DataMarts;
    Data;
    Comment;
    constructor(RequestCompletionRequestDTO) {
        super();
        if (RequestCompletionRequestDTO == null) {
            this.DemandActivityResultID = ko.observable();
            this.Dto = new RequestViewModel();
            this.DataMarts = ko.observableArray();
            this.Data = ko.observable();
            this.Comment = ko.observable();
        }
        else {
            this.DemandActivityResultID = ko.observable(RequestCompletionRequestDTO.DemandActivityResultID);
            this.Dto = new RequestViewModel(RequestCompletionRequestDTO.Dto);
            this.DataMarts = ko.observableArray(RequestCompletionRequestDTO.DataMarts == null ? null : RequestCompletionRequestDTO.DataMarts.map((item) => { return new RequestDataMartViewModel(item); }));
            this.Data = ko.observable(RequestCompletionRequestDTO.Data);
            this.Comment = ko.observable(RequestCompletionRequestDTO.Comment);
        }
    }
    toData() {
        return {
            DemandActivityResultID: this.DemandActivityResultID(),
            Dto: this.Dto.toData(),
            DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => { return item.toData(); }),
            Data: this.Data(),
            Comment: this.Comment(),
        };
    }
}
export class RequestCompletionResponseViewModel extends ViewModel {
    Uri;
    Entity;
    DataMarts;
    constructor(RequestCompletionResponseDTO) {
        super();
        if (RequestCompletionResponseDTO == null) {
            this.Uri = ko.observable();
            this.Entity = new RequestViewModel();
            this.DataMarts = ko.observableArray();
        }
        else {
            this.Uri = ko.observable(RequestCompletionResponseDTO.Uri);
            this.Entity = new RequestViewModel(RequestCompletionResponseDTO.Entity);
            this.DataMarts = ko.observableArray(RequestCompletionResponseDTO.DataMarts == null ? null : RequestCompletionResponseDTO.DataMarts.map((item) => { return new RequestDataMartViewModel(item); }));
        }
    }
    toData() {
        return {
            Uri: this.Uri(),
            Entity: this.Entity.toData(),
            DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => { return item.toData(); }),
        };
    }
}
export class RequestSearchTermViewModel extends ViewModel {
    Type;
    StringValue;
    NumberValue;
    DateFrom;
    DateTo;
    NumberFrom;
    NumberTo;
    RequestID;
    constructor(RequestSearchTermDTO) {
        super();
        if (RequestSearchTermDTO == null) {
            this.Type = ko.observable();
            this.StringValue = ko.observable();
            this.NumberValue = ko.observable();
            this.DateFrom = ko.observable();
            this.DateTo = ko.observable();
            this.NumberFrom = ko.observable();
            this.NumberTo = ko.observable();
            this.RequestID = ko.observable();
        }
        else {
            this.Type = ko.observable(RequestSearchTermDTO.Type);
            this.StringValue = ko.observable(RequestSearchTermDTO.StringValue);
            this.NumberValue = ko.observable(RequestSearchTermDTO.NumberValue);
            this.DateFrom = ko.observable(RequestSearchTermDTO.DateFrom);
            this.DateTo = ko.observable(RequestSearchTermDTO.DateTo);
            this.NumberFrom = ko.observable(RequestSearchTermDTO.NumberFrom);
            this.NumberTo = ko.observable(RequestSearchTermDTO.NumberTo);
            this.RequestID = ko.observable(RequestSearchTermDTO.RequestID);
        }
    }
    toData() {
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
    }
}
export class RequestTypeModelViewModel extends ViewModel {
    RequestTypeID;
    DataModelID;
    constructor(RequestTypeModelDTO) {
        super();
        if (RequestTypeModelDTO == null) {
            this.RequestTypeID = ko.observable();
            this.DataModelID = ko.observable();
        }
        else {
            this.RequestTypeID = ko.observable(RequestTypeModelDTO.RequestTypeID);
            this.DataModelID = ko.observable(RequestTypeModelDTO.DataModelID);
        }
    }
    toData() {
        return {
            RequestTypeID: this.RequestTypeID(),
            DataModelID: this.DataModelID(),
        };
    }
}
export class RequestUserViewModel extends ViewModel {
    RequestID;
    UserID;
    Username;
    FullName;
    Email;
    WorkflowRoleID;
    WorkflowRole;
    IsRequestCreatorRole;
    constructor(RequestUserDTO) {
        super();
        if (RequestUserDTO == null) {
            this.RequestID = ko.observable();
            this.UserID = ko.observable();
            this.Username = ko.observable();
            this.FullName = ko.observable();
            this.Email = ko.observable();
            this.WorkflowRoleID = ko.observable();
            this.WorkflowRole = ko.observable();
            this.IsRequestCreatorRole = ko.observable();
        }
        else {
            this.RequestID = ko.observable(RequestUserDTO.RequestID);
            this.UserID = ko.observable(RequestUserDTO.UserID);
            this.Username = ko.observable(RequestUserDTO.Username);
            this.FullName = ko.observable(RequestUserDTO.FullName);
            this.Email = ko.observable(RequestUserDTO.Email);
            this.WorkflowRoleID = ko.observable(RequestUserDTO.WorkflowRoleID);
            this.WorkflowRole = ko.observable(RequestUserDTO.WorkflowRole);
            this.IsRequestCreatorRole = ko.observable(RequestUserDTO.IsRequestCreatorRole);
        }
    }
    toData() {
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
    }
}
export class ResponseHistoryViewModel extends ViewModel {
    DataMartName;
    HistoryItems;
    ErrorMessage;
    constructor(ResponseHistoryDTO) {
        super();
        if (ResponseHistoryDTO == null) {
            this.DataMartName = ko.observable();
            this.HistoryItems = ko.observableArray();
            this.ErrorMessage = ko.observable();
        }
        else {
            this.DataMartName = ko.observable(ResponseHistoryDTO.DataMartName);
            this.HistoryItems = ko.observableArray(ResponseHistoryDTO.HistoryItems == null ? null : ResponseHistoryDTO.HistoryItems.map((item) => { return new ResponseHistoryItemViewModel(item); }));
            this.ErrorMessage = ko.observable(ResponseHistoryDTO.ErrorMessage);
        }
    }
    toData() {
        return {
            DataMartName: this.DataMartName(),
            HistoryItems: this.HistoryItems == null ? null : this.HistoryItems().map((item) => { return item.toData(); }),
            ErrorMessage: this.ErrorMessage(),
        };
    }
}
export class ResponseHistoryItemViewModel extends ViewModel {
    ResponseID;
    RequestID;
    DateTime;
    Action;
    UserName;
    Message;
    IsResponseItem;
    IsCurrent;
    constructor(ResponseHistoryItemDTO) {
        super();
        if (ResponseHistoryItemDTO == null) {
            this.ResponseID = ko.observable();
            this.RequestID = ko.observable();
            this.DateTime = ko.observable();
            this.Action = ko.observable();
            this.UserName = ko.observable();
            this.Message = ko.observable();
            this.IsResponseItem = ko.observable();
            this.IsCurrent = ko.observable();
        }
        else {
            this.ResponseID = ko.observable(ResponseHistoryItemDTO.ResponseID);
            this.RequestID = ko.observable(ResponseHistoryItemDTO.RequestID);
            this.DateTime = ko.observable(ResponseHistoryItemDTO.DateTime);
            this.Action = ko.observable(ResponseHistoryItemDTO.Action);
            this.UserName = ko.observable(ResponseHistoryItemDTO.UserName);
            this.Message = ko.observable(ResponseHistoryItemDTO.Message);
            this.IsResponseItem = ko.observable(ResponseHistoryItemDTO.IsResponseItem);
            this.IsCurrent = ko.observable(ResponseHistoryItemDTO.IsCurrent);
        }
    }
    toData() {
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
    }
}
export class SaveCriteriaGroupRequestViewModel extends ViewModel {
    Name;
    Description;
    Json;
    AdapterDetail;
    TemplateID;
    RequestTypeID;
    RequestID;
    constructor(SaveCriteriaGroupRequestDTO) {
        super();
        if (SaveCriteriaGroupRequestDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Json = ko.observable();
            this.AdapterDetail = ko.observable();
            this.TemplateID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestID = ko.observable();
        }
        else {
            this.Name = ko.observable(SaveCriteriaGroupRequestDTO.Name);
            this.Description = ko.observable(SaveCriteriaGroupRequestDTO.Description);
            this.Json = ko.observable(SaveCriteriaGroupRequestDTO.Json);
            this.AdapterDetail = ko.observable(SaveCriteriaGroupRequestDTO.AdapterDetail);
            this.TemplateID = ko.observable(SaveCriteriaGroupRequestDTO.TemplateID);
            this.RequestTypeID = ko.observable(SaveCriteriaGroupRequestDTO.RequestTypeID);
            this.RequestID = ko.observable(SaveCriteriaGroupRequestDTO.RequestID);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Json: this.Json(),
            AdapterDetail: this.AdapterDetail(),
            TemplateID: this.TemplateID(),
            RequestTypeID: this.RequestTypeID(),
            RequestID: this.RequestID(),
        };
    }
}
export class UpdateRequestDataMartStatusViewModel extends ViewModel {
    RequestDataMartID;
    DataMartID;
    NewStatus;
    Message;
    constructor(UpdateRequestDataMartStatusDTO) {
        super();
        if (UpdateRequestDataMartStatusDTO == null) {
            this.RequestDataMartID = ko.observable();
            this.DataMartID = ko.observable();
            this.NewStatus = ko.observable();
            this.Message = ko.observable();
        }
        else {
            this.RequestDataMartID = ko.observable(UpdateRequestDataMartStatusDTO.RequestDataMartID);
            this.DataMartID = ko.observable(UpdateRequestDataMartStatusDTO.DataMartID);
            this.NewStatus = ko.observable(UpdateRequestDataMartStatusDTO.NewStatus);
            this.Message = ko.observable(UpdateRequestDataMartStatusDTO.Message);
        }
    }
    toData() {
        return {
            RequestDataMartID: this.RequestDataMartID(),
            DataMartID: this.DataMartID(),
            NewStatus: this.NewStatus(),
            Message: this.Message(),
        };
    }
}
export class UpdateRequestTypeModelsViewModel extends ViewModel {
    RequestTypeID;
    DataModels;
    constructor(UpdateRequestTypeModelsDTO) {
        super();
        if (UpdateRequestTypeModelsDTO == null) {
            this.RequestTypeID = ko.observable();
            this.DataModels = ko.observableArray();
        }
        else {
            this.RequestTypeID = ko.observable(UpdateRequestTypeModelsDTO.RequestTypeID);
            this.DataModels = ko.observableArray(UpdateRequestTypeModelsDTO.DataModels == null ? null : UpdateRequestTypeModelsDTO.DataModels.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            RequestTypeID: this.RequestTypeID(),
            DataModels: this.DataModels(),
        };
    }
}
export class UpdateRequestTypeRequestViewModel extends ViewModel {
    RequestType;
    Permissions;
    Queries;
    Terms;
    NotAllowedTerms;
    Models;
    constructor(UpdateRequestTypeRequestDTO) {
        super();
        if (UpdateRequestTypeRequestDTO == null) {
            this.RequestType = new RequestTypeViewModel();
            this.Permissions = ko.observableArray();
            this.Queries = ko.observableArray();
            this.Terms = ko.observableArray();
            this.NotAllowedTerms = ko.observableArray();
            this.Models = ko.observableArray();
        }
        else {
            this.RequestType = new RequestTypeViewModel(UpdateRequestTypeRequestDTO.RequestType);
            this.Permissions = ko.observableArray(UpdateRequestTypeRequestDTO.Permissions == null ? null : UpdateRequestTypeRequestDTO.Permissions.map((item) => { return new AclRequestTypeViewModel(item); }));
            this.Queries = ko.observableArray(UpdateRequestTypeRequestDTO.Queries == null ? null : UpdateRequestTypeRequestDTO.Queries.map((item) => { return new TemplateViewModel(item); }));
            this.Terms = ko.observableArray(UpdateRequestTypeRequestDTO.Terms == null ? null : UpdateRequestTypeRequestDTO.Terms.map((item) => { return item; }));
            this.NotAllowedTerms = ko.observableArray(UpdateRequestTypeRequestDTO.NotAllowedTerms == null ? null : UpdateRequestTypeRequestDTO.NotAllowedTerms.map((item) => { return new SectionSpecificTermViewModel(item); }));
            this.Models = ko.observableArray(UpdateRequestTypeRequestDTO.Models == null ? null : UpdateRequestTypeRequestDTO.Models.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            RequestType: this.RequestType.toData(),
            Permissions: this.Permissions == null ? null : this.Permissions().map((item) => { return item.toData(); }),
            Queries: this.Queries == null ? null : this.Queries().map((item) => { return item.toData(); }),
            Terms: this.Terms(),
            NotAllowedTerms: this.NotAllowedTerms == null ? null : this.NotAllowedTerms().map((item) => { return item.toData(); }),
            Models: this.Models(),
        };
    }
}
export class UpdateRequestTypeResponseViewModel extends ViewModel {
    RequestType;
    Queries;
    constructor(UpdateRequestTypeResponseDTO) {
        super();
        if (UpdateRequestTypeResponseDTO == null) {
            this.RequestType = new RequestTypeViewModel();
            this.Queries = ko.observableArray();
        }
        else {
            this.RequestType = new RequestTypeViewModel(UpdateRequestTypeResponseDTO.RequestType);
            this.Queries = ko.observableArray(UpdateRequestTypeResponseDTO.Queries == null ? null : UpdateRequestTypeResponseDTO.Queries.map((item) => { return new TemplateViewModel(item); }));
        }
    }
    toData() {
        return {
            RequestType: this.RequestType.toData(),
            Queries: this.Queries == null ? null : this.Queries().map((item) => { return item.toData(); }),
        };
    }
}
export class UpdateRequestTypeTermsViewModel extends ViewModel {
    RequestTypeID;
    Terms;
    constructor(UpdateRequestTypeTermsDTO) {
        super();
        if (UpdateRequestTypeTermsDTO == null) {
            this.RequestTypeID = ko.observable();
            this.Terms = ko.observableArray();
        }
        else {
            this.RequestTypeID = ko.observable(UpdateRequestTypeTermsDTO.RequestTypeID);
            this.Terms = ko.observableArray(UpdateRequestTypeTermsDTO.Terms == null ? null : UpdateRequestTypeTermsDTO.Terms.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            RequestTypeID: this.RequestTypeID(),
            Terms: this.Terms(),
        };
    }
}
export class HomepageTaskRequestUserViewModel extends ViewModel {
    RequestID;
    TaskID;
    UserID;
    UserName;
    FirstName;
    LastName;
    WorkflowRoleID;
    WorkflowRole;
    constructor(HomepageTaskRequestUserDTO) {
        super();
        if (HomepageTaskRequestUserDTO == null) {
            this.RequestID = ko.observable();
            this.TaskID = ko.observable();
            this.UserID = ko.observable();
            this.UserName = ko.observable();
            this.FirstName = ko.observable();
            this.LastName = ko.observable();
            this.WorkflowRoleID = ko.observable();
            this.WorkflowRole = ko.observable();
        }
        else {
            this.RequestID = ko.observable(HomepageTaskRequestUserDTO.RequestID);
            this.TaskID = ko.observable(HomepageTaskRequestUserDTO.TaskID);
            this.UserID = ko.observable(HomepageTaskRequestUserDTO.UserID);
            this.UserName = ko.observable(HomepageTaskRequestUserDTO.UserName);
            this.FirstName = ko.observable(HomepageTaskRequestUserDTO.FirstName);
            this.LastName = ko.observable(HomepageTaskRequestUserDTO.LastName);
            this.WorkflowRoleID = ko.observable(HomepageTaskRequestUserDTO.WorkflowRoleID);
            this.WorkflowRole = ko.observable(HomepageTaskRequestUserDTO.WorkflowRole);
        }
    }
    toData() {
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
    }
}
export class HomepageTaskSummaryViewModel extends ViewModel {
    TaskID;
    TaskName;
    TaskStatus;
    TaskStatusText;
    CreatedOn;
    StartOn;
    EndOn;
    Type;
    DirectToRequest;
    Name;
    Identifier;
    RequestID;
    MSRequestID;
    RequestStatus;
    RequestStatusText;
    NewUserID;
    AssignedResources;
    constructor(HomepageTaskSummaryDTO) {
        super();
        if (HomepageTaskSummaryDTO == null) {
            this.TaskID = ko.observable();
            this.TaskName = ko.observable();
            this.TaskStatus = ko.observable();
            this.TaskStatusText = ko.observable();
            this.CreatedOn = ko.observable();
            this.StartOn = ko.observable();
            this.EndOn = ko.observable();
            this.Type = ko.observable();
            this.DirectToRequest = ko.observable();
            this.Name = ko.observable();
            this.Identifier = ko.observable();
            this.RequestID = ko.observable();
            this.MSRequestID = ko.observable();
            this.RequestStatus = ko.observable();
            this.RequestStatusText = ko.observable();
            this.NewUserID = ko.observable();
            this.AssignedResources = ko.observable();
        }
        else {
            this.TaskID = ko.observable(HomepageTaskSummaryDTO.TaskID);
            this.TaskName = ko.observable(HomepageTaskSummaryDTO.TaskName);
            this.TaskStatus = ko.observable(HomepageTaskSummaryDTO.TaskStatus);
            this.TaskStatusText = ko.observable(HomepageTaskSummaryDTO.TaskStatusText);
            this.CreatedOn = ko.observable(HomepageTaskSummaryDTO.CreatedOn);
            this.StartOn = ko.observable(HomepageTaskSummaryDTO.StartOn);
            this.EndOn = ko.observable(HomepageTaskSummaryDTO.EndOn);
            this.Type = ko.observable(HomepageTaskSummaryDTO.Type);
            this.DirectToRequest = ko.observable(HomepageTaskSummaryDTO.DirectToRequest);
            this.Name = ko.observable(HomepageTaskSummaryDTO.Name);
            this.Identifier = ko.observable(HomepageTaskSummaryDTO.Identifier);
            this.RequestID = ko.observable(HomepageTaskSummaryDTO.RequestID);
            this.MSRequestID = ko.observable(HomepageTaskSummaryDTO.MSRequestID);
            this.RequestStatus = ko.observable(HomepageTaskSummaryDTO.RequestStatus);
            this.RequestStatusText = ko.observable(HomepageTaskSummaryDTO.RequestStatusText);
            this.NewUserID = ko.observable(HomepageTaskSummaryDTO.NewUserID);
            this.AssignedResources = ko.observable(HomepageTaskSummaryDTO.AssignedResources);
        }
    }
    toData() {
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
    }
}
export class ActivityViewModel extends ViewModel {
    ID;
    Name;
    Activities;
    Description;
    ProjectID;
    DisplayOrder;
    TaskLevel;
    ParentActivityID;
    Acronym;
    Deleted;
    constructor(ActivityDTO) {
        super();
        if (ActivityDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Activities = ko.observableArray();
            this.Description = ko.observable();
            this.ProjectID = ko.observable();
            this.DisplayOrder = ko.observable();
            this.TaskLevel = ko.observable();
            this.ParentActivityID = ko.observable();
            this.Acronym = ko.observable();
            this.Deleted = ko.observable();
        }
        else {
            this.ID = ko.observable(ActivityDTO.ID);
            this.Name = ko.observable(ActivityDTO.Name);
            this.Activities = ko.observableArray(ActivityDTO.Activities == null ? null : ActivityDTO.Activities.map((item) => { return new ActivityViewModel(item); }));
            this.Description = ko.observable(ActivityDTO.Description);
            this.ProjectID = ko.observable(ActivityDTO.ProjectID);
            this.DisplayOrder = ko.observable(ActivityDTO.DisplayOrder);
            this.TaskLevel = ko.observable(ActivityDTO.TaskLevel);
            this.ParentActivityID = ko.observable(ActivityDTO.ParentActivityID);
            this.Acronym = ko.observable(ActivityDTO.Acronym);
            this.Deleted = ko.observable(ActivityDTO.Deleted);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            Activities: this.Activities == null ? null : this.Activities().map((item) => { return item.toData(); }),
            Description: this.Description(),
            ProjectID: this.ProjectID(),
            DisplayOrder: this.DisplayOrder(),
            TaskLevel: this.TaskLevel(),
            ParentActivityID: this.ParentActivityID(),
            Acronym: this.Acronym(),
            Deleted: this.Deleted(),
        };
    }
}
export class DataMartTypeViewModel extends ViewModel {
    ID;
    Name;
    constructor(DataMartTypeDTO) {
        super();
        if (DataMartTypeDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
        }
        else {
            this.ID = ko.observable(DataMartTypeDTO.ID);
            this.Name = ko.observable(DataMartTypeDTO.Name);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
        };
    }
}
export class DataMartInstalledModelViewModel extends ViewModel {
    DataMartID;
    ModelID;
    Model;
    Properties;
    constructor(DataMartInstalledModelDTO) {
        super();
        if (DataMartInstalledModelDTO == null) {
            this.DataMartID = ko.observable();
            this.ModelID = ko.observable();
            this.Model = ko.observable();
            this.Properties = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(DataMartInstalledModelDTO.DataMartID);
            this.ModelID = ko.observable(DataMartInstalledModelDTO.ModelID);
            this.Model = ko.observable(DataMartInstalledModelDTO.Model);
            this.Properties = ko.observable(DataMartInstalledModelDTO.Properties);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            ModelID: this.ModelID(),
            Model: this.Model(),
            Properties: this.Properties(),
        };
    }
}
export class DemographicViewModel extends ViewModel {
    Country;
    State;
    Town;
    Region;
    Gender;
    AgeGroup;
    Ethnicity;
    Count;
    constructor(DemographicDTO) {
        super();
        if (DemographicDTO == null) {
            this.Country = ko.observable();
            this.State = ko.observable();
            this.Town = ko.observable();
            this.Region = ko.observable();
            this.Gender = ko.observable();
            this.AgeGroup = ko.observable();
            this.Ethnicity = ko.observable();
            this.Count = ko.observable();
        }
        else {
            this.Country = ko.observable(DemographicDTO.Country);
            this.State = ko.observable(DemographicDTO.State);
            this.Town = ko.observable(DemographicDTO.Town);
            this.Region = ko.observable(DemographicDTO.Region);
            this.Gender = ko.observable(DemographicDTO.Gender);
            this.AgeGroup = ko.observable(DemographicDTO.AgeGroup);
            this.Ethnicity = ko.observable(DemographicDTO.Ethnicity);
            this.Count = ko.observable(DemographicDTO.Count);
        }
    }
    toData() {
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
    }
}
export class LookupListCategoryViewModel extends ViewModel {
    ListId;
    CategoryId;
    CategoryName;
    constructor(LookupListCategoryDTO) {
        super();
        if (LookupListCategoryDTO == null) {
            this.ListId = ko.observable();
            this.CategoryId = ko.observable();
            this.CategoryName = ko.observable();
        }
        else {
            this.ListId = ko.observable(LookupListCategoryDTO.ListId);
            this.CategoryId = ko.observable(LookupListCategoryDTO.CategoryId);
            this.CategoryName = ko.observable(LookupListCategoryDTO.CategoryName);
        }
    }
    toData() {
        return {
            ListId: this.ListId(),
            CategoryId: this.CategoryId(),
            CategoryName: this.CategoryName(),
        };
    }
}
export class LookupListDetailRequestViewModel extends ViewModel {
    Codes;
    ListID;
    constructor(LookupListDetailRequestDTO) {
        super();
        if (LookupListDetailRequestDTO == null) {
            this.Codes = ko.observableArray();
            this.ListID = ko.observable();
        }
        else {
            this.Codes = ko.observableArray(LookupListDetailRequestDTO.Codes == null ? null : LookupListDetailRequestDTO.Codes.map((item) => { return item; }));
            this.ListID = ko.observable(LookupListDetailRequestDTO.ListID);
        }
    }
    toData() {
        return {
            Codes: this.Codes == null ? null : this.Codes().map((item) => { return item; }),
            ListID: this.ListID(),
        };
    }
}
export class LookupListViewModel extends ViewModel {
    ListId;
    ListName;
    Version;
    constructor(LookupListDTO) {
        super();
        if (LookupListDTO == null) {
            this.ListId = ko.observable();
            this.ListName = ko.observable();
            this.Version = ko.observable();
        }
        else {
            this.ListId = ko.observable(LookupListDTO.ListId);
            this.ListName = ko.observable(LookupListDTO.ListName);
            this.Version = ko.observable(LookupListDTO.Version);
        }
    }
    toData() {
        return {
            ListId: this.ListId(),
            ListName: this.ListName(),
            Version: this.Version(),
        };
    }
}
export class LookupListValueViewModel extends ViewModel {
    ListId;
    CategoryId;
    ItemName;
    ItemCode;
    ItemCodeWithNoPeriod;
    ExpireDate;
    ID;
    constructor(LookupListValueDTO) {
        super();
        if (LookupListValueDTO == null) {
            this.ListId = ko.observable();
            this.CategoryId = ko.observable();
            this.ItemName = ko.observable();
            this.ItemCode = ko.observable();
            this.ItemCodeWithNoPeriod = ko.observable();
            this.ExpireDate = ko.observable();
            this.ID = ko.observable();
        }
        else {
            this.ListId = ko.observable(LookupListValueDTO.ListId);
            this.CategoryId = ko.observable(LookupListValueDTO.CategoryId);
            this.ItemName = ko.observable(LookupListValueDTO.ItemName);
            this.ItemCode = ko.observable(LookupListValueDTO.ItemCode);
            this.ItemCodeWithNoPeriod = ko.observable(LookupListValueDTO.ItemCodeWithNoPeriod);
            this.ExpireDate = ko.observable(LookupListValueDTO.ExpireDate);
            this.ID = ko.observable(LookupListValueDTO.ID);
        }
    }
    toData() {
        return {
            ListId: this.ListId(),
            CategoryId: this.CategoryId(),
            ItemName: this.ItemName(),
            ItemCode: this.ItemCode(),
            ItemCodeWithNoPeriod: this.ItemCodeWithNoPeriod(),
            ExpireDate: this.ExpireDate(),
            ID: this.ID(),
        };
    }
}
export class ProjectDataMartViewModel extends ViewModel {
    ProjectID;
    Project;
    ProjectAcronym;
    DataMartID;
    DataMart;
    Organization;
    constructor(ProjectDataMartDTO) {
        super();
        if (ProjectDataMartDTO == null) {
            this.ProjectID = ko.observable();
            this.Project = ko.observable();
            this.ProjectAcronym = ko.observable();
            this.DataMartID = ko.observable();
            this.DataMart = ko.observable();
            this.Organization = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectDataMartDTO.ProjectID);
            this.Project = ko.observable(ProjectDataMartDTO.Project);
            this.ProjectAcronym = ko.observable(ProjectDataMartDTO.ProjectAcronym);
            this.DataMartID = ko.observable(ProjectDataMartDTO.DataMartID);
            this.DataMart = ko.observable(ProjectDataMartDTO.DataMart);
            this.Organization = ko.observable(ProjectDataMartDTO.Organization);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            Project: this.Project(),
            ProjectAcronym: this.ProjectAcronym(),
            DataMartID: this.DataMartID(),
            DataMart: this.DataMart(),
            Organization: this.Organization(),
        };
    }
}
export class RegistryItemDefinitionViewModel extends ViewModel {
    ID;
    Category;
    Title;
    constructor(RegistryItemDefinitionDTO) {
        super();
        if (RegistryItemDefinitionDTO == null) {
            this.ID = ko.observable();
            this.Category = ko.observable();
            this.Title = ko.observable();
        }
        else {
            this.ID = ko.observable(RegistryItemDefinitionDTO.ID);
            this.Category = ko.observable(RegistryItemDefinitionDTO.Category);
            this.Title = ko.observable(RegistryItemDefinitionDTO.Title);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Category: this.Category(),
            Title: this.Title(),
        };
    }
}
export class UpdateRegistryItemsViewModel extends ViewModel {
    constructor(UpdateRegistryItemsDTO) {
        super();
        if (UpdateRegistryItemsDTO == null) {
        }
        else {
        }
    }
    toData() {
        return {};
    }
}
export class WorkplanTypeViewModel extends ViewModel {
    ID;
    WorkplanTypeID;
    Name;
    NetworkID;
    Acronym;
    constructor(WorkplanTypeDTO) {
        super();
        if (WorkplanTypeDTO == null) {
            this.ID = ko.observable();
            this.WorkplanTypeID = ko.observable();
            this.Name = ko.observable();
            this.NetworkID = ko.observable();
            this.Acronym = ko.observable();
        }
        else {
            this.ID = ko.observable(WorkplanTypeDTO.ID);
            this.WorkplanTypeID = ko.observable(WorkplanTypeDTO.WorkplanTypeID);
            this.Name = ko.observable(WorkplanTypeDTO.Name);
            this.NetworkID = ko.observable(WorkplanTypeDTO.NetworkID);
            this.Acronym = ko.observable(WorkplanTypeDTO.Acronym);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            WorkplanTypeID: this.WorkplanTypeID(),
            Name: this.Name(),
            NetworkID: this.NetworkID(),
            Acronym: this.Acronym(),
        };
    }
}
export class RequesterCenterViewModel extends ViewModel {
    ID;
    RequesterCenterID;
    Name;
    NetworkID;
    constructor(RequesterCenterDTO) {
        super();
        if (RequesterCenterDTO == null) {
            this.ID = ko.observable();
            this.RequesterCenterID = ko.observable();
            this.Name = ko.observable();
            this.NetworkID = ko.observable();
        }
        else {
            this.ID = ko.observable(RequesterCenterDTO.ID);
            this.RequesterCenterID = ko.observable(RequesterCenterDTO.RequesterCenterID);
            this.Name = ko.observable(RequesterCenterDTO.Name);
            this.NetworkID = ko.observable(RequesterCenterDTO.NetworkID);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            RequesterCenterID: this.RequesterCenterID(),
            Name: this.Name(),
            NetworkID: this.NetworkID(),
        };
    }
}
export class QueryTypeViewModel extends ViewModel {
    ID;
    Name;
    Description;
    constructor(QueryTypeDTO) {
        super();
        if (QueryTypeDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
        }
        else {
            this.ID = ko.observable(QueryTypeDTO.ID);
            this.Name = ko.observable(QueryTypeDTO.Name);
            this.Description = ko.observable(QueryTypeDTO.Description);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            Description: this.Description(),
        };
    }
}
export class SecurityTupleViewModel extends ViewModel {
    ID1;
    ID2;
    ID3;
    ID4;
    SubjectID;
    PrivilegeID;
    ViaMembership;
    DeniedEntries;
    ExplicitDeniedEntries;
    ExplicitAllowedEntries;
    ChangedOn;
    constructor(SecurityTupleDTO) {
        super();
        if (SecurityTupleDTO == null) {
            this.ID1 = ko.observable();
            this.ID2 = ko.observable();
            this.ID3 = ko.observable();
            this.ID4 = ko.observable();
            this.SubjectID = ko.observable();
            this.PrivilegeID = ko.observable();
            this.ViaMembership = ko.observable();
            this.DeniedEntries = ko.observable();
            this.ExplicitDeniedEntries = ko.observable();
            this.ExplicitAllowedEntries = ko.observable();
            this.ChangedOn = ko.observable();
        }
        else {
            this.ID1 = ko.observable(SecurityTupleDTO.ID1);
            this.ID2 = ko.observable(SecurityTupleDTO.ID2);
            this.ID3 = ko.observable(SecurityTupleDTO.ID3);
            this.ID4 = ko.observable(SecurityTupleDTO.ID4);
            this.SubjectID = ko.observable(SecurityTupleDTO.SubjectID);
            this.PrivilegeID = ko.observable(SecurityTupleDTO.PrivilegeID);
            this.ViaMembership = ko.observable(SecurityTupleDTO.ViaMembership);
            this.DeniedEntries = ko.observable(SecurityTupleDTO.DeniedEntries);
            this.ExplicitDeniedEntries = ko.observable(SecurityTupleDTO.ExplicitDeniedEntries);
            this.ExplicitAllowedEntries = ko.observable(SecurityTupleDTO.ExplicitAllowedEntries);
            this.ChangedOn = ko.observable(SecurityTupleDTO.ChangedOn);
        }
    }
    toData() {
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
    }
}
export class UpdateUserSecurityGroupsViewModel extends ViewModel {
    UserID;
    Groups;
    constructor(UpdateUserSecurityGroupsDTO) {
        super();
        if (UpdateUserSecurityGroupsDTO == null) {
            this.UserID = ko.observable();
            this.Groups = ko.observableArray();
        }
        else {
            this.UserID = ko.observable(UpdateUserSecurityGroupsDTO.UserID);
            this.Groups = ko.observableArray(UpdateUserSecurityGroupsDTO.Groups == null ? null : UpdateUserSecurityGroupsDTO.Groups.map((item) => { return new SecurityGroupViewModel(item); }));
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            Groups: this.Groups == null ? null : this.Groups().map((item) => { return item.toData(); }),
        };
    }
}
export class DesignViewModel extends ViewModel {
    Locked;
    constructor(DesignDTO) {
        super();
        if (DesignDTO == null) {
            this.Locked = ko.observable();
        }
        else {
            this.Locked = ko.observable(DesignDTO.Locked);
        }
    }
    toData() {
        return {
            Locked: this.Locked(),
        };
    }
}
export class CodeSelectorValueViewModel extends ViewModel {
    Code;
    Name;
    ExpireDate;
    constructor(CodeSelectorValueDTO) {
        super();
        if (CodeSelectorValueDTO == null) {
            this.Code = ko.observable();
            this.Name = ko.observable();
            this.ExpireDate = ko.observable();
        }
        else {
            this.Code = ko.observable(CodeSelectorValueDTO.Code);
            this.Name = ko.observable(CodeSelectorValueDTO.Name);
            this.ExpireDate = ko.observable(CodeSelectorValueDTO.ExpireDate);
        }
    }
    toData() {
        return {
            Code: this.Code(),
            Name: this.Name(),
            ExpireDate: this.ExpireDate(),
        };
    }
}
export class ThemeViewModel extends ViewModel {
    Title;
    Terms;
    Info;
    Resources;
    Footer;
    LogoImage;
    SystemUserConfirmationTitle;
    SystemUserConfirmationContent;
    ContactUsHref;
    constructor(ThemeDTO) {
        super();
        if (ThemeDTO == null) {
            this.Title = ko.observable();
            this.Terms = ko.observable();
            this.Info = ko.observable();
            this.Resources = ko.observable();
            this.Footer = ko.observable();
            this.LogoImage = ko.observable();
            this.SystemUserConfirmationTitle = ko.observable();
            this.SystemUserConfirmationContent = ko.observable();
            this.ContactUsHref = ko.observable();
        }
        else {
            this.Title = ko.observable(ThemeDTO.Title);
            this.Terms = ko.observable(ThemeDTO.Terms);
            this.Info = ko.observable(ThemeDTO.Info);
            this.Resources = ko.observable(ThemeDTO.Resources);
            this.Footer = ko.observable(ThemeDTO.Footer);
            this.LogoImage = ko.observable(ThemeDTO.LogoImage);
            this.SystemUserConfirmationTitle = ko.observable(ThemeDTO.SystemUserConfirmationTitle);
            this.SystemUserConfirmationContent = ko.observable(ThemeDTO.SystemUserConfirmationContent);
            this.ContactUsHref = ko.observable(ThemeDTO.ContactUsHref);
        }
    }
    toData() {
        return {
            Title: this.Title(),
            Terms: this.Terms(),
            Info: this.Info(),
            Resources: this.Resources(),
            Footer: this.Footer(),
            LogoImage: this.LogoImage(),
            SystemUserConfirmationTitle: this.SystemUserConfirmationTitle(),
            SystemUserConfirmationContent: this.SystemUserConfirmationContent(),
            ContactUsHref: this.ContactUsHref(),
        };
    }
}
export class AssignedUserNotificationViewModel extends ViewModel {
    Event;
    EventID;
    Level;
    Description;
    constructor(AssignedUserNotificationDTO) {
        super();
        if (AssignedUserNotificationDTO == null) {
            this.Event = ko.observable();
            this.EventID = ko.observable();
            this.Level = ko.observable();
            this.Description = ko.observable();
        }
        else {
            this.Event = ko.observable(AssignedUserNotificationDTO.Event);
            this.EventID = ko.observable(AssignedUserNotificationDTO.EventID);
            this.Level = ko.observable(AssignedUserNotificationDTO.Level);
            this.Description = ko.observable(AssignedUserNotificationDTO.Description);
        }
    }
    toData() {
        return {
            Event: this.Event(),
            EventID: this.EventID(),
            Level: this.Level(),
            Description: this.Description(),
        };
    }
}
export class MetadataEditPermissionsSummaryViewModel extends ViewModel {
    CanEditRequestMetadata;
    EditableDataMarts;
    constructor(MetadataEditPermissionsSummaryDTO) {
        super();
        if (MetadataEditPermissionsSummaryDTO == null) {
            this.CanEditRequestMetadata = ko.observable();
            this.EditableDataMarts = ko.observableArray();
        }
        else {
            this.CanEditRequestMetadata = ko.observable(MetadataEditPermissionsSummaryDTO.CanEditRequestMetadata);
            this.EditableDataMarts = ko.observableArray(MetadataEditPermissionsSummaryDTO.EditableDataMarts == null ? null : MetadataEditPermissionsSummaryDTO.EditableDataMarts.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            CanEditRequestMetadata: this.CanEditRequestMetadata(),
            EditableDataMarts: this.EditableDataMarts(),
        };
    }
}
export class NotificationViewModel extends ViewModel {
    Timestamp;
    Event;
    Message;
    constructor(NotificationDTO) {
        super();
        if (NotificationDTO == null) {
            this.Timestamp = ko.observable();
            this.Event = ko.observable();
            this.Message = ko.observable();
        }
        else {
            this.Timestamp = ko.observable(NotificationDTO.Timestamp);
            this.Event = ko.observable(NotificationDTO.Event);
            this.Message = ko.observable(NotificationDTO.Message);
        }
    }
    toData() {
        return {
            Timestamp: this.Timestamp(),
            Event: this.Event(),
            Message: this.Message(),
        };
    }
}
export class ForgotPasswordViewModel extends ViewModel {
    UserName;
    Email;
    constructor(ForgotPasswordDTO) {
        super();
        if (ForgotPasswordDTO == null) {
            this.UserName = ko.observable();
            this.Email = ko.observable();
        }
        else {
            this.UserName = ko.observable(ForgotPasswordDTO.UserName);
            this.Email = ko.observable(ForgotPasswordDTO.Email);
        }
    }
    toData() {
        return {
            UserName: this.UserName(),
            Email: this.Email(),
        };
    }
}
export class LoginViewModel extends ViewModel {
    UserName;
    Password;
    RememberMe;
    IPAddress;
    Enviorment;
    constructor(LoginDTO) {
        super();
        if (LoginDTO == null) {
            this.UserName = ko.observable();
            this.Password = ko.observable();
            this.RememberMe = ko.observable();
            this.IPAddress = ko.observable();
            this.Enviorment = ko.observable();
        }
        else {
            this.UserName = ko.observable(LoginDTO.UserName);
            this.Password = ko.observable(LoginDTO.Password);
            this.RememberMe = ko.observable(LoginDTO.RememberMe);
            this.IPAddress = ko.observable(LoginDTO.IPAddress);
            this.Enviorment = ko.observable(LoginDTO.Enviorment);
        }
    }
    toData() {
        return {
            UserName: this.UserName(),
            Password: this.Password(),
            RememberMe: this.RememberMe(),
            IPAddress: this.IPAddress(),
            Enviorment: this.Enviorment(),
        };
    }
}
export class MenuItemViewModel extends ViewModel {
    text;
    url;
    encoded;
    content;
    items;
    constructor(MenuItemDTO) {
        super();
        if (MenuItemDTO == null) {
            this.text = ko.observable();
            this.url = ko.observable();
            this.encoded = ko.observable();
            this.content = ko.observable();
            this.items = ko.observableArray();
        }
        else {
            this.text = ko.observable(MenuItemDTO.text);
            this.url = ko.observable(MenuItemDTO.url);
            this.encoded = ko.observable(MenuItemDTO.encoded);
            this.content = ko.observable(MenuItemDTO.content);
            this.items = ko.observableArray(MenuItemDTO.items == null ? null : MenuItemDTO.items.map((item) => { return new MenuItemViewModel(item); }));
        }
    }
    toData() {
        return {
            text: this.text(),
            url: this.url(),
            encoded: this.encoded(),
            content: this.content(),
            items: this.items == null ? null : this.items().map((item) => { return item.toData(); }),
        };
    }
}
export class ObserverViewModel extends ViewModel {
    ID;
    DisplayName;
    DisplayNameWithType;
    ObserverType;
    constructor(ObserverDTO) {
        super();
        if (ObserverDTO == null) {
            this.ID = ko.observable();
            this.DisplayName = ko.observable();
            this.DisplayNameWithType = ko.observable();
            this.ObserverType = ko.observable();
        }
        else {
            this.ID = ko.observable(ObserverDTO.ID);
            this.DisplayName = ko.observable(ObserverDTO.DisplayName);
            this.DisplayNameWithType = ko.observable(ObserverDTO.DisplayNameWithType);
            this.ObserverType = ko.observable(ObserverDTO.ObserverType);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            DisplayName: this.DisplayName(),
            DisplayNameWithType: this.DisplayNameWithType(),
            ObserverType: this.ObserverType(),
        };
    }
}
export class ObserverEventViewModel extends ViewModel {
    ID;
    Name;
    constructor(ObserverEventDTO) {
        super();
        if (ObserverEventDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
        }
        else {
            this.ID = ko.observable(ObserverEventDTO.ID);
            this.Name = ko.observable(ObserverEventDTO.Name);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
        };
    }
}
export class RestorePasswordViewModel extends ViewModel {
    PasswordRestoreToken;
    Password;
    constructor(RestorePasswordDTO) {
        super();
        if (RestorePasswordDTO == null) {
            this.PasswordRestoreToken = ko.observable();
            this.Password = ko.observable();
        }
        else {
            this.PasswordRestoreToken = ko.observable(RestorePasswordDTO.PasswordRestoreToken);
            this.Password = ko.observable(RestorePasswordDTO.Password);
        }
    }
    toData() {
        return {
            PasswordRestoreToken: this.PasswordRestoreToken(),
            Password: this.Password(),
        };
    }
}
export class TreeItemViewModel extends ViewModel {
    ID;
    Name;
    Path;
    Type;
    SubItems;
    HasChildren;
    constructor(TreeItemDTO) {
        super();
        if (TreeItemDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Path = ko.observable();
            this.Type = ko.observable();
            this.SubItems = ko.observableArray();
            this.HasChildren = ko.observable();
        }
        else {
            this.ID = ko.observable(TreeItemDTO.ID);
            this.Name = ko.observable(TreeItemDTO.Name);
            this.Path = ko.observable(TreeItemDTO.Path);
            this.Type = ko.observable(TreeItemDTO.Type);
            this.SubItems = ko.observableArray(TreeItemDTO.SubItems == null ? null : TreeItemDTO.SubItems.map((item) => { return new TreeItemViewModel(item); }));
            this.HasChildren = ko.observable(TreeItemDTO.HasChildren);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            Path: this.Path(),
            Type: this.Type(),
            SubItems: this.SubItems == null ? null : this.SubItems().map((item) => { return item.toData(); }),
            HasChildren: this.HasChildren(),
        };
    }
}
export class UpdateUserPasswordViewModel extends ViewModel {
    UserID;
    Password;
    constructor(UpdateUserPasswordDTO) {
        super();
        if (UpdateUserPasswordDTO == null) {
            this.UserID = ko.observable();
            this.Password = ko.observable();
        }
        else {
            this.UserID = ko.observable(UpdateUserPasswordDTO.UserID);
            this.Password = ko.observable(UpdateUserPasswordDTO.Password);
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            Password: this.Password(),
        };
    }
}
export class UserAuthenticationViewModel extends ViewModel {
    ID;
    UserID;
    Success;
    Description;
    IPAddress;
    Environment;
    Source;
    Details;
    DMCVersion;
    DateTime;
    constructor(UserAuthenticationDTO) {
        super();
        if (UserAuthenticationDTO == null) {
            this.ID = ko.observable();
            this.UserID = ko.observable();
            this.Success = ko.observable();
            this.Description = ko.observable();
            this.IPAddress = ko.observable();
            this.Environment = ko.observable();
            this.Source = ko.observable();
            this.Details = ko.observable();
            this.DMCVersion = ko.observable();
            this.DateTime = ko.observable();
        }
        else {
            this.ID = ko.observable(UserAuthenticationDTO.ID);
            this.UserID = ko.observable(UserAuthenticationDTO.UserID);
            this.Success = ko.observable(UserAuthenticationDTO.Success);
            this.Description = ko.observable(UserAuthenticationDTO.Description);
            this.IPAddress = ko.observable(UserAuthenticationDTO.IPAddress);
            this.Environment = ko.observable(UserAuthenticationDTO.Environment);
            this.Source = ko.observable(UserAuthenticationDTO.Source);
            this.Details = ko.observable(UserAuthenticationDTO.Details);
            this.DMCVersion = ko.observable(UserAuthenticationDTO.DMCVersion);
            this.DateTime = ko.observable(UserAuthenticationDTO.DateTime);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            UserID: this.UserID(),
            Success: this.Success(),
            Description: this.Description(),
            IPAddress: this.IPAddress(),
            Environment: this.Environment(),
            Source: this.Source(),
            Details: this.Details(),
            DMCVersion: this.DMCVersion(),
            DateTime: this.DateTime(),
        };
    }
}
export class UserRegistrationViewModel extends ViewModel {
    UserName;
    Password;
    Title;
    FirstName;
    LastName;
    MiddleName;
    Phone;
    Fax;
    Email;
    Active;
    SignedUpOn;
    OrganizationRequested;
    RoleRequested;
    constructor(UserRegistrationDTO) {
        super();
        if (UserRegistrationDTO == null) {
            this.UserName = ko.observable();
            this.Password = ko.observable();
            this.Title = ko.observable();
            this.FirstName = ko.observable();
            this.LastName = ko.observable();
            this.MiddleName = ko.observable();
            this.Phone = ko.observable();
            this.Fax = ko.observable();
            this.Email = ko.observable();
            this.Active = ko.observable();
            this.SignedUpOn = ko.observable();
            this.OrganizationRequested = ko.observable();
            this.RoleRequested = ko.observable();
        }
        else {
            this.UserName = ko.observable(UserRegistrationDTO.UserName);
            this.Password = ko.observable(UserRegistrationDTO.Password);
            this.Title = ko.observable(UserRegistrationDTO.Title);
            this.FirstName = ko.observable(UserRegistrationDTO.FirstName);
            this.LastName = ko.observable(UserRegistrationDTO.LastName);
            this.MiddleName = ko.observable(UserRegistrationDTO.MiddleName);
            this.Phone = ko.observable(UserRegistrationDTO.Phone);
            this.Fax = ko.observable(UserRegistrationDTO.Fax);
            this.Email = ko.observable(UserRegistrationDTO.Email);
            this.Active = ko.observable(UserRegistrationDTO.Active);
            this.SignedUpOn = ko.observable(UserRegistrationDTO.SignedUpOn);
            this.OrganizationRequested = ko.observable(UserRegistrationDTO.OrganizationRequested);
            this.RoleRequested = ko.observable(UserRegistrationDTO.RoleRequested);
        }
    }
    toData() {
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
    }
}
export class DataMartRegistrationResultViewModel extends ViewModel {
    DataMarts;
    DataMartModels;
    Users;
    ResearchOrganization;
    ProviderOrganization;
    constructor(DataMartRegistrationResultDTO) {
        super();
        if (DataMartRegistrationResultDTO == null) {
            this.DataMarts = ko.observableArray();
            this.DataMartModels = ko.observableArray();
            this.Users = ko.observableArray();
            this.ResearchOrganization = new OrganizationViewModel();
            this.ProviderOrganization = new OrganizationViewModel();
        }
        else {
            this.DataMarts = ko.observableArray(DataMartRegistrationResultDTO.DataMarts == null ? null : DataMartRegistrationResultDTO.DataMarts.map((item) => { return new DataMartViewModel(item); }));
            this.DataMartModels = ko.observableArray(DataMartRegistrationResultDTO.DataMartModels == null ? null : DataMartRegistrationResultDTO.DataMartModels.map((item) => { return new DataMartInstalledModelViewModel(item); }));
            this.Users = ko.observableArray(DataMartRegistrationResultDTO.Users == null ? null : DataMartRegistrationResultDTO.Users.map((item) => { return new UserWithSecurityDetailsViewModel(item); }));
            this.ResearchOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ResearchOrganization);
            this.ProviderOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ProviderOrganization);
        }
    }
    toData() {
        return {
            DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => { return item.toData(); }),
            DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map((item) => { return item.toData(); }),
            Users: this.Users == null ? null : this.Users().map((item) => { return item.toData(); }),
            ResearchOrganization: this.ResearchOrganization.toData(),
            ProviderOrganization: this.ProviderOrganization.toData(),
        };
    }
}
export class GetChangeRequestViewModel extends ViewModel {
    LastChecked;
    ProviderIDs;
    constructor(GetChangeRequestDTO) {
        super();
        if (GetChangeRequestDTO == null) {
            this.LastChecked = ko.observable();
            this.ProviderIDs = ko.observableArray();
        }
        else {
            this.LastChecked = ko.observable(GetChangeRequestDTO.LastChecked);
            this.ProviderIDs = ko.observableArray(GetChangeRequestDTO.ProviderIDs == null ? null : GetChangeRequestDTO.ProviderIDs.map((item) => { return item; }));
        }
    }
    toData() {
        return {
            LastChecked: this.LastChecked(),
            ProviderIDs: this.ProviderIDs(),
        };
    }
}
export class RegisterDataMartViewModel extends ViewModel {
    Password;
    Token;
    constructor(RegisterDataMartDTO) {
        super();
        if (RegisterDataMartDTO == null) {
            this.Password = ko.observable();
            this.Token = ko.observable();
        }
        else {
            this.Password = ko.observable(RegisterDataMartDTO.Password);
            this.Token = ko.observable(RegisterDataMartDTO.Token);
        }
    }
    toData() {
        return {
            Password: this.Password(),
            Token: this.Token(),
        };
    }
}
export class RequestDocumentViewModel extends ViewModel {
    ID;
    Name;
    FileName;
    MimeType;
    Viewable;
    ItemID;
    constructor(RequestDocumentDTO) {
        super();
        if (RequestDocumentDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.FileName = ko.observable();
            this.MimeType = ko.observable();
            this.Viewable = ko.observable();
            this.ItemID = ko.observable();
        }
        else {
            this.ID = ko.observable(RequestDocumentDTO.ID);
            this.Name = ko.observable(RequestDocumentDTO.Name);
            this.FileName = ko.observable(RequestDocumentDTO.FileName);
            this.MimeType = ko.observable(RequestDocumentDTO.MimeType);
            this.Viewable = ko.observable(RequestDocumentDTO.Viewable);
            this.ItemID = ko.observable(RequestDocumentDTO.ItemID);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            FileName: this.FileName(),
            MimeType: this.MimeType(),
            Viewable: this.Viewable(),
            ItemID: this.ItemID(),
        };
    }
}
export class UpdateResponseStatusRequestViewModel extends ViewModel {
    RequestID;
    ResponseID;
    DataMartID;
    ProjectID;
    OrganizationID;
    UserID;
    StatusID;
    Message;
    RejectReason;
    HoldReason;
    RequestTypeID;
    RequestTypeName;
    constructor(UpdateResponseStatusRequestDTO) {
        super();
        if (UpdateResponseStatusRequestDTO == null) {
            this.RequestID = ko.observable();
            this.ResponseID = ko.observable();
            this.DataMartID = ko.observable();
            this.ProjectID = ko.observable();
            this.OrganizationID = ko.observable();
            this.UserID = ko.observable();
            this.StatusID = ko.observable();
            this.Message = ko.observable();
            this.RejectReason = ko.observable();
            this.HoldReason = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestTypeName = ko.observable();
        }
        else {
            this.RequestID = ko.observable(UpdateResponseStatusRequestDTO.RequestID);
            this.ResponseID = ko.observable(UpdateResponseStatusRequestDTO.ResponseID);
            this.DataMartID = ko.observable(UpdateResponseStatusRequestDTO.DataMartID);
            this.ProjectID = ko.observable(UpdateResponseStatusRequestDTO.ProjectID);
            this.OrganizationID = ko.observable(UpdateResponseStatusRequestDTO.OrganizationID);
            this.UserID = ko.observable(UpdateResponseStatusRequestDTO.UserID);
            this.StatusID = ko.observable(UpdateResponseStatusRequestDTO.StatusID);
            this.Message = ko.observable(UpdateResponseStatusRequestDTO.Message);
            this.RejectReason = ko.observable(UpdateResponseStatusRequestDTO.RejectReason);
            this.HoldReason = ko.observable(UpdateResponseStatusRequestDTO.HoldReason);
            this.RequestTypeID = ko.observable(UpdateResponseStatusRequestDTO.RequestTypeID);
            this.RequestTypeName = ko.observable(UpdateResponseStatusRequestDTO.RequestTypeName);
        }
    }
    toData() {
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
    }
}
export class WbdChangeSetViewModel extends ViewModel {
    Requests;
    Projects;
    DataMarts;
    DataMartModels;
    RequestDataMarts;
    ProjectDataMarts;
    Organizations;
    Documents;
    Users;
    Responses;
    SecurityGroups;
    RequestResponseSecurityACLs;
    DataMartSecurityACLs;
    ManageWbdACLs;
    constructor(WbdChangeSetDTO) {
        super();
        if (WbdChangeSetDTO == null) {
            this.Requests = ko.observableArray();
            this.Projects = ko.observableArray();
            this.DataMarts = ko.observableArray();
            this.DataMartModels = ko.observableArray();
            this.RequestDataMarts = ko.observableArray();
            this.ProjectDataMarts = ko.observableArray();
            this.Organizations = ko.observableArray();
            this.Documents = ko.observableArray();
            this.Users = ko.observableArray();
            this.Responses = ko.observableArray();
            this.SecurityGroups = ko.observableArray();
            this.RequestResponseSecurityACLs = ko.observableArray();
            this.DataMartSecurityACLs = ko.observableArray();
            this.ManageWbdACLs = ko.observableArray();
        }
        else {
            this.Requests = ko.observableArray(WbdChangeSetDTO.Requests == null ? null : WbdChangeSetDTO.Requests.map((item) => { return new RequestViewModel(item); }));
            this.Projects = ko.observableArray(WbdChangeSetDTO.Projects == null ? null : WbdChangeSetDTO.Projects.map((item) => { return new ProjectViewModel(item); }));
            this.DataMarts = ko.observableArray(WbdChangeSetDTO.DataMarts == null ? null : WbdChangeSetDTO.DataMarts.map((item) => { return new DataMartViewModel(item); }));
            this.DataMartModels = ko.observableArray(WbdChangeSetDTO.DataMartModels == null ? null : WbdChangeSetDTO.DataMartModels.map((item) => { return new DataMartInstalledModelViewModel(item); }));
            this.RequestDataMarts = ko.observableArray(WbdChangeSetDTO.RequestDataMarts == null ? null : WbdChangeSetDTO.RequestDataMarts.map((item) => { return new RequestDataMartViewModel(item); }));
            this.ProjectDataMarts = ko.observableArray(WbdChangeSetDTO.ProjectDataMarts == null ? null : WbdChangeSetDTO.ProjectDataMarts.map((item) => { return new ProjectDataMartViewModel(item); }));
            this.Organizations = ko.observableArray(WbdChangeSetDTO.Organizations == null ? null : WbdChangeSetDTO.Organizations.map((item) => { return new OrganizationViewModel(item); }));
            this.Documents = ko.observableArray(WbdChangeSetDTO.Documents == null ? null : WbdChangeSetDTO.Documents.map((item) => { return new RequestDocumentViewModel(item); }));
            this.Users = ko.observableArray(WbdChangeSetDTO.Users == null ? null : WbdChangeSetDTO.Users.map((item) => { return new UserWithSecurityDetailsViewModel(item); }));
            this.Responses = ko.observableArray(WbdChangeSetDTO.Responses == null ? null : WbdChangeSetDTO.Responses.map((item) => { return new ResponseDetailViewModel(item); }));
            this.SecurityGroups = ko.observableArray(WbdChangeSetDTO.SecurityGroups == null ? null : WbdChangeSetDTO.SecurityGroups.map((item) => { return new SecurityGroupWithUsersViewModel(item); }));
            this.RequestResponseSecurityACLs = ko.observableArray(WbdChangeSetDTO.RequestResponseSecurityACLs == null ? null : WbdChangeSetDTO.RequestResponseSecurityACLs.map((item) => { return new SecurityTupleViewModel(item); }));
            this.DataMartSecurityACLs = ko.observableArray(WbdChangeSetDTO.DataMartSecurityACLs == null ? null : WbdChangeSetDTO.DataMartSecurityACLs.map((item) => { return new SecurityTupleViewModel(item); }));
            this.ManageWbdACLs = ko.observableArray(WbdChangeSetDTO.ManageWbdACLs == null ? null : WbdChangeSetDTO.ManageWbdACLs.map((item) => { return new SecurityTupleViewModel(item); }));
        }
    }
    toData() {
        return {
            Requests: this.Requests == null ? null : this.Requests().map((item) => { return item.toData(); }),
            Projects: this.Projects == null ? null : this.Projects().map((item) => { return item.toData(); }),
            DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => { return item.toData(); }),
            DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map((item) => { return item.toData(); }),
            RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map((item) => { return item.toData(); }),
            ProjectDataMarts: this.ProjectDataMarts == null ? null : this.ProjectDataMarts().map((item) => { return item.toData(); }),
            Organizations: this.Organizations == null ? null : this.Organizations().map((item) => { return item.toData(); }),
            Documents: this.Documents == null ? null : this.Documents().map((item) => { return item.toData(); }),
            Users: this.Users == null ? null : this.Users().map((item) => { return item.toData(); }),
            Responses: this.Responses == null ? null : this.Responses().map((item) => { return item.toData(); }),
            SecurityGroups: this.SecurityGroups == null ? null : this.SecurityGroups().map((item) => { return item.toData(); }),
            RequestResponseSecurityACLs: this.RequestResponseSecurityACLs == null ? null : this.RequestResponseSecurityACLs().map((item) => { return item.toData(); }),
            DataMartSecurityACLs: this.DataMartSecurityACLs == null ? null : this.DataMartSecurityACLs().map((item) => { return item.toData(); }),
            ManageWbdACLs: this.ManageWbdACLs == null ? null : this.ManageWbdACLs().map((item) => { return item.toData(); }),
        };
    }
}
export class CommonResponseDetailViewModel extends ViewModel {
    RequestDataMarts;
    Responses;
    Documents;
    CanViewPendingApprovalResponses;
    ExportForFileDistribution;
    constructor(CommonResponseDetailDTO) {
        super();
        if (CommonResponseDetailDTO == null) {
            this.RequestDataMarts = ko.observableArray();
            this.Responses = ko.observableArray();
            this.Documents = ko.observableArray();
            this.CanViewPendingApprovalResponses = ko.observable();
            this.ExportForFileDistribution = ko.observable();
        }
        else {
            this.RequestDataMarts = ko.observableArray(CommonResponseDetailDTO.RequestDataMarts == null ? null : CommonResponseDetailDTO.RequestDataMarts.map((item) => { return new RequestDataMartViewModel(item); }));
            this.Responses = ko.observableArray(CommonResponseDetailDTO.Responses == null ? null : CommonResponseDetailDTO.Responses.map((item) => { return new ResponseViewModel(item); }));
            this.Documents = ko.observableArray(CommonResponseDetailDTO.Documents == null ? null : CommonResponseDetailDTO.Documents.map((item) => { return new ExtendedDocumentViewModel(item); }));
            this.CanViewPendingApprovalResponses = ko.observable(CommonResponseDetailDTO.CanViewPendingApprovalResponses);
            this.ExportForFileDistribution = ko.observable(CommonResponseDetailDTO.ExportForFileDistribution);
        }
    }
    toData() {
        return {
            RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map((item) => { return item.toData(); }),
            Responses: this.Responses == null ? null : this.Responses().map((item) => { return item.toData(); }),
            Documents: this.Documents == null ? null : this.Documents().map((item) => { return item.toData(); }),
            CanViewPendingApprovalResponses: this.CanViewPendingApprovalResponses(),
            ExportForFileDistribution: this.ExportForFileDistribution(),
        };
    }
}
export class PrepareSpecificationViewModel extends ViewModel {
    constructor(PrepareSpecificationDTO) {
        super();
        if (PrepareSpecificationDTO == null) {
        }
        else {
        }
    }
    toData() {
        return {};
    }
}
export class RequestFormViewModel extends ViewModel {
    RequestDueDate;
    ContactInfo;
    RequestingTeam;
    FDAReview;
    FDADivisionNA;
    FDADivisionDAAAP;
    FDADivisionDBRUP;
    FDADivisionDCARP;
    FDADivisionDDDP;
    FDADivisionDGIEP;
    FDADivisionDMIP;
    FDADivisionDMEP;
    FDADivisionDNP;
    FDADivisionDDP;
    FDADivisionDPARP;
    FDADivisionOther;
    QueryLevel;
    AdjustmentMethod;
    CohortID;
    StudyObjectives;
    RequestStartDate;
    RequestEndDate;
    AgeGroups;
    CoverageTypes;
    EnrollmentGap;
    EnrollmentExposure;
    DefineExposures;
    WashoutPeirod;
    OtherExposures;
    OneOrManyExposures;
    AdditionalInclusion;
    AdditionalInclusionEvaluation;
    AdditionalExclusion;
    AdditionalExclusionEvaluation;
    VaryWashoutPeirod;
    VaryExposures;
    DefineExposures1;
    DefineExposures2;
    DefineExposures3;
    DefineExposures4;
    DefineExposures5;
    DefineExposures6;
    DefineExposures7;
    DefineExposures8;
    DefineExposures9;
    DefineExposures10;
    DefineExposures11;
    DefineExposures12;
    WashoutPeriod1;
    WashoutPeriod2;
    WashoutPeriod3;
    WashoutPeriod4;
    WashoutPeriod5;
    WashoutPeriod6;
    WashoutPeriod7;
    WashoutPeriod8;
    WashoutPeriod9;
    WashoutPeriod10;
    WashoutPeriod11;
    WashoutPeriod12;
    IncidenceRefinement1;
    IncidenceRefinement2;
    IncidenceRefinement3;
    IncidenceRefinement4;
    IncidenceRefinement5;
    IncidenceRefinement6;
    IncidenceRefinement7;
    IncidenceRefinement8;
    IncidenceRefinement9;
    IncidenceRefinement10;
    IncidenceRefinement11;
    IncidenceRefinement12;
    SpecifyExposedTimeAssessment1;
    SpecifyExposedTimeAssessment2;
    SpecifyExposedTimeAssessment3;
    SpecifyExposedTimeAssessment4;
    SpecifyExposedTimeAssessment5;
    SpecifyExposedTimeAssessment6;
    SpecifyExposedTimeAssessment7;
    SpecifyExposedTimeAssessment8;
    SpecifyExposedTimeAssessment9;
    SpecifyExposedTimeAssessment10;
    SpecifyExposedTimeAssessment11;
    SpecifyExposedTimeAssessment12;
    EpisodeAllowableGap1;
    EpisodeAllowableGap2;
    EpisodeAllowableGap3;
    EpisodeAllowableGap4;
    EpisodeAllowableGap5;
    EpisodeAllowableGap6;
    EpisodeAllowableGap7;
    EpisodeAllowableGap8;
    EpisodeAllowableGap9;
    EpisodeAllowableGap10;
    EpisodeAllowableGap11;
    EpisodeAllowableGap12;
    EpisodeExtensionPeriod1;
    EpisodeExtensionPeriod2;
    EpisodeExtensionPeriod3;
    EpisodeExtensionPeriod4;
    EpisodeExtensionPeriod5;
    EpisodeExtensionPeriod6;
    EpisodeExtensionPeriod7;
    EpisodeExtensionPeriod8;
    EpisodeExtensionPeriod9;
    EpisodeExtensionPeriod10;
    EpisodeExtensionPeriod11;
    EpisodeExtensionPeriod12;
    MinimumEpisodeDuration1;
    MinimumEpisodeDuration2;
    MinimumEpisodeDuration3;
    MinimumEpisodeDuration4;
    MinimumEpisodeDuration5;
    MinimumEpisodeDuration6;
    MinimumEpisodeDuration7;
    MinimumEpisodeDuration8;
    MinimumEpisodeDuration9;
    MinimumEpisodeDuration10;
    MinimumEpisodeDuration11;
    MinimumEpisodeDuration12;
    MinimumDaysSupply1;
    MinimumDaysSupply2;
    MinimumDaysSupply3;
    MinimumDaysSupply4;
    MinimumDaysSupply5;
    MinimumDaysSupply6;
    MinimumDaysSupply7;
    MinimumDaysSupply8;
    MinimumDaysSupply9;
    MinimumDaysSupply10;
    MinimumDaysSupply11;
    MinimumDaysSupply12;
    SpecifyFollowUpDuration1;
    SpecifyFollowUpDuration2;
    SpecifyFollowUpDuration3;
    SpecifyFollowUpDuration4;
    SpecifyFollowUpDuration5;
    SpecifyFollowUpDuration6;
    SpecifyFollowUpDuration7;
    SpecifyFollowUpDuration8;
    SpecifyFollowUpDuration9;
    SpecifyFollowUpDuration10;
    SpecifyFollowUpDuration11;
    SpecifyFollowUpDuration12;
    AllowOnOrMultipleExposureEpisodes1;
    AllowOnOrMultipleExposureEpisodes2;
    AllowOnOrMultipleExposureEpisodes3;
    AllowOnOrMultipleExposureEpisodes4;
    AllowOnOrMultipleExposureEpisodes5;
    AllowOnOrMultipleExposureEpisodes6;
    AllowOnOrMultipleExposureEpisodes7;
    AllowOnOrMultipleExposureEpisodes8;
    AllowOnOrMultipleExposureEpisodes9;
    AllowOnOrMultipleExposureEpisodes10;
    AllowOnOrMultipleExposureEpisodes11;
    AllowOnOrMultipleExposureEpisodes12;
    TruncateExposedtime1;
    TruncateExposedtime2;
    TruncateExposedtime3;
    TruncateExposedtime4;
    TruncateExposedtime5;
    TruncateExposedtime6;
    TruncateExposedtime7;
    TruncateExposedtime8;
    TruncateExposedtime9;
    TruncateExposedtime10;
    TruncateExposedtime11;
    TruncateExposedtime12;
    TruncateExposedTimeSpecified1;
    TruncateExposedTimeSpecified2;
    TruncateExposedTimeSpecified3;
    TruncateExposedTimeSpecified4;
    TruncateExposedTimeSpecified5;
    TruncateExposedTimeSpecified6;
    TruncateExposedTimeSpecified7;
    TruncateExposedTimeSpecified8;
    TruncateExposedTimeSpecified9;
    TruncateExposedTimeSpecified10;
    TruncateExposedTimeSpecified11;
    TruncateExposedTimeSpecified12;
    SpecifyBlackoutPeriod1;
    SpecifyBlackoutPeriod2;
    SpecifyBlackoutPeriod3;
    SpecifyBlackoutPeriod4;
    SpecifyBlackoutPeriod5;
    SpecifyBlackoutPeriod6;
    SpecifyBlackoutPeriod7;
    SpecifyBlackoutPeriod8;
    SpecifyBlackoutPeriod9;
    SpecifyBlackoutPeriod10;
    SpecifyBlackoutPeriod11;
    SpecifyBlackoutPeriod12;
    SpecifyAdditionalInclusionInclusionCriteriaGroup11;
    SpecifyAdditionalInclusionInclusionCriteriaGroup12;
    SpecifyAdditionalInclusionInclusionCriteriaGroup13;
    SpecifyAdditionalInclusionInclusionCriteriaGroup14;
    SpecifyAdditionalInclusionInclusionCriteriaGroup15;
    SpecifyAdditionalInclusionInclusionCriteriaGroup16;
    SpecifyAdditionalInclusionEvaluationWindowGroup11;
    SpecifyAdditionalInclusionEvaluationWindowGroup12;
    SpecifyAdditionalInclusionEvaluationWindowGroup13;
    SpecifyAdditionalInclusionEvaluationWindowGroup14;
    SpecifyAdditionalInclusionEvaluationWindowGroup15;
    SpecifyAdditionalInclusionEvaluationWindowGroup16;
    SpecifyAdditionalInclusionInclusionCriteriaGroup21;
    SpecifyAdditionalInclusionInclusionCriteriaGroup22;
    SpecifyAdditionalInclusionInclusionCriteriaGroup23;
    SpecifyAdditionalInclusionInclusionCriteriaGroup24;
    SpecifyAdditionalInclusionInclusionCriteriaGroup25;
    SpecifyAdditionalInclusionInclusionCriteriaGroup26;
    SpecifyAdditionalInclusionEvaluationWindowGroup21;
    SpecifyAdditionalInclusionEvaluationWindowGroup22;
    SpecifyAdditionalInclusionEvaluationWindowGroup23;
    SpecifyAdditionalInclusionEvaluationWindowGroup24;
    SpecifyAdditionalInclusionEvaluationWindowGroup25;
    SpecifyAdditionalInclusionEvaluationWindowGroup26;
    SpecifyAdditionalInclusionInclusionCriteriaGroup31;
    SpecifyAdditionalInclusionInclusionCriteriaGroup32;
    SpecifyAdditionalInclusionInclusionCriteriaGroup33;
    SpecifyAdditionalInclusionInclusionCriteriaGroup34;
    SpecifyAdditionalInclusionInclusionCriteriaGroup35;
    SpecifyAdditionalInclusionInclusionCriteriaGroup36;
    SpecifyAdditionalInclusionEvaluationWindowGroup31;
    SpecifyAdditionalInclusionEvaluationWindowGroup32;
    SpecifyAdditionalInclusionEvaluationWindowGroup33;
    SpecifyAdditionalInclusionEvaluationWindowGroup34;
    SpecifyAdditionalInclusionEvaluationWindowGroup35;
    SpecifyAdditionalInclusionEvaluationWindowGroup36;
    SpecifyAdditionalInclusionInclusionCriteriaGroup41;
    SpecifyAdditionalInclusionInclusionCriteriaGroup42;
    SpecifyAdditionalInclusionInclusionCriteriaGroup43;
    SpecifyAdditionalInclusionInclusionCriteriaGroup44;
    SpecifyAdditionalInclusionInclusionCriteriaGroup45;
    SpecifyAdditionalInclusionInclusionCriteriaGroup46;
    SpecifyAdditionalInclusionEvaluationWindowGroup41;
    SpecifyAdditionalInclusionEvaluationWindowGroup42;
    SpecifyAdditionalInclusionEvaluationWindowGroup43;
    SpecifyAdditionalInclusionEvaluationWindowGroup44;
    SpecifyAdditionalInclusionEvaluationWindowGroup45;
    SpecifyAdditionalInclusionEvaluationWindowGroup46;
    SpecifyAdditionalInclusionInclusionCriteriaGroup51;
    SpecifyAdditionalInclusionInclusionCriteriaGroup52;
    SpecifyAdditionalInclusionInclusionCriteriaGroup53;
    SpecifyAdditionalInclusionInclusionCriteriaGroup54;
    SpecifyAdditionalInclusionInclusionCriteriaGroup55;
    SpecifyAdditionalInclusionInclusionCriteriaGroup56;
    SpecifyAdditionalInclusionEvaluationWindowGroup51;
    SpecifyAdditionalInclusionEvaluationWindowGroup52;
    SpecifyAdditionalInclusionEvaluationWindowGroup53;
    SpecifyAdditionalInclusionEvaluationWindowGroup54;
    SpecifyAdditionalInclusionEvaluationWindowGroup55;
    SpecifyAdditionalInclusionEvaluationWindowGroup56;
    SpecifyAdditionalInclusionInclusionCriteriaGroup61;
    SpecifyAdditionalInclusionInclusionCriteriaGroup62;
    SpecifyAdditionalInclusionInclusionCriteriaGroup63;
    SpecifyAdditionalInclusionInclusionCriteriaGroup64;
    SpecifyAdditionalInclusionInclusionCriteriaGroup65;
    SpecifyAdditionalInclusionInclusionCriteriaGroup66;
    SpecifyAdditionalInclusionEvaluationWindowGroup61;
    SpecifyAdditionalInclusionEvaluationWindowGroup62;
    SpecifyAdditionalInclusionEvaluationWindowGroup63;
    SpecifyAdditionalInclusionEvaluationWindowGroup64;
    SpecifyAdditionalInclusionEvaluationWindowGroup65;
    SpecifyAdditionalInclusionEvaluationWindowGroup66;
    SpecifyAdditionalInclusionInclusionCriteriaGroup71;
    SpecifyAdditionalInclusionInclusionCriteriaGroup72;
    SpecifyAdditionalInclusionInclusionCriteriaGroup73;
    SpecifyAdditionalInclusionInclusionCriteriaGroup74;
    SpecifyAdditionalInclusionInclusionCriteriaGroup75;
    SpecifyAdditionalInclusionInclusionCriteriaGroup76;
    SpecifyAdditionalInclusionEvaluationWindowGroup71;
    SpecifyAdditionalInclusionEvaluationWindowGroup72;
    SpecifyAdditionalInclusionEvaluationWindowGroup73;
    SpecifyAdditionalInclusionEvaluationWindowGroup74;
    SpecifyAdditionalInclusionEvaluationWindowGroup75;
    SpecifyAdditionalInclusionEvaluationWindowGroup76;
    SpecifyAdditionalInclusionInclusionCriteriaGroup81;
    SpecifyAdditionalInclusionInclusionCriteriaGroup82;
    SpecifyAdditionalInclusionInclusionCriteriaGroup83;
    SpecifyAdditionalInclusionInclusionCriteriaGroup84;
    SpecifyAdditionalInclusionInclusionCriteriaGroup85;
    SpecifyAdditionalInclusionInclusionCriteriaGroup86;
    SpecifyAdditionalInclusionEvaluationWindowGroup81;
    SpecifyAdditionalInclusionEvaluationWindowGroup82;
    SpecifyAdditionalInclusionEvaluationWindowGroup83;
    SpecifyAdditionalInclusionEvaluationWindowGroup84;
    SpecifyAdditionalInclusionEvaluationWindowGroup85;
    SpecifyAdditionalInclusionEvaluationWindowGroup86;
    SpecifyAdditionalInclusionInclusionCriteriaGroup91;
    SpecifyAdditionalInclusionInclusionCriteriaGroup92;
    SpecifyAdditionalInclusionInclusionCriteriaGroup93;
    SpecifyAdditionalInclusionInclusionCriteriaGroup94;
    SpecifyAdditionalInclusionInclusionCriteriaGroup95;
    SpecifyAdditionalInclusionInclusionCriteriaGroup96;
    SpecifyAdditionalInclusionEvaluationWindowGroup91;
    SpecifyAdditionalInclusionEvaluationWindowGroup92;
    SpecifyAdditionalInclusionEvaluationWindowGroup93;
    SpecifyAdditionalInclusionEvaluationWindowGroup94;
    SpecifyAdditionalInclusionEvaluationWindowGroup95;
    SpecifyAdditionalInclusionEvaluationWindowGroup96;
    SpecifyAdditionalInclusionInclusionCriteriaGroup101;
    SpecifyAdditionalInclusionInclusionCriteriaGroup102;
    SpecifyAdditionalInclusionInclusionCriteriaGroup103;
    SpecifyAdditionalInclusionInclusionCriteriaGroup104;
    SpecifyAdditionalInclusionInclusionCriteriaGroup105;
    SpecifyAdditionalInclusionInclusionCriteriaGroup106;
    SpecifyAdditionalInclusionEvaluationWindowGroup101;
    SpecifyAdditionalInclusionEvaluationWindowGroup102;
    SpecifyAdditionalInclusionEvaluationWindowGroup103;
    SpecifyAdditionalInclusionEvaluationWindowGroup104;
    SpecifyAdditionalInclusionEvaluationWindowGroup105;
    SpecifyAdditionalInclusionEvaluationWindowGroup106;
    SpecifyAdditionalInclusionInclusionCriteriaGroup111;
    SpecifyAdditionalInclusionInclusionCriteriaGroup112;
    SpecifyAdditionalInclusionInclusionCriteriaGroup113;
    SpecifyAdditionalInclusionInclusionCriteriaGroup114;
    SpecifyAdditionalInclusionInclusionCriteriaGroup115;
    SpecifyAdditionalInclusionInclusionCriteriaGroup116;
    SpecifyAdditionalInclusionEvaluationWindowGroup111;
    SpecifyAdditionalInclusionEvaluationWindowGroup112;
    SpecifyAdditionalInclusionEvaluationWindowGroup113;
    SpecifyAdditionalInclusionEvaluationWindowGroup114;
    SpecifyAdditionalInclusionEvaluationWindowGroup115;
    SpecifyAdditionalInclusionEvaluationWindowGroup116;
    SpecifyAdditionalInclusionInclusionCriteriaGroup121;
    SpecifyAdditionalInclusionInclusionCriteriaGroup122;
    SpecifyAdditionalInclusionInclusionCriteriaGroup123;
    SpecifyAdditionalInclusionInclusionCriteriaGroup124;
    SpecifyAdditionalInclusionInclusionCriteriaGroup125;
    SpecifyAdditionalInclusionInclusionCriteriaGroup126;
    SpecifyAdditionalInclusionEvaluationWindowGroup121;
    SpecifyAdditionalInclusionEvaluationWindowGroup122;
    SpecifyAdditionalInclusionEvaluationWindowGroup123;
    SpecifyAdditionalInclusionEvaluationWindowGroup124;
    SpecifyAdditionalInclusionEvaluationWindowGroup125;
    SpecifyAdditionalInclusionEvaluationWindowGroup126;
    SpecifyAdditionalExclusionInclusionCriteriaGroup11;
    SpecifyAdditionalExclusionInclusionCriteriaGroup12;
    SpecifyAdditionalExclusionInclusionCriteriaGroup13;
    SpecifyAdditionalExclusionInclusionCriteriaGroup14;
    SpecifyAdditionalExclusionInclusionCriteriaGroup15;
    SpecifyAdditionalExclusionInclusionCriteriaGroup16;
    SpecifyAdditionalExclusionEvaluationWindowGroup11;
    SpecifyAdditionalExclusionEvaluationWindowGroup12;
    SpecifyAdditionalExclusionEvaluationWindowGroup13;
    SpecifyAdditionalExclusionEvaluationWindowGroup14;
    SpecifyAdditionalExclusionEvaluationWindowGroup15;
    SpecifyAdditionalExclusionEvaluationWindowGroup16;
    SpecifyAdditionalExclusionInclusionCriteriaGroup21;
    SpecifyAdditionalExclusionInclusionCriteriaGroup22;
    SpecifyAdditionalExclusionInclusionCriteriaGroup23;
    SpecifyAdditionalExclusionInclusionCriteriaGroup24;
    SpecifyAdditionalExclusionInclusionCriteriaGroup25;
    SpecifyAdditionalExclusionInclusionCriteriaGroup26;
    SpecifyAdditionalExclusionEvaluationWindowGroup21;
    SpecifyAdditionalExclusionEvaluationWindowGroup22;
    SpecifyAdditionalExclusionEvaluationWindowGroup23;
    SpecifyAdditionalExclusionEvaluationWindowGroup24;
    SpecifyAdditionalExclusionEvaluationWindowGroup25;
    SpecifyAdditionalExclusionEvaluationWindowGroup26;
    SpecifyAdditionalExclusionInclusionCriteriaGroup31;
    SpecifyAdditionalExclusionInclusionCriteriaGroup32;
    SpecifyAdditionalExclusionInclusionCriteriaGroup33;
    SpecifyAdditionalExclusionInclusionCriteriaGroup34;
    SpecifyAdditionalExclusionInclusionCriteriaGroup35;
    SpecifyAdditionalExclusionInclusionCriteriaGroup36;
    SpecifyAdditionalExclusionEvaluationWindowGroup31;
    SpecifyAdditionalExclusionEvaluationWindowGroup32;
    SpecifyAdditionalExclusionEvaluationWindowGroup33;
    SpecifyAdditionalExclusionEvaluationWindowGroup34;
    SpecifyAdditionalExclusionEvaluationWindowGroup35;
    SpecifyAdditionalExclusionEvaluationWindowGroup36;
    SpecifyAdditionalExclusionInclusionCriteriaGroup41;
    SpecifyAdditionalExclusionInclusionCriteriaGroup42;
    SpecifyAdditionalExclusionInclusionCriteriaGroup43;
    SpecifyAdditionalExclusionInclusionCriteriaGroup44;
    SpecifyAdditionalExclusionInclusionCriteriaGroup45;
    SpecifyAdditionalExclusionInclusionCriteriaGroup46;
    SpecifyAdditionalExclusionEvaluationWindowGroup41;
    SpecifyAdditionalExclusionEvaluationWindowGroup42;
    SpecifyAdditionalExclusionEvaluationWindowGroup43;
    SpecifyAdditionalExclusionEvaluationWindowGroup44;
    SpecifyAdditionalExclusionEvaluationWindowGroup45;
    SpecifyAdditionalExclusionEvaluationWindowGroup46;
    SpecifyAdditionalExclusionInclusionCriteriaGroup51;
    SpecifyAdditionalExclusionInclusionCriteriaGroup52;
    SpecifyAdditionalExclusionInclusionCriteriaGroup53;
    SpecifyAdditionalExclusionInclusionCriteriaGroup54;
    SpecifyAdditionalExclusionInclusionCriteriaGroup55;
    SpecifyAdditionalExclusionInclusionCriteriaGroup56;
    SpecifyAdditionalExclusionEvaluationWindowGroup51;
    SpecifyAdditionalExclusionEvaluationWindowGroup52;
    SpecifyAdditionalExclusionEvaluationWindowGroup53;
    SpecifyAdditionalExclusionEvaluationWindowGroup54;
    SpecifyAdditionalExclusionEvaluationWindowGroup55;
    SpecifyAdditionalExclusionEvaluationWindowGroup56;
    SpecifyAdditionalExclusionInclusionCriteriaGroup61;
    SpecifyAdditionalExclusionInclusionCriteriaGroup62;
    SpecifyAdditionalExclusionInclusionCriteriaGroup63;
    SpecifyAdditionalExclusionInclusionCriteriaGroup64;
    SpecifyAdditionalExclusionInclusionCriteriaGroup65;
    SpecifyAdditionalExclusionInclusionCriteriaGroup66;
    SpecifyAdditionalExclusionEvaluationWindowGroup61;
    SpecifyAdditionalExclusionEvaluationWindowGroup62;
    SpecifyAdditionalExclusionEvaluationWindowGroup63;
    SpecifyAdditionalExclusionEvaluationWindowGroup64;
    SpecifyAdditionalExclusionEvaluationWindowGroup65;
    SpecifyAdditionalExclusionEvaluationWindowGroup66;
    SpecifyAdditionalExclusionInclusionCriteriaGroup71;
    SpecifyAdditionalExclusionInclusionCriteriaGroup72;
    SpecifyAdditionalExclusionInclusionCriteriaGroup73;
    SpecifyAdditionalExclusionInclusionCriteriaGroup74;
    SpecifyAdditionalExclusionInclusionCriteriaGroup75;
    SpecifyAdditionalExclusionInclusionCriteriaGroup76;
    SpecifyAdditionalExclusionEvaluationWindowGroup71;
    SpecifyAdditionalExclusionEvaluationWindowGroup72;
    SpecifyAdditionalExclusionEvaluationWindowGroup73;
    SpecifyAdditionalExclusionEvaluationWindowGroup74;
    SpecifyAdditionalExclusionEvaluationWindowGroup75;
    SpecifyAdditionalExclusionEvaluationWindowGroup76;
    SpecifyAdditionalExclusionInclusionCriteriaGroup81;
    SpecifyAdditionalExclusionInclusionCriteriaGroup82;
    SpecifyAdditionalExclusionInclusionCriteriaGroup83;
    SpecifyAdditionalExclusionInclusionCriteriaGroup84;
    SpecifyAdditionalExclusionInclusionCriteriaGroup85;
    SpecifyAdditionalExclusionInclusionCriteriaGroup86;
    SpecifyAdditionalExclusionEvaluationWindowGroup81;
    SpecifyAdditionalExclusionEvaluationWindowGroup82;
    SpecifyAdditionalExclusionEvaluationWindowGroup83;
    SpecifyAdditionalExclusionEvaluationWindowGroup84;
    SpecifyAdditionalExclusionEvaluationWindowGroup85;
    SpecifyAdditionalExclusionEvaluationWindowGroup86;
    SpecifyAdditionalExclusionInclusionCriteriaGroup91;
    SpecifyAdditionalExclusionInclusionCriteriaGroup92;
    SpecifyAdditionalExclusionInclusionCriteriaGroup93;
    SpecifyAdditionalExclusionInclusionCriteriaGroup94;
    SpecifyAdditionalExclusionInclusionCriteriaGroup95;
    SpecifyAdditionalExclusionInclusionCriteriaGroup96;
    SpecifyAdditionalExclusionEvaluationWindowGroup91;
    SpecifyAdditionalExclusionEvaluationWindowGroup92;
    SpecifyAdditionalExclusionEvaluationWindowGroup93;
    SpecifyAdditionalExclusionEvaluationWindowGroup94;
    SpecifyAdditionalExclusionEvaluationWindowGroup95;
    SpecifyAdditionalExclusionEvaluationWindowGroup96;
    SpecifyAdditionalExclusionInclusionCriteriaGroup101;
    SpecifyAdditionalExclusionInclusionCriteriaGroup102;
    SpecifyAdditionalExclusionInclusionCriteriaGroup103;
    SpecifyAdditionalExclusionInclusionCriteriaGroup104;
    SpecifyAdditionalExclusionInclusionCriteriaGroup105;
    SpecifyAdditionalExclusionInclusionCriteriaGroup106;
    SpecifyAdditionalExclusionEvaluationWindowGroup101;
    SpecifyAdditionalExclusionEvaluationWindowGroup102;
    SpecifyAdditionalExclusionEvaluationWindowGroup103;
    SpecifyAdditionalExclusionEvaluationWindowGroup104;
    SpecifyAdditionalExclusionEvaluationWindowGroup105;
    SpecifyAdditionalExclusionEvaluationWindowGroup106;
    SpecifyAdditionalExclusionInclusionCriteriaGroup111;
    SpecifyAdditionalExclusionInclusionCriteriaGroup112;
    SpecifyAdditionalExclusionInclusionCriteriaGroup113;
    SpecifyAdditionalExclusionInclusionCriteriaGroup114;
    SpecifyAdditionalExclusionInclusionCriteriaGroup115;
    SpecifyAdditionalExclusionInclusionCriteriaGroup116;
    SpecifyAdditionalExclusionEvaluationWindowGroup111;
    SpecifyAdditionalExclusionEvaluationWindowGroup112;
    SpecifyAdditionalExclusionEvaluationWindowGroup113;
    SpecifyAdditionalExclusionEvaluationWindowGroup114;
    SpecifyAdditionalExclusionEvaluationWindowGroup115;
    SpecifyAdditionalExclusionEvaluationWindowGroup116;
    SpecifyAdditionalExclusionInclusionCriteriaGroup121;
    SpecifyAdditionalExclusionInclusionCriteriaGroup122;
    SpecifyAdditionalExclusionInclusionCriteriaGroup123;
    SpecifyAdditionalExclusionInclusionCriteriaGroup124;
    SpecifyAdditionalExclusionInclusionCriteriaGroup125;
    SpecifyAdditionalExclusionInclusionCriteriaGroup126;
    SpecifyAdditionalExclusionEvaluationWindowGroup121;
    SpecifyAdditionalExclusionEvaluationWindowGroup122;
    SpecifyAdditionalExclusionEvaluationWindowGroup123;
    SpecifyAdditionalExclusionEvaluationWindowGroup124;
    SpecifyAdditionalExclusionEvaluationWindowGroup125;
    SpecifyAdditionalExclusionEvaluationWindowGroup126;
    LookBackPeriodGroup1;
    LookBackPeriodGroup2;
    LookBackPeriodGroup3;
    LookBackPeriodGroup4;
    LookBackPeriodGroup5;
    LookBackPeriodGroup6;
    LookBackPeriodGroup7;
    LookBackPeriodGroup8;
    LookBackPeriodGroup9;
    LookBackPeriodGroup10;
    LookBackPeriodGroup11;
    LookBackPeriodGroup12;
    IncludeIndexDate1;
    IncludeIndexDate2;
    IncludeIndexDate3;
    IncludeIndexDate4;
    IncludeIndexDate5;
    IncludeIndexDate6;
    IncludeIndexDate7;
    IncludeIndexDate8;
    IncludeIndexDate9;
    IncludeIndexDate10;
    IncludeIndexDate11;
    IncludeIndexDate12;
    StratificationCategories1;
    StratificationCategories2;
    StratificationCategories3;
    StratificationCategories4;
    StratificationCategories5;
    StratificationCategories6;
    StratificationCategories7;
    StratificationCategories8;
    StratificationCategories9;
    StratificationCategories10;
    StratificationCategories11;
    StratificationCategories12;
    TwelveSpecifyLoopBackPeriod1;
    TwelveSpecifyLoopBackPeriod2;
    TwelveSpecifyLoopBackPeriod3;
    TwelveSpecifyLoopBackPeriod4;
    TwelveSpecifyLoopBackPeriod5;
    TwelveSpecifyLoopBackPeriod6;
    TwelveSpecifyLoopBackPeriod7;
    TwelveSpecifyLoopBackPeriod8;
    TwelveSpecifyLoopBackPeriod9;
    TwelveSpecifyLoopBackPeriod10;
    TwelveSpecifyLoopBackPeriod11;
    TwelveSpecifyLoopBackPeriod12;
    TwelveIncludeIndexDate1;
    TwelveIncludeIndexDate2;
    TwelveIncludeIndexDate3;
    TwelveIncludeIndexDate4;
    TwelveIncludeIndexDate5;
    TwelveIncludeIndexDate6;
    TwelveIncludeIndexDate7;
    TwelveIncludeIndexDate8;
    TwelveIncludeIndexDate9;
    TwelveIncludeIndexDate10;
    TwelveIncludeIndexDate11;
    TwelveIncludeIndexDate12;
    CareSettingsToDefineMedicalVisits1;
    CareSettingsToDefineMedicalVisits2;
    CareSettingsToDefineMedicalVisits3;
    CareSettingsToDefineMedicalVisits4;
    CareSettingsToDefineMedicalVisits5;
    CareSettingsToDefineMedicalVisits6;
    CareSettingsToDefineMedicalVisits7;
    CareSettingsToDefineMedicalVisits8;
    CareSettingsToDefineMedicalVisits9;
    CareSettingsToDefineMedicalVisits10;
    CareSettingsToDefineMedicalVisits11;
    CareSettingsToDefineMedicalVisits12;
    TwelveStratificationCategories1;
    TwelveStratificationCategories2;
    TwelveStratificationCategories3;
    TwelveStratificationCategories4;
    TwelveStratificationCategories5;
    TwelveStratificationCategories6;
    TwelveStratificationCategories7;
    TwelveStratificationCategories8;
    TwelveStratificationCategories9;
    TwelveStratificationCategories10;
    TwelveStratificationCategories11;
    TwelveStratificationCategories12;
    VaryLengthOfWashoutPeriod1;
    VaryLengthOfWashoutPeriod2;
    VaryLengthOfWashoutPeriod3;
    VaryLengthOfWashoutPeriod4;
    VaryLengthOfWashoutPeriod5;
    VaryLengthOfWashoutPeriod6;
    VaryLengthOfWashoutPeriod7;
    VaryLengthOfWashoutPeriod8;
    VaryLengthOfWashoutPeriod9;
    VaryLengthOfWashoutPeriod10;
    VaryLengthOfWashoutPeriod11;
    VaryLengthOfWashoutPeriod12;
    VaryUserExposedTime1;
    VaryUserExposedTime2;
    VaryUserExposedTime3;
    VaryUserExposedTime4;
    VaryUserExposedTime5;
    VaryUserExposedTime6;
    VaryUserExposedTime7;
    VaryUserExposedTime8;
    VaryUserExposedTime9;
    VaryUserExposedTime10;
    VaryUserExposedTime11;
    VaryUserExposedTime12;
    VaryUserFollowupPeriodDuration1;
    VaryUserFollowupPeriodDuration2;
    VaryUserFollowupPeriodDuration3;
    VaryUserFollowupPeriodDuration4;
    VaryUserFollowupPeriodDuration5;
    VaryUserFollowupPeriodDuration6;
    VaryUserFollowupPeriodDuration7;
    VaryUserFollowupPeriodDuration8;
    VaryUserFollowupPeriodDuration9;
    VaryUserFollowupPeriodDuration10;
    VaryUserFollowupPeriodDuration11;
    VaryUserFollowupPeriodDuration12;
    VaryBlackoutPeriodPeriod1;
    VaryBlackoutPeriodPeriod2;
    VaryBlackoutPeriodPeriod3;
    VaryBlackoutPeriodPeriod4;
    VaryBlackoutPeriodPeriod5;
    VaryBlackoutPeriodPeriod6;
    VaryBlackoutPeriodPeriod7;
    VaryBlackoutPeriodPeriod8;
    VaryBlackoutPeriodPeriod9;
    VaryBlackoutPeriodPeriod10;
    VaryBlackoutPeriodPeriod11;
    VaryBlackoutPeriodPeriod12;
    Level2or3DefineExposures1Exposure;
    Level2or3DefineExposures1Compare;
    Level2or3DefineExposures2Exposure;
    Level2or3DefineExposures2Compare;
    Level2or3DefineExposures3Exposure;
    Level2or3DefineExposures3Compare;
    Level2or3WashoutPeriod1Exposure;
    Level2or3WashoutPeriod1Compare;
    Level2or3WashoutPeriod2Exposure;
    Level2or3WashoutPeriod2Compare;
    Level2or3WashoutPeriod3Exposure;
    Level2or3WashoutPeriod3Compare;
    Level2or3SpecifyExposedTimeAssessment1Exposure;
    Level2or3SpecifyExposedTimeAssessment1Compare;
    Level2or3SpecifyExposedTimeAssessment2Exposure;
    Level2or3SpecifyExposedTimeAssessment2Compare;
    Level2or3SpecifyExposedTimeAssessment3Exposure;
    Level2or3SpecifyExposedTimeAssessment3Compare;
    Level2or3EpisodeAllowableGap1Exposure;
    Level2or3EpisodeAllowableGap1Compare;
    Level2or3EpisodeAllowableGap2Exposure;
    Level2or3EpisodeAllowableGap2Compare;
    Level2or3EpisodeAllowableGap3Exposure;
    Level2or3EpisodeAllowableGap3Compare;
    Level2or3EpisodeExtensionPeriod1Exposure;
    Level2or3EpisodeExtensionPeriod1Compare;
    Level2or3EpisodeExtensionPeriod2Exposure;
    Level2or3EpisodeExtensionPeriod2Compare;
    Level2or3EpisodeExtensionPeriod3Exposure;
    Level2or3EpisodeExtensionPeriod3Compare;
    Level2or3MinimumEpisodeDuration1Exposure;
    Level2or3MinimumEpisodeDuration1Compare;
    Level2or3MinimumEpisodeDuration2Exposure;
    Level2or3MinimumEpisodeDuration2Compare;
    Level2or3MinimumEpisodeDuration3Exposure;
    Level2or3MinimumEpisodeDuration3Compare;
    Level2or3MinimumDaysSupply1Exposure;
    Level2or3MinimumDaysSupply1Compare;
    Level2or3MinimumDaysSupply2Exposure;
    Level2or3MinimumDaysSupply2Compare;
    Level2or3MinimumDaysSupply3Exposure;
    Level2or3MinimumDaysSupply3Compare;
    Level2or3SpecifyFollowUpDuration1Exposure;
    Level2or3SpecifyFollowUpDuration1Compare;
    Level2or3SpecifyFollowUpDuration2Exposure;
    Level2or3SpecifyFollowUpDuration2Compare;
    Level2or3SpecifyFollowUpDuration3Exposure;
    Level2or3SpecifyFollowUpDuration3Compare;
    Level2or3AllowOnOrMultipleExposureEpisodes1Exposure;
    Level2or3AllowOnOrMultipleExposureEpisodes1Compare;
    Level2or3AllowOnOrMultipleExposureEpisodes2Exposure;
    Level2or3AllowOnOrMultipleExposureEpisodes2Compare;
    Level2or3AllowOnOrMultipleExposureEpisodes3Exposure;
    Level2or3AllowOnOrMultipleExposureEpisodes3Compare;
    Level2or3TruncateExposedtime1Exposure;
    Level2or3TruncateExposedtime1Compare;
    Level2or3TruncateExposedtime2Exposure;
    Level2or3TruncateExposedtime2Compare;
    Level2or3TruncateExposedtime3Exposure;
    Level2or3TruncateExposedtime3Compare;
    Level2or3TruncateExposedTimeSpecified1Exposure;
    Level2or3TruncateExposedTimeSpecified1Compare;
    Level2or3TruncateExposedTimeSpecified2Exposure;
    Level2or3TruncateExposedTimeSpecified2Compare;
    Level2or3TruncateExposedTimeSpecified3Exposure;
    Level2or3TruncateExposedTimeSpecified3Compare;
    Level2or3SpecifyBlackoutPeriod1Exposure;
    Level2or3SpecifyBlackoutPeriod1Compare;
    Level2or3SpecifyBlackoutPeriod2Exposure;
    Level2or3SpecifyBlackoutPeriod2Compare;
    Level2or3SpecifyBlackoutPeriod3Exposure;
    Level2or3SpecifyBlackoutPeriod3Compare;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62;
    Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62;
    Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62;
    Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62;
    Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63;
    Level2or3VaryLengthOfWashoutPeriod1Exposure;
    Level2or3VaryLengthOfWashoutPeriod1Compare;
    Level2or3VaryLengthOfWashoutPeriod2Exposure;
    Level2or3VaryLengthOfWashoutPeriod2Compare;
    Level2or3VaryLengthOfWashoutPeriod3Exposure;
    Level2or3VaryLengthOfWashoutPeriod3Compare;
    Level2or3VaryUserExposedTime1Exposure;
    Level2or3VaryUserExposedTime1Compare;
    Level2or3VaryUserExposedTime2Exposure;
    Level2or3VaryUserExposedTime2Compare;
    Level2or3VaryUserExposedTime3Exposure;
    Level2or3VaryUserExposedTime3Compare;
    Level2or3VaryBlackoutPeriodPeriod1Exposure;
    Level2or3VaryBlackoutPeriodPeriod1Compare;
    Level2or3VaryBlackoutPeriodPeriod2Exposure;
    Level2or3VaryBlackoutPeriodPeriod2Compare;
    Level2or3VaryBlackoutPeriodPeriod3Exposure;
    Level2or3VaryBlackoutPeriodPeriod3Compare;
    OutcomeList;
    AgeCovariate;
    SexCovariate;
    TimeCovariate;
    YearCovariate;
    ComorbidityCovariate;
    HealthCovariate;
    DrugCovariate;
    CovariateList;
    hdPSAnalysis;
    InclusionCovariates;
    PoolCovariates;
    SelectionCovariates;
    ZeroCellCorrection;
    MatchingRatio;
    MatchingCalipers;
    VaryMatchingRatio;
    VaryMatchingCalipers;
    constructor(RequestFormDTO) {
        super();
        if (RequestFormDTO == null) {
            this.RequestDueDate = ko.observable();
            this.ContactInfo = ko.observable();
            this.RequestingTeam = ko.observable();
            this.FDAReview = ko.observable();
            this.FDADivisionNA = ko.observable();
            this.FDADivisionDAAAP = ko.observable();
            this.FDADivisionDBRUP = ko.observable();
            this.FDADivisionDCARP = ko.observable();
            this.FDADivisionDDDP = ko.observable();
            this.FDADivisionDGIEP = ko.observable();
            this.FDADivisionDMIP = ko.observable();
            this.FDADivisionDMEP = ko.observable();
            this.FDADivisionDNP = ko.observable();
            this.FDADivisionDDP = ko.observable();
            this.FDADivisionDPARP = ko.observable();
            this.FDADivisionOther = ko.observable();
            this.QueryLevel = ko.observable();
            this.AdjustmentMethod = ko.observable();
            this.CohortID = ko.observable();
            this.StudyObjectives = ko.observable();
            this.RequestStartDate = ko.observable();
            this.RequestEndDate = ko.observable();
            this.AgeGroups = ko.observable();
            this.CoverageTypes = ko.observable();
            this.EnrollmentGap = ko.observable();
            this.EnrollmentExposure = ko.observable();
            this.DefineExposures = ko.observable();
            this.WashoutPeirod = ko.observable();
            this.OtherExposures = ko.observable();
            this.OneOrManyExposures = ko.observable();
            this.AdditionalInclusion = ko.observable();
            this.AdditionalInclusionEvaluation = ko.observable();
            this.AdditionalExclusion = ko.observable();
            this.AdditionalExclusionEvaluation = ko.observable();
            this.VaryWashoutPeirod = ko.observable();
            this.VaryExposures = ko.observable();
            this.DefineExposures1 = ko.observable();
            this.DefineExposures2 = ko.observable();
            this.DefineExposures3 = ko.observable();
            this.DefineExposures4 = ko.observable();
            this.DefineExposures5 = ko.observable();
            this.DefineExposures6 = ko.observable();
            this.DefineExposures7 = ko.observable();
            this.DefineExposures8 = ko.observable();
            this.DefineExposures9 = ko.observable();
            this.DefineExposures10 = ko.observable();
            this.DefineExposures11 = ko.observable();
            this.DefineExposures12 = ko.observable();
            this.WashoutPeriod1 = ko.observable();
            this.WashoutPeriod2 = ko.observable();
            this.WashoutPeriod3 = ko.observable();
            this.WashoutPeriod4 = ko.observable();
            this.WashoutPeriod5 = ko.observable();
            this.WashoutPeriod6 = ko.observable();
            this.WashoutPeriod7 = ko.observable();
            this.WashoutPeriod8 = ko.observable();
            this.WashoutPeriod9 = ko.observable();
            this.WashoutPeriod10 = ko.observable();
            this.WashoutPeriod11 = ko.observable();
            this.WashoutPeriod12 = ko.observable();
            this.IncidenceRefinement1 = ko.observable();
            this.IncidenceRefinement2 = ko.observable();
            this.IncidenceRefinement3 = ko.observable();
            this.IncidenceRefinement4 = ko.observable();
            this.IncidenceRefinement5 = ko.observable();
            this.IncidenceRefinement6 = ko.observable();
            this.IncidenceRefinement7 = ko.observable();
            this.IncidenceRefinement8 = ko.observable();
            this.IncidenceRefinement9 = ko.observable();
            this.IncidenceRefinement10 = ko.observable();
            this.IncidenceRefinement11 = ko.observable();
            this.IncidenceRefinement12 = ko.observable();
            this.SpecifyExposedTimeAssessment1 = ko.observable();
            this.SpecifyExposedTimeAssessment2 = ko.observable();
            this.SpecifyExposedTimeAssessment3 = ko.observable();
            this.SpecifyExposedTimeAssessment4 = ko.observable();
            this.SpecifyExposedTimeAssessment5 = ko.observable();
            this.SpecifyExposedTimeAssessment6 = ko.observable();
            this.SpecifyExposedTimeAssessment7 = ko.observable();
            this.SpecifyExposedTimeAssessment8 = ko.observable();
            this.SpecifyExposedTimeAssessment9 = ko.observable();
            this.SpecifyExposedTimeAssessment10 = ko.observable();
            this.SpecifyExposedTimeAssessment11 = ko.observable();
            this.SpecifyExposedTimeAssessment12 = ko.observable();
            this.EpisodeAllowableGap1 = ko.observable();
            this.EpisodeAllowableGap2 = ko.observable();
            this.EpisodeAllowableGap3 = ko.observable();
            this.EpisodeAllowableGap4 = ko.observable();
            this.EpisodeAllowableGap5 = ko.observable();
            this.EpisodeAllowableGap6 = ko.observable();
            this.EpisodeAllowableGap7 = ko.observable();
            this.EpisodeAllowableGap8 = ko.observable();
            this.EpisodeAllowableGap9 = ko.observable();
            this.EpisodeAllowableGap10 = ko.observable();
            this.EpisodeAllowableGap11 = ko.observable();
            this.EpisodeAllowableGap12 = ko.observable();
            this.EpisodeExtensionPeriod1 = ko.observable();
            this.EpisodeExtensionPeriod2 = ko.observable();
            this.EpisodeExtensionPeriod3 = ko.observable();
            this.EpisodeExtensionPeriod4 = ko.observable();
            this.EpisodeExtensionPeriod5 = ko.observable();
            this.EpisodeExtensionPeriod6 = ko.observable();
            this.EpisodeExtensionPeriod7 = ko.observable();
            this.EpisodeExtensionPeriod8 = ko.observable();
            this.EpisodeExtensionPeriod9 = ko.observable();
            this.EpisodeExtensionPeriod10 = ko.observable();
            this.EpisodeExtensionPeriod11 = ko.observable();
            this.EpisodeExtensionPeriod12 = ko.observable();
            this.MinimumEpisodeDuration1 = ko.observable();
            this.MinimumEpisodeDuration2 = ko.observable();
            this.MinimumEpisodeDuration3 = ko.observable();
            this.MinimumEpisodeDuration4 = ko.observable();
            this.MinimumEpisodeDuration5 = ko.observable();
            this.MinimumEpisodeDuration6 = ko.observable();
            this.MinimumEpisodeDuration7 = ko.observable();
            this.MinimumEpisodeDuration8 = ko.observable();
            this.MinimumEpisodeDuration9 = ko.observable();
            this.MinimumEpisodeDuration10 = ko.observable();
            this.MinimumEpisodeDuration11 = ko.observable();
            this.MinimumEpisodeDuration12 = ko.observable();
            this.MinimumDaysSupply1 = ko.observable();
            this.MinimumDaysSupply2 = ko.observable();
            this.MinimumDaysSupply3 = ko.observable();
            this.MinimumDaysSupply4 = ko.observable();
            this.MinimumDaysSupply5 = ko.observable();
            this.MinimumDaysSupply6 = ko.observable();
            this.MinimumDaysSupply7 = ko.observable();
            this.MinimumDaysSupply8 = ko.observable();
            this.MinimumDaysSupply9 = ko.observable();
            this.MinimumDaysSupply10 = ko.observable();
            this.MinimumDaysSupply11 = ko.observable();
            this.MinimumDaysSupply12 = ko.observable();
            this.SpecifyFollowUpDuration1 = ko.observable();
            this.SpecifyFollowUpDuration2 = ko.observable();
            this.SpecifyFollowUpDuration3 = ko.observable();
            this.SpecifyFollowUpDuration4 = ko.observable();
            this.SpecifyFollowUpDuration5 = ko.observable();
            this.SpecifyFollowUpDuration6 = ko.observable();
            this.SpecifyFollowUpDuration7 = ko.observable();
            this.SpecifyFollowUpDuration8 = ko.observable();
            this.SpecifyFollowUpDuration9 = ko.observable();
            this.SpecifyFollowUpDuration10 = ko.observable();
            this.SpecifyFollowUpDuration11 = ko.observable();
            this.SpecifyFollowUpDuration12 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes1 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes2 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes3 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes4 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes5 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes6 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes7 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes8 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes9 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes10 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes11 = ko.observable();
            this.AllowOnOrMultipleExposureEpisodes12 = ko.observable();
            this.TruncateExposedtime1 = ko.observable();
            this.TruncateExposedtime2 = ko.observable();
            this.TruncateExposedtime3 = ko.observable();
            this.TruncateExposedtime4 = ko.observable();
            this.TruncateExposedtime5 = ko.observable();
            this.TruncateExposedtime6 = ko.observable();
            this.TruncateExposedtime7 = ko.observable();
            this.TruncateExposedtime8 = ko.observable();
            this.TruncateExposedtime9 = ko.observable();
            this.TruncateExposedtime10 = ko.observable();
            this.TruncateExposedtime11 = ko.observable();
            this.TruncateExposedtime12 = ko.observable();
            this.TruncateExposedTimeSpecified1 = ko.observable();
            this.TruncateExposedTimeSpecified2 = ko.observable();
            this.TruncateExposedTimeSpecified3 = ko.observable();
            this.TruncateExposedTimeSpecified4 = ko.observable();
            this.TruncateExposedTimeSpecified5 = ko.observable();
            this.TruncateExposedTimeSpecified6 = ko.observable();
            this.TruncateExposedTimeSpecified7 = ko.observable();
            this.TruncateExposedTimeSpecified8 = ko.observable();
            this.TruncateExposedTimeSpecified9 = ko.observable();
            this.TruncateExposedTimeSpecified10 = ko.observable();
            this.TruncateExposedTimeSpecified11 = ko.observable();
            this.TruncateExposedTimeSpecified12 = ko.observable();
            this.SpecifyBlackoutPeriod1 = ko.observable();
            this.SpecifyBlackoutPeriod2 = ko.observable();
            this.SpecifyBlackoutPeriod3 = ko.observable();
            this.SpecifyBlackoutPeriod4 = ko.observable();
            this.SpecifyBlackoutPeriod5 = ko.observable();
            this.SpecifyBlackoutPeriod6 = ko.observable();
            this.SpecifyBlackoutPeriod7 = ko.observable();
            this.SpecifyBlackoutPeriod8 = ko.observable();
            this.SpecifyBlackoutPeriod9 = ko.observable();
            this.SpecifyBlackoutPeriod10 = ko.observable();
            this.SpecifyBlackoutPeriod11 = ko.observable();
            this.SpecifyBlackoutPeriod12 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup14 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup15 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup16 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup14 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup15 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup16 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup24 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup25 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup26 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup24 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup25 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup26 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup34 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup35 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup36 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup34 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup35 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup36 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup44 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup45 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup46 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup44 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup45 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup46 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup54 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup55 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup56 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup54 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup55 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup56 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup64 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup65 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup66 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup64 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup65 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup66 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup71 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup72 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup73 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup74 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup75 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup76 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup71 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup72 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup73 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup74 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup75 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup76 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup81 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup82 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup83 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup84 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup85 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup86 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup81 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup82 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup83 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup84 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup85 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup86 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup91 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup92 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup93 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup94 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup95 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup96 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup91 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup92 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup93 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup94 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup95 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup96 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup101 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup102 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup103 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup104 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup105 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup106 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup101 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup102 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup103 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup104 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup105 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup106 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup111 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup112 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup113 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup114 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup115 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup116 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup111 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup112 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup113 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup114 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup115 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup116 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup121 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup122 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup123 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup124 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup125 = ko.observable();
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup126 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup121 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup122 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup123 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup124 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup125 = ko.observable();
            this.SpecifyAdditionalInclusionEvaluationWindowGroup126 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup14 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup15 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup16 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup14 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup15 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup16 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup24 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup25 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup26 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup24 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup25 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup26 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup34 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup35 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup36 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup34 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup35 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup36 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup44 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup45 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup46 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup44 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup45 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup46 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup54 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup55 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup56 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup54 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup55 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup56 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup64 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup65 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup66 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup64 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup65 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup66 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup71 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup72 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup73 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup74 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup75 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup76 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup71 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup72 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup73 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup74 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup75 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup76 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup81 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup82 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup83 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup84 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup85 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup86 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup81 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup82 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup83 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup84 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup85 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup86 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup91 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup92 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup93 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup94 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup95 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup96 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup91 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup92 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup93 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup94 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup95 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup96 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup101 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup102 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup103 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup104 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup105 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup106 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup101 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup102 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup103 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup104 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup105 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup106 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup111 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup112 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup113 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup114 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup115 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup116 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup111 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup112 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup113 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup114 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup115 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup116 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup121 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup122 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup123 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup124 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup125 = ko.observable();
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup126 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup121 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup122 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup123 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup124 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup125 = ko.observable();
            this.SpecifyAdditionalExclusionEvaluationWindowGroup126 = ko.observable();
            this.LookBackPeriodGroup1 = ko.observable();
            this.LookBackPeriodGroup2 = ko.observable();
            this.LookBackPeriodGroup3 = ko.observable();
            this.LookBackPeriodGroup4 = ko.observable();
            this.LookBackPeriodGroup5 = ko.observable();
            this.LookBackPeriodGroup6 = ko.observable();
            this.LookBackPeriodGroup7 = ko.observable();
            this.LookBackPeriodGroup8 = ko.observable();
            this.LookBackPeriodGroup9 = ko.observable();
            this.LookBackPeriodGroup10 = ko.observable();
            this.LookBackPeriodGroup11 = ko.observable();
            this.LookBackPeriodGroup12 = ko.observable();
            this.IncludeIndexDate1 = ko.observable();
            this.IncludeIndexDate2 = ko.observable();
            this.IncludeIndexDate3 = ko.observable();
            this.IncludeIndexDate4 = ko.observable();
            this.IncludeIndexDate5 = ko.observable();
            this.IncludeIndexDate6 = ko.observable();
            this.IncludeIndexDate7 = ko.observable();
            this.IncludeIndexDate8 = ko.observable();
            this.IncludeIndexDate9 = ko.observable();
            this.IncludeIndexDate10 = ko.observable();
            this.IncludeIndexDate11 = ko.observable();
            this.IncludeIndexDate12 = ko.observable();
            this.StratificationCategories1 = ko.observable();
            this.StratificationCategories2 = ko.observable();
            this.StratificationCategories3 = ko.observable();
            this.StratificationCategories4 = ko.observable();
            this.StratificationCategories5 = ko.observable();
            this.StratificationCategories6 = ko.observable();
            this.StratificationCategories7 = ko.observable();
            this.StratificationCategories8 = ko.observable();
            this.StratificationCategories9 = ko.observable();
            this.StratificationCategories10 = ko.observable();
            this.StratificationCategories11 = ko.observable();
            this.StratificationCategories12 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod1 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod2 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod3 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod4 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod5 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod6 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod7 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod8 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod9 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod10 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod11 = ko.observable();
            this.TwelveSpecifyLoopBackPeriod12 = ko.observable();
            this.TwelveIncludeIndexDate1 = ko.observable();
            this.TwelveIncludeIndexDate2 = ko.observable();
            this.TwelveIncludeIndexDate3 = ko.observable();
            this.TwelveIncludeIndexDate4 = ko.observable();
            this.TwelveIncludeIndexDate5 = ko.observable();
            this.TwelveIncludeIndexDate6 = ko.observable();
            this.TwelveIncludeIndexDate7 = ko.observable();
            this.TwelveIncludeIndexDate8 = ko.observable();
            this.TwelveIncludeIndexDate9 = ko.observable();
            this.TwelveIncludeIndexDate10 = ko.observable();
            this.TwelveIncludeIndexDate11 = ko.observable();
            this.TwelveIncludeIndexDate12 = ko.observable();
            this.CareSettingsToDefineMedicalVisits1 = ko.observable();
            this.CareSettingsToDefineMedicalVisits2 = ko.observable();
            this.CareSettingsToDefineMedicalVisits3 = ko.observable();
            this.CareSettingsToDefineMedicalVisits4 = ko.observable();
            this.CareSettingsToDefineMedicalVisits5 = ko.observable();
            this.CareSettingsToDefineMedicalVisits6 = ko.observable();
            this.CareSettingsToDefineMedicalVisits7 = ko.observable();
            this.CareSettingsToDefineMedicalVisits8 = ko.observable();
            this.CareSettingsToDefineMedicalVisits9 = ko.observable();
            this.CareSettingsToDefineMedicalVisits10 = ko.observable();
            this.CareSettingsToDefineMedicalVisits11 = ko.observable();
            this.CareSettingsToDefineMedicalVisits12 = ko.observable();
            this.TwelveStratificationCategories1 = ko.observable();
            this.TwelveStratificationCategories2 = ko.observable();
            this.TwelveStratificationCategories3 = ko.observable();
            this.TwelveStratificationCategories4 = ko.observable();
            this.TwelveStratificationCategories5 = ko.observable();
            this.TwelveStratificationCategories6 = ko.observable();
            this.TwelveStratificationCategories7 = ko.observable();
            this.TwelveStratificationCategories8 = ko.observable();
            this.TwelveStratificationCategories9 = ko.observable();
            this.TwelveStratificationCategories10 = ko.observable();
            this.TwelveStratificationCategories11 = ko.observable();
            this.TwelveStratificationCategories12 = ko.observable();
            this.VaryLengthOfWashoutPeriod1 = ko.observable();
            this.VaryLengthOfWashoutPeriod2 = ko.observable();
            this.VaryLengthOfWashoutPeriod3 = ko.observable();
            this.VaryLengthOfWashoutPeriod4 = ko.observable();
            this.VaryLengthOfWashoutPeriod5 = ko.observable();
            this.VaryLengthOfWashoutPeriod6 = ko.observable();
            this.VaryLengthOfWashoutPeriod7 = ko.observable();
            this.VaryLengthOfWashoutPeriod8 = ko.observable();
            this.VaryLengthOfWashoutPeriod9 = ko.observable();
            this.VaryLengthOfWashoutPeriod10 = ko.observable();
            this.VaryLengthOfWashoutPeriod11 = ko.observable();
            this.VaryLengthOfWashoutPeriod12 = ko.observable();
            this.VaryUserExposedTime1 = ko.observable();
            this.VaryUserExposedTime2 = ko.observable();
            this.VaryUserExposedTime3 = ko.observable();
            this.VaryUserExposedTime4 = ko.observable();
            this.VaryUserExposedTime5 = ko.observable();
            this.VaryUserExposedTime6 = ko.observable();
            this.VaryUserExposedTime7 = ko.observable();
            this.VaryUserExposedTime8 = ko.observable();
            this.VaryUserExposedTime9 = ko.observable();
            this.VaryUserExposedTime10 = ko.observable();
            this.VaryUserExposedTime11 = ko.observable();
            this.VaryUserExposedTime12 = ko.observable();
            this.VaryUserFollowupPeriodDuration1 = ko.observable();
            this.VaryUserFollowupPeriodDuration2 = ko.observable();
            this.VaryUserFollowupPeriodDuration3 = ko.observable();
            this.VaryUserFollowupPeriodDuration4 = ko.observable();
            this.VaryUserFollowupPeriodDuration5 = ko.observable();
            this.VaryUserFollowupPeriodDuration6 = ko.observable();
            this.VaryUserFollowupPeriodDuration7 = ko.observable();
            this.VaryUserFollowupPeriodDuration8 = ko.observable();
            this.VaryUserFollowupPeriodDuration9 = ko.observable();
            this.VaryUserFollowupPeriodDuration10 = ko.observable();
            this.VaryUserFollowupPeriodDuration11 = ko.observable();
            this.VaryUserFollowupPeriodDuration12 = ko.observable();
            this.VaryBlackoutPeriodPeriod1 = ko.observable();
            this.VaryBlackoutPeriodPeriod2 = ko.observable();
            this.VaryBlackoutPeriodPeriod3 = ko.observable();
            this.VaryBlackoutPeriodPeriod4 = ko.observable();
            this.VaryBlackoutPeriodPeriod5 = ko.observable();
            this.VaryBlackoutPeriodPeriod6 = ko.observable();
            this.VaryBlackoutPeriodPeriod7 = ko.observable();
            this.VaryBlackoutPeriodPeriod8 = ko.observable();
            this.VaryBlackoutPeriodPeriod9 = ko.observable();
            this.VaryBlackoutPeriodPeriod10 = ko.observable();
            this.VaryBlackoutPeriodPeriod11 = ko.observable();
            this.VaryBlackoutPeriodPeriod12 = ko.observable();
            this.Level2or3DefineExposures1Exposure = ko.observable();
            this.Level2or3DefineExposures1Compare = ko.observable();
            this.Level2or3DefineExposures2Exposure = ko.observable();
            this.Level2or3DefineExposures2Compare = ko.observable();
            this.Level2or3DefineExposures3Exposure = ko.observable();
            this.Level2or3DefineExposures3Compare = ko.observable();
            this.Level2or3WashoutPeriod1Exposure = ko.observable();
            this.Level2or3WashoutPeriod1Compare = ko.observable();
            this.Level2or3WashoutPeriod2Exposure = ko.observable();
            this.Level2or3WashoutPeriod2Compare = ko.observable();
            this.Level2or3WashoutPeriod3Exposure = ko.observable();
            this.Level2or3WashoutPeriod3Compare = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment1Exposure = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment1Compare = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment2Exposure = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment2Compare = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment3Exposure = ko.observable();
            this.Level2or3SpecifyExposedTimeAssessment3Compare = ko.observable();
            this.Level2or3EpisodeAllowableGap1Exposure = ko.observable();
            this.Level2or3EpisodeAllowableGap1Compare = ko.observable();
            this.Level2or3EpisodeAllowableGap2Exposure = ko.observable();
            this.Level2or3EpisodeAllowableGap2Compare = ko.observable();
            this.Level2or3EpisodeAllowableGap3Exposure = ko.observable();
            this.Level2or3EpisodeAllowableGap3Compare = ko.observable();
            this.Level2or3EpisodeExtensionPeriod1Exposure = ko.observable();
            this.Level2or3EpisodeExtensionPeriod1Compare = ko.observable();
            this.Level2or3EpisodeExtensionPeriod2Exposure = ko.observable();
            this.Level2or3EpisodeExtensionPeriod2Compare = ko.observable();
            this.Level2or3EpisodeExtensionPeriod3Exposure = ko.observable();
            this.Level2or3EpisodeExtensionPeriod3Compare = ko.observable();
            this.Level2or3MinimumEpisodeDuration1Exposure = ko.observable();
            this.Level2or3MinimumEpisodeDuration1Compare = ko.observable();
            this.Level2or3MinimumEpisodeDuration2Exposure = ko.observable();
            this.Level2or3MinimumEpisodeDuration2Compare = ko.observable();
            this.Level2or3MinimumEpisodeDuration3Exposure = ko.observable();
            this.Level2or3MinimumEpisodeDuration3Compare = ko.observable();
            this.Level2or3MinimumDaysSupply1Exposure = ko.observable();
            this.Level2or3MinimumDaysSupply1Compare = ko.observable();
            this.Level2or3MinimumDaysSupply2Exposure = ko.observable();
            this.Level2or3MinimumDaysSupply2Compare = ko.observable();
            this.Level2or3MinimumDaysSupply3Exposure = ko.observable();
            this.Level2or3MinimumDaysSupply3Compare = ko.observable();
            this.Level2or3SpecifyFollowUpDuration1Exposure = ko.observable();
            this.Level2or3SpecifyFollowUpDuration1Compare = ko.observable();
            this.Level2or3SpecifyFollowUpDuration2Exposure = ko.observable();
            this.Level2or3SpecifyFollowUpDuration2Compare = ko.observable();
            this.Level2or3SpecifyFollowUpDuration3Exposure = ko.observable();
            this.Level2or3SpecifyFollowUpDuration3Compare = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure = ko.observable();
            this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare = ko.observable();
            this.Level2or3TruncateExposedtime1Exposure = ko.observable();
            this.Level2or3TruncateExposedtime1Compare = ko.observable();
            this.Level2or3TruncateExposedtime2Exposure = ko.observable();
            this.Level2or3TruncateExposedtime2Compare = ko.observable();
            this.Level2or3TruncateExposedtime3Exposure = ko.observable();
            this.Level2or3TruncateExposedtime3Compare = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified1Exposure = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified1Compare = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified2Exposure = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified2Compare = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified3Exposure = ko.observable();
            this.Level2or3TruncateExposedTimeSpecified3Compare = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod1Exposure = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod1Compare = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod2Exposure = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod2Compare = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod3Exposure = ko.observable();
            this.Level2or3SpecifyBlackoutPeriod3Compare = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable();
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable();
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod1Exposure = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod1Compare = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod2Exposure = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod2Compare = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod3Exposure = ko.observable();
            this.Level2or3VaryLengthOfWashoutPeriod3Compare = ko.observable();
            this.Level2or3VaryUserExposedTime1Exposure = ko.observable();
            this.Level2or3VaryUserExposedTime1Compare = ko.observable();
            this.Level2or3VaryUserExposedTime2Exposure = ko.observable();
            this.Level2or3VaryUserExposedTime2Compare = ko.observable();
            this.Level2or3VaryUserExposedTime3Exposure = ko.observable();
            this.Level2or3VaryUserExposedTime3Compare = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod1Exposure = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod1Compare = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod2Exposure = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod2Compare = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod3Exposure = ko.observable();
            this.Level2or3VaryBlackoutPeriodPeriod3Compare = ko.observable();
            this.OutcomeList = ko.observableArray();
            this.AgeCovariate = ko.observable();
            this.SexCovariate = ko.observable();
            this.TimeCovariate = ko.observable();
            this.YearCovariate = ko.observable();
            this.ComorbidityCovariate = ko.observable();
            this.HealthCovariate = ko.observable();
            this.DrugCovariate = ko.observable();
            this.CovariateList = ko.observableArray();
            this.hdPSAnalysis = ko.observable();
            this.InclusionCovariates = ko.observable();
            this.PoolCovariates = ko.observable();
            this.SelectionCovariates = ko.observable();
            this.ZeroCellCorrection = ko.observable();
            this.MatchingRatio = ko.observable();
            this.MatchingCalipers = ko.observable();
            this.VaryMatchingRatio = ko.observable();
            this.VaryMatchingCalipers = ko.observable();
        }
        else {
            this.RequestDueDate = ko.observable(RequestFormDTO.RequestDueDate);
            this.ContactInfo = ko.observable(RequestFormDTO.ContactInfo);
            this.RequestingTeam = ko.observable(RequestFormDTO.RequestingTeam);
            this.FDAReview = ko.observable(RequestFormDTO.FDAReview);
            this.FDADivisionNA = ko.observable(RequestFormDTO.FDADivisionNA);
            this.FDADivisionDAAAP = ko.observable(RequestFormDTO.FDADivisionDAAAP);
            this.FDADivisionDBRUP = ko.observable(RequestFormDTO.FDADivisionDBRUP);
            this.FDADivisionDCARP = ko.observable(RequestFormDTO.FDADivisionDCARP);
            this.FDADivisionDDDP = ko.observable(RequestFormDTO.FDADivisionDDDP);
            this.FDADivisionDGIEP = ko.observable(RequestFormDTO.FDADivisionDGIEP);
            this.FDADivisionDMIP = ko.observable(RequestFormDTO.FDADivisionDMIP);
            this.FDADivisionDMEP = ko.observable(RequestFormDTO.FDADivisionDMEP);
            this.FDADivisionDNP = ko.observable(RequestFormDTO.FDADivisionDNP);
            this.FDADivisionDDP = ko.observable(RequestFormDTO.FDADivisionDDP);
            this.FDADivisionDPARP = ko.observable(RequestFormDTO.FDADivisionDPARP);
            this.FDADivisionOther = ko.observable(RequestFormDTO.FDADivisionOther);
            this.QueryLevel = ko.observable(RequestFormDTO.QueryLevel);
            this.AdjustmentMethod = ko.observable(RequestFormDTO.AdjustmentMethod);
            this.CohortID = ko.observable(RequestFormDTO.CohortID);
            this.StudyObjectives = ko.observable(RequestFormDTO.StudyObjectives);
            this.RequestStartDate = ko.observable(RequestFormDTO.RequestStartDate);
            this.RequestEndDate = ko.observable(RequestFormDTO.RequestEndDate);
            this.AgeGroups = ko.observable(RequestFormDTO.AgeGroups);
            this.CoverageTypes = ko.observable(RequestFormDTO.CoverageTypes);
            this.EnrollmentGap = ko.observable(RequestFormDTO.EnrollmentGap);
            this.EnrollmentExposure = ko.observable(RequestFormDTO.EnrollmentExposure);
            this.DefineExposures = ko.observable(RequestFormDTO.DefineExposures);
            this.WashoutPeirod = ko.observable(RequestFormDTO.WashoutPeirod);
            this.OtherExposures = ko.observable(RequestFormDTO.OtherExposures);
            this.OneOrManyExposures = ko.observable(RequestFormDTO.OneOrManyExposures);
            this.AdditionalInclusion = ko.observable(RequestFormDTO.AdditionalInclusion);
            this.AdditionalInclusionEvaluation = ko.observable(RequestFormDTO.AdditionalInclusionEvaluation);
            this.AdditionalExclusion = ko.observable(RequestFormDTO.AdditionalExclusion);
            this.AdditionalExclusionEvaluation = ko.observable(RequestFormDTO.AdditionalExclusionEvaluation);
            this.VaryWashoutPeirod = ko.observable(RequestFormDTO.VaryWashoutPeirod);
            this.VaryExposures = ko.observable(RequestFormDTO.VaryExposures);
            this.DefineExposures1 = ko.observable(RequestFormDTO.DefineExposures1);
            this.DefineExposures2 = ko.observable(RequestFormDTO.DefineExposures2);
            this.DefineExposures3 = ko.observable(RequestFormDTO.DefineExposures3);
            this.DefineExposures4 = ko.observable(RequestFormDTO.DefineExposures4);
            this.DefineExposures5 = ko.observable(RequestFormDTO.DefineExposures5);
            this.DefineExposures6 = ko.observable(RequestFormDTO.DefineExposures6);
            this.DefineExposures7 = ko.observable(RequestFormDTO.DefineExposures7);
            this.DefineExposures8 = ko.observable(RequestFormDTO.DefineExposures8);
            this.DefineExposures9 = ko.observable(RequestFormDTO.DefineExposures9);
            this.DefineExposures10 = ko.observable(RequestFormDTO.DefineExposures10);
            this.DefineExposures11 = ko.observable(RequestFormDTO.DefineExposures11);
            this.DefineExposures12 = ko.observable(RequestFormDTO.DefineExposures12);
            this.WashoutPeriod1 = ko.observable(RequestFormDTO.WashoutPeriod1);
            this.WashoutPeriod2 = ko.observable(RequestFormDTO.WashoutPeriod2);
            this.WashoutPeriod3 = ko.observable(RequestFormDTO.WashoutPeriod3);
            this.WashoutPeriod4 = ko.observable(RequestFormDTO.WashoutPeriod4);
            this.WashoutPeriod5 = ko.observable(RequestFormDTO.WashoutPeriod5);
            this.WashoutPeriod6 = ko.observable(RequestFormDTO.WashoutPeriod6);
            this.WashoutPeriod7 = ko.observable(RequestFormDTO.WashoutPeriod7);
            this.WashoutPeriod8 = ko.observable(RequestFormDTO.WashoutPeriod8);
            this.WashoutPeriod9 = ko.observable(RequestFormDTO.WashoutPeriod9);
            this.WashoutPeriod10 = ko.observable(RequestFormDTO.WashoutPeriod10);
            this.WashoutPeriod11 = ko.observable(RequestFormDTO.WashoutPeriod11);
            this.WashoutPeriod12 = ko.observable(RequestFormDTO.WashoutPeriod12);
            this.IncidenceRefinement1 = ko.observable(RequestFormDTO.IncidenceRefinement1);
            this.IncidenceRefinement2 = ko.observable(RequestFormDTO.IncidenceRefinement2);
            this.IncidenceRefinement3 = ko.observable(RequestFormDTO.IncidenceRefinement3);
            this.IncidenceRefinement4 = ko.observable(RequestFormDTO.IncidenceRefinement4);
            this.IncidenceRefinement5 = ko.observable(RequestFormDTO.IncidenceRefinement5);
            this.IncidenceRefinement6 = ko.observable(RequestFormDTO.IncidenceRefinement6);
            this.IncidenceRefinement7 = ko.observable(RequestFormDTO.IncidenceRefinement7);
            this.IncidenceRefinement8 = ko.observable(RequestFormDTO.IncidenceRefinement8);
            this.IncidenceRefinement9 = ko.observable(RequestFormDTO.IncidenceRefinement9);
            this.IncidenceRefinement10 = ko.observable(RequestFormDTO.IncidenceRefinement10);
            this.IncidenceRefinement11 = ko.observable(RequestFormDTO.IncidenceRefinement11);
            this.IncidenceRefinement12 = ko.observable(RequestFormDTO.IncidenceRefinement12);
            this.SpecifyExposedTimeAssessment1 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment1);
            this.SpecifyExposedTimeAssessment2 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment2);
            this.SpecifyExposedTimeAssessment3 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment3);
            this.SpecifyExposedTimeAssessment4 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment4);
            this.SpecifyExposedTimeAssessment5 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment5);
            this.SpecifyExposedTimeAssessment6 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment6);
            this.SpecifyExposedTimeAssessment7 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment7);
            this.SpecifyExposedTimeAssessment8 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment8);
            this.SpecifyExposedTimeAssessment9 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment9);
            this.SpecifyExposedTimeAssessment10 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment10);
            this.SpecifyExposedTimeAssessment11 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment11);
            this.SpecifyExposedTimeAssessment12 = ko.observable(RequestFormDTO.SpecifyExposedTimeAssessment12);
            this.EpisodeAllowableGap1 = ko.observable(RequestFormDTO.EpisodeAllowableGap1);
            this.EpisodeAllowableGap2 = ko.observable(RequestFormDTO.EpisodeAllowableGap2);
            this.EpisodeAllowableGap3 = ko.observable(RequestFormDTO.EpisodeAllowableGap3);
            this.EpisodeAllowableGap4 = ko.observable(RequestFormDTO.EpisodeAllowableGap4);
            this.EpisodeAllowableGap5 = ko.observable(RequestFormDTO.EpisodeAllowableGap5);
            this.EpisodeAllowableGap6 = ko.observable(RequestFormDTO.EpisodeAllowableGap6);
            this.EpisodeAllowableGap7 = ko.observable(RequestFormDTO.EpisodeAllowableGap7);
            this.EpisodeAllowableGap8 = ko.observable(RequestFormDTO.EpisodeAllowableGap8);
            this.EpisodeAllowableGap9 = ko.observable(RequestFormDTO.EpisodeAllowableGap9);
            this.EpisodeAllowableGap10 = ko.observable(RequestFormDTO.EpisodeAllowableGap10);
            this.EpisodeAllowableGap11 = ko.observable(RequestFormDTO.EpisodeAllowableGap11);
            this.EpisodeAllowableGap12 = ko.observable(RequestFormDTO.EpisodeAllowableGap12);
            this.EpisodeExtensionPeriod1 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod1);
            this.EpisodeExtensionPeriod2 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod2);
            this.EpisodeExtensionPeriod3 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod3);
            this.EpisodeExtensionPeriod4 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod4);
            this.EpisodeExtensionPeriod5 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod5);
            this.EpisodeExtensionPeriod6 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod6);
            this.EpisodeExtensionPeriod7 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod7);
            this.EpisodeExtensionPeriod8 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod8);
            this.EpisodeExtensionPeriod9 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod9);
            this.EpisodeExtensionPeriod10 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod10);
            this.EpisodeExtensionPeriod11 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod11);
            this.EpisodeExtensionPeriod12 = ko.observable(RequestFormDTO.EpisodeExtensionPeriod12);
            this.MinimumEpisodeDuration1 = ko.observable(RequestFormDTO.MinimumEpisodeDuration1);
            this.MinimumEpisodeDuration2 = ko.observable(RequestFormDTO.MinimumEpisodeDuration2);
            this.MinimumEpisodeDuration3 = ko.observable(RequestFormDTO.MinimumEpisodeDuration3);
            this.MinimumEpisodeDuration4 = ko.observable(RequestFormDTO.MinimumEpisodeDuration4);
            this.MinimumEpisodeDuration5 = ko.observable(RequestFormDTO.MinimumEpisodeDuration5);
            this.MinimumEpisodeDuration6 = ko.observable(RequestFormDTO.MinimumEpisodeDuration6);
            this.MinimumEpisodeDuration7 = ko.observable(RequestFormDTO.MinimumEpisodeDuration7);
            this.MinimumEpisodeDuration8 = ko.observable(RequestFormDTO.MinimumEpisodeDuration8);
            this.MinimumEpisodeDuration9 = ko.observable(RequestFormDTO.MinimumEpisodeDuration9);
            this.MinimumEpisodeDuration10 = ko.observable(RequestFormDTO.MinimumEpisodeDuration10);
            this.MinimumEpisodeDuration11 = ko.observable(RequestFormDTO.MinimumEpisodeDuration11);
            this.MinimumEpisodeDuration12 = ko.observable(RequestFormDTO.MinimumEpisodeDuration12);
            this.MinimumDaysSupply1 = ko.observable(RequestFormDTO.MinimumDaysSupply1);
            this.MinimumDaysSupply2 = ko.observable(RequestFormDTO.MinimumDaysSupply2);
            this.MinimumDaysSupply3 = ko.observable(RequestFormDTO.MinimumDaysSupply3);
            this.MinimumDaysSupply4 = ko.observable(RequestFormDTO.MinimumDaysSupply4);
            this.MinimumDaysSupply5 = ko.observable(RequestFormDTO.MinimumDaysSupply5);
            this.MinimumDaysSupply6 = ko.observable(RequestFormDTO.MinimumDaysSupply6);
            this.MinimumDaysSupply7 = ko.observable(RequestFormDTO.MinimumDaysSupply7);
            this.MinimumDaysSupply8 = ko.observable(RequestFormDTO.MinimumDaysSupply8);
            this.MinimumDaysSupply9 = ko.observable(RequestFormDTO.MinimumDaysSupply9);
            this.MinimumDaysSupply10 = ko.observable(RequestFormDTO.MinimumDaysSupply10);
            this.MinimumDaysSupply11 = ko.observable(RequestFormDTO.MinimumDaysSupply11);
            this.MinimumDaysSupply12 = ko.observable(RequestFormDTO.MinimumDaysSupply12);
            this.SpecifyFollowUpDuration1 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration1);
            this.SpecifyFollowUpDuration2 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration2);
            this.SpecifyFollowUpDuration3 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration3);
            this.SpecifyFollowUpDuration4 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration4);
            this.SpecifyFollowUpDuration5 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration5);
            this.SpecifyFollowUpDuration6 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration6);
            this.SpecifyFollowUpDuration7 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration7);
            this.SpecifyFollowUpDuration8 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration8);
            this.SpecifyFollowUpDuration9 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration9);
            this.SpecifyFollowUpDuration10 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration10);
            this.SpecifyFollowUpDuration11 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration11);
            this.SpecifyFollowUpDuration12 = ko.observable(RequestFormDTO.SpecifyFollowUpDuration12);
            this.AllowOnOrMultipleExposureEpisodes1 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes1);
            this.AllowOnOrMultipleExposureEpisodes2 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes2);
            this.AllowOnOrMultipleExposureEpisodes3 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes3);
            this.AllowOnOrMultipleExposureEpisodes4 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes4);
            this.AllowOnOrMultipleExposureEpisodes5 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes5);
            this.AllowOnOrMultipleExposureEpisodes6 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes6);
            this.AllowOnOrMultipleExposureEpisodes7 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes7);
            this.AllowOnOrMultipleExposureEpisodes8 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes8);
            this.AllowOnOrMultipleExposureEpisodes9 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes9);
            this.AllowOnOrMultipleExposureEpisodes10 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes10);
            this.AllowOnOrMultipleExposureEpisodes11 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes11);
            this.AllowOnOrMultipleExposureEpisodes12 = ko.observable(RequestFormDTO.AllowOnOrMultipleExposureEpisodes12);
            this.TruncateExposedtime1 = ko.observable(RequestFormDTO.TruncateExposedtime1);
            this.TruncateExposedtime2 = ko.observable(RequestFormDTO.TruncateExposedtime2);
            this.TruncateExposedtime3 = ko.observable(RequestFormDTO.TruncateExposedtime3);
            this.TruncateExposedtime4 = ko.observable(RequestFormDTO.TruncateExposedtime4);
            this.TruncateExposedtime5 = ko.observable(RequestFormDTO.TruncateExposedtime5);
            this.TruncateExposedtime6 = ko.observable(RequestFormDTO.TruncateExposedtime6);
            this.TruncateExposedtime7 = ko.observable(RequestFormDTO.TruncateExposedtime7);
            this.TruncateExposedtime8 = ko.observable(RequestFormDTO.TruncateExposedtime8);
            this.TruncateExposedtime9 = ko.observable(RequestFormDTO.TruncateExposedtime9);
            this.TruncateExposedtime10 = ko.observable(RequestFormDTO.TruncateExposedtime10);
            this.TruncateExposedtime11 = ko.observable(RequestFormDTO.TruncateExposedtime11);
            this.TruncateExposedtime12 = ko.observable(RequestFormDTO.TruncateExposedtime12);
            this.TruncateExposedTimeSpecified1 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified1);
            this.TruncateExposedTimeSpecified2 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified2);
            this.TruncateExposedTimeSpecified3 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified3);
            this.TruncateExposedTimeSpecified4 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified4);
            this.TruncateExposedTimeSpecified5 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified5);
            this.TruncateExposedTimeSpecified6 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified6);
            this.TruncateExposedTimeSpecified7 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified7);
            this.TruncateExposedTimeSpecified8 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified8);
            this.TruncateExposedTimeSpecified9 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified9);
            this.TruncateExposedTimeSpecified10 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified10);
            this.TruncateExposedTimeSpecified11 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified11);
            this.TruncateExposedTimeSpecified12 = ko.observable(RequestFormDTO.TruncateExposedTimeSpecified12);
            this.SpecifyBlackoutPeriod1 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod1);
            this.SpecifyBlackoutPeriod2 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod2);
            this.SpecifyBlackoutPeriod3 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod3);
            this.SpecifyBlackoutPeriod4 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod4);
            this.SpecifyBlackoutPeriod5 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod5);
            this.SpecifyBlackoutPeriod6 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod6);
            this.SpecifyBlackoutPeriod7 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod7);
            this.SpecifyBlackoutPeriod8 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod8);
            this.SpecifyBlackoutPeriod9 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod9);
            this.SpecifyBlackoutPeriod10 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod10);
            this.SpecifyBlackoutPeriod11 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod11);
            this.SpecifyBlackoutPeriod12 = ko.observable(RequestFormDTO.SpecifyBlackoutPeriod12);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup11);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup12);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup13);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup14);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup15);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup16);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup11);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup12);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup13);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup14);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup15);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup16);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup21);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup22);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup23);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup24);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup25);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup26);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup21);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup22);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup23);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup24);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup25);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup26);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup31);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup32);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup33);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup34);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup35);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup36);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup31);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup32);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup33);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup34);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup35);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup36);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup41);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup42);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup43);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup44);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup45);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup46);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup41);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup42);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup43);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup44);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup45);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup46);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup51);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup52);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup53);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup54);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup55);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup56);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup51);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup52);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup53);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup54);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup55);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup56);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup61);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup62);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup63);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup64);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup65);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup66);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup61);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup62);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup63);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup64);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup65);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup66);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup71);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup72);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup73);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup74);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup75);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup76);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup71);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup72);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup73);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup74);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup75);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup76);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup81);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup82);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup83);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup84);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup85);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup86);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup81);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup82);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup83);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup84);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup85);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup86);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup91);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup92);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup93);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup94);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup95);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup96);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup91);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup92);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup93);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup94);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup95);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup96);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup101);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup102);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup103);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup104);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup105);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup106);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup101);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup102);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup103);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup104);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup105);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup106);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup111);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup112);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup113);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup114);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup115);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup116);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup111);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup112);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup113);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup114);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup115);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup116);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup121);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup122);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup123);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup124);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup125);
            this.SpecifyAdditionalInclusionInclusionCriteriaGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionInclusionCriteriaGroup126);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup121);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup122);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup123);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup124);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup125);
            this.SpecifyAdditionalInclusionEvaluationWindowGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalInclusionEvaluationWindowGroup126);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup11);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup12);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup13);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup14);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup15);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup16);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup11);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup12);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup13);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup14 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup14);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup15 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup15);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup16 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup16);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup21);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup22);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup23);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup24);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup25);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup26);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup21);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup22);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup23);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup24 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup24);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup25 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup25);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup26 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup26);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup31);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup32);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup33);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup34);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup35);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup36);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup31);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup32);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup33);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup34 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup34);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup35 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup35);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup36 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup36);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup41);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup42);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup43);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup44);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup45);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup46);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup41);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup42);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup43);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup44 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup44);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup45 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup45);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup46 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup46);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup51);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup52);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup53);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup54);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup55);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup56);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup51);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup52);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup53);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup54 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup54);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup55 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup55);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup56 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup56);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup61);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup62);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup63);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup64);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup65);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup66);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup61);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup62);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup63);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup64 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup64);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup65 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup65);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup66 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup66);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup71);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup72);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup73);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup74);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup75);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup76);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup71 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup71);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup72 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup72);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup73 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup73);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup74 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup74);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup75 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup75);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup76 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup76);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup81);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup82);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup83);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup84);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup85);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup86);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup81 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup81);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup82 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup82);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup83 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup83);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup84 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup84);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup85 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup85);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup86 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup86);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup91);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup92);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup93);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup94);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup95);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup96);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup91 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup91);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup92 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup92);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup93 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup93);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup94 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup94);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup95 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup95);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup96 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup96);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup101);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup102);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup103);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup104);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup105);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup106);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup101 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup101);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup102 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup102);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup103 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup103);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup104 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup104);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup105 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup105);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup106 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup106);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup111);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup112);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup113);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup114);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup115);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup116);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup111 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup111);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup112 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup112);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup113 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup113);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup114 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup114);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup115 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup115);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup116 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup116);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup121);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup122);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup123);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup124);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup125);
            this.SpecifyAdditionalExclusionInclusionCriteriaGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionInclusionCriteriaGroup126);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup121 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup121);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup122 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup122);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup123 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup123);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup124 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup124);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup125 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup125);
            this.SpecifyAdditionalExclusionEvaluationWindowGroup126 = ko.observable(RequestFormDTO.SpecifyAdditionalExclusionEvaluationWindowGroup126);
            this.LookBackPeriodGroup1 = ko.observable(RequestFormDTO.LookBackPeriodGroup1);
            this.LookBackPeriodGroup2 = ko.observable(RequestFormDTO.LookBackPeriodGroup2);
            this.LookBackPeriodGroup3 = ko.observable(RequestFormDTO.LookBackPeriodGroup3);
            this.LookBackPeriodGroup4 = ko.observable(RequestFormDTO.LookBackPeriodGroup4);
            this.LookBackPeriodGroup5 = ko.observable(RequestFormDTO.LookBackPeriodGroup5);
            this.LookBackPeriodGroup6 = ko.observable(RequestFormDTO.LookBackPeriodGroup6);
            this.LookBackPeriodGroup7 = ko.observable(RequestFormDTO.LookBackPeriodGroup7);
            this.LookBackPeriodGroup8 = ko.observable(RequestFormDTO.LookBackPeriodGroup8);
            this.LookBackPeriodGroup9 = ko.observable(RequestFormDTO.LookBackPeriodGroup9);
            this.LookBackPeriodGroup10 = ko.observable(RequestFormDTO.LookBackPeriodGroup10);
            this.LookBackPeriodGroup11 = ko.observable(RequestFormDTO.LookBackPeriodGroup11);
            this.LookBackPeriodGroup12 = ko.observable(RequestFormDTO.LookBackPeriodGroup12);
            this.IncludeIndexDate1 = ko.observable(RequestFormDTO.IncludeIndexDate1);
            this.IncludeIndexDate2 = ko.observable(RequestFormDTO.IncludeIndexDate2);
            this.IncludeIndexDate3 = ko.observable(RequestFormDTO.IncludeIndexDate3);
            this.IncludeIndexDate4 = ko.observable(RequestFormDTO.IncludeIndexDate4);
            this.IncludeIndexDate5 = ko.observable(RequestFormDTO.IncludeIndexDate5);
            this.IncludeIndexDate6 = ko.observable(RequestFormDTO.IncludeIndexDate6);
            this.IncludeIndexDate7 = ko.observable(RequestFormDTO.IncludeIndexDate7);
            this.IncludeIndexDate8 = ko.observable(RequestFormDTO.IncludeIndexDate8);
            this.IncludeIndexDate9 = ko.observable(RequestFormDTO.IncludeIndexDate9);
            this.IncludeIndexDate10 = ko.observable(RequestFormDTO.IncludeIndexDate10);
            this.IncludeIndexDate11 = ko.observable(RequestFormDTO.IncludeIndexDate11);
            this.IncludeIndexDate12 = ko.observable(RequestFormDTO.IncludeIndexDate12);
            this.StratificationCategories1 = ko.observable(RequestFormDTO.StratificationCategories1);
            this.StratificationCategories2 = ko.observable(RequestFormDTO.StratificationCategories2);
            this.StratificationCategories3 = ko.observable(RequestFormDTO.StratificationCategories3);
            this.StratificationCategories4 = ko.observable(RequestFormDTO.StratificationCategories4);
            this.StratificationCategories5 = ko.observable(RequestFormDTO.StratificationCategories5);
            this.StratificationCategories6 = ko.observable(RequestFormDTO.StratificationCategories6);
            this.StratificationCategories7 = ko.observable(RequestFormDTO.StratificationCategories7);
            this.StratificationCategories8 = ko.observable(RequestFormDTO.StratificationCategories8);
            this.StratificationCategories9 = ko.observable(RequestFormDTO.StratificationCategories9);
            this.StratificationCategories10 = ko.observable(RequestFormDTO.StratificationCategories10);
            this.StratificationCategories11 = ko.observable(RequestFormDTO.StratificationCategories11);
            this.StratificationCategories12 = ko.observable(RequestFormDTO.StratificationCategories12);
            this.TwelveSpecifyLoopBackPeriod1 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod1);
            this.TwelveSpecifyLoopBackPeriod2 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod2);
            this.TwelveSpecifyLoopBackPeriod3 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod3);
            this.TwelveSpecifyLoopBackPeriod4 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod4);
            this.TwelveSpecifyLoopBackPeriod5 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod5);
            this.TwelveSpecifyLoopBackPeriod6 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod6);
            this.TwelveSpecifyLoopBackPeriod7 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod7);
            this.TwelveSpecifyLoopBackPeriod8 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod8);
            this.TwelveSpecifyLoopBackPeriod9 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod9);
            this.TwelveSpecifyLoopBackPeriod10 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod10);
            this.TwelveSpecifyLoopBackPeriod11 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod11);
            this.TwelveSpecifyLoopBackPeriod12 = ko.observable(RequestFormDTO.TwelveSpecifyLoopBackPeriod12);
            this.TwelveIncludeIndexDate1 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate1);
            this.TwelveIncludeIndexDate2 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate2);
            this.TwelveIncludeIndexDate3 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate3);
            this.TwelveIncludeIndexDate4 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate4);
            this.TwelveIncludeIndexDate5 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate5);
            this.TwelveIncludeIndexDate6 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate6);
            this.TwelveIncludeIndexDate7 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate7);
            this.TwelveIncludeIndexDate8 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate8);
            this.TwelveIncludeIndexDate9 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate9);
            this.TwelveIncludeIndexDate10 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate10);
            this.TwelveIncludeIndexDate11 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate11);
            this.TwelveIncludeIndexDate12 = ko.observable(RequestFormDTO.TwelveIncludeIndexDate12);
            this.CareSettingsToDefineMedicalVisits1 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits1);
            this.CareSettingsToDefineMedicalVisits2 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits2);
            this.CareSettingsToDefineMedicalVisits3 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits3);
            this.CareSettingsToDefineMedicalVisits4 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits4);
            this.CareSettingsToDefineMedicalVisits5 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits5);
            this.CareSettingsToDefineMedicalVisits6 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits6);
            this.CareSettingsToDefineMedicalVisits7 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits7);
            this.CareSettingsToDefineMedicalVisits8 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits8);
            this.CareSettingsToDefineMedicalVisits9 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits9);
            this.CareSettingsToDefineMedicalVisits10 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits10);
            this.CareSettingsToDefineMedicalVisits11 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits11);
            this.CareSettingsToDefineMedicalVisits12 = ko.observable(RequestFormDTO.CareSettingsToDefineMedicalVisits12);
            this.TwelveStratificationCategories1 = ko.observable(RequestFormDTO.TwelveStratificationCategories1);
            this.TwelveStratificationCategories2 = ko.observable(RequestFormDTO.TwelveStratificationCategories2);
            this.TwelveStratificationCategories3 = ko.observable(RequestFormDTO.TwelveStratificationCategories3);
            this.TwelveStratificationCategories4 = ko.observable(RequestFormDTO.TwelveStratificationCategories4);
            this.TwelveStratificationCategories5 = ko.observable(RequestFormDTO.TwelveStratificationCategories5);
            this.TwelveStratificationCategories6 = ko.observable(RequestFormDTO.TwelveStratificationCategories6);
            this.TwelveStratificationCategories7 = ko.observable(RequestFormDTO.TwelveStratificationCategories7);
            this.TwelveStratificationCategories8 = ko.observable(RequestFormDTO.TwelveStratificationCategories8);
            this.TwelveStratificationCategories9 = ko.observable(RequestFormDTO.TwelveStratificationCategories9);
            this.TwelveStratificationCategories10 = ko.observable(RequestFormDTO.TwelveStratificationCategories10);
            this.TwelveStratificationCategories11 = ko.observable(RequestFormDTO.TwelveStratificationCategories11);
            this.TwelveStratificationCategories12 = ko.observable(RequestFormDTO.TwelveStratificationCategories12);
            this.VaryLengthOfWashoutPeriod1 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod1);
            this.VaryLengthOfWashoutPeriod2 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod2);
            this.VaryLengthOfWashoutPeriod3 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod3);
            this.VaryLengthOfWashoutPeriod4 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod4);
            this.VaryLengthOfWashoutPeriod5 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod5);
            this.VaryLengthOfWashoutPeriod6 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod6);
            this.VaryLengthOfWashoutPeriod7 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod7);
            this.VaryLengthOfWashoutPeriod8 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod8);
            this.VaryLengthOfWashoutPeriod9 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod9);
            this.VaryLengthOfWashoutPeriod10 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod10);
            this.VaryLengthOfWashoutPeriod11 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod11);
            this.VaryLengthOfWashoutPeriod12 = ko.observable(RequestFormDTO.VaryLengthOfWashoutPeriod12);
            this.VaryUserExposedTime1 = ko.observable(RequestFormDTO.VaryUserExposedTime1);
            this.VaryUserExposedTime2 = ko.observable(RequestFormDTO.VaryUserExposedTime2);
            this.VaryUserExposedTime3 = ko.observable(RequestFormDTO.VaryUserExposedTime3);
            this.VaryUserExposedTime4 = ko.observable(RequestFormDTO.VaryUserExposedTime4);
            this.VaryUserExposedTime5 = ko.observable(RequestFormDTO.VaryUserExposedTime5);
            this.VaryUserExposedTime6 = ko.observable(RequestFormDTO.VaryUserExposedTime6);
            this.VaryUserExposedTime7 = ko.observable(RequestFormDTO.VaryUserExposedTime7);
            this.VaryUserExposedTime8 = ko.observable(RequestFormDTO.VaryUserExposedTime8);
            this.VaryUserExposedTime9 = ko.observable(RequestFormDTO.VaryUserExposedTime9);
            this.VaryUserExposedTime10 = ko.observable(RequestFormDTO.VaryUserExposedTime10);
            this.VaryUserExposedTime11 = ko.observable(RequestFormDTO.VaryUserExposedTime11);
            this.VaryUserExposedTime12 = ko.observable(RequestFormDTO.VaryUserExposedTime12);
            this.VaryUserFollowupPeriodDuration1 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration1);
            this.VaryUserFollowupPeriodDuration2 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration2);
            this.VaryUserFollowupPeriodDuration3 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration3);
            this.VaryUserFollowupPeriodDuration4 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration4);
            this.VaryUserFollowupPeriodDuration5 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration5);
            this.VaryUserFollowupPeriodDuration6 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration6);
            this.VaryUserFollowupPeriodDuration7 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration7);
            this.VaryUserFollowupPeriodDuration8 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration8);
            this.VaryUserFollowupPeriodDuration9 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration9);
            this.VaryUserFollowupPeriodDuration10 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration10);
            this.VaryUserFollowupPeriodDuration11 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration11);
            this.VaryUserFollowupPeriodDuration12 = ko.observable(RequestFormDTO.VaryUserFollowupPeriodDuration12);
            this.VaryBlackoutPeriodPeriod1 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod1);
            this.VaryBlackoutPeriodPeriod2 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod2);
            this.VaryBlackoutPeriodPeriod3 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod3);
            this.VaryBlackoutPeriodPeriod4 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod4);
            this.VaryBlackoutPeriodPeriod5 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod5);
            this.VaryBlackoutPeriodPeriod6 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod6);
            this.VaryBlackoutPeriodPeriod7 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod7);
            this.VaryBlackoutPeriodPeriod8 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod8);
            this.VaryBlackoutPeriodPeriod9 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod9);
            this.VaryBlackoutPeriodPeriod10 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod10);
            this.VaryBlackoutPeriodPeriod11 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod11);
            this.VaryBlackoutPeriodPeriod12 = ko.observable(RequestFormDTO.VaryBlackoutPeriodPeriod12);
            this.Level2or3DefineExposures1Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures1Exposure);
            this.Level2or3DefineExposures1Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures1Compare);
            this.Level2or3DefineExposures2Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures2Exposure);
            this.Level2or3DefineExposures2Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures2Compare);
            this.Level2or3DefineExposures3Exposure = ko.observable(RequestFormDTO.Level2or3DefineExposures3Exposure);
            this.Level2or3DefineExposures3Compare = ko.observable(RequestFormDTO.Level2or3DefineExposures3Compare);
            this.Level2or3WashoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod1Exposure);
            this.Level2or3WashoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod1Compare);
            this.Level2or3WashoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod2Exposure);
            this.Level2or3WashoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod2Compare);
            this.Level2or3WashoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3WashoutPeriod3Exposure);
            this.Level2or3WashoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3WashoutPeriod3Compare);
            this.Level2or3SpecifyExposedTimeAssessment1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment1Exposure);
            this.Level2or3SpecifyExposedTimeAssessment1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment1Compare);
            this.Level2or3SpecifyExposedTimeAssessment2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment2Exposure);
            this.Level2or3SpecifyExposedTimeAssessment2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment2Compare);
            this.Level2or3SpecifyExposedTimeAssessment3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment3Exposure);
            this.Level2or3SpecifyExposedTimeAssessment3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyExposedTimeAssessment3Compare);
            this.Level2or3EpisodeAllowableGap1Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap1Exposure);
            this.Level2or3EpisodeAllowableGap1Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap1Compare);
            this.Level2or3EpisodeAllowableGap2Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap2Exposure);
            this.Level2or3EpisodeAllowableGap2Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap2Compare);
            this.Level2or3EpisodeAllowableGap3Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap3Exposure);
            this.Level2or3EpisodeAllowableGap3Compare = ko.observable(RequestFormDTO.Level2or3EpisodeAllowableGap3Compare);
            this.Level2or3EpisodeExtensionPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod1Exposure);
            this.Level2or3EpisodeExtensionPeriod1Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod1Compare);
            this.Level2or3EpisodeExtensionPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod2Exposure);
            this.Level2or3EpisodeExtensionPeriod2Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod2Compare);
            this.Level2or3EpisodeExtensionPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod3Exposure);
            this.Level2or3EpisodeExtensionPeriod3Compare = ko.observable(RequestFormDTO.Level2or3EpisodeExtensionPeriod3Compare);
            this.Level2or3MinimumEpisodeDuration1Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration1Exposure);
            this.Level2or3MinimumEpisodeDuration1Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration1Compare);
            this.Level2or3MinimumEpisodeDuration2Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration2Exposure);
            this.Level2or3MinimumEpisodeDuration2Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration2Compare);
            this.Level2or3MinimumEpisodeDuration3Exposure = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration3Exposure);
            this.Level2or3MinimumEpisodeDuration3Compare = ko.observable(RequestFormDTO.Level2or3MinimumEpisodeDuration3Compare);
            this.Level2or3MinimumDaysSupply1Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply1Exposure);
            this.Level2or3MinimumDaysSupply1Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply1Compare);
            this.Level2or3MinimumDaysSupply2Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply2Exposure);
            this.Level2or3MinimumDaysSupply2Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply2Compare);
            this.Level2or3MinimumDaysSupply3Exposure = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply3Exposure);
            this.Level2or3MinimumDaysSupply3Compare = ko.observable(RequestFormDTO.Level2or3MinimumDaysSupply3Compare);
            this.Level2or3SpecifyFollowUpDuration1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration1Exposure);
            this.Level2or3SpecifyFollowUpDuration1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration1Compare);
            this.Level2or3SpecifyFollowUpDuration2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration2Exposure);
            this.Level2or3SpecifyFollowUpDuration2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration2Compare);
            this.Level2or3SpecifyFollowUpDuration3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration3Exposure);
            this.Level2or3SpecifyFollowUpDuration3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyFollowUpDuration3Compare);
            this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure);
            this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes1Compare);
            this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure);
            this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes2Compare);
            this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure);
            this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare = ko.observable(RequestFormDTO.Level2or3AllowOnOrMultipleExposureEpisodes3Compare);
            this.Level2or3TruncateExposedtime1Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime1Exposure);
            this.Level2or3TruncateExposedtime1Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime1Compare);
            this.Level2or3TruncateExposedtime2Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime2Exposure);
            this.Level2or3TruncateExposedtime2Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime2Compare);
            this.Level2or3TruncateExposedtime3Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime3Exposure);
            this.Level2or3TruncateExposedtime3Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedtime3Compare);
            this.Level2or3TruncateExposedTimeSpecified1Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified1Exposure);
            this.Level2or3TruncateExposedTimeSpecified1Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified1Compare);
            this.Level2or3TruncateExposedTimeSpecified2Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified2Exposure);
            this.Level2or3TruncateExposedTimeSpecified2Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified2Compare);
            this.Level2or3TruncateExposedTimeSpecified3Exposure = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified3Exposure);
            this.Level2or3TruncateExposedTimeSpecified3Compare = ko.observable(RequestFormDTO.Level2or3TruncateExposedTimeSpecified3Compare);
            this.Level2or3SpecifyBlackoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod1Exposure);
            this.Level2or3SpecifyBlackoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod1Compare);
            this.Level2or3SpecifyBlackoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod2Exposure);
            this.Level2or3SpecifyBlackoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod2Compare);
            this.Level2or3SpecifyBlackoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod3Exposure);
            this.Level2or3SpecifyBlackoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3SpecifyBlackoutPeriod3Compare);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62);
            this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62);
            this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62);
            this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62);
            this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable(RequestFormDTO.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63);
            this.Level2or3VaryLengthOfWashoutPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod1Exposure);
            this.Level2or3VaryLengthOfWashoutPeriod1Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod1Compare);
            this.Level2or3VaryLengthOfWashoutPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod2Exposure);
            this.Level2or3VaryLengthOfWashoutPeriod2Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod2Compare);
            this.Level2or3VaryLengthOfWashoutPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod3Exposure);
            this.Level2or3VaryLengthOfWashoutPeriod3Compare = ko.observable(RequestFormDTO.Level2or3VaryLengthOfWashoutPeriod3Compare);
            this.Level2or3VaryUserExposedTime1Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime1Exposure);
            this.Level2or3VaryUserExposedTime1Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime1Compare);
            this.Level2or3VaryUserExposedTime2Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime2Exposure);
            this.Level2or3VaryUserExposedTime2Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime2Compare);
            this.Level2or3VaryUserExposedTime3Exposure = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime3Exposure);
            this.Level2or3VaryUserExposedTime3Compare = ko.observable(RequestFormDTO.Level2or3VaryUserExposedTime3Compare);
            this.Level2or3VaryBlackoutPeriodPeriod1Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod1Exposure);
            this.Level2or3VaryBlackoutPeriodPeriod1Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod1Compare);
            this.Level2or3VaryBlackoutPeriodPeriod2Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod2Exposure);
            this.Level2or3VaryBlackoutPeriodPeriod2Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod2Compare);
            this.Level2or3VaryBlackoutPeriodPeriod3Exposure = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod3Exposure);
            this.Level2or3VaryBlackoutPeriodPeriod3Compare = ko.observable(RequestFormDTO.Level2or3VaryBlackoutPeriodPeriod3Compare);
            this.OutcomeList = ko.observableArray(RequestFormDTO.OutcomeList == null ? null : RequestFormDTO.OutcomeList.map((item) => { return new OutcomeItemViewModel(item); }));
            this.AgeCovariate = ko.observable(RequestFormDTO.AgeCovariate);
            this.SexCovariate = ko.observable(RequestFormDTO.SexCovariate);
            this.TimeCovariate = ko.observable(RequestFormDTO.TimeCovariate);
            this.YearCovariate = ko.observable(RequestFormDTO.YearCovariate);
            this.ComorbidityCovariate = ko.observable(RequestFormDTO.ComorbidityCovariate);
            this.HealthCovariate = ko.observable(RequestFormDTO.HealthCovariate);
            this.DrugCovariate = ko.observable(RequestFormDTO.DrugCovariate);
            this.CovariateList = ko.observableArray(RequestFormDTO.CovariateList == null ? null : RequestFormDTO.CovariateList.map((item) => { return new CovariateItemViewModel(item); }));
            this.hdPSAnalysis = ko.observable(RequestFormDTO.hdPSAnalysis);
            this.InclusionCovariates = ko.observable(RequestFormDTO.InclusionCovariates);
            this.PoolCovariates = ko.observable(RequestFormDTO.PoolCovariates);
            this.SelectionCovariates = ko.observable(RequestFormDTO.SelectionCovariates);
            this.ZeroCellCorrection = ko.observable(RequestFormDTO.ZeroCellCorrection);
            this.MatchingRatio = ko.observable(RequestFormDTO.MatchingRatio);
            this.MatchingCalipers = ko.observable(RequestFormDTO.MatchingCalipers);
            this.VaryMatchingRatio = ko.observable(RequestFormDTO.VaryMatchingRatio);
            this.VaryMatchingCalipers = ko.observable(RequestFormDTO.VaryMatchingCalipers);
        }
    }
    toData() {
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
            OutcomeList: this.OutcomeList == null ? null : this.OutcomeList().map((item) => { return item.toData(); }),
            AgeCovariate: this.AgeCovariate(),
            SexCovariate: this.SexCovariate(),
            TimeCovariate: this.TimeCovariate(),
            YearCovariate: this.YearCovariate(),
            ComorbidityCovariate: this.ComorbidityCovariate(),
            HealthCovariate: this.HealthCovariate(),
            DrugCovariate: this.DrugCovariate(),
            CovariateList: this.CovariateList == null ? null : this.CovariateList().map((item) => { return item.toData(); }),
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
    }
}
export class OutcomeItemViewModel extends ViewModel {
    CommonName;
    Outcome;
    WashoutPeriod;
    VaryWashoutPeriod;
    constructor(OutcomeItemDTO) {
        super();
        if (OutcomeItemDTO == null) {
            this.CommonName = ko.observable();
            this.Outcome = ko.observable();
            this.WashoutPeriod = ko.observable();
            this.VaryWashoutPeriod = ko.observable();
        }
        else {
            this.CommonName = ko.observable(OutcomeItemDTO.CommonName);
            this.Outcome = ko.observable(OutcomeItemDTO.Outcome);
            this.WashoutPeriod = ko.observable(OutcomeItemDTO.WashoutPeriod);
            this.VaryWashoutPeriod = ko.observable(OutcomeItemDTO.VaryWashoutPeriod);
        }
    }
    toData() {
        return {
            CommonName: this.CommonName(),
            Outcome: this.Outcome(),
            WashoutPeriod: this.WashoutPeriod(),
            VaryWashoutPeriod: this.VaryWashoutPeriod(),
        };
    }
}
export class CovariateItemViewModel extends ViewModel {
    GroupingIndicator;
    Description;
    CodeType;
    Ingredients;
    SubGroupAnalysis;
    constructor(CovariateItemDTO) {
        super();
        if (CovariateItemDTO == null) {
            this.GroupingIndicator = ko.observable();
            this.Description = ko.observable();
            this.CodeType = ko.observable();
            this.Ingredients = ko.observable();
            this.SubGroupAnalysis = ko.observable();
        }
        else {
            this.GroupingIndicator = ko.observable(CovariateItemDTO.GroupingIndicator);
            this.Description = ko.observable(CovariateItemDTO.Description);
            this.CodeType = ko.observable(CovariateItemDTO.CodeType);
            this.Ingredients = ko.observable(CovariateItemDTO.Ingredients);
            this.SubGroupAnalysis = ko.observable(CovariateItemDTO.SubGroupAnalysis);
        }
    }
    toData() {
        return {
            GroupingIndicator: this.GroupingIndicator(),
            Description: this.Description(),
            CodeType: this.CodeType(),
            Ingredients: this.Ingredients(),
            SubGroupAnalysis: this.SubGroupAnalysis(),
        };
    }
}
export class WorkflowHistoryItemViewModel extends ViewModel {
    TaskID;
    TaskName;
    UserID;
    UserName;
    UserFullName;
    Message;
    Date;
    RoutingID;
    DataMart;
    WorkflowActivityID;
    constructor(WorkflowHistoryItemDTO) {
        super();
        if (WorkflowHistoryItemDTO == null) {
            this.TaskID = ko.observable();
            this.TaskName = ko.observable();
            this.UserID = ko.observable();
            this.UserName = ko.observable();
            this.UserFullName = ko.observable();
            this.Message = ko.observable();
            this.Date = ko.observable();
            this.RoutingID = ko.observable();
            this.DataMart = ko.observable();
            this.WorkflowActivityID = ko.observable();
        }
        else {
            this.TaskID = ko.observable(WorkflowHistoryItemDTO.TaskID);
            this.TaskName = ko.observable(WorkflowHistoryItemDTO.TaskName);
            this.UserID = ko.observable(WorkflowHistoryItemDTO.UserID);
            this.UserName = ko.observable(WorkflowHistoryItemDTO.UserName);
            this.UserFullName = ko.observable(WorkflowHistoryItemDTO.UserFullName);
            this.Message = ko.observable(WorkflowHistoryItemDTO.Message);
            this.Date = ko.observable(WorkflowHistoryItemDTO.Date);
            this.RoutingID = ko.observable(WorkflowHistoryItemDTO.RoutingID);
            this.DataMart = ko.observable(WorkflowHistoryItemDTO.DataMart);
            this.WorkflowActivityID = ko.observable(WorkflowHistoryItemDTO.WorkflowActivityID);
        }
    }
    toData() {
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
    }
}
export class LegacySchedulerRequestViewModel extends ViewModel {
    RequestID;
    AdapterPackageVersion;
    ScheduleJSON;
    constructor(LegacySchedulerRequestDTO) {
        super();
        if (LegacySchedulerRequestDTO == null) {
            this.RequestID = ko.observable();
            this.AdapterPackageVersion = ko.observable();
            this.ScheduleJSON = ko.observable();
        }
        else {
            this.RequestID = ko.observable(LegacySchedulerRequestDTO.RequestID);
            this.AdapterPackageVersion = ko.observable(LegacySchedulerRequestDTO.AdapterPackageVersion);
            this.ScheduleJSON = ko.observable(LegacySchedulerRequestDTO.ScheduleJSON);
        }
    }
    toData() {
        return {
            RequestID: this.RequestID(),
            AdapterPackageVersion: this.AdapterPackageVersion(),
            ScheduleJSON: this.ScheduleJSON(),
        };
    }
}
export class AvailableTermsRequestViewModel extends ViewModel {
    Adapters;
    QueryType;
    constructor(AvailableTermsRequestDTO) {
        super();
        if (AvailableTermsRequestDTO == null) {
            this.Adapters = ko.observableArray();
            this.QueryType = ko.observable();
        }
        else {
            this.Adapters = ko.observableArray(AvailableTermsRequestDTO.Adapters == null ? null : AvailableTermsRequestDTO.Adapters.map((item) => { return item; }));
            this.QueryType = ko.observable(AvailableTermsRequestDTO.QueryType);
        }
    }
    toData() {
        return {
            Adapters: this.Adapters(),
            QueryType: this.QueryType(),
        };
    }
}
export class DistributedRegressionManifestFile extends ViewModel {
    Items;
    DataPartners;
    constructor(DistributedRegressionManifestFile) {
        super();
        if (DistributedRegressionManifestFile == null) {
            this.Items = ko.observableArray();
            this.DataPartners = ko.observableArray();
        }
        else {
            this.Items = ko.observableArray(DistributedRegressionManifestFile.Items == null ? null : DistributedRegressionManifestFile.Items.map((item) => { return new DistributedRegressionAnalysisCenterManifestItem(item); }));
            this.DataPartners = ko.observableArray(DistributedRegressionManifestFile.DataPartners == null ? null : DistributedRegressionManifestFile.DataPartners.map((item) => { return new DistributedRegressionManifestDataPartner(item); }));
        }
    }
    toData() {
        return {
            Items: this.Items == null ? null : this.Items().map((item) => { return item.toData(); }),
            DataPartners: this.DataPartners == null ? null : this.DataPartners().map((item) => { return item.toData(); }),
        };
    }
}
export class DistributedRegressionAnalysisCenterManifestItem extends ViewModel {
    DocumentID;
    RevisionSetID;
    ResponseID;
    DataMartID;
    DataPartnerIdentifier;
    DataMart;
    RequestDataMartID;
    constructor(DistributedRegressionAnalysisCenterManifestItem) {
        super();
        if (DistributedRegressionAnalysisCenterManifestItem == null) {
            this.DocumentID = ko.observable();
            this.RevisionSetID = ko.observable();
            this.ResponseID = ko.observable();
            this.DataMartID = ko.observable();
            this.DataPartnerIdentifier = ko.observable();
            this.DataMart = ko.observable();
            this.RequestDataMartID = ko.observable();
        }
        else {
            this.DocumentID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DocumentID);
            this.RevisionSetID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RevisionSetID);
            this.ResponseID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.ResponseID);
            this.DataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMartID);
            this.DataPartnerIdentifier = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataPartnerIdentifier);
            this.DataMart = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMart);
            this.RequestDataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RequestDataMartID);
        }
    }
    toData() {
        return {
            DocumentID: this.DocumentID(),
            RevisionSetID: this.RevisionSetID(),
            ResponseID: this.ResponseID(),
            DataMartID: this.DataMartID(),
            DataPartnerIdentifier: this.DataPartnerIdentifier(),
            DataMart: this.DataMart(),
            RequestDataMartID: this.RequestDataMartID(),
        };
    }
}
export class DistributedRegressionManifestDataPartner extends ViewModel {
    DataMartID;
    RouteType;
    DataMartIdentifier;
    DataMartCode;
    constructor(DistributedRegressionManifestDataPartner) {
        super();
        if (DistributedRegressionManifestDataPartner == null) {
            this.DataMartID = ko.observable();
            this.RouteType = ko.observable();
            this.DataMartIdentifier = ko.observable();
            this.DataMartCode = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(DistributedRegressionManifestDataPartner.DataMartID);
            this.RouteType = ko.observable(DistributedRegressionManifestDataPartner.RouteType);
            this.DataMartIdentifier = ko.observable(DistributedRegressionManifestDataPartner.DataMartIdentifier);
            this.DataMartCode = ko.observable(DistributedRegressionManifestDataPartner.DataMartCode);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            RouteType: this.RouteType(),
            DataMartIdentifier: this.DataMartIdentifier(),
            DataMartCode: this.DataMartCode(),
        };
    }
}
export class QueryComposerQueryViewModel extends ViewModel {
    Header;
    Where;
    Select;
    TemporalEvents;
    constructor(QueryComposerQueryDTO) {
        super();
        if (QueryComposerQueryDTO == null) {
            this.Header = new QueryComposerQueryHeaderViewModel();
            this.Where = new QueryComposerWhereViewModel();
            this.Select = new QueryComposerSelectViewModel();
            this.TemporalEvents = ko.observableArray();
        }
        else {
            this.Header = new QueryComposerQueryHeaderViewModel(QueryComposerQueryDTO.Header);
            this.Where = new QueryComposerWhereViewModel(QueryComposerQueryDTO.Where);
            this.Select = new QueryComposerSelectViewModel(QueryComposerQueryDTO.Select);
            this.TemporalEvents = ko.observableArray(QueryComposerQueryDTO.TemporalEvents == null ? null : QueryComposerQueryDTO.TemporalEvents.map((item) => { return new QueryComposerTemporalEventViewModel(item); }));
        }
    }
    toData() {
        return {
            Header: this.Header.toData(),
            Where: this.Where.toData(),
            Select: this.Select.toData(),
            TemporalEvents: this.TemporalEvents == null ? null : this.TemporalEvents().map((item) => { return item.toData(); }),
        };
    }
}
export class QueryComposerResponseAggregationDefinitionViewModel extends ViewModel {
    GroupBy;
    Select;
    Name;
    constructor(QueryComposerResponseAggregationDefinitionDTO) {
        super();
        if (QueryComposerResponseAggregationDefinitionDTO == null) {
            this.GroupBy = ko.observableArray();
            this.Select = ko.observableArray();
            this.Name = ko.observable();
        }
        else {
            this.GroupBy = ko.observableArray(QueryComposerResponseAggregationDefinitionDTO.GroupBy == null ? null : QueryComposerResponseAggregationDefinitionDTO.GroupBy.map((item) => { return item; }));
            this.Select = ko.observableArray(QueryComposerResponseAggregationDefinitionDTO.Select == null ? null : QueryComposerResponseAggregationDefinitionDTO.Select.map((item) => { return item; }));
            this.Name = ko.observable(QueryComposerResponseAggregationDefinitionDTO.Name);
        }
    }
    toData() {
        return {
            GroupBy: this.GroupBy == null ? null : this.GroupBy().map((item) => { return item; }),
            Select: this.Select(),
            Name: this.Name(),
        };
    }
}
export class QueryComposerResponseHeaderViewModel extends ViewModel {
    ID;
    RequestID;
    DocumentID;
    QueryingStart;
    QueryingEnd;
    DataMart;
    constructor(QueryComposerResponseHeaderDTO) {
        super();
        if (QueryComposerResponseHeaderDTO == null) {
            this.ID = ko.observable();
            this.RequestID = ko.observable();
            this.DocumentID = ko.observable();
            this.QueryingStart = ko.observable();
            this.QueryingEnd = ko.observable();
            this.DataMart = ko.observable();
        }
        else {
            this.ID = ko.observable(QueryComposerResponseHeaderDTO.ID);
            this.RequestID = ko.observable(QueryComposerResponseHeaderDTO.RequestID);
            this.DocumentID = ko.observable(QueryComposerResponseHeaderDTO.DocumentID);
            this.QueryingStart = ko.observable(QueryComposerResponseHeaderDTO.QueryingStart);
            this.QueryingEnd = ko.observable(QueryComposerResponseHeaderDTO.QueryingEnd);
            this.DataMart = ko.observable(QueryComposerResponseHeaderDTO.DataMart);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            RequestID: this.RequestID(),
            DocumentID: this.DocumentID(),
            QueryingStart: this.QueryingStart(),
            QueryingEnd: this.QueryingEnd(),
            DataMart: this.DataMart(),
        };
    }
}
export class QueryComposerResponsePropertyDefinitionViewModel extends ViewModel {
    Name;
    Type;
    As;
    Aggregate;
    constructor(QueryComposerResponsePropertyDefinitionDTO) {
        super();
        if (QueryComposerResponsePropertyDefinitionDTO == null) {
            this.Name = ko.observable();
            this.Type = ko.observable();
            this.As = ko.observable();
            this.Aggregate = ko.observable();
        }
        else {
            this.Name = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Name);
            this.Type = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Type);
            this.As = ko.observable(QueryComposerResponsePropertyDefinitionDTO.As);
            this.Aggregate = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Aggregate);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Type: this.Type(),
            As: this.As(),
            Aggregate: this.Aggregate(),
        };
    }
}
export class QueryComposerResponseQueryResultViewModel extends ViewModel {
    ID;
    Name;
    QueryStart;
    QueryEnd;
    PostProcessStart;
    PostProcessEnd;
    Errors;
    Results;
    LowCellThrehold;
    Properties;
    Aggregation;
    constructor(QueryComposerResponseQueryResultDTO) {
        super();
        if (QueryComposerResponseQueryResultDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.QueryStart = ko.observable();
            this.QueryEnd = ko.observable();
            this.PostProcessStart = ko.observable();
            this.PostProcessEnd = ko.observable();
            this.Errors = ko.observableArray();
            this.Results = ko.observableArray();
            this.LowCellThrehold = ko.observable();
            this.Properties = ko.observableArray();
            this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel();
        }
        else {
            this.ID = ko.observable(QueryComposerResponseQueryResultDTO.ID);
            this.Name = ko.observable(QueryComposerResponseQueryResultDTO.Name);
            this.QueryStart = ko.observable(QueryComposerResponseQueryResultDTO.QueryStart);
            this.QueryEnd = ko.observable(QueryComposerResponseQueryResultDTO.QueryEnd);
            this.PostProcessStart = ko.observable(QueryComposerResponseQueryResultDTO.PostProcessStart);
            this.PostProcessEnd = ko.observable(QueryComposerResponseQueryResultDTO.PostProcessEnd);
            this.Errors = ko.observableArray(QueryComposerResponseQueryResultDTO.Errors == null ? null : QueryComposerResponseQueryResultDTO.Errors.map((item) => { return new QueryComposerResponseErrorViewModel(item); }));
            this.Results = ko.observableArray(QueryComposerResponseQueryResultDTO.Results == null ? null : QueryComposerResponseQueryResultDTO.Results.map((item) => { return item; }));
            this.LowCellThrehold = ko.observable(QueryComposerResponseQueryResultDTO.LowCellThrehold);
            this.Properties = ko.observableArray(QueryComposerResponseQueryResultDTO.Properties == null ? null : QueryComposerResponseQueryResultDTO.Properties.map((item) => { return new QueryComposerResponsePropertyDefinitionViewModel(item); }));
            this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel(QueryComposerResponseQueryResultDTO.Aggregation);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            QueryStart: this.QueryStart(),
            QueryEnd: this.QueryEnd(),
            PostProcessStart: this.PostProcessStart(),
            PostProcessEnd: this.PostProcessEnd(),
            Errors: this.Errors == null ? null : this.Errors().map((item) => { return item.toData(); }),
            Results: this.Results(),
            LowCellThrehold: this.LowCellThrehold(),
            Properties: this.Properties == null ? null : this.Properties().map((item) => { return item.toData(); }),
            Aggregation: this.Aggregation.toData(),
        };
    }
}
export class QueryComposerTemporalEventViewModel extends ViewModel {
    IndexEventDateIdentifier;
    DaysBefore;
    DaysAfter;
    Criteria;
    constructor(QueryComposerTemporalEventDTO) {
        super();
        if (QueryComposerTemporalEventDTO == null) {
            this.IndexEventDateIdentifier = ko.observable();
            this.DaysBefore = ko.observable();
            this.DaysAfter = ko.observable();
            this.Criteria = ko.observableArray();
        }
        else {
            this.IndexEventDateIdentifier = ko.observable(QueryComposerTemporalEventDTO.IndexEventDateIdentifier);
            this.DaysBefore = ko.observable(QueryComposerTemporalEventDTO.DaysBefore);
            this.DaysAfter = ko.observable(QueryComposerTemporalEventDTO.DaysAfter);
            this.Criteria = ko.observableArray(QueryComposerTemporalEventDTO.Criteria == null ? null : QueryComposerTemporalEventDTO.Criteria.map((item) => { return new QueryComposerCriteriaViewModel(item); }));
        }
    }
    toData() {
        return {
            IndexEventDateIdentifier: this.IndexEventDateIdentifier(),
            DaysBefore: this.DaysBefore(),
            DaysAfter: this.DaysAfter(),
            Criteria: this.Criteria == null ? null : this.Criteria().map((item) => { return item.toData(); }),
        };
    }
}
export class SectionSpecificTermViewModel extends ViewModel {
    TemplateID;
    TermID;
    Section;
    constructor(SectionSpecificTermDTO) {
        super();
        if (SectionSpecificTermDTO == null) {
            this.TemplateID = ko.observable();
            this.TermID = ko.observable();
            this.Section = ko.observable();
        }
        else {
            this.TemplateID = ko.observable(SectionSpecificTermDTO.TemplateID);
            this.TermID = ko.observable(SectionSpecificTermDTO.TermID);
            this.Section = ko.observable(SectionSpecificTermDTO.Section);
        }
    }
    toData() {
        return {
            TemplateID: this.TemplateID(),
            TermID: this.TermID(),
            Section: this.Section(),
        };
    }
}
export class TemplateTermViewModel extends ViewModel {
    TemplateID;
    Template;
    TermID;
    Term;
    Allowed;
    Section;
    constructor(TemplateTermDTO) {
        super();
        if (TemplateTermDTO == null) {
            this.TemplateID = ko.observable();
            this.Template = new TemplateViewModel();
            this.TermID = ko.observable();
            this.Term = new TermViewModel();
            this.Allowed = ko.observable();
            this.Section = ko.observable();
        }
        else {
            this.TemplateID = ko.observable(TemplateTermDTO.TemplateID);
            this.Template = new TemplateViewModel(TemplateTermDTO.Template);
            this.TermID = ko.observable(TemplateTermDTO.TermID);
            this.Term = new TermViewModel(TemplateTermDTO.Term);
            this.Allowed = ko.observable(TemplateTermDTO.Allowed);
            this.Section = ko.observable(TemplateTermDTO.Section);
        }
    }
    toData() {
        return {
            TemplateID: this.TemplateID(),
            Template: this.Template.toData(),
            TermID: this.TermID(),
            Term: this.Term.toData(),
            Allowed: this.Allowed(),
            Section: this.Section(),
        };
    }
}
export class MatchingCriteriaViewModel extends ViewModel {
    TermIDs;
    ProjectID;
    Request;
    RequestID;
    constructor(MatchingCriteriaDTO) {
        super();
        if (MatchingCriteriaDTO == null) {
            this.TermIDs = ko.observableArray();
            this.ProjectID = ko.observable();
            this.Request = ko.observable();
            this.RequestID = ko.observable();
        }
        else {
            this.TermIDs = ko.observableArray(MatchingCriteriaDTO.TermIDs == null ? null : MatchingCriteriaDTO.TermIDs.map((item) => { return item; }));
            this.ProjectID = ko.observable(MatchingCriteriaDTO.ProjectID);
            this.Request = ko.observable(MatchingCriteriaDTO.Request);
            this.RequestID = ko.observable(MatchingCriteriaDTO.RequestID);
        }
    }
    toData() {
        return {
            TermIDs: this.TermIDs(),
            ProjectID: this.ProjectID(),
            Request: this.Request(),
            RequestID: this.RequestID(),
        };
    }
}
export class QueryComposerCriteriaViewModel extends ViewModel {
    ID;
    RelatedToID;
    Name;
    Operator;
    IndexEvent;
    Exclusion;
    Criteria;
    Terms;
    Type;
    constructor(QueryComposerCriteriaDTO) {
        super();
        if (QueryComposerCriteriaDTO == null) {
            this.ID = ko.observable();
            this.RelatedToID = ko.observable();
            this.Name = ko.observable();
            this.Operator = ko.observable();
            this.IndexEvent = ko.observable();
            this.Exclusion = ko.observable();
            this.Criteria = ko.observableArray();
            this.Terms = ko.observableArray();
            this.Type = ko.observable();
        }
        else {
            this.ID = ko.observable(QueryComposerCriteriaDTO.ID);
            this.RelatedToID = ko.observable(QueryComposerCriteriaDTO.RelatedToID);
            this.Name = ko.observable(QueryComposerCriteriaDTO.Name);
            this.Operator = ko.observable(QueryComposerCriteriaDTO.Operator);
            this.IndexEvent = ko.observable(QueryComposerCriteriaDTO.IndexEvent);
            this.Exclusion = ko.observable(QueryComposerCriteriaDTO.Exclusion);
            this.Criteria = ko.observableArray(QueryComposerCriteriaDTO.Criteria == null ? null : QueryComposerCriteriaDTO.Criteria.map((item) => { return new QueryComposerCriteriaViewModel(item); }));
            this.Terms = ko.observableArray(QueryComposerCriteriaDTO.Terms == null ? null : QueryComposerCriteriaDTO.Terms.map((item) => { return new QueryComposerTermViewModel(item); }));
            this.Type = ko.observable(QueryComposerCriteriaDTO.Type);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            RelatedToID: this.RelatedToID(),
            Name: this.Name(),
            Operator: this.Operator(),
            IndexEvent: this.IndexEvent(),
            Exclusion: this.Exclusion(),
            Criteria: this.Criteria == null ? null : this.Criteria().map((item) => { return item.toData(); }),
            Terms: this.Terms == null ? null : this.Terms().map((item) => { return item.toData(); }),
            Type: this.Type(),
        };
    }
}
export class QueryComposerFieldViewModel extends ViewModel {
    FieldName;
    Type;
    GroupBy;
    StratifyBy;
    Aggregate;
    Select;
    OrderBy;
    constructor(QueryComposerFieldDTO) {
        super();
        if (QueryComposerFieldDTO == null) {
            this.FieldName = ko.observable();
            this.Type = ko.observable();
            this.GroupBy = ko.observable();
            this.StratifyBy = ko.observable();
            this.Aggregate = ko.observable();
            this.Select = ko.observableArray();
            this.OrderBy = ko.observable();
        }
        else {
            this.FieldName = ko.observable(QueryComposerFieldDTO.FieldName);
            this.Type = ko.observable(QueryComposerFieldDTO.Type);
            this.GroupBy = ko.observable(QueryComposerFieldDTO.GroupBy);
            this.StratifyBy = ko.observable(QueryComposerFieldDTO.StratifyBy);
            this.Aggregate = ko.observable(QueryComposerFieldDTO.Aggregate);
            this.Select = ko.observableArray(QueryComposerFieldDTO.Select == null ? null : QueryComposerFieldDTO.Select.map((item) => { return new QueryComposerSelectViewModel(item); }));
            this.OrderBy = ko.observable(QueryComposerFieldDTO.OrderBy);
        }
    }
    toData() {
        return {
            FieldName: this.FieldName(),
            Type: this.Type(),
            GroupBy: this.GroupBy(),
            StratifyBy: this.StratifyBy(),
            Aggregate: this.Aggregate(),
            Select: this.Select == null ? null : this.Select().map((item) => { return item.toData(); }),
            OrderBy: this.OrderBy(),
        };
    }
}
export class QueryComposerGroupByViewModel extends ViewModel {
    Field;
    Aggregate;
    constructor(QueryComposerGroupByDTO) {
        super();
        if (QueryComposerGroupByDTO == null) {
            this.Field = ko.observable();
            this.Aggregate = ko.observable();
        }
        else {
            this.Field = ko.observable(QueryComposerGroupByDTO.Field);
            this.Aggregate = ko.observable(QueryComposerGroupByDTO.Aggregate);
        }
    }
    toData() {
        return {
            Field: this.Field(),
            Aggregate: this.Aggregate(),
        };
    }
}
export class QueryComposerHeaderViewModel extends ViewModel {
    ID;
    Name;
    Description;
    ViewUrl;
    Priority;
    DueDate;
    SubmittedOn;
    constructor(QueryComposerHeaderDTO) {
        super();
        if (QueryComposerHeaderDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.ViewUrl = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.SubmittedOn = ko.observable();
        }
        else {
            this.ID = ko.observable(QueryComposerHeaderDTO.ID);
            this.Name = ko.observable(QueryComposerHeaderDTO.Name);
            this.Description = ko.observable(QueryComposerHeaderDTO.Description);
            this.ViewUrl = ko.observable(QueryComposerHeaderDTO.ViewUrl);
            this.Priority = ko.observable(QueryComposerHeaderDTO.Priority);
            this.DueDate = ko.observable(QueryComposerHeaderDTO.DueDate);
            this.SubmittedOn = ko.observable(QueryComposerHeaderDTO.SubmittedOn);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            Description: this.Description(),
            ViewUrl: this.ViewUrl(),
            Priority: this.Priority(),
            DueDate: this.DueDate(),
            SubmittedOn: this.SubmittedOn(),
        };
    }
}
export class QueryComposerOrderByViewModel extends ViewModel {
    Direction;
    constructor(QueryComposerOrderByDTO) {
        super();
        if (QueryComposerOrderByDTO == null) {
            this.Direction = ko.observable();
        }
        else {
            this.Direction = ko.observable(QueryComposerOrderByDTO.Direction);
        }
    }
    toData() {
        return {
            Direction: this.Direction(),
        };
    }
}
export class QueryComposerRequestViewModel extends ViewModel {
    SchemaVersion;
    Header;
    Queries;
    constructor(QueryComposerRequestDTO) {
        super();
        if (QueryComposerRequestDTO == null) {
            this.SchemaVersion = ko.observable();
            this.Header = new QueryComposerRequestHeaderViewModel();
            this.Queries = ko.observableArray();
        }
        else {
            this.SchemaVersion = ko.observable(QueryComposerRequestDTO.SchemaVersion);
            this.Header = new QueryComposerRequestHeaderViewModel(QueryComposerRequestDTO.Header);
            this.Queries = ko.observableArray(QueryComposerRequestDTO.Queries == null ? null : QueryComposerRequestDTO.Queries.map((item) => { return new QueryComposerQueryViewModel(item); }));
        }
    }
    toData() {
        return {
            SchemaVersion: this.SchemaVersion(),
            Header: this.Header.toData(),
            Queries: this.Queries == null ? null : this.Queries().map((item) => { return item.toData(); }),
        };
    }
}
export class QueryComposerResponseErrorViewModel extends ViewModel {
    QueryID;
    Code;
    Description;
    constructor(QueryComposerResponseErrorDTO) {
        super();
        if (QueryComposerResponseErrorDTO == null) {
            this.QueryID = ko.observable();
            this.Code = ko.observable();
            this.Description = ko.observable();
        }
        else {
            this.QueryID = ko.observable(QueryComposerResponseErrorDTO.QueryID);
            this.Code = ko.observable(QueryComposerResponseErrorDTO.Code);
            this.Description = ko.observable(QueryComposerResponseErrorDTO.Description);
        }
    }
    toData() {
        return {
            QueryID: this.QueryID(),
            Code: this.Code(),
            Description: this.Description(),
        };
    }
}
export class QueryComposerSelectViewModel extends ViewModel {
    Fields;
    constructor(QueryComposerSelectDTO) {
        super();
        if (QueryComposerSelectDTO == null) {
            this.Fields = ko.observableArray();
        }
        else {
            this.Fields = ko.observableArray(QueryComposerSelectDTO.Fields == null ? null : QueryComposerSelectDTO.Fields.map((item) => { return new QueryComposerFieldViewModel(item); }));
        }
    }
    toData() {
        return {
            Fields: this.Fields == null ? null : this.Fields().map((item) => { return item.toData(); }),
        };
    }
}
export class QueryComposerResponseViewModel extends ViewModel {
    SchemaVersion;
    Header;
    Errors;
    Queries;
    constructor(QueryComposerResponseDTO) {
        super();
        if (QueryComposerResponseDTO == null) {
            this.SchemaVersion = ko.observable();
            this.Header = new QueryComposerResponseHeaderViewModel();
            this.Errors = ko.observableArray();
            this.Queries = ko.observableArray();
        }
        else {
            this.SchemaVersion = ko.observable(QueryComposerResponseDTO.SchemaVersion);
            this.Header = new QueryComposerResponseHeaderViewModel(QueryComposerResponseDTO.Header);
            this.Errors = ko.observableArray(QueryComposerResponseDTO.Errors == null ? null : QueryComposerResponseDTO.Errors.map((item) => { return new QueryComposerResponseErrorViewModel(item); }));
            this.Queries = ko.observableArray(QueryComposerResponseDTO.Queries == null ? null : QueryComposerResponseDTO.Queries.map((item) => { return new QueryComposerResponseQueryResultViewModel(item); }));
        }
    }
    toData() {
        return {
            SchemaVersion: this.SchemaVersion(),
            Header: this.Header.toData(),
            Errors: this.Errors == null ? null : this.Errors().map((item) => { return item.toData(); }),
            Queries: this.Queries == null ? null : this.Queries().map((item) => { return item.toData(); }),
        };
    }
}
export class QueryComposerTermViewModel extends ViewModel {
    Operator;
    Type;
    Values;
    Criteria;
    Design;
    constructor(QueryComposerTermDTO) {
        super();
        if (QueryComposerTermDTO == null) {
            this.Operator = ko.observable();
            this.Type = ko.observable();
            this.Values = ko.observable({});
            this.Criteria = ko.observableArray();
            this.Design = new DesignViewModel();
        }
        else {
            this.Operator = ko.observable(QueryComposerTermDTO.Operator);
            this.Type = ko.observable(QueryComposerTermDTO.Type);
            this.Values = ko.observable(QueryComposerTermDTO.Values);
            this.Criteria = ko.observableArray(QueryComposerTermDTO.Criteria == null ? null : QueryComposerTermDTO.Criteria.map((item) => { return new QueryComposerCriteriaViewModel(item); }));
            this.Design = new DesignViewModel(QueryComposerTermDTO.Design);
        }
    }
    toData() {
        return {
            Operator: this.Operator(),
            Type: this.Type(),
            Values: ko.mapping.toJS(this.Values()),
            Criteria: this.Criteria == null ? null : this.Criteria().map((item) => { return item.toData(); }),
            Design: this.Design.toData(),
        };
    }
}
export class QueryComposerWhereViewModel extends ViewModel {
    Criteria;
    constructor(QueryComposerWhereDTO) {
        super();
        if (QueryComposerWhereDTO == null) {
            this.Criteria = ko.observableArray();
        }
        else {
            this.Criteria = ko.observableArray(QueryComposerWhereDTO.Criteria == null ? null : QueryComposerWhereDTO.Criteria.map((item) => { return new QueryComposerCriteriaViewModel(item); }));
        }
    }
    toData() {
        return {
            Criteria: this.Criteria == null ? null : this.Criteria().map((item) => { return item.toData(); }),
        };
    }
}
export class ProjectRequestTypeViewModel extends EntityDtoViewModel {
    ProjectID;
    RequestTypeID;
    RequestType;
    WorkflowID;
    Workflow;
    constructor(ProjectRequestTypeDTO) {
        super();
        if (ProjectRequestTypeDTO == null) {
            this.ProjectID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestType = ko.observable();
            this.WorkflowID = ko.observable();
            this.Workflow = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectRequestTypeDTO.ProjectID);
            this.RequestTypeID = ko.observable(ProjectRequestTypeDTO.RequestTypeID);
            this.RequestType = ko.observable(ProjectRequestTypeDTO.RequestType);
            this.WorkflowID = ko.observable(ProjectRequestTypeDTO.WorkflowID);
            this.Workflow = ko.observable(ProjectRequestTypeDTO.Workflow);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            RequestTypeID: this.RequestTypeID(),
            RequestType: this.RequestType(),
            WorkflowID: this.WorkflowID(),
            Workflow: this.Workflow(),
        };
    }
}
export class RequestObserverEventSubscriptionViewModel extends EntityDtoViewModel {
    RequestObserverID;
    EventID;
    LastRunTime;
    NextDueTime;
    Frequency;
    constructor(RequestObserverEventSubscriptionDTO) {
        super();
        if (RequestObserverEventSubscriptionDTO == null) {
            this.RequestObserverID = ko.observable();
            this.EventID = ko.observable();
            this.LastRunTime = ko.observable();
            this.NextDueTime = ko.observable();
            this.Frequency = ko.observable();
        }
        else {
            this.RequestObserverID = ko.observable(RequestObserverEventSubscriptionDTO.RequestObserverID);
            this.EventID = ko.observable(RequestObserverEventSubscriptionDTO.EventID);
            this.LastRunTime = ko.observable(RequestObserverEventSubscriptionDTO.LastRunTime);
            this.NextDueTime = ko.observable(RequestObserverEventSubscriptionDTO.NextDueTime);
            this.Frequency = ko.observable(RequestObserverEventSubscriptionDTO.Frequency);
        }
    }
    toData() {
        return {
            RequestObserverID: this.RequestObserverID(),
            EventID: this.EventID(),
            LastRunTime: this.LastRunTime(),
            NextDueTime: this.NextDueTime(),
            Frequency: this.Frequency(),
        };
    }
}
export class RequestTypeTermViewModel extends EntityDtoViewModel {
    RequestTypeID;
    TermID;
    Term;
    Description;
    OID;
    ReferenceUrl;
    constructor(RequestTypeTermDTO) {
        super();
        if (RequestTypeTermDTO == null) {
            this.RequestTypeID = ko.observable();
            this.TermID = ko.observable();
            this.Term = ko.observable();
            this.Description = ko.observable();
            this.OID = ko.observable();
            this.ReferenceUrl = ko.observable();
        }
        else {
            this.RequestTypeID = ko.observable(RequestTypeTermDTO.RequestTypeID);
            this.TermID = ko.observable(RequestTypeTermDTO.TermID);
            this.Term = ko.observable(RequestTypeTermDTO.Term);
            this.Description = ko.observable(RequestTypeTermDTO.Description);
            this.OID = ko.observable(RequestTypeTermDTO.OID);
            this.ReferenceUrl = ko.observable(RequestTypeTermDTO.ReferenceUrl);
        }
    }
    toData() {
        return {
            RequestTypeID: this.RequestTypeID(),
            TermID: this.TermID(),
            Term: this.Term(),
            Description: this.Description(),
            OID: this.OID(),
            ReferenceUrl: this.ReferenceUrl(),
        };
    }
}
export class BaseFieldOptionAclViewModel extends EntityDtoViewModel {
    FieldIdentifier;
    Permission;
    Overridden;
    SecurityGroupID;
    SecurityGroup;
    constructor(BaseFieldOptionAclDTO) {
        super();
        if (BaseFieldOptionAclDTO == null) {
            this.FieldIdentifier = ko.observable();
            this.Permission = ko.observable();
            this.Overridden = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
        }
        else {
            this.FieldIdentifier = ko.observable(BaseFieldOptionAclDTO.FieldIdentifier);
            this.Permission = ko.observable(BaseFieldOptionAclDTO.Permission);
            this.Overridden = ko.observable(BaseFieldOptionAclDTO.Overridden);
            this.SecurityGroupID = ko.observable(BaseFieldOptionAclDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(BaseFieldOptionAclDTO.SecurityGroup);
        }
    }
    toData() {
        return {
            FieldIdentifier: this.FieldIdentifier(),
            Permission: this.Permission(),
            Overridden: this.Overridden(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
        };
    }
}
export class BaseEventPermissionViewModel extends EntityDtoViewModel {
    SecurityGroupID;
    SecurityGroup;
    Allowed;
    Overridden;
    EventID;
    Event;
    constructor(BaseEventPermissionDTO) {
        super();
        if (BaseEventPermissionDTO == null) {
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.SecurityGroupID = ko.observable(BaseEventPermissionDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(BaseEventPermissionDTO.SecurityGroup);
            this.Allowed = ko.observable(BaseEventPermissionDTO.Allowed);
            this.Overridden = ko.observable(BaseEventPermissionDTO.Overridden);
            this.EventID = ko.observable(BaseEventPermissionDTO.EventID);
            this.Event = ko.observable(BaseEventPermissionDTO.Event);
        }
    }
    toData() {
        return {
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class OrganizationGroupViewModel extends EntityDtoViewModel {
    OrganizationID;
    Organization;
    GroupID;
    Group;
    constructor(OrganizationGroupDTO) {
        super();
        if (OrganizationGroupDTO == null) {
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.GroupID = ko.observable();
            this.Group = ko.observable();
        }
        else {
            this.OrganizationID = ko.observable(OrganizationGroupDTO.OrganizationID);
            this.Organization = ko.observable(OrganizationGroupDTO.Organization);
            this.GroupID = ko.observable(OrganizationGroupDTO.GroupID);
            this.Group = ko.observable(OrganizationGroupDTO.Group);
        }
    }
    toData() {
        return {
            OrganizationID: this.OrganizationID(),
            Organization: this.Organization(),
            GroupID: this.GroupID(),
            Group: this.Group(),
        };
    }
}
export class OrganizationRegistryViewModel extends EntityDtoViewModel {
    OrganizationID;
    Organization;
    Acronym;
    OrganizationParent;
    RegistryID;
    Registry;
    Description;
    Type;
    constructor(OrganizationRegistryDTO) {
        super();
        if (OrganizationRegistryDTO == null) {
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.Acronym = ko.observable();
            this.OrganizationParent = ko.observable();
            this.RegistryID = ko.observable();
            this.Registry = ko.observable();
            this.Description = ko.observable();
            this.Type = ko.observable();
        }
        else {
            this.OrganizationID = ko.observable(OrganizationRegistryDTO.OrganizationID);
            this.Organization = ko.observable(OrganizationRegistryDTO.Organization);
            this.Acronym = ko.observable(OrganizationRegistryDTO.Acronym);
            this.OrganizationParent = ko.observable(OrganizationRegistryDTO.OrganizationParent);
            this.RegistryID = ko.observable(OrganizationRegistryDTO.RegistryID);
            this.Registry = ko.observable(OrganizationRegistryDTO.Registry);
            this.Description = ko.observable(OrganizationRegistryDTO.Description);
            this.Type = ko.observable(OrganizationRegistryDTO.Type);
        }
    }
    toData() {
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
    }
}
export class ProjectDataMartWithRequestTypesViewModel extends ProjectDataMartViewModel {
    RequestTypes;
    constructor(ProjectDataMartWithRequestTypesDTO) {
        super();
        if (ProjectDataMartWithRequestTypesDTO == null) {
            this.RequestTypes = ko.observableArray();
            this.ProjectID = ko.observable();
            this.Project = ko.observable();
            this.ProjectAcronym = ko.observable();
            this.DataMartID = ko.observable();
            this.DataMart = ko.observable();
            this.Organization = ko.observable();
        }
        else {
            this.RequestTypes = ko.observableArray(ProjectDataMartWithRequestTypesDTO.RequestTypes == null ? null : ProjectDataMartWithRequestTypesDTO.RequestTypes.map((item) => { return new RequestTypeViewModel(item); }));
            this.ProjectID = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectID);
            this.Project = ko.observable(ProjectDataMartWithRequestTypesDTO.Project);
            this.ProjectAcronym = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectAcronym);
            this.DataMartID = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMartID);
            this.DataMart = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMart);
            this.Organization = ko.observable(ProjectDataMartWithRequestTypesDTO.Organization);
        }
    }
    toData() {
        return {
            RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => { return item.toData(); }),
            ProjectID: this.ProjectID(),
            Project: this.Project(),
            ProjectAcronym: this.ProjectAcronym(),
            DataMartID: this.DataMartID(),
            DataMart: this.DataMart(),
            Organization: this.Organization(),
        };
    }
}
export class ProjectOrganizationViewModel extends EntityDtoViewModel {
    ProjectID;
    Project;
    OrganizationID;
    Organization;
    constructor(ProjectOrganizationDTO) {
        super();
        if (ProjectOrganizationDTO == null) {
            this.ProjectID = ko.observable();
            this.Project = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectOrganizationDTO.ProjectID);
            this.Project = ko.observable(ProjectOrganizationDTO.Project);
            this.OrganizationID = ko.observable(ProjectOrganizationDTO.OrganizationID);
            this.Organization = ko.observable(ProjectOrganizationDTO.Organization);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            Project: this.Project(),
            OrganizationID: this.OrganizationID(),
            Organization: this.Organization(),
        };
    }
}
export class BaseAclViewModel extends EntityDtoViewModel {
    SecurityGroupID;
    SecurityGroup;
    Overridden;
    constructor(BaseAclDTO) {
        super();
        if (BaseAclDTO == null) {
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.SecurityGroupID = ko.observable(BaseAclDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(BaseAclDTO.SecurityGroup);
            this.Overridden = ko.observable(BaseAclDTO.Overridden);
        }
    }
    toData() {
        return {
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class UserEventSubscriptionViewModel extends EntityDtoViewModel {
    UserID;
    EventID;
    LastRunTime;
    NextDueTime;
    Frequency;
    FrequencyForMy;
    constructor(UserEventSubscriptionDTO) {
        super();
        if (UserEventSubscriptionDTO == null) {
            this.UserID = ko.observable();
            this.EventID = ko.observable();
            this.LastRunTime = ko.observable();
            this.NextDueTime = ko.observable();
            this.Frequency = ko.observable();
            this.FrequencyForMy = ko.observable();
        }
        else {
            this.UserID = ko.observable(UserEventSubscriptionDTO.UserID);
            this.EventID = ko.observable(UserEventSubscriptionDTO.EventID);
            this.LastRunTime = ko.observable(UserEventSubscriptionDTO.LastRunTime);
            this.NextDueTime = ko.observable(UserEventSubscriptionDTO.NextDueTime);
            this.Frequency = ko.observable(UserEventSubscriptionDTO.Frequency);
            this.FrequencyForMy = ko.observable(UserEventSubscriptionDTO.FrequencyForMy);
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            EventID: this.EventID(),
            LastRunTime: this.LastRunTime(),
            NextDueTime: this.NextDueTime(),
            Frequency: this.Frequency(),
            FrequencyForMy: this.FrequencyForMy(),
        };
    }
}
export class UserSettingViewModel extends EntityDtoViewModel {
    UserID;
    Key;
    Setting;
    constructor(UserSettingDTO) {
        super();
        if (UserSettingDTO == null) {
            this.UserID = ko.observable();
            this.Key = ko.observable();
            this.Setting = ko.observable();
        }
        else {
            this.UserID = ko.observable(UserSettingDTO.UserID);
            this.Key = ko.observable(UserSettingDTO.Key);
            this.Setting = ko.observable(UserSettingDTO.Setting);
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            Key: this.Key(),
            Setting: this.Setting(),
        };
    }
}
export class QueryComposerQueryHeaderViewModel extends QueryComposerHeaderViewModel {
    QueryType;
    ComposerInterface;
    constructor(QueryComposerQueryHeaderDTO) {
        super();
        if (QueryComposerQueryHeaderDTO == null) {
            this.QueryType = ko.observable();
            this.ComposerInterface = ko.observable();
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.ViewUrl = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.SubmittedOn = ko.observable();
        }
        else {
            this.QueryType = ko.observable(QueryComposerQueryHeaderDTO.QueryType);
            this.ComposerInterface = ko.observable(QueryComposerQueryHeaderDTO.ComposerInterface);
            this.ID = ko.observable(QueryComposerQueryHeaderDTO.ID);
            this.Name = ko.observable(QueryComposerQueryHeaderDTO.Name);
            this.Description = ko.observable(QueryComposerQueryHeaderDTO.Description);
            this.ViewUrl = ko.observable(QueryComposerQueryHeaderDTO.ViewUrl);
            this.Priority = ko.observable(QueryComposerQueryHeaderDTO.Priority);
            this.DueDate = ko.observable(QueryComposerQueryHeaderDTO.DueDate);
            this.SubmittedOn = ko.observable(QueryComposerQueryHeaderDTO.SubmittedOn);
        }
    }
    toData() {
        return {
            QueryType: this.QueryType(),
            ComposerInterface: this.ComposerInterface(),
            ID: this.ID(),
            Name: this.Name(),
            Description: this.Description(),
            ViewUrl: this.ViewUrl(),
            Priority: this.Priority(),
            DueDate: this.DueDate(),
            SubmittedOn: this.SubmittedOn(),
        };
    }
}
export class QueryComposerRequestHeaderViewModel extends QueryComposerHeaderViewModel {
    constructor(QueryComposerRequestHeaderDTO) {
        super();
        if (QueryComposerRequestHeaderDTO == null) {
            this.ID = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.ViewUrl = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.SubmittedOn = ko.observable();
        }
        else {
            this.ID = ko.observable(QueryComposerRequestHeaderDTO.ID);
            this.Name = ko.observable(QueryComposerRequestHeaderDTO.Name);
            this.Description = ko.observable(QueryComposerRequestHeaderDTO.Description);
            this.ViewUrl = ko.observable(QueryComposerRequestHeaderDTO.ViewUrl);
            this.Priority = ko.observable(QueryComposerRequestHeaderDTO.Priority);
            this.DueDate = ko.observable(QueryComposerRequestHeaderDTO.DueDate);
            this.SubmittedOn = ko.observable(QueryComposerRequestHeaderDTO.SubmittedOn);
        }
    }
    toData() {
        return {
            ID: this.ID(),
            Name: this.Name(),
            Description: this.Description(),
            ViewUrl: this.ViewUrl(),
            Priority: this.Priority(),
            DueDate: this.DueDate(),
            SubmittedOn: this.SubmittedOn(),
        };
    }
}
export class WFCommentViewModel extends EntityDtoWithIDViewModel {
    Comment;
    CreatedOn;
    CreatedByID;
    CreatedBy;
    RequestID;
    TaskID;
    WorkflowActivityID;
    WorkflowActivity;
    constructor(WFCommentDTO) {
        super();
        if (WFCommentDTO == null) {
            this.Comment = ko.observable();
            this.CreatedOn = ko.observable();
            this.CreatedByID = ko.observable();
            this.CreatedBy = ko.observable();
            this.RequestID = ko.observable();
            this.TaskID = ko.observable();
            this.WorkflowActivityID = ko.observable();
            this.WorkflowActivity = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Comment = ko.observable(WFCommentDTO.Comment);
            this.CreatedOn = ko.observable(WFCommentDTO.CreatedOn);
            this.CreatedByID = ko.observable(WFCommentDTO.CreatedByID);
            this.CreatedBy = ko.observable(WFCommentDTO.CreatedBy);
            this.RequestID = ko.observable(WFCommentDTO.RequestID);
            this.TaskID = ko.observable(WFCommentDTO.TaskID);
            this.WorkflowActivityID = ko.observable(WFCommentDTO.WorkflowActivityID);
            this.WorkflowActivity = ko.observable(WFCommentDTO.WorkflowActivity);
            this.ID = ko.observable(WFCommentDTO.ID);
            this.Timestamp = ko.observable(WFCommentDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class CommentViewModel extends EntityDtoWithIDViewModel {
    Comment;
    ItemID;
    ItemTitle;
    CreatedOn;
    CreatedByID;
    CreatedBy;
    constructor(CommentDTO) {
        super();
        if (CommentDTO == null) {
            this.Comment = ko.observable();
            this.ItemID = ko.observable();
            this.ItemTitle = ko.observable();
            this.CreatedOn = ko.observable();
            this.CreatedByID = ko.observable();
            this.CreatedBy = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Comment = ko.observable(CommentDTO.Comment);
            this.ItemID = ko.observable(CommentDTO.ItemID);
            this.ItemTitle = ko.observable(CommentDTO.ItemTitle);
            this.CreatedOn = ko.observable(CommentDTO.CreatedOn);
            this.CreatedByID = ko.observable(CommentDTO.CreatedByID);
            this.CreatedBy = ko.observable(CommentDTO.CreatedBy);
            this.ID = ko.observable(CommentDTO.ID);
            this.Timestamp = ko.observable(CommentDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class DocumentViewModel extends EntityDtoWithIDViewModel {
    Name;
    FileName;
    Viewable;
    MimeType;
    Kind;
    Data;
    Length;
    ItemID;
    constructor(DocumentDTO) {
        super();
        if (DocumentDTO == null) {
            this.Name = ko.observable();
            this.FileName = ko.observable();
            this.Viewable = ko.observable();
            this.MimeType = ko.observable();
            this.Kind = ko.observable();
            this.Data = ko.observable();
            this.Length = ko.observable();
            this.ItemID = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(DocumentDTO.Name);
            this.FileName = ko.observable(DocumentDTO.FileName);
            this.Viewable = ko.observable(DocumentDTO.Viewable);
            this.MimeType = ko.observable(DocumentDTO.MimeType);
            this.Kind = ko.observable(DocumentDTO.Kind);
            this.Data = ko.observable(DocumentDTO.Data);
            this.Length = ko.observable(DocumentDTO.Length);
            this.ItemID = ko.observable(DocumentDTO.ItemID);
            this.ID = ko.observable(DocumentDTO.ID);
            this.Timestamp = ko.observable(DocumentDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class ExtendedDocumentViewModel extends EntityDtoWithIDViewModel {
    Name;
    FileName;
    Viewable;
    MimeType;
    Kind;
    Length;
    ItemID;
    CreatedOn;
    ContentModifiedOn;
    ContentCreatedOn;
    ItemTitle;
    Description;
    ParentDocumentID;
    UploadedByID;
    UploadedBy;
    RevisionSetID;
    RevisionDescription;
    MajorVersion;
    MinorVersion;
    BuildVersion;
    RevisionVersion;
    TaskItemType;
    DocumentType;
    constructor(ExtendedDocumentDTO) {
        super();
        if (ExtendedDocumentDTO == null) {
            this.Name = ko.observable();
            this.FileName = ko.observable();
            this.Viewable = ko.observable();
            this.MimeType = ko.observable();
            this.Kind = ko.observable();
            this.Length = ko.observable();
            this.ItemID = ko.observable();
            this.CreatedOn = ko.observable();
            this.ContentModifiedOn = ko.observable();
            this.ContentCreatedOn = ko.observable();
            this.ItemTitle = ko.observable();
            this.Description = ko.observable();
            this.ParentDocumentID = ko.observable();
            this.UploadedByID = ko.observable();
            this.UploadedBy = ko.observable();
            this.RevisionSetID = ko.observable();
            this.RevisionDescription = ko.observable();
            this.MajorVersion = ko.observable();
            this.MinorVersion = ko.observable();
            this.BuildVersion = ko.observable();
            this.RevisionVersion = ko.observable();
            this.TaskItemType = ko.observable();
            this.DocumentType = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(ExtendedDocumentDTO.Name);
            this.FileName = ko.observable(ExtendedDocumentDTO.FileName);
            this.Viewable = ko.observable(ExtendedDocumentDTO.Viewable);
            this.MimeType = ko.observable(ExtendedDocumentDTO.MimeType);
            this.Kind = ko.observable(ExtendedDocumentDTO.Kind);
            this.Length = ko.observable(ExtendedDocumentDTO.Length);
            this.ItemID = ko.observable(ExtendedDocumentDTO.ItemID);
            this.CreatedOn = ko.observable(ExtendedDocumentDTO.CreatedOn);
            this.ContentModifiedOn = ko.observable(ExtendedDocumentDTO.ContentModifiedOn);
            this.ContentCreatedOn = ko.observable(ExtendedDocumentDTO.ContentCreatedOn);
            this.ItemTitle = ko.observable(ExtendedDocumentDTO.ItemTitle);
            this.Description = ko.observable(ExtendedDocumentDTO.Description);
            this.ParentDocumentID = ko.observable(ExtendedDocumentDTO.ParentDocumentID);
            this.UploadedByID = ko.observable(ExtendedDocumentDTO.UploadedByID);
            this.UploadedBy = ko.observable(ExtendedDocumentDTO.UploadedBy);
            this.RevisionSetID = ko.observable(ExtendedDocumentDTO.RevisionSetID);
            this.RevisionDescription = ko.observable(ExtendedDocumentDTO.RevisionDescription);
            this.MajorVersion = ko.observable(ExtendedDocumentDTO.MajorVersion);
            this.MinorVersion = ko.observable(ExtendedDocumentDTO.MinorVersion);
            this.BuildVersion = ko.observable(ExtendedDocumentDTO.BuildVersion);
            this.RevisionVersion = ko.observable(ExtendedDocumentDTO.RevisionVersion);
            this.TaskItemType = ko.observable(ExtendedDocumentDTO.TaskItemType);
            this.DocumentType = ko.observable(ExtendedDocumentDTO.DocumentType);
            this.ID = ko.observable(ExtendedDocumentDTO.ID);
            this.Timestamp = ko.observable(ExtendedDocumentDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            FileName: this.FileName(),
            Viewable: this.Viewable(),
            MimeType: this.MimeType(),
            Kind: this.Kind(),
            Length: this.Length(),
            ItemID: this.ItemID(),
            CreatedOn: this.CreatedOn(),
            ContentModifiedOn: this.ContentModifiedOn(),
            ContentCreatedOn: this.ContentCreatedOn(),
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
    }
}
export class OrganizationEHRSViewModel extends EntityDtoWithIDViewModel {
    OrganizationID;
    Type;
    System;
    Other;
    StartYear;
    EndYear;
    constructor(OrganizationEHRSDTO) {
        super();
        if (OrganizationEHRSDTO == null) {
            this.OrganizationID = ko.observable();
            this.Type = ko.observable();
            this.System = ko.observable();
            this.Other = ko.observable();
            this.StartYear = ko.observable();
            this.EndYear = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.OrganizationID = ko.observable(OrganizationEHRSDTO.OrganizationID);
            this.Type = ko.observable(OrganizationEHRSDTO.Type);
            this.System = ko.observable(OrganizationEHRSDTO.System);
            this.Other = ko.observable(OrganizationEHRSDTO.Other);
            this.StartYear = ko.observable(OrganizationEHRSDTO.StartYear);
            this.EndYear = ko.observable(OrganizationEHRSDTO.EndYear);
            this.ID = ko.observable(OrganizationEHRSDTO.ID);
            this.Timestamp = ko.observable(OrganizationEHRSDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class TemplateViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    CreatedByID;
    CreatedBy;
    CreatedOn;
    Data;
    Type;
    Notes;
    QueryType;
    ComposerInterface;
    Order;
    RequestTypeID;
    RequestType;
    constructor(TemplateDTO) {
        super();
        if (TemplateDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.CreatedByID = ko.observable();
            this.CreatedBy = ko.observable();
            this.CreatedOn = ko.observable();
            this.Data = ko.observable();
            this.Type = ko.observable();
            this.Notes = ko.observable();
            this.QueryType = ko.observable();
            this.ComposerInterface = ko.observable();
            this.Order = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestType = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(TemplateDTO.Name);
            this.Description = ko.observable(TemplateDTO.Description);
            this.CreatedByID = ko.observable(TemplateDTO.CreatedByID);
            this.CreatedBy = ko.observable(TemplateDTO.CreatedBy);
            this.CreatedOn = ko.observable(TemplateDTO.CreatedOn);
            this.Data = ko.observable(TemplateDTO.Data);
            this.Type = ko.observable(TemplateDTO.Type);
            this.Notes = ko.observable(TemplateDTO.Notes);
            this.QueryType = ko.observable(TemplateDTO.QueryType);
            this.ComposerInterface = ko.observable(TemplateDTO.ComposerInterface);
            this.Order = ko.observable(TemplateDTO.Order);
            this.RequestTypeID = ko.observable(TemplateDTO.RequestTypeID);
            this.RequestType = ko.observable(TemplateDTO.RequestType);
            this.ID = ko.observable(TemplateDTO.ID);
            this.Timestamp = ko.observable(TemplateDTO.Timestamp);
        }
    }
    toData() {
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
            Order: this.Order(),
            RequestTypeID: this.RequestTypeID(),
            RequestType: this.RequestType(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class TermViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    OID;
    ReferenceUrl;
    Type;
    constructor(TermDTO) {
        super();
        if (TermDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.OID = ko.observable();
            this.ReferenceUrl = ko.observable();
            this.Type = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(TermDTO.Name);
            this.Description = ko.observable(TermDTO.Description);
            this.OID = ko.observable(TermDTO.OID);
            this.ReferenceUrl = ko.observable(TermDTO.ReferenceUrl);
            this.Type = ko.observable(TermDTO.Type);
            this.ID = ko.observable(TermDTO.ID);
            this.Timestamp = ko.observable(TermDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            OID: this.OID(),
            ReferenceUrl: this.ReferenceUrl(),
            Type: this.Type(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class HomepageRequestDetailViewModel extends EntityDtoWithIDViewModel {
    Name;
    Identifier;
    SubmittedOn;
    SubmittedByName;
    SubmittedBy;
    SubmittedByID;
    StatusText;
    Status;
    RequestType;
    Project;
    Priority;
    DueDate;
    MSRequestID;
    IsWorkflowRequest;
    CanEditMetadata;
    constructor(HomepageRequestDetailDTO) {
        super();
        if (HomepageRequestDetailDTO == null) {
            this.Name = ko.observable();
            this.Identifier = ko.observable();
            this.SubmittedOn = ko.observable();
            this.SubmittedByName = ko.observable();
            this.SubmittedBy = ko.observable();
            this.SubmittedByID = ko.observable();
            this.StatusText = ko.observable();
            this.Status = ko.observable();
            this.RequestType = ko.observable();
            this.Project = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.MSRequestID = ko.observable();
            this.IsWorkflowRequest = ko.observable();
            this.CanEditMetadata = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(HomepageRequestDetailDTO.Name);
            this.Identifier = ko.observable(HomepageRequestDetailDTO.Identifier);
            this.SubmittedOn = ko.observable(HomepageRequestDetailDTO.SubmittedOn);
            this.SubmittedByName = ko.observable(HomepageRequestDetailDTO.SubmittedByName);
            this.SubmittedBy = ko.observable(HomepageRequestDetailDTO.SubmittedBy);
            this.SubmittedByID = ko.observable(HomepageRequestDetailDTO.SubmittedByID);
            this.StatusText = ko.observable(HomepageRequestDetailDTO.StatusText);
            this.Status = ko.observable(HomepageRequestDetailDTO.Status);
            this.RequestType = ko.observable(HomepageRequestDetailDTO.RequestType);
            this.Project = ko.observable(HomepageRequestDetailDTO.Project);
            this.Priority = ko.observable(HomepageRequestDetailDTO.Priority);
            this.DueDate = ko.observable(HomepageRequestDetailDTO.DueDate);
            this.MSRequestID = ko.observable(HomepageRequestDetailDTO.MSRequestID);
            this.IsWorkflowRequest = ko.observable(HomepageRequestDetailDTO.IsWorkflowRequest);
            this.CanEditMetadata = ko.observable(HomepageRequestDetailDTO.CanEditMetadata);
            this.ID = ko.observable(HomepageRequestDetailDTO.ID);
            this.Timestamp = ko.observable(HomepageRequestDetailDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class ReportAggregationLevelViewModel extends EntityDtoWithIDViewModel {
    NetworkID;
    Name;
    DeletedOn;
    constructor(ReportAggregationLevelDTO) {
        super();
        if (ReportAggregationLevelDTO == null) {
            this.NetworkID = ko.observable();
            this.Name = ko.observable();
            this.DeletedOn = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.NetworkID = ko.observable(ReportAggregationLevelDTO.NetworkID);
            this.Name = ko.observable(ReportAggregationLevelDTO.Name);
            this.DeletedOn = ko.observable(ReportAggregationLevelDTO.DeletedOn);
            this.ID = ko.observable(ReportAggregationLevelDTO.ID);
            this.Timestamp = ko.observable(ReportAggregationLevelDTO.Timestamp);
        }
    }
    toData() {
        return {
            NetworkID: this.NetworkID(),
            Name: this.Name(),
            DeletedOn: this.DeletedOn(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class RequestBudgetInfoViewModel extends EntityDtoWithIDViewModel {
    BudgetActivityID;
    BudgetActivityDescription;
    BudgetActivityProjectID;
    BudgetActivityProjectDescription;
    BudgetTaskOrderID;
    BudgetTaskOrderDescription;
    constructor(RequestBudgetInfoDTO) {
        super();
        if (RequestBudgetInfoDTO == null) {
            this.BudgetActivityID = ko.observable();
            this.BudgetActivityDescription = ko.observable();
            this.BudgetActivityProjectID = ko.observable();
            this.BudgetActivityProjectDescription = ko.observable();
            this.BudgetTaskOrderID = ko.observable();
            this.BudgetTaskOrderDescription = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.BudgetActivityID = ko.observable(RequestBudgetInfoDTO.BudgetActivityID);
            this.BudgetActivityDescription = ko.observable(RequestBudgetInfoDTO.BudgetActivityDescription);
            this.BudgetActivityProjectID = ko.observable(RequestBudgetInfoDTO.BudgetActivityProjectID);
            this.BudgetActivityProjectDescription = ko.observable(RequestBudgetInfoDTO.BudgetActivityProjectDescription);
            this.BudgetTaskOrderID = ko.observable(RequestBudgetInfoDTO.BudgetTaskOrderID);
            this.BudgetTaskOrderDescription = ko.observable(RequestBudgetInfoDTO.BudgetTaskOrderDescription);
            this.ID = ko.observable(RequestBudgetInfoDTO.ID);
            this.Timestamp = ko.observable(RequestBudgetInfoDTO.Timestamp);
        }
    }
    toData() {
        return {
            BudgetActivityID: this.BudgetActivityID(),
            BudgetActivityDescription: this.BudgetActivityDescription(),
            BudgetActivityProjectID: this.BudgetActivityProjectID(),
            BudgetActivityProjectDescription: this.BudgetActivityProjectDescription(),
            BudgetTaskOrderID: this.BudgetTaskOrderID(),
            BudgetTaskOrderDescription: this.BudgetTaskOrderDescription(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class RequestMetadataViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    DueDate;
    Priority;
    PurposeOfUse;
    PhiDisclosureLevel;
    RequesterCenterID;
    ActivityID;
    ActivityProjectID;
    TaskOrderID;
    SourceActivityID;
    SourceActivityProjectID;
    SourceTaskOrderID;
    WorkplanTypeID;
    MSRequestID;
    ReportAggregationLevelID;
    ApplyChangesToRoutings;
    constructor(RequestMetadataDTO) {
        super();
        if (RequestMetadataDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.DueDate = ko.observable();
            this.Priority = ko.observable();
            this.PurposeOfUse = ko.observable();
            this.PhiDisclosureLevel = ko.observable();
            this.RequesterCenterID = ko.observable();
            this.ActivityID = ko.observable();
            this.ActivityProjectID = ko.observable();
            this.TaskOrderID = ko.observable();
            this.SourceActivityID = ko.observable();
            this.SourceActivityProjectID = ko.observable();
            this.SourceTaskOrderID = ko.observable();
            this.WorkplanTypeID = ko.observable();
            this.MSRequestID = ko.observable();
            this.ReportAggregationLevelID = ko.observable();
            this.ApplyChangesToRoutings = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(RequestMetadataDTO.Name);
            this.Description = ko.observable(RequestMetadataDTO.Description);
            this.DueDate = ko.observable(RequestMetadataDTO.DueDate);
            this.Priority = ko.observable(RequestMetadataDTO.Priority);
            this.PurposeOfUse = ko.observable(RequestMetadataDTO.PurposeOfUse);
            this.PhiDisclosureLevel = ko.observable(RequestMetadataDTO.PhiDisclosureLevel);
            this.RequesterCenterID = ko.observable(RequestMetadataDTO.RequesterCenterID);
            this.ActivityID = ko.observable(RequestMetadataDTO.ActivityID);
            this.ActivityProjectID = ko.observable(RequestMetadataDTO.ActivityProjectID);
            this.TaskOrderID = ko.observable(RequestMetadataDTO.TaskOrderID);
            this.SourceActivityID = ko.observable(RequestMetadataDTO.SourceActivityID);
            this.SourceActivityProjectID = ko.observable(RequestMetadataDTO.SourceActivityProjectID);
            this.SourceTaskOrderID = ko.observable(RequestMetadataDTO.SourceTaskOrderID);
            this.WorkplanTypeID = ko.observable(RequestMetadataDTO.WorkplanTypeID);
            this.MSRequestID = ko.observable(RequestMetadataDTO.MSRequestID);
            this.ReportAggregationLevelID = ko.observable(RequestMetadataDTO.ReportAggregationLevelID);
            this.ApplyChangesToRoutings = ko.observable(RequestMetadataDTO.ApplyChangesToRoutings);
            this.ID = ko.observable(RequestMetadataDTO.ID);
            this.Timestamp = ko.observable(RequestMetadataDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class RequestObserverViewModel extends EntityDtoWithIDViewModel {
    RequestID;
    UserID;
    SecurityGroupID;
    DisplayName;
    Email;
    EventSubscriptions;
    constructor(RequestObserverDTO) {
        super();
        if (RequestObserverDTO == null) {
            this.RequestID = ko.observable();
            this.UserID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.DisplayName = ko.observable();
            this.Email = ko.observable();
            this.EventSubscriptions = ko.observableArray();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.RequestID = ko.observable(RequestObserverDTO.RequestID);
            this.UserID = ko.observable(RequestObserverDTO.UserID);
            this.SecurityGroupID = ko.observable(RequestObserverDTO.SecurityGroupID);
            this.DisplayName = ko.observable(RequestObserverDTO.DisplayName);
            this.Email = ko.observable(RequestObserverDTO.Email);
            this.EventSubscriptions = ko.observableArray(RequestObserverDTO.EventSubscriptions == null ? null : RequestObserverDTO.EventSubscriptions.map((item) => { return new RequestObserverEventSubscriptionViewModel(item); }));
            this.ID = ko.observable(RequestObserverDTO.ID);
            this.Timestamp = ko.observable(RequestObserverDTO.Timestamp);
        }
    }
    toData() {
        return {
            RequestID: this.RequestID(),
            UserID: this.UserID(),
            SecurityGroupID: this.SecurityGroupID(),
            DisplayName: this.DisplayName(),
            Email: this.Email(),
            EventSubscriptions: this.EventSubscriptions == null ? null : this.EventSubscriptions().map((item) => { return item.toData(); }),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class ResponseGroupViewModel extends EntityDtoWithIDViewModel {
    Name;
    constructor(ResponseGroupDTO) {
        super();
        if (ResponseGroupDTO == null) {
            this.Name = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(ResponseGroupDTO.Name);
            this.ID = ko.observable(ResponseGroupDTO.ID);
            this.Timestamp = ko.observable(ResponseGroupDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class AclGlobalFieldOptionViewModel extends BaseFieldOptionAclViewModel {
    constructor(AclGlobalFieldOptionDTO) {
        super();
        if (AclGlobalFieldOptionDTO == null) {
            this.FieldIdentifier = ko.observable();
            this.Permission = ko.observable();
            this.Overridden = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
        }
        else {
            this.FieldIdentifier = ko.observable(AclGlobalFieldOptionDTO.FieldIdentifier);
            this.Permission = ko.observable(AclGlobalFieldOptionDTO.Permission);
            this.Overridden = ko.observable(AclGlobalFieldOptionDTO.Overridden);
            this.SecurityGroupID = ko.observable(AclGlobalFieldOptionDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclGlobalFieldOptionDTO.SecurityGroup);
        }
    }
    toData() {
        return {
            FieldIdentifier: this.FieldIdentifier(),
            Permission: this.Permission(),
            Overridden: this.Overridden(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
        };
    }
}
export class AclProjectFieldOptionViewModel extends BaseFieldOptionAclViewModel {
    ProjectID;
    constructor(AclProjectFieldOptionDTO) {
        super();
        if (AclProjectFieldOptionDTO == null) {
            this.ProjectID = ko.observable();
            this.FieldIdentifier = ko.observable();
            this.Permission = ko.observable();
            this.Overridden = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectFieldOptionDTO.ProjectID);
            this.FieldIdentifier = ko.observable(AclProjectFieldOptionDTO.FieldIdentifier);
            this.Permission = ko.observable(AclProjectFieldOptionDTO.Permission);
            this.Overridden = ko.observable(AclProjectFieldOptionDTO.Overridden);
            this.SecurityGroupID = ko.observable(AclProjectFieldOptionDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectFieldOptionDTO.SecurityGroup);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            FieldIdentifier: this.FieldIdentifier(),
            Permission: this.Permission(),
            Overridden: this.Overridden(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
        };
    }
}
export class BaseAclRequestTypeViewModel extends BaseAclViewModel {
    RequestTypeID;
    Permission;
    constructor(BaseAclRequestTypeDTO) {
        super();
        if (BaseAclRequestTypeDTO == null) {
            this.RequestTypeID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.RequestTypeID = ko.observable(BaseAclRequestTypeDTO.RequestTypeID);
            this.Permission = ko.observable(BaseAclRequestTypeDTO.Permission);
            this.SecurityGroupID = ko.observable(BaseAclRequestTypeDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(BaseAclRequestTypeDTO.SecurityGroup);
            this.Overridden = ko.observable(BaseAclRequestTypeDTO.Overridden);
        }
    }
    toData() {
        return {
            RequestTypeID: this.RequestTypeID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class SecurityEntityViewModel extends EntityDtoWithIDViewModel {
    Name;
    Type;
    constructor(SecurityEntityDTO) {
        super();
        if (SecurityEntityDTO == null) {
            this.Name = ko.observable();
            this.Type = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(SecurityEntityDTO.Name);
            this.Type = ko.observable(SecurityEntityDTO.Type);
            this.ID = ko.observable(SecurityEntityDTO.ID);
            this.Timestamp = ko.observable(SecurityEntityDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Type: this.Type(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class TaskViewModel extends EntityDtoWithIDViewModel {
    Subject;
    Location;
    Body;
    DueDate;
    CreatedOn;
    StartOn;
    EndOn;
    EstimatedCompletedOn;
    Priority;
    Status;
    Type;
    PercentComplete;
    WorkflowActivityID;
    DirectToRequest;
    constructor(TaskDTO) {
        super();
        if (TaskDTO == null) {
            this.Subject = ko.observable();
            this.Location = ko.observable();
            this.Body = ko.observable();
            this.DueDate = ko.observable();
            this.CreatedOn = ko.observable();
            this.StartOn = ko.observable();
            this.EndOn = ko.observable();
            this.EstimatedCompletedOn = ko.observable();
            this.Priority = ko.observable();
            this.Status = ko.observable();
            this.Type = ko.observable();
            this.PercentComplete = ko.observable();
            this.WorkflowActivityID = ko.observable();
            this.DirectToRequest = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Subject = ko.observable(TaskDTO.Subject);
            this.Location = ko.observable(TaskDTO.Location);
            this.Body = ko.observable(TaskDTO.Body);
            this.DueDate = ko.observable(TaskDTO.DueDate);
            this.CreatedOn = ko.observable(TaskDTO.CreatedOn);
            this.StartOn = ko.observable(TaskDTO.StartOn);
            this.EndOn = ko.observable(TaskDTO.EndOn);
            this.EstimatedCompletedOn = ko.observable(TaskDTO.EstimatedCompletedOn);
            this.Priority = ko.observable(TaskDTO.Priority);
            this.Status = ko.observable(TaskDTO.Status);
            this.Type = ko.observable(TaskDTO.Type);
            this.PercentComplete = ko.observable(TaskDTO.PercentComplete);
            this.WorkflowActivityID = ko.observable(TaskDTO.WorkflowActivityID);
            this.DirectToRequest = ko.observable(TaskDTO.DirectToRequest);
            this.ID = ko.observable(TaskDTO.ID);
            this.Timestamp = ko.observable(TaskDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class DataModelViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    RequiresConfiguration;
    QueryComposer;
    constructor(DataModelDTO) {
        super();
        if (DataModelDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.RequiresConfiguration = ko.observable();
            this.QueryComposer = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(DataModelDTO.Name);
            this.Description = ko.observable(DataModelDTO.Description);
            this.RequiresConfiguration = ko.observable(DataModelDTO.RequiresConfiguration);
            this.QueryComposer = ko.observable(DataModelDTO.QueryComposer);
            this.ID = ko.observable(DataModelDTO.ID);
            this.Timestamp = ko.observable(DataModelDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            RequiresConfiguration: this.RequiresConfiguration(),
            QueryComposer: this.QueryComposer(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class DataMartListViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    Acronym;
    StartDate;
    EndDate;
    OrganizationID;
    Organization;
    ParentOrganziationID;
    ParentOrganization;
    Priority;
    DueDate;
    constructor(DataMartListDTO) {
        super();
        if (DataMartListDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Acronym = ko.observable();
            this.StartDate = ko.observable();
            this.EndDate = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.ParentOrganziationID = ko.observable();
            this.ParentOrganization = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(DataMartListDTO.Name);
            this.Description = ko.observable(DataMartListDTO.Description);
            this.Acronym = ko.observable(DataMartListDTO.Acronym);
            this.StartDate = ko.observable(DataMartListDTO.StartDate);
            this.EndDate = ko.observable(DataMartListDTO.EndDate);
            this.OrganizationID = ko.observable(DataMartListDTO.OrganizationID);
            this.Organization = ko.observable(DataMartListDTO.Organization);
            this.ParentOrganziationID = ko.observable(DataMartListDTO.ParentOrganziationID);
            this.ParentOrganization = ko.observable(DataMartListDTO.ParentOrganization);
            this.Priority = ko.observable(DataMartListDTO.Priority);
            this.DueDate = ko.observable(DataMartListDTO.DueDate);
            this.ID = ko.observable(DataMartListDTO.ID);
            this.Timestamp = ko.observable(DataMartListDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class EventViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    Locations;
    SupportsMyNotifications;
    constructor(EventDTO) {
        super();
        if (EventDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Locations = ko.observableArray();
            this.SupportsMyNotifications = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(EventDTO.Name);
            this.Description = ko.observable(EventDTO.Description);
            this.Locations = ko.observableArray(EventDTO.Locations == null ? null : EventDTO.Locations.map((item) => { return item; }));
            this.SupportsMyNotifications = ko.observable(EventDTO.SupportsMyNotifications);
            this.ID = ko.observable(EventDTO.ID);
            this.Timestamp = ko.observable(EventDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Locations: this.Locations(),
            SupportsMyNotifications: this.SupportsMyNotifications(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class GroupEventViewModel extends BaseEventPermissionViewModel {
    GroupID;
    constructor(GroupEventDTO) {
        super();
        if (GroupEventDTO == null) {
            this.GroupID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.GroupID = ko.observable(GroupEventDTO.GroupID);
            this.SecurityGroupID = ko.observable(GroupEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(GroupEventDTO.SecurityGroup);
            this.Allowed = ko.observable(GroupEventDTO.Allowed);
            this.Overridden = ko.observable(GroupEventDTO.Overridden);
            this.EventID = ko.observable(GroupEventDTO.EventID);
            this.Event = ko.observable(GroupEventDTO.Event);
        }
    }
    toData() {
        return {
            GroupID: this.GroupID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class OrganizationEventViewModel extends BaseEventPermissionViewModel {
    OrganizationID;
    constructor(OrganizationEventDTO) {
        super();
        if (OrganizationEventDTO == null) {
            this.OrganizationID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.OrganizationID = ko.observable(OrganizationEventDTO.OrganizationID);
            this.SecurityGroupID = ko.observable(OrganizationEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(OrganizationEventDTO.SecurityGroup);
            this.Allowed = ko.observable(OrganizationEventDTO.Allowed);
            this.Overridden = ko.observable(OrganizationEventDTO.Overridden);
            this.EventID = ko.observable(OrganizationEventDTO.EventID);
            this.Event = ko.observable(OrganizationEventDTO.Event);
        }
    }
    toData() {
        return {
            OrganizationID: this.OrganizationID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class RegistryEventViewModel extends BaseEventPermissionViewModel {
    RegistryID;
    constructor(RegistryEventDTO) {
        super();
        if (RegistryEventDTO == null) {
            this.RegistryID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.RegistryID = ko.observable(RegistryEventDTO.RegistryID);
            this.SecurityGroupID = ko.observable(RegistryEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(RegistryEventDTO.SecurityGroup);
            this.Allowed = ko.observable(RegistryEventDTO.Allowed);
            this.Overridden = ko.observable(RegistryEventDTO.Overridden);
            this.EventID = ko.observable(RegistryEventDTO.EventID);
            this.Event = ko.observable(RegistryEventDTO.Event);
        }
    }
    toData() {
        return {
            RegistryID: this.RegistryID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class UserEventViewModel extends BaseEventPermissionViewModel {
    UserID;
    constructor(UserEventDTO) {
        super();
        if (UserEventDTO == null) {
            this.UserID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.UserID = ko.observable(UserEventDTO.UserID);
            this.SecurityGroupID = ko.observable(UserEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(UserEventDTO.SecurityGroup);
            this.Allowed = ko.observable(UserEventDTO.Allowed);
            this.Overridden = ko.observable(UserEventDTO.Overridden);
            this.EventID = ko.observable(UserEventDTO.EventID);
            this.Event = ko.observable(UserEventDTO.Event);
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class GroupViewModel extends EntityDtoWithIDViewModel {
    Name;
    Deleted;
    ApprovalRequired;
    constructor(GroupDTO) {
        super();
        if (GroupDTO == null) {
            this.Name = ko.observable();
            this.Deleted = ko.observable();
            this.ApprovalRequired = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(GroupDTO.Name);
            this.Deleted = ko.observable(GroupDTO.Deleted);
            this.ApprovalRequired = ko.observable(GroupDTO.ApprovalRequired);
            this.ID = ko.observable(GroupDTO.ID);
            this.Timestamp = ko.observable(GroupDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Deleted: this.Deleted(),
            ApprovalRequired: this.ApprovalRequired(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class NetworkMessageViewModel extends EntityDtoWithIDViewModel {
    Subject;
    MessageText;
    CreatedOn;
    Targets;
    constructor(NetworkMessageDTO) {
        super();
        if (NetworkMessageDTO == null) {
            this.Subject = ko.observable();
            this.MessageText = ko.observable();
            this.CreatedOn = ko.observable();
            this.Targets = ko.observableArray();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Subject = ko.observable(NetworkMessageDTO.Subject);
            this.MessageText = ko.observable(NetworkMessageDTO.MessageText);
            this.CreatedOn = ko.observable(NetworkMessageDTO.CreatedOn);
            this.Targets = ko.observableArray(NetworkMessageDTO.Targets == null ? null : NetworkMessageDTO.Targets.map((item) => { return item; }));
            this.ID = ko.observable(NetworkMessageDTO.ID);
            this.Timestamp = ko.observable(NetworkMessageDTO.Timestamp);
        }
    }
    toData() {
        return {
            Subject: this.Subject(),
            MessageText: this.MessageText(),
            CreatedOn: this.CreatedOn(),
            Targets: this.Targets(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class OrganizationViewModel extends EntityDtoWithIDViewModel {
    Name;
    Acronym;
    Deleted;
    Primary;
    ParentOrganizationID;
    ParentOrganization;
    ContactEmail;
    ContactFirstName;
    ContactLastName;
    ContactPhone;
    SpecialRequirements;
    UsageRestrictions;
    OrganizationDescription;
    PragmaticClinicalTrials;
    ObservationalParticipation;
    ProspectiveTrials;
    EnableClaimsAndBilling;
    EnableEHRA;
    EnableRegistries;
    DataModelMSCDM;
    DataModelHMORNVDW;
    DataModelESP;
    DataModelI2B2;
    DataModelOMOP;
    DataModelPCORI;
    DataModelOther;
    DataModelOtherText;
    InpatientClaims;
    OutpatientClaims;
    OutpatientPharmacyClaims;
    EnrollmentClaims;
    DemographicsClaims;
    LaboratoryResultsClaims;
    VitalSignsClaims;
    OtherClaims;
    OtherClaimsText;
    Biorepositories;
    PatientReportedOutcomes;
    PatientReportedBehaviors;
    PrescriptionOrders;
    X509PublicKey;
    constructor(OrganizationDTO) {
        super();
        if (OrganizationDTO == null) {
            this.Name = ko.observable();
            this.Acronym = ko.observable();
            this.Deleted = ko.observable();
            this.Primary = ko.observable();
            this.ParentOrganizationID = ko.observable();
            this.ParentOrganization = ko.observable();
            this.ContactEmail = ko.observable();
            this.ContactFirstName = ko.observable();
            this.ContactLastName = ko.observable();
            this.ContactPhone = ko.observable();
            this.SpecialRequirements = ko.observable();
            this.UsageRestrictions = ko.observable();
            this.OrganizationDescription = ko.observable();
            this.PragmaticClinicalTrials = ko.observable();
            this.ObservationalParticipation = ko.observable();
            this.ProspectiveTrials = ko.observable();
            this.EnableClaimsAndBilling = ko.observable();
            this.EnableEHRA = ko.observable();
            this.EnableRegistries = ko.observable();
            this.DataModelMSCDM = ko.observable();
            this.DataModelHMORNVDW = ko.observable();
            this.DataModelESP = ko.observable();
            this.DataModelI2B2 = ko.observable();
            this.DataModelOMOP = ko.observable();
            this.DataModelPCORI = ko.observable();
            this.DataModelOther = ko.observable();
            this.DataModelOtherText = ko.observable();
            this.InpatientClaims = ko.observable();
            this.OutpatientClaims = ko.observable();
            this.OutpatientPharmacyClaims = ko.observable();
            this.EnrollmentClaims = ko.observable();
            this.DemographicsClaims = ko.observable();
            this.LaboratoryResultsClaims = ko.observable();
            this.VitalSignsClaims = ko.observable();
            this.OtherClaims = ko.observable();
            this.OtherClaimsText = ko.observable();
            this.Biorepositories = ko.observable();
            this.PatientReportedOutcomes = ko.observable();
            this.PatientReportedBehaviors = ko.observable();
            this.PrescriptionOrders = ko.observable();
            this.X509PublicKey = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(OrganizationDTO.Name);
            this.Acronym = ko.observable(OrganizationDTO.Acronym);
            this.Deleted = ko.observable(OrganizationDTO.Deleted);
            this.Primary = ko.observable(OrganizationDTO.Primary);
            this.ParentOrganizationID = ko.observable(OrganizationDTO.ParentOrganizationID);
            this.ParentOrganization = ko.observable(OrganizationDTO.ParentOrganization);
            this.ContactEmail = ko.observable(OrganizationDTO.ContactEmail);
            this.ContactFirstName = ko.observable(OrganizationDTO.ContactFirstName);
            this.ContactLastName = ko.observable(OrganizationDTO.ContactLastName);
            this.ContactPhone = ko.observable(OrganizationDTO.ContactPhone);
            this.SpecialRequirements = ko.observable(OrganizationDTO.SpecialRequirements);
            this.UsageRestrictions = ko.observable(OrganizationDTO.UsageRestrictions);
            this.OrganizationDescription = ko.observable(OrganizationDTO.OrganizationDescription);
            this.PragmaticClinicalTrials = ko.observable(OrganizationDTO.PragmaticClinicalTrials);
            this.ObservationalParticipation = ko.observable(OrganizationDTO.ObservationalParticipation);
            this.ProspectiveTrials = ko.observable(OrganizationDTO.ProspectiveTrials);
            this.EnableClaimsAndBilling = ko.observable(OrganizationDTO.EnableClaimsAndBilling);
            this.EnableEHRA = ko.observable(OrganizationDTO.EnableEHRA);
            this.EnableRegistries = ko.observable(OrganizationDTO.EnableRegistries);
            this.DataModelMSCDM = ko.observable(OrganizationDTO.DataModelMSCDM);
            this.DataModelHMORNVDW = ko.observable(OrganizationDTO.DataModelHMORNVDW);
            this.DataModelESP = ko.observable(OrganizationDTO.DataModelESP);
            this.DataModelI2B2 = ko.observable(OrganizationDTO.DataModelI2B2);
            this.DataModelOMOP = ko.observable(OrganizationDTO.DataModelOMOP);
            this.DataModelPCORI = ko.observable(OrganizationDTO.DataModelPCORI);
            this.DataModelOther = ko.observable(OrganizationDTO.DataModelOther);
            this.DataModelOtherText = ko.observable(OrganizationDTO.DataModelOtherText);
            this.InpatientClaims = ko.observable(OrganizationDTO.InpatientClaims);
            this.OutpatientClaims = ko.observable(OrganizationDTO.OutpatientClaims);
            this.OutpatientPharmacyClaims = ko.observable(OrganizationDTO.OutpatientPharmacyClaims);
            this.EnrollmentClaims = ko.observable(OrganizationDTO.EnrollmentClaims);
            this.DemographicsClaims = ko.observable(OrganizationDTO.DemographicsClaims);
            this.LaboratoryResultsClaims = ko.observable(OrganizationDTO.LaboratoryResultsClaims);
            this.VitalSignsClaims = ko.observable(OrganizationDTO.VitalSignsClaims);
            this.OtherClaims = ko.observable(OrganizationDTO.OtherClaims);
            this.OtherClaimsText = ko.observable(OrganizationDTO.OtherClaimsText);
            this.Biorepositories = ko.observable(OrganizationDTO.Biorepositories);
            this.PatientReportedOutcomes = ko.observable(OrganizationDTO.PatientReportedOutcomes);
            this.PatientReportedBehaviors = ko.observable(OrganizationDTO.PatientReportedBehaviors);
            this.PrescriptionOrders = ko.observable(OrganizationDTO.PrescriptionOrders);
            this.X509PublicKey = ko.observable(OrganizationDTO.X509PublicKey);
            this.ID = ko.observable(OrganizationDTO.ID);
            this.Timestamp = ko.observable(OrganizationDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class ProjectViewModel extends EntityDtoWithIDViewModel {
    Name;
    Acronym;
    StartDate;
    EndDate;
    Deleted;
    Active;
    Description;
    GroupID;
    Group;
    constructor(ProjectDTO) {
        super();
        if (ProjectDTO == null) {
            this.Name = ko.observable();
            this.Acronym = ko.observable();
            this.StartDate = ko.observable();
            this.EndDate = ko.observable();
            this.Deleted = ko.observable();
            this.Active = ko.observable();
            this.Description = ko.observable();
            this.GroupID = ko.observable();
            this.Group = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(ProjectDTO.Name);
            this.Acronym = ko.observable(ProjectDTO.Acronym);
            this.StartDate = ko.observable(ProjectDTO.StartDate);
            this.EndDate = ko.observable(ProjectDTO.EndDate);
            this.Deleted = ko.observable(ProjectDTO.Deleted);
            this.Active = ko.observable(ProjectDTO.Active);
            this.Description = ko.observable(ProjectDTO.Description);
            this.GroupID = ko.observable(ProjectDTO.GroupID);
            this.Group = ko.observable(ProjectDTO.Group);
            this.ID = ko.observable(ProjectDTO.ID);
            this.Timestamp = ko.observable(ProjectDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class RegistryViewModel extends EntityDtoWithIDViewModel {
    Deleted;
    Type;
    Name;
    Description;
    RoPRUrl;
    constructor(RegistryDTO) {
        super();
        if (RegistryDTO == null) {
            this.Deleted = ko.observable();
            this.Type = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.RoPRUrl = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Deleted = ko.observable(RegistryDTO.Deleted);
            this.Type = ko.observable(RegistryDTO.Type);
            this.Name = ko.observable(RegistryDTO.Name);
            this.Description = ko.observable(RegistryDTO.Description);
            this.RoPRUrl = ko.observable(RegistryDTO.RoPRUrl);
            this.ID = ko.observable(RegistryDTO.ID);
            this.Timestamp = ko.observable(RegistryDTO.Timestamp);
        }
    }
    toData() {
        return {
            Deleted: this.Deleted(),
            Type: this.Type(),
            Name: this.Name(),
            Description: this.Description(),
            RoPRUrl: this.RoPRUrl(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class RequestDataMartViewModel extends EntityDtoWithIDViewModel {
    RequestID;
    DataMartID;
    DataMart;
    Status;
    Priority;
    DueDate;
    ErrorMessage;
    ErrorDetail;
    RejectReason;
    ResultsGrouped;
    Properties;
    RoutingType;
    ResponseID;
    ResponseGroupID;
    ResponseGroup;
    ResponseMessage;
    ResponseSubmittedOn;
    ResponseSubmittedByID;
    ResponseSubmittedBy;
    ResponseTime;
    constructor(RequestDataMartDTO) {
        super();
        if (RequestDataMartDTO == null) {
            this.RequestID = ko.observable();
            this.DataMartID = ko.observable();
            this.DataMart = ko.observable();
            this.Status = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.ErrorMessage = ko.observable();
            this.ErrorDetail = ko.observable();
            this.RejectReason = ko.observable();
            this.ResultsGrouped = ko.observable();
            this.Properties = ko.observable();
            this.RoutingType = ko.observable();
            this.ResponseID = ko.observable();
            this.ResponseGroupID = ko.observable();
            this.ResponseGroup = ko.observable();
            this.ResponseMessage = ko.observable();
            this.ResponseSubmittedOn = ko.observable();
            this.ResponseSubmittedByID = ko.observable();
            this.ResponseSubmittedBy = ko.observable();
            this.ResponseTime = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.RequestID = ko.observable(RequestDataMartDTO.RequestID);
            this.DataMartID = ko.observable(RequestDataMartDTO.DataMartID);
            this.DataMart = ko.observable(RequestDataMartDTO.DataMart);
            this.Status = ko.observable(RequestDataMartDTO.Status);
            this.Priority = ko.observable(RequestDataMartDTO.Priority);
            this.DueDate = ko.observable(RequestDataMartDTO.DueDate);
            this.ErrorMessage = ko.observable(RequestDataMartDTO.ErrorMessage);
            this.ErrorDetail = ko.observable(RequestDataMartDTO.ErrorDetail);
            this.RejectReason = ko.observable(RequestDataMartDTO.RejectReason);
            this.ResultsGrouped = ko.observable(RequestDataMartDTO.ResultsGrouped);
            this.Properties = ko.observable(RequestDataMartDTO.Properties);
            this.RoutingType = ko.observable(RequestDataMartDTO.RoutingType);
            this.ResponseID = ko.observable(RequestDataMartDTO.ResponseID);
            this.ResponseGroupID = ko.observable(RequestDataMartDTO.ResponseGroupID);
            this.ResponseGroup = ko.observable(RequestDataMartDTO.ResponseGroup);
            this.ResponseMessage = ko.observable(RequestDataMartDTO.ResponseMessage);
            this.ResponseSubmittedOn = ko.observable(RequestDataMartDTO.ResponseSubmittedOn);
            this.ResponseSubmittedByID = ko.observable(RequestDataMartDTO.ResponseSubmittedByID);
            this.ResponseSubmittedBy = ko.observable(RequestDataMartDTO.ResponseSubmittedBy);
            this.ResponseTime = ko.observable(RequestDataMartDTO.ResponseTime);
            this.ID = ko.observable(RequestDataMartDTO.ID);
            this.Timestamp = ko.observable(RequestDataMartDTO.Timestamp);
        }
    }
    toData() {
        return {
            RequestID: this.RequestID(),
            DataMartID: this.DataMartID(),
            DataMart: this.DataMart(),
            Status: this.Status(),
            Priority: this.Priority(),
            DueDate: this.DueDate(),
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
            ResponseSubmittedOn: this.ResponseSubmittedOn(),
            ResponseSubmittedByID: this.ResponseSubmittedByID(),
            ResponseSubmittedBy: this.ResponseSubmittedBy(),
            ResponseTime: this.ResponseTime(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class RequestViewModel extends EntityDtoWithIDViewModel {
    Identifier;
    MSRequestID;
    ProjectID;
    Project;
    Name;
    Description;
    AdditionalInstructions;
    UpdatedOn;
    UpdatedByID;
    UpdatedBy;
    MirrorBudgetFields;
    Scheduled;
    Template;
    Deleted;
    Priority;
    OrganizationID;
    Organization;
    PurposeOfUse;
    PhiDisclosureLevel;
    ReportAggregationLevelID;
    ReportAggregationLevel;
    Schedule;
    ScheduleCount;
    SubmittedOn;
    SubmittedByID;
    SubmittedByName;
    SubmittedBy;
    RequestTypeID;
    RequestType;
    AdapterPackageVersion;
    IRBApprovalNo;
    DueDate;
    ActivityDescription;
    ActivityID;
    SourceActivityID;
    SourceActivityProjectID;
    SourceTaskOrderID;
    RequesterCenterID;
    RequesterCenter;
    WorkplanTypeID;
    WorkplanType;
    WorkflowID;
    Workflow;
    CurrentWorkFlowActivityID;
    CurrentWorkFlowActivity;
    Status;
    StatusText;
    MajorEventDate;
    MajorEventByID;
    MajorEventBy;
    CreatedOn;
    CreatedByID;
    CreatedBy;
    CompletedOn;
    CancelledOn;
    UserIdentifier;
    Query;
    ParentRequestID;
    constructor(RequestDTO) {
        super();
        if (RequestDTO == null) {
            this.Identifier = ko.observable();
            this.MSRequestID = ko.observable();
            this.ProjectID = ko.observable();
            this.Project = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.AdditionalInstructions = ko.observable();
            this.UpdatedOn = ko.observable();
            this.UpdatedByID = ko.observable();
            this.UpdatedBy = ko.observable();
            this.MirrorBudgetFields = ko.observable();
            this.Scheduled = ko.observable();
            this.Template = ko.observable();
            this.Deleted = ko.observable();
            this.Priority = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.PurposeOfUse = ko.observable();
            this.PhiDisclosureLevel = ko.observable();
            this.ReportAggregationLevelID = ko.observable();
            this.ReportAggregationLevel = ko.observable();
            this.Schedule = ko.observable();
            this.ScheduleCount = ko.observable();
            this.SubmittedOn = ko.observable();
            this.SubmittedByID = ko.observable();
            this.SubmittedByName = ko.observable();
            this.SubmittedBy = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestType = ko.observable();
            this.AdapterPackageVersion = ko.observable();
            this.IRBApprovalNo = ko.observable();
            this.DueDate = ko.observable();
            this.ActivityDescription = ko.observable();
            this.ActivityID = ko.observable();
            this.SourceActivityID = ko.observable();
            this.SourceActivityProjectID = ko.observable();
            this.SourceTaskOrderID = ko.observable();
            this.RequesterCenterID = ko.observable();
            this.RequesterCenter = ko.observable();
            this.WorkplanTypeID = ko.observable();
            this.WorkplanType = ko.observable();
            this.WorkflowID = ko.observable();
            this.Workflow = ko.observable();
            this.CurrentWorkFlowActivityID = ko.observable();
            this.CurrentWorkFlowActivity = ko.observable();
            this.Status = ko.observable();
            this.StatusText = ko.observable();
            this.MajorEventDate = ko.observable();
            this.MajorEventByID = ko.observable();
            this.MajorEventBy = ko.observable();
            this.CreatedOn = ko.observable();
            this.CreatedByID = ko.observable();
            this.CreatedBy = ko.observable();
            this.CompletedOn = ko.observable();
            this.CancelledOn = ko.observable();
            this.UserIdentifier = ko.observable();
            this.Query = ko.observable();
            this.ParentRequestID = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Identifier = ko.observable(RequestDTO.Identifier);
            this.MSRequestID = ko.observable(RequestDTO.MSRequestID);
            this.ProjectID = ko.observable(RequestDTO.ProjectID);
            this.Project = ko.observable(RequestDTO.Project);
            this.Name = ko.observable(RequestDTO.Name);
            this.Description = ko.observable(RequestDTO.Description);
            this.AdditionalInstructions = ko.observable(RequestDTO.AdditionalInstructions);
            this.UpdatedOn = ko.observable(RequestDTO.UpdatedOn);
            this.UpdatedByID = ko.observable(RequestDTO.UpdatedByID);
            this.UpdatedBy = ko.observable(RequestDTO.UpdatedBy);
            this.MirrorBudgetFields = ko.observable(RequestDTO.MirrorBudgetFields);
            this.Scheduled = ko.observable(RequestDTO.Scheduled);
            this.Template = ko.observable(RequestDTO.Template);
            this.Deleted = ko.observable(RequestDTO.Deleted);
            this.Priority = ko.observable(RequestDTO.Priority);
            this.OrganizationID = ko.observable(RequestDTO.OrganizationID);
            this.Organization = ko.observable(RequestDTO.Organization);
            this.PurposeOfUse = ko.observable(RequestDTO.PurposeOfUse);
            this.PhiDisclosureLevel = ko.observable(RequestDTO.PhiDisclosureLevel);
            this.ReportAggregationLevelID = ko.observable(RequestDTO.ReportAggregationLevelID);
            this.ReportAggregationLevel = ko.observable(RequestDTO.ReportAggregationLevel);
            this.Schedule = ko.observable(RequestDTO.Schedule);
            this.ScheduleCount = ko.observable(RequestDTO.ScheduleCount);
            this.SubmittedOn = ko.observable(RequestDTO.SubmittedOn);
            this.SubmittedByID = ko.observable(RequestDTO.SubmittedByID);
            this.SubmittedByName = ko.observable(RequestDTO.SubmittedByName);
            this.SubmittedBy = ko.observable(RequestDTO.SubmittedBy);
            this.RequestTypeID = ko.observable(RequestDTO.RequestTypeID);
            this.RequestType = ko.observable(RequestDTO.RequestType);
            this.AdapterPackageVersion = ko.observable(RequestDTO.AdapterPackageVersion);
            this.IRBApprovalNo = ko.observable(RequestDTO.IRBApprovalNo);
            this.DueDate = ko.observable(RequestDTO.DueDate);
            this.ActivityDescription = ko.observable(RequestDTO.ActivityDescription);
            this.ActivityID = ko.observable(RequestDTO.ActivityID);
            this.SourceActivityID = ko.observable(RequestDTO.SourceActivityID);
            this.SourceActivityProjectID = ko.observable(RequestDTO.SourceActivityProjectID);
            this.SourceTaskOrderID = ko.observable(RequestDTO.SourceTaskOrderID);
            this.RequesterCenterID = ko.observable(RequestDTO.RequesterCenterID);
            this.RequesterCenter = ko.observable(RequestDTO.RequesterCenter);
            this.WorkplanTypeID = ko.observable(RequestDTO.WorkplanTypeID);
            this.WorkplanType = ko.observable(RequestDTO.WorkplanType);
            this.WorkflowID = ko.observable(RequestDTO.WorkflowID);
            this.Workflow = ko.observable(RequestDTO.Workflow);
            this.CurrentWorkFlowActivityID = ko.observable(RequestDTO.CurrentWorkFlowActivityID);
            this.CurrentWorkFlowActivity = ko.observable(RequestDTO.CurrentWorkFlowActivity);
            this.Status = ko.observable(RequestDTO.Status);
            this.StatusText = ko.observable(RequestDTO.StatusText);
            this.MajorEventDate = ko.observable(RequestDTO.MajorEventDate);
            this.MajorEventByID = ko.observable(RequestDTO.MajorEventByID);
            this.MajorEventBy = ko.observable(RequestDTO.MajorEventBy);
            this.CreatedOn = ko.observable(RequestDTO.CreatedOn);
            this.CreatedByID = ko.observable(RequestDTO.CreatedByID);
            this.CreatedBy = ko.observable(RequestDTO.CreatedBy);
            this.CompletedOn = ko.observable(RequestDTO.CompletedOn);
            this.CancelledOn = ko.observable(RequestDTO.CancelledOn);
            this.UserIdentifier = ko.observable(RequestDTO.UserIdentifier);
            this.Query = ko.observable(RequestDTO.Query);
            this.ParentRequestID = ko.observable(RequestDTO.ParentRequestID);
            this.ID = ko.observable(RequestDTO.ID);
            this.Timestamp = ko.observable(RequestDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class RequestTypeViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    Metadata;
    PostProcess;
    AddFiles;
    RequiresProcessing;
    Notes;
    WorkflowID;
    Workflow;
    SupportMultiQuery;
    constructor(RequestTypeDTO) {
        super();
        if (RequestTypeDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Metadata = ko.observable();
            this.PostProcess = ko.observable();
            this.AddFiles = ko.observable();
            this.RequiresProcessing = ko.observable();
            this.Notes = ko.observable();
            this.WorkflowID = ko.observable();
            this.Workflow = ko.observable();
            this.SupportMultiQuery = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(RequestTypeDTO.Name);
            this.Description = ko.observable(RequestTypeDTO.Description);
            this.Metadata = ko.observable(RequestTypeDTO.Metadata);
            this.PostProcess = ko.observable(RequestTypeDTO.PostProcess);
            this.AddFiles = ko.observable(RequestTypeDTO.AddFiles);
            this.RequiresProcessing = ko.observable(RequestTypeDTO.RequiresProcessing);
            this.Notes = ko.observable(RequestTypeDTO.Notes);
            this.WorkflowID = ko.observable(RequestTypeDTO.WorkflowID);
            this.Workflow = ko.observable(RequestTypeDTO.Workflow);
            this.SupportMultiQuery = ko.observable(RequestTypeDTO.SupportMultiQuery);
            this.ID = ko.observable(RequestTypeDTO.ID);
            this.Timestamp = ko.observable(RequestTypeDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Metadata: this.Metadata(),
            PostProcess: this.PostProcess(),
            AddFiles: this.AddFiles(),
            RequiresProcessing: this.RequiresProcessing(),
            Notes: this.Notes(),
            WorkflowID: this.WorkflowID(),
            Workflow: this.Workflow(),
            SupportMultiQuery: this.SupportMultiQuery(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class ResponseViewModel extends EntityDtoWithIDViewModel {
    RequestDataMartID;
    ResponseGroupID;
    RespondedByID;
    ResponseTime;
    Count;
    SubmittedOn;
    SubmittedByID;
    SubmitMessage;
    ResponseMessage;
    constructor(ResponseDTO) {
        super();
        if (ResponseDTO == null) {
            this.RequestDataMartID = ko.observable();
            this.ResponseGroupID = ko.observable();
            this.RespondedByID = ko.observable();
            this.ResponseTime = ko.observable();
            this.Count = ko.observable();
            this.SubmittedOn = ko.observable();
            this.SubmittedByID = ko.observable();
            this.SubmitMessage = ko.observable();
            this.ResponseMessage = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.RequestDataMartID = ko.observable(ResponseDTO.RequestDataMartID);
            this.ResponseGroupID = ko.observable(ResponseDTO.ResponseGroupID);
            this.RespondedByID = ko.observable(ResponseDTO.RespondedByID);
            this.ResponseTime = ko.observable(ResponseDTO.ResponseTime);
            this.Count = ko.observable(ResponseDTO.Count);
            this.SubmittedOn = ko.observable(ResponseDTO.SubmittedOn);
            this.SubmittedByID = ko.observable(ResponseDTO.SubmittedByID);
            this.SubmitMessage = ko.observable(ResponseDTO.SubmitMessage);
            this.ResponseMessage = ko.observable(ResponseDTO.ResponseMessage);
            this.ID = ko.observable(ResponseDTO.ID);
            this.Timestamp = ko.observable(ResponseDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class DataMartEventViewModel extends BaseEventPermissionViewModel {
    DataMartID;
    constructor(DataMartEventDTO) {
        super();
        if (DataMartEventDTO == null) {
            this.DataMartID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(DataMartEventDTO.DataMartID);
            this.SecurityGroupID = ko.observable(DataMartEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(DataMartEventDTO.SecurityGroup);
            this.Allowed = ko.observable(DataMartEventDTO.Allowed);
            this.Overridden = ko.observable(DataMartEventDTO.Overridden);
            this.EventID = ko.observable(DataMartEventDTO.EventID);
            this.Event = ko.observable(DataMartEventDTO.Event);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class AclViewModel extends BaseAclViewModel {
    Allowed;
    PermissionID;
    Permission;
    constructor(AclDTO) {
        super();
        if (AclDTO == null) {
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.Allowed = ko.observable(AclDTO.Allowed);
            this.PermissionID = ko.observable(AclDTO.PermissionID);
            this.Permission = ko.observable(AclDTO.Permission);
            this.SecurityGroupID = ko.observable(AclDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclDTO.SecurityGroup);
            this.Overridden = ko.observable(AclDTO.Overridden);
        }
    }
    toData() {
        return {
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class ProjectDataMartEventViewModel extends BaseEventPermissionViewModel {
    ProjectID;
    DataMartID;
    constructor(ProjectDataMartEventDTO) {
        super();
        if (ProjectDataMartEventDTO == null) {
            this.ProjectID = ko.observable();
            this.DataMartID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectDataMartEventDTO.ProjectID);
            this.DataMartID = ko.observable(ProjectDataMartEventDTO.DataMartID);
            this.SecurityGroupID = ko.observable(ProjectDataMartEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(ProjectDataMartEventDTO.SecurityGroup);
            this.Allowed = ko.observable(ProjectDataMartEventDTO.Allowed);
            this.Overridden = ko.observable(ProjectDataMartEventDTO.Overridden);
            this.EventID = ko.observable(ProjectDataMartEventDTO.EventID);
            this.Event = ko.observable(ProjectDataMartEventDTO.Event);
        }
    }
    toData() {
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
    }
}
export class ProjectEventViewModel extends BaseEventPermissionViewModel {
    ProjectID;
    constructor(ProjectEventDTO) {
        super();
        if (ProjectEventDTO == null) {
            this.ProjectID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectEventDTO.ProjectID);
            this.SecurityGroupID = ko.observable(ProjectEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(ProjectEventDTO.SecurityGroup);
            this.Allowed = ko.observable(ProjectEventDTO.Allowed);
            this.Overridden = ko.observable(ProjectEventDTO.Overridden);
            this.EventID = ko.observable(ProjectEventDTO.EventID);
            this.Event = ko.observable(ProjectEventDTO.Event);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Allowed: this.Allowed(),
            Overridden: this.Overridden(),
            EventID: this.EventID(),
            Event: this.Event(),
        };
    }
}
export class ProjectOrganizationEventViewModel extends BaseEventPermissionViewModel {
    ProjectID;
    OrganizationID;
    constructor(ProjectOrganizationEventDTO) {
        super();
        if (ProjectOrganizationEventDTO == null) {
            this.ProjectID = ko.observable();
            this.OrganizationID = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Allowed = ko.observable();
            this.Overridden = ko.observable();
            this.EventID = ko.observable();
            this.Event = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(ProjectOrganizationEventDTO.ProjectID);
            this.OrganizationID = ko.observable(ProjectOrganizationEventDTO.OrganizationID);
            this.SecurityGroupID = ko.observable(ProjectOrganizationEventDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(ProjectOrganizationEventDTO.SecurityGroup);
            this.Allowed = ko.observable(ProjectOrganizationEventDTO.Allowed);
            this.Overridden = ko.observable(ProjectOrganizationEventDTO.Overridden);
            this.EventID = ko.observable(ProjectOrganizationEventDTO.EventID);
            this.Event = ko.observable(ProjectOrganizationEventDTO.Event);
        }
    }
    toData() {
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
    }
}
export class PermissionViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    Locations;
    constructor(PermissionDTO) {
        super();
        if (PermissionDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Locations = ko.observableArray();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(PermissionDTO.Name);
            this.Description = ko.observable(PermissionDTO.Description);
            this.Locations = ko.observableArray(PermissionDTO.Locations == null ? null : PermissionDTO.Locations.map((item) => { return item; }));
            this.ID = ko.observable(PermissionDTO.ID);
            this.Timestamp = ko.observable(PermissionDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Locations: this.Locations(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class SecurityGroupViewModel extends EntityDtoWithIDViewModel {
    Name;
    Path;
    OwnerID;
    Owner;
    ParentSecurityGroupID;
    ParentSecurityGroup;
    Kind;
    Type;
    constructor(SecurityGroupDTO) {
        super();
        if (SecurityGroupDTO == null) {
            this.Name = ko.observable();
            this.Path = ko.observable();
            this.OwnerID = ko.observable();
            this.Owner = ko.observable();
            this.ParentSecurityGroupID = ko.observable();
            this.ParentSecurityGroup = ko.observable();
            this.Kind = ko.observable();
            this.Type = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(SecurityGroupDTO.Name);
            this.Path = ko.observable(SecurityGroupDTO.Path);
            this.OwnerID = ko.observable(SecurityGroupDTO.OwnerID);
            this.Owner = ko.observable(SecurityGroupDTO.Owner);
            this.ParentSecurityGroupID = ko.observable(SecurityGroupDTO.ParentSecurityGroupID);
            this.ParentSecurityGroup = ko.observable(SecurityGroupDTO.ParentSecurityGroup);
            this.Kind = ko.observable(SecurityGroupDTO.Kind);
            this.Type = ko.observable(SecurityGroupDTO.Type);
            this.ID = ko.observable(SecurityGroupDTO.ID);
            this.Timestamp = ko.observable(SecurityGroupDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class SsoEndpointViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    PostUrl;
    oAuthKey;
    oAuthHash;
    RequirePassword;
    Group;
    DisplayIndex;
    Enabled;
    constructor(SsoEndpointDTO) {
        super();
        if (SsoEndpointDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.PostUrl = ko.observable();
            this.oAuthKey = ko.observable();
            this.oAuthHash = ko.observable();
            this.RequirePassword = ko.observable();
            this.Group = ko.observable();
            this.DisplayIndex = ko.observable();
            this.Enabled = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(SsoEndpointDTO.Name);
            this.Description = ko.observable(SsoEndpointDTO.Description);
            this.PostUrl = ko.observable(SsoEndpointDTO.PostUrl);
            this.oAuthKey = ko.observable(SsoEndpointDTO.oAuthKey);
            this.oAuthHash = ko.observable(SsoEndpointDTO.oAuthHash);
            this.RequirePassword = ko.observable(SsoEndpointDTO.RequirePassword);
            this.Group = ko.observable(SsoEndpointDTO.Group);
            this.DisplayIndex = ko.observable(SsoEndpointDTO.DisplayIndex);
            this.Enabled = ko.observable(SsoEndpointDTO.Enabled);
            this.ID = ko.observable(SsoEndpointDTO.ID);
            this.Timestamp = ko.observable(SsoEndpointDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class UserViewModel extends EntityDtoWithIDViewModel {
    UserName;
    Title;
    FirstName;
    LastName;
    MiddleName;
    Phone;
    Fax;
    Email;
    Active;
    Deleted;
    OrganizationID;
    Organization;
    OrganizationRequested;
    RoleID;
    RoleRequested;
    SignedUpOn;
    ActivatedOn;
    DeactivatedOn;
    DeactivatedByID;
    DeactivatedBy;
    DeactivationReason;
    RejectReason;
    RejectedOn;
    RejectedByID;
    RejectedBy;
    constructor(UserDTO) {
        super();
        if (UserDTO == null) {
            this.UserName = ko.observable();
            this.Title = ko.observable();
            this.FirstName = ko.observable();
            this.LastName = ko.observable();
            this.MiddleName = ko.observable();
            this.Phone = ko.observable();
            this.Fax = ko.observable();
            this.Email = ko.observable();
            this.Active = ko.observable();
            this.Deleted = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.OrganizationRequested = ko.observable();
            this.RoleID = ko.observable();
            this.RoleRequested = ko.observable();
            this.SignedUpOn = ko.observable();
            this.ActivatedOn = ko.observable();
            this.DeactivatedOn = ko.observable();
            this.DeactivatedByID = ko.observable();
            this.DeactivatedBy = ko.observable();
            this.DeactivationReason = ko.observable();
            this.RejectReason = ko.observable();
            this.RejectedOn = ko.observable();
            this.RejectedByID = ko.observable();
            this.RejectedBy = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.UserName = ko.observable(UserDTO.UserName);
            this.Title = ko.observable(UserDTO.Title);
            this.FirstName = ko.observable(UserDTO.FirstName);
            this.LastName = ko.observable(UserDTO.LastName);
            this.MiddleName = ko.observable(UserDTO.MiddleName);
            this.Phone = ko.observable(UserDTO.Phone);
            this.Fax = ko.observable(UserDTO.Fax);
            this.Email = ko.observable(UserDTO.Email);
            this.Active = ko.observable(UserDTO.Active);
            this.Deleted = ko.observable(UserDTO.Deleted);
            this.OrganizationID = ko.observable(UserDTO.OrganizationID);
            this.Organization = ko.observable(UserDTO.Organization);
            this.OrganizationRequested = ko.observable(UserDTO.OrganizationRequested);
            this.RoleID = ko.observable(UserDTO.RoleID);
            this.RoleRequested = ko.observable(UserDTO.RoleRequested);
            this.SignedUpOn = ko.observable(UserDTO.SignedUpOn);
            this.ActivatedOn = ko.observable(UserDTO.ActivatedOn);
            this.DeactivatedOn = ko.observable(UserDTO.DeactivatedOn);
            this.DeactivatedByID = ko.observable(UserDTO.DeactivatedByID);
            this.DeactivatedBy = ko.observable(UserDTO.DeactivatedBy);
            this.DeactivationReason = ko.observable(UserDTO.DeactivationReason);
            this.RejectReason = ko.observable(UserDTO.RejectReason);
            this.RejectedOn = ko.observable(UserDTO.RejectedOn);
            this.RejectedByID = ko.observable(UserDTO.RejectedByID);
            this.RejectedBy = ko.observable(UserDTO.RejectedBy);
            this.ID = ko.observable(UserDTO.ID);
            this.Timestamp = ko.observable(UserDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class WorkflowActivityViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    Start;
    End;
    constructor(WorkflowActivityDTO) {
        super();
        if (WorkflowActivityDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Start = ko.observable();
            this.End = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(WorkflowActivityDTO.Name);
            this.Description = ko.observable(WorkflowActivityDTO.Description);
            this.Start = ko.observable(WorkflowActivityDTO.Start);
            this.End = ko.observable(WorkflowActivityDTO.End);
            this.ID = ko.observable(WorkflowActivityDTO.ID);
            this.Timestamp = ko.observable(WorkflowActivityDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            Start: this.Start(),
            End: this.End(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class WorkflowViewModel extends EntityDtoWithIDViewModel {
    Name;
    Description;
    constructor(WorkflowDTO) {
        super();
        if (WorkflowDTO == null) {
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Name = ko.observable(WorkflowDTO.Name);
            this.Description = ko.observable(WorkflowDTO.Description);
            this.ID = ko.observable(WorkflowDTO.ID);
            this.Timestamp = ko.observable(WorkflowDTO.Timestamp);
        }
    }
    toData() {
        return {
            Name: this.Name(),
            Description: this.Description(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class WorkflowRoleViewModel extends EntityDtoWithIDViewModel {
    WorkflowID;
    Name;
    Description;
    IsRequestCreator;
    constructor(WorkflowRoleDTO) {
        super();
        if (WorkflowRoleDTO == null) {
            this.WorkflowID = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.IsRequestCreator = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.WorkflowID = ko.observable(WorkflowRoleDTO.WorkflowID);
            this.Name = ko.observable(WorkflowRoleDTO.Name);
            this.Description = ko.observable(WorkflowRoleDTO.Description);
            this.IsRequestCreator = ko.observable(WorkflowRoleDTO.IsRequestCreator);
            this.ID = ko.observable(WorkflowRoleDTO.ID);
            this.Timestamp = ko.observable(WorkflowRoleDTO.Timestamp);
        }
    }
    toData() {
        return {
            WorkflowID: this.WorkflowID(),
            Name: this.Name(),
            Description: this.Description(),
            IsRequestCreator: this.IsRequestCreator(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class DataModelWithRequestTypesViewModel extends DataModelViewModel {
    RequestTypes;
    constructor(DataModelWithRequestTypesDTO) {
        super();
        if (DataModelWithRequestTypesDTO == null) {
            this.RequestTypes = ko.observableArray();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.RequiresConfiguration = ko.observable();
            this.QueryComposer = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.RequestTypes = ko.observableArray(DataModelWithRequestTypesDTO.RequestTypes == null ? null : DataModelWithRequestTypesDTO.RequestTypes.map((item) => { return new RequestTypeViewModel(item); }));
            this.Name = ko.observable(DataModelWithRequestTypesDTO.Name);
            this.Description = ko.observable(DataModelWithRequestTypesDTO.Description);
            this.RequiresConfiguration = ko.observable(DataModelWithRequestTypesDTO.RequiresConfiguration);
            this.QueryComposer = ko.observable(DataModelWithRequestTypesDTO.QueryComposer);
            this.ID = ko.observable(DataModelWithRequestTypesDTO.ID);
            this.Timestamp = ko.observable(DataModelWithRequestTypesDTO.Timestamp);
        }
    }
    toData() {
        return {
            RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => { return item.toData(); }),
            Name: this.Name(),
            Description: this.Description(),
            RequiresConfiguration: this.RequiresConfiguration(),
            QueryComposer: this.QueryComposer(),
            ID: this.ID(),
            Timestamp: this.Timestamp(),
        };
    }
}
export class AclTemplateViewModel extends AclViewModel {
    TemplateID;
    constructor(AclTemplateDTO) {
        super();
        if (AclTemplateDTO == null) {
            this.TemplateID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.TemplateID = ko.observable(AclTemplateDTO.TemplateID);
            this.Allowed = ko.observable(AclTemplateDTO.Allowed);
            this.PermissionID = ko.observable(AclTemplateDTO.PermissionID);
            this.Permission = ko.observable(AclTemplateDTO.Permission);
            this.SecurityGroupID = ko.observable(AclTemplateDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclTemplateDTO.SecurityGroup);
            this.Overridden = ko.observable(AclTemplateDTO.Overridden);
        }
    }
    toData() {
        return {
            TemplateID: this.TemplateID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class DataMartViewModel extends DataMartListViewModel {
    RequiresApproval;
    DataMartTypeID;
    DataMartType;
    AvailablePeriod;
    ContactEmail;
    ContactFirstName;
    ContactLastName;
    ContactPhone;
    SpecialRequirements;
    UsageRestrictions;
    Deleted;
    HealthPlanDescription;
    IsGroupDataMart;
    UnattendedMode;
    DataUpdateFrequency;
    InpatientEHRApplication;
    OutpatientEHRApplication;
    OtherClaims;
    OtherInpatientEHRApplication;
    OtherOutpatientEHRApplication;
    LaboratoryResultsAny;
    LaboratoryResultsClaims;
    LaboratoryResultsTestName;
    LaboratoryResultsDates;
    LaboratoryResultsTestLOINC;
    LaboratoryResultsTestSNOMED;
    LaboratoryResultsSpecimenSource;
    LaboratoryResultsTestDescriptions;
    LaboratoryResultsOrderDates;
    LaboratoryResultsTestResultsInterpretation;
    LaboratoryResultsTestOther;
    LaboratoryResultsTestOtherText;
    InpatientEncountersAny;
    InpatientEncountersEncounterID;
    InpatientEncountersProviderIdentifier;
    InpatientDatesOfService;
    InpatientICD9Procedures;
    InpatientICD10Procedures;
    InpatientICD9Diagnosis;
    InpatientICD10Diagnosis;
    InpatientSNOMED;
    InpatientHPHCS;
    InpatientDisposition;
    InpatientDischargeStatus;
    InpatientOther;
    InpatientOtherText;
    OutpatientEncountersAny;
    OutpatientEncountersEncounterID;
    OutpatientEncountersProviderIdentifier;
    OutpatientClinicalSetting;
    OutpatientDatesOfService;
    OutpatientICD9Procedures;
    OutpatientICD10Procedures;
    OutpatientICD9Diagnosis;
    OutpatientICD10Diagnosis;
    OutpatientSNOMED;
    OutpatientHPHCS;
    OutpatientOther;
    OutpatientOtherText;
    ERPatientID;
    EREncounterID;
    EREnrollmentDates;
    EREncounterDates;
    ERClinicalSetting;
    ERICD9Diagnosis;
    ERICD10Diagnosis;
    ERHPHCS;
    ERNDC;
    ERSNOMED;
    ERProviderIdentifier;
    ERProviderFacility;
    EREncounterType;
    ERDRG;
    ERDRGType;
    EROther;
    EROtherText;
    DemographicsAny;
    DemographicsPatientID;
    DemographicsSex;
    DemographicsDateOfBirth;
    DemographicsDateOfDeath;
    DemographicsAddressInfo;
    DemographicsRace;
    DemographicsEthnicity;
    DemographicsOther;
    DemographicsOtherText;
    PatientOutcomesAny;
    PatientOutcomesInstruments;
    PatientOutcomesInstrumentText;
    PatientOutcomesHealthBehavior;
    PatientOutcomesHRQoL;
    PatientOutcomesReportedOutcome;
    PatientOutcomesOther;
    PatientOutcomesOtherText;
    PatientBehaviorHealthBehavior;
    PatientBehaviorInstruments;
    PatientBehaviorInstrumentText;
    PatientBehaviorOther;
    PatientBehaviorOtherText;
    VitalSignsAny;
    VitalSignsTemperature;
    VitalSignsHeight;
    VitalSignsWeight;
    VitalSignsBMI;
    VitalSignsBloodPressure;
    VitalSignsOther;
    VitalSignsOtherText;
    VitalSignsLength;
    PrescriptionOrdersAny;
    PrescriptionOrderDates;
    PrescriptionOrderRxNorm;
    PrescriptionOrderNDC;
    PrescriptionOrderOther;
    PrescriptionOrderOtherText;
    PharmacyDispensingAny;
    PharmacyDispensingDates;
    PharmacyDispensingRxNorm;
    PharmacyDispensingDaysSupply;
    PharmacyDispensingAmountDispensed;
    PharmacyDispensingNDC;
    PharmacyDispensingOther;
    PharmacyDispensingOtherText;
    BiorepositoriesAny;
    BiorepositoriesName;
    BiorepositoriesDescription;
    BiorepositoriesDiseaseName;
    BiorepositoriesSpecimenSource;
    BiorepositoriesSpecimenType;
    BiorepositoriesProcessingMethod;
    BiorepositoriesSNOMED;
    BiorepositoriesStorageMethod;
    BiorepositoriesOther;
    BiorepositoriesOtherText;
    LongitudinalCaptureAny;
    LongitudinalCapturePatientID;
    LongitudinalCaptureStart;
    LongitudinalCaptureStop;
    LongitudinalCaptureOther;
    LongitudinalCaptureOtherValue;
    DataModel;
    OtherDataModel;
    IsLocal;
    Url;
    AdapterID;
    Adapter;
    ProcessorID;
    DataPartnerIdentifier;
    DataPartnerCode;
    constructor(DataMartDTO) {
        super();
        if (DataMartDTO == null) {
            this.RequiresApproval = ko.observable();
            this.DataMartTypeID = ko.observable();
            this.DataMartType = ko.observable();
            this.AvailablePeriod = ko.observable();
            this.ContactEmail = ko.observable();
            this.ContactFirstName = ko.observable();
            this.ContactLastName = ko.observable();
            this.ContactPhone = ko.observable();
            this.SpecialRequirements = ko.observable();
            this.UsageRestrictions = ko.observable();
            this.Deleted = ko.observable();
            this.HealthPlanDescription = ko.observable();
            this.IsGroupDataMart = ko.observable();
            this.UnattendedMode = ko.observable();
            this.DataUpdateFrequency = ko.observable();
            this.InpatientEHRApplication = ko.observable();
            this.OutpatientEHRApplication = ko.observable();
            this.OtherClaims = ko.observable();
            this.OtherInpatientEHRApplication = ko.observable();
            this.OtherOutpatientEHRApplication = ko.observable();
            this.LaboratoryResultsAny = ko.observable();
            this.LaboratoryResultsClaims = ko.observable();
            this.LaboratoryResultsTestName = ko.observable();
            this.LaboratoryResultsDates = ko.observable();
            this.LaboratoryResultsTestLOINC = ko.observable();
            this.LaboratoryResultsTestSNOMED = ko.observable();
            this.LaboratoryResultsSpecimenSource = ko.observable();
            this.LaboratoryResultsTestDescriptions = ko.observable();
            this.LaboratoryResultsOrderDates = ko.observable();
            this.LaboratoryResultsTestResultsInterpretation = ko.observable();
            this.LaboratoryResultsTestOther = ko.observable();
            this.LaboratoryResultsTestOtherText = ko.observable();
            this.InpatientEncountersAny = ko.observable();
            this.InpatientEncountersEncounterID = ko.observable();
            this.InpatientEncountersProviderIdentifier = ko.observable();
            this.InpatientDatesOfService = ko.observable();
            this.InpatientICD9Procedures = ko.observable();
            this.InpatientICD10Procedures = ko.observable();
            this.InpatientICD9Diagnosis = ko.observable();
            this.InpatientICD10Diagnosis = ko.observable();
            this.InpatientSNOMED = ko.observable();
            this.InpatientHPHCS = ko.observable();
            this.InpatientDisposition = ko.observable();
            this.InpatientDischargeStatus = ko.observable();
            this.InpatientOther = ko.observable();
            this.InpatientOtherText = ko.observable();
            this.OutpatientEncountersAny = ko.observable();
            this.OutpatientEncountersEncounterID = ko.observable();
            this.OutpatientEncountersProviderIdentifier = ko.observable();
            this.OutpatientClinicalSetting = ko.observable();
            this.OutpatientDatesOfService = ko.observable();
            this.OutpatientICD9Procedures = ko.observable();
            this.OutpatientICD10Procedures = ko.observable();
            this.OutpatientICD9Diagnosis = ko.observable();
            this.OutpatientICD10Diagnosis = ko.observable();
            this.OutpatientSNOMED = ko.observable();
            this.OutpatientHPHCS = ko.observable();
            this.OutpatientOther = ko.observable();
            this.OutpatientOtherText = ko.observable();
            this.ERPatientID = ko.observable();
            this.EREncounterID = ko.observable();
            this.EREnrollmentDates = ko.observable();
            this.EREncounterDates = ko.observable();
            this.ERClinicalSetting = ko.observable();
            this.ERICD9Diagnosis = ko.observable();
            this.ERICD10Diagnosis = ko.observable();
            this.ERHPHCS = ko.observable();
            this.ERNDC = ko.observable();
            this.ERSNOMED = ko.observable();
            this.ERProviderIdentifier = ko.observable();
            this.ERProviderFacility = ko.observable();
            this.EREncounterType = ko.observable();
            this.ERDRG = ko.observable();
            this.ERDRGType = ko.observable();
            this.EROther = ko.observable();
            this.EROtherText = ko.observable();
            this.DemographicsAny = ko.observable();
            this.DemographicsPatientID = ko.observable();
            this.DemographicsSex = ko.observable();
            this.DemographicsDateOfBirth = ko.observable();
            this.DemographicsDateOfDeath = ko.observable();
            this.DemographicsAddressInfo = ko.observable();
            this.DemographicsRace = ko.observable();
            this.DemographicsEthnicity = ko.observable();
            this.DemographicsOther = ko.observable();
            this.DemographicsOtherText = ko.observable();
            this.PatientOutcomesAny = ko.observable();
            this.PatientOutcomesInstruments = ko.observable();
            this.PatientOutcomesInstrumentText = ko.observable();
            this.PatientOutcomesHealthBehavior = ko.observable();
            this.PatientOutcomesHRQoL = ko.observable();
            this.PatientOutcomesReportedOutcome = ko.observable();
            this.PatientOutcomesOther = ko.observable();
            this.PatientOutcomesOtherText = ko.observable();
            this.PatientBehaviorHealthBehavior = ko.observable();
            this.PatientBehaviorInstruments = ko.observable();
            this.PatientBehaviorInstrumentText = ko.observable();
            this.PatientBehaviorOther = ko.observable();
            this.PatientBehaviorOtherText = ko.observable();
            this.VitalSignsAny = ko.observable();
            this.VitalSignsTemperature = ko.observable();
            this.VitalSignsHeight = ko.observable();
            this.VitalSignsWeight = ko.observable();
            this.VitalSignsBMI = ko.observable();
            this.VitalSignsBloodPressure = ko.observable();
            this.VitalSignsOther = ko.observable();
            this.VitalSignsOtherText = ko.observable();
            this.VitalSignsLength = ko.observable();
            this.PrescriptionOrdersAny = ko.observable();
            this.PrescriptionOrderDates = ko.observable();
            this.PrescriptionOrderRxNorm = ko.observable();
            this.PrescriptionOrderNDC = ko.observable();
            this.PrescriptionOrderOther = ko.observable();
            this.PrescriptionOrderOtherText = ko.observable();
            this.PharmacyDispensingAny = ko.observable();
            this.PharmacyDispensingDates = ko.observable();
            this.PharmacyDispensingRxNorm = ko.observable();
            this.PharmacyDispensingDaysSupply = ko.observable();
            this.PharmacyDispensingAmountDispensed = ko.observable();
            this.PharmacyDispensingNDC = ko.observable();
            this.PharmacyDispensingOther = ko.observable();
            this.PharmacyDispensingOtherText = ko.observable();
            this.BiorepositoriesAny = ko.observable();
            this.BiorepositoriesName = ko.observable();
            this.BiorepositoriesDescription = ko.observable();
            this.BiorepositoriesDiseaseName = ko.observable();
            this.BiorepositoriesSpecimenSource = ko.observable();
            this.BiorepositoriesSpecimenType = ko.observable();
            this.BiorepositoriesProcessingMethod = ko.observable();
            this.BiorepositoriesSNOMED = ko.observable();
            this.BiorepositoriesStorageMethod = ko.observable();
            this.BiorepositoriesOther = ko.observable();
            this.BiorepositoriesOtherText = ko.observable();
            this.LongitudinalCaptureAny = ko.observable();
            this.LongitudinalCapturePatientID = ko.observable();
            this.LongitudinalCaptureStart = ko.observable();
            this.LongitudinalCaptureStop = ko.observable();
            this.LongitudinalCaptureOther = ko.observable();
            this.LongitudinalCaptureOtherValue = ko.observable();
            this.DataModel = ko.observable();
            this.OtherDataModel = ko.observable();
            this.IsLocal = ko.observable();
            this.Url = ko.observable();
            this.AdapterID = ko.observable();
            this.Adapter = ko.observable();
            this.ProcessorID = ko.observable();
            this.DataPartnerIdentifier = ko.observable();
            this.DataPartnerCode = ko.observable();
            this.Name = ko.observable();
            this.Description = ko.observable();
            this.Acronym = ko.observable();
            this.StartDate = ko.observable();
            this.EndDate = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.ParentOrganziationID = ko.observable();
            this.ParentOrganization = ko.observable();
            this.Priority = ko.observable();
            this.DueDate = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.RequiresApproval = ko.observable(DataMartDTO.RequiresApproval);
            this.DataMartTypeID = ko.observable(DataMartDTO.DataMartTypeID);
            this.DataMartType = ko.observable(DataMartDTO.DataMartType);
            this.AvailablePeriod = ko.observable(DataMartDTO.AvailablePeriod);
            this.ContactEmail = ko.observable(DataMartDTO.ContactEmail);
            this.ContactFirstName = ko.observable(DataMartDTO.ContactFirstName);
            this.ContactLastName = ko.observable(DataMartDTO.ContactLastName);
            this.ContactPhone = ko.observable(DataMartDTO.ContactPhone);
            this.SpecialRequirements = ko.observable(DataMartDTO.SpecialRequirements);
            this.UsageRestrictions = ko.observable(DataMartDTO.UsageRestrictions);
            this.Deleted = ko.observable(DataMartDTO.Deleted);
            this.HealthPlanDescription = ko.observable(DataMartDTO.HealthPlanDescription);
            this.IsGroupDataMart = ko.observable(DataMartDTO.IsGroupDataMart);
            this.UnattendedMode = ko.observable(DataMartDTO.UnattendedMode);
            this.DataUpdateFrequency = ko.observable(DataMartDTO.DataUpdateFrequency);
            this.InpatientEHRApplication = ko.observable(DataMartDTO.InpatientEHRApplication);
            this.OutpatientEHRApplication = ko.observable(DataMartDTO.OutpatientEHRApplication);
            this.OtherClaims = ko.observable(DataMartDTO.OtherClaims);
            this.OtherInpatientEHRApplication = ko.observable(DataMartDTO.OtherInpatientEHRApplication);
            this.OtherOutpatientEHRApplication = ko.observable(DataMartDTO.OtherOutpatientEHRApplication);
            this.LaboratoryResultsAny = ko.observable(DataMartDTO.LaboratoryResultsAny);
            this.LaboratoryResultsClaims = ko.observable(DataMartDTO.LaboratoryResultsClaims);
            this.LaboratoryResultsTestName = ko.observable(DataMartDTO.LaboratoryResultsTestName);
            this.LaboratoryResultsDates = ko.observable(DataMartDTO.LaboratoryResultsDates);
            this.LaboratoryResultsTestLOINC = ko.observable(DataMartDTO.LaboratoryResultsTestLOINC);
            this.LaboratoryResultsTestSNOMED = ko.observable(DataMartDTO.LaboratoryResultsTestSNOMED);
            this.LaboratoryResultsSpecimenSource = ko.observable(DataMartDTO.LaboratoryResultsSpecimenSource);
            this.LaboratoryResultsTestDescriptions = ko.observable(DataMartDTO.LaboratoryResultsTestDescriptions);
            this.LaboratoryResultsOrderDates = ko.observable(DataMartDTO.LaboratoryResultsOrderDates);
            this.LaboratoryResultsTestResultsInterpretation = ko.observable(DataMartDTO.LaboratoryResultsTestResultsInterpretation);
            this.LaboratoryResultsTestOther = ko.observable(DataMartDTO.LaboratoryResultsTestOther);
            this.LaboratoryResultsTestOtherText = ko.observable(DataMartDTO.LaboratoryResultsTestOtherText);
            this.InpatientEncountersAny = ko.observable(DataMartDTO.InpatientEncountersAny);
            this.InpatientEncountersEncounterID = ko.observable(DataMartDTO.InpatientEncountersEncounterID);
            this.InpatientEncountersProviderIdentifier = ko.observable(DataMartDTO.InpatientEncountersProviderIdentifier);
            this.InpatientDatesOfService = ko.observable(DataMartDTO.InpatientDatesOfService);
            this.InpatientICD9Procedures = ko.observable(DataMartDTO.InpatientICD9Procedures);
            this.InpatientICD10Procedures = ko.observable(DataMartDTO.InpatientICD10Procedures);
            this.InpatientICD9Diagnosis = ko.observable(DataMartDTO.InpatientICD9Diagnosis);
            this.InpatientICD10Diagnosis = ko.observable(DataMartDTO.InpatientICD10Diagnosis);
            this.InpatientSNOMED = ko.observable(DataMartDTO.InpatientSNOMED);
            this.InpatientHPHCS = ko.observable(DataMartDTO.InpatientHPHCS);
            this.InpatientDisposition = ko.observable(DataMartDTO.InpatientDisposition);
            this.InpatientDischargeStatus = ko.observable(DataMartDTO.InpatientDischargeStatus);
            this.InpatientOther = ko.observable(DataMartDTO.InpatientOther);
            this.InpatientOtherText = ko.observable(DataMartDTO.InpatientOtherText);
            this.OutpatientEncountersAny = ko.observable(DataMartDTO.OutpatientEncountersAny);
            this.OutpatientEncountersEncounterID = ko.observable(DataMartDTO.OutpatientEncountersEncounterID);
            this.OutpatientEncountersProviderIdentifier = ko.observable(DataMartDTO.OutpatientEncountersProviderIdentifier);
            this.OutpatientClinicalSetting = ko.observable(DataMartDTO.OutpatientClinicalSetting);
            this.OutpatientDatesOfService = ko.observable(DataMartDTO.OutpatientDatesOfService);
            this.OutpatientICD9Procedures = ko.observable(DataMartDTO.OutpatientICD9Procedures);
            this.OutpatientICD10Procedures = ko.observable(DataMartDTO.OutpatientICD10Procedures);
            this.OutpatientICD9Diagnosis = ko.observable(DataMartDTO.OutpatientICD9Diagnosis);
            this.OutpatientICD10Diagnosis = ko.observable(DataMartDTO.OutpatientICD10Diagnosis);
            this.OutpatientSNOMED = ko.observable(DataMartDTO.OutpatientSNOMED);
            this.OutpatientHPHCS = ko.observable(DataMartDTO.OutpatientHPHCS);
            this.OutpatientOther = ko.observable(DataMartDTO.OutpatientOther);
            this.OutpatientOtherText = ko.observable(DataMartDTO.OutpatientOtherText);
            this.ERPatientID = ko.observable(DataMartDTO.ERPatientID);
            this.EREncounterID = ko.observable(DataMartDTO.EREncounterID);
            this.EREnrollmentDates = ko.observable(DataMartDTO.EREnrollmentDates);
            this.EREncounterDates = ko.observable(DataMartDTO.EREncounterDates);
            this.ERClinicalSetting = ko.observable(DataMartDTO.ERClinicalSetting);
            this.ERICD9Diagnosis = ko.observable(DataMartDTO.ERICD9Diagnosis);
            this.ERICD10Diagnosis = ko.observable(DataMartDTO.ERICD10Diagnosis);
            this.ERHPHCS = ko.observable(DataMartDTO.ERHPHCS);
            this.ERNDC = ko.observable(DataMartDTO.ERNDC);
            this.ERSNOMED = ko.observable(DataMartDTO.ERSNOMED);
            this.ERProviderIdentifier = ko.observable(DataMartDTO.ERProviderIdentifier);
            this.ERProviderFacility = ko.observable(DataMartDTO.ERProviderFacility);
            this.EREncounterType = ko.observable(DataMartDTO.EREncounterType);
            this.ERDRG = ko.observable(DataMartDTO.ERDRG);
            this.ERDRGType = ko.observable(DataMartDTO.ERDRGType);
            this.EROther = ko.observable(DataMartDTO.EROther);
            this.EROtherText = ko.observable(DataMartDTO.EROtherText);
            this.DemographicsAny = ko.observable(DataMartDTO.DemographicsAny);
            this.DemographicsPatientID = ko.observable(DataMartDTO.DemographicsPatientID);
            this.DemographicsSex = ko.observable(DataMartDTO.DemographicsSex);
            this.DemographicsDateOfBirth = ko.observable(DataMartDTO.DemographicsDateOfBirth);
            this.DemographicsDateOfDeath = ko.observable(DataMartDTO.DemographicsDateOfDeath);
            this.DemographicsAddressInfo = ko.observable(DataMartDTO.DemographicsAddressInfo);
            this.DemographicsRace = ko.observable(DataMartDTO.DemographicsRace);
            this.DemographicsEthnicity = ko.observable(DataMartDTO.DemographicsEthnicity);
            this.DemographicsOther = ko.observable(DataMartDTO.DemographicsOther);
            this.DemographicsOtherText = ko.observable(DataMartDTO.DemographicsOtherText);
            this.PatientOutcomesAny = ko.observable(DataMartDTO.PatientOutcomesAny);
            this.PatientOutcomesInstruments = ko.observable(DataMartDTO.PatientOutcomesInstruments);
            this.PatientOutcomesInstrumentText = ko.observable(DataMartDTO.PatientOutcomesInstrumentText);
            this.PatientOutcomesHealthBehavior = ko.observable(DataMartDTO.PatientOutcomesHealthBehavior);
            this.PatientOutcomesHRQoL = ko.observable(DataMartDTO.PatientOutcomesHRQoL);
            this.PatientOutcomesReportedOutcome = ko.observable(DataMartDTO.PatientOutcomesReportedOutcome);
            this.PatientOutcomesOther = ko.observable(DataMartDTO.PatientOutcomesOther);
            this.PatientOutcomesOtherText = ko.observable(DataMartDTO.PatientOutcomesOtherText);
            this.PatientBehaviorHealthBehavior = ko.observable(DataMartDTO.PatientBehaviorHealthBehavior);
            this.PatientBehaviorInstruments = ko.observable(DataMartDTO.PatientBehaviorInstruments);
            this.PatientBehaviorInstrumentText = ko.observable(DataMartDTO.PatientBehaviorInstrumentText);
            this.PatientBehaviorOther = ko.observable(DataMartDTO.PatientBehaviorOther);
            this.PatientBehaviorOtherText = ko.observable(DataMartDTO.PatientBehaviorOtherText);
            this.VitalSignsAny = ko.observable(DataMartDTO.VitalSignsAny);
            this.VitalSignsTemperature = ko.observable(DataMartDTO.VitalSignsTemperature);
            this.VitalSignsHeight = ko.observable(DataMartDTO.VitalSignsHeight);
            this.VitalSignsWeight = ko.observable(DataMartDTO.VitalSignsWeight);
            this.VitalSignsBMI = ko.observable(DataMartDTO.VitalSignsBMI);
            this.VitalSignsBloodPressure = ko.observable(DataMartDTO.VitalSignsBloodPressure);
            this.VitalSignsOther = ko.observable(DataMartDTO.VitalSignsOther);
            this.VitalSignsOtherText = ko.observable(DataMartDTO.VitalSignsOtherText);
            this.VitalSignsLength = ko.observable(DataMartDTO.VitalSignsLength);
            this.PrescriptionOrdersAny = ko.observable(DataMartDTO.PrescriptionOrdersAny);
            this.PrescriptionOrderDates = ko.observable(DataMartDTO.PrescriptionOrderDates);
            this.PrescriptionOrderRxNorm = ko.observable(DataMartDTO.PrescriptionOrderRxNorm);
            this.PrescriptionOrderNDC = ko.observable(DataMartDTO.PrescriptionOrderNDC);
            this.PrescriptionOrderOther = ko.observable(DataMartDTO.PrescriptionOrderOther);
            this.PrescriptionOrderOtherText = ko.observable(DataMartDTO.PrescriptionOrderOtherText);
            this.PharmacyDispensingAny = ko.observable(DataMartDTO.PharmacyDispensingAny);
            this.PharmacyDispensingDates = ko.observable(DataMartDTO.PharmacyDispensingDates);
            this.PharmacyDispensingRxNorm = ko.observable(DataMartDTO.PharmacyDispensingRxNorm);
            this.PharmacyDispensingDaysSupply = ko.observable(DataMartDTO.PharmacyDispensingDaysSupply);
            this.PharmacyDispensingAmountDispensed = ko.observable(DataMartDTO.PharmacyDispensingAmountDispensed);
            this.PharmacyDispensingNDC = ko.observable(DataMartDTO.PharmacyDispensingNDC);
            this.PharmacyDispensingOther = ko.observable(DataMartDTO.PharmacyDispensingOther);
            this.PharmacyDispensingOtherText = ko.observable(DataMartDTO.PharmacyDispensingOtherText);
            this.BiorepositoriesAny = ko.observable(DataMartDTO.BiorepositoriesAny);
            this.BiorepositoriesName = ko.observable(DataMartDTO.BiorepositoriesName);
            this.BiorepositoriesDescription = ko.observable(DataMartDTO.BiorepositoriesDescription);
            this.BiorepositoriesDiseaseName = ko.observable(DataMartDTO.BiorepositoriesDiseaseName);
            this.BiorepositoriesSpecimenSource = ko.observable(DataMartDTO.BiorepositoriesSpecimenSource);
            this.BiorepositoriesSpecimenType = ko.observable(DataMartDTO.BiorepositoriesSpecimenType);
            this.BiorepositoriesProcessingMethod = ko.observable(DataMartDTO.BiorepositoriesProcessingMethod);
            this.BiorepositoriesSNOMED = ko.observable(DataMartDTO.BiorepositoriesSNOMED);
            this.BiorepositoriesStorageMethod = ko.observable(DataMartDTO.BiorepositoriesStorageMethod);
            this.BiorepositoriesOther = ko.observable(DataMartDTO.BiorepositoriesOther);
            this.BiorepositoriesOtherText = ko.observable(DataMartDTO.BiorepositoriesOtherText);
            this.LongitudinalCaptureAny = ko.observable(DataMartDTO.LongitudinalCaptureAny);
            this.LongitudinalCapturePatientID = ko.observable(DataMartDTO.LongitudinalCapturePatientID);
            this.LongitudinalCaptureStart = ko.observable(DataMartDTO.LongitudinalCaptureStart);
            this.LongitudinalCaptureStop = ko.observable(DataMartDTO.LongitudinalCaptureStop);
            this.LongitudinalCaptureOther = ko.observable(DataMartDTO.LongitudinalCaptureOther);
            this.LongitudinalCaptureOtherValue = ko.observable(DataMartDTO.LongitudinalCaptureOtherValue);
            this.DataModel = ko.observable(DataMartDTO.DataModel);
            this.OtherDataModel = ko.observable(DataMartDTO.OtherDataModel);
            this.IsLocal = ko.observable(DataMartDTO.IsLocal);
            this.Url = ko.observable(DataMartDTO.Url);
            this.AdapterID = ko.observable(DataMartDTO.AdapterID);
            this.Adapter = ko.observable(DataMartDTO.Adapter);
            this.ProcessorID = ko.observable(DataMartDTO.ProcessorID);
            this.DataPartnerIdentifier = ko.observable(DataMartDTO.DataPartnerIdentifier);
            this.DataPartnerCode = ko.observable(DataMartDTO.DataPartnerCode);
            this.Name = ko.observable(DataMartDTO.Name);
            this.Description = ko.observable(DataMartDTO.Description);
            this.Acronym = ko.observable(DataMartDTO.Acronym);
            this.StartDate = ko.observable(DataMartDTO.StartDate);
            this.EndDate = ko.observable(DataMartDTO.EndDate);
            this.OrganizationID = ko.observable(DataMartDTO.OrganizationID);
            this.Organization = ko.observable(DataMartDTO.Organization);
            this.ParentOrganziationID = ko.observable(DataMartDTO.ParentOrganziationID);
            this.ParentOrganization = ko.observable(DataMartDTO.ParentOrganization);
            this.Priority = ko.observable(DataMartDTO.Priority);
            this.DueDate = ko.observable(DataMartDTO.DueDate);
            this.ID = ko.observable(DataMartDTO.ID);
            this.Timestamp = ko.observable(DataMartDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class ResponseDetailViewModel extends ResponseViewModel {
    Request;
    RequestID;
    DataMart;
    DataMartID;
    SubmittedBy;
    RespondedBy;
    Status;
    constructor(ResponseDetailDTO) {
        super();
        if (ResponseDetailDTO == null) {
            this.Request = ko.observable();
            this.RequestID = ko.observable();
            this.DataMart = ko.observable();
            this.DataMartID = ko.observable();
            this.SubmittedBy = ko.observable();
            this.RespondedBy = ko.observable();
            this.Status = ko.observable();
            this.RequestDataMartID = ko.observable();
            this.ResponseGroupID = ko.observable();
            this.RespondedByID = ko.observable();
            this.ResponseTime = ko.observable();
            this.Count = ko.observable();
            this.SubmittedOn = ko.observable();
            this.SubmittedByID = ko.observable();
            this.SubmitMessage = ko.observable();
            this.ResponseMessage = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Request = ko.observable(ResponseDetailDTO.Request);
            this.RequestID = ko.observable(ResponseDetailDTO.RequestID);
            this.DataMart = ko.observable(ResponseDetailDTO.DataMart);
            this.DataMartID = ko.observable(ResponseDetailDTO.DataMartID);
            this.SubmittedBy = ko.observable(ResponseDetailDTO.SubmittedBy);
            this.RespondedBy = ko.observable(ResponseDetailDTO.RespondedBy);
            this.Status = ko.observable(ResponseDetailDTO.Status);
            this.RequestDataMartID = ko.observable(ResponseDetailDTO.RequestDataMartID);
            this.ResponseGroupID = ko.observable(ResponseDetailDTO.ResponseGroupID);
            this.RespondedByID = ko.observable(ResponseDetailDTO.RespondedByID);
            this.ResponseTime = ko.observable(ResponseDetailDTO.ResponseTime);
            this.Count = ko.observable(ResponseDetailDTO.Count);
            this.SubmittedOn = ko.observable(ResponseDetailDTO.SubmittedOn);
            this.SubmittedByID = ko.observable(ResponseDetailDTO.SubmittedByID);
            this.SubmitMessage = ko.observable(ResponseDetailDTO.SubmitMessage);
            this.ResponseMessage = ko.observable(ResponseDetailDTO.ResponseMessage);
            this.ID = ko.observable(ResponseDetailDTO.ID);
            this.Timestamp = ko.observable(ResponseDetailDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class AclDataMartRequestTypeViewModel extends BaseAclRequestTypeViewModel {
    DataMartID;
    constructor(AclDataMartRequestTypeDTO) {
        super();
        if (AclDataMartRequestTypeDTO == null) {
            this.DataMartID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(AclDataMartRequestTypeDTO.DataMartID);
            this.RequestTypeID = ko.observable(AclDataMartRequestTypeDTO.RequestTypeID);
            this.Permission = ko.observable(AclDataMartRequestTypeDTO.Permission);
            this.SecurityGroupID = ko.observable(AclDataMartRequestTypeDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclDataMartRequestTypeDTO.SecurityGroup);
            this.Overridden = ko.observable(AclDataMartRequestTypeDTO.Overridden);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            RequestTypeID: this.RequestTypeID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclDataMartViewModel extends AclViewModel {
    DataMartID;
    constructor(AclDataMartDTO) {
        super();
        if (AclDataMartDTO == null) {
            this.DataMartID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.DataMartID = ko.observable(AclDataMartDTO.DataMartID);
            this.Allowed = ko.observable(AclDataMartDTO.Allowed);
            this.PermissionID = ko.observable(AclDataMartDTO.PermissionID);
            this.Permission = ko.observable(AclDataMartDTO.Permission);
            this.SecurityGroupID = ko.observable(AclDataMartDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclDataMartDTO.SecurityGroup);
            this.Overridden = ko.observable(AclDataMartDTO.Overridden);
        }
    }
    toData() {
        return {
            DataMartID: this.DataMartID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclGroupViewModel extends AclViewModel {
    GroupID;
    constructor(AclGroupDTO) {
        super();
        if (AclGroupDTO == null) {
            this.GroupID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.GroupID = ko.observable(AclGroupDTO.GroupID);
            this.Allowed = ko.observable(AclGroupDTO.Allowed);
            this.PermissionID = ko.observable(AclGroupDTO.PermissionID);
            this.Permission = ko.observable(AclGroupDTO.Permission);
            this.SecurityGroupID = ko.observable(AclGroupDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclGroupDTO.SecurityGroup);
            this.Overridden = ko.observable(AclGroupDTO.Overridden);
        }
    }
    toData() {
        return {
            GroupID: this.GroupID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclOrganizationViewModel extends AclViewModel {
    OrganizationID;
    constructor(AclOrganizationDTO) {
        super();
        if (AclOrganizationDTO == null) {
            this.OrganizationID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.OrganizationID = ko.observable(AclOrganizationDTO.OrganizationID);
            this.Allowed = ko.observable(AclOrganizationDTO.Allowed);
            this.PermissionID = ko.observable(AclOrganizationDTO.PermissionID);
            this.Permission = ko.observable(AclOrganizationDTO.Permission);
            this.SecurityGroupID = ko.observable(AclOrganizationDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclOrganizationDTO.SecurityGroup);
            this.Overridden = ko.observable(AclOrganizationDTO.Overridden);
        }
    }
    toData() {
        return {
            OrganizationID: this.OrganizationID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclProjectOrganizationViewModel extends AclViewModel {
    ProjectID;
    OrganizationID;
    constructor(AclProjectOrganizationDTO) {
        super();
        if (AclProjectOrganizationDTO == null) {
            this.ProjectID = ko.observable();
            this.OrganizationID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectOrganizationDTO.ProjectID);
            this.OrganizationID = ko.observable(AclProjectOrganizationDTO.OrganizationID);
            this.Allowed = ko.observable(AclProjectOrganizationDTO.Allowed);
            this.PermissionID = ko.observable(AclProjectOrganizationDTO.PermissionID);
            this.Permission = ko.observable(AclProjectOrganizationDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectOrganizationDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectOrganizationDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectOrganizationDTO.Overridden);
        }
    }
    toData() {
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
    }
}
export class AclProjectDataMartViewModel extends AclViewModel {
    ProjectID;
    DataMartID;
    constructor(AclProjectDataMartDTO) {
        super();
        if (AclProjectDataMartDTO == null) {
            this.ProjectID = ko.observable();
            this.DataMartID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectDataMartDTO.ProjectID);
            this.DataMartID = ko.observable(AclProjectDataMartDTO.DataMartID);
            this.Allowed = ko.observable(AclProjectDataMartDTO.Allowed);
            this.PermissionID = ko.observable(AclProjectDataMartDTO.PermissionID);
            this.Permission = ko.observable(AclProjectDataMartDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectDataMartDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectDataMartDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectDataMartDTO.Overridden);
        }
    }
    toData() {
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
    }
}
export class AclProjectViewModel extends AclViewModel {
    ProjectID;
    constructor(AclProjectDTO) {
        super();
        if (AclProjectDTO == null) {
            this.ProjectID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectDTO.ProjectID);
            this.Allowed = ko.observable(AclProjectDTO.Allowed);
            this.PermissionID = ko.observable(AclProjectDTO.PermissionID);
            this.Permission = ko.observable(AclProjectDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectDTO.Overridden);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclProjectRequestTypeViewModel extends BaseAclRequestTypeViewModel {
    ProjectID;
    constructor(AclProjectRequestTypeDTO) {
        super();
        if (AclProjectRequestTypeDTO == null) {
            this.ProjectID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectRequestTypeDTO.ProjectID);
            this.RequestTypeID = ko.observable(AclProjectRequestTypeDTO.RequestTypeID);
            this.Permission = ko.observable(AclProjectRequestTypeDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectRequestTypeDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectRequestTypeDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectRequestTypeDTO.Overridden);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            RequestTypeID: this.RequestTypeID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclRegistryViewModel extends AclViewModel {
    RegistryID;
    constructor(AclRegistryDTO) {
        super();
        if (AclRegistryDTO == null) {
            this.RegistryID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.RegistryID = ko.observable(AclRegistryDTO.RegistryID);
            this.Allowed = ko.observable(AclRegistryDTO.Allowed);
            this.PermissionID = ko.observable(AclRegistryDTO.PermissionID);
            this.Permission = ko.observable(AclRegistryDTO.Permission);
            this.SecurityGroupID = ko.observable(AclRegistryDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclRegistryDTO.SecurityGroup);
            this.Overridden = ko.observable(AclRegistryDTO.Overridden);
        }
    }
    toData() {
        return {
            RegistryID: this.RegistryID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class AclRequestTypeViewModel extends AclViewModel {
    RequestTypeID;
    RequestType;
    constructor(AclRequestTypeDTO) {
        super();
        if (AclRequestTypeDTO == null) {
            this.RequestTypeID = ko.observable();
            this.RequestType = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.RequestTypeID = ko.observable(AclRequestTypeDTO.RequestTypeID);
            this.RequestType = ko.observable(AclRequestTypeDTO.RequestType);
            this.Allowed = ko.observable(AclRequestTypeDTO.Allowed);
            this.PermissionID = ko.observable(AclRequestTypeDTO.PermissionID);
            this.Permission = ko.observable(AclRequestTypeDTO.Permission);
            this.SecurityGroupID = ko.observable(AclRequestTypeDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclRequestTypeDTO.SecurityGroup);
            this.Overridden = ko.observable(AclRequestTypeDTO.Overridden);
        }
    }
    toData() {
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
    }
}
export class AclUserViewModel extends AclViewModel {
    UserID;
    constructor(AclUserDTO) {
        super();
        if (AclUserDTO == null) {
            this.UserID = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.UserID = ko.observable(AclUserDTO.UserID);
            this.Allowed = ko.observable(AclUserDTO.Allowed);
            this.PermissionID = ko.observable(AclUserDTO.PermissionID);
            this.Permission = ko.observable(AclUserDTO.Permission);
            this.SecurityGroupID = ko.observable(AclUserDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclUserDTO.SecurityGroup);
            this.Overridden = ko.observable(AclUserDTO.Overridden);
        }
    }
    toData() {
        return {
            UserID: this.UserID(),
            Allowed: this.Allowed(),
            PermissionID: this.PermissionID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
export class SecurityGroupWithUsersViewModel extends SecurityGroupViewModel {
    Users;
    constructor(SecurityGroupWithUsersDTO) {
        super();
        if (SecurityGroupWithUsersDTO == null) {
            this.Users = ko.observableArray();
            this.Name = ko.observable();
            this.Path = ko.observable();
            this.OwnerID = ko.observable();
            this.Owner = ko.observable();
            this.ParentSecurityGroupID = ko.observable();
            this.ParentSecurityGroup = ko.observable();
            this.Kind = ko.observable();
            this.Type = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.Users = ko.observableArray(SecurityGroupWithUsersDTO.Users == null ? null : SecurityGroupWithUsersDTO.Users.map((item) => { return item; }));
            this.Name = ko.observable(SecurityGroupWithUsersDTO.Name);
            this.Path = ko.observable(SecurityGroupWithUsersDTO.Path);
            this.OwnerID = ko.observable(SecurityGroupWithUsersDTO.OwnerID);
            this.Owner = ko.observable(SecurityGroupWithUsersDTO.Owner);
            this.ParentSecurityGroupID = ko.observable(SecurityGroupWithUsersDTO.ParentSecurityGroupID);
            this.ParentSecurityGroup = ko.observable(SecurityGroupWithUsersDTO.ParentSecurityGroup);
            this.Kind = ko.observable(SecurityGroupWithUsersDTO.Kind);
            this.Type = ko.observable(SecurityGroupWithUsersDTO.Type);
            this.ID = ko.observable(SecurityGroupWithUsersDTO.ID);
            this.Timestamp = ko.observable(SecurityGroupWithUsersDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class UserWithSecurityDetailsViewModel extends UserViewModel {
    PasswordHash;
    constructor(UserWithSecurityDetailsDTO) {
        super();
        if (UserWithSecurityDetailsDTO == null) {
            this.PasswordHash = ko.observable();
            this.UserName = ko.observable();
            this.Title = ko.observable();
            this.FirstName = ko.observable();
            this.LastName = ko.observable();
            this.MiddleName = ko.observable();
            this.Phone = ko.observable();
            this.Fax = ko.observable();
            this.Email = ko.observable();
            this.Active = ko.observable();
            this.Deleted = ko.observable();
            this.OrganizationID = ko.observable();
            this.Organization = ko.observable();
            this.OrganizationRequested = ko.observable();
            this.RoleID = ko.observable();
            this.RoleRequested = ko.observable();
            this.SignedUpOn = ko.observable();
            this.ActivatedOn = ko.observable();
            this.DeactivatedOn = ko.observable();
            this.DeactivatedByID = ko.observable();
            this.DeactivatedBy = ko.observable();
            this.DeactivationReason = ko.observable();
            this.RejectReason = ko.observable();
            this.RejectedOn = ko.observable();
            this.RejectedByID = ko.observable();
            this.RejectedBy = ko.observable();
            this.ID = ko.observable();
            this.Timestamp = ko.observable();
        }
        else {
            this.PasswordHash = ko.observable(UserWithSecurityDetailsDTO.PasswordHash);
            this.UserName = ko.observable(UserWithSecurityDetailsDTO.UserName);
            this.Title = ko.observable(UserWithSecurityDetailsDTO.Title);
            this.FirstName = ko.observable(UserWithSecurityDetailsDTO.FirstName);
            this.LastName = ko.observable(UserWithSecurityDetailsDTO.LastName);
            this.MiddleName = ko.observable(UserWithSecurityDetailsDTO.MiddleName);
            this.Phone = ko.observable(UserWithSecurityDetailsDTO.Phone);
            this.Fax = ko.observable(UserWithSecurityDetailsDTO.Fax);
            this.Email = ko.observable(UserWithSecurityDetailsDTO.Email);
            this.Active = ko.observable(UserWithSecurityDetailsDTO.Active);
            this.Deleted = ko.observable(UserWithSecurityDetailsDTO.Deleted);
            this.OrganizationID = ko.observable(UserWithSecurityDetailsDTO.OrganizationID);
            this.Organization = ko.observable(UserWithSecurityDetailsDTO.Organization);
            this.OrganizationRequested = ko.observable(UserWithSecurityDetailsDTO.OrganizationRequested);
            this.RoleID = ko.observable(UserWithSecurityDetailsDTO.RoleID);
            this.RoleRequested = ko.observable(UserWithSecurityDetailsDTO.RoleRequested);
            this.SignedUpOn = ko.observable(UserWithSecurityDetailsDTO.SignedUpOn);
            this.ActivatedOn = ko.observable(UserWithSecurityDetailsDTO.ActivatedOn);
            this.DeactivatedOn = ko.observable(UserWithSecurityDetailsDTO.DeactivatedOn);
            this.DeactivatedByID = ko.observable(UserWithSecurityDetailsDTO.DeactivatedByID);
            this.DeactivatedBy = ko.observable(UserWithSecurityDetailsDTO.DeactivatedBy);
            this.DeactivationReason = ko.observable(UserWithSecurityDetailsDTO.DeactivationReason);
            this.RejectReason = ko.observable(UserWithSecurityDetailsDTO.RejectReason);
            this.RejectedOn = ko.observable(UserWithSecurityDetailsDTO.RejectedOn);
            this.RejectedByID = ko.observable(UserWithSecurityDetailsDTO.RejectedByID);
            this.RejectedBy = ko.observable(UserWithSecurityDetailsDTO.RejectedBy);
            this.ID = ko.observable(UserWithSecurityDetailsDTO.ID);
            this.Timestamp = ko.observable(UserWithSecurityDetailsDTO.Timestamp);
        }
    }
    toData() {
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
    }
}
export class AclProjectRequestTypeWorkflowActivityViewModel extends AclViewModel {
    ProjectID;
    Project;
    RequestTypeID;
    RequestType;
    WorkflowActivityID;
    WorkflowActivity;
    constructor(AclProjectRequestTypeWorkflowActivityDTO) {
        super();
        if (AclProjectRequestTypeWorkflowActivityDTO == null) {
            this.ProjectID = ko.observable();
            this.Project = ko.observable();
            this.RequestTypeID = ko.observable();
            this.RequestType = ko.observable();
            this.WorkflowActivityID = ko.observable();
            this.WorkflowActivity = ko.observable();
            this.Allowed = ko.observable();
            this.PermissionID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.ProjectID);
            this.Project = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Project);
            this.RequestTypeID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.RequestTypeID);
            this.RequestType = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.RequestType);
            this.WorkflowActivityID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.WorkflowActivityID);
            this.WorkflowActivity = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.WorkflowActivity);
            this.Allowed = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Allowed);
            this.PermissionID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.PermissionID);
            this.Permission = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectRequestTypeWorkflowActivityDTO.Overridden);
        }
    }
    toData() {
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
    }
}
export class AclProjectDataMartRequestTypeViewModel extends AclDataMartRequestTypeViewModel {
    ProjectID;
    constructor(AclProjectDataMartRequestTypeDTO) {
        super();
        if (AclProjectDataMartRequestTypeDTO == null) {
            this.ProjectID = ko.observable();
            this.DataMartID = ko.observable();
            this.RequestTypeID = ko.observable();
            this.Permission = ko.observable();
            this.SecurityGroupID = ko.observable();
            this.SecurityGroup = ko.observable();
            this.Overridden = ko.observable();
        }
        else {
            this.ProjectID = ko.observable(AclProjectDataMartRequestTypeDTO.ProjectID);
            this.DataMartID = ko.observable(AclProjectDataMartRequestTypeDTO.DataMartID);
            this.RequestTypeID = ko.observable(AclProjectDataMartRequestTypeDTO.RequestTypeID);
            this.Permission = ko.observable(AclProjectDataMartRequestTypeDTO.Permission);
            this.SecurityGroupID = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroupID);
            this.SecurityGroup = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroup);
            this.Overridden = ko.observable(AclProjectDataMartRequestTypeDTO.Overridden);
        }
    }
    toData() {
        return {
            ProjectID: this.ProjectID(),
            DataMartID: this.DataMartID(),
            RequestTypeID: this.RequestTypeID(),
            Permission: this.Permission(),
            SecurityGroupID: this.SecurityGroupID(),
            SecurityGroup: this.SecurityGroup(),
            Overridden: this.Overridden(),
        };
    }
}
//# sourceMappingURL=Lpp.Dns.ViewModels.js.map
/// <reference path='../../Lpp.Pmn.Resources/Scripts/typings/knockout.mapping/knockout.mapping.d.ts' />
/// <reference path='Lpp.Dns.Interfaces.ts' />
module Dns.ViewModels {
	 export class ViewModel<D>{
	 	 constructor() {
	 	 }
	 	 public update(obj: any) {
	 	 	 for(var prop in obj) {
	 	 	 	 this[prop](obj[prop]);
	 	 	 }
	 	 }
	 }
	 export class EntityDtoViewModel<T> extends ViewModel<T> {
	 	 constructor(BaseDTO?: T)
	 	 {
	 	 	  super();
	 	 }
	 	  public toData(): Dns.Interfaces.IEntityDto {
	 	 	  return {
	 	 	 };
	 	 }
	 }
	 export class EntityDtoWithIDViewModel<T> extends EntityDtoViewModel<T> {
	 	 public ID: KnockoutObservable<any>;
	 	 public Timestamp: KnockoutObservable<any>;
	 	 constructor(BaseDTO?: T)
	 	 {
	 	 	 super(BaseDTO);
	 	 	 if (BaseDTO == null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	 }
	 	 }
	 	  public toData(): Dns.Interfaces.IEntityDto {
	 	 	  return {
	 	 	 	 ID: this.ID(),
	 	 	 	 Timestamp: this.Timestamp(),
	 	 	 };
	 	 }
	 }
	 export class DataModelProcessorViewModel extends ViewModel<Dns.Interfaces.IDataModelProcessorDTO>{
	 	 public ModelID: KnockoutObservable<any>;
	 	 public Processor: KnockoutObservable<string>;
	 	 public ProcessorID: KnockoutObservable<any>;
	 	 constructor(DataModelProcessorDTO?: Dns.Interfaces.IDataModelProcessorDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataModelProcessorDTO== null) {
	 	 	 	 this.ModelID = ko.observable<any>();
	 	 	 	 this.Processor = ko.observable<any>();
	 	 	 	 this.ProcessorID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ModelID = ko.observable(DataModelProcessorDTO.ModelID);
	 	 	 	 this.Processor = ko.observable(DataModelProcessorDTO.Processor);
	 	 	 	 this.ProcessorID = ko.observable(DataModelProcessorDTO.ProcessorID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataModelProcessorDTO{
	 	 	  return {
	 	 	 	ModelID: this.ModelID(),
	 	 	 	Processor: this.Processor(),
	 	 	 	ProcessorID: this.ProcessorID(),
	 	 	  };
	 	  }



	 }
	 export class PropertyChangeDetailViewModel extends ViewModel<Dns.Interfaces.IPropertyChangeDetailDTO>{
	 	 public Property: KnockoutObservable<string>;
	 	 public PropertyDisplayName: KnockoutObservable<string>;
	 	 public OriginalValue: KnockoutObservable<any>;
	 	 public OriginalValueDisplay: KnockoutObservable<string>;
	 	 public NewValue: KnockoutObservable<any>;
	 	 public NewValueDisplay: KnockoutObservable<string>;
	 	 constructor(PropertyChangeDetailDTO?: Dns.Interfaces.IPropertyChangeDetailDTO)
	 	  {
	 	 	  super();
	 	 	 if (PropertyChangeDetailDTO== null) {
	 	 	 	 this.Property = ko.observable<any>();
	 	 	 	 this.PropertyDisplayName = ko.observable<any>();
	 	 	 	 this.OriginalValue = ko.observable<any>();
	 	 	 	 this.OriginalValueDisplay = ko.observable<any>();
	 	 	 	 this.NewValue = ko.observable<any>();
	 	 	 	 this.NewValueDisplay = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Property = ko.observable(PropertyChangeDetailDTO.Property);
	 	 	 	 this.PropertyDisplayName = ko.observable(PropertyChangeDetailDTO.PropertyDisplayName);
	 	 	 	 this.OriginalValue = ko.observable(PropertyChangeDetailDTO.OriginalValue);
	 	 	 	 this.OriginalValueDisplay = ko.observable(PropertyChangeDetailDTO.OriginalValueDisplay);
	 	 	 	 this.NewValue = ko.observable(PropertyChangeDetailDTO.NewValue);
	 	 	 	 this.NewValueDisplay = ko.observable(PropertyChangeDetailDTO.NewValueDisplay);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IPropertyChangeDetailDTO{
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
	 export class HttpResponseErrors extends ViewModel<Dns.Interfaces.IHttpResponseErrors>{
	 	 public Errors: KnockoutObservableArray<string>;
	 	 constructor(HttpResponseErrors?: Dns.Interfaces.IHttpResponseErrors)
	 	  {
	 	 	  super();
	 	 	 if (HttpResponseErrors== null) {
	 	 	 	 this.Errors = ko.observableArray<string>();
	 	 	  }else{
	 	 	 	 this.Errors = ko.observableArray<string>(HttpResponseErrors.Errors == null ? null : HttpResponseErrors.Errors.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IHttpResponseErrors{
	 	 	  return {
	 	 	 	Errors: this.Errors == null ? null : this.Errors().map((item) => {return item;}),
	 	 	  };
	 	  }



	 }
	 export class AddWFCommentViewModel extends ViewModel<Dns.Interfaces.IAddWFCommentDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public WorkflowActivityID: KnockoutObservable<any>;
	 	 public Comment: KnockoutObservable<string>;
	 	 constructor(AddWFCommentDTO?: Dns.Interfaces.IAddWFCommentDTO)
	 	  {
	 	 	  super();
	 	 	 if (AddWFCommentDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.WorkflowActivityID = ko.observable<any>();
	 	 	 	 this.Comment = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestID = ko.observable(AddWFCommentDTO.RequestID);
	 	 	 	 this.WorkflowActivityID = ko.observable(AddWFCommentDTO.WorkflowActivityID);
	 	 	 	 this.Comment = ko.observable(AddWFCommentDTO.Comment);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAddWFCommentDTO{
	 	 	  return {
	 	 	 	RequestID: this.RequestID(),
	 	 	 	WorkflowActivityID: this.WorkflowActivityID(),
	 	 	 	Comment: this.Comment(),
	 	 	  };
	 	  }



	 }
	 export class CommentDocumentReferenceViewModel extends ViewModel<Dns.Interfaces.ICommentDocumentReferenceDTO>{
	 	 public CommentID: KnockoutObservable<any>;
	 	 public DocumentID: KnockoutObservable<any>;
	 	 public RevisionSetID: KnockoutObservable<any>;
	 	 public DocumentName: KnockoutObservable<string>;
	 	 public FileName: KnockoutObservable<string>;
	 	 constructor(CommentDocumentReferenceDTO?: Dns.Interfaces.ICommentDocumentReferenceDTO)
	 	  {
	 	 	  super();
	 	 	 if (CommentDocumentReferenceDTO== null) {
	 	 	 	 this.CommentID = ko.observable<any>();
	 	 	 	 this.DocumentID = ko.observable<any>();
	 	 	 	 this.RevisionSetID = ko.observable<any>();
	 	 	 	 this.DocumentName = ko.observable<any>();
	 	 	 	 this.FileName = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.CommentID = ko.observable(CommentDocumentReferenceDTO.CommentID);
	 	 	 	 this.DocumentID = ko.observable(CommentDocumentReferenceDTO.DocumentID);
	 	 	 	 this.RevisionSetID = ko.observable(CommentDocumentReferenceDTO.RevisionSetID);
	 	 	 	 this.DocumentName = ko.observable(CommentDocumentReferenceDTO.DocumentName);
	 	 	 	 this.FileName = ko.observable(CommentDocumentReferenceDTO.FileName);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ICommentDocumentReferenceDTO{
	 	 	  return {
	 	 	 	CommentID: this.CommentID(),
	 	 	 	DocumentID: this.DocumentID(),
	 	 	 	RevisionSetID: this.RevisionSetID(),
	 	 	 	DocumentName: this.DocumentName(),
	 	 	 	FileName: this.FileName(),
	 	 	  };
	 	  }



	 }
	 export class UpdateDataMartInstalledModelsViewModel extends ViewModel<Dns.Interfaces.IUpdateDataMartInstalledModelsDTO>{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public Models: KnockoutObservableArray<DataMartInstalledModelViewModel>;
	 	 constructor(UpdateDataMartInstalledModelsDTO?: Dns.Interfaces.IUpdateDataMartInstalledModelsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateDataMartInstalledModelsDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.Models = ko.observableArray<DataMartInstalledModelViewModel>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(UpdateDataMartInstalledModelsDTO.DataMartID);
	 	 	 	 this.Models = ko.observableArray<DataMartInstalledModelViewModel>(UpdateDataMartInstalledModelsDTO.Models == null ? null : UpdateDataMartInstalledModelsDTO.Models.map((item) => {return new DataMartInstalledModelViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateDataMartInstalledModelsDTO{
	 	 	  return {
	 	 	 	DataMartID: this.DataMartID(),
	 	 	 	Models: this.Models == null ? null : this.Models().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class DataAvailabilityPeriodCategoryViewModel extends ViewModel<Dns.Interfaces.IDataAvailabilityPeriodCategoryDTO>{
	 	 public CategoryType: KnockoutObservable<string>;
	 	 public CategoryDescription: KnockoutObservable<string>;
	 	 public Published: KnockoutObservable<boolean>;
	 	 public DataMartDescription: KnockoutObservable<string>;
	 	 constructor(DataAvailabilityPeriodCategoryDTO?: Dns.Interfaces.IDataAvailabilityPeriodCategoryDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataAvailabilityPeriodCategoryDTO== null) {
	 	 	 	 this.CategoryType = ko.observable<any>();
	 	 	 	 this.CategoryDescription = ko.observable<any>();
	 	 	 	 this.Published = ko.observable<any>();
	 	 	 	 this.DataMartDescription = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.CategoryType = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryType);
	 	 	 	 this.CategoryDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.CategoryDescription);
	 	 	 	 this.Published = ko.observable(DataAvailabilityPeriodCategoryDTO.Published);
	 	 	 	 this.DataMartDescription = ko.observable(DataAvailabilityPeriodCategoryDTO.DataMartDescription);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataAvailabilityPeriodCategoryDTO{
	 	 	  return {
	 	 	 	CategoryType: this.CategoryType(),
	 	 	 	CategoryDescription: this.CategoryDescription(),
	 	 	 	Published: this.Published(),
	 	 	 	DataMartDescription: this.DataMartDescription(),
	 	 	  };
	 	  }



	 }
	 export class DataMartAvailabilityPeriodViewModel extends ViewModel<Dns.Interfaces.IDataMartAvailabilityPeriodDTO>{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public Period: KnockoutObservable<string>;
	 	 public Active: KnockoutObservable<boolean>;
	 	 constructor(DataMartAvailabilityPeriodDTO?: Dns.Interfaces.IDataMartAvailabilityPeriodDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartAvailabilityPeriodDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Period = ko.observable<any>();
	 	 	 	 this.Active = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(DataMartAvailabilityPeriodDTO.DataMartID);
	 	 	 	 this.RequestID = ko.observable(DataMartAvailabilityPeriodDTO.RequestID);
	 	 	 	 this.RequestTypeID = ko.observable(DataMartAvailabilityPeriodDTO.RequestTypeID);
	 	 	 	 this.Period = ko.observable(DataMartAvailabilityPeriodDTO.Period);
	 	 	 	 this.Active = ko.observable(DataMartAvailabilityPeriodDTO.Active);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataMartAvailabilityPeriodDTO{
	 	 	  return {
	 	 	 	DataMartID: this.DataMartID(),
	 	 	 	RequestID: this.RequestID(),
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	Period: this.Period(),
	 	 	 	Active: this.Active(),
	 	 	  };
	 	  }



	 }
	 export class NotificationCrudViewModel extends ViewModel<Dns.Interfaces.INotificationCrudDTO>{
	 	 public ObjectID: KnockoutObservable<any>;
	 	 public State: KnockoutObservable<Dns.Enums.ObjectStates>;
	 	 constructor(NotificationCrudDTO?: Dns.Interfaces.INotificationCrudDTO)
	 	  {
	 	 	  super();
	 	 	 if (NotificationCrudDTO== null) {
	 	 	 	 this.ObjectID = ko.observable<any>();
	 	 	 	 this.State = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ObjectID = ko.observable(NotificationCrudDTO.ObjectID);
	 	 	 	 this.State = ko.observable(NotificationCrudDTO.State);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.INotificationCrudDTO{
	 	 	  return {
	 	 	 	ObjectID: this.ObjectID(),
	 	 	 	State: this.State(),
	 	 	  };
	 	  }



	 }
	 export class OrganizationUpdateEHRsesViewModel extends ViewModel<Dns.Interfaces.IOrganizationUpdateEHRsesDTO>{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public EHRS: KnockoutObservableArray<OrganizationEHRSViewModel>;
	 	 constructor(OrganizationUpdateEHRsesDTO?: Dns.Interfaces.IOrganizationUpdateEHRsesDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationUpdateEHRsesDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.EHRS = ko.observableArray<OrganizationEHRSViewModel>();
	 	 	  }else{
	 	 	 	 this.OrganizationID = ko.observable(OrganizationUpdateEHRsesDTO.OrganizationID);
	 	 	 	 this.EHRS = ko.observableArray<OrganizationEHRSViewModel>(OrganizationUpdateEHRsesDTO.EHRS == null ? null : OrganizationUpdateEHRsesDTO.EHRS.map((item) => {return new OrganizationEHRSViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IOrganizationUpdateEHRsesDTO{
	 	 	  return {
	 	 	 	OrganizationID: this.OrganizationID(),
	 	 	 	EHRS: this.EHRS == null ? null : this.EHRS().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class ProjectDataMartUpdateViewModel extends ViewModel<Dns.Interfaces.IProjectDataMartUpdateDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public DataMarts: KnockoutObservableArray<ProjectDataMartViewModel>;
	 	 constructor(ProjectDataMartUpdateDTO?: Dns.Interfaces.IProjectDataMartUpdateDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectDataMartUpdateDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.DataMarts = ko.observableArray<ProjectDataMartViewModel>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectDataMartUpdateDTO.ProjectID);
	 	 	 	 this.DataMarts = ko.observableArray<ProjectDataMartViewModel>(ProjectDataMartUpdateDTO.DataMarts == null ? null : ProjectDataMartUpdateDTO.DataMarts.map((item) => {return new ProjectDataMartViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectDataMartUpdateDTO{
	 	 	  return {
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class ProjectOrganizationUpdateViewModel extends ViewModel<Dns.Interfaces.IProjectOrganizationUpdateDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Organizations: KnockoutObservableArray<ProjectOrganizationViewModel>;
	 	 constructor(ProjectOrganizationUpdateDTO?: Dns.Interfaces.IProjectOrganizationUpdateDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectOrganizationUpdateDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Organizations = ko.observableArray<ProjectOrganizationViewModel>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectOrganizationUpdateDTO.ProjectID);
	 	 	 	 this.Organizations = ko.observableArray<ProjectOrganizationViewModel>(ProjectOrganizationUpdateDTO.Organizations == null ? null : ProjectOrganizationUpdateDTO.Organizations.map((item) => {return new ProjectOrganizationViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectOrganizationUpdateDTO{
	 	 	  return {
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	Organizations: this.Organizations == null ? null : this.Organizations().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class UpdateProjectRequestTypesViewModel extends ViewModel<Dns.Interfaces.IUpdateProjectRequestTypesDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public RequestTypes: KnockoutObservableArray<ProjectRequestTypeViewModel>;
	 	 constructor(UpdateProjectRequestTypesDTO?: Dns.Interfaces.IUpdateProjectRequestTypesDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateProjectRequestTypesDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.RequestTypes = ko.observableArray<ProjectRequestTypeViewModel>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(UpdateProjectRequestTypesDTO.ProjectID);
	 	 	 	 this.RequestTypes = ko.observableArray<ProjectRequestTypeViewModel>(UpdateProjectRequestTypesDTO.RequestTypes == null ? null : UpdateProjectRequestTypesDTO.RequestTypes.map((item) => {return new ProjectRequestTypeViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateProjectRequestTypesDTO{
	 	 	  return {
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class HasGlobalSecurityForTemplateViewModel extends ViewModel<Dns.Interfaces.IHasGlobalSecurityForTemplateDTO>{
	 	 public SecurityGroupExistsForGlobalPermission: KnockoutObservable<boolean>;
	 	 public CurrentUserHasGlobalPermission: KnockoutObservable<boolean>;
	 	 constructor(HasGlobalSecurityForTemplateDTO?: Dns.Interfaces.IHasGlobalSecurityForTemplateDTO)
	 	  {
	 	 	  super();
	 	 	 if (HasGlobalSecurityForTemplateDTO== null) {
	 	 	 	 this.SecurityGroupExistsForGlobalPermission = ko.observable<any>();
	 	 	 	 this.CurrentUserHasGlobalPermission = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.SecurityGroupExistsForGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.SecurityGroupExistsForGlobalPermission);
	 	 	 	 this.CurrentUserHasGlobalPermission = ko.observable(HasGlobalSecurityForTemplateDTO.CurrentUserHasGlobalPermission);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IHasGlobalSecurityForTemplateDTO{
	 	 	  return {
	 	 	 	SecurityGroupExistsForGlobalPermission: this.SecurityGroupExistsForGlobalPermission(),
	 	 	 	CurrentUserHasGlobalPermission: this.CurrentUserHasGlobalPermission(),
	 	 	  };
	 	  }



	 }
	 export class ApproveRejectResponseViewModel extends ViewModel<Dns.Interfaces.IApproveRejectResponseDTO>{
	 	 public ResponseID: KnockoutObservable<any>;
	 	 constructor(ApproveRejectResponseDTO?: Dns.Interfaces.IApproveRejectResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (ApproveRejectResponseDTO== null) {
	 	 	 	 this.ResponseID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ResponseID = ko.observable(ApproveRejectResponseDTO.ResponseID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IApproveRejectResponseDTO{
	 	 	  return {
	 	 	 	ResponseID: this.ResponseID(),
	 	 	  };
	 	  }



	 }
	 export class HomepageRouteDetailViewModel extends ViewModel<Dns.Interfaces.IHomepageRouteDetailDTO>{
	 	 public RequestDataMartID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Identifier: KnockoutObservable<number>;
	 	 public SubmittedOn: KnockoutObservable<Date>;
	 	 public SubmittedByName: KnockoutObservable<string>;
	 	 public StatusText: KnockoutObservable<string>;
	 	 public RequestStatus: KnockoutObservable<Dns.Enums.RequestStatuses>;
	 	 public RoutingStatus: KnockoutObservable<Dns.Enums.RoutingStatus>;
	 	 public RoutingStatusText: KnockoutObservable<string>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 public Project: KnockoutObservable<string>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public MSRequestID: KnockoutObservable<string>;
	 	 public IsWorkflowRequest: KnockoutObservable<boolean>;
	 	 public CanEditMetadata: KnockoutObservable<boolean>;
	 	 constructor(HomepageRouteDetailDTO?: Dns.Interfaces.IHomepageRouteDetailDTO)
	 	  {
	 	 	  super();
	 	 	 if (HomepageRouteDetailDTO== null) {
	 	 	 	 this.RequestDataMartID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Identifier = ko.observable<any>();
	 	 	 	 this.SubmittedOn = ko.observable<any>();
	 	 	 	 this.SubmittedByName = ko.observable<any>();
	 	 	 	 this.StatusText = ko.observable<any>();
	 	 	 	 this.RequestStatus = ko.observable<any>();
	 	 	 	 this.RoutingStatus = ko.observable<any>();
	 	 	 	 this.RoutingStatusText = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.MSRequestID = ko.observable<any>();
	 	 	 	 this.IsWorkflowRequest = ko.observable<any>();
	 	 	 	 this.CanEditMetadata = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestDataMartID = ko.observable(HomepageRouteDetailDTO.RequestDataMartID);
	 	 	 	 this.DataMartID = ko.observable(HomepageRouteDetailDTO.DataMartID);
	 	 	 	 this.RequestID = ko.observable(HomepageRouteDetailDTO.RequestID);
	 	 	 	 this.Name = ko.observable(HomepageRouteDetailDTO.Name);
	 	 	 	 this.Identifier = ko.observable(HomepageRouteDetailDTO.Identifier);
	 	 	 	 this.SubmittedOn = ko.observable(HomepageRouteDetailDTO.SubmittedOn);
	 	 	 	 this.SubmittedByName = ko.observable(HomepageRouteDetailDTO.SubmittedByName);
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

	 	 public toData(): Dns.Interfaces.IHomepageRouteDetailDTO{
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
	 	  }



	 }
	 export class RejectResponseViewModel extends ViewModel<Dns.Interfaces.IRejectResponseDTO>{
	 	 public Message: KnockoutObservable<string>;
	 	 public ResponseIDs: KnockoutObservableArray<any>;
	 	 constructor(RejectResponseDTO?: Dns.Interfaces.IRejectResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (RejectResponseDTO== null) {
	 	 	 	 this.Message = ko.observable<any>();
	 	 	 	 this.ResponseIDs = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.Message = ko.observable(RejectResponseDTO.Message);
	 	 	 	 this.ResponseIDs = ko.observableArray<any>(RejectResponseDTO.ResponseIDs == null ? null : RejectResponseDTO.ResponseIDs.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRejectResponseDTO{
	 	 	  return {
	 	 	 	Message: this.Message(),
	 	 	 	ResponseIDs: this.ResponseIDs(),
	 	 	  };
	 	  }



	 }
	 export class ApproveResponseViewModel extends ViewModel<Dns.Interfaces.IApproveResponseDTO>{
	 	 public Message: KnockoutObservable<string>;
	 	 public ResponseIDs: KnockoutObservableArray<any>;
	 	 constructor(ApproveResponseDTO?: Dns.Interfaces.IApproveResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (ApproveResponseDTO== null) {
	 	 	 	 this.Message = ko.observable<any>();
	 	 	 	 this.ResponseIDs = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.Message = ko.observable(ApproveResponseDTO.Message);
	 	 	 	 this.ResponseIDs = ko.observableArray<any>(ApproveResponseDTO.ResponseIDs == null ? null : ApproveResponseDTO.ResponseIDs.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IApproveResponseDTO{
	 	 	  return {
	 	 	 	Message: this.Message(),
	 	 	 	ResponseIDs: this.ResponseIDs(),
	 	 	  };
	 	  }



	 }
	 export class RequestCompletionRequestViewModel extends ViewModel<Dns.Interfaces.IRequestCompletionRequestDTO>{
	 	 public DemandActivityResultID: KnockoutObservable<any>;
	 	 public Dto: RequestViewModel;
	 	 public DataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
	 	 public Data: KnockoutObservable<string>;
	 	 public Comment: KnockoutObservable<string>;
	 	 constructor(RequestCompletionRequestDTO?: Dns.Interfaces.IRequestCompletionRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestCompletionRequestDTO== null) {
	 	 	 	 this.DemandActivityResultID = ko.observable<any>();
	 	 	 	 this.Dto = new RequestViewModel();
	 	 	 	 this.DataMarts = ko.observableArray<RequestDataMartViewModel>();
	 	 	 	 this.Data = ko.observable<any>();
	 	 	 	 this.Comment = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DemandActivityResultID = ko.observable(RequestCompletionRequestDTO.DemandActivityResultID);
	 	 	 	 this.Dto = new RequestViewModel(RequestCompletionRequestDTO.Dto);
	 	 	 	 this.DataMarts = ko.observableArray<RequestDataMartViewModel>(RequestCompletionRequestDTO.DataMarts == null ? null : RequestCompletionRequestDTO.DataMarts.map((item) => {return new RequestDataMartViewModel(item);}));
	 	 	 	 this.Data = ko.observable(RequestCompletionRequestDTO.Data);
	 	 	 	 this.Comment = ko.observable(RequestCompletionRequestDTO.Comment);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestCompletionRequestDTO{
	 	 	  return {
	 	 	 	DemandActivityResultID: this.DemandActivityResultID(),
	 	 	 	Dto: this.Dto.toData(),
	 	 	 	DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => {return item.toData();}),
	 	 	 	Data: this.Data(),
	 	 	 	Comment: this.Comment(),
	 	 	  };
	 	  }



	 }
	 export class RequestCompletionResponseViewModel extends ViewModel<Dns.Interfaces.IRequestCompletionResponseDTO>{
	 	 public Uri: KnockoutObservable<string>;
	 	 public Entity: RequestViewModel;
	 	 public DataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
	 	 constructor(RequestCompletionResponseDTO?: Dns.Interfaces.IRequestCompletionResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestCompletionResponseDTO== null) {
	 	 	 	 this.Uri = ko.observable<any>();
	 	 	 	 this.Entity = new RequestViewModel();
	 	 	 	 this.DataMarts = ko.observableArray<RequestDataMartViewModel>();
	 	 	  }else{
	 	 	 	 this.Uri = ko.observable(RequestCompletionResponseDTO.Uri);
	 	 	 	 this.Entity = new RequestViewModel(RequestCompletionResponseDTO.Entity);
	 	 	 	 this.DataMarts = ko.observableArray<RequestDataMartViewModel>(RequestCompletionResponseDTO.DataMarts == null ? null : RequestCompletionResponseDTO.DataMarts.map((item) => {return new RequestDataMartViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestCompletionResponseDTO{
	 	 	  return {
	 	 	 	Uri: this.Uri(),
	 	 	 	Entity: this.Entity.toData(),
	 	 	 	DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class RequestSearchTermViewModel extends ViewModel<Dns.Interfaces.IRequestSearchTermDTO>{
	 	 public Type: KnockoutObservable<number>;
	 	 public StringValue: KnockoutObservable<string>;
	 	 public NumberValue: KnockoutObservable<number>;
	 	 public DateFrom: KnockoutObservable<Date>;
	 	 public DateTo: KnockoutObservable<Date>;
	 	 public NumberFrom: KnockoutObservable<number>;
	 	 public NumberTo: KnockoutObservable<number>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 constructor(RequestSearchTermDTO?: Dns.Interfaces.IRequestSearchTermDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestSearchTermDTO== null) {
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.StringValue = ko.observable<any>();
	 	 	 	 this.NumberValue = ko.observable<any>();
	 	 	 	 this.DateFrom = ko.observable<any>();
	 	 	 	 this.DateTo = ko.observable<any>();
	 	 	 	 this.NumberFrom = ko.observable<any>();
	 	 	 	 this.NumberTo = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IRequestSearchTermDTO{
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
	 export class RequestTypeModelViewModel extends ViewModel<Dns.Interfaces.IRequestTypeModelDTO>{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public DataModelID: KnockoutObservable<any>;
	 	 constructor(RequestTypeModelDTO?: Dns.Interfaces.IRequestTypeModelDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestTypeModelDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.DataModelID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypeID = ko.observable(RequestTypeModelDTO.RequestTypeID);
	 	 	 	 this.DataModelID = ko.observable(RequestTypeModelDTO.DataModelID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestTypeModelDTO{
	 	 	  return {
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	DataModelID: this.DataModelID(),
	 	 	  };
	 	  }



	 }
	 export class RequestUserViewModel extends ViewModel<Dns.Interfaces.IRequestUserDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public Username: KnockoutObservable<string>;
	 	 public FullName: KnockoutObservable<string>;
	 	 public Email: KnockoutObservable<string>;
	 	 public WorkflowRoleID: KnockoutObservable<any>;
	 	 public WorkflowRole: KnockoutObservable<string>;
	 	 public IsRequestCreatorRole: KnockoutObservable<boolean>;
	 	 constructor(RequestUserDTO?: Dns.Interfaces.IRequestUserDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestUserDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Username = ko.observable<any>();
	 	 	 	 this.FullName = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	 	 this.WorkflowRoleID = ko.observable<any>();
	 	 	 	 this.WorkflowRole = ko.observable<any>();
	 	 	 	 this.IsRequestCreatorRole = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IRequestUserDTO{
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
	 export class ResponseHistoryViewModel extends ViewModel<Dns.Interfaces.IResponseHistoryDTO>{
	 	 public DataMartName: KnockoutObservable<string>;
	 	 public HistoryItems: KnockoutObservableArray<ResponseHistoryItemViewModel>;
	 	 public ErrorMessage: KnockoutObservable<string>;
	 	 constructor(ResponseHistoryDTO?: Dns.Interfaces.IResponseHistoryDTO)
	 	  {
	 	 	  super();
	 	 	 if (ResponseHistoryDTO== null) {
	 	 	 	 this.DataMartName = ko.observable<any>();
	 	 	 	 this.HistoryItems = ko.observableArray<ResponseHistoryItemViewModel>();
	 	 	 	 this.ErrorMessage = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartName = ko.observable(ResponseHistoryDTO.DataMartName);
	 	 	 	 this.HistoryItems = ko.observableArray<ResponseHistoryItemViewModel>(ResponseHistoryDTO.HistoryItems == null ? null : ResponseHistoryDTO.HistoryItems.map((item) => {return new ResponseHistoryItemViewModel(item);}));
	 	 	 	 this.ErrorMessage = ko.observable(ResponseHistoryDTO.ErrorMessage);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IResponseHistoryDTO{
	 	 	  return {
	 	 	 	DataMartName: this.DataMartName(),
	 	 	 	HistoryItems: this.HistoryItems == null ? null : this.HistoryItems().map((item) => {return item.toData();}),
	 	 	 	ErrorMessage: this.ErrorMessage(),
	 	 	  };
	 	  }



	 }
	 export class ResponseHistoryItemViewModel extends ViewModel<Dns.Interfaces.IResponseHistoryItemDTO>{
	 	 public ResponseID: KnockoutObservable<any>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public DateTime: KnockoutObservable<Date>;
	 	 public Action: KnockoutObservable<string>;
	 	 public UserName: KnockoutObservable<string>;
	 	 public Message: KnockoutObservable<string>;
	 	 public IsResponseItem: KnockoutObservable<boolean>;
	 	 public IsCurrent: KnockoutObservable<boolean>;
	 	 constructor(ResponseHistoryItemDTO?: Dns.Interfaces.IResponseHistoryItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (ResponseHistoryItemDTO== null) {
	 	 	 	 this.ResponseID = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.DateTime = ko.observable<any>();
	 	 	 	 this.Action = ko.observable<any>();
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Message = ko.observable<any>();
	 	 	 	 this.IsResponseItem = ko.observable<any>();
	 	 	 	 this.IsCurrent = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IResponseHistoryItemDTO{
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
	 export class SaveCriteriaGroupRequestViewModel extends ViewModel<Dns.Interfaces.ISaveCriteriaGroupRequestDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Json: KnockoutObservable<string>;
	 	 public AdapterDetail: KnockoutObservable<Dns.Enums.QueryComposerQueryTypes>;
	 	 public TemplateID: KnockoutObservable<any>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 constructor(SaveCriteriaGroupRequestDTO?: Dns.Interfaces.ISaveCriteriaGroupRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (SaveCriteriaGroupRequestDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Json = ko.observable<any>();
	 	 	 	 this.AdapterDetail = ko.observable<any>();
	 	 	 	 this.TemplateID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(SaveCriteriaGroupRequestDTO.Name);
	 	 	 	 this.Description = ko.observable(SaveCriteriaGroupRequestDTO.Description);
	 	 	 	 this.Json = ko.observable(SaveCriteriaGroupRequestDTO.Json);
	 	 	 	 this.AdapterDetail = ko.observable(SaveCriteriaGroupRequestDTO.AdapterDetail);
	 	 	 	 this.TemplateID = ko.observable(SaveCriteriaGroupRequestDTO.TemplateID);
	 	 	 	 this.RequestTypeID = ko.observable(SaveCriteriaGroupRequestDTO.RequestTypeID);
	 	 	 	 this.RequestID = ko.observable(SaveCriteriaGroupRequestDTO.RequestID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ISaveCriteriaGroupRequestDTO{
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
	 export class UpdateRequestDataMartStatusViewModel extends ViewModel<Dns.Interfaces.IUpdateRequestDataMartStatusDTO>{
	 	 public RequestDataMartID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public NewStatus: KnockoutObservable<Dns.Enums.RoutingStatus>;
	 	 public Message: KnockoutObservable<string>;
	 	 constructor(UpdateRequestDataMartStatusDTO?: Dns.Interfaces.IUpdateRequestDataMartStatusDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRequestDataMartStatusDTO== null) {
	 	 	 	 this.RequestDataMartID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.NewStatus = ko.observable<any>();
	 	 	 	 this.Message = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestDataMartID = ko.observable(UpdateRequestDataMartStatusDTO.RequestDataMartID);
	 	 	 	 this.DataMartID = ko.observable(UpdateRequestDataMartStatusDTO.DataMartID);
	 	 	 	 this.NewStatus = ko.observable(UpdateRequestDataMartStatusDTO.NewStatus);
	 	 	 	 this.Message = ko.observable(UpdateRequestDataMartStatusDTO.Message);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRequestDataMartStatusDTO{
	 	 	  return {
	 	 	 	RequestDataMartID: this.RequestDataMartID(),
	 	 	 	DataMartID: this.DataMartID(),
	 	 	 	NewStatus: this.NewStatus(),
	 	 	 	Message: this.Message(),
	 	 	  };
	 	  }



	 }
	 export class UpdateRequestTypeModelsViewModel extends ViewModel<Dns.Interfaces.IUpdateRequestTypeModelsDTO>{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public DataModels: KnockoutObservableArray<any>;
	 	 constructor(UpdateRequestTypeModelsDTO?: Dns.Interfaces.IUpdateRequestTypeModelsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRequestTypeModelsDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.DataModels = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypeID = ko.observable(UpdateRequestTypeModelsDTO.RequestTypeID);
	 	 	 	 this.DataModels = ko.observableArray<any>(UpdateRequestTypeModelsDTO.DataModels == null ? null : UpdateRequestTypeModelsDTO.DataModels.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRequestTypeModelsDTO{
	 	 	  return {
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	DataModels: this.DataModels(),
	 	 	  };
	 	  }



	 }
	 export class UpdateRequestTypeRequestViewModel extends ViewModel<Dns.Interfaces.IUpdateRequestTypeRequestDTO>{
	 	 public RequestType: RequestTypeViewModel;
	 	 public Template: TemplateViewModel;
	 	 public Terms: KnockoutObservableArray<any>;
	 	 public NotAllowedTerms: KnockoutObservableArray<SectionSpecificTermViewModel>;
	 	 public Models: KnockoutObservableArray<any>;
	 	 constructor(UpdateRequestTypeRequestDTO?: Dns.Interfaces.IUpdateRequestTypeRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRequestTypeRequestDTO== null) {
	 	 	 	 this.RequestType = new RequestTypeViewModel();
	 	 	 	 this.Template = new TemplateViewModel();
	 	 	 	 this.Terms = ko.observableArray<any>();
	 	 	 	 this.NotAllowedTerms = ko.observableArray<SectionSpecificTermViewModel>();
	 	 	 	 this.Models = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.RequestType = new RequestTypeViewModel(UpdateRequestTypeRequestDTO.RequestType);
	 	 	 	 this.Template = new TemplateViewModel(UpdateRequestTypeRequestDTO.Template);
	 	 	 	 this.Terms = ko.observableArray<any>(UpdateRequestTypeRequestDTO.Terms == null ? null : UpdateRequestTypeRequestDTO.Terms.map((item) => {return item;}));
	 	 	 	 this.NotAllowedTerms = ko.observableArray<SectionSpecificTermViewModel>(UpdateRequestTypeRequestDTO.NotAllowedTerms == null ? null : UpdateRequestTypeRequestDTO.NotAllowedTerms.map((item) => {return new SectionSpecificTermViewModel(item);}));
	 	 	 	 this.Models = ko.observableArray<any>(UpdateRequestTypeRequestDTO.Models == null ? null : UpdateRequestTypeRequestDTO.Models.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRequestTypeRequestDTO{
	 	 	  return {
	 	 	 	RequestType: this.RequestType.toData(),
	 	 	 	Template: this.Template.toData(),
	 	 	 	Terms: this.Terms(),
	 	 	 	NotAllowedTerms: this.NotAllowedTerms == null ? null : this.NotAllowedTerms().map((item) => {return item.toData();}),
	 	 	 	Models: this.Models(),
	 	 	  };
	 	  }



	 }
	 export class UpdateRequestTypeResponseViewModel extends ViewModel<Dns.Interfaces.IUpdateRequestTypeResponseDTO>{
	 	 public RequestType: RequestTypeViewModel;
	 	 public Template: TemplateViewModel;
	 	 constructor(UpdateRequestTypeResponseDTO?: Dns.Interfaces.IUpdateRequestTypeResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRequestTypeResponseDTO== null) {
	 	 	 	 this.RequestType = new RequestTypeViewModel();
	 	 	 	 this.Template = new TemplateViewModel();
	 	 	  }else{
	 	 	 	 this.RequestType = new RequestTypeViewModel(UpdateRequestTypeResponseDTO.RequestType);
	 	 	 	 this.Template = new TemplateViewModel(UpdateRequestTypeResponseDTO.Template);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRequestTypeResponseDTO{
	 	 	  return {
	 	 	 	RequestType: this.RequestType.toData(),
	 	 	 	Template: this.Template.toData(),
	 	 	  };
	 	  }



	 }
	 export class UpdateRequestTypeTermsViewModel extends ViewModel<Dns.Interfaces.IUpdateRequestTypeTermsDTO>{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public Terms: KnockoutObservableArray<any>;
	 	 constructor(UpdateRequestTypeTermsDTO?: Dns.Interfaces.IUpdateRequestTypeTermsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRequestTypeTermsDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Terms = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypeID = ko.observable(UpdateRequestTypeTermsDTO.RequestTypeID);
	 	 	 	 this.Terms = ko.observableArray<any>(UpdateRequestTypeTermsDTO.Terms == null ? null : UpdateRequestTypeTermsDTO.Terms.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRequestTypeTermsDTO{
	 	 	  return {
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	Terms: this.Terms(),
	 	 	  };
	 	  }



	 }
	 export class HomepageTaskRequestUserViewModel extends ViewModel<Dns.Interfaces.IHomepageTaskRequestUserDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public TaskID: KnockoutObservable<any>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public UserName: KnockoutObservable<string>;
	 	 public FirstName: KnockoutObservable<string>;
	 	 public LastName: KnockoutObservable<string>;
	 	 public WorkflowRoleID: KnockoutObservable<any>;
	 	 public WorkflowRole: KnockoutObservable<string>;
	 	 constructor(HomepageTaskRequestUserDTO?: Dns.Interfaces.IHomepageTaskRequestUserDTO)
	 	  {
	 	 	  super();
	 	 	 if (HomepageTaskRequestUserDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.TaskID = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.FirstName = ko.observable<any>();
	 	 	 	 this.LastName = ko.observable<any>();
	 	 	 	 this.WorkflowRoleID = ko.observable<any>();
	 	 	 	 this.WorkflowRole = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IHomepageTaskRequestUserDTO{
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
	 export class HomepageTaskSummaryViewModel extends ViewModel<Dns.Interfaces.IHomepageTaskSummaryDTO>{
	 	 public TaskID: KnockoutObservable<any>;
	 	 public TaskName: KnockoutObservable<string>;
	 	 public TaskStatus: KnockoutObservable<Dns.Enums.TaskStatuses>;
	 	 public TaskStatusText: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public StartOn: KnockoutObservable<Date>;
	 	 public EndOn: KnockoutObservable<Date>;
	 	 public Type: KnockoutObservable<string>;
	 	 public DirectToRequest: KnockoutObservable<boolean>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Identifier: KnockoutObservable<string>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public MSRequestID: KnockoutObservable<string>;
	 	 public RequestStatus: KnockoutObservable<Dns.Enums.RequestStatuses>;
	 	 public RequestStatusText: KnockoutObservable<string>;
	 	 public NewUserID: KnockoutObservable<any>;
	 	 public AssignedResources: KnockoutObservable<string>;
	 	 constructor(HomepageTaskSummaryDTO?: Dns.Interfaces.IHomepageTaskSummaryDTO)
	 	  {
	 	 	  super();
	 	 	 if (HomepageTaskSummaryDTO== null) {
	 	 	 	 this.TaskID = ko.observable<any>();
	 	 	 	 this.TaskName = ko.observable<any>();
	 	 	 	 this.TaskStatus = ko.observable<any>();
	 	 	 	 this.TaskStatusText = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.StartOn = ko.observable<any>();
	 	 	 	 this.EndOn = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.DirectToRequest = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Identifier = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.MSRequestID = ko.observable<any>();
	 	 	 	 this.RequestStatus = ko.observable<any>();
	 	 	 	 this.RequestStatusText = ko.observable<any>();
	 	 	 	 this.NewUserID = ko.observable<any>();
	 	 	 	 this.AssignedResources = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IHomepageTaskSummaryDTO{
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
	 export class ActivityViewModel extends ViewModel<Dns.Interfaces.IActivityDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Activities: KnockoutObservableArray<ActivityViewModel>;
	 	 public Description: KnockoutObservable<string>;
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public DisplayOrder: KnockoutObservable<number>;
	 	 public TaskLevel: KnockoutObservable<number>;
	 	 public ParentActivityID: KnockoutObservable<any>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 constructor(ActivityDTO?: Dns.Interfaces.IActivityDTO)
	 	  {
	 	 	  super();
	 	 	 if (ActivityDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Activities = ko.observableArray<ActivityViewModel>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.DisplayOrder = ko.observable<any>();
	 	 	 	 this.TaskLevel = ko.observable<any>();
	 	 	 	 this.ParentActivityID = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(ActivityDTO.ID);
	 	 	 	 this.Name = ko.observable(ActivityDTO.Name);
	 	 	 	 this.Activities = ko.observableArray<ActivityViewModel>(ActivityDTO.Activities == null ? null : ActivityDTO.Activities.map((item) => {return new ActivityViewModel(item);}));
	 	 	 	 this.Description = ko.observable(ActivityDTO.Description);
	 	 	 	 this.ProjectID = ko.observable(ActivityDTO.ProjectID);
	 	 	 	 this.DisplayOrder = ko.observable(ActivityDTO.DisplayOrder);
	 	 	 	 this.TaskLevel = ko.observable(ActivityDTO.TaskLevel);
	 	 	 	 this.ParentActivityID = ko.observable(ActivityDTO.ParentActivityID);
	 	 	 	 this.Acronym = ko.observable(ActivityDTO.Acronym);
	 	 	 	 this.Deleted = ko.observable(ActivityDTO.Deleted);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IActivityDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Name: this.Name(),
	 	 	 	Activities: this.Activities == null ? null : this.Activities().map((item) => {return item.toData();}),
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
	 export class DataMartTypeViewModel extends ViewModel<Dns.Interfaces.IDataMartTypeDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 constructor(DataMartTypeDTO?: Dns.Interfaces.IDataMartTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartTypeDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(DataMartTypeDTO.ID);
	 	 	 	 this.Name = ko.observable(DataMartTypeDTO.Name);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataMartTypeDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Name: this.Name(),
	 	 	  };
	 	  }



	 }
	 export class DataMartInstalledModelViewModel extends ViewModel<Dns.Interfaces.IDataMartInstalledModelDTO>{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public ModelID: KnockoutObservable<any>;
	 	 public Model: KnockoutObservable<string>;
	 	 public Properties: KnockoutObservable<string>;
	 	 constructor(DataMartInstalledModelDTO?: Dns.Interfaces.IDataMartInstalledModelDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartInstalledModelDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.ModelID = ko.observable<any>();
	 	 	 	 this.Model = ko.observable<any>();
	 	 	 	 this.Properties = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(DataMartInstalledModelDTO.DataMartID);
	 	 	 	 this.ModelID = ko.observable(DataMartInstalledModelDTO.ModelID);
	 	 	 	 this.Model = ko.observable(DataMartInstalledModelDTO.Model);
	 	 	 	 this.Properties = ko.observable(DataMartInstalledModelDTO.Properties);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataMartInstalledModelDTO{
	 	 	  return {
	 	 	 	DataMartID: this.DataMartID(),
	 	 	 	ModelID: this.ModelID(),
	 	 	 	Model: this.Model(),
	 	 	 	Properties: this.Properties(),
	 	 	  };
	 	  }



	 }
	 export class DemographicViewModel extends ViewModel<Dns.Interfaces.IDemographicDTO>{
	 	 public Country: KnockoutObservable<string>;
	 	 public State: KnockoutObservable<string>;
	 	 public Town: KnockoutObservable<string>;
	 	 public Region: KnockoutObservable<string>;
	 	 public Gender: KnockoutObservable<string>;
	 	 public AgeGroup: KnockoutObservable<Dns.Enums.AgeGroups>;
	 	 public Ethnicity: KnockoutObservable<Dns.Enums.Ethnicities>;
	 	 public Count: KnockoutObservable<number>;
	 	 constructor(DemographicDTO?: Dns.Interfaces.IDemographicDTO)
	 	  {
	 	 	  super();
	 	 	 if (DemographicDTO== null) {
	 	 	 	 this.Country = ko.observable<any>();
	 	 	 	 this.State = ko.observable<any>();
	 	 	 	 this.Town = ko.observable<any>();
	 	 	 	 this.Region = ko.observable<any>();
	 	 	 	 this.Gender = ko.observable<any>();
	 	 	 	 this.AgeGroup = ko.observable<any>();
	 	 	 	 this.Ethnicity = ko.observable<any>();
	 	 	 	 this.Count = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IDemographicDTO{
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
	 export class LookupListCategoryViewModel extends ViewModel<Dns.Interfaces.ILookupListCategoryDTO>{
	 	 public ListId: KnockoutObservable<Dns.Enums.Lists>;
	 	 public CategoryId: KnockoutObservable<number>;
	 	 public CategoryName: KnockoutObservable<string>;
	 	 constructor(LookupListCategoryDTO?: Dns.Interfaces.ILookupListCategoryDTO)
	 	  {
	 	 	  super();
	 	 	 if (LookupListCategoryDTO== null) {
	 	 	 	 this.ListId = ko.observable<any>();
	 	 	 	 this.CategoryId = ko.observable<any>();
	 	 	 	 this.CategoryName = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ListId = ko.observable(LookupListCategoryDTO.ListId);
	 	 	 	 this.CategoryId = ko.observable(LookupListCategoryDTO.CategoryId);
	 	 	 	 this.CategoryName = ko.observable(LookupListCategoryDTO.CategoryName);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ILookupListCategoryDTO{
	 	 	  return {
	 	 	 	ListId: this.ListId(),
	 	 	 	CategoryId: this.CategoryId(),
	 	 	 	CategoryName: this.CategoryName(),
	 	 	  };
	 	  }



	 }
	 export class LookupListDetailRequestViewModel extends ViewModel<Dns.Interfaces.ILookupListDetailRequestDTO>{
	 	 public Codes: KnockoutObservableArray<string>;
	 	 public ListID: KnockoutObservable<Dns.Enums.Lists>;
	 	 constructor(LookupListDetailRequestDTO?: Dns.Interfaces.ILookupListDetailRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (LookupListDetailRequestDTO== null) {
	 	 	 	 this.Codes = ko.observableArray<string>();
	 	 	 	 this.ListID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Codes = ko.observableArray<string>(LookupListDetailRequestDTO.Codes == null ? null : LookupListDetailRequestDTO.Codes.map((item) => {return item;}));
	 	 	 	 this.ListID = ko.observable(LookupListDetailRequestDTO.ListID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ILookupListDetailRequestDTO{
	 	 	  return {
	 	 	 	Codes: this.Codes == null ? null : this.Codes().map((item) => {return item;}),
	 	 	 	ListID: this.ListID(),
	 	 	  };
	 	  }



	 }
	 export class LookupListViewModel extends ViewModel<Dns.Interfaces.ILookupListDTO>{
	 	 public ListId: KnockoutObservable<Dns.Enums.Lists>;
	 	 public ListName: KnockoutObservable<string>;
	 	 public Version: KnockoutObservable<string>;
	 	 constructor(LookupListDTO?: Dns.Interfaces.ILookupListDTO)
	 	  {
	 	 	  super();
	 	 	 if (LookupListDTO== null) {
	 	 	 	 this.ListId = ko.observable<any>();
	 	 	 	 this.ListName = ko.observable<any>();
	 	 	 	 this.Version = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ListId = ko.observable(LookupListDTO.ListId);
	 	 	 	 this.ListName = ko.observable(LookupListDTO.ListName);
	 	 	 	 this.Version = ko.observable(LookupListDTO.Version);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ILookupListDTO{
	 	 	  return {
	 	 	 	ListId: this.ListId(),
	 	 	 	ListName: this.ListName(),
	 	 	 	Version: this.Version(),
	 	 	  };
	 	  }



	 }
	 export class LookupListValueViewModel extends ViewModel<Dns.Interfaces.ILookupListValueDTO>{
	 	 public ListId: KnockoutObservable<Dns.Enums.Lists>;
	 	 public CategoryId: KnockoutObservable<number>;
	 	 public ItemName: KnockoutObservable<string>;
	 	 public ItemCode: KnockoutObservable<string>;
	 	 public ItemCodeWithNoPeriod: KnockoutObservable<string>;
	 	 public ExpireDate: KnockoutObservable<Date>;
	 	 public ID: KnockoutObservable<number>;
	 	 constructor(LookupListValueDTO?: Dns.Interfaces.ILookupListValueDTO)
	 	  {
	 	 	  super();
	 	 	 if (LookupListValueDTO== null) {
	 	 	 	 this.ListId = ko.observable<any>();
	 	 	 	 this.CategoryId = ko.observable<any>();
	 	 	 	 this.ItemName = ko.observable<any>();
	 	 	 	 this.ItemCode = ko.observable<any>();
	 	 	 	 this.ItemCodeWithNoPeriod = ko.observable<any>();
	 	 	 	 this.ExpireDate = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ListId = ko.observable(LookupListValueDTO.ListId);
	 	 	 	 this.CategoryId = ko.observable(LookupListValueDTO.CategoryId);
	 	 	 	 this.ItemName = ko.observable(LookupListValueDTO.ItemName);
	 	 	 	 this.ItemCode = ko.observable(LookupListValueDTO.ItemCode);
	 	 	 	 this.ItemCodeWithNoPeriod = ko.observable(LookupListValueDTO.ItemCodeWithNoPeriod);
	 	 	 	 this.ExpireDate = ko.observable(LookupListValueDTO.ExpireDate);
	 	 	 	 this.ID = ko.observable(LookupListValueDTO.ID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ILookupListValueDTO{
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
	 export class ProjectDataMartViewModel extends ViewModel<Dns.Interfaces.IProjectDataMartDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Project: KnockoutObservable<string>;
	 	 public ProjectAcronym: KnockoutObservable<string>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public DataMart: KnockoutObservable<string>;
	 	 public Organization: KnockoutObservable<string>;
	 	 constructor(ProjectDataMartDTO?: Dns.Interfaces.IProjectDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectDataMartDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.ProjectAcronym = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectDataMartDTO.ProjectID);
	 	 	 	 this.Project = ko.observable(ProjectDataMartDTO.Project);
	 	 	 	 this.ProjectAcronym = ko.observable(ProjectDataMartDTO.ProjectAcronym);
	 	 	 	 this.DataMartID = ko.observable(ProjectDataMartDTO.DataMartID);
	 	 	 	 this.DataMart = ko.observable(ProjectDataMartDTO.DataMart);
	 	 	 	 this.Organization = ko.observable(ProjectDataMartDTO.Organization);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectDataMartDTO{
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
	 export class RegistryItemDefinitionViewModel extends ViewModel<Dns.Interfaces.IRegistryItemDefinitionDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Category: KnockoutObservable<string>;
	 	 public Title: KnockoutObservable<string>;
	 	 constructor(RegistryItemDefinitionDTO?: Dns.Interfaces.IRegistryItemDefinitionDTO)
	 	  {
	 	 	  super();
	 	 	 if (RegistryItemDefinitionDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Category = ko.observable<any>();
	 	 	 	 this.Title = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(RegistryItemDefinitionDTO.ID);
	 	 	 	 this.Category = ko.observable(RegistryItemDefinitionDTO.Category);
	 	 	 	 this.Title = ko.observable(RegistryItemDefinitionDTO.Title);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRegistryItemDefinitionDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Category: this.Category(),
	 	 	 	Title: this.Title(),
	 	 	  };
	 	  }



	 }
	 export class UpdateRegistryItemsViewModel extends ViewModel<Dns.Interfaces.IUpdateRegistryItemsDTO>{
	 	 constructor(UpdateRegistryItemsDTO?: Dns.Interfaces.IUpdateRegistryItemsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateRegistryItemsDTO== null) {
	 	 	  }else{
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateRegistryItemsDTO{
	 	 	  return {
	 	 	  };
	 	  }



	 }
	 export class WorkplanTypeViewModel extends ViewModel<Dns.Interfaces.IWorkplanTypeDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public WorkplanTypeID: KnockoutObservable<number>;
	 	 public Name: KnockoutObservable<string>;
	 	 public NetworkID: KnockoutObservable<any>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 constructor(WorkplanTypeDTO?: Dns.Interfaces.IWorkplanTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (WorkplanTypeDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.WorkplanTypeID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.NetworkID = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(WorkplanTypeDTO.ID);
	 	 	 	 this.WorkplanTypeID = ko.observable(WorkplanTypeDTO.WorkplanTypeID);
	 	 	 	 this.Name = ko.observable(WorkplanTypeDTO.Name);
	 	 	 	 this.NetworkID = ko.observable(WorkplanTypeDTO.NetworkID);
	 	 	 	 this.Acronym = ko.observable(WorkplanTypeDTO.Acronym);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IWorkplanTypeDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	WorkplanTypeID: this.WorkplanTypeID(),
	 	 	 	Name: this.Name(),
	 	 	 	NetworkID: this.NetworkID(),
	 	 	 	Acronym: this.Acronym(),
	 	 	  };
	 	  }



	 }
	 export class RequesterCenterViewModel extends ViewModel<Dns.Interfaces.IRequesterCenterDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public RequesterCenterID: KnockoutObservable<number>;
	 	 public Name: KnockoutObservable<string>;
	 	 public NetworkID: KnockoutObservable<any>;
	 	 constructor(RequesterCenterDTO?: Dns.Interfaces.IRequesterCenterDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequesterCenterDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.RequesterCenterID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.NetworkID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(RequesterCenterDTO.ID);
	 	 	 	 this.RequesterCenterID = ko.observable(RequesterCenterDTO.RequesterCenterID);
	 	 	 	 this.Name = ko.observable(RequesterCenterDTO.Name);
	 	 	 	 this.NetworkID = ko.observable(RequesterCenterDTO.NetworkID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequesterCenterDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	RequesterCenterID: this.RequesterCenterID(),
	 	 	 	Name: this.Name(),
	 	 	 	NetworkID: this.NetworkID(),
	 	 	  };
	 	  }



	 }
	 export class QueryTypeViewModel extends ViewModel<Dns.Interfaces.IQueryTypeDTO>{
	 	 public ID: KnockoutObservable<number>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 constructor(QueryTypeDTO?: Dns.Interfaces.IQueryTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryTypeDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(QueryTypeDTO.ID);
	 	 	 	 this.Name = ko.observable(QueryTypeDTO.Name);
	 	 	 	 this.Description = ko.observable(QueryTypeDTO.Description);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryTypeDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Name: this.Name(),
	 	 	 	Description: this.Description(),
	 	 	  };
	 	  }



	 }
	 export class SecurityTupleViewModel extends ViewModel<Dns.Interfaces.ISecurityTupleDTO>{
	 	 public ID1: KnockoutObservable<any>;
	 	 public ID2: KnockoutObservable<any>;
	 	 public ID3: KnockoutObservable<any>;
	 	 public ID4: KnockoutObservable<any>;
	 	 public SubjectID: KnockoutObservable<any>;
	 	 public PrivilegeID: KnockoutObservable<any>;
	 	 public ViaMembership: KnockoutObservable<number>;
	 	 public DeniedEntries: KnockoutObservable<number>;
	 	 public ExplicitDeniedEntries: KnockoutObservable<number>;
	 	 public ExplicitAllowedEntries: KnockoutObservable<number>;
	 	 public ChangedOn: KnockoutObservable<Date>;
	 	 constructor(SecurityTupleDTO?: Dns.Interfaces.ISecurityTupleDTO)
	 	  {
	 	 	  super();
	 	 	 if (SecurityTupleDTO== null) {
	 	 	 	 this.ID1 = ko.observable<any>();
	 	 	 	 this.ID2 = ko.observable<any>();
	 	 	 	 this.ID3 = ko.observable<any>();
	 	 	 	 this.ID4 = ko.observable<any>();
	 	 	 	 this.SubjectID = ko.observable<any>();
	 	 	 	 this.PrivilegeID = ko.observable<any>();
	 	 	 	 this.ViaMembership = ko.observable<any>();
	 	 	 	 this.DeniedEntries = ko.observable<any>();
	 	 	 	 this.ExplicitDeniedEntries = ko.observable<any>();
	 	 	 	 this.ExplicitAllowedEntries = ko.observable<any>();
	 	 	 	 this.ChangedOn = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.ISecurityTupleDTO{
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
	 export class UpdateUserSecurityGroupsViewModel extends ViewModel<Dns.Interfaces.IUpdateUserSecurityGroupsDTO>{
	 	 public UserID: KnockoutObservable<any>;
	 	 public Groups: KnockoutObservableArray<SecurityGroupViewModel>;
	 	 constructor(UpdateUserSecurityGroupsDTO?: Dns.Interfaces.IUpdateUserSecurityGroupsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateUserSecurityGroupsDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Groups = ko.observableArray<SecurityGroupViewModel>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(UpdateUserSecurityGroupsDTO.UserID);
	 	 	 	 this.Groups = ko.observableArray<SecurityGroupViewModel>(UpdateUserSecurityGroupsDTO.Groups == null ? null : UpdateUserSecurityGroupsDTO.Groups.map((item) => {return new SecurityGroupViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateUserSecurityGroupsDTO{
	 	 	  return {
	 	 	 	UserID: this.UserID(),
	 	 	 	Groups: this.Groups == null ? null : this.Groups().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class DesignViewModel extends ViewModel<Dns.Interfaces.IDesignDTO>{
	 	 public Locked: KnockoutObservable<boolean>;
	 	 constructor(DesignDTO?: Dns.Interfaces.IDesignDTO)
	 	  {
	 	 	  super();
	 	 	 if (DesignDTO== null) {
	 	 	 	 this.Locked = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Locked = ko.observable(DesignDTO.Locked);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDesignDTO{
	 	 	  return {
	 	 	 	Locked: this.Locked(),
	 	 	  };
	 	  }



	 }
	 export class CodeSelectorValueViewModel extends ViewModel<Dns.Interfaces.ICodeSelectorValueDTO>{
	 	 public Code: KnockoutObservable<string>;
	 	 public Name: KnockoutObservable<string>;
	 	 public ExpireDate: KnockoutObservable<Date>;
	 	 constructor(CodeSelectorValueDTO?: Dns.Interfaces.ICodeSelectorValueDTO)
	 	  {
	 	 	  super();
	 	 	 if (CodeSelectorValueDTO== null) {
	 	 	 	 this.Code = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.ExpireDate = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Code = ko.observable(CodeSelectorValueDTO.Code);
	 	 	 	 this.Name = ko.observable(CodeSelectorValueDTO.Name);
	 	 	 	 this.ExpireDate = ko.observable(CodeSelectorValueDTO.ExpireDate);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ICodeSelectorValueDTO{
	 	 	  return {
	 	 	 	Code: this.Code(),
	 	 	 	Name: this.Name(),
	 	 	 	ExpireDate: this.ExpireDate(),
	 	 	  };
	 	  }



	 }
	 export class ThemeViewModel extends ViewModel<Dns.Interfaces.IThemeDTO>{
	 	 public Title: KnockoutObservable<string>;
	 	 public Terms: KnockoutObservable<string>;
	 	 public Info: KnockoutObservable<string>;
	 	 public Resources: KnockoutObservable<string>;
	 	 public Footer: KnockoutObservable<string>;
	 	 public LogoImage: KnockoutObservable<string>;
	 	 public SystemUserConfirmationTitle: KnockoutObservable<string>;
	 	 public SystemUserConfirmationContent: KnockoutObservable<string>;
	 	 constructor(ThemeDTO?: Dns.Interfaces.IThemeDTO)
	 	  {
	 	 	  super();
	 	 	 if (ThemeDTO== null) {
	 	 	 	 this.Title = ko.observable<any>();
	 	 	 	 this.Terms = ko.observable<any>();
	 	 	 	 this.Info = ko.observable<any>();
	 	 	 	 this.Resources = ko.observable<any>();
	 	 	 	 this.Footer = ko.observable<any>();
	 	 	 	 this.LogoImage = ko.observable<any>();
	 	 	 	 this.SystemUserConfirmationTitle = ko.observable<any>();
	 	 	 	 this.SystemUserConfirmationContent = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Title = ko.observable(ThemeDTO.Title);
	 	 	 	 this.Terms = ko.observable(ThemeDTO.Terms);
	 	 	 	 this.Info = ko.observable(ThemeDTO.Info);
	 	 	 	 this.Resources = ko.observable(ThemeDTO.Resources);
	 	 	 	 this.Footer = ko.observable(ThemeDTO.Footer);
	 	 	 	 this.LogoImage = ko.observable(ThemeDTO.LogoImage);
	 	 	 	 this.SystemUserConfirmationTitle = ko.observable(ThemeDTO.SystemUserConfirmationTitle);
	 	 	 	 this.SystemUserConfirmationContent = ko.observable(ThemeDTO.SystemUserConfirmationContent);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IThemeDTO{
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
	 	  }



	 }
	 export class AssignedUserNotificationViewModel extends ViewModel<Dns.Interfaces.IAssignedUserNotificationDTO>{
	 	 public Event: KnockoutObservable<string>;
	 	 public EventID: KnockoutObservable<any>;
	 	 public Level: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 constructor(AssignedUserNotificationDTO?: Dns.Interfaces.IAssignedUserNotificationDTO)
	 	  {
	 	 	  super();
	 	 	 if (AssignedUserNotificationDTO== null) {
	 	 	 	 this.Event = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Level = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Event = ko.observable(AssignedUserNotificationDTO.Event);
	 	 	 	 this.EventID = ko.observable(AssignedUserNotificationDTO.EventID);
	 	 	 	 this.Level = ko.observable(AssignedUserNotificationDTO.Level);
	 	 	 	 this.Description = ko.observable(AssignedUserNotificationDTO.Description);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAssignedUserNotificationDTO{
	 	 	  return {
	 	 	 	Event: this.Event(),
	 	 	 	EventID: this.EventID(),
	 	 	 	Level: this.Level(),
	 	 	 	Description: this.Description(),
	 	 	  };
	 	  }



	 }
	 export class MetadataEditPermissionsSummaryViewModel extends ViewModel<Dns.Interfaces.IMetadataEditPermissionsSummaryDTO>{
	 	 public CanEditRequestMetadata: KnockoutObservable<boolean>;
	 	 public EditableDataMarts: KnockoutObservableArray<any>;
	 	 constructor(MetadataEditPermissionsSummaryDTO?: Dns.Interfaces.IMetadataEditPermissionsSummaryDTO)
	 	  {
	 	 	  super();
	 	 	 if (MetadataEditPermissionsSummaryDTO== null) {
	 	 	 	 this.CanEditRequestMetadata = ko.observable<any>();
	 	 	 	 this.EditableDataMarts = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.CanEditRequestMetadata = ko.observable(MetadataEditPermissionsSummaryDTO.CanEditRequestMetadata);
	 	 	 	 this.EditableDataMarts = ko.observableArray<any>(MetadataEditPermissionsSummaryDTO.EditableDataMarts == null ? null : MetadataEditPermissionsSummaryDTO.EditableDataMarts.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IMetadataEditPermissionsSummaryDTO{
	 	 	  return {
	 	 	 	CanEditRequestMetadata: this.CanEditRequestMetadata(),
	 	 	 	EditableDataMarts: this.EditableDataMarts(),
	 	 	  };
	 	  }



	 }
	 export class NotificationViewModel extends ViewModel<Dns.Interfaces.INotificationDTO>{
	 	 public Timestamp: KnockoutObservable<Date>;
	 	 public Event: KnockoutObservable<string>;
	 	 public Message: KnockoutObservable<string>;
	 	 constructor(NotificationDTO?: Dns.Interfaces.INotificationDTO)
	 	  {
	 	 	  super();
	 	 	 if (NotificationDTO== null) {
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	 	 this.Message = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Timestamp = ko.observable(NotificationDTO.Timestamp);
	 	 	 	 this.Event = ko.observable(NotificationDTO.Event);
	 	 	 	 this.Message = ko.observable(NotificationDTO.Message);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.INotificationDTO{
	 	 	  return {
	 	 	 	Timestamp: this.Timestamp(),
	 	 	 	Event: this.Event(),
	 	 	 	Message: this.Message(),
	 	 	  };
	 	  }



	 }
	 export class ForgotPasswordViewModel extends ViewModel<Dns.Interfaces.IForgotPasswordDTO>{
	 	 public UserName: KnockoutObservable<string>;
	 	 public Email: KnockoutObservable<string>;
	 	 constructor(ForgotPasswordDTO?: Dns.Interfaces.IForgotPasswordDTO)
	 	  {
	 	 	  super();
	 	 	 if (ForgotPasswordDTO== null) {
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserName = ko.observable(ForgotPasswordDTO.UserName);
	 	 	 	 this.Email = ko.observable(ForgotPasswordDTO.Email);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IForgotPasswordDTO{
	 	 	  return {
	 	 	 	UserName: this.UserName(),
	 	 	 	Email: this.Email(),
	 	 	  };
	 	  }



	 }
	 export class LoginViewModel extends ViewModel<Dns.Interfaces.ILoginDTO>{
	 	 public UserName: KnockoutObservable<string>;
	 	 public Password: KnockoutObservable<string>;
	 	 public RememberMe: KnockoutObservable<boolean>;
	 	 public IPAddress: KnockoutObservable<string>;
	 	 public Enviorment: KnockoutObservable<string>;
	 	 constructor(LoginDTO?: Dns.Interfaces.ILoginDTO)
	 	  {
	 	 	  super();
	 	 	 if (LoginDTO== null) {
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Password = ko.observable<any>();
	 	 	 	 this.RememberMe = ko.observable<any>();
	 	 	 	 this.IPAddress = ko.observable<any>();
	 	 	 	 this.Enviorment = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserName = ko.observable(LoginDTO.UserName);
	 	 	 	 this.Password = ko.observable(LoginDTO.Password);
	 	 	 	 this.RememberMe = ko.observable(LoginDTO.RememberMe);
	 	 	 	 this.IPAddress = ko.observable(LoginDTO.IPAddress);
	 	 	 	 this.Enviorment = ko.observable(LoginDTO.Enviorment);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ILoginDTO{
	 	 	  return {
	 	 	 	UserName: this.UserName(),
	 	 	 	Password: this.Password(),
	 	 	 	RememberMe: this.RememberMe(),
	 	 	 	IPAddress: this.IPAddress(),
	 	 	 	Enviorment: this.Enviorment(),
	 	 	  };
	 	  }



	 }
	 export class MenuItemViewModel extends ViewModel<Dns.Interfaces.IMenuItemDTO>{
	 	 public text: KnockoutObservable<string>;
	 	 public url: KnockoutObservable<string>;
	 	 public encoded: KnockoutObservable<boolean>;
	 	 public content: KnockoutObservable<string>;
	 	 public items: KnockoutObservableArray<MenuItemViewModel>;
	 	 constructor(MenuItemDTO?: Dns.Interfaces.IMenuItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (MenuItemDTO== null) {
	 	 	 	 this.text = ko.observable<any>();
	 	 	 	 this.url = ko.observable<any>();
	 	 	 	 this.encoded = ko.observable<any>();
	 	 	 	 this.content = ko.observable<any>();
	 	 	 	 this.items = ko.observableArray<MenuItemViewModel>();
	 	 	  }else{
	 	 	 	 this.text = ko.observable(MenuItemDTO.text);
	 	 	 	 this.url = ko.observable(MenuItemDTO.url);
	 	 	 	 this.encoded = ko.observable(MenuItemDTO.encoded);
	 	 	 	 this.content = ko.observable(MenuItemDTO.content);
	 	 	 	 this.items = ko.observableArray<MenuItemViewModel>(MenuItemDTO.items == null ? null : MenuItemDTO.items.map((item) => {return new MenuItemViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IMenuItemDTO{
	 	 	  return {
	 	 	 	text: this.text(),
	 	 	 	url: this.url(),
	 	 	 	encoded: this.encoded(),
	 	 	 	content: this.content(),
	 	 	 	items: this.items == null ? null : this.items().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class ObserverViewModel extends ViewModel<Dns.Interfaces.IObserverDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public DisplayName: KnockoutObservable<string>;
	 	 public DisplayNameWithType: KnockoutObservable<string>;
	 	 public ObserverType: KnockoutObservable<Dns.Enums.ObserverTypes>;
	 	 constructor(ObserverDTO?: Dns.Interfaces.IObserverDTO)
	 	  {
	 	 	  super();
	 	 	 if (ObserverDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.DisplayName = ko.observable<any>();
	 	 	 	 this.DisplayNameWithType = ko.observable<any>();
	 	 	 	 this.ObserverType = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(ObserverDTO.ID);
	 	 	 	 this.DisplayName = ko.observable(ObserverDTO.DisplayName);
	 	 	 	 this.DisplayNameWithType = ko.observable(ObserverDTO.DisplayNameWithType);
	 	 	 	 this.ObserverType = ko.observable(ObserverDTO.ObserverType);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IObserverDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	DisplayName: this.DisplayName(),
	 	 	 	DisplayNameWithType: this.DisplayNameWithType(),
	 	 	 	ObserverType: this.ObserverType(),
	 	 	  };
	 	  }



	 }
	 export class ObserverEventViewModel extends ViewModel<Dns.Interfaces.IObserverEventDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 constructor(ObserverEventDTO?: Dns.Interfaces.IObserverEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (ObserverEventDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(ObserverEventDTO.ID);
	 	 	 	 this.Name = ko.observable(ObserverEventDTO.Name);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IObserverEventDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Name: this.Name(),
	 	 	  };
	 	  }



	 }
	 export class RestorePasswordViewModel extends ViewModel<Dns.Interfaces.IRestorePasswordDTO>{
	 	 public PasswordRestoreToken: KnockoutObservable<any>;
	 	 public Password: KnockoutObservable<string>;
	 	 constructor(RestorePasswordDTO?: Dns.Interfaces.IRestorePasswordDTO)
	 	  {
	 	 	  super();
	 	 	 if (RestorePasswordDTO== null) {
	 	 	 	 this.PasswordRestoreToken = ko.observable<any>();
	 	 	 	 this.Password = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.PasswordRestoreToken = ko.observable(RestorePasswordDTO.PasswordRestoreToken);
	 	 	 	 this.Password = ko.observable(RestorePasswordDTO.Password);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRestorePasswordDTO{
	 	 	  return {
	 	 	 	PasswordRestoreToken: this.PasswordRestoreToken(),
	 	 	 	Password: this.Password(),
	 	 	  };
	 	  }



	 }
	 export class TreeItemViewModel extends ViewModel<Dns.Interfaces.ITreeItemDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Path: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<number>;
	 	 public SubItems: KnockoutObservableArray<TreeItemViewModel>;
	 	 public HasChildren: KnockoutObservable<boolean>;
	 	 constructor(TreeItemDTO?: Dns.Interfaces.ITreeItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (TreeItemDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Path = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.SubItems = ko.observableArray<TreeItemViewModel>();
	 	 	 	 this.HasChildren = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(TreeItemDTO.ID);
	 	 	 	 this.Name = ko.observable(TreeItemDTO.Name);
	 	 	 	 this.Path = ko.observable(TreeItemDTO.Path);
	 	 	 	 this.Type = ko.observable(TreeItemDTO.Type);
	 	 	 	 this.SubItems = ko.observableArray<TreeItemViewModel>(TreeItemDTO.SubItems == null ? null : TreeItemDTO.SubItems.map((item) => {return new TreeItemViewModel(item);}));
	 	 	 	 this.HasChildren = ko.observable(TreeItemDTO.HasChildren);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ITreeItemDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	Name: this.Name(),
	 	 	 	Path: this.Path(),
	 	 	 	Type: this.Type(),
	 	 	 	SubItems: this.SubItems == null ? null : this.SubItems().map((item) => {return item.toData();}),
	 	 	 	HasChildren: this.HasChildren(),
	 	 	  };
	 	  }



	 }
	 export class UpdateUserPasswordViewModel extends ViewModel<Dns.Interfaces.IUpdateUserPasswordDTO>{
	 	 public UserID: KnockoutObservable<any>;
	 	 public Password: KnockoutObservable<string>;
	 	 constructor(UpdateUserPasswordDTO?: Dns.Interfaces.IUpdateUserPasswordDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateUserPasswordDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Password = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(UpdateUserPasswordDTO.UserID);
	 	 	 	 this.Password = ko.observable(UpdateUserPasswordDTO.Password);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUpdateUserPasswordDTO{
	 	 	  return {
	 	 	 	UserID: this.UserID(),
	 	 	 	Password: this.Password(),
	 	 	  };
	 	  }



	 }
	 export class UserAuthenticationViewModel extends ViewModel<Dns.Interfaces.IUserAuthenticationDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public Success: KnockoutObservable<boolean>;
	 	 public Description: KnockoutObservable<string>;
	 	 public IPAddress: KnockoutObservable<string>;
	 	 public Enviorment: KnockoutObservable<string>;
	 	 public DateTime: KnockoutObservable<Date>;
	 	 constructor(UserAuthenticationDTO?: Dns.Interfaces.IUserAuthenticationDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserAuthenticationDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Success = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.IPAddress = ko.observable<any>();
	 	 	 	 this.Enviorment = ko.observable<any>();
	 	 	 	 this.DateTime = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(UserAuthenticationDTO.ID);
	 	 	 	 this.UserID = ko.observable(UserAuthenticationDTO.UserID);
	 	 	 	 this.Success = ko.observable(UserAuthenticationDTO.Success);
	 	 	 	 this.Description = ko.observable(UserAuthenticationDTO.Description);
	 	 	 	 this.IPAddress = ko.observable(UserAuthenticationDTO.IPAddress);
	 	 	 	 this.Enviorment = ko.observable(UserAuthenticationDTO.Enviorment);
	 	 	 	 this.DateTime = ko.observable(UserAuthenticationDTO.DateTime);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUserAuthenticationDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	UserID: this.UserID(),
	 	 	 	Success: this.Success(),
	 	 	 	Description: this.Description(),
	 	 	 	IPAddress: this.IPAddress(),
	 	 	 	Enviorment: this.Enviorment(),
	 	 	 	DateTime: this.DateTime(),
	 	 	  };
	 	  }



	 }
	 export class UserRegistrationViewModel extends ViewModel<Dns.Interfaces.IUserRegistrationDTO>{
	 	 public UserName: KnockoutObservable<string>;
	 	 public Password: KnockoutObservable<string>;
	 	 public Title: KnockoutObservable<string>;
	 	 public FirstName: KnockoutObservable<string>;
	 	 public LastName: KnockoutObservable<string>;
	 	 public MiddleName: KnockoutObservable<string>;
	 	 public Phone: KnockoutObservable<string>;
	 	 public Fax: KnockoutObservable<string>;
	 	 public Email: KnockoutObservable<string>;
	 	 public Active: KnockoutObservable<boolean>;
	 	 public SignedUpOn: KnockoutObservable<Date>;
	 	 public OrganizationRequested: KnockoutObservable<string>;
	 	 public RoleRequested: KnockoutObservable<string>;
	 	 constructor(UserRegistrationDTO?: Dns.Interfaces.IUserRegistrationDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserRegistrationDTO== null) {
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Password = ko.observable<any>();
	 	 	 	 this.Title = ko.observable<any>();
	 	 	 	 this.FirstName = ko.observable<any>();
	 	 	 	 this.LastName = ko.observable<any>();
	 	 	 	 this.MiddleName = ko.observable<any>();
	 	 	 	 this.Phone = ko.observable<any>();
	 	 	 	 this.Fax = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	 	 this.Active = ko.observable<any>();
	 	 	 	 this.SignedUpOn = ko.observable<any>();
	 	 	 	 this.OrganizationRequested = ko.observable<any>();
	 	 	 	 this.RoleRequested = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IUserRegistrationDTO{
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
	 export class DataMartRegistrationResultViewModel extends ViewModel<Dns.Interfaces.IDataMartRegistrationResultDTO>{
	 	 public DataMarts: KnockoutObservableArray<DataMartViewModel>;
	 	 public DataMartModels: KnockoutObservableArray<DataMartInstalledModelViewModel>;
	 	 public Users: KnockoutObservableArray<UserWithSecurityDetailsViewModel>;
	 	 public ResearchOrganization: OrganizationViewModel;
	 	 public ProviderOrganization: OrganizationViewModel;
	 	 constructor(DataMartRegistrationResultDTO?: Dns.Interfaces.IDataMartRegistrationResultDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartRegistrationResultDTO== null) {
	 	 	 	 this.DataMarts = ko.observableArray<DataMartViewModel>();
	 	 	 	 this.DataMartModels = ko.observableArray<DataMartInstalledModelViewModel>();
	 	 	 	 this.Users = ko.observableArray<UserWithSecurityDetailsViewModel>();
	 	 	 	 this.ResearchOrganization = new OrganizationViewModel();
	 	 	 	 this.ProviderOrganization = new OrganizationViewModel();
	 	 	  }else{
	 	 	 	 this.DataMarts = ko.observableArray<DataMartViewModel>(DataMartRegistrationResultDTO.DataMarts == null ? null : DataMartRegistrationResultDTO.DataMarts.map((item) => {return new DataMartViewModel(item);}));
	 	 	 	 this.DataMartModels = ko.observableArray<DataMartInstalledModelViewModel>(DataMartRegistrationResultDTO.DataMartModels == null ? null : DataMartRegistrationResultDTO.DataMartModels.map((item) => {return new DataMartInstalledModelViewModel(item);}));
	 	 	 	 this.Users = ko.observableArray<UserWithSecurityDetailsViewModel>(DataMartRegistrationResultDTO.Users == null ? null : DataMartRegistrationResultDTO.Users.map((item) => {return new UserWithSecurityDetailsViewModel(item);}));
	 	 	 	 this.ResearchOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ResearchOrganization);
	 	 	 	 this.ProviderOrganization = new OrganizationViewModel(DataMartRegistrationResultDTO.ProviderOrganization);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataMartRegistrationResultDTO{
	 	 	  return {
	 	 	 	DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => {return item.toData();}),
	 	 	 	DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map((item) => {return item.toData();}),
	 	 	 	Users: this.Users == null ? null : this.Users().map((item) => {return item.toData();}),
	 	 	 	ResearchOrganization: this.ResearchOrganization.toData(),
	 	 	 	ProviderOrganization: this.ProviderOrganization.toData(),
	 	 	  };
	 	  }



	 }
	 export class GetChangeRequestViewModel extends ViewModel<Dns.Interfaces.IGetChangeRequestDTO>{
	 	 public LastChecked: KnockoutObservable<Date>;
	 	 public ProviderIDs: KnockoutObservableArray<any>;
	 	 constructor(GetChangeRequestDTO?: Dns.Interfaces.IGetChangeRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (GetChangeRequestDTO== null) {
	 	 	 	 this.LastChecked = ko.observable<any>();
	 	 	 	 this.ProviderIDs = ko.observableArray<any>();
	 	 	  }else{
	 	 	 	 this.LastChecked = ko.observable(GetChangeRequestDTO.LastChecked);
	 	 	 	 this.ProviderIDs = ko.observableArray<any>(GetChangeRequestDTO.ProviderIDs == null ? null : GetChangeRequestDTO.ProviderIDs.map((item) => {return item;}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IGetChangeRequestDTO{
	 	 	  return {
	 	 	 	LastChecked: this.LastChecked(),
	 	 	 	ProviderIDs: this.ProviderIDs(),
	 	 	  };
	 	  }



	 }
	 export class RegisterDataMartViewModel extends ViewModel<Dns.Interfaces.IRegisterDataMartDTO>{
	 	 public Password: KnockoutObservable<string>;
	 	 public Token: KnockoutObservable<string>;
	 	 constructor(RegisterDataMartDTO?: Dns.Interfaces.IRegisterDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (RegisterDataMartDTO== null) {
	 	 	 	 this.Password = ko.observable<any>();
	 	 	 	 this.Token = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Password = ko.observable(RegisterDataMartDTO.Password);
	 	 	 	 this.Token = ko.observable(RegisterDataMartDTO.Token);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRegisterDataMartDTO{
	 	 	  return {
	 	 	 	Password: this.Password(),
	 	 	 	Token: this.Token(),
	 	 	  };
	 	  }



	 }
	 export class RequestDocumentViewModel extends ViewModel<Dns.Interfaces.IRequestDocumentDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public FileName: KnockoutObservable<string>;
	 	 public MimeType: KnockoutObservable<string>;
	 	 public Viewable: KnockoutObservable<boolean>;
	 	 public ItemID: KnockoutObservable<any>;
	 	 constructor(RequestDocumentDTO?: Dns.Interfaces.IRequestDocumentDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestDocumentDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.FileName = ko.observable<any>();
	 	 	 	 this.MimeType = ko.observable<any>();
	 	 	 	 this.Viewable = ko.observable<any>();
	 	 	 	 this.ItemID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(RequestDocumentDTO.ID);
	 	 	 	 this.Name = ko.observable(RequestDocumentDTO.Name);
	 	 	 	 this.FileName = ko.observable(RequestDocumentDTO.FileName);
	 	 	 	 this.MimeType = ko.observable(RequestDocumentDTO.MimeType);
	 	 	 	 this.Viewable = ko.observable(RequestDocumentDTO.Viewable);
	 	 	 	 this.ItemID = ko.observable(RequestDocumentDTO.ItemID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestDocumentDTO{
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
	 export class UpdateResponseStatusRequestViewModel extends ViewModel<Dns.Interfaces.IUpdateResponseStatusRequestDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public ResponseID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public StatusID: KnockoutObservable<Dns.Enums.RoutingStatus>;
	 	 public Message: KnockoutObservable<string>;
	 	 public RejectReason: KnockoutObservable<string>;
	 	 public HoldReason: KnockoutObservable<string>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestTypeName: KnockoutObservable<string>;
	 	 constructor(UpdateResponseStatusRequestDTO?: Dns.Interfaces.IUpdateResponseStatusRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (UpdateResponseStatusRequestDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.ResponseID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.StatusID = ko.observable<any>();
	 	 	 	 this.Message = ko.observable<any>();
	 	 	 	 this.RejectReason = ko.observable<any>();
	 	 	 	 this.HoldReason = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestTypeName = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IUpdateResponseStatusRequestDTO{
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
	 export class WbdChangeSetViewModel extends ViewModel<Dns.Interfaces.IWbdChangeSetDTO>{
	 	 public Requests: KnockoutObservableArray<RequestViewModel>;
	 	 public Projects: KnockoutObservableArray<ProjectViewModel>;
	 	 public DataMarts: KnockoutObservableArray<DataMartViewModel>;
	 	 public DataMartModels: KnockoutObservableArray<DataMartInstalledModelViewModel>;
	 	 public RequestDataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
	 	 public ProjectDataMarts: KnockoutObservableArray<ProjectDataMartViewModel>;
	 	 public Organizations: KnockoutObservableArray<OrganizationViewModel>;
	 	 public Documents: KnockoutObservableArray<RequestDocumentViewModel>;
	 	 public Users: KnockoutObservableArray<UserWithSecurityDetailsViewModel>;
	 	 public Responses: KnockoutObservableArray<ResponseDetailViewModel>;
	 	 public SecurityGroups: KnockoutObservableArray<SecurityGroupWithUsersViewModel>;
	 	 public RequestResponseSecurityACLs: KnockoutObservableArray<SecurityTupleViewModel>;
	 	 public DataMartSecurityACLs: KnockoutObservableArray<SecurityTupleViewModel>;
	 	 public ManageWbdACLs: KnockoutObservableArray<SecurityTupleViewModel>;
	 	 constructor(WbdChangeSetDTO?: Dns.Interfaces.IWbdChangeSetDTO)
	 	  {
	 	 	  super();
	 	 	 if (WbdChangeSetDTO== null) {
	 	 	 	 this.Requests = ko.observableArray<RequestViewModel>();
	 	 	 	 this.Projects = ko.observableArray<ProjectViewModel>();
	 	 	 	 this.DataMarts = ko.observableArray<DataMartViewModel>();
	 	 	 	 this.DataMartModels = ko.observableArray<DataMartInstalledModelViewModel>();
	 	 	 	 this.RequestDataMarts = ko.observableArray<RequestDataMartViewModel>();
	 	 	 	 this.ProjectDataMarts = ko.observableArray<ProjectDataMartViewModel>();
	 	 	 	 this.Organizations = ko.observableArray<OrganizationViewModel>();
	 	 	 	 this.Documents = ko.observableArray<RequestDocumentViewModel>();
	 	 	 	 this.Users = ko.observableArray<UserWithSecurityDetailsViewModel>();
	 	 	 	 this.Responses = ko.observableArray<ResponseDetailViewModel>();
	 	 	 	 this.SecurityGroups = ko.observableArray<SecurityGroupWithUsersViewModel>();
	 	 	 	 this.RequestResponseSecurityACLs = ko.observableArray<SecurityTupleViewModel>();
	 	 	 	 this.DataMartSecurityACLs = ko.observableArray<SecurityTupleViewModel>();
	 	 	 	 this.ManageWbdACLs = ko.observableArray<SecurityTupleViewModel>();
	 	 	  }else{
	 	 	 	 this.Requests = ko.observableArray<RequestViewModel>(WbdChangeSetDTO.Requests == null ? null : WbdChangeSetDTO.Requests.map((item) => {return new RequestViewModel(item);}));
	 	 	 	 this.Projects = ko.observableArray<ProjectViewModel>(WbdChangeSetDTO.Projects == null ? null : WbdChangeSetDTO.Projects.map((item) => {return new ProjectViewModel(item);}));
	 	 	 	 this.DataMarts = ko.observableArray<DataMartViewModel>(WbdChangeSetDTO.DataMarts == null ? null : WbdChangeSetDTO.DataMarts.map((item) => {return new DataMartViewModel(item);}));
	 	 	 	 this.DataMartModels = ko.observableArray<DataMartInstalledModelViewModel>(WbdChangeSetDTO.DataMartModels == null ? null : WbdChangeSetDTO.DataMartModels.map((item) => {return new DataMartInstalledModelViewModel(item);}));
	 	 	 	 this.RequestDataMarts = ko.observableArray<RequestDataMartViewModel>(WbdChangeSetDTO.RequestDataMarts == null ? null : WbdChangeSetDTO.RequestDataMarts.map((item) => {return new RequestDataMartViewModel(item);}));
	 	 	 	 this.ProjectDataMarts = ko.observableArray<ProjectDataMartViewModel>(WbdChangeSetDTO.ProjectDataMarts == null ? null : WbdChangeSetDTO.ProjectDataMarts.map((item) => {return new ProjectDataMartViewModel(item);}));
	 	 	 	 this.Organizations = ko.observableArray<OrganizationViewModel>(WbdChangeSetDTO.Organizations == null ? null : WbdChangeSetDTO.Organizations.map((item) => {return new OrganizationViewModel(item);}));
	 	 	 	 this.Documents = ko.observableArray<RequestDocumentViewModel>(WbdChangeSetDTO.Documents == null ? null : WbdChangeSetDTO.Documents.map((item) => {return new RequestDocumentViewModel(item);}));
	 	 	 	 this.Users = ko.observableArray<UserWithSecurityDetailsViewModel>(WbdChangeSetDTO.Users == null ? null : WbdChangeSetDTO.Users.map((item) => {return new UserWithSecurityDetailsViewModel(item);}));
	 	 	 	 this.Responses = ko.observableArray<ResponseDetailViewModel>(WbdChangeSetDTO.Responses == null ? null : WbdChangeSetDTO.Responses.map((item) => {return new ResponseDetailViewModel(item);}));
	 	 	 	 this.SecurityGroups = ko.observableArray<SecurityGroupWithUsersViewModel>(WbdChangeSetDTO.SecurityGroups == null ? null : WbdChangeSetDTO.SecurityGroups.map((item) => {return new SecurityGroupWithUsersViewModel(item);}));
	 	 	 	 this.RequestResponseSecurityACLs = ko.observableArray<SecurityTupleViewModel>(WbdChangeSetDTO.RequestResponseSecurityACLs == null ? null : WbdChangeSetDTO.RequestResponseSecurityACLs.map((item) => {return new SecurityTupleViewModel(item);}));
	 	 	 	 this.DataMartSecurityACLs = ko.observableArray<SecurityTupleViewModel>(WbdChangeSetDTO.DataMartSecurityACLs == null ? null : WbdChangeSetDTO.DataMartSecurityACLs.map((item) => {return new SecurityTupleViewModel(item);}));
	 	 	 	 this.ManageWbdACLs = ko.observableArray<SecurityTupleViewModel>(WbdChangeSetDTO.ManageWbdACLs == null ? null : WbdChangeSetDTO.ManageWbdACLs.map((item) => {return new SecurityTupleViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IWbdChangeSetDTO{
	 	 	  return {
	 	 	 	Requests: this.Requests == null ? null : this.Requests().map((item) => {return item.toData();}),
	 	 	 	Projects: this.Projects == null ? null : this.Projects().map((item) => {return item.toData();}),
	 	 	 	DataMarts: this.DataMarts == null ? null : this.DataMarts().map((item) => {return item.toData();}),
	 	 	 	DataMartModels: this.DataMartModels == null ? null : this.DataMartModels().map((item) => {return item.toData();}),
	 	 	 	RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map((item) => {return item.toData();}),
	 	 	 	ProjectDataMarts: this.ProjectDataMarts == null ? null : this.ProjectDataMarts().map((item) => {return item.toData();}),
	 	 	 	Organizations: this.Organizations == null ? null : this.Organizations().map((item) => {return item.toData();}),
	 	 	 	Documents: this.Documents == null ? null : this.Documents().map((item) => {return item.toData();}),
	 	 	 	Users: this.Users == null ? null : this.Users().map((item) => {return item.toData();}),
	 	 	 	Responses: this.Responses == null ? null : this.Responses().map((item) => {return item.toData();}),
	 	 	 	SecurityGroups: this.SecurityGroups == null ? null : this.SecurityGroups().map((item) => {return item.toData();}),
	 	 	 	RequestResponseSecurityACLs: this.RequestResponseSecurityACLs == null ? null : this.RequestResponseSecurityACLs().map((item) => {return item.toData();}),
	 	 	 	DataMartSecurityACLs: this.DataMartSecurityACLs == null ? null : this.DataMartSecurityACLs().map((item) => {return item.toData();}),
	 	 	 	ManageWbdACLs: this.ManageWbdACLs == null ? null : this.ManageWbdACLs().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class CommonResponseDetailViewModel extends ViewModel<Dns.Interfaces.ICommonResponseDetailDTO>{
	 	 public RequestDataMarts: KnockoutObservableArray<RequestDataMartViewModel>;
	 	 public Responses: KnockoutObservableArray<ResponseViewModel>;
	 	 public Documents: KnockoutObservableArray<ExtendedDocumentViewModel>;
	 	 public CanViewPendingApprovalResponses: KnockoutObservable<boolean>;
	 	 public ExportForFileDistribution: KnockoutObservable<boolean>;
	 	 constructor(CommonResponseDetailDTO?: Dns.Interfaces.ICommonResponseDetailDTO)
	 	  {
	 	 	  super();
	 	 	 if (CommonResponseDetailDTO== null) {
	 	 	 	 this.RequestDataMarts = ko.observableArray<RequestDataMartViewModel>();
	 	 	 	 this.Responses = ko.observableArray<ResponseViewModel>();
	 	 	 	 this.Documents = ko.observableArray<ExtendedDocumentViewModel>();
	 	 	 	 this.CanViewPendingApprovalResponses = ko.observable<any>();
	 	 	 	 this.ExportForFileDistribution = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestDataMarts = ko.observableArray<RequestDataMartViewModel>(CommonResponseDetailDTO.RequestDataMarts == null ? null : CommonResponseDetailDTO.RequestDataMarts.map((item) => {return new RequestDataMartViewModel(item);}));
	 	 	 	 this.Responses = ko.observableArray<ResponseViewModel>(CommonResponseDetailDTO.Responses == null ? null : CommonResponseDetailDTO.Responses.map((item) => {return new ResponseViewModel(item);}));
	 	 	 	 this.Documents = ko.observableArray<ExtendedDocumentViewModel>(CommonResponseDetailDTO.Documents == null ? null : CommonResponseDetailDTO.Documents.map((item) => {return new ExtendedDocumentViewModel(item);}));
	 	 	 	 this.CanViewPendingApprovalResponses = ko.observable(CommonResponseDetailDTO.CanViewPendingApprovalResponses);
	 	 	 	 this.ExportForFileDistribution = ko.observable(CommonResponseDetailDTO.ExportForFileDistribution);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ICommonResponseDetailDTO{
	 	 	  return {
	 	 	 	RequestDataMarts: this.RequestDataMarts == null ? null : this.RequestDataMarts().map((item) => {return item.toData();}),
	 	 	 	Responses: this.Responses == null ? null : this.Responses().map((item) => {return item.toData();}),
	 	 	 	Documents: this.Documents == null ? null : this.Documents().map((item) => {return item.toData();}),
	 	 	 	CanViewPendingApprovalResponses: this.CanViewPendingApprovalResponses(),
	 	 	 	ExportForFileDistribution: this.ExportForFileDistribution(),
	 	 	  };
	 	  }



	 }
	 export class PrepareSpecificationViewModel extends ViewModel<Dns.Interfaces.IPrepareSpecificationDTO>{
	 	 constructor(PrepareSpecificationDTO?: Dns.Interfaces.IPrepareSpecificationDTO)
	 	  {
	 	 	  super();
	 	 	 if (PrepareSpecificationDTO== null) {
	 	 	  }else{
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IPrepareSpecificationDTO{
	 	 	  return {
	 	 	  };
	 	  }



	 }
	 export class RequestFormViewModel extends ViewModel<Dns.Interfaces.IRequestFormDTO>{
	 	 public RequestDueDate: KnockoutObservable<Date>;
	 	 public ContactInfo: KnockoutObservable<string>;
	 	 public RequestingTeam: KnockoutObservable<string>;
	 	 public FDAReview: KnockoutObservable<string>;
	 	 public FDADivisionNA: KnockoutObservable<boolean>;
	 	 public FDADivisionDAAAP: KnockoutObservable<boolean>;
	 	 public FDADivisionDBRUP: KnockoutObservable<boolean>;
	 	 public FDADivisionDCARP: KnockoutObservable<boolean>;
	 	 public FDADivisionDDDP: KnockoutObservable<boolean>;
	 	 public FDADivisionDGIEP: KnockoutObservable<boolean>;
	 	 public FDADivisionDMIP: KnockoutObservable<boolean>;
	 	 public FDADivisionDMEP: KnockoutObservable<boolean>;
	 	 public FDADivisionDNP: KnockoutObservable<boolean>;
	 	 public FDADivisionDDP: KnockoutObservable<boolean>;
	 	 public FDADivisionDPARP: KnockoutObservable<boolean>;
	 	 public FDADivisionOther: KnockoutObservable<boolean>;
	 	 public QueryLevel: KnockoutObservable<string>;
	 	 public AdjustmentMethod: KnockoutObservable<string>;
	 	 public CohortID: KnockoutObservable<string>;
	 	 public StudyObjectives: KnockoutObservable<string>;
	 	 public RequestStartDate: KnockoutObservable<Date>;
	 	 public RequestEndDate: KnockoutObservable<Date>;
	 	 public AgeGroups: KnockoutObservable<string>;
	 	 public CoverageTypes: KnockoutObservable<string>;
	 	 public EnrollmentGap: KnockoutObservable<string>;
	 	 public EnrollmentExposure: KnockoutObservable<string>;
	 	 public DefineExposures: KnockoutObservable<string>;
	 	 public WashoutPeirod: KnockoutObservable<string>;
	 	 public OtherExposures: KnockoutObservable<string>;
	 	 public OneOrManyExposures: KnockoutObservable<string>;
	 	 public AdditionalInclusion: KnockoutObservable<string>;
	 	 public AdditionalInclusionEvaluation: KnockoutObservable<string>;
	 	 public AdditionalExclusion: KnockoutObservable<string>;
	 	 public AdditionalExclusionEvaluation: KnockoutObservable<string>;
	 	 public VaryWashoutPeirod: KnockoutObservable<string>;
	 	 public VaryExposures: KnockoutObservable<string>;
	 	 public DefineExposures1: KnockoutObservable<string>;
	 	 public DefineExposures2: KnockoutObservable<string>;
	 	 public DefineExposures3: KnockoutObservable<string>;
	 	 public DefineExposures4: KnockoutObservable<string>;
	 	 public DefineExposures5: KnockoutObservable<string>;
	 	 public DefineExposures6: KnockoutObservable<string>;
	 	 public DefineExposures7: KnockoutObservable<string>;
	 	 public DefineExposures8: KnockoutObservable<string>;
	 	 public DefineExposures9: KnockoutObservable<string>;
	 	 public DefineExposures10: KnockoutObservable<string>;
	 	 public DefineExposures11: KnockoutObservable<string>;
	 	 public DefineExposures12: KnockoutObservable<string>;
	 	 public WashoutPeriod1: KnockoutObservable<number>;
	 	 public WashoutPeriod2: KnockoutObservable<number>;
	 	 public WashoutPeriod3: KnockoutObservable<number>;
	 	 public WashoutPeriod4: KnockoutObservable<number>;
	 	 public WashoutPeriod5: KnockoutObservable<number>;
	 	 public WashoutPeriod6: KnockoutObservable<number>;
	 	 public WashoutPeriod7: KnockoutObservable<number>;
	 	 public WashoutPeriod8: KnockoutObservable<number>;
	 	 public WashoutPeriod9: KnockoutObservable<number>;
	 	 public WashoutPeriod10: KnockoutObservable<number>;
	 	 public WashoutPeriod11: KnockoutObservable<number>;
	 	 public WashoutPeriod12: KnockoutObservable<number>;
	 	 public IncidenceRefinement1: KnockoutObservable<string>;
	 	 public IncidenceRefinement2: KnockoutObservable<string>;
	 	 public IncidenceRefinement3: KnockoutObservable<string>;
	 	 public IncidenceRefinement4: KnockoutObservable<string>;
	 	 public IncidenceRefinement5: KnockoutObservable<string>;
	 	 public IncidenceRefinement6: KnockoutObservable<string>;
	 	 public IncidenceRefinement7: KnockoutObservable<string>;
	 	 public IncidenceRefinement8: KnockoutObservable<string>;
	 	 public IncidenceRefinement9: KnockoutObservable<string>;
	 	 public IncidenceRefinement10: KnockoutObservable<string>;
	 	 public IncidenceRefinement11: KnockoutObservable<string>;
	 	 public IncidenceRefinement12: KnockoutObservable<string>;
	 	 public SpecifyExposedTimeAssessment1: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment2: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment3: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment4: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment5: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment6: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment7: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment8: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment9: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment10: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment11: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public SpecifyExposedTimeAssessment12: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public EpisodeAllowableGap1: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap2: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap3: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap4: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap5: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap6: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap7: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap8: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap9: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap10: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap11: KnockoutObservable<number>;
	 	 public EpisodeAllowableGap12: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod1: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod2: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod3: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod4: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod5: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod6: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod7: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod8: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod9: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod10: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod11: KnockoutObservable<number>;
	 	 public EpisodeExtensionPeriod12: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration1: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration2: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration3: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration4: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration5: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration6: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration7: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration8: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration9: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration10: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration11: KnockoutObservable<number>;
	 	 public MinimumEpisodeDuration12: KnockoutObservable<number>;
	 	 public MinimumDaysSupply1: KnockoutObservable<number>;
	 	 public MinimumDaysSupply2: KnockoutObservable<number>;
	 	 public MinimumDaysSupply3: KnockoutObservable<number>;
	 	 public MinimumDaysSupply4: KnockoutObservable<number>;
	 	 public MinimumDaysSupply5: KnockoutObservable<number>;
	 	 public MinimumDaysSupply6: KnockoutObservable<number>;
	 	 public MinimumDaysSupply7: KnockoutObservable<number>;
	 	 public MinimumDaysSupply8: KnockoutObservable<number>;
	 	 public MinimumDaysSupply9: KnockoutObservable<number>;
	 	 public MinimumDaysSupply10: KnockoutObservable<number>;
	 	 public MinimumDaysSupply11: KnockoutObservable<number>;
	 	 public MinimumDaysSupply12: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration1: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration2: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration3: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration4: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration5: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration6: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration7: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration8: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration9: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration10: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration11: KnockoutObservable<number>;
	 	 public SpecifyFollowUpDuration12: KnockoutObservable<number>;
	 	 public AllowOnOrMultipleExposureEpisodes1: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes2: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes3: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes4: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes5: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes6: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes7: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes8: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes9: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes10: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes11: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public AllowOnOrMultipleExposureEpisodes12: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public TruncateExposedtime1: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime2: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime3: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime4: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime5: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime6: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime7: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime8: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime9: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime10: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime11: KnockoutObservable<boolean>;
	 	 public TruncateExposedtime12: KnockoutObservable<boolean>;
	 	 public TruncateExposedTimeSpecified1: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified2: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified3: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified4: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified5: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified6: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified7: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified8: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified9: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified10: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified11: KnockoutObservable<string>;
	 	 public TruncateExposedTimeSpecified12: KnockoutObservable<string>;
	 	 public SpecifyBlackoutPeriod1: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod2: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod3: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod4: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod5: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod6: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod7: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod8: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod9: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod10: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod11: KnockoutObservable<number>;
	 	 public SpecifyBlackoutPeriod12: KnockoutObservable<number>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup11: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup12: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup13: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup14: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup15: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup16: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup11: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup12: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup13: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup14: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup15: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup16: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup21: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup22: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup23: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup24: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup25: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup26: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup21: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup22: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup23: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup24: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup25: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup26: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup31: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup32: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup33: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup34: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup35: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup36: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup31: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup32: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup33: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup34: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup35: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup36: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup41: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup42: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup43: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup44: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup45: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup46: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup41: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup42: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup43: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup44: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup45: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup46: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup51: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup52: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup53: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup54: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup55: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup56: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup51: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup52: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup53: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup54: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup55: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup56: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup61: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup62: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup63: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup64: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup65: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup66: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup61: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup62: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup63: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup64: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup65: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup66: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup71: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup72: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup73: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup74: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup75: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup76: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup71: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup72: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup73: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup74: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup75: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup76: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup81: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup82: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup83: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup84: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup85: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup86: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup81: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup82: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup83: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup84: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup85: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup86: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup91: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup92: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup93: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup94: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup95: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup96: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup91: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup92: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup93: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup94: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup95: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup96: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup101: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup102: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup103: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup104: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup105: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup106: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup101: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup102: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup103: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup104: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup105: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup106: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup111: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup112: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup113: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup114: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup115: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup116: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup111: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup112: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup113: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup114: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup115: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup116: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup121: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup122: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup123: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup124: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup125: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionInclusionCriteriaGroup126: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup121: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup122: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup123: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup124: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup125: KnockoutObservable<string>;
	 	 public SpecifyAdditionalInclusionEvaluationWindowGroup126: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup11: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup12: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup13: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup14: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup15: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup16: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup11: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup12: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup13: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup14: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup15: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup16: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup21: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup22: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup23: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup24: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup25: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup26: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup21: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup22: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup23: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup24: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup25: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup26: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup31: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup32: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup33: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup34: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup35: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup36: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup31: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup32: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup33: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup34: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup35: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup36: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup41: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup42: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup43: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup44: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup45: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup46: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup41: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup42: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup43: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup44: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup45: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup46: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup51: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup52: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup53: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup54: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup55: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup56: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup51: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup52: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup53: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup54: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup55: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup56: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup61: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup62: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup63: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup64: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup65: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup66: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup61: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup62: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup63: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup64: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup65: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup66: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup71: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup72: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup73: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup74: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup75: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup76: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup71: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup72: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup73: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup74: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup75: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup76: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup81: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup82: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup83: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup84: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup85: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup86: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup81: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup82: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup83: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup84: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup85: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup86: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup91: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup92: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup93: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup94: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup95: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup96: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup91: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup92: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup93: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup94: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup95: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup96: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup101: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup102: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup103: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup104: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup105: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup106: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup101: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup102: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup103: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup104: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup105: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup106: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup111: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup112: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup113: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup114: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup115: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup116: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup111: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup112: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup113: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup114: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup115: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup116: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup121: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup122: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup123: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup124: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup125: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionInclusionCriteriaGroup126: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup121: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup122: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup123: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup124: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup125: KnockoutObservable<string>;
	 	 public SpecifyAdditionalExclusionEvaluationWindowGroup126: KnockoutObservable<string>;
	 	 public LookBackPeriodGroup1: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup2: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup3: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup4: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup5: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup6: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup7: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup8: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup9: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup10: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup11: KnockoutObservable<number>;
	 	 public LookBackPeriodGroup12: KnockoutObservable<number>;
	 	 public IncludeIndexDate1: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate2: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate3: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate4: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate5: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate6: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate7: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate8: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate9: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate10: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate11: KnockoutObservable<boolean>;
	 	 public IncludeIndexDate12: KnockoutObservable<boolean>;
	 	 public StratificationCategories1: KnockoutObservable<string>;
	 	 public StratificationCategories2: KnockoutObservable<string>;
	 	 public StratificationCategories3: KnockoutObservable<string>;
	 	 public StratificationCategories4: KnockoutObservable<string>;
	 	 public StratificationCategories5: KnockoutObservable<string>;
	 	 public StratificationCategories6: KnockoutObservable<string>;
	 	 public StratificationCategories7: KnockoutObservable<string>;
	 	 public StratificationCategories8: KnockoutObservable<string>;
	 	 public StratificationCategories9: KnockoutObservable<string>;
	 	 public StratificationCategories10: KnockoutObservable<string>;
	 	 public StratificationCategories11: KnockoutObservable<string>;
	 	 public StratificationCategories12: KnockoutObservable<string>;
	 	 public TwelveSpecifyLoopBackPeriod1: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod2: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod3: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod4: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod5: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod6: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod7: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod8: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod9: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod10: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod11: KnockoutObservable<number>;
	 	 public TwelveSpecifyLoopBackPeriod12: KnockoutObservable<number>;
	 	 public TwelveIncludeIndexDate1: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate2: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate3: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate4: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate5: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate6: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate7: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate8: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate9: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate10: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate11: KnockoutObservable<boolean>;
	 	 public TwelveIncludeIndexDate12: KnockoutObservable<boolean>;
	 	 public CareSettingsToDefineMedicalVisits1: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits2: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits3: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits4: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits5: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits6: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits7: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits8: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits9: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits10: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits11: KnockoutObservable<string>;
	 	 public CareSettingsToDefineMedicalVisits12: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories1: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories2: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories3: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories4: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories5: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories6: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories7: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories8: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories9: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories10: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories11: KnockoutObservable<string>;
	 	 public TwelveStratificationCategories12: KnockoutObservable<string>;
	 	 public VaryLengthOfWashoutPeriod1: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod2: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod3: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod4: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod5: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod6: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod7: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod8: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod9: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod10: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod11: KnockoutObservable<number>;
	 	 public VaryLengthOfWashoutPeriod12: KnockoutObservable<number>;
	 	 public VaryUserExposedTime1: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime2: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime3: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime4: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime5: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime6: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime7: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime8: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime9: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime10: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime11: KnockoutObservable<boolean>;
	 	 public VaryUserExposedTime12: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration1: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration2: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration3: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration4: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration5: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration6: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration7: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration8: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration9: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration10: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration11: KnockoutObservable<boolean>;
	 	 public VaryUserFollowupPeriodDuration12: KnockoutObservable<boolean>;
	 	 public VaryBlackoutPeriodPeriod1: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod2: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod3: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod4: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod5: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod6: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod7: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod8: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod9: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod10: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod11: KnockoutObservable<number>;
	 	 public VaryBlackoutPeriodPeriod12: KnockoutObservable<number>;
	 	 public Level2or3DefineExposures1Exposure: KnockoutObservable<string>;
	 	 public Level2or3DefineExposures1Compare: KnockoutObservable<string>;
	 	 public Level2or3DefineExposures2Exposure: KnockoutObservable<string>;
	 	 public Level2or3DefineExposures2Compare: KnockoutObservable<string>;
	 	 public Level2or3DefineExposures3Exposure: KnockoutObservable<string>;
	 	 public Level2or3DefineExposures3Compare: KnockoutObservable<string>;
	 	 public Level2or3WashoutPeriod1Exposure: KnockoutObservable<number>;
	 	 public Level2or3WashoutPeriod1Compare: KnockoutObservable<number>;
	 	 public Level2or3WashoutPeriod2Exposure: KnockoutObservable<number>;
	 	 public Level2or3WashoutPeriod2Compare: KnockoutObservable<number>;
	 	 public Level2or3WashoutPeriod3Exposure: KnockoutObservable<number>;
	 	 public Level2or3WashoutPeriod3Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyExposedTimeAssessment1Exposure: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3SpecifyExposedTimeAssessment1Compare: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3SpecifyExposedTimeAssessment2Exposure: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3SpecifyExposedTimeAssessment2Compare: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3SpecifyExposedTimeAssessment3Exposure: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3SpecifyExposedTimeAssessment3Compare: KnockoutObservable<Dns.Enums.WorkflowMPSpecifyExposedTimeAssessments>;
	 	 public Level2or3EpisodeAllowableGap1Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeAllowableGap1Compare: KnockoutObservable<number>;
	 	 public Level2or3EpisodeAllowableGap2Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeAllowableGap2Compare: KnockoutObservable<number>;
	 	 public Level2or3EpisodeAllowableGap3Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeAllowableGap3Compare: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod1Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod1Compare: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod2Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod2Compare: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod3Exposure: KnockoutObservable<number>;
	 	 public Level2or3EpisodeExtensionPeriod3Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration1Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration1Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration2Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration2Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration3Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumEpisodeDuration3Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply1Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply1Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply2Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply2Compare: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply3Exposure: KnockoutObservable<number>;
	 	 public Level2or3MinimumDaysSupply3Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration1Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration1Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration2Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration2Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration3Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyFollowUpDuration3Compare: KnockoutObservable<number>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes1Exposure: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes1Compare: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes2Exposure: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes2Compare: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes3Exposure: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3AllowOnOrMultipleExposureEpisodes3Compare: KnockoutObservable<Dns.Enums.WorkflowMPAllowOnOrMultipleExposureEpisodes>;
	 	 public Level2or3TruncateExposedtime1Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedtime1Compare: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedtime2Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedtime2Compare: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedtime3Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedtime3Compare: KnockoutObservable<boolean>;
	 	 public Level2or3TruncateExposedTimeSpecified1Exposure: KnockoutObservable<string>;
	 	 public Level2or3TruncateExposedTimeSpecified1Compare: KnockoutObservable<string>;
	 	 public Level2or3TruncateExposedTimeSpecified2Exposure: KnockoutObservable<string>;
	 	 public Level2or3TruncateExposedTimeSpecified2Compare: KnockoutObservable<string>;
	 	 public Level2or3TruncateExposedTimeSpecified3Exposure: KnockoutObservable<string>;
	 	 public Level2or3TruncateExposedTimeSpecified3Compare: KnockoutObservable<string>;
	 	 public Level2or3SpecifyBlackoutPeriod1Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyBlackoutPeriod1Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyBlackoutPeriod2Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyBlackoutPeriod2Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyBlackoutPeriod3Exposure: KnockoutObservable<number>;
	 	 public Level2or3SpecifyBlackoutPeriod3Compare: KnockoutObservable<number>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62: KnockoutObservable<string>;
	 	 public Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63: KnockoutObservable<string>;
	 	 public Level2or3VaryLengthOfWashoutPeriod1Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryLengthOfWashoutPeriod1Compare: KnockoutObservable<number>;
	 	 public Level2or3VaryLengthOfWashoutPeriod2Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryLengthOfWashoutPeriod2Compare: KnockoutObservable<number>;
	 	 public Level2or3VaryLengthOfWashoutPeriod3Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryLengthOfWashoutPeriod3Compare: KnockoutObservable<number>;
	 	 public Level2or3VaryUserExposedTime1Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3VaryUserExposedTime1Compare: KnockoutObservable<boolean>;
	 	 public Level2or3VaryUserExposedTime2Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3VaryUserExposedTime2Compare: KnockoutObservable<boolean>;
	 	 public Level2or3VaryUserExposedTime3Exposure: KnockoutObservable<boolean>;
	 	 public Level2or3VaryUserExposedTime3Compare: KnockoutObservable<boolean>;
	 	 public Level2or3VaryBlackoutPeriodPeriod1Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryBlackoutPeriodPeriod1Compare: KnockoutObservable<number>;
	 	 public Level2or3VaryBlackoutPeriodPeriod2Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryBlackoutPeriodPeriod2Compare: KnockoutObservable<number>;
	 	 public Level2or3VaryBlackoutPeriodPeriod3Exposure: KnockoutObservable<number>;
	 	 public Level2or3VaryBlackoutPeriodPeriod3Compare: KnockoutObservable<number>;
	 	 public OutcomeList: KnockoutObservableArray<OutcomeItemViewModel>;
	 	 public AgeCovariate: KnockoutObservable<string>;
	 	 public SexCovariate: KnockoutObservable<string>;
	 	 public TimeCovariate: KnockoutObservable<string>;
	 	 public YearCovariate: KnockoutObservable<string>;
	 	 public ComorbidityCovariate: KnockoutObservable<string>;
	 	 public HealthCovariate: KnockoutObservable<string>;
	 	 public DrugCovariate: KnockoutObservable<string>;
	 	 public CovariateList: KnockoutObservableArray<CovariateItemViewModel>;
	 	 public hdPSAnalysis: KnockoutObservable<string>;
	 	 public InclusionCovariates: KnockoutObservable<number>;
	 	 public PoolCovariates: KnockoutObservable<number>;
	 	 public SelectionCovariates: KnockoutObservable<string>;
	 	 public ZeroCellCorrection: KnockoutObservable<string>;
	 	 public MatchingRatio: KnockoutObservable<string>;
	 	 public MatchingCalipers: KnockoutObservable<string>;
	 	 public VaryMatchingRatio: KnockoutObservable<string>;
	 	 public VaryMatchingCalipers: KnockoutObservable<string>;
	 	 constructor(RequestFormDTO?: Dns.Interfaces.IRequestFormDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestFormDTO== null) {
	 	 	 	 this.RequestDueDate = ko.observable<any>();
	 	 	 	 this.ContactInfo = ko.observable<any>();
	 	 	 	 this.RequestingTeam = ko.observable<any>();
	 	 	 	 this.FDAReview = ko.observable<any>();
	 	 	 	 this.FDADivisionNA = ko.observable<any>();
	 	 	 	 this.FDADivisionDAAAP = ko.observable<any>();
	 	 	 	 this.FDADivisionDBRUP = ko.observable<any>();
	 	 	 	 this.FDADivisionDCARP = ko.observable<any>();
	 	 	 	 this.FDADivisionDDDP = ko.observable<any>();
	 	 	 	 this.FDADivisionDGIEP = ko.observable<any>();
	 	 	 	 this.FDADivisionDMIP = ko.observable<any>();
	 	 	 	 this.FDADivisionDMEP = ko.observable<any>();
	 	 	 	 this.FDADivisionDNP = ko.observable<any>();
	 	 	 	 this.FDADivisionDDP = ko.observable<any>();
	 	 	 	 this.FDADivisionDPARP = ko.observable<any>();
	 	 	 	 this.FDADivisionOther = ko.observable<any>();
	 	 	 	 this.QueryLevel = ko.observable<any>();
	 	 	 	 this.AdjustmentMethod = ko.observable<any>();
	 	 	 	 this.CohortID = ko.observable<any>();
	 	 	 	 this.StudyObjectives = ko.observable<any>();
	 	 	 	 this.RequestStartDate = ko.observable<any>();
	 	 	 	 this.RequestEndDate = ko.observable<any>();
	 	 	 	 this.AgeGroups = ko.observable<any>();
	 	 	 	 this.CoverageTypes = ko.observable<any>();
	 	 	 	 this.EnrollmentGap = ko.observable<any>();
	 	 	 	 this.EnrollmentExposure = ko.observable<any>();
	 	 	 	 this.DefineExposures = ko.observable<any>();
	 	 	 	 this.WashoutPeirod = ko.observable<any>();
	 	 	 	 this.OtherExposures = ko.observable<any>();
	 	 	 	 this.OneOrManyExposures = ko.observable<any>();
	 	 	 	 this.AdditionalInclusion = ko.observable<any>();
	 	 	 	 this.AdditionalInclusionEvaluation = ko.observable<any>();
	 	 	 	 this.AdditionalExclusion = ko.observable<any>();
	 	 	 	 this.AdditionalExclusionEvaluation = ko.observable<any>();
	 	 	 	 this.VaryWashoutPeirod = ko.observable<any>();
	 	 	 	 this.VaryExposures = ko.observable<any>();
	 	 	 	 this.DefineExposures1 = ko.observable<any>();
	 	 	 	 this.DefineExposures2 = ko.observable<any>();
	 	 	 	 this.DefineExposures3 = ko.observable<any>();
	 	 	 	 this.DefineExposures4 = ko.observable<any>();
	 	 	 	 this.DefineExposures5 = ko.observable<any>();
	 	 	 	 this.DefineExposures6 = ko.observable<any>();
	 	 	 	 this.DefineExposures7 = ko.observable<any>();
	 	 	 	 this.DefineExposures8 = ko.observable<any>();
	 	 	 	 this.DefineExposures9 = ko.observable<any>();
	 	 	 	 this.DefineExposures10 = ko.observable<any>();
	 	 	 	 this.DefineExposures11 = ko.observable<any>();
	 	 	 	 this.DefineExposures12 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod1 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod2 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod3 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod4 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod5 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod6 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod7 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod8 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod9 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod10 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod11 = ko.observable<any>();
	 	 	 	 this.WashoutPeriod12 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement1 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement2 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement3 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement4 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement5 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement6 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement7 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement8 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement9 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement10 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement11 = ko.observable<any>();
	 	 	 	 this.IncidenceRefinement12 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment1 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment2 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment3 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment4 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment5 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment6 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment7 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment8 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment9 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment10 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment11 = ko.observable<any>();
	 	 	 	 this.SpecifyExposedTimeAssessment12 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap1 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap2 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap3 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap4 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap5 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap6 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap7 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap8 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap9 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap10 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap11 = ko.observable<any>();
	 	 	 	 this.EpisodeAllowableGap12 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod1 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod2 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod3 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod4 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod5 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod6 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod7 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod8 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod9 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod10 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod11 = ko.observable<any>();
	 	 	 	 this.EpisodeExtensionPeriod12 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration1 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration2 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration3 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration4 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration5 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration6 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration7 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration8 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration9 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration10 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration11 = ko.observable<any>();
	 	 	 	 this.MinimumEpisodeDuration12 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply1 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply2 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply3 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply4 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply5 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply6 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply7 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply8 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply9 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply10 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply11 = ko.observable<any>();
	 	 	 	 this.MinimumDaysSupply12 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration1 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration2 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration3 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration4 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration5 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration6 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration7 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration8 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration9 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration10 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration11 = ko.observable<any>();
	 	 	 	 this.SpecifyFollowUpDuration12 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes1 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes2 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes3 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes4 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes5 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes6 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes7 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes8 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes9 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes10 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes11 = ko.observable<any>();
	 	 	 	 this.AllowOnOrMultipleExposureEpisodes12 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime1 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime2 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime3 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime4 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime5 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime6 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime7 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime8 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime9 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime10 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime11 = ko.observable<any>();
	 	 	 	 this.TruncateExposedtime12 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified1 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified2 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified3 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified4 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified5 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified6 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified7 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified8 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified9 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified10 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified11 = ko.observable<any>();
	 	 	 	 this.TruncateExposedTimeSpecified12 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod1 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod2 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod3 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod4 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod5 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod6 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod7 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod8 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod9 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod10 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod11 = ko.observable<any>();
	 	 	 	 this.SpecifyBlackoutPeriod12 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup14 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup15 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup16 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup14 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup15 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup16 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup24 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup25 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup26 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup24 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup25 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup26 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup34 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup35 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup36 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup34 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup35 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup36 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup44 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup45 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup46 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup44 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup45 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup46 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup54 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup55 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup56 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup54 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup55 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup56 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup64 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup65 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup66 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup64 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup65 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup66 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup71 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup72 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup73 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup74 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup75 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup76 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup71 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup72 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup73 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup74 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup75 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup76 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup81 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup82 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup83 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup84 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup85 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup86 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup81 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup82 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup83 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup84 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup85 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup86 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup91 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup92 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup93 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup94 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup95 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup96 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup91 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup92 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup93 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup94 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup95 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup96 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup101 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup102 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup103 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup104 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup105 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup106 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup101 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup102 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup103 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup104 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup105 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup106 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup111 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup112 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup113 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup114 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup115 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup116 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup111 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup112 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup113 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup114 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup115 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup116 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup121 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup122 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup123 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup124 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup125 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionInclusionCriteriaGroup126 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup121 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup122 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup123 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup124 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup125 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalInclusionEvaluationWindowGroup126 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup14 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup15 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup16 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup14 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup15 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup16 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup24 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup25 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup26 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup24 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup25 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup26 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup34 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup35 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup36 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup34 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup35 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup36 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup44 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup45 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup46 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup44 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup45 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup46 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup54 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup55 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup56 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup54 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup55 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup56 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup64 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup65 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup66 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup64 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup65 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup66 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup71 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup72 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup73 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup74 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup75 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup76 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup71 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup72 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup73 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup74 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup75 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup76 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup81 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup82 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup83 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup84 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup85 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup86 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup81 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup82 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup83 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup84 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup85 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup86 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup91 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup92 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup93 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup94 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup95 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup96 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup91 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup92 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup93 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup94 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup95 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup96 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup101 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup102 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup103 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup104 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup105 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup106 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup101 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup102 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup103 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup104 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup105 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup106 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup111 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup112 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup113 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup114 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup115 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup116 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup111 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup112 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup113 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup114 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup115 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup116 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup121 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup122 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup123 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup124 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup125 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionInclusionCriteriaGroup126 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup121 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup122 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup123 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup124 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup125 = ko.observable<any>();
	 	 	 	 this.SpecifyAdditionalExclusionEvaluationWindowGroup126 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup1 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup2 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup3 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup4 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup5 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup6 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup7 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup8 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup9 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup10 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup11 = ko.observable<any>();
	 	 	 	 this.LookBackPeriodGroup12 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate1 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate2 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate3 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate4 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate5 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate6 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate7 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate8 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate9 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate10 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate11 = ko.observable<any>();
	 	 	 	 this.IncludeIndexDate12 = ko.observable<any>();
	 	 	 	 this.StratificationCategories1 = ko.observable<any>();
	 	 	 	 this.StratificationCategories2 = ko.observable<any>();
	 	 	 	 this.StratificationCategories3 = ko.observable<any>();
	 	 	 	 this.StratificationCategories4 = ko.observable<any>();
	 	 	 	 this.StratificationCategories5 = ko.observable<any>();
	 	 	 	 this.StratificationCategories6 = ko.observable<any>();
	 	 	 	 this.StratificationCategories7 = ko.observable<any>();
	 	 	 	 this.StratificationCategories8 = ko.observable<any>();
	 	 	 	 this.StratificationCategories9 = ko.observable<any>();
	 	 	 	 this.StratificationCategories10 = ko.observable<any>();
	 	 	 	 this.StratificationCategories11 = ko.observable<any>();
	 	 	 	 this.StratificationCategories12 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod1 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod2 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod3 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod4 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod5 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod6 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod7 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod8 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod9 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod10 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod11 = ko.observable<any>();
	 	 	 	 this.TwelveSpecifyLoopBackPeriod12 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate1 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate2 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate3 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate4 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate5 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate6 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate7 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate8 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate9 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate10 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate11 = ko.observable<any>();
	 	 	 	 this.TwelveIncludeIndexDate12 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits1 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits2 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits3 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits4 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits5 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits6 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits7 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits8 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits9 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits10 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits11 = ko.observable<any>();
	 	 	 	 this.CareSettingsToDefineMedicalVisits12 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories1 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories2 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories3 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories4 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories5 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories6 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories7 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories8 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories9 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories10 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories11 = ko.observable<any>();
	 	 	 	 this.TwelveStratificationCategories12 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod1 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod2 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod3 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod4 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod5 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod6 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod7 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod8 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod9 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod10 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod11 = ko.observable<any>();
	 	 	 	 this.VaryLengthOfWashoutPeriod12 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime1 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime2 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime3 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime4 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime5 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime6 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime7 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime8 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime9 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime10 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime11 = ko.observable<any>();
	 	 	 	 this.VaryUserExposedTime12 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration1 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration2 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration3 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration4 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration5 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration6 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration7 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration8 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration9 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration10 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration11 = ko.observable<any>();
	 	 	 	 this.VaryUserFollowupPeriodDuration12 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod1 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod2 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod3 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod4 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod5 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod6 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod7 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod8 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod9 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod10 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod11 = ko.observable<any>();
	 	 	 	 this.VaryBlackoutPeriodPeriod12 = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3DefineExposures3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3WashoutPeriod3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyExposedTimeAssessment3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeAllowableGap3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3EpisodeExtensionPeriod3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumEpisodeDuration3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3MinimumDaysSupply3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyFollowUpDuration3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3AllowOnOrMultipleExposureEpisodes3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedtime3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3TruncateExposedTimeSpecified3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyBlackoutPeriod3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62 = ko.observable<any>();
	 	 	 	 this.Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63 = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryLengthOfWashoutPeriod3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryUserExposedTime3Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod1Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod1Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod2Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod2Compare = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod3Exposure = ko.observable<any>();
	 	 	 	 this.Level2or3VaryBlackoutPeriodPeriod3Compare = ko.observable<any>();
	 	 	 	 this.OutcomeList = ko.observableArray<OutcomeItemViewModel>();
	 	 	 	 this.AgeCovariate = ko.observable<any>();
	 	 	 	 this.SexCovariate = ko.observable<any>();
	 	 	 	 this.TimeCovariate = ko.observable<any>();
	 	 	 	 this.YearCovariate = ko.observable<any>();
	 	 	 	 this.ComorbidityCovariate = ko.observable<any>();
	 	 	 	 this.HealthCovariate = ko.observable<any>();
	 	 	 	 this.DrugCovariate = ko.observable<any>();
	 	 	 	 this.CovariateList = ko.observableArray<CovariateItemViewModel>();
	 	 	 	 this.hdPSAnalysis = ko.observable<any>();
	 	 	 	 this.InclusionCovariates = ko.observable<any>();
	 	 	 	 this.PoolCovariates = ko.observable<any>();
	 	 	 	 this.SelectionCovariates = ko.observable<any>();
	 	 	 	 this.ZeroCellCorrection = ko.observable<any>();
	 	 	 	 this.MatchingRatio = ko.observable<any>();
	 	 	 	 this.MatchingCalipers = ko.observable<any>();
	 	 	 	 this.VaryMatchingRatio = ko.observable<any>();
	 	 	 	 this.VaryMatchingCalipers = ko.observable<any>();
	 	 	  }else{
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
	 	 	 	 this.OutcomeList = ko.observableArray<OutcomeItemViewModel>(RequestFormDTO.OutcomeList == null ? null : RequestFormDTO.OutcomeList.map((item) => {return new OutcomeItemViewModel(item);}));
	 	 	 	 this.AgeCovariate = ko.observable(RequestFormDTO.AgeCovariate);
	 	 	 	 this.SexCovariate = ko.observable(RequestFormDTO.SexCovariate);
	 	 	 	 this.TimeCovariate = ko.observable(RequestFormDTO.TimeCovariate);
	 	 	 	 this.YearCovariate = ko.observable(RequestFormDTO.YearCovariate);
	 	 	 	 this.ComorbidityCovariate = ko.observable(RequestFormDTO.ComorbidityCovariate);
	 	 	 	 this.HealthCovariate = ko.observable(RequestFormDTO.HealthCovariate);
	 	 	 	 this.DrugCovariate = ko.observable(RequestFormDTO.DrugCovariate);
	 	 	 	 this.CovariateList = ko.observableArray<CovariateItemViewModel>(RequestFormDTO.CovariateList == null ? null : RequestFormDTO.CovariateList.map((item) => {return new CovariateItemViewModel(item);}));
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

	 	 public toData(): Dns.Interfaces.IRequestFormDTO{
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
	 	 	 	OutcomeList: this.OutcomeList == null ? null : this.OutcomeList().map((item) => {return item.toData();}),
	 	 	 	AgeCovariate: this.AgeCovariate(),
	 	 	 	SexCovariate: this.SexCovariate(),
	 	 	 	TimeCovariate: this.TimeCovariate(),
	 	 	 	YearCovariate: this.YearCovariate(),
	 	 	 	ComorbidityCovariate: this.ComorbidityCovariate(),
	 	 	 	HealthCovariate: this.HealthCovariate(),
	 	 	 	DrugCovariate: this.DrugCovariate(),
	 	 	 	CovariateList: this.CovariateList == null ? null : this.CovariateList().map((item) => {return item.toData();}),
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
	 export class OutcomeItemViewModel extends ViewModel<Dns.Interfaces.IOutcomeItemDTO>{
	 	 public CommonName: KnockoutObservable<string>;
	 	 public Outcome: KnockoutObservable<string>;
	 	 public WashoutPeriod: KnockoutObservable<string>;
	 	 public VaryWashoutPeriod: KnockoutObservable<string>;
	 	 constructor(OutcomeItemDTO?: Dns.Interfaces.IOutcomeItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (OutcomeItemDTO== null) {
	 	 	 	 this.CommonName = ko.observable<any>();
	 	 	 	 this.Outcome = ko.observable<any>();
	 	 	 	 this.WashoutPeriod = ko.observable<any>();
	 	 	 	 this.VaryWashoutPeriod = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.CommonName = ko.observable(OutcomeItemDTO.CommonName);
	 	 	 	 this.Outcome = ko.observable(OutcomeItemDTO.Outcome);
	 	 	 	 this.WashoutPeriod = ko.observable(OutcomeItemDTO.WashoutPeriod);
	 	 	 	 this.VaryWashoutPeriod = ko.observable(OutcomeItemDTO.VaryWashoutPeriod);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IOutcomeItemDTO{
	 	 	  return {
	 	 	 	CommonName: this.CommonName(),
	 	 	 	Outcome: this.Outcome(),
	 	 	 	WashoutPeriod: this.WashoutPeriod(),
	 	 	 	VaryWashoutPeriod: this.VaryWashoutPeriod(),
	 	 	  };
	 	  }



	 }
	 export class CovariateItemViewModel extends ViewModel<Dns.Interfaces.ICovariateItemDTO>{
	 	 public GroupingIndicator: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public CodeType: KnockoutObservable<string>;
	 	 public Ingredients: KnockoutObservable<string>;
	 	 public SubGroupAnalysis: KnockoutObservable<string>;
	 	 constructor(CovariateItemDTO?: Dns.Interfaces.ICovariateItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (CovariateItemDTO== null) {
	 	 	 	 this.GroupingIndicator = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.CodeType = ko.observable<any>();
	 	 	 	 this.Ingredients = ko.observable<any>();
	 	 	 	 this.SubGroupAnalysis = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.GroupingIndicator = ko.observable(CovariateItemDTO.GroupingIndicator);
	 	 	 	 this.Description = ko.observable(CovariateItemDTO.Description);
	 	 	 	 this.CodeType = ko.observable(CovariateItemDTO.CodeType);
	 	 	 	 this.Ingredients = ko.observable(CovariateItemDTO.Ingredients);
	 	 	 	 this.SubGroupAnalysis = ko.observable(CovariateItemDTO.SubGroupAnalysis);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ICovariateItemDTO{
	 	 	  return {
	 	 	 	GroupingIndicator: this.GroupingIndicator(),
	 	 	 	Description: this.Description(),
	 	 	 	CodeType: this.CodeType(),
	 	 	 	Ingredients: this.Ingredients(),
	 	 	 	SubGroupAnalysis: this.SubGroupAnalysis(),
	 	 	  };
	 	  }



	 }
	 export class WorkflowHistoryItemViewModel extends ViewModel<Dns.Interfaces.IWorkflowHistoryItemDTO>{
	 	 public TaskID: KnockoutObservable<any>;
	 	 public TaskName: KnockoutObservable<string>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public UserName: KnockoutObservable<string>;
	 	 public UserFullName: KnockoutObservable<string>;
	 	 public Message: KnockoutObservable<string>;
	 	 public Date: KnockoutObservable<Date>;
	 	 public RoutingID: KnockoutObservable<any>;
	 	 public DataMart: KnockoutObservable<string>;
	 	 public WorkflowActivityID: KnockoutObservable<any>;
	 	 constructor(WorkflowHistoryItemDTO?: Dns.Interfaces.IWorkflowHistoryItemDTO)
	 	  {
	 	 	  super();
	 	 	 if (WorkflowHistoryItemDTO== null) {
	 	 	 	 this.TaskID = ko.observable<any>();
	 	 	 	 this.TaskName = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.UserFullName = ko.observable<any>();
	 	 	 	 this.Message = ko.observable<any>();
	 	 	 	 this.Date = ko.observable<any>();
	 	 	 	 this.RoutingID = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.WorkflowActivityID = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IWorkflowHistoryItemDTO{
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
	 export class DistributedRegressionAnalysisCenterManifestItem extends ViewModel<Dns.Interfaces.IDistributedRegressionAnalysisCenterManifestItem>{
	 	 public DocumentID: KnockoutObservable<any>;
	 	 public RevisionSetID: KnockoutObservable<any>;
	 	 public ResponseID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public DataPartnerIdentifier: KnockoutObservable<string>;
	 	 public DataMart: KnockoutObservable<string>;
	 	 public RequestDataMartID: KnockoutObservable<any>;
	 	 constructor(DistributedRegressionAnalysisCenterManifestItem?: Dns.Interfaces.IDistributedRegressionAnalysisCenterManifestItem)
	 	  {
	 	 	  super();
	 	 	 if (DistributedRegressionAnalysisCenterManifestItem== null) {
	 	 	 	 this.DocumentID = ko.observable<any>();
	 	 	 	 this.RevisionSetID = ko.observable<any>();
	 	 	 	 this.ResponseID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.DataPartnerIdentifier = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.RequestDataMartID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DocumentID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DocumentID);
	 	 	 	 this.RevisionSetID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RevisionSetID);
	 	 	 	 this.ResponseID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.ResponseID);
	 	 	 	 this.DataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMartID);
	 	 	 	 this.DataPartnerIdentifier = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataPartnerIdentifier);
	 	 	 	 this.DataMart = ko.observable(DistributedRegressionAnalysisCenterManifestItem.DataMart);
	 	 	 	 this.RequestDataMartID = ko.observable(DistributedRegressionAnalysisCenterManifestItem.RequestDataMartID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDistributedRegressionAnalysisCenterManifestItem{
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
	 export class SectionSpecificTermViewModel extends ViewModel<Dns.Interfaces.ISectionSpecificTermDTO>{
	 	 public TermID: KnockoutObservable<any>;
	 	 public Section: KnockoutObservable<Dns.Enums.QueryComposerSections>;
	 	 constructor(SectionSpecificTermDTO?: Dns.Interfaces.ISectionSpecificTermDTO)
	 	  {
	 	 	  super();
	 	 	 if (SectionSpecificTermDTO== null) {
	 	 	 	 this.TermID = ko.observable<any>();
	 	 	 	 this.Section = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.TermID = ko.observable(SectionSpecificTermDTO.TermID);
	 	 	 	 this.Section = ko.observable(SectionSpecificTermDTO.Section);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ISectionSpecificTermDTO{
	 	 	  return {
	 	 	 	TermID: this.TermID(),
	 	 	 	Section: this.Section(),
	 	 	  };
	 	  }



	 }
	 export class TemplateTermViewModel extends ViewModel<Dns.Interfaces.ITemplateTermDTO>{
	 	 public TemplateID: KnockoutObservable<any>;
	 	 public Template: TemplateViewModel;
	 	 public TermID: KnockoutObservable<any>;
	 	 public Term: TermViewModel;
	 	 public Allowed: KnockoutObservable<boolean>;
	 	 public Section: KnockoutObservable<Dns.Enums.QueryComposerSections>;
	 	 constructor(TemplateTermDTO?: Dns.Interfaces.ITemplateTermDTO)
	 	  {
	 	 	  super();
	 	 	 if (TemplateTermDTO== null) {
	 	 	 	 this.TemplateID = ko.observable<any>();
	 	 	 	 this.Template = new TemplateViewModel();
	 	 	 	 this.TermID = ko.observable<any>();
	 	 	 	 this.Term = new TermViewModel();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Section = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.TemplateID = ko.observable(TemplateTermDTO.TemplateID);
	 	 	 	 this.Template = new TemplateViewModel(TemplateTermDTO.Template);
	 	 	 	 this.TermID = ko.observable(TemplateTermDTO.TermID);
	 	 	 	 this.Term = new TermViewModel(TemplateTermDTO.Term);
	 	 	 	 this.Allowed = ko.observable(TemplateTermDTO.Allowed);
	 	 	 	 this.Section = ko.observable(TemplateTermDTO.Section);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ITemplateTermDTO{
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
	 export class MatchingCriteriaViewModel extends ViewModel<Dns.Interfaces.IMatchingCriteriaDTO>{
	 	 public TermIDs: KnockoutObservableArray<any>;
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Request: KnockoutObservable<string>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 constructor(MatchingCriteriaDTO?: Dns.Interfaces.IMatchingCriteriaDTO)
	 	  {
	 	 	  super();
	 	 	 if (MatchingCriteriaDTO== null) {
	 	 	 	 this.TermIDs = ko.observableArray<any>();
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Request = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.TermIDs = ko.observableArray<any>(MatchingCriteriaDTO.TermIDs == null ? null : MatchingCriteriaDTO.TermIDs.map((item) => {return item;}));
	 	 	 	 this.ProjectID = ko.observable(MatchingCriteriaDTO.ProjectID);
	 	 	 	 this.Request = ko.observable(MatchingCriteriaDTO.Request);
	 	 	 	 this.RequestID = ko.observable(MatchingCriteriaDTO.RequestID);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IMatchingCriteriaDTO{
	 	 	  return {
	 	 	 	TermIDs: this.TermIDs(),
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	Request: this.Request(),
	 	 	 	RequestID: this.RequestID(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerCriteriaViewModel extends ViewModel<Dns.Interfaces.IQueryComposerCriteriaDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public RelatedToID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Operator: KnockoutObservable<Dns.Enums.QueryComposerOperators>;
	 	 public IndexEvent: KnockoutObservable<boolean>;
	 	 public Exclusion: KnockoutObservable<boolean>;
	 	 public Criteria: KnockoutObservableArray<QueryComposerCriteriaViewModel>;
	 	 public Terms: KnockoutObservableArray<QueryComposerTermViewModel>;
	 	 public Type: KnockoutObservable<Dns.Enums.QueryComposerCriteriaTypes>;
	 	 constructor(QueryComposerCriteriaDTO?: Dns.Interfaces.IQueryComposerCriteriaDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerCriteriaDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.RelatedToID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Operator = ko.observable<any>();
	 	 	 	 this.IndexEvent = ko.observable<any>();
	 	 	 	 this.Exclusion = ko.observable<any>();
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>();
	 	 	 	 this.Terms = ko.observableArray<QueryComposerTermViewModel>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(QueryComposerCriteriaDTO.ID);
	 	 	 	 this.RelatedToID = ko.observable(QueryComposerCriteriaDTO.RelatedToID);
	 	 	 	 this.Name = ko.observable(QueryComposerCriteriaDTO.Name);
	 	 	 	 this.Operator = ko.observable(QueryComposerCriteriaDTO.Operator);
	 	 	 	 this.IndexEvent = ko.observable(QueryComposerCriteriaDTO.IndexEvent);
	 	 	 	 this.Exclusion = ko.observable(QueryComposerCriteriaDTO.Exclusion);
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>(QueryComposerCriteriaDTO.Criteria == null ? null : QueryComposerCriteriaDTO.Criteria.map((item) => {return new QueryComposerCriteriaViewModel(item);}));
	 	 	 	 this.Terms = ko.observableArray<QueryComposerTermViewModel>(QueryComposerCriteriaDTO.Terms == null ? null : QueryComposerCriteriaDTO.Terms.map((item) => {return new QueryComposerTermViewModel(item);}));
	 	 	 	 this.Type = ko.observable(QueryComposerCriteriaDTO.Type);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerCriteriaDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	RelatedToID: this.RelatedToID(),
	 	 	 	Name: this.Name(),
	 	 	 	Operator: this.Operator(),
	 	 	 	IndexEvent: this.IndexEvent(),
	 	 	 	Exclusion: this.Exclusion(),
	 	 	 	Criteria: this.Criteria == null ? null : this.Criteria().map((item) => {return item.toData();}),
	 	 	 	Terms: this.Terms == null ? null : this.Terms().map((item) => {return item.toData();}),
	 	 	 	Type: this.Type(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerFieldViewModel extends ViewModel<Dns.Interfaces.IQueryComposerFieldDTO>{
	 	 public FieldName: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<any>;
	 	 public GroupBy: KnockoutObservable<any>;
	 	 public StratifyBy: KnockoutObservable<any>;
	 	 public Aggregate: KnockoutObservable<Dns.Enums.QueryComposerAggregates>;
	 	 public Select: KnockoutObservableArray<QueryComposerSelectViewModel>;
	 	 public OrderBy: KnockoutObservable<Dns.Enums.OrderByDirections>;
	 	 constructor(QueryComposerFieldDTO?: Dns.Interfaces.IQueryComposerFieldDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerFieldDTO== null) {
	 	 	 	 this.FieldName = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.GroupBy = ko.observable<any>();
	 	 	 	 this.StratifyBy = ko.observable<any>();
	 	 	 	 this.Aggregate = ko.observable<any>();
	 	 	 	 this.Select = ko.observableArray<QueryComposerSelectViewModel>();
	 	 	 	 this.OrderBy = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.FieldName = ko.observable(QueryComposerFieldDTO.FieldName);
	 	 	 	 this.Type = ko.observable(QueryComposerFieldDTO.Type);
	 	 	 	 this.GroupBy = ko.observable(QueryComposerFieldDTO.GroupBy);
	 	 	 	 this.StratifyBy = ko.observable(QueryComposerFieldDTO.StratifyBy);
	 	 	 	 this.Aggregate = ko.observable(QueryComposerFieldDTO.Aggregate);
	 	 	 	 this.Select = ko.observableArray<QueryComposerSelectViewModel>(QueryComposerFieldDTO.Select == null ? null : QueryComposerFieldDTO.Select.map((item) => {return new QueryComposerSelectViewModel(item);}));
	 	 	 	 this.OrderBy = ko.observable(QueryComposerFieldDTO.OrderBy);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerFieldDTO{
	 	 	  return {
	 	 	 	FieldName: this.FieldName(),
	 	 	 	Type: this.Type(),
	 	 	 	GroupBy: this.GroupBy(),
	 	 	 	StratifyBy: this.StratifyBy(),
	 	 	 	Aggregate: this.Aggregate(),
	 	 	 	Select: this.Select == null ? null : this.Select().map((item) => {return item.toData();}),
	 	 	 	OrderBy: this.OrderBy(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerGroupByViewModel extends ViewModel<Dns.Interfaces.IQueryComposerGroupByDTO>{
	 	 public Field: KnockoutObservable<string>;
	 	 public Aggregate: KnockoutObservable<Dns.Enums.QueryComposerAggregates>;
	 	 constructor(QueryComposerGroupByDTO?: Dns.Interfaces.IQueryComposerGroupByDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerGroupByDTO== null) {
	 	 	 	 this.Field = ko.observable<any>();
	 	 	 	 this.Aggregate = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Field = ko.observable(QueryComposerGroupByDTO.Field);
	 	 	 	 this.Aggregate = ko.observable(QueryComposerGroupByDTO.Aggregate);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerGroupByDTO{
	 	 	  return {
	 	 	 	Field: this.Field(),
	 	 	 	Aggregate: this.Aggregate(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerHeaderViewModel extends ViewModel<Dns.Interfaces.IQueryComposerHeaderDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public ViewUrl: KnockoutObservable<string>;
	 	 public Grammar: KnockoutObservable<string>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public QueryType: KnockoutObservable<Dns.Enums.QueryComposerQueryTypes>;
	 	 constructor(QueryComposerHeaderDTO?: Dns.Interfaces.IQueryComposerHeaderDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerHeaderDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.ViewUrl = ko.observable<any>();
	 	 	 	 this.Grammar = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.QueryType = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(QueryComposerHeaderDTO.Name);
	 	 	 	 this.Description = ko.observable(QueryComposerHeaderDTO.Description);
	 	 	 	 this.ViewUrl = ko.observable(QueryComposerHeaderDTO.ViewUrl);
	 	 	 	 this.Grammar = ko.observable(QueryComposerHeaderDTO.Grammar);
	 	 	 	 this.Priority = ko.observable(QueryComposerHeaderDTO.Priority);
	 	 	 	 this.DueDate = ko.observable(QueryComposerHeaderDTO.DueDate);
	 	 	 	 this.QueryType = ko.observable(QueryComposerHeaderDTO.QueryType);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerHeaderDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Description: this.Description(),
	 	 	 	ViewUrl: this.ViewUrl(),
	 	 	 	Grammar: this.Grammar(),
	 	 	 	Priority: this.Priority(),
	 	 	 	DueDate: this.DueDate(),
	 	 	 	QueryType: this.QueryType(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerOrderByViewModel extends ViewModel<Dns.Interfaces.IQueryComposerOrderByDTO>{
	 	 public Direction: KnockoutObservable<Dns.Enums.OrderByDirections>;
	 	 constructor(QueryComposerOrderByDTO?: Dns.Interfaces.IQueryComposerOrderByDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerOrderByDTO== null) {
	 	 	 	 this.Direction = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Direction = ko.observable(QueryComposerOrderByDTO.Direction);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerOrderByDTO{
	 	 	  return {
	 	 	 	Direction: this.Direction(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerResponseErrorViewModel extends ViewModel<Dns.Interfaces.IQueryComposerResponseErrorDTO>{
	 	 public Code: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 constructor(QueryComposerResponseErrorDTO?: Dns.Interfaces.IQueryComposerResponseErrorDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerResponseErrorDTO== null) {
	 	 	 	 this.Code = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Code = ko.observable(QueryComposerResponseErrorDTO.Code);
	 	 	 	 this.Description = ko.observable(QueryComposerResponseErrorDTO.Description);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerResponseErrorDTO{
	 	 	  return {
	 	 	 	Code: this.Code(),
	 	 	 	Description: this.Description(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerSelectViewModel extends ViewModel<Dns.Interfaces.IQueryComposerSelectDTO>{
	 	 public Fields: KnockoutObservableArray<QueryComposerFieldViewModel>;
	 	 constructor(QueryComposerSelectDTO?: Dns.Interfaces.IQueryComposerSelectDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerSelectDTO== null) {
	 	 	 	 this.Fields = ko.observableArray<QueryComposerFieldViewModel>();
	 	 	  }else{
	 	 	 	 this.Fields = ko.observableArray<QueryComposerFieldViewModel>(QueryComposerSelectDTO.Fields == null ? null : QueryComposerSelectDTO.Fields.map((item) => {return new QueryComposerFieldViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerSelectDTO{
	 	 	  return {
	 	 	 	Fields: this.Fields == null ? null : this.Fields().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerResponseViewModel extends ViewModel<Dns.Interfaces.IQueryComposerResponseDTO>{
	 	 public ID: KnockoutObservable<any>;
	 	 public ResponseDateTime: KnockoutObservable<Date>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public Errors: KnockoutObservableArray<QueryComposerResponseErrorViewModel>;
	 	 public Results: KnockoutObservableArray<any>;
	 	 public LowCellThrehold: KnockoutObservable<number>;
	 	 public Properties: KnockoutObservableArray<QueryComposerResponsePropertyDefinitionViewModel>;
	 	 public Aggregation: QueryComposerResponseAggregationDefinitionViewModel;
	 	 constructor(QueryComposerResponseDTO?: Dns.Interfaces.IQueryComposerResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerResponseDTO== null) {
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.ResponseDateTime = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.Errors = ko.observableArray<QueryComposerResponseErrorViewModel>();
	 	 	 	 this.Results = ko.observableArray<any>();
	 	 	 	 this.LowCellThrehold = ko.observable<any>();
	 	 	 	 this.Properties = ko.observableArray<QueryComposerResponsePropertyDefinitionViewModel>();
	 	 	 	 this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel();
	 	 	  }else{
	 	 	 	 this.ID = ko.observable(QueryComposerResponseDTO.ID);
	 	 	 	 this.ResponseDateTime = ko.observable(QueryComposerResponseDTO.ResponseDateTime);
	 	 	 	 this.RequestID = ko.observable(QueryComposerResponseDTO.RequestID);
	 	 	 	 this.Errors = ko.observableArray<QueryComposerResponseErrorViewModel>(QueryComposerResponseDTO.Errors == null ? null : QueryComposerResponseDTO.Errors.map((item) => {return new QueryComposerResponseErrorViewModel(item);}));
	 	 	 	 this.Results = ko.observableArray<any>(QueryComposerResponseDTO.Results == null ? null : QueryComposerResponseDTO.Results.map((item) => {return item;}));
	 	 	 	 this.LowCellThrehold = ko.observable(QueryComposerResponseDTO.LowCellThrehold);
	 	 	 	 this.Properties = ko.observableArray<QueryComposerResponsePropertyDefinitionViewModel>(QueryComposerResponseDTO.Properties == null ? null : QueryComposerResponseDTO.Properties.map((item) => {return new QueryComposerResponsePropertyDefinitionViewModel(item);}));
	 	 	 	 this.Aggregation = new QueryComposerResponseAggregationDefinitionViewModel(QueryComposerResponseDTO.Aggregation);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerResponseDTO{
	 	 	  return {
	 	 	 	ID: this.ID(),
	 	 	 	ResponseDateTime: this.ResponseDateTime(),
	 	 	 	RequestID: this.RequestID(),
	 	 	 	Errors: this.Errors == null ? null : this.Errors().map((item) => {return item.toData();}),
	 	 	 	Results: this.Results(),
	 	 	 	LowCellThrehold: this.LowCellThrehold(),
	 	 	 	Properties: this.Properties == null ? null : this.Properties().map((item) => {return item.toData();}),
	 	 	 	Aggregation: this.Aggregation.toData(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerResponseAggregationDefinitionViewModel extends ViewModel<Dns.Interfaces.IQueryComposerResponseAggregationDefinitionDTO>{
	 	 public GroupBy: KnockoutObservableArray<string>;
	 	 public Select: KnockoutObservableArray<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 constructor(QueryComposerResponseAggregationDefinitionDTO?: Dns.Interfaces.IQueryComposerResponseAggregationDefinitionDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerResponseAggregationDefinitionDTO== null) {
	 	 	 	 this.GroupBy = ko.observableArray<string>();
	 	 	 	 this.Select = ko.observableArray<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.GroupBy = ko.observableArray<string>(QueryComposerResponseAggregationDefinitionDTO.GroupBy == null ? null : QueryComposerResponseAggregationDefinitionDTO.GroupBy.map((item) => {return item;}));
	 	 	 	 this.Select = ko.observableArray<any>(QueryComposerResponseAggregationDefinitionDTO.Select == null ? null : QueryComposerResponseAggregationDefinitionDTO.Select.map((item) => {return item;}));
	 	 	 	 this.Name = ko.observable(QueryComposerResponseAggregationDefinitionDTO.Name);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerResponseAggregationDefinitionDTO{
	 	 	  return {
	 	 	 	GroupBy: this.GroupBy == null ? null : this.GroupBy().map((item) => {return item;}),
	 	 	 	Select: this.Select(),
	 	 	 	Name: this.Name(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerResponsePropertyDefinitionViewModel extends ViewModel<Dns.Interfaces.IQueryComposerResponsePropertyDefinitionDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<string>;
	 	 public As: KnockoutObservable<string>;
	 	 public Aggregate: KnockoutObservable<string>;
	 	 constructor(QueryComposerResponsePropertyDefinitionDTO?: Dns.Interfaces.IQueryComposerResponsePropertyDefinitionDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerResponsePropertyDefinitionDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.As = ko.observable<any>();
	 	 	 	 this.Aggregate = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Name);
	 	 	 	 this.Type = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Type);
	 	 	 	 this.As = ko.observable(QueryComposerResponsePropertyDefinitionDTO.As);
	 	 	 	 this.Aggregate = ko.observable(QueryComposerResponsePropertyDefinitionDTO.Aggregate);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerResponsePropertyDefinitionDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Type: this.Type(),
	 	 	 	As: this.As(),
	 	 	 	Aggregate: this.Aggregate(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerTermViewModel extends ViewModel<Dns.Interfaces.IQueryComposerTermDTO>{
	 	 public Operator: KnockoutObservable<Dns.Enums.QueryComposerOperators>;
	 	 public Type: KnockoutObservable<any>;
	 	 public Values: KnockoutObservable<any>;
	 	 public Criteria: KnockoutObservableArray<QueryComposerCriteriaViewModel>;
	 	 public Design: DesignViewModel;
	 	 constructor(QueryComposerTermDTO?: Dns.Interfaces.IQueryComposerTermDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerTermDTO== null) {
	 	 	 	 this.Operator = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.Values = ko.observable<any>({});
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>();
	 	 	 	 this.Design = new DesignViewModel();
	 	 	  }else{
	 	 	 	 this.Operator = ko.observable(QueryComposerTermDTO.Operator);
	 	 	 	 this.Type = ko.observable(QueryComposerTermDTO.Type);
	 	 	 	 this.Values = ko.observable(QueryComposerTermDTO.Values);
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>(QueryComposerTermDTO.Criteria == null ? null : QueryComposerTermDTO.Criteria.map((item) => {return new QueryComposerCriteriaViewModel(item);}));
	 	 	 	 this.Design = new DesignViewModel(QueryComposerTermDTO.Design);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerTermDTO{
	 	 	  return {
	 	 	 	Operator: this.Operator(),
	 	 	 	Type: this.Type(),
	 	 	 	Values: ko.mapping.toJS(this.Values()),
	 	 	 	Criteria: this.Criteria == null ? null : this.Criteria().map((item) => {return item.toData();}),
	 	 	 	Design: this.Design.toData(),
	 	 	  };
	 	  }



	 }
	 export class QueryComposerWhereViewModel extends ViewModel<Dns.Interfaces.IQueryComposerWhereDTO>{
	 	 public Criteria: KnockoutObservableArray<QueryComposerCriteriaViewModel>;
	 	 constructor(QueryComposerWhereDTO?: Dns.Interfaces.IQueryComposerWhereDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerWhereDTO== null) {
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>();
	 	 	  }else{
	 	 	 	 this.Criteria = ko.observableArray<QueryComposerCriteriaViewModel>(QueryComposerWhereDTO.Criteria == null ? null : QueryComposerWhereDTO.Criteria.map((item) => {return new QueryComposerCriteriaViewModel(item);}));
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerWhereDTO{
	 	 	  return {
	 	 	 	Criteria: this.Criteria == null ? null : this.Criteria().map((item) => {return item.toData();}),
	 	 	  };
	 	  }



	 }
	 export class ProjectRequestTypeViewModel extends EntityDtoViewModel<Dns.Interfaces.IProjectRequestTypeDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 public WorkflowID: KnockoutObservable<any>;
	 	 public Workflow: KnockoutObservable<string>;
	 	 public Template: KnockoutObservable<string>;
	 	 constructor(ProjectRequestTypeDTO?: Dns.Interfaces.IProjectRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectRequestTypeDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.WorkflowID = ko.observable<any>();
	 	 	 	 this.Workflow = ko.observable<any>();
	 	 	 	 this.Template = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectRequestTypeDTO.ProjectID);
	 	 	 	 this.RequestTypeID = ko.observable(ProjectRequestTypeDTO.RequestTypeID);
	 	 	 	 this.RequestType = ko.observable(ProjectRequestTypeDTO.RequestType);
	 	 	 	 this.WorkflowID = ko.observable(ProjectRequestTypeDTO.WorkflowID);
	 	 	 	 this.Workflow = ko.observable(ProjectRequestTypeDTO.Workflow);
	 	 	 	 this.Template = ko.observable(ProjectRequestTypeDTO.Template);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectRequestTypeDTO{
	 	 	  return {
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	RequestType: this.RequestType(),
	 	 	 	WorkflowID: this.WorkflowID(),
	 	 	 	Workflow: this.Workflow(),
	 	 	 	Template: this.Template(),
	 	 	  };
	 	  }



	 }
	 export class RequestObserverEventSubscriptionViewModel extends EntityDtoViewModel<Dns.Interfaces.IRequestObserverEventSubscriptionDTO>{
	 	 public RequestObserverID: KnockoutObservable<any>;
	 	 public EventID: KnockoutObservable<any>;
	 	 public LastRunTime: KnockoutObservable<Date>;
	 	 public NextDueTime: KnockoutObservable<Date>;
	 	 public Frequency: KnockoutObservable<Dns.Enums.Frequencies>;
	 	 constructor(RequestObserverEventSubscriptionDTO?: Dns.Interfaces.IRequestObserverEventSubscriptionDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestObserverEventSubscriptionDTO== null) {
	 	 	 	 this.RequestObserverID = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.LastRunTime = ko.observable<any>();
	 	 	 	 this.NextDueTime = ko.observable<any>();
	 	 	 	 this.Frequency = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestObserverID = ko.observable(RequestObserverEventSubscriptionDTO.RequestObserverID);
	 	 	 	 this.EventID = ko.observable(RequestObserverEventSubscriptionDTO.EventID);
	 	 	 	 this.LastRunTime = ko.observable(RequestObserverEventSubscriptionDTO.LastRunTime);
	 	 	 	 this.NextDueTime = ko.observable(RequestObserverEventSubscriptionDTO.NextDueTime);
	 	 	 	 this.Frequency = ko.observable(RequestObserverEventSubscriptionDTO.Frequency);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestObserverEventSubscriptionDTO{
	 	 	  return {
	 	 	 	RequestObserverID: this.RequestObserverID(),
	 	 	 	EventID: this.EventID(),
	 	 	 	LastRunTime: this.LastRunTime(),
	 	 	 	NextDueTime: this.NextDueTime(),
	 	 	 	Frequency: this.Frequency(),
	 	 	  };
	 	  }



	 }
	 export class RequestTypeTermViewModel extends EntityDtoViewModel<Dns.Interfaces.IRequestTypeTermDTO>{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public TermID: KnockoutObservable<any>;
	 	 public Term: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public OID: KnockoutObservable<string>;
	 	 public ReferenceUrl: KnockoutObservable<string>;
	 	 constructor(RequestTypeTermDTO?: Dns.Interfaces.IRequestTypeTermDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestTypeTermDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.TermID = ko.observable<any>();
	 	 	 	 this.Term = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.OID = ko.observable<any>();
	 	 	 	 this.ReferenceUrl = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypeID = ko.observable(RequestTypeTermDTO.RequestTypeID);
	 	 	 	 this.TermID = ko.observable(RequestTypeTermDTO.TermID);
	 	 	 	 this.Term = ko.observable(RequestTypeTermDTO.Term);
	 	 	 	 this.Description = ko.observable(RequestTypeTermDTO.Description);
	 	 	 	 this.OID = ko.observable(RequestTypeTermDTO.OID);
	 	 	 	 this.ReferenceUrl = ko.observable(RequestTypeTermDTO.ReferenceUrl);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestTypeTermDTO{
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
	 export class BaseFieldOptionAclViewModel extends EntityDtoViewModel<Dns.Interfaces.IBaseFieldOptionAclDTO>{
	 	 public FieldIdentifier: KnockoutObservable<string>;
	 	 public Permission: KnockoutObservable<Dns.Enums.FieldOptionPermissions>;
	 	 public Overridden: KnockoutObservable<boolean>;
	 	 public SecurityGroupID: KnockoutObservable<any>;
	 	 public SecurityGroup: KnockoutObservable<string>;
	 	 constructor(BaseFieldOptionAclDTO?: Dns.Interfaces.IBaseFieldOptionAclDTO)
	 	  {
	 	 	  super();
	 	 	 if (BaseFieldOptionAclDTO== null) {
	 	 	 	 this.FieldIdentifier = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.FieldIdentifier = ko.observable(BaseFieldOptionAclDTO.FieldIdentifier);
	 	 	 	 this.Permission = ko.observable(BaseFieldOptionAclDTO.Permission);
	 	 	 	 this.Overridden = ko.observable(BaseFieldOptionAclDTO.Overridden);
	 	 	 	 this.SecurityGroupID = ko.observable(BaseFieldOptionAclDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(BaseFieldOptionAclDTO.SecurityGroup);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IBaseFieldOptionAclDTO{
	 	 	  return {
	 	 	 	FieldIdentifier: this.FieldIdentifier(),
	 	 	 	Permission: this.Permission(),
	 	 	 	Overridden: this.Overridden(),
	 	 	 	SecurityGroupID: this.SecurityGroupID(),
	 	 	 	SecurityGroup: this.SecurityGroup(),
	 	 	  };
	 	  }



	 }
	 export class BaseEventPermissionViewModel extends EntityDtoViewModel<Dns.Interfaces.IBaseEventPermissionDTO>{
	 	 public SecurityGroupID: KnockoutObservable<any>;
	 	 public SecurityGroup: KnockoutObservable<string>;
	 	 public Allowed: KnockoutObservable<boolean>;
	 	 public Overridden: KnockoutObservable<boolean>;
	 	 public EventID: KnockoutObservable<any>;
	 	 public Event: KnockoutObservable<string>;
	 	 constructor(BaseEventPermissionDTO?: Dns.Interfaces.IBaseEventPermissionDTO)
	 	  {
	 	 	  super();
	 	 	 if (BaseEventPermissionDTO== null) {
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.SecurityGroupID = ko.observable(BaseEventPermissionDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(BaseEventPermissionDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(BaseEventPermissionDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(BaseEventPermissionDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(BaseEventPermissionDTO.EventID);
	 	 	 	 this.Event = ko.observable(BaseEventPermissionDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IBaseEventPermissionDTO{
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
	 export class OrganizationGroupViewModel extends EntityDtoViewModel<Dns.Interfaces.IOrganizationGroupDTO>{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 public GroupID: KnockoutObservable<any>;
	 	 public Group: KnockoutObservable<string>;
	 	 constructor(OrganizationGroupDTO?: Dns.Interfaces.IOrganizationGroupDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationGroupDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.GroupID = ko.observable<any>();
	 	 	 	 this.Group = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.OrganizationID = ko.observable(OrganizationGroupDTO.OrganizationID);
	 	 	 	 this.Organization = ko.observable(OrganizationGroupDTO.Organization);
	 	 	 	 this.GroupID = ko.observable(OrganizationGroupDTO.GroupID);
	 	 	 	 this.Group = ko.observable(OrganizationGroupDTO.Group);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IOrganizationGroupDTO{
	 	 	  return {
	 	 	 	OrganizationID: this.OrganizationID(),
	 	 	 	Organization: this.Organization(),
	 	 	 	GroupID: this.GroupID(),
	 	 	 	Group: this.Group(),
	 	 	  };
	 	  }



	 }
	 export class OrganizationRegistryViewModel extends EntityDtoViewModel<Dns.Interfaces.IOrganizationRegistryDTO>{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 public OrganizationParent: KnockoutObservable<string>;
	 	 public RegistryID: KnockoutObservable<any>;
	 	 public Registry: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<Dns.Enums.RegistryTypes>;
	 	 constructor(OrganizationRegistryDTO?: Dns.Interfaces.IOrganizationRegistryDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationRegistryDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.OrganizationParent = ko.observable<any>();
	 	 	 	 this.RegistryID = ko.observable<any>();
	 	 	 	 this.Registry = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IOrganizationRegistryDTO{
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
	 export class ProjectDataMartWithRequestTypesViewModel extends ProjectDataMartViewModel{
	 	 public RequestTypes: KnockoutObservableArray<RequestTypeViewModel>;
	 	 constructor(ProjectDataMartWithRequestTypesDTO?: Dns.Interfaces.IProjectDataMartWithRequestTypesDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectDataMartWithRequestTypesDTO== null) {
	 	 	 	 this.RequestTypes = ko.observableArray<RequestTypeViewModel>();
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.ProjectAcronym = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypes = ko.observableArray<RequestTypeViewModel>(ProjectDataMartWithRequestTypesDTO.RequestTypes == null ? null : ProjectDataMartWithRequestTypesDTO.RequestTypes.map((item) => {return new RequestTypeViewModel(item);}));
	 	 	 	 this.ProjectID = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectID);
	 	 	 	 this.Project = ko.observable(ProjectDataMartWithRequestTypesDTO.Project);
	 	 	 	 this.ProjectAcronym = ko.observable(ProjectDataMartWithRequestTypesDTO.ProjectAcronym);
	 	 	 	 this.DataMartID = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMartID);
	 	 	 	 this.DataMart = ko.observable(ProjectDataMartWithRequestTypesDTO.DataMart);
	 	 	 	 this.Organization = ko.observable(ProjectDataMartWithRequestTypesDTO.Organization);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectDataMartWithRequestTypesDTO{
	 	 	  return {
	 	 	 	RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => {return item.toData();}),
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	Project: this.Project(),
	 	 	 	ProjectAcronym: this.ProjectAcronym(),
	 	 	 	DataMartID: this.DataMartID(),
	 	 	 	DataMart: this.DataMart(),
	 	 	 	Organization: this.Organization(),
	 	 	  };
	 	  }



	 }
	 export class ProjectOrganizationViewModel extends EntityDtoViewModel<Dns.Interfaces.IProjectOrganizationDTO>{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Project: KnockoutObservable<string>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 constructor(ProjectOrganizationDTO?: Dns.Interfaces.IProjectOrganizationDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectOrganizationDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectOrganizationDTO.ProjectID);
	 	 	 	 this.Project = ko.observable(ProjectOrganizationDTO.Project);
	 	 	 	 this.OrganizationID = ko.observable(ProjectOrganizationDTO.OrganizationID);
	 	 	 	 this.Organization = ko.observable(ProjectOrganizationDTO.Organization);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectOrganizationDTO{
	 	 	  return {
	 	 	 	ProjectID: this.ProjectID(),
	 	 	 	Project: this.Project(),
	 	 	 	OrganizationID: this.OrganizationID(),
	 	 	 	Organization: this.Organization(),
	 	 	  };
	 	  }



	 }
	 export class BaseAclViewModel extends EntityDtoViewModel<Dns.Interfaces.IBaseAclDTO>{
	 	 public SecurityGroupID: KnockoutObservable<any>;
	 	 public SecurityGroup: KnockoutObservable<string>;
	 	 public Overridden: KnockoutObservable<boolean>;
	 	 constructor(BaseAclDTO?: Dns.Interfaces.IBaseAclDTO)
	 	  {
	 	 	  super();
	 	 	 if (BaseAclDTO== null) {
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.SecurityGroupID = ko.observable(BaseAclDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(BaseAclDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(BaseAclDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IBaseAclDTO{
	 	 	  return {
	 	 	 	SecurityGroupID: this.SecurityGroupID(),
	 	 	 	SecurityGroup: this.SecurityGroup(),
	 	 	 	Overridden: this.Overridden(),
	 	 	  };
	 	  }



	 }
	 export class UserEventSubscriptionViewModel extends EntityDtoViewModel<Dns.Interfaces.IUserEventSubscriptionDTO>{
	 	 public UserID: KnockoutObservable<any>;
	 	 public EventID: KnockoutObservable<any>;
	 	 public LastRunTime: KnockoutObservable<Date>;
	 	 public NextDueTime: KnockoutObservable<Date>;
	 	 public Frequency: KnockoutObservable<Dns.Enums.Frequencies>;
	 	 public FrequencyForMy: KnockoutObservable<Dns.Enums.Frequencies>;
	 	 constructor(UserEventSubscriptionDTO?: Dns.Interfaces.IUserEventSubscriptionDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserEventSubscriptionDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.LastRunTime = ko.observable<any>();
	 	 	 	 this.NextDueTime = ko.observable<any>();
	 	 	 	 this.Frequency = ko.observable<any>();
	 	 	 	 this.FrequencyForMy = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(UserEventSubscriptionDTO.UserID);
	 	 	 	 this.EventID = ko.observable(UserEventSubscriptionDTO.EventID);
	 	 	 	 this.LastRunTime = ko.observable(UserEventSubscriptionDTO.LastRunTime);
	 	 	 	 this.NextDueTime = ko.observable(UserEventSubscriptionDTO.NextDueTime);
	 	 	 	 this.Frequency = ko.observable(UserEventSubscriptionDTO.Frequency);
	 	 	 	 this.FrequencyForMy = ko.observable(UserEventSubscriptionDTO.FrequencyForMy);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUserEventSubscriptionDTO{
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
	 export class UserSettingViewModel extends EntityDtoViewModel<Dns.Interfaces.IUserSettingDTO>{
	 	 public UserID: KnockoutObservable<any>;
	 	 public Key: KnockoutObservable<string>;
	 	 public Setting: KnockoutObservable<string>;
	 	 constructor(UserSettingDTO?: Dns.Interfaces.IUserSettingDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserSettingDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Key = ko.observable<any>();
	 	 	 	 this.Setting = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(UserSettingDTO.UserID);
	 	 	 	 this.Key = ko.observable(UserSettingDTO.Key);
	 	 	 	 this.Setting = ko.observable(UserSettingDTO.Setting);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUserSettingDTO{
	 	 	  return {
	 	 	 	UserID: this.UserID(),
	 	 	 	Key: this.Key(),
	 	 	 	Setting: this.Setting(),
	 	 	  };
	 	  }



	 }
	 export class WFCommentViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IWFCommentDTO>{
	 	 public Comment: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public CreatedByID: KnockoutObservable<any>;
	 	 public CreatedBy: KnockoutObservable<string>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public TaskID: KnockoutObservable<any>;
	 	 public WorkflowActivityID: KnockoutObservable<any>;
	 	 public WorkflowActivity: KnockoutObservable<string>;
	 	 constructor(WFCommentDTO?: Dns.Interfaces.IWFCommentDTO)
	 	  {
	 	 	  super();
	 	 	 if (WFCommentDTO== null) {
	 	 	 	 this.Comment = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.CreatedByID = ko.observable<any>();
	 	 	 	 this.CreatedBy = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.TaskID = ko.observable<any>();
	 	 	 	 this.WorkflowActivityID = ko.observable<any>();
	 	 	 	 this.WorkflowActivity = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IWFCommentDTO{
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
	 export class CommentViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ICommentDTO>{
	 	 public Comment: KnockoutObservable<string>;
	 	 public ItemID: KnockoutObservable<any>;
	 	 public ItemTitle: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public CreatedByID: KnockoutObservable<any>;
	 	 public CreatedBy: KnockoutObservable<string>;
	 	 constructor(CommentDTO?: Dns.Interfaces.ICommentDTO)
	 	  {
	 	 	  super();
	 	 	 if (CommentDTO== null) {
	 	 	 	 this.Comment = ko.observable<any>();
	 	 	 	 this.ItemID = ko.observable<any>();
	 	 	 	 this.ItemTitle = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.CreatedByID = ko.observable<any>();
	 	 	 	 this.CreatedBy = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.ICommentDTO{
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
	 export class DocumentViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IDocumentDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public FileName: KnockoutObservable<string>;
	 	 public Viewable: KnockoutObservable<boolean>;
	 	 public MimeType: KnockoutObservable<string>;
	 	 public Kind: KnockoutObservable<string>;
	 	 public Data: KnockoutObservable<any>;
	 	 public Length: KnockoutObservable<number>;
	 	 public ItemID: KnockoutObservable<any>;
	 	 constructor(DocumentDTO?: Dns.Interfaces.IDocumentDTO)
	 	  {
	 	 	  super();
	 	 	 if (DocumentDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.FileName = ko.observable<any>();
	 	 	 	 this.Viewable = ko.observable<any>();
	 	 	 	 this.MimeType = ko.observable<any>();
	 	 	 	 this.Kind = ko.observable<any>();
	 	 	 	 this.Data = ko.observable<any>();
	 	 	 	 this.Length = ko.observable<any>();
	 	 	 	 this.ItemID = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IDocumentDTO{
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
	 export class ExtendedDocumentViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IExtendedDocumentDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public FileName: KnockoutObservable<string>;
	 	 public Viewable: KnockoutObservable<boolean>;
	 	 public MimeType: KnockoutObservable<string>;
	 	 public Kind: KnockoutObservable<string>;
	 	 public Length: KnockoutObservable<number>;
	 	 public ItemID: KnockoutObservable<any>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public ItemTitle: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public ParentDocumentID: KnockoutObservable<any>;
	 	 public UploadedByID: KnockoutObservable<any>;
	 	 public UploadedBy: KnockoutObservable<string>;
	 	 public RevisionSetID: KnockoutObservable<any>;
	 	 public RevisionDescription: KnockoutObservable<string>;
	 	 public MajorVersion: KnockoutObservable<number>;
	 	 public MinorVersion: KnockoutObservable<number>;
	 	 public BuildVersion: KnockoutObservable<number>;
	 	 public RevisionVersion: KnockoutObservable<number>;
	 	 public TaskItemType: KnockoutObservable<Dns.Enums.TaskItemTypes>;
	 	 public DocumentType: KnockoutObservable<Dns.Enums.RequestDocumentType>;
	 	 constructor(ExtendedDocumentDTO?: Dns.Interfaces.IExtendedDocumentDTO)
	 	  {
	 	 	  super();
	 	 	 if (ExtendedDocumentDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.FileName = ko.observable<any>();
	 	 	 	 this.Viewable = ko.observable<any>();
	 	 	 	 this.MimeType = ko.observable<any>();
	 	 	 	 this.Kind = ko.observable<any>();
	 	 	 	 this.Length = ko.observable<any>();
	 	 	 	 this.ItemID = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.ItemTitle = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.ParentDocumentID = ko.observable<any>();
	 	 	 	 this.UploadedByID = ko.observable<any>();
	 	 	 	 this.UploadedBy = ko.observable<any>();
	 	 	 	 this.RevisionSetID = ko.observable<any>();
	 	 	 	 this.RevisionDescription = ko.observable<any>();
	 	 	 	 this.MajorVersion = ko.observable<any>();
	 	 	 	 this.MinorVersion = ko.observable<any>();
	 	 	 	 this.BuildVersion = ko.observable<any>();
	 	 	 	 this.RevisionVersion = ko.observable<any>();
	 	 	 	 this.TaskItemType = ko.observable<any>();
	 	 	 	 this.DocumentType = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(ExtendedDocumentDTO.Name);
	 	 	 	 this.FileName = ko.observable(ExtendedDocumentDTO.FileName);
	 	 	 	 this.Viewable = ko.observable(ExtendedDocumentDTO.Viewable);
	 	 	 	 this.MimeType = ko.observable(ExtendedDocumentDTO.MimeType);
	 	 	 	 this.Kind = ko.observable(ExtendedDocumentDTO.Kind);
	 	 	 	 this.Length = ko.observable(ExtendedDocumentDTO.Length);
	 	 	 	 this.ItemID = ko.observable(ExtendedDocumentDTO.ItemID);
	 	 	 	 this.CreatedOn = ko.observable(ExtendedDocumentDTO.CreatedOn);
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

	 	 public toData(): Dns.Interfaces.IExtendedDocumentDTO{
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
	 	  }



	 }
	 export class OrganizationEHRSViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IOrganizationEHRSDTO>{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Type: KnockoutObservable<Dns.Enums.EHRSTypes>;
	 	 public System: KnockoutObservable<Dns.Enums.EHRSSystems>;
	 	 public Other: KnockoutObservable<string>;
	 	 public StartYear: KnockoutObservable<number>;
	 	 public EndYear: KnockoutObservable<number>;
	 	 constructor(OrganizationEHRSDTO?: Dns.Interfaces.IOrganizationEHRSDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationEHRSDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.System = ko.observable<any>();
	 	 	 	 this.Other = ko.observable<any>();
	 	 	 	 this.StartYear = ko.observable<any>();
	 	 	 	 this.EndYear = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IOrganizationEHRSDTO{
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
	 export class TemplateViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ITemplateDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public CreatedByID: KnockoutObservable<any>;
	 	 public CreatedBy: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public Data: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<Dns.Enums.TemplateTypes>;
	 	 public Notes: KnockoutObservable<string>;
	 	 public QueryType: KnockoutObservable<Dns.Enums.QueryComposerQueryTypes>;
	 	 public ComposerInterface: KnockoutObservable<Dns.Enums.QueryComposerInterface>;
	 	 constructor(TemplateDTO?: Dns.Interfaces.ITemplateDTO)
	 	  {
	 	 	  super();
	 	 	 if (TemplateDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.CreatedByID = ko.observable<any>();
	 	 	 	 this.CreatedBy = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.Data = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.Notes = ko.observable<any>();
	 	 	 	 this.QueryType = ko.observable<any>();
	 	 	 	 this.ComposerInterface = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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
	 	 	 	 this.ID = ko.observable(TemplateDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(TemplateDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ITemplateDTO{
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
	 	  }



	 }
	 export class TermViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ITermDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public OID: KnockoutObservable<string>;
	 	 public ReferenceUrl: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<Dns.Enums.TermTypes>;
	 	 constructor(TermDTO?: Dns.Interfaces.ITermDTO)
	 	  {
	 	 	  super();
	 	 	 if (TermDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.OID = ko.observable<any>();
	 	 	 	 this.ReferenceUrl = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(TermDTO.Name);
	 	 	 	 this.Description = ko.observable(TermDTO.Description);
	 	 	 	 this.OID = ko.observable(TermDTO.OID);
	 	 	 	 this.ReferenceUrl = ko.observable(TermDTO.ReferenceUrl);
	 	 	 	 this.Type = ko.observable(TermDTO.Type);
	 	 	 	 this.ID = ko.observable(TermDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(TermDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ITermDTO{
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
	 export class HomepageRequestDetailViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IHomepageRequestDetailDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Identifier: KnockoutObservable<number>;
	 	 public SubmittedOn: KnockoutObservable<Date>;
	 	 public SubmittedByName: KnockoutObservable<string>;
	 	 public SubmittedBy: KnockoutObservable<string>;
	 	 public SubmittedByID: KnockoutObservable<any>;
	 	 public StatusText: KnockoutObservable<string>;
	 	 public Status: KnockoutObservable<Dns.Enums.RequestStatuses>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 public Project: KnockoutObservable<string>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public MSRequestID: KnockoutObservable<string>;
	 	 public IsWorkflowRequest: KnockoutObservable<boolean>;
	 	 public CanEditMetadata: KnockoutObservable<boolean>;
	 	 constructor(HomepageRequestDetailDTO?: Dns.Interfaces.IHomepageRequestDetailDTO)
	 	  {
	 	 	  super();
	 	 	 if (HomepageRequestDetailDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Identifier = ko.observable<any>();
	 	 	 	 this.SubmittedOn = ko.observable<any>();
	 	 	 	 this.SubmittedByName = ko.observable<any>();
	 	 	 	 this.SubmittedBy = ko.observable<any>();
	 	 	 	 this.SubmittedByID = ko.observable<any>();
	 	 	 	 this.StatusText = ko.observable<any>();
	 	 	 	 this.Status = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.MSRequestID = ko.observable<any>();
	 	 	 	 this.IsWorkflowRequest = ko.observable<any>();
	 	 	 	 this.CanEditMetadata = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IHomepageRequestDetailDTO{
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
	 export class ReportAggregationLevelViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IReportAggregationLevelDTO>{
	 	 public NetworkID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public DeletedOn: KnockoutObservable<Date>;
	 	 constructor(ReportAggregationLevelDTO?: Dns.Interfaces.IReportAggregationLevelDTO)
	 	  {
	 	 	  super();
	 	 	 if (ReportAggregationLevelDTO== null) {
	 	 	 	 this.NetworkID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.DeletedOn = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.NetworkID = ko.observable(ReportAggregationLevelDTO.NetworkID);
	 	 	 	 this.Name = ko.observable(ReportAggregationLevelDTO.Name);
	 	 	 	 this.DeletedOn = ko.observable(ReportAggregationLevelDTO.DeletedOn);
	 	 	 	 this.ID = ko.observable(ReportAggregationLevelDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(ReportAggregationLevelDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IReportAggregationLevelDTO{
	 	 	  return {
	 	 	 	NetworkID: this.NetworkID(),
	 	 	 	Name: this.Name(),
	 	 	 	DeletedOn: this.DeletedOn(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class RequestMetadataViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRequestMetadataDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public PurposeOfUse: KnockoutObservable<string>;
	 	 public PhiDisclosureLevel: KnockoutObservable<string>;
	 	 public RequesterCenterID: KnockoutObservable<any>;
	 	 public ActivityID: KnockoutObservable<any>;
	 	 public ActivityProjectID: KnockoutObservable<any>;
	 	 public TaskOrderID: KnockoutObservable<any>;
	 	 public SourceActivityID: KnockoutObservable<any>;
	 	 public SourceActivityProjectID: KnockoutObservable<any>;
	 	 public SourceTaskOrderID: KnockoutObservable<any>;
	 	 public WorkplanTypeID: KnockoutObservable<any>;
	 	 public MSRequestID: KnockoutObservable<string>;
	 	 public ReportAggregationLevelID: KnockoutObservable<any>;
	 	 public ApplyChangesToRoutings: KnockoutObservable<boolean>;
	 	 constructor(RequestMetadataDTO?: Dns.Interfaces.IRequestMetadataDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestMetadataDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.PurposeOfUse = ko.observable<any>();
	 	 	 	 this.PhiDisclosureLevel = ko.observable<any>();
	 	 	 	 this.RequesterCenterID = ko.observable<any>();
	 	 	 	 this.ActivityID = ko.observable<any>();
	 	 	 	 this.ActivityProjectID = ko.observable<any>();
	 	 	 	 this.TaskOrderID = ko.observable<any>();
	 	 	 	 this.SourceActivityID = ko.observable<any>();
	 	 	 	 this.SourceActivityProjectID = ko.observable<any>();
	 	 	 	 this.SourceTaskOrderID = ko.observable<any>();
	 	 	 	 this.WorkplanTypeID = ko.observable<any>();
	 	 	 	 this.MSRequestID = ko.observable<any>();
	 	 	 	 this.ReportAggregationLevelID = ko.observable<any>();
	 	 	 	 this.ApplyChangesToRoutings = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IRequestMetadataDTO{
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
	 export class RequestObserverViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRequestObserverDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public UserID: KnockoutObservable<any>;
	 	 public SecurityGroupID: KnockoutObservable<any>;
	 	 public DisplayName: KnockoutObservable<string>;
	 	 public Email: KnockoutObservable<string>;
	 	 public EventSubscriptions: KnockoutObservableArray<RequestObserverEventSubscriptionViewModel>;
	 	 constructor(RequestObserverDTO?: Dns.Interfaces.IRequestObserverDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestObserverDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.DisplayName = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	 	 this.EventSubscriptions = ko.observableArray<RequestObserverEventSubscriptionViewModel>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestID = ko.observable(RequestObserverDTO.RequestID);
	 	 	 	 this.UserID = ko.observable(RequestObserverDTO.UserID);
	 	 	 	 this.SecurityGroupID = ko.observable(RequestObserverDTO.SecurityGroupID);
	 	 	 	 this.DisplayName = ko.observable(RequestObserverDTO.DisplayName);
	 	 	 	 this.Email = ko.observable(RequestObserverDTO.Email);
	 	 	 	 this.EventSubscriptions = ko.observableArray<RequestObserverEventSubscriptionViewModel>(RequestObserverDTO.EventSubscriptions == null ? null : RequestObserverDTO.EventSubscriptions.map((item) => {return new RequestObserverEventSubscriptionViewModel(item);}));
	 	 	 	 this.ID = ko.observable(RequestObserverDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(RequestObserverDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestObserverDTO{
	 	 	  return {
	 	 	 	RequestID: this.RequestID(),
	 	 	 	UserID: this.UserID(),
	 	 	 	SecurityGroupID: this.SecurityGroupID(),
	 	 	 	DisplayName: this.DisplayName(),
	 	 	 	Email: this.Email(),
	 	 	 	EventSubscriptions: this.EventSubscriptions == null ? null : this.EventSubscriptions().map((item) => {return item.toData();}),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class ResponseGroupViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IResponseGroupDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 constructor(ResponseGroupDTO?: Dns.Interfaces.IResponseGroupDTO)
	 	  {
	 	 	  super();
	 	 	 if (ResponseGroupDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(ResponseGroupDTO.Name);
	 	 	 	 this.ID = ko.observable(ResponseGroupDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(ResponseGroupDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IResponseGroupDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class AclGlobalFieldOptionViewModel extends BaseFieldOptionAclViewModel{
	 	 constructor(AclGlobalFieldOptionDTO?: Dns.Interfaces.IAclGlobalFieldOptionDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclGlobalFieldOptionDTO== null) {
	 	 	 	 this.FieldIdentifier = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.FieldIdentifier = ko.observable(AclGlobalFieldOptionDTO.FieldIdentifier);
	 	 	 	 this.Permission = ko.observable(AclGlobalFieldOptionDTO.Permission);
	 	 	 	 this.Overridden = ko.observable(AclGlobalFieldOptionDTO.Overridden);
	 	 	 	 this.SecurityGroupID = ko.observable(AclGlobalFieldOptionDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclGlobalFieldOptionDTO.SecurityGroup);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclGlobalFieldOptionDTO{
	 	 	  return {
	 	 	 	FieldIdentifier: this.FieldIdentifier(),
	 	 	 	Permission: this.Permission(),
	 	 	 	Overridden: this.Overridden(),
	 	 	 	SecurityGroupID: this.SecurityGroupID(),
	 	 	 	SecurityGroup: this.SecurityGroup(),
	 	 	  };
	 	  }



	 }
	 export class AclProjectFieldOptionViewModel extends BaseFieldOptionAclViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 constructor(AclProjectFieldOptionDTO?: Dns.Interfaces.IAclProjectFieldOptionDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectFieldOptionDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.FieldIdentifier = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(AclProjectFieldOptionDTO.ProjectID);
	 	 	 	 this.FieldIdentifier = ko.observable(AclProjectFieldOptionDTO.FieldIdentifier);
	 	 	 	 this.Permission = ko.observable(AclProjectFieldOptionDTO.Permission);
	 	 	 	 this.Overridden = ko.observable(AclProjectFieldOptionDTO.Overridden);
	 	 	 	 this.SecurityGroupID = ko.observable(AclProjectFieldOptionDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclProjectFieldOptionDTO.SecurityGroup);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclProjectFieldOptionDTO{
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
	 export class BaseAclRequestTypeViewModel extends BaseAclViewModel{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public Permission: KnockoutObservable<Dns.Enums.RequestTypePermissions>;
	 	 constructor(BaseAclRequestTypeDTO?: Dns.Interfaces.IBaseAclRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (BaseAclRequestTypeDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypeID = ko.observable(BaseAclRequestTypeDTO.RequestTypeID);
	 	 	 	 this.Permission = ko.observable(BaseAclRequestTypeDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(BaseAclRequestTypeDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(BaseAclRequestTypeDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(BaseAclRequestTypeDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IBaseAclRequestTypeDTO{
	 	 	  return {
	 	 	 	RequestTypeID: this.RequestTypeID(),
	 	 	 	Permission: this.Permission(),
	 	 	 	SecurityGroupID: this.SecurityGroupID(),
	 	 	 	SecurityGroup: this.SecurityGroup(),
	 	 	 	Overridden: this.Overridden(),
	 	 	  };
	 	  }



	 }
	 export class SecurityEntityViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ISecurityEntityDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Type: KnockoutObservable<Dns.Enums.SecurityEntityTypes>;
	 	 constructor(SecurityEntityDTO?: Dns.Interfaces.ISecurityEntityDTO)
	 	  {
	 	 	  super();
	 	 	 if (SecurityEntityDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(SecurityEntityDTO.Name);
	 	 	 	 this.Type = ko.observable(SecurityEntityDTO.Type);
	 	 	 	 this.ID = ko.observable(SecurityEntityDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(SecurityEntityDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.ISecurityEntityDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Type: this.Type(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class TaskViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ITaskDTO>{
	 	 public Subject: KnockoutObservable<string>;
	 	 public Location: KnockoutObservable<string>;
	 	 public Body: KnockoutObservable<string>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public StartOn: KnockoutObservable<Date>;
	 	 public EndOn: KnockoutObservable<Date>;
	 	 public EstimatedCompletedOn: KnockoutObservable<Date>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public Status: KnockoutObservable<Dns.Enums.TaskStatuses>;
	 	 public Type: KnockoutObservable<Dns.Enums.TaskTypes>;
	 	 public PercentComplete: KnockoutObservable<number>;
	 	 public WorkflowActivityID: KnockoutObservable<any>;
	 	 public DirectToRequest: KnockoutObservable<boolean>;
	 	 constructor(TaskDTO?: Dns.Interfaces.ITaskDTO)
	 	  {
	 	 	  super();
	 	 	 if (TaskDTO== null) {
	 	 	 	 this.Subject = ko.observable<any>();
	 	 	 	 this.Location = ko.observable<any>();
	 	 	 	 this.Body = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.StartOn = ko.observable<any>();
	 	 	 	 this.EndOn = ko.observable<any>();
	 	 	 	 this.EstimatedCompletedOn = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.Status = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.PercentComplete = ko.observable<any>();
	 	 	 	 this.WorkflowActivityID = ko.observable<any>();
	 	 	 	 this.DirectToRequest = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.ITaskDTO{
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
	 export class DataModelViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IDataModelDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public RequiresConfiguration: KnockoutObservable<boolean>;
	 	 public QueryComposer: KnockoutObservable<boolean>;
	 	 constructor(DataModelDTO?: Dns.Interfaces.IDataModelDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataModelDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.RequiresConfiguration = ko.observable<any>();
	 	 	 	 this.QueryComposer = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(DataModelDTO.Name);
	 	 	 	 this.Description = ko.observable(DataModelDTO.Description);
	 	 	 	 this.RequiresConfiguration = ko.observable(DataModelDTO.RequiresConfiguration);
	 	 	 	 this.QueryComposer = ko.observable(DataModelDTO.QueryComposer);
	 	 	 	 this.ID = ko.observable(DataModelDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(DataModelDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataModelDTO{
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
	 export class DataMartListViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IDataMartListDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 public StartDate: KnockoutObservable<Date>;
	 	 public EndDate: KnockoutObservable<Date>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 public ParentOrganziationID: KnockoutObservable<any>;
	 	 public ParentOrganization: KnockoutObservable<string>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 constructor(DataMartListDTO?: Dns.Interfaces.IDataMartListDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartListDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.StartDate = ko.observable<any>();
	 	 	 	 this.EndDate = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.ParentOrganziationID = ko.observable<any>();
	 	 	 	 this.ParentOrganization = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IDataMartListDTO{
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
	 export class EventViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IEventDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Locations: KnockoutObservableArray<Dns.Enums.PermissionAclTypes>;
	 	 public SupportsMyNotifications: KnockoutObservable<boolean>;
	 	 constructor(EventDTO?: Dns.Interfaces.IEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (EventDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Locations = ko.observableArray<any>();
	 	 	 	 this.SupportsMyNotifications = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(EventDTO.Name);
	 	 	 	 this.Description = ko.observable(EventDTO.Description);
	 	 	 	 this.Locations = ko.observableArray<Dns.Enums.PermissionAclTypes>(EventDTO.Locations == null ? null : EventDTO.Locations.map((item) => {return item;}));
	 	 	 	 this.SupportsMyNotifications = ko.observable(EventDTO.SupportsMyNotifications);
	 	 	 	 this.ID = ko.observable(EventDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(EventDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IEventDTO{
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
	 export class GroupEventViewModel extends BaseEventPermissionViewModel{
	 	 public GroupID: KnockoutObservable<any>;
	 	 constructor(GroupEventDTO?: Dns.Interfaces.IGroupEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (GroupEventDTO== null) {
	 	 	 	 this.GroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.GroupID = ko.observable(GroupEventDTO.GroupID);
	 	 	 	 this.SecurityGroupID = ko.observable(GroupEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(GroupEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(GroupEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(GroupEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(GroupEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(GroupEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IGroupEventDTO{
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
	 export class OrganizationEventViewModel extends BaseEventPermissionViewModel{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 constructor(OrganizationEventDTO?: Dns.Interfaces.IOrganizationEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationEventDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.OrganizationID = ko.observable(OrganizationEventDTO.OrganizationID);
	 	 	 	 this.SecurityGroupID = ko.observable(OrganizationEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(OrganizationEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(OrganizationEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(OrganizationEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(OrganizationEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(OrganizationEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IOrganizationEventDTO{
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
	 export class RegistryEventViewModel extends BaseEventPermissionViewModel{
	 	 public RegistryID: KnockoutObservable<any>;
	 	 constructor(RegistryEventDTO?: Dns.Interfaces.IRegistryEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (RegistryEventDTO== null) {
	 	 	 	 this.RegistryID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RegistryID = ko.observable(RegistryEventDTO.RegistryID);
	 	 	 	 this.SecurityGroupID = ko.observable(RegistryEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(RegistryEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(RegistryEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(RegistryEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(RegistryEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(RegistryEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRegistryEventDTO{
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
	 export class UserEventViewModel extends BaseEventPermissionViewModel{
	 	 public UserID: KnockoutObservable<any>;
	 	 constructor(UserEventDTO?: Dns.Interfaces.IUserEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserEventDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(UserEventDTO.UserID);
	 	 	 	 this.SecurityGroupID = ko.observable(UserEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(UserEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(UserEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(UserEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(UserEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(UserEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IUserEventDTO{
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
	 export class GroupViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IGroupDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public ApprovalRequired: KnockoutObservable<boolean>;
	 	 constructor(GroupDTO?: Dns.Interfaces.IGroupDTO)
	 	  {
	 	 	  super();
	 	 	 if (GroupDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.ApprovalRequired = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(GroupDTO.Name);
	 	 	 	 this.Deleted = ko.observable(GroupDTO.Deleted);
	 	 	 	 this.ApprovalRequired = ko.observable(GroupDTO.ApprovalRequired);
	 	 	 	 this.ID = ko.observable(GroupDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(GroupDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IGroupDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Deleted: this.Deleted(),
	 	 	 	ApprovalRequired: this.ApprovalRequired(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class NetworkMessageViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.INetworkMessageDTO>{
	 	 public Subject: KnockoutObservable<string>;
	 	 public MessageText: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public Targets: KnockoutObservableArray<any>;
	 	 constructor(NetworkMessageDTO?: Dns.Interfaces.INetworkMessageDTO)
	 	  {
	 	 	  super();
	 	 	 if (NetworkMessageDTO== null) {
	 	 	 	 this.Subject = ko.observable<any>();
	 	 	 	 this.MessageText = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.Targets = ko.observableArray<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Subject = ko.observable(NetworkMessageDTO.Subject);
	 	 	 	 this.MessageText = ko.observable(NetworkMessageDTO.MessageText);
	 	 	 	 this.CreatedOn = ko.observable(NetworkMessageDTO.CreatedOn);
	 	 	 	 this.Targets = ko.observableArray<any>(NetworkMessageDTO.Targets == null ? null : NetworkMessageDTO.Targets.map((item) => {return item;}));
	 	 	 	 this.ID = ko.observable(NetworkMessageDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(NetworkMessageDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.INetworkMessageDTO{
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
	 export class OrganizationViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IOrganizationDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public Primary: KnockoutObservable<boolean>;
	 	 public ParentOrganizationID: KnockoutObservable<any>;
	 	 public ParentOrganization: KnockoutObservable<string>;
	 	 public ContactEmail: KnockoutObservable<string>;
	 	 public ContactFirstName: KnockoutObservable<string>;
	 	 public ContactLastName: KnockoutObservable<string>;
	 	 public ContactPhone: KnockoutObservable<string>;
	 	 public SpecialRequirements: KnockoutObservable<string>;
	 	 public UsageRestrictions: KnockoutObservable<string>;
	 	 public OrganizationDescription: KnockoutObservable<string>;
	 	 public PragmaticClinicalTrials: KnockoutObservable<boolean>;
	 	 public ObservationalParticipation: KnockoutObservable<boolean>;
	 	 public ProspectiveTrials: KnockoutObservable<boolean>;
	 	 public EnableClaimsAndBilling: KnockoutObservable<boolean>;
	 	 public EnableEHRA: KnockoutObservable<boolean>;
	 	 public EnableRegistries: KnockoutObservable<boolean>;
	 	 public DataModelMSCDM: KnockoutObservable<boolean>;
	 	 public DataModelHMORNVDW: KnockoutObservable<boolean>;
	 	 public DataModelESP: KnockoutObservable<boolean>;
	 	 public DataModelI2B2: KnockoutObservable<boolean>;
	 	 public DataModelOMOP: KnockoutObservable<boolean>;
	 	 public DataModelPCORI: KnockoutObservable<boolean>;
	 	 public DataModelOther: KnockoutObservable<boolean>;
	 	 public DataModelOtherText: KnockoutObservable<string>;
	 	 public InpatientClaims: KnockoutObservable<boolean>;
	 	 public OutpatientClaims: KnockoutObservable<boolean>;
	 	 public OutpatientPharmacyClaims: KnockoutObservable<boolean>;
	 	 public EnrollmentClaims: KnockoutObservable<boolean>;
	 	 public DemographicsClaims: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsClaims: KnockoutObservable<boolean>;
	 	 public VitalSignsClaims: KnockoutObservable<boolean>;
	 	 public OtherClaims: KnockoutObservable<boolean>;
	 	 public OtherClaimsText: KnockoutObservable<string>;
	 	 public Biorepositories: KnockoutObservable<boolean>;
	 	 public PatientReportedOutcomes: KnockoutObservable<boolean>;
	 	 public PatientReportedBehaviors: KnockoutObservable<boolean>;
	 	 public PrescriptionOrders: KnockoutObservable<boolean>;
	 	 public X509PublicKey: KnockoutObservable<string>;
	 	 constructor(OrganizationDTO?: Dns.Interfaces.IOrganizationDTO)
	 	  {
	 	 	  super();
	 	 	 if (OrganizationDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.Primary = ko.observable<any>();
	 	 	 	 this.ParentOrganizationID = ko.observable<any>();
	 	 	 	 this.ParentOrganization = ko.observable<any>();
	 	 	 	 this.ContactEmail = ko.observable<any>();
	 	 	 	 this.ContactFirstName = ko.observable<any>();
	 	 	 	 this.ContactLastName = ko.observable<any>();
	 	 	 	 this.ContactPhone = ko.observable<any>();
	 	 	 	 this.SpecialRequirements = ko.observable<any>();
	 	 	 	 this.UsageRestrictions = ko.observable<any>();
	 	 	 	 this.OrganizationDescription = ko.observable<any>();
	 	 	 	 this.PragmaticClinicalTrials = ko.observable<any>();
	 	 	 	 this.ObservationalParticipation = ko.observable<any>();
	 	 	 	 this.ProspectiveTrials = ko.observable<any>();
	 	 	 	 this.EnableClaimsAndBilling = ko.observable<any>();
	 	 	 	 this.EnableEHRA = ko.observable<any>();
	 	 	 	 this.EnableRegistries = ko.observable<any>();
	 	 	 	 this.DataModelMSCDM = ko.observable<any>();
	 	 	 	 this.DataModelHMORNVDW = ko.observable<any>();
	 	 	 	 this.DataModelESP = ko.observable<any>();
	 	 	 	 this.DataModelI2B2 = ko.observable<any>();
	 	 	 	 this.DataModelOMOP = ko.observable<any>();
	 	 	 	 this.DataModelPCORI = ko.observable<any>();
	 	 	 	 this.DataModelOther = ko.observable<any>();
	 	 	 	 this.DataModelOtherText = ko.observable<any>();
	 	 	 	 this.InpatientClaims = ko.observable<any>();
	 	 	 	 this.OutpatientClaims = ko.observable<any>();
	 	 	 	 this.OutpatientPharmacyClaims = ko.observable<any>();
	 	 	 	 this.EnrollmentClaims = ko.observable<any>();
	 	 	 	 this.DemographicsClaims = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsClaims = ko.observable<any>();
	 	 	 	 this.VitalSignsClaims = ko.observable<any>();
	 	 	 	 this.OtherClaims = ko.observable<any>();
	 	 	 	 this.OtherClaimsText = ko.observable<any>();
	 	 	 	 this.Biorepositories = ko.observable<any>();
	 	 	 	 this.PatientReportedOutcomes = ko.observable<any>();
	 	 	 	 this.PatientReportedBehaviors = ko.observable<any>();
	 	 	 	 this.PrescriptionOrders = ko.observable<any>();
	 	 	 	 this.X509PublicKey = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IOrganizationDTO{
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
	 export class ProjectViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IProjectDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Acronym: KnockoutObservable<string>;
	 	 public StartDate: KnockoutObservable<Date>;
	 	 public EndDate: KnockoutObservable<Date>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public Active: KnockoutObservable<boolean>;
	 	 public Description: KnockoutObservable<string>;
	 	 public GroupID: KnockoutObservable<any>;
	 	 public Group: KnockoutObservable<string>;
	 	 constructor(ProjectDTO?: Dns.Interfaces.IProjectDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.StartDate = ko.observable<any>();
	 	 	 	 this.EndDate = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.Active = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.GroupID = ko.observable<any>();
	 	 	 	 this.Group = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IProjectDTO{
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
	 export class RegistryViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRegistryDTO>{
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public Type: KnockoutObservable<Dns.Enums.RegistryTypes>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public RoPRUrl: KnockoutObservable<string>;
	 	 constructor(RegistryDTO?: Dns.Interfaces.IRegistryDTO)
	 	  {
	 	 	  super();
	 	 	 if (RegistryDTO== null) {
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.RoPRUrl = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Deleted = ko.observable(RegistryDTO.Deleted);
	 	 	 	 this.Type = ko.observable(RegistryDTO.Type);
	 	 	 	 this.Name = ko.observable(RegistryDTO.Name);
	 	 	 	 this.Description = ko.observable(RegistryDTO.Description);
	 	 	 	 this.RoPRUrl = ko.observable(RegistryDTO.RoPRUrl);
	 	 	 	 this.ID = ko.observable(RegistryDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(RegistryDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRegistryDTO{
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
	 export class RequestDataMartViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRequestDataMartDTO>{
	 	 public RequestID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public DataMart: KnockoutObservable<string>;
	 	 public Status: KnockoutObservable<Dns.Enums.RoutingStatus>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public RequestTime: KnockoutObservable<Date>;
	 	 public ResponseTime: KnockoutObservable<Date>;
	 	 public ErrorMessage: KnockoutObservable<string>;
	 	 public ErrorDetail: KnockoutObservable<string>;
	 	 public RejectReason: KnockoutObservable<string>;
	 	 public ResultsGrouped: KnockoutObservable<boolean>;
	 	 public Properties: KnockoutObservable<string>;
	 	 public RoutingType: KnockoutObservable<Dns.Enums.RoutingType>;
	 	 public ResponseID: KnockoutObservable<any>;
	 	 public ResponseGroupID: KnockoutObservable<any>;
	 	 public ResponseGroup: KnockoutObservable<string>;
	 	 public ResponseMessage: KnockoutObservable<string>;
	 	 constructor(RequestDataMartDTO?: Dns.Interfaces.IRequestDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestDataMartDTO== null) {
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.Status = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.RequestTime = ko.observable<any>();
	 	 	 	 this.ResponseTime = ko.observable<any>();
	 	 	 	 this.ErrorMessage = ko.observable<any>();
	 	 	 	 this.ErrorDetail = ko.observable<any>();
	 	 	 	 this.RejectReason = ko.observable<any>();
	 	 	 	 this.ResultsGrouped = ko.observable<any>();
	 	 	 	 this.Properties = ko.observable<any>();
	 	 	 	 this.RoutingType = ko.observable<any>();
	 	 	 	 this.ResponseID = ko.observable<any>();
	 	 	 	 this.ResponseGroupID = ko.observable<any>();
	 	 	 	 this.ResponseGroup = ko.observable<any>();
	 	 	 	 this.ResponseMessage = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestID = ko.observable(RequestDataMartDTO.RequestID);
	 	 	 	 this.DataMartID = ko.observable(RequestDataMartDTO.DataMartID);
	 	 	 	 this.DataMart = ko.observable(RequestDataMartDTO.DataMart);
	 	 	 	 this.Status = ko.observable(RequestDataMartDTO.Status);
	 	 	 	 this.Priority = ko.observable(RequestDataMartDTO.Priority);
	 	 	 	 this.DueDate = ko.observable(RequestDataMartDTO.DueDate);
	 	 	 	 this.RequestTime = ko.observable(RequestDataMartDTO.RequestTime);
	 	 	 	 this.ResponseTime = ko.observable(RequestDataMartDTO.ResponseTime);
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
	 	 	 	 this.ID = ko.observable(RequestDataMartDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(RequestDataMartDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestDataMartDTO{
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
	 	  }



	 }
	 export class RequestViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRequestDTO>{
	 	 public Identifier: KnockoutObservable<number>;
	 	 public MSRequestID: KnockoutObservable<string>;
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Project: KnockoutObservable<string>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public AdditionalInstructions: KnockoutObservable<string>;
	 	 public UpdatedOn: KnockoutObservable<Date>;
	 	 public UpdatedByID: KnockoutObservable<any>;
	 	 public UpdatedBy: KnockoutObservable<string>;
	 	 public MirrorBudgetFields: KnockoutObservable<boolean>;
	 	 public Scheduled: KnockoutObservable<boolean>;
	 	 public Template: KnockoutObservable<boolean>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public Priority: KnockoutObservable<Dns.Enums.Priorities>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 public PurposeOfUse: KnockoutObservable<string>;
	 	 public PhiDisclosureLevel: KnockoutObservable<string>;
	 	 public ReportAggregationLevelID: KnockoutObservable<any>;
	 	 public ReportAggregationLevel: KnockoutObservable<string>;
	 	 public Schedule: KnockoutObservable<string>;
	 	 public ScheduleCount: KnockoutObservable<number>;
	 	 public SubmittedOn: KnockoutObservable<Date>;
	 	 public SubmittedByID: KnockoutObservable<any>;
	 	 public SubmittedByName: KnockoutObservable<string>;
	 	 public SubmittedBy: KnockoutObservable<string>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 public AdapterPackageVersion: KnockoutObservable<string>;
	 	 public IRBApprovalNo: KnockoutObservable<string>;
	 	 public DueDate: KnockoutObservable<Date>;
	 	 public ActivityDescription: KnockoutObservable<string>;
	 	 public ActivityID: KnockoutObservable<any>;
	 	 public SourceActivityID: KnockoutObservable<any>;
	 	 public SourceActivityProjectID: KnockoutObservable<any>;
	 	 public SourceTaskOrderID: KnockoutObservable<any>;
	 	 public RequesterCenterID: KnockoutObservable<any>;
	 	 public RequesterCenter: KnockoutObservable<string>;
	 	 public WorkplanTypeID: KnockoutObservable<any>;
	 	 public WorkplanType: KnockoutObservable<string>;
	 	 public WorkflowID: KnockoutObservable<any>;
	 	 public Workflow: KnockoutObservable<string>;
	 	 public CurrentWorkFlowActivityID: KnockoutObservable<any>;
	 	 public CurrentWorkFlowActivity: KnockoutObservable<string>;
	 	 public Status: KnockoutObservable<Dns.Enums.RequestStatuses>;
	 	 public StatusText: KnockoutObservable<string>;
	 	 public MajorEventDate: KnockoutObservable<Date>;
	 	 public MajorEventByID: KnockoutObservable<any>;
	 	 public MajorEventBy: KnockoutObservable<string>;
	 	 public CreatedOn: KnockoutObservable<Date>;
	 	 public CreatedByID: KnockoutObservable<any>;
	 	 public CreatedBy: KnockoutObservable<string>;
	 	 public CompletedOn: KnockoutObservable<Date>;
	 	 public CancelledOn: KnockoutObservable<Date>;
	 	 public UserIdentifier: KnockoutObservable<string>;
	 	 public Query: KnockoutObservable<string>;
	 	 public ParentRequestID: KnockoutObservable<any>;
	 	 constructor(RequestDTO?: Dns.Interfaces.IRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestDTO== null) {
	 	 	 	 this.Identifier = ko.observable<any>();
	 	 	 	 this.MSRequestID = ko.observable<any>();
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.AdditionalInstructions = ko.observable<any>();
	 	 	 	 this.UpdatedOn = ko.observable<any>();
	 	 	 	 this.UpdatedByID = ko.observable<any>();
	 	 	 	 this.UpdatedBy = ko.observable<any>();
	 	 	 	 this.MirrorBudgetFields = ko.observable<any>();
	 	 	 	 this.Scheduled = ko.observable<any>();
	 	 	 	 this.Template = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.PurposeOfUse = ko.observable<any>();
	 	 	 	 this.PhiDisclosureLevel = ko.observable<any>();
	 	 	 	 this.ReportAggregationLevelID = ko.observable<any>();
	 	 	 	 this.ReportAggregationLevel = ko.observable<any>();
	 	 	 	 this.Schedule = ko.observable<any>();
	 	 	 	 this.ScheduleCount = ko.observable<any>();
	 	 	 	 this.SubmittedOn = ko.observable<any>();
	 	 	 	 this.SubmittedByID = ko.observable<any>();
	 	 	 	 this.SubmittedByName = ko.observable<any>();
	 	 	 	 this.SubmittedBy = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.AdapterPackageVersion = ko.observable<any>();
	 	 	 	 this.IRBApprovalNo = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.ActivityDescription = ko.observable<any>();
	 	 	 	 this.ActivityID = ko.observable<any>();
	 	 	 	 this.SourceActivityID = ko.observable<any>();
	 	 	 	 this.SourceActivityProjectID = ko.observable<any>();
	 	 	 	 this.SourceTaskOrderID = ko.observable<any>();
	 	 	 	 this.RequesterCenterID = ko.observable<any>();
	 	 	 	 this.RequesterCenter = ko.observable<any>();
	 	 	 	 this.WorkplanTypeID = ko.observable<any>();
	 	 	 	 this.WorkplanType = ko.observable<any>();
	 	 	 	 this.WorkflowID = ko.observable<any>();
	 	 	 	 this.Workflow = ko.observable<any>();
	 	 	 	 this.CurrentWorkFlowActivityID = ko.observable<any>();
	 	 	 	 this.CurrentWorkFlowActivity = ko.observable<any>();
	 	 	 	 this.Status = ko.observable<any>();
	 	 	 	 this.StatusText = ko.observable<any>();
	 	 	 	 this.MajorEventDate = ko.observable<any>();
	 	 	 	 this.MajorEventByID = ko.observable<any>();
	 	 	 	 this.MajorEventBy = ko.observable<any>();
	 	 	 	 this.CreatedOn = ko.observable<any>();
	 	 	 	 this.CreatedByID = ko.observable<any>();
	 	 	 	 this.CreatedBy = ko.observable<any>();
	 	 	 	 this.CompletedOn = ko.observable<any>();
	 	 	 	 this.CancelledOn = ko.observable<any>();
	 	 	 	 this.UserIdentifier = ko.observable<any>();
	 	 	 	 this.Query = ko.observable<any>();
	 	 	 	 this.ParentRequestID = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IRequestDTO{
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
	 export class RequestTypeViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IRequestTypeDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Metadata: KnockoutObservable<boolean>;
	 	 public PostProcess: KnockoutObservable<boolean>;
	 	 public AddFiles: KnockoutObservable<boolean>;
	 	 public RequiresProcessing: KnockoutObservable<boolean>;
	 	 public TemplateID: KnockoutObservable<any>;
	 	 public Template: KnockoutObservable<string>;
	 	 public WorkflowID: KnockoutObservable<any>;
	 	 public Workflow: KnockoutObservable<string>;
	 	 constructor(RequestTypeDTO?: Dns.Interfaces.IRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (RequestTypeDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Metadata = ko.observable<any>();
	 	 	 	 this.PostProcess = ko.observable<any>();
	 	 	 	 this.AddFiles = ko.observable<any>();
	 	 	 	 this.RequiresProcessing = ko.observable<any>();
	 	 	 	 this.TemplateID = ko.observable<any>();
	 	 	 	 this.Template = ko.observable<any>();
	 	 	 	 this.WorkflowID = ko.observable<any>();
	 	 	 	 this.Workflow = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(RequestTypeDTO.Name);
	 	 	 	 this.Description = ko.observable(RequestTypeDTO.Description);
	 	 	 	 this.Metadata = ko.observable(RequestTypeDTO.Metadata);
	 	 	 	 this.PostProcess = ko.observable(RequestTypeDTO.PostProcess);
	 	 	 	 this.AddFiles = ko.observable(RequestTypeDTO.AddFiles);
	 	 	 	 this.RequiresProcessing = ko.observable(RequestTypeDTO.RequiresProcessing);
	 	 	 	 this.TemplateID = ko.observable(RequestTypeDTO.TemplateID);
	 	 	 	 this.Template = ko.observable(RequestTypeDTO.Template);
	 	 	 	 this.WorkflowID = ko.observable(RequestTypeDTO.WorkflowID);
	 	 	 	 this.Workflow = ko.observable(RequestTypeDTO.Workflow);
	 	 	 	 this.ID = ko.observable(RequestTypeDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(RequestTypeDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IRequestTypeDTO{
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
	 	  }



	 }
	 export class ResponseViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IResponseDTO>{
	 	 public RequestDataMartID: KnockoutObservable<any>;
	 	 public ResponseGroupID: KnockoutObservable<any>;
	 	 public RespondedByID: KnockoutObservable<any>;
	 	 public ResponseTime: KnockoutObservable<Date>;
	 	 public Count: KnockoutObservable<number>;
	 	 public SubmittedOn: KnockoutObservable<Date>;
	 	 public SubmittedByID: KnockoutObservable<any>;
	 	 public SubmitMessage: KnockoutObservable<string>;
	 	 public ResponseMessage: KnockoutObservable<string>;
	 	 constructor(ResponseDTO?: Dns.Interfaces.IResponseDTO)
	 	  {
	 	 	  super();
	 	 	 if (ResponseDTO== null) {
	 	 	 	 this.RequestDataMartID = ko.observable<any>();
	 	 	 	 this.ResponseGroupID = ko.observable<any>();
	 	 	 	 this.RespondedByID = ko.observable<any>();
	 	 	 	 this.ResponseTime = ko.observable<any>();
	 	 	 	 this.Count = ko.observable<any>();
	 	 	 	 this.SubmittedOn = ko.observable<any>();
	 	 	 	 this.SubmittedByID = ko.observable<any>();
	 	 	 	 this.SubmitMessage = ko.observable<any>();
	 	 	 	 this.ResponseMessage = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IResponseDTO{
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
	 export class DataMartEventViewModel extends BaseEventPermissionViewModel{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 constructor(DataMartEventDTO?: Dns.Interfaces.IDataMartEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartEventDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(DataMartEventDTO.DataMartID);
	 	 	 	 this.SecurityGroupID = ko.observable(DataMartEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(DataMartEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(DataMartEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(DataMartEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(DataMartEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(DataMartEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataMartEventDTO{
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
	 export class AclViewModel extends BaseAclViewModel{
	 	 public Allowed: KnockoutObservable<boolean>;
	 	 public PermissionID: KnockoutObservable<any>;
	 	 public Permission: KnockoutObservable<string>;
	 	 constructor(AclDTO?: Dns.Interfaces.IAclDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclDTO== null) {
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Allowed = ko.observable(AclDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclDTO{
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
	 export class ProjectDataMartEventViewModel extends BaseEventPermissionViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 constructor(ProjectDataMartEventDTO?: Dns.Interfaces.IProjectDataMartEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectDataMartEventDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IProjectDataMartEventDTO{
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
	 export class ProjectEventViewModel extends BaseEventPermissionViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 constructor(ProjectEventDTO?: Dns.Interfaces.IProjectEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectEventDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(ProjectEventDTO.ProjectID);
	 	 	 	 this.SecurityGroupID = ko.observable(ProjectEventDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(ProjectEventDTO.SecurityGroup);
	 	 	 	 this.Allowed = ko.observable(ProjectEventDTO.Allowed);
	 	 	 	 this.Overridden = ko.observable(ProjectEventDTO.Overridden);
	 	 	 	 this.EventID = ko.observable(ProjectEventDTO.EventID);
	 	 	 	 this.Event = ko.observable(ProjectEventDTO.Event);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IProjectEventDTO{
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
	 export class ProjectOrganizationEventViewModel extends BaseEventPermissionViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 constructor(ProjectOrganizationEventDTO?: Dns.Interfaces.IProjectOrganizationEventDTO)
	 	  {
	 	 	  super();
	 	 	 if (ProjectOrganizationEventDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	 	 this.EventID = ko.observable<any>();
	 	 	 	 this.Event = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IProjectOrganizationEventDTO{
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
	 export class PermissionViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IPermissionDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Locations: KnockoutObservableArray<Dns.Enums.PermissionAclTypes>;
	 	 constructor(PermissionDTO?: Dns.Interfaces.IPermissionDTO)
	 	  {
	 	 	  super();
	 	 	 if (PermissionDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Locations = ko.observableArray<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(PermissionDTO.Name);
	 	 	 	 this.Description = ko.observable(PermissionDTO.Description);
	 	 	 	 this.Locations = ko.observableArray<Dns.Enums.PermissionAclTypes>(PermissionDTO.Locations == null ? null : PermissionDTO.Locations.map((item) => {return item;}));
	 	 	 	 this.ID = ko.observable(PermissionDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(PermissionDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IPermissionDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Description: this.Description(),
	 	 	 	Locations: this.Locations(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class SecurityGroupViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ISecurityGroupDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Path: KnockoutObservable<string>;
	 	 public OwnerID: KnockoutObservable<any>;
	 	 public Owner: KnockoutObservable<string>;
	 	 public ParentSecurityGroupID: KnockoutObservable<any>;
	 	 public ParentSecurityGroup: KnockoutObservable<string>;
	 	 public Kind: KnockoutObservable<Dns.Enums.SecurityGroupKinds>;
	 	 public Type: KnockoutObservable<Dns.Enums.SecurityGroupTypes>;
	 	 constructor(SecurityGroupDTO?: Dns.Interfaces.ISecurityGroupDTO)
	 	  {
	 	 	  super();
	 	 	 if (SecurityGroupDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Path = ko.observable<any>();
	 	 	 	 this.OwnerID = ko.observable<any>();
	 	 	 	 this.Owner = ko.observable<any>();
	 	 	 	 this.ParentSecurityGroupID = ko.observable<any>();
	 	 	 	 this.ParentSecurityGroup = ko.observable<any>();
	 	 	 	 this.Kind = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.ISecurityGroupDTO{
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
	 export class SsoEndpointViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.ISsoEndpointDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public PostUrl: KnockoutObservable<string>;
	 	 public oAuthKey: KnockoutObservable<string>;
	 	 public oAuthHash: KnockoutObservable<string>;
	 	 public RequirePassword: KnockoutObservable<boolean>;
	 	 public Group: KnockoutObservable<any>;
	 	 public DisplayIndex: KnockoutObservable<number>;
	 	 public Enabled: KnockoutObservable<boolean>;
	 	 constructor(SsoEndpointDTO?: Dns.Interfaces.ISsoEndpointDTO)
	 	  {
	 	 	  super();
	 	 	 if (SsoEndpointDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.PostUrl = ko.observable<any>();
	 	 	 	 this.oAuthKey = ko.observable<any>();
	 	 	 	 this.oAuthHash = ko.observable<any>();
	 	 	 	 this.RequirePassword = ko.observable<any>();
	 	 	 	 this.Group = ko.observable<any>();
	 	 	 	 this.DisplayIndex = ko.observable<any>();
	 	 	 	 this.Enabled = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.ISsoEndpointDTO{
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
	 export class UserViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IUserDTO>{
	 	 public UserName: KnockoutObservable<string>;
	 	 public Title: KnockoutObservable<string>;
	 	 public FirstName: KnockoutObservable<string>;
	 	 public LastName: KnockoutObservable<string>;
	 	 public MiddleName: KnockoutObservable<string>;
	 	 public Phone: KnockoutObservable<string>;
	 	 public Fax: KnockoutObservable<string>;
	 	 public Email: KnockoutObservable<string>;
	 	 public Active: KnockoutObservable<boolean>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 public Organization: KnockoutObservable<string>;
	 	 public OrganizationRequested: KnockoutObservable<string>;
	 	 public RoleID: KnockoutObservable<any>;
	 	 public RoleRequested: KnockoutObservable<string>;
	 	 public SignedUpOn: KnockoutObservable<Date>;
	 	 public ActivatedOn: KnockoutObservable<Date>;
	 	 public DeactivatedOn: KnockoutObservable<Date>;
	 	 public DeactivatedByID: KnockoutObservable<any>;
	 	 public DeactivatedBy: KnockoutObservable<string>;
	 	 public DeactivationReason: KnockoutObservable<string>;
	 	 public RejectReason: KnockoutObservable<string>;
	 	 public RejectedOn: KnockoutObservable<Date>;
	 	 public RejectedByID: KnockoutObservable<any>;
	 	 public RejectedBy: KnockoutObservable<string>;
	 	 constructor(UserDTO?: Dns.Interfaces.IUserDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserDTO== null) {
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Title = ko.observable<any>();
	 	 	 	 this.FirstName = ko.observable<any>();
	 	 	 	 this.LastName = ko.observable<any>();
	 	 	 	 this.MiddleName = ko.observable<any>();
	 	 	 	 this.Phone = ko.observable<any>();
	 	 	 	 this.Fax = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	 	 this.Active = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.OrganizationRequested = ko.observable<any>();
	 	 	 	 this.RoleID = ko.observable<any>();
	 	 	 	 this.RoleRequested = ko.observable<any>();
	 	 	 	 this.SignedUpOn = ko.observable<any>();
	 	 	 	 this.ActivatedOn = ko.observable<any>();
	 	 	 	 this.DeactivatedOn = ko.observable<any>();
	 	 	 	 this.DeactivatedByID = ko.observable<any>();
	 	 	 	 this.DeactivatedBy = ko.observable<any>();
	 	 	 	 this.DeactivationReason = ko.observable<any>();
	 	 	 	 this.RejectReason = ko.observable<any>();
	 	 	 	 this.RejectedOn = ko.observable<any>();
	 	 	 	 this.RejectedByID = ko.observable<any>();
	 	 	 	 this.RejectedBy = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IUserDTO{
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
	 export class WorkflowActivityViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IWorkflowActivityDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public Start: KnockoutObservable<boolean>;
	 	 public End: KnockoutObservable<boolean>;
	 	 constructor(WorkflowActivityDTO?: Dns.Interfaces.IWorkflowActivityDTO)
	 	  {
	 	 	  super();
	 	 	 if (WorkflowActivityDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Start = ko.observable<any>();
	 	 	 	 this.End = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(WorkflowActivityDTO.Name);
	 	 	 	 this.Description = ko.observable(WorkflowActivityDTO.Description);
	 	 	 	 this.Start = ko.observable(WorkflowActivityDTO.Start);
	 	 	 	 this.End = ko.observable(WorkflowActivityDTO.End);
	 	 	 	 this.ID = ko.observable(WorkflowActivityDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(WorkflowActivityDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IWorkflowActivityDTO{
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
	 export class WorkflowViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IWorkflowDTO>{
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 constructor(WorkflowDTO?: Dns.Interfaces.IWorkflowDTO)
	 	  {
	 	 	  super();
	 	 	 if (WorkflowDTO== null) {
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Name = ko.observable(WorkflowDTO.Name);
	 	 	 	 this.Description = ko.observable(WorkflowDTO.Description);
	 	 	 	 this.ID = ko.observable(WorkflowDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(WorkflowDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IWorkflowDTO{
	 	 	  return {
	 	 	 	Name: this.Name(),
	 	 	 	Description: this.Description(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class WorkflowRoleViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IWorkflowRoleDTO>{
	 	 public WorkflowID: KnockoutObservable<any>;
	 	 public Name: KnockoutObservable<string>;
	 	 public Description: KnockoutObservable<string>;
	 	 public IsRequestCreator: KnockoutObservable<boolean>;
	 	 constructor(WorkflowRoleDTO?: Dns.Interfaces.IWorkflowRoleDTO)
	 	  {
	 	 	  super();
	 	 	 if (WorkflowRoleDTO== null) {
	 	 	 	 this.WorkflowID = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.IsRequestCreator = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.WorkflowID = ko.observable(WorkflowRoleDTO.WorkflowID);
	 	 	 	 this.Name = ko.observable(WorkflowRoleDTO.Name);
	 	 	 	 this.Description = ko.observable(WorkflowRoleDTO.Description);
	 	 	 	 this.IsRequestCreator = ko.observable(WorkflowRoleDTO.IsRequestCreator);
	 	 	 	 this.ID = ko.observable(WorkflowRoleDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(WorkflowRoleDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IWorkflowRoleDTO{
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
	 export class QueryComposerRequestViewModel extends EntityDtoWithIDViewModel<Dns.Interfaces.IQueryComposerRequestDTO>{
	 	 public Header: QueryComposerHeaderViewModel;
	 	 public Where: QueryComposerWhereViewModel;
	 	 public Select: QueryComposerSelectViewModel;
	 	 constructor(QueryComposerRequestDTO?: Dns.Interfaces.IQueryComposerRequestDTO)
	 	  {
	 	 	  super();
	 	 	 if (QueryComposerRequestDTO== null) {
	 	 	 	 this.Header = new QueryComposerHeaderViewModel();
	 	 	 	 this.Where = new QueryComposerWhereViewModel();
	 	 	 	 this.Select = new QueryComposerSelectViewModel();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Header = new QueryComposerHeaderViewModel(QueryComposerRequestDTO.Header);
	 	 	 	 this.Where = new QueryComposerWhereViewModel(QueryComposerRequestDTO.Where);
	 	 	 	 this.Select = new QueryComposerSelectViewModel(QueryComposerRequestDTO.Select);
	 	 	 	 this.ID = ko.observable(QueryComposerRequestDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(QueryComposerRequestDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IQueryComposerRequestDTO{
	 	 	  return {
	 	 	 	Header: this.Header.toData(),
	 	 	 	Where: this.Where.toData(),
	 	 	 	Select: this.Select.toData(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class DataModelWithRequestTypesViewModel extends DataModelViewModel{
	 	 public RequestTypes: KnockoutObservableArray<RequestTypeViewModel>;
	 	 constructor(DataModelWithRequestTypesDTO?: Dns.Interfaces.IDataModelWithRequestTypesDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataModelWithRequestTypesDTO== null) {
	 	 	 	 this.RequestTypes = ko.observableArray<RequestTypeViewModel>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.RequiresConfiguration = ko.observable<any>();
	 	 	 	 this.QueryComposer = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RequestTypes = ko.observableArray<RequestTypeViewModel>(DataModelWithRequestTypesDTO.RequestTypes == null ? null : DataModelWithRequestTypesDTO.RequestTypes.map((item) => {return new RequestTypeViewModel(item);}));
	 	 	 	 this.Name = ko.observable(DataModelWithRequestTypesDTO.Name);
	 	 	 	 this.Description = ko.observable(DataModelWithRequestTypesDTO.Description);
	 	 	 	 this.RequiresConfiguration = ko.observable(DataModelWithRequestTypesDTO.RequiresConfiguration);
	 	 	 	 this.QueryComposer = ko.observable(DataModelWithRequestTypesDTO.QueryComposer);
	 	 	 	 this.ID = ko.observable(DataModelWithRequestTypesDTO.ID);
	 	 	 	 this.Timestamp = ko.observable(DataModelWithRequestTypesDTO.Timestamp);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IDataModelWithRequestTypesDTO{
	 	 	  return {
	 	 	 	RequestTypes: this.RequestTypes == null ? null : this.RequestTypes().map((item) => {return item.toData();}),
	 	 	 	Name: this.Name(),
	 	 	 	Description: this.Description(),
	 	 	 	RequiresConfiguration: this.RequiresConfiguration(),
	 	 	 	QueryComposer: this.QueryComposer(),
	 	 	 	ID: this.ID(),
	 	 	 	Timestamp: this.Timestamp(),
	 	 	  };
	 	  }



	 }
	 export class AclTemplateViewModel extends AclViewModel{
	 	 public TemplateID: KnockoutObservable<any>;
	 	 constructor(AclTemplateDTO?: Dns.Interfaces.IAclTemplateDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclTemplateDTO== null) {
	 	 	 	 this.TemplateID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.TemplateID = ko.observable(AclTemplateDTO.TemplateID);
	 	 	 	 this.Allowed = ko.observable(AclTemplateDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclTemplateDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclTemplateDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclTemplateDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclTemplateDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclTemplateDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclTemplateDTO{
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
	 export class DataMartViewModel extends DataMartListViewModel{
	 	 public RequiresApproval: KnockoutObservable<boolean>;
	 	 public DataMartTypeID: KnockoutObservable<any>;
	 	 public DataMartType: KnockoutObservable<string>;
	 	 public AvailablePeriod: KnockoutObservable<string>;
	 	 public ContactEmail: KnockoutObservable<string>;
	 	 public ContactFirstName: KnockoutObservable<string>;
	 	 public ContactLastName: KnockoutObservable<string>;
	 	 public ContactPhone: KnockoutObservable<string>;
	 	 public SpecialRequirements: KnockoutObservable<string>;
	 	 public UsageRestrictions: KnockoutObservable<string>;
	 	 public Deleted: KnockoutObservable<boolean>;
	 	 public HealthPlanDescription: KnockoutObservable<string>;
	 	 public IsGroupDataMart: KnockoutObservable<boolean>;
	 	 public UnattendedMode: KnockoutObservable<Dns.Enums.UnattendedModes>;
	 	 public DataUpdateFrequency: KnockoutObservable<string>;
	 	 public InpatientEHRApplication: KnockoutObservable<string>;
	 	 public OutpatientEHRApplication: KnockoutObservable<string>;
	 	 public OtherClaims: KnockoutObservable<string>;
	 	 public OtherInpatientEHRApplication: KnockoutObservable<string>;
	 	 public OtherOutpatientEHRApplication: KnockoutObservable<string>;
	 	 public LaboratoryResultsAny: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsClaims: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestName: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsDates: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestLOINC: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestSNOMED: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsSpecimenSource: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestDescriptions: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsOrderDates: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestResultsInterpretation: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestOther: KnockoutObservable<boolean>;
	 	 public LaboratoryResultsTestOtherText: KnockoutObservable<string>;
	 	 public InpatientEncountersAny: KnockoutObservable<boolean>;
	 	 public InpatientEncountersEncounterID: KnockoutObservable<boolean>;
	 	 public InpatientEncountersProviderIdentifier: KnockoutObservable<boolean>;
	 	 public InpatientDatesOfService: KnockoutObservable<boolean>;
	 	 public InpatientICD9Procedures: KnockoutObservable<boolean>;
	 	 public InpatientICD10Procedures: KnockoutObservable<boolean>;
	 	 public InpatientICD9Diagnosis: KnockoutObservable<boolean>;
	 	 public InpatientICD10Diagnosis: KnockoutObservable<boolean>;
	 	 public InpatientSNOMED: KnockoutObservable<boolean>;
	 	 public InpatientHPHCS: KnockoutObservable<boolean>;
	 	 public InpatientDisposition: KnockoutObservable<boolean>;
	 	 public InpatientDischargeStatus: KnockoutObservable<boolean>;
	 	 public InpatientOther: KnockoutObservable<boolean>;
	 	 public InpatientOtherText: KnockoutObservable<string>;
	 	 public OutpatientEncountersAny: KnockoutObservable<boolean>;
	 	 public OutpatientEncountersEncounterID: KnockoutObservable<boolean>;
	 	 public OutpatientEncountersProviderIdentifier: KnockoutObservable<boolean>;
	 	 public OutpatientClinicalSetting: KnockoutObservable<boolean>;
	 	 public OutpatientDatesOfService: KnockoutObservable<boolean>;
	 	 public OutpatientICD9Procedures: KnockoutObservable<boolean>;
	 	 public OutpatientICD10Procedures: KnockoutObservable<boolean>;
	 	 public OutpatientICD9Diagnosis: KnockoutObservable<boolean>;
	 	 public OutpatientICD10Diagnosis: KnockoutObservable<boolean>;
	 	 public OutpatientSNOMED: KnockoutObservable<boolean>;
	 	 public OutpatientHPHCS: KnockoutObservable<boolean>;
	 	 public OutpatientOther: KnockoutObservable<boolean>;
	 	 public OutpatientOtherText: KnockoutObservable<string>;
	 	 public ERPatientID: KnockoutObservable<boolean>;
	 	 public EREncounterID: KnockoutObservable<boolean>;
	 	 public EREnrollmentDates: KnockoutObservable<boolean>;
	 	 public EREncounterDates: KnockoutObservable<boolean>;
	 	 public ERClinicalSetting: KnockoutObservable<boolean>;
	 	 public ERICD9Diagnosis: KnockoutObservable<boolean>;
	 	 public ERICD10Diagnosis: KnockoutObservable<boolean>;
	 	 public ERHPHCS: KnockoutObservable<boolean>;
	 	 public ERNDC: KnockoutObservable<boolean>;
	 	 public ERSNOMED: KnockoutObservable<boolean>;
	 	 public ERProviderIdentifier: KnockoutObservable<boolean>;
	 	 public ERProviderFacility: KnockoutObservable<boolean>;
	 	 public EREncounterType: KnockoutObservable<boolean>;
	 	 public ERDRG: KnockoutObservable<boolean>;
	 	 public ERDRGType: KnockoutObservable<boolean>;
	 	 public EROther: KnockoutObservable<boolean>;
	 	 public EROtherText: KnockoutObservable<string>;
	 	 public DemographicsAny: KnockoutObservable<boolean>;
	 	 public DemographicsPatientID: KnockoutObservable<boolean>;
	 	 public DemographicsSex: KnockoutObservable<boolean>;
	 	 public DemographicsDateOfBirth: KnockoutObservable<boolean>;
	 	 public DemographicsDateOfDeath: KnockoutObservable<boolean>;
	 	 public DemographicsAddressInfo: KnockoutObservable<boolean>;
	 	 public DemographicsRace: KnockoutObservable<boolean>;
	 	 public DemographicsEthnicity: KnockoutObservable<boolean>;
	 	 public DemographicsOther: KnockoutObservable<boolean>;
	 	 public DemographicsOtherText: KnockoutObservable<string>;
	 	 public PatientOutcomesAny: KnockoutObservable<boolean>;
	 	 public PatientOutcomesInstruments: KnockoutObservable<boolean>;
	 	 public PatientOutcomesInstrumentText: KnockoutObservable<string>;
	 	 public PatientOutcomesHealthBehavior: KnockoutObservable<boolean>;
	 	 public PatientOutcomesHRQoL: KnockoutObservable<boolean>;
	 	 public PatientOutcomesReportedOutcome: KnockoutObservable<boolean>;
	 	 public PatientOutcomesOther: KnockoutObservable<boolean>;
	 	 public PatientOutcomesOtherText: KnockoutObservable<string>;
	 	 public PatientBehaviorHealthBehavior: KnockoutObservable<boolean>;
	 	 public PatientBehaviorInstruments: KnockoutObservable<boolean>;
	 	 public PatientBehaviorInstrumentText: KnockoutObservable<string>;
	 	 public PatientBehaviorOther: KnockoutObservable<boolean>;
	 	 public PatientBehaviorOtherText: KnockoutObservable<string>;
	 	 public VitalSignsAny: KnockoutObservable<boolean>;
	 	 public VitalSignsTemperature: KnockoutObservable<boolean>;
	 	 public VitalSignsHeight: KnockoutObservable<boolean>;
	 	 public VitalSignsWeight: KnockoutObservable<boolean>;
	 	 public VitalSignsBMI: KnockoutObservable<boolean>;
	 	 public VitalSignsBloodPressure: KnockoutObservable<boolean>;
	 	 public VitalSignsOther: KnockoutObservable<boolean>;
	 	 public VitalSignsOtherText: KnockoutObservable<string>;
	 	 public VitalSignsLength: KnockoutObservable<boolean>;
	 	 public PrescriptionOrdersAny: KnockoutObservable<boolean>;
	 	 public PrescriptionOrderDates: KnockoutObservable<boolean>;
	 	 public PrescriptionOrderRxNorm: KnockoutObservable<boolean>;
	 	 public PrescriptionOrderNDC: KnockoutObservable<boolean>;
	 	 public PrescriptionOrderOther: KnockoutObservable<boolean>;
	 	 public PrescriptionOrderOtherText: KnockoutObservable<string>;
	 	 public PharmacyDispensingAny: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingDates: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingRxNorm: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingDaysSupply: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingAmountDispensed: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingNDC: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingOther: KnockoutObservable<boolean>;
	 	 public PharmacyDispensingOtherText: KnockoutObservable<string>;
	 	 public BiorepositoriesAny: KnockoutObservable<boolean>;
	 	 public BiorepositoriesName: KnockoutObservable<boolean>;
	 	 public BiorepositoriesDescription: KnockoutObservable<boolean>;
	 	 public BiorepositoriesDiseaseName: KnockoutObservable<boolean>;
	 	 public BiorepositoriesSpecimenSource: KnockoutObservable<boolean>;
	 	 public BiorepositoriesSpecimenType: KnockoutObservable<boolean>;
	 	 public BiorepositoriesProcessingMethod: KnockoutObservable<boolean>;
	 	 public BiorepositoriesSNOMED: KnockoutObservable<boolean>;
	 	 public BiorepositoriesStorageMethod: KnockoutObservable<boolean>;
	 	 public BiorepositoriesOther: KnockoutObservable<boolean>;
	 	 public BiorepositoriesOtherText: KnockoutObservable<string>;
	 	 public LongitudinalCaptureAny: KnockoutObservable<boolean>;
	 	 public LongitudinalCapturePatientID: KnockoutObservable<boolean>;
	 	 public LongitudinalCaptureStart: KnockoutObservable<boolean>;
	 	 public LongitudinalCaptureStop: KnockoutObservable<boolean>;
	 	 public LongitudinalCaptureOther: KnockoutObservable<boolean>;
	 	 public LongitudinalCaptureOtherValue: KnockoutObservable<string>;
	 	 public DataModel: KnockoutObservable<string>;
	 	 public OtherDataModel: KnockoutObservable<string>;
	 	 public IsLocal: KnockoutObservable<boolean>;
	 	 public Url: KnockoutObservable<string>;
	 	 public AdapterID: KnockoutObservable<any>;
	 	 public Adapter: KnockoutObservable<string>;
	 	 public ProcessorID: KnockoutObservable<any>;
	 	 public DataPartnerIdentifier: KnockoutObservable<string>;
	 	 constructor(DataMartDTO?: Dns.Interfaces.IDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (DataMartDTO== null) {
	 	 	 	 this.RequiresApproval = ko.observable<any>();
	 	 	 	 this.DataMartTypeID = ko.observable<any>();
	 	 	 	 this.DataMartType = ko.observable<any>();
	 	 	 	 this.AvailablePeriod = ko.observable<any>();
	 	 	 	 this.ContactEmail = ko.observable<any>();
	 	 	 	 this.ContactFirstName = ko.observable<any>();
	 	 	 	 this.ContactLastName = ko.observable<any>();
	 	 	 	 this.ContactPhone = ko.observable<any>();
	 	 	 	 this.SpecialRequirements = ko.observable<any>();
	 	 	 	 this.UsageRestrictions = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.HealthPlanDescription = ko.observable<any>();
	 	 	 	 this.IsGroupDataMart = ko.observable<any>();
	 	 	 	 this.UnattendedMode = ko.observable<any>();
	 	 	 	 this.DataUpdateFrequency = ko.observable<any>();
	 	 	 	 this.InpatientEHRApplication = ko.observable<any>();
	 	 	 	 this.OutpatientEHRApplication = ko.observable<any>();
	 	 	 	 this.OtherClaims = ko.observable<any>();
	 	 	 	 this.OtherInpatientEHRApplication = ko.observable<any>();
	 	 	 	 this.OtherOutpatientEHRApplication = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsAny = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsClaims = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestName = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsDates = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestLOINC = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestSNOMED = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsSpecimenSource = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestDescriptions = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsOrderDates = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestResultsInterpretation = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestOther = ko.observable<any>();
	 	 	 	 this.LaboratoryResultsTestOtherText = ko.observable<any>();
	 	 	 	 this.InpatientEncountersAny = ko.observable<any>();
	 	 	 	 this.InpatientEncountersEncounterID = ko.observable<any>();
	 	 	 	 this.InpatientEncountersProviderIdentifier = ko.observable<any>();
	 	 	 	 this.InpatientDatesOfService = ko.observable<any>();
	 	 	 	 this.InpatientICD9Procedures = ko.observable<any>();
	 	 	 	 this.InpatientICD10Procedures = ko.observable<any>();
	 	 	 	 this.InpatientICD9Diagnosis = ko.observable<any>();
	 	 	 	 this.InpatientICD10Diagnosis = ko.observable<any>();
	 	 	 	 this.InpatientSNOMED = ko.observable<any>();
	 	 	 	 this.InpatientHPHCS = ko.observable<any>();
	 	 	 	 this.InpatientDisposition = ko.observable<any>();
	 	 	 	 this.InpatientDischargeStatus = ko.observable<any>();
	 	 	 	 this.InpatientOther = ko.observable<any>();
	 	 	 	 this.InpatientOtherText = ko.observable<any>();
	 	 	 	 this.OutpatientEncountersAny = ko.observable<any>();
	 	 	 	 this.OutpatientEncountersEncounterID = ko.observable<any>();
	 	 	 	 this.OutpatientEncountersProviderIdentifier = ko.observable<any>();
	 	 	 	 this.OutpatientClinicalSetting = ko.observable<any>();
	 	 	 	 this.OutpatientDatesOfService = ko.observable<any>();
	 	 	 	 this.OutpatientICD9Procedures = ko.observable<any>();
	 	 	 	 this.OutpatientICD10Procedures = ko.observable<any>();
	 	 	 	 this.OutpatientICD9Diagnosis = ko.observable<any>();
	 	 	 	 this.OutpatientICD10Diagnosis = ko.observable<any>();
	 	 	 	 this.OutpatientSNOMED = ko.observable<any>();
	 	 	 	 this.OutpatientHPHCS = ko.observable<any>();
	 	 	 	 this.OutpatientOther = ko.observable<any>();
	 	 	 	 this.OutpatientOtherText = ko.observable<any>();
	 	 	 	 this.ERPatientID = ko.observable<any>();
	 	 	 	 this.EREncounterID = ko.observable<any>();
	 	 	 	 this.EREnrollmentDates = ko.observable<any>();
	 	 	 	 this.EREncounterDates = ko.observable<any>();
	 	 	 	 this.ERClinicalSetting = ko.observable<any>();
	 	 	 	 this.ERICD9Diagnosis = ko.observable<any>();
	 	 	 	 this.ERICD10Diagnosis = ko.observable<any>();
	 	 	 	 this.ERHPHCS = ko.observable<any>();
	 	 	 	 this.ERNDC = ko.observable<any>();
	 	 	 	 this.ERSNOMED = ko.observable<any>();
	 	 	 	 this.ERProviderIdentifier = ko.observable<any>();
	 	 	 	 this.ERProviderFacility = ko.observable<any>();
	 	 	 	 this.EREncounterType = ko.observable<any>();
	 	 	 	 this.ERDRG = ko.observable<any>();
	 	 	 	 this.ERDRGType = ko.observable<any>();
	 	 	 	 this.EROther = ko.observable<any>();
	 	 	 	 this.EROtherText = ko.observable<any>();
	 	 	 	 this.DemographicsAny = ko.observable<any>();
	 	 	 	 this.DemographicsPatientID = ko.observable<any>();
	 	 	 	 this.DemographicsSex = ko.observable<any>();
	 	 	 	 this.DemographicsDateOfBirth = ko.observable<any>();
	 	 	 	 this.DemographicsDateOfDeath = ko.observable<any>();
	 	 	 	 this.DemographicsAddressInfo = ko.observable<any>();
	 	 	 	 this.DemographicsRace = ko.observable<any>();
	 	 	 	 this.DemographicsEthnicity = ko.observable<any>();
	 	 	 	 this.DemographicsOther = ko.observable<any>();
	 	 	 	 this.DemographicsOtherText = ko.observable<any>();
	 	 	 	 this.PatientOutcomesAny = ko.observable<any>();
	 	 	 	 this.PatientOutcomesInstruments = ko.observable<any>();
	 	 	 	 this.PatientOutcomesInstrumentText = ko.observable<any>();
	 	 	 	 this.PatientOutcomesHealthBehavior = ko.observable<any>();
	 	 	 	 this.PatientOutcomesHRQoL = ko.observable<any>();
	 	 	 	 this.PatientOutcomesReportedOutcome = ko.observable<any>();
	 	 	 	 this.PatientOutcomesOther = ko.observable<any>();
	 	 	 	 this.PatientOutcomesOtherText = ko.observable<any>();
	 	 	 	 this.PatientBehaviorHealthBehavior = ko.observable<any>();
	 	 	 	 this.PatientBehaviorInstruments = ko.observable<any>();
	 	 	 	 this.PatientBehaviorInstrumentText = ko.observable<any>();
	 	 	 	 this.PatientBehaviorOther = ko.observable<any>();
	 	 	 	 this.PatientBehaviorOtherText = ko.observable<any>();
	 	 	 	 this.VitalSignsAny = ko.observable<any>();
	 	 	 	 this.VitalSignsTemperature = ko.observable<any>();
	 	 	 	 this.VitalSignsHeight = ko.observable<any>();
	 	 	 	 this.VitalSignsWeight = ko.observable<any>();
	 	 	 	 this.VitalSignsBMI = ko.observable<any>();
	 	 	 	 this.VitalSignsBloodPressure = ko.observable<any>();
	 	 	 	 this.VitalSignsOther = ko.observable<any>();
	 	 	 	 this.VitalSignsOtherText = ko.observable<any>();
	 	 	 	 this.VitalSignsLength = ko.observable<any>();
	 	 	 	 this.PrescriptionOrdersAny = ko.observable<any>();
	 	 	 	 this.PrescriptionOrderDates = ko.observable<any>();
	 	 	 	 this.PrescriptionOrderRxNorm = ko.observable<any>();
	 	 	 	 this.PrescriptionOrderNDC = ko.observable<any>();
	 	 	 	 this.PrescriptionOrderOther = ko.observable<any>();
	 	 	 	 this.PrescriptionOrderOtherText = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingAny = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingDates = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingRxNorm = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingDaysSupply = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingAmountDispensed = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingNDC = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingOther = ko.observable<any>();
	 	 	 	 this.PharmacyDispensingOtherText = ko.observable<any>();
	 	 	 	 this.BiorepositoriesAny = ko.observable<any>();
	 	 	 	 this.BiorepositoriesName = ko.observable<any>();
	 	 	 	 this.BiorepositoriesDescription = ko.observable<any>();
	 	 	 	 this.BiorepositoriesDiseaseName = ko.observable<any>();
	 	 	 	 this.BiorepositoriesSpecimenSource = ko.observable<any>();
	 	 	 	 this.BiorepositoriesSpecimenType = ko.observable<any>();
	 	 	 	 this.BiorepositoriesProcessingMethod = ko.observable<any>();
	 	 	 	 this.BiorepositoriesSNOMED = ko.observable<any>();
	 	 	 	 this.BiorepositoriesStorageMethod = ko.observable<any>();
	 	 	 	 this.BiorepositoriesOther = ko.observable<any>();
	 	 	 	 this.BiorepositoriesOtherText = ko.observable<any>();
	 	 	 	 this.LongitudinalCaptureAny = ko.observable<any>();
	 	 	 	 this.LongitudinalCapturePatientID = ko.observable<any>();
	 	 	 	 this.LongitudinalCaptureStart = ko.observable<any>();
	 	 	 	 this.LongitudinalCaptureStop = ko.observable<any>();
	 	 	 	 this.LongitudinalCaptureOther = ko.observable<any>();
	 	 	 	 this.LongitudinalCaptureOtherValue = ko.observable<any>();
	 	 	 	 this.DataModel = ko.observable<any>();
	 	 	 	 this.OtherDataModel = ko.observable<any>();
	 	 	 	 this.IsLocal = ko.observable<any>();
	 	 	 	 this.Url = ko.observable<any>();
	 	 	 	 this.AdapterID = ko.observable<any>();
	 	 	 	 this.Adapter = ko.observable<any>();
	 	 	 	 this.ProcessorID = ko.observable<any>();
	 	 	 	 this.DataPartnerIdentifier = ko.observable<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Description = ko.observable<any>();
	 	 	 	 this.Acronym = ko.observable<any>();
	 	 	 	 this.StartDate = ko.observable<any>();
	 	 	 	 this.EndDate = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.ParentOrganziationID = ko.observable<any>();
	 	 	 	 this.ParentOrganization = ko.observable<any>();
	 	 	 	 this.Priority = ko.observable<any>();
	 	 	 	 this.DueDate = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IDataMartDTO{
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
	 export class ResponseDetailViewModel extends ResponseViewModel{
	 	 public Request: KnockoutObservable<string>;
	 	 public RequestID: KnockoutObservable<any>;
	 	 public DataMart: KnockoutObservable<string>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 public SubmittedBy: KnockoutObservable<string>;
	 	 public RespondedBy: KnockoutObservable<string>;
	 	 public Status: KnockoutObservable<Dns.Enums.RoutingStatus>;
	 	 constructor(ResponseDetailDTO?: Dns.Interfaces.IResponseDetailDTO)
	 	  {
	 	 	  super();
	 	 	 if (ResponseDetailDTO== null) {
	 	 	 	 this.Request = ko.observable<any>();
	 	 	 	 this.RequestID = ko.observable<any>();
	 	 	 	 this.DataMart = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.SubmittedBy = ko.observable<any>();
	 	 	 	 this.RespondedBy = ko.observable<any>();
	 	 	 	 this.Status = ko.observable<any>();
	 	 	 	 this.RequestDataMartID = ko.observable<any>();
	 	 	 	 this.ResponseGroupID = ko.observable<any>();
	 	 	 	 this.RespondedByID = ko.observable<any>();
	 	 	 	 this.ResponseTime = ko.observable<any>();
	 	 	 	 this.Count = ko.observable<any>();
	 	 	 	 this.SubmittedOn = ko.observable<any>();
	 	 	 	 this.SubmittedByID = ko.observable<any>();
	 	 	 	 this.SubmitMessage = ko.observable<any>();
	 	 	 	 this.ResponseMessage = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IResponseDetailDTO{
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
	 export class AclDataMartRequestTypeViewModel extends BaseAclRequestTypeViewModel{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 constructor(AclDataMartRequestTypeDTO?: Dns.Interfaces.IAclDataMartRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclDataMartRequestTypeDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(AclDataMartRequestTypeDTO.DataMartID);
	 	 	 	 this.RequestTypeID = ko.observable(AclDataMartRequestTypeDTO.RequestTypeID);
	 	 	 	 this.Permission = ko.observable(AclDataMartRequestTypeDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclDataMartRequestTypeDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclDataMartRequestTypeDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclDataMartRequestTypeDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclDataMartRequestTypeDTO{
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
	 export class AclDataMartViewModel extends AclViewModel{
	 	 public DataMartID: KnockoutObservable<any>;
	 	 constructor(AclDataMartDTO?: Dns.Interfaces.IAclDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclDataMartDTO== null) {
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.DataMartID = ko.observable(AclDataMartDTO.DataMartID);
	 	 	 	 this.Allowed = ko.observable(AclDataMartDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclDataMartDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclDataMartDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclDataMartDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclDataMartDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclDataMartDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclDataMartDTO{
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
	 export class AclGroupViewModel extends AclViewModel{
	 	 public GroupID: KnockoutObservable<any>;
	 	 constructor(AclGroupDTO?: Dns.Interfaces.IAclGroupDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclGroupDTO== null) {
	 	 	 	 this.GroupID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.GroupID = ko.observable(AclGroupDTO.GroupID);
	 	 	 	 this.Allowed = ko.observable(AclGroupDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclGroupDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclGroupDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclGroupDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclGroupDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclGroupDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclGroupDTO{
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
	 export class AclOrganizationViewModel extends AclViewModel{
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 constructor(AclOrganizationDTO?: Dns.Interfaces.IAclOrganizationDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclOrganizationDTO== null) {
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.OrganizationID = ko.observable(AclOrganizationDTO.OrganizationID);
	 	 	 	 this.Allowed = ko.observable(AclOrganizationDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclOrganizationDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclOrganizationDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclOrganizationDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclOrganizationDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclOrganizationDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclOrganizationDTO{
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
	 export class AclProjectOrganizationViewModel extends AclViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public OrganizationID: KnockoutObservable<any>;
	 	 constructor(AclProjectOrganizationDTO?: Dns.Interfaces.IAclProjectOrganizationDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectOrganizationDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IAclProjectOrganizationDTO{
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
	 export class AclProjectDataMartViewModel extends AclViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public DataMartID: KnockoutObservable<any>;
	 	 constructor(AclProjectDataMartDTO?: Dns.Interfaces.IAclProjectDataMartDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectDataMartDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IAclProjectDataMartDTO{
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
	 export class AclProjectViewModel extends AclViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 constructor(AclProjectDTO?: Dns.Interfaces.IAclProjectDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(AclProjectDTO.ProjectID);
	 	 	 	 this.Allowed = ko.observable(AclProjectDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclProjectDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclProjectDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclProjectDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclProjectDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclProjectDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclProjectDTO{
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
	 export class AclProjectRequestTypeViewModel extends BaseAclRequestTypeViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 constructor(AclProjectRequestTypeDTO?: Dns.Interfaces.IAclProjectRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectRequestTypeDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(AclProjectRequestTypeDTO.ProjectID);
	 	 	 	 this.RequestTypeID = ko.observable(AclProjectRequestTypeDTO.RequestTypeID);
	 	 	 	 this.Permission = ko.observable(AclProjectRequestTypeDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclProjectRequestTypeDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclProjectRequestTypeDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclProjectRequestTypeDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclProjectRequestTypeDTO{
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
	 export class AclRegistryViewModel extends AclViewModel{
	 	 public RegistryID: KnockoutObservable<any>;
	 	 constructor(AclRegistryDTO?: Dns.Interfaces.IAclRegistryDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclRegistryDTO== null) {
	 	 	 	 this.RegistryID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.RegistryID = ko.observable(AclRegistryDTO.RegistryID);
	 	 	 	 this.Allowed = ko.observable(AclRegistryDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclRegistryDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclRegistryDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclRegistryDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclRegistryDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclRegistryDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclRegistryDTO{
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
	 export class AclRequestTypeViewModel extends AclViewModel{
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 constructor(AclRequestTypeDTO?: Dns.Interfaces.IAclRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclRequestTypeDTO== null) {
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IAclRequestTypeDTO{
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
	 export class AclUserViewModel extends AclViewModel{
	 	 public UserID: KnockoutObservable<any>;
	 	 constructor(AclUserDTO?: Dns.Interfaces.IAclUserDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclUserDTO== null) {
	 	 	 	 this.UserID = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.UserID = ko.observable(AclUserDTO.UserID);
	 	 	 	 this.Allowed = ko.observable(AclUserDTO.Allowed);
	 	 	 	 this.PermissionID = ko.observable(AclUserDTO.PermissionID);
	 	 	 	 this.Permission = ko.observable(AclUserDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclUserDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclUserDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclUserDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclUserDTO{
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
	 export class SecurityGroupWithUsersViewModel extends SecurityGroupViewModel{
	 	 public Users: KnockoutObservableArray<any>;
	 	 constructor(SecurityGroupWithUsersDTO?: Dns.Interfaces.ISecurityGroupWithUsersDTO)
	 	  {
	 	 	  super();
	 	 	 if (SecurityGroupWithUsersDTO== null) {
	 	 	 	 this.Users = ko.observableArray<any>();
	 	 	 	 this.Name = ko.observable<any>();
	 	 	 	 this.Path = ko.observable<any>();
	 	 	 	 this.OwnerID = ko.observable<any>();
	 	 	 	 this.Owner = ko.observable<any>();
	 	 	 	 this.ParentSecurityGroupID = ko.observable<any>();
	 	 	 	 this.ParentSecurityGroup = ko.observable<any>();
	 	 	 	 this.Kind = ko.observable<any>();
	 	 	 	 this.Type = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.Users = ko.observableArray<any>(SecurityGroupWithUsersDTO.Users == null ? null : SecurityGroupWithUsersDTO.Users.map((item) => {return item;}));
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

	 	 public toData(): Dns.Interfaces.ISecurityGroupWithUsersDTO{
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
	 export class UserWithSecurityDetailsViewModel extends UserViewModel{
	 	 public PasswordHash: KnockoutObservable<string>;
	 	 constructor(UserWithSecurityDetailsDTO?: Dns.Interfaces.IUserWithSecurityDetailsDTO)
	 	  {
	 	 	  super();
	 	 	 if (UserWithSecurityDetailsDTO== null) {
	 	 	 	 this.PasswordHash = ko.observable<any>();
	 	 	 	 this.UserName = ko.observable<any>();
	 	 	 	 this.Title = ko.observable<any>();
	 	 	 	 this.FirstName = ko.observable<any>();
	 	 	 	 this.LastName = ko.observable<any>();
	 	 	 	 this.MiddleName = ko.observable<any>();
	 	 	 	 this.Phone = ko.observable<any>();
	 	 	 	 this.Fax = ko.observable<any>();
	 	 	 	 this.Email = ko.observable<any>();
	 	 	 	 this.Active = ko.observable<any>();
	 	 	 	 this.Deleted = ko.observable<any>();
	 	 	 	 this.OrganizationID = ko.observable<any>();
	 	 	 	 this.Organization = ko.observable<any>();
	 	 	 	 this.OrganizationRequested = ko.observable<any>();
	 	 	 	 this.RoleID = ko.observable<any>();
	 	 	 	 this.RoleRequested = ko.observable<any>();
	 	 	 	 this.SignedUpOn = ko.observable<any>();
	 	 	 	 this.ActivatedOn = ko.observable<any>();
	 	 	 	 this.DeactivatedOn = ko.observable<any>();
	 	 	 	 this.DeactivatedByID = ko.observable<any>();
	 	 	 	 this.DeactivatedBy = ko.observable<any>();
	 	 	 	 this.DeactivationReason = ko.observable<any>();
	 	 	 	 this.RejectReason = ko.observable<any>();
	 	 	 	 this.RejectedOn = ko.observable<any>();
	 	 	 	 this.RejectedByID = ko.observable<any>();
	 	 	 	 this.RejectedBy = ko.observable<any>();
	 	 	 	 this.ID = ko.observable<any>();
	 	 	 	 this.Timestamp = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IUserWithSecurityDetailsDTO{
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
	 export class AclProjectRequestTypeWorkflowActivityViewModel extends AclViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 public Project: KnockoutObservable<string>;
	 	 public RequestTypeID: KnockoutObservable<any>;
	 	 public RequestType: KnockoutObservable<string>;
	 	 public WorkflowActivityID: KnockoutObservable<any>;
	 	 public WorkflowActivity: KnockoutObservable<string>;
	 	 constructor(AclProjectRequestTypeWorkflowActivityDTO?: Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectRequestTypeWorkflowActivityDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.Project = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.RequestType = ko.observable<any>();
	 	 	 	 this.WorkflowActivityID = ko.observable<any>();
	 	 	 	 this.WorkflowActivity = ko.observable<any>();
	 	 	 	 this.Allowed = ko.observable<any>();
	 	 	 	 this.PermissionID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
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

	 	 public toData(): Dns.Interfaces.IAclProjectRequestTypeWorkflowActivityDTO{
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
	 export class AclProjectDataMartRequestTypeViewModel extends AclDataMartRequestTypeViewModel{
	 	 public ProjectID: KnockoutObservable<any>;
	 	 constructor(AclProjectDataMartRequestTypeDTO?: Dns.Interfaces.IAclProjectDataMartRequestTypeDTO)
	 	  {
	 	 	  super();
	 	 	 if (AclProjectDataMartRequestTypeDTO== null) {
	 	 	 	 this.ProjectID = ko.observable<any>();
	 	 	 	 this.DataMartID = ko.observable<any>();
	 	 	 	 this.RequestTypeID = ko.observable<any>();
	 	 	 	 this.Permission = ko.observable<any>();
	 	 	 	 this.SecurityGroupID = ko.observable<any>();
	 	 	 	 this.SecurityGroup = ko.observable<any>();
	 	 	 	 this.Overridden = ko.observable<any>();
	 	 	  }else{
	 	 	 	 this.ProjectID = ko.observable(AclProjectDataMartRequestTypeDTO.ProjectID);
	 	 	 	 this.DataMartID = ko.observable(AclProjectDataMartRequestTypeDTO.DataMartID);
	 	 	 	 this.RequestTypeID = ko.observable(AclProjectDataMartRequestTypeDTO.RequestTypeID);
	 	 	 	 this.Permission = ko.observable(AclProjectDataMartRequestTypeDTO.Permission);
	 	 	 	 this.SecurityGroupID = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroupID);
	 	 	 	 this.SecurityGroup = ko.observable(AclProjectDataMartRequestTypeDTO.SecurityGroup);
	 	 	 	 this.Overridden = ko.observable(AclProjectDataMartRequestTypeDTO.Overridden);
	 	 	 }
	 	 }

	 	 public toData(): Dns.Interfaces.IAclProjectDataMartRequestTypeDTO{
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
}


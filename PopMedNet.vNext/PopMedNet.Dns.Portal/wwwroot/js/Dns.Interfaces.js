export var KendoModelDataMartAvailabilityPeriodV2DTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'DataTable': { type: 'string', nullable: false },
        'PeriodCategory': { type: 'string', nullable: false },
        'Period': { type: 'string', nullable: false },
        'Year': { type: 'number', nullable: false },
        'Quarter': { type: 'number', nullable: true },
    }
};
export var KendoModelDataModelProcessorDTO = {
    fields: {
        'ModelID': { type: 'any', nullable: false },
        'Processor': { type: 'string', nullable: false },
        'ProcessorID': { type: 'any', nullable: false },
    }
};
export var KendoModelPropertyChangeDetailDTO = {
    fields: {
        'Property': { type: 'string', nullable: false },
        'PropertyDisplayName': { type: 'string', nullable: false },
        'OriginalValue': { type: 'any', nullable: false },
        'OriginalValueDisplay': { type: 'string', nullable: false },
        'NewValue': { type: 'any', nullable: false },
        'NewValueDisplay': { type: 'string', nullable: false },
    }
};
export var KendoModelHttpResponseErrors = {
    fields: {
        'Errors': { type: 'string[]', nullable: false },
    }
};
export var KendoModelAddWFCommentDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'WorkflowActivityID': { type: 'any', nullable: true },
        'Comment': { type: 'string', nullable: false },
    }
};
export var KendoModelCommentDocumentReferenceDTO = {
    fields: {
        'CommentID': { type: 'any', nullable: false },
        'DocumentID': { type: 'any', nullable: true },
        'RevisionSetID': { type: 'any', nullable: true },
        'DocumentName': { type: 'string', nullable: false },
        'FileName': { type: 'string', nullable: false },
    }
};
export var KendoModelUpdateDataMartInstalledModelsDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'Models': { type: 'any[]', nullable: false },
    }
};
export var KendoModelDataAvailabilityPeriodCategoryDTO = {
    fields: {
        'CategoryType': { type: 'string', nullable: false },
        'CategoryDescription': { type: 'string', nullable: false },
        'Published': { type: 'boolean', nullable: false },
        'DataMartDescription': { type: 'string', nullable: false },
    }
};
export var KendoModelDataMartAvailabilityPeriodDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'RequestID': { type: 'any', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'Period': { type: 'string', nullable: false },
        'Active': { type: 'boolean', nullable: false },
    }
};
export var KendoModelNotificationCrudDTO = {
    fields: {
        'ObjectID': { type: 'any', nullable: false },
        'State': { type: 'enums.objectstates', nullable: false },
    }
};
export var KendoModelOrganizationUpdateEHRsesDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'EHRS': { type: 'any[]', nullable: false },
    }
};
export var KendoModelProjectDataMartUpdateDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'DataMarts': { type: 'any[]', nullable: false },
    }
};
export var KendoModelProjectOrganizationUpdateDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'Organizations': { type: 'any[]', nullable: false },
    }
};
export var KendoModelUpdateProjectRequestTypesDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'RequestTypes': { type: 'any[]', nullable: false },
    }
};
export var KendoModelHasGlobalSecurityForTemplateDTO = {
    fields: {
        'SecurityGroupExistsForGlobalPermission': { type: 'boolean', nullable: false },
        'CurrentUserHasGlobalPermission': { type: 'boolean', nullable: false },
    }
};
export var KendoModelApproveRejectResponseDTO = {
    fields: {
        'ResponseID': { type: 'any', nullable: false },
    }
};
export var KendoModelCreateCriteriaGroupTemplateDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Json': { type: 'string', nullable: false },
        'AdapterDetail': { type: 'enums.querycomposerquerytypes', nullable: true },
    }
};
export var KendoModelEnhancedEventLogItemDTO = {
    fields: {
        'Step': { type: 'number', nullable: false },
        'Timestamp': { type: 'date', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Source': { type: 'string', nullable: false },
        'EventType': { type: 'string', nullable: false },
    }
};
export var KendoModelHomepageRouteDetailDTO = {
    fields: {
        'RequestDataMartID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'RoutingType': { type: 'enums.routingtype', nullable: true },
        'RequestID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Identifier': { type: 'number', nullable: false },
        'SubmittedOn': { type: 'date', nullable: true },
        'SubmittedByName': { type: 'string', nullable: false },
        'ResponseID': { type: 'any', nullable: false },
        'ResponseSubmittedOn': { type: 'date', nullable: true },
        'ResponseSubmittedByID': { type: 'any', nullable: true },
        'ResponseSubmittedBy': { type: 'string', nullable: false },
        'ResponseTime': { type: 'date', nullable: true },
        'RespondedByID': { type: 'any', nullable: true },
        'RespondedBy': { type: 'string', nullable: false },
        'ResponseGroupID': { type: 'any', nullable: true },
        'ResponseGroup': { type: 'string', nullable: false },
        'ResponseMessage': { type: 'string', nullable: false },
        'StatusText': { type: 'string', nullable: false },
        'RequestStatus': { type: 'enums.requeststatuses', nullable: false },
        'RoutingStatus': { type: 'enums.routingstatus', nullable: false },
        'RoutingStatusText': { type: 'string', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'Project': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'MSRequestID': { type: 'string', nullable: false },
        'IsWorkflowRequest': { type: 'boolean', nullable: false },
        'CanEditMetadata': { type: 'boolean', nullable: false },
    }
};
export var KendoModelRejectResponseDTO = {
    fields: {
        'Message': { type: 'string', nullable: false },
        'ResponseIDs': { type: 'any[]', nullable: false },
    }
};
export var KendoModelApproveResponseDTO = {
    fields: {
        'Message': { type: 'string', nullable: false },
        'ResponseIDs': { type: 'any[]', nullable: false },
    }
};
export var KendoModelRequestCompletionRequestDTO = {
    fields: {
        'DemandActivityResultID': { type: 'any', nullable: true },
        'Dto': { type: 'any', nullable: false },
        'DataMarts': { type: 'any[]', nullable: false },
        'Data': { type: 'string', nullable: false },
        'Comment': { type: 'string', nullable: false },
    }
};
export var KendoModelRequestCompletionResponseDTO = {
    fields: {
        'Uri': { type: 'string', nullable: false },
        'Entity': { type: 'any', nullable: false },
        'DataMarts': { type: 'any[]', nullable: false },
    }
};
export var KendoModelRequestSearchTermDTO = {
    fields: {
        'Type': { type: 'number', nullable: false },
        'StringValue': { type: 'string', nullable: false },
        'NumberValue': { type: 'number', nullable: true },
        'DateFrom': { type: 'date', nullable: true },
        'DateTo': { type: 'date', nullable: true },
        'NumberFrom': { type: 'number', nullable: true },
        'NumberTo': { type: 'number', nullable: true },
        'RequestID': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestTypeModelDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'DataModelID': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestUserDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'UserID': { type: 'any', nullable: false },
        'Username': { type: 'string', nullable: false },
        'FullName': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
        'WorkflowRoleID': { type: 'any', nullable: false },
        'WorkflowRole': { type: 'string', nullable: false },
        'IsRequestCreatorRole': { type: 'boolean', nullable: false },
    }
};
export var KendoModelResponseHistoryDTO = {
    fields: {
        'DataMartName': { type: 'string', nullable: false },
        'HistoryItems': { type: 'any[]', nullable: false },
        'ErrorMessage': { type: 'string', nullable: false },
    }
};
export var KendoModelResponseHistoryItemDTO = {
    fields: {
        'ResponseID': { type: 'any', nullable: false },
        'RequestID': { type: 'any', nullable: false },
        'DateTime': { type: 'date', nullable: false },
        'Action': { type: 'string', nullable: false },
        'UserName': { type: 'string', nullable: false },
        'Message': { type: 'string', nullable: false },
        'IsResponseItem': { type: 'boolean', nullable: false },
        'IsCurrent': { type: 'boolean', nullable: false },
    }
};
export var KendoModelSaveCriteriaGroupRequestDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Json': { type: 'string', nullable: false },
        'AdapterDetail': { type: 'enums.querycomposerquerytypes', nullable: true },
        'TemplateID': { type: 'any', nullable: true },
        'RequestTypeID': { type: 'any', nullable: true },
        'RequestID': { type: 'any', nullable: true },
    }
};
export var KendoModelUpdateRequestDataMartStatusDTO = {
    fields: {
        'RequestDataMartID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'NewStatus': { type: 'enums.routingstatus', nullable: false },
        'Message': { type: 'string', nullable: false },
    }
};
export var KendoModelUpdateRequestTypeModelsDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'DataModels': { type: 'any[]', nullable: false },
    }
};
export var KendoModelUpdateRequestTypeRequestDTO = {
    fields: {
        'RequestType': { type: 'any', nullable: false },
        'Permissions': { type: 'any[]', nullable: false },
        'Queries': { type: 'any[]', nullable: false },
        'Terms': { type: 'any[]', nullable: false },
        'NotAllowedTerms': { type: 'any[]', nullable: false },
        'Models': { type: 'any[]', nullable: false },
    }
};
export var KendoModelUpdateRequestTypeResponseDTO = {
    fields: {
        'RequestType': { type: 'any', nullable: false },
        'Queries': { type: 'any[]', nullable: false },
    }
};
export var KendoModelUpdateRequestTypeTermsDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'Terms': { type: 'any[]', nullable: false },
    }
};
export var KendoModelHomepageTaskRequestUserDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'TaskID': { type: 'any', nullable: false },
        'UserID': { type: 'any', nullable: false },
        'UserName': { type: 'string', nullable: false },
        'FirstName': { type: 'string', nullable: false },
        'LastName': { type: 'string', nullable: false },
        'WorkflowRoleID': { type: 'any', nullable: false },
        'WorkflowRole': { type: 'string', nullable: false },
    }
};
export var KendoModelHomepageTaskSummaryDTO = {
    fields: {
        'TaskID': { type: 'any', nullable: false },
        'TaskName': { type: 'string', nullable: false },
        'TaskStatus': { type: 'enums.taskstatuses', nullable: false },
        'TaskStatusText': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: true },
        'StartOn': { type: 'date', nullable: true },
        'EndOn': { type: 'date', nullable: true },
        'Type': { type: 'string', nullable: false },
        'DirectToRequest': { type: 'boolean', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Identifier': { type: 'string', nullable: false },
        'RequestID': { type: 'any', nullable: true },
        'MSRequestID': { type: 'string', nullable: false },
        'RequestStatus': { type: 'enums.requeststatuses', nullable: true },
        'RequestStatusText': { type: 'string', nullable: false },
        'NewUserID': { type: 'any', nullable: true },
        'AssignedResources': { type: 'string', nullable: false },
    }
};
export var KendoModelActivityDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Activities': { type: 'any[]', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ProjectID': { type: 'any', nullable: true },
        'DisplayOrder': { type: 'number', nullable: false },
        'TaskLevel': { type: 'number', nullable: false },
        'ParentActivityID': { type: 'any', nullable: true },
        'Acronym': { type: 'string', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
    }
};
export var KendoModelDataMartTypeDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
    }
};
export var KendoModelDataMartInstalledModelDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'ModelID': { type: 'any', nullable: false },
        'Model': { type: 'string', nullable: false },
        'Properties': { type: 'string', nullable: false },
    }
};
export var KendoModelDemographicDTO = {
    fields: {
        'Country': { type: 'string', nullable: false },
        'State': { type: 'string', nullable: false },
        'Town': { type: 'string', nullable: false },
        'Region': { type: 'string', nullable: false },
        'Gender': { type: 'string', nullable: false },
        'AgeGroup': { type: 'enums.agegroups', nullable: false },
        'Ethnicity': { type: 'enums.ethnicities', nullable: false },
        'Count': { type: 'number', nullable: false },
    }
};
export var KendoModelLookupListCategoryDTO = {
    fields: {
        'ListId': { type: 'enums.lists', nullable: false },
        'CategoryId': { type: 'number', nullable: false },
        'CategoryName': { type: 'string', nullable: false },
    }
};
export var KendoModelLookupListDetailRequestDTO = {
    fields: {
        'Codes': { type: 'string[]', nullable: false },
        'ListID': { type: 'enums.lists', nullable: false },
    }
};
export var KendoModelLookupListDTO = {
    fields: {
        'ListId': { type: 'enums.lists', nullable: false },
        'ListName': { type: 'string', nullable: false },
        'Version': { type: 'string', nullable: false },
    }
};
export var KendoModelLookupListValueDTO = {
    fields: {
        'ListId': { type: 'enums.lists', nullable: false },
        'CategoryId': { type: 'number', nullable: false },
        'ItemName': { type: 'string', nullable: false },
        'ItemCode': { type: 'string', nullable: false },
        'ItemCodeWithNoPeriod': { type: 'string', nullable: false },
        'ExpireDate': { type: 'date', nullable: true },
        'ID': { type: 'number', nullable: false },
    }
};
export var KendoModelProjectDataMartDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'Project': { type: 'string', nullable: false },
        'ProjectAcronym': { type: 'string', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'Organization': { type: 'string', nullable: false },
    }
};
export var KendoModelRegistryItemDefinitionDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'Category': { type: 'string', nullable: false },
        'Title': { type: 'string', nullable: false },
    }
};
export var KendoModelUpdateRegistryItemsDTO = {
    fields: {}
};
export var KendoModelWorkplanTypeDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'WorkplanTypeID': { type: 'number', nullable: false },
        'Name': { type: 'string', nullable: false },
        'NetworkID': { type: 'any', nullable: false },
        'Acronym': { type: 'string', nullable: false },
    }
};
export var KendoModelRequesterCenterDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'RequesterCenterID': { type: 'number', nullable: false },
        'Name': { type: 'string', nullable: false },
        'NetworkID': { type: 'any', nullable: false },
    }
};
export var KendoModelQueryTypeDTO = {
    fields: {
        'ID': { type: 'number', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
    }
};
export var KendoModelSecurityTupleDTO = {
    fields: {
        'ID1': { type: 'any', nullable: false },
        'ID2': { type: 'any', nullable: true },
        'ID3': { type: 'any', nullable: true },
        'ID4': { type: 'any', nullable: true },
        'SubjectID': { type: 'any', nullable: false },
        'PrivilegeID': { type: 'any', nullable: false },
        'ViaMembership': { type: 'number', nullable: false },
        'DeniedEntries': { type: 'number', nullable: false },
        'ExplicitDeniedEntries': { type: 'number', nullable: false },
        'ExplicitAllowedEntries': { type: 'number', nullable: false },
        'ChangedOn': { type: 'date', nullable: false },
    }
};
export var KendoModelUpdateUserSecurityGroupsDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'Groups': { type: 'any[]', nullable: false },
    }
};
export var KendoModelDesignDTO = {
    fields: {
        'Locked': { type: 'boolean', nullable: false },
    }
};
export var KendoModelCodeSelectorValueDTO = {
    fields: {
        'Code': { type: 'string', nullable: false },
        'Name': { type: 'string', nullable: false },
        'ExpireDate': { type: 'date', nullable: true },
    }
};
export var KendoModelThemeDTO = {
    fields: {
        'Title': { type: 'string', nullable: false },
        'Terms': { type: 'string', nullable: false },
        'Info': { type: 'string', nullable: false },
        'Resources': { type: 'string', nullable: false },
        'Footer': { type: 'string', nullable: false },
        'LogoImage': { type: 'string', nullable: false },
        'SystemUserConfirmationTitle': { type: 'string', nullable: false },
        'SystemUserConfirmationContent': { type: 'string', nullable: false },
        'ContactUsHref': { type: 'string', nullable: false },
    }
};
export var KendoModelAssignedUserNotificationDTO = {
    fields: {
        'Event': { type: 'string', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Level': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
    }
};
export var KendoModelMetadataEditPermissionsSummaryDTO = {
    fields: {
        'CanEditRequestMetadata': { type: 'boolean', nullable: false },
        'EditableDataMarts': { type: 'any[]', nullable: false },
    }
};
export var KendoModelNotificationDTO = {
    fields: {
        'Timestamp': { type: 'date', nullable: false },
        'Event': { type: 'string', nullable: false },
        'Message': { type: 'string', nullable: false },
    }
};
export var KendoModelForgotPasswordDTO = {
    fields: {
        'UserName': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
    }
};
export var KendoModelLoginDTO = {
    fields: {
        'UserName': { type: 'string', nullable: false },
        'Password': { type: 'string', nullable: false },
        'RememberMe': { type: 'boolean', nullable: false },
        'IPAddress': { type: 'string', nullable: false },
        'Enviorment': { type: 'string', nullable: false },
    }
};
export var KendoModelMenuItemDTO = {
    fields: {
        'text': { type: 'string', nullable: false },
        'url': { type: 'string', nullable: false },
        'encoded': { type: 'boolean', nullable: false },
        'content': { type: 'string', nullable: false },
        'items': { type: 'any[]', nullable: false },
    }
};
export var KendoModelObserverDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'DisplayName': { type: 'string', nullable: false },
        'DisplayNameWithType': { type: 'string', nullable: false },
        'ObserverType': { type: 'enums.observertypes', nullable: false },
    }
};
export var KendoModelObserverEventDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
    }
};
export var KendoModelRestorePasswordDTO = {
    fields: {
        'PasswordRestoreToken': { type: 'any', nullable: false },
        'Password': { type: 'string', nullable: false },
    }
};
export var KendoModelTreeItemDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'Name': { type: 'string', nullable: false },
        'Path': { type: 'string', nullable: false },
        'Type': { type: 'number', nullable: false },
        'SubItems': { type: 'any[]', nullable: false },
        'HasChildren': { type: 'boolean', nullable: false },
    }
};
export var KendoModelUpdateUserPasswordDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'Password': { type: 'string', nullable: false },
    }
};
export var KendoModelUserAuthenticationDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'UserID': { type: 'any', nullable: false },
        'Success': { type: 'boolean', nullable: false },
        'Description': { type: 'string', nullable: false },
        'IPAddress': { type: 'string', nullable: false },
        'Environment': { type: 'string', nullable: false },
        'Source': { type: 'string', nullable: false },
        'Details': { type: 'string', nullable: false },
        'DMCVersion': { type: 'string', nullable: false },
        'DateTime': { type: 'date', nullable: false },
    }
};
export var KendoModelUserRegistrationDTO = {
    fields: {
        'UserName': { type: 'string', nullable: false },
        'Password': { type: 'string', nullable: false },
        'Title': { type: 'string', nullable: false },
        'FirstName': { type: 'string', nullable: false },
        'LastName': { type: 'string', nullable: false },
        'MiddleName': { type: 'string', nullable: false },
        'Phone': { type: 'string', nullable: false },
        'Fax': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
        'Active': { type: 'boolean', nullable: false },
        'SignedUpOn': { type: 'date', nullable: true },
        'OrganizationRequested': { type: 'string', nullable: false },
        'RoleRequested': { type: 'string', nullable: false },
    }
};
export var KendoModelDataMartRegistrationResultDTO = {
    fields: {
        'DataMarts': { type: 'any[]', nullable: false },
        'DataMartModels': { type: 'any[]', nullable: false },
        'Users': { type: 'any[]', nullable: false },
        'ResearchOrganization': { type: 'any', nullable: false },
        'ProviderOrganization': { type: 'any', nullable: false },
    }
};
export var KendoModelGetChangeRequestDTO = {
    fields: {
        'LastChecked': { type: 'date', nullable: false },
        'ProviderIDs': { type: 'any[]', nullable: false },
    }
};
export var KendoModelRegisterDataMartDTO = {
    fields: {
        'Password': { type: 'string', nullable: false },
        'Token': { type: 'string', nullable: false },
    }
};
export var KendoModelRequestDocumentDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'FileName': { type: 'string', nullable: false },
        'MimeType': { type: 'string', nullable: false },
        'Viewable': { type: 'boolean', nullable: false },
        'ItemID': { type: 'any', nullable: false },
    }
};
export var KendoModelUpdateResponseStatusRequestDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'ResponseID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'ProjectID': { type: 'any', nullable: false },
        'OrganizationID': { type: 'any', nullable: false },
        'UserID': { type: 'any', nullable: false },
        'StatusID': { type: 'enums.routingstatus', nullable: false },
        'Message': { type: 'string', nullable: false },
        'RejectReason': { type: 'string', nullable: false },
        'HoldReason': { type: 'string', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'RequestTypeName': { type: 'string', nullable: false },
    }
};
export var KendoModelWbdChangeSetDTO = {
    fields: {
        'Requests': { type: 'any[]', nullable: false },
        'Projects': { type: 'any[]', nullable: false },
        'DataMarts': { type: 'any[]', nullable: false },
        'DataMartModels': { type: 'any[]', nullable: false },
        'RequestDataMarts': { type: 'any[]', nullable: false },
        'ProjectDataMarts': { type: 'any[]', nullable: false },
        'Organizations': { type: 'any[]', nullable: false },
        'Documents': { type: 'any[]', nullable: false },
        'Users': { type: 'any[]', nullable: false },
        'Responses': { type: 'any[]', nullable: false },
        'SecurityGroups': { type: 'any[]', nullable: false },
        'RequestResponseSecurityACLs': { type: 'any[]', nullable: false },
        'DataMartSecurityACLs': { type: 'any[]', nullable: false },
        'ManageWbdACLs': { type: 'any[]', nullable: false },
    }
};
export var KendoModelCommonResponseDetailDTO = {
    fields: {
        'RequestDataMarts': { type: 'any[]', nullable: false },
        'Responses': { type: 'any[]', nullable: false },
        'Documents': { type: 'any[]', nullable: false },
        'CanViewPendingApprovalResponses': { type: 'boolean', nullable: false },
        'ExportForFileDistribution': { type: 'boolean', nullable: false },
    }
};
export var KendoModelPrepareSpecificationDTO = {
    fields: {}
};
export var KendoModelRequestFormDTO = {
    fields: {
        'RequestDueDate': { type: 'date', nullable: true },
        'ContactInfo': { type: 'string', nullable: false },
        'RequestingTeam': { type: 'string', nullable: false },
        'FDAReview': { type: 'string', nullable: false },
        'FDADivisionNA': { type: 'boolean', nullable: false },
        'FDADivisionDAAAP': { type: 'boolean', nullable: false },
        'FDADivisionDBRUP': { type: 'boolean', nullable: false },
        'FDADivisionDCARP': { type: 'boolean', nullable: false },
        'FDADivisionDDDP': { type: 'boolean', nullable: false },
        'FDADivisionDGIEP': { type: 'boolean', nullable: false },
        'FDADivisionDMIP': { type: 'boolean', nullable: false },
        'FDADivisionDMEP': { type: 'boolean', nullable: false },
        'FDADivisionDNP': { type: 'boolean', nullable: false },
        'FDADivisionDDP': { type: 'boolean', nullable: false },
        'FDADivisionDPARP': { type: 'boolean', nullable: false },
        'FDADivisionOther': { type: 'boolean', nullable: false },
        'QueryLevel': { type: 'string', nullable: false },
        'AdjustmentMethod': { type: 'string', nullable: false },
        'CohortID': { type: 'string', nullable: false },
        'StudyObjectives': { type: 'string', nullable: false },
        'RequestStartDate': { type: 'date', nullable: true },
        'RequestEndDate': { type: 'date', nullable: true },
        'AgeGroups': { type: 'string', nullable: false },
        'CoverageTypes': { type: 'string', nullable: false },
        'EnrollmentGap': { type: 'string', nullable: false },
        'EnrollmentExposure': { type: 'string', nullable: false },
        'DefineExposures': { type: 'string', nullable: false },
        'WashoutPeirod': { type: 'string', nullable: false },
        'OtherExposures': { type: 'string', nullable: false },
        'OneOrManyExposures': { type: 'string', nullable: false },
        'AdditionalInclusion': { type: 'string', nullable: false },
        'AdditionalInclusionEvaluation': { type: 'string', nullable: false },
        'AdditionalExclusion': { type: 'string', nullable: false },
        'AdditionalExclusionEvaluation': { type: 'string', nullable: false },
        'VaryWashoutPeirod': { type: 'string', nullable: false },
        'VaryExposures': { type: 'string', nullable: false },
        'DefineExposures1': { type: 'string', nullable: false },
        'DefineExposures2': { type: 'string', nullable: false },
        'DefineExposures3': { type: 'string', nullable: false },
        'DefineExposures4': { type: 'string', nullable: false },
        'DefineExposures5': { type: 'string', nullable: false },
        'DefineExposures6': { type: 'string', nullable: false },
        'DefineExposures7': { type: 'string', nullable: false },
        'DefineExposures8': { type: 'string', nullable: false },
        'DefineExposures9': { type: 'string', nullable: false },
        'DefineExposures10': { type: 'string', nullable: false },
        'DefineExposures11': { type: 'string', nullable: false },
        'DefineExposures12': { type: 'string', nullable: false },
        'WashoutPeriod1': { type: 'number', nullable: true },
        'WashoutPeriod2': { type: 'number', nullable: true },
        'WashoutPeriod3': { type: 'number', nullable: true },
        'WashoutPeriod4': { type: 'number', nullable: true },
        'WashoutPeriod5': { type: 'number', nullable: true },
        'WashoutPeriod6': { type: 'number', nullable: true },
        'WashoutPeriod7': { type: 'number', nullable: true },
        'WashoutPeriod8': { type: 'number', nullable: true },
        'WashoutPeriod9': { type: 'number', nullable: true },
        'WashoutPeriod10': { type: 'number', nullable: true },
        'WashoutPeriod11': { type: 'number', nullable: true },
        'WashoutPeriod12': { type: 'number', nullable: true },
        'IncidenceRefinement1': { type: 'string', nullable: false },
        'IncidenceRefinement2': { type: 'string', nullable: false },
        'IncidenceRefinement3': { type: 'string', nullable: false },
        'IncidenceRefinement4': { type: 'string', nullable: false },
        'IncidenceRefinement5': { type: 'string', nullable: false },
        'IncidenceRefinement6': { type: 'string', nullable: false },
        'IncidenceRefinement7': { type: 'string', nullable: false },
        'IncidenceRefinement8': { type: 'string', nullable: false },
        'IncidenceRefinement9': { type: 'string', nullable: false },
        'IncidenceRefinement10': { type: 'string', nullable: false },
        'IncidenceRefinement11': { type: 'string', nullable: false },
        'IncidenceRefinement12': { type: 'string', nullable: false },
        'SpecifyExposedTimeAssessment1': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment2': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment3': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment4': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment5': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment6': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment7': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment8': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment9': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment10': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment11': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'SpecifyExposedTimeAssessment12': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'EpisodeAllowableGap1': { type: 'number', nullable: true },
        'EpisodeAllowableGap2': { type: 'number', nullable: true },
        'EpisodeAllowableGap3': { type: 'number', nullable: true },
        'EpisodeAllowableGap4': { type: 'number', nullable: true },
        'EpisodeAllowableGap5': { type: 'number', nullable: true },
        'EpisodeAllowableGap6': { type: 'number', nullable: true },
        'EpisodeAllowableGap7': { type: 'number', nullable: true },
        'EpisodeAllowableGap8': { type: 'number', nullable: true },
        'EpisodeAllowableGap9': { type: 'number', nullable: true },
        'EpisodeAllowableGap10': { type: 'number', nullable: true },
        'EpisodeAllowableGap11': { type: 'number', nullable: true },
        'EpisodeAllowableGap12': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod1': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod2': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod3': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod4': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod5': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod6': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod7': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod8': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod9': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod10': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod11': { type: 'number', nullable: true },
        'EpisodeExtensionPeriod12': { type: 'number', nullable: true },
        'MinimumEpisodeDuration1': { type: 'number', nullable: true },
        'MinimumEpisodeDuration2': { type: 'number', nullable: true },
        'MinimumEpisodeDuration3': { type: 'number', nullable: true },
        'MinimumEpisodeDuration4': { type: 'number', nullable: true },
        'MinimumEpisodeDuration5': { type: 'number', nullable: true },
        'MinimumEpisodeDuration6': { type: 'number', nullable: true },
        'MinimumEpisodeDuration7': { type: 'number', nullable: true },
        'MinimumEpisodeDuration8': { type: 'number', nullable: true },
        'MinimumEpisodeDuration9': { type: 'number', nullable: true },
        'MinimumEpisodeDuration10': { type: 'number', nullable: true },
        'MinimumEpisodeDuration11': { type: 'number', nullable: true },
        'MinimumEpisodeDuration12': { type: 'number', nullable: true },
        'MinimumDaysSupply1': { type: 'number', nullable: true },
        'MinimumDaysSupply2': { type: 'number', nullable: true },
        'MinimumDaysSupply3': { type: 'number', nullable: true },
        'MinimumDaysSupply4': { type: 'number', nullable: true },
        'MinimumDaysSupply5': { type: 'number', nullable: true },
        'MinimumDaysSupply6': { type: 'number', nullable: true },
        'MinimumDaysSupply7': { type: 'number', nullable: true },
        'MinimumDaysSupply8': { type: 'number', nullable: true },
        'MinimumDaysSupply9': { type: 'number', nullable: true },
        'MinimumDaysSupply10': { type: 'number', nullable: true },
        'MinimumDaysSupply11': { type: 'number', nullable: true },
        'MinimumDaysSupply12': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration1': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration2': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration3': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration4': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration5': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration6': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration7': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration8': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration9': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration10': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration11': { type: 'number', nullable: true },
        'SpecifyFollowUpDuration12': { type: 'number', nullable: true },
        'AllowOnOrMultipleExposureEpisodes1': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes2': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes3': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes4': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes5': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes6': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes7': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes8': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes9': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes10': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes11': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'AllowOnOrMultipleExposureEpisodes12': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'TruncateExposedtime1': { type: 'boolean', nullable: false },
        'TruncateExposedtime2': { type: 'boolean', nullable: false },
        'TruncateExposedtime3': { type: 'boolean', nullable: false },
        'TruncateExposedtime4': { type: 'boolean', nullable: false },
        'TruncateExposedtime5': { type: 'boolean', nullable: false },
        'TruncateExposedtime6': { type: 'boolean', nullable: false },
        'TruncateExposedtime7': { type: 'boolean', nullable: false },
        'TruncateExposedtime8': { type: 'boolean', nullable: false },
        'TruncateExposedtime9': { type: 'boolean', nullable: false },
        'TruncateExposedtime10': { type: 'boolean', nullable: false },
        'TruncateExposedtime11': { type: 'boolean', nullable: false },
        'TruncateExposedtime12': { type: 'boolean', nullable: false },
        'TruncateExposedTimeSpecified1': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified2': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified3': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified4': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified5': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified6': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified7': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified8': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified9': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified10': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified11': { type: 'string', nullable: false },
        'TruncateExposedTimeSpecified12': { type: 'string', nullable: false },
        'SpecifyBlackoutPeriod1': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod2': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod3': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod4': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod5': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod6': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod7': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod8': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod9': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod10': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod11': { type: 'number', nullable: true },
        'SpecifyBlackoutPeriod12': { type: 'number', nullable: true },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup14': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup15': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup16': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup14': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup15': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup16': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup24': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup25': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup26': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup24': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup25': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup26': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup34': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup35': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup36': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup34': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup35': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup36': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup44': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup45': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup46': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup44': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup45': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup46': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup54': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup55': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup56': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup54': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup55': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup56': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup64': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup65': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup66': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup64': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup65': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup66': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup71': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup72': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup73': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup74': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup75': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup76': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup71': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup72': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup73': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup74': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup75': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup76': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup81': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup82': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup83': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup84': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup85': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup86': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup81': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup82': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup83': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup84': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup85': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup86': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup91': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup92': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup93': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup94': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup95': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup96': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup91': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup92': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup93': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup94': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup95': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup96': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup101': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup102': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup103': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup104': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup105': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup106': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup101': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup102': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup103': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup104': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup105': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup106': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup111': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup112': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup113': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup114': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup115': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup116': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup111': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup112': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup113': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup114': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup115': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup116': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup121': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup122': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup123': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup124': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup125': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionInclusionCriteriaGroup126': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup121': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup122': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup123': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup124': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup125': { type: 'string', nullable: false },
        'SpecifyAdditionalInclusionEvaluationWindowGroup126': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup14': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup15': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup16': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup14': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup15': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup16': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup24': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup25': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup26': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup24': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup25': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup26': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup34': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup35': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup36': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup34': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup35': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup36': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup44': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup45': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup46': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup44': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup45': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup46': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup54': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup55': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup56': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup54': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup55': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup56': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup64': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup65': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup66': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup64': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup65': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup66': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup71': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup72': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup73': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup74': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup75': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup76': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup71': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup72': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup73': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup74': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup75': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup76': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup81': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup82': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup83': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup84': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup85': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup86': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup81': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup82': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup83': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup84': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup85': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup86': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup91': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup92': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup93': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup94': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup95': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup96': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup91': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup92': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup93': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup94': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup95': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup96': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup101': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup102': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup103': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup104': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup105': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup106': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup101': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup102': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup103': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup104': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup105': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup106': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup111': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup112': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup113': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup114': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup115': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup116': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup111': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup112': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup113': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup114': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup115': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup116': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup121': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup122': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup123': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup124': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup125': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionInclusionCriteriaGroup126': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup121': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup122': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup123': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup124': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup125': { type: 'string', nullable: false },
        'SpecifyAdditionalExclusionEvaluationWindowGroup126': { type: 'string', nullable: false },
        'LookBackPeriodGroup1': { type: 'number', nullable: true },
        'LookBackPeriodGroup2': { type: 'number', nullable: true },
        'LookBackPeriodGroup3': { type: 'number', nullable: true },
        'LookBackPeriodGroup4': { type: 'number', nullable: true },
        'LookBackPeriodGroup5': { type: 'number', nullable: true },
        'LookBackPeriodGroup6': { type: 'number', nullable: true },
        'LookBackPeriodGroup7': { type: 'number', nullable: true },
        'LookBackPeriodGroup8': { type: 'number', nullable: true },
        'LookBackPeriodGroup9': { type: 'number', nullable: true },
        'LookBackPeriodGroup10': { type: 'number', nullable: true },
        'LookBackPeriodGroup11': { type: 'number', nullable: true },
        'LookBackPeriodGroup12': { type: 'number', nullable: true },
        'IncludeIndexDate1': { type: 'boolean', nullable: false },
        'IncludeIndexDate2': { type: 'boolean', nullable: false },
        'IncludeIndexDate3': { type: 'boolean', nullable: false },
        'IncludeIndexDate4': { type: 'boolean', nullable: false },
        'IncludeIndexDate5': { type: 'boolean', nullable: false },
        'IncludeIndexDate6': { type: 'boolean', nullable: false },
        'IncludeIndexDate7': { type: 'boolean', nullable: false },
        'IncludeIndexDate8': { type: 'boolean', nullable: false },
        'IncludeIndexDate9': { type: 'boolean', nullable: false },
        'IncludeIndexDate10': { type: 'boolean', nullable: false },
        'IncludeIndexDate11': { type: 'boolean', nullable: false },
        'IncludeIndexDate12': { type: 'boolean', nullable: false },
        'StratificationCategories1': { type: 'string', nullable: false },
        'StratificationCategories2': { type: 'string', nullable: false },
        'StratificationCategories3': { type: 'string', nullable: false },
        'StratificationCategories4': { type: 'string', nullable: false },
        'StratificationCategories5': { type: 'string', nullable: false },
        'StratificationCategories6': { type: 'string', nullable: false },
        'StratificationCategories7': { type: 'string', nullable: false },
        'StratificationCategories8': { type: 'string', nullable: false },
        'StratificationCategories9': { type: 'string', nullable: false },
        'StratificationCategories10': { type: 'string', nullable: false },
        'StratificationCategories11': { type: 'string', nullable: false },
        'StratificationCategories12': { type: 'string', nullable: false },
        'TwelveSpecifyLoopBackPeriod1': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod2': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod3': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod4': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod5': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod6': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod7': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod8': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod9': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod10': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod11': { type: 'number', nullable: true },
        'TwelveSpecifyLoopBackPeriod12': { type: 'number', nullable: true },
        'TwelveIncludeIndexDate1': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate2': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate3': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate4': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate5': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate6': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate7': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate8': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate9': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate10': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate11': { type: 'boolean', nullable: false },
        'TwelveIncludeIndexDate12': { type: 'boolean', nullable: false },
        'CareSettingsToDefineMedicalVisits1': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits2': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits3': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits4': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits5': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits6': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits7': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits8': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits9': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits10': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits11': { type: 'string', nullable: false },
        'CareSettingsToDefineMedicalVisits12': { type: 'string', nullable: false },
        'TwelveStratificationCategories1': { type: 'string', nullable: false },
        'TwelveStratificationCategories2': { type: 'string', nullable: false },
        'TwelveStratificationCategories3': { type: 'string', nullable: false },
        'TwelveStratificationCategories4': { type: 'string', nullable: false },
        'TwelveStratificationCategories5': { type: 'string', nullable: false },
        'TwelveStratificationCategories6': { type: 'string', nullable: false },
        'TwelveStratificationCategories7': { type: 'string', nullable: false },
        'TwelveStratificationCategories8': { type: 'string', nullable: false },
        'TwelveStratificationCategories9': { type: 'string', nullable: false },
        'TwelveStratificationCategories10': { type: 'string', nullable: false },
        'TwelveStratificationCategories11': { type: 'string', nullable: false },
        'TwelveStratificationCategories12': { type: 'string', nullable: false },
        'VaryLengthOfWashoutPeriod1': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod2': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod3': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod4': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod5': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod6': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod7': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod8': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod9': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod10': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod11': { type: 'number', nullable: true },
        'VaryLengthOfWashoutPeriod12': { type: 'number', nullable: true },
        'VaryUserExposedTime1': { type: 'boolean', nullable: false },
        'VaryUserExposedTime2': { type: 'boolean', nullable: false },
        'VaryUserExposedTime3': { type: 'boolean', nullable: false },
        'VaryUserExposedTime4': { type: 'boolean', nullable: false },
        'VaryUserExposedTime5': { type: 'boolean', nullable: false },
        'VaryUserExposedTime6': { type: 'boolean', nullable: false },
        'VaryUserExposedTime7': { type: 'boolean', nullable: false },
        'VaryUserExposedTime8': { type: 'boolean', nullable: false },
        'VaryUserExposedTime9': { type: 'boolean', nullable: false },
        'VaryUserExposedTime10': { type: 'boolean', nullable: false },
        'VaryUserExposedTime11': { type: 'boolean', nullable: false },
        'VaryUserExposedTime12': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration1': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration2': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration3': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration4': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration5': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration6': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration7': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration8': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration9': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration10': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration11': { type: 'boolean', nullable: false },
        'VaryUserFollowupPeriodDuration12': { type: 'boolean', nullable: false },
        'VaryBlackoutPeriodPeriod1': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod2': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod3': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod4': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod5': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod6': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod7': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod8': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod9': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod10': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod11': { type: 'number', nullable: true },
        'VaryBlackoutPeriodPeriod12': { type: 'number', nullable: true },
        'Level2or3DefineExposures1Exposure': { type: 'string', nullable: false },
        'Level2or3DefineExposures1Compare': { type: 'string', nullable: false },
        'Level2or3DefineExposures2Exposure': { type: 'string', nullable: false },
        'Level2or3DefineExposures2Compare': { type: 'string', nullable: false },
        'Level2or3DefineExposures3Exposure': { type: 'string', nullable: false },
        'Level2or3DefineExposures3Compare': { type: 'string', nullable: false },
        'Level2or3WashoutPeriod1Exposure': { type: 'number', nullable: true },
        'Level2or3WashoutPeriod1Compare': { type: 'number', nullable: true },
        'Level2or3WashoutPeriod2Exposure': { type: 'number', nullable: true },
        'Level2or3WashoutPeriod2Compare': { type: 'number', nullable: true },
        'Level2or3WashoutPeriod3Exposure': { type: 'number', nullable: true },
        'Level2or3WashoutPeriod3Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment1Exposure': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment1Compare': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment2Exposure': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment2Compare': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment3Exposure': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3SpecifyExposedTimeAssessment3Compare': { type: 'enums.workflowmpspecifyexposedtimeassessments', nullable: true },
        'Level2or3EpisodeAllowableGap1Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeAllowableGap1Compare': { type: 'number', nullable: true },
        'Level2or3EpisodeAllowableGap2Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeAllowableGap2Compare': { type: 'number', nullable: true },
        'Level2or3EpisodeAllowableGap3Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeAllowableGap3Compare': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod1Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod1Compare': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod2Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod2Compare': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod3Exposure': { type: 'number', nullable: true },
        'Level2or3EpisodeExtensionPeriod3Compare': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration1Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration1Compare': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration2Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration2Compare': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration3Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumEpisodeDuration3Compare': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply1Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply1Compare': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply2Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply2Compare': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply3Exposure': { type: 'number', nullable: true },
        'Level2or3MinimumDaysSupply3Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration1Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration1Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration2Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration2Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration3Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyFollowUpDuration3Compare': { type: 'number', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes1Exposure': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes1Compare': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes2Exposure': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes2Compare': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes3Exposure': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3AllowOnOrMultipleExposureEpisodes3Compare': { type: 'enums.workflowmpallowonormultipleexposureepisodes', nullable: true },
        'Level2or3TruncateExposedtime1Exposure': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedtime1Compare': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedtime2Exposure': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedtime2Compare': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedtime3Exposure': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedtime3Compare': { type: 'boolean', nullable: false },
        'Level2or3TruncateExposedTimeSpecified1Exposure': { type: 'string', nullable: false },
        'Level2or3TruncateExposedTimeSpecified1Compare': { type: 'string', nullable: false },
        'Level2or3TruncateExposedTimeSpecified2Exposure': { type: 'string', nullable: false },
        'Level2or3TruncateExposedTimeSpecified2Compare': { type: 'string', nullable: false },
        'Level2or3TruncateExposedTimeSpecified3Exposure': { type: 'string', nullable: false },
        'Level2or3TruncateExposedTimeSpecified3Compare': { type: 'string', nullable: false },
        'Level2or3SpecifyBlackoutPeriod1Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyBlackoutPeriod1Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyBlackoutPeriod2Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyBlackoutPeriod2Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyBlackoutPeriod3Exposure': { type: 'number', nullable: true },
        'Level2or3SpecifyBlackoutPeriod3Compare': { type: 'number', nullable: true },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalInclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup11': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup12': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup13': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup11': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup12': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup13': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup21': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup22': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup23': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup21': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup22': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup23': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup31': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup32': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup33': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup31': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup32': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup33': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup41': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup42': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup43': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup41': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup42': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup43': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup51': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup52': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup53': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup51': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup52': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup53': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup61': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup62': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionInclusionCriteriaGroup63': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup61': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup62': { type: 'string', nullable: false },
        'Level2or3SpecifyAdditionalExclusionEvaluationWindowGroup63': { type: 'string', nullable: false },
        'Level2or3VaryLengthOfWashoutPeriod1Exposure': { type: 'number', nullable: true },
        'Level2or3VaryLengthOfWashoutPeriod1Compare': { type: 'number', nullable: true },
        'Level2or3VaryLengthOfWashoutPeriod2Exposure': { type: 'number', nullable: true },
        'Level2or3VaryLengthOfWashoutPeriod2Compare': { type: 'number', nullable: true },
        'Level2or3VaryLengthOfWashoutPeriod3Exposure': { type: 'number', nullable: true },
        'Level2or3VaryLengthOfWashoutPeriod3Compare': { type: 'number', nullable: true },
        'Level2or3VaryUserExposedTime1Exposure': { type: 'boolean', nullable: false },
        'Level2or3VaryUserExposedTime1Compare': { type: 'boolean', nullable: false },
        'Level2or3VaryUserExposedTime2Exposure': { type: 'boolean', nullable: false },
        'Level2or3VaryUserExposedTime2Compare': { type: 'boolean', nullable: false },
        'Level2or3VaryUserExposedTime3Exposure': { type: 'boolean', nullable: false },
        'Level2or3VaryUserExposedTime3Compare': { type: 'boolean', nullable: false },
        'Level2or3VaryBlackoutPeriodPeriod1Exposure': { type: 'number', nullable: true },
        'Level2or3VaryBlackoutPeriodPeriod1Compare': { type: 'number', nullable: true },
        'Level2or3VaryBlackoutPeriodPeriod2Exposure': { type: 'number', nullable: true },
        'Level2or3VaryBlackoutPeriodPeriod2Compare': { type: 'number', nullable: true },
        'Level2or3VaryBlackoutPeriodPeriod3Exposure': { type: 'number', nullable: true },
        'Level2or3VaryBlackoutPeriodPeriod3Compare': { type: 'number', nullable: true },
        'OutcomeList': { type: 'any[]', nullable: false },
        'AgeCovariate': { type: 'string', nullable: false },
        'SexCovariate': { type: 'string', nullable: false },
        'TimeCovariate': { type: 'string', nullable: false },
        'YearCovariate': { type: 'string', nullable: false },
        'ComorbidityCovariate': { type: 'string', nullable: false },
        'HealthCovariate': { type: 'string', nullable: false },
        'DrugCovariate': { type: 'string', nullable: false },
        'CovariateList': { type: 'any[]', nullable: false },
        'hdPSAnalysis': { type: 'string', nullable: false },
        'InclusionCovariates': { type: 'number', nullable: false },
        'PoolCovariates': { type: 'number', nullable: false },
        'SelectionCovariates': { type: 'string', nullable: false },
        'ZeroCellCorrection': { type: 'string', nullable: false },
        'MatchingRatio': { type: 'string', nullable: false },
        'MatchingCalipers': { type: 'string', nullable: false },
        'VaryMatchingRatio': { type: 'string', nullable: false },
        'VaryMatchingCalipers': { type: 'string', nullable: false },
    }
};
export var KendoModelOutcomeItemDTO = {
    fields: {
        'CommonName': { type: 'string', nullable: false },
        'Outcome': { type: 'string', nullable: false },
        'WashoutPeriod': { type: 'string', nullable: false },
        'VaryWashoutPeriod': { type: 'string', nullable: false },
    }
};
export var KendoModelCovariateItemDTO = {
    fields: {
        'GroupingIndicator': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'CodeType': { type: 'string', nullable: false },
        'Ingredients': { type: 'string', nullable: false },
        'SubGroupAnalysis': { type: 'string', nullable: false },
    }
};
export var KendoModelWorkflowHistoryItemDTO = {
    fields: {
        'TaskID': { type: 'any', nullable: false },
        'TaskName': { type: 'string', nullable: false },
        'UserID': { type: 'any', nullable: false },
        'UserName': { type: 'string', nullable: false },
        'UserFullName': { type: 'string', nullable: false },
        'Message': { type: 'string', nullable: false },
        'Date': { type: 'date', nullable: false },
        'RoutingID': { type: 'any', nullable: true },
        'DataMart': { type: 'string', nullable: false },
        'WorkflowActivityID': { type: 'any', nullable: true },
    }
};
export var KendoModelLegacySchedulerRequestDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: true },
        'AdapterPackageVersion': { type: 'string', nullable: false },
        'ScheduleJSON': { type: 'string', nullable: false },
    }
};
export var KendoModelAvailableTermsRequestDTO = {
    fields: {
        'Adapters': { type: 'any[]', nullable: false },
        'QueryType': { type: 'enums.querycomposerquerytypes', nullable: true },
    }
};
export var KendoModelDistributedRegressionManifestFile = {
    fields: {
        'Items': { type: 'any[]', nullable: false },
        'DataPartners': { type: 'any[]', nullable: false },
    }
};
export var KendoModelDistributedRegressionAnalysisCenterManifestItem = {
    fields: {
        'DocumentID': { type: 'any', nullable: false },
        'RevisionSetID': { type: 'any', nullable: false },
        'ResponseID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'DataPartnerIdentifier': { type: 'string', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'RequestDataMartID': { type: 'any', nullable: false },
    }
};
export var KendoModelDistributedRegressionManifestDataPartner = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'RouteType': { type: 'enums.routingtype', nullable: false },
        'DataMartIdentifier': { type: 'string', nullable: false },
        'DataMartCode': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerQueryDTO = {
    fields: {
        'Header': { type: 'any', nullable: false },
        'Where': { type: 'any', nullable: false },
        'Select': { type: 'any', nullable: false },
        'TemporalEvents': { type: 'any[]', nullable: false },
    }
};
export var KendoModelQueryComposerResponseAggregationDefinitionDTO = {
    fields: {
        'GroupBy': { type: 'string[]', nullable: false },
        'Select': { type: 'any[]', nullable: false },
        'Name': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerResponseHeaderDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'RequestID': { type: 'any', nullable: true },
        'DocumentID': { type: 'any', nullable: true },
        'QueryingStart': { type: 'date', nullable: true },
        'QueryingEnd': { type: 'date', nullable: true },
        'DataMart': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerResponsePropertyDefinitionDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Type': { type: 'string', nullable: false },
        'As': { type: 'string', nullable: false },
        'Aggregate': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerResponseQueryResultDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'Name': { type: 'string', nullable: false },
        'QueryStart': { type: 'date', nullable: true },
        'QueryEnd': { type: 'date', nullable: true },
        'PostProcessStart': { type: 'date', nullable: true },
        'PostProcessEnd': { type: 'date', nullable: true },
        'Errors': { type: 'any[]', nullable: false },
        'Results': { type: 'any[]', nullable: false },
        'LowCellThrehold': { type: 'number', nullable: true },
        'Properties': { type: 'any[]', nullable: false },
        'Aggregation': { type: 'any', nullable: false },
    }
};
export var KendoModelQueryComposerTemporalEventDTO = {
    fields: {
        'IndexEventDateIdentifier': { type: 'string', nullable: false },
        'DaysBefore': { type: 'number', nullable: false },
        'DaysAfter': { type: 'number', nullable: false },
        'Criteria': { type: 'any[]', nullable: false },
    }
};
export var KendoModelSectionSpecificTermDTO = {
    fields: {
        'TemplateID': { type: 'any', nullable: false },
        'TermID': { type: 'any', nullable: false },
        'Section': { type: 'enums.querycomposersections', nullable: false },
    }
};
export var KendoModelTemplateTermDTO = {
    fields: {
        'TemplateID': { type: 'any', nullable: false },
        'Template': { type: 'any', nullable: false },
        'TermID': { type: 'any', nullable: false },
        'Term': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: false },
        'Section': { type: 'enums.querycomposersections', nullable: false },
    }
};
export var KendoModelMatchingCriteriaDTO = {
    fields: {
        'TermIDs': { type: 'any[]', nullable: false },
        'ProjectID': { type: 'any', nullable: true },
        'Request': { type: 'string', nullable: false },
        'RequestID': { type: 'any', nullable: true },
    }
};
export var KendoModelQueryComposerCriteriaDTO = {
    fields: {
        'ID': { type: 'any', nullable: true },
        'RelatedToID': { type: 'any', nullable: true },
        'Name': { type: 'string', nullable: false },
        'Operator': { type: 'enums.querycomposeroperators', nullable: false },
        'IndexEvent': { type: 'boolean', nullable: false },
        'Exclusion': { type: 'boolean', nullable: false },
        'Criteria': { type: 'any[]', nullable: false },
        'Terms': { type: 'any[]', nullable: false },
        'Type': { type: 'enums.querycomposercriteriatypes', nullable: false },
    }
};
export var KendoModelQueryComposerFieldDTO = {
    fields: {
        'FieldName': { type: 'string', nullable: false },
        'Type': { type: 'any', nullable: false },
        'GroupBy': { type: 'any', nullable: false },
        'StratifyBy': { type: 'any', nullable: false },
        'Aggregate': { type: 'enums.querycomposeraggregates', nullable: true },
        'Select': { type: 'any[]', nullable: false },
        'OrderBy': { type: 'enums.orderbydirections', nullable: false },
    }
};
export var KendoModelQueryComposerGroupByDTO = {
    fields: {
        'Field': { type: 'string', nullable: false },
        'Aggregate': { type: 'enums.querycomposeraggregates', nullable: false },
    }
};
export var KendoModelQueryComposerHeaderDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ViewUrl': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: true },
        'DueDate': { type: 'date', nullable: true },
        'SubmittedOn': { type: 'date', nullable: true },
    }
};
export var KendoModelQueryComposerOrderByDTO = {
    fields: {
        'Direction': { type: 'enums.orderbydirections', nullable: false },
    }
};
export var KendoModelQueryComposerRequestDTO = {
    fields: {
        'SchemaVersion': { type: 'string', nullable: false },
        'Header': { type: 'any', nullable: false },
        'Queries': { type: 'any[]', nullable: false },
    }
};
export var KendoModelQueryComposerResponseErrorDTO = {
    fields: {
        'QueryID': { type: 'any', nullable: true },
        'Code': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerSelectDTO = {
    fields: {
        'Fields': { type: 'any[]', nullable: false },
    }
};
export var KendoModelQueryComposerResponseDTO = {
    fields: {
        'SchemaVersion': { type: 'string', nullable: false },
        'Header': { type: 'any', nullable: false },
        'Errors': { type: 'any[]', nullable: false },
        'Queries': { type: 'any[]', nullable: false },
    }
};
export var KendoModelQueryComposerTermDTO = {
    fields: {
        'Operator': { type: 'enums.querycomposeroperators', nullable: false },
        'Type': { type: 'any', nullable: false },
        'Values': { type: 'any', nullable: false },
        'Criteria': { type: 'any[]', nullable: false },
        'Design': { type: 'any', nullable: false },
    }
};
export var KendoModelQueryComposerWhereDTO = {
    fields: {
        'Criteria': { type: 'any[]', nullable: false },
    }
};
export var KendoModelProjectRequestTypeDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'WorkflowID': { type: 'any', nullable: true },
        'Workflow': { type: 'string', nullable: false },
    }
};
export var KendoModelRequestObserverEventSubscriptionDTO = {
    fields: {
        'RequestObserverID': { type: 'any', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'LastRunTime': { type: 'date', nullable: true },
        'NextDueTime': { type: 'date', nullable: true },
        'Frequency': { type: 'enums.frequencies', nullable: true },
    }
};
export var KendoModelRequestTypeTermDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'TermID': { type: 'any', nullable: false },
        'Term': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'OID': { type: 'string', nullable: false },
        'ReferenceUrl': { type: 'string', nullable: false },
    }
};
export var KendoModelBaseFieldOptionAclDTO = {
    fields: {
        'FieldIdentifier': { type: 'string', nullable: false },
        'Permission': { type: 'enums.fieldoptionpermissions', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
    }
};
export var KendoModelBaseEventPermissionDTO = {
    fields: {
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelOrganizationGroupDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'Organization': { type: 'string', nullable: false },
        'GroupID': { type: 'any', nullable: false },
        'Group': { type: 'string', nullable: false },
    }
};
export var KendoModelOrganizationRegistryDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'Organization': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: false },
        'OrganizationParent': { type: 'string', nullable: false },
        'RegistryID': { type: 'any', nullable: false },
        'Registry': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Type': { type: 'enums.registrytypes', nullable: false },
    }
};
export var KendoModelProjectDataMartWithRequestTypesDTO = {
    fields: {
        'RequestTypes': { type: 'any[]', nullable: false },
        'ProjectID': { type: 'any', nullable: false },
        'Project': { type: 'string', nullable: false },
        'ProjectAcronym': { type: 'string', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'Organization': { type: 'string', nullable: false },
    }
};
export var KendoModelProjectOrganizationDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'Project': { type: 'string', nullable: false },
        'OrganizationID': { type: 'any', nullable: false },
        'Organization': { type: 'string', nullable: false },
    }
};
export var KendoModelBaseAclDTO = {
    fields: {
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelUserEventSubscriptionDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'LastRunTime': { type: 'date', nullable: true },
        'NextDueTime': { type: 'date', nullable: true },
        'Frequency': { type: 'enums.frequencies', nullable: true },
        'FrequencyForMy': { type: 'enums.frequencies', nullable: true },
    }
};
export var KendoModelUserSettingDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'Key': { type: 'string', nullable: false },
        'Setting': { type: 'string', nullable: false },
    }
};
export var KendoModelQueryComposerQueryHeaderDTO = {
    fields: {
        'QueryType': { type: 'enums.querycomposerquerytypes', nullable: true },
        'ComposerInterface': { type: 'enums.querycomposerinterface', nullable: true },
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ViewUrl': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: true },
        'DueDate': { type: 'date', nullable: true },
        'SubmittedOn': { type: 'date', nullable: true },
    }
};
export var KendoModelQueryComposerRequestHeaderDTO = {
    fields: {
        'ID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ViewUrl': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: true },
        'DueDate': { type: 'date', nullable: true },
        'SubmittedOn': { type: 'date', nullable: true },
    }
};
export var KendoModelWFCommentDTO = {
    fields: {
        'Comment': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'CreatedByID': { type: 'any', nullable: false },
        'CreatedBy': { type: 'string', nullable: false },
        'RequestID': { type: 'any', nullable: false },
        'TaskID': { type: 'any', nullable: true },
        'WorkflowActivityID': { type: 'any', nullable: true },
        'WorkflowActivity': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelCommentDTO = {
    fields: {
        'Comment': { type: 'string', nullable: false },
        'ItemID': { type: 'any', nullable: false },
        'ItemTitle': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'CreatedByID': { type: 'any', nullable: false },
        'CreatedBy': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelDocumentDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'FileName': { type: 'string', nullable: false },
        'Viewable': { type: 'boolean', nullable: false },
        'MimeType': { type: 'string', nullable: false },
        'Kind': { type: 'string', nullable: false },
        'Data': { type: 'any', nullable: false },
        'Length': { type: 'number', nullable: false },
        'ItemID': { type: 'any', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelExtendedDocumentDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'FileName': { type: 'string', nullable: false },
        'Viewable': { type: 'boolean', nullable: false },
        'MimeType': { type: 'string', nullable: false },
        'Kind': { type: 'string', nullable: false },
        'Length': { type: 'number', nullable: false },
        'ItemID': { type: 'any', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'ContentModifiedOn': { type: 'date', nullable: true },
        'ContentCreatedOn': { type: 'date', nullable: true },
        'ItemTitle': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ParentDocumentID': { type: 'any', nullable: true },
        'UploadedByID': { type: 'any', nullable: true },
        'UploadedBy': { type: 'string', nullable: false },
        'RevisionSetID': { type: 'any', nullable: true },
        'RevisionDescription': { type: 'string', nullable: false },
        'MajorVersion': { type: 'number', nullable: false },
        'MinorVersion': { type: 'number', nullable: false },
        'BuildVersion': { type: 'number', nullable: false },
        'RevisionVersion': { type: 'number', nullable: false },
        'TaskItemType': { type: 'enums.taskitemtypes', nullable: true },
        'DocumentType': { type: 'enums.requestdocumenttype', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelOrganizationEHRSDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'Type': { type: 'enums.ehrstypes', nullable: false },
        'System': { type: 'enums.ehrssystems', nullable: false },
        'Other': { type: 'string', nullable: false },
        'StartYear': { type: 'number', nullable: true },
        'EndYear': { type: 'number', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelTemplateDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'CreatedByID': { type: 'any', nullable: true },
        'CreatedBy': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'Data': { type: 'string', nullable: false },
        'Type': { type: 'enums.templatetypes', nullable: false },
        'Notes': { type: 'string', nullable: false },
        'QueryType': { type: 'enums.querycomposerquerytypes', nullable: true },
        'ComposerInterface': { type: 'enums.querycomposerinterface', nullable: true },
        'Order': { type: 'number', nullable: false },
        'RequestTypeID': { type: 'any', nullable: true },
        'RequestType': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelTermDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'OID': { type: 'string', nullable: false },
        'ReferenceUrl': { type: 'string', nullable: false },
        'Type': { type: 'enums.termtypes', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelHomepageRequestDetailDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Identifier': { type: 'number', nullable: false },
        'SubmittedOn': { type: 'date', nullable: true },
        'SubmittedByName': { type: 'string', nullable: false },
        'SubmittedBy': { type: 'string', nullable: false },
        'SubmittedByID': { type: 'any', nullable: true },
        'StatusText': { type: 'string', nullable: false },
        'Status': { type: 'enums.requeststatuses', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'Project': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'MSRequestID': { type: 'string', nullable: false },
        'IsWorkflowRequest': { type: 'boolean', nullable: false },
        'CanEditMetadata': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelReportAggregationLevelDTO = {
    fields: {
        'NetworkID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'DeletedOn': { type: 'date', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestBudgetInfoDTO = {
    fields: {
        'BudgetActivityID': { type: 'any', nullable: true },
        'BudgetActivityDescription': { type: 'string', nullable: false },
        'BudgetActivityProjectID': { type: 'any', nullable: true },
        'BudgetActivityProjectDescription': { type: 'string', nullable: false },
        'BudgetTaskOrderID': { type: 'any', nullable: true },
        'BudgetTaskOrderDescription': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestMetadataDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'Priority': { type: 'enums.priorities', nullable: false },
        'PurposeOfUse': { type: 'string', nullable: false },
        'PhiDisclosureLevel': { type: 'string', nullable: false },
        'RequesterCenterID': { type: 'any', nullable: true },
        'ActivityID': { type: 'any', nullable: true },
        'ActivityProjectID': { type: 'any', nullable: true },
        'TaskOrderID': { type: 'any', nullable: true },
        'SourceActivityID': { type: 'any', nullable: true },
        'SourceActivityProjectID': { type: 'any', nullable: true },
        'SourceTaskOrderID': { type: 'any', nullable: true },
        'WorkplanTypeID': { type: 'any', nullable: true },
        'MSRequestID': { type: 'string', nullable: false },
        'ReportAggregationLevelID': { type: 'any', nullable: true },
        'ApplyChangesToRoutings': { type: 'boolean', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestObserverDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'UserID': { type: 'any', nullable: true },
        'SecurityGroupID': { type: 'any', nullable: true },
        'DisplayName': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
        'EventSubscriptions': { type: 'any[]', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelResponseGroupDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelAclGlobalFieldOptionDTO = {
    fields: {
        'FieldIdentifier': { type: 'string', nullable: false },
        'Permission': { type: 'enums.fieldoptionpermissions', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
    }
};
export var KendoModelAclProjectFieldOptionDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'FieldIdentifier': { type: 'string', nullable: false },
        'Permission': { type: 'enums.fieldoptionpermissions', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
    }
};
export var KendoModelBaseAclRequestTypeDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'Permission': { type: 'enums.requesttypepermissions', nullable: true },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelSecurityEntityDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Type': { type: 'enums.securityentitytypes', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelTaskDTO = {
    fields: {
        'Subject': { type: 'string', nullable: false },
        'Location': { type: 'string', nullable: false },
        'Body': { type: 'string', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'CreatedOn': { type: 'date', nullable: false },
        'StartOn': { type: 'date', nullable: true },
        'EndOn': { type: 'date', nullable: true },
        'EstimatedCompletedOn': { type: 'date', nullable: true },
        'Priority': { type: 'enums.priorities', nullable: false },
        'Status': { type: 'enums.taskstatuses', nullable: false },
        'Type': { type: 'enums.tasktypes', nullable: false },
        'PercentComplete': { type: 'number', nullable: false },
        'WorkflowActivityID': { type: 'any', nullable: true },
        'DirectToRequest': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelDataModelDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'RequiresConfiguration': { type: 'boolean', nullable: false },
        'QueryComposer': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelDataMartListDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: false },
        'StartDate': { type: 'date', nullable: true },
        'EndDate': { type: 'date', nullable: true },
        'OrganizationID': { type: 'any', nullable: true },
        'Organization': { type: 'string', nullable: false },
        'ParentOrganziationID': { type: 'any', nullable: true },
        'ParentOrganization': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'DueDate': { type: 'date', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelEventDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Locations': { type: 'any[]', nullable: false },
        'SupportsMyNotifications': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelGroupEventDTO = {
    fields: {
        'GroupID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelOrganizationEventDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelRegistryEventDTO = {
    fields: {
        'RegistryID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelUserEventDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelGroupDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'ApprovalRequired': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelNetworkMessageDTO = {
    fields: {
        'Subject': { type: 'string', nullable: false },
        'MessageText': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'Targets': { type: 'any[]', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelOrganizationDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'Primary': { type: 'boolean', nullable: false },
        'ParentOrganizationID': { type: 'any', nullable: true },
        'ParentOrganization': { type: 'string', nullable: false },
        'ContactEmail': { type: 'string', nullable: false },
        'ContactFirstName': { type: 'string', nullable: false },
        'ContactLastName': { type: 'string', nullable: false },
        'ContactPhone': { type: 'string', nullable: false },
        'SpecialRequirements': { type: 'string', nullable: false },
        'UsageRestrictions': { type: 'string', nullable: false },
        'OrganizationDescription': { type: 'string', nullable: false },
        'PragmaticClinicalTrials': { type: 'boolean', nullable: false },
        'ObservationalParticipation': { type: 'boolean', nullable: false },
        'ProspectiveTrials': { type: 'boolean', nullable: false },
        'EnableClaimsAndBilling': { type: 'boolean', nullable: false },
        'EnableEHRA': { type: 'boolean', nullable: false },
        'EnableRegistries': { type: 'boolean', nullable: false },
        'DataModelMSCDM': { type: 'boolean', nullable: false },
        'DataModelHMORNVDW': { type: 'boolean', nullable: false },
        'DataModelESP': { type: 'boolean', nullable: false },
        'DataModelI2B2': { type: 'boolean', nullable: false },
        'DataModelOMOP': { type: 'boolean', nullable: false },
        'DataModelPCORI': { type: 'boolean', nullable: false },
        'DataModelOther': { type: 'boolean', nullable: false },
        'DataModelOtherText': { type: 'string', nullable: false },
        'InpatientClaims': { type: 'boolean', nullable: false },
        'OutpatientClaims': { type: 'boolean', nullable: false },
        'OutpatientPharmacyClaims': { type: 'boolean', nullable: false },
        'EnrollmentClaims': { type: 'boolean', nullable: false },
        'DemographicsClaims': { type: 'boolean', nullable: false },
        'LaboratoryResultsClaims': { type: 'boolean', nullable: false },
        'VitalSignsClaims': { type: 'boolean', nullable: false },
        'OtherClaims': { type: 'boolean', nullable: false },
        'OtherClaimsText': { type: 'string', nullable: false },
        'Biorepositories': { type: 'boolean', nullable: false },
        'PatientReportedOutcomes': { type: 'boolean', nullable: false },
        'PatientReportedBehaviors': { type: 'boolean', nullable: false },
        'PrescriptionOrders': { type: 'boolean', nullable: false },
        'X509PublicKey': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelProjectDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: false },
        'StartDate': { type: 'date', nullable: true },
        'EndDate': { type: 'date', nullable: true },
        'Deleted': { type: 'boolean', nullable: false },
        'Active': { type: 'boolean', nullable: false },
        'Description': { type: 'string', nullable: false },
        'GroupID': { type: 'any', nullable: true },
        'Group': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRegistryDTO = {
    fields: {
        'Deleted': { type: 'boolean', nullable: false },
        'Type': { type: 'enums.registrytypes', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'RoPRUrl': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestDataMartDTO = {
    fields: {
        'RequestID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'Status': { type: 'enums.routingstatus', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'ErrorMessage': { type: 'string', nullable: false },
        'ErrorDetail': { type: 'string', nullable: false },
        'RejectReason': { type: 'string', nullable: false },
        'ResultsGrouped': { type: 'boolean', nullable: true },
        'Properties': { type: 'string', nullable: false },
        'RoutingType': { type: 'enums.routingtype', nullable: true },
        'ResponseID': { type: 'any', nullable: true },
        'ResponseGroupID': { type: 'any', nullable: true },
        'ResponseGroup': { type: 'string', nullable: false },
        'ResponseMessage': { type: 'string', nullable: false },
        'ResponseSubmittedOn': { type: 'date', nullable: true },
        'ResponseSubmittedByID': { type: 'any', nullable: true },
        'ResponseSubmittedBy': { type: 'string', nullable: false },
        'ResponseTime': { type: 'date', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestDTO = {
    fields: {
        'Identifier': { type: 'number', nullable: false },
        'MSRequestID': { type: 'string', nullable: false },
        'ProjectID': { type: 'any', nullable: false },
        'Project': { type: 'string', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'AdditionalInstructions': { type: 'string', nullable: false },
        'UpdatedOn': { type: 'date', nullable: false },
        'UpdatedByID': { type: 'any', nullable: false },
        'UpdatedBy': { type: 'string', nullable: false },
        'MirrorBudgetFields': { type: 'boolean', nullable: false },
        'Scheduled': { type: 'boolean', nullable: false },
        'Template': { type: 'boolean', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'OrganizationID': { type: 'any', nullable: false },
        'Organization': { type: 'string', nullable: false },
        'PurposeOfUse': { type: 'string', nullable: false },
        'PhiDisclosureLevel': { type: 'string', nullable: false },
        'ReportAggregationLevelID': { type: 'any', nullable: true },
        'ReportAggregationLevel': { type: 'string', nullable: false },
        'Schedule': { type: 'string', nullable: false },
        'ScheduleCount': { type: 'number', nullable: false },
        'SubmittedOn': { type: 'date', nullable: true },
        'SubmittedByID': { type: 'any', nullable: true },
        'SubmittedByName': { type: 'string', nullable: false },
        'SubmittedBy': { type: 'string', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'AdapterPackageVersion': { type: 'string', nullable: false },
        'IRBApprovalNo': { type: 'string', nullable: false },
        'DueDate': { type: 'date', nullable: true },
        'ActivityDescription': { type: 'string', nullable: false },
        'ActivityID': { type: 'any', nullable: true },
        'SourceActivityID': { type: 'any', nullable: true },
        'SourceActivityProjectID': { type: 'any', nullable: true },
        'SourceTaskOrderID': { type: 'any', nullable: true },
        'RequesterCenterID': { type: 'any', nullable: true },
        'RequesterCenter': { type: 'string', nullable: false },
        'WorkplanTypeID': { type: 'any', nullable: true },
        'WorkplanType': { type: 'string', nullable: false },
        'WorkflowID': { type: 'any', nullable: true },
        'Workflow': { type: 'string', nullable: false },
        'CurrentWorkFlowActivityID': { type: 'any', nullable: true },
        'CurrentWorkFlowActivity': { type: 'string', nullable: false },
        'Status': { type: 'enums.requeststatuses', nullable: false },
        'StatusText': { type: 'string', nullable: false },
        'MajorEventDate': { type: 'date', nullable: false },
        'MajorEventByID': { type: 'any', nullable: false },
        'MajorEventBy': { type: 'string', nullable: false },
        'CreatedOn': { type: 'date', nullable: false },
        'CreatedByID': { type: 'any', nullable: false },
        'CreatedBy': { type: 'string', nullable: false },
        'CompletedOn': { type: 'date', nullable: true },
        'CancelledOn': { type: 'date', nullable: true },
        'UserIdentifier': { type: 'string', nullable: false },
        'Query': { type: 'string', nullable: false },
        'ParentRequestID': { type: 'any', nullable: true },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelRequestTypeDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Metadata': { type: 'boolean', nullable: false },
        'PostProcess': { type: 'boolean', nullable: false },
        'AddFiles': { type: 'boolean', nullable: false },
        'RequiresProcessing': { type: 'boolean', nullable: false },
        'Notes': { type: 'string', nullable: false },
        'WorkflowID': { type: 'any', nullable: true },
        'Workflow': { type: 'string', nullable: false },
        'SupportMultiQuery': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelResponseDTO = {
    fields: {
        'RequestDataMartID': { type: 'any', nullable: false },
        'ResponseGroupID': { type: 'any', nullable: true },
        'RespondedByID': { type: 'any', nullable: true },
        'ResponseTime': { type: 'date', nullable: true },
        'Count': { type: 'number', nullable: false },
        'SubmittedOn': { type: 'date', nullable: false },
        'SubmittedByID': { type: 'any', nullable: false },
        'SubmitMessage': { type: 'string', nullable: false },
        'ResponseMessage': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelDataMartEventDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelAclDTO = {
    fields: {
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelProjectDataMartEventDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelProjectEventDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelProjectOrganizationEventDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'OrganizationID': { type: 'any', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'Overridden': { type: 'boolean', nullable: false },
        'EventID': { type: 'any', nullable: false },
        'Event': { type: 'string', nullable: false },
    }
};
export var KendoModelPermissionDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Locations': { type: 'any[]', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelSecurityGroupDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Path': { type: 'string', nullable: false },
        'OwnerID': { type: 'any', nullable: false },
        'Owner': { type: 'string', nullable: false },
        'ParentSecurityGroupID': { type: 'any', nullable: true },
        'ParentSecurityGroup': { type: 'string', nullable: false },
        'Kind': { type: 'enums.securitygroupkinds', nullable: false },
        'Type': { type: 'enums.securitygrouptypes', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelSsoEndpointDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'PostUrl': { type: 'string', nullable: false },
        'oAuthKey': { type: 'string', nullable: false },
        'oAuthHash': { type: 'string', nullable: false },
        'RequirePassword': { type: 'boolean', nullable: false },
        'Group': { type: 'any', nullable: false },
        'DisplayIndex': { type: 'number', nullable: false },
        'Enabled': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelUserDTO = {
    fields: {
        'UserName': { type: 'string', nullable: false },
        'Title': { type: 'string', nullable: false },
        'FirstName': { type: 'string', nullable: false },
        'LastName': { type: 'string', nullable: false },
        'MiddleName': { type: 'string', nullable: false },
        'Phone': { type: 'string', nullable: false },
        'Fax': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
        'Active': { type: 'boolean', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'OrganizationID': { type: 'any', nullable: true },
        'Organization': { type: 'string', nullable: false },
        'OrganizationRequested': { type: 'string', nullable: false },
        'RoleID': { type: 'any', nullable: true },
        'RoleRequested': { type: 'string', nullable: false },
        'SignedUpOn': { type: 'date', nullable: true },
        'ActivatedOn': { type: 'date', nullable: true },
        'DeactivatedOn': { type: 'date', nullable: true },
        'DeactivatedByID': { type: 'any', nullable: true },
        'DeactivatedBy': { type: 'string', nullable: false },
        'DeactivationReason': { type: 'string', nullable: false },
        'RejectReason': { type: 'string', nullable: false },
        'RejectedOn': { type: 'date', nullable: true },
        'RejectedByID': { type: 'any', nullable: true },
        'RejectedBy': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelWorkflowActivityDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Start': { type: 'boolean', nullable: false },
        'End': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelWorkflowDTO = {
    fields: {
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelWorkflowRoleDTO = {
    fields: {
        'WorkflowID': { type: 'any', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'IsRequestCreator': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelDataModelWithRequestTypesDTO = {
    fields: {
        'RequestTypes': { type: 'any[]', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'RequiresConfiguration': { type: 'boolean', nullable: false },
        'QueryComposer': { type: 'boolean', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelAclTemplateDTO = {
    fields: {
        'TemplateID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelDataMartDTO = {
    fields: {
        'RequiresApproval': { type: 'boolean', nullable: false },
        'DataMartTypeID': { type: 'any', nullable: false },
        'DataMartType': { type: 'string', nullable: false },
        'AvailablePeriod': { type: 'string', nullable: false },
        'ContactEmail': { type: 'string', nullable: false },
        'ContactFirstName': { type: 'string', nullable: false },
        'ContactLastName': { type: 'string', nullable: false },
        'ContactPhone': { type: 'string', nullable: false },
        'SpecialRequirements': { type: 'string', nullable: false },
        'UsageRestrictions': { type: 'string', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'HealthPlanDescription': { type: 'string', nullable: false },
        'IsGroupDataMart': { type: 'boolean', nullable: true },
        'UnattendedMode': { type: 'enums.unattendedmodes', nullable: true },
        'DataUpdateFrequency': { type: 'string', nullable: false },
        'InpatientEHRApplication': { type: 'string', nullable: false },
        'OutpatientEHRApplication': { type: 'string', nullable: false },
        'OtherClaims': { type: 'string', nullable: false },
        'OtherInpatientEHRApplication': { type: 'string', nullable: false },
        'OtherOutpatientEHRApplication': { type: 'string', nullable: false },
        'LaboratoryResultsAny': { type: 'boolean', nullable: false },
        'LaboratoryResultsClaims': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestName': { type: 'boolean', nullable: false },
        'LaboratoryResultsDates': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestLOINC': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestSNOMED': { type: 'boolean', nullable: false },
        'LaboratoryResultsSpecimenSource': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestDescriptions': { type: 'boolean', nullable: false },
        'LaboratoryResultsOrderDates': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestResultsInterpretation': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestOther': { type: 'boolean', nullable: false },
        'LaboratoryResultsTestOtherText': { type: 'string', nullable: false },
        'InpatientEncountersAny': { type: 'boolean', nullable: false },
        'InpatientEncountersEncounterID': { type: 'boolean', nullable: false },
        'InpatientEncountersProviderIdentifier': { type: 'boolean', nullable: false },
        'InpatientDatesOfService': { type: 'boolean', nullable: false },
        'InpatientICD9Procedures': { type: 'boolean', nullable: false },
        'InpatientICD10Procedures': { type: 'boolean', nullable: false },
        'InpatientICD9Diagnosis': { type: 'boolean', nullable: false },
        'InpatientICD10Diagnosis': { type: 'boolean', nullable: false },
        'InpatientSNOMED': { type: 'boolean', nullable: false },
        'InpatientHPHCS': { type: 'boolean', nullable: false },
        'InpatientDisposition': { type: 'boolean', nullable: false },
        'InpatientDischargeStatus': { type: 'boolean', nullable: false },
        'InpatientOther': { type: 'boolean', nullable: false },
        'InpatientOtherText': { type: 'string', nullable: false },
        'OutpatientEncountersAny': { type: 'boolean', nullable: false },
        'OutpatientEncountersEncounterID': { type: 'boolean', nullable: false },
        'OutpatientEncountersProviderIdentifier': { type: 'boolean', nullable: false },
        'OutpatientClinicalSetting': { type: 'boolean', nullable: false },
        'OutpatientDatesOfService': { type: 'boolean', nullable: false },
        'OutpatientICD9Procedures': { type: 'boolean', nullable: false },
        'OutpatientICD10Procedures': { type: 'boolean', nullable: false },
        'OutpatientICD9Diagnosis': { type: 'boolean', nullable: false },
        'OutpatientICD10Diagnosis': { type: 'boolean', nullable: false },
        'OutpatientSNOMED': { type: 'boolean', nullable: false },
        'OutpatientHPHCS': { type: 'boolean', nullable: false },
        'OutpatientOther': { type: 'boolean', nullable: false },
        'OutpatientOtherText': { type: 'string', nullable: false },
        'ERPatientID': { type: 'boolean', nullable: false },
        'EREncounterID': { type: 'boolean', nullable: false },
        'EREnrollmentDates': { type: 'boolean', nullable: false },
        'EREncounterDates': { type: 'boolean', nullable: false },
        'ERClinicalSetting': { type: 'boolean', nullable: false },
        'ERICD9Diagnosis': { type: 'boolean', nullable: false },
        'ERICD10Diagnosis': { type: 'boolean', nullable: false },
        'ERHPHCS': { type: 'boolean', nullable: false },
        'ERNDC': { type: 'boolean', nullable: false },
        'ERSNOMED': { type: 'boolean', nullable: false },
        'ERProviderIdentifier': { type: 'boolean', nullable: false },
        'ERProviderFacility': { type: 'boolean', nullable: false },
        'EREncounterType': { type: 'boolean', nullable: false },
        'ERDRG': { type: 'boolean', nullable: false },
        'ERDRGType': { type: 'boolean', nullable: false },
        'EROther': { type: 'boolean', nullable: false },
        'EROtherText': { type: 'string', nullable: false },
        'DemographicsAny': { type: 'boolean', nullable: false },
        'DemographicsPatientID': { type: 'boolean', nullable: false },
        'DemographicsSex': { type: 'boolean', nullable: false },
        'DemographicsDateOfBirth': { type: 'boolean', nullable: false },
        'DemographicsDateOfDeath': { type: 'boolean', nullable: false },
        'DemographicsAddressInfo': { type: 'boolean', nullable: false },
        'DemographicsRace': { type: 'boolean', nullable: false },
        'DemographicsEthnicity': { type: 'boolean', nullable: false },
        'DemographicsOther': { type: 'boolean', nullable: false },
        'DemographicsOtherText': { type: 'string', nullable: false },
        'PatientOutcomesAny': { type: 'boolean', nullable: false },
        'PatientOutcomesInstruments': { type: 'boolean', nullable: false },
        'PatientOutcomesInstrumentText': { type: 'string', nullable: false },
        'PatientOutcomesHealthBehavior': { type: 'boolean', nullable: false },
        'PatientOutcomesHRQoL': { type: 'boolean', nullable: false },
        'PatientOutcomesReportedOutcome': { type: 'boolean', nullable: false },
        'PatientOutcomesOther': { type: 'boolean', nullable: false },
        'PatientOutcomesOtherText': { type: 'string', nullable: false },
        'PatientBehaviorHealthBehavior': { type: 'boolean', nullable: false },
        'PatientBehaviorInstruments': { type: 'boolean', nullable: false },
        'PatientBehaviorInstrumentText': { type: 'string', nullable: false },
        'PatientBehaviorOther': { type: 'boolean', nullable: false },
        'PatientBehaviorOtherText': { type: 'string', nullable: false },
        'VitalSignsAny': { type: 'boolean', nullable: false },
        'VitalSignsTemperature': { type: 'boolean', nullable: false },
        'VitalSignsHeight': { type: 'boolean', nullable: false },
        'VitalSignsWeight': { type: 'boolean', nullable: false },
        'VitalSignsBMI': { type: 'boolean', nullable: false },
        'VitalSignsBloodPressure': { type: 'boolean', nullable: false },
        'VitalSignsOther': { type: 'boolean', nullable: false },
        'VitalSignsOtherText': { type: 'string', nullable: false },
        'VitalSignsLength': { type: 'boolean', nullable: false },
        'PrescriptionOrdersAny': { type: 'boolean', nullable: false },
        'PrescriptionOrderDates': { type: 'boolean', nullable: false },
        'PrescriptionOrderRxNorm': { type: 'boolean', nullable: false },
        'PrescriptionOrderNDC': { type: 'boolean', nullable: false },
        'PrescriptionOrderOther': { type: 'boolean', nullable: false },
        'PrescriptionOrderOtherText': { type: 'string', nullable: false },
        'PharmacyDispensingAny': { type: 'boolean', nullable: false },
        'PharmacyDispensingDates': { type: 'boolean', nullable: false },
        'PharmacyDispensingRxNorm': { type: 'boolean', nullable: false },
        'PharmacyDispensingDaysSupply': { type: 'boolean', nullable: false },
        'PharmacyDispensingAmountDispensed': { type: 'boolean', nullable: false },
        'PharmacyDispensingNDC': { type: 'boolean', nullable: false },
        'PharmacyDispensingOther': { type: 'boolean', nullable: false },
        'PharmacyDispensingOtherText': { type: 'string', nullable: false },
        'BiorepositoriesAny': { type: 'boolean', nullable: false },
        'BiorepositoriesName': { type: 'boolean', nullable: false },
        'BiorepositoriesDescription': { type: 'boolean', nullable: false },
        'BiorepositoriesDiseaseName': { type: 'boolean', nullable: false },
        'BiorepositoriesSpecimenSource': { type: 'boolean', nullable: false },
        'BiorepositoriesSpecimenType': { type: 'boolean', nullable: false },
        'BiorepositoriesProcessingMethod': { type: 'boolean', nullable: false },
        'BiorepositoriesSNOMED': { type: 'boolean', nullable: false },
        'BiorepositoriesStorageMethod': { type: 'boolean', nullable: false },
        'BiorepositoriesOther': { type: 'boolean', nullable: false },
        'BiorepositoriesOtherText': { type: 'string', nullable: false },
        'LongitudinalCaptureAny': { type: 'boolean', nullable: false },
        'LongitudinalCapturePatientID': { type: 'boolean', nullable: false },
        'LongitudinalCaptureStart': { type: 'boolean', nullable: false },
        'LongitudinalCaptureStop': { type: 'boolean', nullable: false },
        'LongitudinalCaptureOther': { type: 'boolean', nullable: false },
        'LongitudinalCaptureOtherValue': { type: 'string', nullable: false },
        'DataModel': { type: 'string', nullable: false },
        'OtherDataModel': { type: 'string', nullable: false },
        'IsLocal': { type: 'boolean', nullable: false },
        'Url': { type: 'string', nullable: false },
        'AdapterID': { type: 'any', nullable: true },
        'Adapter': { type: 'string', nullable: false },
        'ProcessorID': { type: 'any', nullable: true },
        'DataPartnerIdentifier': { type: 'string', nullable: false },
        'DataPartnerCode': { type: 'string', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Description': { type: 'string', nullable: false },
        'Acronym': { type: 'string', nullable: false },
        'StartDate': { type: 'date', nullable: true },
        'EndDate': { type: 'date', nullable: true },
        'OrganizationID': { type: 'any', nullable: true },
        'Organization': { type: 'string', nullable: false },
        'ParentOrganziationID': { type: 'any', nullable: true },
        'ParentOrganization': { type: 'string', nullable: false },
        'Priority': { type: 'enums.priorities', nullable: false },
        'DueDate': { type: 'date', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelResponseDetailDTO = {
    fields: {
        'Request': { type: 'string', nullable: false },
        'RequestID': { type: 'any', nullable: false },
        'DataMart': { type: 'string', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'SubmittedBy': { type: 'string', nullable: false },
        'RespondedBy': { type: 'string', nullable: false },
        'Status': { type: 'enums.routingstatus', nullable: false },
        'RequestDataMartID': { type: 'any', nullable: false },
        'ResponseGroupID': { type: 'any', nullable: true },
        'RespondedByID': { type: 'any', nullable: true },
        'ResponseTime': { type: 'date', nullable: true },
        'Count': { type: 'number', nullable: false },
        'SubmittedOn': { type: 'date', nullable: false },
        'SubmittedByID': { type: 'any', nullable: false },
        'SubmitMessage': { type: 'string', nullable: false },
        'ResponseMessage': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelAclDataMartRequestTypeDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'Permission': { type: 'enums.requesttypepermissions', nullable: true },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclDataMartDTO = {
    fields: {
        'DataMartID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclGroupDTO = {
    fields: {
        'GroupID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclOrganizationDTO = {
    fields: {
        'OrganizationID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclProjectOrganizationDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'OrganizationID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclProjectDataMartDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclProjectDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclProjectRequestTypeDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'Permission': { type: 'enums.requesttypepermissions', nullable: true },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclRegistryDTO = {
    fields: {
        'RegistryID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclRequestTypeDTO = {
    fields: {
        'RequestTypeID': { type: 'any', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclUserDTO = {
    fields: {
        'UserID': { type: 'any', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelSecurityGroupWithUsersDTO = {
    fields: {
        'Users': { type: 'any[]', nullable: false },
        'Name': { type: 'string', nullable: false },
        'Path': { type: 'string', nullable: false },
        'OwnerID': { type: 'any', nullable: false },
        'Owner': { type: 'string', nullable: false },
        'ParentSecurityGroupID': { type: 'any', nullable: true },
        'ParentSecurityGroup': { type: 'string', nullable: false },
        'Kind': { type: 'enums.securitygroupkinds', nullable: false },
        'Type': { type: 'enums.securitygrouptypes', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelUserWithSecurityDetailsDTO = {
    fields: {
        'PasswordHash': { type: 'string', nullable: false },
        'UserName': { type: 'string', nullable: false },
        'Title': { type: 'string', nullable: false },
        'FirstName': { type: 'string', nullable: false },
        'LastName': { type: 'string', nullable: false },
        'MiddleName': { type: 'string', nullable: false },
        'Phone': { type: 'string', nullable: false },
        'Fax': { type: 'string', nullable: false },
        'Email': { type: 'string', nullable: false },
        'Active': { type: 'boolean', nullable: false },
        'Deleted': { type: 'boolean', nullable: false },
        'OrganizationID': { type: 'any', nullable: true },
        'Organization': { type: 'string', nullable: false },
        'OrganizationRequested': { type: 'string', nullable: false },
        'RoleID': { type: 'any', nullable: true },
        'RoleRequested': { type: 'string', nullable: false },
        'SignedUpOn': { type: 'date', nullable: true },
        'ActivatedOn': { type: 'date', nullable: true },
        'DeactivatedOn': { type: 'date', nullable: true },
        'DeactivatedByID': { type: 'any', nullable: true },
        'DeactivatedBy': { type: 'string', nullable: false },
        'DeactivationReason': { type: 'string', nullable: false },
        'RejectReason': { type: 'string', nullable: false },
        'RejectedOn': { type: 'date', nullable: true },
        'RejectedByID': { type: 'any', nullable: true },
        'RejectedBy': { type: 'string', nullable: false },
        'ID': { type: 'any', nullable: true },
        'Timestamp': { type: 'any', nullable: false },
    }
};
export var KendoModelAclProjectRequestTypeWorkflowActivityDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'Project': { type: 'string', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'RequestType': { type: 'string', nullable: false },
        'WorkflowActivityID': { type: 'any', nullable: false },
        'WorkflowActivity': { type: 'string', nullable: false },
        'Allowed': { type: 'boolean', nullable: true },
        'PermissionID': { type: 'any', nullable: false },
        'Permission': { type: 'string', nullable: false },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
export var KendoModelAclProjectDataMartRequestTypeDTO = {
    fields: {
        'ProjectID': { type: 'any', nullable: false },
        'DataMartID': { type: 'any', nullable: false },
        'RequestTypeID': { type: 'any', nullable: false },
        'Permission': { type: 'enums.requesttypepermissions', nullable: true },
        'SecurityGroupID': { type: 'any', nullable: false },
        'SecurityGroup': { type: 'string', nullable: false },
        'Overridden': { type: 'boolean', nullable: false },
    }
};
//# sourceMappingURL=Dns.Interfaces.js.map
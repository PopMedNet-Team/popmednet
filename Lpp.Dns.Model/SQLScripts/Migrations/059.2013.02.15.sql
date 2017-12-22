IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DatamartNotifications_Datamarts_DatamartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DatamartNotifications]'))
ALTER TABLE [dbo].[DatamartNotifications] DROP CONSTRAINT [FK_DatamartNotifications_Datamarts_DatamartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DatamartNotifications_users_NotificationUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DatamartNotifications]'))
ALTER TABLE [dbo].[DatamartNotifications] DROP CONSTRAINT [FK_DatamartNotifications_users_NotificationUserId]
GO

/****** Object:  Table [dbo].[DatamartNotifications]    Script Date: 02/15/2013 09:16:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DatamartNotifications]') AND type in (N'U'))
DROP TABLE [dbo].[DatamartNotifications]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_DataMarts]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_DataMarts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_EntityTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_EntityTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_Groups]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_Organizations]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_Organizations]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailEntityAdded_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]'))
ALTER TABLE [dbo].[EventDetailEntityAdded] DROP CONSTRAINT [FK_EventDetailEntityAdded_Users]
GO

/****** Object:  Table [dbo].[EventDetailEntityAdded]    Script Date: 02/15/2013 09:17:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailEntityAdded]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailEntityAdded]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Events_EventDetailNewGroupResult_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewGroupResult]'))
ALTER TABLE [dbo].[EventDetailNewGroupResult] DROP CONSTRAINT [FK_Events_EventDetailNewGroupResult_EventId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Events_EventDetailNewGroupResult_NetworkQueryGroupId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewGroupResult]'))
ALTER TABLE [dbo].[EventDetailNewGroupResult] DROP CONSTRAINT [FK_Events_EventDetailNewGroupResult_NetworkQueryGroupId]
GO

/****** Object:  Table [dbo].[EventDetailNewGroupResult]    Script Date: 02/15/2013 09:17:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailNewGroupResult]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailNewGroupResult]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_datamarts_EventDetailNewQuery]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQuery]'))
ALTER TABLE [dbo].[EventDetailNewQuery] DROP CONSTRAINT [FK_datamarts_EventDetailNewQuery]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewQueryAndEvents_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQuery]'))
ALTER TABLE [dbo].[EventDetailNewQuery] DROP CONSTRAINT [FK_EventDetailNewQueryAndEvents_EventId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewQueryAndQueries_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQuery]'))
ALTER TABLE [dbo].[EventDetailNewQuery] DROP CONSTRAINT [FK_EventDetailNewQueryAndQueries_QueryId]
GO

/****** Object:  Table [dbo].[EventDetailNewQuery]    Script Date: 02/15/2013 09:17:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailNewQuery]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailNewQuery]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewQueryAndEventsForQueryAdmin_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQueryForQueryAdmin]'))
ALTER TABLE [dbo].[EventDetailNewQueryForQueryAdmin] DROP CONSTRAINT [FK_EventDetailNewQueryAndEventsForQueryAdmin_EventId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewQueryAndQueriesForQueryAdmin_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQueryForQueryAdmin]'))
ALTER TABLE [dbo].[EventDetailNewQueryForQueryAdmin] DROP CONSTRAINT [FK_EventDetailNewQueryAndQueriesForQueryAdmin_QueryId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Organizations_EventDetailNewQueryForQueryAdmin]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewQueryForQueryAdmin]'))
ALTER TABLE [dbo].[EventDetailNewQueryForQueryAdmin] DROP CONSTRAINT [FK_Organizations_EventDetailNewQueryForQueryAdmin]
GO

/****** Object:  Table [dbo].[EventDetailNewQueryForQueryAdmin]    Script Date: 02/15/2013 09:17:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailNewQueryForQueryAdmin]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailNewQueryForQueryAdmin]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_datamarts_EventDetailNewResult]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewResult]'))
ALTER TABLE [dbo].[EventDetailNewResult] DROP CONSTRAINT [FK_datamarts_EventDetailNewResult]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewResultAndEvents_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewResult]'))
ALTER TABLE [dbo].[EventDetailNewResult] DROP CONSTRAINT [FK_EventDetailNewResultAndEvents_EventId]
GO

/****** Object:  Table [dbo].[EventDetailNewResult]    Script Date: 02/15/2013 09:17:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailNewResult]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailNewResult]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_datamarts_EventDetailNewResultForOrganization]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewResultForOrganization]'))
ALTER TABLE [dbo].[EventDetailNewResultForOrganization] DROP CONSTRAINT [FK_datamarts_EventDetailNewResultForOrganization]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailNewResultForOrganizationAndEvents_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailNewResultForOrganization]'))
ALTER TABLE [dbo].[EventDetailNewResultForOrganization] DROP CONSTRAINT [FK_EventDetailNewResultForOrganizationAndEvents_EventId]
GO

/****** Object:  Table [dbo].[EventDetailNewResultForOrganization]    Script Date: 02/15/2013 09:18:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailNewResultForOrganization]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailNewResultForOrganization]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryReminder_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryReminder]'))
ALTER TABLE [dbo].[EventDetailQueryReminder] DROP CONSTRAINT [FK_EventDetailQueryReminder_UserId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryReminderAnddatamarts_DatamartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryReminder]'))
ALTER TABLE [dbo].[EventDetailQueryReminder] DROP CONSTRAINT [FK_EventDetailQueryReminderAnddatamarts_DatamartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryReminderAndEvents_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryReminder]'))
ALTER TABLE [dbo].[EventDetailQueryReminder] DROP CONSTRAINT [FK_EventDetailQueryReminderAndEvents_EventId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryReminderAndQueries_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryReminder]'))
ALTER TABLE [dbo].[EventDetailQueryReminder] DROP CONSTRAINT [FK_EventDetailQueryReminderAndQueries_QueryId]
GO


GO

/****** Object:  Table [dbo].[EventDetailQueryReminder]    Script Date: 02/15/2013 09:18:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailQueryReminder]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailQueryReminder]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_datamarts_EventDetailQueryStatusChange]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryStatusChange]'))
ALTER TABLE [dbo].[EventDetailQueryStatusChange] DROP CONSTRAINT [FK_datamarts_EventDetailQueryStatusChange]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryStatusChange_Users_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryStatusChange]'))
ALTER TABLE [dbo].[EventDetailQueryStatusChange] DROP CONSTRAINT [FK_EventDetailQueryStatusChange_Users_UserId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailStatusChangeAndEvents_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryStatusChange]'))
ALTER TABLE [dbo].[EventDetailQueryStatusChange] DROP CONSTRAINT [FK_EventDetailStatusChangeAndEvents_EventId]
GO


GO

/****** Object:  Table [dbo].[EventDetailQueryStatusChange]    Script Date: 02/15/2013 09:18:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailQueryStatusChange]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailQueryStatusChange]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryTypeDatamartAssociation_DataMarts]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryTypeDatamartAssociation]'))
ALTER TABLE [dbo].[EventDetailQueryTypeDatamartAssociation] DROP CONSTRAINT [FK_EventDetailQueryTypeDatamartAssociation_DataMarts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryTypeDatamartAssociation_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryTypeDatamartAssociation]'))
ALTER TABLE [dbo].[EventDetailQueryTypeDatamartAssociation] DROP CONSTRAINT [FK_EventDetailQueryTypeDatamartAssociation_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailQueryTypeDatamartAssociation_QueryTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailQueryTypeDatamartAssociation]'))
ALTER TABLE [dbo].[EventDetailQueryTypeDatamartAssociation] DROP CONSTRAINT [FK_EventDetailQueryTypeDatamartAssociation_QueryTypes]
GO


GO

/****** Object:  Table [dbo].[EventDetailQueryTypeDatamartAssociation]    Script Date: 02/15/2013 09:18:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailQueryTypeDatamartAssociation]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailQueryTypeDatamartAssociation]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailSubmitterReminder_EventId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailSubmitterReminder]'))
ALTER TABLE [dbo].[EventDetailSubmitterReminder] DROP CONSTRAINT [FK_EventDetailSubmitterReminder_EventId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailSubmitterReminder_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailSubmitterReminder]'))
ALTER TABLE [dbo].[EventDetailSubmitterReminder] DROP CONSTRAINT [FK_EventDetailSubmitterReminder_QueryId]
GO


GO

/****** Object:  Table [dbo].[EventDetailSubmitterReminder]    Script Date: 02/15/2013 09:18:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailSubmitterReminder]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailSubmitterReminder]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailUserUpdated_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailUserUpdated]'))
ALTER TABLE [dbo].[EventDetailUserUpdated] DROP CONSTRAINT [FK_EventDetailUserUpdated_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailUserUpdated_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailUserUpdated]'))
ALTER TABLE [dbo].[EventDetailUserUpdated] DROP CONSTRAINT [FK_EventDetailUserUpdated_Users]
GO


GO

/****** Object:  Table [dbo].[EventDetailUserUpdated]    Script Date: 02/15/2013 09:18:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailUserUpdated]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailUserUpdated]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailViewedResult_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailViewedResult]'))
ALTER TABLE [dbo].[EventDetailViewedResult] DROP CONSTRAINT [FK_EventDetailViewedResult_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailViewedResult_Queries]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailViewedResult]'))
ALTER TABLE [dbo].[EventDetailViewedResult] DROP CONSTRAINT [FK_EventDetailViewedResult_Queries]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventDetailViewedResult_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[EventDetailViewedResult]'))
ALTER TABLE [dbo].[EventDetailViewedResult] DROP CONSTRAINT [FK_EventDetailViewedResult_Users]
GO


GO

/****** Object:  Table [dbo].[EventDetailViewedResult]    Script Date: 02/15/2013 09:19:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventDetailViewedResult]') AND type in (N'U'))
DROP TABLE [dbo].[EventDetailViewedResult]
GO


GO

/****** Object:  Table [dbo].[EventMessage]    Script Date: 02/15/2013 09:19:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventMessage]') AND type in (N'U'))
DROP TABLE [dbo].[EventMessage]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventsAndEventSources_EventSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Events]'))
ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_EventsAndEventSources_EventSourceId]
GO


/****** Object:  Table [dbo].[Events]    Script Date: 02/15/2013 09:19:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
DROP TABLE [dbo].[Events]
GO


GO

/****** Object:  Table [dbo].[EventSources]    Script Date: 02/15/2013 09:19:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventSources]') AND type in (N'U'))
DROP TABLE [dbo].[EventSources]
GO


GO

/****** Object:  Table [dbo].[EventTypeNotificationFrequency]    Script Date: 02/15/2013 09:19:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTypeNotificationFrequency]') AND type in (N'U'))
DROP TABLE [dbo].[EventTypeNotificationFrequency]
GO


GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__EventType__Canbe__23F3538A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventTypes] DROP CONSTRAINT [DF__EventType__Canbe__23F3538A]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__EventType__Event__24E777C3]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventTypes] DROP CONSTRAINT [DF__EventType__Event__24E777C3]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__EventType__Categ__25DB9BFC]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventTypes] DROP CONSTRAINT [DF__EventType__Categ__25DB9BFC]
END

GO

/****** Object:  Table [dbo].[EventTypesCategory]    Script Date: 02/15/2013 09:19:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTypesCategory]') AND type in (N'U'))
DROP TABLE [dbo].[EventTypesCategory]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupAdministrators_Groups_GroupId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupAdministrators]'))
ALTER TABLE [dbo].[GroupAdministrators] DROP CONSTRAINT [FK_GroupAdministrators_Groups_GroupId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupAdministrators_Users_AdminUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupAdministrators]'))
ALTER TABLE [dbo].[GroupAdministrators] DROP CONSTRAINT [FK_GroupAdministrators_Users_AdminUserId]
GO


GO

/****** Object:  Table [dbo].[GroupAdministrators]    Script Date: 02/15/2013 09:20:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupAdministrators]') AND type in (N'U'))
DROP TABLE [dbo].[GroupAdministrators]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupedDatamartsMap_DataMarts_DataMartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupedDatamartsMap]'))
ALTER TABLE [dbo].[GroupedDatamartsMap] DROP CONSTRAINT [FK_GroupedDatamartsMap_DataMarts_DataMartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupedDatamartsMap_DataMarts_GroupDataMartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupedDatamartsMap]'))
ALTER TABLE [dbo].[GroupedDatamartsMap] DROP CONSTRAINT [FK_GroupedDatamartsMap_DataMarts_GroupDataMartId]
GO


GO

/****** Object:  Table [dbo].[GroupedDatamartsMap]    Script Date: 02/15/2013 09:20:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupedDatamartsMap]') AND type in (N'U'))
DROP TABLE [dbo].[GroupedDatamartsMap]
GO


GO

/****** Object:  Table [dbo].[HCPCSProcedures]    Script Date: 02/15/2013 09:20:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HCPCSProcedures]') AND type in (N'U'))
DROP TABLE [dbo].[HCPCSProcedures]
GO


GO

/****** Object:  Table [dbo].[ICD9Diagnosis]    Script Date: 02/15/2013 09:20:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ICD9Diagnosis]') AND type in (N'U'))
DROP TABLE [dbo].[ICD9Diagnosis]
GO


GO

/****** Object:  Table [dbo].[ICD9Diagnosis_4_digit]    Script Date: 02/15/2013 09:21:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ICD9Diagnosis_4_digit]') AND type in (N'U'))
DROP TABLE [dbo].[ICD9Diagnosis_4_digit]
GO


GO

/****** Object:  Table [dbo].[ICD9Diagnosis_5_digit]    Script Date: 02/15/2013 09:21:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ICD9Diagnosis_5_digit]') AND type in (N'U'))
DROP TABLE [dbo].[ICD9Diagnosis_5_digit]
GO


GO

/****** Object:  Table [dbo].[ICD9Procedures]    Script Date: 02/15/2013 09:21:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ICD9Procedures]') AND type in (N'U'))
DROP TABLE [dbo].[ICD9Procedures]
GO


GO

/****** Object:  Table [dbo].[ICD9Procedures_4_digit]    Script Date: 02/15/2013 09:21:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ICD9Procedures_4_digit]') AND type in (N'U'))
DROP TABLE [dbo].[ICD9Procedures_4_digit]
GO

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Informati__Query__740F363E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Information]'))
ALTER TABLE [dbo].[Information] DROP CONSTRAINT [FK__Informati__Query__740F363E]
GO


GO

/****** Object:  Table [dbo].[Information]    Script Date: 02/15/2013 09:21:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Information]') AND type in (N'U'))
DROP TABLE [dbo].[Information]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__LookUpVal__Categ__4277DAAA]') AND parent_object_id = OBJECT_ID(N'[dbo].[LookUpValues]'))
ALTER TABLE [dbo].[LookUpValues] DROP CONSTRAINT [FK__LookUpVal__Categ__4277DAAA]
GO


GO

/****** Object:  Table [dbo].[LookUpValues]    Script Date: 02/15/2013 09:24:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LookUpValues]') AND type in (N'U'))
DROP TABLE [dbo].[LookUpValues]
GO


GO

/****** Object:  Table [dbo].[NetworkQueriesGroups]    Script Date: 02/15/2013 09:25:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NetworkQueriesGroups]') AND type in (N'U'))
DROP TABLE [dbo].[NetworkQueriesGroups]
GO


GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_NewFeatures_CreateDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NewFeatures] DROP CONSTRAINT [DF_NewFeatures_CreateDate]
END

GO


GO

/****** Object:  Table [dbo].[NewFeatures]    Script Date: 02/15/2013 09:25:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NewFeatures]') AND type in (N'U'))
DROP TABLE [dbo].[NewFeatures]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NotificationOptionsAndNotificationTypes_NotificationTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationOptions]'))
ALTER TABLE [dbo].[NotificationOptions] DROP CONSTRAINT [FK_NotificationOptionsAndNotificationTypes_NotificationTypeId]
GO


GO

/****** Object:  Table [dbo].[NotificationOptions]    Script Date: 02/15/2013 09:25:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationOptions]') AND type in (N'U'))
DROP TABLE [dbo].[NotificationOptions]
GO


GO

/****** Object:  Table [dbo].[NotificationProcessingConfig]    Script Date: 02/15/2013 09:26:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationProcessingConfig]') AND type in (N'U'))
DROP TABLE [dbo].[NotificationProcessingConfig]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NotificationsAndNotificationTypes_NotificationTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Notifications]'))
ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT [FK_NotificationsAndNotificationTypes_NotificationTypeId]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Notificat__Encry__2AA05119]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Notifications] DROP CONSTRAINT [DF__Notificat__Encry__2AA05119]
END

GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Fk_NotificationId_Notifications_NotificationSecureGUID]') AND parent_object_id = OBJECT_ID(N'[dbo].[NotificationSecureGUID]'))
ALTER TABLE [dbo].[NotificationSecureGUID] DROP CONSTRAINT [Fk_NotificationId_Notifications_NotificationSecureGUID]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Notificat__IsAct__3335971A]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[NotificationSecureGUID] DROP CONSTRAINT [DF__Notificat__IsAct__3335971A]
END

GO

/****** Object:  Table [dbo].[NotificationSecureGUID]    Script Date: 02/15/2013 09:26:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationSecureGUID]') AND type in (N'U'))
DROP TABLE [dbo].[NotificationSecureGUID]
GO


GO

/****** Object:  Table [dbo].[NotificationTypes]    Script Date: 02/15/2013 09:26:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationTypes]') AND type in (N'U'))
DROP TABLE [dbo].[NotificationTypes]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationAdministrators_Organizations_OrganizationId]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationAdministrators]'))
ALTER TABLE [dbo].[OrganizationAdministrators] DROP CONSTRAINT [FK_OrganizationAdministrators_Organizations_OrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrganizationAdministrators_Users_AdminUserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrganizationAdministrators]'))
ALTER TABLE [dbo].[OrganizationAdministrators] DROP CONSTRAINT [FK_OrganizationAdministrators_Users_AdminUserId]
GO


GO

/****** Object:  Table [dbo].[OrganizationAdministrators]    Script Date: 02/15/2013 09:26:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrganizationAdministrators]') AND type in (N'U'))
DROP TABLE [dbo].[OrganizationAdministrators]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DataMarts_PermissionsGroupsDataMarts_DatamartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsDataMarts]'))
ALTER TABLE [dbo].[PermissionsGroupsDataMarts] DROP CONSTRAINT [FK_DataMarts_PermissionsGroupsDataMarts_DatamartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_PermissionsGroupsDataMarts_GroupId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsDataMarts]'))
ALTER TABLE [dbo].[PermissionsGroupsDataMarts] DROP CONSTRAINT [FK_Groups_PermissionsGroupsDataMarts_GroupId]
GO


GO

/****** Object:  Table [dbo].[PermissionsGroupsDataMarts]    Script Date: 02/15/2013 09:27:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsDataMarts]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsGroupsDataMarts]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_PermissionsGroupsQueryTypes_GroupId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsQueryTypes]'))
ALTER TABLE [dbo].[PermissionsGroupsQueryTypes] DROP CONSTRAINT [FK_Groups_PermissionsGroupsQueryTypes_GroupId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryType_PermissionsGroupsQueryTypes_QueryTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsQueryTypes]'))
ALTER TABLE [dbo].[PermissionsGroupsQueryTypes] DROP CONSTRAINT [FK_QueryType_PermissionsGroupsQueryTypes_QueryTypeId]
GO


GO

/****** Object:  Table [dbo].[PermissionsGroupsQueryTypes]    Script Date: 02/15/2013 09:27:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsQueryTypes]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsGroupsQueryTypes]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Groups_GroupsRights_GroupId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsRights]'))
ALTER TABLE [dbo].[PermissionsGroupsRights] DROP CONSTRAINT [FK_Groups_GroupsRights_GroupId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rights_GroupsRights_RightId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsRights]'))
ALTER TABLE [dbo].[PermissionsGroupsRights] DROP CONSTRAINT [FK_Rights_GroupsRights_RightId]
GO


GO

/****** Object:  Table [dbo].[PermissionsGroupsRights]    Script Date: 02/15/2013 09:27:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsGroupsRights]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsGroupsRights]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DataMarts_PermissionsOrganizationsDataMarts_DatamartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsDataMarts]'))
ALTER TABLE [dbo].[PermissionsOrganizationsDataMarts] DROP CONSTRAINT [FK_DataMarts_PermissionsOrganizationsDataMarts_DatamartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Organizations_PermissionsOrganizationsDataMarts_OrgId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsDataMarts]'))
ALTER TABLE [dbo].[PermissionsOrganizationsDataMarts] DROP CONSTRAINT [FK_Organizations_PermissionsOrganizationsDataMarts_OrgId]
GO


GO

/****** Object:  Table [dbo].[PermissionsOrganizationsDataMarts]    Script Date: 02/15/2013 09:27:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsDataMarts]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsOrganizationsDataMarts]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Organizations_PermissionsOrganizationsQueryTypes_OrgId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsQueryTypes]'))
ALTER TABLE [dbo].[PermissionsOrganizationsQueryTypes] DROP CONSTRAINT [FK_Organizations_PermissionsOrganizationsQueryTypes_OrgId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryTypes_PermissionsOrganizationsQueryTypes_QueryTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsQueryTypes]'))
ALTER TABLE [dbo].[PermissionsOrganizationsQueryTypes] DROP CONSTRAINT [FK_QueryTypes_PermissionsOrganizationsQueryTypes_QueryTypeId]
GO


GO

/****** Object:  Table [dbo].[PermissionsOrganizationsQueryTypes]    Script Date: 02/15/2013 09:27:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsQueryTypes]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsOrganizationsQueryTypes]
GO

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Organizations_OrganizationsRights_OrgId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsRights]'))
ALTER TABLE [dbo].[PermissionsOrganizationsRights] DROP CONSTRAINT [FK_Organizations_OrganizationsRights_OrgId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rights_OrganizationsRights_RightId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsRights]'))
ALTER TABLE [dbo].[PermissionsOrganizationsRights] DROP CONSTRAINT [FK_Rights_OrganizationsRights_RightId]
GO


GO

/****** Object:  Table [dbo].[PermissionsOrganizationsRights]    Script Date: 02/15/2013 09:27:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsOrganizationsRights]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsOrganizationsRights]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Organizations_PermissionsQueryTypesDataMarts_DatamartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsQueryTypesDataMarts]'))
ALTER TABLE [dbo].[PermissionsQueryTypesDataMarts] DROP CONSTRAINT [FK_Organizations_PermissionsQueryTypesDataMarts_DatamartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryTypes_PermissionsQueryTypesDataMarts_QueryTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsQueryTypesDataMarts]'))
ALTER TABLE [dbo].[PermissionsQueryTypesDataMarts] DROP CONSTRAINT [FK_QueryTypes_PermissionsQueryTypesDataMarts_QueryTypeId]
GO


GO

/****** Object:  Table [dbo].[PermissionsQueryTypesDataMarts]    Script Date: 02/15/2013 09:28:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsQueryTypesDataMarts]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsQueryTypesDataMarts]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PermissionsUsersDataMarts_DataMarts]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersDataMarts]'))
ALTER TABLE [dbo].[PermissionsUsersDataMarts] DROP CONSTRAINT [FK_PermissionsUsersDataMarts_DataMarts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PermissionsUsersDataMarts_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersDataMarts]'))
ALTER TABLE [dbo].[PermissionsUsersDataMarts] DROP CONSTRAINT [FK_PermissionsUsersDataMarts_Users]
GO


GO

/****** Object:  Table [dbo].[PermissionsUsersDataMarts]    Script Date: 02/15/2013 09:28:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsUsersDataMarts]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsUsersDataMarts]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PermissionsUsersQueryTypes_QueryTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersQueryTypes]'))
ALTER TABLE [dbo].[PermissionsUsersQueryTypes] DROP CONSTRAINT [FK_PermissionsUsersQueryTypes_QueryTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PermissionsUsersQueryTypes_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersQueryTypes]'))
ALTER TABLE [dbo].[PermissionsUsersQueryTypes] DROP CONSTRAINT [FK_PermissionsUsersQueryTypes_Users]
GO


GO

/****** Object:  Table [dbo].[PermissionsUsersQueryTypes]    Script Date: 02/15/2013 09:28:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsUsersQueryTypes]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsUsersQueryTypes]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rights_UsersRight_RightsId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersRights]'))
ALTER TABLE [dbo].[PermissionsUsersRights] DROP CONSTRAINT [FK_Rights_UsersRight_RightsId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_UsersRight_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[PermissionsUsersRights]'))
ALTER TABLE [dbo].[PermissionsUsersRights] DROP CONSTRAINT [FK_Users_UsersRight_UserId]
GO


GO

/****** Object:  Table [dbo].[PermissionsUsersRights]    Script Date: 02/15/2013 09:28:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionsUsersRights]') AND type in (N'U'))
DROP TABLE [dbo].[PermissionsUsersRights]
GO

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[Fk_Queries_ViewedQueriesResults_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueriesCachedResults]'))
ALTER TABLE [dbo].[QueriesCachedResults] DROP CONSTRAINT [Fk_Queries_ViewedQueriesResults_QueryId]
GO


GO

/****** Object:  Table [dbo].[QueriesCachedResults]    Script Date: 02/15/2013 09:28:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueriesCachedResults]') AND type in (N'U'))
DROP TABLE [dbo].[QueriesCachedResults]
GO

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueriesDataMarts_DataMarts]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueriesDataMarts]'))
ALTER TABLE [dbo].[QueriesDataMarts] DROP CONSTRAINT [FK_QueriesDataMarts_DataMarts]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueriesDataMarts_Queries]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueriesDataMarts]'))
ALTER TABLE [dbo].[QueriesDataMarts] DROP CONSTRAINT [FK_QueriesDataMarts_Queries]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__QueriesDa__IsRes__351DDF8C]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[QueriesDataMarts] DROP CONSTRAINT [DF__QueriesDa__IsRes__351DDF8C]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__QueriesDa__Respo__361203C5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[QueriesDataMarts] DROP CONSTRAINT [DF__QueriesDa__Respo__361203C5]
END

GO


GO

/****** Object:  Table [dbo].[QueriesGroupStratifications]    Script Date: 02/15/2013 09:29:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueriesGroupStratifications]') AND type in (N'U'))
DROP TABLE [dbo].[QueriesGroupStratifications]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Query_Queries]') AND parent_object_id = OBJECT_ID(N'[dbo].[Query]'))
ALTER TABLE [dbo].[Query] DROP CONSTRAINT [FK_Query_Queries]
GO


GO

/****** Object:  Table [dbo].[Query]    Script Date: 02/15/2013 09:29:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Query]') AND type in (N'U'))
DROP TABLE [dbo].[Query]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryOrganizationalDataMart_DataMarts_DataMartId]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueryOrganizationalDataMart]'))
ALTER TABLE [dbo].[QueryOrganizationalDataMart] DROP CONSTRAINT [FK_QueryOrganizationalDataMart_DataMarts_DataMartId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryOrganizationalDataMart_Organizations_OrganizationId]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueryOrganizationalDataMart]'))
ALTER TABLE [dbo].[QueryOrganizationalDataMart] DROP CONSTRAINT [FK_QueryOrganizationalDataMart_Organizations_OrganizationId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryOrganizationalDataMart_Queries_QueryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueryOrganizationalDataMart]'))
ALTER TABLE [dbo].[QueryOrganizationalDataMart] DROP CONSTRAINT [FK_QueryOrganizationalDataMart_Queries_QueryId]
GO


GO

/****** Object:  Table [dbo].[QueryOrganizationalDataMart]    Script Date: 02/15/2013 09:29:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueryOrganizationalDataMart]') AND type in (N'U'))
DROP TABLE [dbo].[QueryOrganizationalDataMart]
GO


GO

/****** Object:  Table [dbo].[QueryStatusTypes]    Script Date: 02/15/2013 09:30:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueryStatusTypes]') AND type in (N'U'))
DROP TABLE [dbo].[QueryStatusTypes]
GO



GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Rights_CategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Rights]'))
ALTER TABLE [dbo].[Rights] DROP CONSTRAINT [FK_Rights_CategoryId]
GO



GO

/****** Object:  Table [dbo].[RightsCategories]    Script Date: 02/15/2013 09:30:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RightsCategories]') AND type in (N'U'))
DROP TABLE [dbo].[RightsCategories]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleRightsMap_Rights_RightId]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleRightsMap]'))
ALTER TABLE [dbo].[RoleRightsMap] DROP CONSTRAINT [FK_RoleRightsMap_Rights_RightId]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleRightsMap_RoleTypes_RoleTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleRightsMap]'))
ALTER TABLE [dbo].[RoleRightsMap] DROP CONSTRAINT [FK_RoleRightsMap_RoleTypes_RoleTypeId]
GO


GO

/****** Object:  Table [dbo].[RoleRightsMap]    Script Date: 02/15/2013 09:30:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleRightsMap]') AND type in (N'U'))
DROP TABLE [dbo].[RoleRightsMap]
GO


GO

/****** Object:  Table [dbo].[SecurityObjects2]    Script Date: 02/15/2013 09:31:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityObjects2]') AND type in (N'U'))
DROP TABLE [dbo].[SecurityObjects2]
GO


GO

/****** Object:  Table [dbo].[SiteThemes]    Script Date: 02/15/2013 09:31:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteThemes]') AND type in (N'U'))
DROP TABLE [dbo].[SiteThemes]
GO

GO

/****** Object:  View [dbo].[DNS3_DataMartTypes]    Script Date: 02/15/2013 09:32:37 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_DataMartTypes]'))
DROP VIEW [dbo].[DNS3_DataMartTypes]
GO

GO

/****** Object:  View [dbo].[DNS3_Responses]    Script Date: 02/15/2013 09:32:54 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Responses]'))
DROP VIEW [dbo].[DNS3_Responses]
GO

GO

/****** Object:  View [dbo].[EventTypeNotificationFrequency_view]    Script Date: 02/15/2013 09:33:05 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[EventTypeNotificationFrequency_view]'))
DROP VIEW [dbo].[EventTypeNotificationFrequency_view]
GO


GO

/****** Object:  View [dbo].[RequestStatusTypes]    Script Date: 02/15/2013 09:33:30 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[RequestStatusTypes]'))
DROP VIEW [dbo].[RequestStatusTypes]
GO


GO

/****** Object:  View [dbo].[vw_DataMartNotificationUsers]    Script Date: 02/15/2013 09:33:52 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_DataMartNotificationUsers]'))
DROP VIEW [dbo].[vw_DataMartNotificationUsers]
GO


GO

/****** Object:  View [dbo].[vw_Query]    Script Date: 02/15/2013 09:34:00 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Query]'))
DROP VIEW [dbo].[vw_Query]
GO


/****** Object:  View [dbo].[vw_SummaryNotification]    Script Date: 02/15/2013 09:34:11 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_SummaryNotification]'))
DROP VIEW [dbo].[vw_SummaryNotification]
GO


/****** Object:  Table [dbo].[EntityTypes]    Script Date: 02/15/2013 09:16:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntityTypes]') AND type in (N'U'))
DROP TABLE [dbo].[EntityTypes]
GO

/****** Object:  Table [dbo].[LookUpCategory]    Script Date: 02/15/2013 09:22:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LookUpCategory]') AND type in (N'U'))
DROP TABLE [dbo].[LookUpCategory]
GO


/****** THIS TABLE IS REFERENCED BY THE QUERIES TABLE;  REMOVE REFERENCE BEFORE DROPPING TABLE ****/
/****** Object:  Table [dbo].[QueryTypes]    Script Date: 02/15/2013 09:30:16 *****
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueryTypes]') AND type in (N'U'))
DROP TABLE [dbo].[QueryTypes]
GO
*/

/****** Object:  Table [dbo].[Rights]    Script Date: 02/15/2013 09:30:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rights]') AND type in (N'U'))
DROP TABLE [dbo].[Rights]
GO


GO

/*** THIS TABLE IS STILL BEING REFERENCED BY THE USERS TABLE ****/
/****** Object:  Table [dbo].[RoleTypes]    Script Date: 02/15/2013 09:31:06 *****
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleTypes]') AND type in (N'U'))
DROP TABLE [dbo].[RoleTypes]
GO
*/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EventsAndEventTypes_EventTypeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Events]'))
ALTER TABLE [dbo].[Events] DROP CONSTRAINT [FK_EventsAndEventTypes_EventTypeId]
GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_QueryTypes_QueryCategory_QueryCategoryId]') AND parent_object_id = OBJECT_ID(N'[dbo].[QueryTypes]'))
ALTER TABLE [dbo].[QueryTypes] DROP CONSTRAINT [FK_QueryTypes_QueryCategory_QueryCategoryId]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__QueryType__IsAdm__2B947552]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[QueryTypes] DROP CONSTRAINT [DF__QueryType__IsAdm__2B947552]
END

GO


GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserNotificationFrequency_EventTypeID]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserNotificationFrequency]'))
ALTER TABLE [dbo].[UserNotificationFrequency] DROP CONSTRAINT [FK_UserNotificationFrequency_EventTypeID]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserNotificationFrequency_FrequencyID]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserNotificationFrequency]'))
ALTER TABLE [dbo].[UserNotificationFrequency] DROP CONSTRAINT [FK_UserNotificationFrequency_FrequencyID]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserNotificationFrequency_UserID]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserNotificationFrequency]'))
ALTER TABLE [dbo].[UserNotificationFrequency] DROP CONSTRAINT [FK_UserNotificationFrequency_UserID]
GO


GO

/****** Object:  Table [dbo].[UserNotificationFrequency]    Script Date: 02/15/2013 11:06:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserNotificationFrequency]') AND type in (N'U'))
DROP TABLE [dbo].[UserNotificationFrequency]
GO



GO

/****** Object:  Table [dbo].[NotificationFrequency]    Script Date: 02/15/2013 09:25:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationFrequency]') AND type in (N'U'))
DROP TABLE [dbo].[NotificationFrequency]
GO


GO

/****** Object:  Table [dbo].[EventTypes]    Script Date: 02/15/2013 09:19:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTypes]') AND type in (N'U'))
DROP TABLE [dbo].[EventTypes]
GO


/****** Object:  Table [dbo].[Notifications]    Script Date: 02/15/2013 09:26:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notifications]') AND type in (N'U'))
DROP TABLE [dbo].[Notifications]
GO


/****** Object:  Table [dbo].[QueryCategory]    Script Date: 02/15/2013 09:29:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QueryCategory]') AND type in (N'U'))
DROP TABLE [dbo].[QueryCategory]
GO

/****** Object:  StoredProcedure [dbo].[GetUserDatamartForQueryType]    Script Date: 02/15/2013 11:43:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserDatamartForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetUserDatamartForQueryType]
GO

/****** Object:  StoredProcedure [dbo].[uspAddDataMartToQuery]    Script Date: 02/15/2013 11:45:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddDataMartToQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddDataMartToQuery]
GO

/****** Object:  StoredProcedure [dbo].[uspAddDocumentResponseMap]    Script Date: 02/15/2013 11:45:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddDocumentResponseMap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddDocumentResponseMap]
GO

/****** Object:  StoredProcedure [dbo].[uspAddDocumentToResponse]    Script Date: 02/15/2013 11:45:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddDocumentToResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddDocumentToResponse]
GO

/****** Object:  StoredProcedure [dbo].[uspAddQueryDataMarts]    Script Date: 02/15/2013 11:45:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddQueryDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddQueryDataMarts]
GO

/****** Object:  StoredProcedure [dbo].[uspAddQueryDocumentMap]    Script Date: 02/15/2013 11:45:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddQueryDocumentMap]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddQueryDocumentMap]
GO

/****** Object:  StoredProcedure [dbo].[uspAddRightsToRole]    Script Date: 02/15/2013 11:46:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddRightsToRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddRightsToRole]
GO

/****** Object:  StoredProcedure [dbo].[uspAddRightToRole]    Script Date: 02/15/2013 11:46:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddRightToRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddRightToRole]
GO

/****** Object:  StoredProcedure [dbo].[uspAdminUsersForOrganizationGroup]    Script Date: 02/15/2013 11:46:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAdminUsersForOrganizationGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAdminUsersForOrganizationGroup]
GO

/****** Object:  StoredProcedure [dbo].[uspApproveResponse]    Script Date: 02/15/2013 11:46:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspApproveResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspApproveResponse]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspAuthenticateUser]    Script Date: 02/15/2013 11:46:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAuthenticateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAuthenticateUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCheckUserRight]    Script Date: 02/15/2013 11:47:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCheckUserRight]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCheckUserRight]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCleanUpData]    Script Date: 02/15/2013 11:47:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCleanUpData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCleanUpData]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateDMClientUpdateEvent]    Script Date: 02/15/2013 11:47:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateDMClientUpdateEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateDMClientUpdateEvent]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateEntityAddedRemovedEvent]    Script Date: 02/15/2013 11:47:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateEntityAddedRemovedEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateEntityAddedRemovedEvent]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewEvent]    Script Date: 02/15/2013 11:47:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewEvent]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewEventForNetworkMessage]    Script Date: 02/15/2013 11:47:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewEventForNetworkMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewEventForNetworkMessage]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewEventForQueryAdmin]    Script Date: 02/15/2013 11:47:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewEventForQueryAdmin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewEventForQueryAdmin]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewGroupResult]    Script Date: 02/15/2013 11:47:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewGroupResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewGroupResult]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewResult]    Script Date: 02/15/2013 11:48:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewResult]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateNewResultForOrganizationEvent]    Script Date: 02/15/2013 11:48:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateNewResultForOrganizationEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateNewResultForOrganizationEvent]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateQuery]    Script Date: 02/15/2013 11:49:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateQuery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateQueryTypeDatamartAssociationEvent]    Script Date: 02/15/2013 11:49:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateQueryTypeDatamartAssociationEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateQueryTypeDatamartAssociationEvent]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateReminderEvent]    Script Date: 02/15/2013 11:49:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateReminderEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateReminderEvent]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspCreateResponse]    Script Date: 02/15/2013 11:49:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateResponse]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspCreateResult]    Script Date: 02/15/2013 11:49:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateResult]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateResult]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspCreateSubmitterReminderEvent]    Script Date: 02/15/2013 11:49:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateSubmitterReminderEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateSubmitterReminderEvent]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspCreateUser]    Script Date: 02/15/2013 11:49:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspCreateUserUpdatedEvent]    Script Date: 02/15/2013 11:50:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspCreateUserUpdatedEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspCreateUserUpdatedEvent]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspDeleteDataMart]    Script Date: 02/15/2013 11:50:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteDataMart]
GO


GO

/****** Object:  StoredProcedure [dbo].[UspDeleteDocumentsByIds]    Script Date: 02/15/2013 11:50:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspDeleteDocumentsByIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspDeleteDocumentsByIds]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteExpiredFiles]    Script Date: 02/15/2013 11:50:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteExpiredFiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteExpiredFiles]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspDeleteExpiredQueries]    Script Date: 02/15/2013 11:50:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteExpiredQueries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteExpiredQueries]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteGroup]    Script Date: 02/15/2013 11:50:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteGroup]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteOrganization]    Script Date: 02/15/2013 11:50:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteOrganization]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspDeleteQuery]    Script Date: 02/15/2013 11:50:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteQuery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteQueryDataMarts]    Script Date: 02/15/2013 11:50:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteQueryDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteQueryDataMarts]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteResponse]    Script Date: 02/15/2013 11:51:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteResponse]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteUnMappedDocuments]    Script Date: 02/15/2013 11:51:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteUnMappedDocuments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteUnMappedDocuments]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspDeleteUser]    Script Date: 02/15/2013 11:51:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspDeleteUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspDeleteUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAdministrativeUsersForGroupApproval]    Script Date: 02/15/2013 11:53:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAdministrativeUsersForGroupApproval]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAdministrativeUsersForGroupApproval]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAdministrativeUsersForQueryApproval]    Script Date: 02/15/2013 11:53:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAdministrativeUsersForQueryApproval]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAdministrativeUsersForQueryApproval]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAgeGroups]    Script Date: 02/15/2013 11:53:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAgeGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAgeGroups]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAgeStartificationMappings]    Script Date: 02/15/2013 11:53:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAgeStartificationMappings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAgeStartificationMappings]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetAggregatedResultsByQueryId]    Script Date: 02/15/2013 11:53:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAggregatedResultsByQueryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAggregatedResultsByQueryId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllActiveUsers]    Script Date: 02/15/2013 11:53:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllActiveUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllActiveUsers]
GO




GO

/****** Object:  StoredProcedure [dbo].[uspGetAllDataMarts]    Script Date: 02/15/2013 11:54:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllDataMarts]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroupedQueryDataMartsByQueryId]    Script Date: 02/15/2013 11:54:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroupedQueryDataMartsByQueryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroupedQueryDataMartsByQueryId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroupForDatamart]    Script Date: 02/15/2013 11:54:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroupForDatamart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroupForDatamart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroupForQueryType]    Script Date: 02/15/2013 11:54:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroupForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroupForQueryType]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroups]    Script Date: 02/15/2013 11:54:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroups]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroupsForOrganization]    Script Date: 02/15/2013 11:54:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroupsForOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroupsForOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllGroupWithRight]    Script Date: 02/15/2013 11:55:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllGroupWithRight]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllGroupWithRight]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllNotificationDataforDelivery]    Script Date: 02/15/2013 11:55:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllNotificationDataforDelivery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllNotificationDataforDelivery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllNotifications]    Script Date: 02/15/2013 11:55:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllNotifications]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllNotifications]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationByGroupName]    Script Date: 02/15/2013 11:55:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationByGroupName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationByGroupName]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationForDatamart]    Script Date: 02/15/2013 11:55:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationForDatamart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationForDatamart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationForQueryType]    Script Date: 02/15/2013 11:55:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationForQueryType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizations]    Script Date: 02/15/2013 11:55:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizations]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationsForGroup]    Script Date: 02/15/2013 11:55:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationsForGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationsForGroup]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationsWithGroups]    Script Date: 02/15/2013 11:56:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationsWithGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationsWithGroups]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationUsersWithAdminRights]    Script Date: 02/15/2013 11:56:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationUsersWithAdminRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationUsersWithAdminRights]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllOrganizationWithRight]    Script Date: 02/15/2013 11:56:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllOrganizationWithRight]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllOrganizationWithRight]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetAllQueries]    Script Date: 02/15/2013 11:56:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllQueries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllQueries]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllQueriesForUser]    Script Date: 02/15/2013 11:56:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllQueriesForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllQueriesForUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllQueryStatusTypes]    Script Date: 02/15/2013 11:56:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllQueryStatusTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllQueryStatusTypes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllQueryTypes]    Script Date: 02/15/2013 11:56:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllQueryTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllQueryTypes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllResponsesByQueryId]    Script Date: 02/15/2013 11:57:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllResponsesByQueryId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllResponsesByQueryId]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetAllRights]    Script Date: 02/15/2013 11:57:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllRights]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllRoleTypes]    Script Date: 02/15/2013 11:57:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllRoleTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllRoleTypes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllSummaryNotificationsForDelivery]    Script Date: 02/15/2013 11:57:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllSummaryNotificationsForDelivery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllSummaryNotificationsForDelivery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllUserForDatamart]    Script Date: 02/15/2013 11:57:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllUserForDatamart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllUserForDatamart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllUserForQueryType]    Script Date: 02/15/2013 11:57:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllUserForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllUserForQueryType]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetAllUsers]    Script Date: 02/15/2013 11:57:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAllUserWithRight]    Script Date: 02/15/2013 11:57:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAllUserWithRight]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAllUserWithRight]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAvailableDataMartTypes]    Script Date: 02/15/2013 11:57:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAvailableDataMartTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAvailableDataMartTypes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetAvailablePeriodBycategry]    Script Date: 02/15/2013 11:58:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetAvailablePeriodBycategry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetAvailablePeriodBycategry]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetCachedResults]    Script Date: 02/15/2013 11:58:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetCachedResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetCachedResults]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetCumulativeGroupRightsForOrganization]    Script Date: 02/15/2013 11:58:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetCumulativeGroupRightsForOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetCumulativeGroupRightsForOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMart]    Script Date: 02/15/2013 11:58:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartAuditReport]    Script Date: 02/15/2013 11:58:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartAuditReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartAuditReport]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartById]    Script Date: 02/15/2013 11:58:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartById]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForQueryType]    Script Date: 02/15/2013 11:58:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForQueryType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForQueryTypePeriod]    Script Date: 02/15/2013 11:58:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForQueryTypePeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForQueryTypePeriod]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForUserGroup]    Script Date: 02/15/2013 11:58:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForUserGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForUserGroup]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForUserGroupAndQueryTypePeriod]    Script Date: 02/15/2013 11:59:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForUserGroupAndQueryTypePeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForUserGroupAndQueryTypePeriod]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForUserOrganization]    Script Date: 02/15/2013 11:59:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForUserOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForUserOrganization]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartForUserOrganizationAndQueryTypePeriod]    Script Date: 02/15/2013 11:59:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartForUserOrganizationAndQueryTypePeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartForUserOrganizationAndQueryTypePeriod]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartListWithPermissionsForGroup]    Script Date: 02/15/2013 11:59:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartListWithPermissionsForGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartListWithPermissionsForGroup]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartListWithPermissionsForOrganization]    Script Date: 02/15/2013 11:59:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartListWithPermissionsForOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartListWithPermissionsForOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartListWithPermissionsForUser]    Script Date: 02/15/2013 11:59:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartListWithPermissionsForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartListWithPermissionsForUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartPeriodsByQueryType]    Script Date: 02/15/2013 11:59:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartPeriodsByQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartPeriodsByQueryType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsByAdminUserId]    Script Date: 02/15/2013 11:59:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsByAdminUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsByAdminUserId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsByIds]    Script Date: 02/15/2013 11:59:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsByIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsByIds]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartsForNotification]    Script Date: 02/15/2013 12:00:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartsForNotification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartsForNotification]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsForQueryType]    Script Date: 02/15/2013 12:00:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsForQueryType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsForQueryTypePeriod]    Script Date: 02/15/2013 12:00:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsForQueryTypePeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsForQueryTypePeriod]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetDatamartsForResultsViewed]    Script Date: 02/15/2013 12:00:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDatamartsForResultsViewed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDatamartsForResultsViewed]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsForUser]    Script Date: 02/15/2013 12:00:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsForUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDataMartsIncludedInQuery]    Script Date: 02/15/2013 12:00:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDataMartsIncludedInQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDataMartsIncludedInQuery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDaysforPasswordExpiry]    Script Date: 02/15/2013 12:00:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDaysforPasswordExpiry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDaysforPasswordExpiry]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetDocument]    Script Date: 02/15/2013 12:00:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetDocument]
GO


GO

/****** Object:  StoredProcedure [dbo].[UspGetDocumentsByIds]    Script Date: 02/15/2013 12:00:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetDocumentsByIds]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetDocumentsByIds]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetEntities]    Script Date: 02/15/2013 12:03:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetEntities]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetEntities]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetEntitiesByType]    Script Date: 02/15/2013 12:03:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetEntitiesByType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetEntitiesByType]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetEventById]    Script Date: 02/15/2013 12:03:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetEventById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetEventById]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetFrequencyListForEventType]    Script Date: 02/15/2013 12:03:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetFrequencyListForEventType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetFrequencyListForEventType]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetGender]    Script Date: 02/15/2013 12:04:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGender]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGender]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupAdministrators]    Script Date: 02/15/2013 12:04:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupAdministrators]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupAdministrators]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupById]    Script Date: 02/15/2013 12:04:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupById]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupDataMartPermissions]    Script Date: 02/15/2013 12:04:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupDataMartPermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupDataMartPermissions]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupedQueryResultStatus]    Script Date: 02/15/2013 12:04:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupedQueryResultStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupedQueryResultStatus]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupOrganizations]    Script Date: 02/15/2013 12:04:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupOrganizations]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupOrganizationsByUserOrganization]    Script Date: 02/15/2013 12:04:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupOrganizationsByUserOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupOrganizationsByUserOrganization]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupQueryTyePermissions]    Script Date: 02/15/2013 12:04:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupQueryTyePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupQueryTyePermissions]
GO





GO

/****** Object:  StoredProcedure [dbo].[uspGetGroupRights]    Script Date: 02/15/2013 12:04:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetGroupRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetGroupRights]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetLatestFeatures]    Script Date: 02/15/2013 12:05:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetLatestFeatures]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetLatestFeatures]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetMFUOutputCritieriaList]    Script Date: 02/15/2013 12:05:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetMFUOutputCritieriaList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetMFUOutputCritieriaList]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationOptionsByUser]    Script Date: 02/15/2013 12:05:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationOptionsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationOptionsByUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationProcessingSetting]    Script Date: 02/15/2013 12:05:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationProcessingSetting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationProcessingSetting]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationsByQueryDataMart]    Script Date: 02/15/2013 12:05:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationsByQueryDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationsByQueryDataMart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationsByUser]    Script Date: 02/15/2013 12:05:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationsByUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationsByUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationSecureGUID]    Script Date: 02/15/2013 12:05:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationSecureGUID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationSecureGUID]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetNotificationUserIdsForDataMart]    Script Date: 02/15/2013 12:05:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetNotificationUserIdsForDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetNotificationUserIdsForDataMart]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetObserversByOrganization]    Script Date: 02/15/2013 12:05:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetObserversByOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetObserversByOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetObserversByUserId]    Script Date: 02/15/2013 12:06:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetObserversByUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetObserversByUserId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetOrgAcronymForDataMarts]    Script Date: 02/15/2013 12:06:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrgAcronymForDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrgAcronymForDataMarts]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationAdministrators]    Script Date: 02/15/2013 12:06:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationAdministrators]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationAdministrators]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationById]    Script Date: 02/15/2013 12:06:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationById]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationDataMartPermissions]    Script Date: 02/15/2013 12:06:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationDataMartPermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationDataMartPermissions]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationQueryTyePermissions]    Script Date: 02/15/2013 12:06:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationQueryTyePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationQueryTyePermissions]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationRights]    Script Date: 02/15/2013 12:06:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationRights]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizations]    Script Date: 02/15/2013 12:06:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizations]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationsInGroupByUserOrganization]    Script Date: 02/15/2013 12:06:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationsInGroupByUserOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationsInGroupByUserOrganization]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetOrganizationUsers]    Script Date: 02/15/2013 12:06:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetOrganizationUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetOrganizationUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetPatientCodes]    Script Date: 02/15/2013 12:07:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetPatientCodes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetPatientCodes]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetPendingQueries]    Script Date: 02/15/2013 12:07:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetPendingQueries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetPendingQueries]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesByAdminUser]    Script Date: 02/15/2013 12:07:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesByAdminUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesByAdminUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesByAdminUserId]    Script Date: 02/15/2013 12:07:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesByAdminUserId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesByAdminUserId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesByDataMartId]    Script Date: 02/15/2013 12:07:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesByDataMartId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesByDataMartId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesByUserAndOrganization]    Script Date: 02/15/2013 12:07:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesByUserAndOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesByUserAndOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesByUserOrganization]    Script Date: 02/15/2013 12:07:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesByUserOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesByUserOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesForReminders]    Script Date: 02/15/2013 12:07:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesForReminders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesForReminders]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesForSubmitterReminders]    Script Date: 02/15/2013 12:07:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesForSubmitterReminders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesForSubmitterReminders]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesSubmittedByGroupUsers]    Script Date: 02/15/2013 12:07:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesSubmittedByGroupUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesSubmittedByGroupUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesSubmittedByOrganizationUsers]    Script Date: 02/15/2013 12:07:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesSubmittedByOrganizationUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesSubmittedByOrganizationUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueriesSubmittedToGroupAdmin]    Script Date: 02/15/2013 12:07:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueriesSubmittedToGroupAdmin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueriesSubmittedToGroupAdmin]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQuery]    Script Date: 02/15/2013 12:08:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQuery]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryAttributes]    Script Date: 02/15/2013 12:08:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryAttributes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryAttributes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryByEventId]    Script Date: 02/15/2013 12:08:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryByEventId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryByEventId]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryCategories]    Script Date: 02/15/2013 12:08:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryCategories]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryDataMart]    Script Date: 02/15/2013 12:08:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryDataMart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryDatamartsIncludedInQueryGroupResults]    Script Date: 02/15/2013 12:08:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryDatamartsIncludedInQueryGroupResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryDatamartsIncludedInQueryGroupResults]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryDatamartsIncludedInResults]    Script Date: 02/15/2013 12:08:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryDatamartsIncludedInResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryDatamartsIncludedInResults]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryDocuments]    Script Date: 02/15/2013 12:08:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryDocuments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryDocuments]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryGroupResponseById]    Script Date: 02/15/2013 12:08:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryGroupResponseById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryGroupResponseById]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryGroupStratification]    Script Date: 02/15/2013 12:08:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryGroupStratification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryGroupStratification]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryResultStatus]    Script Date: 02/15/2013 12:09:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryResultStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryResultStatus]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryStatus]    Script Date: 02/15/2013 12:09:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryStatus]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryTypeListWithPermissionsForGroup]    Script Date: 02/15/2013 12:09:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryTypeListWithPermissionsForGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryTypeListWithPermissionsForGroup]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryTypeListWithPermissionsForOrganization]    Script Date: 02/15/2013 12:09:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryTypeListWithPermissionsForOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryTypeListWithPermissionsForOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryTypeListWithPermissionsForUser]    Script Date: 02/15/2013 12:09:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryTypeListWithPermissionsForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryTypeListWithPermissionsForUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryTypesForUser]    Script Date: 02/15/2013 12:09:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryTypesForUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryTypesForUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetQueryTypesSupportedByDatamart]    Script Date: 02/15/2013 12:09:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetQueryTypesSupportedByDatamart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetQueryTypesSupportedByDatamart]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetResponseDocuments]    Script Date: 02/16/2013 09:17:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetResponseDocuments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetResponseDocuments]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetResultStatus]    Script Date: 02/16/2013 09:17:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetResultStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetResultStatus]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetRightById]    Script Date: 02/16/2013 09:18:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetRightById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetRightById]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetRightsForRole]    Script Date: 02/16/2013 09:18:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetRightsForRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetRightsForRole]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetRoleTypeById]    Script Date: 02/16/2013 09:18:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetRoleTypeById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetRoleTypeById]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetSelfAdministeredDatamartForQueryTypeandPeriod]    Script Date: 02/16/2013 09:18:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetSelfAdministeredDatamartForQueryTypeandPeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetSelfAdministeredDatamartForQueryTypeandPeriod]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetSelfAdministeredDatamarts]    Script Date: 02/16/2013 09:18:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetSelfAdministeredDatamarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetSelfAdministeredDatamarts]
GO





GO

/****** Object:  StoredProcedure [dbo].[uspGetSiteTheme]    Script Date: 02/16/2013 09:18:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetSiteTheme]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetSiteTheme]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetStratificationCategoryList]    Script Date: 02/16/2013 09:18:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetStratificationCategoryList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetStratificationCategoryList]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetSupportedQueryTypesByQuery]    Script Date: 02/16/2013 09:18:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetSupportedQueryTypesByQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetSupportedQueryTypesByQuery]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetSupportedQueryTypesForDataMart]    Script Date: 02/16/2013 09:18:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetSupportedQueryTypesForDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetSupportedQueryTypesForDataMart]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetUser]    Script Date: 02/16/2013 09:18:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUser]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetUserDatamartForQueryType]    Script Date: 02/16/2013 09:19:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUserDatamartForQueryType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUserDatamartForQueryType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetUserDataMartPermissions]    Script Date: 02/16/2013 09:19:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUserDataMartPermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUserDataMartPermissions]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetUserDatamartsForQueryTypeNotInQuery]    Script Date: 02/16/2013 09:19:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUserDatamartsForQueryTypeNotInQuery]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUserDatamartsForQueryTypeNotInQuery]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspGetUserQueryTyePermissions]    Script Date: 02/16/2013 09:19:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUserQueryTyePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUserQueryTyePermissions]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetUserRights]    Script Date: 02/16/2013 09:19:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUserRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUserRights]
GO


GO

/****** Object:  StoredProcedure [dbo].[UspGetUsersByRoleType]    Script Date: 02/16/2013 09:19:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspGetUsersByRoleType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspGetUsersByRoleType]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetUsersForDMClientUpdateNotification]    Script Date: 02/16/2013 09:19:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetUsersForDMClientUpdateNotification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetUsersForDMClientUpdateNotification]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGrantDatamartForGroups]    Script Date: 02/16/2013 09:19:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantDatamartForGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantDatamartForGroups]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGrantDatamartForOrganizations]    Script Date: 02/16/2013 09:19:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantDatamartForOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantDatamartForOrganizations]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantDatamartForUsers]    Script Date: 02/16/2013 09:20:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantDatamartForUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantDatamartForUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantGroupDataMartPermission]    Script Date: 02/16/2013 09:20:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantGroupDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantGroupDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantGroupQueryTypePermission]    Script Date: 02/16/2013 09:20:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantGroupQueryTypePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantGroupQueryTypePermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantGroupRightPermission]    Script Date: 02/16/2013 09:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantGroupRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantGroupRightPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantOrganizationDataMartPermission]    Script Date: 02/16/2013 09:20:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantOrganizationDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantOrganizationDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantOrganizationRightPermission]    Script Date: 02/16/2013 09:20:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantOrganizationRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantOrganizationRightPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantQueryTypeForGroups]    Script Date: 02/16/2013 09:20:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantQueryTypeForGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantQueryTypeForGroups]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantQueryTypeForOrganizations]    Script Date: 02/16/2013 09:20:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantQueryTypeForOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantQueryTypeForOrganizations]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantQueryTypeForUsers]    Script Date: 02/16/2013 09:20:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantQueryTypeForUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantQueryTypeForUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantRightsToGroup]    Script Date: 02/16/2013 09:20:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantRightsToGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantRightsToGroup]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantRightsToOrganization]    Script Date: 02/16/2013 09:20:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantRightsToOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantRightsToOrganization]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantUserDataMartPermission]    Script Date: 02/16/2013 09:21:02 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantUserDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantUserDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantUserQueryTypePermission]    Script Date: 02/16/2013 09:21:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantUserQueryTypePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantUserQueryTypePermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGrantUserRightPermission]    Script Date: 02/16/2013 09:21:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGrantUserRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGrantUserRightPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGroupAndApproveResponse]    Script Date: 02/16/2013 09:21:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGroupAndApproveResponse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGroupAndApproveResponse]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspIsQueryResultViewed]    Script Date: 02/16/2013 09:21:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspIsQueryResultViewed]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspIsQueryResultViewed]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspRemoveRightFromRole]    Script Date: 02/16/2013 09:22:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRemoveRightFromRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRemoveRightFromRole]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspResetPassword]    Script Date: 02/16/2013 09:22:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspResetPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspResetPassword]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeGroupDataMartPermission]    Script Date: 02/16/2013 09:22:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeGroupDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeGroupDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeGroupQueryTypePermission]    Script Date: 02/16/2013 09:22:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeGroupQueryTypePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeGroupQueryTypePermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeGroupRightPermission]    Script Date: 02/16/2013 09:22:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeGroupRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeGroupRightPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeOrganizationDataMartPermission]    Script Date: 02/16/2013 09:22:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeOrganizationDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeOrganizationDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeOrganizationRightPermission]    Script Date: 02/16/2013 09:22:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeOrganizationRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeOrganizationRightPermission]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspRevokeUserDataMartPermission]    Script Date: 02/16/2013 09:22:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeUserDataMartPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeUserDataMartPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeUserQueryTypePermission]    Script Date: 02/16/2013 09:22:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeUserQueryTypePermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeUserQueryTypePermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRevokeUserRightPermission]    Script Date: 02/16/2013 09:22:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRevokeUserRightPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRevokeUserRightPermission]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspRunOnceInitialPasswordEncryption]    Script Date: 02/16/2013 09:22:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRunOnceInitialPasswordEncryption]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRunOnceInitialPasswordEncryption]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspSaveAggregatedResults]    Script Date: 02/16/2013 09:23:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveAggregatedResults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveAggregatedResults]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveAllEventTypeNotificationOption]    Script Date: 02/16/2013 09:23:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveAllEventTypeNotificationOption]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveAllEventTypeNotificationOption]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveDataMarts]    Script Date: 02/16/2013 09:23:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveDataMarts]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveDocument]    Script Date: 02/16/2013 09:23:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveDocument]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveDocument]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveEntityDataMarts]    Script Date: 02/16/2013 09:23:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveEntityDataMarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveEntityDataMarts]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveEntityQueryTypes]    Script Date: 02/16/2013 09:23:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveEntityQueryTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveEntityQueryTypes]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveGroup]    Script Date: 02/16/2013 09:23:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveGroup]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspSaveNotification]    Script Date: 02/16/2013 09:23:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveNotification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveNotification]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveNotificationOption]    Script Date: 02/16/2013 09:23:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveNotificationOption]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveNotificationOption]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveNotificationProcessingConfig]    Script Date: 02/16/2013 09:23:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveNotificationProcessingConfig]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveNotificationProcessingConfig]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveOrganization]    Script Date: 02/16/2013 09:24:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveOrganization]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveOrganization]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspSaveQueryGroupStratification]    Script Date: 02/16/2013 09:24:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveQueryGroupStratification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveQueryGroupStratification]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveResultViewedStatus]    Script Date: 02/16/2013 09:24:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveResultViewedStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveResultViewedStatus]
GO



GO

/****** Object:  StoredProcedure [dbo].[uspSaveRightForGroups]    Script Date: 02/16/2013 09:24:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveRightForGroups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveRightForGroups]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspSaveRightForOrganizations]    Script Date: 02/16/2013 09:24:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveRightForOrganizations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveRightForOrganizations]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspSaveRightForUsers]    Script Date: 02/16/2013 09:24:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveRightForUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveRightForUsers]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSaveRole]    Script Date: 02/16/2013 09:24:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSaveRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSaveRole]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSetEncryptedUserPassword]    Script Date: 02/16/2013 09:25:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSetEncryptedUserPassword]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSetEncryptedUserPassword]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspSetQueryStatus]    Script Date: 02/16/2013 09:25:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSetQueryStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSetQueryStatus]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSetQueryStatusChange]    Script Date: 02/16/2013 09:25:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSetQueryStatusChange]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSetQueryStatusChange]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspSetResultViewedEventAndDetail]    Script Date: 02/16/2013 09:25:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSetResultViewedEventAndDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSetResultViewedEventAndDetail]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspUpdateNotificationSecureGUID]    Script Date: 02/16/2013 09:25:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateNotificationSecureGUID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateNotificationSecureGUID]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspUpdateQueryComment]    Script Date: 02/16/2013 09:25:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateQueryComment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateQueryComment]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspUpdateQueryDataMart]    Script Date: 02/16/2013 09:25:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateQueryDataMart]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateQueryDataMart]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspUpdateQueryTypeAvailabilityPeriod]    Script Date: 02/16/2013 09:25:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateQueryTypeAvailabilityPeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateQueryTypeAvailabilityPeriod]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspUpdateUser]    Script Date: 02/16/2013 09:25:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateUser]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspUpdateUserRole]    Script Date: 02/16/2013 09:25:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspUpdateUserRole]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspUpdateUserRole]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetLookUpValues]    Script Date: 02/16/2013 09:28:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetLookUpValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetLookUpValues]
GO


GO

/****** Object:  StoredProcedure [dbo].[uspGetLookupMetricsList]    Script Date: 02/16/2013 09:29:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetLookupMetricsList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetLookupMetricsList]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetLookupListCategories]    Script Date: 02/16/2013 09:30:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetLookupListCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetLookupListCategories]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetICD9Diagnosis]    Script Date: 02/16/2013 09:30:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetICD9Diagnosis]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetICD9Diagnosis]
GO

GO

/****** Object:  StoredProcedure [dbo].[uspGetHCPCSProcedures]    Script Date: 02/16/2013 09:31:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspGetHCPCSProcedures]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspGetHCPCSProcedures]
GO


GO

/****** Object:  StoredProcedure [dbo].[SetUserPermission]    Script Date: 02/16/2013 09:32:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SetUserPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SetUserPermission]
GO



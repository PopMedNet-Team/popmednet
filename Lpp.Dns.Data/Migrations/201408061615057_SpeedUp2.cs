namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUp2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE NONCLUSTERED INDEX [_dta_index_AclOrganizations_6_1905441862__K2_K4_K3_K1] ON [dbo].[AclOrganizations]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[OrganizationID] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclOrganizations_6_1905441862__K2_K3_K1_K4] ON [dbo].[AclOrganizations]
(
	[PermissionID] ASC,
	[OrganizationID] ASC,
	[SecurityGroupID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1905441862_4_3_1] ON [dbo].[AclOrganizations]([Allowed], [OrganizationID], [SecurityGroupID])


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_6_210099789__K24_K30_K35_K39_K37_K40_K19_K48_K46_K33_K34_K9_K42_1_3_6_11_13_15_16_18_20_22_23_26_27_28_29_] ON [dbo].[Requests]
(
	[ID] ASC,
	[ProjectId] ASC,
	[OrganizationID] ASC,
	[CreatedByID] ASC,
	[SubmittedByID] ASC,
	[UpdatedByID] ASC,
	[RequestTypeID] ASC,
	[CancelledByID] ASC,
	[RejectedByID] ASC,
	[RequesterCenterID] ASC,
	[WorkplanTypeID] ASC,
	[IsDeleted] ASC,
	[Status] ASC
)
INCLUDE ( 	[Identifier],
	[CreatedOn],
	[Name],
	[Priority],
	[ActivityDescription],
	[DueDate],
	[IRBApprovalNo],
	[IsScheduled],
	[SubmittedOn],
	[UpdatedOn],
	[IsTemplate],
	[PurposeOfUse],
	[PhiDisclosureLevel],
	[Schedule],
	[ScheduleCount],
	[Description],
	[ActivityID],
	[TimeStamp],
	[RejectedOn],
	[CancelledOn],
	[AdapterPackageVersion]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_6_210099789__K20D_K24_1_3_6_9_11_13_15_16_18_19_22_23_26_27_28_29_30_33_34_35_36_37_38_39_40_41_42_45_46_] ON [dbo].[Requests]
(
	[SubmittedOn] DESC,
	[ID] ASC
)
INCLUDE ( 	[Identifier],
	[CreatedOn],
	[Name],
	[IsDeleted],
	[Priority],
	[ActivityDescription],
	[DueDate],
	[IRBApprovalNo],
	[IsScheduled],
	[RequestTypeID],
	[UpdatedOn],
	[IsTemplate],
	[PurposeOfUse],
	[PhiDisclosureLevel],
	[Schedule],
	[ScheduleCount],
	[ProjectId],
	[RequesterCenterID],
	[WorkplanTypeID],
	[OrganizationID],
	[Description],
	[SubmittedByID],
	[ActivityID],
	[CreatedByID],
	[UpdatedByID],
	[TimeStamp],
	[Status],
	[RejectedOn],
	[RejectedByID],
	[CancelledOn],
	[CancelledByID],
	[AdapterPackageVersion]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_Requests_6_210099789__K9] ON [dbo].[Requests]
(
	[IsDeleted] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_210099789_1_9] ON [dbo].[Requests]([Identifier], [IsDeleted])


CREATE STATISTICS [_dta_stat_210099789_9_42] ON [dbo].[Requests]([IsDeleted], [Status])


CREATE STATISTICS [_dta_stat_210099789_33_24] ON [dbo].[Requests]([RequesterCenterID], [ID])


CREATE STATISTICS [_dta_stat_210099789_19_42_24] ON [dbo].[Requests]([RequestTypeID], [Status], [ID])


CREATE STATISTICS [_dta_stat_210099789_42_20_24] ON [dbo].[Requests]([Status], [SubmittedOn], [ID])


CREATE STATISTICS [_dta_stat_210099789_24_42_9_30] ON [dbo].[Requests]([ID], [Status], [IsDeleted], [ProjectId])


CREATE STATISTICS [_dta_stat_210099789_42_24_9_39] ON [dbo].[Requests]([Status], [ID], [IsDeleted], [CreatedByID])


CREATE STATISTICS [_dta_stat_210099789_35_30_42_24] ON [dbo].[Requests]([OrganizationID], [ProjectId], [Status], [ID])


CREATE STATISTICS [_dta_stat_210099789_24_9_37_39_40_46] ON [dbo].[Requests]([ID], [IsDeleted], [SubmittedByID], [CreatedByID], [UpdatedByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_24_9_40_39_48_46] ON [dbo].[Requests]([ID], [IsDeleted], [UpdatedByID], [CreatedByID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_39_37_42_24_9_30] ON [dbo].[Requests]([CreatedByID], [SubmittedByID], [Status], [ID], [IsDeleted], [ProjectId])


CREATE STATISTICS [_dta_stat_210099789_40_39_48_46_37_42_24] ON [dbo].[Requests]([UpdatedByID], [CreatedByID], [CancelledByID], [RejectedByID], [SubmittedByID], [Status], [ID])


CREATE STATISTICS [_dta_stat_210099789_42_24_9_37_39_40_46] ON [dbo].[Requests]([Status], [ID], [IsDeleted], [SubmittedByID], [CreatedByID], [UpdatedByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_24_9_30_35_39_37_40] ON [dbo].[Requests]([ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID])


CREATE STATISTICS [_dta_stat_210099789_24_35_9_42_30_39_37] ON [dbo].[Requests]([ID], [OrganizationID], [IsDeleted], [Status], [ProjectId], [CreatedByID], [SubmittedByID])


CREATE STATISTICS [_dta_stat_210099789_35_42_24_9_30_39_37_40] ON [dbo].[Requests]([OrganizationID], [Status], [ID], [IsDeleted], [ProjectId], [CreatedByID], [SubmittedByID], [UpdatedByID])


CREATE STATISTICS [_dta_stat_210099789_40_39_48_46_37_24_9_30_35] ON [dbo].[Requests]([UpdatedByID], [CreatedByID], [CancelledByID], [RejectedByID], [SubmittedByID], [ID], [IsDeleted], [ProjectId], [OrganizationID])


CREATE STATISTICS [_dta_stat_210099789_42_24_9_40_39_48_46_37_30_35] ON [dbo].[Requests]([Status], [ID], [IsDeleted], [UpdatedByID], [CreatedByID], [CancelledByID], [RejectedByID], [SubmittedByID], [ProjectId], [OrganizationID])


CREATE STATISTICS [_dta_stat_210099789_24_9_33_30_35_39_37_40_19_48] ON [dbo].[Requests]([ID], [IsDeleted], [RequesterCenterID], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [RequestTypeID], [CancelledByID])


CREATE STATISTICS [_dta_stat_210099789_34_24_9_30_35_39_37_40_19_48_46] ON [dbo].[Requests]([WorkplanTypeID], [ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [RequestTypeID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_33_42_24_9_30_35_39_37_40_19_48] ON [dbo].[Requests]([RequesterCenterID], [Status], [ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [RequestTypeID], [CancelledByID])


CREATE STATISTICS [_dta_stat_210099789_34_42_24_9_30_35_39_37_40_19_48_46] ON [dbo].[Requests]([WorkplanTypeID], [Status], [ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [RequestTypeID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_42_24_9_19_30_35_39_37_40_48_46_33_34] ON [dbo].[Requests]([Status], [ID], [IsDeleted], [RequestTypeID], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [CancelledByID], [RejectedByID], [RequesterCenterID], [WorkplanTypeID])


CREATE STATISTICS [_dta_stat_210099789_19_24_9_30_35_39_37_40_48_46_33_34_20] ON [dbo].[Requests]([RequestTypeID], [ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [CancelledByID], [RejectedByID], [RequesterCenterID], [WorkplanTypeID], [SubmittedOn])


CREATE STATISTICS [_dta_stat_210099789_30_42_24_9_35_39_37_40_19_48_46_33_34_20] ON [dbo].[Requests]([ProjectId], [Status], [ID], [IsDeleted], [OrganizationID], [CreatedByID], [SubmittedByID], [UpdatedByID], [RequestTypeID], [CancelledByID], [RejectedByID], [RequesterCenterID], [WorkplanTypeID], [SubmittedOn])


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Organizations_6_245575913__K3_K6_K49_2_8_9_10_11_12_13_19_20_21_23_24_26_27_28_29_30_31_32_33_34_35_36_37_38_39_40_] ON [dbo].[Organizations]
(
	[IsDeleted] ASC,
	[ID] ASC,
	[ParentOrganizationID] ASC
)
INCLUDE ( 	[Name],
	[ContactEmail],
	[ContactFirstName],
	[ContactLastName],
	[ContactPhone],
	[SpecialRequirements],
	[UsageRestrictions],
	[InpatientClaims],
	[OutpatientClaims],
	[OutpatientPharmacyClaims],
	[ObservationalParticipation],
	[ProspectiveTrials],
	[EnrollmentClaims],
	[DemographicsClaims],
	[LaboratoryResultsClaims],
	[VitalSignsClaims],
	[OtherClaimsText],
	[EnableClaimsAndBilling],
	[EnableEHRA],
	[EnableRegistries],
	[DataModelMSCDM],
	[DataModelHMORNVDW],
	[DataModelESP],
	[DataModelI2B2],
	[DataModelOtherText],
	[Primary],
	[X509PublicKey],
	[PragmaticClinicalTrials],
	[Biorepositories],
	[PatientReportedOutcomes],
	[PatientReportedBehaviors],
	[PrescriptionOrders],
	[OrganizationDescription],
	[DataModelOMOP],
	[Acronym],
	[OtherClaims],
	[DataModelOther]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Organizations_6_245575913__K6_2] ON [dbo].[Organizations]
(
	[ID] ASC
)
INCLUDE ( 	[Name]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_Organizations_6_245575913__K3_K6] ON [dbo].[Organizations]
(
	[IsDeleted] ASC,
	[ID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_245575913_2_6] ON [dbo].[Organizations]([Name], [ID])


CREATE STATISTICS [_dta_stat_245575913_6_49] ON [dbo].[Organizations]([ID], [ParentOrganizationID])


CREATE STATISTICS [_dta_stat_245575913_49_3_6] ON [dbo].[Organizations]([ParentOrganizationID], [IsDeleted], [ID])


CREATE NONCLUSTERED INDEX [_dta_index_AclProjects_6_1937441976__K2_K3_K4_K1] ON [dbo].[AclProjects]
(
	[PermissionID] ASC,
	[ProjectID] ASC,
	[Allowed] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_477960779_3_5] ON [dbo].[AclProjectDataMarts]([ProjectID], [Allowed])


CREATE STATISTICS [_dta_stat_1498488417_4_3_5] ON [dbo].[AclProjectOrganizations]([OrganizationID], [ProjectID], [Allowed])


CREATE STATISTICS [_dta_stat_1498488417_2_5_4_1] ON [dbo].[AclProjectOrganizations]([PermissionID], [Allowed], [OrganizationID], [SecurityGroupID])


");
        }
        
        public override void Down()
        {
        }
    }
}

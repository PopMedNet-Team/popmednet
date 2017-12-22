namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUpProjectScreenLoad : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE NONCLUSTERED INDEX [_dta_index_AclProjects_24_473104776__K3_K1_K2_4_5] ON [dbo].[AclProjects]
(
	[ProjectID] ASC,
	[SecurityGroupID] ASC,
	[PermissionID] ASC
)
INCLUDE ( 	[Allowed],
	[Overridden]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_SecurityGroups_24_1801773476__K9_K8_K2_1_4_5_7_10] ON [dbo].[SecurityGroups]
(
	[Type] ASC,
	[OwnerID] ASC,
	[Name] ASC
)
INCLUDE ( 	[ID],
	[Kind],
	[ParentSecurityGroupID],
	[Timestamp],
	[Path]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_SecurityGroups_24_1801773476__K9_K8] ON [dbo].[SecurityGroups]
(
	[Type] ASC,
	[OwnerID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_SecurityGroups_24_1801773476__K9] ON [dbo].[SecurityGroups]
(
	[Type] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_SecurityGroups_24_1801773476__K1_10] ON [dbo].[SecurityGroups]
(
	[ID] ASC
)
INCLUDE ( 	[Path]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectDataMarts_24_1897109849__K3_K4_K2_K1_5_6] ON [dbo].[AclProjectDataMarts]
(
	[ProjectID] ASC,
	[DataMartID] ASC,
	[PermissionID] ASC,
	[SecurityGroupID] ASC
)
INCLUDE ( 	[Allowed],
	[Overridden]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
");
        }
        
        public override void Down()
        {
        }
    }
}

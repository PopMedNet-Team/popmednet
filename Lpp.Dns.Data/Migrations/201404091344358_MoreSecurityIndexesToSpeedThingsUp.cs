namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreSecurityIndexesToSpeedThingsUp : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE NONCLUSTERED INDEX ID3SubjectIDPrivilegeIDExplicitDeniedEntriesID1
ON [dbo].[Security_Tuple3] ([ID3],[SubjectID],[PrivilegeID],[ExplicitDeniedEntries],[ID1])
INCLUDE ([ID2])");

            Sql(@"CREATE NONCLUSTERED INDEX ID2SubjectIDPrivilegeIDExplicitDeniedEntries
ON [dbo].[Security_Tuple2] ([ID2],[SubjectID],[PrivilegeID],[ExplicitDeniedEntries])
INCLUDE ([ID1])");
        }
        
        public override void Down()
        {
        }
    }
}

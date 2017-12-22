namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGlobalEventTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER  TRIGGER [dbo].[GlobalEvents_InsertItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [GlobalEvents] (SecurityGroupID, EventID, Allowed) SELECT SecurityGroups.ID, inserted.EventID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [GlobalEvents] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.EventID = inserted.EventID)
						END
                    END");

            Sql(@"ALTER  TRIGGER [dbo].[GlobalEvents_UpdateItem] 
                        ON  [dbo].[GlobalEvents]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [GlobalEvents] SET [GlobalEvents].Allowed = inserted.Allowed FROM [GlobalEvents] a INNER JOIN inserted ON a.EventID = inserted.EventID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE a.Overridden = 0
						END
                    END");
        }
        
        public override void Down()
        {
        }
    }
}

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRequestDataMartResponsesInsertTrigger : DbMigration
    {
        public override void Up()
        {
                Sql(@"ALTER TRIGGER [dbo].[RequestDataMartResponsesInsert] 
		    ON  [dbo].[RequestDataMartResponses]
		    AFTER INSERT
	    AS 
	    BEGIN
		    UPDATE RequestDataMartResponses SET Count = (SELECT COUNT(*) FROM RequestDataMartResponses r WHERE r.RequestDataMartID = RequestDataMartResponses.RequestDataMartID AND r.SubmittedOn < RequestDataMartResponses.SubmittedOn) + 1 WHERE RequestDataMartResponses.ID IN (SELECT ID FROM inserted)
	    END");
        }
        
        public override void Down()
        {
        }
    }
}

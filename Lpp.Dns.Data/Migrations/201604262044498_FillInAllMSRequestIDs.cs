namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillInAllMSRequestIDs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "MSRequestID", c => c.String(maxLength: 255));

            Sql(@"UPDATE REQUESTS 
                SET MSRequestID = Name
                WHERE MSRequestID IS NULL OR MSRequestID = '' ");

            Sql(@"UPDATE Requests SET MSRequestID = RTRIM(MSRequestID)+' - '+STR(Identifier)
                WHERE Identifier NOT IN (
	                SELECT MIN(Identifier)
	                FROM Requests
	                GROUP BY MSRequestID
                )");

        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "MSRequestID", c => c.String(maxLength: 100));
        }
    }
}

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteRequestTypeAclsOnDeleteOfParents : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER [dbo].[ProjectDataMarts_DeleteItem] 
    ON  [dbo].[ProjectDataMarts]
    AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DELETE a FROM [AclProjectDataMartRequestTypes] a INNER JOIN deleted ON a.ProjectID = deleted.ProjectId AND a.DataMartID = deleted.DataMartID
	DELETE a FROM [AclProjectDataMarts] a INNER JOIN deleted ON a.ProjectID = deleted.ProjectId AND a.DataMartID = deleted.DataMartID
END");
        }
        
        public override void Down()
        {
        }
    }
}

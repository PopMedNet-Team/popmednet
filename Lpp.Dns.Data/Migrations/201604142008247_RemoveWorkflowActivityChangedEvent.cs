namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveWorkflowActivityChangedEvent : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM [Events] WHERE ID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4'");

        }
        
        public override void Down()
        {

            Sql(@"IF NOT EXISTS(SELECT NULL FROM EVENTS WHERE ID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4' AND NAME = 'Request Workflow Activity Change')
INSERT INTO EVENTS (ID, NAME, DESCRIPTION) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 'Request Workflow Activity Change', 'Users will be notified whenever the Workflow Activity for a Request changes if they have permission to view the Request.')
");

        }
    }
}

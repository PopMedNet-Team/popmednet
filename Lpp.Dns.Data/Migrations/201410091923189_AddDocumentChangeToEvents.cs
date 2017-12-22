namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentChangeToEvents : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [Events] (ID, [Name], [Description]) VALUES ('F9C20001-E0C2-4996-B5CC-A3BF01301150', 'Document Change', 'Users will be notified whenever a change occurs to a document.')");

            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('F9C20001-E0C2-4996-B5CC-A3BF01301150', 3)");//org
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('F9C20001-E0C2-4996-B5CC-A3BF01301150', 4)");//proj
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('F9C20001-E0C2-4996-B5CC-A3BF01301150', 11)");//projdm

            //locations for Request.WorkflowActivityChanged
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 3)");
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 4)");
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('D2DC0001-43E9-477E-B60D-A3BE01550FA4', 11)");

            //locations for Task.Change
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 3)");
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 4)");
            Sql(@"INSERT INTO EventLocations (EventID, Location) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 11)");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM EventLocations WHERE EventID = 'F9C20001-E0C2-4996-B5CC-A3BF01301150' OR EventID = 'D2DC0001-43E9-477E-B60D-A3BE01550FA4' OR EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql(@"DELETE FROM [Events] WHERE ID = 'F9C20001-E0C2-4996-B5CC-A3BF01301150'");
        }
    }
}

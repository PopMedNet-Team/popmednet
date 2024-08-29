namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTaskChangeEventLocations : DbMigration
    {
        public override void Up()
        {
            //Remove ProjectDataMarts from Task.Change event.
            Sql("DELETE FROM ProjectDataMartEvents WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("DELETE FROM EventLocations WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396' AND Location = 11");

            //Add Users to Task.Change event locations
            Sql("IF(NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396' AND Location = 9)) INSERT INTO EventLocations (EventID, Location) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 9)");
        }
        
        public override void Down()
        {
            //Remove Users from Task.Change Event
            Sql("DELETE FROM UserEvents WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("DELETE FROM EventLocations WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396' AND Location = 9");

            //Add Users to Task.Change event locations
            Sql("IF(NOT EXISTS(SELECT NULL FROM EventLocations WHERE EventID = '2DFE0001-B98D-461D-A705-A3BE01411396' AND Location = 11)) INSERT INTO EventLocations (EventID, Location) VALUES ('2DFE0001-B98D-461D-A705-A3BE01411396', 11)");
        }
    }
}

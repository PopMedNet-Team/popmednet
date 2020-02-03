namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTaskChangeNotification : DbMigration
    {
        public override void Up()
        {
            Sql("delete from UserEventSubscriptions where EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("delete from UserEvents where EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("delete from ProjectEvents where EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("delete from OrganizationEvents where EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
            Sql("delete from EventLocations where EventID = '2DFE0001-B98D-461D-A705-A3BE01411396'");
        }
        
        public override void Down()
        {
            Sql("insert into EventLocations (EventID, Location) values ('2DFE0001-B98D-461D-A705-A3BE01411396', 3)");
            Sql("insert into EventLocations (EventID, Location) values ('2DFE0001-B98D-461D-A705-A3BE01411396', 4)");
            Sql("insert into EventLocations (EventID, Location) values ('2DFE0001-B98D-461D-A705-A3BE01411396', 9)");
        }
    }
}

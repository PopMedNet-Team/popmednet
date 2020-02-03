namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateReqCenterIDtoGuid : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RequesterCenters");
            CreateTable(
                "dbo.RequesterCenters",
                c => new
                {
                    ID = c.Guid(nullable: false, identity: true),
                    RequesterCenterID = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 50),
                    NetworkID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.RequesterCenters");
            CreateTable(
                "dbo.RequesterCenters",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    RequesterCenterID = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 50),
                    NetworkID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(1, 'CDER', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(2, 'CBER', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(3, 'CDRH', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(4, 'OMP', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(5, 'WG', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(6, 'MSOC', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
        }
    }
}

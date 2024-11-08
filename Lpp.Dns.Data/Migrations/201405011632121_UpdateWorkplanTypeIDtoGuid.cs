namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateWorkplanTypeIDtoGuid : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.WorkplanTypes");
            CreateTable(
                "dbo.WorkplanTypes",
                c => new
                {
                    ID = c.Guid(nullable: false, identity: true),
                    WorkplanTypeID = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 50),
                    NetworkID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.WorkplanTypes");
            CreateTable(
                "dbo.WorkplanTypes",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    WorkplanTypeID = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 50),
                    NetworkID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.ID);
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(1, 'Ad Hoc Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(2, 'Ad Hoc Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(3, 'Common Components Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(4, 'Common Components Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(5, 'Common Components  Development', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(6, 'Common Components Development Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(7, 'Modular Program Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(8, 'Modular Program Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(9, 'Modular Program  Development', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(10, 'Modular Program  Dev Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(11, 'PROMPT Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(12, 'PROMPT Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(13, 'PROMPT Development', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(14, 'PROMPT Development Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(15, 'Quality Assurance Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(16, 'Quality Assurance Development Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(17, 'Query Tool Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(18, 'Query Tool Development Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(19, 'Summary Table Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(20, 'Summary Table Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(21, 'Summary Table Development', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(22, 'Survey Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(23, 'Survey Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(24, 'Workgroup Request', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(25, 'Workgroup Request Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
            Sql(@"INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(26, 'Workgroup Development Beta', '253D0001-945A-4FFC-B8F7-A2F10104A692')");
        }
    }
}

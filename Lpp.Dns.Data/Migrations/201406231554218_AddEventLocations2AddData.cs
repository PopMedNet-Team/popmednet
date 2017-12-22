namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventLocations2AddData : DbMigration
    {
        public override void Up()
        {
            //Groups
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('D80E0001-27BC-4FCB-BA75-A22200CD2426', 2)");

            //Projects
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('1C090001-9F95-400C-9780-A22200CD0234', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('D2460001-F0FA-4BAA-AEE1-A22200CCADB4', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7B30001-2704-4A57-A71A-A22200CC1736', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('E39A0001-A4CA-46B8-B7EF-A22200E72B08', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('0A850001-FC8A-4DE2-9AA5-A22200E82398', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('25EC0001-3AC0-45FB-AF72-A22200CC334C', 4)");
            Sql("UPDATE Events SET Name = 'Results Viewed' WHERE ID = '25EC0001-3AC0-45FB-AF72-A22200CC334C'");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('06E30001-ED86-4427-9936-A22200CC74F0', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('5AB90001-8072-42CD-940F-A22200CC24A2', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('688B0001-1572-41CA-8298-A22200CBD542', 4)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('F31C0001-6900-4BDB-A03A-A22200CC019C', 4)");

            //Project DataMarts
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('06E30001-ED86-4427-9936-A22200CC74F0', 11)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('5AB90001-8072-42CD-940F-A22200CC24A2', 11)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('688B0001-1572-41CA-8298-A22200CBD542', 11)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('F31C0001-6900-4BDB-A03A-A22200CC019C', 11)");
           
            //Project Organizations
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('0A850001-FC8A-4DE2-9AA5-A22200E82398', 20)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('25EC0001-3AC0-45FB-AF72-A22200CC334C', 20)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7B30001-2704-4A57-A71A-A22200CC1736', 20)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('E39A0001-A4CA-46B8-B7EF-A22200E72B08', 20)");

            //Organizations
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B8A50001-B556-43D2-A1B8-A22200CD12DC', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('3AC20001-D8A4-4BE7-957C-A22200CC84BB', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('76B10001-2B49-453C-A8E1-A22200CC9356', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('0A850001-FC8A-4DE2-9AA5-A22200E82398', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('25EC0001-3AC0-45FB-AF72-A22200CC334C', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7B30001-2704-4A57-A71A-A22200CC1736', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('E39A0001-A4CA-46B8-B7EF-A22200E72B08', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('59A90001-539E-4C21-A4F2-A22200CD3C7D', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('06E30001-ED86-4427-9936-A22200CC74F0', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('5AB90001-8072-42CD-940F-A22200CC24A2', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('688B0001-1572-41CA-8298-A22200CBD542', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('F31C0001-6900-4BDB-A03A-A22200CC019C', 3)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7640001-7247-49B8-A818-A22200CCEAF7', 3)");

            //DataMarts
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('59A90001-539E-4C21-A4F2-A22200CD3C7D', 1)");

            //Users
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7640001-7247-49B8-A818-A22200CCEAF7', 1)");

            //User Profile
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('C2790001-2FF6-456C-9497-A22200CCCD1F', 13)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B6B10001-07FB-47F5-83B8-A22200CCDB90', 13)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('D5EF0001-4122-477E-9C55-A2210142C609', 13)");

            //Registries
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('553FD350-8F3B-40C6-9E31-11D8BC7420A2', 6)");

            //Global
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('60F20001-77FF-4F4B-9153-A2220129E466', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B8A50001-B556-43D2-A1B8-A22200CD12DC', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('3AC20001-D8A4-4BE7-957C-A22200CC84BB', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('76B10001-2B49-453C-A8E1-A22200CC9356', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('D80E0001-27BC-4FCB-BA75-A22200CD2426', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('1C090001-9F95-400C-9780-A22200CD0234', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('D2460001-F0FA-4BAA-AEE1-A22200CCADB4', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('553FD350-8F3B-40C6-9E31-11D8BC7420A2', 0)");
            Sql("INSERT INTO EventLocations (EventID, Location) VALUES ('B7640001-7247-49B8-A818-A22200CCEAF7', 0)");

            Sql("DELETE FROM Permissions WHERE ID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9'"); //Remove the observe permission and clean the database as a result.
        }
        
        public override void Down()
        {
        }
    }
}

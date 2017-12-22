namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkplanTypesReqCenters : DbMigration
    {
        public override void Up()
        {
            // Deploy only if M-S.
            Sql(@"DECLARE @MINISENTINEL_NETWORK_ID UNIQUEIDENTIFIER;
                    SET @MINISENTINEL_NETWORK_ID = '51340001-58CF-4BDB-9E06-A2A9011D7AAF'
                    IF EXISTS(SELECT * FROM [dbo].[Networks] WHERE ID=@MINISENTINEL_NETWORK_ID)
                    BEGIN
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(1, 'CDER', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(2, 'CBER', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(3, 'CDRH', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(4, 'OMP', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(5, 'WG', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO RequesterCenters (RequesterCenterID, Name, NetworkID) VALUES(6, 'MSOC', @MINISENTINEL_NETWORK_ID);

                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(1, 'Ad Hoc Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(2, 'Ad Hoc Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(3, 'Common Components Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(4, 'Common Components Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(5, 'Common Components  Development', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(6, 'Common Components Development Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(7, 'Modular Program Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(8, 'Modular Program Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(9, 'Modular Program  Development', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(10, 'Modular Program  Dev Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(11, 'PROMPT Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(12, 'PROMPT Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(13, 'PROMPT Development', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(14, 'PROMPT Development Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(15, 'Quality Assurance Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(16, 'Quality Assurance Development Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(17, '
Tool Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(18, 'Query Tool Development Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(19, 'Summary Table Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(20, 'Summary Table Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(21, 'Summary Table Development', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(22, 'Survey Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(23, 'Survey Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(24, 'Workgroup Request', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(25, 'Workgroup Request Beta', @MINISENTINEL_NETWORK_ID);
                        INSERT INTO Workplantypes (WorkplanTypeID, Name, NetworkID) VALUES(26, 'Workgroup Development Beta', @MINISENTINEL_NETWORK_ID);
                END");
        }
        
        public override void Down()
        {
            Sql(@"DECLARE @MINISENTINEL_NETWORK_ID UNIQUEIDENTIFIER;
                    SET @MINISENTINEL_NETWORK_ID = '51340001-58CF-4BDB-9E06-A2A9011D7AAF'
                    DELETE FROM RequesterCenters WHERE NetworkID = @MINISENTINEL_NETWORK_ID;
                    DELETE FROM WorkplanTypes WHERE NetworkID = @MINISENTINEL_NETWORK_ID;");
        }
    }
}

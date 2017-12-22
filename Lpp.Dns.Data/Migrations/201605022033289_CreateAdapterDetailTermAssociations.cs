namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAdapterDetailTermAssociations : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.DataAdapterDetailTerms", (c) => new
            {
                QueryType = c.Int(nullable: false),
                TermID = c.Guid(nullable: false)
            })
            .PrimaryKey(t => new { t.QueryType, t.TermID })
            .ForeignKey("dbo.Terms", t => t.TermID, cascadeDelete: true)
            .Index(t => t.QueryType)
            .Index(t => t.TermID);

            Sql(@"DECLARE @querytype int = 1;
--insert for Projections
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '702CE918-9A59-4082-A8C7-A9234536FE79') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '702CE918-9A59-4082-A8C7-A9234536FE79')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'EC593176-D0BF-4E5A-BCFF-4BBD13E88DBF')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '5E5020DC-C0E4-487F-ADF2-45431C2B7695')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D0800001-2810-48ED-96B9-A3D40146BAAE') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D0800001-2810-48ED-96B9-A3D40146BAAE')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '80750001-6C3B-4C2D-90EC-A3D40146C26D') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '80750001-6C3B-4C2D-90EC-A3D40146C26D')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '98A78326-35D2-461A-B858-5B69E0FED28A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '98A78326-35D2-461A-B858-5B69E0FED28A')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '834F0001-FA03-4ECD-BE28-A3CD00EC02E2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '834F0001-FA03-4ECD-BE28-A3CD00EC02E2')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '71B4545C-345B-48B2-AF5E-F84DC18E4E1A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '71B4545C-345B-48B2-AF5E-F84DC18E4E1A')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '342C354B-9ECC-479B-BE61-1770E4B44675') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '342C354B-9ECC-479B-BE61-1770E4B44675')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'F01BE0A4-7D8E-4288-AE33-C65166AF8335') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'F01BE0A4-7D8E-4288-AE33-C65166AF8335')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '8B5FAA77-4A4B-4AC7-B817-69F1297E24C5') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '8B5FAA77-4A4B-4AC7-B817-69F1297E24C5')

--Data Characterization: Demographic - Age Range
SET @querytype = 10
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'C2BFBB73-8F93-4318-A8C8-73570494FF29') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'C2BFBB73-8F93-4318-A8C8-73570494FF29')

--Data Characterization: Demographic - Ethnicity
SET @querytype = 11
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '7B18DAC8-08C7-4E83-9100-A4BB49708DAE') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '7B18DAC8-08C7-4E83-9100-A4BB49708DAE')

--Data Characterization: Demographic - Race
SET @querytype = 12
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '010414AC-1F22-488F-B948-BE0C673CFEE2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '010414AC-1F22-488F-B948-BE0C673CFEE2')

--Data Characterization: Demographic - Sex
SET @querytype = 13
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '484806D5-237A-4414-9621-5E240DAE1CAD') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '484806D5-237A-4414-9621-5E240DAE1CAD')

--Data Characterization: Procedure - Procedure Codes
SET @querytype = 14
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '1CC38753-CD3F-4696-AF5F-9818EABF8AD0') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '1CC38753-CD3F-4696-AF5F-9818EABF8AD0')

--Data Characterization: Diagnosis - Diagnosis Codes
SET @querytype = 15
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73')

--Data Characterization: Diagnosis - PDX
SET @querytype = 16
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '8B6E6E5C-7FE8-4962-AA37-2641D396CFF7') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '8B6E6E5C-7FE8-4962-AA37-2641D396CFF7')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '0255F535-B529-4753-834A-BE5AFB4B4E5E') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '0255F535-B529-4753-834A-BE5AFB4B4E5E')

--Data Characterization: Dispensing - NDC
SET @querytype = 17
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '02A7FD90-AAD2-4986-9C5D-1D22E27AB960') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '02A7FD90-AAD2-4986-9C5D-1D22E27AB960')

--Data Characterization: Dispensing - Rx Amount
SET @querytype = 18
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'E60113C0-9475-4BD4-92E5-04B063D07B30') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'E60113C0-9475-4BD4-92E5-04B063D07B30')

--Data Characterization: Dispensing - Rx Supply
SET @querytype = 19
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'B996CA0B-8E82-4E23-BAE0-2507B56464BE') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'B996CA0B-8E82-4E23-BAE0-2507B56464BE')

--Data Characterization: Metadata - Data Completeness
SET @querytype = 20
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '8EDC3AF2-DBB6-462F-91A3-E91F52DAB3FB') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '8EDC3AF2-DBB6-462F-91A3-E91F52DAB3FB')

--Data Characterization: Vital - Height
SET @querytype = 21
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4316F265-8A02-44F8-9502-9688E2B0F3A0') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4316F265-8A02-44F8-9502-9688E2B0F3A0')

--Data Characterization: Vital - Weight
SET @querytype = 22
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '332A6ABA-2F01-4BA9-9A05-78D136403E97') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '332A6ABA-2F01-4BA9-9A05-78D136403E97')

--Summary Table: Prevalence
SET @querytype = 40
--year
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '781A0001-1FF0-41AB-9E67-A3CB014C37F2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '781A0001-1FF0-41AB-9E67-A3CB014C37F2')
-- year/quarter
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7')
--coverage
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'DC880001-23B2-4F36-94E2-A3CB00DA8248') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'DC880001-23B2-4F36-94E2-A3CB00DA8248')
--age range
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C')
--sex
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '71B4545C-345B-48B2-AF5E-F84DC18E4E1A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '71B4545C-345B-48B2-AF5E-F84DC18E4E1A')
--HCPCS Procedure Codes
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '096A0001-73B4-405D-B45F-A3CA014C6E7D') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '096A0001-73B4-405D-B45F-A3CA014C6E7D')
-- setting
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '2DE50001-7882-4EDE-AC4F-A3CB00D9051A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '2DE50001-7882-4EDE-AC4F-A3CB00D9051A')
-- ICD Diagnosis
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '5E5020DC-C0E4-487F-ADF2-45431C2B7695')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D0800001-2810-48ED-96B9-A3D40146BAAE') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D0800001-2810-48ED-96B9-A3D40146BAAE')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '80750001-6C3B-4C2D-90EC-A3D40146C26D') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '80750001-6C3B-4C2D-90EC-A3D40146C26D')
-- ICD Procedure
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '9E870001-1D48-4AA3-8889-A3D40146CCB3') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '9E870001-1D48-4AA3-8889-A3D40146CCB3')
-- drug class and name
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '75290001-0E78-490C-9635-A3CA01550704') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '75290001-0E78-490C-9635-A3CA01550704')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84')

--Summary Table: Incidence
SET @querytype = 41
--year
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '781A0001-1FF0-41AB-9E67-A3CB014C37F2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '781A0001-1FF0-41AB-9E67-A3CB014C37F2')
-- year/quarter
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7')
--age range
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C')
--sex
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '71B4545C-345B-48B2-AF5E-F84DC18E4E1A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '71B4545C-345B-48B2-AF5E-F84DC18E4E1A')
-- setting
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '2DE50001-7882-4EDE-AC4F-A3CB00D9051A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '2DE50001-7882-4EDE-AC4F-A3CB00D9051A')
-- ICD Diagnosis
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '5E5020DC-C0E4-487F-ADF2-45431C2B7695')
-- drug class and name
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '75290001-0E78-490C-9635-A3CA01550704') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '75290001-0E78-490C-9635-A3CA01550704')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84')


--Summary Table: MFU
SET @querytype = 42
--year
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '781A0001-1FF0-41AB-9E67-A3CB014C37F2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '781A0001-1FF0-41AB-9E67-A3CB014C37F2')
-- year/quarter
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D62F0001-3FE1-4910-99A9-A3CB014C2BC7')
--age range
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D9DD6E82-BBCA-466A-8022-B54FF3D99A3C')
--sex
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '71B4545C-345B-48B2-AF5E-F84DC18E4E1A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '71B4545C-345B-48B2-AF5E-F84DC18E4E1A')
--HCPCS Procedure Codes
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '096A0001-73B4-405D-B45F-A3CA014C6E7D') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '096A0001-73B4-405D-B45F-A3CA014C6E7D')
-- setting
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '2DE50001-7882-4EDE-AC4F-A3CB00D9051A') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '2DE50001-7882-4EDE-AC4F-A3CB00D9051A')
-- ICD Diagnosis
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '5E5020DC-C0E4-487F-ADF2-45431C2B7695')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'D0800001-2810-48ED-96B9-A3D40146BAAE') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'D0800001-2810-48ED-96B9-A3D40146BAAE')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '80750001-6C3B-4C2D-90EC-A3D40146C26D') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '80750001-6C3B-4C2D-90EC-A3D40146C26D')
-- ICD Procedure
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '9E870001-1D48-4AA3-8889-A3D40146CCB3') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '9E870001-1D48-4AA3-8889-A3D40146CCB3')
-- drug class and name
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '75290001-0E78-490C-9635-A3CA01550704') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '75290001-0E78-490C-9635-A3CA01550704')
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '0E1F0001-CA0C-42D2-A9CC-A3CA01550E84')
--code metric
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = 'E39D0001-07A1-4DFD-9299-A3CB00F2474B') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, 'E39D0001-07A1-4DFD-9299-A3CB00F2474B')
--dispensing metric
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '16ED0001-7E2D-4B27-B943-A3CB0162C262') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '16ED0001-7E2D-4B27-B943-A3CB0162C262')
--output criteria
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '17540001-8185-41BB-BE64-A3CB00F27EC2') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '17540001-8185-41BB-BE64-A3CB00F27EC2')

--Sql Query
SET @querytype = 50
IF NOT EXISTS(SELECT NULL FROM DataAdapterDetailTerms WHERE QueryType = @querytype AND TermID = '9FCCD255-EEC2-49D5-A446-997EA2853BD5') INSERT INTO DataAdapterDetailTerms (QueryType, TermID) VALUES (@querytype, '9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataAdapterDetailTerms", "TermID", "dbo.Terms");
            DropIndex("dbo.DataAdapterDetailTerms", new[] { "TermID" });
            DropIndex("dbo.DataAdapterDetailTerms", new[] { "QueryType" });
            DropTable("dbo.DataAdapterDetailTerms");
        }
    }
}

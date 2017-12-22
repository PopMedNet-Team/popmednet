namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSummaryTablesModelAndTerms : DbMigration
    {
        public override void Up()
        {
            //Adding Summary Table Term
            Sql(@"INSERT INTO [dbo].[DataModels]([ID],[Name],[Description],[RequiresConfiguration],[QueryComposer])VALUES
           ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','Summary Tables',NULL,0,1)");

            //Codes

            //Adding ICD-9 Diagnosis Codes (3 digit) Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','5E5020DC-C0E4-487F-ADF2-45431C2B7695')");
            //Adding Drug Class Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','75290001-0E78-490C-9635-A3CA01550704')");
            //Adding Drug Name Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','0E1F0001-CA0C-42D2-A9CC-A3CA01550E84')");
            //Adding HCPCS Procedure Codes Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','096A0001-73B4-405D-B45F-A3CA014C6E7D')");
            //Adding ICD-9 Procedure Codes (3 digit) Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','E1CC0001-1D9A-442A-94C4-A3CA014C7B94')");
            //Adding ICD-9 Diagnosis Codes (4 digit) Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','D0800001-2810-48ED-96B9-A3D40146BAAE')");
            //Adding ICD-9 Diagnosis Codes (5 digit) Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','80750001-6C3B-4C2D-90EC-A3D40146C26D')");
            //Adding ICD-9 Procedure Codes (4 digit) Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','9E870001-1D48-4AA3-8889-A3D40146CCB3')");


            //Criteria

            //Adding Setting Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','2DE50001-7882-4EDE-AC4F-A3CB00D9051A')");
            //Adding Quarterly Year Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','D62F0001-3FE1-4910-99A9-A3CB014C2BC7')");
            //Adding Code Metric Type Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','E39D0001-07A1-4DFD-9299-A3CB00F2474B')");
            //Adding Criteria Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','17540001-8185-41BB-BE64-A3CB00F27EC2')");
            //Adding Dispensing Metric Type Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','16ED0001-7E2D-4B27-B943-A3CB0162C262')");
            //Adding Coverage Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','DC880001-23B2-4F36-94E2-A3CB00DA8248')");
            //Adding Year Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','781A0001-1FF0-41AB-9E67-A3CB014C37F2')");
            

            //Demographic

            //Adding Sex Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','71B4545C-345B-48B2-AF5E-F84DC18E4E1A')");
            //Adding Age Range Term to Summary Model
            Sql(@"INSERT INTO DataModelSupportedTerms(DataModelID, TermID) VALUES
		 ('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','D9DD6E82-BBCA-466A-8022-B54FF3D99A3C')");

        }
        
        public override void Down()
        {
            Sql(@"delete from DataModelSupportedTerms where DataModelID = 'CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB'");
            Sql(@"delete from DataModels where DataModelID = 'CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB'");
        }
    }
}

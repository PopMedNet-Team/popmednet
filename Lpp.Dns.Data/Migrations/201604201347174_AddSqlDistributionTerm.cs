namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSqlDistributionTerm : DbMigration
    {
        public override void Up()
        {
            //Create the SQL Distribution Term
            Sql(@"INSERT INTO [dbo].[Terms]([ID],[Name],[Description],[OID],[ReferenceUrl],[Type])
     VALUES('9FCCD255-EEC2-49D5-A446-997EA2853BD5','SQL Distribution','Custom SQL to run against Data Partners',null,null,3)");
            //Link SQL Distribution Term to Data Checker QE DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('321ADAA1-A350-4DD0-93DE-5DE658A507DF','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
            //Link SQL Distribution Term to Modular Program DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
            //Link SQL Distribution Term to Summary Tables DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('CC14E6A2-99A8-4EF8-B4CB-779A7B93A7BB','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
            //Link SQL Distribution Term to ESP Request DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('7C69584A-5602-4FC0-9F3F-A27F329B1113','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
            //Link SQL Distribution Term to Query Composer DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('455C772A-DF9B-4C6B-A6B0-D4FD4DD98488','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
            //Link SQL Distribution Term to PCORnet CDM DataModel
            Sql(@"INSERT INTO [dbo].[DataModelSupportedTerms]([DataModelID],[TermID]) VALUES('85EE982E-F017-4BC4-9ACD-EE6EE55D2446','9FCCD255-EEC2-49D5-A446-997EA2853BD5')");
        }

        public override void Down()
        {
            Sql(@"delete from DataModelSupportedTerms where TermID = '9FCCD255-EEC2-49D5-A446-997EA2853BD5'");
            Sql(@"delete from Terms where ID = '9FCCD255-EEC2-49D5-A446-997EA2853BD5'");
        }
    }
}

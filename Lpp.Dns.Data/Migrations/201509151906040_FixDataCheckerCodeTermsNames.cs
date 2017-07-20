namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataCheckerCodeTermsNames : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Terms SET [Name] = 'Diagnosis Codes', [Description] = 'Diagnosis Codes Term for DataChecker' WHERE ID ='E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73'");
            Sql("UPDATE Terms SET [Name] = 'Procedure Codes', [Description] = 'Procedure Codes Term for DataChecker' WHERE ID ='1CC38753-CD3F-4696-AF5F-9818EABF8AD0'");
        }
        
        public override void Down()
        {
        }
    }
}

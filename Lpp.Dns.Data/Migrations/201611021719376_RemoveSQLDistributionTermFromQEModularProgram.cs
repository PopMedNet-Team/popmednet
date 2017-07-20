namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSQLDistributionTermFromQEModularProgram : DbMigration
    {
        public override void Up()
        {
            //remove support for the SQL Distribution term from Modular Program
            Sql("DELETE FROM DataModelSupportedTerms WHERE DataModelID = '1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154' AND TermID = '9FCCD255-EEC2-49D5-A446-997EA2853BD5'");
        }
        
        public override void Down()
        {
        }
    }
}

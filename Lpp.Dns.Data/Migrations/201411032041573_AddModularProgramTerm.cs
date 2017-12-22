namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModularProgramTerm : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Terms (ID, [Name], [Description], [Type]) VALUES ('A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8', 'Modular Program', 'Upload files for a modular program request.', 3)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Terms WHERE ID = 'A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8'");
        }
    }
}

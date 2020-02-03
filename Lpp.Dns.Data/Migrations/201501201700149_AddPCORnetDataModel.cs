namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPCORnetDataModel : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'85EE982E-F017-4BC4-9ACD-EE6EE55D2446', N'PCORI', NULL, 0)");
        }
        
        public override void Down()
        {
            Sql("Delete from [dbo].[DataModels] Where ID = '85EE982E-F017-4BC4-9ACD-EE6EE55D2446'");
        }
    }
}

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDNSExpectingInheritenceClosureTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SecurityInheritanceClosure3",
                c => new
                    {
                        Start = c.Guid(nullable: false),
                        End = c.Guid(nullable: false),
                        Distance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Start, t.End });
            
            CreateTable(
                "dbo.SecurityInheritanceClosure2",
                c => new
                    {
                        Start = c.Guid(nullable: false),
                        End = c.Guid(nullable: false),
                        Distance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Start, t.End });
            
            CreateTable(
                "dbo.SecurityInheritanceClosure4",
                c => new
                    {
                        Start = c.Guid(nullable: false),
                        End = c.Guid(nullable: false),
                        Distance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Start, t.End });
            
            CreateTable(
                "dbo.SecurityInheritanceClosure5",
                c => new
                    {
                        Start = c.Guid(nullable: false),
                        End = c.Guid(nullable: false),
                        Distance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Start, t.End });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SecurityInheritanceClosure5");
            DropTable("dbo.SecurityInheritanceClosure4");
            DropTable("dbo.SecurityInheritanceClosure2");
            DropTable("dbo.SecurityInheritanceClosure3");
        }
    }
}

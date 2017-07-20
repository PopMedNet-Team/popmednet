namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNetworkMessageUserTable : DbMigration
    {
        public override void Up()
        {

            //CreateTable(
            //    "dbo.NetworkMessageUsers",
            //    c => new
            //        {
            //            NetworkMessageID = c.Guid(nullable: false),
            //            UserID = c.Guid(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.NetworkMessageID, t.UserID })
            //    .ForeignKey("dbo.NetworkMessages", t => t.NetworkMessageID, cascadeDelete: true)
            //    .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
            //    .Index(t => t.NetworkMessageID)
            //    .Index(t => t.UserID);

            Sql(@"IF NOT EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'NetworkMessageUsers')
BEGIN
CREATE TABLE [dbo].[NetworkMessageUsers](
	[NetworkMessageID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.NetworkMessageUsers] PRIMARY KEY CLUSTERED 
(
	[NetworkMessageID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[NetworkMessageUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NetworkMessageUsers_dbo.NetworkMessages_NetworkMessageID] FOREIGN KEY([NetworkMessageID])
REFERENCES [dbo].[NetworkMessages] ([ID])
ON DELETE CASCADE

ALTER TABLE [dbo].[NetworkMessageUsers] CHECK CONSTRAINT [FK_dbo.NetworkMessageUsers_dbo.NetworkMessages_NetworkMessageID]

ALTER TABLE [dbo].[NetworkMessageUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.NetworkMessageUsers_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE

ALTER TABLE [dbo].[NetworkMessageUsers] CHECK CONSTRAINT [FK_dbo.NetworkMessageUsers_dbo.Users_UserID]

END");

            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NetworkMessageUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.NetworkMessageUsers", "NetworkMessageID", "dbo.NetworkMessages");
            DropIndex("dbo.NetworkMessageUsers", new[] { "UserID" });
            DropIndex("dbo.NetworkMessageUsers", new[] { "NetworkMessageID" });
            DropTable("dbo.NetworkMessageUsers");
        }
    }
}

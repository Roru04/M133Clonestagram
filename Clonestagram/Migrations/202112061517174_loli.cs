namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class loli : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Posts", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Posts", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentContent = c.String(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        date = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Post_Id);
            
            DropColumn("dbo.Posts", "ApplicationUserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "ApplicationUserName", c => c.String());
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Comments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Comments");
            RenameIndex(table: "dbo.Posts", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            RenameColumn(table: "dbo.Posts", name: "ApplicationUser_Id", newName: "ApplicationUserId");
        }
    }
}

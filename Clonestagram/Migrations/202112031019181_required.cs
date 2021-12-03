namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class required : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Posts", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Posts", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "ApplicationUserName", c => c.String(nullable: false));
            CreateIndex("dbo.Posts", "ApplicationUserId");
            AddForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Posts", "ApplicationUserName", c => c.String());
            AlterColumn("dbo.Posts", "Content", c => c.String());
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "Title", c => c.String());
            CreateIndex("dbo.Posts", "ApplicationUserId");
            AddForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}

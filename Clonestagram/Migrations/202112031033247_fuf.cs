namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fuf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Posts", "ApplicationUserName", c => c.String());
            CreateIndex("dbo.Posts", "ApplicationUserId");
            AddForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Posts", "ApplicationUserName", c => c.String(nullable: false));
            AlterColumn("dbo.Posts", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Posts", "ApplicationUserId");
            AddForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}

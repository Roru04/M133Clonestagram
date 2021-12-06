namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class luul : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts");
            DropIndex("dbo.AspNetUsers", new[] { "Post_Id" });
            DropColumn("dbo.AspNetUsers", "Post_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Post_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Post_Id");
            AddForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts", "Id");
        }
    }
}

namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class likeUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Post_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Post_Id");
            AddForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Post_Id", "dbo.Posts");
            DropIndex("dbo.AspNetUsers", new[] { "Post_Id" });
            DropColumn("dbo.AspNetUsers", "Post_Id");
        }
    }
}

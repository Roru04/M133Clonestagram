namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedEverything : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comments", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameColumn(table: "dbo.Posts", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Comments", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            RenameIndex(table: "dbo.Posts", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            AddColumn("dbo.Comments", "ApplicationUserName", c => c.String());
            AddColumn("dbo.Posts", "ApplicationUserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "ApplicationUserName");
            DropColumn("dbo.Comments", "ApplicationUserName");
            RenameIndex(table: "dbo.Posts", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.Comments", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Posts", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Comments", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}

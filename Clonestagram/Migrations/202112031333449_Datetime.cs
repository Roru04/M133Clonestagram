namespace Clonestagram.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datetime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "date");
        }
    }
}

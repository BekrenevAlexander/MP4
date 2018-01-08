namespace MP4_Durak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Wins", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Games", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Games");
            DropColumn("dbo.AspNetUsers", "Wins");
        }
    }
}

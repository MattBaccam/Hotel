namespace ASPScenicHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedAppID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AppID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AppID");
        }
    }
}

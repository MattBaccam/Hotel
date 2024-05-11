namespace ASPScenicHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpositiontitleandphonenumbertotheregisteruser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PositionTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PositionTitle");
        }
    }
}

namespace ASPScenicHotel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedGuest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guests", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Guests", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Guests", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Guests", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Guests", "Email", c => c.String());
            AlterColumn("dbo.Guests", "Phone", c => c.String());
            AlterColumn("dbo.Guests", "LastName", c => c.String());
            AlterColumn("dbo.Guests", "FirstName", c => c.String());
        }
    }
}

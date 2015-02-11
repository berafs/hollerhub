namespace HollerHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Title", c => c.String(nullable: false));
        }
    }
}

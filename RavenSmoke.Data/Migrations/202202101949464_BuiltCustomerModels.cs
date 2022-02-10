namespace RavenSmoke.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuiltCustomerModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "ModifiedUtc", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "ModifiedUtc");
        }
    }
}

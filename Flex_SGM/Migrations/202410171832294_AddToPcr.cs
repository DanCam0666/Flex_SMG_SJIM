namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToPcr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pcrs", "BuildOut", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            DropColumn("dbo.pcrs", "BuildOut");
        }
    }
}

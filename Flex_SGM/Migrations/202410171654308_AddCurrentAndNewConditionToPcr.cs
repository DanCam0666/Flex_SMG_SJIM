namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurrentAndNewConditionToPcr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pcrs", "CurrentCondition", c => c.String(maxLength: 300));
            AddColumn("dbo.pcrs", "NewCondition", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.pcrs", "NewCondition");
            DropColumn("dbo.pcrs", "CurrentCondition");
        }
    }
}

namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerApprovalRequiredToPcrs : DbMigration
    {
        public override void Up()
		{
			AddColumn("dbo.pcrs", "CustomerApprovalRequired", c => c.Boolean(nullable: false, defaultValue: false));
		}
        
        public override void Down()
		{
			DropColumn("dbo.pcrs", "CustomerApprovalRequired");
		}
    }
}

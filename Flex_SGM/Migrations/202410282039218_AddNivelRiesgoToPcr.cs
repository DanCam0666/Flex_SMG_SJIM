namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNivelRiesgoToPcr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.pcrs", "NivelRiesgo", c => c.Int(nullable: false));
            AddColumn("dbo.MatrizDecisions", "Arautomatizacion", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MatrizDecisions", "Arautomatizacion");
            DropColumn("dbo.pcrs", "NivelRiesgo");
        }
    }
}

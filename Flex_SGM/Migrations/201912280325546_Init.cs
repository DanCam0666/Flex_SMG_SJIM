namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bitacoras",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DiaHora = c.DateTime(nullable: false),
                        usuario = c.String(maxLength: 50),
                        usuario_area = c.String(maxLength: 25),
                        usuario_puesto = c.String(maxLength: 25),
                        MaquinasID = c.Int(nullable: false),
                        Sintoma = c.String(maxLength: 150),
                        Causa = c.String(maxLength: 150),
                        AccionCorrectiva = c.String(),
                        Atendio = c.String(maxLength: 150),
                        Fallaoperacion = c.Boolean(nullable: false),
                        Tiempo = c.Int(nullable: false),
                        Scrap = c.String(maxLength: 150),
                        Folio = c.String(maxLength: 12),
                        Porcentaje = c.Double(nullable: false),
                        MTBF = c.Double(nullable: false),
                        MTTR = c.Double(nullable: false),
                        Verifico = c.String(maxLength: 50),
                        FechaVerificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.Maquinas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Area = c.String(maxLength: 50),
                        Cliente = c.String(maxLength: 50),
                        Maquina = c.String(nullable: false, maxLength: 100),
                        Codigo = c.String(maxLength: 50),
                        DiaHora = c.DateTime(),
                        Ubicacion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OILs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaquinasID = c.Int(nullable: false),
                        Actividad = c.String(),
                        User_gen = c.String(),
                        DiaHora = c.DateTime(nullable: false),
                        User_res = c.String(),
                        DiaHora_Compromiso = c.DateTime(),
                        DiaHora_Cierre = c.DateTime(),
                        DiaHora_Verificado = c.DateTime(),
                        Usuario_Verifico = c.String(maxLength: 100),
                        Comentarios = c.String(),
                        Material_necesario = c.String(),
                        urgente = c.Boolean(nullable: false),
                        Estatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.Fallas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Area = c.String(maxLength: 20),
                        Tipo = c.String(maxLength: 20),
                        Descripcion = c.String(maxLength: 50),
                        Codigo = c.String(maxLength: 50),
                        DescripcionCodigo = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserFullName = c.String(),
                        Area = c.String(maxLength: 50),
                        Puesto = c.String(maxLength: 50),
                        Nomina = c.String(maxLength: 20),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.OILs", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.Bitacoras", "MaquinasID", "dbo.Maquinas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.OILs", new[] { "MaquinasID" });
            DropIndex("dbo.Bitacoras", new[] { "MaquinasID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Fallas");
            DropTable("dbo.OILs");
            DropTable("dbo.Maquinas");
            DropTable("dbo.Bitacoras");
        }
    }
}

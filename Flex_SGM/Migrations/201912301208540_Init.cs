namespace Flex_SGM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AndonAuditors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AreasID = c.Int(nullable: false),
                        Auditor = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.cAreas", t => t.AreasID, cascadeDelete: true)
                .Index(t => t.AreasID);
            
            CreateTable(
                "dbo.cAreas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Area = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AndonDefectoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AreasID = c.Int(nullable: false),
                        Defecto = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.cAreas", t => t.AreasID, cascadeDelete: true)
                .Index(t => t.AreasID);
            
            CreateTable(
                "dbo.AndonSupervisores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AreasID = c.Int(nullable: false),
                        Supervisor = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.cAreas", t => t.AreasID, cascadeDelete: true)
                .Index(t => t.AreasID);
            
            CreateTable(
                "dbo.Bitacoras",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DiaHora = c.DateTime(nullable: false),
                        usuario = c.String(maxLength: 100),
                        usuario_area = c.String(maxLength: 25),
                        usuario_puesto = c.String(maxLength: 25),
                        MaquinasID = c.Int(nullable: false),
                        Sintoma = c.String(nullable: false, maxLength: 300),
                        Causa = c.String(nullable: false),
                        AccionCorrectiva = c.String(nullable: false),
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
                        MttoPreventivo = c.Boolean(nullable: false),
                        MttoCorrectivo = c.Boolean(nullable: false),
                        MttoMejora = c.Boolean(nullable: false),
                        noterminado = c.Boolean(nullable: false),
                        findesemana = c.Boolean(nullable: false),
                        turno = c.String(maxLength: 3),
                        Tipos = c.String(maxLength: 60),
                        Descripcion = c.String(maxLength: 80),
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
                        SubMaquina = c.String(nullable: false, maxLength: 100),
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
                        Tipo = c.String(maxLength: 100),
                        folio = c.String(maxLength: 50),
                        Actividad = c.String(),
                        User_gen = c.String(maxLength: 100),
                        DiaHora = c.DateTime(nullable: false),
                        User_res = c.String(maxLength: 100),
                        DiaHora_Compromiso = c.DateTime(),
                        User_asig = c.String(maxLength: 100),
                        DiaHora_Cierre = c.DateTime(),
                        DiaHora_Verificado = c.DateTime(),
                        Usuario_Verifico = c.String(maxLength: 100),
                        Comentarios = c.String(),
                        Comentarios2 = c.String(),
                        Material_necesario = c.String(),
                        urgente = c.Boolean(nullable: false),
                        Estatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.CalendarioProyes",
                c => new
                    {
                        EventID = c.Long(nullable: false, identity: true),
                        Subject = c.String(maxLength: 100),
                        Description = c.String(maxLength: 300),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        daysOfWeek = c.String(maxLength: 30),
                        ThemeColor = c.String(maxLength: 10),
                        IsFullDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.CAndon2",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Turno = c.String(maxLength: 1),
                        Hora = c.String(maxLength: 5),
                        AreasgID = c.Int(),
                        AreaseID = c.Int(),
                        ClientesID = c.Int(nullable: false),
                        ProyectosID = c.Int(nullable: false),
                        lote = c.String(maxLength: 20),
                        NoDeParte = c.String(),
                        NombreDeParte = c.String(maxLength: 150),
                        AndonDefectoID = c.Int(),
                        Comentarios = c.String(maxLength: 600),
                        Cantidadpz = c.String(maxLength: 20),
                        AndonAuditorID = c.Int(),
                        AndonSupervisoresID = c.Int(),
                        Asistentes_Nombre1 = c.String(maxLength: 150),
                        Asistentes_Area1 = c.String(maxLength: 50),
                        Asistentes_Nombre2 = c.String(maxLength: 150),
                        Asistentes_Area2 = c.String(maxLength: 50),
                        Asistentes_Nombre3 = c.String(maxLength: 150),
                        Asistentes_Area3 = c.String(maxLength: 50),
                        Asistentes_Nombre4 = c.String(maxLength: 150),
                        Asistentes_Area4 = c.String(maxLength: 50),
                        Asistentes_Nombre5 = c.String(maxLength: 150),
                        Asistentes_Area5 = c.String(maxLength: 50),
                        Asistentes_Nombre6 = c.String(maxLength: 150),
                        Asistentes_Area6 = c.String(maxLength: 50),
                        Asistentes_Nombre7 = c.String(maxLength: 150),
                        Asistentes_Area7 = c.String(maxLength: 50),
                        Asistentes_Nombre8 = c.String(maxLength: 150),
                        Asistentes_Area8 = c.String(maxLength: 50),
                        Asistentes_Nombre9 = c.String(maxLength: 150),
                        Asistentes_Area9 = c.String(maxLength: 50),
                        EstatusAndon = c.String(maxLength: 50),
                        Esproblema = c.Boolean(nullable: false),
                        Esproblemaseguridad = c.Boolean(nullable: false),
                        Esproblemavario = c.Boolean(nullable: false),
                        a1why = c.String(maxLength: 500),
                        a2why = c.String(maxLength: 500),
                        a3why = c.String(maxLength: 500),
                        a4why = c.String(maxLength: 500),
                        a50d = c.String(maxLength: 500),
                        Causas = c.String(maxLength: 500),
                        Acciones = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AndonAuditors", t => t.AndonAuditorID)
                .ForeignKey("dbo.AndonDefectoes", t => t.AndonDefectoID)
                .ForeignKey("dbo.AndonSupervisores", t => t.AndonSupervisoresID)
                .ForeignKey("dbo.cAreas", t => t.AreaseID)
                .ForeignKey("dbo.cClientes", t => t.ClientesID, cascadeDelete: true)
                .ForeignKey("dbo.cAreas", t => t.AreasgID)
                .ForeignKey("dbo.cProyectos", t => t.ProyectosID, cascadeDelete: true)
                .Index(t => t.AreasgID)
                .Index(t => t.AreaseID)
                .Index(t => t.ClientesID)
                .Index(t => t.ProyectosID)
                .Index(t => t.AndonDefectoID)
                .Index(t => t.AndonAuditorID)
                .Index(t => t.AndonSupervisoresID);
            
            CreateTable(
                "dbo.cClientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Cliente = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.cProyectos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientesID = c.Int(),
                        Proyecto = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.cClientes", t => t.ClientesID)
                .Index(t => t.ClientesID);
            
            CreateTable(
                "dbo.CAndons",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Turno = c.String(maxLength: 1),
                        AreaActivacion = c.String(maxLength: 25),
                        ZonaActivacion = c.String(maxLength: 25),
                        AuditorArea = c.String(maxLength: 80),
                        cliente = c.String(maxLength: 50),
                        proyecto = c.String(maxLength: 50),
                        lote = c.String(maxLength: 50),
                        NoDeParte = c.String(maxLength: 100),
                        NombreDeParte = c.String(maxLength: 100),
                        Defecto1 = c.String(maxLength: 250),
                        Cantidadpz1 = c.String(maxLength: 20),
                        Preventivos = c.String(maxLength: 100),
                        AreaDefecto = c.String(maxLength: 50),
                        SubArea = c.String(maxLength: 50),
                        AuditorAreaDefecto = c.String(maxLength: 80),
                        ResponsableAndon = c.String(maxLength: 50),
                        FormatoAndon = c.String(maxLength: 50),
                        AlertaCalidad = c.String(maxLength: 50),
                        MetCertificación = c.String(maxLength: 50),
                        EstatusAndon = c.String(maxLength: 50),
                        Comentarios = c.String(maxLength: 600),
                        a1d = c.String(maxLength: 500),
                        a5d = c.String(maxLength: 500),
                        a10d = c.String(maxLength: 500),
                        a20d = c.String(maxLength: 500),
                        a30d = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CDockaudits",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NoDockAudit = c.String(maxLength: 8),
                        Fecha = c.DateTime(nullable: false),
                        lote = c.String(maxLength: 50),
                        NoDeParte = c.String(maxLength: 100),
                        Area = c.String(maxLength: 25),
                        Cantidad = c.String(maxLength: 25),
                        cliente = c.String(maxLength: 50),
                        Descripcion = c.String(maxLength: 700),
                        Clasificacion = c.String(maxLength: 100),
                        AreaOrigen = c.String(maxLength: 50),
                        SupOrigen = c.String(maxLength: 80),
                        SupRecibio = c.String(maxLength: 80),
                        AuditorReporto = c.String(maxLength: 80),
                        Comentarios = c.String(maxLength: 600),
                        a1d = c.String(maxLength: 500),
                        a5d = c.String(maxLength: 500),
                        a10d = c.String(maxLength: 500),
                        a20d = c.String(maxLength: 500),
                        a30d = c.String(maxLength: 500),
                        Turno = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Usuario = c.String(maxLength: 100),
                        fecha = c.DateTime(nullable: false),
                        message = c.String(),
                        OILs_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OILs", t => t.OILs_ID)
                .Index(t => t.OILs_ID);
            
            CreateTable(
                "dbo.ControldeEquipos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaquinasID = c.Int(nullable: false),
                        DiaHora = c.DateTime(nullable: false),
                        equipo = c.String(nullable: false),
                        descripcion = c.String(nullable: false),
                        iplocal = c.String(nullable: false),
                        ipintra = c.String(nullable: false),
                        vlan = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.controlplanos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Program = c.String(maxLength: 75),
                        Level = c.Int(nullable: false),
                        InternalPN = c.String(maxLength: 25),
                        CustomerPN = c.String(maxLength: 25),
                        Description = c.String(maxLength: 75),
                        Eng_Level = c.String(maxLength: 25),
                        Revision = c.String(maxLength: 10),
                        Date = c.DateTime(nullable: false),
                        MathData_3D = c.String(maxLength: 400),
                        DrawingPN_2D = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Disponibilidads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Maquinas = c.String(nullable: false),
                        Subject = c.String(maxLength: 100),
                        Description = c.String(maxLength: 300),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        daysOfWeek = c.String(maxLength: 30),
                        ThemeColor = c.String(maxLength: 10),
                        IsFullDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.Juntas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Accidente = c.String(),
                        Dia_Accidente = c.DateTime(nullable: false),
                        OILSSeguridad = c.String(),
                        OILSnuevosproyectos = c.String(),
                        Oshas = c.String(),
                        RelevanteSafety = c.String(),
                        ToC_Pendientes_Servicios = c.String(),
                        ToC_Pendientes_Montacargas = c.String(),
                        ToC_Pendientes_Estampado = c.String(),
                        ToC_Pendientes_Soldadura = c.String(),
                        ToC_Pendientes_Cromo1 = c.String(),
                        ToC_Pendientes_Pulido1 = c.String(),
                        ToC_Pendientes_Cromo2 = c.String(),
                        ToC_Pendientes_Pulido2 = c.String(),
                        ToC_Pendientes_Ensamble = c.String(),
                        ToC_Pendientes_E_Coat = c.String(),
                        ToC_Pendientes_Top_Coat = c.String(),
                        Vacaciones_Servicios = c.String(),
                        Vacaciones_Montacargas = c.String(),
                        Vacaciones_Estampado = c.String(),
                        Vacaciones_Soldadura = c.String(),
                        Vacaciones_Cromo1 = c.String(),
                        Vacaciones_Pulido1 = c.String(),
                        Vacaciones_Cromo2 = c.String(),
                        Vacaciones_Pulido2 = c.String(),
                        Vacaciones_Ensamble = c.String(),
                        Vacaciones_E_Coat = c.String(),
                        Vacaciones_Top_Coat = c.String(),
                        Preventivos_Servicios = c.String(),
                        Preventivos_Montacargas = c.String(),
                        Preventivos_Estampado = c.String(),
                        Preventivos_Soldadura = c.String(),
                        Preventivos_Cromo1 = c.String(),
                        Preventivos_Pulido1 = c.String(),
                        Preventivos_Cromo2 = c.String(),
                        Preventivos_Pulido2 = c.String(),
                        Preventivos_Ensamble = c.String(),
                        Preventivos_E_Coat = c.String(),
                        Preventivos_Top_Coat = c.String(),
                        comentariosa_Servicios = c.String(),
                        comentariosa_Montacargas = c.String(),
                        comentariosa_Estampado = c.String(),
                        comentariosa_Soldadura = c.String(),
                        comentariosa_Cromo1 = c.String(),
                        comentariosa_Pulido1 = c.String(),
                        comentariosa_Cromo2 = c.String(),
                        comentariosa_Pulido2 = c.String(),
                        comentariosa_Ensamble = c.String(),
                        comentariosa_E_Coat = c.String(),
                        comentariosa_Top_Coat = c.String(),
                        comentarios = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Proyectos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaquinasID = c.Int(nullable: false),
                        User_gen = c.String(maxLength: 100),
                        DiaHora = c.DateTime(nullable: false),
                        Tipo = c.String(maxLength: 100),
                        Que = c.String(),
                        Porque = c.String(),
                        Paraque = c.String(),
                        Paraquien = c.String(),
                        Donde = c.String(),
                        Cuando = c.String(),
                        Conquien = c.String(),
                        conque = c.String(),
                        Como = c.String(),
                        DiaHora_Cierre = c.DateTime(),
                        DiaHora_Verificado = c.DateTime(),
                        Usuario_Verifico = c.String(maxLength: 100),
                        Comentarios = c.String(),
                        urgente = c.Boolean(nullable: false),
                        Estatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.CReclamos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Oficial = c.Boolean(nullable: false),
                        IngCalidad = c.String(maxLength: 80),
                        NoReclamoCliente = c.String(maxLength: 50),
                        NoReclamoFNG = c.String(maxLength: 50),
                        Area = c.String(maxLength: 25),
                        NoParte = c.String(maxLength: 50),
                        Descripcionpz = c.String(maxLength: 200),
                        Defecto = c.String(maxLength: 500),
                        Cliente = c.String(maxLength: 50),
                        Planta = c.String(maxLength: 50),
                        costo = c.String(maxLength: 25),
                        PLM = c.String(maxLength: 25),
                        Cantidadpz = c.String(maxLength: 10),
                        Proceso = c.String(maxLength: 50),
                        Comentarios = c.String(maxLength: 600),
                        a1d = c.String(maxLength: 500),
                        a5d = c.String(maxLength: 500),
                        a10d = c.String(maxLength: 500),
                        a20d = c.String(maxLength: 500),
                        a30d = c.String(maxLength: 500),
                        tipo = c.String(maxLength: 10),
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
                "dbo.SubClientes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientesID = c.Int(nullable: false),
                        SubCliente = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.cClientes", t => t.ClientesID, cascadeDelete: true)
                .Index(t => t.ClientesID);
            
            CreateTable(
                "dbo.troubleshootings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DiaHora = c.DateTime(nullable: false),
                        usuario = c.String(maxLength: 100),
                        MaquinasID = c.Int(nullable: false),
                        Problema = c.String(nullable: false, maxLength: 500),
                        Posiblerazon = c.String(nullable: false, maxLength: 600),
                        Solucion = c.String(nullable: false),
                        Comentarios = c.String(maxLength: 800),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Maquinas", t => t.MaquinasID, cascadeDelete: true)
                .Index(t => t.MaquinasID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserFullName = c.String(maxLength: 150),
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
            DropForeignKey("dbo.troubleshootings", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.SubClientes", "ClientesID", "dbo.cClientes");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Proyectos", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.ControldeEquipos", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.Comments", "OILs_ID", "dbo.OILs");
            DropForeignKey("dbo.CAndon2", "ProyectosID", "dbo.cProyectos");
            DropForeignKey("dbo.cProyectos", "ClientesID", "dbo.cClientes");
            DropForeignKey("dbo.CAndon2", "AreasgID", "dbo.cAreas");
            DropForeignKey("dbo.CAndon2", "ClientesID", "dbo.cClientes");
            DropForeignKey("dbo.CAndon2", "AreaseID", "dbo.cAreas");
            DropForeignKey("dbo.CAndon2", "AndonSupervisoresID", "dbo.AndonSupervisores");
            DropForeignKey("dbo.CAndon2", "AndonDefectoID", "dbo.AndonDefectoes");
            DropForeignKey("dbo.CAndon2", "AndonAuditorID", "dbo.AndonAuditors");
            DropForeignKey("dbo.OILs", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.Bitacoras", "MaquinasID", "dbo.Maquinas");
            DropForeignKey("dbo.AndonSupervisores", "AreasID", "dbo.cAreas");
            DropForeignKey("dbo.AndonDefectoes", "AreasID", "dbo.cAreas");
            DropForeignKey("dbo.AndonAuditors", "AreasID", "dbo.cAreas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.troubleshootings", new[] { "MaquinasID" });
            DropIndex("dbo.SubClientes", new[] { "ClientesID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Proyectos", new[] { "MaquinasID" });
            DropIndex("dbo.ControldeEquipos", new[] { "MaquinasID" });
            DropIndex("dbo.Comments", new[] { "OILs_ID" });
            DropIndex("dbo.cProyectos", new[] { "ClientesID" });
            DropIndex("dbo.CAndon2", new[] { "AndonSupervisoresID" });
            DropIndex("dbo.CAndon2", new[] { "AndonAuditorID" });
            DropIndex("dbo.CAndon2", new[] { "AndonDefectoID" });
            DropIndex("dbo.CAndon2", new[] { "ProyectosID" });
            DropIndex("dbo.CAndon2", new[] { "ClientesID" });
            DropIndex("dbo.CAndon2", new[] { "AreaseID" });
            DropIndex("dbo.CAndon2", new[] { "AreasgID" });
            DropIndex("dbo.OILs", new[] { "MaquinasID" });
            DropIndex("dbo.Bitacoras", new[] { "MaquinasID" });
            DropIndex("dbo.AndonSupervisores", new[] { "AreasID" });
            DropIndex("dbo.AndonDefectoes", new[] { "AreasID" });
            DropIndex("dbo.AndonAuditors", new[] { "AreasID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.troubleshootings");
            DropTable("dbo.SubClientes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.CReclamos");
            DropTable("dbo.Proyectos");
            DropTable("dbo.Juntas");
            DropTable("dbo.Fallas");
            DropTable("dbo.Disponibilidads");
            DropTable("dbo.controlplanos");
            DropTable("dbo.ControldeEquipos");
            DropTable("dbo.Comments");
            DropTable("dbo.CDockaudits");
            DropTable("dbo.CAndons");
            DropTable("dbo.cProyectos");
            DropTable("dbo.cClientes");
            DropTable("dbo.CAndon2");
            DropTable("dbo.CalendarioProyes");
            DropTable("dbo.OILs");
            DropTable("dbo.Maquinas");
            DropTable("dbo.Bitacoras");
            DropTable("dbo.AndonSupervisores");
            DropTable("dbo.AndonDefectoes");
            DropTable("dbo.cAreas");
            DropTable("dbo.AndonAuditors");
        }
    }
}

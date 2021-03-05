using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{

    public class controlplanosview
    {
        public string href { get; set; }
        public string imgurl { get; set; }
        public string imgname { get; set; }
        public string Nombreproyecto { get; set; }
        public string fechaactualizacion2d { get; set; }
        public string fechaactualizacion3d { get; set; }
        public string Descripcion { get; set; }
        public string actualizaciones { get; set; }

        public string fechaactualizacion { get; set; }

    }

    public class controlplanos
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(75)]
        public string Program { get; set; }
        public int Level { get; set; }
        [MaxLength(25)]
        public string InternalPN { get; set; }
        [MaxLength(25)]
        public string CustomerPN { get; set; }
        [MaxLength(75)]
        public string Description { get; set; }
        [MaxLength(25)]
        public string Eng_Level { get; set; }
        [MaxLength(10)]
        public string Revision { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        [MaxLength(400)]
        public string MathData_3D { get; set; }
        [MaxLength(400)]
        public string DrawingPN_2D { get; set; }

    }
    public class CReclamos
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        public bool Oficial { get; set; }

        [MaxLength(80)]
        public string IngCalidad { get; set; }

        [MaxLength(50)]
        public string NoReclamoCliente { get; set; }
        [MaxLength(50)]
        public string NoReclamoFNG { get; set; }
        [MaxLength(25)]
        public string Area { get; set; }
        [MaxLength(50)]
        public string NoParte { get; set; }
        [MaxLength(200)]
        public string Descripcionpz { get; set; }
        [MaxLength(500)]
        public string Defecto { get; set; }
        [MaxLength(50)]
        public string Cliente { get; set; }
        [MaxLength(50)]
        public string Planta { get; set; }
        [MaxLength(25)]
        public string costo { get; set; }
        [MaxLength(25)]
        public string PLM { get; set; }
        [MaxLength(10)]
        public string Cantidadpz { get; set; }
        [MaxLength(50)]
        public string Proceso { get; set; }
        [MaxLength(600)]
        public string Comentarios { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 1 día")]
        public string a1d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 5 días")]
        public string a5d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 10 días")]
        public string a10d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 20 días")]
        public string a20d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 30 días")]
        public string a30d { get; set; }

        [MaxLength(10)]
        public string tipo { get; set; }

    }

    public class CAndon
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [MaxLength(1)]
        public string Turno { get; set; }
     
        [MaxLength(25)]
        public string AreaActivacion { get; set; }
        [MaxLength(25)]
        public string ZonaActivacion { get; set; }
        [MaxLength(80)]
        public string AuditorArea { get; set; }

        [MaxLength(50)]
        public string cliente { get; set; }

        [MaxLength(50)]
        public string proyecto { get; set; }

        [MaxLength(50)]
        public string lote { get; set; }
        [MaxLength(100)]
        public string NoDeParte { get; set; }

        [MaxLength(100)]
        public string NombreDeParte { get; set; }

        [MaxLength(250)]
        public string Defecto1 { get; set; }
        [MaxLength(20)]
        public string Cantidadpz1 { get; set; }

        [MaxLength(100)]
        public string Preventivos { get; set; }

        [MaxLength(50)]
        public string AreaDefecto { get; set; }

        [MaxLength(50)]
        public string SubArea { get; set; }
        [MaxLength(80)]
        public string AuditorAreaDefecto { get; set; }
        [MaxLength(50)]
        public string ResponsableAndon { get; set; }
        [MaxLength(50)]
        public string FormatoAndon { get; set; }
        [MaxLength(50)]
        public string AlertaCalidad { get; set; }
        [MaxLength(50)]
        public string MetCertificación { get; set; }
        [MaxLength(50)]
        public string EstatusAndon { get; set; }

        [MaxLength(600)]
        public string Comentarios { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 1 día")]
        public string a1d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 5 días")]
        public string a5d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 10 días")]
        public string a10d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 20 días")]
        public string a20d { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones a 30 días")]
        public string a30d { get; set; }
    }

    public class AndonDefecto
    {
        [Key]
        public int ID { get; set; }

     
        public int AreasID { get; set; }
        [MaxLength(50)]
        [Display(Name = "Defecto")]
        public string Defecto { get; set; }

        public virtual cAreas Areas { get; set; }
    }
    public class AndonSupervisores
    {
        [Key]
        public int ID { get; set; }

     
        public int AreasID { get; set; }
        [MaxLength(100)]
        [Display(Name = "Supervisor Area")]
        public string Supervisor { get; set; }

        public virtual cAreas Areas { get; set; }
    }
    public class AndonAuditor
    {
        [Key]
        public int ID { get; set; }

   
        public int AreasID { get; set; }
        [MaxLength(100)]
        [Display(Name = "Auditor Area")]
        public string Auditor { get; set; }

        public virtual cAreas Areas { get; set; }
    }

    public class CAndon2
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [MaxLength(1)]
        public string Turno { get; set; }

        [MaxLength(5)]
        public string Hora { get; set; }

        [Display(Name = "Area Generadora")]
        [ForeignKey("Primary")]
        public int? AreasgID { get; set; }
  
        [Display(Name = "Area Emisora")]
        [ForeignKey("Assigned")]
        public int? AreaseID { get; set; }


        [Display(Name = "Cliente")]
        public int ClientesID { get; set; }
 
        [Display(Name = "Proyecto")]
        public int ProyectosID { get; set; }
        [Display(Name = "Proyecto")]

        public int? MaquinasID { get; set; }
        [MaxLength(20)]
        public string lote { get; set; }

  
        [Display(Name = "No. De parte")]
        public string NoDeParte { get; set; }

        [MaxLength(150)]

        [Display(Name = "Nombre De la parte")]
        public string NombreDeParte { get; set; }

        [Display(Name = "Defecto")]
        public int? AndonDefectoID { get; set; }

        [MaxLength(600)]
        public string Comentarios { get; set; }
        [MaxLength(20)]
        public string Cantidadpz { get; set; }

        [Display(Name = "Auditor de Area Emisora")]
        public int? AndonAuditorID { get; set; }


        [Display(Name = "Supervisor de Area Generadora")]
        public int? AndonSupervisoresID { get; set; }


        public Asistentes Asistentes { get; set; }

        [MaxLength(50)]
        public string EstatusAndon { get; set; }
        [Display(Name = "Es un problema?")]

        public bool Esproblema { get; set; }
        [Display(Name = "El problema pone en riego la seguridad?")]
 
        public bool Esproblemaseguridad { get; set; }

        [Display(Name = "El problema es repetitivo/ de Dock Audit / Pudo causar un problema con cliente / Atraso de Entrega / Paro de linea 2Hr o mas?")]
 
        public bool Esproblemavario { get; set; }

        [MaxLength(500)]
        [Display(Name = "1er Porque")]
        public string a1why { get; set; }
        [MaxLength(500)]
        [Display(Name = "2do Porque")]
        public string a2why { get; set; }
        [MaxLength(500)]
        [Display(Name = "3er Porque")]
        public string a3why { get; set; }
        [MaxLength(500)]
        [Display(Name = "4to Porque")]
        public string a4why { get; set; }
        [MaxLength(500)]
        [Display(Name = "5to Porque")]
        public string a50d { get; set; }

        [MaxLength(500)]
        [Display(Name = "Causas")]
        public string Causas { get; set; }
        [MaxLength(500)]
        [Display(Name = "Acciones")]
        public string Acciones { get; set; }

        public virtual cAreas Primary { get; set; }

        public virtual cAreas Assigned { get; set; }
        public virtual Maquinas Maquinas { get; set; }
        public virtual cClientes Clientes { get; set; }
        public virtual cProyectos Proyectos { get; set; }

        public virtual AndonAuditor AndonAuditor { get; set; }

        public virtual AndonSupervisores AndonSupervisores { get; set; }

        public virtual AndonDefecto AndonDefecto { get; set; }
    }

    public class Asistentes 
    {
        [MaxLength(150)]
        public string Nombre1 { get; set; }

        [MaxLength(50)]
        public string Area1 { get; set; }
        [MaxLength(150)]
        public string Nombre2 { get; set; }

        [MaxLength(50)]
        public string Area2 { get; set; }
        [MaxLength(150)]
        public string Nombre3 { get; set; }

        [MaxLength(50)]
        public string Area3 { get; set; }
        [MaxLength(150)]
        public string Nombre4 { get; set; }

        [MaxLength(50)]
        public string Area4 { get; set; }
        [MaxLength(150)]
        public string Nombre5 { get; set; }

        [MaxLength(50)]
        public string Area5 { get; set; }
        [MaxLength(150)]
        public string Nombre6 { get; set; }

        [MaxLength(50)]
        public string Area6 { get; set; }
        [MaxLength(150)]
        public string Nombre7 { get; set; }

        [MaxLength(50)]
        public string Area7 { get; set; }
        [MaxLength(150)]
        public string Nombre8 { get; set; }

        [MaxLength(50)]
        public string Area8 { get; set; }
        [MaxLength(150)]
        public string Nombre9 { get; set; }

        [MaxLength(50)]
        public string Area9 { get; set; }
    }
    public class CDockaudit
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(8)]
        public string NoDockAudit { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }

        [MaxLength(50)]
        public string lote { get; set; }

        [MaxLength(100)]
        public string NoDeParte { get; set; }

        [MaxLength(25)]
        public string Area { get; set; }

        [MaxLength(25)]
        public string Cantidad { get; set; }

        [MaxLength(50)]
        public string cliente { get; set; }

        [MaxLength(700)]
        public string Descripcion { get; set; }

        [MaxLength(100)]
        public string Clasificacion { get; set; }

        [MaxLength(50)]
        public string AreaOrigen { get; set; }

        [MaxLength(80)]
        public string SupOrigen { get; set; }

        [MaxLength(80)]
        public string SupRecibio { get; set; }

        [MaxLength(80)]
        public string AuditorReporto { get; set; }

        [MaxLength(600)]
        public string Comentarios { get; set; }

        [MaxLength(500)]
        [Display(Name = "1er Porque")]
        public string a1why { get; set; }
        [MaxLength(500)]
        [Display(Name = "2do Porque")]
        public string a2why { get; set; }  
        [MaxLength(500)]
        [Display(Name = "3er Porque")]
        public string a3why { get; set; }
        [MaxLength(500)]
        [Display(Name = "4to Porque")]
        public string a4why { get; set; }
        [MaxLength(500)]
        [Display(Name = "5to Porque")]
        public string a5why { get; set; }

        [MaxLength(500)]
        [Display(Name = "Causa")]
        public string acausa { get; set; }
        [MaxLength(500)]
        [Display(Name = "Accion")]
        public string aaccion { get; set; }


        [MaxLength(1)]
        public string Turno { get; set; }
    }


    public class Trendline
    {
        public Trendline(IList<decimal> yAxisValues, IList<decimal> xAxisValues) : this(yAxisValues.Select((t, i) => new Tuple<decimal, decimal>(xAxisValues[i], t)))
        { }
        public Trendline(IEnumerable<Tuple<Decimal, Decimal>> data)
        {
            var cachedData = data.ToList();
            var n = cachedData.Count;
            var sumX = cachedData.Sum(x => x.Item1);
            var sumX2 = cachedData.Sum(x => x.Item1 * x.Item1);
            var sumY = cachedData.Sum(x => x.Item2);
            var sumXY = cachedData.Sum(x => x.Item1 * x.Item2);
            //b = (sum(x*y) - sum(x)sum(y)/n) 
            // / (sum(x^2) - sum(x)^2/n) 
            Slope = (sumXY - ((sumX * sumY) / n)) / (sumX2 - (sumX * sumX / n));
            //a = sum(y)/n - b(sum(x)/n) 
            Intercept = (sumY / n) - (Slope * (sumX / n));
            Start = GetYValue(cachedData.Min(a => a.Item1));
            End = GetYValue(cachedData.Max(a => a.Item1));
        }
        public decimal Slope { get; private set; }
        public decimal Intercept { get; private set; }
        public decimal Start { get; private set; }
        public decimal End { get; private set; }
        public decimal GetYValue(decimal xValue)
        {
            return Intercept + Slope * xValue;
        }
    }

    public class template
    {
         public string tiporeclamo { get; set; }
         public string fecha { get; set; }
         public string reportecliente { get; set; }
         public string reportefng { get; set; }
         public string area { get; set; }
         public string noparte { get; set; }
         public string cantidad { get; set; }
         public string cliente { get; set; }
         public string lugar { get; set; }
         public string costo { get; set; }
         public string descripcion { get; set; }
         public string descripcion2 { get; set; }


    }

    public class templateandon
    {
        public string fecha { get; set; }
        public string hora_activacion { get; set; }
        public string no_parte { get; set; }
        public string descripcionpza { get; set; }
        public string Problemar1 { get; set; }
        public string Problemar2 { get; set; }
        public string Problemar3 { get; set; }
        public string Problemar4 { get; set; }
        public string Area_Prensasn1 { get; set; }
        public string Area_Prensasn2 { get; set; }
        public string Area_Cromo { get; set; }
        public string Area_Pulido { get; set; }
        public string Area_Soldadura { get; set; }
        public string Area_Pedestales { get; set; }
        public string Area_EnsambleDJ { get; set; }
        public string Area_EnsambleGM { get; set; }
        public string Area_Topcoat { get; set; }
        public string Area_Ecoat { get; set; }
        public string Area_Nuevoproyecto { get; set; }
        public string Area_otro { get; set; }
        public string Area_otrodes { get; set; }
        public string Nombre1 { get; set; }
        public string Depa1 { get; set; }
        public string Nombre2 { get; set; }
        public string Depa2 { get; set; }
        public string Nombre3 { get; set; }
        public string Depa3 { get; set; }
        public string Nombre4 { get; set; }
        public string Depa4 { get; set; }
        public string Nombre5 { get; set; }
        public string Depa5 { get; set; }

        public string Nombre6 { get; set; }
        public string Depa6 { get; set; }
        public string Nombre7 { get; set; }
        public string Depa7 { get; set; }
        public string Nombre8 { get; set; }
        public string Depa8 { get; set; }


        public string Esproblema { get; set; }
        public string Esprobleman { get; set; }
        public string Pone_riesgos { get; set; }
        public string Pone_riesgon { get; set; }
        public string repetitivos { get; set; }
        public string repetitivon { get; set; }

        public string DESCRIPCION5P { get; set; }
        public string primer5P { get; set; }
        public string segundo5P { get; set; }
        public string tercer5P { get; set; }
        public string cuarto5P { get; set; }
        public string quinto5P { get; set; }
        public string causa5P { get; set; }
        public string acciones5P { get; set; }

    }

    public class templatedock
    {
        public string fecha { get; set; }
        public string cliente { get; set; }
        public string noreporte { get; set; }
        public string noparte { get; set; }
        public string descripcionparte { get; set; }
        public string modelo { get; set; }
        public string donde { get; set; }
        public string cuadofecha { get; set; }
        public string que { get; set; }
        public string que2 { get; set; }
        public string supervisor { get; set; }
        public string lote { get; set; }
        public string cantidad { get; set; }
        public string reporto { get; set; }
        public string recibio { get; set; }
        public string a1d { get; set; }
        public string a5d { get; set; }
        public string a10d { get; set; }
        public string a20d { get; set; }
        public string a30d { get; set; }
        public string a1why { get; set; }
        public string a2why { get; set; }
        public string a3why { get; set; }
        public string a4why { get; set; }
        public string a5why { get; set; }
        public string acausa { get; set; }
        public string aacion { get; set; }

    }
    //---------------------------------------------------------------------------------

    public class QuaCost
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Bill to")]
        public int? BilltoID { get; set; }
        [Display(Name = "Ubicacion")]
        public string Location { get; set; } //  pir, AETC DCP   &*(Referencia de Cargo )

        [Display(Name = "CPO#")]
        public string CPO { get; set; } //  pir, AETC DCP   &*(Referencia de Cargo )
        [Display(Name = "I/C")]
        public string ioc { get; set; } //Ivoice or credit
        [Display(Name = "A/D")]
        public string aod { get; set; } // Accrual *(provision ) debit *(cargo aplicado)
        [Display(Name = "Codigo")]
        public int? QuaCodesID { get; set; }
        [Display(Name = "Numero de parte")]
        public string partnum { get; set; }// tabla extra con numeros de parte y descripciones
        [Display(Name = "Precio")]
        public double? price { get; set; }

        [ForeignKey("Primary")]
        [Display(Name = "Areas")]
        public int? AreasID { get; set; }
        [Display(Name = "Champions")]
        public int? AndonSupervisoresID { get; set; }
        [Display(Name = "Descripcion del problema")]
        public string issueDescription { get; set; } /// descripcion detallada del cargo
        [Display(Name = "Causa Raiz")]
        public string rootcause { get; set; } /// droot cause
        [Display(Name = "Contramedidas")]
        public string Countermeasure  { get; set; } ///
        [Display(Name = "Comentarios")]
        public string Comments { get; set; } ///
        [Display(Name = "INV")]
        public string INV { get; set; } ///Amounts valued at financial  statments rate
        
        [Display(Name = "Monto valuado al tipo de cambio del cierre de mes")]
        public double? AVFSR { get; set; } ///Amounts valued at financial  statments rate

        public virtual cAreas Primary { get; set; }

        public virtual AndonSupervisores AndonSupervisores { get; set; }

        public virtual Billto Billto { get; set; }

        public virtual QuaCodes QuaCodes { get; set; }

    }

    public class QuaCodes
    {
        [Key]
        public int QuaCodesID { get; set; }
        [MaxLength(10)]
        [Display(Name = "Codigo")]
        public string Code { get; set; } ///
        [MaxLength(200)]
        [Display(Name = "Definicion de codigo")]
        public string Code_Definition { get; set; } ///

    }

    public class Billto
    {
        [Key]
        public int BilltoID { get; set; }
        [MaxLength(20)]
        [Display(Name = "Bill to")]
        public string bill { get; set; } ///
        [MaxLength(300)]
        [Display(Name = "Nombre de Cliente")]
        public string Costumername { get; set; } ///
        [MaxLength(300)]
        [Display(Name = "Locacion")]
        public string Costumerlocation{ get; set; } ///

    }
}
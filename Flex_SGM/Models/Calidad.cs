using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public int AreasID { get; set; }
        [MaxLength(25)]
        [Display(Name = "Defecto")]
        public string Defecto { get; set; }

        public virtual cAreas Areas { get; set; }
    }
    public class AndonSupervisores
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int AreasID { get; set; }
        [MaxLength(25)]
        [Display(Name = "Supervisor Area")]
        public string Supervisor { get; set; }

        public virtual cAreas Areas { get; set; }
    }
    public class AndonAuditor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int AreasID { get; set; }
        [MaxLength(25)]
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
        [Required]
        [MaxLength(1)]
        public string Turno { get; set; }
        [Required]
        [MaxLength(5)]
        public string Hora { get; set; }
        [Required]
        [Display(Name = "Area Generadora")]
        public int AreasgID { get; set; }

        [Required]
        [Display(Name = "Area Emisora")]
        public int AreaseID { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClientesID { get; set; }

        [Required]
        [Display(Name = "Proyecto")]
        public int ProyectosID { get; set; }
        [MaxLength(20)]
        public string lote { get; set; }
        [MaxLength(100)]
        public string NoDeParte { get; set; }

        [MaxLength(100)]
        public string NombreDeParte { get; set; }

    
        [MaxLength(80)]
        public string AuditorAreaDefecto { get; set; }
        [MaxLength(50)]
        public string ResponsableAndon { get; set; }

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

        public virtual cAreas Areasg { get; set; }
        public virtual cAreas Arease { get; set; }

        public virtual cClientes Clientes { get; set; }
        public virtual cProyectos Proyectos { get; set; }
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
}
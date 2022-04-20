using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flex_SGM.Models
{
    public class Metricos
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd H:mm}")]
        public DateTime DiaHora { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        [MaxLength(100)]
        public string Usuario { get; set; }

        [Display(Name = "Área")]
        [MaxLength(25)]
        public string Usuario_area { get; set; }

        [Display(Name = "Puesto")]
        [MaxLength(25)]
        public string Usuario_puesto { get; set; }

        [Required]
        [Display(Name = "Responsable")]
        [MaxLength(100)]
        public string Usuario_responsable { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [MaxLength(150)]
        public string Comentarios { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        [RegularExpression(@"^\d{1,6}$", ErrorMessage = "Solo numeros enteros positivos y menos de 7 digitos")]
        public int Proyectos { get; set; }

        public virtual Maquinas Maquinas { get; set; }

    }
    public class MyViewMetricos
    {
        public MyViewFallas MyViewFallas { get; set; }
        public Metricos Metricos { get; set; }
    }
    public class ChartInfo
    {
        public DateTime DiaHora { get; set; }
        public string Usuario { get; set; }
        public string Usuario_area { get; set; }
        public string Usuario_puesto { get; set; }
        public string Usuario_responsable { get; set; }
        public string Descripcion { get; set; }
        public int Proyectos { get; set; }
        public double Goal { get; set; }
        public double Calculation { get; set; }

    }
    public class MetricsNew
    {
        public string TiempoLabel { get; set; }
        public int Amef { get; set; }
        public int BiBo { get; set; }
        public double Caps { get; set; }
        public int CapNum { get; set; }
        public double CapPer { get; set; }
        public int CoIm { get; set; }
        public int CuCo { get; set; }
        public int CuSc { get; set; }
        public int Ecn { get; set; }
        public int LaOu { get; set; }
        public int Lpa { get; set; }
        public int PaPo { get; set; }
        public int PaDe { get; set; }
        public int Plm { get; set; }
        public int QuHs { get; set; }
        public int ReRa { get; set; }
        public int Safe { get; set; }
        public int ScCo { get; set; }
        public int Toc { get; set; }
        public int YeSh { get; set; }
    }

}
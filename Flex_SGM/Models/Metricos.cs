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


}
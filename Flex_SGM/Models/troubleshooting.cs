using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class troubleshooting
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [Display(Name = "Usuario")]
        [MaxLength(100)]
        public string usuario { get; set; }

        [Required]
        public int MaquinasID { get; set; }
        [Required]
        [MaxLength(500)]
        public string Problema { get; set; }
        [Required]
        [MaxLength(600)]
        public string Posiblerazon { get; set; }
        [Required]
        public string Solucion { get; set; }

        [MaxLength(800)]
        public string Comentarios { get; set; }
        public virtual Maquinas Maquinas { get; set; }
        // public virtual Fallas Fallas { get; set; }

    }
}
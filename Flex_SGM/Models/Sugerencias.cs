using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flex_SGM.Models
{
    public class Sugerencias
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Mes")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM}")]
        public DateTime DiaHora { get; set; }

        [Display(Name = "Usuario")]
        [MaxLength(100)]
        public string Usuario { get; set; }

        [Display(Name = "Muy Bien")]
        public bool Muy_Bien { get; set; }

        [Display(Name = "Bien")]
        public bool Bien { get; set; }

        [Display(Name = "Mediocre")]
        public bool Mediocre { get; set; }

        [Display(Name = "Mal")]
        public bool Mal { get; set; }

        [Display(Name = "Muy Mal")]
        public bool Muy_Mal { get; set; }

        [Display(Name = "Comentarios")]
        [MaxLength(256)]
        public string Comentarios { get; set; }

    }
}
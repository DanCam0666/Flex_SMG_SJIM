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

        [Display(Name = "Fecha")]
        [DataType(DataType.DateTime)]
        public DateTime DiaHora { get; set; }

        [Display(Name = "Usuario")]
        [MaxLength(100)]
        public string Usuario { get; set; }

        [Display(Name = "Excelente")]
        public bool Excelente { get; set; }

        [Display(Name = "Bien")]
        public bool Bien { get; set; }

        [Display(Name = "Regular")]
        public bool Regular { get; set; }

        [Display(Name = "Mal")]
        public bool Mal { get; set; }

        [Display(Name = "Pésima")]
        public bool Pesima { get; set; }

        [Display(Name = "Comentarios")]
        [MaxLength(256)]
        public string Comentarios { get; set; }

    }
}
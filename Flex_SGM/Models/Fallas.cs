using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flex_SGM.Models
{
    public class Fallas
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(20)]
        public string Area { get; set; }

        [MaxLength(20)]
        
        public string Tipo { get; set; }
        [MaxLength(50)]
   
        public string Descripcion { get; set; }
        [MaxLength(50)]
      
        public string Codigo { get; set; }

        [MaxLength(50)]
 
        public string DescripcionCodigo { get; set; }

    }

    public class qry_fallas
    {
        public string Code { get; set; }

        public string Descripcion { get; set; }
        public string Estatus { get; set; }
    }
    public class MyViewFallas
    {
        [DisplayName("Area de Falla")]
        [Required]
        public string Area { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }

        [DisplayName("Tipo de Falla")]
        public string Tipo { get; set; }
        public IEnumerable<SelectListItem> Tipos { get; set; }

        [DisplayName("Descripcion de Falla")]
        public string Des { get; set; }
        public IEnumerable<SelectListItem> Dess { get; set; }
    }
}
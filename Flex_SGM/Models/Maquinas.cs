using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flex_SGM.Models
{
    public class Maquinas
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(50)]
        public string Area { get; set; }
        [MaxLength(50)]
        public string Cliente { get; set; }
        [Display(Name = "Maquina")]
        [MaxLength(100)]
        [Required(AllowEmptyStrings = false)]
        public string Maquina { get; set; }
        [Display(Name = "Sub Maquina")]
        [MaxLength(100)]
        [Required(AllowEmptyStrings = false)]
        public string SubMaquina { get; set; }
        [MaxLength(50)]
        public string Codigo { get; set; }
        [Display(Name = "Tiempo Produccion")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime? DiaHora { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Ubicacion{ get; set; }

        public virtual ICollection<Bitacora> Bitacora { get; set; }

        public virtual ICollection<OILs> OILs { get; set; }

    }

}
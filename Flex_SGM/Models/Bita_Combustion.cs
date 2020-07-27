using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class Bita_Combustion
    {
        public int ID { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [Display(Name = "Usuario")]
        public string Turno { get; set; }
        [MaxLength(20)]
        public string tipo { get; set; }
        [MaxLength(40)]
        public string usuario { get; set; }
        public string MaquinasID { get; set; }

        [MaxLength(150)]
        public string Sintoma { get; set; }
        [MaxLength(150)]
        public string Causa { get; set; }
        public string AccionCorrectiva { get; set; }
        [Display(Name = "Tiempo Muerto (minutos)")]
        // [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Not Equal to Zero")]
        // [RegularExpression("^(?!0*(\.0+)?$)(\d+|\d*\.\d+)$", ErrorMessage = "Not Equal to Zero")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo numeros enteros positivos")]
        public int Tiempo { get; set; }
        [Display(Name = "Folio del formato")]
        [MaxLength(12)]
        public string Folio { get; set; }

        public Double Porcentaje { get; set; }
        public Double MTBF { get; set; }
        public Double MTTR { get; set; }

        public virtual Maquinas Maquinas { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class ControldeEquipos
    {
        [Key]

        public int ID { get; set; }

        [Required]
        public int MaquinasID { get; set; }

        [Display(Name = "Fecha de modificacion")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [Required]
        [Display(Name = "Nombre del Equipo")]
        public string equipo { get; set; }

        [Required]
        [Display(Name = "Descripcion del Equipo ( Password, ayudas, Etc... )")]
        public string descripcion { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere una IP Local ")]
        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", ErrorMessage = "No es una IP válida.")]
        [Display(Name = "IP Local del Equipo")]
        public string iplocal { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere un correo electrónico")]
        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", ErrorMessage = "No es una IP válida.")]
        [Display(Name = "IP Intranet del Equipo")]
        public string ipintra { get; set; }

        [Required]
        [Display(Name = "vlan de la  Red intranet")]
        public string vlan { get; set; }
        public virtual Maquinas Maquinas { get; set; }
        // public virtual Fallas Fallas { get; set; }




    }
}
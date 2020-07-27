using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace Flex_SGM.Models
{
    public class OILs
    {
        public int ID { get; set; }
        public int MaquinasID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Tipo de OIL")]
        public string Tipo { get; set; }
        [Display(Name = "No. Folio o PCR")]
        [MaxLength(50)]
        public string folio { get; set; }
        [Display(Name = "Detalles de la Actividad a realizar, Ser especifico...")]
        public string Actividad { get; set; }
        [MaxLength(100)]
        [Display(Name = "Genero")]
        public string User_gen{ get; set; }
        [Display(Name = "Fecha Creacion")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [MaxLength(100)]
        [Display(Name = "Responsable")]
        public string User_res { get; set; }
        [Display(Name = "Fecha Compromiso")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime? DiaHora_Compromiso { get; set; }
        [MaxLength(100)]
        [Display(Name = "Asignado")]
        public string User_asig { get; set; }
        [Display(Name = "Fecha de cierre")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime? DiaHora_Cierre { get; set; }
        [Display(Name = "Fecha de Verificacion")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime? DiaHora_Verificado { get; set; }
        [MaxLength(100)]
        [Display(Name = "Actividad Verificada por")]
        public string Usuario_Verifico { get; set; }
        [Display(Name = "Comentarios de Apertura de actividad: ")]
        public string Comentarios { get; set; }
        [Display(Name = "Comentarios de Cierre de actividad: ")]
        public string Comentarios2 { get; set; }
        [Display(Name = "Material de actividad")]
        public string Material_necesario { get; set; }
        public bool urgente { get; set; }
        public int Estatus { get; set; }
        public virtual Maquinas Maquinas { get; set; }
    }

}
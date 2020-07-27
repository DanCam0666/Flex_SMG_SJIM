using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flex_SGM.Models
{
    public class Proyectos
    {

        public int ID { get; set; }
        public int MaquinasID { get; set; }

        [MaxLength(100)]
        [Display(Name = "Genero")]
        public string User_gen { get; set; }
        [Display(Name = "Fecha Creacion")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [MaxLength(100)]
        [Display(Name = "Tipo de Proyecto")]
        public string Tipo { get; set; }
        [Display(Name = "Que? Objetivo del proyecto")]
        public string Que { get; set; }
        [Display(Name = "Por que? Finalidad del proyecto")]
        public string Porque { get; set; }
        [Display(Name = "Para que? Resultados del proyecto")]
        public string Paraque { get; set; }

        [Display(Name = "Para quien? Usuarios del proyecto")]
        public string Paraquien { get; set; }

        [Display(Name = "Donde? Ubicacion del proyecto")]
        public string Donde { get; set; }

        [Display(Name = "Cuando? Cronograma del proyecto")]
        public string Cuando { get; set; }

        [Display(Name = "Con quien? Ejecutores del proyecto")]
        public string Conquien { get; set; }

        [Display(Name = "Con Que? Recursos del proyecto")]
        public string conque { get; set; }

        [Display(Name = "Como? Especificaciones del proyecto")]
        public string Como { get; set; }
    
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
        public string Comentarios { get; set; }
        [Display(Name = "Material para realizar actividad")]
        public bool urgente { get; set; }
        public int Estatus { get; set; }


        public virtual Maquinas Maquinas { get; set; }

    }
}
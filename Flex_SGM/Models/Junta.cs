using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flex_SGM.Models
{
    public class Junta
    {
        [Key]
        public int ID { get; set; }
        public string Accidente { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime Dia_Accidente { get; set; }

        public string OILSSeguridad { get; set; }

        public string OILSnuevosproyectos { get; set; }

        public string Oshas { get; set; }

        public string RelevanteSafety { get; set; }

        public areas ToC_Pendientes { get; set; }

        public areas Vacaciones { get; set; }

        public areas Preventivos { get; set; }

        public areas comentariosa { get; set; }
        public string comentarios { get; set; }

    }

    public class areas
    {
            
        public string Servicios { get; set; }

        public string Montacargas { get; set; }

        public string Estampado { get; set; }

        public string Soldadura { get; set; }

        public string Cromo1 { get; set; }

        public string Pulido1 { get; set; }

        public string Cromo2 { get; set; }

        public string Pulido2 { get; set; }

        public string Ensamble { get; set; }

        public string E_Coat { get; set; }

        public string Top_Coat { get; set; }


    }
}
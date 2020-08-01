using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class General
    {


    }

    public class cClientes
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Cliente")]
        public string Cliente { get; set; }
    }
    public class SubClientes
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ClientesID { get; set; }
        [MaxLength(25)]
        [Display(Name = "SubCliente")]
        public string SubCliente { get; set; }

        public virtual cClientes Clientes { get; set; }
    }

    public class cProyectos
    {
        [Key]
        public int ID { get; set; }

        public int? ClientesID { get; set; }
        [MaxLength(25)]
        [Display(Name = "Proyecto")]
        public string Proyecto { get; set; }

        public virtual cClientes Clientes { get; set; }
    }
    public class cAreas
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(25)]
        [Display(Name = "Area")]
        public string Area { get; set; }
    }




}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Flex_SGM.Models
{
    public class Comments
    {
        public int ID { get; set; }
        [MaxLength(100)]
        public string Usuario { get; set; }
        public DateTime fecha { get; set; }
        public string message { get; set; }

        public OILs OILs { get; set; }


    }
}
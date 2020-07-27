using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class Perfil
    {
        public IEnumerable<Bitacora> bita { get; set; }
        public IEnumerable<OILs> oils { get; set; }
        public ApplicationUser Apuser { get; set; }

    }
}
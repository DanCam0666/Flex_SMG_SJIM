using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class Engineering
    {

        public class flujopcr
        {
            [Key]
            public int ID { get; set; }

            public int pcrrequestid { get; set; }

            public int pcrrequestlvl { get; set; }

            public int pcrverification { get; set; }
            public int pcrdecision { get; set; }

            public int pcrclientapproval { get; set; }

            public int pcrclientdecision { get; set; }

            public int pcrmanagerdecision { get; set; }

            public int pcrmanagerclose { get; set; }
        }

        public class pcr
        {
            [Key]
            public int ID { get; set; }

            public int Originator { get; set; }

            public int Department { get; set; }
            public int Date { get; set; }

            public int Customer { get; set; }

            public int Program { get; set; }
            public int Reason { get; set; }

            public int PartNumber { get; set; }

            public int RevLevel { get; set; }
            public int PartName { get; set; }

            public int docreason { get; set; }
            public int docscope { get; set; }
            public int doctypeofchange { get; set; }

            public int cipieceprice { get; set; }
            public int cicapital { get; set; }
            public int citooling { get; set; }
            public int ciengineering { get; set; }
            public int cipackaging { get; set; }
            public int ciobsolescence { get; set; }
            public int cimaterial { get; set; }
            public int cifreight { get; set; }
            public int ciovertime { get; set; }
            public int ciother { get; set; }
            public int citotal { get; set; }

            public int crannualvolume { get; set; }
            public int crcapacityfng { get; set; }
            public int crcapacitysupplier { get; set; }

            public int Reviewedby { get; set; }
            public int Reviewedby_date { get; set; }

            public int support_purchasing { get; set; }
            public int support_materials { get; set; }
            public int support_maintenance { get; set; }
            public int support_automation { get; set; }
            public int support_quality { get; set; }
            public int support_safety { get; set; }
            public int support_environmental { get; set; }
            public int support_tooling { get; set; }
            public int support_stamping { get; set; }
            public int support_welding { get; set; }
            public int support_chrome { get; set; }
            public int support_ecoat { get; set; }
            public int support_topcoat { get; set; }
            public int support_backcoat { get; set; }
            public int support_assembly { get; set; }
            public int support_finance { get; set; }

            public int Keymilestones_buildmrd1 { get; set; }

            public int Keymilestones_buildmrd2 { get; set; }
            public int Keymilestones_buildmrd3 { get; set; }

            public int Keymilestones_customrrar { get; set; }
            public int Keymilestones_ppap { get; set; }
            public int Keymilestones_internalsop { get; set; }

            public int Keymilestones_customersop { get; set; }
            public int Keymilestones_closure { get; set; }

            public int leadtime_engineering { get; set; }

            public int leadtime_tooling { get; set; }
            public int leadtime_facilities { get; set; }
            public int leadtime_capital { get; set; }
            public int leadtime_material { get; set; }
            public int leadtime_inventory { get; set; }
            public int leadtime_approval { get; set; }
            public int leadtime_totallt { get; set; }
         

        }





    }
}
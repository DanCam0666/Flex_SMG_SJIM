using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{


    public class pcr
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Originator")]
        public int? OriginatorID { get; set; }

        [Display(Name = "Department")]
        public int AreasID { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Customer")]
        public int ClientesID { get; set; }

        [Display(Name = "Program")]
        public int ProyectosID { get; set; }
        [Display(Name = "Reason")]
        public int? ReasonID { get; set; }
        [MaxLength(100)]
        public string PartNumber { get; set; }
        [MaxLength(50)]
        public string RevLevel { get; set; }
        [MaxLength(200)]
        public string PartName { get; set; }
        [Display(Name = "Description Reason")]
        public string docreason { get; set; }
        [Display(Name = "Scope")]
        public string docscope { get; set; }
        [Display(Name = "type of change")]
        public string doctypeofchange { get; set; }

        public double cipieceprice { get; set; }
        public double cicapital { get; set; }
        public double citooling { get; set; }
        public double ciengineering { get; set; }
        public double cipackaging { get; set; }
        public double ciobsolescence { get; set; }
        public double cimaterial { get; set; }
        public double cifreight { get; set; }
        public double ciovertime { get; set; }
        public double ciother { get; set; }
        public double citotal { get; set; }
        [MaxLength(15)]
        public string crannualvolume { get; set; }
        [MaxLength(15)]
        public string crcapacityfng { get; set; }
        [MaxLength(15)]
        public string crcapacitysupplier { get; set; }
        [MaxLength(100)]
        public string Reviewedby { get; set; }
        public DateTime? Reviewedby_date { get; set; }
        [MaxLength(5)]
        public string support_purchasing { get; set; }
        [MaxLength(5)]
        public string support_materials { get; set; }
        [MaxLength(5)]
        public string support_maintenance { get; set; }
        [MaxLength(5)]
        public string support_automation { get; set; }
        [MaxLength(5)]
        public string support_quality { get; set; }
        [MaxLength(5)]
        public string support_safety { get; set; }
        [MaxLength(5)]
        public string support_environmental { get; set; }
        [MaxLength(5)]
        public string support_tooling { get; set; }
        [MaxLength(5)]
        public string support_stamping { get; set; }
        [MaxLength(5)]
        public string support_welding { get; set; }
        [MaxLength(5)]
        public string support_chrome { get; set; }
        [MaxLength(5)]
        public string support_ecoat { get; set; }
        [MaxLength(5)]
        public string support_topcoat { get; set; }
        [MaxLength(5)]
        public string support_backcoat { get; set; }
        [MaxLength(5)]
        public string support_assembly { get; set; }
        [MaxLength(5)]
        public string support_finance { get; set; }
        [MaxLength(10)]
        public string Keymilestones_buildmrd1 { get; set; }
        [MaxLength(10)]
        public string Keymilestones_buildmrd2 { get; set; }
        [MaxLength(10)]
        public string Keymilestones_buildmrd3 { get; set; }
        [MaxLength(10)]
        public string Keymilestones_customrrar { get; set; }
        [MaxLength(10)]
        public string Keymilestones_ppap { get; set; }
        [MaxLength(10)]
        public string Keymilestones_internalsop { get; set; }
        [MaxLength(10)]

        public string Keymilestones_customersop { get; set; }
        [MaxLength(10)]
        public string Keymilestones_closure { get; set; }

        public double leadtime_engineering { get; set; }
        public double leadtime_tooling { get; set; }
        public double leadtime_facilities { get; set; }
        public double leadtime_capital { get; set; }
        public double leadtime_material { get; set; }
        public double leadtime_inventory { get; set; }
        public double leadtime_approval { get; set; }
        public double leadtime_totallt { get; set; }
        //*--------------*//
        [MaxLength(25)]
        public string pcrrequestlvl { get; set; }

        public int pcrverification { get; set; }
        public int pcrdecision { get; set; }

        public int pcrclientapproval { get; set; }

        public int pcrclientdecision { get; set; }

        public int pcrmanagerdecision { get; set; }

        public int pcrmanagerclose { get; set; }

        public virtual ereason Reason { get; set; }
        public virtual eoriginator Originator { get; set; }
        public virtual cAreas Areas { get; set; }
        public virtual cClientes Clientes { get; set; }
        public virtual cProyectos Proyectos { get; set; }
    }

    public class eoriginator
    {
        [Key]
        public int ID { get; set; }


        public int AreasID { get; set; }
        [MaxLength(100)]
        [Display(Name = "Supervisor Area")]
        public string Supervisor { get; set; }

        public virtual cAreas Areas { get; set; }
    }
    public class ereason
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(250)]
        public string Reason { get; set; }

    }

}
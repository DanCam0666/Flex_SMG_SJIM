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

        [MaxLength(13)]
        public string PCRID { get; set; }

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
        [Display(Name = "Piece Price")]
        public double cipieceprice { get; set; }
        [Display(Name = "Capital")]
        public double cicapital { get; set; }
        [Display(Name = "Tooling")]
        public double citooling { get; set; }
        [Display(Name = "Engineering")]
        public double ciengineering { get; set; }
        [Display(Name = "Packaging")]
        public double cipackaging { get; set; }
        [Display(Name = "Obsolescence")]
        public double ciobsolescence { get; set; }
        [Display(Name = "Material")]
        public double cimaterial { get; set; }
        [Display(Name = "Freight")]
        public double cifreight { get; set; }
        [Display(Name = "Overtime")]
        public double ciovertime { get; set; }
        [Display(Name = "Other")]
        public double ciother { get; set; }
        [Display(Name = "Total")]
        public double citotal { get; set; }
        [MaxLength(15)]
        [Display(Name = "Annual Volume")]
        public string crannualvolume { get; set; }
        [MaxLength(15)]
        [Display(Name = "Capacity FNG")]
        public string crcapacityfng { get; set; }
        [MaxLength(15)]
        [Display(Name = "Capacity Supplier")]
        public string crcapacitysupplier { get; set; }
        [MaxLength(100)]
        [Display(Name = "Reviewed by")]
        public string Reviewedby { get; set; }
        [Display(Name = "Reviewed Date")]
        public DateTime? Reviewedby_date { get; set; }
        [MaxLength(5)]
        [Display(Name = "Purchasing")]
        public string support_purchasing { get; set; }
        [MaxLength(5)]
        [Display(Name = "Meterials")]
        public string support_materials { get; set; }
        [MaxLength(5)]
        [Display(Name = "Maintenance")]
        public string support_maintenance { get; set; }
        [MaxLength(5)]
        [Display(Name = "Automation")]
        public string support_automation { get; set; }
        [MaxLength(5)]
        [Display(Name = "Quality")]
        public string support_quality { get; set; }
        [MaxLength(5)]
        [Display(Name = "Safety")]
        public string support_safety { get; set; }
        [MaxLength(5)]
        [Display(Name = "Environmental")]
        public string support_environmental { get; set; }
        [MaxLength(5)]
        [Display(Name = "Tooling")]
        public string support_tooling { get; set; }
        [MaxLength(5)]
        [Display(Name = "Stamping")]
        public string support_stamping { get; set; }
        [MaxLength(5)]
        [Display(Name = "Welding")]
        public string support_welding { get; set; }
        [MaxLength(5)]
        [Display(Name = "Chrome")]
        public string support_chrome { get; set; }
        [MaxLength(5)]
        [Display(Name = "E-Coat")]
        public string support_ecoat { get; set; }
        [MaxLength(5)]
        [Display(Name = "Top-Coat")]
        public string support_topcoat { get; set; }
        [MaxLength(5)]
        [Display(Name = "back-Coat")]
        public string support_backcoat { get; set; }
        [MaxLength(5)]
        [Display(Name = "Assembly")]
        public string support_assembly { get; set; }
        [MaxLength(5)]
        [Display(Name = "Finance")]
        public string support_finance { get; set; }
        [MaxLength(10)]
        [Display(Name = "MRD 1")]
        public string Keymilestones_buildmrd1 { get; set; }
        [MaxLength(10)]
        [Display(Name = "MRD 2")]
        public string Keymilestones_buildmrd2 { get; set; }
        [MaxLength(10)]
        [Display(Name = "MRD 3")]
        public string Keymilestones_buildmrd3 { get; set; }
        [MaxLength(10)]
        [Display(Name = "Customer R@R")]
        public string Keymilestones_customrrar { get; set; }
        [MaxLength(10)]
        [Display(Name = "PPAP")]
        public string Keymilestones_ppap { get; set; }
        [MaxLength(10)]
        [Display(Name = "Internal SOP")]
        public string Keymilestones_internalsop { get; set; }
        [MaxLength(10)]
        [Display(Name = "Customer SOP")]
        public string Keymilestones_customersop { get; set; }
        [MaxLength(10)]
        [Display(Name = "Closure")]
        public string Keymilestones_closure { get; set; }
        [Display(Name = "Engineering")]
        public double leadtime_engineering { get; set; }
        [Display(Name = "Tooling")]
        public double leadtime_tooling { get; set; }
        [Display(Name = "Facilities")]
        public double leadtime_facilities { get; set; }
        [Display(Name = "Capital")]
        public double leadtime_capital { get; set; }
        [Display(Name = "Material")]
        public double leadtime_material { get; set; }
        [Display(Name = "Inventory")]
        public double leadtime_inventory { get; set; }
        [Display(Name = "Approval")]
        public double leadtime_approval { get; set; }
        [Display(Name = "Total LT")]
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


    public class templatepcr
    {
      
        public string OriginatorID { get; set; }


        public string AreasID { get; set; }

        public string Date { get; set; }


        public string ClientesID { get; set; }


        public string ProyectosID { get; set; }

        public string ReasonID { get; set; }

        public string PartNumber { get; set; }

        public string RevLevel { get; set; }

        public string PartName { get; set; }

        public string docreason { get; set; }

        public string docscope { get; set; }

        public string doctypeofchange { get; set; }

        public string cipieceprice { get; set; }

        public string cicapital { get; set; }

        public string citooling { get; set; }
       
        public string ciengineering { get; set; }
      
        public string cipackaging { get; set; }

        public string ciobsolescence { get; set; }
 
        public string cimaterial { get; set; }

        public string cifreight { get; set; }
    
        public string ciovertime { get; set; }

        public string ciother { get; set; }
  
        public string citotal { get; set; }

   
        public string crannualvolume { get; set; }

        public string crcapacityfng { get; set; }

        public string crcapacitysupplier { get; set; }

        public string Reviewedby { get; set; }
   
        public string Reviewedby_date { get; set; }

        public string support_purchasing { get; set; }

        public string support_materials { get; set; }

        public string support_maintenance { get; set; }

        public string support_automation { get; set; }

        public string support_quality { get; set; }

        public string support_safety { get; set; }

        public string support_environmental { get; set; }

        public string support_tooling { get; set; }

        public string support_stamping { get; set; }

        public string support_welding { get; set; }

        public string support_chrome { get; set; }

        public string support_ecoat { get; set; }

        public string support_topcoat { get; set; }

        public string support_backcoat { get; set; }

        public string support_assembly { get; set; }

        public string support_finance { get; set; }

        public string Keymilestones_buildmrd1 { get; set; }

        public string Keymilestones_buildmrd2 { get; set; }

        public string Keymilestones_buildmrd3 { get; set; }

        public string Keymilestones_customrrar { get; set; }

        public string Keymilestones_ppap { get; set; }

        public string Keymilestones_internalsop { get; set; }

        public string Keymilestones_customersop { get; set; }

        public string Keymilestones_closure { get; set; }

        public string leadtime_engineering { get; set; }

        public string leadtime_tooling { get; set; }

        public string leadtime_facilities { get; set; }

        public string leadtime_capital { get; set; }
    
        public string leadtime_material { get; set; }

        public string leadtime_inventory { get; set; }

        public string leadtime_approval { get; set; }

        public string leadtime_totallt { get; set; }
        //*--------------*//

        public string pcrrequestlvl { get; set; }

        public string pcrverification { get; set; }
        public string pcrdecision { get; set; }

        public string pcrclientapproval { get; set; }

        public string pcrclientdecision { get; set; }

        public string pcrmanagerdecision { get; set; }

        public string pcrmanagerclose { get; set; }

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
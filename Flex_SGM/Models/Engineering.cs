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

        [MaxLength(20)]

        [Display(Name = "En Proceso de")]/// CAMBIAR POR USUARIO aCTUAL 
        public string Status { get; set; }

        [Display(Name = "Originator")]/// CAMBIAR POR USUARIO aCTUAL 
        [MaxLength(140)]
        public string Originator { get; set; }

        [Display(Name = "Department")]
        [MaxLength(70)]
        public string Department { get; set; }

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
        public int MatrizDecisionID { get; set; }// SELECCION DESDE TABLA 

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

        [Display(Name = "Reviewed by")]/// Reviewed byReviewed by
        [MaxLength(140)]
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
        //*------Feasibility--------*//
        [Display(Name = "Is product adequately defined (application requirements, etc. to enable feasibility evaluation?")]
        public bool FConsiderations1 { get; set; }
        [Display(Name = "Can Engineering Performance Specifications be met as written?")]
        public bool FConsiderations2 { get; set; }
        [Display(Name = "Can product be manufactured to tolerances specified on drawing?")]
        public bool FConsiderations3 { get; set; }
        [Display(Name = "Can product be manufactured with process capability that meet requirements?")]
        public bool FConsiderations4 { get; set; }
        [Display(Name = "Is there adequate capacity to produce product?")]
        public bool FConsiderations5 { get; set; }
        [Display(Name = "Does the design allow the use of efficient material handling techniques?")]
        public bool FConsiderations6 { get; set; }
        [Display(Name = "Can the product be manufactured without incurring any unusual Costs for capital equipment?")]
        public bool FConsiderations7 { get; set; }
        [Display(Name = "Can the product be manufactured without incurring any unusual Costs for tooling?")]
        public bool FConsiderations8 { get; set; }
        [Display(Name = "Can the product be manufactured without incurring any unusual Alternative manufacturing methods?")]
        public bool FConsiderations9 { get; set; }
        [Display(Name = "Is statistical process control required on the product?")]
        public bool FConsiderations10 { get; set; }
        [Display(Name = "Is statistical process control presently used on similar products?")]
        public bool FConsiderations11 { get; set; }
        [Display(Name = "Where statistical process control is used on similar products. Are the processesing control reliable and stable?")]
        public bool FConsiderations12 { get; set; }
        [Display(Name = "Where statistical process control is used on similar products. Does process capability meet customer requirements?")]
        public bool FConsiderations13 { get; set; }
        [Display(Name = "Are due Care and Product Safety Items Involved, and can be met?")]
        public bool FConsiderations14 { get; set; }
        [Display(Name = "Were all Customer Specific requirements  reviewed, an can be met?")]
        public bool FConsiderations15 { get; set; }
        //*------Risk --------*//
        [Display(Name = "Does exist any risk asociated with the adoption of this Change?")]
        public bool FRisk1 { get; set; }
        [Display(Name = "Use of any special material, for which we do not have expertise?")]
        public bool FRisk2 { get; set; }
        [Display(Name = "Use of any Manufacturing Technology for which we do not have actual experience?")]
        public bool FRisk3 { get; set; }
        [Display(Name = "Does need any Capital Invesment aditional to the actual installed?")]
        public bool FRisk4 { get; set; }
        [Display(Name = "Does need the adoption of any new supplier which we do not have experience with?")]
        public bool FRisk5 { get; set; }
        [Display(Name = "Not achievement of the projected volumes by the customer will create a risk?")]
        public bool FRisk6 { get; set; }
        [Display(Name = "Does the change represent any safety issue to create a risk?")]
        public bool FRisk7 { get; set; }
        [Display(Name = "What is the risk level of the change (1-Low, 2-Med, >3-High)")]
        public int FRisk8 { get; set; }




        public virtual ereason Reason { get; set; }
        public virtual cClientes Clientes { get; set; }
        public virtual cProyectos Proyectos { get; set; }

        public virtual MatrizDecision MatrizDecision { get; set; }
    }
    public class templatepcr
    {

        public string pcrID { get; set; }
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

    public class MatrizDecision
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Nivel de Riesgo")]
        public int NivelRiesgo { get; set; }

        [Display(Name = "Tipo de Cambio/Inpacto")]
        public string TipoCambio { get; set; }

        [Display(Name = "Comunicacion Hacia el cliente")]
        public bool commcliente { get; set; }
        //AR  Aprovaciones requeridas
        [Display(Name = "Planta")]
        public bool Arplanta { get; set; }
        [Display(Name = "Ingeniería")]
        public bool ArIngenieria { get; set; }
        [Display(Name = "Manufactura")]
        public bool Armanufactura { get; set; }
        [Display(Name = "Calidad")]
        public bool Arcalidad { get; set; }
        [Display(Name = "Finanzas")]
        public bool Arfinanzas { get; set; }
        [Display(Name = "Compras")]
        public bool Arcompras { get; set; }
        [Display(Name = "Materiales")]
        public bool Armateriales { get; set; }
        [Display(Name = "Mantenimiento")]
        public bool Armantenimiento  { get; set; }
        [Display(Name = "Seguridad")]
        public bool Arseguridad { get; set; }
        [Display(Name = "Ambiental")]
        public bool Arambiental { get; set; }
        [Display(Name = "Tooling")]
        public bool Artooling { get; set; }
        [Display(Name = "Estampado")]
        public bool Arestampado { get; set; }
        [Display(Name = "Soldadura")]
        public bool Arsoldadura { get; set; }
        [Display(Name = "Cromo")]
        public bool Arcromo { get; set; }
        [Display(Name = "Pintura")]
        public bool Arpintura { get; set; }
        [Display(Name = "Ensamble")]
        public bool Arensamble { get; set; }

        //DA  documentos afectados
        [Display(Name = "PPAP")]
        public bool Ppap { get; set; }
        [Display(Name = "DRW")]
        public bool Drw { get; set; }
        [Display(Name = "SPEC")]
        public bool Spec { get; set; }
        [Display(Name = "PFD")]
        public bool Pfd { get; set; }
        [Display(Name = "PFMEA")]
        public bool Pfmea { get; set; }
        [Display(Name = "SW")]
        public bool Sw { get; set; }
        [Display(Name = "PCP")]
        public bool Pcp { get; set; }
        [Display(Name = "IS")]
        public bool Is { get; set; }
        [Display(Name = "MSA")]
        public bool Msa { get; set; }
        [Display(Name = "PS")]
        public bool Ps { get; set; }
        [Display(Name = "SC/CC")]
        public bool Sccc { get; set; }
        [Display(Name = "PL")]
        public bool Pl { get; set; }
        [Display(Name = "CPO")]
        public bool Cpo { get; set; }
        [Display(Name = "SPO")]
        public bool Spo { get; set; }
        [Display(Name = "IMDS")]
        public bool Imds { get; set; }
        [Display(Name = "IM")]
        public bool Im { get; set; }
        [Display(Name = "BOM")]
        public bool Bom { get; set; }
        [Display(Name = "PR")]
        public bool Pr { get; set; }
        [Display(Name = "MSS")]
        public bool Mss { get; set; }
        [Display(Name = "VA")]
        public bool Va { get; set; }
        [Display(Name = "SM")]
        public bool Sm { get; set; }
        [Display(Name = "PTR")]
        public bool Ptr { get; set; }
        [Display(Name = "PDR")]
        public bool Pdr { get; set; }
    }
    public class FeasibilitySigns
    {
        [Key]
        public int sid { get; set; }

        public int pcrID { get; set; }

        [Display(Name = "Dep")]/// Reviewed byReviewed by
        [MaxLength(100)]
        public string Dep { get; set; }

        [Display(Name = "Reviewed by")]/// Reviewed byReviewed by
        [MaxLength(140)]
        public string Reviewedby { get; set; }

        [Display(Name = "Reviewed Date")]
        public DateTime? Reviewedby_date { get; set; }

        [Display(Name = "MSG")]
        public string msg { get; set; }

        [Display(Name = "Status")]
        [MaxLength(100)]
        public string Status { get; set; }


        public virtual pcr pcr { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flex_SGM.Models
{
    public class Bitacora
    {
        [Key]
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime DiaHora { get; set; }
        [Display(Name = "Usuario")]
        [MaxLength(100)]
        public string usuario { get; set; }
        [MaxLength(25)]
        public string usuario_area { get; set; }
        [MaxLength(25)]
        public string usuario_puesto { get; set; }
        [Required]
        public int MaquinasID { get; set; }
        [Required]
        [MaxLength(300)]
        public string Sintoma { get; set; }
        [Required]
        public string Causa { get; set; }
        [Required]
        public string AccionCorrectiva { get; set; }
        [MaxLength(150)]
        public string Atendio { get; set; }
        [Display(Name = "Falla de Operacion ")]
        public bool Fallaoperacion { get; set; }
        [Display(Name = "Tiempo Muerto (minutos)")]
        // [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Not Equal to Zero")]
        // [RegularExpression("^(?!0*(\.0+)?$)(\d+|\d*\.\d+)$", ErrorMessage = "Not Equal to Zero")]
        [RegularExpression(@"^\d{1,6}$", ErrorMessage = "Solo numeros enteros positivos y menos de 7 digitos")]
        public int Tiempo { get; set; }
        [Display(Name = "Scrap Generado")]
        [MaxLength(150)]
        public string Scrap { get; set; }
        [Display(Name = "Folio del formato")]
        [MaxLength(12)]
        public string Folio { get; set; }

        public Double Porcentaje { get; set; }
        public Double MTBF { get; set; }
        public Double MTTR { get; set; }

        [MaxLength(50)]
        public string Verifico { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime? FechaVerificacion { get; set; }
        [Display(Name = "Mtto Preventivo ")]
        public bool MttoPreventivo { get; set; }
        [Display(Name = "Mtto Correctivo ")]
        public bool MttoCorrectivo { get; set; }
        [Display(Name = "Mtto Mejora ")]
        public bool MttoMejora { get; set; }
        [Display(Name = "Trabajo no Finalizado")]
        public bool noterminado { get; set; }
        [Display(Name = "Plan Fin de semana")]
        public bool findesemana { get; set; }
        [MaxLength(3)]
        public string turno { get; set; }
       [MaxLength(60)]
        public string Tipos { get; set; }
        [MaxLength(80)]
        public string Descripcion { get; set; }
        public virtual Maquinas Maquinas { get; set; }
       // public virtual Fallas Fallas { get; set; }
    }

    public class roadmap {
        public string Falla { get; set; }
        public int sumfallas { get; set; }
        public int  sumatiempo { get; set; }
        public int contribuciont { get; set; }
        public int contribuciontacc { get; set; }
        public int contribucionf { get; set; }
        public int contribucionfacc { get; set; }
    }
    public class Repos
    {
        public Bitacora servicios { get; set; }
        public Bitacora Estampado { get; set; }
        public Bitacora PRocesos_Soldadura { get; set; }
        public Bitacora Matto_Soldadura { get; set; }
        public Bitacora Metal_Finish { get; set; }
        public Bitacora Ensamble { get; set; }

        public Bitacora Automatizacion { get; set; }
    }


    public class MyViewBitcora
    {
        public MyViewFallas MyViewFallas { get; set; }

        public Bitacora Bitacora { get; set; }
    }

    public class newmetricos
    {
        public string Maquina { get; set; }
        public int count { get; set; }
        public int time { get; set; }
        public int count1 { get; set; }
        public int time1 { get; set; }
        public int count2 { get; set; }
        public int time2 { get; set; }
        public int count3 { get; set; }
        public int time3 { get; set; }
        public double Minmtbf { get; set; }
        public double Maxmttr { get; set; }
        public double Dis { get; set; }
        public double NewMinmtbf1 { get; set; }
        public double NewMaxmttr1 { get; set; }
        public double NewMinmtbf2 { get; set; }
        public double NewMaxmttr2 { get; set; }
        public double NewMinmtbf3 { get; set; }
        public double NewMaxmttr3 { get; set; }

    }

    public class newmetricos2
    {
        public double Disponible1 { get; set; }
        public double Disponible2 { get; set; }
        public double Disponible3 { get; set; }
        public double TiempoM1 { get; set; }
        public double TiempoM2 { get; set; }
        public double TiempoM3 { get; set; }
        public double FallasT1 { get; set; }
        public double FallasT2 { get; set; }
        public double FallasT3 { get; set; }
        public string TiempoLabel { get; set; }
        public double TiempoMuerto1 { get; set; }
        public double TiempoMuerto2 { get; set; }
        public double TiempoMuerto3 { get; set; }
        public double MTTR1 { get; set; }
        public double MTTR2 { get; set; }
        public double MTTR3 { get; set; }
        public double MTBF { get; set; }
        public double TarjetasTPM { get; set; }
        public double Confiabilidad { get; set; }

    }
    public class newmetricos3
    {
        public string TiempoLabel { get; set; }
        public string maquina { get; set; }
        public double Disponible1 { get; set; }
        public double Disponible2 { get; set; }
        public double Disponible3 { get; set; }
        public double TiempoMuerto1 { get; set; }
        public double TiempoMuerto2 { get; set; }
        public double TiempoMuerto3 { get; set; }
        public double CantidadFallas1 { get; set; }
        public double CantidadFallas2 { get; set; }
        public double CantidadFallas3 { get; set; }
        public double MTTR1 { get; set; }
        public double MTTR2 { get; set; }
        public double MTTR3 { get; set; }
        public double MTBF { get; set; }
        public double TarjetasTPM { get; set; }
        public double Confiabilidad { get; set; }

    }
    public class newmetricosmaquina
    {
        public string maquina { get; set; }
        public double tiempod { get; set; }
        public double fallas { get; set; }
        public double tiempof { get; set; }
        public double mttr { get; set; }
        public double mtbf { get; set; }    
        public double confiabilidad { get; set; }

    }

}
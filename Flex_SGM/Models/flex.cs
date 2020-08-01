using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public enum flex_Cliente
    {
        GM,FCA, Nissan, Volkswagen, Toyota, Ford, FlexNGate

    }

    public enum flex_Depas
    {
        Produccion, Calidad, Mantenimiento, Herramentales, Ingenieria, Manufactura, Automatizacion, FlexNGate

    }
    public enum flex_Areasv1
    {
        Servicios, Estampado,MetalFinish, Cromo, Pintura, Mtto_Soldadura, Proc_Soldadura, Ensamble, Automatizacion, FlexNGate

    }
    public enum flex_Areas
    {
        Servicios, Estampado, MetalFinish, Cromo, Cromo1, Cromo2, AutoPulido1, AutoPulido2, Pintura, Ecoat, Topcoat, Soldadura, Ensamble, Automatizacion, FlexNGate

    }
    public enum flex_Puesto
    {
        Gerente, SuperIntendente, Asistente, Supervisor, Ingeniero, Lider, Tecnico, Aprendiz, FlexNGate

    }

    public enum flex_Oils
    {
       TPM,Montacargas, Seguridad, Mantenimiento, Automatizacion

    }


    /** e1a04367-19d1-4b44-acae-ecbd2ed9d885	Admin
ce81e36f-49b9-4e13-b082-7d57aff3d98f	Calidad
4dee5941-3457-4dcb-b4d6-a7329263a101	Ingenieria
da2775b7-5438-4b24-8834-256445f32335	Mantenimiento
acfa3932-f4ae-423d-94a8-883fc3e5596f	Supervisor




    Update-Database [-SourceMigration ] [-TargetMigration ] [-Script] [-Force] [-ProjectName ] [-StartUpProjectName ] [-ConfigurationTypeName ] [-ConnectionStringName ] []
     * */
}
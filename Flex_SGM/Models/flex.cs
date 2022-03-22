using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public enum flex_Cliente
    {
        GM, Stellantis, Ford, Toyota, Nissan, Volkswagen
    }
    public enum flex_Depasold
    {
        Materiales, Produccion, Calidad, Mantenimiento, Herramentales, Ingenieria, Manufactura, Automatizacion, FlexNGate
    }
    public enum flex_Departamento
    {
       Estampado, Rolado, Soldadura, Cromo, Ecoat, Topcoat, Ensamble
    }
    public enum flex_Areasv1
    {
        Safety_HS, Scrap, LPA_COVID, PLM, GAPs, ECNs_PCRs, Capacities, Customer_Complaints, Lay_Out, Cont_Imprv_ManP, TOC_HS_Audits, Cust_Score_Cards, Parts_Delivery, LIM_Invt_Ctrl, Quality_HS, Blue_Book, Red_Rabbits, AMEF, Packaging, Yellow_Sheets
    }
    public enum flex_Areas
    {
        Ingenieria, Manufactura, Servicios, Estampado, MetalFinish, Cromo1, Cromo2, AutoPulido1, AutoPulido2, Ecoat, Topcoat, Soldadura, Ensamble, Automatizacion, FlexNGate
    }
    public enum flex_Puesto
    {
        Gerente, SuperIntendente, Asistente, Supervisor, Ingeniero, Lider, Tecnico, Aprendiz, FlexNGate
    }
    public enum flex_Oils
    {
        Ingenieria, Manufactura
    }


    /** e1a04367-19d1-4b44-acae-ecbd2ed9d885	Admin
        ce81e36f-49b9-4e13-b082-7d57aff3d98f	Calidad
        4dee5941-3457-4dcb-b4d6-a7329263a101	Ingenieria
        da2775b7-5438-4b24-8834-256445f32335	Mantenimiento
        acfa3932-f4ae-423d-94a8-883fc3e5596f	Supervisor
        7a269541-b9f5-4bfe-8eea-38c0ebe11373    Gerentes



    Update-Database [-SourceMigration ] [-TargetMigration ] [-Script] [-Force] [-ProjectName ] [-StartUpProjectName ] [-ConfigurationTypeName ] [-ConnectionStringName ] []
     * */
}
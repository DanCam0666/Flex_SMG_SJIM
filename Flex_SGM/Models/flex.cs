﻿using System;
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
        Cromo, Ecoat, Empaque, Ensamble, Estampado, Rolado, Soldadura, Topcoat
    }
    public enum flex_Areasv1
    {
        None, AMEF_N1_N4, AMEF_N2_N3, Build_In_Build_Out, Capacities_Review, Continuous_Improvment, Customer_Complaints, PPAPs, ECNs_PCRs, Lay_Outs, LPA_Bluebook, Packaging, Vacaciones, PLM, Capacity_Tickets, Red_Rabbits, Highlights, Scrap, MDRs, Yellow_Sheets, FlexNGate
    }
    public enum flex_Areas
    {
        Ingenieria, Manufactura, Calidad, Finanzas, Compras, Materiales, Mantenimiento, Produccion, Seguridad, Ambiental, Tooling, Servicios, Estampado, MetalFinish, Cromo1, Cromo2, AutoPulido1, AutoPulido2, Ecoat, Topcoat, Soldadura, Ensamble, Automatizacion, FlexNGate
    }
    public enum flex_Puesto
    {
        Aprendiz, Asistente, FlexNGate, Gerente, Ingeniero, Lider, Superintendente, Supervisor, Tecnico
    }
    public enum flex_Oils
    {
		Calidad, Ingenieria, Manufactura
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
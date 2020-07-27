using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;       
using ClosedXML.Excel;
using ClosedXML.Extensions;
using System.Globalization;

namespace Flex_SGM.Controllers
{
      public class BitacorasControllerno : Controller
    {


        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /* public ActionResult Download(string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false)
         {
             using (var wb = GenerateClosedXMLWorkbook())
             {
                 // Add ClosedXML.Extensions in your using declarations

                 return wb.Deliver("generatedfile.xlsx");

                 // or specify the content type:
                 return wb.Deliver("generatedFile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
             }
         }*/
        public FileResult Export(string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false,bool nt=false, bool fs = false)
        {
            //******************************
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff + new TimeSpan(23, 0, 0);
            var ff3 = ff.AddDays(1) + new TimeSpan(8, 0, 0);
            var f1i = 0;
            var f1f = 0;
            var f2i = 0;
            var f2f = 0;
            var f3i = 25;
            var f3f = -1;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
            }



            //************************


            var bitacora = from s in db.Bitacoras.Include(b => b.Maquinas)
                           select s;
            if (nt)
                bitacora = bitacora.Where(s => s.noterminado == true);
            if (fs)
                bitacora = bitacora.Where(s => s.findesemana == true);
            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                if (area.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.usuario_area == "Cromo" || s.usuario_area == "Pintura" || s.usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.usuario_area.Contains(area));
            }
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }

                                    if (amaquina.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
             if (amaquina=="Cromo")
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
                if (amaquina.Contains("Pintura"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacora = bitacora.Where(s => s.Maquinas.Area.Contains(amaquina));
            }
            if (!String.IsNullOrEmpty(puesto) && puesto != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Maquinas.SubMaquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                        || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }
            bitacora = bitacora.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3)));
            // var path = Server.MapPath("~/Temp");

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("REPORTE");
            int i = 9;
            bool color = false, inhibit = true;
            string x = "R";
            foreach (Bitacora bita in bitacora)
            {
                if (inhibit)
                {
                    ws.Column("A").Width = 28;
                    ws.Column("B").Width = 28;
                    ws.Column("C").Width = 28;
                    ws.Column("D").Width = 28;
                    ws.Column("E").Width = 28;
                    ws.Column("F").Width = 28;
                    ws.Column("G").Width = 28;
                    ws.Column("H").Width = 40;
                    ws.Column("I").Width = 28;
                    ws.Column("J").Width = 28;
                    ws.Column("K").Width = 28;
                    ws.Column("L").Width = 28;
                    ws.Column("M").Width = 28;
                    ws.Column("N").Width = 28;
                    ws.Column("O").Width = 28;
                    ws.Column("P").Width = 28;
                    ws.Column("Q").Width = 28;
                    ws.Column("R").Width = 28;
                    ws.Column("S").Width = 28;

                    ws.Range("A1:R1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("A1:R1").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    ws.Range("A1:R1").Style.Border.SetOutsideBorderColor(XLColor.Black);


                    ws.Ranges("A3:B6").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                    ws.Cell("A1").Style.Font.Bold = true;
                    ws.Range("A1:D1").Row(1).Merge();

                    ws.Cell("A1").Value = "Bitacora de AutoMantto";

                    ws.Cell("A3").Value = "Fecha Inicial:";
                    ws.Cell("B3").SetDataType(XLDataType.DateTime);
                    //  ws.Cell("B3").Value = bitacora.FirstOrDefault().DiaHora;

                    ws.Cell("A4").Value = "Fecha Final:";
                    ws.Cell("B4").SetDataType(XLDataType.DateTime);
                    //  ws.Cell("B4").Value = bitacora.LastOrDefault().DiaHora;

                    ws.Cell(8, 1).Value = "Usuario";
                    ws.Cell(8, 2).Value = "Dia y Hora";
                    ws.Cell(8, 3).Value = "Cliente";
                    ws.Cell(8, 4).Value = "Area";
                    ws.Cell(8, 5).Value = "Maquina Principal";
                    ws.Cell(8, 6).Value = "Sub Maquina";
                    ws.Cell(8, 7).Value = "Sintoma";
                    ws.Cell(8, 8).Value = "Causa";
                    ws.Cell(8, 9).Value = "Accion Correctiva";
                    ws.Cell(8, 10).Value = "Atendio";
                    ws.Cell(8, 11).Value = "Tiempo Muerto (min)";
                    ws.Cell(8, 12).Value = "Falla de produccion";
                    ws.Cell(8, 13).Value = "Scrap";
                    ws.Cell(8, 14).Value = "Folio del formato";
                    ws.Cell(8, 15).Value = "% de afectaccion";
                    ws.Cell(8, 16).Value = "MTBF";
                    ws.Cell(8, 17).Value = "MTTR";
                    ws.Cell(8, 18).Value = "Verificado Por";
                    ws.Cell(8, 19).Value = "Fecha de vrificacion";


                    ws.Ranges($"A8:{x}8").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                    ws.Cells($"A8:{x}8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    ws.Cells($"A8:{x}8").Style.Border.SetOutsideBorderColor(XLColor.Black);
                    inhibit = false;
                }

                if (!string.IsNullOrEmpty(bita.usuario))
                {
                    if (color)
                    {
                        ws.Range($"A{i}:{x}{i}").Style.Fill.SetBackgroundColor(XLColor.LightGray);
                        color = false;
                    }
                    else color = true;

                    ws.Cell(i, 1).Value = bita.usuario;
                    ws.Cell(i, 2).Value = bita.DiaHora;
                    ws.Cell(i, 3).Value = bita.Maquinas.Cliente;
                    ws.Cell(i, 4).Value = bita.Maquinas.Area;
                    ws.Cell(i, 5).Value = bita.Maquinas.Maquina;
                    ws.Cell(i, 6).Value = bita.Maquinas.SubMaquina;
                    ws.Cell(i, 7).Value = bita.Sintoma;
                    ws.Cell(i, 8).Value = bita.Causa;
                    ws.Cell(i, 9).Value = bita.AccionCorrectiva;
                    ws.Cell(i, 10).Value = bita.Atendio;
                    ws.Cell(i, 11).Value = bita.Tiempo;
                    ws.Cell(i, 12).Value = bita.Fallaoperacion;
                    ws.Cell(i, 13).Value = bita.Scrap;
                    ws.Cell(i, 14).Value = bita.Folio;
                    ws.Cell(i, 15).Value = bita.Porcentaje;
                    ws.Cell(i, 16).Value = bita.MTBF;
                    ws.Cell(i, 17).Value = bita.MTTR;
                    ws.Cell(i, 18).Value = bita.Verifico;
                    ws.Cell(i, 19).Value = bita.FechaVerificacion;

                    ws.Cells($"A{i}:{x}{i}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    ws.Cells($"A{i}:{x}{i}").Style.Border.SetOutsideBorderColor(XLColor.Black);

                    i++;
                }


            }


            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Bitacora.xlsx");
            }


        }

        public FileResult Exportsuper(string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {
            var user = User.Identity;

            //******************************
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff + new TimeSpan(23, 0, 0);
            var ff3 = ff.AddDays(1) + new TimeSpan(8, 0, 0);
            var f1i = 0;
            var f1f = 0;
            var f2i = 0;
            var f2f = 0;
            var f3i = 25;
            var f3f = -1;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
            }



            //************************


            var bitacora = from s in db.Bitacoras.Include(b => b.Maquinas)
                           select s;

            if (nt)
                bitacora = bitacora.Where(s => s.noterminado == true);
            if (fs)
                bitacora = bitacora.Where(s => s.findesemana == true);

            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                if (area.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.usuario_area == "Cromo" || s.usuario_area == "Pintura" || s.usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.usuario_area.Contains(area));
            }
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }
                if (amaquina.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
if (amaquina.Contains("Cromo"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
if (amaquina.Contains("Pintura"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacora = bitacora.Where(s => s.Maquinas.Area.Contains(amaquina));
            }
            if (!String.IsNullOrEmpty(puesto) && puesto != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Maquinas.SubMaquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                        || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }
            bitacora = bitacora.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3)));
            // var path = Server.MapPath("~/Temp");

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Actividades");
            int i = 3;
            bool inhibit = true;
            string x = "H";
            foreach (Bitacora bita in bitacora)
            {
                if (inhibit)
                {
                    string turno = "";
                    if (t1)
                        turno = "1er Turno";
                    if (t2)
                        turno = "2do Turno";
                    if (t3)
                        turno = "3er Turno";

                    ws.Column("A").Width = 10;
                    ws.Column("B").Width = 20;
                    ws.Column("C").Width = 20;
                    ws.Column("D").Width = 50;
                    ws.Column("E").Width = 100;
                    ws.Column("F").Width = 25;
                    ws.Column("G").Width = 10;
                    ws.Column("H").Width = 10;
                    ws.Ranges("A1:H2").Style.Fill.SetBackgroundColor(XLColor.FromArgb(31, 73, 125));
                    ws.Ranges("A1:H2").Style.Font.SetFontColor(XLColor.White);
                    ws.Ranges("A1:H2").Style.Font.SetBold(true);
                    ws.Range("A1:H1").Merge();
                    ws.Cell("A1").Value = "Reporte de Actividades, Fecha: " + fecha_inicial;

                    ws.Cell(2, 1).Value = "Turno";
                    ws.Cell(2, 2).Value = "Responsable";
                    ws.Cell(2, 3).Value = "Equipo";
                    ws.Cell(2, 4).Value = "Descripción de la actividad y/o problema";
                    ws.Cell(2, 5).Value = "Acciones correctivas";
                    ws.Cell(2, 6).Value = "Atendio";
                    ws.Cell(2, 7).Value = "T. M. (min)";
                    ws.Cell(2, 8).Value = "Folio";

                    ws.Cells($"A1:{x}2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    ws.Cells($"A1:{x}2").Style.Border.SetOutsideBorderColor(XLColor.Black);

                    ws.Cell("A3").Value = turno;
                    ws.Cell("B3").Value = user.Name;

                    inhibit = false;
                }

                if (!string.IsNullOrEmpty(bita.usuario))
                {


                    ws.Cell(i, 3).Value = bita.Maquinas.SubMaquina;
                    ws.Cell(i, 4).Value = bita.Sintoma + " - " + bita.Causa;
                    ws.Cell(i, 5).Value = bita.AccionCorrectiva;
                    ws.Cell(i, 6).Value = bita.Atendio;

                    if (bita.Tiempo < 2)
                        ws.Cell(i, 7).Value = "N/A";
                    else
                        ws.Cell(i, 7).Value = bita.Tiempo.ToString();

                    ws.Cell(i, 8).Value = bita.Folio;
                    ws.Cells($"A{i}:{x}{i}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    ws.Cells($"A{i}:{x}{i}").Style.Border.SetOutsideBorderColor(XLColor.Black);
                    i++;
                }
                ws.Range($"A3:A{i - 1}").Merge();
                ws.Range($"B3:B{i - 1}").Merge();
                ws.Range($"C3:C{i - 1}").Style.Alignment.WrapText = true;
                ws.Range($"D3:D{i - 1}").Style.Alignment.WrapText = true;
                ws.Range($"E3:E{i - 1}").Style.Alignment.WrapText = true;
                ws.Range($"A1:{x}{i - 1}").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range($"A1:{x}{i - 1}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }


            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Actividades_" + fecha_inicial + ".xlsx");
            }


        }
        // GET: Bitacoras
        [AllowAnonymous]
        public async Task<ActionResult> Index(string smarea, string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, string btn = "", bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {
            if (btn.Contains("Top 5"))
            {
                return RedirectToAction("Show", new { smarea= smarea,searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }
            if (btn.Contains("Resumen"))
            {
                return RedirectToAction("Repo", new { smarea = smarea, searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }
            if (btn.Contains("Metricos"))
            {
                return RedirectToAction("Metricos", new { smarea = smarea, searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }
            if (btn.Contains("Exportar Excel"))
            {
                return RedirectToAction("Export", new { smarea = smarea, searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }
            if (btn.Contains("Exportar Formato"))
            {
                return RedirectToAction("Exportsuper", new { smarea = smarea, searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }
            if (btn.Contains("RoadMap"))
            {
                return RedirectToAction("roadmap", new { smarea = smarea, searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
            }

            var bitacoras = db.Bitacoras.Include(b => b.Maquinas);

            if(nt)
                bitacoras = bitacoras.Where(s => s.noterminado==true);
            if (fs)
                bitacoras = bitacoras.Where(s => s.findesemana == true);

            if (string.IsNullOrEmpty(fecha_inicial))
            {
                fecha_final = DateTime.Now.ToShortDateString();
                if (DateTime.Now.Hour < 15)
                    fecha_inicial = DateTime.Now.AddDays(-1).ToShortDateString();
                else
                    fecha_inicial = DateTime.Now.ToShortDateString();
                t1 = true;
                t2 = true;
                t3 = true;
            }
            ViewBag.user = "x";
            var user = User.Identity;
            if (!string.IsNullOrEmpty(user.Name))
                ViewBag.user = user.Name;
            //******************************
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff + new TimeSpan(23, 0, 0);
            var ff3 = ff.AddDays(1) + new TimeSpan(8, 0, 0);
            var f1i = 0;
            var f1f = 0;
            var f2i = 0;
            var f2f = 0;
            var f3i = 25;
            var f3f = -1;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
            }



            //************************

            ViewBag.t1 = t1;
            ViewBag.t2 = t2;
            ViewBag.t3 = t3;
            ViewBag.fecha_inicial = fi.ToShortDateString();
            ViewBag.fecha_final = ff.ToShortDateString();
            ViewBag.area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            ViewBag.puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());
            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            var maquinas = db.Maquinas.Where(m => m.ID > 0);
            if (!string.IsNullOrEmpty(smarea))
                maquinas = db.Maquinas.Where(m => m.Area == smarea);
  
            ViewBag.maquina = new SelectList(maquinas, "ID", "SubMaquina");

            ViewBag.area2 = area;
            ViewBag.puesto2 = puesto;
            ViewBag.amaquina2 = amaquina;
            ViewBag.maquina2 = maquina;
             ViewBag.searchString2 = searchString;
            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                if (area.Contains("MetalFinish"))
                {
                    bitacoras = bitacoras.Where(s => s.usuario_area == "Cromo" || s.usuario_area == "Pintura" || s.usuario_area == "MetalFinish");
                }
                else
                    bitacoras = bitacoras.Where(s => s.usuario_area.Contains(area));
            }
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }
                else
                if (amaquina.Contains("MetalFinish"))
                {
                    bitacoras = bitacoras.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
                 if (amaquina=="Cromo")
                {
                    bitacoras = bitacoras.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
                 if (amaquina.Contains("Pintura"))
                {
                    bitacoras = bitacoras.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacoras = bitacoras.Where(s => s.Maquinas.Area.Contains(amaquina));
            }
            if (!String.IsNullOrEmpty(puesto) && puesto != "--Todas--")
            {
                bitacoras = bitacoras.Where(s => s.usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacoras = bitacoras.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacoras = bitacoras.Where(s => s.usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Maquinas.SubMaquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                        || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }

            bitacoras = bitacoras.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) &&( s.DiaHora >= fi && s.DiaHora <= ff3)));



            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id); 
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;

           
            return View(await bitacoras.ToListAsync());
       
        }

        [Authorize]
        public string Verify(int? id)
        {
            if (id == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return "Error de ID";
             }
            Bitacora bitacora =  db.Bitacoras.Find(id);
            if (bitacora == null)
            {
                // return HttpNotFound();
                return "Error de ID";
            }
            var uid = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(uid); 

            bitacora.findesemana = false;
            bitacora.Verifico = currentUser.UserFullName;
            bitacora.FechaVerificacion = DateTime.Now;
            db.Entry(bitacora).State = EntityState.Modified;
            db.SaveChanges();
            return "Actividad Verificada";
            // return new EmptyResult();
            // return RedirectToAction("Index",new { searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3 ,nt=nt,fs=fs});
        }

        [Authorize]
        public async Task<ActionResult> Finish(int? id, string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = await db.Bitacoras.FindAsync(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }          
            bitacora.noterminado = false;
            //bitacora.findesemana = false;
            db.Entry(bitacora).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Create");
           // return RedirectToAction("Index", new { searchString = searchString, area = area, amaquina = amaquina, puesto = puesto, maquina = maquina, fecha_inicial = fecha_inicial, fecha_final = fecha_final, t1 = t1, t2 = t2, t3 = t3, nt = nt, fs = fs });
        }
        public ActionResult Show(string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {

            var bitacora = from s in db.Bitacoras
                           select s;
            if (nt)
                bitacora = bitacora.Where(s => s.noterminado == true);
            if (fs)
                bitacora = bitacora.Where(s => s.findesemana == true);
            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                if (area.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.usuario_area == "Cromo" || s.usuario_area == "Pintura" || s.usuario_area == "MetalFinish");
                }
                else
                bitacora = bitacora.Where(s => s.usuario_area.Contains(area));
            }
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }
                if (amaquina.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
if (amaquina.Contains("Cromo"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
if (amaquina.Contains("Pintura"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacora = bitacora.Where(s => s.Maquinas.Area.Contains(amaquina));
            }
            if (!String.IsNullOrEmpty(puesto) && puesto != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                        || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }
            if (string.IsNullOrEmpty(fecha_inicial))
            {
                fecha_final = DateTime.Now.ToShortDateString();
                if (DateTime.Now.Hour < 15)
                    fecha_inicial = DateTime.Now.AddDays(-1).ToShortDateString();
                else
                    fecha_inicial = DateTime.Now.ToShortDateString();
                t1 = true;
                t2 = true;
                t3 = true;
            }

            //******************************
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff + new TimeSpan(23, 0, 0);
            var ff3 = ff.AddDays(1) + new TimeSpan(8, 0, 0);
            var f1i = 0;
            var f1f = 0;
            var f2i = 0;
            var f2f = 0;
            var f3i = 25;
            var f3f = -1;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
            }



            //************************
            bitacora = bitacora.Where(x => x.Tiempo >= 1);
            var query2 = bitacora.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3))).GroupBy(p => p.Maquinas.Maquina)
                        .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });

            var query = query2.ToList();
            if (query.Count() >= 1)
            {
                query = query.OrderByDescending(s => s.count).ToList();
                List<string> ObjectName = new List<string>();
                List<int> Data = new List<int>();
                foreach (var item in query)
                {
                    ObjectName.Add(item.Maquina);
                    Data.Add(item.count);

                }
                string objet;
                string dat;
                if (Data.Count() >= 1)
                {
                    objet = "'";
                    dat = "";
                    var i = 0;
                    foreach (var item in ObjectName)
                    {
                        objet = objet + item.ToString() + "','";
                        i++;
                        if (i == 5)
                            break;
                    }
                    i = 0;
                    foreach (var item in Data)
                    {
                        dat = dat + item.ToString() + ",";
                        i++;
                        if (i == 5)
                            break;
                    }
                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.ObjectName = objet;
                    ViewBag.Data = dat;

                    query = query.OrderByDescending(s => s.time).ToList();
                    List<string> ObjectName2 = new List<string>();
                    List<int> Data2 = new List<int>();
                    foreach (var item in query)
                    {


                        ObjectName2.Add(item.Maquina);
                        Data2.Add(item.time);
                    }

                    objet = "'";
                    dat = "";
                    i = 0;
                    foreach (var item in ObjectName2)
                    {
                        objet = objet + item.ToString() + "','";
                        i++;
                        if(i==5)
                            break;
                    }
                    i = 0;
                    foreach (var item in Data2)
                    {
                        dat = dat + item.ToString() + ",";
                        i++;
                        if (i == 5)
                            break;
                    }
                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.ObjectName2 = objet;
                    ViewBag.Data2 = dat;
                    

                    var bitacora2 = from s in db.Bitacoras
                                    select s;

                    bitacora2 = bitacora2.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3)));
                    if (nt)
                        bitacora2 = bitacora2.Where(s => s.noterminado == true);
                    if (fs)
                        bitacora2 = bitacora2.Where(s => s.findesemana == true);
                    if (!string.IsNullOrEmpty(area))                  
                        bitacora2 = bitacora2.Where(s => s.usuario_area.Contains(area)); 

                        string xx = ObjectName2.ElementAt(0).ToString();
                    ViewBag.Machinetop1name = xx;                  
                    List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machinetop1 = Machinetop1;
                    if (query.Count() > 1) { 
                        xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machinetop2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machinetop2 = Machinetop2;
                    }
                    if (query.Count() > 2)
                    {
                        xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machinetop3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.DiaHora >= fi && p.DiaHora <= ff && p.Maquinas.Maquina == xx ).ToList();
                    ViewBag.Machinetop3 = Machinetop3;
                    }
                    if (query.Count() > 3)
                    {
                        xx = ObjectName2.ElementAt(3).ToString();
                    ViewBag.Machinetop4name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop4 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machinetop4 = Machinetop4;
                    }
                    if (query.Count() > 4)
                    {
                        xx = ObjectName2.ElementAt(4).ToString();
                    ViewBag.Machinetop5name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop5 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machinetop5 = Machinetop5;
                    }



                }
                ViewBag.fecha_inicial = fi.ToShortDateString();
                ViewBag.fecha_final = ff.ToShortDateString();


                return View();
            }
                            else
                            {
                               ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                                TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                               return RedirectToAction("Index");

                            }

                        }

        public ActionResult Repo (string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {

            ViewBag.Junta = db.Juntas.OrderByDescending(p => p.ID).FirstOrDefault();
           
            var bitacora = from s in db.Bitacoras
                           select s;
            if (string.IsNullOrEmpty(fecha_inicial))
            {
                fecha_final = DateTime.Now.ToShortDateString();
                if (DateTime.Now.Hour < 15)
                    fecha_inicial = DateTime.Now.AddDays(-1).ToShortDateString();
                else
                    fecha_inicial = DateTime.Now.ToShortDateString();
                t1 = true;
                t2 = true;
                t3 = true;
            }
            //******************************
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff + new TimeSpan(23, 0, 0);
            var ff3 = ff.AddDays(1) + new TimeSpan(8, 0, 0);
            var f1i = 0;
            var f1f = 0;
            var f2i = 0;
            var f2f = 0;
            var f3i = 25;
            var f3f = -1;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
            }



            //************************


            bitacora = bitacora.Where(s => (((s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || (((s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff)))
                                        || ((s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3)));

            if (nt)
                bitacora = bitacora.Where(s => s.noterminado == true);
            if (fs)
                bitacora = bitacora.Where(s => s.findesemana == true);
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }
                if (amaquina.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
if (amaquina.Contains("Cromo"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
if (amaquina.Contains("Pintura"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacora = bitacora.Where(s => s.Maquinas.Area.Contains(amaquina));
            }

            var bitacorase = bitacora.Where(s => s.Maquinas.Area.Contains("Servicios"));
            var bitacoraes = bitacora.Where(s => s.Maquinas.Area.Contains("Estampado"));
            var bitacoramf = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.usuario_area == "Pintura");
            var bitacoraps = bitacora.Where(s => s.usuario_area.Contains("Mtto_Soldadura")||(s.usuario_area.Contains("Automatizacion")&& s.Maquinas.Area.Contains("Soldadura")));
            var bitacorams = bitacora.Where(s => s.usuario_area.Contains("Proc_Soldadura"));
            var bitacoraen = bitacora.Where(s => s.Maquinas.Area.Contains("Ensamble"));
            var bitacoraau = bitacora;
            bitacorase = bitacorase.Where(d => d.Tiempo > 0);
            bitacoraes = bitacoraes.Where(d => d.Tiempo > 0);
            bitacoramf = bitacoramf.Where(d => d.Tiempo > 0);
            bitacoraps = bitacoraps.Where(d => d.Tiempo > 0);
            bitacorams = bitacorams.Where(d => d.Tiempo > 0);
            bitacoraen = bitacoraen.Where(d => d.Tiempo > 0);
            bitacoraau = bitacoraau.Where(d => d.Tiempo > 0);


            var query2 = bitacorase.GroupBy(p => p.Maquinas.Maquina)
                        .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            var query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
             
                    query = query.OrderByDescending(s => s.time).ToList();
                    List<string> ObjectName2 = new List<string>();
                    List<int> Data2 = new List<int>();
                    foreach (var item in query)
                    {


                        ObjectName2.Add(item.Maquina);
                        Data2.Add(item.time);
                    }

                    objet = "'";
                    dat = "";
                    i = 0;
                    foreach (var item in ObjectName2)
                    {
                        objet = objet + item.ToString() + "','";
                        i++;
                        if (i == 3)
                            break;
                    }
                    i = 0;
                    foreach (var item in Data2)
                    {
                        dat = dat + item.ToString() + ",";
                        i++;
                        if (i == 3)
                            break;
                    }
                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.ObjectName1 = objet;
                    ViewBag.Data1 = dat;


                var bitacora2 = bitacorase;

                    string xx = ObjectName2.ElementAt(0).ToString();
                    ViewBag.Machine1top1name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine1top1 = Machinetop1;
                    if (query.Count() > 1)
                    {
                        xx = ObjectName2.ElementAt(1).ToString();
                        ViewBag.Machine1top2name = xx;
                        List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                        ViewBag.Machine1top2 = Machinetop2;
                    }
                    if (query.Count() > 2)
                    {
                        xx = ObjectName2.ElementAt(2).ToString();
                        ViewBag.Machine1top3name = xx;
                        List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                        ViewBag.Machine1top3 = Machinetop3;
                    }
            }
            else
            {
                ViewBag.Machine1top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine1top1 = null;

              //  ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
               // TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //  return RedirectToAction("Index");

            }


             query2 = bitacoraes.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
             query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName2 = objet;
                ViewBag.Data2 = dat;

                var bitacora2 = bitacoraes;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine2top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine2top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine2top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine2top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine2top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine2top3 = Machinetop3;
                }

            }
            else
            {

                ViewBag.Machine2top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine2top1 = null;

               // ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
               // TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }

            query2 = bitacoraps.GroupBy(p => p.Maquinas.Maquina)
.Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName3 = objet;
                ViewBag.Data3 = dat;


                var bitacora2 = bitacoraps;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine3top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine3top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine3top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine3top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine3top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine3top3 = Machinetop3;
                }
            }
            else
            {
                ViewBag.Machine3top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine3top1 = null;
              //  ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //  TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }

            query2 = bitacorams.GroupBy(p => p.Maquinas.Maquina)
.Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName4 = objet;
                ViewBag.Data4 = dat;


                var bitacora2 = bitacorams;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine4top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine4top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine4top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine4top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine4top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine4top3 = Machinetop3;
                }
            }
            else
            {

                ViewBag.Machine4top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine4top1 = null;

              //  ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //  TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }

            query2 = bitacoramf.GroupBy(p => p.Maquinas.Maquina)
.Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName5 = objet;
                ViewBag.Data5 = dat;


                var bitacora2 = bitacoramf;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine5top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine5top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine5top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine5top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine5top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine5top3 = Machinetop3;
                }

            }
            else
            {

                ViewBag.Machine5top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine5top1 = null;
             //   ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //  TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }

            query2 = bitacoraen.GroupBy(p => p.Maquinas.Maquina)
.Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName6 = objet;
                ViewBag.Data6 = dat;


                var bitacora2 = bitacoraen;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine6top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine6top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine6top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine6top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine6top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine6top3 = Machinetop3;
                }

            }
            else
            {

                ViewBag.Machine6top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine6top1 = null;
            //    ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
            //    TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }

            query2 = bitacoraau.GroupBy(p => p.Maquinas.Maquina)
.Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;

                query = query.OrderByDescending(s => s.time).ToList();
                List<string> ObjectName2 = new List<string>();
                List<int> Data2 = new List<int>();
                foreach (var item in query)
                {


                    ObjectName2.Add(item.Maquina);
                    Data2.Add(item.time);
                }

                objet = "'";
                dat = "";
                i = 0;
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                    i++;
                    if (i == 3)
                        break;
                }
                i = 0;
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName7 = objet;
                ViewBag.Data7 = dat;


                var bitacora2 = bitacoraau;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine7top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine7top1 = Machinetop1;
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine7top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine7top2 = Machinetop2;
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine7top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine7top3 = Machinetop3;
                }

            }
            else
            {

                ViewBag.Machine7top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine7top1 = null;
             //   ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             //   TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                //  return RedirectToAction("Index");

            }


            return View();
        }

        public ActionResult roadmap(string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false, string btn = "mes", string dti = "", string dtf = "")
        {

            var data = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var fecha = DateTime.Now.AddDays(-7);
            var fechaf = DateTime.Now;

            if (!string.IsNullOrEmpty(dti))
            {
                fecha = Convert.ToDateTime(dti);
            }
            if (!string.IsNullOrEmpty(dtf))
            {
                fechaf = Convert.ToDateTime(dtf);
            }
            //---data---

            data = db.Bitacoras.Where(w => (w.DiaHora.Year >= fecha.Year) &&
                                      (w.DiaHora.Year <= fechaf.Year) &&
                                      (w.Tiempo > 0)
                                     ).ToList();

            //*******************************************************************************************
            if (btn == "Por Años")
            {

                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                datafiltered = data;
                var datatempa = data.GroupBy(g => g.DiaHora.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();

                for (int i = fecha.Year; i <= fechaf.Year; i++)
                {
                    var dd = datatempa.Where(w => w.Key == i).ToList();
                    labels = labels + i.ToString() + "','";
                    if(dd.Count!=0)
                    gdata = gdata + dd.FirstOrDefault().Sum(s => s.Tiempo).ToString() + ",";
                    else
                    gdata = gdata + "0,";

                    x.Add(i);
                    if (dd.Count != 0)
                        y.Add(dd.FirstOrDefault().Count());
                    else
                        y.Add(0);


                 

                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    for (int i = fecha.Year; i <= fechaf.Year; i++)
                    {
                        var tt = fecha.Year;
                        var res = rvalue.GetYValue(tt);
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                int stempt = datafiltered.Sum(s => s.Tiempo);
                int stemp = 0;
                foreach (var h in groupdata)
                {
                    labels = labels + h.Key + "','";
                    gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                    stemp = stemp + h.Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(fecha.Month);
                ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                ViewBag.dxx = " Anual";
            }
            //*******************************************************************************************
            if (btn == "Por Mes")
            {
                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                var datatempa = data.GroupBy(g => g.DiaHora.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        if (
                             ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                             ||
                              ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                            ||
                              ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                            ||
                            (iaño != fecha.Year && iaño != fechaf.Year)
                            )
                        {
                            labels = labels + iaño.ToString() + "-" + nombreMes + "','";
                            var temp = dataaño.Where(w => w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);
                            gdata = gdata + temp.Sum(s => s.Tiempo).ToString() + ",";
                            x.Add(i);
                            y.Add(temp.Sum(s => s.Tiempo));
                            i++;
                        }


                    }

                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;
                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            if (
                                  ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                                 ||
                                  ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                                ||
                                  ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                                ||
                                (iaño != fecha.Year && iaño != fechaf.Year)
                                )
                            {
                                var res = rvalue.GetYValue(i);
                                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                i++;
                            }


                        }

                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                int stempt = datafiltered.Sum(s => s.Tiempo);
                int stemp = 0;
                foreach (var h in groupdata)
                {
                    labels = labels + h.Key + "','";
                    gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                    stemp = stemp + h.Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                string nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************
            if (btn == "Por Dia")
            {
                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);
                        var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                        for (int kdia = 1; kdia <= dias; kdia++)
                        {
                            DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                            if (idi >= fecha && idi <= fechaf)
                            {
                                labels = labels + iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString() + "','";
                                var temp = datames.Where(w => w.DiaHora.Day == kdia);
                                datafiltered.AddRange(temp);
                                gdata = gdata + temp.Sum(s=>s.Tiempo).ToString() + ",";
                                x.Add(i);
                                y.Add(temp.Sum(s => s.Tiempo));
                                i++;
                            }

                        }


                    }

                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;

                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                            string nombreMes = formatoFecha.GetMonthName(jmes);
                            var dias = DateTime.DaysInMonth(iaño, jmes);
                            var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                            for (int kdia = 1; kdia <= dias; kdia++)
                            {
                                DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                                if (idi >= fecha && idi <= fechaf)
                                {
                                    var res = rvalue.GetYValue(i);
                                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                    i++;
                                }

                            }


                        }

                    }
      
                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'";
                    gdata.TrimEnd(',', (char)39);
                    gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)); 
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    int stempt = datafiltered.Sum(s => s.Tiempo);
                    int stemp = 0;
                    foreach (var h in groupdata)
                    {                       
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                        stemp = stemp + h.Sum(s => s.Tiempo);
                        double res = (stemp / (double)stempt) * 100;
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }

                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'";
                    gdata.TrimEnd(',', (char)39);
                    gdata2.TrimEnd(',', (char)39);
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;

                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                    ViewBag.dxx = " Dia";
                }

            }

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;

            return View(datafiltered);
        }
        // GET: Bitacoras/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = await db.Bitacoras.FindAsync(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // GET: Bitacoras/Create


        public ActionResult drop3(string Tipo)
        {
            // TODO: based on the selected country return the cities:
            var fallas = db.Fallas.Where(f => f.Tipo == Tipo);
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")

            List<string> codigo = new List<string>();

            foreach (var item in fallas)
            {
                codigo.Add(item.Descripcion);


            }
            // var cities = fallas.ToList();
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Create()
        {

            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string scliente ="";
            string sarea = currentUser.Area;
            string sarea2 = sarea;
            if (TempData["myarea"] != null)
                sarea2 = TempData["myareaf"].ToString();
            if (sarea.Contains("Soldadura"))
            {
                sarea = "Soldadura";
            }
            if (TempData["mycliente"]!=null)
            scliente = TempData["mycliente"].ToString();
            if (TempData["myarea"] != null)
             sarea = TempData["myarea"].ToString();
            var maquinas = db.Maquinas.Where(m=>m.ID>=0);
            if (!string.IsNullOrEmpty(scliente))
            {
                if (maquinas.Where(m => m.Cliente == scliente).Count() != 0)
                    maquinas = maquinas.Where(m => m.Cliente == scliente);
            }
              
            if (!string.IsNullOrEmpty(sarea))
            {
                if (sarea.Contains("MetalFinish"))
                {
                    if (maquinas.Where(m => m.Area == "Cromo").Count() != 0|| maquinas.Where(m => m.Area == "Pintura").Count() != 0 || maquinas.Where(m => m.Area == "MetalFinish").Count() != 0)
                        maquinas = maquinas.Where(m => m.Area == "Cromo" || m.Area== "Pintura" || m.Area == "MetalFinish");
                }
                else
                if (maquinas.Where(m => m.Area == sarea).Count() != 0)
                    maquinas = maquinas.Where(m => m.Area == sarea);
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");
            ViewBag.MaquinasID = new SelectList(maquinas, "ID", "SubMaquina");


            List<SelectListItem> lst = new List<SelectListItem>();

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 14)
                lst.Add(new SelectListItem() { Text = "1", Value = "1", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "1", Value = "1"});

            if (DateTime.Now.Hour >= 14 && DateTime.Now.Hour < 22)
                lst.Add(new SelectListItem() { Text = "2", Value = "2", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "2", Value = "2"});

            if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 6)
                lst.Add(new SelectListItem() { Text = "3", Value = "3", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "3", Value = "3"});


            ViewBag.turno = lst;
            Bitacora bitacora = new Bitacora();
            bitacora.usuario = currentUser.UserFullName;
            bitacora.DiaHora = DateTime.Now;
            bitacora.Tiempo = 0;
            ViewBag.listcliente = new SelectList(Enum.GetValues(typeof(flex_Cliente)).Cast<flex_Cliente>().ToList());
            ViewBag.listarea = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());

            var fallas = db.Fallas.Where(f => f.Area == sarea2);
            if (fallas.Count() == 0)
                fallas = db.Fallas;
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")
            var gfallast = fallas.GroupBy(g => g.Tipo);

            var model = new MyViewFallas
            {
                Area = sarea2,
                // TODO: Fetch areas from somewhere               
                Areas = new SelectList(fallas, "Area", "Area"),  //new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList()),
                // initially we set the ddl to empty
                Tipos = new SelectList(gfallast, "Key", "Key"),
                // initially we set the ddl to empty
                Dess = Enumerable.Empty<SelectListItem>()
            };

            MyViewBitcora bitax = new MyViewBitcora { Bitacora = bitacora, MyViewFallas = model };

            return View(bitax);
        }

        [Authorize]
        public ActionResult Createa(string Area, string cliente)
        {
            TempData["myareaf"] = Area;
            if (Area.Contains("Soldadura"))
            {
                Area = "Soldadura";
            }
            
            TempData["mycliente"] = cliente;
            TempData["myarea"] = Area;
            return RedirectToAction("Create");
        }

        // POST: Bitacoras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Mantenimiento")]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,turno,UserID,MaquinasID,Sintoma,Causa,AccionCorrectiva,Atendio,Tiempo,Scrap ,Folio,findesemana,Fallaoperacion,MttoPreventivo,MttoCorrectivo,MttoMejora,noterminado,Tipos,Descripcion")] Bitacora bitacora,MyViewFallas MyViewFallas,int MaquinasID)
        {

            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            bitacora.MaquinasID = MaquinasID;
            bitacora.Tipos = MyViewFallas.Area+"|"+ MyViewFallas.Tipo;
            bitacora.Descripcion = MyViewFallas.Tipo + "|" + MyViewFallas.Des;
            bitacora.usuario = currentUser.UserFullName;
            bitacora.usuario_area = currentUser.Area;
            bitacora.usuario_puesto = currentUser.Puesto;
            bitacora.DiaHora = DateTime.Now;

            if (ModelState.IsValid)
            {
       /*
                try
                {

                    var fall = db.Bitacoras.Where(s => s.Maquinas.ID==(bitacora.MaquinasID) && s.Tiempo >= 1);
                    double total_fallas = 0;
                    total_fallas = fall.Count();
                    if (bitacora.Tiempo > 0&&!bitacora.Fallaoperacion)
                        total_fallas = total_fallas + 1;
                    var tiempomue = db.Bitacoras.Where(s => s.Maquinas.ID == (bitacora.MaquinasID) && s.Tiempo >= 1);
                    double tiempomueto = 0;
                    if (total_fallas >= 1)
                        tiempomueto = tiempomue.Sum(s => s.Tiempo);
                    if (total_fallas > 1&&!bitacora.Fallaoperacion)
                        tiempomueto = tiempomueto + bitacora.Tiempo;

                    var maqui = db.Maquinas.Where(s => s.ID == (bitacora.MaquinasID));
                    DateTime x = Convert.ToDateTime(maqui.FirstOrDefault().DiaHora);
                    DateTime y = DateTime.Now;
                    var z = ((y - x));

                    double Tiempo_total_de_funcionamiento = z.TotalMinutes* .75;
                    double MTBF = Tiempo_total_de_funcionamiento;
                    double MTTR = 0;
                   
                    if (total_fallas != 0)
                    {
                        MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                        MTTR = tiempomueto / total_fallas;
                    }

                    double disponibilidad = MTBF / (MTBF + MTTR);

                    bitacora.Porcentaje = Math.Round(100 - (disponibilidad * 100), 2); 

                    bitacora.MTBF = Math.Round(MTBF, 2); 
                    bitacora.MTTR = Math.Round(MTTR, 2);

                }
                catch (Exception ex)
                {

                    bitacora.Porcentaje = 0;

                    bitacora.MTBF = 0;
                    bitacora.MTTR = 0;

                }
                */
                bitacora.Porcentaje = 0;

                bitacora.MTBF = 0;
                bitacora.MTTR = 0;
                if (string.IsNullOrEmpty(bitacora.Atendio))
                    bitacora.Atendio = bitacora.usuario;
                if (string.IsNullOrEmpty(bitacora.Scrap))
                    bitacora.Scrap = "N/A";
                if (string.IsNullOrEmpty(bitacora.Folio))
                    bitacora.Folio = "N/A";
                if (!bitacora.Fallaoperacion&&!bitacora.MttoCorrectivo&&!bitacora.MttoPreventivo&&!bitacora.MttoMejora)
                    bitacora.MttoMejora = true;

                db.Bitacoras.Add(bitacora);
                await db.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina", bitacora.MaquinasID);
            return View(bitacora);
        }

        // GET: Bitacoras/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = await db.Bitacoras.FindAsync(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> lst = new List<SelectListItem>();

            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour < 14)
                lst.Add(new SelectListItem() { Text = "1", Value = "1", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "1", Value = "1" });

            if (DateTime.Now.Hour >= 14 && DateTime.Now.Hour < 22)
                lst.Add(new SelectListItem() { Text = "2", Value = "2", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "2", Value = "2" });

            if (DateTime.Now.Hour >= 22 || DateTime.Now.Hour < 6)
                lst.Add(new SelectListItem() { Text = "3", Value = "3", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "3", Value = "3" });


            ViewBag.turno = lst;
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", bitacora.MaquinasID);
            return View(bitacora);
        }

        // POST: Bitacoras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,turno,usuario,usuario_area,usuario_puesto,MaquinasID,Sintoma,Causa,AccionCorrectiva,Atendio ,Tiempo,Scrap ,Folio,findesemana,Fallaoperacion ,MttoPreventivo,MttoCorrectivo,MttoMejora, Porcentaje,MTBF,MTTR,Verifico,FechaVerificacion,noterminado,Tipos,Descripcion")] Bitacora bitacora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bitacora).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", bitacora.MaquinasID);
            return View(bitacora);
        }

        // GET: Bitacoras/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = await db.Bitacoras.FindAsync(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // POST: Bitacoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Bitacora bitacora = await db.Bitacoras.FindAsync(id);
            db.Bitacoras.Remove(bitacora);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public ActionResult Metricos(string searchString, string area, string puesto, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false, bool nt = false, bool fs = false)
        {

            string stemp = "";

            var bitacora = from s in db.Bitacoras
                           select s;
            newmetricos marea = new newmetricos
            {
                Maquina = "1 Total-" + amaquina,
                count = 0,
                time = 0

            };



            /*
            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                if (area.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.usuario_area == "Cromo" || s.usuario_area == "Pintura" || s.usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.usuario_area.Contains(area));
            }
            */
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }
                if (amaquina.Contains("MetalFinish"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
if (amaquina.Contains("Cromo"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
if (amaquina.Contains("Pintura"))
                {
                    bitacora = bitacora.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    bitacora = bitacora.Where(s => s.Maquinas.Area.Contains(amaquina));
            }
            /*
            if (!String.IsNullOrEmpty(puesto) && puesto != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }

            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                        || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }
            */


            if (string.IsNullOrEmpty(fecha_inicial))
            {
                fecha_final = DateTime.Now.ToShortDateString();
                if (DateTime.Now.Hour < 15)
                    fecha_inicial = DateTime.Now.AddDays(-1).ToShortDateString();
                else
                    fecha_inicial = DateTime.Now.ToShortDateString();
                t1 = true;
                t2 = true;
                t3 = true;
            }
            
            var fi = Convert.ToDateTime(fecha_inicial);
            fi = fi + new TimeSpan(7, 0, 0);
            var ff = Convert.ToDateTime(fecha_final);
            ff = ff.AddDays(1) + new TimeSpan(7, 0, 0);
            var ff3 = ff;
            var f1i =0;
            var f1f = 0;
            var f2i =0;
            var f2f = 0;
            var f3i = 0;
            var f3f = 0;
            string turnos = "";
            if (t1)
            {
                f1i = 6;
                f1f = 14;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 14;
                f2f = 22;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 22;
                f3f = 6;
                ff3 = ff;
                turnos = turnos + "| 3er ";
            }


            bitacora = bitacora.Where(s => s.Tiempo > 0 && (s.DiaHora >= fi && s.DiaHora <= ff3));

            var bitacora1 = bitacora.Where(s => (s.DiaHora.Hour > f1i && s.DiaHora.Hour <= f1f)&&(s.DiaHora >= fi && s.DiaHora <= ff));

            var bitacora2 = bitacora.Where(s =>(s.DiaHora.Hour > f2i && s.DiaHora.Hour <= f2f) && (s.DiaHora >= fi && s.DiaHora <= ff));

            var bitacora3 = bitacora.Where(s => (s.DiaHora.Hour > f3i || s.DiaHora.Hour <= f3f) && (s.DiaHora >= fi && s.DiaHora <= ff3));

            var quef = bitacora.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new newmetricos
{
   Maquina = g.Key,
   count = g.Count(),
    time = g.Sum(xx1 => xx1.Tiempo),
    count1 = 0,
    time1 = 0,
    count2 = 0,
    time2 = 0,
    count3 = 0,
    time3 = 0,
    Minmtbf = g.Min(zx1 => zx1.MTBF),
    Maxmttr = g.Max(zz2 => zz2.MTTR),
    NewMinmtbf1 = 0.0,
    NewMaxmttr1 = 0.0,
    NewMinmtbf2 = 0.0,
    NewMaxmttr2 = 0.0,
    NewMinmtbf3 = 0.0,
    NewMaxmttr3 = 0.0

}).ToList();


            var que1 = bitacora1.GroupBy(p => p.Maquinas.Maquina)
           .Select(g => new newmetricos
           {
               Maquina = g.Key,
               count = g.Count(),
               time = g.Sum(x1 => x1.Tiempo),
               count1 =0,
               time1 = 0,
               count2 = 0,
               time2 = 0,
               count3 = 0,
               time3 = 0,
               Minmtbf = g.Min(z1 => z1.MTBF),
               Maxmttr = g.Max(z2 => z2.MTTR),
               NewMinmtbf1 = 0.0,
               NewMaxmttr1 = 0.0,
               NewMinmtbf2 = 0.0,
               NewMaxmttr2 = 0.0,
               NewMinmtbf3 = 0.0,
               NewMaxmttr3 = 0.0
           }).ToList();

            var que2 = bitacora2.GroupBy(p => p.Maquinas.Maquina)
           .Select(g => new newmetricos
           {
               Maquina = g.Key,
               count = g.Count(),
               time = g.Sum(x2 => x2.Tiempo),
               count1 = 0,
               time1 = 0,
               count2 = 0,
               time2 = 0,
               count3 = 0,
               time3 = 0,
               Minmtbf = g.Min(z2 => z2.MTBF),
               Maxmttr = g.Max(z3 => z3.MTTR),
                NewMinmtbf1 = 0.0,
               NewMaxmttr1 = 0.0,
               NewMinmtbf2 = 0.0,
               NewMaxmttr2 = 0.0,
               NewMinmtbf3 = 0.0,
               NewMaxmttr3 = 0.0
           }).ToList();

            var que3 = bitacora3.GroupBy(p => p.Maquinas.Maquina)
           .Select(g => new newmetricos
           {
               Maquina = g.Key,
               count = g.Count(),
               time = g.Sum(x3 => x3.Tiempo),
               count1 = 0,
               time1 = 0,
               count2 = 0,
               time2 = 0,
               count3 = 0,
               time3 = 0,
               Minmtbf = g.Min(z3 => z3.MTBF),
               Maxmttr = g.Max(z4 => z4.MTTR),
               NewMinmtbf1 = 0.0,
               NewMaxmttr1 = 0.0,
               NewMinmtbf2 = 0.0,
               NewMaxmttr2 = 0.0,
               NewMinmtbf3 = 0.0,
               NewMaxmttr3 = 0.0
           }).ToList();


            List<newmetricos> allmaquinas = new List<newmetricos>();
            allmaquinas.Add(marea);
            DateTime x = fi;
            DateTime y = ff;
            var z = ((y - x));
            double Tiempo_total_de_funcionamiento = z.TotalMinutes * .72;
            double Tiempo_total_de_funcionamiento_p = 0.0;


            foreach (var allmaqui in quef.Where(w => w.time > 0))
            {
                newmetricos tempmaqui = new newmetricos
                {
                Maquina = allmaqui.Maquina,
                count = allmaqui.count,
               time = allmaqui.time,
               count1 = 0,
               time1 = 0,
               count2 = 0,
               time2 = 0,
               count3 = 0,
               time3 = 0,
               Minmtbf =0.0,
               Maxmttr =0.0,
               NewMinmtbf1 = 0.0,
               NewMaxmttr1 = 0.0,
               NewMinmtbf2 = 0.0,
               NewMaxmttr2 = 0.0,
               NewMinmtbf3 = 0.0,
               NewMaxmttr3 = 0.0

                };

                foreach (var maqui1 in que1)
                {
                    if (tempmaqui.Maquina == maqui1.Maquina)
                    {
                        tempmaqui.count1 = maqui1.count;
                        tempmaqui.time1 = maqui1.time;

                        tempmaqui.NewMinmtbf1 = Tiempo_total_de_funcionamiento;

                        if (tempmaqui.count1 != 0)
                        {
                            tempmaqui.NewMinmtbf1 = Tiempo_total_de_funcionamiento / tempmaqui.count1;
                            tempmaqui.NewMaxmttr1 = tempmaqui.time1 / tempmaqui.count1;
                        }


                    }

                }

                foreach (var maqui12 in que2)
                {
                    if (tempmaqui.Maquina == maqui12.Maquina)
                    {
                        tempmaqui.count2 = maqui12.count;
                        tempmaqui.time2 = maqui12.time;

                        tempmaqui.NewMinmtbf2 = Tiempo_total_de_funcionamiento;

                        if (tempmaqui.count2 != 0)
                        {
                            tempmaqui.NewMinmtbf2 = Tiempo_total_de_funcionamiento / tempmaqui.count2;
                            tempmaqui.NewMaxmttr2 = tempmaqui.time2 / tempmaqui.count2;
                        }


                    }

                }

                foreach (var maqui3 in que3)
                {
                    if (tempmaqui.Maquina == maqui3.Maquina)
                    {
                        tempmaqui.count3 = maqui3.count;
                        tempmaqui.time3 = maqui3.time;

                        tempmaqui.NewMinmtbf3 = Tiempo_total_de_funcionamiento;

                        if (tempmaqui.count3 != 0)
                        {
                            tempmaqui.NewMinmtbf3 = Tiempo_total_de_funcionamiento / tempmaqui.count3;
                            tempmaqui.NewMaxmttr3 = tempmaqui.time3 / tempmaqui.count3;
                        }


                    }

                }

                tempmaqui.Minmtbf = Tiempo_total_de_funcionamiento;
               // tempmaqui.count = tempmaqui.count1 + tempmaqui.count2 + tempmaqui.count3;
               // tempmaqui.time = tempmaqui.time1 + tempmaqui.time2 + tempmaqui.time3;
                if (tempmaqui.count != 0)
                {
                    tempmaqui.Minmtbf = Tiempo_total_de_funcionamiento / tempmaqui.count;
                    tempmaqui.Maxmttr = tempmaqui.time / tempmaqui.count;
                    tempmaqui.Dis = tempmaqui.Minmtbf / (tempmaqui.Minmtbf + tempmaqui.Maxmttr);
                }

                Tiempo_total_de_funcionamiento_p = Tiempo_total_de_funcionamiento_p + Tiempo_total_de_funcionamiento;
                allmaquinas.ElementAt(0).Maquina = "";
                allmaquinas.ElementAt(0).Maquina = "1 Area:" + amaquina;
                stemp = "1 Area:" + amaquina + " - Fecha inicial:" + x.ToString() + " - Fecha Final:" + y.ToString() + " - Multiplicador activo:0.72 " + turnos + " |";
                allmaquinas.ElementAt(0).Minmtbf = Tiempo_total_de_funcionamiento_p;
                allmaquinas.ElementAt(0).count1 = allmaquinas.ElementAt(0).count1+tempmaqui.count1;
                allmaquinas.ElementAt(0).time1 = allmaquinas.ElementAt(0).time1+ tempmaqui.time1;
                allmaquinas.ElementAt(0).count2 = allmaquinas.ElementAt(0).count2+tempmaqui.count2;
                allmaquinas.ElementAt(0).time2 = allmaquinas.ElementAt(0).time2+ tempmaqui.time2;
                allmaquinas.ElementAt(0).count3 = allmaquinas.ElementAt(0).count3+tempmaqui.count3;
                allmaquinas.ElementAt(0).time3 = allmaquinas.ElementAt(0).time3+tempmaqui.time3;
                allmaquinas.ElementAt(0).count = allmaquinas.ElementAt(0).count1 + allmaquinas.ElementAt(0).count2 + allmaquinas.ElementAt(0).count3;
                allmaquinas.ElementAt(0).time = allmaquinas.ElementAt(0).time1 + allmaquinas.ElementAt(0).time2 + allmaquinas.ElementAt(0).time3;
                if (allmaquinas.ElementAt(0).count != 0)
                {
                    allmaquinas.ElementAt(0).Minmtbf = Tiempo_total_de_funcionamiento_p / allmaquinas.ElementAt(0).count;
                    allmaquinas.ElementAt(0).Maxmttr = allmaquinas.ElementAt(0).time / allmaquinas.ElementAt(0).count;
                    allmaquinas.ElementAt(0).Dis = allmaquinas.ElementAt(0).Minmtbf / (allmaquinas.ElementAt(0).Minmtbf + allmaquinas.ElementAt(0).Maxmttr);
                }
                if (allmaquinas.ElementAt(0).count1 != 0)
                {
                    allmaquinas.ElementAt(0).NewMinmtbf1 = Tiempo_total_de_funcionamiento / allmaquinas.ElementAt(0).count1;
                    allmaquinas.ElementAt(0).NewMaxmttr1 = allmaquinas.ElementAt(0).time1 / allmaquinas.ElementAt(0).count1;
                }
                if (allmaquinas.ElementAt(0).count2 != 0)
                {
                    allmaquinas.ElementAt(0).NewMinmtbf2 = Tiempo_total_de_funcionamiento / allmaquinas.ElementAt(0).count2;
                    allmaquinas.ElementAt(0).NewMaxmttr2 = allmaquinas.ElementAt(0).time2 / allmaquinas.ElementAt(0).count2;
                }
                if (allmaquinas.ElementAt(0).count3 != 0)
                {
                    allmaquinas.ElementAt(0).NewMinmtbf3 = Tiempo_total_de_funcionamiento / allmaquinas.ElementAt(0).count3;
                    allmaquinas.ElementAt(0).NewMaxmttr3 = allmaquinas.ElementAt(0).time3 / allmaquinas.ElementAt(0).count3;
                }

                allmaquinas.Add(tempmaqui);
            }
                    ///***************


            List<string> ObjectName = new List<string>();
            List<double> Data = new List<double>();
            foreach (var item in allmaquinas.OrderBy(o => o.Minmtbf))
            {
                ObjectName.Add(item.Maquina);
                Data.Add(item.Minmtbf);

            }

            string objet;
            string dat;
            if (Data.Count() >= 1)
            {
                objet = "'";
                dat = "";
                foreach (var item in ObjectName)
                {
                    objet = objet + item.ToString() + "','";
                }
                foreach (var item in Data)
                {
                    dat = dat + item.ToString() + ",";
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName = objet;
                ViewBag.Data = dat;
               

            List<string> ObjectName2 = new List<string>();
            List<double> Data2 = new List<double>();
            foreach (var item in allmaquinas.OrderByDescending(o=>o.Maxmttr))
            {
                ObjectName2.Add(item.Maquina);
                Data2.Add(item.Maxmttr);

            }
            if (Data2.Count() >= 1)
            {

                objet = "'";
                dat = "";
                foreach (var item in ObjectName2)
                {
                    objet = objet + item.ToString() + "','";
                }
                foreach (var item in Data2)
                {
                    dat = dat + item.ToString() + ",";
                }
                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.ObjectName2 = objet;
                ViewBag.Data2 = dat;


            }
                ViewBag.items = Data.Count();
                if (Data2.Count() > Data.Count())
                ViewBag.items = Data2.Count();
                ViewBag.info = stemp;


                return View(allmaquinas);


    }
    else
            {
                ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                return RedirectToAction("Index");

            }

        }

        public ActionResult Metricos2(string amaquina, string maquina, string submaquina,string mgroup, string xmgroup, string btn = "Metricos por Mes", string dti = "", string dtf = "")
        {
            var datafiltered = new List<Bitacora>();
            var datafiltered1 = new List<Bitacora>();
            var datafiltered2 = new List<Bitacora>();
            var datafiltered3 = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<newmetricos2> Ldata = new List<newmetricos2>();

            double total_fallas_full = 0.0f, tiempomueto_full = 0.0f;

            var fecha = DateTime.Now.AddMonths(-1);
            var fechaf = DateTime.Now;

            if (!string.IsNullOrEmpty(dti))
            {
                fecha = Convert.ToDateTime(dti);
            }
            if (!string.IsNullOrEmpty(dtf))
            {
                fechaf = Convert.ToDateTime(dtf);
            }
            //---data---

            var mindia = 480.0d;
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                
                if (amaquina.Contains("Soldadura"))
                {
                    mindia = 440.0d;
                }
 
                if (amaquina.Contains("Ensamble"))
                {
                    mindia = 440.0d;
                }
                if (amaquina.Contains("Estampado"))
                {
                    mindia = 440.0d;
                }

            }
            var multiplicador = mindia;
            /*
            if (!string.IsNullOrEmpty(xmgroup))
                try
                {
                    mindia = Convert.ToDouble(xmgroup);
                }
                catch { }
          
            if (mindia > 480)
                mindia = 480;
            if (mindia < 48)
                mindia = 48;
  */
            ViewBag.xmgroup = mindia.ToString();

            if (btn == "Metricos por Años")
            {
                multiplicador = mindia * 295.494d;
            }
            //*******************************************************************************************
            if (btn == "Metricos por Mes")
            {
                multiplicador = mindia * 24.624d;
            }
            //*******************************************************************************************
            if (btn == "Metricos por Dia")
            {
                 multiplicador = mindia;
            }


            var dataf = db.Bitacoras.Where(w => (w.DiaHora.Year >= fecha.Year) &&
                           (w.DiaHora.Year <= fechaf.Year) &&
                           (w.Tiempo > 0)
                            );

            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                }

                if (amaquina.Contains("MetalFinish"))
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                }
                else
             if (amaquina=="Cromo")
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                }
                else
                if (amaquina.Contains("Pintura"))
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                }
                else
                    dataf = dataf.Where(s => s.Maquinas.Area.Contains(amaquina));
            }

            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            string[] array = { "Area", "SubMaquina" };
            ViewBag.mgroup = new SelectList(array);

            var maquinas = db.Maquinas.Where(m => m.ID > 0);
            if (!string.IsNullOrEmpty(amaquina))
                maquinas = db.Maquinas.Where(m => m.Area == amaquina);

            var maquis = maquinas.GroupBy(g => g.Maquina).ToList();
            if (mgroup == "Area")
                maquis = maquinas.GroupBy(g => g.Area).ToList();
            if (mgroup == "SubMaquina")
                    maquis = maquinas.GroupBy(g => g.SubMaquina).ToList();

                
            var mul_maquinas = maquis.Count();
           ViewBag.maquina = new SelectList(maquinas.GroupBy(g => g.Maquina), "Key", "Key");

            ViewBag.submaquina = new SelectList(maquinas, "ID", "SubMaquina");


            var data = dataf.ToList();
            //*******************************************************************************************
            if (btn == "Metricos por Años")
            {

                datafiltered = data;

                var datatempa = data.GroupBy(g => g.DiaHora.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();

                for (int i = fecha.Year; i <= fechaf.Year; i++)
                {
                    total_fallas_full = 0;
                    tiempomueto_full = 0;
                    var dd = datatempa.Where(w => w.Key == i).ToList();                   
                    int Count = 0;
                    if (dd.FirstOrDefault() != null)
                    {
                        Count = dd.FirstOrDefault().Count();

                        foreach(var item in dd) {
                            newmetricos2 datasd = new newmetricos2 { TiempoLabel = i.ToString(), TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 =0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                            int realizada = 0;
                            int pendiente = 0;
                            foreach (OILs oil in oILs.Where(s => s.Tipo == "TPM"))
                            {
                                if(!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--") {
                                if(oil.DiaHora.Year== i && oil.Maquinas.Area == amaquina) { 
                                if (oil.Estatus == 1)
                                    realizada++;

                                    pendiente++;
                                }
                                }
                                else
                                {
                                    if (oil.DiaHora.Year == i)
                                    {
                                        if (oil.Estatus == 1)
                                            realizada++;

                                        pendiente++;
                                    }
                                }
                            }



                            datasd.TarjetasTPM =Math.Round((realizada / (Double)pendiente) * 100,2);


                            datafiltered1 = item.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 14)).ToList();

                            double total_fallas = 0;
                            total_fallas = datafiltered1.Count();
                            double tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered1.Sum(s => s.Tiempo);
                            
                            DateTime x = fecha;
                            DateTime y = fechaf;
                            var z = ((y - x));
                            var zz = z.TotalDays;

                            double Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador; 
                            double MTBF = Tiempo_total_de_funcionamiento;
                            double MTTR = 0;
                            datasd.TiempoMuerto1 = Math.Round((tiempomueto/ Tiempo_total_de_funcionamiento)*100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                MTTR = tiempomueto / total_fallas;
                            }

                           // double disponibilidad = MTBF / (MTBF + MTTR);

                          //  datasd.Confiabilidad1 = Math.Round((disponibilidad * 100), 2);

                            //datasd.MTBF1 = Math.Round(MTBF, 2);
                            datasd.MTTR1 = Math.Round(MTTR, 2);


                            //-------------------------------------------------------------------------------------------

                            datafiltered2 = item.Where(s => (s.DiaHora.Hour > 14 && s.DiaHora.Hour <= 23)).ToList();

                             total_fallas = 0;
                            total_fallas = datafiltered2.Count();
                             tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                             x = Convert.ToDateTime("01/01/2020");
                            y = DateTime.Now;
                             z = ((y - x));

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                            MTBF = Tiempo_total_de_funcionamiento;
                             MTTR = 0;
                            datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                MTTR = tiempomueto / total_fallas;
                            }


                            datasd.MTTR2 = Math.Round(MTTR, 2);

                            //-------------------------------------------------------------------------------------------
                            datafiltered3 = item.Where(s => (s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)).ToList();

                            total_fallas = 0;
                            total_fallas = datafiltered3.Count();
                            tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered3.Sum(s => s.Tiempo);

                            x = Convert.ToDateTime("01/01/2020");
                            y = DateTime.Now;
                            z = ((y - x));

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                            MTBF = Tiempo_total_de_funcionamiento;
                            MTTR = 0;
                            datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            datasd.MTTR3 = Math.Round(MTTR, 2);

                            Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                            MTBF = Tiempo_total_de_funcionamiento;
                            if (total_fallas_full != 0)
                            {
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas_full;
                                MTTR = tiempomueto_full / total_fallas_full;
                            }

                            var disponibilidad = MTBF / (MTBF + MTTR);

                            datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);

                            datasd.MTBF = Math.Round(MTBF, 2);

                            Ldata.Add(datasd);
                        }
                    }



                }

                //--------------------------------------------------------

            }
            //*******************************************************************************************
            if (btn == "Metricos por Mes")
            {
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        if (
                             ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                             ||
                              ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                            ||
                              ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                            ||
                            (iaño != fecha.Year && iaño != fechaf.Year)
                            )
                        {
                            //*******************
                            datafiltered.Clear();
                            total_fallas_full = 0;
                            tiempomueto_full = 0;
                            var temp = dataaño.Where(w => w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);


                                newmetricos2 datasd = new newmetricos2 { TiempoLabel = iaño.ToString() + "-" + nombreMes, TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 = 0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                                int realizada = 0;
                                int pendiente = 0;
                                foreach (OILs oil in oILs.Where(s => s.Tipo == "TPM"))
                                {
                                if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                                {
                                    if (oil.DiaHora.Year == iaño && oil.DiaHora.Month <= jmes && oil.Maquinas.Area == amaquina)
                                    {
                                        if (oil.Estatus == 1)
                                            realizada++;

                                        pendiente++;
                                    }
                                }
                                else
                                {
                                    if (oil.DiaHora.Year == iaño && oil.DiaHora.Month <= jmes)
                                    {
                                        if (oil.Estatus == 1)
                                            realizada++;

                                        pendiente++;
                                    }
                                }

                            }


                                datasd.TarjetasTPM = Math.Round((realizada / (Double)pendiente) * 100,2);


                                datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 14)).ToList();

                                double total_fallas = 0;
                                total_fallas = datafiltered1.Count();
                                double tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered1.Sum(s => s.Tiempo);

                                DateTime x = Convert.ToDateTime("01/01/2020");
                                DateTime y = DateTime.Now;
                                var z = ((y - x));

                            double Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                                double MTBF = Tiempo_total_de_funcionamiento;
                                double MTTR = 0;
                                datasd.TiempoMuerto1 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR1 = Math.Round(MTTR, 2);


                                //-------------------------------------------------------------------------------------------

                                datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 14 && s.DiaHora.Hour <= 23)).ToList();

                                total_fallas = 0;
                                total_fallas = datafiltered2.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                            MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR2 = Math.Round(MTTR, 2);

                                //-------------------------------------------------------------------------------------------
                                datafiltered3 = datafiltered.Where(s => (s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)).ToList();

                                total_fallas = 0;
                                total_fallas = datafiltered3.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered3.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));

                            Tiempo_total_de_funcionamiento =(Double)mul_maquinas*multiplicador;
                            MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR3 = Math.Round(MTTR, 2);


                            Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                            MTBF = Tiempo_total_de_funcionamiento;
                            if (total_fallas_full != 0)
                            {
                                MTBF = (Tiempo_total_de_funcionamiento) / total_fallas_full;
                                MTTR = tiempomueto_full / total_fallas_full;
                            }

                            var disponibilidad = MTBF / (MTBF + MTTR);

                            datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);

                            datasd.MTBF = Math.Round(MTBF, 2);

                            Ldata.Add(datasd);
                            


                            i++;
                        }


                    }
             
                }
            
               
            }
            //*******************************************************************************************
            if (btn == "Metricos por Dia")
            {
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);
                        var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                        for (int kdia = 1; kdia <= dias; kdia++)
                        {
                            DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                            DateTime idi3er = idi.AddDays(+1);

                            if (idi >= fecha && idi <= fechaf)
                            {
                                //*******************
                                datafiltered.Clear();
                                total_fallas_full = 0;
                                tiempomueto_full = 0;
                                var temp = datames.Where(w => w.DiaHora.Day == kdia).ToList();
                                var temp3er = dataaño.Where(w => w.DiaHora.Month == idi3er.Month && w.DiaHora.Day == idi3er.Day).ToList();
                              //  var temp3er = datames.Where(w => w.DiaHora.Day == kdia+1).ToList();

                                if (temp.Count()!=0)
                                datafiltered.AddRange(temp);

                                newmetricos2 datasd = new newmetricos2 { TiempoLabel = iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString(), TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 = 0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                                int realizada = 0;
                                int pendiente = 0;
                                foreach (OILs oil in oILs.Where(s => s.Tipo == "TPM"))
                                {
                                    if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                                    {
                                        if (oil.DiaHora.Year == iaño && oil.DiaHora.Month <= jmes && oil.DiaHora.Day <= kdia && oil.Maquinas.Area == amaquina)
                                        {
                                            if (oil.Estatus == 1)
                                                realizada++;

                                            pendiente++;
                                        }
                                    }
                                    else
                                    {
                                        if (oil.DiaHora.Year == iaño && oil.DiaHora.Month <= jmes && oil.DiaHora.Day <= kdia)
                                        {
                                            if (oil.Estatus == 1)
                                                realizada++;

                                            pendiente++;
                                        }
                                    }

                                }



                                datasd.TarjetasTPM = Math.Round((realizada / (Double)pendiente) * 100,2);


                                datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 14)).ToList();

                                double total_fallas = 0;
                                if(datafiltered1.Count()!=0)
                                total_fallas = datafiltered1.Count();
                                double tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered1.Sum(s => s.Tiempo);

                                DateTime x = Convert.ToDateTime("01/01/2020");
                                DateTime y = DateTime.Now;
                                var z = ((y - x));

                                double Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                                double MTBF = Tiempo_total_de_funcionamiento;
                                double MTTR = 0;
                                datasd.TiempoMuerto1 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR1 = Math.Round(MTTR, 2);


                                //-------------------------------------------------------------------------------------------

                                datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 14 && s.DiaHora.Hour <= 23)).ToList();

                                total_fallas = 0;
                                if (datafiltered2.Count() != 0)
                                    total_fallas = datafiltered2.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));

                                Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                                MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR2 = Math.Round(MTTR, 2);

                                //-------------------------------------------------------------------------------------------


                                datafiltered3 = datafiltered.Where(s => (s.DiaHora.Hour > 23)).ToList();
                                var tdata3 = temp3er.Where(s => (s.DiaHora.Hour <= 7)).ToList();
                                datafiltered3.AddRange(tdata3);

                                total_fallas = 0;
                                if (datafiltered3.Count() != 0)
                                    total_fallas = datafiltered3.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered3.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));

                                Tiempo_total_de_funcionamiento = 480 *(Double)mul_maquinas;
                                MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = Tiempo_total_de_funcionamiento / total_fallas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR3 = Math.Round(MTTR, 2);

                                Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                                MTBF = Tiempo_total_de_funcionamiento;

                                if (total_fallas_full != 0)
                                {
                                    MTBF = (Tiempo_total_de_funcionamiento) / total_fallas_full;
                                    MTTR = tiempomueto_full / total_fallas_full;
                                }

                                var disponibilidad = MTBF / (MTBF + MTTR);

                                datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);

                                datasd.MTBF = Math.Round(MTBF, 2);

                                Ldata.Add(datasd);

                                i++;
                            }

                        }


                    }

                }
             
            }

            //---------------------------------------------


            ViewBag.data = Ldata;

            //--------------------------------------------

            //*******************************************************************************************
            if (btn == "Metricos por Años")
            {

                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                datafiltered = data;
                var datatempa = data.GroupBy(g => g.DiaHora.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();

                for (int i = fecha.Year; i <= fechaf.Year; i++)
                {
                    var dd = datatempa.Where(w => w.Key == i).ToList();
                    labels = labels + i.ToString() + "','";
                    if (dd.Count != 0)
                        gdata = gdata + dd.FirstOrDefault().Sum(s => s.Tiempo).ToString() + ",";
                    else
                        gdata = gdata + "0,";

                    x.Add(i);
                    if (dd.Count != 0)
                        y.Add(dd.FirstOrDefault().Count());
                    else
                        y.Add(0);




                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    for (int i = fecha.Year; i <= fechaf.Year; i++)
                    {
                        var tt = fecha.Year;
                        var res = rvalue.GetYValue(tt);
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata=gdata.TrimEnd(',', (char)39);
                gdata2=gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                foreach (var h in groupdata)
                {
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 10)
                    {
                        hdata.Add(h);
                    }
                    exit++;

                }
                int stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                int stemp = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    labels = labels + hdata[di].Key + "','";
                    double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                    double re1 = (eltiempotemp / (double)eltiempo) * 100;
                    gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                    stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";

                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata=gdata.TrimEnd(',', (char)39);
                gdata2=gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(fecha.Month);
                ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                ViewBag.dxx = " Anual";
            }
            //*******************************************************************************************
            if (btn == "Metricos por Mes")
            {
                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        if (
                             ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                             ||
                              ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                            ||
                              ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                            ||
                            (iaño != fecha.Year && iaño != fechaf.Year)
                            )
                        {
                            labels = labels + iaño.ToString() + "-" + nombreMes + "','";
                            var temp = dataaño.Where(w => w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);
                            gdata = gdata + temp.Sum(s => s.Tiempo).ToString() + ",";
                            x.Add(i);
                            y.Add(temp.Sum(s => s.Tiempo));
                            i++;
                        }


                    }

                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;
                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            if (
                                  ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                                 ||
                                  ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                                ||
                                  ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                                ||
                                (iaño != fecha.Year && iaño != fechaf.Year)
                                )
                            {
                                var res = rvalue.GetYValue(i);
                                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                i++;
                            }


                        }

                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata=gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                foreach (var h in groupdata)
                {
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 10) { 
                    hdata.Add(h);
                    }
                    exit++;
          
                }
                int stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }
               
                int stemp = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    labels = labels + hdata[di].Key + "','";
                    double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                    double re1 = (eltiempotemp / (double)eltiempo) * 100;
                    gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                    stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";

                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'";
                gdata=gdata.TrimEnd(',', (char)39);
                gdata2=gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                string nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************
            if (btn == "Metricos por Dia")
            {
                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);
                        var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                        for (int kdia = 1; kdia <= dias; kdia++)
                        {
                            DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());

                            if (idi >= fecha && idi <= fechaf)
                            {
                                labels = labels + iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString() + "','";
                                var temp = datames.Where(w => w.DiaHora.Day == kdia);
                                datafiltered.AddRange(temp);
                                gdata = gdata + temp.Sum(s => s.Tiempo).ToString() + ",";
                                x.Add(i);
                                y.Add(temp.Sum(s => s.Tiempo));
                                i++;
                            }

                        }


                    }

                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;

                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        var dataaño = data.Where(w => w.DiaHora.Year == iaño);
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                            string nombreMes = formatoFecha.GetMonthName(jmes);
                            var dias = DateTime.DaysInMonth(iaño, jmes);
                            var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                            for (int kdia = 1; kdia <= dias; kdia++)
                            {
                                DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                                if (idi >= fecha && idi <= fechaf)
                                {
                                    var res = rvalue.GetYValue(i);
                                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                    i++;
                                }

                            }


                        }

                    }

                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'";
                    gdata=gdata.TrimEnd(',', (char)39);
                    gdata2=gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                    labels = "'";
                    gdata = "";
                    gdata2 = "";

                    List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                    int exit = 0;
                    int eltiempo = 0;
                    foreach (var h in groupdata)
                    {
                        eltiempo += h.Sum(s => s.Tiempo);
                        if (exit < 10)
                        {
                            hdata.Add(h);
                        }
                        exit++;

                    }
                    int stempt = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        stempt += hdata[di].Sum(s => s.Tiempo);

                    }

                    int stemp = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        labels = labels + hdata[di].Key + "','";
                        double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                        double re1 = (eltiempotemp / (double)eltiempo) * 100;
                        gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                        stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                        double res = (stemp / (double)stempt) * 100;
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }

                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'";
                    gdata=gdata.TrimEnd(',', (char)39);
                    gdata2=gdata2.TrimEnd(',', (char)39);
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;

                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                    ViewBag.dxx = " Dia";
                }

            }

            //--------------------------------------------
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");
            return View(datafiltered);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

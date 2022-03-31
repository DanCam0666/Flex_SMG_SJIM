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
    public class BitacorasController : Controller
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
        public FileResult Export(string searchString, string area, string puesto, string usuario, string amaquina, string maquina, string fecha_inicial, string fecha_final, bool t1 = false, bool t2 = false, bool t3 = false,bool nt=false, bool fs = false)
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
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
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
                    bitacora = bitacora.Where(s => s.Usuario_area == "Cromo" || s.Usuario_area == "Cromo1" || s.Usuario_area == "Cromo2" || s.Usuario_area == "AutoPulido1" || s.Usuario_area == "AutoPulido2" || s.Usuario_area == "Pintura" || s.Usuario_area == "Ecoat" || s.Usuario_area == "Topcoat" || s.Usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.Usuario_area.Contains(area));
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
                bitacora = bitacora.Where(s => s.Usuario_puesto.Contains(puesto));
            }
            if (!String.IsNullOrEmpty(usuario) && usuario != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.Usuario.Contains(usuario));
            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.Usuario.Contains(searchString)
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

                if (!string.IsNullOrEmpty(bita.Usuario))
                {
                    if (color)
                    {
                        ws.Range($"A{i}:{x}{i}").Style.Fill.SetBackgroundColor(XLColor.LightGray);
                        color = false;
                    }
                    else color = true;
                    ws.Cell(i, 1).Value = bita.Usuario;
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
                    ws.Cell(i, 15).Value = 0;// bita.Porcentaje;
                    ws.Cell(i, 16).Value = 0;//bita.MTBF;
                    ws.Cell(i, 17).Value = 0;//bita.MTTR;
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
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
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
                    bitacora = bitacora.Where(s => s.Usuario_area == "Cromo" || s.Usuario_area == "Pintura" || s.Usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.Usuario_area.Contains(area));
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
                bitacora = bitacora.Where(s => s.Usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.Usuario.Contains(searchString)
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

                if (!string.IsNullOrEmpty(bita.Usuario))
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

            var bitacoras = db.Bitacoras.Include(b => b.Maquinas).Include(b=>b.Fallas);

            if(nt)
                bitacoras = bitacoras.Where(s => s.noterminado==true);
            if (fs)
                bitacoras = bitacoras.Where(s => s.findesemana == true);

            if (string.IsNullOrEmpty(fecha_inicial))
            {
                fecha_final = DateTime.Now.ToShortDateString();
                if (DateTime.Now.Hour < 15)
                    fecha_inicial = DateTime.Now.AddDays(-7).ToShortDateString();
                else
                    fecha_inicial = DateTime.Now.ToShortDateString();
                t1 = true;
                t2 = true;
                t3 = true;
            }
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
            {
                t1 = true;
            }
            else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 23)
            {
                t2 = true;
            }
            else
            {
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
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
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
                    bitacoras = bitacoras.Where(s => s.Usuario_area == "Cromo" || s.Usuario_area == "Pintura" || s.Usuario_area == "MetalFinish");
                }
                else
                    bitacoras = bitacoras.Where(s => s.Usuario_area.Contains(area));
            }
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("Soldadura"))
                {
                    amaquina = "Soldadura";
                    bitacoras = bitacoras.Where(s => s.Maquinas.Area.Contains(amaquina));
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
                bitacoras = bitacoras.Where(s => s.Usuario_puesto.Contains(puesto));
            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacoras = bitacoras.Where(s => s.MaquinasID.ToString().Contains(maquina));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacoras = bitacoras.Where(s => s.Usuario.Contains(searchString)
                                       || s.Maquinas.Cliente.Contains(searchString)
                                       || s.Maquinas.Area.Contains(searchString)
                                       || s.Maquinas.Maquina.Contains(searchString)
                                       || s.Maquinas.SubMaquina.Contains(searchString)
                                       || s.Sintoma.Contains(searchString)
                                       || s.Causa.Contains(searchString)
                                       || s.Folio.Contains(searchString)
                                       || s.AccionCorrectiva.Contains(searchString));
            }

            bitacoras = bitacoras.Where(s => (((s.DiaHora.Hour >= f1i && s.DiaHora.Hour < f1f) && (s.DiaHora >= fi && s.DiaHora < ff)))
                                       || (((s.DiaHora.Hour >= f2i && s.DiaHora.Hour < f2f) && (s.DiaHora >= fi && s.DiaHora < ff)))
                                       || ((s.DiaHora.Hour >= f3i || s.DiaHora.Hour < f3f) &&( s.DiaHora >= fi && s.DiaHora < ff3)));

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
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
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
                    bitacora = bitacora.Where(s => s.Usuario_area == "Cromo" || s.Usuario_area == "Pintura" || s.Usuario_area == "MetalFinish");
                }
                else
                bitacora = bitacora.Where(s => s.Usuario_area.Contains(area));
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
                bitacora = bitacora.Where(s => s.Usuario_puesto.Contains(puesto));
            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.Usuario.Contains(searchString)
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
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
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
                        bitacora2 = bitacora2.Where(s => s.Usuario_area.Contains(area)); 

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
                fecha_final = DateTime.Now.AddDays(-1).ToShortDateString();
                fecha_inicial = DateTime.Now.AddDays(-1).ToShortDateString();

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
            double Target = 0.0;

            if (t1)
            {
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
                Target += 15;
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
                Target += 15;
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
                ff3 = ff.AddDays(1);
                turnos = turnos + "| 3er ";
                Target += 15;
            }

            var tdays = ff - fi;

            Target = Target * Math.Round(tdays.TotalDays);

            ViewBag.Targetf = Target;
            //************************
            ViewBag.fechaconsulta = "La fecha inicial es: " + f1i.ToString() + "  La Fecha final es: " + f3f.ToString();

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
            var bitacoramf = bitacora.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
            var bitacoraps = bitacora.Where(s => s.Maquinas.Area.Contains("Soldadura"));
          //var bitacorams = bitacora.Where(s => s.Maquinas.Area.Contains("Soldadura"));
            var bitacoraen = bitacora.Where(s => s.Maquinas.Area.Contains("Ensamble"));
            var bitacoraau = bitacora;
            var bitacoraseM = bitacorase.Where(d => d.Tiempo > 0 && d.Maquinas.Maquina.ToUpper().Contains("MONTACARGAS"));
            bitacorase = bitacorase.Where(d => d.Tiempo > 0&& !d.Maquinas.Maquina.ToUpper().Contains("MONTACARGAS"));
           
            bitacoraes = bitacoraes.Where(d => d.Tiempo > 0);
            bitacoramf = bitacoramf.Where(d => d.Tiempo > 0);
            bitacoraps = bitacoraps.Where(d => d.Tiempo > 0);
          //bitacorams = bitacorams.Where(d => d.Tiempo > 0);
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
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);
                tdat = tdat.TrimEnd(',', (char)39);

                ViewBag.ObjectName1 = objet;
                ViewBag.Data1 = dat;
                ViewBag.tData1 = tdat;

                var bitacora2 = bitacorase;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine1top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine1top1 = Machinetop1.OrderByDescending(o=>o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine1top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine1top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine1top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine1top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
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

            query2 = bitacoraseM.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName1M = objet;
                ViewBag.Data1M = dat;
                ViewBag.tData1M = tdat;

                var bitacora2 = bitacoraseM;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine1top1nameM = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine1top1M = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine1top2nameM = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine1top2M = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine1top3nameM = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine1top3M = Machinetop3.OrderByDescending(o => o.Tiempo);
                }
            }
            else
            {
                ViewBag.Machine1top1nameM = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine1top1M = null;

                // ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
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
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName2 = objet;
                ViewBag.Data2 = dat;
                ViewBag.tData2 = tdat;

                var bitacora2 = bitacoraes;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine2top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine2top1 = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine2top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine2top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine2top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine2top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
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
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName3 = objet;
                ViewBag.Data3 = dat;
                ViewBag.tData3 = tdat;

                var bitacora2 = bitacoraps;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine3top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine3top1 = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine3top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine3top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine3top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine3top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
                }
            }
            else
            {
                ViewBag.Machine3top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine3top1 = null;
              //ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
              //return RedirectToAction("Index");

            }
            /*
            query2 = bitacorams.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName4 = objet;
                ViewBag.Data4 = dat;
                ViewBag.tData4 = tdat;

                var bitacora2 = bitacorams;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine4top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine4top1 = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine4top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine4top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine4top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine4top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
                }
                }
                else
                {
                    ViewBag.Machine4top1name = "Sin Maquinas con tiempo muerto";
                    ViewBag.Machine4top1 = null;

                  //ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                  //TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
                  //return RedirectToAction("Index");
                }
                */
            query2 = bitacoramf.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName5 = objet;
                ViewBag.Data5 = dat;
                ViewBag.tData5 = tdat;

                var bitacora2 = bitacoramf;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine5top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine5top1 = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine5top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine5top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine5top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine5top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
                }
            }
            else
            {

                ViewBag.Machine5top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine5top1 = null;
             // ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // return RedirectToAction("Index");
            }

            query2 = bitacoraen.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName6 = objet;
                ViewBag.Data6 = dat;
                ViewBag.tData6 = tdat;

                var bitacora2 = bitacoraen;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine6top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine6top1 = Machinetop1.OrderByDescending(o => o.Tiempo);
                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine6top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine6top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine6top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine6top3 = Machinetop3.OrderByDescending(o => o.Tiempo);
                }
            }
            else
            {
                ViewBag.Machine6top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine6top1 = null;
             // ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // return RedirectToAction("Index");
            }
           
            query2 = bitacoraau.GroupBy(p => p.Maquinas.Maquina)
            .Select(g => new { Maquina = g.Key, count = g.Count(), time = g.Sum(x => x.Tiempo) });
            query = query2.ToList();
            if (query.Count() >= 1)
            {
                int i = 0;
                string objet;
                string dat;
                string tdat;

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
                tdat = "";
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
                    tdat = tdat + Target.ToString() + ",";
                    i++;
                    if (i == 3)
                        break;
                }
                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.ObjectName7 = objet;
                ViewBag.Data7 = dat;
                ViewBag.tData7 = tdat;

                var bitacora2 = bitacoraau;

                string xx = ObjectName2.ElementAt(0).ToString();
                ViewBag.Machine7top1name = xx;
                List<Flex_SGM.Models.Bitacora> Machinetop1 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                ViewBag.Machine7top1 = Machinetop1.OrderByDescending(o => o.Tiempo);

                if (query.Count() > 1)
                {
                    xx = ObjectName2.ElementAt(1).ToString();
                    ViewBag.Machine7top2name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop2 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine7top2 = Machinetop2.OrderByDescending(o => o.Tiempo);
                }
                if (query.Count() > 2)
                {
                    xx = ObjectName2.ElementAt(2).ToString();
                    ViewBag.Machine7top3name = xx;
                    List<Flex_SGM.Models.Bitacora> Machinetop3 = bitacora2.Where(p => p.Tiempo >= 1 && p.Maquinas.Maquina == xx).ToList();
                    ViewBag.Machine7top3 = Machinetop3.OrderByDescending(o => o.Tiempo);

                }
            }
            else
            {

                ViewBag.Machine7top1name = "Sin Maquinas con tiempo muerto";
                ViewBag.Machine7top1 = null;
             // ViewBag.Danger = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // TempData["Danger"] = "No existe la cantidad minima de 1 maquina con falla para poder generar el Reporte!";
             // return RedirectToAction("Index");
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5);
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata.TrimEnd(',', (char)39);
                gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5);
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata.TrimEnd(',', (char)39);
                    gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    var groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5); 
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
        public ActionResult code(int Codigo)
        {
            // TODO: based on the selected
            var fallas = db.Fallas.Where(f => f.ID == Codigo).ToList();
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")


            string x1des = fallas.FirstOrDefault().Tipo;
            string x2des = fallas.FirstOrDefault().Descripcion;

            return Json(new { x1des = x1des, x2des = x2des }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult code1(string Tipo)
        {
            // TODO: based on the selected
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
        public ActionResult code2(string Tipo,string Des)
        {
            // TODO: based on the selected
            // TODO: based on the selected
            var fallas = db.Fallas.Where(f => f.Tipo == Tipo && f.Descripcion == Des).ToList();
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")

            var codigo = Tipo +"-"+ Des;

            //  List<string> codigo = new List<string>();
            /*
            foreach (var item in fallas)
            {
                codigo.Add(fallas.FirstOrDefault().Codigo);

            }
            */

            // var cities = fallas.ToList();
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult drop3(string Tipo)
        {
            // TODO: based on the selected
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

        //  [Authorize]
        public ActionResult Create()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string scliente = "";
            string sarea = currentUser.Area;
            string sarea2 = sarea;
            if (TempData["myarea"] != null)
                sarea2 = TempData["myareaf"].ToString();
            if (sarea.Contains("Soldadura"))
            {
                sarea = "Soldadura";
            }
            if (TempData["mycliente"] != null)
                scliente = TempData["mycliente"].ToString();
            if (TempData["myarea"] != null)
                sarea = TempData["myarea"].ToString();
            var maquinas = db.Maquinas.Where(m => m.ID >= 0);
            if (!string.IsNullOrEmpty(scliente))
            {
                if (maquinas.Where(m => m.Cliente == scliente).Count() != 0)
                    maquinas = maquinas.Where(m => m.Cliente == scliente);
            }

            if (!string.IsNullOrEmpty(sarea))
            {
                if (sarea.Contains("MetalFinish"))
                {
                    if (maquinas.Where(m => m.Area == "Cromo").Count() != 0 || maquinas.Where(m => m.Area == "Pintura").Count() != 0 || maquinas.Where(m => m.Area == "MetalFinish").Count() != 0)
                        maquinas = maquinas.Where(m => m.Area == "Cromo" || m.Area == "Pintura" || m.Area == "MetalFinish");
                }
                else
                if (maquinas.Where(m => m.Area == sarea).Count() != 0)
                    maquinas = maquinas.Where(m => m.Area == sarea);
            }
            ViewBag.Area = sarea;
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");
            ViewBag.MaquinasID = new SelectList(maquinas, "ID", "SubMaquina");

            List<SelectListItem> lst = new List<SelectListItem>();

            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
                lst.Add(new SelectListItem() { Text = "1", Value = "1", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "1", Value = "1" });

            if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 23)
                lst.Add(new SelectListItem() { Text = "2", Value = "2", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "2", Value = "2" });

            if (DateTime.Now.Hour >= 23 || DateTime.Now.Hour < 7)
                lst.Add(new SelectListItem() { Text = "3", Value = "3", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "3", Value = "3" });

            ViewBag.turno = lst;
            Bitacora bitacora = new Bitacora();
            bitacora.Usuario = currentUser.UserFullName;
            bitacora.DiaHora = DateTime.Now;
            bitacora.Tiempo = 0;
            ViewBag.listcliente = new SelectList(Enum.GetValues(typeof(flex_Cliente)).Cast<flex_Cliente>().ToList());
            ViewBag.listarea = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());

            var fallas = db.Fallas.Where(f => f.Area == sarea2);
            if (fallas.Count() == 0)
                fallas = db.Fallas;
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")
            var fallasg = fallas.GroupBy(g => g.Tipo).ToList();

            ViewBag.ltipos = new SelectList(fallasg, "Key", "Key");    // initially we set the ddl to empty
            var fallasg2 = fallas.GroupBy(g => g.Descripcion).ToList();
            ViewBag.lareas = new SelectList(fallasg2, "Key", "Key");  //new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList()),

            return View(bitacora);
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
        //  [Authorize(Roles = "Mantenimiento")]
        public async Task<ActionResult> Create( Bitacora bitacora,int MaquinasID)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            bitacora.MaquinasID = MaquinasID;

         // bitacora.Tipos = MyViewFallas.Area + "|" + MyViewFallas.Tipo;
         // bitacora.Descripcion = MyViewFallas.Tipo + "|" + MyViewFallas.Des;

            if (bitacora.FallasID != null)
            {
                var falla = db.Fallas.Where(w => w.ID == bitacora.FallasID);
                bitacora.Descripcion = falla.FirstOrDefault().Codigo;
            }

            bitacora.Usuario = currentUser.UserFullName;
            bitacora.Usuario_area = currentUser.Area;
            bitacora.Usuario_puesto = currentUser.Puesto;

            // bitacora.Porcentaje = 0;
            // bitacora.MTBF = 0;
            // bitacora.MTTR = 0;

            if (string.IsNullOrEmpty(bitacora.Atendio))
                bitacora.Atendio = bitacora.Usuario;
            if (string.IsNullOrEmpty(bitacora.Scrap))
                bitacora.Scrap = "N/A";
            if (string.IsNullOrEmpty(bitacora.Folio))
                bitacora.Folio = "N/A";
            if (!bitacora.Fallaoperacion && !bitacora.MttoCorrectivo && !bitacora.MttoPreventivo && !bitacora.MttoMejora)
                bitacora.MttoMejora = true;

            if (ModelState.IsValid)
            {
                db.Bitacoras.Add(bitacora);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            MyViewBitcora nbita = new MyViewBitcora();
        
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina", bitacora.MaquinasID);
            return View(nbita);
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

            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
                lst.Add(new SelectListItem() { Text = "1", Value = "1", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "1", Value = "1" });

            if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 23)
                lst.Add(new SelectListItem() { Text = "2", Value = "2", Selected = true });
            else
                lst.Add(new SelectListItem() { Text = "2", Value = "2" });

            if (DateTime.Now.Hour >= 23 || DateTime.Now.Hour < 7)
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,turno,usuario,usuario_area,usuario_puesto,MaquinasID,Sintoma,Causa,AccionCorrectiva,Atendio ,Tiempo,Scrap ,Folio,findesemana,Fallaoperacion ,MttoPreventivo,MttoCorrectivo,MttoMejora,Verifico,FechaVerificacion,noterminado,Descripcion")] Bitacora bitacora)
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
                    bitacora = bitacora.Where(s => s.Usuario_area == "Cromo" || s.Usuario_area == "Pintura" || s.Usuario_area == "MetalFinish");
                }
                else
                    bitacora = bitacora.Where(s => s.Usuario_area.Contains(area));
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
                bitacora = bitacora.Where(s => s.Usuario_puesto.Contains(puesto));

            }
            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                bitacora = bitacora.Where(s => s.MaquinasID.ToString().Contains(maquina));

            }

            if (!String.IsNullOrEmpty(searchString))
            {
                bitacora = bitacora.Where(s => s.Usuario.Contains(searchString)
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
                f1i = 7;
                f1f = 15;
                turnos = turnos + "| 1er ";
            }
            if (t2)
            {
                f2i = 15;
                f2f = 23;
                turnos = turnos + "| 2do ";
            }
            if (t3)
            {
                f3i = 23;
                f3f = 7;
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
                // Minmtbf = g.Min(zx1 => zx1.MTBF),
                // Maxmttr = g.Max(zz2 => zz2.MTTR),
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
                // Minmtbf = g.Min(z1 => z1.MTBF),
                // Maxmttr = g.Max(z2 => z2.MTTR),
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
                //  Minmtbf = g.Min(z2 => z2.MTBF),
                // Maxmttr = g.Max(z3 => z3.MTTR),
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
                //  Minmtbf = g.Min(z3 => z3.MTBF),
                // Maxmttr = g.Max(z4 => z4.MTTR),
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

        public ActionResult Metricos2(string amaquina, string maquina, string submaquina,string mgroup, string xmgroup, string btn = "Metricos por Dia", string dti = "", string dtf = "")
        {
            var datafiltered = new List<Bitacora>();
            var datafiltered1 = new List<Bitacora>();
            var datafiltered2 = new List<Bitacora>();
            var datafiltered3 = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<newmetricos2> Ldata = new List<newmetricos2>();

            double total_fallas_full = 0.0f, tiempomueto_full = 0.0f;

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

            if (!string.IsNullOrEmpty(maquina))
                dataf = dataf.Where(m => m.Maquinas.Maquina == maquina);

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

                if (!string.IsNullOrEmpty(maquina) && amaquina != "--Todas--")
                {
                    dataf = dataf.Where(s => s.Maquinas.Maquina==maquina);                
                }
            }

            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            string[] array = { "Maquina", "Falla", "Area", "SubMaquina" };
            ViewBag.mgroup = new SelectList(array);

            var maquinas = db.Maquinas.Where(m => m.ID > 0);
            if (!string.IsNullOrEmpty(amaquina))
                maquinas = db.Maquinas.Where(m => m.Area == amaquina);

            var maquis = maquinas.GroupBy(g => g.Maquina).ToList();
            /*
            if (mgroup == "Area")
                maquis = maquinas.GroupBy(g => g.Area).ToList();
            if (mgroup == "SubMaquina")
                    maquis = maquinas.GroupBy(g => g.SubMaquina).ToList();
            */
                
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

                        foreach (var item in dd) {
                            newmetricos2 datasd = new newmetricos2 { TiempoLabel = i.ToString(), TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 = 0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                            int realizada = 0;
                            int pendiente = 0;
                            var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                            if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                            {
                                alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                            }
                            foreach (OILs oil in alloils)
                            {
                                if (oil.DiaHora.Year <= i)
                                    pendiente++;

                                if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year <= i)
                                    realizada++;
                            }

                            datasd.TarjetasTPM = Math.Round((realizada / (Double)pendiente) * 100, 2);
                            //---- primero
                            datafiltered1 = item.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();
                       
                            double total_fallas = 0;
                            total_fallas = datafiltered1.Count();
                            double tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered1.Sum(s => s.Tiempo);
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;
                            DateTime x = fecha;
                            DateTime y = fechaf;
                            var z = ((y - x));
                            var zz = z.TotalDays;

                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered1.GroupBy(g => g.Maquinas.Maquina).Count();

                            double Tiempo_total_de_funcionamiento = (Double)mul_maquinas * multiplicador;
                            datasd.Disponible1 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM1 = tiempomueto;
                            datasd.FallasT1 = total_fallas;
                            double MTBF = Tiempo_total_de_funcionamiento;
                            double MTTR = 0;
                            datasd.TiempoMuerto1 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100, 2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            // double disponibilidad = MTBF / (MTBF + MTTR);

                            //  datasd.Confiabilidad1 = Math.Round((disponibilidad * 100), 2);

                            //datasd.MTBF1 = Math.Round(MTBF, 2);
                            datasd.MTTR1 = Math.Round(MTTR, 2);

                            //-------------------------------------------------------------------------------------------

                            datafiltered2 = item.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();

                            total_fallas = 0;
                            total_fallas = datafiltered2.Count();
                            tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                            x = Convert.ToDateTime("01/01/2020");
                            y = DateTime.Now;
                            z = ((y - x));

                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered2.GroupBy(g => g.Maquinas.Maquina).Count();
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas * multiplicador;
                            datasd.Disponible2 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM2 = tiempomueto;
                            datasd.FallasT2 = total_fallas;
                            MTBF = Tiempo_total_de_funcionamiento;
                            MTTR = 0;
                            datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100, 2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
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

                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered3.GroupBy(g => g.Maquinas.Maquina).Count();
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas * multiplicador;
                            datasd.Disponible3 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM3 = tiempomueto;
                            datasd.FallasT3 = total_fallas;
                            MTBF = Tiempo_total_de_funcionamiento;
                            MTTR = 0;
                            datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            datasd.MTTR3 = Math.Round(MTTR, 2);

                            Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                            MTBF = Tiempo_total_de_funcionamiento;
                            if (total_fallas_full != 0)
                            {
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas_full)/ mul_maquinas;
                                MTTR = tiempomueto_full / total_fallas_full;
                            }

                            var disponibilidad = MTBF / (MTBF + MTTR);

                            datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);

                            datasd.MTBF = Clamp(Math.Round(MTBF, 2), 0, (multiplicador * 3));

                            Ldata.Add(datasd);
                        }
                    }
                }
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
                         
                            total_fallas_full = 0;
                            tiempomueto_full = 0;
                            datafiltered.Clear();
                            var temp = dataaño.Where(w => w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);
                       
                            List<newmetricosmaquina> allmaq = new List<newmetricosmaquina>();
                            foreach (var simplemaquina in maquis)
                            {
                                newmetricosmaquina simplemaquinam = new newmetricosmaquina
                                {
                                    maquina = simplemaquina.Key,
                                    tiempod = (Double) multiplicador,
                                    fallas = datafiltered.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    tiempof = datafiltered.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    mttr = 0,
                                    mtbf = (Double) multiplicador,
                                    confiabilidad = 100
                                };
                                if (simplemaquinam.fallas != 0) {
                                simplemaquinam.mttr = simplemaquinam.tiempof / simplemaquinam.fallas;
                                simplemaquinam.mtbf = simplemaquinam.tiempod / simplemaquinam.fallas;
                                simplemaquinam.confiabilidad = simplemaquinam.mtbf / (simplemaquinam.mtbf + simplemaquinam.mttr);
                                }
                                allmaq.Add(simplemaquinam);
                            }
                          
                            newmetricos2 datasd = new newmetricos2 { TiempoLabel = iaño.ToString() + "-" + nombreMes, TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 = 0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                            int realizada = 0;
                            int pendiente = 0;
                            var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                            if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                            {
                                alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                            }
                            foreach (OILs oil in alloils)
                            {


                                if (oil.DiaHora.Year < iaño)
                                    pendiente++;
                                else
                                if (oil.DiaHora.Year == iaño && oil.DiaHora.Month <= jmes)
                                    pendiente++;



                                if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year < iaño)
                                    realizada++;
                                else
                                if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year == iaño && oil.DiaHora_Cierre.Value.Month <= jmes)
                                    realizada++;


                            }

                            datasd.TarjetasTPM = Math.Round((realizada / (Double)pendiente) * 100, 2);


                            datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();

                            double total_fallas = 0;
                            total_fallas = datafiltered1.Count();
                            double tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered1.Sum(s => s.Tiempo);
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            DateTime x = Convert.ToDateTime("01/01/2020");
                            DateTime y = DateTime.Now;
                            var z = ((y - x));
                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered1.GroupBy(g => g.Maquinas.Maquina).Count();
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            double Tiempo_total_de_funcionamiento = (Double)mul_maquinas * multiplicador;
                            datasd.Disponible1 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM1 = tiempomueto;
                            datasd.FallasT1 = total_fallas;
                            double MTBF = Tiempo_total_de_funcionamiento;
                            double MTTR = 0;
                            datasd.TiempoMuerto1 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100, 2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            datasd.MTTR1 = Math.Round(MTTR, 2);


                            //-------------------------------------------------------------------------------------------

                            datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();

                            total_fallas = 0;
                            total_fallas = datafiltered2.Count();
                            tiempomueto = 0;
                            if (total_fallas >= 1)
                                tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                            x = Convert.ToDateTime("01/01/2020");
                            y = DateTime.Now;
                            z = ((y - x));
                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered2.GroupBy(g => g.Maquinas.Maquina).Count();
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            Tiempo_total_de_funcionamiento = (Double)mul_maquinas * multiplicador;
                            datasd.Disponible2 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM2 = tiempomueto;
                            datasd.FallasT2 = total_fallas;
                            MTBF = Tiempo_total_de_funcionamiento;
                            MTTR = 0;
                            datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            datasd.MTTR2 = Math.Round(MTTR, 2);

                            //-------------------------------------------------------------------------------------------
                            datafiltered3 = datafiltered.Where(s => ((s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)&& s.DiaHora.Day!=1)).ToList();
                            datafiltered3.AddRange(datafiltered.Where(s => ((s.DiaHora.Hour > 23 ) && s.DiaHora.Day == 1)).ToList());

                            total_fallas = 0;
                                total_fallas = datafiltered3.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered3.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));
                            if (mgroup == "Falla")
                                mul_maquinas = datafiltered3.GroupBy(g => g.Maquinas.Maquina).Count();
                            if (mul_maquinas == 0)
                                mul_maquinas = 1;

                            Tiempo_total_de_funcionamiento =(Double)mul_maquinas*multiplicador;
                            datasd.Disponible3 = Tiempo_total_de_funcionamiento;
                            datasd.TiempoM3 = tiempomueto;
                            datasd.FallasT3 = total_fallas;
                            MTBF = Tiempo_total_de_funcionamiento;
                            MTTR = 0;
                            datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                            if (total_fallas != 0)
                            {
                                total_fallas_full += total_fallas;
                                tiempomueto_full += tiempomueto;
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                MTTR = tiempomueto / total_fallas;
                            }

                            datasd.MTTR3 = Math.Round(MTTR, 2);

                            Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                            MTBF = Tiempo_total_de_funcionamiento;

                            if (total_fallas_full != 0)
                            {
                                MTBF = (Tiempo_total_de_funcionamiento / total_fallas_full)/ mul_maquinas;
                                MTTR = tiempomueto_full / total_fallas_full;
                            }
                            
                            double suma = 0;
                            double cant = 0;
                            foreach (var maqx in allmaq)
                            {
                                suma = suma + maqx.mtbf;
                                cant++;
                            }
                            var days = DateTime.DaysInMonth(iaño, jmes);
                            MTBF = (suma / cant);
                            
                            var disponibilidad = MTBF / (MTBF + MTTR);

                            datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);
                            datasd.MTBF = Clamp(Math.Round(MTBF, 2), 0, (multiplicador * 3));

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
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);

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
                                var temp = data.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day).ToList();
                                var temp3er = data.Where(w => w.DiaHora.Year == idi3er.Year && w.DiaHora.Month == idi3er.Month && w.DiaHora.Day == idi3er.Day).ToList();

                                if (temp.Count()!=0)
                                datafiltered.AddRange(temp);

                                newmetricos2 datasd = new newmetricos2 { TiempoLabel = iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString(), TiempoMuerto1 = 0, TiempoMuerto2 = 0, TiempoMuerto3 = 0, MTTR1 = 0, MTTR2 = 0, MTTR3 = 0, MTBF = 0, TarjetasTPM = 0 };

                                int realizada = 0;
                                int pendiente = 0;
                                var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                                if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                                {
                                    alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                                }
                                foreach (OILs oil in alloils)
                                {
                                    if (oil.DiaHora.Year < iaño )
                                        pendiente++;
                                    else
                                    if (oil.DiaHora.Year == iaño && oil.DiaHora.Month < jmes)
                                        pendiente++;
                                    else
                                    if (oil.DiaHora.Year == iaño && oil.DiaHora.Month == jmes && oil.DiaHora.Day <= kdia)
                                        pendiente++;

                                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year < iaño)
                                            realizada++;
                                    else
                                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year == iaño && oil.DiaHora_Cierre.Value.Month < jmes)
                                            realizada++;
                                    else
                                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year == iaño && oil.DiaHora_Cierre.Value.Month == jmes && oil.DiaHora_Cierre.Value.Day <= kdia)
                                            realizada++;
                                }

                                datasd.TarjetasTPM = Math.Round((realizada / (Double)pendiente) * 100,2);

                                datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();

                                double total_fallas = 0;
                                if(datafiltered1.Count()!=0)
                                total_fallas = datafiltered1.Count();
                                double tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered1.Sum(s => s.Tiempo);

                                DateTime x = Convert.ToDateTime("01/01/2020");
                                DateTime y = DateTime.Now;
                                var z = ((y - x));
                                if (mgroup == "Falla")
                                    mul_maquinas = datafiltered1.GroupBy(g => g.Maquinas.Maquina).Count();
                                if (mul_maquinas == 0)
                                    mul_maquinas = 1;
                                double Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                                datasd.Disponible1 = Tiempo_total_de_funcionamiento;
                                datasd.TiempoM1 = tiempomueto;
                                datasd.FallasT1 = total_fallas;
                                double MTBF = Tiempo_total_de_funcionamiento;
                                double MTTR = 0;
                                datasd.TiempoMuerto1 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                    MTTR = tiempomueto / total_fallas;
                                }
                                datasd.MTTR1 = Math.Round(MTTR, 2);

                                //-------------------------------------------------------------------------------------------

                                datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();

                                total_fallas = 0;
                                if (datafiltered2.Count() != 0)
                                    total_fallas = datafiltered2.Count();
                                tiempomueto = 0;
                                if (total_fallas >= 1)
                                    tiempomueto = datafiltered2.Sum(s => s.Tiempo);

                                x = Convert.ToDateTime("01/01/2020");
                                y = DateTime.Now;
                                z = ((y - x));
                                if (mgroup == "Falla")
                                    mul_maquinas = datafiltered2.GroupBy(g => g.Maquinas.Maquina).Count();
                                if (mul_maquinas == 0)
                                    mul_maquinas = 1;
                                Tiempo_total_de_funcionamiento = (Double)mul_maquinas*multiplicador;
                                datasd.Disponible2 = Tiempo_total_de_funcionamiento;
                                datasd.TiempoM2 = tiempomueto;
                                datasd.FallasT2 = total_fallas;
                                MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto2 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
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
                                if (mgroup == "Falla")
                                    mul_maquinas = datafiltered3.GroupBy(g => g.Maquinas.Maquina).Count();
                                if (mul_maquinas == 0)
                                    mul_maquinas = 1;
                                Tiempo_total_de_funcionamiento = mindia * (Double)mul_maquinas;
                                datasd.Disponible3 = Tiempo_total_de_funcionamiento;
                                datasd.TiempoM3 = tiempomueto;
                                datasd.FallasT3 = total_fallas;
                                MTBF = Tiempo_total_de_funcionamiento;
                                MTTR = 0;
                                datasd.TiempoMuerto3 = Math.Round((tiempomueto / Tiempo_total_de_funcionamiento) * 100,2);
                                if (total_fallas != 0)
                                {
                                    total_fallas_full += total_fallas;
                                    tiempomueto_full += tiempomueto;
                                    MTBF = (Tiempo_total_de_funcionamiento / total_fallas)/ mul_maquinas;
                                    MTTR = tiempomueto / total_fallas;
                                }

                                datasd.MTTR3 = Math.Round(MTTR, 2);

                                Tiempo_total_de_funcionamiento = Tiempo_total_de_funcionamiento * 3;
                                MTBF = Tiempo_total_de_funcionamiento;

                                if (total_fallas_full != 0)
                                {
                                    MTBF = (Tiempo_total_de_funcionamiento / total_fallas_full)/ mul_maquinas;
                                    MTTR = tiempomueto_full / total_fallas_full;
                                }

                                var disponibilidad = MTBF / (MTBF + MTTR);

                                datasd.Confiabilidad = Math.Round((disponibilidad * 100), 2);

        
                                datasd.MTBF =Clamp(Math.Round(MTBF, 2),0, (multiplicador * 3));
                                Ldata.Add(datasd);

                                i++;
                            }
                        }
                    }
                }
            }

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
               // datafiltered = data;
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata=gdata.TrimEnd(',', (char)39);
                gdata2=gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                var groupdata = datafiltered.GroupBy(g => g.Maquinas.Maquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;
                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                datafiltered.Clear();
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

                            var temp = dataaño.Where(w =>w.DiaHora.Year==iaño && w.DiaHora.Month == jmes);
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;
                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                datafiltered.Clear();
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
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
                    List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                    foreach (var h in groupdata)
                    {
                        newmetricosmaquina nmm = new newmetricosmaquina();
                        eltiempo += h.Sum(s => s.Tiempo);
                        if (exit < 5)
                        {
                            hdata.Add(h);
                            nmm.tiempod = multiplicador;
                            nmm.maquina = h.Key;
                            nmm.tiempof = h.Sum(s => s.Tiempo);
                            nmm.fallas = h.Count();
                            nmm.mttr = nmm.tiempof / nmm.fallas;
                            nmm.mtbf = multiplicador / nmm.fallas;
                            var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                            nmm.confiabilidad = disponibilidad;
                            lnmm.Add(nmm);
                        }
                        exit++;
                    }
                    ViewBag.lnmm = lnmm;

                    var objet = "'";
                    var dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mttr.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmttr = objet;
                    ViewBag.Datamttr = dat;
                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.tiempof.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelstf = objet;
                    ViewBag.Datatf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.fallas.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsf = objet;
                    ViewBag.Dataf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mtbf.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmtbf = objet;
                    ViewBag.Datamtbf = dat;

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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
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

        public ActionResult Metricos3(string amaquina, string maquina, string submaquina, string mgroup, string xmgroup, string btn = "Metricos por Dia", string dti = "", string dtf = "")
        {
            var fulldatafiltered = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var datafiltered1 = new List<Bitacora>();
            var datafiltered2 = new List<Bitacora>();
            var datafiltered3 = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<newmetricos2> Ldata = new List<newmetricos2>();
            List<newmetricos3> Ldata3 = new List<newmetricos3>();
            var fecha = DateTime.Now.AddDays(-7);
            var fechaf = DateTime.Now;
            DateTimeFormatInfo formatoFecha1 = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes1 = formatoFecha1.GetMonthName(fecha.Month);
            string nombreMes2 = formatoFecha1.GetMonthName(fechaf.Month);

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

            ViewBag.xmgroup = mindia.ToString();
            var dataf = db.Bitacoras.Where(w => (w.DiaHora.Year >= fecha.Year) &&
                           (w.DiaHora.Year <= fechaf.Year) &&
                           (w.Tiempo > 0)
                            );
            if (!string.IsNullOrEmpty(maquina))
                dataf = dataf.Where(m => m.Maquinas.Maquina == maquina);

            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("MetalFinish"))
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                    var dtdt = dataf.ToList();
                }
                else { 
                    if (amaquina == "Cromo")
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                    }
       
                    if (amaquina.Contains("Pintura"))
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                    }

                    if (amaquina.Contains("Soldadura"))
                    {
                        amaquina = "Soldadura";
                    }
                    dataf = dataf.Where(s => s.Maquinas.Area.Contains(amaquina));
                }
            }

            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            string[] array = { "Maquina", "Falla", "Area", "SubMaquina" };
            ViewBag.mgroup = new SelectList(array);

            var maquinas = db.Maquinas.Where(m => m.ID > 0);
            if (!string.IsNullOrEmpty(amaquina))
                if (amaquina.Contains("MetalFinish"))
                {
                    maquinas = db.Maquinas.Where(m => m.Area == "Cromo" || m.Area == "Cromo1" || m.Area == "Cromo2" || m.Area == "AutoPulido1" || m.Area == "AutoPulido2" || m.Area == "Pintura" || m.Area == "Ecoat" || m.Area == "Topcoat" || m.Area == "MetalFinish");
              
                }
                else
                    maquinas = db.Maquinas.Where(m => m.Area == amaquina);

            if (!string.IsNullOrEmpty(maquina))
                maquinas = db.Maquinas.Where(m => m.Maquina == maquina);

            var maquis = maquinas.GroupBy(g => g.Maquina).ToList();

            var mul_maquinas = maquis.Count();
            ViewBag.maquina = new SelectList(maquinas.GroupBy(g => g.Maquina), "Key", "Key");

            ViewBag.submaquina = new SelectList(maquinas, "ID", "SubMaquina");

            List<newmetricos3> allmaq = new List<newmetricos3>();

            for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
            {
                for (int jmes = 1; jmes <= 12; jmes++)
                {
                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                    string nombreMes = formatoFecha.GetMonthName(jmes);
                    var dias = DateTime.DaysInMonth(iaño, jmes);

                    for (int kdia = 1; kdia <= dias; kdia++)
                    {
                        DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                        DateTime idi3er = idi.AddDays(+1);

                        if (idi >= fecha && idi <= fechaf)
                        {
                            //datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Day != 1).ToList();
                            datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Hour > 7).ToList();
                            //datafiltered.AddRange(temp1);
                            var temp2 = dataf.Where(w => w.DiaHora.Year == idi3er.Year && w.DiaHora.Month == idi3er.Month && w.DiaHora.Day == idi3er.Day && w.DiaHora.Hour <= 7).ToList();
                            datafiltered.AddRange(temp2);
                            datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();
                            datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();
                            datafiltered3 = datafiltered.Where(s => (s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)).ToList();
                            fulldatafiltered.AddRange(datafiltered);

                            foreach (var simplemaquina in maquis)
                            {
                                string thistiempo="";
                                if (btn == "Metricos por Dia")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString();
                                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                                    ViewBag.dxx = " Dia";
                                }

                                if (btn == "Metricos por Mes")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes;
                                    ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                                    ViewBag.dxx = " Mes";
                                }
                                if (btn == "Metricos por Años")
                                {
                                        thistiempo = iaño.ToString();
                                    ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                                    ViewBag.dxx = " Anual";
                                }
                                newmetricos3 simplemaquinam = new newmetricos3
                                {
                                    TiempoLabel = thistiempo,
                                    maquina = simplemaquina.Key,
                                    Disponible1 = (Double)multiplicador,
                                    Disponible2 = (Double)multiplicador,
                                    Disponible3 = (Double)multiplicador,
                                    CantidadFallas1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    TiempoMuerto1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    MTTR1 = 0,
                                    MTTR2 = 0,
                                    MTTR3 = 0,
                                    MTBF = (Double)multiplicador*3,
                                    Confiabilidad = 100,
                                    TarjetasTPM = 0
                                };

                                if (simplemaquinam.CantidadFallas1 != 0)
                                {
                                    simplemaquinam.MTTR1 = simplemaquinam.TiempoMuerto1 / simplemaquinam.CantidadFallas1;
                                }
                                if (simplemaquinam.CantidadFallas2 != 0)
                                {
                                    simplemaquinam.MTTR2 = simplemaquinam.TiempoMuerto2 / simplemaquinam.CantidadFallas2;
                                }
                                if (simplemaquinam.CantidadFallas3 != 0)
                                {
                                    simplemaquinam.MTTR3 = simplemaquinam.TiempoMuerto3 / simplemaquinam.CantidadFallas3;
                                }
                                var SumDisponobilidad = simplemaquinam.Disponible1 + simplemaquinam.Disponible2 + simplemaquinam.Disponible3;
                                simplemaquinam.MTBF = SumDisponobilidad;
                                if (simplemaquinam.CantidadFallas1 != 0 || simplemaquinam.CantidadFallas2 != 0 || simplemaquinam.CantidadFallas3 != 0)
                                {                                  
                                    var sumCantidadFallas = simplemaquinam.CantidadFallas1 + simplemaquinam.CantidadFallas2 + simplemaquinam.CantidadFallas3;
                                    var sumTiempoMuerto = simplemaquinam.TiempoMuerto1 + simplemaquinam.TiempoMuerto2 + simplemaquinam.TiempoMuerto3;
                                    var MTTR = (simplemaquinam.MTTR1 + simplemaquinam.MTTR2 + simplemaquinam.MTTR3) / 3;
                                    simplemaquinam.MTBF = SumDisponobilidad / sumCantidadFallas;
                                  //  simplemaquinam.Confiabilidad = simplemaquinam.MTBF / (simplemaquinam.MTBF + MTTR);
                                    simplemaquinam.Confiabilidad = (simplemaquinam.MTBF - MTTR) / simplemaquinam.MTBF;
                                }
                                allmaq.Add(simplemaquinam);
                            }
                        }
                    }
                }
            }

            var allmaqbyday1 = allmaq.GroupBy(g => g.maquina).ToList();

            foreach (var inmaq in allmaqbyday1)
            {
                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);
                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);
                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);
                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);
                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);
                var sumatm1 = sumtp1;// ((sumtp1 / sumd1) * 100);
                var sumatm2 = sumtp2;//((sumtp2 / sumd2) * 100);
                var sumatm3 = sumtp2;//((sumtp3 / sumd3) * 100);
                var smttr11 = inmaq.Sum(s => s.MTTR1);
                var smttr12 = inmaq.Sum(s => s.MTTR2);
                var smttr13 = inmaq.Sum(s => s.MTTR3);
                var sumamttr1 = smttr11 / inmaq.Count();
                var sumamttr2 = smttr12 / inmaq.Count();
                var sumamttr3 = smttr13 / inmaq.Count();
                var sumamtbf = (sumd1 + sumd2 + sumd3) / (ft1 + ft2 + ft3); //inmaq.Sum(s => s.MTBF) / inmaq.Count();
                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;
                var sumaMant = ((sumamtbf - sumamttrs) / sumamtbf) * 100;
                newmetricos3 data_show = new newmetricos3
                {
                    TiempoLabel = "-",
                    maquina = inmaq.Key,                  
                    CantidadFallas1= ft1,
                    CantidadFallas2 = ft2,
                    CantidadFallas3 = ft3,
                    TiempoMuerto1 = Math.Round(sumatm1, 2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = 0,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2)
                };
                Ldata3.Add(data_show);
            }

            ViewBag.metricospermachine = Ldata3.ToList();

            var allmaqbyday = allmaq.GroupBy(g => g.TiempoLabel).ToList();

            foreach (var inmaq in allmaqbyday)
            {
                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);
                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);
                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);
                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);
                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);
                var sumatm1 = ((sumtp1/ sumd1) *100);
                var sumatm2 = ((sumtp2 / sumd2) * 100);
                var sumatm3 = ((sumtp3 / sumd3) * 100);
                var smttr11 = inmaq.Sum(s => s.MTTR1);
                var smttr12 = inmaq.Sum(s => s.MTTR2);
                var smttr13 = inmaq.Sum(s => s.MTTR3);
                var sumamttr1 = smttr11 / inmaq.Count();
                var sumamttr2 = smttr12 / inmaq.Count();
                var sumamttr3 = smttr13 / inmaq.Count();
                var sumamtbf = inmaq.Sum(s => s.MTBF) / inmaq.Count();
                // var sumamtbf = ((sumd1 + sumd2 + sumd3) / 1);
                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;
                var sumaMant = ((sumamtbf- sumamttrs ) / sumamtbf)*100;

                //tpm
                int realizada = 0;
                int pendiente = 0;
                var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                {
                    alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                }
                foreach (OILs oil in alloils)
                {
                    if (oil.DiaHora.Year <= fecha.Year)
                        pendiente++;

                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year <= fecha.Year)
                        realizada++;
                }

                var tpmt= Math.Round((realizada / (Double)pendiente) * 100, 2);

                newmetricos2 data_show = new newmetricos2
                {
                    TiempoLabel = inmaq.Key,                  
                    TiempoMuerto1 = Math.Round(sumatm1,2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = tpmt,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2),
                    FallasT1 = Math.Round(ft1, 2),
                    FallasT2 = Math.Round(ft2, 2),
                    FallasT3 = Math.Round(ft3, 2),
                    TiempoM1 = Math.Round(sumtp1, 2),
                    TiempoM2 = Math.Round(sumtp2, 2),
                    TiempoM3 = Math.Round(sumtp3, 2)

                };
                Ldata.Add(data_show);
            }

            // Grafica de tiempo  --------------------------------------------

            string labels = "'";
            string gdata = "";
            string gdata2 = "";
            List<decimal> x = new List<decimal>();
            List<decimal> y = new List<decimal>();
            int j = 0;

            foreach (var h in Ldata)
            {
                labels = labels+ h.TiempoLabel  +"','";

                gdata = gdata+ Convert.ToString(h.TiempoM1 + h.TiempoM2 + h.TiempoM3)+",";

                j++;
                x.Add(j);
                if ((h.TiempoM1 + h.TiempoM2 + h.TiempoM3) != 0)
                    y.Add(Convert.ToDecimal(h.TiempoM1 + h.TiempoM2 + h.TiempoM3));
                else
                    y.Add(0);
            }

            if (x.Count > 1)
            {
                Trendline rvalue = new Trendline(y, x);
                for (int i = 0; i <= x.Count; i++)
                {
                    var tt = i;
                    var res = rvalue.GetYValue(tt);
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }
            }

            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata = gdata.TrimEnd(',', (char)39);
            gdata2 = gdata2.TrimEnd(',', (char)39);
            labels = labels.Replace('-', '_');

            ViewBag.labelsgrap = labels;// Label
            ViewBag.datasgrap = gdata; // Suma de timepo 
            ViewBag.data2sgrap = gdata2;// % acumulado

            // Pareto --------------------------------------------

            var groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5);
            labels = "'";
            gdata = "";
            gdata2 = "";
            int stempt = 0; 
            int stemp = fulldatafiltered.Sum(s => s.Tiempo);
            foreach (var h in groupdata)
            {
                labels = labels + h.Key + "','";
                gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                stemp = stemp + h.Sum(s => s.Tiempo);
                double res = (stemp / (double)stempt) * 100;
                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
            }

            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata.TrimEnd(',', (char)39);
            gdata2.TrimEnd(',', (char)39);

            ViewBag.labelsgrap2 = labels;
            ViewBag.datasgrap2 = gdata;
            ViewBag.data2sgrap2 = gdata2;

            //--------------------------------------------

            //*******************************************************************************************
            if (btn == "Metricos por Años")
            {

                 labels = "'";
                 gdata = "";
                 gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                // datafiltered = data;
                var datatempa = fulldatafiltered.GroupBy(g => g.DiaHora.Year).ToList();
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.Maquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;

                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;


                 stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                 stemp = 0;
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
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
                datafiltered.Clear();
                 labels = "'";
                 gdata = "";
                 gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();

                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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

                            var temp = dataaño.Where(w => w.DiaHora.Year == iaño && w.DiaHora.Month == jmes);
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                 groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;

                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

                 stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                 stemp = 0;
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                 nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                 nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************

            if (btn == "Metricos por Dia")
            {
                datafiltered.Clear();
                 labels = "'";
                 gdata = "";
                 gdata2 = "";
                // List<decimal> x = new List<decimal>();
                // List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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
                        var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                    labels = "'";
                    gdata = "";
                    gdata2 = "";

                    List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                    int exit = 0;
                    int eltiempo = 0;
                    List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                    foreach (var h in groupdata)
                    {
                        newmetricosmaquina nmm = new newmetricosmaquina();
                        eltiempo += h.Sum(s => s.Tiempo);
                        if (exit < 5)
                        {
                            hdata.Add(h);
                            nmm.tiempod = multiplicador;
                            nmm.maquina = h.Key;
                            nmm.tiempof = h.Sum(s => s.Tiempo);
                            nmm.fallas = h.Count();
                            nmm.mttr = nmm.tiempof / nmm.fallas;
                            nmm.mtbf = multiplicador / nmm.fallas;
                            var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                            nmm.confiabilidad = disponibilidad;
                            lnmm.Add(nmm);
                        }
                        exit++;

                    }
                    ViewBag.lnmm = lnmm;

                    var objet = "'";
                    var dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mttr.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmttr = objet;
                    ViewBag.Datamttr = dat;
                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.tiempof.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelstf = objet;
                    ViewBag.Datatf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.fallas.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsf = objet;
                    ViewBag.Dataf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mtbf.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmtbf = objet;
                    ViewBag.Datamtbf = dat;

                     stempt = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        stempt += hdata[di].Sum(s => s.Tiempo);

                    }

                     stemp = 0;
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;

                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                    ViewBag.dxx = " Dia";
                }

            }





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
            //--------------------------------------------
            ViewBag.data = Ldata;

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View("Metricos3", fulldatafiltered);
        }
        public ActionResult Metricos4(string amaquina, string cliente, string grupo, string maquina, string submaquina, string mgroup, string xmgroup, string btn = "Metricos por Dia", string dti = "", string dtf = "")
        {
            var fulldatafiltered = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var datafiltered1 = new List<Bitacora>();
            var datafiltered2 = new List<Bitacora>();
            var datafiltered3 = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<newmetricos2> Ldata = new List<newmetricos2>();
            List<newmetricos3> Ldata3 = new List<newmetricos3>();
            var fecha = DateTime.Now.AddDays(-2);
            var fechaf = DateTime.Now;
            DateTimeFormatInfo formatoFecha1 = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes1 = formatoFecha1.GetMonthName(fecha.Month);
            string nombreMes2 = formatoFecha1.GetMonthName(fechaf.Month);

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

            ViewBag.xmgroup = mindia.ToString();
            var dataf = db.Bitacoras.Where(w => (w.DiaHora.Year >= fecha.Year) &&(w.DiaHora.Year <= fechaf.Year) &&(w.Tiempo > 0)&&(w.Maquinas.Critica==true)
                            );
            
            if (!string.IsNullOrEmpty(maquina))
                dataf = dataf.Where(m => m.Maquinas.Maquina == maquina);

            if (!string.IsNullOrEmpty(cliente))
                dataf = dataf.Where(m => m.Maquinas.Cliente == cliente);

            if (!string.IsNullOrEmpty(grupo))
                dataf = dataf.Where(m => m.Maquinas.Grupo == grupo);

            if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("MetalFinish"))
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                    var dtdt = dataf.ToList();
                }
                else
                {

                    if (amaquina == "Cromo")
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                    }

                    if (amaquina.Contains("Pintura"))
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                    }

                    if (amaquina.Contains("Soldadura"))
                    {
                        amaquina = "Soldadura";
                    }


                    dataf = dataf.Where(s => s.Maquinas.Area.Contains(amaquina));
                }
            }


            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            string[] array = { "Maquina", "Falla", "Area", "SubMaquina" };
            ViewBag.mgroup = new SelectList(array);

            var maquinas = db.Maquinas.Where(m => m.Critica ==true);


            if (!string.IsNullOrEmpty(amaquina))
                if (amaquina.Contains("MetalFinish"))
                {
                    maquinas = maquinas.Where(m => m.Area == "Cromo" || m.Area == "Cromo1" || m.Area == "Cromo2" || m.Area == "AutoPulido1" || m.Area == "AutoPulido2" || m.Area == "Pintura" || m.Area == "Ecoat" || m.Area == "Topcoat" || m.Area == "MetalFinish");

                }
                else {
                    if (amaquina.Contains("Soldadura"))
                    {
                        amaquina = "Soldadura";
                        maquinas = maquinas.Where(m => m.Area == amaquina);
                    }
                    else { 
                    maquinas = maquinas.Where(m => m.Area == amaquina);
                    }
                }
            //  maquinas = maquinas.Where(m => m.Area == amaquina);
            if (!string.IsNullOrEmpty(cliente))
                maquinas = maquinas.Where(m => m.Cliente==cliente);

            if (!string.IsNullOrEmpty(grupo))
                maquinas = maquinas.Where(m => m.Grupo == grupo);

            if (!string.IsNullOrEmpty(maquina))
                maquinas = maquinas.Where(m => m.Maquina == maquina);

            var maquis = maquinas.GroupBy(g => g.Maquina).ToList();

            var mul_maquinas = maquis.Count();


            ViewBag.cliente = new SelectList(maquinas.GroupBy(g => g.Cliente), "Key", "Key");

            ViewBag.grupo = new SelectList(maquinas.GroupBy(g => g.Grupo), "Key", "Key");

            ViewBag.maquina = new SelectList(maquinas.GroupBy(g => g.Maquina), "Key", "Key");

            ViewBag.submaquina = new SelectList(maquinas, "ID", "SubMaquina");

            List<newmetricos3> allmaq = new List<newmetricos3>();


            for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
            {

                for (int jmes = 1; jmes <= 12; jmes++)
                {
                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                    string nombreMes = formatoFecha.GetMonthName(jmes);
                    var dias = DateTime.DaysInMonth(iaño, jmes);

                    for (int kdia = 1; kdia <= dias; kdia++)
                    {
                        DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                        DateTime idi3er = idi.AddDays(+1);

                        if (idi >= fecha && idi <= fechaf)
                        {

                            //datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Day != 1).ToList();
                            datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Hour > 7).ToList();
                            //datafiltered.AddRange(temp1);
                            var temp2 = dataf.Where(w => w.DiaHora.Year == idi3er.Year && w.DiaHora.Month == idi3er.Month && w.DiaHora.Day == idi3er.Day && w.DiaHora.Hour <= 7).ToList();
                            datafiltered.AddRange(temp2);



                            datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();
                            datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();
                            datafiltered3 = datafiltered.Where(s => (s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)).ToList();
                            fulldatafiltered.AddRange(datafiltered);

                            foreach (var simplemaquina in maquis)
                            {


                                string thistiempo = "";
                                if (btn == "Metricos por Dia")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString();
                                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                                    ViewBag.dxx = " Dia";
                                }

                                if (btn == "Metricos por Mes")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes;
                                    ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                                    ViewBag.dxx = " Mes";
                                }
                                if (btn == "Metricos por Años")
                                {
                                    thistiempo = iaño.ToString();
                                    ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                                    ViewBag.dxx = " Anual";
                                }


                                newmetricos3 simplemaquinam = new newmetricos3
                                {
                                    TiempoLabel = thistiempo,
                                    maquina = simplemaquina.Key,
                                    Disponible1 = (Double)multiplicador,
                                    Disponible2 = (Double)multiplicador,
                                    Disponible3 = (Double)multiplicador,
                                    CantidadFallas1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    TiempoMuerto1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    MTTR1 = 0,
                                    MTTR2 = 0,
                                    MTTR3 = 0,
                                    MTBF = (Double)multiplicador * 3,
                                    Confiabilidad = 100,
                                    TarjetasTPM = 0
                                };


                            

                                if (simplemaquinam.CantidadFallas1 != 0)
                                {
                                    simplemaquinam.MTTR1 = simplemaquinam.TiempoMuerto1 / simplemaquinam.CantidadFallas1;
                                }
                                if (simplemaquinam.CantidadFallas2 != 0)
                                {
                                    simplemaquinam.MTTR2 = simplemaquinam.TiempoMuerto2 / simplemaquinam.CantidadFallas2;
                                }
                                if (simplemaquinam.CantidadFallas3 != 0)
                                {
                                    simplemaquinam.MTTR3 = simplemaquinam.TiempoMuerto3 / simplemaquinam.CantidadFallas3;
                                }

                                var SumDisponobilidad = simplemaquinam.Disponible1 + simplemaquinam.Disponible2 + simplemaquinam.Disponible3;
                                simplemaquinam.MTBF = SumDisponobilidad;
                                if (simplemaquinam.CantidadFallas1 != 0 || simplemaquinam.CantidadFallas2 != 0 || simplemaquinam.CantidadFallas3 != 0)
                                {
                                    var sumCantidadFallas = simplemaquinam.CantidadFallas1 + simplemaquinam.CantidadFallas2 + simplemaquinam.CantidadFallas3;
                                    var sumTiempoMuerto = simplemaquinam.TiempoMuerto1 + simplemaquinam.TiempoMuerto2 + simplemaquinam.TiempoMuerto3;
                                    var MTTR = (simplemaquinam.MTTR1 + simplemaquinam.MTTR2 + simplemaquinam.MTTR3) / 3;
                                    simplemaquinam.MTBF = SumDisponobilidad / sumCantidadFallas;
                                    //  simplemaquinam.Confiabilidad = simplemaquinam.MTBF / (simplemaquinam.MTBF + MTTR);
                                    simplemaquinam.Confiabilidad = (simplemaquinam.MTBF - MTTR) / simplemaquinam.MTBF;
                                }

                                allmaq.Add(simplemaquinam);
                            }




                        }

                    }


                }

            }

            var allmaqbyday1 = allmaq.GroupBy(g => g.maquina).ToList();

            foreach (var inmaq in allmaqbyday1)
            {

                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);

                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);



                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);

                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);

                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);

                var sumatm1 = sumtp1;// ((sumtp1 / sumd1) * 100);

                var sumatm2 = sumtp2;// ((sumtp2 / sumd2) * 100);

                var sumatm3 = sumtp3;// ((sumtp3 / sumd3) * 100);

                var smttr11 = inmaq.Sum(s => s.MTTR1);

                var smttr12 = inmaq.Sum(s => s.MTTR2);

                var smttr13 = inmaq.Sum(s => s.MTTR3);

                var sumamttr1 = sumatm1 / ft1;

                var sumamttr2 = sumatm2 / ft2;

                var sumamttr3 = sumatm3 / ft3;

                var sumamtbf = (sumd1 + sumd2 + sumd3) / (ft1 + ft2 + ft3); //inmaq.Sum(s => s.MTBF) / inmaq.Count();

                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;

                var sumaMant = ((sumamtbf - sumamttrs) / sumamtbf) * 100;



                newmetricos3 data_show = new newmetricos3
                {
                    TiempoLabel = "-",
                    maquina = inmaq.Key,
                    CantidadFallas1 = ft1,
                    CantidadFallas2 = ft2,
                    CantidadFallas3 = ft3,
                    TiempoMuerto1 = Math.Round(sumatm1, 2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = 0,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2)


                };
                Ldata3.Add(data_show);
            }

            ViewBag.metricospermachine = Ldata3.ToList();

            var allmaqbyday = allmaq.GroupBy(g => g.TiempoLabel).ToList();

            foreach (var inmaq in allmaqbyday)
            {

                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);

                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);



                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);
                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);
                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);


                var sumatm1 = ((sumtp1 / sumd1) * 100);

                var sumatm2 = ((sumtp2 / sumd2) * 100);

                var sumatm3 = ((sumtp3 / sumd3) * 100);


                var smttr11 = inmaq.Sum(s => s.MTTR1);

                var smttr12 = inmaq.Sum(s => s.MTTR2);

                var smttr13 = inmaq.Sum(s => s.MTTR3);


                var sumamttr1 = smttr11 / inmaq.Count();
                var sumamttr2 = smttr12 / inmaq.Count();
                var sumamttr3 = smttr13 / inmaq.Count();


                var sumamtbf = inmaq.Sum(s => s.MTBF) / inmaq.Count();
                // var sumamtbf = ((sumd1 + sumd2 + sumd3) / 1);

                //       if ((ft1 != 0 || ft2 !=0 || ft3 != 0))
                //            sumamtbf = ((sumd1 + sumd2 + sumd3) / (ft1 + ft2 + ft3));


                //  var sumaMant = inmaq.Sum(s => s.Confiabilidad) / inmaq.Count();

                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;

                var sumaMant = ((sumamtbf - sumamttrs) / sumamtbf) * 100;

                //tpm
                int realizada = 0;
                int pendiente = 0;
                var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                {
                    alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                }
                foreach (OILs oil in alloils)
                {


                    if (oil.DiaHora.Year <= fecha.Year)
                        pendiente++;

                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year <= fecha.Year)
                        realizada++;


                }


                var tpmt = Math.Round((realizada / (Double)pendiente) * 100, 2);

                //ENtpm

                /*
                                 var sumamttr1 = sumtp1 / ft1;

                var sumamttr2 = sumtp2 / ft2;

                var sumamttr3 = sumtp3 / ft3;

                var sumamtbf = ((sumd1+ sumd2 + sumd3)  / (ft1+ ft2+ ft3));
                
                 
                var sumamttr1 = 0.0;
                if (ft1 != 0)
                 sumamttr1 = smttr11 / ft1;

                var sumamttr2 = 0.0;
                if (ft2 != 0)
                     sumamttr2 = sumd2 / ft2;

                var sumamttr3 = 0.0;
                if (ft3 != 0)
                     sumamttr3 = sumd3 / ft3;

                var sumamttrs = sumamttr1 + sumamttr2+sumamttr3;
                 
                 
                 */





                newmetricos2 data_show = new newmetricos2
                {
                    TiempoLabel = inmaq.Key,
                    TiempoMuerto1 = Math.Round(sumatm1, 2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = tpmt,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2),
                    FallasT1 = Math.Round(ft1, 2),
                    FallasT2 = Math.Round(ft2, 2),
                    FallasT3 = Math.Round(ft3, 2),
                    TiempoM1 = Math.Round(sumtp1, 2),
                    TiempoM2 = Math.Round(sumtp2, 2),
                    TiempoM3 = Math.Round(sumtp3, 2)

                };
                Ldata.Add(data_show);
            }

            // Grafica de tiempo  --------------------------------------------

            string labels = "'";
            string gdata = "";
            string gdata2 = "";
            List<decimal> x = new List<decimal>();
            List<decimal> y = new List<decimal>();
            int j = 0;
            foreach (var h in Ldata)
            {
                labels = labels + h.TiempoLabel + "','";

                gdata = gdata + Convert.ToString(h.TiempoM1 + h.TiempoM2 + h.TiempoM3) + ",";

                j++;
                x.Add(j);
                if ((h.TiempoM1 + h.TiempoM2 + h.TiempoM3) != 0)
                    y.Add(Convert.ToDecimal(h.TiempoM1 + h.TiempoM2 + h.TiempoM3));
                else
                    y.Add(0);
            }

            if (x.Count > 1)
            {
                Trendline rvalue = new Trendline(y, x);
                for (int i = 0; i <= x.Count; i++)
                {
                    var tt = i;
                    var res = rvalue.GetYValue(tt);
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }
            }


            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata = gdata.TrimEnd(',', (char)39);
            gdata2 = gdata2.TrimEnd(',', (char)39);


            labels = labels.Replace('-', '_');


            ViewBag.labelsgrap = labels;// Label
            ViewBag.datasgrap = gdata; // Suma de timepo 
            ViewBag.data2sgrap = gdata2;// % acumulado

            // Pareto --------------------------------------------

            var groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5);
            labels = "'";
            gdata = "";
            gdata2 = "";
            int stempt = 0;
            int stemp = fulldatafiltered.Sum(s => s.Tiempo);
            foreach (var h in groupdata)
            {
                labels = labels + h.Key + "','";
                gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                stemp = stemp + h.Sum(s => s.Tiempo);
                double res = (stemp / (double)stempt) * 100;
                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
            }

            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata.TrimEnd(',', (char)39);
            gdata2.TrimEnd(',', (char)39);

            ViewBag.labelsgrap2 = labels;
            ViewBag.datasgrap2 = gdata;
            ViewBag.data2sgrap2 = gdata2;

            //--------------------------------------------

            //*******************************************************************************************
            if (btn == "Metricos por Años")
            {

                labels = "'";
                gdata = "";
                gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                // datafiltered = data;
                var datatempa = fulldatafiltered.GroupBy(g => g.DiaHora.Year).ToList();
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.Maquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;

                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;


                stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                stemp = 0;
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
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
                datafiltered.Clear();
                labels = "'";
                gdata = "";
                gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();

                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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

                            var temp = dataaño.Where(w => w.DiaHora.Year == iaño && w.DiaHora.Month == jmes);
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;

                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

                stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                stemp = 0;
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
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************

            if (btn == "Metricos por Dia")
            {
                datafiltered.Clear();
                labels = "'";
                gdata = "";
                gdata2 = "";
                // List<decimal> x = new List<decimal>();
                // List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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
                        var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                    labels = "'";
                    gdata = "";
                    gdata2 = "";

                    List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                    int exit = 0;
                    int eltiempo = 0;
                    List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                    foreach (var h in groupdata)
                    {
                        newmetricosmaquina nmm = new newmetricosmaquina();
                        eltiempo += h.Sum(s => s.Tiempo);
                        if (exit < 5)
                        {
                            hdata.Add(h);
                            nmm.tiempod = multiplicador;
                            nmm.maquina = h.Key;
                            nmm.tiempof = h.Sum(s => s.Tiempo);
                            nmm.fallas = h.Count();
                            nmm.mttr = nmm.tiempof / nmm.fallas;
                            nmm.mtbf = multiplicador / nmm.fallas;
                            var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                            nmm.confiabilidad = disponibilidad;
                            lnmm.Add(nmm);
                        }
                        exit++;

                    }
                    ViewBag.lnmm = lnmm;

                    var objet = "'";
                    var dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mttr.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmttr = objet;
                    ViewBag.Datamttr = dat;
                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.tiempof.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelstf = objet;
                    ViewBag.Datatf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.fallas.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsf = objet;
                    ViewBag.Dataf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mtbf.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmtbf = objet;
                    ViewBag.Datamtbf = dat;

                    stempt = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        stempt += hdata[di].Sum(s => s.Tiempo);

                    }

                    stemp = 0;
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
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;

                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                    ViewBag.dxx = " Dia";
                }

            }





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
            //--------------------------------------------
            ViewBag.data = Ldata;

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View("Metricos4", fulldatafiltered);
        }

        public static T Clamp<T>(T value, T min, T max)
        where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using ClosedXML.Excel;
using ClosedXML.Extensions;
using ClosedXML.Report;

namespace Flex_SGM.Controllers
{
    public class CReclamosController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public FileResult ExportFormat(int? id)
        {


            CReclamos reo = db.reo.Find(id);


            var user = User.Identity;

            //******************************
            var templatepath=Server.MapPath($"~/Evidence/Quality/Template.xlsx");

            var template = new XLTemplate(templatepath);

            template tempy = new template
            {
                tiporeclamo=reo.tipo,
                fecha=reo.Fecha.ToShortDateString(),
                reportecliente=reo.NoReclamoCliente,
                reportefng=reo.NoReclamoFNG,
                area=reo.Area,
                noparte=reo.NoParte,
                cliente=reo.Cliente,
                lugar=reo.Planta,
                costo=reo.costo,
                descripcion=reo.Descripcionpz,
                cantidad=reo.Cantidadpz,
                descripcion2=reo.Defecto
            };
            try { 
               var path1 = Server.MapPath($"~/Evidence/Quality/Reclamos/before/{id}");

               var path2 = Server.MapPath($"~/Evidence/Quality/Reclamos/after/{id}");
            DirectoryInfo Folder;
            Folder = new DirectoryInfo(path1);
            FileInfo[]  Images1 = Folder.GetFiles();
            Folder = new DirectoryInfo(path2);
            FileInfo[] Images2 = Folder.GetFiles();

            var ws = template.Workbook.Worksheet("Data");
            Images1= Images1.Where(w => w.Name.ToLower().Contains(".jpeg") || w.Name.ToLower().Contains(".jpg")).ToArray();
            Images2 = Images2.Where(w => w.Name.ToLower().Contains(".jpeg") || w.Name.ToLower().Contains(".jpg")).ToArray();
            if (Images1.Count() > 0) { 
            var image1 = ws.AddPicture(Images1.FirstOrDefault().FullName)
                .MoveTo(ws.Cell("A17"))
            .Scale(0.35); // optional: resize picture
            }

            if (Images2.Count() > 0)
            {
                var image2 = ws.AddPicture(Images2.FirstOrDefault().FullName)
    .MoveTo(ws.Cell("C17"))
    .Scale(0.35); // optional: resize picture



            }
            }
            catch (Exception ex) { }

            template.AddVariable(tempy);
            template.Generate();
            using (MemoryStream stream = new MemoryStream())
            {
                template.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Reclamo_" + reo.NoReclamoFNG + ".xlsx");
            }


        }

        // GET: CReclamos
        public ActionResult Index(string[] tipor,string btn = "Por Mes", string dti = "", string dtf = "" , string paret = "Cliente")
        {
            var data = new List<CReclamos>();
            var datafiltered = new List<CReclamos>();
            var fecha = DateTime.Now.AddMonths(-6);
            var fechaf = DateTime.Now;

            string[] array = { "Todos", "Oficial", "No Oficial", "FNG", "Nissan", "Mopar", "FCA", "GM", "ILN", "Ford" };
            ViewBag.tipor = new SelectList(array);

            string[] array2 = { "Por Mes", "Por Años", "Por Dia"};
            ViewBag.btn = new SelectList(array2);

            string[] array3 = { "Cliente", "Proceso", "Defecto", "No.Parte", "Ing.Calidad", "Planta", "Estatus PLM" };
            ViewBag.paret = new SelectList(array3);

            if (!string.IsNullOrEmpty(dti))
            {
                fecha = Convert.ToDateTime(dti);
            }
            if (!string.IsNullOrEmpty(dtf))
            {
                fechaf = Convert.ToDateTime(dtf);
            }
            //---data---

            data = db.reo.Where(w => (w.Fecha.Year >= fecha.Year) &&
                                      (w.Fecha.Year <= fechaf.Year)
                                     ).ToList();
            List<CReclamos> rev = new List<CReclamos>();
            if(tipor ==null)
                tipor=new string[] { "Todos" };
            List<CReclamos> dtemp1 =new List<CReclamos>();

            foreach ( var item in tipor) {

                if (item == "Oficial")
                    data = data.Where(w => w.Oficial == true).ToList();
                if (item == "No Oficial")
                    data = data.Where(w => w.Oficial == false).ToList();

                if (item.Contains("FNG"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("FNG")).ToList());
                if (item.Contains("Nissan"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("Nissan")).ToList());
                if (item.Contains("Mopar"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("Mopar")).ToList());
                if (item.Contains("FCA"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("FCA")).ToList());
                if (item.Contains("GM"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("GM")).ToList());
                if (item.Contains("ILN"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("ILN")).ToList());
                if (item.Contains("Ford"))
                    dtemp1.AddRange(data.Where(w => w.Cliente.Contains("Ford")).ToList());

                /*
                 * 
                 *             if (item == "Oficial")
                        dtemp1.AddRange( data.Where(w => w.Oficial == true).ToList());
                if (item == "No Oficial")
                        dtemp1.AddRange(data.Where(w => w.Oficial == false).ToList());

                if (item.Contains("FNG"))
                         dtemp1.AddRange( data.Where(w => w.Cliente.Contains("FNG")).ToList());
                if (item.Contains("Nissan"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("Nissan")).ToList());
                if (item.Contains("Mopar"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("Mopar")).ToList());
                if (item.Contains("FCA"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("FCA")).ToList());
                if (item.Contains("GM"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("GM")).ToList());
                if (item.Contains("ILN"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("ILN")).ToList());
                if (item.Contains("Ford"))
                         dtemp1.AddRange( data.Where(w =>  w.Cliente.Contains("Ford")).ToList());


                }
                  */
            }
            if (dtemp1.Count != 0)
                data = dtemp1;
            //*******************************************************************************************
            if (btn == "Por Años")
            {

                string labels = "'";
                string gdata = "";
                string gdata2 = "";
                List<decimal> x = new List<decimal>();
                List<decimal> y = new List<decimal>();
                datafiltered = data;
                var datatempa = data.GroupBy(g => g.Fecha.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();

                for (int i = fecha.Year; i <= fechaf.Year; i++)
                {
                    var dd = datatempa.Where(w => w.Key == i).ToList();
                    labels = labels + i.ToString() + "','";
                    int Count = 0;
                    if (dd.FirstOrDefault() != null)
                    {
                        Count = dd.FirstOrDefault().Count();
                    }
                    gdata = gdata + Count.ToString()+ ",";
                    x.Add(i);
                    y.Add(Count);

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
                if (paret == "Cliente")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Cliente).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    int stempt = datafiltered.Count();
                    int stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Proceso")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Proceso).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Defecto")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Defecto).OrderByDescending(k => k.Count()).Take(10);
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "No.Parte")
                {
                    var groupdata = datafiltered.GroupBy(g => g.NoParte).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Ing.Calidad")
                {
                    var groupdata = datafiltered.GroupBy(g => g.IngCalidad).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Planta")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Planta).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Estatus PLM")
                {
                    var groupdata = datafiltered.GroupBy(g => g.PLM).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }

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
                var datatempa = data.GroupBy(g => g.Fecha.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = data.Where(w => w.Fecha.Year == iaño);
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
                            var temp = dataaño.Where(w => w.Fecha.Month == jmes);
                            datafiltered.AddRange(temp);
                            gdata = gdata + temp.Count().ToString() + ",";
                            x.Add(i);
                            y.Add(temp.Count());
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
                if (paret == "Cliente")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Cliente).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    int stempt = datafiltered.Count();
                    int stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Proceso")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Proceso).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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
                }
                //--------------------------------------------------------
                if (paret == "Defecto")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Defecto).OrderByDescending(k => k.Count()).Take(10);
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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

                }
                //--------------------------------------------------------
                if (paret == "No.Parte")
                {
                    var groupdata = datafiltered.GroupBy(g => g.NoParte).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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

                }
                //--------------------------------------------------------
                if (paret == "Ing.Calidad")
                {
                    var groupdata = datafiltered.GroupBy(g => g.IngCalidad).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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

                }
                //--------------------------------------------------------
                if (paret == "Planta")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Planta).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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

                }
                //--------------------------------------------------------
                if (paret == "Estatus PLM")
                {
                    var groupdata = datafiltered.GroupBy(g => g.PLM).OrderByDescending(k => k.Count());
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        labels = labels + h.Key + "','";
                        gdata = gdata + h.Count().ToString() + ",";
                        stemp = stemp + h.Count();
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

                }

                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                string nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + "/" + fecha.Year.ToString() + " al Mes " + nombreMes2 + "/" + fechaf.Year.ToString();
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
                    var dataaño = data.Where(w => w.Fecha.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);
                        var datames = dataaño.Where(w => w.Fecha.Month == jmes);
                        for (int kdia = 1; kdia <= dias; kdia++)
                        {
                            DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                            if (idi >= fecha && idi <= fechaf)
                            {
                                labels = labels + iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString() + "','";
                                var temp = datames.Where(w => w.Fecha.Day == kdia);
                                datafiltered.AddRange(temp);
                                gdata = gdata + temp.Count().ToString() + ",";
                                x.Add(i);
                                y.Add(temp.Count());
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
                        var dataaño = data.Where(w => w.Fecha.Year == iaño);
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                            string nombreMes = formatoFecha.GetMonthName(jmes);
                            var dias = DateTime.DaysInMonth(iaño, jmes);
                            var datames = dataaño.Where(w => w.Fecha.Month == jmes);
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
                    if (paret == "Cliente")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Cliente).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        int stempt = datafiltered.Count();
                        int stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "Proceso")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Proceso).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "Defecto")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Defecto).OrderByDescending(k => k.Count()).Take(10);
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "No.Parte")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.NoParte).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "Ing.Calidad")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.IngCalidad).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "Planta")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Planta).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }
                    //--------------------------------------------------------
                    if (paret == "Estatus PLM")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.PLM).OrderByDescending(k => k.Count());
                        labels = "'";
                        gdata = "";
                        gdata2 = "";
                        var stempt = datafiltered.Count();
                        var stemp = 0;
                        foreach (var h in groupdata)
                        {
                            labels = labels + h.Key + "','";
                            gdata = gdata + h.Count().ToString() + ",";
                            stemp = stemp + h.Count();
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
                    }

                    ViewBag.dx = "Dia " + fecha.ToString("dd/MM/yyyy") + " al Dia " + fechaf.ToString("dd/MM/yyyy");
                    ViewBag.dxx = " Dia";
                }

            }

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View(datafiltered);
        }
        // GET: CReclamos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CReclamos reo = db.reo.Find(id);
            if (reo == null)
            {
                return HttpNotFound();
            }
            return View(reo);
        }

        // GET: CReclamos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CReclamos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Fecha,Oficial,IngCalidad,NoReclamoCliente,NoReclamoFNG,NoParte,Defecto,Cliente,Planta,PLM,Cantidadpz,Proceso,Comentarios,a1d,a5d,a10d,a20d,a30d,costo,Descripcionpz")] CReclamos reo)
        {
            if (ModelState.IsValid)
            {

                db.reo.Add(reo);
                db.SaveChanges();
                TempData["Section"] = "Evidence";
                return RedirectToAction("Edit", new { id = reo.ID });
            }

            return View(reo);
        }
        [HttpPost]
        public ActionResult Save([Bind(Include = "ID,Fecha,Oficial,IngCalidad,NoReclamoCliente,NoReclamoFNG,NoParte,Defecto,Cliente,Planta,PLM,Cantidadpz,Proceso,Comentarios,a1d,a5d,a10d,a20d,a30d,costo,Descripcionpz")] CReclamos reo)
        {
            if (reo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // CReclamos reo =db.reo.Find(id);
            if (reo == null)
            {
                return HttpNotFound();
            }
            return View(reo);
        }
        // GET: CReclamos/Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.Section = TempData["Section"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CReclamos reo = db.reo.Find(id);
            if (reo == null)
            {
                return HttpNotFound();
            }
            return View(reo);
        }

        // POST: CReclamos/Edit
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Fecha,Oficial,IngCalidad,NoReclamoCliente,NoReclamoFNG,NoParte,Defecto,Cliente,Planta,PLM,Cantidadpz,Proceso,Comentarios,a1d,a5d,a10d,a20d,a30d,costo,Descripcionpz")] CReclamos reo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reo);
        }

        [HttpPost]
        public ActionResult SaveFile(int id, string t)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var path = "cool";
                    if (t == "a")
                        path = Server.MapPath($"~/Evidence/Quality/Reclamos/before/{id}");

                    if (t == "d")
                        path = Server.MapPath($"~/Evidence/Quality/Reclamos/after/{id}");

                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    path = Path.Combine(path, file.FileName);
                    file.SaveAs(path);
                    return Json(true);
                }
                else
                    return Json(false);
            }
            else
                return Json(false);

        }
        public FileResult deleteFile(string link)
        {

            string fileName2 = Server.MapPath("~/Evidence/Delete");
            string fileName = "";
            link = link.Replace(@"../../", "");
            fileName2 = fileName2 + link;
            string dirName2 = Path.GetDirectoryName(fileName2);
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            if (!System.IO.Directory.Exists(dirName2))
                System.IO.Directory.CreateDirectory(dirName2);

            System.IO.File.Copy(link, fileName2);
            System.IO.File.Delete(link);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);



        }
        public FileResult Download(string link)
        {
            string fileName = "";
            link = link.Replace(@"../../", "");
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        // GET: CReclamos/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CReclamos reo = db.reo.Find(id);
            if (reo == null)
            {
                return HttpNotFound();
            }
            return View(reo);
        }

        // POST: CReclamos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CReclamos reo = db.reo.Find(id);
            db.reo.Remove(reo);
            db.SaveChanges();
            return RedirectToAction("Index");
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

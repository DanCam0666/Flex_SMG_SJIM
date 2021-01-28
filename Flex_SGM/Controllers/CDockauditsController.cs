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
using ClosedXML.Report;
using Flex_SGM.Models;

namespace Flex_SGM.Controllers
{
    public class CDockauditsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CDockaudits
        public ActionResult Index(string btn = "Por Mes", string dti = "", string dtf = "", string paret = "Cliente")
        {

            var data = new List<CDockaudit>();
            var datafiltered = new List<CDockaudit>();
            var fecha = DateTime.Now.AddMonths(-1);
            var fechaf = DateTime.Now;
            string[] array = { "Cliente", "Proceso", "Descripcion", "Clasificacion", "No.Parte", "Auditor", "Supervisor" };
            ViewBag.paret = new SelectList(array);
            string[] array2 = { "Por Mes", "Por Años", "Por Dia"};
            ViewBag.btn = new SelectList(array2);
            if (!string.IsNullOrEmpty(dti))
            {
                fecha = Convert.ToDateTime(dti);
            }
            if (!string.IsNullOrEmpty(dtf))
            {
                fechaf = Convert.ToDateTime(dtf);
            }
            //---data---

            data = db.CDockaudits.Where(w => (w.Fecha.Year >= fecha.Year) &&
                                      (w.Fecha.Year <= fechaf.Year)
                                     ).ToList();
            /*  if (tipor == "Oficial"){ "Cliente", "Proceso", "Defecto", "No.Parte", "Auditor", "Supervisor" };
                  data = data.Where(w => w.Oficial = true).ToList();
              if (tipor == "No Oficial")
                  data = data.Where(w => w.Oficial = false).ToList();*/
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
                    gdata = gdata + Count.ToString() + ",";
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
                    var groupdata = datafiltered.GroupBy(g => g.cliente).OrderByDescending(k => k.Count()).ToList();
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
                    var groupdata = datafiltered.GroupBy(g => g.AreaOrigen).OrderByDescending(k => k.Count()).ToList();
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
                if (paret == "Descripcion")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Descripcion).OrderByDescending(k => k.Count());
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
                if (paret == "Clasificacion")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Clasificacion).OrderByDescending(k => k.Count());
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
                    var groupdata = datafiltered.GroupBy(g => g.NoDeParte).OrderByDescending(k => k.Count()).ToList();
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
                if (paret == "Auditor")
                {
                    var groupdata = datafiltered.GroupBy(g => g.AuditorReporto).OrderByDescending(k => k.Count()).ToList();
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
                if (paret == "Supervisor")
                {
                    var groupdata = datafiltered.GroupBy(g => g.SupOrigen).OrderByDescending(k => k.Count()).ToList();
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var  stempt = datafiltered.Count();
                    var  stemp = 0;
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
                    var groupdata = datafiltered.GroupBy(g => g.cliente).OrderByDescending(k => k.Count());
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
                   var groupdata = datafiltered.GroupBy(g => g.AreaOrigen).OrderByDescending(k => k.Count());
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
                if (paret == "Descripcion")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Descripcion).OrderByDescending(k => k.Count()).Take(10);
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        if (h.Key.Length>20)
                        labels = labels + h.Key.Substring(0, 20) + "','";
                        else
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
                if (paret == "Clasificacion")
                {
                    var groupdata = datafiltered.GroupBy(g => g.Clasificacion).OrderByDescending(k => k.Count()).Take(10);
                    labels = "'";
                    gdata = "";
                    gdata2 = "";
                    var stempt = datafiltered.Count();
                    var stemp = 0;
                    foreach (var h in groupdata)
                    {
                        if (h.Key.Length > 20)
                            labels = labels + h.Key.Substring(0, 20) + "','";
                        else
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
                    labels = labels.Replace("\r\n","");
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;
                }
                //--------------------------------------------------------
                if (paret == "No.Parte")
                {
                    var groupdata = datafiltered.GroupBy(g => g.NoDeParte).OrderByDescending(k => k.Count()).Take(10); 
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
                if (paret == "Auditor")
                {
                    var groupdata = datafiltered.GroupBy(g => g.AuditorReporto).OrderByDescending(k => k.Count());
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
                if (paret == "Supervisor")
                {
                    var groupdata = datafiltered.GroupBy(g => g.SupOrigen).OrderByDescending(k => k.Count());
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
                ViewBag.dx = "Mes " + nombreMes1 +"/"+fecha.Year.ToString()+ " al Mes " + nombreMes2 + "/" + fechaf.Year.ToString();
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
                        var groupdata = datafiltered.GroupBy(g => g.cliente).OrderByDescending(k => k.Count());
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
                        var groupdata = datafiltered.GroupBy(g => g.AreaOrigen).OrderByDescending(k => k.Count());
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
                    if (paret == "Descripcion")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Descripcion).OrderByDescending(k => k.Count());
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
                    if (paret == "Clasificacion")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.Clasificacion).OrderByDescending(k => k.Count());
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
                        var groupdata = datafiltered.GroupBy(g => g.NoDeParte).OrderByDescending(k => k.Count());
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
                    if (paret == "Auditor")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.AuditorReporto).OrderByDescending(k => k.Count());
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
                    if (paret == "Supervisor")
                    {
                        var groupdata = datafiltered.GroupBy(g => g.SupOrigen).OrderByDescending(k => k.Count());
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
            //  return View(db.CDockaudits.ToList());
        }


        public FileResult ExportFormat(int? id)
        {


            CDockaudit reo = db.CDockaudits.Find(id);


            var user = User.Identity;

            //******************************
            var templatepath = Server.MapPath($"~/Evidence/Quality/TemplateDock.xlsx");

            var template = new XLTemplate(templatepath);

            templatedock tempy = new templatedock
            {
                fecha = reo.Fecha.ToShortDateString(),
                cliente = reo.cliente,
         noreporte = reo.NoDockAudit,
         noparte = reo.NoDeParte,
         descripcionparte = reo.Descripcion,
         modelo = reo.NoDeParte,
         donde = reo.AreaOrigen,
         cuadofecha = reo.Fecha.ToShortDateString(),
                que = reo.Clasificacion,
         que2 = reo.Comentarios,
         supervisor = reo.SupOrigen,
         lote = reo.lote,
         cantidad = reo.Cantidad,
         reporto = reo.AuditorReporto,
         recibio = reo.SupRecibio,
                a1why = reo.a1why,
                a2why = reo.a2why,
                a3why = reo.a3why,
                a4why = reo.a4why,
                a5why = reo.a5why,
                acausa = reo.acausa,
                          aacion = reo.aaccion
 
            };
            try
            {
                var path1 = Server.MapPath($"~/Evidence/Quality/Docks/before/{id}");

                var path2 = Server.MapPath($"~/Evidence/Quality/Docks/after/{id}");
                DirectoryInfo Folder;
                Folder = new DirectoryInfo(path1);
                FileInfo[] Images1 = Folder.GetFiles();
                Folder = new DirectoryInfo(path2);
                FileInfo[] Images2 = Folder.GetFiles();

                var ws = template.Workbook.Worksheet("REPORTE");
                Images1 = Images1.Where(w => w.Name.ToLower().Contains(".jpeg") || w.Name.ToLower().Contains(".jpg")).ToArray();
                Images2 = Images2.Where(w => w.Name.ToLower().Contains(".jpeg") || w.Name.ToLower().Contains(".jpg")).ToArray();
                if (Images1.Count() > 0)
                {
                    var image1 = ws.AddPicture(Images1.FirstOrDefault().FullName)
                        .MoveTo(ws.Cell("B14"))
                    .Scale(0.25); // optional: resize picture
                }

                if (Images2.Count() > 0)
                {
                    var image2 = ws.AddPicture(Images2.FirstOrDefault().FullName)
        .MoveTo(ws.Cell("H14"))
        .Scale(0.25); // optional: resize picture



                }
            }
            catch (Exception ex) { }

            template.AddVariable(tempy);
            template.Generate();
            using (MemoryStream stream = new MemoryStream())
            {
                template.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Dock_" + reo.NoDockAudit + ".xlsx");
            }


        }

        // GET: CDockaudits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDockaudit cDockaudit = db.CDockaudits.Find(id);
            if (cDockaudit == null)
            {
                return HttpNotFound();
            }
            return View(cDockaudit);
        }

        // GET: CDockaudits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CDockaudits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NoDockAudit,Fecha,NoDeParte,Area,Cantidad,cliente,Descripcion,Clasificacion,AreaOrigen,SupOrigen,SupRecibio,AuditorReporto,Comentarios,Turno,lote,a1why,a2why,a3why,a4why,a5why,acausa,aaccion")] CDockaudit cDockaudit)
        {
            if (ModelState.IsValid)
            {
                db.CDockaudits.Add(cDockaudit);
                db.SaveChanges();
                TempData["Section"] = "Evidence";
                return RedirectToAction("Edit", new { id = cDockaudit.ID });
            }

            return View(cDockaudit);
        }

        // GET: CDockaudits/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Section = TempData["Section"];

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDockaudit cDockaudit = db.CDockaudits.Find(id);
            if (cDockaudit == null)
            {
                return HttpNotFound();
            }
            return View(cDockaudit);
        }

        // POST: CDockaudits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NoDockAudit,Fecha,NoDeParte,Area,Cantidad,cliente,Descripcion,Clasificacion,AreaOrigen,SupOrigen,SupRecibio,AuditorReporto,Comentarios,Turno,lote,a1why,a2why,a3why,a4why,a5why,acausa,aaccion")] CDockaudit cDockaudit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cDockaudit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cDockaudit);
        }

        // GET: CDockaudits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CDockaudit cDockaudit = db.CDockaudits.Find(id);
            if (cDockaudit == null)
            {
                return HttpNotFound();
            }
            return View(cDockaudit);
        }

        // POST: CDockaudits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CDockaudit cDockaudit = db.CDockaudits.Find(id);
            db.CDockaudits.Remove(cDockaudit);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                        path = Server.MapPath($"~/Evidence/Quality/Docks/before/{id}");

                    if (t == "d")
                        path = Server.MapPath($"~/Evidence/Quality/Docks/after/{id}");

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


            string fileName = "";
            link = link.Replace(@"../../", "");
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
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

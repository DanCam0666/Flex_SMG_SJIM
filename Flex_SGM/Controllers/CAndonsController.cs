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

namespace Flex_SGM.Controllers
{
    public class CAndonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CAndons
        public ActionResult Index(string btn = "Por Mes", string dti = "", string dtf = "",string paret= "Cliente")
        {

            var data = new List<CAndon>();
            var datafiltered = new List<CAndon>();
            var fecha = DateTime.Now.AddMonths(-1);
            var fechaf = DateTime.Now;

            string[] array2 = { "Por Mes", "Por Años", "Por Dia" };
            ViewBag.btn = new SelectList(array2);

            string[] array3 = { "Cliente", "Defecto1","Turno", "AreaActivacion", "ZonaActivacion", "AuditorArea", "lote",  "Proyecto", "Nombredeparte", "AreaDefecto" };
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

            data = db.CAndons.Where(w => (w.Fecha.Year >= fecha.Year) &&
                                      (w.Fecha.Year <= fechaf.Year)
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
                 

                    ViewBag.dx = "Dia " + fecha.ToString("dd/MM/yyyy") + " al Dia " + fechaf.ToString("dd/MM/yyyy");
                    ViewBag.dxx = " Dia";
                }

            }
            //--------------------------------------------------------
            if (paret == "Turno")
            {
                var groupdata = datafiltered.GroupBy(g => g.Turno).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "AreaActivacion")
            {
                var groupdata = datafiltered.GroupBy(g => g.AreaActivacion).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "ZonaActivacion")
            {
                var groupdata = datafiltered.GroupBy(g => g.ZonaActivacion).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "AuditorArea")
            {
                var groupdata = datafiltered.GroupBy(g => g.AuditorArea).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "lote")
            {
                var groupdata = datafiltered.GroupBy(g => g.lote).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "Cliente")
            {
                var groupdata = datafiltered.GroupBy(g => g.cliente).OrderByDescending(k => k.Count()).ToList();
               var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "Proyecto")
            {
                var groupdata = datafiltered.GroupBy(g => g.proyecto).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "Nombredeparte")
            {
                var groupdata = datafiltered.GroupBy(g => g.NombreDeParte).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "AreaDefecto")
            {
                var groupdata = datafiltered.GroupBy(g => g.AreaDefecto).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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
            if (paret == "Defecto1")
            {
                var groupdata = datafiltered.GroupBy(g => g.Defecto1).OrderByDescending(k => k.Count()).ToList();
                var labels = "'";
                var gdata = "";
                var gdata2 = "";
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


            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View(datafiltered);
            //  return View(db.CAndons.ToList());
        }

        // GET: CAndons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon cAndon = db.CAndons.Find(id);
            if (cAndon == null)
            {
                return HttpNotFound();
            }
            return View(cAndon);
        }

        // GET: CAndons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CAndons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NoAndon,Fecha,Turno,AreaActivacion,ZonaActivacion,AuditorArea,cliente,proyecto,NombreDeParte,Defecto1,Cantidadpz1,Defecto2,Cantidadpz2,Defecto3,Cantidadpz3,Preventivos,AreaDefecto,SubArea,AuditorAreaDefecto,ResponsableAndon,FormatoAndon,AlertaCalidad,MetCertificación,EstatusAndon,Comentarios,a1d,a5d,a10d,a20d,a30d,lote")] CAndon cAndon)
        {
            if (ModelState.IsValid)
            {
                db.CAndons.Add(cAndon);
                db.SaveChanges();
                TempData["Section"] = "Evidence";
                return RedirectToAction("Edit", new { id = cAndon.ID });

            }

            return View(cAndon);
        }

        // GET: CAndons/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Section = TempData["Section"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon cAndon = db.CAndons.Find(id);
            if (cAndon == null)
            {
                return HttpNotFound();
            }
            return View(cAndon);
        }

        // POST: CAndons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NoAndon,Fecha,Turno,AreaActivacion,ZonaActivacion,AuditorArea,cliente,proyecto,NombreDeParte,Defecto1,Cantidadpz1,Defecto2,Cantidadpz2,Defecto3,Cantidadpz3,Preventivos,AreaDefecto,SubArea,AuditorAreaDefecto,ResponsableAndon,FormatoAndon,AlertaCalidad,MetCertificación,EstatusAndon,Comentarios,a1d,a5d,a10d,a20d,a30d,lote")] CAndon cAndon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAndon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cAndon);
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
                        path = Server.MapPath($"~/Evidence/Quality/Andon/before/{id}");

                    if (t == "d")
                        path = Server.MapPath($"~/Evidence/Quality/Andon/after/{id}");

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

        public FileResult Download(string link)
        {
            string fileName = "";
            link = link.Replace(@"../../", "");
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: CAndons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon cAndon = db.CAndons.Find(id);
            if (cAndon == null)
            {
                return HttpNotFound();
            }
            return View(cAndon);
        }

        // POST: CAndons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CAndon cAndon = db.CAndons.Find(id);
            db.CAndons.Remove(cAndon);
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

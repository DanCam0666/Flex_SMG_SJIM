using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;

namespace Flex_SGM.Controllers
{
    public class CAndon2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CAndon2
        public ActionResult Index(string btn = "Por Mes", string dti = "", string dtf = "", string paret = "Cliente")
        {

            var data = new List<CAndon2>();
            var datafiltered = new List<CAndon2>();
            var fecha = DateTime.Now.AddMonths(-1);
            var fechaf = DateTime.Now;

            string[] array2 = { "Por Mes", "Por Años", "Por Dia" };
            ViewBag.btn = new SelectList(array2);

            string[] array3 = { "Turno", "AreaActivacion", "ZonaActivacion", "AuditorArea", "lote", "Cliente", "Proyecto", "Nombredeparte", "AreaDefecto", "Defecto1" };
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
            var cAndon2 = db.CAndon2.Include(c => c.Assigned).Include(c => c.Clientes).Include(c => c.Primary).Include(c => c.Proyectos);
            data = cAndon2.ToList();

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


            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View(datafiltered);

           // return View(cAndon2.ToList());
        }

        // GET: CAndon2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon2 cAndon2 = db.CAndon2.Find(id);
            if (cAndon2 == null)
            {
                return HttpNotFound();
            }
            return View(cAndon2);
        }

        // GET: CAndon2/Create
        public ActionResult Create()
        {
            ViewBag.AreaseID = new SelectList(db.cAreas, "ID", "Area");
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente");
            ViewBag.AreasgID = new SelectList(db.cAreas, "ID", "Area");
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto");
            return View();
        }

        // POST: CAndon2/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Fecha,Turno,Hora,AreasgID,AreaseID,ClientesID,ProyectosID,lote,NoDeParte,NombreDeParte,AndonDefectoID,Comentarios,AndonAuditorID,AndonSupervisoresID,Asistentes,EstatusAndon,Esproblema,Esproblemaseguridad,Esproblemavario,a1why,a2why,a3why,a4why,a50d,Causas,Acciones")] CAndon2 cAndon2)
        {
            if (ModelState.IsValid)
            {
                db.CAndon2.Add(cAndon2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaseID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreaseID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cAndon2.ClientesID);
            ViewBag.AreasgID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreasgID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", cAndon2.ProyectosID);
            return View(cAndon2);
        }

        // GET: CAndon2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon2 cAndon2 = db.CAndon2.Find(id);
            if (cAndon2 == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaseID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreaseID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cAndon2.ClientesID);
            ViewBag.AreasgID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreasgID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", cAndon2.ProyectosID);
            return View(cAndon2);
        }

        // POST: CAndon2/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Fecha,Turno,Hora,AreasgID,AreaseID,ClientesID,ProyectosID,lote,NoDeParte,NombreDeParte,AndonDefectoID,Comentarios,AndonAuditorID,AndonSupervisoresID,Asistentes,EstatusAndon,Esproblema,Esproblemaseguridad,Esproblemavario,a1why,a2why,a3why,a4why,a50d,Causas,Acciones")] CAndon2 cAndon2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAndon2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaseID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreaseID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cAndon2.ClientesID);
            ViewBag.AreasgID = new SelectList(db.cAreas, "ID", "Area", cAndon2.AreasgID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", cAndon2.ProyectosID);
            return View(cAndon2);
        }

        // GET: CAndon2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CAndon2 cAndon2 = db.CAndon2.Find(id);
            if (cAndon2 == null)
            {
                return HttpNotFound();
            }
            return View(cAndon2);
        }

        // POST: CAndon2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CAndon2 cAndon2 = db.CAndon2.Find(id);
            db.CAndon2.Remove(cAndon2);
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

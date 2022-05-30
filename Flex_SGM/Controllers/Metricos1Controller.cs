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
using System.Globalization;
using System.IO;
using ClosedXML.Excel;
using Flex_SGM.Scripts;



namespace Flex_SGM.Controllers
{
    public class Metricos1Controller : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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

        // GET: Metricos1
        public async Task<ActionResult> Index(string amaquina, string maquina, string submaquina, string mgroup, string xmgroup, string btn = "Metricos por Mes", string dti = "", string dtf = "")
        {
            var metricos = db.Metricos.ToList<Metricos>();
            var fulldatafiltered = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<MetricsNew> Ldata = new List<MetricsNew>();
            var StartDate = "01/01/" + DateTime.Now.Year + " 00:00:00";
            var EndDate = "31/12/" + DateTime.Now.Year + " 23:59:59";
            var fecha = Convert.ToDateTime(StartDate);
            var fechaf = Convert.ToDateTime(EndDate);
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

            for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
            {
                for (int jmes = 1; jmes <= 12; jmes++)
                {
                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                    string nombreMes = formatoFecha.GetMonthName(jmes);
                    var dias = DateTime.DaysInMonth(iaño, jmes);
                    string thistiempo = "";
                    ViewBag.CapPer = 0;

                    for (int kdia = 1; kdia <= dias; kdia++)
                    {
                        DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                        DateTime idi3er = idi.AddDays(+1);

                        if (btn == "Metricos por Mes")
                        {
                            thistiempo = iaño.ToString() + "-" + nombreMes;
                            ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                            ViewBag.dxx = " Mes";
                        }
                    }
                    thistiempo = iaño.ToString() + "-" + nombreMes;

                    var sum_AMEF = (metricos.Where(w => w.Usuario_area == "AMEF_Reverse" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum());
                    var cnt_AMEF = (metricos.Where(w => w.Usuario_area == "AMEF_Reverse" && w.DiaHora.Month == jmes).Count());
                    var sum_Capacities = (metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum());
                    var cnt_Capacities = (metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes).Count());
                    var sum_Cont_Imprv = (metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum());
                    var cnt_Cont_Imprv = (metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes).Count());
                    var sum_LPA_Covid = (metricos.Where(w => w.Usuario_area == "LPA_COVID" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum());
                    var cnt_LPA_Covid = (metricos.Where(w => w.Usuario_area == "LPA_COVID" && w.DiaHora.Month == jmes).Count());
                    var sum_TOC_HS_Audits = (metricos.Where(w => w.Usuario_area == "TOC_HS_Audits" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum());
                    var cnt_TOC_HS_Audits = (metricos.Where(w => w.Usuario_area == "TOC_HS_Audits" && w.DiaHora.Month == jmes).Count());

                    if (cnt_AMEF != 0)
                    {
                        ViewBag.AMEFPer = sum_AMEF / cnt_AMEF;
                    }
                    else
                    {
                        ViewBag.AMEFPer = 0;
                    }
                    if (cnt_Capacities != 0)
                    {
                        ViewBag.CapPer = sum_Capacities/cnt_Capacities;
                    }
                    else
                    {
                        ViewBag.CapPer = 0;
                    }
                    if (cnt_Cont_Imprv != 0)
                    {
                        ViewBag.ConPer = (cnt_Cont_Imprv * 100) / 22;
                    }
                    else
                    {
                        ViewBag.ConPer = 0;
                    }
                    if (sum_LPA_Covid != 0)
                    {
                        ViewBag.LpaPer = (sum_LPA_Covid / cnt_LPA_Covid);
                    }
                    else
                    {
                        ViewBag.LpaPer = 0;
                    }
                    if (sum_TOC_HS_Audits != 0)
                    {
                        ViewBag.TocPer = (sum_TOC_HS_Audits / cnt_TOC_HS_Audits);
                    }
                    else
                    {
                        ViewBag.TocPer = 0;
                    }

                    MetricsNew data_show = new MetricsNew
                    {
                        TiempoLabel = thistiempo,
                        Amef = ViewBag.AMEFPer,
                        BiBo = metricos.Where(w => w.Usuario_area == "Build_In_Build_Out" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Caps = metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        CapNum = metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes).Count(),
                        CapPer = ViewBag.CapPer,
                        CoImCnt = metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes).Count(),
                        CoImPer = ViewBag.ConPer,
                        CuCo = metricos.Where(w => w.Usuario_area == "Customer_Complaints" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Ecn = metricos.Where(w => w.Usuario_area == "ECNs_PCRs" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        LaOu = metricos.Where(w => w.Usuario_area == "Lay_Outs" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Lpa = ViewBag.LpaPer,
                        PaPo = metricos.Where(w => w.Usuario_area == "Packaging" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        PaDe = metricos.Where(w => w.Usuario_area == "Parts_Delivery" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Plm = metricos.Where(w => w.Usuario_area == "PLM" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        QuHs = metricos.Where(w => w.Usuario_area == "Quality_HS" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        ReRa = metricos.Where(w => w.Usuario_area == "Red_Rabbits" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Safe = metricos.Where(w => w.Usuario_area == "Safety_HS" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        ScCo = metricos.Where(w => w.Usuario_area == "Scrap" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),
                        Toc = ViewBag.TocPer,
                        YeSh = metricos.Where(w => w.Usuario_area == "Yellow_Sheets" && w.DiaHora.Month == jmes).Select(w => w.Proyectos).Sum(),

                        Ford = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("Ford"))
                        .Select(w => w.Proyectos).Sum(),

                        GM = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("GM"))
                        .Select(w => w.Proyectos).Sum(),

                        Mopar = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("Mopar"))
                        .Select(w => w.Proyectos).Sum(),

                        Nissan = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("Nissan"))
                        .Select(w => w.Proyectos).Sum(),

                        Stellantis = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("Stellantis"))
                        .Select(w => w.Proyectos).Sum(),

                        Toyota = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("Toyota"))
                        .Select(w => w.Proyectos).Sum(),

                        VW = metricos
                        .Where(w => w.Usuario_area == "Cust_Score_Cards" && w.DiaHora.Month == jmes)
                        .Where(w => w.Descripcion.Contains("VW"))
                        .Select(w => w.Proyectos).Sum(),
                    };
                    Ldata.Add(data_show);
                }
            }

            ViewBag.metricospermachine = Ldata.ToList();

            //--------------------------------------------

            if (btn == "Metricos por Mes")
            {
                datafiltered.Clear();
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

                            var temp = dataaño.Where(w => w.DiaHora.Year == iaño && w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);
                            i++;
                        }
                    }
                }
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************

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
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("Superintendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;
            //--------------------------------------------
            ViewBag.data = Ldata;
            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View(await db.Metricos.ToListAsync());
        }

        // GET: Metricos1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // GET: Metricos1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Metricos1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            if (ModelState.IsValid)
            {
                db.Metricos.Add(metricos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(metricos);
        }

        // GET: Metricos1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Metricos1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metricos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(metricos);
        }

        // GET: Metricos1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Metricos1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Metricos metricos = await db.Metricos.FindAsync(id);
            db.Metricos.Remove(metricos);
            await db.SaveChangesAsync();
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

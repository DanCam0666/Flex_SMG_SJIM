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
using DocumentFormat.OpenXml.Wordprocessing;

namespace Flex_SGM.Controllers
{
    [Authorize]
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
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        // GET: Metricos1
        public async Task<ActionResult> Index(string btn = "Metricos por Mes", string dti = "", string dtf = "")
        {
            string indYear = DateTime.Now.Year.ToString();
            var cookie = Request.Cookies["indYear"];
            if (cookie != null)
            {
                indYear = Request.Cookies["indYear"].Value;
            }            
            int year = Convert.ToInt32(indYear);

            var metricos = db.Metricos.ToList<Metricos>();
            var fulldatafiltered = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<MetricsNew> Ldata = new List<MetricsNew>();
            var StartDate = "01/01/" + year + " 00:00:00";
            var EndDate = "31/12/" + year + " 23:59:59";
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

            for (int iaño = year; iaño <= fechaf.Year; iaño++)
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

					var sum_AMEF_N1_N4 = (metricos.Where(w => w.Usuario_area == "AMEF_N1_N4" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
					var cnt_AMEF_N1_N4 = (metricos.Where(w => w.Usuario_area == "AMEF_N1_N4" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
					var sum_AMEF_N2_N3 = (metricos.Where(w => w.Usuario_area == "AMEF_N2_N3" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
					var cnt_AMEF_N2_N3 = (metricos.Where(w => w.Usuario_area == "AMEF_N2_N3" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
					var sum_Capacities = (metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
					var cnt_Capacities = (metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
					var sum_Highlights = (metricos.Where(w => w.Usuario_area == "Highlights" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
					var cnt_Highlights = (metricos.Where(w => w.Usuario_area == "Highlights" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
					var sum_Capacity_Tickets = (metricos.Where(w => w.Usuario_area == "Capacity_Tickets" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
					var cnt_Capacity_Tickets = (metricos.Where(w => w.Usuario_area == "Capacity_Tickets" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
					var sum_Cust_Comp = (metricos.Where(w => w.Usuario_area == "Customer_Complaints" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
                    var sum_Cont_Imprv = (metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum());
                    var cnt_Cont_Imprv = (metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count());
                    var sum_MDRs = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area.Contains("MDRs") && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();
                    var cnt_MDRs = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area.Contains("MDRs") && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Count();
                    var sum_PPAPs = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area.Contains("PPAPs") && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();
                    var cnt_PPAPs = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area.Contains("PPAPs") && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Count();

                    var sum_ECN_PCR = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "ECNs_PCRs" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var sum_Layout = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Lay_Outs" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var sum_LPA_Bluebook = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "LPA_Bluebook" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var cnt_LPA_Bluebook = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "LPA_Bluebook" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Count();

                    var sum_Packaging = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Packaging" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var sum_PLM = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "PLM" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var sum_Yellow_SH = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Yellow_Sheets" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var sum_Vacaciones = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Vacaciones" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var cnt_Vacaciones = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Vacaciones" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Count();

                    var sum_Red_Rabbits = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Red_Rabbits" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Select(w => w.Proyectos)
                        .Sum();

                    var cnt_Red_Rabbits = metricos
                        .Where(w => w.Usuario_area != null && w.Usuario_area == "Red_Rabbits" && w.DiaHora != null && w.DiaHora.Month == jmes && w.DiaHora.Year == year)
                        .Count();
                    var LpaPer = (10 - sum_LPA_Bluebook) * 10;
                    var EcnPcr = (10 - sum_ECN_PCR) * 10;
                    var Packaging = (10 - sum_Packaging) * 10;
                    var PLM = (10 - sum_PLM) * 10;
                    var Cust_Comp = (10 - sum_Cust_Comp) * 10;
					var PPAPs = 0;
					if (cnt_PPAPs != 0)
						PPAPs = sum_PPAPs / cnt_PPAPs;

					var MDRs = 0;
					if (cnt_MDRs != 0)
						MDRs = sum_MDRs / cnt_MDRs;

					if (Cust_Comp <= 0){Cust_Comp = 0;}

                    if (Packaging <= 0){Packaging = 0;}

                    if (PLM <= 0){PLM = 0;}

                    if (EcnPcr <= 0){EcnPcr = 0;}

                    if (LpaPer <= 0){LpaPer = 0;}

					if (cnt_AMEF_N1_N4 != 0) { ViewBag.AMEFPerN1 = sum_AMEF_N1_N4 / cnt_AMEF_N1_N4; }
					else { ViewBag.AMEFPerN1 = 0; }

					if (cnt_AMEF_N2_N3 != 0) { ViewBag.AMEFPerN2 = sum_AMEF_N2_N3 / cnt_AMEF_N2_N3; }
					else { ViewBag.AMEFPerN2 = 0; }

					if (cnt_Capacities != 0) { ViewBag.CapPer = sum_Capacities / cnt_Capacities; }
					else { ViewBag.CapPer = 0; }

					if (cnt_Capacity_Tickets != 0) { ViewBag.CapTic = sum_Capacity_Tickets / cnt_Capacity_Tickets; }
					else { ViewBag.CapTic = 0; }

					if (cnt_Cont_Imprv != 0){ViewBag.ConPer = (cnt_Cont_Imprv * 100) / 21;}
                    else{ViewBag.ConPer = 0;}

                    if (sum_Vacaciones != 0){ViewBag.VacPer = (sum_Vacaciones / 23);}
                    else{ViewBag.VacPer = 0;}

                    if (cnt_Red_Rabbits != 0){ViewBag.ReRaPer = (sum_Red_Rabbits / cnt_Red_Rabbits);}
                    else{ViewBag.ReRaPer = 0;}

                    ViewBag.WrkStd = (ViewBag.AMEFPerN1 + ViewBag.AMEFPerN2 + ViewBag.ReRaPer) / 3; //(sum_Operator_Instructions + sum_Operator_Training + sum_Poka_Yoke + sum_SMED_Training) / 4;
                    ViewBag.Accnt = (LpaPer + EcnPcr + ViewBag.VacPer) / 3;
                    ViewBag.ConImp = (ViewBag.ConPer + sum_Yellow_SH) / 2;
                    ViewBag.CstFcs = (Packaging + PPAPs + MDRs + Cust_Comp + ViewBag.CapTic) / 5;
                    ViewBag.Sustan = (sum_Highlights + ViewBag.CapPer + PLM + sum_Layout) / 4; //(sum_GAP + sum_Lay_Outs + sum_Manpower_Gypsa + sum_Manpower_LunkoMex) / 4;
                    ViewBag.indYear = year;

                    MetricsNew data_show = new MetricsNew
                    {
                        TiempoLabel = thistiempo,
						Amef_N1_N4 = ViewBag.AMEFPerN1,
						Amef_N2_N3 = ViewBag.AMEFPerN2,
						BiBo = metricos.Where(w => w.Usuario_area == "Build_In_Build_Out" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        Caps = metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        CapNum = metricos.Where(w => w.Usuario_area == "Capacities_Review" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count(),
                        CapPer = ViewBag.CapPer,
						MdrPer = metricos.Where(w => w.Usuario_area == "MDRs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
						MdrCnt = metricos.Where(w => w.Usuario_area == "MDRs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count(),
						PPAPsPer = metricos.Where(w => w.Usuario_area == "PPAPs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
						PPAPsCnt = metricos.Where(w => w.Usuario_area == "PPAPs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count(),
						CoImCnt = metricos.Where(w => w.Usuario_area == "Continuous_Improvment" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Count(),
                        CoImPer = ViewBag.ConPer,
                        CuCo = metricos.Where(w => w.Usuario_area == "Customer_Complaints" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        Ecn = metricos.Where(w => w.Usuario_area == "ECNs_PCRs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        LaOu = metricos.Where(w => w.Usuario_area == "Lay_Outs" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        Lpa = metricos.Where(w => w.Usuario_area == "LPA_Bluebook" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        PaPo = metricos.Where(w => w.Usuario_area == "Packaging" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        PaDe = metricos.Where(w => w.Usuario_area == "Vacaciones" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        Plm = metricos.Where(w => w.Usuario_area == "PLM" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        QuHs = metricos.Where(w => w.Usuario_area == "Capacity_Tickets" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        ReRa = ViewBag.ReRaPer,
                        HiFi = metricos.Where(w => w.Usuario_area == "Highlights" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        ScCo = metricos.Where(w => w.Usuario_area == "Scrap" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),
                        Vacas = ViewBag.VacPer,
                        YeSh = metricos.Where(w => w.Usuario_area == "Yellow_Sheets" && w.DiaHora.Month == jmes && w.DiaHora.Year == year).Select(w => w.Proyectos).Sum(),

                        WrkStd = ViewBag.WrkStd,
                        Accnt = ViewBag.Accnt,
                        ConImp = ViewBag.ConImp,
                        CstFcs = ViewBag.CstFcs,
                        Sustan = ViewBag.Sustan,


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
                for (int iaño = year; iaño <= fechaf.Year; iaño++)
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
                            var temp = dataaño.Where(w => w.DiaHora.Year == iaño && w.DiaHora.Month == jmes && w.DiaHora.Year == year);
                            datafiltered.AddRange(temp);
                            i++;
                        }
                    }
                }
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = year;
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
            if (cpuesto.Contains("Super") || cpuesto.Contains("Gerente"))
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

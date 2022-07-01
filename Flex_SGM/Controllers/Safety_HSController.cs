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
using Flex_SGM.emaildata;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Flex_SGM.Controllers
{
    public class Safety_HSController : Controller
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

        // GET: Safety_HS
        public async Task<ActionResult> Index()
        {
            var metricos = db.Metricos.ToList<Metricos>();
            var validMetricosRecords = metricos.Where(m => m.Usuario_area is "Safety_HS").ToList();
            var users = validMetricosRecords.Where(m => m.Usuario_area is "Safety_HS")
                .GroupBy(m => m.Usuario_responsable)
                .ToList();

            List<DisplayUserChart> displayUserChartList = new List<DisplayUserChart>();

            foreach (var item in users)
            {
                DisplayUserChart displayUserChartJanuary = new DisplayUserChart();
                displayUserChartJanuary.Usuario_responsable = item.Key;
                var chartData1 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 1).Sum(m => m.Proyectos);
                displayUserChartJanuary.Proyectos = chartData1;
                displayUserChartJanuary.Month = 1;
                displayUserChartJanuary.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartJanuary);

                //For february
                DisplayUserChart displayUserChartFebruary = new DisplayUserChart();
                displayUserChartFebruary.Usuario_responsable = item.Key;
                var chartData2 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 2).Sum(m => m.Proyectos);
                displayUserChartFebruary.Proyectos = chartData2;
                displayUserChartFebruary.Month = 2;
                displayUserChartFebruary.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartFebruary);

                //For march
                DisplayUserChart displayUserChartMarch = new DisplayUserChart();
                displayUserChartMarch.Usuario_responsable = item.Key;
                var chartData3 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 3).Sum(m => m.Proyectos);
                displayUserChartMarch.Proyectos = chartData3;
                displayUserChartMarch.Month = 3;
                displayUserChartMarch.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartMarch);

                //For april
                DisplayUserChart displayUserChartApril = new DisplayUserChart();
                displayUserChartApril.Usuario_responsable = item.Key;
                var chartData4 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 4).Sum(m => m.Proyectos);
                displayUserChartApril.Proyectos = chartData4;
                displayUserChartApril.Month = 4;
                displayUserChartApril.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartApril);

                //For may
                DisplayUserChart displayUserChartMay = new DisplayUserChart();
                displayUserChartMay.Usuario_responsable = item.Key;
                var chartData5 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 5).Sum(m => m.Proyectos);
                displayUserChartMay.Proyectos = chartData5;
                displayUserChartMay.Month = 5;
                displayUserChartMay.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartMay);

                //For june
                DisplayUserChart displayUserChartJune = new DisplayUserChart();
                displayUserChartJune.Usuario_responsable = item.Key;
                var chartData6 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 6).Sum(m => m.Proyectos);
                displayUserChartJune.Proyectos = chartData6;
                displayUserChartJune.Month = 6;
                displayUserChartJune.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartJune);

                //For july
                DisplayUserChart displayUserChartJuly = new DisplayUserChart();
                displayUserChartJuly.Usuario_responsable = item.Key;
                var chartData7 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 7).Sum(m => m.Proyectos);
                displayUserChartJuly.Proyectos = chartData7;
                displayUserChartJuly.Month = 7;
                displayUserChartJuly.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartJuly);

                //For august
                DisplayUserChart displayUserChartAugust = new DisplayUserChart();
                displayUserChartAugust.Usuario_responsable = item.Key;
                var chartData8 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 8).Sum(m => m.Proyectos);
                displayUserChartAugust.Proyectos = chartData8;
                displayUserChartAugust.Month = 8;
                displayUserChartAugust.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartAugust);

                //For september
                DisplayUserChart displayUserChartSeptember = new DisplayUserChart();
                displayUserChartSeptember.Usuario_responsable = item.Key;
                var chartData9 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 9).Sum(m => m.Proyectos);
                displayUserChartSeptember.Proyectos = chartData9;
                displayUserChartSeptember.Month = 9;
                displayUserChartSeptember.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartSeptember);

                //For october
                DisplayUserChart displayUserChartOctober = new DisplayUserChart();
                displayUserChartOctober.Usuario_responsable = item.Key;
                var chartData10 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 10).Sum(m => m.Proyectos);
                displayUserChartOctober.Proyectos = chartData10;
                displayUserChartOctober.Month = 10;
                displayUserChartOctober.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartOctober);

                //For november
                DisplayUserChart displayUserChartNovember = new DisplayUserChart();
                displayUserChartNovember.Usuario_responsable = item.Key;
                var chartData11 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 11).Sum(m => m.Proyectos);
                displayUserChartNovember.Proyectos = chartData11;
                displayUserChartNovember.Month = 11;
                displayUserChartNovember.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartNovember);

                //For december
                DisplayUserChart displayUserChartDecember = new DisplayUserChart();
                displayUserChartDecember.Usuario_responsable = item.Key;
                var chartData12 = validMetricosRecords.Where(m => m.Usuario_responsable == item.Key)
               .Where(m => m.DiaHora.Year == DateTime.Now.Year)
               .Where(m => m.DiaHora.Month == 12).Sum(m => m.Proyectos);
                displayUserChartDecember.Proyectos = chartData12;
                displayUserChartDecember.Month = 12;
                displayUserChartDecember.Year = DateTime.Now.Year;
                displayUserChartList.Add(displayUserChartDecember);
            };

            var lchartData1 = displayUserChartList.Where(f => f.Month == 1).Select(f => f.Proyectos).ToList();
            var lchartData2 = displayUserChartList.Where(f => f.Month == 2).Select(f => f.Proyectos).ToList();
            var lchartData3 = displayUserChartList.Where(f => f.Month == 3).Select(f => f.Proyectos).ToList();
            var lchartData4 = displayUserChartList.Where(f => f.Month == 4).Select(f => f.Proyectos).ToList();
            var lchartData5 = displayUserChartList.Where(f => f.Month == 5).Select(f => f.Proyectos).ToList();
            var lchartData6 = displayUserChartList.Where(f => f.Month == 6).Select(f => f.Proyectos).ToList();
            var lchartData7 = displayUserChartList.Where(f => f.Month == 7).Select(f => f.Proyectos).ToList();
            var lchartData8 = displayUserChartList.Where(f => f.Month == 8).Select(f => f.Proyectos).ToList();
            var lchartData9 = displayUserChartList.Where(f => f.Month == 9).Select(f => f.Proyectos).ToList();
            var lchartData10 = displayUserChartList.Where(f => f.Month == 10).Select(f => f.Proyectos).ToList();
            var lchartData11 = displayUserChartList.Where(f => f.Month == 11).Select(f => f.Proyectos).ToList();
            var lchartData12 = displayUserChartList.Where(f => f.Month == 12).Select(f => f.Proyectos).ToList();

            var chartLabel = metricos
                .Where(m => m.Usuario_area is "Safety_HS")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .GroupBy(m => m.Usuario_responsable)
                .Select(m => m.Key)
                .ToList();

            ViewBag.ChartData1 = lchartData1;
            ViewBag.ChartData2 = lchartData2;
            ViewBag.ChartData3 = lchartData3;
            ViewBag.ChartData4 = lchartData4;
            ViewBag.ChartData5 = lchartData5;
            ViewBag.ChartData6 = lchartData6;
            ViewBag.ChartData7 = lchartData7;
            ViewBag.ChartData8 = lchartData8;
            ViewBag.ChartData9 = lchartData9;
            ViewBag.ChartData10 = lchartData10;
            ViewBag.ChartData11 = lchartData11;
            ViewBag.ChartData12 = lchartData12;
            ViewBag.ChartLabel = chartLabel;

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

            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());

            return View(await db.Metricos.ToListAsync());
        }

        // GET: Safety_HS/Details/5
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

        [Authorize]
        // GET: Safety_HS/Create
        public ActionResult Create()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            Metricos metricos = new Metricos
            {
                Usuario = currentUser.UserFullName,
                DiaHora = DateTime.Now
            };
            Console.WriteLine("User: ", currentUser.UserFullName);

            ViewBag.Usuario = currentUser.UserFullName;
            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());


            return View(metricos);
        }

        // POST: Safety_HS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Atendio,Proyectos,listarea")] Metricos metricos)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(metricos.Usuario_responsable))
                    metricos.Usuario_responsable = "-";

                ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(metricos.Usuario_responsable)).FirstOrDefault();
                var scorreo = new EmailController();
                if (cUser1 != null)
                {
                    if (cUser1.Email != null && cUser1.UserFullName != cuser)
                        if (cUser1.Email.Contains("@flexngate.com"))
                        {
                            scorreo.NewMetrico(cUser1.Email, cuser, metricos.Descripcion, metricos.Usuario_area);
                        }
                }

                db.Metricos.Add(metricos);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");
            ViewBag.Usuario = currentUser.UserFullName;
            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());

            return View(metricos);
        }

        [Authorize]
        // GET: AMEF/Edit/5
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

        // POST: AMEF/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
                ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(metricos.Usuario_responsable)).FirstOrDefault();
                var scorreo = new EmailController();
                if (cUser1 != null)
                {
                    if (cUser1.Email != null && cUser1.UserFullName != cuser)
                        if (cUser1.Email.Contains("@flexngate.com"))
                        {
                            string Id_String = metricos.ID.ToString();
                            scorreo.UpdateMetrico(cUser1.Email, cuser, Id_String, metricos.Descripcion, metricos.Usuario_area);
                        }
                }

                db.Entry(metricos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");

            return View(metricos);
        }

        [Authorize]
        // GET: Safety_HS/Delete/5
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

        // POST: Safety_HS/Delete/5
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

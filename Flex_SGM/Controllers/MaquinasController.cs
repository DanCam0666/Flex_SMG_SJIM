using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Flex_SGM.Controllers
{

    public class MaquinasController : Controller
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

        // GET: Maquinas
        public ActionResult Index()
        {
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

            return View(db.Maquinas.ToList());
        }

        // GET: Maquinas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquinas maquinas = db.Maquinas.Find(id);
            if (maquinas == null)
            {
                return HttpNotFound();
            }
            return View(maquinas);
        }

        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: Maquinas/Create
        public ActionResult Create()
        {
          
            ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());
            ViewBag.Cliente = new SelectList(Enum.GetValues(typeof(flex_Cliente)).Cast<flex_Cliente>().ToList());
            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());

            return View();
        }

        // POST: Maquinas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Area,Cliente,Grupo,Codigo,Maquina,SubMaquina,DiaHora,Ubicacion,Critica")] Maquinas maquinas)
        {
            maquinas.DiaHora = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Maquinas.Add(maquinas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maquinas);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: Maquinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquinas maquinas = db.Maquinas.Find(id);
            if (maquinas == null)
            {
                return HttpNotFound();
            }
            var clien = Enum.GetValues(typeof(flex_Cliente)).Cast<flex_Cliente>().ToList();
         
            var list = clien.FirstOrDefault();
            for (int ii = 0; ii < clien.Count(); ii++) 
            {
                if (clien[ii].ToString() == maquinas.Cliente) 
                {
                    list = clien[ii];
                    break;
                };
            }
            var clienl= new SelectList(clien, list);

    

            var are = Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList();
            var list2 = are.FirstOrDefault();
            for (int ii = 0; ii < are.Count(); ii++)
            {
                if (are[ii].ToString() == maquinas.Area)
                {
                    list2 = are[ii];
                    break;
                };
            }
            var arel = new SelectList(are, list2);


            ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());
            ViewBag.Cliente = clienl;
            ViewBag.Area = arel;
            return View(maquinas);
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // POST: Maquinas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Area,Cliente,Grupo,Codigo,Maquina,SubMaquina,DiaHora,Ubicacion,Critica")] Maquinas maquinas)
        {
            if (ModelState.IsValid)
            {
                if (maquinas.Area.Contains("Soldadura"))
                    maquinas.Area ="Soldadura";
                db.Entry(maquinas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maquinas);
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: Maquinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquinas maquinas = db.Maquinas.Find(id);
            if (maquinas == null)
            {
                return HttpNotFound();
            }
            return View(maquinas);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin,Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // POST: Maquinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maquinas maquinas = db.Maquinas.Find(id);
            db.Maquinas.Remove(maquinas);
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

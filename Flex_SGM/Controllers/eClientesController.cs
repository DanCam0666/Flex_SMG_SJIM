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
    public class eClientesController : Controller
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

        // GET: eClientes
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
            if (cpuesto.Contains("Super") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;

            return View(db.eClientes.ToList());
        }

        // GET: eClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eClientes eClientes = db.eClientes.Find(id);
            if (eClientes == null)
            {
                return HttpNotFound();
            }
            return View(eClientes);
        }

        // GET: eClientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: eClientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Cliente")] eClientes eClientes)
        {
            if (ModelState.IsValid)
            {
                db.eClientes.Add(eClientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eClientes);
        }

        // GET: eClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eClientes eClientes = db.eClientes.Find(id);
            if (eClientes == null)
            {
                return HttpNotFound();
            }
            return View(eClientes);
        }

        // POST: eClientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Cliente")] eClientes eClientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eClientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eClientes);
        }

        // GET: eClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eClientes eClientes = db.eClientes.Find(id);
            if (eClientes == null)
            {
                return HttpNotFound();
            }
            return View(eClientes);
        }

        // POST: eClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eClientes eClientes = db.eClientes.Find(id);
            db.eClientes.Remove(eClientes);
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

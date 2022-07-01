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
    public class eProyectosController : Controller
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

        // GET: eProyectos
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


            var eProyectos = db.eProyectos.Include(c => c.Clientes);
            return View(eProyectos.ToList());
        }

        // GET: eProyectos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eProyectos eProyectos = db.eProyectos.Find(id);
            if (eProyectos == null)
            {
                return HttpNotFound();
            }
            return View(eProyectos);
        }

        // GET: eProyectos/Create
        public ActionResult Create()
        {
            ViewBag.Proyecto = new SelectList(db.eProyectos, "ID", "Proyecto");
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente");
            return View();
        }

        // POST: eProyectos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClientesID,Proyecto")] eProyectos eProyectos)
        {
            if (ModelState.IsValid)
            {
                db.eProyectos.Add(eProyectos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", eProyectos.ClientesID);
            return View(eProyectos);
        }

        // GET: eProyectos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eProyectos eProyectos = db.eProyectos.Find(id);
            if (eProyectos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Proyecto = new SelectList(db.eProyectos, "ID", "Proyecto");
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", eProyectos.ClientesID);
            return View(eProyectos);
        }

        // POST: eProyectos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClientesID,Proyecto")] eProyectos eProyectos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eProyectos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", eProyectos.ClientesID);
            return View(eProyectos);
        }

        // GET: eProyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eProyectos eProyectos = db.eProyectos.Find(id);
            if (eProyectos == null)
            {
                return HttpNotFound();
            }
            return View(eProyectos);
        }

        // POST: eProyectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eProyectos eProyectos = db.eProyectos.Find(id);
            db.eProyectos.Remove(eProyectos);
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

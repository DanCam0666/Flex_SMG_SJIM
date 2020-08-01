using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;

namespace Flex_SGM.Controllers
{
    public class cProyectosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: cProyectos
        public ActionResult Index()
        {
            var cProyectos = db.cProyectos.Include(c => c.Clientes);
            return View(cProyectos.ToList());
        }

        // GET: cProyectos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cProyectos cProyectos = db.cProyectos.Find(id);
            if (cProyectos == null)
            {
                return HttpNotFound();
            }
            return View(cProyectos);
        }

        // GET: cProyectos/Create
        public ActionResult Create()
        {
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente");
            return View();
        }

        // POST: cProyectos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClientesID,Proyecto")] cProyectos cProyectos)
        {
            if (ModelState.IsValid)
            {
                db.cProyectos.Add(cProyectos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cProyectos.ClientesID);
            return View(cProyectos);
        }

        // GET: cProyectos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cProyectos cProyectos = db.cProyectos.Find(id);
            if (cProyectos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cProyectos.ClientesID);
            return View(cProyectos);
        }

        // POST: cProyectos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClientesID,Proyecto")] cProyectos cProyectos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cProyectos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", cProyectos.ClientesID);
            return View(cProyectos);
        }

        // GET: cProyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cProyectos cProyectos = db.cProyectos.Find(id);
            if (cProyectos == null)
            {
                return HttpNotFound();
            }
            return View(cProyectos);
        }

        // POST: cProyectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cProyectos cProyectos = db.cProyectos.Find(id);
            db.cProyectos.Remove(cProyectos);
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

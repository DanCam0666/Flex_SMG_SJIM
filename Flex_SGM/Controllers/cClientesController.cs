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
    public class cClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: cClientes
        public ActionResult Index()
        {
            return View(db.cClientes.ToList());
        }

        // GET: cClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cClientes cClientes = db.cClientes.Find(id);
            if (cClientes == null)
            {
                return HttpNotFound();
            }
            return View(cClientes);
        }

        // GET: cClientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: cClientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Cliente")] cClientes cClientes)
        {
            if (ModelState.IsValid)
            {
                db.cClientes.Add(cClientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cClientes);
        }

        // GET: cClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cClientes cClientes = db.cClientes.Find(id);
            if (cClientes == null)
            {
                return HttpNotFound();
            }
            return View(cClientes);
        }

        // POST: cClientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Cliente")] cClientes cClientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cClientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cClientes);
        }

        // GET: cClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cClientes cClientes = db.cClientes.Find(id);
            if (cClientes == null)
            {
                return HttpNotFound();
            }
            return View(cClientes);
        }

        // POST: cClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cClientes cClientes = db.cClientes.Find(id);
            db.cClientes.Remove(cClientes);
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

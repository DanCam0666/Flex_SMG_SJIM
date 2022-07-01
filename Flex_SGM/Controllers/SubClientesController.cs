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
    public class SubClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SubClientes
        public ActionResult Index()
        {
            var subClientes = db.SubClientes.Include(s => s.Clientes);
            return View(subClientes.ToList());
        }

        // GET: SubClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubClientes subClientes = db.SubClientes.Find(id);
            if (subClientes == null)
            {
                return HttpNotFound();
            }
            return View(subClientes);
        }

        // GET: SubClientes/Create
        public ActionResult Create()
        {
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente");
            return View();
        }

        // POST: SubClientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ClientesID,SubCliente")] SubClientes subClientes)
        {
            if (ModelState.IsValid)
            {
                db.SubClientes.Add(subClientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", subClientes.ClientesID);
            return View(subClientes);
        }

        // GET: SubClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubClientes subClientes = db.SubClientes.Find(id);
            if (subClientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", subClientes.ClientesID);
            return View(subClientes);
        }

        // POST: SubClientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ClientesID,SubCliente")] SubClientes subClientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subClientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", subClientes.ClientesID);
            return View(subClientes);
        }

        // GET: SubClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubClientes subClientes = db.SubClientes.Find(id);
            if (subClientes == null)
            {
                return HttpNotFound();
            }
            return View(subClientes);
        }

        // POST: SubClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubClientes subClientes = db.SubClientes.Find(id);
            db.SubClientes.Remove(subClientes);
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

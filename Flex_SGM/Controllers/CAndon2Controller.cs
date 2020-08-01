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
    public class CAndon2Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CAndon2
        public ActionResult Index()
        {
            var cAndon2 = db.CAndon2.Include(c => c.Assigned).Include(c => c.Clientes).Include(c => c.Primary).Include(c => c.Proyectos);
            return View(cAndon2.ToList());
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

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
    public class pcrsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: pcrs
        public ActionResult Index()
        {
            var pcrs = db.pcrs.Include(p => p.Areas).Include(p => p.Clientes).Include(p => p.Originator).Include(p => p.Proyectos).Include(p => p.Reason);
            return View(pcrs.ToList());
        }

        // GET: pcrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pcr pcr = db.pcrs.Find(id);
            if (pcr == null)
            {
                return HttpNotFound();
            }
            return View(pcr);
        }

        // GET: pcrs/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente");
            ViewBag.OriginatorID = new SelectList(db.eoriginators, "ID", "Supervisor");
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto");
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason");
            return View();
        }

        // POST: pcrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OriginatorID,AreasID,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,docreason,docscope,doctypeofchange,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_purchasing,support_materials,support_maintenance,support_automation,support_quality,support_safety,support_environmental,support_tooling,support_stamping,support_welding,support_chrome,support_ecoat,support_topcoat,support_backcoat,support_assembly,support_finance,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,pcrrequestlvl,pcrverification,pcrdecision,pcrclientapproval,pcrclientdecision,pcrmanagerdecision,pcrmanagerclose")] pcr pcr)
        {
            if (ModelState.IsValid)
            {
                db.pcrs.Add(pcr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", pcr.AreasID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.OriginatorID = new SelectList(db.eoriginators, "ID", "Supervisor", pcr.OriginatorID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", pcr.ProyectosID);
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
            return View(pcr);
        }

        // GET: pcrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pcr pcr = db.pcrs.Find(id);
            if (pcr == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", pcr.AreasID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.OriginatorID = new SelectList(db.eoriginators, "ID", "Supervisor", pcr.OriginatorID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", pcr.ProyectosID);
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
            return View(pcr);
        }

        // POST: pcrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OriginatorID,AreasID,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,docreason,docscope,doctypeofchange,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_purchasing,support_materials,support_maintenance,support_automation,support_quality,support_safety,support_environmental,support_tooling,support_stamping,support_welding,support_chrome,support_ecoat,support_topcoat,support_backcoat,support_assembly,support_finance,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,pcrrequestlvl,pcrverification,pcrdecision,pcrclientapproval,pcrclientdecision,pcrmanagerdecision,pcrmanagerclose")] pcr pcr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pcr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", pcr.AreasID);
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.OriginatorID = new SelectList(db.eoriginators, "ID", "Supervisor", pcr.OriginatorID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", pcr.ProyectosID);
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
            return View(pcr);
        }

        // GET: pcrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pcr pcr = db.pcrs.Find(id);
            if (pcr == null)
            {
                return HttpNotFound();
            }
            return View(pcr);
        }

        // POST: pcrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pcr pcr = db.pcrs.Find(id);
            db.pcrs.Remove(pcr);
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

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
    public class MatrizDecisionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MatrizDecisions
        public ActionResult Index()
        {
            return View(db.MatrizDecisions.ToList());
        }

        // GET: MatrizDecisions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatrizDecision matrizDecision = db.MatrizDecisions.Find(id);
            if (matrizDecision == null)
            {
                return HttpNotFound();
            }
            return View(matrizDecision);
        }

        // GET: MatrizDecisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatrizDecisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NivelRiesgo,TipoCambio,commcliente,Arplanta,Aringenieria,Armanufactura,Arcalidad,Arfinanzas,Arcompras,Armateriales,Armantenimiento,Arseguridad,Arambiental,Artooling,Arestampado,Arsoldadura,Arcromo,Arpintura,Arensamble,Ppap,Drw,Spec,Pfd,Pfmea,Sw,Pcp,Is,Msa,Ps,Sccc,Pl,Cpo,Spo,Imds,Im,Bom,Pr,Mss,Va,Sm,Ptr,Pdr,PreStart")] MatrizDecision matrizDecision)
        {
            if (ModelState.IsValid)
            {
                db.MatrizDecisions.Add(matrizDecision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(matrizDecision);
        }

        // GET: MatrizDecisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatrizDecision matrizDecision = db.MatrizDecisions.Find(id);
            if (matrizDecision == null)
            {
                return HttpNotFound();
            }
            return View(matrizDecision);
        }

        // POST: MatrizDecisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NivelRiesgo,TipoCambio,commcliente,Arplanta,Aringenieria,Armanufactura,Arcalidad,Arfinanzas,Arcompras,Armateriales,Armantenimiento,Arseguridad,Arambiental,Artooling,Arestampado,Arsoldadura,Arcromo,Arpintura,Arensamble,Ppap,Drw,Spec,Pfd,Pfmea,Sw,Pcp,Is,Msa,Ps,Sccc,Pl,Cpo,Spo,Imds,Im,Bom,Pr,Mss,Va,Sm,Ptr,Pdr,PreStart")] MatrizDecision matrizDecision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matrizDecision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(matrizDecision);
        }

        // GET: MatrizDecisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatrizDecision matrizDecision = db.MatrizDecisions.Find(id);
            if (matrizDecision == null)
            {
                return HttpNotFound();
            }
            return View(matrizDecision);
        }

        // POST: MatrizDecisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MatrizDecision matrizDecision = db.MatrizDecisions.Find(id);
            db.MatrizDecisions.Remove(matrizDecision);
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

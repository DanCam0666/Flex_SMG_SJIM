using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using ClosedXML.Report;
using System.IO;
using Microsoft.AspNet.Identity;


namespace Flex_SGM.Controllers
{
    [Authorize]
    public class pcrsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // export

        public FileResult ExportFormat(int? id)
        {

            pcr pcrd = db.pcrs.Find(id);


            var user = User.Identity;

            //******************************
            var templatepath = Server.MapPath($"~/Evidence/Engineering/TemplatePCR.xlsx");

            var template = new XLTemplate(templatepath);

            templatepcr tempy = new templatepcr
            {
                pcrID=pcrd.PCRID,
                
                OriginatorID = pcrd.Originator,

                AreasID = pcrd.Department

      ,
                Date = pcrd.Date.ToString()


      ,
                ClientesID = pcrd.Clientes.Cliente


      ,
                ProyectosID = pcrd.Proyectos.Proyecto

      ,
                ReasonID = pcrd.Reason.Reason

      ,
                PartNumber = pcrd.PartNumber

      ,
                RevLevel = pcrd.RevLevel

      ,
                PartName = pcrd.PartName

      ,
                docreason = pcrd.docreason

      ,
                docscope = pcrd.docscope

      ,
                doctypeofchange = pcrd.MatrizDecision.TipoCambio

      ,
                cipieceprice = pcrd.cipieceprice.ToString()

      ,
                cicapital = pcrd.cicapital.ToString()

      ,
                citooling = pcrd.citooling.ToString()

      ,
                ciengineering = pcrd.ciengineering.ToString()

      ,
                cipackaging = pcrd.cipackaging.ToString()

      ,
                ciobsolescence = pcrd.ciobsolescence.ToString()

      ,
                cimaterial = pcrd.cimaterial.ToString()

      ,
                cifreight = pcrd.cifreight.ToString()

      ,
                ciovertime = pcrd.ciovertime.ToString()

      ,
                ciother = pcrd.ciother.ToString()

      ,
                citotal = pcrd.ciother.ToString()


      ,
                crannualvolume = pcrd.crannualvolume

      ,
                crcapacityfng = pcrd.crcapacityfng

      ,
                crcapacitysupplier = pcrd.crcapacitysupplier

      ,
                Reviewedby = pcrd.Reviewedby

      ,
                Reviewedby_date = pcrd.Reviewedby_date.ToString()

      ,
                support_purchasing = pcrd.support_purchasing

      ,
                support_materials = pcrd.support_materials

      ,
                support_maintenance = pcrd.support_maintenance

      ,
                support_automation = pcrd.support_automation

      ,
                support_quality = pcrd.support_quality

      ,
                support_safety = pcrd.support_safety

      ,
                support_environmental = pcrd.support_environmental

      ,
                support_tooling = pcrd.support_tooling

      ,
                support_stamping = pcrd.support_stamping

      ,
                support_welding = pcrd.support_welding

      ,
                support_chrome = pcrd.support_chrome

      ,
                support_ecoat = pcrd.support_ecoat

      ,
                support_topcoat = pcrd.support_topcoat

      ,
                support_backcoat = pcrd.support_backcoat

      ,
                support_assembly = pcrd.support_assembly

      ,
                support_finance = pcrd.support_finance

      ,
                Keymilestones_buildmrd1 = pcrd.Keymilestones_buildmrd1

      ,
                Keymilestones_buildmrd2 = pcrd.Keymilestones_buildmrd2

      ,
                Keymilestones_buildmrd3 = pcrd.Keymilestones_buildmrd3

      ,
                Keymilestones_customrrar = pcrd.Keymilestones_customrrar

      ,
                Keymilestones_ppap = pcrd.Keymilestones_ppap

      ,
                Keymilestones_internalsop = pcrd.Keymilestones_internalsop

      ,
                Keymilestones_customersop = pcrd.Keymilestones_customersop

      ,
                Keymilestones_closure = pcrd.Keymilestones_closure

      ,
                leadtime_engineering = pcrd.leadtime_engineering.ToString()

      ,
                leadtime_tooling = pcrd.leadtime_tooling.ToString()

      ,
                leadtime_facilities = pcrd.leadtime_facilities.ToString()

      ,
                leadtime_capital = pcrd.leadtime_capital.ToString()

      ,
                leadtime_material = pcrd.leadtime_material.ToString()

      ,
                leadtime_inventory = pcrd.leadtime_inventory.ToString()

      ,
                leadtime_approval = pcrd.leadtime_approval.ToString()

      ,
                leadtime_totallt = pcrd.leadtime_totallt.ToString()
      //*--------------*//



            };


            template.AddVariable(tempy);
            template.Generate();
            using (MemoryStream stream = new MemoryStream())
            {
                template.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Product_Process_Change_Request_" + pcrd.PCRID + ".xlsx");
            }


        }

        // GET: pcrs  
        [AllowAnonymous]
        public ActionResult Index()
        {
            var pcrs = db.pcrs.Include(p => p.Clientes).Include(p => p.MatrizDecision).Include(p => p.Proyectos).Include(p => p.Reason);
            return View(pcrs.ToList());
        }

        // GET: pcrs/Details/5
        [AllowAnonymous]
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
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(w => w.Id == id).FirstOrDefault();
            var users2 = db.Users.Where(w => w.Departamento == currentUser.Departamento);
            var users = users2.ToList();
           List <ApplicationUser> Gerentes = new List<ApplicationUser>();
         
            foreach (var user in users)
            {
                foreach (var userroles in user.Roles)
                {
                    if (userroles.RoleId == "7a269541-b9f5-4bfe-8eea-38c0ebe11373")
                        Gerentes.Add(user);


                }


            }
           

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente");
            ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio");
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto");
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason");
            ViewBag.GerentesID = new SelectList(Gerentes, "ID", "UserFullName");
            ViewBag.Originator = currentUser.UserFullName;
            ViewBag.Department = currentUser.Departamento;

            ViewBag.Reviewedby = Gerentes.FirstOrDefault().UserFullName;

            pcr mpcr = new pcr();

            mpcr.Date = DateTime.Now;
            mpcr.cicapital = 0.0;
            mpcr.ciengineering = 0.0;
            mpcr.cifreight = 0.0;
            mpcr.cimaterial = 0.0;
            mpcr.ciobsolescence = 0.0;
            mpcr.ciother = 0.0;
            mpcr.ciovertime = 0.0;
            mpcr.cipackaging = 0.0;
            mpcr.cipieceprice = 0.0;
            mpcr.citooling = 0.0;
            mpcr.citotal = 0.0;

            mpcr.leadtime_approval = 0.0;
            mpcr.leadtime_capital = 0.0;
            mpcr.leadtime_engineering = 0.0;
            mpcr.leadtime_facilities = 0.0;
            mpcr.leadtime_inventory = 0.0;
            mpcr.leadtime_material = 0.0;
            mpcr.leadtime_tooling = 0.0;
            mpcr.leadtime_totallt = 0.0;


            mpcr.FRisk8 = 0;
            return View(mpcr);
        }
    
        public ActionResult Matrizd(int Codigo)
        {
            // TODO: based on the selected
            var req = db.MatrizDecisions.Where(f => f.ID == Codigo).FirstOrDefault();
      


            return Json(new { r1 = req }, JsonRequestBehavior.AllowGet);
        }

        // POST: pcrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PCRID,Status,Originator,Department,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,docreason,docscope,MatrizDecisionID,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_purchasing,support_materials,support_maintenance,support_automation,support_quality,support_safety,support_environmental,support_tooling,support_stamping,support_welding,support_chrome,support_ecoat,support_topcoat,support_backcoat,support_assembly,support_finance,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,FConsiderations1,FConsiderations2,FConsiderations3,FConsiderations4,FConsiderations5,FConsiderations6,FConsiderations7,FConsiderations8,FConsiderations9,FConsiderations10,FConsiderations11,FConsiderations12,FConsiderations13,FConsiderations14,FConsiderations15,FRisk1,FRisk2,FRisk3,FRisk4,FRisk5,FRisk6,FRisk7,FRisk8")] pcr pcr)
        {
            var noPCRS = db.pcrs.Count();
            var dt = DateTime.Now;
            string year= dt.ToString("yy");
            string month= dt.Month.ToString("00");

            noPCRS = noPCRS + 1;
            string PCRID = "SJI-"+ year + "-"+ noPCRS.ToString("000") + "-M";

            if (ModelState.IsValid)
            {
                pcr.Status = "Revisión";
                pcr.PCRID = PCRID;
                db.pcrs.Add(pcr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

    
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
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
       
            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
            ViewBag.ProyectosID = new SelectList(db.cProyectos, "ID", "Proyecto", pcr.ProyectosID);
            ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
            return View(pcr);
        }

        // POST: pcrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PCRID,Status,Originator,Department,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,docreason,docscope,MatrizDecisionID,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_purchasing,support_materials,support_maintenance,support_automation,support_quality,support_safety,support_environmental,support_tooling,support_stamping,support_welding,support_chrome,support_ecoat,support_topcoat,support_backcoat,support_assembly,support_finance,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,FConsiderations1,FConsiderations2,FConsiderations3,FConsiderations4,FConsiderations5,FConsiderations6,FConsiderations7,FConsiderations8,FConsiderations9,FConsiderations10,FConsiderations11,FConsiderations12,FConsiderations13,FConsiderations14,FConsiderations15,FRisk1,FRisk2,FRisk3,FRisk4,FRisk5,FRisk6,FRisk7,FRisk8")] pcr pcr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pcr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientesID = new SelectList(db.cClientes, "ID", "Cliente", pcr.ClientesID);
            ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
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

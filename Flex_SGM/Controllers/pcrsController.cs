using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using ClosedXML.Report;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Flex_SGM.Controllers
{
    public class pcrsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //user management
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public pcrsController()
        {

        }
        public pcrsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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
                OriginatorID = pcrd.Originator.Supervisor,

                AreasID = pcrd.Areas.Area

      , Date = pcrd.Date.ToString()


      , ClientesID = pcrd.Clientes.Cliente


      , ProyectosID = pcrd.Proyectos.Proyecto

      , ReasonID = pcrd.Reason.Reason

      , PartNumber = pcrd.PartNumber

      , RevLevel = pcrd.RevLevel

      , PartName = pcrd.PartName

      , docreason = pcrd.docreason

      , docscope = pcrd.docscope

      , doctypeofchange = pcrd.doctypeofchange

      , cipieceprice = pcrd.cipieceprice.ToString()

      , cicapital= pcrd.cicapital.ToString()

      , citooling= pcrd.citooling.ToString()

      , ciengineering= pcrd.ciengineering.ToString()

      , cipackaging= pcrd.cipackaging.ToString()

      , ciobsolescence= pcrd.ciobsolescence.ToString()

      , cimaterial= pcrd.cimaterial.ToString()

      , cifreight= pcrd.cifreight.ToString()

      , ciovertime= pcrd.ciovertime.ToString()

      , ciother= pcrd.ciother.ToString()

      , citotal= pcrd.ciother.ToString()


      , crannualvolume= pcrd.crannualvolume

      , crcapacityfng= pcrd.crcapacityfng

      , crcapacitysupplier= pcrd.crcapacitysupplier

      , Reviewedby= pcrd.Reviewedby

      , Reviewedby_date = pcrd.Reviewedby_date.ToString()

      , support_purchasing = pcrd.support_purchasing

      , support_materials = pcrd.support_materials

      , support_maintenance = pcrd.support_maintenance

      , support_automation = pcrd.support_automation

      , support_quality = pcrd.support_quality

      , support_safety = pcrd.support_safety

      , support_environmental = pcrd.support_environmental

      , support_tooling = pcrd.support_tooling

      , support_stamping = pcrd.support_stamping

      , support_welding = pcrd.support_welding

      , support_chrome = pcrd.support_chrome

      , support_ecoat = pcrd.support_ecoat

      , support_topcoat = pcrd.support_topcoat

      , support_backcoat = pcrd.support_backcoat

      , support_assembly = pcrd.support_assembly

      , support_finance = pcrd.support_finance

      , Keymilestones_buildmrd1 = pcrd.Keymilestones_buildmrd1

      , Keymilestones_buildmrd2 = pcrd.Keymilestones_buildmrd2

      , Keymilestones_buildmrd3 = pcrd.Keymilestones_buildmrd3

      , Keymilestones_customrrar = pcrd.Keymilestones_customrrar

      , Keymilestones_ppap = pcrd.Keymilestones_ppap

      , Keymilestones_internalsop = pcrd.Keymilestones_internalsop

      , Keymilestones_customersop = pcrd.Keymilestones_customersop

      , Keymilestones_closure = pcrd.Keymilestones_closure

      , leadtime_engineering = pcrd.leadtime_engineering.ToString()

      , leadtime_tooling = pcrd.leadtime_tooling.ToString()

      , leadtime_facilities = pcrd.leadtime_facilities.ToString()

      , leadtime_capital = pcrd.leadtime_capital.ToString()

      , leadtime_material = pcrd.leadtime_material.ToString()

      , leadtime_inventory = pcrd.leadtime_inventory.ToString()

      , leadtime_approval = pcrd.leadtime_approval.ToString()

      , leadtime_totallt = pcrd.leadtime_totallt.ToString()
      //*--------------*//

      , pcrrequestlvl = pcrd.pcrrequestlvl

      , pcrverification = pcrd.pcrverification.ToString()
      , pcrdecision = pcrd.pcrdecision.ToString()

      , pcrclientapproval = pcrd.pcrclientapproval.ToString()

      , pcrclientdecision = pcrd.pcrclientdecision.ToString()

      , pcrmanagerdecision = pcrd.pcrmanagerdecision.ToString()

      , pcrmanagerclose = pcrd.pcrmanagerclose.ToString()

            };
       

            template.AddVariable(tempy);
            template.Generate();
            using (MemoryStream stream = new MemoryStream())
            {
                template.SaveAs(stream);
                return File(stream.ToArray(), "holotopo", "Product_Process_Change_Request_" + pcrd.ID.ToString() + ".xlsx");
            }


        }


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

            var userid = User.Identity.GetUserId();
            var users = UserManager.Users.ToList();

            List<ApplicationUser> Gerentes = new List<ApplicationUser>();


            foreach (var user in users)
            {
                foreach(var userroles in user.Roles )
                {
                    if (userroles.RoleId == "7a269541-b9f5-4bfe-8eea-38c0ebe11373")
                        Gerentes.Add(user);


                }


            }


            ViewBag.UsersID = new SelectList(db.ereasons, "ID", "Reason");

            pcr mpcr =new pcr();

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

            mpcr.pcrclientapproval = 0;
            mpcr.pcrclientdecision = 0;
            mpcr.pcrdecision = 0;
            mpcr.pcrmanagerclose = 0;
            mpcr.pcrmanagerdecision = 0;
            mpcr.pcrverification = 0;



            return View(mpcr);
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

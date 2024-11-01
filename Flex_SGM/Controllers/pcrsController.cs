﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using ClosedXML.Report;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNet.Identity;
using Flex_SGM.emaildata;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;

namespace Flex_SGM.Controllers
{
    [Authorize]
    public class PCRsController : Controller
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

        private EmailController correo = new EmailController();
		// export
		public FileResult ExportFormat(int? id)
		{
			// Fetch the PCR record
			var pcrd = db.PCRs.Find(id);
			if (pcrd == null)
			{
				return null;  // Handle the case where the PCR is not found
			}

			// Fetch the Maquina name based on the MaquinaID from the PCR record
			var maquinaName = db.Maquinas
								.Where(m => m.ID == pcrd.MaquinaID)
								.Select(m => m.Maquina)
								.FirstOrDefault() ?? "Not assigned";  // Default to "Not assigned" if no Maquina is found

			var user = User.Identity;

			//******************************
			var templatepath = Server.MapPath($"~/Templates/TemplatePCR.xlsx");
			var template = new XLTemplate(templatepath);

			templatepcr tempy = new templatepcr
			{
				pcrID = pcrd.PCRID,
				OriginatorID = pcrd.Originator,
				AreasID = pcrd.Department,
				Date = pcrd.Date.ToString(),
				ClientesID = pcrd.Clientes.Cliente,
				ProyectosID = pcrd.Proyectos.Proyecto,
				ReasonID = pcrd.Reason.Reason,
				PartNumber = pcrd.PartNumber,
				RevLevel = pcrd.RevLevel,
				CustAprovReqYes = pcrd.CustomerApprovalRequired ? "X" : "",
				CustAprovReqNo = !pcrd.CustomerApprovalRequired ? "X" : "",
				PartName = pcrd.PartName,
				Equipment = maquinaName,  // Use the fetched Maquina name here
                CurCondition = pcrd.CurrentCondition,
                NewCondition = pcrd.NewCondition,
				docreason = pcrd.docreason,
				docscope = pcrd.docscope,
				doctypeofchange = pcrd.MatrizDecision.TipoCambio,
				cipieceprice = pcrd.cipieceprice.ToString(),
				cicapital = pcrd.cicapital.ToString(),
				citooling = pcrd.citooling.ToString(),
				ciengineering = pcrd.ciengineering.ToString(),
				cipackaging = pcrd.cipackaging.ToString(),
				ciobsolescence = pcrd.ciobsolescence.ToString(),
				cimaterial = pcrd.cimaterial.ToString(),
				cifreight = pcrd.cifreight.ToString(),
				ciovertime = pcrd.ciovertime.ToString(),
				ciother = pcrd.ciother.ToString(),
				citotal = pcrd.citotal.ToString(),
				crannualvolume = pcrd.crannualvolume,
				crcapacityfng = pcrd.crcapacityfng,
				crcapacitysupplier = pcrd.crcapacitysupplier,
				BuildOutCurrent = pcrd.BuildOut,
				Reviewedby = pcrd.Reviewedby,
				Reviewedby_date = pcrd.Reviewedby_date?.ToString("dd/MM/yyyy HH:mm") ?? "",
				support_Compras = pcrd.support_Compras,
				support_Materiales = pcrd.support_Materiales,
				support_Mantenimiento = pcrd.support_Mantenimiento,
				support_Automatizacion = pcrd.support_Automatizacion,
				support_Calidad = pcrd.support_Calidad,
				support_Seguridad = pcrd.support_Seguridad,
				support_Ambiental = pcrd.support_Ambiental,
				support_Tooling = pcrd.support_Tooling,
				support_Estampado = pcrd.support_Estampado,
				support_Soldadura = pcrd.support_Soldadura,
				support_Chromo = pcrd.support_Chromo,
				support_Ecoat = pcrd.support_Ecoat,
				support_Topcoat = pcrd.support_Topcoat,
				support_Backcoat = pcrd.support_Backcoat,
				support_Ensamble = pcrd.support_Ensamble,
				support_Finanzas = pcrd.support_Finanzas,
				Keymilestones_buildmrd1 = pcrd.Keymilestones_buildmrd1,
				Keymilestones_buildmrd2 = pcrd.Keymilestones_buildmrd2,
				Keymilestones_buildmrd3 = pcrd.Keymilestones_buildmrd3,
				Keymilestones_customrrar = pcrd.Keymilestones_customrrar,
				Keymilestones_ppap = pcrd.Keymilestones_ppap,
				Keymilestones_internalsop = pcrd.Keymilestones_internalsop,
				Keymilestones_customersop = pcrd.Keymilestones_customersop,
				Keymilestones_closure = pcrd.Keymilestones_closure,
				leadtime_engineering = pcrd.leadtime_engineering.ToString(),
				leadtime_tooling = pcrd.leadtime_tooling.ToString(),
				leadtime_facilities = pcrd.leadtime_facilities.ToString(),
				leadtime_capital = pcrd.leadtime_capital.ToString(),
				leadtime_material = pcrd.leadtime_material.ToString(),
				leadtime_inventory = pcrd.leadtime_inventory.ToString(),
				leadtime_approval = pcrd.leadtime_approval.ToString(),
				leadtime_totallt = pcrd.leadtime_totallt.ToString()
			};

			template.AddVariable(tempy);
			template.Generate();

			// Protect the first (and only) worksheet with a password
			string protectionPassword = "9876543210"; // Replace with your desired password
			var worksheet = template.Workbook.Worksheets.First();

			// Ensure all cells are locked
			foreach (var cell in worksheet.CellsUsed())
			{
				cell.Style.Protection.Locked = true;
			}

			worksheet.Protect(protectionPassword);

			// Use a temporary file with a .xlsx extension
			var tempFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
			template.Workbook.SaveAs(tempFileName);

			// Now, read the file into a memory stream
			byte[] fileBytes;
			using (FileStream fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read))
			{
				using (MemoryStream ms = new MemoryStream())
				{
					fs.CopyTo(ms);
					fileBytes = ms.ToArray();
				}
			}

			// Clean up the temporary file
			System.IO.File.Delete(tempFileName);

			// Return the Excel file
			return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
						"Process_Change_Request_" + pcrd.PCRID + ".xlsx");
		}

		// GET: PCRs/Review/5
		[Authorize(Roles = "Admin, Gerente")]
		public ActionResult Review(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Fetch the PCR record
			var pcr = db.PCRs
				.Include(p => p.Clientes)       // Include related Cliente
				.Include(p => p.Proyectos)      // Include related Proyecto
				.Include(p => p.Reason)         // Include related Reason
				.Include(p => p.MatrizDecision) // Include MatrizDecision for risk level
				.FirstOrDefault(p => p.ID == id);

			if (pcr == null)
			{
				return HttpNotFound();
			}

			// Fetch the Maquina name if available
			var maquina = db.Maquinas.FirstOrDefault(m => m.ID == pcr.MaquinaID);
			ViewBag.Equipo = maquina != null ? maquina.Maquina : "Not assigned";

			// Get the current user information
			var userId = User.Identity.GetUserId();
			var currentUser = db.Users.FirstOrDefault(u => u.Id == userId);

			// Set ViewBag properties for user information and roles
			ViewBag.cUser = currentUser?.UserFullName;
			ViewBag.Dep = currentUser?.Departamento;
			ViewBag.Puesto = currentUser?.Puesto;
			ViewBag.Risk = pcr.MatrizDecision?.NivelRiesgo;

			// Pass necessary data to the view
			return View(pcr);
		}

		// GET: PCRs  
		public ActionResult Index()
        {
            var uiid = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(w => w.Id == uiid).FirstOrDefault();

            ViewBag.Admin = false;
            if (currentUser!=null)
            if (currentUser.Puesto.Contains("Gerente"))
            ViewBag.Admin = true;

            var Id = User.Identity.GetUserId();
            ApplicationUser CurrentUser = UserManager.FindById(Id);
            ViewBag.Dep = CurrentUser.Departamento;
            ViewBag.Puesto = CurrentUser.Puesto;
            ViewBag.cUser = CurrentUser.UserFullName;

            ViewBag.ReqAmbiental = db.PCRs.Where(r => r.support_Ambiental != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqCalidad = db.PCRs.Where(r => r.support_Calidad != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqCompras = db.PCRs.Where(r => r.support_Compras != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqFinanzas = db.PCRs.Where(r => r.support_Finanzas != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqMantenimiento = db.PCRs.Where(r => r.support_Mantenimiento != "X")
                .Where(r => r.support_Automatizacion != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqMateriales = db.PCRs.Where(r => r.support_Materiales != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqSeguridad = db.PCRs.Where(r => r.support_Seguridad != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqTooling = db.PCRs.Where(r => r.support_Tooling != "X").Where(r => r.Status == "Aprobado").Count();
            ViewBag.ReqProduccion = db.PCRs.Where(r => r.support_Backcoat != "X").Where(r => r.support_Chromo != "X")
                .Where(r => r.support_Ecoat != "X").Where(r => r.support_Ensamble != "X").Where(r => r.support_Estampado != "X")
                .Where(r => r.support_Soldadura != "X").Where(r => r.support_Topcoat != "X").Where(r => r.Status == "Aprobado").Count();

            ViewBag.Ambiental = db.PCRs.Where(f => f.support_Ambiental == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Calidad = db.PCRs.Where(f => f.support_Calidad == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Compras = db.PCRs.Where(f => f.support_Compras == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Finanzas = db.PCRs.Where(f => f.support_Finanzas == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Mantenimiento = db.PCRs.Where(f => f.support_Mantenimiento == "P")
                .Where(f => f.support_Automatizacion == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Materiales = db.PCRs.Where(f => f.support_Materiales == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Seguridad = db.PCRs.Where(f => f.support_Seguridad == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Tooling = db.PCRs.Where(f => f.support_Tooling == "P").Where(f => f.Status == "Aprobado").Count();
            ViewBag.Produccion = db.PCRs.Where(f => f.support_Backcoat == "P").Where(f => f.support_Chromo == "P")
                .Where(f => f.support_Ecoat == "P").Where(f => f.support_Ensamble == "P").Where(f => f.support_Estampado == "P")
                .Where(f => f.support_Soldadura == "P").Where(f => f.support_Topcoat == "P").Where(f => f.Status == "Aprobado").Count();

            var DateLess = DateTime.Today.AddDays(-7);

            ViewBag.TarAmbiental = db.PCRs.Where(t => t.support_Ambiental == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarCalidad = db.PCRs.Where(t => t.support_Calidad == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarCompras = db.PCRs.Where(t => t.support_Compras == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarFinanzas = db.PCRs.Where(t => t.support_Finanzas == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarMantenimiento = db.PCRs.Where(t => t.support_Mantenimiento == "P")
                .Where(t => t.support_Automatizacion == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarMateriales = db.PCRs.Where(t => t.support_Materiales == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarSeguridad = db.PCRs.Where(t => t.support_Seguridad == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarTooling = db.PCRs.Where(t => t.support_Tooling == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();
            ViewBag.TarProduccion = db.PCRs.Where(t => t.support_Backcoat == "P").Where(t => t.support_Chromo == "P")
                .Where(t => t.support_Ecoat == "P").Where(t => t.support_Ensamble == "P").Where(t => t.support_Estampado == "P")
                .Where(t => t.support_Soldadura == "P").Where(t => t.support_Topcoat == "P").Where(t => t.Status == "Aprobado").Where(t => t.Date <= DateLess).Count();

            ViewBag.PcrComplete = 0;
            if (ViewBag.Ambiental == 0 && ViewBag.Calidad == 0 && ViewBag.Compras == 0 && ViewBag.Finanzas == 0 && ViewBag.Mantenimiento == 0 && 
                ViewBag.Materiales == 0 && ViewBag.Produccion == 0 && ViewBag.Seguridad == 0 && ViewBag.Tooling == 0)
            {
                ViewBag.PcrComplete = 1;
            }
            var PCRs = db.PCRs.Include(p => p.Clientes).Include(p => p.MatrizDecision).Include(p => p.Proyectos).Include(p => p.Reason);

            return View(PCRs.ToList());
        }

		// GET: PCRs/Details/5
		[AllowAnonymous]
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Get the current user and their information
			var uiid = User.Identity.GetUserId();
			var currentUser = db.Users.FirstOrDefault(u => u.Id == uiid);

			ViewBag.Admin = false;
			if (currentUser != null && currentUser.Puesto.Contains("Gerente"))
			{
				ViewBag.Admin = true;
			}

			var currentUserFull = UserManager.FindById(uiid);
			ViewBag.Dep = currentUserFull.Departamento;
			ViewBag.Puesto = currentUserFull.Puesto;
			ViewBag.cUser = currentUserFull.UserFullName;

			// Fetch the PCR record
			var pcr = db.PCRs.Find(id);
			if (pcr == null)
			{
				return HttpNotFound();
			}

			// Fetch and set the MatrizDecision risk level if available
			ViewBag.Risk = pcr.MatrizDecision?.NivelRiesgo;

			// Fetch the Maquina name based on MaquinaID
			var maquina = db.Maquinas.FirstOrDefault(m => m.ID == pcr.MaquinaID);
			ViewBag.Equipo = maquina != null ? maquina.Maquina : "Not assigned";

			// Fetch related FeasibilitySigns
			var signs = db.FeasibilitySigns.Where(w => w.pcrID == id).ToList();

			// Use ViewBag to pass the signs if needed in the view
			ViewBag.Signs = signs;

			return View(pcr);  // Return the PCR object as the model
		}

		// GET: PCRs/Create
		public ActionResult Create()
		{
			var id = User.Identity.GetUserId();
			ApplicationUser currentUser = db.Users.FirstOrDefault(w => w.Id == id);
			var users = db.Users.Where(w => w.Departamento == currentUser.Departamento).ToList();

			List<ApplicationUser> Gerentes = new List<ApplicationUser>();
			foreach (var user in users)
			{
				foreach (var userRole in user.Roles)
				{
					if (userRole.RoleId == "8dcec765-580a-4b6e-9454-b7af0c4ee717" || userRole.RoleId == "96179ad6-83ef-4aa2-a9e7-1837f8df338f")
						Gerentes.Add(user);
				}
			}

			ViewBag.AreasID = new SelectList(db.eAreas, "ID", "Area");
			ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente");
			ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio");
			ViewBag.ProyectosID = new SelectList(db.eProyectos, "ID", "Proyecto");
			ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason");
			ViewBag.GerentesID = new SelectList(Gerentes, "ID", "UserFullName");
			ViewBag.Originator = currentUser.UserFullName;
			ViewBag.Department = currentUser.Departamento;
			ViewBag.MaquinaID = new SelectList(db.Maquinas, "ID", "Maquina");

			pcr mpcr = new pcr
			{
				Date = DateTime.Now,
				cicapital = 0.0,
				ciengineering = 0.0,
				cifreight = 0.0,
				cimaterial = 0.0,
				ciobsolescence = 0.0,
				ciother = 0.0,
				ciovertime = 0.0,
				cipackaging = 0.0,
				cipieceprice = 0.0,
				citooling = 0.0,
				citotal = 0.0,
				leadtime_approval = 0.0,
				leadtime_capital = 0.0,
				leadtime_engineering = 0.0,
				leadtime_facilities = 0.0,
				leadtime_inventory = 0.0,
				leadtime_material = 0.0,
				leadtime_tooling = 0.0,
				leadtime_totallt = 0.0,
				FRisk8 = 0,
				CurrentCondition = string.Empty,
				NewCondition = string.Empty,
				CustomerApprovalRequired = false
			};

			return View(mpcr);
		}
		public ActionResult Matrizd(int Codigo)
        {
            // TODO: based on the selected
            var req = db.MatrizDecisions.Where(f => f.ID == Codigo).FirstOrDefault();
            return Json(new { r1 = req }, JsonRequestBehavior.AllowGet);
        }

		// POST: PCRs/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(
			[Bind(Include = "ID,PCRID,Status,Originator,Department,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,MaquinaID,BuildOut,docreason,docscope,MatrizDecisionID,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_Compras,support_Materiales,support_Mantenimiento,support_Automatizacion,support_Calidad,support_Seguridad,support_Ambiental,support_Tooling,support_Estampado,support_Soldadura,support_Chromo,support_Ecoat,support_Topcoat,support_Backcoat,support_Ensamble,support_Finanzas,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,FConsiderations1,FConsiderations2,FConsiderations3,FConsiderations4,FConsiderations5,FConsiderations6,FConsiderations7,FConsiderations8,FConsiderations9,FConsiderations10,FConsiderations11,FConsiderations12,FConsiderations13,FConsiderations14,FConsiderations15,FRisk1,FRisk2,FRisk3,FRisk4,FRisk5,FRisk6,FRisk7,FRisk8,CurrentCondition,NewCondition,CustomerApprovalRequired")] pcr pcr)
		{
			if (ModelState.IsValid)
			{
				var id = User.Identity.GetUserId();
				ApplicationUser currentUser = db.Users.FirstOrDefault(w => w.Id == id);

				// Get the current year
				var dt = DateTime.Now;
				string currentYear = dt.ToString("yy");

				// Fetch the last PCR entry (if any)
				var lastPCR = db.PCRs
					.OrderByDescending(p => p.ID)  // Ensure the latest entry is fetched
					.FirstOrDefault();

				int noPCRs = 1;  // Default to 1 if no PCR exists or if it's a new year

				if (lastPCR != null)
				{
					// Extract the year and sequence number from the last PCRID
					string lastPCRYear = lastPCR.PCRID.Substring(4, 2);
					string lastPCRnum = lastPCR.PCRID.Substring(7, 3);

					// Check if the last PCR's year matches the current year
					if (lastPCRYear == currentYear)
					{
						noPCRs = Int32.Parse(lastPCRnum) + 1;  // Increment the sequence number
					}
				}

				// Create the new PCRID
				string PCRID = $"SJI-{currentYear}-{noPCRs:000}-M";

				// Set the PCR properties
				pcr.Status = "En Aprobación";
				pcr.PCRID = PCRID;
				pcr.Date = DateTime.Now;

				// Save the new PCR to the database
				db.PCRs.Add(pcr);
				db.SaveChanges();

				// Send email notification
				string[] emails = { "dcamacho@flexngate.com", currentUser.Email };
				correo.newpcr(emails, currentUser.UserFullName, pcr.PCRID, pcr.ID.ToString());

				return RedirectToAction("Index");
			}

			// Reload the dropdowns if the model state is not valid
			ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", pcr.ClientesID);
			ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
			ViewBag.ProyectosID = new SelectList(db.eProyectos, "ID", "Proyecto", pcr.ProyectosID);
			ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
			ViewBag.MaquinaID = new SelectList(db.Maquinas, "ID", "Maquina", pcr.MaquinaID);

			return View(pcr);
		}

		// GET: PCRs/Signatures/5
		[Authorize(Roles = "Admin, Gerente")]
		public ActionResult Signatures(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			// Fetch the PCR record to confirm it exists
			var pcr = db.PCRs.FirstOrDefault(p => p.ID == id);
			if (pcr == null)
			{
				return HttpNotFound();
			}

			// Fetch all related FeasibilitySigns for the given PCR ID
			var signs = db.FeasibilitySigns
						  .Where(s => s.pcrID == id)
						  .ToList();

			if (signs == null || !signs.Any())
			{
				ViewBag.Message = "No signatures found for this PCR.";
			}

			// Fetch Maquina name if available
			var maquina = db.Maquinas.FirstOrDefault(m => m.ID == pcr.MaquinaID);
			ViewBag.Equipo = maquina != null ? maquina.Maquina : "Not assigned";

			// Get the current user information
			var userId = User.Identity.GetUserId();
			var currentUser = db.Users.FirstOrDefault(u => u.Id == userId);

			// Set ViewBag properties for user and PCR information
			ViewBag.PCRID = pcr.PCRID;
			ViewBag.cUser = currentUser?.UserFullName;
			ViewBag.Dep = currentUser?.Departamento;
			ViewBag.Puesto = currentUser?.Puesto;

			return View(signs);  // Pass the list of signatures to the view
		}

		[Authorize(Roles = "Admin, Gerente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Firma(int id, string Response,string msg, string datos)
        {
            var Id = User.Identity.GetUserId();
            ApplicationUser CurrentUser = UserManager.FindById(Id);
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            string cudep = "xxx";
            if (CurrentUser != null)
            {
                cuser = CurrentUser.UserFullName;
                cpuesto = CurrentUser.Puesto;
                cuare = CurrentUser.Area;
                cudep = CurrentUser.Departamento;
            }

            var uiid = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.Where(w => w.Id == uiid).FirstOrDefault();
            var ok = false;
           foreach(var rol in currentUser.Roles)
            {
                if (rol.RoleId == "8dcec765-580a-4b6e-9454-b7af0c4ee717" || rol.RoleId == "96179ad6-83ef-4aa2-a9e7-1837f8df338f")
                {
                    ok = true;
                }
            }
            if (ok)  //8dcec765-580a-4b6e-9454-b7af0c4ee717 (Admin) or 96179ad6-83ef-4aa2-a9e7-1837f8df338f (Gerente)
            {
                if (datos!=null&& msg!=null)
                if (!string.IsNullOrEmpty(Response) && id != 0)
                {
                    pcr pcr = db.PCRs.Find(id);
                    FeasibilitySigns sign = new FeasibilitySigns();

                    var signs = db.FeasibilitySigns.Where(w => w.pcrID == id);

                    foreach (var minisign in signs)
                    {
                        if (minisign.Dep == currentUser.Departamento && minisign.Status != "Necesita Arreglos")
                            goto alreadysin;
                    }

                    switch (Response) 
                    {
                        case ("Aceptar"):
                                if (pcr.Status == "En Aprobación" && cudep == "Ingenieria" && cpuesto == "Gerente")
                                {
                                    pcr.Reviewedby = cuser;
                                    pcr.Reviewedby_date = DateTime.Now;
                                    pcr.Status = "Aprobado";
                                    sign.msg = msg;
                                    sign.Reviewedby_date = DateTime.Now;
                                    sign.pcrID = id;
                                    sign.Status = "Aprobado por " + currentUser.Departamento;
                                    sign.Reviewedby = cuser;
                                    sign.Dep = currentUser.Departamento;
                                    Id = id.ToString();


                                    // Send the email to autorization personal 
                                    var Managers = db.Users.ToList<ApplicationUser>();
                                    var managersList = Managers.Where(m => m.Puesto == "Gerente").ToList();

                                    foreach (var manager in managersList)
                                    {
                                        if (manager.Departamento == "Compras" && pcr.support_Compras == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Materiales" && pcr.support_Materiales == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Mantenimiento" && (pcr.support_Mantenimiento == "P" || pcr.support_Automatizacion == "P"))
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Calidad" && pcr.support_Calidad == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }
                                            
                                        if (manager.Departamento == "Seguridad" && pcr.support_Seguridad == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Ambiental" && pcr.support_Ambiental == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Tooling" && pcr.support_Tooling == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Produccion" && (pcr.support_Estampado == "P" || pcr.support_Soldadura == "P" || pcr.support_Chromo == "P" || 
                                            pcr.support_Ecoat == "P" || pcr.support_Topcoat == "P" || pcr.support_Backcoat == "P" || pcr.support_Ensamble == "P"))
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }

                                        if (manager.Departamento == "Finanzas" && pcr.support_Finanzas == "P")
                                        {
                                            string[] eMail = { manager.Email };
                                            correo.newReview(eMail, currentUser.UserFullName, pcr.PCRID, Id, manager.Departamento);
                                        }
                                    }
                                }
                                else
                                if (pcr.Status == "Aprobado")
                                {
                                    sign.msg = msg;
                                    sign.Reviewedby_date = DateTime.Now;
                                    sign.pcrID = id;
                                    sign.Status = "Aprobado por " + currentUser.Departamento;
                                    sign.Reviewedby = cuser;
                                    sign.Dep = currentUser.Departamento;
                           
                                    switch (currentUser.Departamento)
                                    {
                                        case ("Calidad"):
                                            if (pcr.support_Calidad == "P") pcr.support_Calidad = "R";
                                            break;
                                        case ("Finanzas"):
                                            if (pcr.support_Finanzas == "P") pcr.support_Finanzas = "R";
                                            break;
                                        case ("Compras"):
                                            if (pcr.support_Compras == "P") pcr.support_Compras = "R";
                                            break;
                                        case ("Materiales"):
                                            if (pcr.support_Materiales == "P") pcr.support_Materiales = "R";
                                            break;
                                        case ("Mantenimiento"):
                                            if (pcr.support_Mantenimiento == "P") pcr.support_Mantenimiento = "R";
                                            if (pcr.support_Automatizacion == "P") pcr.support_Automatizacion = "R";
                                            break;
                                        case ("Seguridad"):
                                            if (pcr.support_Seguridad == "P") pcr.support_Seguridad = "R";
                                            break;
                                        case ("Ambiental"):
                                            if (pcr.support_Ambiental == "P") pcr.support_Ambiental = "R";
                                            break;
                                        case ("Tooling"):
                                            if (pcr.support_Tooling == "P") pcr.support_Tooling = "R";
                                            break;
                                        case ("Produccion"):
                                            if (pcr.support_Estampado == "P") pcr.support_Estampado = "R";
                                            if (pcr.support_Soldadura == "P") pcr.support_Soldadura = "R";
                                            if (pcr.support_Chromo == "P") pcr.support_Chromo = "R";
                                            if (pcr.support_Topcoat == "P") pcr.support_Topcoat = "R";
                                            if (pcr.support_Ensamble == "P") pcr.support_Ensamble = "R";
                                            if (pcr.support_Ecoat == "P") pcr.support_Ecoat = "R";
                                            if (pcr.support_Backcoat == "P") pcr.support_Backcoat = "R";
                                            break;
                                    }
                                }
                                else
                                    pcr.Status = "En Aprobación";

                                break;
                        case ("Arreglos"):
                                if (pcr.Status == "En Aprobación")
                                {
                                    pcr.Status = "Necesita Arreglos";
                                        // Send the email to autorization personal 
                                    //    foreach (var minisign in signs)
                                    //     {
                                        //         if (minisign.Dep == currentUser.Departamento)
                                        //             goto alreadysin;
                                        //     }

                                    sign.msg = msg;
                                    sign.Reviewedby_date = DateTime.Now;
                                    sign.pcrID = id;
                                    sign.Status = "Necesita Arreglos";
                                    sign.Reviewedby = cuser;
                                    sign.Dep = currentUser.Departamento;

                                    var UserList = db.Users.Where(w => w.UserFullName == pcr.Originator).FirstOrDefault();
                                    string email = UserList.Email.ToString();
                                    string[] eMail = { email };
                                    string emailId = sign.pcrID.ToString();

                                    correo.Arreglos(eMail, currentUser.UserFullName, pcr.PCRID, emailId);

                                }
                                else
                                if (pcr.Status == "Aprobado")
                                {
                                    pcr.Status = currentUser.UserFullName + " necesita arreglos";

                                        sign.msg = msg;
                                        sign.Reviewedby_date = DateTime.Now;
                                        sign.pcrID = id;
                                        sign.Status = "Necesita Arreglos";
                                        sign.Reviewedby = cuser;
                                        sign.Dep = currentUser.UserFullName;
                                    }
                                else
                                    pcr.Status = "En Aprobación";
                                break;
                        case ("Rechazar"):
                                if (pcr.Status == "En Aprobación")
                                {
                                    pcr.Status = "Rechazado";
                                    sign.msg = msg;
                                    sign.Reviewedby_date = DateTime.Now;
                                    sign.pcrID = id;
                                    sign.Status = "Rechazado";
                                    sign.Reviewedby = cuser;
                                    sign.Dep = currentUser.Departamento;

                                    var UserList = db.Users.Where(w => w.UserFullName == pcr.Originator).FirstOrDefault();
                                    string email = UserList.Email.ToString();
                                    string[] eMail = { email };
                                    string emailId = sign.pcrID.ToString();

                                    correo.Rechazado(eMail, currentUser.UserFullName, pcr.PCRID, emailId);
                                }
                                else
                                    pcr.Status = "En Aprobación";
                                break;
                        default:
                                pcr.Status = currentUser.UserFullName + " Canceló";
                                sign.msg = msg;
                                sign.Reviewedby_date = DateTime.Now;
                                sign.pcrID = id;
                                sign.Status = "Cancelado";
                                sign.Reviewedby = cuser;
                                pcr.Reviewedby = cuser;
                                sign.Dep = currentUser.Departamento;
                                break;
                    };

                    db.Entry(pcr).State = EntityState.Modified;
                        if (sign.pcrID != 0)
                        {
                            db.FeasibilitySigns.Add(sign);
                        }
                    db.SaveChanges();
                    return Response;
                }
            }
            return "No está permitido... nada ha cambiado...";
            alreadysin: 
            return "Su departamento ya ha firmado éste PCR ... ";
        }

		// GET: PCRs/Edit/5
		[Authorize(Roles = "Admin, Gerente")]
		public ActionResult Edit(int? id, bool isReview = false)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			var uiid = User.Identity.GetUserId();
			ApplicationUser currentUser = db.Users.FirstOrDefault(w => w.Id == uiid);

			ViewBag.Admin = false;
			if (currentUser != null && currentUser.Puesto.Contains("Gerente"))
			{
				ViewBag.Admin = true;
			}

			pcr pcr = db.PCRs.Find(id);
			if (pcr == null)
			{
				return HttpNotFound();
			}

			// Common SelectLists
			ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", pcr.ClientesID);
			ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
			ViewBag.ProyectosID = new SelectList(db.eProyectos, "ID", "Proyecto", pcr.ProyectosID);
			ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);
			ViewBag.MaquinaID = new SelectList(db.Maquinas, "ID", "Maquina", pcr.MaquinaID);
			ViewBag.Risk = pcr.MatrizDecision?.NivelRiesgo;

			if (isReview)
			{
				ViewBag.Dep = currentUser?.Departamento;
				ViewBag.Puesto = currentUser?.Puesto;
				ViewBag.cUser = currentUser?.UserFullName;
			}

			return View(pcr);
		}


		// POST: PCRs/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Gerente")]
		public ActionResult Edit(
			[Bind(Include = "ID,PCRID,Status,Originator,Department,Date,ClientesID,ProyectosID,ReasonID,PartNumber,RevLevel,PartName,MaquinaID,BuildOut,docreason,docscope,MatrizDecisionID,cipieceprice,cicapital,citooling,ciengineering,cipackaging,ciobsolescence,cimaterial,cifreight,ciovertime,ciother,citotal,crannualvolume,crcapacityfng,crcapacitysupplier,Reviewedby,Reviewedby_date,support_Compras,support_Materiales,support_Mantenimiento,support_Automatizacion,support_Calidad,support_Seguridad,support_Ambiental,support_Tooling,support_Estampado,support_Soldadura,support_Chromo,support_Ecoat,support_Topcoat,support_Backcoat,support_Ensamble,support_Finanzas,Keymilestones_buildmrd1,Keymilestones_buildmrd2,Keymilestones_buildmrd3,Keymilestones_customrrar,Keymilestones_ppap,Keymilestones_internalsop,Keymilestones_customersop,Keymilestones_closure,leadtime_engineering,leadtime_tooling,leadtime_facilities,leadtime_capital,leadtime_material,leadtime_inventory,leadtime_approval,leadtime_totallt,FConsiderations1,FConsiderations2,FConsiderations3,FConsiderations4,FConsiderations5,FConsiderations6,FConsiderations7,FConsiderations8,FConsiderations9,FConsiderations10,FConsiderations11,FConsiderations12,FConsiderations13,FConsiderations14,FConsiderations15,FRisk1,FRisk2,FRisk3,FRisk4,FRisk5,FRisk6,FRisk7,FRisk8,CurrentCondition,NewCondition,CustomerApprovalRequired")] pcr pcr)
		{
			var Id = User.Identity.GetUserId();
			ApplicationUser CurrentUser = UserManager.FindById(Id);

			if (ModelState.IsValid)
			{
				pcr.Status = "En Aprobación";
				pcr.Date = DateTime.Now;

				string[] eMail = { "dcamacho@flexngate.com" };
				string emailId = pcr.ID.ToString();

				correo.Arreglado(eMail, CurrentUser.UserFullName, pcr.PCRID, emailId);

				using (SqlConnection connection = new SqlConnection("Data Source=SJIMSVAP7\\SQLEXPRESS;Initial Catalog=SGM_SJI;Integrated Security=False;User ID=monitor;Password=M0n1t0r@F13x;Connect Timeout=20;Encrypt=False;TrustServerCertificate=False"))
				{
					connection.Open();
					SqlCommand commandDelete = new SqlCommand("DELETE FROM FeasibilitySigns WHERE pcrID = " + pcr.ID, connection);
					commandDelete.ExecuteNonQuery();
				}

				db.Entry(pcr).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", pcr.ClientesID);
			ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
			ViewBag.ProyectosID = new SelectList(db.eProyectos, "ID", "Proyecto", pcr.ProyectosID);
			ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);

			return View(pcr);
		}

		// GET: PCRs/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pcr pcr = db.PCRs.Find(id);
            if (pcr == null)
            {
                return HttpNotFound();
            }
            return View(pcr);
        }

        // POST: PCRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pcr pcr = db.PCRs.Find(id);
            db.PCRs.Remove(pcr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposign)
        {
            if (disposign)
            {
                db.Dispose();
            }
            base.Dispose(disposign);
        }
    }
}

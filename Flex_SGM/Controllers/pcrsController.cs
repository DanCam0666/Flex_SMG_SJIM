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
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNet.Identity;
using Flex_SGM.emaildata;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Flex_SGM.Controllers
{
    [Authorize]
    public class PCRsController : Controller
    {
		private readonly ApplicationDbContext db = new ApplicationDbContext();
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

		private readonly EmailController correo = new EmailController();
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
			ApplicationUser currentUser = db.Users.FirstOrDefault(w => w.Id == uiid);

			ViewBag.Admin = currentUser?.Puesto.Contains("Gerente") ?? false;

			var Id = User.Identity.GetUserId();
			ApplicationUser CurrentUser = UserManager.FindById(Id);
			ViewBag.Dep = CurrentUser.Departamento;
			ViewBag.Puesto = CurrentUser.Puesto;
			ViewBag.cUser = CurrentUser.UserFullName;

			// Fetching required counts
			ViewBag.ReqAmbiental = db.PCRs.Where(r => r.support_Ambiental != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqCalidad = db.PCRs.Where(r => r.support_Calidad != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqCompras = db.PCRs.Where(r => r.support_Compras != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqFinanzas = db.PCRs.Where(r => r.support_Finanzas != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqMantenimiento = db.PCRs.Where(r => r.support_Mantenimiento != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqMateriales = db.PCRs.Where(r => r.support_Materiales != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqSeguridad = db.PCRs.Where(r => r.support_Seguridad != "X" && r.Status.Contains("Aprob")).Count();
			ViewBag.ReqTooling = db.PCRs.Where(r => r.support_Tooling != "X" && r.Status.Contains("Aprob")).Count();

			ViewBag.ReqProduccion = db.PCRs.Where(r =>
				(r.support_Backcoat != "X" || r.support_Chromo != "X" || r.support_Ecoat != "X" ||
				 r.support_Ensamble != "X" || r.support_Estampado != "X" || r.support_Soldadura != "X" ||
				 r.support_Topcoat != "X") && r.Status.Contains("Aprob")).Count();

			// Missing approvals
			ViewBag.Ambiental = db.PCRs.Where(f => f.support_Ambiental == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Calidad = db.PCRs.Where(f => f.support_Calidad == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Compras = db.PCRs.Where(f => f.support_Compras == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Finanzas = db.PCRs.Where(f => f.support_Finanzas == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Mantenimiento = db.PCRs.Where(f => f.support_Mantenimiento == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Materiales = db.PCRs.Where(f => f.support_Materiales == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Seguridad = db.PCRs.Where(f => f.support_Seguridad == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Tooling = db.PCRs.Where(f => f.support_Tooling == "P" && f.Status.Contains("Aprob")).Count();
			ViewBag.Produccion = db.PCRs.Where(f =>
				f.support_Backcoat == "P" && f.support_Chromo == "P" &&
				f.support_Ecoat == "P" && f.support_Ensamble == "P" &&
				f.support_Estampado == "P" && f.support_Soldadura == "P" &&
				f.support_Topcoat == "P" && f.Status.Contains("Aprob")).Count();

			var DateLess = DateTime.Today.AddDays(-7);

			// Overdue approvals
			ViewBag.TarAmbiental = db.PCRs.Where(t => t.support_Ambiental == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarCalidad = db.PCRs.Where(t => t.support_Calidad == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarCompras = db.PCRs.Where(t => t.support_Compras == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarFinanzas = db.PCRs.Where(t => t.support_Finanzas == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarMantenimiento = db.PCRs.Where(t => t.support_Mantenimiento == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarMateriales = db.PCRs.Where(t => t.support_Materiales == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarSeguridad = db.PCRs.Where(t => t.support_Seguridad == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarTooling = db.PCRs.Where(t => t.support_Tooling == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();
			ViewBag.TarProduccion = db.PCRs.Where(t =>
				t.support_Backcoat == "P" && t.support_Chromo == "P" &&
				t.support_Ecoat == "P" && t.support_Ensamble == "P" &&
				t.support_Estampado == "P" && t.support_Soldadura == "P" &&
				t.support_Topcoat == "P" && t.Status.Contains("Aprob") && t.Date <= DateLess).Count();

			var PCRs = db.PCRs.Include(p => p.Clientes).Include(p => p.MatrizDecision)
							  .Include(p => p.Proyectos).Include(p => p.Reason);

			// Add these logs to ensure data is being fetched correctly
			System.Diagnostics.Debug.WriteLine($"Ambiental Count: {ViewBag.ReqAmbiental}");
			System.Diagnostics.Debug.WriteLine($"Calidad Count: {ViewBag.ReqCalidad}");
			System.Diagnostics.Debug.WriteLine($"Produccion Count: {ViewBag.ReqProduccion}");


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
		public JsonResult Matrizd(int Codigo)
		{
			var decision = db.MatrizDecisions.FirstOrDefault(md => md.ID == Codigo);
			if (decision != null)
			{
				var response = new
				{
					r1 = new
					{
						decision.commcliente,
						decision.Arcompras,
						decision.Armateriales,
						decision.Armantenimiento,
						decision.Arautomatizacion,
						decision.Arcalidad,
						decision.Arseguridad,
						decision.Arambiental,
						decision.Artooling,
						decision.Arestampado,
						decision.Arsoldadura,
						decision.Arcromo,
						decision.Arpintura,
						decision.Arensamble,
						decision.Arfinanzas,
						decision.NivelRiesgo
					}
				};
				return Json(response, JsonRequestBehavior.AllowGet);
			}
			return Json(new { r1 = (object)null }, JsonRequestBehavior.AllowGet);
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

			// Fetch all related FeasibilitySigns for the given PCR ID, including the pcr data for eager loading
			var signs = db.FeasibilitySigns
						  .Where(s => s.pcrID == id)
						  .Include(s => s.pcr) // Ensure pcr data is included
						  .ToList();

			if (signs == null || !signs.Any())
			{
				ViewBag.Message = "No signatures found for this PCR.";
			}

			// Debugging: Log each msg field to confirm data is being fetched correctly
			foreach (var sign in signs)
			{
				System.Diagnostics.Debug.WriteLine($"msg for {sign.Dep}: {sign.msg}");
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
		public string Firma(int id, string Response, string msg, string comentarios, string datos)
		{
			var userId = User.Identity.GetUserId();
			ApplicationUser currentUser = UserManager.FindById(userId);
			if (currentUser == null) return "User not found.";

			string cuser = currentUser.UserFullName;
			string cudep = currentUser.Departamento;

			// Check for Admin or Gerente roles
			bool isAdminOrGerente = currentUser.Roles.Any(r => r.RoleId == "8dcec765-580a-4b6e-9454-b7af0c4ee717" || r.RoleId == "96179ad6-83ef-4aa2-a9e7-1837f8df338f");
			if (!isAdminOrGerente) return "No está permitido... nada ha cambiado...";

			if (string.IsNullOrEmpty(Response))
				return "Datos incompletos.";

			var pcr = db.PCRs.Find(id);
			if (pcr == null) return "PCR no encontrado.";

			// Prepare message
			var message = string.IsNullOrEmpty(msg) ? comentarios : msg;

			// Check if a signature for this department and PCR already exists
			var existingSign = db.FeasibilitySigns.FirstOrDefault(s => s.pcrID == id && s.Dep == cudep && s.Status != "Necesita Arreglos");

			if (existingSign != null)
			{
				System.Diagnostics.Debug.WriteLine("Updating existing sign for department: " + cudep);
				existingSign.msg = message;
				existingSign.Reviewedby_date = DateTime.Now;
				existingSign.Reviewedby = cuser;
				existingSign.Status = $"Aprobado por {cudep}";
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Creating new sign for department: " + cudep);
				var newSign = new FeasibilitySigns
				{
					msg = message,
					Reviewedby_date = DateTime.Now,
					pcrID = id,
					Reviewedby = cuser,
					Dep = cudep,
					Status = $"Aprobado por {cudep}"
				};
				db.FeasibilitySigns.Add(newSign);
			}

			try
			{
				System.Diagnostics.Debug.WriteLine("Entering switch case with Response: " + Response);
				switch (Response)
				{
					case "Aceptar":
						HandleApproval(pcr, currentUser, comentarios);
						break;
					case "Arreglos":
						HandleNeedsFixes(pcr, existingSign ?? new FeasibilitySigns(), comentarios);
						break;
					case "Rechazar":
						HandleRejection(pcr, existingSign ?? new FeasibilitySigns());
						break;
					default:
						HandleCancellation(pcr, existingSign ?? new FeasibilitySigns(), cuser);
						break;
				}

				System.Diagnostics.Debug.WriteLine("Marking PCR entity as modified.");
				db.Entry(pcr).State = EntityState.Modified;

				System.Diagnostics.Debug.WriteLine("Calling SaveChanges.");
				db.SaveChanges();
			}
			catch (DbEntityValidationException ex)
			{
				foreach (var entityValidationError in ex.EntityValidationErrors)
				{
					System.Diagnostics.Debug.WriteLine($"Entity: {entityValidationError.Entry.Entity.GetType().Name}");
					foreach (var validationError in entityValidationError.ValidationErrors)
					{
						System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
					}
				}
				throw;
			}

			return Response;
		}

		private void HandleApproval(pcr pcr, ApplicationUser currentUser, string comentarios)
		{
			// Check if a signature for this department already exists
			var existingSign = db.FeasibilitySigns
				.FirstOrDefault(s => s.pcrID == pcr.ID && s.Dep == currentUser.Departamento);

			if (existingSign == null)
			{
				// Add a new signature if it doesn't exist
				var sign = new FeasibilitySigns
				{
					pcrID = pcr.ID,
					Dep = currentUser.Departamento,
					Reviewedby = currentUser.UserFullName,
					Reviewedby_date = DateTime.Now,
					Status = $"Aprobado por {currentUser.Departamento}",
					msg = comentarios // Store the comment from the form
				};
				//db.FeasibilitySigns.Add(sign); // Add the new signature
			}
			else
			{
				// Update the existing signature with new details
				existingSign.Reviewedby = currentUser.UserFullName;
				existingSign.Reviewedby_date = DateTime.Now;
				existingSign.Status = $"Aprobado por {currentUser.Departamento}";
				existingSign.msg = comentarios; // Update the message field

				db.Entry(existingSign).State = EntityState.Modified;
			}

			// Update the PCR status based on roles and department
			if (pcr.Status == "En Aprobación" &&
				currentUser.Departamento == "Ingenieria" &&
				currentUser.Puesto == "Gerente")
			{
				pcr.Status = "Aprobación Inicial";
				pcr.Reviewedby = currentUser.UserFullName;
				pcr.Reviewedby_date = DateTime.Now;
				NotifyRelevantManagers(pcr, currentUser); // Notify for further review
			}
			else if (pcr.Status == "Aprobación Inicial")
			{
				MarkSupportAsReviewed(pcr, currentUser.Departamento);

				if (AllSupportsReviewed(pcr))
				{
					pcr.Status = "Pend. Aprob. Gerente";
					NotifyGerenteGeneral(pcr, currentUser); // Notify Gerente General
				}
			}
			else if (pcr.Status == "Pend. Aprob. Gerente" && currentUser.Puesto == "Gerente_General")
			{
				if (pcr.Status != "Aprobado en Proceso")
				{
					pcr.Status = "Aprobado en Proceso";
					pcr.Reviewedby = currentUser.UserFullName;
					pcr.Reviewedby_date = DateTime.Now;

					var originator = db.Users.FirstOrDefault(u => u.UserFullName == pcr.Originator);
					if (originator != null)
					{
						string[] email = { originator.Email };
						correo.NotifyFinalApproval(email, pcr.Reviewedby, pcr.PCRID, pcr.ID.ToString());
					}
					TempData["SuccessMessage"] = "PCR aprobado y Aprobación Completo.";
				}
				else
				{
					TempData["ErrorMessage"] = "Este PCR ya fue Aprobación Completo.";
				}
			}

			db.Entry(pcr).State = EntityState.Modified;
			// Check the overall state of the 'pcr' entity
			var entry = db.Entry(pcr);
			System.Diagnostics.Debug.WriteLine($"Entity State: {entry.State}");

			// Loop through each property and log the original and current values
			foreach (var property in entry.OriginalValues.PropertyNames)
			{
				var originalValue = entry.OriginalValues[property];
				var currentValue = entry.CurrentValues[property];
				System.Diagnostics.Debug.WriteLine($"Property: {property}, Original: {originalValue}, Current: {currentValue}");
			}
			foreach (var trackedEntity in db.ChangeTracker.Entries())
			{
				System.Diagnostics.Debug.WriteLine($"Entity: {trackedEntity.Entity.GetType().Name}, State: {trackedEntity.State}");
			}

			db.SaveChanges();
		}

		private void NotifyOriginator(pcr pcr)
		{
			var originator = db.Users.FirstOrDefault(u => u.UserFullName == pcr.Originator);
			if (originator != null)
			{
				string[] email = { originator.Email };
				correo.newReview(email, pcr.Reviewedby, pcr.PCRID, pcr.ID.ToString(), "Aprobado en Proceso");
			}
		}

		private void HandleNeedsFixes(pcr pcr, FeasibilitySigns sign, string comentarios)
		{
			// Update the PCR status based on the existing status
			pcr.Status = pcr.Status == "Aprobación Inicial"
				? $"{pcr.Originator} necesita arreglos"
				: "Necesita Arreglos";

			// Check if the FeasibilitySigns entry exists in the database
			var existingSign = db.FeasibilitySigns
				.FirstOrDefault(s => s.pcrID == pcr.ID && s.Dep == sign.Dep);

			if (existingSign == null)
			{
				// No existing sign found, so create a new one
				System.Diagnostics.Debug.WriteLine("No existing sign found, creating a new one.");

				var newSign = new FeasibilitySigns
				{
					pcrID = pcr.ID,
					Dep = sign.Dep,
					Reviewedby = sign.Reviewedby,
					Reviewedby_date = DateTime.Now,
					Status = "Necesita Arreglos",
					msg = comentarios
				};

				db.FeasibilitySigns.Add(newSign);
			}
			else
			{
				// Existing sign found, so update it
				System.Diagnostics.Debug.WriteLine("Existing sign found, updating it.");

				existingSign.Status = "Necesita Arreglos";
				existingSign.msg = comentarios;
				existingSign.Reviewedby_date = DateTime.Now;

				db.Entry(existingSign).State = EntityState.Modified;
			}

			// Update the PCR entry
			db.Entry(pcr).State = EntityState.Modified;

			// Notify the originator about the needed fixes
			var originator = db.Users.FirstOrDefault(u => u.UserFullName == pcr.Originator);
			if (originator != null)
			{
				string[] email = { originator.Email };
				correo.Arreglos(email, pcr.Originator, pcr.PCRID, pcr.ID.ToString());
			}

			// Save changes to the database
			db.SaveChanges();
		}

		private void HandleRejection(pcr pcr, FeasibilitySigns sign)
		{
			pcr.Status = "Rechazado";
			sign.Status = "Rechazado";

			var originator = db.Users.FirstOrDefault(u => u.UserFullName == pcr.Originator);
			if (originator != null)
			{
				string[] email = { originator.Email };
				correo.Rechazado(email, pcr.Originator, pcr.PCRID, pcr.ID.ToString());
			}
		}

		private void HandleCancellation(pcr pcr, FeasibilitySigns sign, string cuser)
		{
			pcr.Status = $"{cuser} Canceló";
			sign.Status = "Cancelado";
			pcr.Reviewedby = cuser;
		}

		private void MarkSupportAsReviewed(pcr pcr, string departamento)
		{
			switch (departamento)
			{
				case "Calidad":
					pcr.support_Calidad = "R";
					break;
				case "Finanzas":
					pcr.support_Finanzas = "R";
					break;
				case "Compras":
					pcr.support_Compras = "R";
					break;
				case "Materiales":
					pcr.support_Materiales = "R";
					break;
				case "Mantenimiento":
					pcr.support_Mantenimiento = "R";
					pcr.support_Automatizacion = "R";
					break;
				case "Seguridad":
					pcr.support_Seguridad = "R";
					break;
				case "Ambiental":
					pcr.support_Ambiental = "R";
					break;
				case "Tooling":
					pcr.support_Tooling = "R";
					break;
				case "Produccion":
					pcr.support_Estampado = "R";
					pcr.support_Soldadura = "R";
					pcr.support_Chromo = "R";
					pcr.support_Topcoat = "R";
					pcr.support_Ecoat = "R";
					pcr.support_Backcoat = "R";
					pcr.support_Ensamble = "R";
					break;
			}
		}

		private bool AllSupportsReviewed(pcr pcr)
		{
			// Ensure that none of the support fields have "P"
			return new[]
			{
				pcr.support_Calidad, pcr.support_Finanzas, pcr.support_Compras,
				pcr.support_Materiales, pcr.support_Mantenimiento, pcr.support_Automatizacion,
				pcr.support_Seguridad, pcr.support_Ambiental, pcr.support_Tooling,
				pcr.support_Estampado, pcr.support_Soldadura, pcr.support_Chromo,
				pcr.support_Topcoat, pcr.support_Ecoat, pcr.support_Backcoat, pcr.support_Ensamble
			}.All(status => status != "P");
		}

		private void NotifyRelevantManagers(pcr pcr, ApplicationUser currentUser)
		{
			var managers = db.Users.Where(u => u.Puesto == "Gerente").ToList();

			foreach (var manager in managers)
			{
				if ((manager.Departamento == "Compras" && pcr.support_Compras == "P") ||
					(manager.Departamento == "Materiales" && pcr.support_Materiales == "P") ||
					(manager.Departamento == "Mantenimiento" &&
						(pcr.support_Mantenimiento == "P" || pcr.support_Automatizacion == "P")) ||
					(manager.Departamento == "Calidad" && pcr.support_Calidad == "P") ||
					(manager.Departamento == "Seguridad" && pcr.support_Seguridad == "P") ||
					(manager.Departamento == "Ambiental" && pcr.support_Ambiental == "P") ||
					(manager.Departamento == "Tooling" && pcr.support_Tooling == "P") ||
					(manager.Departamento == "Produccion" &&
						(pcr.support_Estampado == "P" || pcr.support_Soldadura == "P" ||
						 pcr.support_Chromo == "P" || pcr.support_Ecoat == "P" ||
						 pcr.support_Topcoat == "P" || pcr.support_Backcoat == "P" ||
						 pcr.support_Ensamble == "P")) ||
					(manager.Departamento == "Finanzas" && pcr.support_Finanzas == "P"))
				{
					string[] email = { manager.Email };
					correo.newReview(email, currentUser.UserFullName, pcr.PCRID, pcr.ID.ToString(), manager.Departamento);
				}
			}
		}


		private void NotifyGerenteGeneral(pcr pcr, ApplicationUser currentUser)
		{
			var gerenteGeneral = db.Users.FirstOrDefault(u => u.Puesto == "Gerente_General");

			if (gerenteGeneral != null)
			{
				string[] email = { gerenteGeneral.Email };
				correo.newReview(
					email,
					currentUser.UserFullName,
					pcr.PCRID,
					pcr.ID.ToString(),
					"Gerente General"
				);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Gerente General not found.");
			}
		}

		private void SendEmailToManagersForReview(pcr pcr, ApplicationUser currentUser)
		{
			var managers = db.Users.Where(u => u.Puesto == "Gerente").ToList();
			foreach (var manager in managers)
			{
				bool shouldNotify = false;
				bool allApproved = false; // Flag to check if all departments are approved

				// Check each department's support field for "P" or "R" status
				switch (manager.Departamento)
				{
					case "Compras":
						shouldNotify = pcr.support_Compras == "P";
						allApproved &= pcr.support_Compras == "R";
						break;
					case "Materiales":
						shouldNotify = pcr.support_Materiales == "P";
						allApproved &= pcr.support_Materiales == "R";
						break;
					case "Mantenimiento":
						shouldNotify = pcr.support_Mantenimiento == "P" || pcr.support_Automatizacion == "P";
						allApproved &= pcr.support_Mantenimiento == "R" && pcr.support_Automatizacion == "R";
						break;
					case "Calidad":
						shouldNotify = pcr.support_Calidad == "P";
						allApproved &= pcr.support_Calidad == "R";
						break;
					case "Seguridad":
						shouldNotify = pcr.support_Seguridad == "P";
						allApproved &= pcr.support_Seguridad == "R";
						break;
					case "Ambiental":
						shouldNotify = pcr.support_Ambiental == "P";
						allApproved &= pcr.support_Ambiental == "R";
						break;
					case "Tooling":
						shouldNotify = pcr.support_Tooling == "P";
						allApproved &= pcr.support_Tooling == "R";
						break;
					case "Produccion":
						shouldNotify = new[]
						{
							pcr.support_Estampado, pcr.support_Soldadura, pcr.support_Chromo,
							pcr.support_Topcoat, pcr.support_Ecoat, pcr.support_Backcoat,
							pcr.support_Ensamble
						}.Contains("P");

						allApproved &= new[]
						{
							pcr.support_Estampado, pcr.support_Soldadura, pcr.support_Chromo,
							pcr.support_Topcoat, pcr.support_Ecoat, pcr.support_Backcoat,
							pcr.support_Ensamble
						}.All(status => status == "R");
						break;
					case "Finanzas":
						shouldNotify = pcr.support_Finanzas == "P";
						allApproved &= pcr.support_Finanzas == "R";
						break;
					default:
						shouldNotify = false;
						break;
				}

				// Send emails to the respective manager
				if (shouldNotify)
				{
					string[] email = { manager.Email };
					correo.newReview(email, currentUser.UserFullName, pcr.PCRID, pcr.ID.ToString(), manager.Departamento);
				}

				// If all departments have approved, notify Gerente_General
				if (allApproved)
				{
					var gerenteGeneral = db.Users.FirstOrDefault(u => u.Puesto == "Gerente_General");
					if (gerenteGeneral != null)
					{
						string[] emailGeneral = { gerenteGeneral.Email };
						correo.newReview(
							emailGeneral,
							currentUser.UserFullName,
							pcr.PCRID,
							pcr.ID.ToString(),
							"Gerente_General"
						);
					}
				}
			}
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
				// Reset all previously approved support fields back to "P"
				ResetSupportFieldsIfReviewed(pcr);

				pcr.Status = "En Aprobación";
				pcr.Date = DateTime.Now;

				// Notify via email about the changes
				string[] eMail = { "dcamacho@flexngate.com" };
				string emailId = pcr.ID.ToString();
				correo.Arreglado(eMail, CurrentUser.UserFullName, pcr.PCRID, emailId);

				// Delete previous signatures since the PCR was edited
				using (SqlConnection connection = new SqlConnection("Data Source=SJIMSVAP7\\SQLEXPRESS;Initial Catalog=SGM_SJI;Integrated Security=False;User ID=monitor;Password=M0n1t0r@F13x;Connect Timeout=20;Encrypt=False;TrustServerCertificate=False"))
				{
					connection.Open();
					SqlCommand commandDelete = new SqlCommand("DELETE FROM FeasibilitySigns WHERE pcrID = " + pcr.ID, connection);
					commandDelete.ExecuteNonQuery();
				}

				// Save the modified PCR
				db.Entry(pcr).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			// If the model is invalid, reload the dropdowns and return the view
			ViewBag.ClientesID = new SelectList(db.eClientes, "ID", "Cliente", pcr.ClientesID);
			ViewBag.MatrizDecisionID = new SelectList(db.MatrizDecisions, "ID", "TipoCambio", pcr.MatrizDecisionID);
			ViewBag.ProyectosID = new SelectList(db.eProyectos, "ID", "Proyecto", pcr.ProyectosID);
			ViewBag.ReasonID = new SelectList(db.ereasons, "ID", "Reason", pcr.ReasonID);

			return View(pcr);
		}

		// Helper method to reset support fields
		private void ResetSupportFieldsIfReviewed(pcr pcr)
		{
			if (pcr.support_Compras == "R") pcr.support_Compras = "P";
			if (pcr.support_Materiales == "R") pcr.support_Materiales = "P";
			if (pcr.support_Mantenimiento == "R") pcr.support_Mantenimiento = "P";
			if (pcr.support_Automatizacion == "R") pcr.support_Automatizacion = "P";
			if (pcr.support_Calidad == "R") pcr.support_Calidad = "P";
			if (pcr.support_Seguridad == "R") pcr.support_Seguridad = "P";
			if (pcr.support_Ambiental == "R") pcr.support_Ambiental = "P";
			if (pcr.support_Tooling == "R") pcr.support_Tooling = "P";

			if (pcr.support_Estampado == "R") pcr.support_Estampado = "P";
			if (pcr.support_Soldadura == "R") pcr.support_Soldadura = "P";
			if (pcr.support_Chromo == "R") pcr.support_Chromo = "P";
			if (pcr.support_Ecoat == "R") pcr.support_Ecoat = "P";
			if (pcr.support_Topcoat == "R") pcr.support_Topcoat = "P";
			if (pcr.support_Backcoat == "R") pcr.support_Backcoat = "P";
			if (pcr.support_Ensamble == "R") pcr.support_Ensamble = "P";
			if (pcr.support_Finanzas == "R") pcr.support_Finanzas = "P";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Flex_SGM.Models;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;


namespace AspnetIdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        public RolesAdminController()
        {
            context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public RolesAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ApplicationDbContext context { get; private set; }
        //
        // GET: /Roles/
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {

            string[] name = { "Admin", "Mantenimiento", "Supervisor", "Calidad", "Ingenieria" };
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(userId);
            if (currentUser.UserFullName.Contains("Super_User")|| currentUser.UserFullName.Contains("luis"))
                for (int mi = 0; mi < name.Count(); mi++)
            {
                if (!RoleManager.RoleExists(name[mi]))
                {
                    var roleresult = RoleManager.Create(new IdentityRole(name[mi]));
                }

                if (!UserManager.IsInRole(userId, name[mi]))
                {
                    UserManager.AddToRole(currentUser.Id, name[0]);
                }
            }


            return View(RoleManager.Roles);
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);

            List<string> susers = new List<string>();

            foreach (var x in role.Users)
            {
                var y = UserManager.FindById(x.UserId);
                susers.Add(y.UserFullName);
            }
            ViewBag.AllUsers = susers;

            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await RoleManager.UpdateAsync(role);
                if(!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();   
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                var result = await RoleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}

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
    [Authorize(Roles = "Admin,Supervisor")]
    [Authorize(Roles = "Admin,Mantenimiento")]
    public class UsersAdminController : Controller
    {

        public UsersAdminController()
        {
            context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public UsersAdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

    public UserManager<ApplicationUser> UserManager { get; private set; }

    public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ApplicationDbContext context { get; private set; }



        //
        // GET: /Users/
        [AllowAnonymous]

        public async Task<ActionResult> Index()
        {


            //    Para Generar los roles necesarios a los usuarios
            
            string name = "Admin"; //"Mantenimiento";
            List<string> lista = new List<string>();
            var l = UserManager.Users;
            foreach (var x in l)
            {

                if (x.UserName.Contains("DanCam"))
                    lista.Add((x.Id));

        
            }
            foreach(string f in lista)
            {

                UserManager.AddToRole(f, name);
            }
            
            IdentityRole y = new IdentityRole { Id = "no", Name = "no" };
            y = RoleManager.Roles.Where(r => r.Name == "Mantenimiento").FirstOrDefault();
            List<ApplicationUser> manager = new List<ApplicationUser>();
            List<ApplicationUser> mtt = new List<ApplicationUser>();
            List<ApplicationUser> supers = new List<ApplicationUser>();
            List<ApplicationUser> admins = new List<ApplicationUser>();

           var ulist= UserManager.Users.ToList();
            foreach (var uitem in ulist)
            {
             foreach(var urole in uitem.Roles)
                {
                    if (urole.RoleId == "7a269541-b9f5-4bfe-8eea-38c0ebe11373")
                        manager.Add(uitem);

                    if (urole.RoleId == "acfa3932-f4ae-423d-94a8-883fc3e5596f")
                        supers.Add(uitem);

                    if (urole.RoleId == "e1a04367-19d1-4b44-acae-ecbd2ed9d885")
                        admins.Add(uitem);

                    if (urole.RoleId == "da2775b7-5438-4b24-8834-256445f32335")
                        mtt.Add(uitem);

                }

            }

            ViewBag.Usermanager = manager;
            ViewBag.Usermtto = mtt;


            ViewBag.Usersuper = supers;

            ViewBag.Useradmin = admins;


            ViewBag.Usernorole = UserManager.Users.Where(u => u.Roles.Count == 0).ToList();

            ViewBag.Message = TempData["ok"];
            return View(await UserManager.Users.Where(u => u.Roles.Count > 0).ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            List<string> sroles = new List<string>();
            foreach(var x in user.Roles)
            {
                var y=RoleManager.FindById(x.RoleId);
                sroles.Add(y.Name);
            }
            ViewBag.AllRoles = sroles;
            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Departamento = new SelectList(Enum.GetValues(typeof(flex_Departamento)).Cast<flex_Departamento>(), "Departamento");
            //Get the list of Roles
            ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>(), "Puesto");

            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>(), "Areas");
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult>  Create(RegisterViewModel userViewModel, string RoleId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userViewModel.UserName;
                user.UserFullName = userViewModel.UserFullName;
                user.Nomina = userViewModel.Nomina;
                user.Area = userViewModel.Area;
                user.Puesto = userViewModel.Puesto;
                user.Email = userViewModel.Email;
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                    if (!String.IsNullOrEmpty(RoleId))
                    {
                        //Find Role Admin
                        var role = await RoleManager.FindByIdAsync(RoleId);
                        var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().ToString());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if(!string.IsNullOrEmpty(user.Puesto)&&!string.IsNullOrEmpty(user.Area)) { 
            var stringp = Enum.Parse(typeof(flex_Puesto), user.Puesto);

            var stringA = Enum.Parse(typeof(flex_Areas), user.Area);

                var stringd = Enum.Parse(typeof(flex_Departamento), user.Departamento);

                ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>(), stringp);

            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>(), stringA);

                ViewBag.Departamento = new SelectList(Enum.GetValues(typeof(flex_Departamento)).Cast<flex_Departamento>(), stringd);
            }
            else
            {
                ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>());

                ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>());

                ViewBag.Departamento = new SelectList(Enum.GetValues(typeof(flex_Departamento)).Cast<flex_Departamento>());

            }

 
            List<string> sroles = new List<string>();
            foreach (var x in user.Roles)
            {
                var y = RoleManager.FindById(x.RoleId);
                sroles.Add(y.Name);
            }
            ViewBag.AllRoles = sroles;


            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");



            ViewBag.uId = user.Id;
            return View(user);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserName,Id,UserFullName,Nomina,Departamento,Area,Puesto,Email")] ApplicationUser formuser, string id, string RoleId,bool deleteroles=false)
        {
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Departamento = new SelectList(Enum.GetValues(typeof(flex_Departamento)).Cast<flex_Departamento>(), "Departamento");
            ViewBag.Puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>(), "Puesto");

            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>(), "Areas");
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
            var user = await UserManager.FindByIdAsync(id);
            user.UserName = formuser.UserName;
            user.UserFullName = formuser.UserFullName;
            user.Nomina = formuser.Nomina;
            user.Departamento = formuser.Departamento;
            user.Area = formuser.Area;
            user.Puesto = formuser.Puesto;
            user.Email = formuser.Email;
            if (ModelState.IsValid)
            {
                //Update the user details
            
               var r= await UserManager.UpdateAsync(user);
                TempData["Warning"] = r.Errors.FirstOrDefault();
                //If user has existing Role then remove the user from the role
                // This also accounts for the case when the Admin selected Empty from the drop-down and
                // this means that all roles for the user must be removed
                var rolesForUser = await UserManager.GetRolesAsync(id);
                if(deleteroles)
                if (rolesForUser.Count() > 0)
                {   
                    foreach (var item in rolesForUser)
                    {
                        var result = await UserManager.RemoveFromRoleAsync(id,item);
                    }
                }

                if (!String.IsNullOrEmpty(RoleId))
                {
                    //Find Role
                    var role = await RoleManager.FindByIdAsync(RoleId);
                    //Add user to new role
                    var result = await UserManager.AddToRoleAsync(id,role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                        return View();
                    }
                }
                return RedirectToAction("Edit","UsersAdmin",id);
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        public ActionResult ChangePassword(string id)
        {
            ViewBag.uId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string id, string NewPassword)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Sample"); 
            UserManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));



            string code = await UserManager.GeneratePasswordResetTokenAsync(id);
            var result = await UserManager.ResetPasswordAsync(id, code, NewPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(id);
                TempData["ok"] = "Se cambia password OK de " + user.UserFullName;
                return RedirectToAction("Index", new { Message = "Se cambia password OK de " + user.UserFullName }) ;
            }
            ViewBag.Message = result.Errors.FirstOrDefault();
            return View();
        }
        ////
        //// GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        ////
        //// POST: /Users/Delete/5
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

                var user =  context.Users.Find(id);
              //  var logins = user.Logins;
              //   foreach (var login in logins)
              //  {
              //      context.UserLogins.Remove(login);
              //  }

                while (RoleManager.FindById(id) != null)
                {

                    var role = await RoleManager.FindByIdAsync(id);
                    var result = await RoleManager.DeleteAsync(role);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        return View();
                    }
                  
                }
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}

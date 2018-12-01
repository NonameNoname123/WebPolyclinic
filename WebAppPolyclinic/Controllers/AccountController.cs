using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Infrastructure;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (AuthManager.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (returnUrl == null || returnUrl == String.Empty)
            {
                returnUrl = "Index";
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details, string returnUrl)
        {
            User user = await UserManager.FindAsync(details.UserName, details.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Некорректное имя или пароль.");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return RedirectToAction("Index", "Home");
            }

            return View(details);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Details(string username)
        {
            if (username == null)
            {
                username = User.Identity.Name;
            }


            User user = await UserManager.FindByNameAsync(username);
            AppIdentityDbContext context = new AppIdentityDbContext();

            if (user != null)
            {
                if (user.DoctorId != null)
                {
                    user.Doctor = await context.Doctors.FindAsync(user.DoctorId);
                }

                if (user.PatientId != null)
                {
                    user.Patient = await context.Patients.FindAsync(user.PatientId);
                }

                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        
    }
}
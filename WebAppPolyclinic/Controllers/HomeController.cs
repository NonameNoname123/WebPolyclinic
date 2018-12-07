using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //[Authorize]
        public ActionResult Index()
        {

            //LogginedUserInfo lui = null;

            //if (
            //AuthManager.User.Identity.IsAuthenticated)
            //{
            //    lui = await new LogginedUserInfo().getLogUserInfoAsync(
            //        HttpContext.GetOwinContext(),
            //        AuthManager.User.Identity.Name);
            //}

            //return View(lui);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Infrastructure;

namespace WebAppPolyclinic.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}
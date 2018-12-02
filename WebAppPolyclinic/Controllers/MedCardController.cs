using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    public class MedCardController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            MedCard medCard = new MedCard();

            User currentUser = context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            // если пользователь не найден (o_O)
            if (currentUser == null) return RedirectToAction("Index", "Home");

            // если пользователь не доктор
            if (currentUser.PatientId == null) return RedirectToAction("Index", "Home");




            return View(
                context
                .MedCards
                .Include("Appointment")
                .Where(x => x.Patient.Id == currentUser.Id));//включая в запрос данные о докторе

        }


    }
}
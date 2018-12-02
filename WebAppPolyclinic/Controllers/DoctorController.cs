using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        [Authorize]
        public ActionResult Index()
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = new Appointment();

            User currentUser = context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            // если пользователь не найден (o_O)
            if (currentUser == null) return RedirectToAction("Index", "Home");

            // если пользователь не доктор
            if (currentUser.DoctorId == null) return RedirectToAction("Index","Home");
            

            return View(
                context
                .Appointments
                .Include("Doctor")
                .Include("Patient")
                .Where(x => x.Doctor.UserName == currentUser.UserName));//включая в запрос данные о докторе
        }

        [HttpGet]
        public ActionResult StartApp(int id)
        {


            AppIdentityDbContext context = new AppIdentityDbContext();
            context.Appointments
                .Where(x => x.Id == id)
                .FirstOrDefault()
                .Status = 1;

            context.SaveChanges();

            return RedirectToAction("Appointment", new { id = id });
            
        }


        public ActionResult Appointment(int id)
        {


            AppIdentityDbContext context = new AppIdentityDbContext();

            Appointment app = context.Appointments.Include("Patient").Where(x => x.Id == id).FirstOrDefault();

            if (app.Status != 1) RedirectToAction("Index");

            DetailApp detApp = new DetailApp();

            detApp.AppointmentId = app.Id;
            detApp.AppointmentDateTime = app.AppointmentDateTime;
            detApp.Patient = app.Patient;
           
            return View(detApp);

        }

        [HttpPost]
        public ActionResult Appointment(DetailApp detApp)
        {


            AppIdentityDbContext context = new AppIdentityDbContext();

            Appointment app = context // id = 1
                .Appointments
                .Include("Patient")
                .Where(x => x.Id == detApp.AppointmentId)  // id = 1
                .FirstOrDefault();



            if (app.Status != 1) RedirectToAction("Index");


            app.Status = 2;

            context.SaveChanges();

            return RedirectToAction("Index");

        }

        
        public ActionResult CloseApp(int id)
        {


            AppIdentityDbContext context = new AppIdentityDbContext();

            Appointment app = context // id = 1
                .Appointments
                .Where(x => x.Id == id)  // id = 1
                .FirstOrDefault();


            app.Status = 2;

            context.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult AppArchive(int id)
        {


            AppIdentityDbContext context = new AppIdentityDbContext();

            Appointment app = context // id = 1
                .Appointments
                .Include("Patient")
                .Include("Doctor")
                .Where(x => x.Id == id)  // id = 1
                .FirstOrDefault();

            DetailApp detailApp = new DetailApp();
            detailApp.Patient = app.Patient;
            detailApp.Doctor = app.Doctor;
            detailApp.AppointmentId = app.Id;
            detailApp.AppointmentDateTime = app.AppointmentDateTime;
            detailApp.Message = "Message";
            return View(detailApp);

        }

    }
}
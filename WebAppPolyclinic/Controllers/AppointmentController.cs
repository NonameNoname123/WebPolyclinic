using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Infrastructure;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    public class AppointmentController : Controller
    {
        
        // GET: Appointment
        public ActionResult Index()
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = new Appointment();

            return View(context.Appointments.Include(x => x.Doctor));
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAppointment model)
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = new Appointment();

            if (ModelState.IsValid)
            {



                Appointment app = new Appointment
                {
                    CommonAppointment = model.CommonAppointment,
                    AppointmentDateTime = model.StartDateTime,
                    Duration = model.Duration,


                };


                if (model.Doctor != null && model.Doctor != "")
                {
                    app.Doctor = context.Users.Where(x => x.UserName == model.Doctor).FirstOrDefault(); // выкидывает null, если пользователь не найден

                    if (app.Doctor == null)
                    {
                        return RedirectToAction("Error", "Appointment", "Нет такого врачевского");
                    }
                    else
                    {
                        context.Appointments.Add(app);
                    }
                }
            }

            context.SaveChanges();

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> MakeApp(int id)
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = await context.Appointments
                .Include(x => x.Doctor)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            

            return View(appointment);
        }


        [HttpGet]
        public async Task<ActionResult> Accept(int id)
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = context.Appointments
                .Include(x => x.Doctor)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            //AppUserManager uManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

            appointment.Patient = context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            context.SaveChanges();

            return RedirectToAction("Index");
        }
        



        //[HttpPost]
        //public async Task<ActionResult> Create(CreateAppointment model)
        //{

        //}
        //    return View();
        //}
    }
}
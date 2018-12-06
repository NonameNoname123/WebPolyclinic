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

            return View(context.Appointments.Include(x => x.Doctor));//включая в запрос данные о докторе
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
            int count = 0;
            User doctor = null;
            long minutesTicks = 0;

            if (ModelState.IsValid)
            {

                if (model.Doctor != null && model.Doctor != "") // проверка что имя доктора введёно
                {
                    doctor = context.Users.Where(x => x.UserName == model.Doctor).FirstOrDefault(); // выкидывает null, если пользователь не найден

                    if (doctor == null) // проверка что пользователь есть в системе
                    {
                        return RedirectToAction("Error", "Appointment", "Пользователь " + model.Doctor + " не найден");
                    }
                    else
                    {
                        if (doctor.DoctorId == null) // проверка что пользователь есть в системе
                        {
                            return RedirectToAction("Error", "Appointment", "Пользователь " + model.Doctor + " не является доктором");
                        }

                    }
                }
                else
                {
                    return RedirectToAction("Error", "Appointment", "Не указано имя доктора");
                }

                if (model.StartDateTime < DateTime.Now) // проверка начала
                {
                    return RedirectToAction("Error", "Appointment", "Нельзя ставить время меньше, чем текущее!");
                }

                minutesTicks = TimeSpan.FromMinutes(model.Duration).Ticks;//model.Duration * 100 * 60; // получение минут продолжительности приёма

                if (model.FinDateTime.Ticks < model.StartDateTime.Ticks + minutesTicks) // проверка что длительность не превышает границ
                {
                    return RedirectToAction("Error", "Appointment", "Нельзя ставить время меньше, чем текущее!");
                }

                // расчет количества приемов, которые возможно создать в указанные временные рамки
                // из финального времени вычитается стартовое время, остаток делится на время продолжительности каждого приёма.
                count = (int)((model.FinDateTime.Ticks - model.StartDateTime.Ticks) / minutesTicks);

                for (int i = 0;i<count; i++)
                {
                    Appointment app = new Appointment
                    {
                        CommonAppointment = model.CommonAppointment,
                        AppointmentDateTime = model.StartDateTime.AddMinutes(model.Duration * i),
                        Duration = model.Duration,
                        Status = 0,
                        Doctor = doctor,
                    };

                    context.Appointments.Add(app);
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
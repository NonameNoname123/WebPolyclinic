using Microsoft.AspNet.Identity;
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

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }


        // GET: Appointment
        public ActionResult Index()
        {
            AppIdentityDbContext context = new AppIdentityDbContext();
            Appointment appointment = new Appointment();

            return View(context.Appointments.Include(x => x.Doctor.Doctor).Include(x=>x.Patient));//включая в запрос данные о докторе
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
        public ActionResult Create(CreateAppointment model)
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

                for (int i = 0; i < count; i++)
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
            Appointment app = await context.Appointments
                .Include(x => x.Doctor)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            // забивка данных о текущем пользователе как пациенте (но в БД пока оно не записывается), чтобы вьюшка решила выдать или нет юзеру возможность подписаться
            app.Patient = await context.Users
                .Include(x => x.Patient)
                .Where(x => x.UserName == User.Identity.Name)
                .FirstOrDefaultAsync();


            return View(app);
        }


        [HttpGet]
        public ActionResult Accept(int id)
        {

            AppIdentityDbContext context = new AppIdentityDbContext();

            User currentUser = context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            
            Appointment app = context.Appointments
                //.Include(x => x.Doctor)
                .Where(x => x.Id == id)
                .FirstOrDefault();

            // если это не открытый приём и юзверь не числится как зареганый пациент - отправляем на мороз
            if (!app.CommonAppointment && currentUser.PatientId == null)
            {
                return RedirectToAction("Index");
            }

            //AppUserManager uManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

            app.Patient = currentUser;

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateNewUser(int id)
        {

            return View(new AppCreateUserModel { AppId = id });
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewUser(AppCreateUserModel model)
        {

            AppIdentityDbContext context = new AppIdentityDbContext();

            User user = null;
            if (ModelState.IsValid)
            {
                user = new User
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    //DateTime.TryParse(model.DateOfBirth, DateOfBirth),
                    AddDate = DateTime.Now,
                    Email = model.Email,
                    Password = model.Password
                };
            }



            IdentityResult result =
                UserManager.Create(user, model.Password);



            if (!result.Succeeded)
            {

                return RedirectToAction("Index");
            }

            Appointment appointment =  context.Appointments
                //.Include(x => x.Doctor)
                .Where(x => x.Id == model.AppId)
                .FirstOrDefault();

            //context.SaveChanges();

            appointment.Patient = context.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

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
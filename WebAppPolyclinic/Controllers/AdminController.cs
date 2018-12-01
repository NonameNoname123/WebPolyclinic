using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppPolyclinic.Infrastructure;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Controllers
{
    public class AdminController : Controller
    {
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserName,
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

                if (model.Doctor)
                {
                    Doctor dc = new Doctor();
                    dc.Speciality = model.DoctorSpeciality;
                    user.Doctor = dc;
                }

                if (model.Patient)
                {
                    Patient pt = new Patient();
                    pt.Policy = model.PatientPolicy;
                    pt.Passport = model.PatientPassport;
                    pt.Address = model.PatientAddress;
                    user.Patient = pt;
                }

                if (model.Admin)
                {
                    Admin ad = new Admin();
                    user.Admin = ad;
                }



                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);

                

                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }


            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }
        }



        public async Task<ActionResult> Edit(string id)
        {
            


            User user = await UserManager.FindByIdAsync(id);
            AppIdentityDbContext context = new AppIdentityDbContext();


            if (user != null)
            {
                EditUserModel eum = new EditUserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.Password,
                    Name = user.Name,
                    Surname = user.Surname,
                    Patronymic = user.Patronymic,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Doctor = user.DoctorId != null, // если доктор не создан, то нужно убрать галочку, то есть фолс
                    //DoctorSpeciality = user.Doctor.Speciality,
                    Admin = user.AdminId != null,
                    Patient = user.PatientId != null,
                    //PatientPolicy = user.Patient.Policy,
                    //PatientPassport = user.Patient.Passport,
                    //PatientAddress = user.Patient.Address,
                };

                //if (eum.Admin)
                //{
                //    user.Admin = await context.Admins.FindAsync(user.AdminId);
                //}

                if (eum.Doctor)
                {
                    user.Doctor = await context.Doctors.FindAsync(user.DoctorId);
                    eum.DoctorSpeciality = user.Doctor.Speciality;
                }

                if (eum.Patient)
                {
                    user.Patient = await context.Patients.FindAsync(user.PatientId);
                    eum.PatientAddress = user.Patient.Address;
                    eum.PatientPassport = user.Patient.Passport;
                    eum.PatientPolicy = user.Patient.Policy;
                }

                return View(eum);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserModel model)
        {
            //Получение пользователя из БД
            User user = await UserManager.FindByIdAsync(model.Id);

            if (user != null)
            {

                //Обновляем его мейл
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.DateOfBirth = model.DateOfBirth;
                user.Email = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Patronymic = model.Patronymic;


                //user.Admin = admin;
                //user.Doctor = doctor;
                //user.Patient = patient;
                //user.IsAdmin = isAdmin;
                //user.IsDoctor = isDoctor;
                //user.IsPatient = isPatient;
                

                // проверка на роли

                // Проверка на админа
                if (model.Admin) // если выбран админ (галочка)
                {
                    user.Admin = new Admin();
                } else // если галочка убрана проверяем, был ли юзер админом, и если да -- убираем ему это свойство
                {
                    user.AdminId = null;
                }

                // Проверка на доктора
                if (model.Doctor) // если выбран админ (галочка)
                {
                    user.Doctor = new Doctor();
                }
                else // если галочка убрана проверяем, был ли юзер админом, и если да -- убираем ему это свойство
                {
                    user.DoctorId = null;
                }

                // Проверка на доктора
                if (model.Patient) // если выбран админ (галочка)
                {
                    user.Patient = new Patient();
                    user.Patient.Address = model.PatientAddress;
                    user.Patient.Passport = model.PatientPassport;
                    user.Patient.Policy = model.PatientPolicy;
                }
                else // если галочка убрана проверяем, был ли юзер админом, и если да -- убираем ему это свойство
                {
                    user.PatientId = null;
                }

                IdentityResult validAllExeptPassword
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validAllExeptPassword.Succeeded)
                {
                    AddErrorsFromResult(validAllExeptPassword);
                }
                
                // Обновляем пасс
                IdentityResult validPass = null;
                if (model.Password != null && model.Password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(model.Password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(model.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }


                // Если всё прошло успешно -- обновляем данные о пользователе
                // Но только если пароль не был обновлён, или обновлён успешно
                if ((validAllExeptPassword.Succeeded && validPass == null) ||
                        (validAllExeptPassword.Succeeded && model.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }

                
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(model);
        }



    }
}
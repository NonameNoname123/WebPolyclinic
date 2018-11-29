using Microsoft.AspNet.Identity;
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
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login,
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth,
                    //DateTime.TryParse(model.DateOfBirth, DateOfBirth),
                    AddDate = DateTime.Now,
                    Email = model.Email,
                    Password = model.Password};

                if (model.Doctor)
                {
                    AppIdentityDbContext db = new AppIdentityDbContext();

                    db.Doctors.Add(new Doctor()
                    {
                        Speciality = model.Speciality
                    });
                    db.SaveChanges();

                }

                if (model.Patient)
                {
                    AppIdentityDbContext db = new AppIdentityDbContext();

                    db.Patients.Add(new Patient()
                    {
                        Policy = model.Policy,
                        Passport = model.Passport,
                        Address = model.Address
                    });
                    db.SaveChanges();
                }

                //if (model.Patient)
                //{
                //    user.Patient = new Patient();
                //}

                if (model.Admin)
                {
                   user.Admin = new Admin();
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
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string name, string surname, string patronymic, string phoneNumber, DateTime dateOfBirth, string email, string password)
        {
            //Получение пользователя из БД
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {

                //Обновляем его мейл
                user.PhoneNumber = phoneNumber;
                user.DateOfBirth = dateOfBirth;
                user.Email = email;
                user.Name = name;
                user.Surname = surname;
                user.Patronymic = patronymic;

                IdentityResult validAllExeptPassword
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validAllExeptPassword.Succeeded)
                {
                    AddErrorsFromResult(validAllExeptPassword);
                }
                
                // Обновляем пасс
                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }


                // Если всё прошло успешно -- обновляем данные о пользователе
                if ((validAllExeptPassword.Succeeded && validPass == null) ||
                        (validAllExeptPassword.Succeeded && password != string.Empty && validPass.Succeeded))
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
            return View(user);
        }



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
    }
}
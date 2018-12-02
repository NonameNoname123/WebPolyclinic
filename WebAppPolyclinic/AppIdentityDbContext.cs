using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebAppPolyclinic.Infrastructure;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic
{
    public class AppIdentityDbContext : IdentityDbContext<User>
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedCard> MedCards { get; set; }

        public AppIdentityDbContext() : base("name=IdentityDb")
        {
            //Database.SetInitializer(
            //    new DropCreateDatabaseIfModelChanges<AppIdentityDbContext>());
            base.Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }
        
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}
        

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }

    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            
            base.Seed(context);
        }

        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            // настройки конфигурации контекста будут указываться здесь
            AppUserManager userAdm = new AppUserManager(new UserStore<User>(context));
            string userName = "admin";
            string name = "admin";
            string surname = "admin";
            DateTime dateOfBirth = DateTime.Now;
            DateTime addDate = DateTime.Now;
            string phoneNumber = "88005553535";
            string password = "admin123";
            string email = "admin@mail.com";

            User user = userAdm.FindByName(userName);
            if (user == null)
            {
                userAdm.Create(new User { Admin = new Admin(),
                    UserName = userName,
                    Email = email,
                    Name = name,
                    Surname = surname,
                    DateOfBirth = dateOfBirth,
                    AddDate = addDate,
                    PhoneNumber = phoneNumber },
                    password);

                user = userAdm.FindByName(userName);
            }
        }
    }
}
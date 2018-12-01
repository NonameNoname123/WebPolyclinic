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

            return View(context.Appointments);
        }

        //public ActionResult Create()
        //{

        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> Create(CreateAppointment model)
        //{
            
        //}
        //    return View();
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebAppPolyclinic.Infrastructure;

namespace WebAppPolyclinic.Models
{
    public class LogginedUserInfo
    {

        public string UserName { get; set; }
        public bool IsPatient { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDoctor { get; set; }

        public async Task<LogginedUserInfo> getLogUserInfoAsync(IOwinContext owinContext, string username)
        {
            AppUserManager userManager = owinContext.GetUserManager<AppUserManager>();

            User user = await userManager.FindByNameAsync(username);

            this.UserName = user.UserName;
            this.IsAdmin = user.AdminId != null;
            this.IsDoctor = user.DoctorId != null;
            this.IsPatient = user.PatientId != null;

            return this;
        }


    }
}
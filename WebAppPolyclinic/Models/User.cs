using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class User : IdentityUser
    {

        //public int Id { get; set; }
        public string  Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        //public int Login { get; set; }
        public string Password { get; set; }
        //public int Email { get; set; }
    }
}
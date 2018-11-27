using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Surname { get; set; }
        public int Patronymic { get; set; }
        public int Login { get; set; }
        public int Password { get; set; }
        public int Email { get; set; }
    }
}
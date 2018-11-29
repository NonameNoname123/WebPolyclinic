﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        //public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AddDate { get; set; }
        //public int Email { get; set; }

        //public Doctor Doctor { get; set; }
        //public Admin Admin { get; set; }
        //public Patient Patient { get; set; }


        public int? AdminId { get; set; }

        [ForeignKey(nameof(AdminId))]
        public virtual Admin Admin { get; set; }

        public int? DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; }

        public int? PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}
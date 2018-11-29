using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class CreateUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public bool Doctor { get; set; }
        public string DoctorSpeciality { get; set; }

        [Required]
        public bool Admin { get; set; }

        [Required]
        public bool Patient { get; set; }
        public string PatientPolicy { get; set; }
        public string PatientPassport { get; set; }
        public string PatientAddress { get; set; }
    }
}
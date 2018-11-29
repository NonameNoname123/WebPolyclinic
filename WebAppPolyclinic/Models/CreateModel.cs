using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class CreateModel
    {
        [Required]
        public string Login { get; set; }

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
        public string Speciality { get; set; }

        [Required]
        public bool Admin { get; set; }

        public bool Patient { get; set; }
        public string Policy { get; set; }
        public string Passport { get; set; }
        public string Address { get; set; }
    }
}
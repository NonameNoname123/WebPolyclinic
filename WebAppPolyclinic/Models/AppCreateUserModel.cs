using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class AppCreateUserModel
    {

        [Required]
        public int AppId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Required]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Required]
        public string Surname { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Patronymic { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Длина строки должна быть равна 11 символов")]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }
        
        //[Required]
        //public bool Doctor { get; set; }
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        //public string DoctorSpeciality { get; set; }

        //[Required]
        //public bool Admin { get; set; }

        //[Required]
        //public bool Patient { get; set; }

        //[StringLength(50)]
        //public string PatientPolicy { get; set; }

        //[StringLength(50)]
        //public string PatientPassport { get; set; }

        //[StringLength(50)]
        //public string PatientAddress { get; set; }
    }
}
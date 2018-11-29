using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class EditUserModel : CreateUserModel
    {
        [Required]
        public string Id { get; set; }
    }
}
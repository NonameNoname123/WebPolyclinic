using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class CreateAppointment
    {
        [Required]
        public bool CommonAppointment { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public DateTime FinDateTime { get; set; }

        [Required]
        public long Duration { get; set; }

        [Required]
        public string Doctor { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
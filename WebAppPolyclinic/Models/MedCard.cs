using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class MedCard : BaseIdentificator
    {
        public string Message { get; set; }
        public DateTime CreateDT { get; set; }
        public User Patient { get; set; }
        public Appointment Appointment { get; set; }
    }
}
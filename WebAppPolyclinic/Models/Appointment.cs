using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class Appointment : BaseIdentificator
    {
        public byte CommonAppointment { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public long Duration { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
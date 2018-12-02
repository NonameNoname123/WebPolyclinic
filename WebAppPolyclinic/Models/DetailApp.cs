using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class DetailApp 
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public User Patient { get; set; }
        public User Doctor { get; set; }
        public string Message { get; set; }
    }
}
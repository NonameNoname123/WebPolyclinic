﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class Appointment : BaseIdentificator
    {
        public bool CommonAppointment { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public long Duration { get; set; }
        public User Doctor { get; set; }
        public User Patient { get; set; }
        public int Status { get; set; }
    }
}
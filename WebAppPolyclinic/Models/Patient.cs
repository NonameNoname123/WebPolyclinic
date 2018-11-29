using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPolyclinic.Models
{
    public class Patient : BaseIdentificator
    {
        public string Policy { get; set; }
        public string Passport { get; set; }
        public string Address { get; set; }
    }
}
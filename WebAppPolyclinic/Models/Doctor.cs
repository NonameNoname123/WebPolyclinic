using Microsoft.AspNet.Identity.EntityFramework;

namespace WebAppPolyclinic.Models
{

    public class Doctor :  BaseIdentificator
    {
        //public int ID { get; set; }
        public string Speciality { get; set; }
    }
}
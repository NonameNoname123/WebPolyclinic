using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAppPolyclinic.Models;

namespace WebAppPolyclinic.Infrastructure
{
    public class CustomUserValidator : UserValidator<User>
    {
        public CustomUserValidator(AppUserManager manager)
            : base(manager)
        { }




        public override async Task<IdentityResult> ValidateAsync(User user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            EmailAddressAttribute esa = new EmailAddressAttribute();

            if (!esa.IsValid(user.Email))
            {
                var errors = result.Errors.ToList();
                //errors.Add("Неккоректный email-адрес");
                result = new IdentityResult(errors);
            }

            //if (!user.Email.ToLower().EndsWith("@mail.com"))
            //{
            //    var errors = result.Errors.ToList();
            //    errors.Add("Любой email-адрес, отличный от mail.com запрещен");
            //    result = new IdentityResult(errors);
            //}

            if (user.Name == string.Empty)
            {
                var errors = result.Errors.ToList();
                errors.Add("Поле 'Имя' не должно быть пустым");
                result = new IdentityResult(errors);
            }

            if (user.Surname == string.Empty)
            {
                var errors = result.Errors.ToList();
                errors.Add("Поле 'Фамилия' не должно быть пустым");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
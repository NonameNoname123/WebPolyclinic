using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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

            if (!user.Email.ToLower().EndsWith("@mail.com"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Любой email-адрес, отличный от mail.com запрещен");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}
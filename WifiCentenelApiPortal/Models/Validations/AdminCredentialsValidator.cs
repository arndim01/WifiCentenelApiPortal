using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Validations
{
    public class AdminCredentialsValidator: AbstractValidator<AdminCredentials>
    {
        public AdminCredentialsValidator()
        {
            RuleFor(ac => ac.Username).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(ac => ac.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}

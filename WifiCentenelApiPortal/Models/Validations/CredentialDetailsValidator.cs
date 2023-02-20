using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Validations
{
    public class CredentialDetailsValidator : AbstractValidator<AuthCredentials>    
    {
        public CredentialDetailsValidator()
        {
            RuleFor(vm => vm.MacAddress).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 100).WithMessage("Password invalid");
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Validations
{
    public class ApRegistrationDetailsValidator: AbstractValidator<ApRegistrationDetails>
    {
        public ApRegistrationDetailsValidator()
        {
            RuleFor(ap => ap.Mac).NotEmpty().WithMessage("Mac cannot be empty");
            RuleFor(ap => ap.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(ap => ap.Address).NotEmpty().WithMessage("Address cannot be empty");
        }
        
    }
}

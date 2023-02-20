using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Validations
{
    public class SubscribeLoadValidator : AbstractValidator<SubscribeLoad>
    {
        public SubscribeLoadValidator()
        {
            RuleFor(sl => sl.Ap).NotEmpty().WithMessage("Ap cannot be empty");
            RuleFor(sl => sl.Code).NotEmpty().WithMessage("Code cannot be empty");
        }
    }
}

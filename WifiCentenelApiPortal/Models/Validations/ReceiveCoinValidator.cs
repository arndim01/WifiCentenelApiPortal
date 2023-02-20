using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models.Validations
{
    public class ReceiveCoinValidator: AbstractValidator<ReceiveCoin>
    {
        public ReceiveCoinValidator()
        {
            RuleFor(rc => rc.Coin).NotEmpty().WithMessage("Invalid Coin");
            RuleFor(rc => rc.Device).NotEmpty().WithMessage("Invalid Device");
        }
    }
}

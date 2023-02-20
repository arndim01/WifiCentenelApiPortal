using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models.Validations;

namespace WifiCentenelApiPortal.Models
{
    [Validator(typeof(ApRegistrationDetailsValidator))]
    public class ApRegistrationDetails
    {
        public string Mac { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models.Validations;

namespace WifiCentenelApiPortal.Models
{
    [Validator(typeof(CredentialDetailsValidator))]
    public class AuthCredentials
    {
        public string MacAddress { get; set; }
        public string Password { get; set; }
    }
}

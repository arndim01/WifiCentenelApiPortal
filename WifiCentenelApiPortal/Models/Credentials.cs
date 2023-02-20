    using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WifiCentenelApiPortal.Models.Validations;

namespace WifiCentenelApiPortal.Models
{
    public class Credentials
    {
        public string device { get; set; }
        public string ap { get; set; }
    }
}

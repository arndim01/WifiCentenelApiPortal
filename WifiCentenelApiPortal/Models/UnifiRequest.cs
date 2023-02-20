using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Models
{
    public class UnifiRequest
    {
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public object JsonResult { get; set; }
    }
}

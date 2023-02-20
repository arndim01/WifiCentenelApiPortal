using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Helper
{
    public static class Generate
    {
        public static TimeSpan GetTotalMinutes(DateTime started, DateTime expired)
        {
            TimeSpan span = expired.Subtract(started);

            return span;
        }
    }
}

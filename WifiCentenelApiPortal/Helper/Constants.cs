using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WifiCentenelApiPortal.Helper
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }
            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
                public const string AdminAccess = "admin_access";
            }
            public static class DateFormat
            {
                public static string DateWithTime(DateTime date)
                {
                    DateTime dateFormat = date;
                    if (dateFormat != DateTime.MinValue)
                    {
                        return String.Format("{0:G}", dateFormat);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
        }
    }
}

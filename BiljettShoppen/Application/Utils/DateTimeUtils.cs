using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    internal static class DateTimeUtils
    {
        /// <summary>
        /// Postgres kräver datum i UTC-format.
        /// Denna metod konverterar DateTime till UTC.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToUtc(DateTime value) 
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            if (value.Kind == DateTimeKind.Local)
                return value.ToUniversalTime();

            return DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime();
        }

        public static DateTime? ToUtc(DateTime? value) => value.HasValue ? ToUtc(value.Value) : null;
    }
}

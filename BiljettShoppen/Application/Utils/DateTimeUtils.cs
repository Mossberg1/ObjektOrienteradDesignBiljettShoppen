namespace Application.Utils
{
    internal static class DateTimeUtils
    {
        /// <summary>
        /// Postgres kräver datum i UTC-format.
        /// Denna metod konverterar DateTime till UTC.
        /// </summary>
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

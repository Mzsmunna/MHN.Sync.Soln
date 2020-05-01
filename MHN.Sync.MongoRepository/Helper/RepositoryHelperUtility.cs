using System;

namespace MHN.Sync.MongoRepository.Helper
{
    public static class RepositoryHelperUtility
    {
        public static DateTime[] GetPreviousDayDates()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);

            var previousStartDate = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 0, 0, 0);
            var previousEndDate = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);

            DateTime[] dateTimes = { previousStartDate.AddHours(-5), previousEndDate.AddHours(-5) };
            return dateTimes;
        }

        public static DateTime[] GetYearlyDate()
        {
            var firstDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var lastDate = new DateTime(DateTime.UtcNow.Year, 12, 31);

            return new[]
            {
                TimeZoneInfo.ConvertTimeToUtc(firstDate).AddHours(-5),
                TimeZoneInfo.ConvertTimeToUtc(lastDate).AddHours(-5)
            };
        }

        public static DateTime[] GetYearlyDates(bool isServer)
        {
            var firstDate = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var lastDate = new DateTime(DateTime.UtcNow.Year, 12, 31);
            if (isServer)
            {
                return new[]
                {
                    firstDate.AddHours(-5),
                    lastDate.AddHours(-5)
                };
            }
            else
            {
                return new[]
                {
                    firstDate,
                    lastDate
                };
            }
        }

        public static void CheckDateTime()
        {
            int year = DateTime.UtcNow.Year;
            DateTime yearStartDate = new DateTime(year, 1, 1);
            Console.WriteLine($"Normal Creation DateTime {yearStartDate}, {yearStartDate.Kind}");


            var d = DateTime.UtcNow;
            var da = new DateTime(d.Year, 1, 1, 0, 0, 0, d.Kind);
            Console.WriteLine($"Second Testing date {da}, and kind {da.Kind}");

            var dates = GetYearlyDates(true);
            Console.WriteLine("5 hours Minus DateTime " + dates[0]);
        }
    }
}

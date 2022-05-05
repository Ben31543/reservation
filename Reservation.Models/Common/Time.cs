using System;
using Newtonsoft.Json;

namespace Reservation.Models.Common
{
    public class Time
    {
        public string Hour { get; set; }

        public string Minute { get; set; }

        public int GetHour()
        {
            switch (Hour)
            {
                case "00":
                    return 24;
                default:
                    return int.Parse(Hour);
            }
        }

        public static string ToDisplayFormat(Time openTime, Time closeTime)
        {
            return $"{openTime.Hour}:{openTime.Minute} - {closeTime.Hour}:{closeTime.Minute}";
        }

        public static bool IsValidWorkingSchedule(Time openTime, Time closeTime)
        {
            if (openTime == null || closeTime == null)
            {
                return false;
            }

            var openingTime = new TimeSpan(openTime.GetHour(), 0, 0);
            var closeingTime = new TimeSpan(closeTime.GetHour(), 0, 0);

            // TimeSpan's CompareTo returns
            // -1 if opening time is earlier than closing time,
            // 0 if times are equal,
            // 1 if opening time is later than close time
            // -1 is the correct case for us
            if (openingTime.CompareTo(closeingTime) == -1)
            {
                return true;
            }

            return false;
        }
    }
}

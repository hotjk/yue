using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract
{
    public class TimeSlot
    {
        public TimeSlot(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException("To MUST >= From.");
            }
            this.From = from;
            this.To = to;
        }

        // Json.Net require only one constructor with parameters
        public static TimeSlot Create(DateTime from, int minutes)
        {
            return new TimeSlot(from, from.AddMinutes(minutes));
        }

        public TimeSlot Clone()
        {
            return new TimeSlot(this.From, this.To);
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int Minutes
        {
            get
            {
                return (int)((To - From).TotalMinutes);
            }
        }

        public static TimeSlot Day(DateTime day)
        {
            return new TimeSlot(day.Date, day.Date.AddDays(1));
        }

        public static TimeSlot Week(int year, int weekOfYear)
        {
            DateTime begin = FirstDateOfWeekISO8601(year, weekOfYear);
            return new TimeSlot(begin, begin.AddDays(7));
        }

        private static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            return firstThursday.AddDays(weekNum * 7).AddDays(-3);
        }

        public static TimeSlot Month(int year, int monthOfYear)
        {
            DateTime begin = new DateTime(year, monthOfYear, 1);
            return new TimeSlot(begin, begin.AddMonths(1));
        }
    }
}

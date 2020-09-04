using System;
using System.Globalization;

namespace RedShot.Infrastructure.Formatting.Formatters
{
    /// <summary>
    /// Week format item.
    /// </summary>
    internal class WeekFormatItem : IFormatItem
    {
        /// <inheritdoc />
        public string Name => "Week number";

        /// <inheritdoc />
        public string Pattern => "week";

        /// <inheritdoc />
        public string GetText()
        {
            var time = DateTime.Now;

            // If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right.
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of the adjusted day.
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
        }
    }
}

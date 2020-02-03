using System;
using System.Linq;
using System.Collections.Generic;
using Lpp.Utilities;
using Lpp.Dns.DTO.Schedule;

namespace Lpp.Dns.Api.Requests
{
    internal class ScheduleManager
    {
        /// <summary>
        /// Hangfire uses NCronTab expressions.
        /// A crontab expression are a very compact way to express a recurring schedule. A single expression is composed of 5 space-delimited fields:
        /// MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static string GetCronExpression(RequestScheduleModel model)
        {
            int hour = model.RunTime.Hour;
            int mins = model.RunTime.Minute;

            // MINS HOURS DAYS MONTHS DAYS-WEEK
            string cronPattern = "{0} {1} {2} {3} {4}";
            string cronExpr = null;

            switch (model.RecurrenceType)
            {
                case "Daily":
                    switch (model.DailyType)
                    {
                        case "EveryNDays":
                            if (model.NDays == 1)
                                cronExpr = string.Format(cronPattern, mins, hour, "*", "*", "*");
                            else
                                cronExpr = string.Format(cronPattern, mins, hour, "*/" + model.NDays, "*", "*");
                            break;
                        case "EveryWeekDay":
                            cronExpr = string.Format(cronPattern, mins, hour, "*", "*", "0-6");
                            break;
                    }
                    break;
                case "Weekly":
                    string dow = "";
                    if (model.Sunday) dow += "0,";
                    if (model.Monday) dow += "1,";
                    if (model.Tuesday) dow += "2,";
                    if (model.Wednesday) dow += "3,";
                    if (model.Thursday) dow += "4,";
                    if (model.Friday) dow += "5,";
                    if (model.Saturday) dow += "6,";
                    dow = dow.Length > 0 ? dow.Substring(0, dow.Length - 1) : "*";
                    cronExpr = string.Format(cronPattern, mins, hour, "*", "*", dow);
                    break;
                case "Monthly":
                    switch (model.MonthlyType)
                    {
                        case "DayOfMonth":
                            if (model.NMonthsForMonthDay == 1)
                                cronExpr = string.Format(cronPattern, mins, hour, model.MonthDay, "*", "*");
                            else
                                cronExpr = string.Format(cronPattern, mins, hour, model.MonthDay + "/" + model.NMonthsForMonthDay, "*", "*");
                            break;
                        case "WeekDayOfMonth":
                            if (model.NMonthsForWeekDay == 1)
                                cronExpr = string.Format(cronPattern, mins, hour, "*", "*", GetWeekDayNumber(model.WeekDay));
                            else
                                cronExpr = string.Format(cronPattern, mins, hour, "*", "*/" + model.NMonthsForWeekDay, GetWeekDayNumber(model.WeekDay) + GetWeekDayPattern(model.NthWeekDay));
                            break;
                    }
                    break;
                case "Yearly":
                    switch (model.YearlyType)
                    {
                        case "YearlyDayOfMonth":
                            cronExpr = string.Format(cronPattern, mins, hour, model.DayOfMonth, GetMonthNumber(model.Month), "*");
                            break;
                        case "YearlyWeekDayOfMonth":
                            cronExpr = string.Format(cronPattern, mins, hour, "*", GetMonthNumber(model.Month), GetWeekDayNumber(model.WeekDay) + GetWeekDayPattern(model.NthWeekDay));
                            break;
                    }
                    break;
            }
            return cronExpr;
        }

        static readonly DayOfWeek[] _daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();
        static readonly Months[] _months = Enum.GetValues(typeof(Months)).Cast<Months>().ToArray();
        private static int GetWeekDayNumber(DayOfWeek WeekDayName)
        {
            return Math.Max(0, Math.Min(6, Array.IndexOf(_daysOfWeek, WeekDayName)));
        }

        private static int GetMonthNumber(Months MonthName)
        {
            return Math.Max(0, Math.Min(11, Array.IndexOf(_months, MonthName))) + 1;
        }

        private static string GetWeekDayPattern(NthWeekDays NthWeekDay)
        {
            switch (NthWeekDay)
            {
                case NthWeekDays.first:
                    return "#1";
                case NthWeekDays.second:
                    return "#2";
                case NthWeekDays.third:
                    return "#3";
                case NthWeekDays.fourth:
                    return "#4";
                case NthWeekDays.last:
                    return "L";
            }

            return "";
        }
    }

    internal static class LegacyExtensions
    {
        public static T If<T>(this T source, bool condition, Func<T, T> then)
        {
            //Contract.Requires( then != null );
            return condition ? then(source) : source;
        }

        public static U If<T, U>(this T source, bool condition, Func<T, U> then, Func<T, U> fnElse)
        {
            //Contract.Requires( then != null );
            //Contract.Requires( fnElse != null );
            return (condition ? then : fnElse)(source);
        }
        public static ISet<T> ToSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
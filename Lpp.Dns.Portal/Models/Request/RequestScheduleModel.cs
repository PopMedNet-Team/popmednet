using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using System.Xml.Serialization;

namespace Lpp.Dns.Portal.Models
{
    [XmlRootAttribute("RequestSchedulerModel", Namespace = "", IsNullable = false)]
    public class RequestScheduleModel
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime RunTime { get; set; }

        // Daily, Weekly, Monthly or Yearly
        public string RecurrenceType { get; set; }

        /// <summary>
        /// Daily Type - EveryNDays or EveryWeekDay
        /// </summary>
        public string DailyType { get; set; }      // EveryNDays
        public int NDays { get; set; }             // EveryWeekDay

        // Recur every NWeeks on one of Sunday-Saturday
        public int NWeeks { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }

        /// <summary>
        /// Monthly Type - DayOfMonth or WeekDayOfMonth
        /// </summary>
        public string MonthlyType { get; set; }    // DayOfMonth OR WeekDayOfMonth

        // DayOfMonth - MonthDay of every NMonths
        public int MonthDay { get; set; }
        public int NMonthsForMonthDay { get; set; }

        // WeekDayOfMonth - NthWeekDay of WeekDay of Every NMonths
        public NthWeekDays NthWeekDay { get; set; }
        public DayOfWeek WeekDay { get; set; }
        public int NMonthsForWeekDay { get; set; }

        /// <summary>
        /// Yearly - DayOfMonth or WeekDayOfMonth
        /// </summary>
        public int NYears { get; set; }
        public string YearlyType { get; set; }
        public Months Month { get; set; }
        public int Day { get; set; }
        public int NthMonthDay { get; set; }
        public int DayOfMonth { get; set; }
        public string MonthOfYear { get; set; }
        
        public string RecurrenceRangeType { get; set; }
        public bool PauseJob { get; set; }
    }

    public enum NthWeekDays
    {
        first, second, third, fourth, last
    }

    public enum Months
    {
        January, February, March, April, May, June, July, August, September, October, November, December
    }
}
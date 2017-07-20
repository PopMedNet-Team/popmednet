using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lpp.Dns.DTO.Schedule
{
    /// <summary>
    /// Request scheduler
    /// </summary>
    [XmlRootAttribute("RequestSchedulerModel", Namespace = "", IsNullable = false)]
    public class RequestScheduleModel
    {
        /// <summary>
        /// Start date
        /// </summary>
        public DateTimeOffset StartDate { get; set; }
        /// <summary>
        /// End date
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }
        /// <summary>
        /// Run time
        /// </summary>
        public DateTimeOffset RunTime { get; set; }

        // Daily, Weekly, Monthly or Yearly
        /// <summary>
        /// Recurrence type
        /// </summary>
        public string RecurrenceType { get; set; }

        /// <summary>
        /// Daily Type - EveryNDays or EveryWeekDay
        /// </summary>
        public string DailyType { get; set; }      // EveryNDays
        /// <summary>
        /// returns ndays
        /// </summary>
        public int NDays { get; set; }             // EveryWeekDay

        // Recur every NWeeks on one of Sunday-Saturday
        /// <summary>
        /// nweeks
        /// </summary>
        public int NWeeks { get; set; }
        /// <summary>
        /// sunday
        /// </summary>
        public bool Sunday { get; set; }
        /// <summary>
        /// monday
        /// </summary>
        public bool Monday { get; set; }
        /// <summary>
        /// tuesday
        /// </summary>
        public bool Tuesday { get; set; }
        /// <summary>
        /// wednesday
        /// </summary>
        public bool Wednesday { get; set; }
        /// <summary>
        /// thrusday
        /// </summary>
        public bool Thursday { get; set; }
        /// <summary>
        /// friday
        /// </summary>
        public bool Friday { get; set; }
        /// <summary>
        /// saturday
        /// </summary>
        public bool Saturday { get; set; }

        /// <summary>
        /// Monthly Type - DayOfMonth or WeekDayOfMonth
        /// </summary>
        public string MonthlyType { get; set; }    // DayOfMonth OR WeekDayOfMonth

        // DayOfMonth - MonthDay of every NMonths
        /// <summary>
        /// returns monthday
        /// </summary>
        public int MonthDay { get; set; }
        /// <summary>
        /// returns nmonths for month day
        /// </summary>
        public int NMonthsForMonthDay { get; set; }

        // WeekDayOfMonth - NthWeekDay of WeekDay of Every NMonths
        /// <summary>
        /// nthweek day
        /// </summary>
        public NthWeekDays NthWeekDay { get; set; }
        /// <summary>
        /// week day
        /// </summary>
        public DayOfWeek WeekDay { get; set; }
        /// <summary>
        /// nmonths for week day
        /// </summary>
        public int NMonthsForWeekDay { get; set; }

        /// <summary>
        /// Yearly - DayOfMonth or WeekDayOfMonth
        /// </summary>
        public int NYears { get; set; }
        /// <summary>
        /// yearly type
        /// </summary>
        public string YearlyType { get; set; }
        /// <summary>
        /// month
        /// </summary>
        public Months Month { get; set; }
        /// <summary>
        /// returns day
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// retuns nthmonth day
        /// </summary>
        public int NthMonthDay { get; set; }
        /// <summary>
        /// returns day of month
        /// </summary>
        public int DayOfMonth { get; set; }
        /// <summary>
        /// month of year
        /// </summary>
        public string MonthOfYear { get; set; }
        /// <summary>
        /// recurrence range type
        /// </summary>

        public string RecurrenceRangeType { get; set; }
        /// <summary>
        /// Pause job
        /// </summary>
        public bool PauseJob { get; set; }
    }
    /// <summary>
    /// nth week days
    /// </summary>
    public enum NthWeekDays
    {
        /// <summary>
        /// nthweekdays first
        /// </summary>
        first,
        /// <summary>
        /// second
        /// </summary>
        
        second, 
        /// <summary>
        /// third
        /// </summary>
        third, 
        /// <summary>
        /// fourth
        /// </summary>
        fourth, 
        /// <summary>
        /// last
        /// </summary>
        last
    }
    /// <summary>
    /// Months
    /// </summary>
    public enum Months
    {
        /// <summary>
        /// january
        /// </summary>
        January,
        /// <summary>
        /// february
        /// </summary>
        February, 
        /// <summary>
        /// march
        /// </summary>
        March,
        /// <summary>
        /// april
        /// </summary>
        April, 
        /// <summary>
        /// may
        /// </summary>
        May, 
        /// <summary>
        /// june
        /// </summary>
        June, 
        /// <summary>
        /// july
        /// </summary>
        July,
        /// <summary>
        /// August
        /// </summary>
        August, 
        /// <summary>
        /// september
        /// </summary>
        September, 
        /// <summary>
        /// october
        /// </summary>
        October, 
        /// <summary>
        /// november
        /// </summary>
        November, 
        /// <summary>
        /// December
        /// </summary>
        December
    }

}

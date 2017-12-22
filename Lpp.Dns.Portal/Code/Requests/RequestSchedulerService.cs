using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Lpp.Composition;
//using Lpp.Data;
//using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Dns.Scheduler;
using Lpp.Dns.Scheduler.Jobs;
using Quartz;
using Quartz.Impl;
using Lpp.Utilities.Legacy;
using Lpp.Dns.Data;
using System.Data.Entity;
using System.Net.Sockets;

namespace Lpp.Dns.Portal
{
    [Export, PartMetadata(ExportScope.Key, TransactionScope.Id)]
    public class RequestSchedulerService : ISchedulerService
    {
        static readonly DnsResult _schedulerServiceUnavailable = DnsResult.Failed("The scheduler service is offline or not available");
        static readonly DnsResult _cannotSchedule = DnsResult.Failed("Request cannot be scheduled - if specifying end date, ensure that it falls on a weekday selected");

        [Import]
        public IRequestService RequestService { get; set; }
        [Import]
        public IPluginService Plugins { get; set; }
        [Import]
        public IAuthenticationService Auth { get; set; }
        [Import]
        public HttpContextBase HttpContext { get; set; }
        [Import]
        public ILog Log { get; set; }        

        public DnsResult Schedule(IRequestContext request, RequestScheduleModel model)
        {
            model = model ?? _defaultSchedule;

            return DnsResult.Catch(() =>
            {
                var jobKey = GetJobKey(request);
                var valid = RequestService.ValidateRequest(request);
                if (!valid.IsSuccess) return valid;

                var job = JobBuilder
                    .Create<SchedulerJob>()
                    .WithIdentity(jobKey.Name, jobKey.Group)
                    .UsingJobData(new JobDataMap
                    { 
                        { SchedulerJob.RequestIdKey, request.RequestID.ToString() },
                        { SchedulerJob.UserIdKey, Auth.CurrentUser.ID.ToString() },
                        { SchedulerJob.HostKey, HttpContext.Request.Url.Host }
                    })
                    .Build();

                // EndAt date has to be on a day specified in the cron or this will throw an error.
                var trigger = TriggerBuilder
                    .Create()
                    .WithIdentity(jobKey.Name, jobKey.Group)
                    .WithCronSchedule(GetCronExpr(model))
                    .StartAt(model.StartDate)
                    .If(model.RecurrenceRangeType == "EndBy" && model.EndDate.HasValue, tb => tb.EndAt(model.EndDate.Value))
                    .Build();

                try
                {
                    var scheduler = new StdSchedulerFactory().GetScheduler();
                    var triggerKey = new TriggerKey(jobKey.Name.ToString(), jobKey.Group.ToString());
                    var a = scheduler.GetTrigger(triggerKey);
                    if (a != null)
                    {
                        scheduler.DeleteJob(new JobKey(jobKey.Name, jobKey.Group));
                    }

                    scheduler.ScheduleJob(job, trigger);
                    if (model.PauseJob) scheduler.PauseTrigger(triggerKey);
                    else scheduler.ResumeTrigger(triggerKey);

                    request.Request.Scheduled = true;

                    return DnsResult.Success;
                }
                catch (SocketException ex)
                {
                    Log.Error(ex);
                    return _schedulerServiceUnavailable;
                }
                catch(SchedulerException ex)
                {
                    Log.Error(ex);
                    return _cannotSchedule;
                }
            });
        }

        public RequestScheduleModel GetSchedule(IRequestContext request)
        {
            var res = from xml in Maybe.Value(request.Request.Schedule)
                      select Deserialize(xml);

            return res.Catch().ValueOrDefault(_defaultSchedule);
        }

        public void SetSchedule(IRequestContext request, RequestScheduleModel model)
        {
            request.Request.Schedule = Serialize(model ?? _defaultSchedule);
        }

        public DnsResult DeleteSchedule(IRequestContext request)
        {
            var res = from factory in Maybe.Value(new StdSchedulerFactory())
                      from scheduler in factory.GetScheduler()
                      from key in GetJobKey(request)
                      from job in scheduler.GetJobDetail(key)
                      from done in scheduler.DeleteJob(key)
                      where done
                      select DnsResult.Success;

            return res.Lift(_schedulerServiceUnavailable);
        }

        public void SubmitSchedulerRequest(string userId, string requestId)
        {
            var db = System.Web.HttpContext.Current.Items["DataContext"] as DataContext;

            var userID = new Guid(userId);

            var user = (from u in db.Users where u.ID == userID select u).AsNoTracking().FirstOrDefault();

            try
            {
                Auth.SetCurrentUser(user, AuthenticationScope.Transaction);
                var ctx = RequestService.GetRequestContext(new Guid(requestId));
                if (ctx == null) 
                    throw new Exception("Cannot find scheduled request ID = " + requestId);

                var newRequest = RequestService.CopyRequest(ctx);
                db.Entry(ctx.Request).Reload();
                
               
                var newCtx = RequestService.GetRequestContext(newRequest.ID);
                db.Entry(newCtx.Request).Reload();
                // Suffix the request name by its recurrence type and instance count.
                RequestScheduleModel scheduleModel = Deserialize(ctx.Request.Schedule);
                

                string schedName = string.IsNullOrEmpty(ctx.Header.Name) ? "" : ctx.Header.Name.Substring(0, (ctx.Header.Name.Length > 100) ? 100 : ctx.Header.Name.Length);
                schedName = schedName + " (" + scheduleModel.RecurrenceType + " " + (++ctx.Request.ScheduleCount) + ")";
              
                newCtx.Request.Name = schedName;
                // If there is a request due date and there is a schedule start date and request due date > schedule start date,
                // then slide the due date forward by the same amount from the current date. 
                if (ctx.Header.DueDate != null && scheduleModel.StartDate != null)
                {
                    DateTime newDueDate = DateTime.Now.Add(((DateTime)ctx.Header.DueDate).Subtract(scheduleModel.StartDate));
                    TimeSpan delta = newDueDate.Subtract((DateTime)ctx.Request.DueDate);
                    newCtx.Request.DueDate = newDueDate;
                    RequestService.TimeShift(newCtx, delta);
                }

                var res = RequestService.TimeShift(newCtx, newCtx.Request.CreatedOn - ctx.Request.CreatedOn);
                //db.SaveChanges();
                res = res.IsSuccess ? RequestService.SubmitRequest(newCtx) : res;
                if (!res.IsSuccess) 
                    throw new Exception("Failed to submit request: " + string.Join(", ", res.ErrorMessages));

               
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        static readonly RequestScheduleModel _defaultSchedule = new RequestScheduleModel
        {
            StartDate = DateTime.Now.Date,
            EndDate = DateTime.Now.Date,
            RunTime = DateTime.Now.Date,
            RecurrenceType = "Daily",
            DailyType = "EveryNDays",
            NDays = 1,
            NWeeks = 1,
            MonthlyType = "DayOfMonth",
            MonthDay = 1,
            NMonthsForWeekDay = 1,
            NMonthsForMonthDay = 1,
            YearlyType = "YearlyDayOfMonth",
            DayOfMonth = 1,
            NYears = 1,
            RecurrenceRangeType = "NoEndDate"
        };

        private JobKey GetJobKey(IRequestContext request)
        {
            return new JobKey("REQUEST_" + request.Request.ID, HttpContext.Request.Url.Host + "/REQUESTS");
        }

        private string GetCronExpr(RequestScheduleModel model)
        {
            int hour = model.RunTime.Hour;
            int mins = model.RunTime.Minute;

            // sec min hours day-of-month month day-of-week year
            string cronPattern = "{0} {1} {2} {3} {4} {5}";
            string cronExpr = null;

            switch (model.RecurrenceType)
            {
                case "Daily":
                    switch (model.DailyType)
                    {
                        case "EveryNDays":
                            if (model.NDays == 1)
                                cronExpr = string.Format(cronPattern, 0, mins, hour, "*", "*", "?");
                            else
                                cronExpr = string.Format(cronPattern, 0, mins, hour, "*/" + model.NDays, "*", "?");
                            break;
                        case "EveryWeekDay":
                            cronExpr = string.Format(cronPattern, 0, mins, hour, "?", "*", "SUN-SAT");
                            break;
                    }
                    break;
                case "Weekly":
                    string dow = "";
                    if (model.Sunday) dow += "1,";
                    if (model.Monday) dow += "2,";
                    if (model.Tuesday) dow += "3,";
                    if (model.Wednesday) dow += "4,";
                    if (model.Thursday) dow += "5,";
                    if (model.Friday) dow += "6,";
                    if (model.Sunday) dow += "7,";
                    dow = dow.Length > 0 ? dow.Substring(0, dow.Length - 1) : "?";
                    cronExpr = string.Format(cronPattern, 0, mins, hour, "?", "*", dow);
                    break;
                case "Monthly":
                    switch (model.MonthlyType)
                    {
                        case "DayOfMonth":
                            if (model.NMonthsForMonthDay == 1)
                                cronExpr = string.Format(cronPattern, 0, mins, hour, model.MonthDay, "*", "?");
                            else
                                cronExpr = string.Format(cronPattern, 0, mins, hour, model.MonthDay + "/" + model.NMonthsForMonthDay, "*", "?");
                            break;
                        case "WeekDayOfMonth":
                            if (model.NMonthsForWeekDay == 1)
                                cronExpr = string.Format(cronPattern, 0, mins, hour, "?", "*", GetWeekDayNumber(model.WeekDay));
                            else
                                cronExpr = string.Format(cronPattern, 0, mins, hour, "?", "*/" + model.NMonthsForWeekDay, GetWeekDayNumber(model.WeekDay) + GetWeekDayPattern(model.NthWeekDay));
                            break;
                    }
                    break;
                case "Yearly":
                    switch (model.YearlyType)
                    {
                        case "YearlyDayOfMonth":
                            cronExpr = string.Format(cronPattern, 0, mins, hour, model.DayOfMonth, GetMonthNumber(model.Month), "?");
                            break;
                        case "YearlyWeekDayOfMonth":
                            cronExpr = string.Format(cronPattern, 0, mins, hour, "?", GetMonthNumber(model.Month), GetWeekDayNumber(model.WeekDay) + GetWeekDayPattern(model.NthWeekDay));
                            break;
                    }
                    break;
            }
            return cronExpr;
        }

        private string GetWeekDayPattern(NthWeekDays NthWeekDay)
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

        static readonly DayOfWeek[] _daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();

        private int GetWeekDayNumber(DayOfWeek WeekDayName) 
        { 
            return Math.Max(0, Math.Min(6, Array.IndexOf(_daysOfWeek, WeekDayName))); 
        }

        static readonly Months[] _months = Enum.GetValues(typeof(Months)).Cast<Months>().ToArray();
        
        private int GetMonthNumber(Months MonthName) 
        { 
            return Math.Max(0, Math.Min(11, Array.IndexOf(_months, MonthName))) + 1; 
        }

        static readonly XmlSerializer _modelSerializer = new XmlSerializer(typeof(RequestScheduleModel));
        private static string Serialize(RequestScheduleModel model)
        {
            var sw = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                _modelSerializer.Serialize(xmlWriter, model, null);
                return sw.ToString();
            }
        }

        private static RequestScheduleModel Deserialize(string serializedModel)
        {
            return _modelSerializer.Deserialize(new StringReader(serializedModel ?? "")) as RequestScheduleModel;
        }
    }
}
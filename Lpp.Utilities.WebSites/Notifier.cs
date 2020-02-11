using Lpp.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites
{
    /// <summary>
    /// This notifier only supports email. In the future this could be replaced with one that knows about email and sms for example or signalr etc.
    /// </summary>
    public class Notifier : INotifier
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool HasSalutation { get; set; }
        public bool HasPostscript { get; set; }

        public Notifier()
        {
            HasSalutation = true;
            HasPostscript = true;
        }

        public void Notify(IEnumerable<Notification> notifications)
        {
            using (var smtp = new SmtpClient())
            {
                foreach (var notification in notifications)
                {
                    foreach (var recipient in notification.Recipients.Where(d => !string.IsNullOrWhiteSpace(d.Email)).DistinctBy(d => d.Email))
                    {
                        using (var msg = new MailMessage())
                        {
                            var salutation = HasSalutation ? "Dear " + recipient.Name + ",\r\n<br/><br/>" : "";
                            var postscript = HasPostscript ? "\r\n<br/><br/><p style=\"font-size: 0.8em;\">We are notifying you of this change because you have subscribed to this notification. If you do not wish to receive this notification again, please login and change your subscription settings in your profile.</p>" : "";
                            msg.To.Add(new MailAddress(recipient.Email, recipient.Name));
                            msg.Subject = notification.Subject;
                            msg.Body = salutation + notification.Body + postscript;
                            msg.IsBodyHtml = true;

                            try
                            {
                                smtp.Send(msg);
                            }
                            catch (SmtpFailedRecipientsException e)
                            {
								System.Diagnostics.Debug.WriteLine(e);
                                //Do nothing right now, should batch up all of the failed recipients and then send a note to the administrator saying that they're invalid.
                            }
                            catch (SmtpException se)
                            {
                                //Record exception in log file.
                                log.Error("There was an error sending the notification: '" + notification.Subject + "' to '" + recipient.Email + "'.", se);
                            }
                        }
                    }
                }
            }
        }
    }
}

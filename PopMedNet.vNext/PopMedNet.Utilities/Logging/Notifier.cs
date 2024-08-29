using Microsoft.Extensions.Logging;
using PopMedNet.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Logging
{
    public class Notifier : PopMedNet.Utilities.Logging.INotifier
    {
        readonly Serilog.ILogger _log;

        public bool HasSalutation { get; set; }
        public bool HasPostscript { get; set; }

        public Notifier(Serilog.ILogger log)
        {
            _log = log;
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
                            msg.Body = salutation + notification.Body;
                            if (notification.NeedsPostScript)
                                msg.Body += postscript;
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
                                _log.Error(se, "There was an error sending the notification: '{0}' to '{1}'", notification.Subject, recipient.Email);
                            }
                        }
                    }
                }
            }
        }
    }
}

using Lpp.Dns.Data;
using System.Data.Entity;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.Logging;
using Lpp.Utilities.WebSites;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lpp.Dns.Api.Networks
{
    /// <summary>
    /// Controller that services the Network messages
    /// </summary>
    public class NetworkMessagesController : LppApiDataController<NetworkMessage, NetworkMessageDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Returns the specific network message.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async System.Threading.Tasks.Task<NetworkMessageDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Return a secure list of Network Messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<NetworkMessageDTO> List()
        {
            return (from l in DataContext.Secure<NetworkMessage>(Identity) where l.Users.Any() == false || l.Users.Any(u => u.UserID == Identity.ID) select l).Map<NetworkMessage, NetworkMessageDTO>();
        }

        /// <summary>
        /// Returns a list of network messages less than X days old.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<NetworkMessageDTO> ListLastDays(int days)
        {
            var dt = DateTime.UtcNow.AddDays(days * -1);
            return (from l in DataContext.Secure<NetworkMessage>(Identity) where l.CreatedOn >= dt && (l.Users.Any() == false || l.Users.Any(u => u.UserID == Identity.ID)) select l).Map<NetworkMessage, NetworkMessageDTO>();
        }
        /// <summary>
        /// Insert list of values associated with Network messages
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public override async Task<IEnumerable<NetworkMessageDTO>> Insert(IEnumerable<NetworkMessageDTO> values)
        {
            var messages = await base.Insert(values);

            var originals = values.ToArray();
            if (!originals.Any())
                return messages;

            var sendNotifications = new ConcurrentBag<Notification>();
            //Now handle if there are targets or not. If not, then send to all. If there is, then send only to those.
            for (int j = 0; j < messages.Count(); j++)
            {
                var original = originals[j];

                Notification notification = new Notification
                {
                    Subject = original.Subject,
                    Body = original.MessageText
                };

                if (original.Targets == null || !original.Targets.Any())
                {
                    notification.Recipients = await (from u in DataContext.Users
                                               where u.Active == true && u.Deleted == false
                                               select new Recipient
                                      {
                                          Name = u.FirstName + " " + u.LastName,
                                          Email = u.Email,
                                          Phone = u.Phone,
                                          UserID = u.ID
                                      }).ToArrayAsync();
                } else {
                    notification.Recipients = await (from u in DataContext.Users
                                              where u.Active == true && u.Deleted == false && original.Targets.Contains(u.ID) || u.SecurityGroups.Any(sg => original.Targets.Contains(sg.SecurityGroupID))
                                       select new Recipient
                                       {
                                           Name = u.FirstName + " " + u.LastName,
                                           Email = u.Email,
                                           Phone = u.Phone,
                                           UserID = u.ID
                                       }).ToArrayAsync();

                    foreach (var item in notification.Recipients)
                    {
                        NetworkMessageUser messageUser = new NetworkMessageUser();
                        messageUser.NetworkMessageID = original.ID.Value;
                        messageUser.UserID = item.UserID.Value;

                        DataContext.NetworkMessageUsers.Add(messageUser);
                    }
                    await DataContext.SaveChangesAsync();

                }

                //This adds it the aggrigate notification to the list of notifications that will be sent.
                //Each of these notifications only have one recipient.
                sendNotifications.Add(notification);
            }
            
            
            //Create the notifier
            var notifier = new Notifier
                {
                    HasPostscript = false,
                    HasSalutation = true
                };

            //Asynchronously send all of the notifications
            await Task.Run(() => notifier.Notify(sendNotifications.AsEnumerable()));

            return messages;
        }
    }
}

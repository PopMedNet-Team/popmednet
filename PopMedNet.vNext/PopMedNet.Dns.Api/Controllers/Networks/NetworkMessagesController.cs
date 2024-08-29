using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.DTO;
using PopMedNet.Dns.Data;
using Microsoft.EntityFrameworkCore;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OData.Query;
using PopMedNet.Utilities;
using AutoMapper.AspNet.OData;
using PopMedNet.Utilities.Logging;
using System.Collections.Concurrent;

namespace PopMedNet.Dns.Api.Networks
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class NetworkMessagesController : ApiDataControllerBase<NetworkMessage, NetworkMessageDTO, DataContext, PermissionDefinition>
    {

        public NetworkMessagesController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }

        /// <summary>
        /// Returns the specific network message.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Task<ActionResult<NetworkMessageDTO>> Get(Guid ID)
        {
            return base.Get(ID);
        }

        /// <summary>
        /// Return a secure list of Network Messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public override IActionResult List(ODataQueryOptions<NetworkMessageDTO> options)
        {
            var query = (from l in DataContext.Secure<NetworkMessage>(Identity)
                        let userID = Identity.ID
                        where
                        DataContext.NetworkMessageUsers.Where(nu => nu.NetworkMessageID == l.ID).Any() == false
                        ||
                        DataContext.NetworkMessageUsers.Where(nu => nu.NetworkMessageID == l.ID && nu.UserID == userID).Any()
                        select l).ProjectTo<NetworkMessageDTO>(_mapper.ConfigurationProvider);

            var queryHelper = new Utilities.WebSites.ODataQueryHandler<NetworkMessageDTO>(query, options);
            return Ok(queryHelper.Result());
        }

        /// <summary>
        /// Returns a list of network messages less than X days old.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpGet("listlastdays"), EnableQuery]
        public IQueryable<NetworkMessageDTO> ListLastDays(int days)
        {
            var dt = DateTime.UtcNow.AddDays(days * -1);
            return (from l in DataContext.Secure<NetworkMessage>(Identity) where l.CreatedOn >= dt && (l.Users.Any() == false || l.Users.Any(u => u.UserID == Identity.ID)) select l).ProjectTo<NetworkMessageDTO>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        /// Insert list of values associated with Network messages
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        public async Task<IActionResult> Insert(IEnumerable<NetworkMessageDTO> values)
        {
            //return if there are no messages to send
            if(values == null || values.Count() == 0)
                return StatusCode(StatusCodes.Status204NoContent);

            //confirm has permission to create message: PermissionIdentifiers.Portal.CreateNetworkMessages
            if(await DataContext.HasPermission(Identity, PermissionIdentifiers.Portal.CreateNetworkMessages) == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to create new network messages.");
            }

            //create the messages and update the dto's
            List<NetworkMessage> messages = new List<NetworkMessage>();
            foreach(var dto in values)
            {
                var msg = new NetworkMessage
                {
                    CreatedOn = DateTime.UtcNow,
                    MessageText = dto.MessageText ?? string.Empty,
                    Subject = dto.Subject ?? string.Empty
                };

                dto.ID = msg.ID;

                if (dto.Targets != null)
                {
                    //get the userID's for all user's in the targets, and all the users associated with the security groups in targets.
                    var userIDs = await DataContext.Users.Where(u => dto.Targets.Contains(u.ID) || u.SecurityGroups.Any(sg => dto.Targets.Contains(sg.SecurityGroupID))).Select(u => u.ID).ToArrayAsync();

                    msg.Users = userIDs.Select(t => new NetworkMessageUser { NetworkMessageID = msg.ID, UserID = t }).ToArray();
                }

                messages.Add(msg);

            }

            await DataContext.NetworkMessages.AddRangeAsync(messages);

            await DataContext.SaveChangesAsync();

            //TODO: send the notifications on a background thread

            //var messages = await base.InsertOrUpdate(values);

            //var originals = values.ToArray();
            ////if (!originals.Any())
            ////    return messages;

            //var sendNotifications = new ConcurrentBag<Notification>();
            ////Now handle if there are targets or not. If not, then send to all. If there is, then send only to those.
            //for (int j = 0; j < messages.Count(); j++)
            //{
            //    var original = originals[j];

            //    Notification notification = new Notification
            //    {
            //        Subject = original.Subject,
            //        Body = original.MessageText
            //    };

            //    if (original.Targets == null || !original.Targets.Any())
            //    {
            //        notification.Recipients = await (from u in DataContext.Users
            //                                         where u.Active == true && u.Deleted == false
            //                                         select new Recipient
            //                                         {
            //                                             Name = u.FirstName + " " + u.LastName,
            //                                             Email = u.Email,
            //                                             Phone = u.Phone,
            //                                             UserID = u.ID
            //                                         }).ToArrayAsync();
            //    }
            //    else
            //    {
            //        notification.Recipients = await (from u in DataContext.Users
            //                                         where u.Active == true && u.Deleted == false && original.Targets.Contains(u.ID) || u.SecurityGroups.Any(sg => original.Targets.Contains(sg.SecurityGroupID))
            //                                         select new Recipient
            //                                         {
            //                                             Name = u.FirstName + " " + u.LastName,
            //                                             Email = u.Email,
            //                                             Phone = u.Phone,
            //                                             UserID = u.ID
            //                                         }).ToArrayAsync();

            //        foreach (var item in notification.Recipients)
            //        {
            //            NetworkMessageUser messageUser = new NetworkMessageUser();
            //            messageUser.NetworkMessageID = original.ID.Value;
            //            messageUser.UserID = item.UserID.Value;

            //            DataContext.NetworkMessageUsers.Add(messageUser);
            //        }
            //        await DataContext.SaveChangesAsync();

            //    }

            //    //This adds it the aggrigate notification to the list of notifications that will be sent.
            //    //Each of these notifications only have one recipient.
            //    sendNotifications.Add(notification);
            //}


            ////Create the notifier
            //var notifier = new Notifier
            //{
            //    HasPostscript = false,
            //    HasSalutation = true
            //};

            ////Asynchronously send all of the notifications
            //await Task.Run(() => notifier.Notify(sendNotifications.AsEnumerable()));

            return Ok(values);
        }

    }
}

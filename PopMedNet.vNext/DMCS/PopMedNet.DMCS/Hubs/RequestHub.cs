using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PopMedNet.DMCS.Models.Notifications;
using System;
using System.Threading.Tasks;

namespace PopMedNet.DMCS
{
    [Authorize]
    public class RequestHub : Hub
    {
        public const string RequestListGroupName = "RequestList";
        

        public async Task ConnectedToRequest(Guid requestID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, requestID.ToString("D"));
        }

        public async Task ConnectedToRequestList()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, RequestListGroupName);
        }

        public static class EventIdentifiers
        {
            public const string RequestList_RequestListUpdated = "requestListUpdated";
            public const string RequestDataMart_Metadata = "requestMetadata";
            public const string Response_DocumentAdded = "responseDocAdded";
            public const string Response_DocumentRemoved = "responseDocRemoved";
            public const string Response_CacheCleared = "responseCacheCleared";
        }
    }
}

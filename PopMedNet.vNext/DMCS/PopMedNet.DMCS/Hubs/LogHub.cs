using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace PopMedNet.DMCS
{
    [Authorize]
    public class LogHub : Hub
    {
        public async Task ConnectedToResponse(Guid responseID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, responseID.ToString("D"));
        }

        public async Task ConnectedToGlobal()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "GlobalLog");
        }
    }
}

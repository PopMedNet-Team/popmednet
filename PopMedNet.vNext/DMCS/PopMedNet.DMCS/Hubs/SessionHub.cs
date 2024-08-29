using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS
{
    [Authorize]
    public class SessionHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await this.Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            }

            await base.OnConnectedAsync();
        }
    }
}

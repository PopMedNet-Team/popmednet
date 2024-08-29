using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PopMedNet.Dns.Portal.Hubs
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

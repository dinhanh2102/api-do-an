using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HRM.Services.Cores.Signalr
{
    public class SignalrHub : Hub<IHubClient>
    {
        public async Task DashBoard()
        {
            await Clients.All.DashBoard();
        }
    }
}
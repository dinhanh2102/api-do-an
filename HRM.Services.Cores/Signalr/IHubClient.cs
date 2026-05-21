using System.Threading.Tasks;

namespace HRM.Services.Cores.Signalr
{
    public interface IHubClient
    {
        //Task BroadcastMessage(MessageInstance msg);
        Task DashBoard();
    }
}
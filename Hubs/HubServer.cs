
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebRazor.Hubs
{
    public class HubServer : Hub
    {
        public async void HasNewData()
        {
           await Clients.All.SendAsync("ReloadProduct");
        }
    }
}

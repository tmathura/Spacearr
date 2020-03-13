using Spacearr.Pusher.API.Interfaces;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class PusherValidation : IPusherValidation
    {
        public async Task<bool> Validate(string appId, string key, string secret, string cluster)
        {
            var pusherSend = new PusherServer.Pusher(appId, key, secret, new PusherServer.PusherOptions { Cluster = cluster });
            var getResult = await pusherSend.GetAsync<object>("/channels/ValidationTestChannel");
            return getResult.Data != null;
        }
    }
}

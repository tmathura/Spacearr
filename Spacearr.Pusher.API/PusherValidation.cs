using Spacearr.Pusher.API.Interfaces;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API
{
    public class PusherValidation : IPusherValidation
    {
        /// <summary>
        /// Validate if the Pusher details are correct.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns>Returns a bool</returns>
        public async Task<bool> Validate(string appId, string key, string secret, string cluster)
        {
            var pusherSend = new PusherServer.Pusher(appId, key, secret, new PusherServer.PusherOptions { Cluster = cluster });
            var getResult = await pusherSend.GetAsync<object>("/channels/ValidationTestChannel");
            return getResult.Data != null;
        }
    }
}

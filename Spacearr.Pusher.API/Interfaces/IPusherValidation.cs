using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IPusherValidation
    {
        /// <summary>
        /// Validate if the Pusher details are correct.
        /// </summary>
        /// <param name="appId">The Pusher app id</param>
        /// <param name="key">The Pusher key</param>
        /// <param name="secret">The Pusher secret</param>
        /// <param name="cluster">The Pusher cluster</param>
        /// <returns>Returns a bool</returns>
        Task<bool> Validate(string appId, string key, string secret, string cluster);
    }
}
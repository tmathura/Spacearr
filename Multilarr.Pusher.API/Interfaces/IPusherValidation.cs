using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IPusherValidation
    {
        Task<bool> Validate(string appId, string key, string secret, string cluster);
    }
}
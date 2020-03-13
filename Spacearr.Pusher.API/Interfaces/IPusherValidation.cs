using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IPusherValidation
    {
        Task<bool> Validate(string appId, string key, string secret, string cluster);
    }
}
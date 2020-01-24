using PusherClient;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces
{
    public interface IPusherClientInterface
    {
        Task<ConnectionState> ConnectAsync();
        Task<ConnectionState> DisconnectAsync();
        Task<Channel> SubscribeAsync(string channelName);
    }
}
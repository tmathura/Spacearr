using Multilarr.Common.Interfaces;
using PusherClient;

namespace Multilarr.Common
{
    public class PusherClientInterface : Pusher, IPusherClientInterface
    {
        public PusherClientInterface(string key, PusherOptions options) : base(key, options) { }
    }
}

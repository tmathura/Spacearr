using Spacearr.Common.Interfaces.Command;
using System;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces
{
    public interface IComputerDrivesCommandReceiver
    {
        Task Connect(Action<ICommand, string, string> executeCommand, string appId, string key, string secret, string cluster);
    }
}
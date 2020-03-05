using Multilarr.Common.Interfaces.Command;
using System;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IComputerDrivesCommandReceiver
    {
        Task Connect(Action<ICommand, string, string> executeCommand);
    }
}
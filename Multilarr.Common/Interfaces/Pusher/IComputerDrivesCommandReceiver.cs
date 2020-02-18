using Multilarr.Common.Interfaces.Command;
using System;
using System.Threading.Tasks;

namespace Multilarr.Common.Interfaces.Pusher
{
    public interface IComputerDrivesCommandReceiver
    {
        Task Connect(Action<ICommand, string, string> executeCommand);
    }
}
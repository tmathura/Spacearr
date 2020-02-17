using System;
using System.Threading.Tasks;
using Multilarr.Common.Interfaces.Command;

namespace Multilarr.Common.Interfaces
{
    public interface IComputerDrivesCommandReceiver
    {
        Task Connect(Action<ICommand, string, string> executeCommand);
    }
}
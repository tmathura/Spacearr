using Multilarr.WorkerService.Windows.Command.MessageCommand;
using Multilarr.WorkerService.Windows.Common;

namespace Multilarr.WorkerService.Windows.Command
{
    public interface ICommand
    {
        CommandObjectSerialized Invoke(Enumeration.CommandType command);
    }
}
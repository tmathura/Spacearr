using Multilarr.WorkerService.Windows.Command.MessageCommand;
    
namespace Multilarr.WorkerService.Windows.Command
{
    public interface ICommand
    {
        CommandObjectSerialized Invoke(string command);
    }
}
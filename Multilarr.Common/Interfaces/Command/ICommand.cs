using Multilarr.Common.Command.MessageCommand;

namespace Multilarr.Common.Interfaces.Command
{
    public interface ICommand
    {
        CommandObjectSerialized Invoke(Enumeration.CommandType command);
    }
}
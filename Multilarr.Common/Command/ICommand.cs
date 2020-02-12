using Multilarr.Common.Command.MessageCommand;

namespace Multilarr.Common.Command
{
    public interface ICommand
    {
        CommandObjectSerialized Invoke(Enumeration.CommandType command);
    }
}
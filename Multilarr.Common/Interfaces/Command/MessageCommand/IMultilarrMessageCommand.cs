using Multilarr.Common.Command.MessageCommand;

namespace Multilarr.Common.Interfaces.Command.MessageCommand
{
    public interface IMultilarrMessageCommand
    {
        CommandObjectSerialized Invoke(IMessageCommand command);
    }
}
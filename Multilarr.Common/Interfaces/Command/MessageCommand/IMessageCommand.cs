using Multilarr.Common.Command.MessageCommand;

namespace Multilarr.Common.Interfaces.Command.MessageCommand
{
    public interface IMessageCommand
    {
        CommandObjectSerialized Execute();
    }
}
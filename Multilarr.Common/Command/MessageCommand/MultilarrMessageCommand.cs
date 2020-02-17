using Multilarr.Common.Interfaces.Command.MessageCommand;

namespace Multilarr.Common.Command.MessageCommand
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public CommandObjectSerialized Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
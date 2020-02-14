using Multilarr.Common.Command.MessageCommand;

namespace Multilarr.Common.Interfaces.Command.MessageCommand
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public CommandObjectSerialized Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
using Multilarr.Common.Interfaces.Command.MessageCommand;

namespace Multilarr.Common.Command.MessageCommand
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public string Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
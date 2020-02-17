using Multilarr.Common.Interfaces.Command;

namespace Multilarr.Common.Command
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public string Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
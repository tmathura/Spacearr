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
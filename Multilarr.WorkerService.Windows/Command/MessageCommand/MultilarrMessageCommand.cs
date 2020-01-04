namespace Multilarr.WorkerService.Windows.Command.MessageCommand
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public MessageCommandObject Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
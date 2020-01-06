namespace Multilarr.WorkerService.Windows.Command.MessageCommand
{
    public class MultilarrMessageCommand : IMultilarrMessageCommand
    {
        public CommandObjectSerialized Invoke(IMessageCommand command)
        {
            return command.Execute();
        }
    }
}
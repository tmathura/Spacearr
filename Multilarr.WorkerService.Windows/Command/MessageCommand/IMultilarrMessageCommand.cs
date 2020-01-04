namespace Multilarr.WorkerService.Windows.Command.MessageCommand
{
    public interface IMultilarrMessageCommand
    {
        MessageCommandObject Invoke(IMessageCommand command);
    }
}
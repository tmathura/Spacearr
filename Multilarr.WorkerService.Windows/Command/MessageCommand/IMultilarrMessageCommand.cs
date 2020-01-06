namespace Multilarr.WorkerService.Windows.Command.MessageCommand
{
    public interface IMultilarrMessageCommand
    {
        CommandObjectSerialized Invoke(IMessageCommand command);
    }
}
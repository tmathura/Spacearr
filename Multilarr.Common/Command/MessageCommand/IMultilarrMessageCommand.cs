namespace Multilarr.Common.Command.MessageCommand
{
    public interface IMultilarrMessageCommand
    {
        CommandObjectSerialized Invoke(IMessageCommand command);
    }
}
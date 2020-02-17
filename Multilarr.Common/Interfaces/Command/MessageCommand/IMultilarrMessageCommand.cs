namespace Multilarr.Common.Interfaces.Command.MessageCommand
{
    public interface IMultilarrMessageCommand
    {
        string Invoke(IMessageCommand command);
    }
}
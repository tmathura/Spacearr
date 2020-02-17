namespace Multilarr.Common.Interfaces.Command
{
    public interface IMultilarrMessageCommand
    {
        string Invoke(IMessageCommand command);
    }
}
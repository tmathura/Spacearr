namespace Multilarr.Common.Interfaces.Command
{
    public interface IInvoker
    {
        string Invoke(ICommand command);
    }
}
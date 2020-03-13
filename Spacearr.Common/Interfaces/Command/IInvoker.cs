namespace Spacearr.Common.Interfaces.Command
{
    public interface IInvoker
    {
        string Invoke(ICommand command);
    }
}
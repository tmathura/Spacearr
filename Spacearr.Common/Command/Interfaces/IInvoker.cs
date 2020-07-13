namespace Spacearr.Common.Command.Interfaces
{
    public interface IInvoker
    {
        string Invoke(ICommand command);
    }
}
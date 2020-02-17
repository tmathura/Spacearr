namespace Multilarr.Common.Interfaces.Command
{
    public interface ICommand
    {
        string Invoke(Enumeration.CommandType command);
    }
}
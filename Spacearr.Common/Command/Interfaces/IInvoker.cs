using System.Threading.Tasks;

namespace Spacearr.Common.Command.Interfaces
{
    public interface IInvoker
    {
        Task<string> Invoke(ICommand command);
    }
}
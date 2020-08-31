using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Interfaces
{
    public interface IInvoker
    {
        Task<List<string>> Invoke(ICommand command);
    }
}
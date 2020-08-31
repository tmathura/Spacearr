using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Interfaces
{
    public interface ICommand
    {
        Task<List<string>> Execute();
    }
}
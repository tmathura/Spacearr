using System.Collections.Generic;
using Spacearr.Common.Command.Interfaces;
using System.Threading.Tasks;

namespace Spacearr.Common.Command.Implementations
{
    public class Invoker : IInvoker
    {
        public Invoker() { }
        public async Task<List<string>> Invoke(ICommand command)
        {
            return await command.Execute();
        }
    }
}

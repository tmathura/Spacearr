using Spacearr.Common.Command.Interfaces;

namespace Spacearr.Common.Command.Implementations
{
    public class Invoker : IInvoker
    {
        public Invoker() { }
        public string Invoke(ICommand command)
        {
            return command.Execute();
        }
    }
}

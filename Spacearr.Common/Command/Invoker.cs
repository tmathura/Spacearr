using Spacearr.Common.Interfaces.Command;

namespace Spacearr.Common.Command
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

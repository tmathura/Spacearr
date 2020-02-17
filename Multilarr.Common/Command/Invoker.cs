using Multilarr.Common.Interfaces.Command;

namespace Multilarr.Common.Command
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

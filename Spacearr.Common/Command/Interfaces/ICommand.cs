using System.Threading.Tasks;

namespace Spacearr.Common.Command.Interfaces
{
    public interface ICommand
    {
        Task<string> Execute();
    }
}
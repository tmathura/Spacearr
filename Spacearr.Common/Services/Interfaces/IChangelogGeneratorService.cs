using Spacearr.Common.Enums;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IChangelogGeneratorService
    {
        /// <summary>
        /// Create changelog
        /// </summary>
        /// <returns></returns>
        Task CreateChangelog();
    }
}
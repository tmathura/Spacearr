using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces.Service
{
    public interface IGetComputerService
    {
        /// <summary>
        /// Returns all the computers and their hard disks.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerModel</returns>
        Task<IEnumerable<ComputerModel>> GetComputersAsync();
    }
}

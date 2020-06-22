using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces.Service
{
    public interface IComputerDriveService
    {
        /// <summary>
        /// Returns all the computer hard disks.
        /// </summary>
        /// <returns>Returns a IEnumerable of ComputerDriveModel</returns>
        Task<IEnumerable<ComputerDriveModel>> GetComputerDrivesAsync();
    }
}

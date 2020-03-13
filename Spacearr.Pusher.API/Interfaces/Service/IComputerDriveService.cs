using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spacearr.Pusher.API.Interfaces.Service
{
    public interface IComputerDriveService
    {
        Task<IEnumerable<ComputerDriveModel>> GetComputerDrivesAsync();
    }
}

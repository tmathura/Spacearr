using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces.Service
{
    public interface IComputerDriveService
    {
        Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync();
    }
}

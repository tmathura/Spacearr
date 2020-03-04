using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API.Interfaces
{
    public interface IComputerDriveService
    {
        Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync();
    }
}

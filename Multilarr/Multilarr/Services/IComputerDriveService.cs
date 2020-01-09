using Multilarr.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public interface IComputerDriveService
    {
        Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync();
    }
}

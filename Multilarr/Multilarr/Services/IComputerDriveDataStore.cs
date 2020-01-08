using System.Collections.Generic;
using System.Threading.Tasks;
using Multilarr.Models;

namespace Multilarr.Services
{
    public interface IComputerDriveDataStore
    {
        Task<IEnumerable<ComputerDrive>> GetComputerDrivesAsync();
    }
}

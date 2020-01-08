using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public interface IComputerDriveDataStore<T>
    {
        Task<IEnumerable<T>> GetComputerDrivesAsync();
    }
}

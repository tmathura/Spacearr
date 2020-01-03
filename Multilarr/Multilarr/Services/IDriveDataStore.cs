using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public interface IDriveDataStore<T>
    {
        Task<T> GetDriveAsync(string name);
        Task<IEnumerable<T>> GetDrivesAsync(bool forceRefresh = false);
    }
}

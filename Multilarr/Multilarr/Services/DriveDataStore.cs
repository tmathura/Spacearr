using Multilarr.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public class MockDriveDataStore : IDriveDataStore<Drive>
    {
        private readonly List<Drive> _drives;

        public MockDriveDataStore()
        {
            _drives = CreateDrives(10);
        }

        public static List<Drive> CreateDrives(int total)
        {
            var drives = new List<Drive>();
            for (var i = 0; i < total; i++)
            {
                drives.Add(new Drive
                {
                    Name = $"Drive {i}",
                    RootDirectory = "",
                    VolumeLabel = $"Volume Label {i}",
                    DriveFormat = "FAT32",
                    DriveType = DriveType.Fixed,
                    IsReady = true,
                    TotalFreeSpace = 5000,
                    TotalSize = 10000
                });
            }

            return drives;
        }

        public async Task<Drive> GetDriveAsync(string name)
        {
            return await Task.FromResult(_drives.FirstOrDefault(s => s.Name == name));
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_drives);
        }
    }
}
using Multilarr.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Multilarr.Services
{
    public class MockDriveDataStore : IDriveDataStore<Drive>
    {
        public static List<Drive> CreateDrives(int total)
        {
            var random = new Random();
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
                    TotalFreeSpace = i * random.Next(1, 10),
                    TotalSize = i * random.Next(11, 20)
                });
            }

            return drives;
        }

        public async Task<Drive> GetDriveAsync(string name)
        {
            return await Task.FromResult(CreateDrives(10).FirstOrDefault(s => s.Name == name));
        }

        public async Task<IEnumerable<Drive>> GetDrivesAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(CreateDrives(10));
        }
    }
}
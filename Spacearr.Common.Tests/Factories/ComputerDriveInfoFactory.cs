using System;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Common.Tests.Factories
{
    public static class ComputerDriveInfoFactory
    {
        public static List<ComputerDriveInfo> CreateComputerDriveInfoFixed => CreateComputerDriveInfos(1, DriveType.Fixed);

        public static List<ComputerDriveInfo> CreateComputerDriveInfos(int total, DriveType? driveType = null)
        {
            var computerDriveInfos = new List<ComputerDriveInfo>();

            for (var i = 0; i < total; i++)
            {
                var driveTypes = Enum.GetValues(typeof(DriveType));
                var random = new Random();
                var randomDriveType = (DriveType)driveTypes.GetValue(random.Next(driveTypes.Length));
                randomDriveType = driveType ?? randomDriveType;

                var randomTotalSize = random.Next(1, 100);
                var randomTotalFreeSpace = random.Next(1, randomTotalSize);

                computerDriveInfos.Add(new ComputerDriveInfo
                {
                    Name = $"Drive {i}",
                    RootDirectory = $"Root Directory {i}",
                    VolumeLabel = $"Volume Label {i}",
                    DriveFormat = $"Drive Format {i}",
                    DriveType = randomDriveType,
                    IsReady = true,
                    TotalFreeSpace = randomTotalFreeSpace,
                    TotalSize = randomTotalSize
                });
            }

            return computerDriveInfos;
        }
    }
}
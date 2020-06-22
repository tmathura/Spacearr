using Spacearr.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Core.Xamarin.Tests.Factories
{
    public static class ComputerDriveModelFactory
    {
        public static List<ComputerDriveModel> CreateComputerDriveModels(int total, DriveType? driveType = null)
        {
            var computerDriveModels = new List<ComputerDriveModel>();

            for (var i = 0; i < total; i++)
            {
                var driveTypes = Enum.GetValues(typeof(DriveType));
                var random = new Random();
                var randomDriveType = (DriveType)driveTypes.GetValue(random.Next(driveTypes.Length));
                randomDriveType = driveType ?? randomDriveType;

                var randomTotalSize = random.Next(1, 100);
                var randomTotalFreeSpace = random.Next(1, randomTotalSize);

                computerDriveModels.Add(new ComputerDriveModel
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

            return computerDriveModels;
        }
    }
}
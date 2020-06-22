using Spacearr.Common.Models;
using System.Collections.Generic;
using System.IO;

namespace Spacearr.Core.Xamarin.Tests.Factories
{
    public static class ComputerModelFactory
    {
        public static IEnumerable<ComputerModel> CreateComputerModels(int total, DriveType? driveType = null)
        {
            var computerDriveModels = new List<ComputerModel>();

            for (var i = 0; i < total; i++)
            {
                computerDriveModels.Add(new ComputerModel
                {
                    Name = $"Computer {i}",
                    ComputerDrives = ComputerDriveModelFactory.CreateComputerDriveModels(total, driveType)
                });
            }

            return computerDriveModels;
        }
    }
}
using Spacearr.Common.Models;
using System;
using System.Collections.Generic;

namespace Spacearr.Common.Tests.Factories
{
    public static class FirebasePushNotificationDeviceModelFactory
    {
        public static List<FirebasePushNotificationDeviceModel> CreateFirebasePushNotificationDeviceModels(int total)
        {
            var computerDriveInfos = new List<FirebasePushNotificationDeviceModel>();

            for (var i = 0; i < total; i++)
            {
                computerDriveInfos.Add(new FirebasePushNotificationDeviceModel
                {
                    DeviceId = Guid.NewGuid(),
                    Token = Guid.NewGuid().ToString()
                });
            }

            return computerDriveInfos;
        }
    }
}
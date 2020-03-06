using Multilarr.Common.Models;
using System.Collections.Generic;

namespace Multilarr.Common.Tests.Factories
{
    public static class SettingLogFactory
    {
        public static List<SettingLog> Default(string appId, string key, string secret, string cluster) => CreateSettingLogs(1, appId, key, secret, cluster);

        public static List<SettingLog> CreateSettingLogs(int total, string appId = null, string key = null, string secret = null, string cluster = null)
        {
            var settingLogs = new List<SettingLog>();

            for (var i = 0; i < total; i++)
            {
                settingLogs.Add(new SettingLog
                {
                    PusherAppId = appId ?? $"AppId {i}",
                    PusherKey = key ?? $"Key {i}",
                    PusherSecret = secret ?? $"Secret {i}",
                    PusherCluster = cluster ?? $"Cluster {i}",
                    IsDefault = i == 0
                });
            }

            return settingLogs;
        }
    }
}
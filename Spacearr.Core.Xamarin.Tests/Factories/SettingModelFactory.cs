using Spacearr.Common.Models;
using System.Collections.Generic;

namespace Spacearr.Core.Xamarin.Tests.Factories
{
    public static class SettingModelFactory
    {
        public static List<SettingModel> Default(string appId, string key, string secret, string cluster) => CreateSettingModels(1, appId, key, secret, cluster, null);

        public static List<SettingModel> CreateSettingModels(int total, string appId = null, string key = null, string secret = null, string cluster = null, bool? isDefault = null)
        {
            var settings = new List<SettingModel>();

            for (var i = 0; i < total; i++)
            {
                settings.Add(new SettingModel
                {
                    PusherAppId = appId ?? $"AppId {i}",
                    PusherKey = key ?? $"Key {i}",
                    PusherSecret = secret ?? $"Secret {i}",
                    PusherCluster = cluster ?? $"Cluster {i}",
                    IsDefault = isDefault ?? i == 0
                });
            }

            return settings;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Spacearr.Common.Interfaces;
using Spacearr.Common.Interfaces.Logger;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Spacearr.Common
{
    public class Setting : ISetting
    {
        private readonly bool _useConfig;
        private readonly ILogger _logger;

        public string AppId { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Cluster { get; set; }

        public Setting(ILogger logger)
        {
            _logger = logger;
        }

        public Setting(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;

            _useConfig = true;
            AppId = configuration.GetSection("PusherAppId").Value;
            Key = configuration.GetSection("PusherKey").Value;
            Secret = configuration.GetSection("PusherSecret").Value;
            Cluster = configuration.GetSection("PusherCluster").Value;
        }

        public async Task PopulateSetting()
        {
            if (!_useConfig)
            {
                try
                {
                    var settings = await _logger.GetSettingsAsync();
                    if (settings == null)
                    {
                        throw new Exception("No settings saved.");
                    }
                    else if (settings.Any(x => x.IsDefault))
                    {
                        var setting = settings.FirstOrDefault(x => x.IsDefault);

                        AppId = setting?.PusherAppId;
                        Key = setting?.PusherKey;
                        Secret = setting?.PusherSecret;
                        Cluster = setting?.PusherCluster;
                    }
                    else
                    {
                        throw new Exception("No default setting saved.");
                    }
                }
                catch (Exception e)
                {
                    _logger?.LogWarnAsync(e.Message);
                }
            }
        }
    }
}
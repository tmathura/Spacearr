using Microsoft.Extensions.Configuration;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Logger;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Multilarr.Common
{
    public class Setting : ISetting
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public string AppId { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Cluster { get; set; }

        public Setting() { }

        public Setting(ILogger logger)
        {
            _logger = logger;

            PopulateSetting();
        }

        public Setting(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            AppId = configuration.GetSection("PusherAppId").Value;
            Key = configuration.GetSection("PusherKey").Value;
            Secret = configuration.GetSection("PusherSecret").Value;
            Cluster = configuration.GetSection("PusherCluster").Value;
        }

        public void PopulateSetting()
        {
            try
            {
                if (_logger != null && _configuration == null)
                {
                    var settings = Task.Run(_logger.GetSettingLogsAsync).Result;
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
            }
            catch (Exception e)
            {
                _logger?.LogWarnAsync(e.Message);
            }
        }
    }
}
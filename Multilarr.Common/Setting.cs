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

        public void PopulateSetting()
        {
            try
            {
                if (_logger != null)
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
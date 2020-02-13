using Multilarr.Common.Interfaces;
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
            if (_logger != null)
            {
                var settings = Task.Run(_logger.GetSettingLogsAsync).Result;
                if (settings == null)
                {
                    _logger.LogErrorAsync("No settings saved.");
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
                    _logger.LogErrorAsync("No default setting saved.");
                }
            }
        }
    }
}
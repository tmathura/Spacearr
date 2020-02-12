using Multilarr.Common.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Multilarr.Common
{
    public class Setting : ISetting
    {
        public string AppId { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Cluster { get; set; }

        public Setting(ILogger logger)
        {
            var settings = Task.Run(logger.GetSettingLogsAsync).Result;
            if (settings == null)
            {
                logger.LogErrorAsync("No settings saved.");
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
                logger.LogErrorAsync("No default setting saved.");
            }
        }
    }
}
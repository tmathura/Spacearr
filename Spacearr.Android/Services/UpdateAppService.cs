using Spacearr.Core.Xamarin.Services.Interfaces;
using Spacearr.Droid.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UpdateAppService))]
namespace Spacearr.Droid.Services
{
    public class UpdateAppService : IUpdateAppService
    {
        /// <summary>
        /// Update app.
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        public async Task Update(string versionNumber)
        {
            var apkPath = System.IO.Path.Combine(Plugin.XF.AppInstallHelper.CrossInstallHelper.Current.GetPublicDownloadPath(), $"com.Spacearr.Android-Signed-{versionNumber}.apk");
            await Plugin.XF.AppInstallHelper.CrossInstallHelper.Current.InstallApp(apkPath, Plugin.XF.AppInstallHelper.Abstractions.InstallMode.OutOfAppStore);
        }
    }
}
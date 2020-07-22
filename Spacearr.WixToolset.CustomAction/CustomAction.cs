using Microsoft.Deployment.WindowsInstaller;
using Spacearr.Common.Models;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Spacearr.WixToolset.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult ShowUpdateAppSettingsConfigurationScreens(Session session)
        {
            var appSettingModel = new CustomActionModel
            {
                Form = new Dictionary<Common.Enums.Controls, object>(),
                InstallationDirectory = session.CustomActionData["InstallDirectory"],
                AppSettingsModel = new AppSettingsModel(),
                UpdaterAppSettingsModel = new UpdaterAppSettingsModel()
            };

            var notificationTimerMinutesIntervalForm = new MainForm(appSettingModel);
            if (notificationTimerMinutesIntervalForm.ShowDialog() == DialogResult.Cancel)
            {
                return ActionResult.UserExit;
            }

            return ActionResult.Success;
        }
    }
}

using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;

namespace Spacearr.WixToolset.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult ShowUpdateAppSettingsConfigurationScreens(Session session)
        {
            var appSettingModel = new AppSettingModel
            {
                Form = new Dictionary<Enumeration.Controls, object>(),
                InstallationDirectory = session.CustomActionData["InstallDirectory"]
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

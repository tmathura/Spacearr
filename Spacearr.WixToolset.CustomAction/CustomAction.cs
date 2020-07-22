using Microsoft.Deployment.WindowsInstaller;
using Spacearr.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Spacearr.WixToolset.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult ShowUpdateAppSettingsConfigurationScreens(Session session)
        {
            try
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
            catch (Exception ex)
            {
                using (var eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Spacearr Installer";
                    eventLog.WriteEntry($"{ex}", EventLogEntryType.Error);
                }

                return ActionResult.Failure;
            }
        }
    }
}

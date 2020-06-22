using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spacearr.WixToolset.CustomAction.Controls;

namespace Spacearr.WixToolset.CustomAction
{
    public partial class MainForm : Form
    {
        private readonly AppSettingModel _appSettingModel;

        public MainForm(AppSettingModel appSettingModel)
        {
            _appSettingModel = appSettingModel;

            InitializeComponent();
            Application.EnableVisualStyles();
            TopMost = true;

            _appSettingModel.Form.Add(Enums.Controls.LowComputerDriveGBValueControl, new LowComputerDriveGBValueControl(_appSettingModel));
            _appSettingModel.Form.Add(Enums.Controls.NotificationTimerMinutesIntervalControl, new NotificationTimerMinutesIntervalControl(_appSettingModel));
            _appSettingModel.Form.Add(Enums.Controls.PusherControl, new PusherControl(_appSettingModel));

            var control = _appSettingModel.Form[_appSettingModel.CurrentControl];

            panelMain.Controls.Add((BaseControl) control);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var currentControl = (BaseControl) _appSettingModel.Form[_appSettingModel.CurrentControl];

            if (currentControl.ValidForm(out var errorMessage))
            {
                var lastIndex = (Enum.GetValues(typeof(Enums.Controls)).Length - 1);
                var nextEnumIndex = (int) _appSettingModel.CurrentControl + 1;

                if (nextEnumIndex > lastIndex)
                {
                    if (UpdateAppSettingsJson(out errorMessage))
                    {
                        DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        MessageBox.Show(errorMessage, @"Invalid Value");
                    }

                }
                else
                {
                    var nextEnum = (Enums.Controls) nextEnumIndex;

                    var newControlToShow = (BaseControl) _appSettingModel.Form[nextEnum];
                    newControlToShow.SetCurrentForm();
                    panelMain.Controls.Add(newControlToShow);

                    var previousEnumIndex = (int) _appSettingModel.CurrentControl - 1 < 0 ? 0 : (int) _appSettingModel.CurrentControl - 1;
                    var previousEnum = ((Enums.Controls) previousEnumIndex);
                    var oldControlToHide = (BaseControl)_appSettingModel.Form[previousEnum];
                    panelMain.Controls.Remove(oldControlToHide);
                }
            }
            else
            {
                MessageBox.Show(errorMessage, @"Invalid Value");
            }

            btnBack.Enabled = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var currentControl = (BaseControl) _appSettingModel.Form[_appSettingModel.CurrentControl];
            var previousEnumIndex = (int) _appSettingModel.CurrentControl - 1 < 0 ? 0 : (int) _appSettingModel.CurrentControl - 1;
            var previousEnum = (Enums.Controls) previousEnumIndex;

            var newControlToShow = (BaseControl) _appSettingModel.Form[previousEnum];
            newControlToShow.SetCurrentForm();
            panelMain.Controls.Add(newControlToShow);

            panelMain.Controls.Remove(currentControl);

            btnBack.Enabled = previousEnumIndex != 0;
        }

        private bool UpdateAppSettingsJson(out string errorMessage)
        {
            try
            {
                var appSettingsJsonPath = $@"{_appSettingModel.InstallationDirectory}\appsettings.json";
                var jsonString = File.ReadAllText(appSettingsJsonPath);
                var jObject = JsonConvert.DeserializeObject(jsonString) as JObject;

                var lowComputerDriveGBValue = jObject?.SelectToken("LowComputerDriveGBValue");
                lowComputerDriveGBValue?.Replace(_appSettingModel.LowComputerDriveGBValue);

                var notificationTimerMinutesInterval = jObject?.SelectToken("NotificationTimerMinutesInterval");
                notificationTimerMinutesInterval?.Replace(_appSettingModel.NotificationTimerMinutesInterval);

                var pusherAppId = jObject?.SelectToken("PusherAppId");
                pusherAppId?.Replace(_appSettingModel.PusherAppId);

                var pusherKey = jObject?.SelectToken("PusherKey");
                pusherKey?.Replace(_appSettingModel.PusherKey);

                var pusherSecret = jObject?.SelectToken("PusherSecret");
                pusherSecret?.Replace(_appSettingModel.PusherSecret);

                var pusherCluster = jObject?.SelectToken("PusherCluster");
                pusherCluster?.Replace(_appSettingModel.PusherCluster);

                var updatedJsonString = jObject?.ToString();
                File.WriteAllText(appSettingsJsonPath, updatedJsonString);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

            errorMessage = null;
            return true;
        }
    }
}

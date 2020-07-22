using Newtonsoft.Json;
using Spacearr.Common.Models;
using Spacearr.WixToolset.CustomAction.Controls;
using System;
using System.IO;
using System.Windows.Forms;

namespace Spacearr.WixToolset.CustomAction
{
    public partial class MainForm : Form
    {
        private readonly CustomActionModel _customActionModel;

        public MainForm(CustomActionModel customActionModel)
        {
            _customActionModel = customActionModel;

            InitializeComponent();
            Application.EnableVisualStyles();
            TopMost = true;

            _customActionModel.Form.Add(Common.Enums.Controls.LowSpaceControl, new LowSpaceControl(_customActionModel));
            _customActionModel.Form.Add(Common.Enums.Controls.UpdateAppControl, new UpdateAppControl(_customActionModel));
            _customActionModel.Form.Add(Common.Enums.Controls.PusherControl, new PusherControl(_customActionModel));

            var control = _customActionModel.Form[_customActionModel.CurrentControl];

            panelMain.Controls.Add((BaseControl) control);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var currentControl = (BaseControl) _customActionModel.Form[_customActionModel.CurrentControl];

            if (currentControl.ValidForm(out var errorMessage))
            {
                var lastIndex = (Enum.GetValues(typeof(Common.Enums.Controls)).Length - 1);
                var nextEnumIndex = (int) _customActionModel.CurrentControl + 1;

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
                    var nextEnum = (Common.Enums.Controls) nextEnumIndex;

                    var newControlToShow = (BaseControl) _customActionModel.Form[nextEnum];
                    newControlToShow.SetCurrentForm();
                    panelMain.Controls.Add(newControlToShow);

                    var previousEnumIndex = (int) _customActionModel.CurrentControl - 1 < 0 ? 0 : (int) _customActionModel.CurrentControl - 1;
                    var previousEnum = ((Common.Enums.Controls) previousEnumIndex);
                    var oldControlToHide = (BaseControl)_customActionModel.Form[previousEnum];
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
            var currentControl = (BaseControl) _customActionModel.Form[_customActionModel.CurrentControl];
            var previousEnumIndex = (int) _customActionModel.CurrentControl - 1 < 0 ? 0 : (int) _customActionModel.CurrentControl - 1;
            var previousEnum = (Common.Enums.Controls) previousEnumIndex;

            var newControlToShow = (BaseControl) _customActionModel.Form[previousEnum];
            newControlToShow.SetCurrentForm();
            panelMain.Controls.Add(newControlToShow);

            panelMain.Controls.Remove(currentControl);

            btnBack.Enabled = previousEnumIndex != 0;
        }

        private bool UpdateAppSettingsJson(out string errorMessage)
        {
            try
            {
                var appSettingsPath = $@"{_customActionModel.InstallationDirectory}\appsettings.json";
                var updaterAppSettingsPath = $@"{_customActionModel.InstallationDirectory}\Updater\appsettings.json";
                var appSettingsJsonString = JsonConvert.SerializeObject(_customActionModel.AppSettingsModel, Formatting.Indented);
                var updaterAppSettingsJsonString = JsonConvert.SerializeObject(_customActionModel.UpdaterAppSettingsModel, Formatting.Indented);

                File.WriteAllText(appSettingsPath, appSettingsJsonString);
                File.WriteAllText(updaterAppSettingsPath, updaterAppSettingsJsonString);
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

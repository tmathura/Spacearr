using Spacearr.Common.Models;
using System;
using System.Text.RegularExpressions;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class LowSpaceControl : MiddleControl
    {
        private readonly CustomActionModel _customActionModel;

        public LowSpaceControl(CustomActionModel customActionModel)
        {
            _customActionModel = customActionModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _customActionModel.CurrentControl = Common.Enums.Controls.LowSpaceControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtLowSpaceGBValue.Text))
            {
                errorMessage = @"Please enter a value for 'Low Space GB Value'.";
                return false;
            }

            if (!Regex.IsMatch(txtLowSpaceGBValue.Text, @"^\d+$"))
            {
                errorMessage = @"The value entered for 'Low Space GB Value' does not appear to be a valid number. Please try again.";
                return false;
            }

            if (string.IsNullOrEmpty(txtLowSpaceNotificationInterval.Text))
            {
                errorMessage = @"Please enter a value 'Low Space Notification Interval'.";
                return false;
            }

            if (!Regex.IsMatch(txtLowSpaceNotificationInterval.Text, @"^\d+$"))
            {
                errorMessage = @"The value entered for 'Low Space Notification Interval' does not appear to be a valid number. Please try again.";
                return false;
            }

            _customActionModel.AppSettingsModel.LowSpaceGBValue = Convert.ToInt32(txtLowSpaceGBValue.Text);
            _customActionModel.AppSettingsModel.LowSpaceNotificationInterval = Convert.ToInt32(txtLowSpaceNotificationInterval.Text);
            errorMessage = null;
            return true;
        }
    }
}

using Spacearr.Common.Models;
using System;
using System.Text.RegularExpressions;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class UpdateAppControl : MiddleControl
    {
        private readonly CustomActionModel _customActionModel;

        public UpdateAppControl(CustomActionModel customActionModel)
        {
            _customActionModel = customActionModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _customActionModel.CurrentControl = Common.Enums.Controls.UpdateAppControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtUpdateAppInterval.Text))
            {
                errorMessage = @"Please enter a value for 'Update App Interval'.";
                return false;
            }

            if (!Regex.IsMatch(txtUpdateAppInterval.Text, @"^\d+$"))
            {
                errorMessage = @"The value entered for 'Update App Interval' does not appear to be a valid number. Please try again.";
                return false;
            }

            _customActionModel.UpdaterAppSettingsModel.UpdateAppInterval = Convert.ToInt32(txtUpdateAppInterval.Text);
            errorMessage = null;
            return true;
        }
    }
}
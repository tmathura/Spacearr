using System.Text.RegularExpressions;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class LowComputerDriveGBValueControl : MiddleControl
    {
        private readonly AppSettingModel _appSettingModel;

        public LowComputerDriveGBValueControl(AppSettingModel appSettingModel)
        {
            _appSettingModel = appSettingModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _appSettingModel.CurrentControl = Enumeration.Controls.LowComputerDriveGBValueControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtLowComputerDriveGBValue.Text))
            {
                errorMessage = @"Please enter a value.";
                return false;
            }

            if (Regex.IsMatch(txtLowComputerDriveGBValue.Text, @"^\d+$"))
            {
                _appSettingModel.LowComputerDriveGBValue = txtLowComputerDriveGBValue.Text;
                errorMessage = null;
                return true;
            }

            errorMessage = @"The value entered does not appear to be a valid number. Please try again.";
            return false;
        }
    }
}

using System.Text.RegularExpressions;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class NotificationTimerMinutesIntervalControl : MiddleControl
    {
        private readonly AppSettingModel _appSettingModel;

        public NotificationTimerMinutesIntervalControl(AppSettingModel appSettingModel)
        {
            _appSettingModel = appSettingModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _appSettingModel.CurrentControl = Enumeration.Controls.NotificationTimerMinutesIntervalControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtNotificationTimerMinutesInterval.Text))
            {
                errorMessage = @"Please enter a value.";
                return false;
            }

            if (Regex.IsMatch(txtNotificationTimerMinutesInterval.Text, @"^\d+$"))
            {
                _appSettingModel.NotificationTimerMinutesInterval = txtNotificationTimerMinutesInterval.Text;
                errorMessage = null;
                return true;
            }

            errorMessage = @"The value entered does not appear to be a valid number. Please try again.";
            return false;
        }
    }
}
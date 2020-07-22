using Spacearr.Common.Models;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class PusherControl : MiddleControl
    {
        private readonly CustomActionModel _customActionModel;

        public PusherControl(CustomActionModel customActionModel)
        {
            _customActionModel = customActionModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _customActionModel.CurrentControl = Common.Enums.Controls.PusherControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtPusherAppId.Text))
            {
                errorMessage = @"Please enter a value for 'Pusher App Id'.";
                return false;
            }

            if (string.IsNullOrEmpty(txtPusherKey.Text))
            {
                errorMessage = @"Please enter a value for 'Pusher Key'.";
                return false;
            }

            if (string.IsNullOrEmpty(txtPusherSecret.Text))
            {
                errorMessage = @"Please enter a value for 'Pusher Secret'.";
                return false;
            }

            if (string.IsNullOrEmpty(txtPusherCluster.Text))
            {
                errorMessage = @"Please enter a value for 'Pusher Cluster'.";
                return false;
            }

            _customActionModel.AppSettingsModel.PusherAppId = txtPusherAppId.Text;
            _customActionModel.AppSettingsModel.PusherKey = txtPusherKey.Text;
            _customActionModel.AppSettingsModel.PusherSecret = txtPusherSecret.Text;
            _customActionModel.AppSettingsModel.PusherCluster = txtPusherCluster.Text;
            errorMessage = null;
            return true;
        }
    }
}

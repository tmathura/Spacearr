namespace Spacearr.WixToolset.CustomAction.Controls
{
    public partial class PusherControl : MiddleControl
    {
        private readonly AppSettingModel _appSettingModel;

        public PusherControl(AppSettingModel appSettingModel)
        {
            _appSettingModel = appSettingModel;

            InitializeComponent();
        }

        public override void SetCurrentForm()
        {
            _appSettingModel.CurrentControl = Enums.Controls.PusherControl;
        }

        public override bool ValidForm(out string errorMessage)
        {
            if (string.IsNullOrEmpty(txtPusherAppId.Text) || string.IsNullOrEmpty(txtPusherKey.Text) || string.IsNullOrEmpty(txtPusherSecret.Text) || string.IsNullOrEmpty(txtPusherCluster.Text))
            {
                errorMessage = @"Please enter a value.";
                return false;
            }

            _appSettingModel.PusherAppId = txtPusherAppId.Text;
            _appSettingModel.PusherKey = txtPusherKey.Text;
            _appSettingModel.PusherSecret = txtPusherSecret.Text;
            _appSettingModel.PusherCluster = txtPusherCluster.Text;
            errorMessage = null;
            return true;
        }
    }
}

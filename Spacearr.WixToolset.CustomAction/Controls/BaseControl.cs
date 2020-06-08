using System.Windows.Forms;

namespace Spacearr.WixToolset.CustomAction.Controls
{
    public abstract class BaseControl : UserControl
    {
        public abstract bool ValidForm(out string errorMessage);
        public abstract void SetCurrentForm();
    }
}

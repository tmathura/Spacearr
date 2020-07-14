using Android.Content;
using Android.Views.InputMethods;
using Spacearr.Core.Xamarin.Views.Controls.Android;
using Spacearr.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Spacearr.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private Android.Text.InputTypes _inputType;

        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.OldElement == null)
            {
                ((CustomEntry) Element).OnHideKeyboardTriggerRenderer += CustomEntryRenderer_HideKeyboard;
                ((CustomEntry) Element).OnFocusedTriggerRenderer += CustomEntryRenderer_FocusControl;

                _inputType = Control.InputType;

                Element.PropertyChanged += (sender, eve) =>
                {
                    if (eve.PropertyName == CustomEntry.CanShowVirtualKeyboardPropertyName)
                    {
                        Control.InputType = !((CustomEntry) Element).CanShowVirtualKeyboard ? 0 : _inputType;
                    }
                };

                if (!((CustomEntry) Element).CanShowVirtualKeyboard)
                {
                    Control.InputType = 0;
                }
            }
        }

        private void CustomEntryRenderer_HideKeyboard()
        {
            HideKeyboard();
        }

        private void CustomEntryRenderer_FocusControl(object sender, GenericEventArgs<FocusArgs> args)
        {
            args.EventData.CouldFocusBeSet = Control.RequestFocus();
            if (!((CustomEntry) Element).CanShowVirtualKeyboard)
            {
                HideKeyboard();
            }
        }

        public void HideKeyboard()
        {
            Control.RequestFocus();
            if (!((CustomEntry) Element).CanShowVirtualKeyboard)
            {
                Control.InputType = 0;
                var inputMethodManager = Control.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                inputMethodManager?.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
            }
        }
    }
}
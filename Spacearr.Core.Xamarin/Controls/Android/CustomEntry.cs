using System;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Controls.Android
{
    public class FocusArgs
    {
        public bool CouldFocusBeSet { get; set; }
    }

    public class CustomEntry : Entry
    {
        public event Action OnHideKeyboardTriggerRenderer;

        public CustomEntry()
        {
            Focused += (sender, e) =>
            {
                if (!CanShowVirtualKeyboard)
                {
                    HideKeyboard();
                }
            };
        }

        public void HideKeyboard()
        {
            OnHideKeyboardTriggerRenderer?.Invoke();
        }

        public const string CanShowVirtualKeyboardPropertyName = "CanShowVirtualKeyboard";
        public static readonly BindableProperty CanShowVirtualKeyboardProperty = BindableProperty.Create(CanShowVirtualKeyboardPropertyName, typeof(bool), typeof(CustomEntry), true);
       
        public bool CanShowVirtualKeyboard
        {
            get => (bool)GetValue(CanShowVirtualKeyboardProperty);
            set => SetValue(CanShowVirtualKeyboardProperty, value);
        }

        public event EventHandler<GenericEventArgs<FocusArgs>> OnFocusedTriggerRenderer;

		public new bool Focus()
        {
            var args = new GenericEventArgs<FocusArgs>(new FocusArgs { CouldFocusBeSet = false });
            OnFocusedTriggerRenderer?.Invoke(this, args);
            return args.EventData.CouldFocusBeSet;
        }
    }
}



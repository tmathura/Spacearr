using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Controls
{
    [DesignTimeVisible(false)]
    public partial class FloatingEntry : ContentView
    {
        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingEntry), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newValue) => {
            var floatingEntry = (FloatingEntry)bindable;

            if (!string.IsNullOrWhiteSpace((string)newValue))
            {
                floatingEntry.HiddenLabel.IsVisible = true;
                floatingEntry.HiddenLabel.TranslationY = -21;
                floatingEntry.EntryField.Placeholder = null;
            }
        });
        public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FloatingEntry), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newValue) => {
            var floatingEntry = (FloatingEntry)bindable;
            floatingEntry.EntryField.Placeholder = (string)newValue;
            floatingEntry.HiddenLabel.Text = (string)newValue;
        });
        public static BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FloatingEntry), false, propertyChanged: (bindable, oldVal, newVal) => {
            var floatingEntry = (FloatingEntry)bindable;
            floatingEntry.EntryField.IsPassword = (bool)newVal;
        });
        public static BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FloatingEntry), Keyboard.Default, propertyChanged: (bindable, oldVal, newVal) => {
            var floatingEntry = (FloatingEntry)bindable;
            floatingEntry.EntryField.Keyboard = (Keyboard)newVal;
        });
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FloatingEntry), Color.Accent);
        public static BindableProperty EntryPlaceholderColorProperty = BindableProperty.Create(nameof(EntryPlaceholderColor), typeof(Color), typeof(FloatingEntry), Color.Accent);
        public static BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(nameof(EntryBackgroundColor), typeof(Color), typeof(FloatingEntry), Color.Accent);
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        public Color EntryPlaceholderColor
        {
            get => (Color)GetValue(EntryPlaceholderColorProperty);
            set => SetValue(EntryPlaceholderColorProperty, value);
        }
        public Color EntryBackgroundColor
        {
            get => (Color)GetValue(EntryBackgroundColorProperty);
            set => SetValue(EntryBackgroundColorProperty, value);
        }
        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }
        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public FloatingEntry()
        {
            InitializeComponent();
            EntryField.BindingContext = this;

            EntryField.TextColor = TextColor;
            EntryField.PlaceholderColor = EntryPlaceholderColor;
            EntryField.BackgroundColor = EntryBackgroundColor;
            HiddenLabel.TextColor = TextColor;

            EntryField.Focused += async (s, a) =>
            {
                HiddenLabel.IsVisible = true;
                if (string.IsNullOrEmpty(EntryField.Text))
                {
                    HiddenLabel.Margin = new Thickness(0, 15, 0, 10);
                    await Task.WhenAll(HiddenLabel.FadeTo(1, 120), HiddenLabel.TranslateTo(HiddenLabel.TranslationX, EntryField.Y - EntryField.Height, 200, Easing.BounceIn));
                    EntryField.Placeholder = null;
                }
            };
            EntryField.Unfocused += async (s, a) => 
            {
                if (string.IsNullOrEmpty(EntryField.Text))
                {
                    await Task.WhenAll(HiddenLabel.FadeTo(0, 180), HiddenLabel.TranslateTo(HiddenLabel.TranslationX, EntryField.Y, 50, Easing.BounceIn));
                    EntryField.Placeholder = Placeholder;
                }
            };
        }
    }
}
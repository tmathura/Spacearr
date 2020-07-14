using System.ComponentModel;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Views.Controls
{
    [DesignTimeVisible(false)]
    public partial class FloatingLabel : ContentView
    {
        public static BindableProperty LabelBottomTextProperty = BindableProperty.Create(nameof(LabelBottomText), typeof(string), typeof(FloatingLabel), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newValue) => {
            var floatingLabel = (FloatingLabel)bindable;
            floatingLabel.LabelBottom.Text = (string)newValue;

            if (!string.IsNullOrWhiteSpace((string)newValue))
            {
                floatingLabel.LabelTop.TranslationY = -18;
            }
        });
        public static BindableProperty LabelTopTextProperty = BindableProperty.Create(nameof(LabelTopText), typeof(string), typeof(FloatingLabel), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldVal, newValue) => {
            var floatingLabel = (FloatingLabel)bindable;
            floatingLabel.LabelTop.Text = (string)newValue;
        });
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FloatingLabel), Color.Accent);
        public static BindableProperty LabelBackgroundColorProperty = BindableProperty.Create(nameof(LabelBackgroundColor), typeof(Color), typeof(FloatingLabel), Color.Accent);
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        public Color LabelBackgroundColor
        {
            get => (Color)GetValue(LabelBackgroundColorProperty);
            set => SetValue(LabelBackgroundColorProperty, value);
        }
        public string LabelBottomText
        {
            get => (string)GetValue(LabelBottomTextProperty);
            set => SetValue(LabelBottomTextProperty, value);
        }
        public string LabelTopText
        {
            get => (string)GetValue(LabelTopTextProperty);
            set => SetValue(LabelTopTextProperty, value);
        }

        public FloatingLabel()
        {
            InitializeComponent();
            LabelBottom.BindingContext = this;

            LabelTop.TextColor = TextColor;
            LabelBottom.TextColor = TextColor;
            LabelTop.BackgroundColor = LabelBackgroundColor;
            LabelBottom.BackgroundColor = LabelBackgroundColor;
        }
    }
}
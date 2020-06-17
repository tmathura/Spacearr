using Spacearr.Core.Xamarin.Themes;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.Helpers
{
    public class ThemeLoaderHelper
    {
        public static void LoadTheme(bool darkTheme)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (darkTheme)
                {
                    mergedDictionaries.Add(new DarkTheme());
                }
                else
                {
                    mergedDictionaries.Add(new LightTheme());
                }
            }
        }
    }
}

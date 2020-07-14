using System;

namespace Spacearr.Core.Xamarin.Views.Controls.Android
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T EventData { get; }
        public GenericEventArgs(T eventData)
        {
            EventData = eventData;
        }
    }
}

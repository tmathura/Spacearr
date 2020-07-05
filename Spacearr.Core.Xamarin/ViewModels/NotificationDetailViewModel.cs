namespace Spacearr.Core.Xamarin.ViewModels
{
    public class NotificationDetailViewModel : BaseViewModel
    {
        public string Message { get; set; }
        public NotificationDetailViewModel(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}

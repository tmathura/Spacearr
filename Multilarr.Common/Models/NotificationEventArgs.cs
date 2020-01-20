using System;

namespace Multilarr.Common.Models
{
    public class NotificationEventArgs : EventArgs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

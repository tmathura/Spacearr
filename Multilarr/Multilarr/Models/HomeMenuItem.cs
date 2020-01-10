namespace Multilarr.Models
{
    public enum MenuItemType
    {
        Home,
        ComputerDrives,
        Notifications,
        Logs
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

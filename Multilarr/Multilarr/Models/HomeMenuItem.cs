namespace Multilarr.Models
{
    public enum MenuItemType
    {
        Home,
        ComputerDrives,
        Logs,
        Settings
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

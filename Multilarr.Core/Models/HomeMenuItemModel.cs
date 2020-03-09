namespace Multilarr.Core.Models
{
    public enum MenuItemType
    {
        Home,
        ComputerDrives,
        Logs,
        Settings
    }
    public class HomeMenuItemModel
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

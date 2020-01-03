namespace Multilarr.Models
{
    public enum MenuItemType
    {
        Drives,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

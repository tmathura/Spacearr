﻿namespace Multilarr.Models
{
    public enum MenuItemType
    {
        ComputerDrives,
        Logs
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

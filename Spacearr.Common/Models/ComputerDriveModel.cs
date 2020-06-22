using Microcharts;
using SkiaSharp;
using Spacearr.Common.Util;
using System.Drawing;
using System.IO;

namespace Spacearr.Common.Models
{
    public class ComputerDriveModel
    {
        public string Name { get; set; }
        public string RootDirectory { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        public DriveType DriveType { get; set; }
        public bool IsReady { get; set; }
        public long TotalFreeSpace { get; set; }
        public string TotalFreeSpaceString => DataSize.SizeSuffix(TotalFreeSpace, 2);
        public long TotalUsedSpace => TotalSize - TotalFreeSpace;
        public string TotalUsedSpaceString => DataSize.SizeSuffix(TotalSize - TotalFreeSpace, 2);
        public long TotalSize { get; set; }
        public string TotalSizeString => DataSize.SizeSuffix(TotalSize, 2);
        public bool LoadPieChart { get; set; }
        public Color ThemeTextColor { get; set; }
        public Color ThemeLightColor { get; set; }
        public Color MicroChartsFreeSpaceColor { get; set; }
        public Color MicroChartsUsedSpaceColor { get; set; }
        public PieChart PieChart
        {
            get
            {
                if (LoadPieChart)
                {
                    var entries = new[]
                    {
                        new ChartEntry(TotalUsedSpace)
                        {
                            Label = "Used Space",
                            ValueLabel = $"{TotalUsedSpaceString}",
                            Color = SKColor.Parse(MicroChartsUsedSpaceColor.Name),
                            TextColor = SKColor.Parse(ThemeTextColor.Name)
                        },
                        new ChartEntry(TotalFreeSpace)
                        {
                            Label = "Total Free Space",
                            ValueLabel = $"{TotalFreeSpaceString}",
                            Color = SKColor.Parse(MicroChartsFreeSpaceColor.Name),
                            TextColor = SKColor.Parse(ThemeTextColor.Name)
                        }
                    };
                    return new PieChart { Entries = entries, BackgroundColor = SKColor.Parse(ThemeLightColor.Name) };
                }

                return null;
            }
        }
    }
}
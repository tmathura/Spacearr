using System.Collections.Generic;
using Multilarr.WorkerService.Windows.Common.Interfaces;

namespace Multilarr.WorkerService.Windows.Command.MessageCommand.Commands
{
    public class DrivesCommand : IMessageCommand
    {
        private readonly IDataSize _dataSize;
        private readonly IComputerDrives _computerDrives;

        public DrivesCommand(IDataSize dataSize, IComputerDrives computerDrives)
        {
            _dataSize = dataSize;
            _computerDrives = computerDrives;
        }

        public MessageCommandObject Execute()
        {
            var telegramMsg = new List<string>();
            if (_computerDrives.GetDrives().Length > 0)
            {
                telegramMsg.Add("Drives:" + string.Format("\r\n{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", "\u2796"));
                foreach (var drive in _computerDrives.GetDrives())
                {
                    telegramMsg.Add(
                        $"\r\nName: {drive.Name}" +
                        $"\r\nRoot Directory: {drive.RootDirectory}" +
                        $"\r\nVolume Label: {drive.VolumeLabel}" +
                        $"\r\nDrive Format: {drive.DriveFormat}" +
                        $"\r\nDrive Type: {drive.DriveType}" +
                        $"\r\nIs Ready: {drive.IsReady}" +
                        $"\r\nTotal Free Space: {_dataSize.SizeSuffix(drive.TotalFreeSpace, 2)}" +
                        $"\r\nTotal Size: {_dataSize.SizeSuffix(drive.TotalSize, 2)}" +
                        string.Format("\r\n{0}{0}{0}{0}{0}{0}{0}{0}{0}{0}", "\u2796")
                    );
                }
            }

            if (telegramMsg.Count == 0) telegramMsg.Add("No drives.");

            var result = string.Join("\r\n", telegramMsg.ToArray());

            var messageCommandObject = new MessageCommandObject
            {
                Message = result
            };
            return messageCommandObject;
        }
    }
}
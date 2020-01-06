using Multilarr.WorkerService.Windows.Command.MessageCommand;
using Multilarr.WorkerService.Windows.Command.MessageCommand.Commands;
using Multilarr.WorkerService.Windows.Common.Interfaces;

namespace Multilarr.WorkerService.Windows.Command
{
    public class Command : ICommand
    {
        private readonly IDataSize _dataSize;
        private readonly IMultilarrMessageCommand _multilarrMessageCommand;
        private readonly IComputerDrives _computerDrives;

        public Command(IDataSize dataSize, IMultilarrMessageCommand multilarrMessageCommand, IComputerDrives computerDrives)
        {
            _dataSize = dataSize;
            _multilarrMessageCommand = multilarrMessageCommand;
            _computerDrives = computerDrives;
        }

        public CommandObjectSerialized Invoke(string command)
        {
            var messageCommandObject = new CommandObjectSerialized();

            switch (command)
            {
                case "drives":
                {
                    messageCommandObject = InvokeDrivesCommand();
                    break;
                }
            }

            return messageCommandObject;
        }

        #region MultilarrMessageCommands

        private CommandObjectSerialized InvokeDrivesCommand()
        {
            var command = new DrivesCommand(_dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        #endregion
    }
}
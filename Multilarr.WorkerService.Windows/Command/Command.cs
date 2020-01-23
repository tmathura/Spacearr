using Multilarr.Common;
using Multilarr.Common.Interfaces;
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

        public CommandObjectSerialized Invoke(Enumeration.CommandType command)
        {
            var messageCommandObject = new CommandObjectSerialized();

            switch (command)
            {
                case Enumeration.CommandType.ComputerDrivesCommand:
                {
                    messageCommandObject = InvokeComputerDrivesCommand();
                    break;
                }

                case Enumeration.CommandType.ComputerDrivesLowCommand:
                {
                    messageCommandObject = InvokeComputerDrivesLowCommand();
                    break;
                }
            }

            return messageCommandObject;
        }

        #region MultilarrMessageCommands

        private CommandObjectSerialized InvokeComputerDrivesCommand()
        {
            var command = new ComputerDrivesCommand(_dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        private CommandObjectSerialized InvokeComputerDrivesLowCommand()
        {
            var command = new ComputerDrivesLowCommand(_dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        #endregion
    }
}
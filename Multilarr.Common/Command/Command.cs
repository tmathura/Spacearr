using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command.MessageCommand;
using Multilarr.Common.Command.MessageCommand.Commands;
using Multilarr.Common.Interfaces;

namespace Multilarr.Common.Command
{
    public class Command : ICommand
    {
        private readonly IConfiguration _configuration;
        private readonly IDataSize _dataSize;
        private readonly IMultilarrMessageCommand _multilarrMessageCommand;
        private readonly IComputerDrives _computerDrives;

        public Command(IConfiguration configuration, IDataSize dataSize, IMultilarrMessageCommand multilarrMessageCommand, IComputerDrives computerDrives)
        {
            _configuration = configuration;
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
            var command = new ComputerDrivesLowCommand(_configuration, _dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        #endregion
    }
}
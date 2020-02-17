using Microsoft.Extensions.Configuration;
using Multilarr.Common.Command.MessageCommand.Commands;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Command.MessageCommand;
using Multilarr.Common.Interfaces.Util;

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

        public string Invoke(Enumeration.CommandType command)
        {
            string json = null;

            switch (command)
            {
                case Enumeration.CommandType.ComputerDrivesCommand:
                {
                    json = InvokeComputerDrivesCommand();
                    break;
                }

                case Enumeration.CommandType.ComputerDrivesLowCommand:
                {
                    json = InvokeComputerDrivesLowCommand();
                    break;
                }
            }

            return json;
        }

        #region MultilarrMessageCommands

        private string InvokeComputerDrivesCommand()
        {
            var command = new ComputerDrivesCommand(_dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        private string InvokeComputerDrivesLowCommand()
        {
            var command = new ComputerDrivesLowCommand(_configuration, _dataSize, _computerDrives);
            return _multilarrMessageCommand.Invoke(command);
        }

        #endregion
    }
}
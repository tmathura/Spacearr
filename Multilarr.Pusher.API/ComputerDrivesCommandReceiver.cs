﻿using Multilarr.Common;
using Multilarr.Common.Command.Commands;
using Multilarr.Common.Interfaces;
using Multilarr.Common.Interfaces.Command;
using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Interfaces.Util;
using Multilarr.Common.Models;
using Multilarr.Pusher.API.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Multilarr.Pusher.API
{
    public class ComputerDrivesCommandReceiver : IComputerDrivesCommandReceiver
    {
        private readonly ILogger _logger;
        private readonly ISetting _setting;
        private readonly IDataSize _dataSize;
        private readonly IComputerDrives _computerDrives;

        private readonly string _channelNameReceive;
        private readonly string _eventNameReceive;
        private readonly string _channelNameSend;
        private readonly string _eventNameSend;

        public ComputerDrivesCommandReceiver(ILogger logger, ISetting setting, IDataSize dataSize, IComputerDrives computerDrives)
        {
            _logger = logger;
            _setting = setting;
            _dataSize = dataSize;
            _computerDrives = computerDrives;

            _channelNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.MultilarrWorkerServiceWindowsChannel.ToString() }";
            _eventNameReceive = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.WorkerServiceEvent.ToString() }";
            _channelNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherChannel.MultilarrChannel.ToString() }";
            _eventNameSend = $"{ Enumeration.CommandType.ComputerDrivesCommand }{ Enumeration.PusherEvent.MultilarrEvent.ToString() }";
        }

        public async Task Connect(Action<ICommand, string, string> executeCommand)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_setting.AppId) && !string.IsNullOrWhiteSpace(_setting.Key) && !string.IsNullOrWhiteSpace(_setting.Secret) && !string.IsNullOrWhiteSpace(_setting.Cluster))
                {
                    var pusherReceive = new PusherClient.Pusher(_setting.Key, new PusherClient.PusherOptions { Cluster = _setting.Cluster });

                    var myChannel = await pusherReceive.SubscribeAsync(_channelNameReceive);
                    myChannel.Bind(_eventNameReceive, (dynamic data) =>
                    {
                        PusherReceiveMessageObject pusherReceiveMessage = JsonConvert.DeserializeObject<PusherReceiveMessageObject>(data.ToString());
                        var pusherMessage = JsonConvert.DeserializeObject<PusherReceiveMessage>(pusherReceiveMessage.Data);
                        var deserializeObject = JsonConvert.DeserializeObject<PusherSendMessage>(pusherMessage.Message);
                        if (deserializeObject.Command == Enumeration.CommandType.ComputerDrivesCommand)
                        {
                            var command = new ComputerDrivesCommand(_dataSize, _computerDrives);
                            executeCommand(command, _channelNameSend, _eventNameSend);
                        }
                    });

                    await pusherReceive.ConnectAsync();
                }
                else
                {
                    throw new Exception("No default setting saved.");
                }
            }
            catch (Exception e)
            {
                await _logger.LogErrorAsync(e.Message, e.StackTrace);
            }
        }
    }
}
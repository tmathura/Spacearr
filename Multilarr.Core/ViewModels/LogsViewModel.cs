﻿using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Multilarr.Core.ViewModels
{
    public class LogsViewModel : BaseViewModel
    {
        private readonly Page _page;
        private readonly ILogger _logger;
        public ObservableCollection<LogModel> Logs { get; set; }
        public Command LoadItemsCommand { get; set; }

        public LogsViewModel(Page page, ILogger logger)
        {
            _page = page;
            _logger = logger;

            Title = "Logs";
            Logs = new ObservableCollection<LogModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        private async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Logs.Clear();
                var logs = await _logger.GetLogsAsync();
                foreach (var log in logs)
                {
                    Logs.Add(log);
                }
            }
            catch (Exception ex)
            {
                _page?.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
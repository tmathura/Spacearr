﻿using Multilarr.Common.Interfaces.Logger;
using Multilarr.Common.Models;
using Multilarr.Core.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Multilarr.Core.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingDetailPage : ContentPage
    {
        public SettingDetailPage(ILogger logger, SettingModel settingModel)
        {
            InitializeComponent();
            
            BindingContext = new SettingDetailViewModel(this, logger, settingModel);
        }
    }
}
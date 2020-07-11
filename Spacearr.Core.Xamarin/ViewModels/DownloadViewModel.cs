using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Spacearr.Core.Xamarin.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Spacearr.Core.Xamarin.ViewModels
{
    public class DownloadViewModel : ViewModelBase
    {
        public string VersionNumber { get; set; }

        private double _progressValue;
        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        /// <value>The progress value.</value>
        public double ProgressValue
        {
            get => _progressValue;
            set => Set(ref _progressValue, value);
        }

        private bool _isDownloading;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Spacearr.Core.Xamarin.ViewModels.DownloadViewModel"/>
        /// is downloading.
        /// </summary>
        /// <value><c>true</c> if is downloading; otherwise, <c>false</c>.</value>
        public bool IsDownloading
        {
            get => _isDownloading;
            set => Set(ref _isDownloading, value);
        }

        /// <summary>
        /// The download service.
        /// </summary>
        private readonly IDownloadService _downloadService;

        /// <summary>
        /// Gets the start download command.
        /// </summary>
        /// <value>The start download command.</value>
        public ICommand StartDownloadCommand { get; }

        public DownloadViewModel(IDownloadService downloadService)
        {
            _downloadService = downloadService;
            StartDownloadCommand = new RelayCommand<string>(async (url) => await StartDownloadAsync(url));
        }

        /// <summary>
        /// Starts the download async.
        /// </summary>
        /// <returns>The download async.</returns>
        public async Task StartDownloadAsync(string url)
        { 
            var progressIndicator = new Progress<double>(ReportProgress);
            var cts = new CancellationTokenSource();
            try
            {
                IsDownloading = true;

                await _downloadService.DownloadFileAsync(url, progressIndicator, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                //Manage cancellation here
            }
            finally
            {
                IsDownloading = false;
                await DependencyService.Get<IUpdateAppService>().Update(VersionNumber);
            }
        }

        /// <summary>
        /// Reports the progress status for the download.
        /// </summary>
        /// <param name="value">Value.</param>
        internal void ReportProgress(double value)
        {
            ProgressValue = value;
        }       
    }
}

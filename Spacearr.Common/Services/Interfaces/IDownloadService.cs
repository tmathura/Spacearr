using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IDownloadService
    {
        /// <summary>
        /// bool to check if the download service is downloading.
        /// </summary>
        bool IsDownloading { get; }

        /// <summary>
        /// Method to download a file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="progress"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token);
    }
}

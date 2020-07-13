using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spacearr.Common.Services.Interfaces
{
    public interface IDownloadService
    {
        Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token);
    }
}

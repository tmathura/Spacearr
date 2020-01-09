﻿using Multilarr.Common.Logger;
using System.Collections.Generic;
using System.Threading.Tasks;
using Multilarr.Models;

namespace Multilarr.Common.Interfaces.Logger
{
    public interface ILogger
    {
        Task<List<Log>> GetLogsAsync();
        Task<Log> GetLogAsync(int id);
        Task<int> LogInfoAsync(string logMessage);
        Task<int> LogWarnAsync(string logMessage);
        Task<int> LogErrorAsync(string logMessage);
        Task<int> DeleteLogAsync(Log item);
    }
}